using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace FFUComponents
{
	// Token: 0x02000048 RID: 72
	internal static class NativeMethods
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x000043BE File Offset: 0x000025BE
		internal static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
		{
			return DeviceType << 16 | Access << 14 | Function << 2 | Method;
		}

		// Token: 0x060000C9 RID: 201
		[DllImport("ufphost.dll", SetLastError = true)]
		internal static extern uint CreateFlashingPlatform(ref IntPtr flashingPlatform);

		// Token: 0x060000CA RID: 202
		[DllImport("winusb.dll", EntryPoint = "WinUsb_Initialize", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbInitialize(SafeFileHandle deviceHandle, ref IntPtr interfaceHandle);

		// Token: 0x060000CB RID: 203
		[DllImport("winusb.dll", EntryPoint = "WinUsb_Free", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbFree(IntPtr interfaceHandle);

		// Token: 0x060000CC RID: 204
		[DllImport("winusb.dll", EntryPoint = "WinUsb_ControlTransfer", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbControlTransfer(IntPtr interfaceHandle, WinUsbSetupPacket setupPacket, IntPtr buffer, uint bufferLength, ref uint lengthTransferred, IntPtr overlapped);

		// Token: 0x060000CD RID: 205
		[DllImport("winusb.dll", EntryPoint = "WinUsb_ControlTransfer", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal unsafe static extern bool WinUsbControlTransfer(IntPtr interfaceHandle, WinUsbSetupPacket setupPacket, byte* buffer, uint bufferLength, ref uint lengthTransferred, IntPtr overlapped);

		// Token: 0x060000CE RID: 206
		[DllImport("winusb.dll", EntryPoint = "WinUsb_QueryInterfaceSettings", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbQueryInterfaceSettings(IntPtr interfaceHandle, byte alternateInterfaceNumber, ref WinUsbInterfaceDescriptor usbAltInterfaceDescriptor);

		// Token: 0x060000CF RID: 207
		[DllImport("winusb.dll", EntryPoint = "WinUsb_QueryPipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbQueryPipe(IntPtr interfaceHandle, byte alternateInterfaceNumber, byte pipeIndex, ref WinUsbPipeInformation pipeInformation);

		// Token: 0x060000D0 RID: 208
		[DllImport("winusb.dll", EntryPoint = "WinUsb_SetPipePolicy", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbSetPipePolicy(IntPtr interfaceHandle, byte pipeID, uint policyType, uint valueLength, ref bool value);

		// Token: 0x060000D1 RID: 209
		[DllImport("winusb.dll", EntryPoint = "WinUsb_SetPipePolicy", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbSetPipePolicy(IntPtr interfaceHandle, byte pipeID, uint policyType, uint valueLength, ref uint value);

		// Token: 0x060000D2 RID: 210
		[DllImport("winusb.dll", EntryPoint = "WinUsb_ResetPipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbResetPipe(IntPtr interfaceHandle, byte pipeID);

		// Token: 0x060000D3 RID: 211
		[DllImport("winusb.dll", EntryPoint = "WinUsb_AbortPipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbAbortPipe(IntPtr interfaceHandle, byte pipeID);

		// Token: 0x060000D4 RID: 212
		[DllImport("winusb.dll", EntryPoint = "WinUsb_FlushPipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbFlushPipe(IntPtr interfaceHandle, byte pipeID);

		// Token: 0x060000D5 RID: 213
		[DllImport("winusb.dll", EntryPoint = "WinUsb_ReadPipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal unsafe static extern bool WinUsbReadPipe(IntPtr interfaceHandle, byte pipeID, byte* buffer, uint bufferLength, IntPtr lenghtTransferred, NativeOverlapped* overlapped);

		// Token: 0x060000D6 RID: 214
		[DllImport("winusb.dll", EntryPoint = "WinUsb_WritePipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal unsafe static extern bool WinUsbWritePipe(IntPtr interfaceHandle, byte pipeID, byte* buffer, uint bufferLength, IntPtr lenghtTransferred, NativeOverlapped* overlapped);

		// Token: 0x060000D7 RID: 215
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, string enumerator, IntPtr parent, int flags);

		// Token: 0x060000D8 RID: 216
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, ref DeviceInterfaceData deviceInterfaceData);

		// Token: 0x060000D9 RID: 217
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref DeviceInterfaceData deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);

		// Token: 0x060000DA RID: 218
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal unsafe static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref DeviceInterfaceData deviceInterfaceData, DeviceInterfaceDetailData* deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, ref DeviceInformationData deviceInfoData);

		// Token: 0x060000DB RID: 219
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern SafeFileHandle CreateFile(string fileName, uint desiredAccess, uint shareMode, IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes, IntPtr templateFileHandle);

		// Token: 0x060000DC RID: 220
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060000DD RID: 221
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CancelIo(SafeFileHandle handle);

		// Token: 0x060000DE RID: 222
		[DllImport("iphlpapi.dll", ExactSpelling = true)]
		public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

		// Token: 0x060000DF RID: 223
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, int Flags);

		// Token: 0x060000E0 RID: 224
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnregisterDeviceNotification(IntPtr hNotification);

		// Token: 0x060000E1 RID: 225
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		// Token: 0x060000E2 RID: 226
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr SetupDiGetClassDevs(IntPtr ClassGuid, string Enumerator, IntPtr hwndParent, uint Flags);

		// Token: 0x060000E3 RID: 227
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		// Token: 0x060000E4 RID: 228
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, SP_DEVINFO_DATA DeviceInfoData);

		// Token: 0x060000E5 RID: 229
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiGetDeviceProperty(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, DEVPROPKEY PropertyKey, out uint PropertyType, out uint PropertyBuffer, uint PropertyBufferSize, IntPtr RequiredSize, uint Flags);

		// Token: 0x060000E6 RID: 230
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiGetDeviceProperty(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, DEVPROPKEY PropertyKey, out uint PropertyType, MY_FILETIME PropertyBuffer, uint PropertyBufferSize, IntPtr RequiredSize, uint Flags);

		// Token: 0x060000E7 RID: 231
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiGetDeviceProperty(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, DEVPROPKEY PropertyKey, out uint PropertyType, ref Guid PropertyBuffer, uint PropertyBufferSize, IntPtr RequiredSize, uint Flags);

		// Token: 0x060000E8 RID: 232
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiGetDeviceProperty(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, DEVPROPKEY PropertyKey, out uint PropertyType, IntPtr PropertyBuffer, uint PropertyBufferSize, out int RequiredSize, uint Flags);

		// Token: 0x060000E9 RID: 233
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiGetDeviceProperty(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, DEVPROPKEY PropertyKey, out uint PropertyType, StringBuilder PropertyBuffer, int PropertyBufferSize, IntPtr RequiredSize, uint Flags);

		// Token: 0x060000EA RID: 234
		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, uint MemberIndex, SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

		// Token: 0x060000EB RID: 235
		[DllImport("cfgmgr32.dll", CharSet = CharSet.Auto)]
		internal static extern uint CM_Get_Parent(out uint pdnDevInst, uint dnDevInst, uint ulFlags);

		// Token: 0x060000EC RID: 236
		[DllImport("cfgmgr32.dll", CharSet = CharSet.Auto)]
		internal static extern uint CM_Get_Device_ID_Size(out uint pulLen, uint dnDevInst, uint ulFlags);

		// Token: 0x060000ED RID: 237
		[DllImport("cfgmgr32.dll", CharSet = CharSet.Auto)]
		internal static extern uint CM_Get_Device_ID(uint dnDevInst, StringBuilder Buffer, uint BufferLen, uint ulFlags);

		// Token: 0x060000EE RID: 238
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool SetupDiGetDeviceInterfaceDetailW(IntPtr DeviceInfoSet, SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, [In] [Out] SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, uint DeviceInterfaceDetailDataSize, IntPtr RequiredSize, IntPtr DeviceInfoData);

		// Token: 0x060000EF RID: 239
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DeviceIoControl(SafeFileHandle hDevice, uint dwIoControlCode, USB_NODE_CONNECTION_INFORMATION_EX_V2 lpInBuffer, uint nInBufferSize, USB_NODE_CONNECTION_INFORMATION_EX_V2 lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

		// Token: 0x040000F9 RID: 249
		internal const uint CR_SUCCESS = 0U;

		// Token: 0x040000FA RID: 250
		internal const uint GENERIC_WRITE = 1073741824U;

		// Token: 0x040000FB RID: 251
		internal const uint FILE_SHARE_READ = 1U;

		// Token: 0x040000FC RID: 252
		internal const uint FILE_SHARE_WRITE = 2U;

		// Token: 0x040000FD RID: 253
		internal const uint FILE_SHARE_DELETE = 4U;

		// Token: 0x040000FE RID: 254
		internal const uint USB_GET_NODE_CONNECTION_INFORMATION_EX_V2 = 279U;

		// Token: 0x040000FF RID: 255
		internal const uint FILE_DEVICE_USB = 34U;

		// Token: 0x04000100 RID: 256
		internal const uint FILE_DEVICE_UNKNOWN = 34U;

		// Token: 0x04000101 RID: 257
		internal const uint METHOD_BUFFERED = 0U;

		// Token: 0x04000102 RID: 258
		internal const uint FILE_ANY_ACCESS = 0U;

		// Token: 0x04000103 RID: 259
		internal static DEVPROPKEY DEVPKEY_Device_Address = new DEVPROPKEY(new Guid(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224), 30U);

		// Token: 0x04000104 RID: 260
		internal static DEVPROPKEY DEVPKEY_Device_InstallDate = new DEVPROPKEY(new Guid(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41), 100U);

		// Token: 0x04000105 RID: 261
		internal static DEVPROPKEY DEVPKEY_Device_ContainerId = new DEVPROPKEY(new Guid(2357121542U, 16266, 18471, 179, 171, 174, 158, 31, 174, 252, 108), 2U);

		// Token: 0x04000106 RID: 262
		internal static DEVPROPKEY DEVPKEY_DeviceContainer_FriendlyName = new DEVPROPKEY(new Guid(1701460915U, 60608, 17405, 132, 119, 74, 224, 64, 74, 150, 205), 12288U);

		// Token: 0x04000107 RID: 263
		internal static DEVPROPKEY DEVPKEY_Device_DeviceDesc = new DEVPROPKEY(new Guid(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224), 2U);

		// Token: 0x04000108 RID: 264
		internal static DEVPROPKEY DEVPKEY_Device_FriendlyName = new DEVPROPKEY(new Guid(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224), 14U);

		// Token: 0x04000109 RID: 265
		internal static DEVPROPKEY DEVPKEY_Device_Manufacturer = new DEVPROPKEY(new Guid(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224), 13U);

		// Token: 0x0400010A RID: 266
		internal static IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

		// Token: 0x0400010B RID: 267
		internal static Guid GUID_DEVINTERFACE_USB_HUB = new Guid(4052356744U, 49932, 4560, 136, 21, 0, 160, 201, 6, 190, 216);

		// Token: 0x0400010C RID: 268
		internal static uint IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX_V2 = NativeMethods.CTL_CODE(34U, 279U, 0U, 0U);
	}
}
