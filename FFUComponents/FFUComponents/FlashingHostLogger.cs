using System;
using System.Diagnostics.Eventing;

namespace FFUComponents
{
	// Token: 0x0200005B RID: 91
	public class FlashingHostLogger : IDisposable
	{
		// Token: 0x060001C8 RID: 456 RVA: 0x00009043 File Offset: 0x00007243
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.m_provider.Dispose();
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00009053 File Offset: 0x00007253
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009064 File Offset: 0x00007264
		public FlashingHostLogger()
		{
			this.Flash_Start = new EventDescriptor(0, 0, 0, 4, 1, 1, 0L);
			this.Flash_Stop = new EventDescriptor(1, 0, 0, 4, 2, 1, 0L);
			this.Device_Attach = new EventDescriptor(2, 0, 0, 4, 10, 2, 0L);
			this.Device_Detach = new EventDescriptor(3, 0, 0, 4, 11, 2, 0L);
			this.Device_Remove = new EventDescriptor(4, 0, 0, 4, 12, 2, 0L);
			this.Flash_Error = new EventDescriptor(5, 0, 0, 2, 0, 1, 0L);
			this.Flash_Timeout = new EventDescriptor(6, 0, 0, 2, 0, 1, 0L);
			this.TransferException = new EventDescriptor(7, 0, 0, 2, 0, 3, 0L);
			this.ReconnectIOException = new EventDescriptor(8, 0, 0, 2, 13, 8, 0L);
			this.ReconnectWin32Exception = new EventDescriptor(9, 0, 0, 2, 14, 8, 0L);
			this.ReadBootmeIOException = new EventDescriptor(10, 0, 0, 2, 13, 4, 0L);
			this.ReadBootmeWin32Exception = new EventDescriptor(11, 0, 0, 2, 14, 4, 0L);
			this.SkipIOException = new EventDescriptor(12, 0, 0, 2, 0, 5, 0L);
			this.SkipWin32Exception = new EventDescriptor(13, 0, 0, 2, 0, 5, 0L);
			this.WriteSkipFailed = new EventDescriptor(14, 0, 0, 2, 15, 5, 0L);
			this.USBResetWin32Exception = new EventDescriptor(15, 0, 0, 2, 14, 6, 0L);
			this.RebootIOException = new EventDescriptor(16, 0, 0, 2, 13, 7, 0L);
			this.RebootWin32Exception = new EventDescriptor(17, 0, 0, 2, 14, 7, 0L);
			this.ConnectWin32Exception = new EventDescriptor(18, 0, 0, 2, 14, 9, 0L);
			this.ThreadException = new EventDescriptor(19, 0, 0, 2, 15, 2, 0L);
			this.FileRead_Start = new EventDescriptor(20, 0, 0, 4, 1, 10, 0L);
			this.FileRead_Stop = new EventDescriptor(21, 0, 0, 4, 2, 10, 0L);
			this.WaitAbandoned = new EventDescriptor(22, 0, 0, 2, 2, 11, 0L);
			this.MutexTimeout = new EventDescriptor(23, 0, 0, 2, 2, 11, 0L);
			this.ConnectNotifyException = new EventDescriptor(24, 0, 0, 3, 10, 2, 0L);
			this.DisconnectNotifyException = new EventDescriptor(25, 0, 0, 3, 12, 2, 0L);
			this.InitNotifyException = new EventDescriptor(26, 0, 0, 3, 10, 2, 0L);
			this.MassStorageIOException = new EventDescriptor(27, 0, 0, 2, 13, 12, 0L);
			this.MassStorageWin32Exception = new EventDescriptor(28, 0, 0, 2, 14, 12, 0L);
			this.StreamClearStart = new EventDescriptor(29, 0, 0, 4, 1, 13, 0L);
			this.StreamClearStop = new EventDescriptor(30, 0, 0, 4, 2, 13, 0L);
			this.StreamClearPushWin32Exception = new EventDescriptor(31, 0, 0, 4, 14, 13, 0L);
			this.StreamClearPullWin32Exception = new EventDescriptor(32, 0, 0, 4, 14, 13, 0L);
			this.StreamClearIOException = new EventDescriptor(33, 0, 0, 4, 13, 13, 0L);
			this.ClearIdIOException = new EventDescriptor(34, 0, 0, 2, 13, 14, 0L);
			this.ClearIdWin32Exception = new EventDescriptor(35, 0, 0, 2, 14, 14, 0L);
			this.WimSuccess = new EventDescriptor(36, 0, 0, 4, 16, 15, 0L);
			this.WimError = new EventDescriptor(37, 0, 0, 2, 16, 15, 0L);
			this.WimIOException = new EventDescriptor(38, 0, 0, 2, 13, 15, 0L);
			this.WimWin32Exception = new EventDescriptor(39, 0, 0, 2, 14, 15, 0L);
			this.WimTransferStart = new EventDescriptor(40, 0, 0, 4, 1, 16, 0L);
			this.WimTransferStop = new EventDescriptor(41, 0, 0, 4, 2, 16, 0L);
			this.WimPacketStart = new EventDescriptor(42, 0, 0, 4, 1, 17, 0L);
			this.WimPacketStop = new EventDescriptor(43, 0, 0, 4, 2, 17, 0L);
			this.WimGetStatus = new EventDescriptor(44, 0, 0, 4, 1, 15, 0L);
			this.BootModeIOException = new EventDescriptor(45, 0, 0, 2, 13, 18, 0L);
			this.BootModeWin32Exception = new EventDescriptor(46, 0, 0, 2, 14, 18, 0L);
			this.DeviceFlashParameters = new EventDescriptor(47, 0, 0, 4, 0, 1, 0L);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00009599 File Offset: 0x00007799
		public bool EventWriteFlash_Start(Guid DeviceId, string DeviceFriendlyName, string AssemblyFileVersion)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEventWithString(ref this.Flash_Start, DeviceId, DeviceFriendlyName, AssemblyFileVersion);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000095BE File Offset: 0x000077BE
		public bool EventWriteFlash_Stop(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.Flash_Stop, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000095E2 File Offset: 0x000077E2
		public bool EventWriteDevice_Attach(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.Device_Attach, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00009606 File Offset: 0x00007806
		public bool EventWriteDevice_Detach(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.Device_Detach, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000962A File Offset: 0x0000782A
		public bool EventWriteDevice_Remove(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.Device_Remove, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000964E File Offset: 0x0000784E
		public bool EventWriteFlash_Error(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.Flash_Error, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00009672 File Offset: 0x00007872
		public bool EventWriteFlash_Timeout(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.Flash_Timeout, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009696 File Offset: 0x00007896
		public bool EventWriteTransferException(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.TransferException, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000096BB File Offset: 0x000078BB
		public bool EventWriteReconnectIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.ReconnectIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000096DF File Offset: 0x000078DF
		public bool EventWriteReconnectWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.ReconnectWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00009704 File Offset: 0x00007904
		public bool EventWriteReadBootmeIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.ReadBootmeIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00009728 File Offset: 0x00007928
		public bool EventWriteReadBootmeWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.ReadBootmeWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000974D File Offset: 0x0000794D
		public bool EventWriteSkipIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.SkipIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009771 File Offset: 0x00007971
		public bool EventWriteSkipWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.SkipWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009796 File Offset: 0x00007996
		public bool EventWriteWriteSkipFailed(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.WriteSkipFailed, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000097BB File Offset: 0x000079BB
		public bool EventWriteUSBResetWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.USBResetWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000097E0 File Offset: 0x000079E0
		public bool EventWriteRebootIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.RebootIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009804 File Offset: 0x00007A04
		public bool EventWriteRebootWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.RebootWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009829 File Offset: 0x00007A29
		public bool EventWriteConnectWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.ConnectWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000984E File Offset: 0x00007A4E
		public bool EventWriteThreadException(string String)
		{
			return this.m_provider.WriteEvent(ref this.ThreadException, String);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009862 File Offset: 0x00007A62
		public bool EventWriteFileRead_Start(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.FileRead_Start, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009886 File Offset: 0x00007A86
		public bool EventWriteFileRead_Stop(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.FileRead_Stop, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000098AA File Offset: 0x00007AAA
		public bool EventWriteWaitAbandoned(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.WaitAbandoned, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000098CE File Offset: 0x00007ACE
		public bool EventWriteMutexTimeout(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.MutexTimeout, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000098F2 File Offset: 0x00007AF2
		public bool EventWriteConnectNotifyException(string DevicePath, string Exception)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateNotifyException(ref this.ConnectNotifyException, DevicePath, Exception);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009916 File Offset: 0x00007B16
		public bool EventWriteDisconnectNotifyException(string DevicePath, string Exception)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateNotifyException(ref this.DisconnectNotifyException, DevicePath, Exception);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000993A File Offset: 0x00007B3A
		public bool EventWriteInitNotifyException(string DevicePath, string Exception)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateNotifyException(ref this.InitNotifyException, DevicePath, Exception);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000995E File Offset: 0x00007B5E
		public bool EventWriteMassStorageIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.MassStorageIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009982 File Offset: 0x00007B82
		public bool EventWriteMassStorageWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.MassStorageWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000099A7 File Offset: 0x00007BA7
		public bool EventWriteStreamClearStart(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.StreamClearStart, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000099CB File Offset: 0x00007BCB
		public bool EventWriteStreamClearStop(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.StreamClearStop, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000099EF File Offset: 0x00007BEF
		public bool EventWriteStreamClearPushWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.StreamClearPushWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009A14 File Offset: 0x00007C14
		public bool EventWriteStreamClearPullWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.StreamClearPullWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009A39 File Offset: 0x00007C39
		public bool EventWriteStreamClearIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.StreamClearIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009A5D File Offset: 0x00007C5D
		public bool EventWriteClearIdIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.ClearIdIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009A81 File Offset: 0x00007C81
		public bool EventWriteClearIdWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.ClearIdWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009AA6 File Offset: 0x00007CA6
		public bool EventWriteWimSuccess(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.WimSuccess, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009ACB File Offset: 0x00007CCB
		public bool EventWriteWimError(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.WimError, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009AF0 File Offset: 0x00007CF0
		public bool EventWriteWimIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.WimIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009B14 File Offset: 0x00007D14
		public bool EventWriteWimWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.WimWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00009B39 File Offset: 0x00007D39
		public bool EventWriteWimTransferStart(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.WimTransferStart, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009B5D File Offset: 0x00007D5D
		public bool EventWriteWimTransferStop(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.WimTransferStop, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009B81 File Offset: 0x00007D81
		public bool EventWriteWimPacketStart(Guid DeviceId, string DeviceFriendlyName, int TransferSize)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEventWithSize(ref this.WimPacketStart, DeviceId, DeviceFriendlyName, TransferSize);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00009BA6 File Offset: 0x00007DA6
		public bool EventWriteWimPacketStop(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.WimPacketStop, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00009BCB File Offset: 0x00007DCB
		public bool EventWriteWimGetStatus(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.WimGetStatus, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00009BEF File Offset: 0x00007DEF
		public bool EventWriteBootModeIOException(Guid DeviceId, string DeviceFriendlyName)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceSpecificEvent(ref this.BootModeIOException, DeviceId, DeviceFriendlyName);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00009C13 File Offset: 0x00007E13
		public bool EventWriteBootModeWin32Exception(Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceEventWithErrorCode(ref this.BootModeWin32Exception, DeviceId, DeviceFriendlyName, ErrorCode);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00009C38 File Offset: 0x00007E38
		public bool EventWriteDeviceFlashParameters(int USBTransactionSize, int PacketDataSize)
		{
			return !this.m_provider.IsEnabled() || this.m_provider.TemplateDeviceFlashParameters(ref this.DeviceFlashParameters, USBTransactionSize, PacketDataSize);
		}

		// Token: 0x0400018C RID: 396
		internal EventProviderVersionTwo m_provider = new EventProviderVersionTwo(new Guid("fb961307-bc64-4de4-8828-81d583524da0"));

		// Token: 0x0400018D RID: 397
		private Guid FlashId = new Guid("80ada65c-a7fa-49f8-a2ed-f67790c8f016");

		// Token: 0x0400018E RID: 398
		private Guid DeviceStatusChangeId = new Guid("3a02d575-c63d-4a76-9adf-9b6b736c66dc");

		// Token: 0x0400018F RID: 399
		private Guid TransferId = new Guid("211e6307-fd7c-49f9-a4db-d9ae5a4adb22");

		// Token: 0x04000190 RID: 400
		private Guid BootmeId = new Guid("a0cd9e55-fb70-452f-ac50-2eb82d2984b5");

		// Token: 0x04000191 RID: 401
		private Guid SkipId = new Guid("4979cb5a-17d4-47c6-9ac6-e97446bd74f4");

		// Token: 0x04000192 RID: 402
		private Guid ResetId = new Guid("768fda16-a5c7-44cf-8a47-03580b28538d");

		// Token: 0x04000193 RID: 403
		private Guid RebootId = new Guid("850eedee-9b52-4171-af6f-73c34d84a893");

		// Token: 0x04000194 RID: 404
		private Guid ReconnectId = new Guid("1a80ed37-3a4f-4b81-a466-accb411f96e1");

		// Token: 0x04000195 RID: 405
		private Guid ConnectId = new Guid("bebe24cb-92b1-40ca-843a-f2f9f0cab947");

		// Token: 0x04000196 RID: 406
		private Guid FileReadId = new Guid("d875a842-f690-40bf-880a-16e7d2a88d85");

		// Token: 0x04000197 RID: 407
		private Guid MutexWaitId = new Guid("3120aadc-6b30-4509-bedf-9696c78ddd9c");

		// Token: 0x04000198 RID: 408
		private Guid MassStorageId = new Guid("1b67e5c6-caab-4424-8d24-5c2c258aff5f");

		// Token: 0x04000199 RID: 409
		private Guid StreamClearId = new Guid("d32ce88a-c858-4ed1-86ac-764c58bf2599");

		// Token: 0x0400019A RID: 410
		private Guid ClearIdId = new Guid("3aa9618a-8ac9-4386-b524-c32f4326e59e");

		// Token: 0x0400019B RID: 411
		private Guid WimId = new Guid("0a86e459-1f85-459f-a9da-dca82415c492");

		// Token: 0x0400019C RID: 412
		private Guid WimTransferId = new Guid("53874dd6-905f-4a4c-ac66-5dadb02f4ce8");

		// Token: 0x0400019D RID: 413
		private Guid WimPacketId = new Guid("6f4a3de2-cddd-40d5-829d-861ccbcaff4d");

		// Token: 0x0400019E RID: 414
		private Guid BootModeId = new Guid("07bacab6-769a-4b6c-a68f-3524423291d2");

		// Token: 0x0400019F RID: 415
		protected EventDescriptor Flash_Start;

		// Token: 0x040001A0 RID: 416
		protected EventDescriptor Flash_Stop;

		// Token: 0x040001A1 RID: 417
		protected EventDescriptor Device_Attach;

		// Token: 0x040001A2 RID: 418
		protected EventDescriptor Device_Detach;

		// Token: 0x040001A3 RID: 419
		protected EventDescriptor Device_Remove;

		// Token: 0x040001A4 RID: 420
		protected EventDescriptor Flash_Error;

		// Token: 0x040001A5 RID: 421
		protected EventDescriptor Flash_Timeout;

		// Token: 0x040001A6 RID: 422
		protected EventDescriptor TransferException;

		// Token: 0x040001A7 RID: 423
		protected EventDescriptor ReconnectIOException;

		// Token: 0x040001A8 RID: 424
		protected EventDescriptor ReconnectWin32Exception;

		// Token: 0x040001A9 RID: 425
		protected EventDescriptor ReadBootmeIOException;

		// Token: 0x040001AA RID: 426
		protected EventDescriptor ReadBootmeWin32Exception;

		// Token: 0x040001AB RID: 427
		protected EventDescriptor SkipIOException;

		// Token: 0x040001AC RID: 428
		protected EventDescriptor SkipWin32Exception;

		// Token: 0x040001AD RID: 429
		protected EventDescriptor WriteSkipFailed;

		// Token: 0x040001AE RID: 430
		protected EventDescriptor USBResetWin32Exception;

		// Token: 0x040001AF RID: 431
		protected EventDescriptor RebootIOException;

		// Token: 0x040001B0 RID: 432
		protected EventDescriptor RebootWin32Exception;

		// Token: 0x040001B1 RID: 433
		protected EventDescriptor ConnectWin32Exception;

		// Token: 0x040001B2 RID: 434
		protected EventDescriptor ThreadException;

		// Token: 0x040001B3 RID: 435
		protected EventDescriptor FileRead_Start;

		// Token: 0x040001B4 RID: 436
		protected EventDescriptor FileRead_Stop;

		// Token: 0x040001B5 RID: 437
		protected EventDescriptor WaitAbandoned;

		// Token: 0x040001B6 RID: 438
		protected EventDescriptor MutexTimeout;

		// Token: 0x040001B7 RID: 439
		protected EventDescriptor ConnectNotifyException;

		// Token: 0x040001B8 RID: 440
		protected EventDescriptor DisconnectNotifyException;

		// Token: 0x040001B9 RID: 441
		protected EventDescriptor InitNotifyException;

		// Token: 0x040001BA RID: 442
		protected EventDescriptor MassStorageIOException;

		// Token: 0x040001BB RID: 443
		protected EventDescriptor MassStorageWin32Exception;

		// Token: 0x040001BC RID: 444
		protected EventDescriptor StreamClearStart;

		// Token: 0x040001BD RID: 445
		protected EventDescriptor StreamClearStop;

		// Token: 0x040001BE RID: 446
		protected EventDescriptor StreamClearPushWin32Exception;

		// Token: 0x040001BF RID: 447
		protected EventDescriptor StreamClearPullWin32Exception;

		// Token: 0x040001C0 RID: 448
		protected EventDescriptor StreamClearIOException;

		// Token: 0x040001C1 RID: 449
		protected EventDescriptor ClearIdIOException;

		// Token: 0x040001C2 RID: 450
		protected EventDescriptor ClearIdWin32Exception;

		// Token: 0x040001C3 RID: 451
		protected EventDescriptor WimSuccess;

		// Token: 0x040001C4 RID: 452
		protected EventDescriptor WimError;

		// Token: 0x040001C5 RID: 453
		protected EventDescriptor WimIOException;

		// Token: 0x040001C6 RID: 454
		protected EventDescriptor WimWin32Exception;

		// Token: 0x040001C7 RID: 455
		protected EventDescriptor WimTransferStart;

		// Token: 0x040001C8 RID: 456
		protected EventDescriptor WimTransferStop;

		// Token: 0x040001C9 RID: 457
		protected EventDescriptor WimPacketStart;

		// Token: 0x040001CA RID: 458
		protected EventDescriptor WimPacketStop;

		// Token: 0x040001CB RID: 459
		protected EventDescriptor WimGetStatus;

		// Token: 0x040001CC RID: 460
		protected EventDescriptor BootModeIOException;

		// Token: 0x040001CD RID: 461
		protected EventDescriptor BootModeWin32Exception;

		// Token: 0x040001CE RID: 462
		protected EventDescriptor DeviceFlashParameters;
	}
}
