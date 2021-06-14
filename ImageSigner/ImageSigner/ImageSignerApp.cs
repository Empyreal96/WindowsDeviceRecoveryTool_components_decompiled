using System;
using System.IO;
using System.Text;

namespace Microsoft.WindowsPhone.Imaging.ImageSignerApp
{
	// Token: 0x02000004 RID: 4
	public class ImageSignerApp
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000021C0 File Offset: 0x000003C0
		public static int Main(string[] args)
		{
			if (args.Length != 3)
			{
				return ImageSignerApp.WriteUsage();
			}
			try
			{
				string text = args[0];
				string fullPath = Path.GetFullPath(args[1]);
				string fullPath2 = Path.GetFullPath(args[2]);
				string a;
				if ((a = text.ToLower()) != null)
				{
					if (a == "sign")
					{
						return ImageSignerApp.SignImage(fullPath, fullPath2);
					}
					if (a == "getcatalog")
					{
						return ImageSignerApp.ExtractCatalog(fullPath, fullPath2);
					}
					if (a == "truncate")
					{
						return ImageSignerApp.TruncateImage(fullPath, fullPath2);
					}
				}
				Console.WriteLine("Unrecognized command: {0}", text);
				return ImageSignerApp.WriteUsage();
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine("File not found: {0}", ex.FileName);
			}
			catch (FormatException ex2)
			{
				Console.WriteLine("FFU format invalid: {0}", ex2.Message);
			}
			catch (ImageCommonException ex3)
			{
				Console.WriteLine("Error occured in ImageCommon: {0}", ex3.Message);
			}
			catch (IOException ex4)
			{
				Console.WriteLine("File IO failed: {0}", ex4.ToString());
			}
			catch (UnauthorizedAccessException ex5)
			{
				Console.WriteLine("Unable to access file: {0}", ex5.Message);
			}
			return -1;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000230C File Offset: 0x0000050C
		private static int WriteUsage()
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("\timagesigner sign <FFU> <catalog file>");
			Console.WriteLine("\timagesigner getcatalog <FFU> <catalog file>");
			Console.WriteLine("\timagesigner truncate <FFU> <truncated FFU>");
			return -1;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002338 File Offset: 0x00000538
		private static int SignImage(string ffuPath, string catalogPath)
		{
			FileStream fileStream = File.Open(ffuPath, FileMode.Open, FileAccess.ReadWrite);
			uint num;
			using (FileStream fileStream2 = File.OpenRead(catalogPath))
			{
				num = (uint)fileStream2.Length;
			}
			uint num2;
			uint num3;
			uint num4;
			uint num5;
			byte[] array;
			byte[] array2;
			ImageSignerApp.ReadHeaderFromStream(fileStream, out num2, out num3, out num4, out num5, out array, out array2);
			if (!ImageSigner.VerifyCatalogData(catalogPath, array2))
			{
				Console.WriteLine("The specified catalog does not match the image, or the image has been corrupted.");
				return -1;
			}
			if ((ulong)num > (ulong)((long)array.Length + (long)((ulong)num5)))
			{
				uint bytesNeeded = num - (uint)array.Length - num5;
				uint dataOffset = num2 + (uint)array.Length + (uint)array2.Length + num5;
				ImageSignerApp.ExtendImage(fileStream, dataOffset, bytesNeeded, num3, ref num5);
			}
			if ((ulong)num < (ulong)((long)array.Length) && array.Length - (int)num + (int)num5 > (int)num3)
			{
				uint excessPadding = (uint)(array.Length - (int)num + (int)num5);
				uint dataOffset2 = num2 + (uint)array.Length + (uint)array2.Length + num5;
				ImageSignerApp.ShrinkImage(fileStream, dataOffset2, excessPadding, num3, ref num5);
			}
			fileStream.Position = (long)((ulong)(num2 - 8U));
			byte[] bytes = BitConverter.GetBytes(num);
			fileStream.Write(bytes, 0, bytes.Length);
			byte[] bytes2 = BitConverter.GetBytes((uint)array2.Length);
			fileStream.Write(bytes2, 0, bytes2.Length);
			byte[] array3 = File.ReadAllBytes(catalogPath);
			fileStream.Write(array3, 0, array3.Length);
			fileStream.Write(array2, 0, array2.Length);
			Console.WriteLine("Successfully signed image.");
			return 0;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000247C File Offset: 0x0000067C
		private static void ShrinkImage(FileStream ffuFile, uint dataOffset, uint excessPadding, uint chunkSize, ref uint paddingSize)
		{
			long position = ffuFile.Position;
			uint num = ImageSignerApp.Align(excessPadding - (chunkSize - 1U), chunkSize);
			byte[] array = new byte[chunkSize];
			long num2 = (long)((ulong)dataOffset);
			long num3 = (long)((ulong)(dataOffset - num));
			while (num2 < ffuFile.Length)
			{
				ffuFile.Position = num2;
				ffuFile.Read(array, 0, array.Length);
				num2 += (long)((ulong)chunkSize);
				ffuFile.Position = num3;
				ffuFile.Write(array, 0, array.Length);
				num3 += (long)((ulong)chunkSize);
			}
			ffuFile.SetLength(ffuFile.Length - (long)((ulong)num));
			paddingSize = excessPadding - num;
			ffuFile.Position = position;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002508 File Offset: 0x00000708
		private static void ExtendImage(FileStream ffuFile, uint dataOffset, uint bytesNeeded, uint chunkSize, ref uint paddingSize)
		{
			long position = ffuFile.Position;
			uint num = ImageSignerApp.Align(bytesNeeded, chunkSize);
			byte[] array = new byte[chunkSize];
			ffuFile.SetLength(ffuFile.Length + (long)((ulong)num));
			long num2 = ffuFile.Length - (long)((ulong)chunkSize);
			long num3 = ffuFile.Length - (long)((ulong)num) - (long)((ulong)chunkSize);
			while (num3 >= (long)((ulong)dataOffset))
			{
				ffuFile.Position = num3;
				ffuFile.Read(array, 0, array.Length);
				num3 -= (long)((ulong)chunkSize);
				ffuFile.Position = num2;
				ffuFile.Write(array, 0, array.Length);
				num2 -= (long)((ulong)chunkSize);
			}
			paddingSize += num;
			ffuFile.Position = position;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000259C File Offset: 0x0000079C
		private static int ExtractCatalog(string ffuPath, string catalogPath)
		{
			FileStream ffuFile = File.OpenRead(ffuPath);
			byte[] bytes;
			int num = ImageSignerApp.ReadCatalogDataFromStream(ffuFile, out bytes);
			if (num != 0)
			{
				return num;
			}
			File.WriteAllBytes(catalogPath, bytes);
			Console.WriteLine("Successfully extracted catalog.");
			return 0;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000025D0 File Offset: 0x000007D0
		private static int ReadCatalogDataFromStream(FileStream ffuFile, out byte[] catalogData)
		{
			uint num;
			uint num2;
			uint num3;
			uint num4;
			byte[] array;
			ImageSignerApp.ReadHeaderFromStream(ffuFile, out num, out num2, out num3, out num4, out catalogData, out array);
			string text = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
			File.WriteAllBytes(text, catalogData);
			if (!ImageSigner.VerifyCatalogData(text, array))
			{
				Console.WriteLine("The catalog does not match the image, or the image has been corrupted.");
				return -1;
			}
			long firstChunkOffset = (long)((ulong)num + (ulong)catalogData.LongLength + (ulong)array.LongLength + (ulong)num4);
			try
			{
				HashedChunkReader hashedChunkReader = new HashedChunkReader(array, ffuFile, num2, firstChunkOffset);
				byte[] nextChunk = hashedChunkReader.GetNextChunk();
				uint num5 = BitConverter.ToUInt32(nextChunk, 0);
				if (num5 != 24U)
				{
					Console.WriteLine("Unable to read manifest length from image.");
					return -1;
				}
				string text2 = "ImageFlash  ";
				string @string = Encoding.ASCII.GetString(nextChunk, 4, text2.Length);
				if (!@string.Equals(text2))
				{
					Console.WriteLine("Invalid manifest signature encountered.");
					return -1;
				}
				uint num6 = BitConverter.ToUInt32(nextChunk, text2.Length + 4);
				for (uint num7 = num6 + num5; num7 > num2; num7 -= num2)
				{
					hashedChunkReader.GetNextChunk();
				}
				byte[] nextChunk2 = hashedChunkReader.GetNextChunk();
				BitConverter.ToUInt32(nextChunk2, 0);
				ushort num8 = BitConverter.ToUInt16(nextChunk2, 4);
				BitConverter.ToUInt16(nextChunk2, 6);
				if (num8 < 1 || num8 > 2)
				{
					Console.WriteLine("Image appears to be corrupt, or newer tools are required.");
					return -1;
				}
				int count = 192;
				string string2 = Encoding.ASCII.GetString(nextChunk2, 12, count);
				string text3 = string2;
				char[] trimChars = new char[1];
				string text4 = text3.TrimEnd(trimChars);
				char[] separator = new char[1];
				string[] array2 = text4.Split(separator);
				if (array2.Length == 0)
				{
					Console.WriteLine("Unable to read platform IDs from image.");
					return -1;
				}
				foreach (string arg in array2)
				{
					Console.WriteLine("Platform ID: {0}", arg);
				}
			}
			catch (HashedChunkReaderException ex)
			{
				Console.WriteLine(ex.Message);
				return -1;
			}
			return 0;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000027C8 File Offset: 0x000009C8
		private static int TruncateImage(string ffuPath, string outputPath)
		{
			FileStream fileStream = File.OpenRead(ffuPath);
			byte[] array;
			int num = ImageSignerApp.ReadCatalogDataFromStream(fileStream, out array);
			if (num != 0)
			{
				return num;
			}
			long position = fileStream.Position;
			fileStream.Seek(0L, SeekOrigin.Begin);
			byte[] array2 = new byte[position];
			fileStream.Read(array2, 0, array2.Length);
			File.WriteAllBytes(outputPath, array2);
			return 0;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000281B File Offset: 0x00000A1B
		private static uint Align(uint value, uint boundary)
		{
			return (value + (boundary - 1U)) / boundary * boundary;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002834 File Offset: 0x00000A34
		private static void ReadHeaderFromStream(FileStream ffu, out uint headerSize, out uint chunkSize, out uint algorithmId, out uint paddingSize, out byte[] catalogData, out byte[] hashData)
		{
			byte[] array = new byte[4];
			ffu.Read(array, 0, 4);
			headerSize = BitConverter.ToUInt32(array, 0);
			if (headerSize > 65536U)
			{
				throw new FormatException("bad header size.");
			}
			byte[] array2 = new byte[headerSize];
			ffu.Read(array2, 0, array2.Length - 4);
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(array2));
			byte[] array3 = new byte[]
			{
				83,
				105,
				103,
				110,
				101,
				100,
				73,
				109,
				97,
				103,
				101,
				32
			};
			for (int i = 0; i < array3.Length; i++)
			{
				if (binaryReader.ReadByte() != array3[i])
				{
					throw new FormatException("bad signature in header.");
				}
			}
			chunkSize = binaryReader.ReadUInt32() * 1024U;
			algorithmId = binaryReader.ReadUInt32();
			uint num = binaryReader.ReadUInt32();
			uint num2 = binaryReader.ReadUInt32();
			catalogData = new byte[num];
			if (num > 0U)
			{
				ffu.Read(catalogData, 0, catalogData.Length);
			}
			hashData = new byte[num2];
			ffu.Read(hashData, 0, hashData.Length);
			uint num3 = headerSize + num + num2;
			paddingSize = ImageSignerApp.Align(num3, chunkSize) - num3;
			ffu.Seek(0L, SeekOrigin.Begin);
		}
	}
}
