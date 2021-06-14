using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Nokia.Lucid.Interop.Win32Types;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x0200001B RID: 27
	public sealed class DeviceInfo : IDevicePropertySet
	{
		// Token: 0x060000CC RID: 204 RVA: 0x000078F8 File Offset: 0x00005AF8
		internal DeviceInfo(INativeDeviceInfoSet deviceInfoSet, string devicePath, int instanceHandle, string instanceId, Guid setupClass, DeviceType deviceType, SP_DEVICE_INTERFACE_DATA interfaceData, IntPtr reserved)
		{
			this.deviceInfoSet = deviceInfoSet;
			this.path = devicePath;
			this.instanceHandle = instanceHandle;
			this.instanceId = instanceId;
			this.setupClass = setupClass;
			this.deviceType = deviceType;
			this.status = (DeviceStatus)interfaceData.Flags;
			this.DeviceInterfaceGuid = interfaceData.InterfaceClassGuid;
			this.reserved = reserved;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000795A File Offset: 0x00005B5A
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00007962 File Offset: 0x00005B62
		public Guid DeviceInterfaceGuid { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000796B File Offset: 0x00005B6B
		public string InstanceId
		{
			get
			{
				return this.instanceId;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00007973 File Offset: 0x00005B73
		public string Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000797B File Offset: 0x00005B7B
		public DeviceStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00007983 File Offset: 0x00005B83
		public bool IsPresent
		{
			get
			{
				return (this.status & DeviceStatus.Present) == DeviceStatus.Present;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00007990 File Offset: 0x00005B90
		public bool IsDefault
		{
			get
			{
				return (this.status & DeviceStatus.Default) == DeviceStatus.Default;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000799D File Offset: 0x00005B9D
		public bool IsRemoved
		{
			get
			{
				return (this.status & DeviceStatus.Removed) == DeviceStatus.Removed;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000079AA File Offset: 0x00005BAA
		public DeviceType DeviceType
		{
			get
			{
				return this.deviceType;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000079B4 File Offset: 0x00005BB4
		IEnumerable<PropertyKey> IDevicePropertySet.EnumeratePropertyKeys()
		{
			SP_DEVINFO_DATA sp_DEVINFO_DATA = new SP_DEVINFO_DATA
			{
				cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA)),
				ClassGuid = this.setupClass,
				DevInst = this.instanceHandle,
				Reserved = this.reserved
			};
			return this.deviceInfoSet.GetDevicePropertyKeys(ref sp_DEVINFO_DATA);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007A18 File Offset: 0x00005C18
		object IDevicePropertySet.ReadProperty(PropertyKey key, IPropertyValueFormatter formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			SP_DEVINFO_DATA sp_DEVINFO_DATA = new SP_DEVINFO_DATA
			{
				cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA)),
				ClassGuid = this.setupClass,
				DevInst = this.instanceHandle,
				Reserved = this.reserved
			};
			int propertyType;
			byte[] deviceProperty;
			try
			{
				deviceProperty = this.deviceInfoSet.GetDeviceProperty(ref sp_DEVINFO_DATA, ref key, out propertyType);
			}
			catch (Win32Exception ex)
			{
				if (ex.NativeErrorCode == 1168)
				{
					string str;
					string text = DeviceInfo.TryGetPropertyKeyIdentifier(key, out str) ? (" (" + str + ")") : string.Empty;
					string message = string.Format(CultureInfo.CurrentCulture, Resources.KeyNotFoundException_MessageFormat_PropertyNotFound, new object[]
					{
						key,
						text
					});
					throw new KeyNotFoundException(message, ex);
				}
				throw;
			}
			return formatter.ReadFrom(deviceProperty, 0, deviceProperty.Length, (PropertyType)propertyType);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007B18 File Offset: 0x00005D18
		bool IDevicePropertySet.TryReadProperty(PropertyKey key, IPropertyValueFormatter formatter, out object value)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			SP_DEVINFO_DATA sp_DEVINFO_DATA = new SP_DEVINFO_DATA
			{
				cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA)),
				ClassGuid = this.setupClass,
				DevInst = this.instanceHandle,
				Reserved = this.reserved
			};
			int propertyType;
			byte[] array;
			if (!this.deviceInfoSet.TryGetDeviceProperty(ref sp_DEVINFO_DATA, ref key, out propertyType, out array))
			{
				value = null;
				return false;
			}
			return formatter.TryReadFrom(array, 0, array.Length, (PropertyType)propertyType, out value);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007BA4 File Offset: 0x00005DA4
		public void RefreshStatus()
		{
			this.status = (DeviceStatus)this.deviceInfoSet.GetDeviceInterface(this.path).Flags;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007BD0 File Offset: 0x00005DD0
		public DeviceStatus ReadStatus()
		{
			this.RefreshStatus();
			return this.status;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00007BDE File Offset: 0x00005DDE
		public bool ReadIsPresent()
		{
			return (this.ReadStatus() & DeviceStatus.Present) == DeviceStatus.Present;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007BEB File Offset: 0x00005DEB
		public bool ReadIsDefault()
		{
			return (this.ReadStatus() & DeviceStatus.Default) == DeviceStatus.Default;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00007BF8 File Offset: 0x00005DF8
		public bool ReadIsRemoved()
		{
			return (this.ReadStatus() & DeviceStatus.Removed) == DeviceStatus.Removed;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00007C08 File Offset: 0x00005E08
		private static bool TryGetPropertyKeyIdentifier(PropertyKey key, out string identifier)
		{
			Dictionary<PropertyKey, string> dictionary = new Dictionary<PropertyKey, string>
			{
				{
					new PropertyKey(3072717104U, 18415, 4122, 165, 241, 2, 96, 140, 158, 235, 172, 10),
					"DEVPKEY_NAME"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 2),
					"DEVPKEY_Device_DeviceDesc"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 3),
					"DEVPKEY_Device_HardwareIds"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 4),
					"DEVPKEY_Device_CompatibleIds"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 6),
					"DEVPKEY_Device_Service"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 9),
					"DEVPKEY_Device_Class"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 10),
					"DEVPKEY_Device_ClassGuid"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 11),
					"DEVPKEY_Device_Driver"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 12),
					"DEVPKEY_Device_ConfigFlags"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 13),
					"DEVPKEY_Device_Manufacturer"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 14),
					"DEVPKEY_Device_FriendlyName"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 15),
					"DEVPKEY_Device_LocationInfo"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 16),
					"DEVPKEY_Device_PDOName"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 17),
					"DEVPKEY_Device_Capabilities"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 18),
					"DEVPKEY_Device_UINumber"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 19),
					"DEVPKEY_Device_UpperFilters"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 20),
					"DEVPKEY_Device_LowerFilters"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 21),
					"DEVPKEY_Device_BusTypeGuid"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 22),
					"DEVPKEY_Device_LegacyBusType"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 23),
					"DEVPKEY_Device_BusNumber"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 24),
					"DEVPKEY_Device_EnumeratorName"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 25),
					"DEVPKEY_Device_Security"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 26),
					"DEVPKEY_Device_SecuritySDS"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 27),
					"DEVPKEY_Device_DevType"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 28),
					"DEVPKEY_Device_Exclusive"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 29),
					"DEVPKEY_Device_Characteristics"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 30),
					"DEVPKEY_Device_Address"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 31),
					"DEVPKEY_Device_UINumberDescFormat"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 32),
					"DEVPKEY_Device_PowerData"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 33),
					"DEVPKEY_Device_RemovalPolicy"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 34),
					"DEVPKEY_Device_RemovalPolicyDefault"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 35),
					"DEVPKEY_Device_RemovalPolicyOverride"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 36),
					"DEVPKEY_Device_InstallState"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 37),
					"DEVPKEY_Device_LocationPaths"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 38),
					"DEVPKEY_Device_BaseContainerId"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 2),
					"DEVPKEY_Device_DevNodeStatus"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 3),
					"DEVPKEY_Device_ProblemCode"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 4),
					"DEVPKEY_Device_EjectionRelations"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 5),
					"DEVPKEY_Device_RemovalRelations"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 6),
					"DEVPKEY_Device_PowerRelations"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 7),
					"DEVPKEY_Device_BusRelations"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 8),
					"DEVPKEY_Device_Parent"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 9),
					"DEVPKEY_Device_Children"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 10),
					"DEVPKEY_Device_Siblings"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 11),
					"DEVPKEY_Device_TransportRelations"
				},
				{
					new PropertyKey(2152296704U, 35955, 18617, 170, 217, 206, 56, 126, 25, 197, 110, 2),
					"DEVPKEY_Device_Reported"
				},
				{
					new PropertyKey(2152296704U, 35955, 18617, 170, 217, 206, 56, 126, 25, 197, 110, 3),
					"DEVPKEY_Device_Legacy"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 256),
					"DEVPKEY_Device_InstanceId"
				},
				{
					new PropertyKey(2357121542U, 16266, 18471, 179, 171, 174, 158, 31, 174, 252, 108, 2),
					"DEVPKEY_Device_ContainerId"
				},
				{
					new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 2),
					"DEVPKEY_Device_ModelId"
				},
				{
					new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 3),
					"DEVPKEY_Device_FriendlyNameAttributes"
				},
				{
					new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 4),
					"DEVPKEY_Device_ManufacturerAttributes"
				},
				{
					new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 5),
					"DEVPKEY_Device_PresenceNotForDevice"
				},
				{
					new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 1),
					"DEVPKEY_Numa_Proximity_Domain"
				},
				{
					new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 2),
					"DEVPKEY_Device_DHP_Rebalance_Policy"
				},
				{
					new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 3),
					"DEVPKEY_Device_Numa_Node"
				},
				{
					new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 4),
					"DEVPKEY_Device_BusReportedDeviceDesc"
				},
				{
					new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 6),
					"DEVPKEY_Device_SessionId"
				},
				{
					new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 100),
					"DEVPKEY_Device_InstallDate"
				},
				{
					new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 101),
					"DEVPKEY_Device_FirstInstallDate"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 2),
					"DEVPKEY_Device_DriverDate"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 3),
					"DEVPKEY_Device_DriverVersion"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 4),
					"DEVPKEY_Device_DriverDesc"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 5),
					"DEVPKEY_Device_DriverInfPath"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 6),
					"DEVPKEY_Device_DriverInfSection"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 7),
					"DEVPKEY_Device_DriverInfSectionExt"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 8),
					"DEVPKEY_Device_MatchingDeviceId"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 9),
					"DEVPKEY_Device_DriverProvider"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 10),
					"DEVPKEY_Device_DriverPropPageProvider"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 11),
					"DEVPKEY_Device_DriverCoInstallers"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 12),
					"DEVPKEY_Device_ResourcePickerTags"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 13),
					"DEVPKEY_Device_ResourcePickerExceptions"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 14),
					"DEVPKEY_Device_DriverRank"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 15),
					"DEVPKEY_Device_DriverLogoLevel"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 17),
					"DEVPKEY_Device_NoConnectSound"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 18),
					"DEVPKEY_Device_GenericDriverInstalled"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 19),
					"DEVPKEY_Device_AdditionalSoftwareRequested"
				},
				{
					new PropertyKey(2950264384U, 34467, 16912, 182, 124, 40, 156, 65, 170, 190, 85, 2),
					"DEVPKEY_Device_SafeRemovalRequired"
				},
				{
					new PropertyKey(2950264384U, 34467, 16912, 182, 124, 40, 156, 65, 170, 190, 85, 3),
					"DEVPKEY_Device_SafeRemovalRequiredOverride"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 2),
					"DEVPKEY_DrvPkg_Model"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 3),
					"DEVPKEY_DrvPkg_VendorWebSite"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 4),
					"DEVPKEY_DrvPkg_DetailedDescription"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 5),
					"DEVPKEY_DrvPkg_DocumentationLink"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 6),
					"DEVPKEY_DrvPkg_Icon"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 7),
					"DEVPKEY_DrvPkg_BrandingIcon"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 19),
					"DEVPKEY_DeviceClass_UpperFilters"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 20),
					"DEVPKEY_DeviceClass_LowerFilters"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 25),
					"DEVPKEY_DeviceClass_Security"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 26),
					"DEVPKEY_DeviceClass_SecuritySDS"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 27),
					"DEVPKEY_DeviceClass_DevType"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 28),
					"DEVPKEY_DeviceClass_Exclusive"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 29),
					"DEVPKEY_DeviceClass_Characteristics"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 2),
					"DEVPKEY_DeviceClass_Name"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 3),
					"DEVPKEY_DeviceClass_ClassName"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 4),
					"DEVPKEY_DeviceClass_Icon"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 5),
					"DEVPKEY_DeviceClass_ClassInstaller"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 6),
					"DEVPKEY_DeviceClass_PropPageProvider"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 7),
					"DEVPKEY_DeviceClass_NoInstallClass"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 8),
					"DEVPKEY_DeviceClass_NoDisplayClass"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 9),
					"DEVPKEY_DeviceClass_SilentInstall"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 10),
					"DEVPKEY_DeviceClass_NoUseClass"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 11),
					"DEVPKEY_DeviceClass_DefaultService"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 12),
					"DEVPKEY_DeviceClass_IconPath"
				},
				{
					new PropertyKey(3511500531U, 26319, 19362, 157, 56, 13, 219, 55, 171, 71, 1, 2),
					"DEVPKEY_DeviceClass_DHPRebalanceOptOut"
				},
				{
					new PropertyKey(1899828995U, 41698, 18933, 146, 20, 86, 71, 46, 243, 218, 92, 2),
					"DEVPKEY_DeviceClass_ClassCoInstallers"
				},
				{
					new PropertyKey(40784238U, 47124, 16715, 131, 205, 133, 109, 111, 239, 72, 34, 2),
					"DEVPKEY_DeviceInterface_FriendlyName"
				},
				{
					new PropertyKey(40784238U, 47124, 16715, 131, 205, 133, 109, 111, 239, 72, 34, 3),
					"DEVPKEY_DeviceInterface_Enabled"
				},
				{
					new PropertyKey(40784238U, 47124, 16715, 131, 205, 133, 109, 111, 239, 72, 34, 4),
					"DEVPKEY_DeviceInterface_ClassGuid"
				},
				{
					new PropertyKey(348666521, 2879, 17591, 190, 76, 161, 120, 211, 153, 5, 100, 2),
					"DEVPKEY_DeviceInterfaceClass_DefaultInterface"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 68),
					"DEVPKEY_DeviceDisplay_IsShowInDisconnectedState"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 74),
					"DEVPKEY_DeviceDisplay_IsNotInterestingForDisplay"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 90),
					"DEVPKEY_DeviceDisplay_Category"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 98),
					"DEVPKEY_DeviceDisplay_UnpairUninstall"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 99),
					"DEVPKEY_DeviceDisplay_RequiresUninstallElevation"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 101),
					"DEVPKEY_DeviceDisplay_AlwaysShowDeviceAsConnected"
				}
			};
			return dictionary.TryGetValue(key, out identifier);
		}

		// Token: 0x0400006F RID: 111
		private readonly INativeDeviceInfoSet deviceInfoSet;

		// Token: 0x04000070 RID: 112
		private readonly string path;

		// Token: 0x04000071 RID: 113
		private readonly string instanceId;

		// Token: 0x04000072 RID: 114
		private readonly int instanceHandle;

		// Token: 0x04000073 RID: 115
		private readonly Guid setupClass;

		// Token: 0x04000074 RID: 116
		private readonly DeviceType deviceType;

		// Token: 0x04000075 RID: 117
		private readonly IntPtr reserved;

		// Token: 0x04000076 RID: 118
		private DeviceStatus status;
	}
}
