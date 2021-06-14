using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using Nokia.Lucid.DeviceInformation;
using Nokia.Lucid.Interop.SafeHandles;
using Nokia.Lucid.Interop.Win32Types;

namespace Nokia.Lucid.Interop
{
	// Token: 0x02000031 RID: 49
	internal static class SetupApiNativeMethods
	{
		// Token: 0x0600013C RID: 316
		[DllImport("setupapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern IntPtr SetupDiGetClassDevsEx(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags, SafeDeviceInfoSetHandle DeviceInfoSet, IntPtr MachineName, IntPtr Reserved);

		// Token: 0x0600013D RID: 317
		[DllImport("setupapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern SafeDeviceInfoSetHandle SetupDiCreateDeviceInfoListEx(IntPtr ClassGuid, IntPtr hwndParent, IntPtr MachineName, IntPtr Reserved);

		// Token: 0x0600013E RID: 318
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("setupapi.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		// Token: 0x0600013F RID: 319
		[DllImport("setupapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiOpenDeviceInterface(SafeDeviceInfoSetHandle DeviceInfoSet, string DevicePath, int OpenFlags, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

		// Token: 0x06000140 RID: 320
		[DllImport("setupapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(SafeDeviceInfoSetHandle DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, IntPtr RequiredSize, ref SP_DEVINFO_DATA DeviceInfoData);

		// Token: 0x06000141 RID: 321
		[DllImport("setupapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDeviceInterfaceDetail(SafeDeviceInfoSetHandle DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, out int RequiredSize, IntPtr DeviceInfoData);

		// Token: 0x06000142 RID: 322
		[DllImport("setupapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDeviceInstanceId(SafeDeviceInfoSetHandle DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, IntPtr DeviceInstanceId, int DeviceInstanceIdSize, out int RequiredSize);

		// Token: 0x06000143 RID: 323
		[DllImport("setupapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDeviceInstanceId(SafeDeviceInfoSetHandle DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, StringBuilder DeviceInstanceId, int DeviceInstanceIdSize, IntPtr RequiredSize);

		// Token: 0x06000144 RID: 324
		[DllImport("setupapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiEnumDeviceInterfaces(SafeDeviceInfoSetHandle DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, int MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

		// Token: 0x06000145 RID: 325
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDeviceProperty(SafeDeviceInfoSetHandle DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, ref PropertyKey PropertyKey, out int PropertyType, byte[] PropertyBuffer, int PropertyBufferSize, IntPtr RequiredSize, int Flags);

		// Token: 0x06000146 RID: 326
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDeviceProperty(SafeDeviceInfoSetHandle DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, ref PropertyKey PropertyKey, out int PropertyType, IntPtr PropertyBuffer, int PropertyBufferSize, out int RequiredSize, int Flags);

		// Token: 0x06000147 RID: 327
		[DllImport("setupapi.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDevicePropertyKeys(SafeDeviceInfoSetHandle DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, [Out] PropertyKey[] PropertyKeyArray, int PropertyKeyCount, IntPtr RequiredPropertyKeyCount, int Flags);

		// Token: 0x06000148 RID: 328
		[DllImport("setupapi.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetupDiGetDevicePropertyKeys(SafeDeviceInfoSetHandle DeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, IntPtr PropertyKeyArray, int PropertyKeyCount, out int RequiredPropertyKeyCount, int Flags);

		// Token: 0x040000C6 RID: 198
		private const string SetupApiDllName = "setupapi.dll";
	}
}
