using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace FFUComponents
{
	// Token: 0x0200000C RID: 12
	public class DeviceSet
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002970 File Offset: 0x00000B70
		public DeviceSet(string DevinstId)
		{
			this.Initialize(DevinstId);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002980 File Offset: 0x00000B80
		public uint GetAddress()
		{
			uint num = 0U;
			uint num2;
			if (!NativeMethods.SetupDiGetDeviceProperty(this.deviceSetHandle, this.deviceInfoData, NativeMethods.DEVPKEY_Device_Address, out num2, out num, (uint)Marshal.SizeOf<uint>(num), IntPtr.Zero, 0U))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new FFUException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_SETUP_DI_GET_DEVICE_PROPERTY, new object[]
				{
					lastWin32Error
				}));
			}
			return num;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000029EC File Offset: 0x00000BEC
		public string GetParentId()
		{
			uint devinst;
			uint num = NativeMethods.CM_Get_Parent(out devinst, this.deviceInfoData.DevInst, 0U);
			if (num != 0U)
			{
				throw new FFUException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_CM_GET_PARENT, new object[]
				{
					num
				}));
			}
			return DeviceSet.DeviceIdFromCmDevinst(devinst);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002A3C File Offset: 0x00000C3C
		public string GetHubDevicePath()
		{
			SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
			if (!NativeMethods.SetupDiEnumDeviceInterfaces(this.deviceSetHandle, IntPtr.Zero, ref NativeMethods.GUID_DEVINTERFACE_USB_HUB, 0U, deviceInterfaceData))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new FFUException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_SETUP_DI_ENUM_DEVICE_INTERFACES, new object[]
				{
					lastWin32Error
				}));
			}
			SP_DEVICE_INTERFACE_DETAIL_DATA sp_DEVICE_INTERFACE_DETAIL_DATA = new SP_DEVICE_INTERFACE_DETAIL_DATA();
			if (!NativeMethods.SetupDiGetDeviceInterfaceDetailW(this.deviceSetHandle, deviceInterfaceData, sp_DEVICE_INTERFACE_DETAIL_DATA, (uint)Marshal.SizeOf<SP_DEVICE_INTERFACE_DETAIL_DATA>(sp_DEVICE_INTERFACE_DETAIL_DATA), IntPtr.Zero, IntPtr.Zero))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new FFUException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_SETUP_DI_GET_DEVICE_INTERFACE_DETAIL_W, new object[]
				{
					lastWin32Error
				}));
			}
			return sp_DEVICE_INTERFACE_DETAIL_DATA.DevicePath.ToString();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002AFC File Offset: 0x00000CFC
		private static string DeviceIdFromCmDevinst(uint devinst)
		{
			uint num2;
			uint num = NativeMethods.CM_Get_Device_ID_Size(out num2, devinst, 0U);
			if (num != 0U)
			{
				throw new FFUException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_CM_GET_DEVICE_ID_SIZE, new object[]
				{
					num
				}));
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Capacity = (int)(num2 + 1U);
			num = NativeMethods.CM_Get_Device_ID(devinst, stringBuilder, (uint)stringBuilder.Capacity, 0U);
			if (num != 0U)
			{
				throw new FFUException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_CM_GET_DEVICE_ID, new object[]
				{
					num
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B8C File Offset: 0x00000D8C
		private void GetDeviceInfoData()
		{
			SP_DEVINFO_DATA sp_DEVINFO_DATA = new SP_DEVINFO_DATA();
			bool flag = NativeMethods.SetupDiEnumDeviceInfo(this.deviceSetHandle, 0U, sp_DEVINFO_DATA);
			if (flag)
			{
				this.deviceInfoData = sp_DEVINFO_DATA;
				return;
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			throw new FFUException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_SETUP_DI_ENUM_DEVICE_INFO, new object[]
			{
				lastWin32Error
			}));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002BE4 File Offset: 0x00000DE4
		private void Initialize(string DevinstId)
		{
			if (string.IsNullOrEmpty(DevinstId))
			{
				throw new FFUException(Resources.ERROR_NULL_OR_EMPTY_STRING);
			}
			this.deviceInfoData = null;
			this.deviceSetHandle = NativeMethods.SetupDiGetClassDevs(IntPtr.Zero, DevinstId, IntPtr.Zero, 22U);
			if (this.deviceSetHandle == NativeMethods.INVALID_HANDLE_VALUE)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new FFUException(string.Format(CultureInfo.CurrentCulture, Resources.ERROR_INVALID_HANDLE, new object[]
				{
					DevinstId,
					lastWin32Error
				}));
			}
			this.GetDeviceInfoData();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C6C File Offset: 0x00000E6C
		~DeviceSet()
		{
			if (NativeMethods.INVALID_HANDLE_VALUE != this.deviceSetHandle)
			{
				NativeMethods.SetupDiDestroyDeviceInfoList(this.deviceSetHandle);
				this.deviceSetHandle = NativeMethods.INVALID_HANDLE_VALUE;
			}
		}

		// Token: 0x0400000F RID: 15
		private IntPtr deviceSetHandle;

		// Token: 0x04000010 RID: 16
		private SP_DEVINFO_DATA deviceInfoData;
	}
}
