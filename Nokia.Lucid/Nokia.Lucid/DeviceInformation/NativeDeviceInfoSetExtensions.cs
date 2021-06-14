using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Nokia.Lucid.Interop;
using Nokia.Lucid.Interop.Win32Types;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x02000016 RID: 22
	internal static class NativeDeviceInfoSetExtensions
	{
		// Token: 0x06000094 RID: 148 RVA: 0x000050E0 File Offset: 0x000032E0
		public static string GetDeviceInstanceId(this INativeDeviceInfoSet deviceInfoSet, ref SP_DEVINFO_DATA deviceData)
		{
			int num;
			if (!SetupApiNativeMethods.SetupDiGetDeviceInstanceId(deviceInfoSet.SafeDeviceInfoSetHandle, ref deviceData, IntPtr.Zero, 0, out num) && Marshal.GetLastWin32Error() != 122)
			{
				throw new Win32Exception();
			}
			StringBuilder stringBuilder = new StringBuilder(num);
			if (!SetupApiNativeMethods.SetupDiGetDeviceInstanceId(deviceInfoSet.SafeDeviceInfoSetHandle, ref deviceData, stringBuilder, num, IntPtr.Zero))
			{
				throw new Win32Exception();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000513C File Offset: 0x0000333C
		public static bool TryGetDeviceProperty(this INativeDeviceInfoSet deviceInfoSet, ref SP_DEVINFO_DATA deviceData, ref PropertyKey propertyKey, out int propertyType, out byte[] value)
		{
			int num;
			if (!SetupApiNativeMethods.SetupDiGetDeviceProperty(deviceInfoSet.SafeDeviceInfoSetHandle, ref deviceData, ref propertyKey, out propertyType, IntPtr.Zero, 0, out num, 0) && Marshal.GetLastWin32Error() != 122)
			{
				value = null;
				return false;
			}
			byte[] array = new byte[num];
			if (!SetupApiNativeMethods.SetupDiGetDeviceProperty(deviceInfoSet.SafeDeviceInfoSetHandle, ref deviceData, ref propertyKey, out propertyType, array, num, IntPtr.Zero, 0))
			{
				value = null;
				return false;
			}
			value = array;
			return true;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000519C File Offset: 0x0000339C
		public static PropertyKey[] GetDevicePropertyKeys(this INativeDeviceInfoSet deviceInfoSet, ref SP_DEVINFO_DATA deviceData)
		{
			int num;
			if (!SetupApiNativeMethods.SetupDiGetDevicePropertyKeys(deviceInfoSet.SafeDeviceInfoSetHandle, ref deviceData, IntPtr.Zero, 0, out num, 0) && Marshal.GetLastWin32Error() != 122)
			{
				throw new Win32Exception();
			}
			PropertyKey[] array = new PropertyKey[num];
			if (!SetupApiNativeMethods.SetupDiGetDevicePropertyKeys(deviceInfoSet.SafeDeviceInfoSetHandle, ref deviceData, array, num, IntPtr.Zero, 0))
			{
				throw new Win32Exception();
			}
			return array;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000051F4 File Offset: 0x000033F4
		public static byte[] GetDeviceProperty(this INativeDeviceInfoSet deviceInfoSet, ref SP_DEVINFO_DATA deviceData, ref PropertyKey propertyKey, out int propertyType)
		{
			int num;
			if (!SetupApiNativeMethods.SetupDiGetDeviceProperty(deviceInfoSet.SafeDeviceInfoSetHandle, ref deviceData, ref propertyKey, out propertyType, IntPtr.Zero, 0, out num, 0) && Marshal.GetLastWin32Error() != 122)
			{
				throw new Win32Exception();
			}
			byte[] array = new byte[num];
			if (!SetupApiNativeMethods.SetupDiGetDeviceProperty(deviceInfoSet.SafeDeviceInfoSetHandle, ref deviceData, ref propertyKey, out propertyType, array, num, IntPtr.Zero, 0))
			{
				throw new Win32Exception();
			}
			return array;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005250 File Offset: 0x00003450
		public static SP_DEVICE_INTERFACE_DATA GetDeviceInterface(this INativeDeviceInfoSet deviceInfoSet, string path)
		{
			SP_DEVICE_INTERFACE_DATA result = new SP_DEVICE_INTERFACE_DATA
			{
				cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA))
			};
			if (SetupApiNativeMethods.SetupDiOpenDeviceInterface(deviceInfoSet.SafeDeviceInfoSetHandle, path, 1, ref result))
			{
				return result;
			}
			if (Marshal.GetLastWin32Error() == -536870363)
			{
				string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidOperationException_MessageFormat_CouldNotRetrieveDeviceInfo, new object[]
				{
					path
				});
				throw new InvalidOperationException(message);
			}
			throw new Win32Exception();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000052C4 File Offset: 0x000034C4
		public static void AddDeviceInterfaceClass(this INativeDeviceInfoSet deviceInfoSet, Guid interfaceClass)
		{
			IntPtr value = SetupApiNativeMethods.SetupDiGetClassDevsEx(ref interfaceClass, IntPtr.Zero, IntPtr.Zero, 16, deviceInfoSet.SafeDeviceInfoSetHandle, IntPtr.Zero, IntPtr.Zero);
			if (value == IntPtr.Zero || value == new IntPtr(-1))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005470 File Offset: 0x00003670
		public static IEnumerable<SP_DEVICE_INTERFACE_DATA> EnumerateDeviceInterfaces(this INativeDeviceInfoSet deviceInfoSet, Guid interfaceClass)
		{
			SP_DEVICE_INTERFACE_DATA interfaceData = new SP_DEVICE_INTERFACE_DATA
			{
				cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA))
			};
			int index = 0;
			while (SetupApiNativeMethods.SetupDiEnumDeviceInterfaces(deviceInfoSet.SafeDeviceInfoSetHandle, IntPtr.Zero, ref interfaceClass, index, ref interfaceData))
			{
				yield return interfaceData;
				index++;
			}
			yield break;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005494 File Offset: 0x00003694
		public static string GetDevicePath(this INativeDeviceInfoSet deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA interfaceData, out SP_DEVINFO_DATA deviceData)
		{
			int num;
			if (!SetupApiNativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet.SafeDeviceInfoSetHandle, ref interfaceData, IntPtr.Zero, 0, out num, IntPtr.Zero) && Marshal.GetLastWin32Error() != 122)
			{
				throw new Win32Exception();
			}
			IntPtr intPtr = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			string result;
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					intPtr = Marshal.AllocHGlobal(num);
				}
				deviceData = new SP_DEVINFO_DATA
				{
					cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA))
				};
				SP_DEVICE_INTERFACE_DETAIL_DATA sp_DEVICE_INTERFACE_DETAIL_DATA = default(SP_DEVICE_INTERFACE_DETAIL_DATA);
				if (IntPtr.Size == 8)
				{
					sp_DEVICE_INTERFACE_DETAIL_DATA.cbSize = 8;
				}
				else
				{
					sp_DEVICE_INTERFACE_DETAIL_DATA.cbSize = 4 + Marshal.SystemDefaultCharSize;
				}
				Marshal.StructureToPtr(sp_DEVICE_INTERFACE_DETAIL_DATA, intPtr, false);
				if (!SetupApiNativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet.SafeDeviceInfoSetHandle, ref interfaceData, intPtr, num, IntPtr.Zero, ref deviceData))
				{
					throw new Win32Exception();
				}
				int offset = Marshal.OffsetOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA), "DevicePath").ToInt32();
				string text = Marshal.PtrToStringAuto(intPtr + offset);
				result = text;
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return result;
		}
	}
}
