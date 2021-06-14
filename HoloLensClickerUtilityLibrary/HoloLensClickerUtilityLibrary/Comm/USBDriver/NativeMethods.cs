using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000035 RID: 53
	internal static class NativeMethods
	{
		// Token: 0x0600012E RID: 302
		[DllImport("winusb.dll", EntryPoint = "WinUsb_Initialize", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbInitialize(SafeFileHandle deviceHandle, ref IntPtr interfaceHandle);

		// Token: 0x0600012F RID: 303
		[DllImport("winusb.dll", EntryPoint = "WinUsb_Free", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbFree(IntPtr interfaceHandle);

		// Token: 0x06000130 RID: 304
		[DllImport("winusb.dll", EntryPoint = "WinUsb_QueryInterfaceSettings", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbQueryInterfaceSettings(IntPtr interfaceHandle, byte alternateInterfaceNumber, ref WinUsbInterfaceDescriptor usbAltInterfaceDescriptor);

		// Token: 0x06000131 RID: 305
		[DllImport("winusb.dll", EntryPoint = "WinUsb_QueryPipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbQueryPipe(IntPtr interfaceHandle, byte alternateInterfaceNumber, byte pipeIndex, ref WinUsbPipeInformation pipeInformation);

		// Token: 0x06000132 RID: 306
		[DllImport("winusb.dll", EntryPoint = "WinUsb_SetPipePolicy", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbSetPipePolicy(IntPtr interfaceHandle, byte pipeID, uint policyType, uint valueLength, ref bool value);

		// Token: 0x06000133 RID: 307
		[DllImport("winusb.dll", EntryPoint = "WinUsb_SetPipePolicy", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WinUsbSetPipePolicy(IntPtr interfaceHandle, byte pipeID, uint policyType, uint valueLength, ref uint value);

		// Token: 0x06000134 RID: 308
		[DllImport("winusb.dll", EntryPoint = "WinUsb_ReadPipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal unsafe static extern bool WinUsbReadPipe(IntPtr interfaceHandle, byte pipeID, byte* buffer, uint bufferLength, out uint lengthTransferred, NativeOverlapped* overlapped);

		// Token: 0x06000135 RID: 309
		[DllImport("winusb.dll", EntryPoint = "WinUsb_WritePipe", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal unsafe static extern bool WinUsbWritePipe(IntPtr interfaceHandle, byte pipeID, byte* buffer, uint bufferLength, out uint lengthTransferred, NativeOverlapped* overlapped);

		// Token: 0x06000136 RID: 310
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, string enumerator, int parent, int flags);

		// Token: 0x06000137 RID: 311
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, int deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, ref DeviceInterfaceData deviceInterfaceData);

		// Token: 0x06000138 RID: 312
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref DeviceInterfaceData deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);

		// Token: 0x06000139 RID: 313
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal unsafe static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref DeviceInterfaceData deviceInterfaceData, DeviceInterfaceDetailData* deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, ref DeviceInformationData deviceInfoData);

		// Token: 0x0600013A RID: 314
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal unsafe static extern bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, ref DeviceInformationData deviceInfoData, DeviceRegistryProperties property, out uint propertyRegDataType, byte* propertyBuffer, uint propertyBufferSize, out uint requiredSize);

		// Token: 0x0600013B RID: 315
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetupDiGetDeviceInstanceId(IntPtr deviceInfoSet, ref DeviceInformationData deviceInfoData, IntPtr deviceInstanceId, uint deviceInstanceIdSize, out uint requiredSize);

		// Token: 0x0600013C RID: 316
		[DllImport("newdev.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DiUninstallDevice(IntPtr hwdParent, IntPtr deviceInfoSet, ref DeviceInformationData deviceInfoData, uint flags, out bool needReboot);

		// Token: 0x0600013D RID: 317
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(string fileName, uint desiredAccess, uint shareMode, IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes, IntPtr templateFileHandle);
	}
}
