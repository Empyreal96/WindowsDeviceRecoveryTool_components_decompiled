using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace FFUComponents
{
	// Token: 0x0200004B RID: 75
	public class SimpleIODevice : IFFUDeviceInternal, IFFUDevice, IDisposable
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000047E2 File Offset: 0x000029E2
		// (set) Token: 0x06000109 RID: 265 RVA: 0x000047EA File Offset: 0x000029EA
		public string DeviceFriendlyName { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000047F3 File Offset: 0x000029F3
		// (set) Token: 0x0600010B RID: 267 RVA: 0x000047FB File Offset: 0x000029FB
		public Guid DeviceUniqueID { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00004804 File Offset: 0x00002A04
		public Guid SerialNumber
		{
			get
			{
				if (!this.serialNumberChecked)
				{
					this.serialNumberChecked = true;
					this.serialNumber = this.GetSerialNumberFromDevice();
				}
				return this.serialNumber;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00004827 File Offset: 0x00002A27
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00004830 File Offset: 0x00002A30
		public string UsbDevicePath
		{
			get
			{
				return this.usbDevicePath;
			}
			private set
			{
				lock (this.pathSync)
				{
					if (this.syncMutex != null)
					{
						this.syncMutex.Close();
						this.syncMutex = null;
					}
					string str = this.GetPnPIdFromDevicePath(value).Replace('\\', '_');
					this.syncMutex = new Mutex(false, "Global\\FFU_Mutex_" + str);
					this.usbDevicePath = value;
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000048B4 File Offset: 0x00002AB4
		public string DeviceType
		{
			get
			{
				return "SimpleIODevice";
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000110 RID: 272 RVA: 0x000048BC File Offset: 0x00002ABC
		// (remove) Token: 0x06000111 RID: 273 RVA: 0x000048F4 File Offset: 0x00002AF4
		public event EventHandler<ProgressEventArgs> ProgressEvent;

		// Token: 0x06000112 RID: 274 RVA: 0x0000492C File Offset: 0x00002B2C
		public SimpleIODevice(string devicePath)
		{
			this.fConnected = false;
			this.fOperationStarted = false;
			this.forceClearOnReconnect = true;
			this.usbStream = null;
			this.memStm = new MemoryStream();
			this.connectEvent = new AutoResetEvent(false);
			this.pathSync = new object();
			this.UsbDevicePath = devicePath;
			this.hostLogger = FFUManager.HostLogger;
			this.deviceLogger = FFUManager.DeviceLogger;
			this.packets = new PacketConstructor();
			this.DeviceUniqueID = Guid.Empty;
			this.DeviceFriendlyName = string.Empty;
			this.resetCount = 0;
			this.diskTransferSize = 0;
			this.diskBlockSize = 0U;
			this.diskLastBlock = 0UL;
			this.serialNumber = Guid.Empty;
			this.serialNumberChecked = false;
			this.usbTransactionSize = 16376;
			this.supportsFastFlash = false;
			this.supportsCompatFastFlash = false;
			this.hasCheckedForV2 = false;
			this.clientVersion = 1;
			this.telemetryLogger = FlashingTelemetryLogger.Instance;
			this.errorEvent = new ManualResetEvent(false);
			this.writeEvent = new AutoResetEvent(false);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004A38 File Offset: 0x00002C38
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004A48 File Offset: 0x00002C48
		private void Dispose(bool fDisposing)
		{
			if (fDisposing)
			{
				FFUManager.DisconnectDevice(this);
				if (this.usbStream != null)
				{
					this.usbStream.Dispose();
					this.usbStream = null;
					this.fConnected = false;
				}
				if (this.memStm != null)
				{
					this.memStm.Dispose();
					this.memStm = null;
				}
				if (this.syncMutex != null)
				{
					this.syncMutex.Close();
					this.syncMutex = null;
				}
				if (this.packets != null)
				{
					this.packets.Dispose();
					this.packets = null;
				}
				if (this.connectEvent != null)
				{
					this.connectEvent.Close();
					this.connectEvent = null;
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004AEC File Offset: 0x00002CEC
		public void FlashFFUFile(string ffuFilePath)
		{
			this.FlashFFUFile(ffuFilePath, false);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004AF8 File Offset: 0x00002CF8
		public void FlashFFUFile(string ffuFilePath, bool optimizeHint)
		{
			bool flag = false;
			if (this.curPosition != 0L)
			{
				throw new FFUException(this.DeviceFriendlyName, this.DeviceUniqueID, Resources.ERROR_ALREADY_RECEIVED_DATA);
			}
			this.lastProgress = 0L;
			this.fConnected = true;
			this.fOperationStarted = true;
			Guid sessionId = Guid.NewGuid();
			try
			{
				using (this.packets.DataStream = this.GetBufferedFileStream(ffuFilePath))
				{
					this.InitFlashingStream(optimizeHint, out flag);
					this.telemetryLogger.LogFlashingInitialized(sessionId, this, optimizeHint, ffuFilePath);
					this.hostLogger.EventWriteDeviceFlashParameters(this.usbTransactionSize, (int)this.packets.PacketDataSize);
					Assembly executingAssembly = Assembly.GetExecutingAssembly();
					object[] customAttributes = executingAssembly.GetCustomAttributes(typeof(AssemblyVersionAttribute), false);
					if (customAttributes.Length > 0)
					{
						AssemblyVersionAttribute assemblyVersionAttribute = (AssemblyVersionAttribute)customAttributes[0];
						this.hostLogger.EventWriteFlash_Start(this.DeviceUniqueID, this.DeviceFriendlyName, string.Format(CultureInfo.CurrentCulture, Resources.MODULE_VERSION, new object[]
						{
							assemblyVersionAttribute.ToString()
						}));
					}
					this.telemetryLogger.LogFlashingStarted(sessionId);
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					if (flag)
					{
						byte[] array = new byte[this.usbTransactionSize];
						this.usbStream.BeginRead(array, 0, array.Length, new AsyncCallback(this.ErrorCallback), this.errorEvent);
					}
					this.TransferPackets(flag);
					this.WaitForEndResponse(flag);
					stopwatch.Stop();
					this.telemetryLogger.LogFlashingEnded(sessionId, stopwatch, ffuFilePath, this);
					this.hostLogger.EventWriteFlash_Stop(this.DeviceUniqueID, this.DeviceFriendlyName);
				}
			}
			catch (Exception e)
			{
				this.telemetryLogger.LogFlashingException(sessionId, e);
				throw;
			}
			finally
			{
				if (flag)
				{
					this.usbTransactionSize = 16376;
					this.packets.PacketDataSize = PacketConstructor.DefaultPacketDataSize;
				}
				this.fConnected = false;
				FFUManager.DisconnectDevice(this.DeviceUniqueID);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004D2C File Offset: 0x00002F2C
		public void FlashDataFile(string path)
		{
			string fileName = Path.GetFileName(path);
			this.InitFlashingStream();
			this.usbStream.WriteByte(9);
			this.packets.DataStream = this.GetStringStream(fileName);
			this.TransferPackets(false);
			this.WaitForEndResponse(false);
			this.packets.DataStream = this.GetBufferedFileStream(path);
			this.TransferPackets(false);
			this.WaitForEndResponse(false);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004D98 File Offset: 0x00002F98
		public bool WriteWim(string wimPath)
		{
			bool result = false;
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					return false;
				}
				this.fOperationStarted = true;
				try
				{
					using (Stream bufferedFileStream = this.GetBufferedFileStream(wimPath))
					{
						using (Stream stream = new MemoryStream(Resources.bootsdi))
						{
							using (this.usbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(1.0)))
							{
								this.usbStream.SetShortPacketTerminate();
								try
								{
									this.WriteWim(stream, bufferedFileStream);
								}
								catch (Win32Exception ex)
								{
									this.hostLogger.EventWriteWimWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
								}
								this.usbStream.SetTransferTimeout(TimeSpan.FromSeconds(15.0));
								result = this.ReadStatus();
							}
						}
					}
				}
				catch (IOException)
				{
					this.hostLogger.EventWriteWimIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
				}
				catch (Win32Exception ex2)
				{
					this.hostLogger.EventWriteWimWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex2.NativeErrorCode);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004FB4 File Offset: 0x000031B4
		public bool GetDiskInfo(out uint blockSize, out ulong lastBlock)
		{
			bool result = false;
			blockSize = 0U;
			lastBlock = 0UL;
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					return false;
				}
				try
				{
					using (this.usbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(1.0)))
					{
						this.ReadDiskInfo(out this.diskTransferSize, out this.diskBlockSize, out this.diskLastBlock);
						result = true;
					}
				}
				catch (IOException)
				{
				}
				catch (Win32Exception)
				{
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			blockSize = this.diskBlockSize;
			lastBlock = this.diskLastBlock;
			return result;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000050B0 File Offset: 0x000032B0
		public string GetServicingLogs(string logFolderPath)
		{
			string result = null;
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					throw new FFUDeviceNotReadyException(this);
				}
				try
				{
					using (this.usbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromMinutes(1.0)))
					{
						if (!this.QueryForCommandAvailable(this.usbStream, SimpleIODevice.SioOpcode.SioGetUpdateLogs))
						{
							throw new FFUDeviceCommandNotAvailableException(this);
						}
						if (string.IsNullOrEmpty(logFolderPath))
						{
							throw new ArgumentNullException("logFolderPath");
						}
						this.usbStream.WriteByte(24);
						byte[] array = new byte[262144];
						int num = 0;
						byte[] array2 = new byte[4];
						int num2 = this.usbStream.Read(array2, 0, array2.Length);
						int num3 = BitConverter.ToInt32(array2, 0);
						string text = LongPath.GetFullPath(logFolderPath);
						LongPathDirectory.CreateDirectory(text);
						text = Path.Combine(text, Path.GetRandomFileName() + ".cab");
						using (FileStream fileStream = LongPathFile.Open(text, FileMode.Create, FileAccess.Write))
						{
							do
							{
								Array.Clear(array, 0, array.Length);
								num2 = this.usbStream.Read(array, 0, array.Length);
								num += num2;
								fileStream.Write(array, 0, array.Length);
							}
							while (num != num3);
							result = text;
						}
					}
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000527C File Offset: 0x0000347C
		public void ReadDisk(ulong diskOffset, byte[] buffer, int offset, int count)
		{
			lock (this.pathSync)
			{
				if (this.diskTransferSize <= 0 || this.fConnected || !this.AcquirePathMutex())
				{
					throw new FFUDeviceNotReadyException(this);
				}
				ulong num = (this.diskLastBlock + 1UL) * (ulong)this.diskBlockSize;
				if (count <= 0 || diskOffset >= num || num - diskOffset < (ulong)((long)count))
				{
					throw new FFUDeviceDiskReadException(this, Resources.ERROR_UNABLE_TO_READ_REGION, null);
				}
				try
				{
					using (this.usbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(1.0)))
					{
						this.ReadDataToBuffer(diskOffset, buffer, offset, count);
					}
				}
				catch (IOException)
				{
					throw new FFUDeviceNotReadyException(this);
				}
				catch (Win32Exception e)
				{
					throw new FFUDeviceDiskReadException(this, Resources.ERROR_USB_TRANSFER, e);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005398 File Offset: 0x00003598
		public void WriteDisk(ulong diskOffset, byte[] buffer, int offset, int count)
		{
			lock (this.pathSync)
			{
				if (this.diskTransferSize <= 0 || this.fConnected || !this.AcquirePathMutex())
				{
					throw new FFUDeviceNotReadyException(this);
				}
				ulong num = (this.diskLastBlock + 1UL) * (ulong)this.diskBlockSize;
				if (count <= 0 || diskOffset >= num || num - diskOffset < (ulong)((long)count))
				{
					throw new FFUDeviceDiskReadException(this, Resources.ERROR_UNABLE_TO_READ_REGION, null);
				}
				try
				{
					using (this.usbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(1.0)))
					{
						this.WriteDataFromBuffer(diskOffset, buffer, offset, count);
					}
				}
				catch (IOException)
				{
					throw new FFUDeviceNotReadyException(this);
				}
				catch (Win32Exception e)
				{
					throw new FFUDeviceDiskWriteException(this, Resources.ERROR_USB_TRANSFER, e);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000054B4 File Offset: 0x000036B4
		public bool SkipTransfer()
		{
			bool result = false;
			lock (this.pathSync)
			{
				if (this.curPosition != 0L || this.fConnected || !this.AcquirePathMutex())
				{
					return false;
				}
				this.fOperationStarted = true;
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						result = this.WriteSkip(dtsfusbStream);
					}
				}
				catch (IOException)
				{
					this.hostLogger.EventWriteSkipIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
				}
				catch (Win32Exception ex)
				{
					this.hostLogger.EventWriteSkipWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000055C8 File Offset: 0x000037C8
		public bool EndTransfer()
		{
			bool result = false;
			if (this.curPosition == 0L)
			{
				return true;
			}
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					return false;
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						dtsfusbStream.WriteByte(8);
						byte[] array = new byte[this.usbTransactionSize];
						do
						{
							dtsfusbStream.Read(array, 0, array.Length);
						}
						while (array[0] == 5);
						if (array[0] == 6)
						{
							this.ReadBootmeFromStream(dtsfusbStream);
							if (this.curPosition == 0L)
							{
								result = true;
							}
						}
					}
				}
				catch (IOException)
				{
				}
				catch (Win32Exception)
				{
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000056D8 File Offset: 0x000038D8
		public bool Reboot()
		{
			bool result = false;
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					return false;
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						dtsfusbStream.WriteByte(10);
						result = true;
					}
				}
				catch (IOException)
				{
					this.hostLogger.EventWriteRebootIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
				}
				catch (Win32Exception ex)
				{
					this.hostLogger.EventWriteRebootWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000057DC File Offset: 0x000039DC
		public void QueryDeviceUnlockId(out byte[] unlockId, out byte[] oemId, out byte[] platformId)
		{
			unlockId = new byte[32];
			oemId = new byte[16];
			platformId = new byte[16];
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					throw new FFUDeviceNotReadyException(this);
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						BinaryWriter binaryWriter = new BinaryWriter(dtsfusbStream);
						BinaryReader binaryReader = new BinaryReader(dtsfusbStream);
						if (!this.QueryForCommandAvailable(dtsfusbStream, SimpleIODevice.SioOpcode.SioQueryDeviceUnlockId))
						{
							throw new FFUDeviceCommandNotAvailableException(this);
						}
						binaryWriter.Write(25);
						int num = binaryReader.ReadInt32();
						binaryReader.ReadUInt32();
						unlockId = binaryReader.ReadBytes(32);
						oemId = binaryReader.ReadBytes(16);
						platformId = binaryReader.ReadBytes(16);
						if (num != 0)
						{
							throw new FFUDeviceRetailUnlockException(this, num);
						}
					}
				}
				catch (IOException)
				{
					throw new FFUDeviceNotReadyException(this);
				}
				catch (Win32Exception e)
				{
					throw new FFUDeviceRetailUnlockException(this, Resources.ERROR_USB_TRANSFER, e);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005928 File Offset: 0x00003B28
		public void RelockDeviceUnlockId()
		{
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					throw new FFUDeviceNotReadyException(this);
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						BinaryWriter binaryWriter = new BinaryWriter(dtsfusbStream);
						BinaryReader binaryReader = new BinaryReader(dtsfusbStream);
						if (!this.QueryForCommandAvailable(dtsfusbStream, SimpleIODevice.SioOpcode.SioRelockDeviceUnlockId))
						{
							throw new FFUDeviceCommandNotAvailableException(this);
						}
						binaryWriter.Write(26);
						int num = binaryReader.ReadInt32();
						if (num != 0)
						{
							throw new FFUDeviceRetailUnlockException(this, num);
						}
					}
				}
				catch (IOException)
				{
					throw new FFUDeviceNotReadyException(this);
				}
				catch (Win32Exception e)
				{
					throw new FFUDeviceRetailUnlockException(this, Resources.ERROR_USB_TRANSFER, e);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005A34 File Offset: 0x00003C34
		public void WriteUnlockTokenFile(uint unlockTokenId, byte[] fileData)
		{
			uint value = 0U;
			uint value2 = (uint)fileData.Length;
			if (1048576 < fileData.Length)
			{
				throw new ArgumentException("fileData");
			}
			if (127U < unlockTokenId)
			{
				throw new ArgumentException("unlockTokenId");
			}
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					throw new FFUDeviceNotReadyException(this);
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						BinaryWriter binaryWriter = new BinaryWriter(dtsfusbStream);
						BinaryReader binaryReader = new BinaryReader(dtsfusbStream);
						if (!this.QueryForCommandAvailable(dtsfusbStream, SimpleIODevice.SioOpcode.SioWriteUnlockTokenFile))
						{
							throw new FFUDeviceCommandNotAvailableException(this);
						}
						binaryWriter.Write(28);
						binaryWriter.Write(value);
						binaryWriter.Write(value2);
						binaryWriter.Write(unlockTokenId);
						binaryWriter.Write(fileData);
						int num = binaryReader.ReadInt32();
						if (num != 0)
						{
							throw new FFUDeviceRetailUnlockException(this, num);
						}
					}
				}
				catch (IOException)
				{
					throw new FFUDeviceNotReadyException(this);
				}
				catch (Win32Exception e)
				{
					throw new FFUDeviceRetailUnlockException(this, Resources.ERROR_USB_TRANSFER, e);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005B94 File Offset: 0x00003D94
		public uint[] QueryUnlockTokenFiles()
		{
			byte[] bytes = new byte[16];
			List<uint> list = new List<uint>();
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					throw new FFUDeviceNotReadyException(this);
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						BinaryWriter binaryWriter = new BinaryWriter(dtsfusbStream);
						BinaryReader binaryReader = new BinaryReader(dtsfusbStream);
						if (!this.QueryForCommandAvailable(dtsfusbStream, SimpleIODevice.SioOpcode.SioQueryUnlockTokenFiles))
						{
							throw new FFUDeviceCommandNotAvailableException(this);
						}
						binaryWriter.Write(27);
						int num = binaryReader.ReadInt32();
						binaryReader.ReadUInt32();
						bytes = binaryReader.ReadBytes(16);
						BitArray bitArray = new BitArray(bytes);
						uint num2 = 0U;
						while ((ulong)num2 < (ulong)((long)bitArray.Count))
						{
							if (bitArray.Get(Convert.ToInt32(num2)))
							{
								list.Add(num2);
							}
							num2 += 1U;
						}
						if (num != 0)
						{
							throw new FFUDeviceRetailUnlockException(this, num);
						}
					}
				}
				catch (IOException)
				{
					throw new FFUDeviceNotReadyException(this);
				}
				catch (Win32Exception e)
				{
					throw new FFUDeviceRetailUnlockException(this, Resources.ERROR_USB_TRANSFER, e);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005D04 File Offset: 0x00003F04
		public bool QueryBitlockerState()
		{
			bool result = false;
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					throw new FFUDeviceNotReadyException(this);
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						BinaryWriter binaryWriter = new BinaryWriter(dtsfusbStream);
						BinaryReader binaryReader = new BinaryReader(dtsfusbStream);
						if (!this.QueryForCommandAvailable(dtsfusbStream, SimpleIODevice.SioOpcode.SioQueryBitlockerState))
						{
							throw new FFUDeviceCommandNotAvailableException(this);
						}
						binaryWriter.Write(29);
						int num = binaryReader.ReadInt32();
						result = (binaryReader.ReadByte() != 0);
						if (num != 0)
						{
							throw new FFUDeviceRetailUnlockException(this, num);
						}
					}
				}
				catch (IOException)
				{
					throw new FFUDeviceNotReadyException(this);
				}
				catch (Win32Exception e)
				{
					throw new FFUDeviceRetailUnlockException(this, Resources.ERROR_USB_TRANSFER, e);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005E24 File Offset: 0x00004024
		public bool EnterMassStorage()
		{
			bool result = false;
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					return false;
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						dtsfusbStream.WriteByte(11);
						int num = dtsfusbStream.ReadByte();
						if (num == 3)
						{
							result = true;
						}
					}
				}
				catch (IOException)
				{
					this.hostLogger.EventWriteMassStorageIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
				}
				catch (Win32Exception ex)
				{
					this.hostLogger.EventWriteMassStorageWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005F34 File Offset: 0x00004134
		public bool ClearIdOverride()
		{
			bool result = false;
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					return false;
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(1.0)))
					{
						dtsfusbStream.WriteByte(15);
						int num = dtsfusbStream.ReadByte();
						if (num == 3)
						{
							result = true;
							this.ReadBootmeFromStream(dtsfusbStream);
						}
					}
				}
				catch (IOException)
				{
					this.hostLogger.EventWriteClearIdIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
				}
				catch (Win32Exception ex)
				{
					this.hostLogger.EventWriteClearIdWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000604C File Offset: 0x0000424C
		public uint SetBootMode(uint bootMode, string profileName)
		{
			uint result = 2147483669U;
			lock (this.pathSync)
			{
				if (this.fConnected || !this.AcquirePathMutex())
				{
					return result;
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(5.0)))
					{
						if (Encoding.Unicode.GetByteCount(profileName) >= 128)
						{
							result = 2147483650U;
							throw new Win32Exception(87);
						}
						uint num = 132U;
						byte[] array = new byte[num];
						Array.Clear(array, 0, array.Length);
						byte[] bytes = BitConverter.GetBytes(bootMode);
						bytes.CopyTo(array, 0);
						bytes = Encoding.Unicode.GetBytes(profileName);
						bytes.CopyTo(array, 4);
						dtsfusbStream.WriteByte(19);
						dtsfusbStream.Write(array, 0, array.Length);
						byte[] array2 = new byte[4];
						dtsfusbStream.Read(array2, 0, array2.Length);
						result = BitConverter.ToUInt32(array2, 0);
					}
				}
				catch (IOException)
				{
					this.hostLogger.EventWriteBootModeIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
				}
				catch (Win32Exception ex)
				{
					this.hostLogger.EventWriteBootModeWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000621C File Offset: 0x0000441C
		public bool OnConnect(SimpleIODevice device)
		{
			if (device != null && device.UsbDevicePath != this.UsbDevicePath)
			{
				this.UsbDevicePath = device.UsbDevicePath;
			}
			if (this.fConnected)
			{
				this.connectEvent.Set();
				return true;
			}
			return this.ReadBootme();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000626E File Offset: 0x0000446E
		public bool IsConnected()
		{
			return this.fConnected || this.ReadBootme();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006282 File Offset: 0x00004482
		public bool NeedsTimer()
		{
			return !this.fOperationStarted;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000628F File Offset: 0x0000448F
		public bool OnDisconnect()
		{
			return false;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006294 File Offset: 0x00004494
		private bool AcquirePathMutex()
		{
			TimeSpan timeout = TimeSpan.FromMinutes(2.0);
			TimeoutHelper timeoutHelper = new TimeoutHelper(timeout);
			TimeSpan remaining = timeoutHelper.Remaining;
			if (remaining <= TimeSpan.Zero)
			{
				this.hostLogger.EventWriteMutexTimeout(this.DeviceUniqueID, this.DeviceFriendlyName);
				return false;
			}
			bool result;
			try
			{
				if (!this.syncMutex.WaitOne(remaining, false))
				{
					this.hostLogger.EventWriteMutexTimeout(this.DeviceUniqueID, this.DeviceFriendlyName);
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (AbandonedMutexException)
			{
				this.hostLogger.EventWriteWaitAbandoned(this.DeviceUniqueID, this.DeviceFriendlyName);
				result = true;
			}
			return result;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006344 File Offset: 0x00004544
		private void ReleasePathMutex()
		{
			this.syncMutex.ReleaseMutex();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006354 File Offset: 0x00004554
		private void InitFlashingStream()
		{
			bool flag = false;
			this.InitFlashingStream(false, out flag);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000636C File Offset: 0x0000456C
		private void InitFlashingStream(bool optimizeHint, out bool useOptimize)
		{
			bool flag = false;
			bool flag2 = false;
			useOptimize = false;
			lock (this.pathSync)
			{
				if (!this.AcquirePathMutex())
				{
					throw new FFUException(this.DeviceFriendlyName, this.DeviceUniqueID, Resources.ERROR_ACQUIRE_MUTEX);
				}
				try
				{
					if (this.usbStream != null)
					{
						this.usbStream.Dispose();
					}
					this.usbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromMinutes(1.0));
					if (optimizeHint)
					{
						int num = 0;
						do
						{
							this.ReadBootmeFromStream(this.usbStream);
							num++;
						}
						while (!this.supportsFastFlash && !this.supportsCompatFastFlash && num < 1000);
						flag2 = (this.supportsFastFlash || this.supportsCompatFastFlash);
					}
					if (!flag2)
					{
						this.usbStream.WriteByte(2);
					}
					else if (this.supportsFastFlash)
					{
						this.usbStream.WriteByte(20);
						this.InitFastFlash();
						useOptimize = true;
					}
					else if (this.supportsCompatFastFlash)
					{
						this.usbStream.WriteByte(20);
						this.usbTransactionSize = 8388600;
						this.packets.PacketDataSize = 8388608L;
						useOptimize = true;
					}
				}
				catch (IOException)
				{
					flag = true;
				}
				catch (Win32Exception ex)
				{
					flag = true;
					if (ex.NativeErrorCode == 31)
					{
						this.forceClearOnReconnect = false;
					}
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			if (flag)
			{
				this.WaitForReconnect();
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000652C File Offset: 0x0000472C
		private void InitFastFlash()
		{
			byte[] array = new byte[this.usbTransactionSize];
			this.usbStream.Read(array, 0, array.Length);
			SimpleIODevice.SioOpcode sioOpcode = (SimpleIODevice.SioOpcode)array[0];
			if (sioOpcode != SimpleIODevice.SioOpcode.SioDeviceParams)
			{
				if (sioOpcode == SimpleIODevice.SioOpcode.SioErr)
				{
					this.hostLogger.EventWriteFlash_Error(this.DeviceUniqueID, this.DeviceFriendlyName);
					throw new FFUFlashException(this.DeviceFriendlyName, this.DeviceUniqueID, (FFUFlashException.ErrorCode)this.errId, string.Format(CultureInfo.CurrentCulture, Resources.ERROR_FLASH, new object[]
					{
						this.errInfo
					}));
				}
				throw new FFUFlashException();
			}
			else
			{
				BinaryReader binaryReader = new BinaryReader(new MemoryStream(array));
				binaryReader.ReadByte();
				uint num = binaryReader.ReadUInt32();
				if (num != 13U)
				{
					throw new FFUFlashException(Resources.ERROR_INVALID_DEVICE_PARAMS);
				}
				uint num2 = binaryReader.ReadUInt32();
				if (num2 < 16376U)
				{
					throw new FFUFlashException(Resources.ERROR_INVALID_DEVICE_PARAMS);
				}
				uint num3 = binaryReader.ReadUInt32();
				if ((ulong)num3 < (ulong)PacketConstructor.DefaultPacketDataSize || (ulong)num3 > (ulong)PacketConstructor.MaxPacketDataSize || (ulong)num3 % (ulong)PacketConstructor.DefaultPacketDataSize != 0UL)
				{
					throw new FFUFlashException(Resources.ERROR_INVALID_DEVICE_PARAMS);
				}
				this.usbTransactionSize = (int)num2;
				this.packets.PacketDataSize = (long)((ulong)num3);
				return;
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000664C File Offset: 0x0000484C
		private Stream GetBufferedFileStream(string path)
		{
			return new BufferedStream(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), 5242880);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006664 File Offset: 0x00004864
		private Stream GetStringStream(string src)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.BigEndianUnicode);
			binaryWriter.Write(src);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006698 File Offset: 0x00004898
		private void WriteCallback(IAsyncResult ar)
		{
			AutoResetEvent autoResetEvent = ar.AsyncState as AutoResetEvent;
			autoResetEvent.Set();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000066B8 File Offset: 0x000048B8
		private void SendPacket(byte[] packet, bool optimize)
		{
			bool flag = false;
			WaitHandle[] waitHandles = new WaitHandle[]
			{
				this.writeEvent,
				this.errorEvent
			};
			while (!flag)
			{
				try
				{
					for (int i = 0; i < packet.Length; i += this.usbTransactionSize)
					{
						if (optimize)
						{
							this.usbStream.BeginWrite(packet, i, Math.Min(this.usbTransactionSize, packet.Length - i), new AsyncCallback(this.WriteCallback), this.writeEvent);
							int num = WaitHandle.WaitAny(waitHandles);
							if (num == 1)
							{
								if (this.usbStream != null)
								{
									this.usbStream.Dispose();
									this.usbStream = null;
									this.fConnected = false;
								}
								this.hostLogger.EventWriteFlash_Error(this.DeviceUniqueID, this.DeviceFriendlyName);
								throw new FFUFlashException(this.DeviceFriendlyName, this.DeviceUniqueID, (FFUFlashException.ErrorCode)this.errId, string.Format(CultureInfo.CurrentCulture, Resources.ERROR_FLASH, new object[]
								{
									this.errInfo
								}));
							}
						}
						else
						{
							this.usbStream.Write(packet, i, Math.Min(this.usbTransactionSize, packet.Length - i));
						}
					}
					flag = (optimize || this.WaitForAck());
				}
				catch (Win32Exception ex)
				{
					this.hostLogger.EventWriteTransferException(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
					long position = this.packets.Position;
					this.WaitForReconnect();
					if (position != this.packets.Position)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006854 File Offset: 0x00004A54
		private bool WaitForAck()
		{
			for (;;)
			{
				byte[] array = new byte[this.usbTransactionSize];
				this.usbStream.Read(array, 0, array.Length);
				switch (array[0])
				{
				case 3:
					return true;
				case 5:
				{
					BinaryReader binaryReader = new BinaryReader(new MemoryStream(array));
					binaryReader.ReadByte();
					this.errId = (int)binaryReader.ReadInt16();
					this.deviceLogger.LogDeviceEvent(array, this.DeviceUniqueID, this.DeviceFriendlyName, out this.errInfo);
					if (string.IsNullOrEmpty(this.errInfo))
					{
						this.errId = 0;
						continue;
					}
					continue;
				}
				case 6:
					goto IL_9C;
				}
				break;
			}
			return false;
			IL_9C:
			this.usbStream.Dispose();
			this.usbStream = null;
			this.fConnected = false;
			this.hostLogger.EventWriteFlash_Error(this.DeviceUniqueID, this.DeviceFriendlyName);
			throw new FFUFlashException(this.DeviceFriendlyName, this.DeviceUniqueID, (FFUFlashException.ErrorCode)this.errId, string.Format(CultureInfo.CurrentCulture, Resources.ERROR_FLASH, new object[]
			{
				this.errInfo
			}));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000696C File Offset: 0x00004B6C
		private bool WaitForEndResponse(bool optimize)
		{
			return optimize || this.WaitForAck();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000697C File Offset: 0x00004B7C
		private bool WriteSkip(DTSFUsbStream skipStream)
		{
			skipStream.WriteByte(7);
			int num = skipStream.ReadByte();
			if (num == 3)
			{
				return true;
			}
			this.hostLogger.EventWriteWriteSkipFailed(this.DeviceUniqueID, this.DeviceFriendlyName, num);
			return false;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000069B8 File Offset: 0x00004BB8
		private void WaitForReconnect()
		{
			this.hostLogger.EventWriteDevice_Detach(this.DeviceUniqueID, this.DeviceFriendlyName);
			if (!this.DoWaitForDevice())
			{
				this.hostLogger.EventWriteFlash_Timeout(this.DeviceUniqueID, this.DeviceFriendlyName);
				throw new FFUException(this.DeviceFriendlyName, this.DeviceUniqueID, Resources.ERROR_RECONNECT_TIMEOUT);
			}
			if (0L == this.curPosition && this.resetCount < 3)
			{
				this.packets.Reset();
				this.resetCount++;
			}
			if (this.packets.Position - this.curPosition > this.packets.PacketDataSize)
			{
				throw new FFUException(this.DeviceFriendlyName, this.DeviceUniqueID, string.Format(CultureInfo.CurrentCulture, Resources.ERROR_RESUME_UNEXPECTED_POSITION, new object[]
				{
					this.packets.Position,
					this.curPosition
				}));
			}
			this.usbStream.WriteByte(2);
			this.hostLogger.EventWriteDevice_Attach(this.DeviceUniqueID, this.DeviceFriendlyName);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006AD0 File Offset: 0x00004CD0
		private bool DoWaitForDevice()
		{
			bool result = false;
			if (this.usbStream != null)
			{
				this.usbStream.Dispose();
				this.usbStream = null;
			}
			this.connectEvent.WaitOne(30000, false);
			lock (this.pathSync)
			{
				if (!this.AcquirePathMutex())
				{
					throw new FFUException(this.DeviceFriendlyName, this.DeviceUniqueID, Resources.ERROR_ACQUIRE_MUTEX);
				}
				try
				{
					bool flag2 = this.forceClearOnReconnect;
					this.forceClearOnReconnect = true;
					if (flag2)
					{
						using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromMilliseconds(100.0)))
						{
							this.ClearJunkDataFromStream(dtsfusbStream);
						}
					}
					this.usbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromMinutes(1.0));
					this.ReadBootmeFromStream(this.usbStream);
					result = true;
				}
				catch (IOException)
				{
					this.hostLogger.EventWriteReconnectIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
				}
				catch (Win32Exception ex)
				{
					this.hostLogger.EventWriteReconnectWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006C40 File Offset: 0x00004E40
		private void ClearJunkDataFromStream(DTSFUsbStream clearStream)
		{
			this.hostLogger.EventWriteStreamClearStart(this.DeviceUniqueID, this.DeviceFriendlyName);
			try
			{
				clearStream.PipeReset();
				for (int i = 0; i < 3; i++)
				{
					byte[] array = new byte[this.usbTransactionSize];
					for (int j = 0; j < 17; j++)
					{
						try
						{
							clearStream.Write(array, 0, array.Length);
						}
						catch (Win32Exception ex)
						{
							this.hostLogger.EventWriteStreamClearPushWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.ErrorCode);
						}
					}
					for (int k = 0; k < 5; k++)
					{
						try
						{
							clearStream.Read(array, 0, array.Length);
						}
						catch (Win32Exception ex2)
						{
							this.hostLogger.EventWriteStreamClearPullWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex2.ErrorCode);
						}
					}
				}
				clearStream.PipeReset();
			}
			catch (IOException)
			{
				this.hostLogger.EventWriteStreamClearIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
				this.connectEvent.WaitOne(5000, false);
			}
			Thread.Sleep(TimeSpan.FromSeconds(1.0));
			this.hostLogger.EventWriteStreamClearStop(this.DeviceUniqueID, this.DeviceFriendlyName);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006D90 File Offset: 0x00004F90
		private string GetPnPIdFromDevicePath(string path)
		{
			string text = path.Replace('#', '\\');
			text = text.Substring(4);
			return text.Remove(text.IndexOf('\\', 22));
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006DC4 File Offset: 0x00004FC4
		public void ErrorCallback(IAsyncResult ar)
		{
			this.usbStream.EndRead(ar);
			DTSFUsbStreamReadAsyncResult dtsfusbStreamReadAsyncResult = (DTSFUsbStreamReadAsyncResult)ar;
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(dtsfusbStreamReadAsyncResult.Buffer));
			binaryReader.ReadByte();
			this.errId = (int)binaryReader.ReadInt16();
			this.deviceLogger.LogDeviceEvent(dtsfusbStreamReadAsyncResult.Buffer, this.DeviceUniqueID, this.DeviceFriendlyName, out this.errInfo);
			if (string.IsNullOrEmpty(this.errInfo))
			{
				this.errId = 0;
			}
			ManualResetEvent manualResetEvent = ar.AsyncState as ManualResetEvent;
			manualResetEvent.Set();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006E7C File Offset: 0x0000507C
		private void TransferPackets(bool optimize)
		{
			while (this.packets.RemainingData > 0L)
			{
				this.hostLogger.EventWriteFileRead_Start(this.DeviceUniqueID, this.DeviceFriendlyName);
				byte[] packet = this.packets.GetNextPacket(optimize);
				this.hostLogger.EventWriteFileRead_Stop(this.DeviceUniqueID, this.DeviceFriendlyName);
				this.SendPacket(packet, optimize);
				if (this.ProgressEvent != null && (this.packets.Position - this.lastProgress > 1048576L || this.packets.Position == this.packets.Length))
				{
					this.lastProgress = this.packets.Position;
					ProgressEventArgs args = new ProgressEventArgs(this, this.packets.Position, this.packets.Length);
					Task.Factory.StartNew(delegate()
					{
						this.ProgressEvent(this, args);
					});
				}
			}
			if (this.packets.Length % this.packets.PacketDataSize == 0L)
			{
				byte[] packet = this.packets.GetZeroLengthPacket();
				this.SendPacket(packet, optimize);
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006FB0 File Offset: 0x000051B0
		private bool HasWimHeader(Stream wimStream)
		{
			byte[] array = new byte[]
			{
				77,
				83,
				87,
				73,
				77,
				0,
				0,
				0
			};
			byte[] array2 = new byte[array.Length];
			long position = wimStream.Position;
			wimStream.Read(array2, 0, array2.Length);
			wimStream.Position = position;
			return array.SequenceEqual(array2);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006FFC File Offset: 0x000051FC
		private void WriteWim(Stream sdiStream, Stream wimStream)
		{
			int num = 1048576;
			if (this.DeviceFriendlyName.Contains("Nokia.MSM8960.P4301"))
			{
				num = 16376;
			}
			bool flag = this.HasWimHeader(wimStream);
			byte[] array = new byte[12];
			uint value = 0U;
			if (flag)
			{
				value = (uint)sdiStream.Length;
			}
			BitConverter.GetBytes(value).CopyTo(array, 0);
			BitConverter.GetBytes((uint)wimStream.Length).CopyTo(array, 4);
			BitConverter.GetBytes(num).CopyTo(array, 8);
			byte[] buffer = new byte[num];
			Stream[] array2;
			if (flag)
			{
				array2 = new Stream[]
				{
					sdiStream,
					wimStream
				};
			}
			else
			{
				array2 = new Stream[]
				{
					wimStream
				};
			}
			this.usbStream.WriteByte(16);
			this.usbStream.Write(array, 0, array.Length);
			foreach (Stream stream in array2)
			{
				this.hostLogger.EventWriteWimTransferStart(this.DeviceUniqueID, this.DeviceFriendlyName);
				while (stream.Position < stream.Length)
				{
					int num2 = stream.Read(buffer, 0, num);
					this.hostLogger.EventWriteWimPacketStart(this.DeviceUniqueID, this.DeviceFriendlyName, num2);
					this.usbStream.Write(buffer, 0, num2);
					this.hostLogger.EventWriteWimPacketStop(this.DeviceUniqueID, this.DeviceFriendlyName, 0);
				}
				this.hostLogger.EventWriteWimTransferStop(this.DeviceUniqueID, this.DeviceFriendlyName);
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007178 File Offset: 0x00005378
		private bool ReadStatus()
		{
			this.hostLogger.EventWriteWimGetStatus(this.DeviceUniqueID, this.DeviceFriendlyName);
			byte[] array = new byte[4];
			this.usbStream.Read(array, 0, array.Length);
			int num = BitConverter.ToInt32(array, 0);
			bool flag = num >= 0;
			if (flag)
			{
				this.hostLogger.EventWriteWimSuccess(this.DeviceUniqueID, this.DeviceFriendlyName, num);
				return flag;
			}
			this.hostLogger.EventWriteWimError(this.DeviceUniqueID, this.DeviceFriendlyName, num);
			throw new FFUException(this.DeviceFriendlyName, this.DeviceUniqueID, string.Format(CultureInfo.CurrentCulture, Resources.ERROR_WIMBOOT, new object[]
			{
				num
			}));
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007230 File Offset: 0x00005430
		private void ReadDiskInfo(out int transferSize, out uint blockSize, out ulong lastBlock)
		{
			this.usbStream.WriteByte(12);
			byte[] array = new byte[16];
			this.usbStream.Read(array, 0, array.Length);
			int num = 0;
			transferSize = BitConverter.ToInt32(array, num);
			num += 4;
			blockSize = BitConverter.ToUInt32(array, num);
			num += 4;
			lastBlock = BitConverter.ToUInt64(array, num);
			num += 8;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000728C File Offset: 0x0000548C
		private bool NeedsToHandleZLP()
		{
			string[] array = new string[]
			{
				".*\\.MSM8960\\.*"
			};
			foreach (string pattern in array)
			{
				if (Regex.IsMatch(this.DeviceFriendlyName, pattern))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000072DC File Offset: 0x000054DC
		private void ReadDataToBuffer(ulong diskOffset, byte[] buffer, int offset, int count)
		{
			ulong value = (ulong)((long)count);
			this.usbStream.WriteByte(13);
			byte[] array = new byte[16];
			BitConverter.GetBytes(diskOffset).CopyTo(array, 0);
			BitConverter.GetBytes(value).CopyTo(array, 8);
			this.usbStream.Write(array, 0, array.Length);
			int i = offset;
			int num = offset + count;
			while (i < num)
			{
				int num2 = this.diskTransferSize;
				if (num2 > num - i)
				{
					num2 = num - i;
				}
				this.usbStream.Read(buffer, i, num2);
				if (num2 % 512 == 0 && this.NeedsToHandleZLP())
				{
					this.usbStream.ReadByte();
				}
				i += num2;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007380 File Offset: 0x00005580
		private void WriteDataFromBuffer(ulong diskOffset, byte[] buffer, int offset, int count)
		{
			ulong value = (ulong)((long)count);
			this.usbStream.WriteByte(14);
			byte[] array = new byte[16];
			BitConverter.GetBytes(diskOffset).CopyTo(array, 0);
			BitConverter.GetBytes(value).CopyTo(array, 8);
			this.usbStream.Write(array, 0, array.Length);
			int i = offset;
			int num = offset + count;
			while (i < num)
			{
				int num2 = this.diskTransferSize;
				if (num2 > num - i)
				{
					num2 = num - i;
				}
				this.usbStream.Write(buffer, i, num2);
				if (num2 % 512 == 0)
				{
					byte[] array2 = new byte[0];
					this.usbStream.Write(array2, 0, array2.Length);
				}
				i += num2;
			}
			byte[] array3 = new byte[8];
			this.usbStream.Read(array3, 0, array3.Length);
			if ((long)count != (long)BitConverter.ToUInt64(array3, 0))
			{
				throw new FFUDeviceDiskWriteException(this, Resources.ERROR_UNABLE_TO_COMPLETE_WRITE, null);
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007460 File Offset: 0x00005660
		private bool QueryForCommandAvailable(DTSFUsbStream idStream, SimpleIODevice.SioOpcode Cmd)
		{
			if (!this.hasCheckedForV2)
			{
				int num = 0;
				do
				{
					this.ReadBootmeFromStream(idStream);
					num++;
				}
				while (!this.supportsFastFlash && !this.supportsCompatFastFlash && this.clientVersion < 2 && num < 1000);
				this.hasCheckedForV2 = true;
			}
			if (this.clientVersion < 2)
			{
				return Cmd < SimpleIODevice.SioOpcode.SioFastFlash || this.supportsFastFlash;
			}
			idStream.WriteByte(23);
			idStream.WriteByte((byte)Cmd);
			return idStream.ReadByte() != 0;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000074E4 File Offset: 0x000056E4
		private unsafe void ReadBootmeFromStream(DTSFUsbStream idStream)
		{
			idStream.WriteByte(1);
			BinaryReader binaryReader = new BinaryReader(idStream);
			this.curPosition = binaryReader.ReadInt64();
			Guid guid = new Guid(binaryReader.ReadBytes(sizeof(Guid)));
			byte* ptr = (byte*)(&guid);
			if (*ptr >= 1)
			{
				this.supportsFastFlash = true;
			}
			else if (ptr[15] == 1)
			{
				this.supportsCompatFastFlash = true;
			}
			if (ptr[14] >= 1)
			{
				this.clientVersion = (int)(ptr[14] + 1);
			}
			this.DeviceUniqueID = new Guid(binaryReader.ReadBytes(sizeof(Guid)));
			this.DeviceFriendlyName = binaryReader.ReadString();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007580 File Offset: 0x00005780
		private bool ReadBootme()
		{
			bool result = false;
			for (int i = 0; i < 3; i++)
			{
				lock (this.pathSync)
				{
					if (this.syncMutex == null || !this.AcquirePathMutex())
					{
						return false;
					}
					try
					{
						if (i > 0)
						{
							using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromMilliseconds(100.0)))
							{
								this.ClearJunkDataFromStream(dtsfusbStream);
							}
						}
						using (DTSFUsbStream dtsfusbStream2 = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(2.0)))
						{
							this.ReadBootmeFromStream(dtsfusbStream2);
							result = true;
							break;
						}
					}
					catch (IOException)
					{
						this.hostLogger.EventWriteReadBootmeIOException(this.DeviceUniqueID, this.DeviceFriendlyName);
					}
					catch (Win32Exception ex)
					{
						this.hostLogger.EventWriteReadBootmeWin32Exception(this.DeviceUniqueID, this.DeviceFriendlyName, ex.NativeErrorCode);
					}
					finally
					{
						this.ReleasePathMutex();
					}
				}
			}
			return result;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000076D4 File Offset: 0x000058D4
		private Guid GetSerialNumberFromDevice()
		{
			Guid result = Guid.Empty;
			lock (this.pathSync)
			{
				if (this.syncMutex == null || !this.AcquirePathMutex())
				{
					return result;
				}
				try
				{
					using (DTSFUsbStream dtsfusbStream = new DTSFUsbStream(this.UsbDevicePath, TimeSpan.FromSeconds(1.0)))
					{
						byte[] array = new byte[16];
						dtsfusbStream.WriteByte(17);
						dtsfusbStream.Read(array, 0, array.Length);
						result = new Guid(array);
					}
				}
				catch (IOException)
				{
				}
				catch (Win32Exception)
				{
				}
				finally
				{
					this.ReleasePathMutex();
				}
			}
			return result;
		}

		// Token: 0x04000115 RID: 277
		private const int DefaultUSBTransactionSize = 16376;

		// Token: 0x04000116 RID: 278
		private const int DefaultWIMTransactionSize = 1048576;

		// Token: 0x04000117 RID: 279
		private const int MaxResets = 3;

		// Token: 0x04000118 RID: 280
		private const int ManufacturingProfileNameSizeInBytes = 128;

		// Token: 0x04000119 RID: 281
		private const int COMPATFLASH_MagicSequence = 1000;

		// Token: 0x0400011A RID: 282
		private const byte INDEX_SUPPORTCOMPATFLASH = 15;

		// Token: 0x0400011B RID: 283
		private const byte INDEX_SUPPORTV2CMDS = 14;

		// Token: 0x0400011C RID: 284
		private const byte INDEX_SUPPORTFASTFLASH = 0;

		// Token: 0x0400011D RID: 285
		private volatile bool fConnected;

		// Token: 0x0400011E RID: 286
		private volatile bool fOperationStarted;

		// Token: 0x0400011F RID: 287
		private DTSFUsbStream usbStream;

		// Token: 0x04000120 RID: 288
		private MemoryStream memStm;

		// Token: 0x04000121 RID: 289
		private AutoResetEvent connectEvent;

		// Token: 0x04000122 RID: 290
		private PacketConstructor packets;

		// Token: 0x04000123 RID: 291
		private FlashingHostLogger hostLogger;

		// Token: 0x04000124 RID: 292
		private FlashingDeviceLogger deviceLogger;

		// Token: 0x04000125 RID: 293
		private long curPosition;

		// Token: 0x04000126 RID: 294
		private Mutex syncMutex;

		// Token: 0x04000127 RID: 295
		private string usbDevicePath;

		// Token: 0x04000128 RID: 296
		private object pathSync;

		// Token: 0x04000129 RID: 297
		private int errId;

		// Token: 0x0400012A RID: 298
		private string errInfo;

		// Token: 0x0400012B RID: 299
		private int resetCount;

		// Token: 0x0400012C RID: 300
		private int diskTransferSize;

		// Token: 0x0400012D RID: 301
		private uint diskBlockSize;

		// Token: 0x0400012E RID: 302
		private ulong diskLastBlock;

		// Token: 0x0400012F RID: 303
		private long lastProgress;

		// Token: 0x04000130 RID: 304
		private bool forceClearOnReconnect;

		// Token: 0x04000131 RID: 305
		private Guid serialNumber;

		// Token: 0x04000132 RID: 306
		private bool serialNumberChecked;

		// Token: 0x04000133 RID: 307
		private int usbTransactionSize;

		// Token: 0x04000134 RID: 308
		private bool supportsFastFlash;

		// Token: 0x04000135 RID: 309
		private bool supportsCompatFastFlash;

		// Token: 0x04000136 RID: 310
		private bool hasCheckedForV2;

		// Token: 0x04000137 RID: 311
		private int clientVersion;

		// Token: 0x04000138 RID: 312
		private FlashingTelemetryLogger telemetryLogger;

		// Token: 0x04000139 RID: 313
		private ManualResetEvent errorEvent;

		// Token: 0x0400013A RID: 314
		private AutoResetEvent writeEvent;

		// Token: 0x0200004C RID: 76
		private enum SioOpcode : byte
		{
			// Token: 0x0400013F RID: 319
			SioId = 1,
			// Token: 0x04000140 RID: 320
			SioFlash,
			// Token: 0x04000141 RID: 321
			SioAck,
			// Token: 0x04000142 RID: 322
			SioNack,
			// Token: 0x04000143 RID: 323
			SioLog,
			// Token: 0x04000144 RID: 324
			SioErr,
			// Token: 0x04000145 RID: 325
			SioSkip,
			// Token: 0x04000146 RID: 326
			SioReset,
			// Token: 0x04000147 RID: 327
			SioFile,
			// Token: 0x04000148 RID: 328
			SioReboot,
			// Token: 0x04000149 RID: 329
			SioMassStorage,
			// Token: 0x0400014A RID: 330
			SioGetDiskInfo,
			// Token: 0x0400014B RID: 331
			SioReadDisk,
			// Token: 0x0400014C RID: 332
			SioWriteDisk,
			// Token: 0x0400014D RID: 333
			SioClearIdOverride,
			// Token: 0x0400014E RID: 334
			SioWim,
			// Token: 0x0400014F RID: 335
			SioSerialNumber,
			// Token: 0x04000150 RID: 336
			SioExternalWim,
			// Token: 0x04000151 RID: 337
			SioSetBootMode,
			// Token: 0x04000152 RID: 338
			SioFastFlash,
			// Token: 0x04000153 RID: 339
			SioDeviceParams,
			// Token: 0x04000154 RID: 340
			SioDeviceVersion,
			// Token: 0x04000155 RID: 341
			SioQueryForCmd,
			// Token: 0x04000156 RID: 342
			SioGetUpdateLogs,
			// Token: 0x04000157 RID: 343
			SioQueryDeviceUnlockId,
			// Token: 0x04000158 RID: 344
			SioRelockDeviceUnlockId,
			// Token: 0x04000159 RID: 345
			SioQueryUnlockTokenFiles,
			// Token: 0x0400015A RID: 346
			SioWriteUnlockTokenFile,
			// Token: 0x0400015B RID: 347
			SioQueryBitlockerState,
			// Token: 0x0400015C RID: 348
			SioLast = 29
		}
	}
}
