using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Nokia.Lucid.Interop;
using Nokia.Lucid.Interop.SafeHandles;
using Nokia.Lucid.Interop.Win32Types;
using Nokia.Lucid.Primitives;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x0200001C RID: 28
	public sealed class DeviceInfoSet
	{
		// Token: 0x060000DF RID: 223 RVA: 0x000097C5 File Offset: 0x000079C5
		public DeviceInfoSet()
		{
			this.Filter = FilterExpression.DefaultExpression;
			this.DeviceTypeMap = DeviceTypeMap.DefaultMap;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000097E3 File Offset: 0x000079E3
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x000097EB File Offset: 0x000079EB
		public Expression<Func<DeviceIdentifier, bool>> Filter { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000097F4 File Offset: 0x000079F4
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x000097FC File Offset: 0x000079FC
		public DeviceTypeMap DeviceTypeMap { get; set; }

		// Token: 0x060000E4 RID: 228 RVA: 0x00009808 File Offset: 0x00007A08
		public DeviceInfo GetDevice(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.EnsureInitialized();
			DeviceIdentifier arg;
			if (!DeviceIdentifier.TryParse(path, out arg) || !this.compiledFilter(arg))
			{
				string message = string.Format(CultureInfo.CurrentCulture, Resources.InvalidOperationException_MessageFormat_CouldNotRetrieveDeviceInfo, new object[]
				{
					path
				});
				throw new InvalidOperationException(message);
			}
			SP_DEVICE_INTERFACE_DATA interfaceData = new SP_DEVICE_INTERFACE_DATA
			{
				cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA))
			};
			DeviceInfoSet.NativeDeviceInfoSet nativeDeviceInfoSet = new DeviceInfoSet.NativeDeviceInfoSet();
			if (!SetupApiNativeMethods.SetupDiOpenDeviceInterface(nativeDeviceInfoSet.SafeDeviceInfoSetHandle, path, 0, ref interfaceData))
			{
				if (Marshal.GetLastWin32Error() == -536870363)
				{
					string message2 = string.Format(CultureInfo.CurrentCulture, Resources.InvalidOperationException_MessageFormat_CouldNotRetrieveDeviceInfo, new object[]
					{
						path
					});
					throw new InvalidOperationException(message2);
				}
				throw new Win32Exception();
			}
			else
			{
				SP_DEVINFO_DATA sp_DEVINFO_DATA = new SP_DEVINFO_DATA
				{
					cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA))
				};
				if (!SetupApiNativeMethods.SetupDiGetDeviceInterfaceDetail(nativeDeviceInfoSet.SafeDeviceInfoSetHandle, ref interfaceData, IntPtr.Zero, 0, IntPtr.Zero, ref sp_DEVINFO_DATA) && Marshal.GetLastWin32Error() != 122)
				{
					throw new Win32Exception();
				}
				DeviceType mapping = this.cachedTypeMap.GetMapping(interfaceData.InterfaceClassGuid);
				string deviceInstanceId = nativeDeviceInfoSet.GetDeviceInstanceId(ref sp_DEVINFO_DATA);
				return new DeviceInfo(nativeDeviceInfoSet, path, sp_DEVINFO_DATA.DevInst, deviceInstanceId, sp_DEVINFO_DATA.ClassGuid, mapping, interfaceData, sp_DEVINFO_DATA.Reserved);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00009CB4 File Offset: 0x00007EB4
		public IEnumerable<DeviceInfo> EnumerateDevices()
		{
			this.EnsureInitialized();
			DeviceInfoSet.NativeDeviceInfoSet deviceInfoSet = new DeviceInfoSet.NativeDeviceInfoSet();
			foreach (Guid interfaceClass in this.cachedTypeMap.InterfaceClasses)
			{
				deviceInfoSet.AddDeviceInterfaceClass(interfaceClass);
				foreach (SP_DEVICE_INTERFACE_DATA interfaceData in deviceInfoSet.EnumerateDeviceInterfaces(interfaceClass))
				{
					SP_DEVICE_INTERFACE_DATA temp = interfaceData;
					SP_DEVINFO_DATA deviceData;
					string devicePath = deviceInfoSet.GetDevicePath(ref temp, out deviceData);
					DeviceIdentifier p;
					if (DeviceIdentifier.TryParse(devicePath, out p) && this.compiledFilter(p))
					{
						string deviceInstanceId = deviceInfoSet.GetDeviceInstanceId(ref deviceData);
						DeviceType deviceType = this.cachedTypeMap.GetMapping(interfaceClass);
						DeviceInfo device = new DeviceInfo(deviceInfoSet, devicePath, deviceData.DevInst, deviceInstanceId, deviceData.ClassGuid, deviceType, interfaceData, deviceData.Reserved);
						yield return device;
					}
				}
			}
			yield break;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00009CD9 File Offset: 0x00007ED9
		public IEnumerable<DeviceInfo> EnumeratePresentDevices()
		{
			return from d in this.EnumerateDevices()
			where d.IsPresent
			select d;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00009D03 File Offset: 0x00007F03
		private void EnsureInitialized()
		{
			if (this.initialized)
			{
				return;
			}
			this.cachedTypeMap = this.DeviceTypeMap;
			this.compiledFilter = (this.Filter ?? FilterExpression.EmptyExpression).Compile();
			this.initialized = true;
		}

		// Token: 0x04000078 RID: 120
		private volatile bool initialized;

		// Token: 0x04000079 RID: 121
		private Func<DeviceIdentifier, bool> compiledFilter;

		// Token: 0x0400007A RID: 122
		private DeviceTypeMap cachedTypeMap;

		// Token: 0x0200001D RID: 29
		private sealed class NativeDeviceInfoSet : IDisposable, INativeDeviceInfoSet
		{
			// Token: 0x060000E9 RID: 233 RVA: 0x00009D40 File Offset: 0x00007F40
			public NativeDeviceInfoSet()
			{
				this.handle = SetupApiNativeMethods.SetupDiCreateDeviceInfoListEx(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
				if (this.handle == null || this.handle.IsInvalid)
				{
					throw new Win32Exception();
				}
			}

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000EA RID: 234 RVA: 0x00009D8D File Offset: 0x00007F8D
			public SafeDeviceInfoSetHandle SafeDeviceInfoSetHandle
			{
				get
				{
					return this.handle;
				}
			}

			// Token: 0x060000EB RID: 235 RVA: 0x00009D95 File Offset: 0x00007F95
			public void Dispose()
			{
				this.handle.Dispose();
			}

			// Token: 0x0400007E RID: 126
			private readonly SafeDeviceInfoSetHandle handle;
		}
	}
}
