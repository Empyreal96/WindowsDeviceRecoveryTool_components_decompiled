using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public struct PropertyNameMap : IEquatable<PropertyNameMap>
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00005704 File Offset: 0x00003904
		public PropertyNameMap(PropertyKey propertyKey, string name)
		{
			this.mappings = new Dictionary<PropertyKey, string>
			{
				{
					propertyKey,
					name
				}
			};
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005728 File Offset: 0x00003928
		public PropertyNameMap(IEnumerable<KeyValuePair<PropertyKey, string>> mappings)
		{
			if (mappings == null)
			{
				this.mappings = null;
				return;
			}
			this.mappings = new Dictionary<PropertyKey, string>();
			foreach (KeyValuePair<PropertyKey, string> keyValuePair in mappings)
			{
				this.mappings.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005798 File Offset: 0x00003998
		private PropertyNameMap(IEnumerable<KeyValuePair<PropertyKey, string>> mappings, PropertyKey propertyKey, string name)
		{
			this = new PropertyNameMap(mappings);
			if (this.mappings == null)
			{
				this.mappings = new Dictionary<PropertyKey, string>(1);
			}
			this.mappings[propertyKey] = name;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000057C2 File Offset: 0x000039C2
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000057C9 File Offset: 0x000039C9
		public static PropertyNameMap DefaultMap
		{
			get
			{
				return PropertyNameMap.defaultMap;
			}
			set
			{
				PropertyNameMap.defaultMap = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000057D1 File Offset: 0x000039D1
		public bool IsEmpty
		{
			get
			{
				return this.mappings == null || this.mappings.Count == 0;
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000057EC File Offset: 0x000039EC
		public static PropertyNameMap CreateDefaultMap()
		{
			return new PropertyNameMap(new Dictionary<PropertyKey, string>
			{
				{
					new PropertyKey(3072717104U, 18415, 4122, 165, 241, 2, 96, 140, 158, 235, 172, 10),
					"NAME"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 2),
					"Device_DeviceDesc"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 3),
					"Device_HardwareIds"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 4),
					"Device_CompatibleIds"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 6),
					"Device_Service"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 9),
					"Device_Class"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 10),
					"Device_ClassGuid"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 11),
					"Device_Driver"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 12),
					"Device_ConfigFlags"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 13),
					"Device_Manufacturer"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 14),
					"Device_FriendlyName"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 15),
					"Device_LocationInfo"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 16),
					"Device_PDOName"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 17),
					"Device_Capabilities"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 18),
					"Device_UINumber"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 19),
					"Device_UpperFilters"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 20),
					"Device_LowerFilters"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 21),
					"Device_BusTypeGuid"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 22),
					"Device_LegacyBusType"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 23),
					"Device_BusNumber"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 24),
					"Device_EnumeratorName"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 25),
					"Device_Security"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 26),
					"Device_SecuritySDS"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 27),
					"Device_DevType"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 28),
					"Device_Exclusive"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 29),
					"Device_Characteristics"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 30),
					"Device_Address"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 31),
					"Device_UINumberDescFormat"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 32),
					"Device_PowerData"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 33),
					"Device_RemovalPolicy"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 34),
					"Device_RemovalPolicyDefault"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 35),
					"Device_RemovalPolicyOverride"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 36),
					"Device_InstallState"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 37),
					"Device_LocationPaths"
				},
				{
					new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 38),
					"Device_BaseContainerId"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 2),
					"Device_DevNodeStatus"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 3),
					"Device_ProblemCode"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 4),
					"Device_EjectionRelations"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 5),
					"Device_RemovalRelations"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 6),
					"Device_PowerRelations"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 7),
					"Device_BusRelations"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 8),
					"Device_Parent"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 9),
					"Device_Children"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 10),
					"Device_Siblings"
				},
				{
					new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 11),
					"Device_TransportRelations"
				},
				{
					new PropertyKey(2152296704U, 35955, 18617, 170, 217, 206, 56, 126, 25, 197, 110, 2),
					"Device_Reported"
				},
				{
					new PropertyKey(2152296704U, 35955, 18617, 170, 217, 206, 56, 126, 25, 197, 110, 3),
					"Device_Legacy"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 256),
					"Device_InstanceId"
				},
				{
					new PropertyKey(2357121542U, 16266, 18471, 179, 171, 174, 158, 31, 174, 252, 108, 2),
					"Device_ContainerId"
				},
				{
					new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 2),
					"Device_ModelId"
				},
				{
					new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 3),
					"Device_FriendlyNameAttributes"
				},
				{
					new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 4),
					"Device_ManufacturerAttributes"
				},
				{
					new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 5),
					"Device_PresenceNotForDevice"
				},
				{
					new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 1),
					"Numa_Proximity_Domain"
				},
				{
					new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 2),
					"Device_DHP_Rebalance_Policy"
				},
				{
					new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 3),
					"Device_Numa_Node"
				},
				{
					new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 4),
					"Device_BusReportedDeviceDesc"
				},
				{
					new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 6),
					"Device_SessionId"
				},
				{
					new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 100),
					"Device_InstallDate"
				},
				{
					new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 101),
					"Device_FirstInstallDate"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 2),
					"Device_DriverDate"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 3),
					"Device_DriverVersion"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 4),
					"Device_DriverDesc"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 5),
					"Device_DriverInfPath"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 6),
					"Device_DriverInfSection"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 7),
					"Device_DriverInfSectionExt"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 8),
					"Device_MatchingDeviceId"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 9),
					"Device_DriverProvider"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 10),
					"Device_DriverPropPageProvider"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 11),
					"Device_DriverCoInstallers"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 12),
					"Device_ResourcePickerTags"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 13),
					"Device_ResourcePickerExceptions"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 14),
					"Device_DriverRank"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 15),
					"Device_DriverLogoLevel"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 17),
					"Device_NoConnectSound"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 18),
					"Device_GenericDriverInstalled"
				},
				{
					new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 19),
					"Device_AdditionalSoftwareRequested"
				},
				{
					new PropertyKey(2950264384U, 34467, 16912, 182, 124, 40, 156, 65, 170, 190, 85, 2),
					"Device_SafeRemovalRequired"
				},
				{
					new PropertyKey(2950264384U, 34467, 16912, 182, 124, 40, 156, 65, 170, 190, 85, 3),
					"Device_SafeRemovalRequiredOverride"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 2),
					"DrvPkg_Model"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 3),
					"DrvPkg_VendorWebSite"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 4),
					"DrvPkg_DetailedDescription"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 5),
					"DrvPkg_DocumentationLink"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 6),
					"DrvPkg_Icon"
				},
				{
					new PropertyKey(3480468305U, 15039, 17570, 133, 224, 154, 61, 199, 161, 33, 50, 7),
					"DrvPkg_BrandingIcon"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 19),
					"DeviceClass_UpperFilters"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 20),
					"DeviceClass_LowerFilters"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 25),
					"DeviceClass_Security"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 26),
					"DeviceClass_SecuritySDS"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 27),
					"DeviceClass_DevType"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 28),
					"DeviceClass_Exclusive"
				},
				{
					new PropertyKey(1126273419U, 63134, 18189, 165, 222, 77, 136, 199, 90, 210, 75, 29),
					"DeviceClass_Characteristics"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 2),
					"DeviceClass_Name"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 3),
					"DeviceClass_ClassName"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 4),
					"DeviceClass_Icon"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 5),
					"DeviceClass_ClassInstaller"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 6),
					"DeviceClass_PropPageProvider"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 7),
					"DeviceClass_NoInstallClass"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 8),
					"DeviceClass_NoDisplayClass"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 9),
					"DeviceClass_SilentInstall"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 10),
					"DeviceClass_NoUseClass"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 11),
					"DeviceClass_DefaultService"
				},
				{
					new PropertyKey(630898684, 20647, 18382, 175, 8, 104, 201, 167, 215, 51, 102, 12),
					"DeviceClass_IconPath"
				},
				{
					new PropertyKey(3511500531U, 26319, 19362, 157, 56, 13, 219, 55, 171, 71, 1, 2),
					"DeviceClass_DHPRebalanceOptOut"
				},
				{
					new PropertyKey(1899828995U, 41698, 18933, 146, 20, 86, 71, 46, 243, 218, 92, 2),
					"DeviceClass_ClassCoInstallers"
				},
				{
					new PropertyKey(40784238U, 47124, 16715, 131, 205, 133, 109, 111, 239, 72, 34, 2),
					"DeviceInterface_FriendlyName"
				},
				{
					new PropertyKey(40784238U, 47124, 16715, 131, 205, 133, 109, 111, 239, 72, 34, 3),
					"DeviceInterface_Enabled"
				},
				{
					new PropertyKey(40784238U, 47124, 16715, 131, 205, 133, 109, 111, 239, 72, 34, 4),
					"DeviceInterface_ClassGuid"
				},
				{
					new PropertyKey(348666521, 2879, 17591, 190, 76, 161, 120, 211, 153, 5, 100, 2),
					"DeviceInterfaceClass_DefaultInterface"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 68),
					"DeviceDisplay_IsShowInDisconnectedState"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 74),
					"DeviceDisplay_IsNotInterestingForDisplay"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 90),
					"DeviceDisplay_Category"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 98),
					"DeviceDisplay_UnpairUninstall"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 99),
					"DeviceDisplay_RequiresUninstallElevation"
				},
				{
					new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 101),
					"DeviceDisplay_AlwaysShowDeviceAsConnected"
				}
			});
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000073A5 File Offset: 0x000055A5
		public static bool operator !=(PropertyNameMap left, PropertyNameMap right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000073BB File Offset: 0x000055BB
		public static bool operator ==(PropertyNameMap left, PropertyNameMap right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000073CE File Offset: 0x000055CE
		public PropertyNameMap SetMapping(PropertyKey propertyKey, string name)
		{
			return new PropertyNameMap(this.mappings, propertyKey, name);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000073DD File Offset: 0x000055DD
		public PropertyNameMap SetMappings(IEnumerable<KeyValuePair<PropertyKey, string>> range)
		{
			if (range == null)
			{
				return this;
			}
			if (this.mappings == null)
			{
				return new PropertyNameMap(range);
			}
			return new PropertyNameMap(this.mappings.Union(range));
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00007428 File Offset: 0x00005628
		public PropertyNameMap ClearMapping(PropertyKey propertyKey)
		{
			return new PropertyNameMap(from m in this.mappings
			where m.Key != propertyKey
			select m);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000745E File Offset: 0x0000565E
		public PropertyNameMap ClearMappings(params PropertyKey[] range)
		{
			return this.ClearMappings((IEnumerable<PropertyKey>)range);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000747C File Offset: 0x0000567C
		public PropertyNameMap ClearMappings(IEnumerable<PropertyKey> range)
		{
			if (range == null || this.mappings == null)
			{
				return this;
			}
			return new PropertyNameMap(this.mappings.Except(this.mappings.Join(range, (KeyValuePair<PropertyKey, string> m) => m.Key, (PropertyKey m) => m, (KeyValuePair<PropertyKey, string> m, PropertyKey g) => m)));
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000750E File Offset: 0x0000570E
		public bool TryGetMapping(PropertyKey propertyKey, out string name)
		{
			return this.mappings.TryGetValue(propertyKey, out name);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00007520 File Offset: 0x00005720
		public string GetMapping(PropertyKey propertyKey)
		{
			string result;
			if (!this.mappings.TryGetValue(propertyKey, out result))
			{
				string message = string.Format(CultureInfo.CurrentCulture, Resources.KeyNotFoundException_MessageFormat_PropertyKeyMappingNotFound, new object[]
				{
					propertyKey.Category,
					propertyKey.PropertyId
				});
				throw new KeyNotFoundException(message);
			}
			return result;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000757B File Offset: 0x0000577B
		public override bool Equals(object obj)
		{
			return obj is PropertyNameMap && this.Equals((PropertyNameMap)obj);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000075A8 File Offset: 0x000057A8
		public bool Equals(PropertyNameMap other)
		{
			if (this.IsEmpty)
			{
				return other.IsEmpty;
			}
			if (other.IsEmpty)
			{
				return false;
			}
			if (this.mappings.Count != other.mappings.Count)
			{
				return false;
			}
			return (from m in this.mappings
			orderby m.Key
			select m).SequenceEqual(from m in other.mappings
			orderby m.Key
			select m);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000764A File Offset: 0x0000584A
		public override int GetHashCode()
		{
			if (this.mappings == null)
			{
				return 0;
			}
			return (from m in this.mappings
			orderby m.Key
			select m).GetHashCode();
		}

		// Token: 0x04000061 RID: 97
		private static PropertyNameMap defaultMap = PropertyNameMap.CreateDefaultMap();

		// Token: 0x04000062 RID: 98
		private readonly Dictionary<PropertyKey, string> mappings;
	}
}
