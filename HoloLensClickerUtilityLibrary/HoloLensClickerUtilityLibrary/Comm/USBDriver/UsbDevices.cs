using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x0200003A RID: 58
	public class UsbDevices
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00008235 File Offset: 0x00006435
		// (set) Token: 0x0600015B RID: 347 RVA: 0x0000823D File Offset: 0x0000643D
		public Dictionary<string, UsbDevice> Devices { get; private set; }

		// Token: 0x0600015C RID: 348 RVA: 0x00008246 File Offset: 0x00006446
		public UsbDevices()
		{
			this.Devices = new Dictionary<string, UsbDevice>();
			this.DiscoverUsbDevices();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008264 File Offset: 0x00006464
		private unsafe void DiscoverUsbDevices()
		{
			this.Devices = new Dictionary<string, UsbDevice>();
			IntPtr intPtr = NativeMethods.SetupDiGetClassDevs(ref UsbDevices.UsbIfGuid, null, 0, 18);
			bool flag = IntPtr.Zero == intPtr;
			if (flag)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new Win32Exception(lastWin32Error);
			}
			int num = 0;
			int lastWin32Error2;
			int lastWin32Error3;
			int lastWin32Error4;
			for (;;)
			{
				int num2 = 0;
				DeviceInterfaceData deviceInterfaceData = new DeviceInterfaceData
				{
					Size = Marshal.SizeOf(typeof(DeviceInterfaceData))
				};
				bool flag2 = !NativeMethods.SetupDiEnumDeviceInterfaces(intPtr, 0, ref UsbDevices.UsbIfGuid, num, ref deviceInterfaceData);
				if (flag2)
				{
					break;
				}
				bool flag3 = !NativeMethods.SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, IntPtr.Zero, 0, ref num2, IntPtr.Zero);
				if (flag3)
				{
					lastWin32Error2 = Marshal.GetLastWin32Error();
					bool flag4 = lastWin32Error2 != 122;
					if (flag4)
					{
						goto Block_5;
					}
				}
				DeviceInterfaceDetailData* ptr = (DeviceInterfaceDetailData*)((void*)Marshal.AllocHGlobal(num2));
				bool flag5 = IntPtr.Size == 4;
				if (flag5)
				{
					ptr->Size = 6;
				}
				else
				{
					ptr->Size = 8;
				}
				DeviceInformationData deviceInformationData = new DeviceInformationData
				{
					Size = Marshal.SizeOf(typeof(DeviceInformationData))
				};
				bool flag6 = !NativeMethods.SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, ptr, num2, ref num2, ref deviceInformationData);
				if (flag6)
				{
					goto Block_7;
				}
				string text = Marshal.PtrToStringAuto(new IntPtr((void*)(&ptr->DevicePath)));
				bool flag7 = text == null;
				if (flag7)
				{
					goto Block_8;
				}
				Marshal.FreeHGlobal((IntPtr)((void*)ptr));
				uint num3;
				uint num4;
				bool flag8 = NativeMethods.SetupDiGetDeviceRegistryProperty(intPtr, ref deviceInformationData, DeviceRegistryProperties.SPDRP_FRIENDLYNAME, out num3, (byte*)((void*)IntPtr.Zero), 0U, out num4);
				bool flag9 = !flag8;
				if (flag9)
				{
					lastWin32Error3 = Marshal.GetLastWin32Error();
					bool flag10 = lastWin32Error3 != 122;
					if (flag10)
					{
						goto Block_10;
					}
				}
				byte[] array = new byte[num4];
				fixed (byte* ptr2 = array)
				{
					flag8 = NativeMethods.SetupDiGetDeviceRegistryProperty(intPtr, ref deviceInformationData, DeviceRegistryProperties.SPDRP_FRIENDLYNAME, out num3, ptr2, num4, out num4);
					bool flag11 = !flag8;
					if (flag11)
					{
						goto Block_12;
					}
				}
				string text2 = Encoding.Unicode.GetString(array);
				text2 = text2.Remove(text2.Length - 1);
				flag8 = NativeMethods.SetupDiGetDeviceInstanceId(intPtr, ref deviceInformationData, IntPtr.Zero, 0U, out num4);
				bool flag12 = !flag8;
				if (flag12)
				{
					lastWin32Error4 = Marshal.GetLastWin32Error();
					bool flag13 = lastWin32Error4 != 122;
					if (flag13)
					{
						goto Block_14;
					}
				}
				array = new byte[num4 * 2U];
				fixed (byte* ptr3 = array)
				{
					flag8 = NativeMethods.SetupDiGetDeviceInstanceId(intPtr, ref deviceInformationData, (IntPtr)((void*)ptr3), num4, out num4);
					bool flag14 = !flag8;
					if (flag14)
					{
						goto Block_16;
					}
				}
				string text3 = Encoding.Unicode.GetString(array);
				text3 = text3.Remove(text3.Length - 2);
				UsbDevice value = new UsbDevice(text3, text2, text);
				string key = text.ToUpper(CultureInfo.InvariantCulture);
				this.Devices.Add(key, value);
				num++;
			}
			int lastWin32Error5 = Marshal.GetLastWin32Error();
			bool flag15 = lastWin32Error5 == 259;
			if (flag15)
			{
				return;
			}
			throw new Win32Exception(lastWin32Error5);
			Block_5:
			throw new Win32Exception(lastWin32Error2);
			Block_7:
			int lastWin32Error6 = Marshal.GetLastWin32Error();
			throw new Win32Exception(lastWin32Error6);
			Block_8:
			throw new Exception("Device interface symbolic name is null.");
			Block_10:
			throw new Win32Exception(lastWin32Error3);
			Block_12:
			int lastWin32Error7 = Marshal.GetLastWin32Error();
			throw new Win32Exception(lastWin32Error7);
			Block_14:
			throw new Win32Exception(lastWin32Error4);
			Block_16:
			int lastWin32Error8 = Marshal.GetLastWin32Error();
			throw new Win32Exception(lastWin32Error8);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000085BC File Offset: 0x000067BC
		public unsafe static UsbDevices.UninstallStatus UninstallDevices(out List<UsbDevice> uninstalledDevices)
		{
			UsbDevices.UninstallStatus result = UsbDevices.UninstallStatus.StatusOk;
			IntPtr intPtr = NativeMethods.SetupDiGetClassDevs(ref UsbDevices.UsbIfGuid, null, 0, 16);
			bool flag = IntPtr.Zero == intPtr;
			if (flag)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new Win32Exception(lastWin32Error);
			}
			int num = 0;
			uninstalledDevices = new List<UsbDevice>();
			int lastWin32Error2;
			int lastWin32Error3;
			int lastWin32Error4;
			for (;;)
			{
				int num2 = 0;
				DeviceInterfaceData deviceInterfaceData = new DeviceInterfaceData
				{
					Size = Marshal.SizeOf(typeof(DeviceInterfaceData))
				};
				bool flag2 = !NativeMethods.SetupDiEnumDeviceInterfaces(intPtr, 0, ref UsbDevices.UsbIfGuid, num, ref deviceInterfaceData);
				if (flag2)
				{
					break;
				}
				bool flag3 = !NativeMethods.SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, IntPtr.Zero, 0, ref num2, IntPtr.Zero);
				if (flag3)
				{
					lastWin32Error2 = Marshal.GetLastWin32Error();
					bool flag4 = lastWin32Error2 != 122;
					if (flag4)
					{
						goto Block_5;
					}
				}
				DeviceInterfaceDetailData* ptr = (DeviceInterfaceDetailData*)((void*)Marshal.AllocHGlobal(num2));
				bool flag5 = IntPtr.Size == 4;
				if (flag5)
				{
					ptr->Size = 6;
				}
				else
				{
					ptr->Size = 8;
				}
				DeviceInformationData deviceInformationData = new DeviceInformationData
				{
					Size = Marshal.SizeOf(typeof(DeviceInformationData))
				};
				bool flag6 = !NativeMethods.SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, ptr, num2, ref num2, ref deviceInformationData);
				if (flag6)
				{
					goto Block_7;
				}
				string text = Marshal.PtrToStringAuto(new IntPtr((void*)(&ptr->DevicePath)));
				bool flag7 = text == null;
				if (flag7)
				{
					goto Block_8;
				}
				Marshal.FreeHGlobal((IntPtr)((void*)ptr));
				uint num3;
				uint num4;
				bool flag8 = NativeMethods.SetupDiGetDeviceRegistryProperty(intPtr, ref deviceInformationData, DeviceRegistryProperties.SPDRP_FRIENDLYNAME, out num3, (byte*)((void*)IntPtr.Zero), 0U, out num4);
				bool flag9 = !flag8;
				if (flag9)
				{
					lastWin32Error3 = Marshal.GetLastWin32Error();
					bool flag10 = lastWin32Error3 != 122;
					if (flag10)
					{
						goto Block_10;
					}
				}
				byte[] array = new byte[num4];
				fixed (byte* ptr2 = array)
				{
					flag8 = NativeMethods.SetupDiGetDeviceRegistryProperty(intPtr, ref deviceInformationData, DeviceRegistryProperties.SPDRP_FRIENDLYNAME, out num3, ptr2, num4, out num4);
					bool flag11 = !flag8;
					if (flag11)
					{
						goto Block_12;
					}
				}
				string text2 = Encoding.Unicode.GetString(array);
				text2 = text2.Remove(text2.Length - 1);
				flag8 = NativeMethods.SetupDiGetDeviceInstanceId(intPtr, ref deviceInformationData, IntPtr.Zero, 0U, out num4);
				bool flag12 = !flag8;
				if (flag12)
				{
					lastWin32Error4 = Marshal.GetLastWin32Error();
					bool flag13 = lastWin32Error4 != 122;
					if (flag13)
					{
						goto Block_14;
					}
				}
				array = new byte[num4 * 2U];
				fixed (byte* ptr3 = array)
				{
					flag8 = NativeMethods.SetupDiGetDeviceInstanceId(intPtr, ref deviceInformationData, (IntPtr)((void*)ptr3), num4, out num4);
					bool flag14 = !flag8;
					if (flag14)
					{
						goto Block_16;
					}
				}
				string text3 = Encoding.Unicode.GetString(array);
				text3 = text3.Remove(text3.Length - 2);
				bool flag15;
				flag8 = NativeMethods.DiUninstallDevice(IntPtr.Zero, intPtr, ref deviceInformationData, 0U, out flag15);
				bool flag16 = flag15;
				if (flag16)
				{
					result = UsbDevices.UninstallStatus.RebootNeeded;
				}
				bool flag17 = !flag8;
				if (flag17)
				{
					uint lastWin32Error5 = (uint)Marshal.GetLastWin32Error();
					bool flag18 = lastWin32Error5 == 5U;
					if (flag18)
					{
						goto Block_19;
					}
					bool flag19 = lastWin32Error5 == 3758096949U;
					if (flag19)
					{
						goto Block_20;
					}
				}
				UsbDevice item = new UsbDevice(text3, text2, text);
				uninstalledDevices.Add(item);
				num++;
			}
			int lastWin32Error6 = Marshal.GetLastWin32Error();
			bool flag20 = lastWin32Error6 == 259;
			if (flag20)
			{
				return result;
			}
			throw new Win32Exception(lastWin32Error6);
			Block_5:
			throw new Win32Exception(lastWin32Error2);
			Block_7:
			int lastWin32Error7 = Marshal.GetLastWin32Error();
			throw new Win32Exception(lastWin32Error7);
			Block_8:
			throw new Exception("Device interface symbolic name is null.");
			Block_10:
			throw new Win32Exception(lastWin32Error3);
			Block_12:
			int lastWin32Error8 = Marshal.GetLastWin32Error();
			throw new Win32Exception(lastWin32Error8);
			Block_14:
			throw new Win32Exception(lastWin32Error4);
			Block_16:
			int lastWin32Error9 = Marshal.GetLastWin32Error();
			throw new Win32Exception(lastWin32Error9);
			Block_19:
			return UsbDevices.UninstallStatus.AccessDenied;
			Block_20:
			result = UsbDevices.UninstallStatus.ErrorInWow64;
			return result;
		}

		// Token: 0x0400015D RID: 349
		private static readonly string UsbIfGuidString = "{875d47fc-d331-4663-b339-624001a2dc5e}";

		// Token: 0x0400015E RID: 350
		private static Guid UsbIfGuid = new Guid(UsbDevices.UsbIfGuidString);

		// Token: 0x0200005B RID: 91
		public enum UninstallStatus
		{
			// Token: 0x040001E3 RID: 483
			StatusOk,
			// Token: 0x040001E4 RID: 484
			RebootNeeded,
			// Token: 0x040001E5 RID: 485
			AccessDenied,
			// Token: 0x040001E6 RID: 486
			ErrorInWow64
		}
	}
}
