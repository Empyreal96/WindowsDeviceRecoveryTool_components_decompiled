using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using FFUComponents;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000005 RID: 5
	public class GptDevice
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000020BF File Offset: 0x000002BF
		private GptDevice(IFFUDevice device, ulong blockSize, GptPartition[] partitions)
		{
			this.device = device;
			this.blockSize = blockSize;
			this.partitions = partitions;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020DC File Offset: 0x000002DC
		public static bool CreateInstance(IFFUDevice device, uint blockSize, out GptDevice gptDevice)
		{
			gptDevice = null;
			byte[] array = new byte[blockSize];
			device.ReadDisk((ulong)blockSize, array, 0, array.Length);
			int num = 0;
			ulong num2 = BitConverter.ToUInt64(array, num);
			num += 8;
			if (num2 != 6075990659671082565UL)
			{
				return false;
			}
			uint num3 = BitConverter.ToUInt32(array, num);
			num += 4;
			if (num3 != 65536U)
			{
				return false;
			}
			int length = BitConverter.ToInt32(array, num);
			num += 4;
			int crc = BitConverter.ToInt32(array, num);
			BitConverter.GetBytes(0).CopyTo(array, num);
			num += 4;
			if (!GptDevice.CheckCrc32(array, 0, length, crc))
			{
				return false;
			}
			num += 36;
			num += 16;
			ulong num4 = BitConverter.ToUInt64(array, num);
			num += 8;
			uint num5 = BitConverter.ToUInt32(array, num);
			num += 4;
			uint num6 = BitConverter.ToUInt32(array, num);
			num += 4;
			int crc2 = BitConverter.ToInt32(array, num);
			num += 4;
			byte[] array2 = new byte[num5 * num6];
			device.ReadDisk(num4 * (ulong)blockSize, array2, 0, array2.Length);
			if (!GptDevice.CheckCrc32(array2, crc2))
			{
				return false;
			}
			GptPartition[] array3;
			if (!GptPartition.ReadFrom(array2, num5, num6, out array3))
			{
				return false;
			}
			gptDevice = new GptDevice(device, (ulong)blockSize, array3);
			return true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021F4 File Offset: 0x000003F4
		public bool ReadPartition(string name, out byte[] data)
		{
			data = null;
			foreach (GptPartition gptPartition in this.partitions)
			{
				if (gptPartition.Name == name)
				{
					ulong num = (gptPartition.LastLBA - gptPartition.FirstLBA + 1UL) * this.blockSize;
					data = new byte[num];
					this.device.ReadDisk(gptPartition.FirstLBA * this.blockSize, data, 0, data.Length);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002278 File Offset: 0x00000478
		public bool WritePartition(string name, byte[] data)
		{
			foreach (GptPartition gptPartition in this.partitions)
			{
				if (gptPartition.Name == name)
				{
					ulong num = (gptPartition.LastLBA - gptPartition.FirstLBA + 1UL) * this.blockSize;
					bool result;
					if (data.LongLength != (long)num)
					{
						result = false;
					}
					else
					{
						this.device.WriteDisk(gptPartition.FirstLBA * this.blockSize, data, 0, data.Length);
						result = true;
					}
					return result;
				}
			}
			return false;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022FA File Offset: 0x000004FA
		private static bool CheckCrc32(byte[] data, int crc)
		{
			return GptDevice.CheckCrc32(data, 0, data.Length, crc);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002308 File Offset: 0x00000508
		private static bool CheckCrc32(byte[] data, int offset, int length, int crc)
		{
			int num = GptDevice.RtlComputeCrc32(0, data, offset, length);
			return crc == num;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002324 File Offset: 0x00000524
		private static int RtlComputeCrc32(int PartialCrc, byte[] data, int offset, int length)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(length);
			Marshal.Copy(data, offset, intPtr, length);
			int result = NativeMethods.RtlComputeCrc32(PartialCrc, intPtr, length);
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002354 File Offset: 0x00000554
		private static bool DoBackup(IFFUDevice device, string outputPath)
		{
			string[] array = new string[]
			{
				"DPP",
				"MODEM_FS1",
				"MODEM_FS2",
				"MODEM_FSG"
			};
			outputPath = Path.Combine(outputPath, device.DeviceUniqueID.ToString("B"));
			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}
			uint num;
			ulong num2;
			if (!device.GetDiskInfo(out num, out num2))
			{
				Console.WriteLine("Unable to retrieve disk size details.  Please ensure the device supports FFU disk I/O.");
				return false;
			}
			GptDevice gptDevice;
			if (!GptDevice.CreateInstance(device, num, out gptDevice))
			{
				Console.WriteLine("Unable to parse GPT on device.  The disk may have been corrupted.");
				return false;
			}
			Console.WriteLine("Backing up partitions to {0}", outputPath);
			foreach (string text in array)
			{
				Console.WriteLine(" Backing up partition {0}", text);
				byte[] bytes;
				if (!gptDevice.ReadPartition(text, out bytes))
				{
					Console.WriteLine(" Error reading partition {0}.", text);
					return false;
				}
				string path = Path.Combine(outputPath, text);
				File.WriteAllBytes(path, bytes);
			}
			Console.WriteLine("Finished partition backup.");
			return true;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002468 File Offset: 0x00000668
		private static bool DoRestore(IFFUDevice device, string inputPath)
		{
			inputPath = Path.Combine(inputPath, device.DeviceUniqueID.ToString("B"));
			if (!Directory.Exists(inputPath))
			{
				return false;
			}
			uint num;
			ulong num2;
			if (!device.GetDiskInfo(out num, out num2))
			{
				Console.WriteLine("Unable to retrieve disk size details.  Please ensure the device supports FFU disk I/O.");
				return false;
			}
			GptDevice gptDevice;
			if (!GptDevice.CreateInstance(device, num, out gptDevice))
			{
				Console.WriteLine("Unable to parse GPT on device.  The disk may have been corrupted.");
				return false;
			}
			IEnumerable<string> enumerable = from path in Directory.GetFiles(inputPath)
			select Path.GetFileName(path);
			Console.WriteLine("Restoring partitions from {0}", inputPath);
			foreach (string text in enumerable)
			{
				Console.WriteLine(" Restoring partition {0}", text);
				string path2 = Path.Combine(inputPath, text);
				byte[] data = File.ReadAllBytes(path2);
				if (!gptDevice.WritePartition(text, data))
				{
					Console.WriteLine(" Error writing partition {0}.", text);
					return false;
				}
			}
			Console.WriteLine("Finished partition restore.");
			return true;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002590 File Offset: 0x00000790
		private static bool DoVerify(IFFUDevice device, string inputPath)
		{
			inputPath = Path.Combine(inputPath, device.DeviceUniqueID.ToString("B"));
			if (!Directory.Exists(inputPath))
			{
				return false;
			}
			uint num;
			ulong num2;
			if (!device.GetDiskInfo(out num, out num2))
			{
				Console.WriteLine("Unable to retrieve disk size details.  Please ensure the device supports FFU disk I/O.");
				return false;
			}
			GptDevice gptDevice;
			if (!GptDevice.CreateInstance(device, num, out gptDevice))
			{
				Console.WriteLine("Unable to parse GPT on device.  The disk may have been corrupted.");
				return false;
			}
			IEnumerable<string> enumerable = from path in Directory.GetFiles(inputPath)
			select Path.GetFileName(path);
			Console.WriteLine("Verifying partitions from {0}", inputPath);
			foreach (string text in enumerable)
			{
				Console.WriteLine(" Verifying partition {0}", text);
				string path2 = Path.Combine(inputPath, text);
				byte[] array = File.ReadAllBytes(path2);
				byte[] array2;
				if (!gptDevice.ReadPartition(text, out array2))
				{
					Console.WriteLine(" Error reading partition {0}.", text);
					return false;
				}
				if (array.LongLength != array2.LongLength)
				{
					Console.WriteLine(" Size mismatch for partition {0}.", text);
					return false;
				}
				for (long num3 = 0L; num3 < array.LongLength; num3 += 1L)
				{
					if (checked(array[(int)((IntPtr)num3)] != array2[(int)((IntPtr)num3)]))
					{
						Console.WriteLine(" Byte mismatch for partition {0} at offset {1}.", text, num3);
						return false;
					}
				}
			}
			Console.WriteLine("Partitions verified.");
			return true;
		}

		// Token: 0x04000003 RID: 3
		private IFFUDevice device;

		// Token: 0x04000004 RID: 4
		private ulong blockSize;

		// Token: 0x04000005 RID: 5
		private GptPartition[] partitions;
	}
}
