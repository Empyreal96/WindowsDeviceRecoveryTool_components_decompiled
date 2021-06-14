using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using ClickerUtilityLibrary.Comm;
using ClickerUtilityLibrary.Comm.USBDriver;
using ClickerUtilityLibrary.DataModel;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary
{
	// Token: 0x02000004 RID: 4
	public class ClickerBlUpdater : IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000007 RID: 7 RVA: 0x00002084 File Offset: 0x00000284
		// (remove) Token: 0x06000008 RID: 8 RVA: 0x000020BC File Offset: 0x000002BC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<BlUpdaterEventArgs> UpdaterEvent;

		// Token: 0x06000009 RID: 9 RVA: 0x000020F4 File Offset: 0x000002F4
		protected virtual void OnUpdaterEvent(BlUpdaterEventArgs updaterEventArgs)
		{
			bool flag = this.UpdaterEvent != null;
			if (flag)
			{
				this.UpdaterEvent(this, updaterEventArgs);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002120 File Offset: 0x00000320
		private void LogInfo(string Value)
		{
			bool flag = this.mLogger != null;
			if (flag)
			{
				this.mLogger.LogInfo(Value);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000214C File Offset: 0x0000034C
		private void LogError(string Value)
		{
			bool flag = this.mLogger != null;
			if (flag)
			{
				this.mLogger.LogError(Value);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002178 File Offset: 0x00000378
		public ClickerBlUpdater(ILogger Logger)
		{
			bool flag = Logger != null;
			if (flag)
			{
				this.mLogger = Logger;
			}
			CommandEngine.Instance.CommandEngineEvent += this.ReceivedEventHandler;
			bool flag2 = this.deviceList.Devices.Count > 0;
			if (flag2)
			{
				UsbDevice value = this.deviceList.Devices.First<KeyValuePair<string, UsbDevice>>().Value;
				CommandEngine.Instance.StartCommandEngine(BootLoaderProtocol.Instance, value);
			}
			else
			{
				CommandEngine.Instance.StartCommandEngine(BootLoaderProtocol.Instance);
			}
			this.InitializeDataElements();
			this.mUpdateState = ClickerBlUpdater.UPDATE_STATE.FLASHING_UPDATER;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002273 File Offset: 0x00000473
		public ClickerBlUpdater() : this(null)
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002280 File Offset: 0x00000480
		private void InitializeDataElements()
		{
			this.mFwAddress = DataElementDictionary.Instance[DataElementType.DI_FW_ADDRESS];
			this.mFwSize = DataElementDictionary.Instance[DataElementType.DI_FW_SIZE];
			this.mChecksum = DataElementDictionary.Instance[DataElementType.DI_FW_CHECKSUM];
			this.mTransferOffset = DataElementDictionary.Instance[DataElementType.DI_FW_TRANSFER_OFFSET];
			this.mFWBinary = DataElementDictionary.Instance[DataElementType.DI_FW_BINARY];
			this.mFwBLVer = DataElementDictionary.Instance[DataElementType.DI_BL_VER];
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022F8 File Offset: 0x000004F8
		public bool StartBootLoaderDownload(string bootLoaderFilename, string bootLoaderUpdaterFilename)
		{
			bool flag = this.RunBootLoader();
			bool flag2 = !flag;
			bool result;
			if (flag2)
			{
				this.LogError("Failed to run the boot loader.");
				result = false;
			}
			else
			{
				this.LogInfo("Reading application firmware configuration.");
				this.mUpdateState = ClickerBlUpdater.UPDATE_STATE.BACKUP_APPCONFIG;
				ClickerBlUpdater.ReadFirmwareConfiguration();
				int num = 0;
				while (this.mUpdateState == ClickerBlUpdater.UPDATE_STATE.BACKUP_APPCONFIG && num < 5)
				{
					Thread.Sleep(1000);
					num++;
				}
				bool flag3 = this.mUpdateState == ClickerBlUpdater.UPDATE_STATE.BACKUP_APPCONFIG;
				if (flag3)
				{
					this.LogError("Timed out while reading the firmware configuration.");
					result = false;
				}
				else
				{
					this.LogInfo("Bootloader version is " + this.mFwBLVer.HexData);
					this.LogInfo("Start Bootloader-Updater downloading.");
					flag = this.OpenFile(bootLoaderUpdaterFilename);
					bool flag4 = !flag;
					if (flag4)
					{
						result = false;
					}
					else
					{
						this.SendProgressUpdate();
						this.WriteBootLoaderUpdaterConguration();
						num = 0;
						while (this.mUpdateState == ClickerBlUpdater.UPDATE_STATE.FLASHING_UPDATER && num < 5)
						{
							Thread.Sleep(1000);
							num++;
						}
						bool flag5 = this.mUpdateState == ClickerBlUpdater.UPDATE_STATE.FLASHING_UPDATER;
						if (flag5)
						{
							this.LogError("Timed out waiting for the boot loader updater to be downloaded.");
							result = false;
						}
						else
						{
							flag = this.RunBootLoaderUpdater(bootLoaderFilename);
							bool flag6 = !flag;
							if (flag6)
							{
								this.LogInfo("Boot Loader Updater couldn't be run.");
								result = false;
							}
							else
							{
								bool flag7 = !this.mFlashingCompleted.WaitOne(50000);
								if (flag7)
								{
									this.LogInfo("Timed out waiting for the boot loader to be downloaded.");
									result = false;
								}
								else
								{
									flag = this.RunBootLoader();
									bool flag8 = !flag;
									if (flag8)
									{
										this.LogError("Failed to run the boot loader.");
										result = false;
									}
									else
									{
										BlUpdaterEventArgs updaterEventArgs = new BlUpdaterEventArgs
										{
											Type = BlUpdaterEventArgs.EventType.UpdateCompleted
										};
										this.OnUpdaterEvent(updaterEventArgs);
										result = true;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000024C4 File Offset: 0x000006C4
		private static void ReadFirmwareConfiguration()
		{
			IPacket packet = BootLoaderProtocol.Instance.CreateNewPacket();
			BootLoaderProtocol.Instance.FormCommandPacket(packet, 3);
			CommandEngine.Instance.SendRawData(packet.RawPacket, packet.Length);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002504 File Offset: 0x00000704
		public bool GetBootLoaderVersion(out string version)
		{
			bool flag = this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToBootLoader;
			if (flag)
			{
				CommandResponse.CommandSent(CommandResponse.CommandCode.BL_VER);
				IPacket packet = BootLoaderProtocol.Instance.CreateNewPacket();
				BootLoaderProtocol.Instance.FormCommandPacket(packet, 0);
				CommandEngine.Instance.SendRawData(packet.RawPacket, packet.Length);
			}
			bool result = true;
			version = CommandResponse.GetResponse(CommandResponse.CommandCode.BL_VER);
			bool flag2 = version == null;
			if (flag2)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002575 File Offset: 0x00000775
		private void WriteBootLoaderUpdaterConguration()
		{
			this.mFwAddress.Data = 458752U;
			ClickerBlUpdater.WriteFirmwareConfiguration();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002594 File Offset: 0x00000794
		private static void WriteFirmwareConfiguration()
		{
			IPacket packet = BootLoaderProtocol.Instance.CreateNewPacket();
			BootLoaderProtocol.Instance.FormCommandPacket(packet, 2);
			CommandEngine.Instance.SendRawData(packet.RawPacket, packet.Length);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025D4 File Offset: 0x000007D4
		private void ResetDevice()
		{
			bool flag = this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToApplication;
			if (flag)
			{
				IPacket packet = AppProtocol.Instance.CreateNewPacket();
				AppProtocol.Instance.FormResetCommandPacket(packet);
				CommandEngine.Instance.SendRawData(packet.RawPacket, packet.Length);
			}
			else
			{
				bool flag2 = this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToBootLoader || this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToBootLoaderUpdater;
				if (flag2)
				{
					IPacket packet2 = BootLoaderProtocol.Instance.CreateNewPacket();
					BootLoaderProtocol.Instance.FormCommandPacket(packet2, 9);
					CommandEngine.Instance.SendRawData(packet2.RawPacket, packet2.Length);
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000266C File Offset: 0x0000086C
		private bool OpenFile(string FileName)
		{
			try
			{
				this.mFileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
				bool flag = this.mFileStream.Length > (long)this.mBuffer.Length;
				if (flag)
				{
					return false;
				}
				this.mFwSize.Data = (uint)this.mFileStream.Length;
				this.mFileStream.Read(this.mBuffer, 0, (int)this.mFileStream.Length);
				this.mChecksum.Data = BootLoaderProtocol.Instance.CalculateChecksum(this.mBuffer, 0, (int)this.mFileStream.Length);
				this.mFileStream.Position = 0L;
				Array.Clear(this.mBuffer, 0, 1048576);
			}
			catch (Exception ex)
			{
				bool flag2 = this.mLogger != null;
				if (flag2)
				{
					this.LogError(ex.Message);
				}
			}
			return true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002770 File Offset: 0x00000970
		public bool RunBootLoader()
		{
			bool flag = this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToApplication || this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToBootLoaderUpdater;
			if (flag)
			{
				this.mDeviceConnectedToApp.Reset();
				this.mDeviceConnectedToBootLoaderUpdater.Reset();
				this.ResetDevice();
				bool flag2 = !this.mDeviceConnectedToBootLoader.WaitOne(5000);
				if (flag2)
				{
					this.LogInfo("Couldn't connect to the boot loader on the device.");
					return false;
				}
				ClickerFwUpdater.PingDevice();
			}
			else
			{
				this.mDeviceConnectedToBootLoader.Reset();
			}
			return true;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000027FC File Offset: 0x000009FC
		private bool RunBootLoaderUpdater(string bootLoaderFilename)
		{
			bool flag = this.deviceStatus != ClickerBlUpdater.DeviceStatus.ConnectedToBootLoaderUpdater;
			if (flag)
			{
				bool flag2 = !this.mDeviceConnectedToBootLoaderUpdater.WaitOne(5000);
				if (flag2)
				{
					this.LogInfo("Couldn't connect to the boot loader on the device.");
					return false;
				}
			}
			else
			{
				this.mDeviceConnectedToBootLoaderUpdater.Reset();
			}
			try
			{
				bool flag3 = this.OpenFile(bootLoaderFilename);
				bool flag4 = !flag3;
				if (flag4)
				{
					return false;
				}
				this.SendProgressUpdate();
				bool flag5 = this.mFileStream.Length - this.mFileStream.Position <= 4096L;
				if (flag5)
				{
					this.mFWBinary.Length = (int)((ushort)(this.mFileStream.Length - this.mFileStream.Position));
					this.mIsEOF = true;
				}
				else
				{
					this.mFWBinary.Length = 4096;
					this.mIsEOF = false;
				}
				IPacket packet = BootLoaderProtocol.Instance.CreateNewPacket();
				bool flag6 = this.mFWBinary.Length != 0;
				if (flag6)
				{
					this.mFwAddress.Data = 0U;
					this.mTransferOffset.Data = (long)((ulong)((uint)this.mFwAddress.Data) + (ulong)this.mFileStream.Position);
					this.mFileStream.Read(this.mFWBinary.GetRawData(), 0, this.mFWBinary.Length);
					BootLoaderProtocol.Instance.FormDataPacket(packet);
					CommandEngine.Instance.SendRawData(packet.RawPacket, packet.Length);
				}
			}
			catch (Exception ex)
			{
				this.mFileStream = null;
				this.LogError(ex.Message);
			}
			return true;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000029D8 File Offset: 0x00000BD8
		private void ReceivedEventHandler(object sender, FEvent receivedEventArgs)
		{
			BlUpdaterEventArgs blUpdaterEventArgs = new BlUpdaterEventArgs();
			switch (receivedEventArgs.EventType)
			{
			case EventType.ExceptionMessage:
			{
				string value = DateTime.Now.ToString("HH:mm:ss: Error - ", CultureInfo.CurrentCulture) + receivedEventArgs.StringParameter;
				this.LogInfo(value);
				receivedEventArgs.StringParameter = "";
				break;
			}
			case EventType.OperationMessage:
			{
				string value = DateTime.Now.ToString("HH:mm:ss:\t", CultureInfo.CurrentCulture) + receivedEventArgs.StringParameter;
				this.LogInfo(value);
				LogManager.Instance.Log(receivedEventArgs.StringParameter);
				receivedEventArgs.StringParameter = "";
				break;
			}
			case EventType.PacketReceived:
			{
				FPacket fpacket = receivedEventArgs.ObjectParameter as FPacket;
				bool flag = fpacket == null;
				if (!flag)
				{
					bool flag2 = this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToBootLoader || this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToBootLoaderUpdater;
					if (flag2)
					{
						this.ProcessBootLoaderPacket(fpacket);
					}
				}
				break;
			}
			case EventType.UsbDeviceConnected:
			{
				string text = receivedEventArgs.ObjectParameter as string;
				bool flag3 = text != null && ClickerFwUpdater.IsBootLoaderUsbFriendlyName(text);
				if (flag3)
				{
					this.deviceStatus = ClickerBlUpdater.DeviceStatus.ConnectedToBootLoader;
					CommandEngine.Instance.ChangeProtocol(BootLoaderProtocol.Instance);
					this.UpdateConnectionEvents();
					string value = "Device is connected.(BL)";
					this.LogInfo(value);
					blUpdaterEventArgs.Type = BlUpdaterEventArgs.EventType.ConnectedToBootLoader;
					this.OnUpdaterEvent(blUpdaterEventArgs);
				}
				else
				{
					bool flag4 = text != null && ClickerFwUpdater.IsBootLoaderUpdaterUsbFriendlyName(text);
					if (flag4)
					{
						this.deviceStatus = ClickerBlUpdater.DeviceStatus.ConnectedToBootLoaderUpdater;
						CommandEngine.Instance.ChangeProtocol(BootLoaderProtocol.Instance);
						this.UpdateConnectionEvents();
						string value = "Device is connected.(BL Updater)";
						this.LogInfo(value);
						blUpdaterEventArgs.Type = BlUpdaterEventArgs.EventType.ConnectedToBootLoaderUpdater;
						this.OnUpdaterEvent(blUpdaterEventArgs);
					}
					else
					{
						this.deviceStatus = ClickerBlUpdater.DeviceStatus.ConnectedToApplication;
						CommandEngine.Instance.ChangeProtocol(AppProtocol.Instance);
						this.UpdateConnectionEvents();
						string value = "Device is connected.(App)";
						this.LogInfo(value);
						blUpdaterEventArgs.Type = BlUpdaterEventArgs.EventType.ConnectedToApplication;
						this.OnUpdaterEvent(blUpdaterEventArgs);
					}
				}
				break;
			}
			case EventType.UsbDeviceDisconnected:
			{
				this.deviceStatus = ClickerBlUpdater.DeviceStatus.Disconnected;
				this.UpdateConnectionEvents();
				string value = "Device is disconnected!";
				this.LogInfo(value);
				blUpdaterEventArgs.Type = BlUpdaterEventArgs.EventType.DeviceDisconnected;
				this.OnUpdaterEvent(blUpdaterEventArgs);
				break;
			}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002C0C File Offset: 0x00000E0C
		private void UpdateConnectionEvents()
		{
			bool flag = this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToApplication;
			if (flag)
			{
				this.mDeviceConnectedToBootLoader.Reset();
				this.mDeviceConnectedToBootLoaderUpdater.Reset();
				this.mDeviceConnectedToApp.Set();
			}
			else
			{
				bool flag2 = this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToBootLoader;
				if (flag2)
				{
					this.mDeviceConnectedToBootLoader.Set();
					this.mDeviceConnectedToBootLoaderUpdater.Reset();
					this.mDeviceConnectedToApp.Reset();
				}
				else
				{
					bool flag3 = this.deviceStatus == ClickerBlUpdater.DeviceStatus.ConnectedToBootLoaderUpdater;
					if (flag3)
					{
						this.mDeviceConnectedToBootLoader.Reset();
						this.mDeviceConnectedToBootLoaderUpdater.Set();
						this.mDeviceConnectedToApp.Reset();
					}
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002CB8 File Offset: 0x00000EB8
		private void ProcessBootLoaderPacket(FPacket packet)
		{
			try
			{
				bool flag = packet.Command == 2;
				if (flag)
				{
					bool flag2 = this.mUpdateState == ClickerBlUpdater.UPDATE_STATE.RESTORING_APPCONFIG;
					if (flag2)
					{
						this.mUpdateState = ClickerBlUpdater.UPDATE_STATE.BACKUP_APPCONFIG;
						this.mFlashingCompleted.Set();
					}
					else
					{
						IPacket packet2 = BootLoaderProtocol.Instance.CreateNewPacket();
						bool flag3 = this.mFileStream.Length < 4096L;
						if (flag3)
						{
							this.mFWBinary.Length = (int)((ushort)this.mFileStream.Length);
							this.mIsEOF = true;
						}
						else
						{
							this.mFWBinary.Length = 4096;
							this.mIsEOF = false;
						}
						this.mTransferOffset.Data = (long)((ulong)((uint)this.mFwAddress.Data) + (ulong)this.mFileStream.Position);
						this.mFileStream.Read(this.mFWBinary.GetRawData(), 0, this.mFWBinary.Length);
						BootLoaderProtocol.Instance.FormDataPacket(packet2);
						CommandEngine.Instance.SendRawData(packet2.RawPacket, packet2.Length);
						this.SendProgressUpdate();
					}
				}
				else
				{
					bool flag4 = packet.Command == 3;
					if (flag4)
					{
						this.BackupFirmwareConfiguration();
						this.mUpdateState = ClickerBlUpdater.UPDATE_STATE.FLASHING_UPDATER;
					}
					else
					{
						bool flag5 = packet.Command == 4;
						if (flag5)
						{
							IPacket packet3 = BootLoaderProtocol.Instance.CreateNewPacket();
							FCommand fcommand = CommandDictionary.Instance[4];
							bool flag6 = this.mFileStream == null;
							if (flag6)
							{
								bool flag7 = this.mLastPacketSent;
								if (flag7)
								{
									bool flag8 = this.mUpdateState == ClickerBlUpdater.UPDATE_STATE.FLASHING_UPDATER;
									if (flag8)
									{
										this.mUpdateState = ClickerBlUpdater.UPDATE_STATE.FLASHING_BL;
										this.LogInfo("Bootloader-Updater is successfully flashed!");
										this.LogInfo("Continue booting to Bootloader-Updater !");
										ClickerFwUpdater.RunApplication();
									}
									else
									{
										this.mUpdateState = ClickerBlUpdater.UPDATE_STATE.RESTORING_APPCONFIG;
										this.LogInfo("Bootloader is successfully flashed!");
										this.LogInfo("Restoring App configuration!");
										this.RestoreFirmwareConfiguration();
									}
								}
							}
							else
							{
								bool flag9 = (uint)fcommand.ResponseArgs[0].Data > 0U;
								if (flag9)
								{
									this.LogInfo("Flash Error!");
									this.mTransferOffset.Data = 0;
									this.mFWBinary.Length = 0;
									this.mFileStream.Close();
									this.mFileStream = null;
									this.mIsEOF = false;
									this.mLastPacketSent = false;
									this.mUpdateState = ClickerBlUpdater.UPDATE_STATE.BACKUP_APPCONFIG;
								}
								else
								{
									bool flag10 = this.mIsEOF;
									if (flag10)
									{
										this.mFileStream.Close();
										this.mFileStream = null;
										this.mTransferOffset.Data = 0;
										this.mFWBinary.Length = 0;
										BootLoaderProtocol.Instance.FormDataPacket(packet3);
										CommandEngine.Instance.SendRawData(packet3.RawPacket, packet3.Length);
										this.mIsEOF = false;
										this.mLastPacketSent = true;
									}
									else
									{
										bool flag11 = this.mFileStream.Length - this.mFileStream.Position <= 4096L;
										if (flag11)
										{
											this.mFWBinary.Length = (int)((ushort)(this.mFileStream.Length - this.mFileStream.Position));
											this.mIsEOF = true;
										}
										else
										{
											this.mFWBinary.Length = 4096;
											this.mIsEOF = false;
										}
										bool flag12 = this.mFWBinary.Length != 0;
										if (flag12)
										{
											this.mTransferOffset.Data = (long)((ulong)((uint)this.mFwAddress.Data) + (ulong)this.mFileStream.Position);
											this.mFileStream.Read(this.mFWBinary.GetRawData(), 0, this.mFWBinary.Length);
											BootLoaderProtocol.Instance.FormDataPacket(packet3);
											CommandEngine.Instance.SendRawData(packet3.RawPacket, packet3.Length);
										}
										this.SendProgressUpdate();
									}
								}
							}
						}
						else
						{
							bool flag13 = packet.Command == 0;
							if (flag13)
							{
								DataElement dataElement = DataElementDictionary.Instance[DataElementType.DI_BL_VER];
								int version = Convert.ToInt32(dataElement.Data, CultureInfo.InvariantCulture);
								ImageVersion imageVersion = new ImageVersion(version);
								CommandResponse.UpdateResponseData(CommandResponse.CommandCode.BL_VER, imageVersion.ToString());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.LogError(ex.Message);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003128 File Offset: 0x00001328
		private void RestoreFirmwareConfiguration()
		{
			int num;
			for (int i = 0; i < 6; i = num + 1)
			{
				CommandDictionary.Instance[2].Args[i].Data = this.mAppFWConfigBackup[i];
				num = i;
			}
			ClickerBlUpdater.WriteFirmwareConfiguration();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000317C File Offset: 0x0000137C
		private void BackupFirmwareConfiguration()
		{
			this.mAppFWConfigBackup = new uint[6];
			int num;
			for (int i = 1; i < 7; i = num + 1)
			{
				this.mAppFWConfigBackup[i - 1] = (uint)CommandDictionary.Instance[3].ResponseArgs[i].Data;
				num = i;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000031D4 File Offset: 0x000013D4
		private void SendProgressUpdate()
		{
			string value = string.Concat(new object[]
			{
				"Sending ",
				this.mFileStream.Position,
				" / ",
				this.mFileStream.Length
			});
			this.LogInfo(value);
			BlUpdaterEventArgs blUpdaterEventArgs = new BlUpdaterEventArgs
			{
				Type = BlUpdaterEventArgs.EventType.UpdateProgress
			};
			BlUpdaterUpdateProgressEventParameters blUpdaterUpdateProgressEventParameters = new BlUpdaterUpdateProgressEventParameters
			{
				Progress = (double)this.mFileStream.Position / (double)this.mFileStream.Length
			};
			ClickerBlUpdater.UPDATE_STATE update_STATE = this.mUpdateState;
			if (update_STATE != ClickerBlUpdater.UPDATE_STATE.FLASHING_UPDATER)
			{
				if (update_STATE == ClickerBlUpdater.UPDATE_STATE.FLASHING_BL)
				{
					blUpdaterUpdateProgressEventParameters.UpdateType = BlUpdaterUpdateProgressEventParameters.EventSubType.BootLoader;
				}
			}
			else
			{
				blUpdaterUpdateProgressEventParameters.UpdateType = BlUpdaterUpdateProgressEventParameters.EventSubType.BootLoaderUpdater;
			}
			blUpdaterEventArgs.Parameters = blUpdaterUpdateProgressEventParameters;
			this.OnUpdaterEvent(blUpdaterEventArgs);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000032A0 File Offset: 0x000014A0
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				CommandEngine.Instance.CommandEngineEvent -= this.ReceivedEventHandler;
				bool flag = this.mFileStream != null;
				if (flag)
				{
					this.mFileStream.Dispose();
				}
				bool flag2 = this.mDeviceConnectedToApp != null;
				if (flag2)
				{
					this.mDeviceConnectedToApp.Dispose();
				}
				bool flag3 = this.mDeviceConnectedToBootLoader != null;
				if (flag3)
				{
					this.mDeviceConnectedToBootLoader.Dispose();
				}
				bool flag4 = this.mDeviceConnectedToBootLoaderUpdater != null;
				if (flag4)
				{
					this.mDeviceConnectedToBootLoaderUpdater.Dispose();
				}
				bool flag5 = this.mFlashingCompleted != null;
				if (flag5)
				{
					this.mFlashingCompleted.Dispose();
				}
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000335A File Offset: 0x0000155A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000005 RID: 5
		private const ushort MAXIMUM_BIN_PARTITION = 4096;

		// Token: 0x04000006 RID: 6
		private const uint DEFAULT_BL_ADDRESS = 0U;

		// Token: 0x04000007 RID: 7
		private const uint DEFAULT_FW_ADDRESS = 131072U;

		// Token: 0x04000008 RID: 8
		private const uint DEFAULT_BL_UPDATER_ADDRESS = 458752U;

		// Token: 0x04000009 RID: 9
		private const uint MAXIMUMMEMORYSIZE = 1048576U;

		// Token: 0x0400000B RID: 11
		private readonly AutoResetEvent mDeviceConnectedToBootLoader = new AutoResetEvent(false);

		// Token: 0x0400000C RID: 12
		private readonly AutoResetEvent mFlashingCompleted = new AutoResetEvent(false);

		// Token: 0x0400000D RID: 13
		private readonly AutoResetEvent mDeviceConnectedToApp = new AutoResetEvent(false);

		// Token: 0x0400000E RID: 14
		private readonly AutoResetEvent mDeviceConnectedToBootLoaderUpdater = new AutoResetEvent(false);

		// Token: 0x0400000F RID: 15
		private readonly UsbDevices deviceList = new UsbDevices();

		// Token: 0x04000010 RID: 16
		private ClickerBlUpdater.DeviceStatus deviceStatus = ClickerBlUpdater.DeviceStatus.Disconnected;

		// Token: 0x04000011 RID: 17
		private readonly ILogger mLogger;

		// Token: 0x04000012 RID: 18
		private ClickerBlUpdater.UPDATE_STATE mUpdateState;

		// Token: 0x04000013 RID: 19
		private DataElement mFwAddress;

		// Token: 0x04000014 RID: 20
		private DataElement mFwSize;

		// Token: 0x04000015 RID: 21
		private DataElement mChecksum;

		// Token: 0x04000016 RID: 22
		private DataElement mTransferOffset;

		// Token: 0x04000017 RID: 23
		private DataElement mFWBinary;

		// Token: 0x04000018 RID: 24
		private DataElement mFwBLVer;

		// Token: 0x04000019 RID: 25
		private FileStream mFileStream;

		// Token: 0x0400001A RID: 26
		private bool mIsEOF = true;

		// Token: 0x0400001B RID: 27
		private uint[] mAppFWConfigBackup;

		// Token: 0x0400001C RID: 28
		private readonly byte[] mBuffer = new byte[1048576];

		// Token: 0x0400001D RID: 29
		private bool mLastPacketSent;

		// Token: 0x0200003F RID: 63
		private enum DeviceStatus
		{
			// Token: 0x04000172 RID: 370
			Disconnected,
			// Token: 0x04000173 RID: 371
			ConnectedToBootLoader,
			// Token: 0x04000174 RID: 372
			ConnectedToApplication,
			// Token: 0x04000175 RID: 373
			ConnectedToBootLoaderUpdater
		}

		// Token: 0x02000040 RID: 64
		private enum UPDATE_STATE
		{
			// Token: 0x04000177 RID: 375
			BACKUP_APPCONFIG,
			// Token: 0x04000178 RID: 376
			FLASHING_UPDATER,
			// Token: 0x04000179 RID: 377
			FLASHING_BL,
			// Token: 0x0400017A RID: 378
			RESTORING_APPCONFIG
		}
	}
}
