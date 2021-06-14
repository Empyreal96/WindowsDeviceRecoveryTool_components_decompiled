using System;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x02000011 RID: 17
	public static class DevicePropertySetExtensions
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000037B8 File Offset: 0x000019B8
		public static string ReadName(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(3072717104U, 18415, 4122, 165, 241, 2, 96, 140, 158, 235, 172, 10), PropertyValueFormatter.Default);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000380C File Offset: 0x00001A0C
		public static string ReadDescription(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 2), PropertyValueFormatter.Default);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000385C File Offset: 0x00001A5C
		public static string[] ReadHardwareIds(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 3), PropertyValueFormatter.Default);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000038AC File Offset: 0x00001AAC
		public static string[] ReadCompatibleIds(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 4), PropertyValueFormatter.Default);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000038FC File Offset: 0x00001AFC
		public static string ReadService(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 6), PropertyValueFormatter.Default);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000394C File Offset: 0x00001B4C
		public static string ReadSetupClassName(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 9), PropertyValueFormatter.Default);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000399C File Offset: 0x00001B9C
		public static Guid ReadSetupClass(this IDevicePropertySet propertySet)
		{
			return (Guid)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 10), PropertyValueFormatter.Default);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000039EC File Offset: 0x00001BEC
		public static string ReadDriverName(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 11), PropertyValueFormatter.Default);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003A3C File Offset: 0x00001C3C
		public static int ReadConfiguration(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 12), PropertyValueFormatter.Default);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003A8C File Offset: 0x00001C8C
		public static string ReadManufacturer(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 13), PropertyValueFormatter.Default);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003ADC File Offset: 0x00001CDC
		public static string ReadFriendlyName(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 14), PropertyValueFormatter.Default);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003B2C File Offset: 0x00001D2C
		public static string ReadLocationInformation(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 15), PropertyValueFormatter.Default);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003B7C File Offset: 0x00001D7C
		public static string ReadPhysicalDeviceObjectName(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 16), PropertyValueFormatter.Default);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003BCC File Offset: 0x00001DCC
		public static int ReadCapabilities(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 17), PropertyValueFormatter.Default);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003C1C File Offset: 0x00001E1C
		public static string ReadUINumber(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 18), PropertyValueFormatter.Default);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003C6C File Offset: 0x00001E6C
		public static string[] ReadUpperFilters(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 19), PropertyValueFormatter.Default);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003CBC File Offset: 0x00001EBC
		public static string[] ReadLowerFilters(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 20), PropertyValueFormatter.Default);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003D0C File Offset: 0x00001F0C
		public static Guid ReadBusType(this IDevicePropertySet propertySet)
		{
			return (Guid)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 21), PropertyValueFormatter.Default);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003D5C File Offset: 0x00001F5C
		public static int ReadLegacyBusType(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 22), PropertyValueFormatter.Default);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003DAC File Offset: 0x00001FAC
		public static int ReadBusNumber(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 23), PropertyValueFormatter.Default);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003DFC File Offset: 0x00001FFC
		public static string ReadEnumerator(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 24), PropertyValueFormatter.Default);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003E4C File Offset: 0x0000204C
		public static byte[] ReadSecurityDescriptor(this IDevicePropertySet propertySet)
		{
			return (byte[])propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 25), PropertyValueFormatter.Default);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003E9C File Offset: 0x0000209C
		public static string ReadSecurityDescriptorString(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 26), PropertyValueFormatter.Default);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003EEC File Offset: 0x000020EC
		public static int ReadDevType(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 27), PropertyValueFormatter.Default);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003F3C File Offset: 0x0000213C
		public static bool ReadExclusive(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 28), PropertyValueFormatter.Default);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003F8C File Offset: 0x0000218C
		public static int ReadCharacteristics(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 29), PropertyValueFormatter.Default);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003FDC File Offset: 0x000021DC
		public static int ReadAddress(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 30), PropertyValueFormatter.Default);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000402C File Offset: 0x0000222C
		public static string ReadUINumberDescriptionFormat(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 31), PropertyValueFormatter.Default);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000407C File Offset: 0x0000227C
		public static byte[] ReadPowerData(this IDevicePropertySet propertySet)
		{
			return (byte[])propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 32), PropertyValueFormatter.Default);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000040CC File Offset: 0x000022CC
		public static int ReadRemovalPolicy(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 33), PropertyValueFormatter.Default);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000411C File Offset: 0x0000231C
		public static int ReadRemovalPolicyDefault(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 34), PropertyValueFormatter.Default);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000416C File Offset: 0x0000236C
		public static int ReadRemovalPolicyOverride(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 35), PropertyValueFormatter.Default);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000041BC File Offset: 0x000023BC
		public static int ReadInstallState(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 36), PropertyValueFormatter.Default);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000420C File Offset: 0x0000240C
		public static string[] ReadLocationPaths(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 37), PropertyValueFormatter.Default);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000425C File Offset: 0x0000245C
		public static Guid ReadBaseContainerId(this IDevicePropertySet propertySet)
		{
			return (Guid)propertySet.ReadProperty(new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 38), PropertyValueFormatter.Default);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000042AC File Offset: 0x000024AC
		public static int ReadInstanceStatus(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 2), PropertyValueFormatter.Default);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000042FC File Offset: 0x000024FC
		public static int ReadProblemCode(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 3), PropertyValueFormatter.Default);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000434C File Offset: 0x0000254C
		public static string[] ReadEjectionRelations(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 4), PropertyValueFormatter.Default);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000439C File Offset: 0x0000259C
		public static string[] ReadRemovalRelations(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 5), PropertyValueFormatter.Default);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000043EC File Offset: 0x000025EC
		public static string[] ReadPowerRelations(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 6), PropertyValueFormatter.Default);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000443C File Offset: 0x0000263C
		public static string[] ReadBusRelations(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 7), PropertyValueFormatter.Default);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000448C File Offset: 0x0000268C
		public static string ReadParentInstanceId(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 8), PropertyValueFormatter.Default);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000044DC File Offset: 0x000026DC
		public static string[] ReadChildInstanceIds(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 9), PropertyValueFormatter.Default);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000452C File Offset: 0x0000272C
		public static string[] ReadSiblingInstanceIds(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 10), PropertyValueFormatter.Default);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000457C File Offset: 0x0000277C
		public static string[] ReadTransportRelations(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(1128310469U, 37882, 18182, 151, 44, 123, 100, 128, 8, 165, 167, 11), PropertyValueFormatter.Default);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000045CC File Offset: 0x000027CC
		public static bool ReadIsReported(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2152296704U, 35955, 18617, 170, 217, 206, 56, 126, 25, 197, 110, 2), PropertyValueFormatter.Default);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000461C File Offset: 0x0000281C
		public static bool ReadIsLegacy(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2152296704U, 35955, 18617, 170, 217, 206, 56, 126, 25, 197, 110, 3), PropertyValueFormatter.Default);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000466C File Offset: 0x0000286C
		public static string ReadInstanceId(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2026065864, 4170, 19146, 158, 164, 82, 77, 82, 153, 110, 87, 256), PropertyValueFormatter.Default);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000046BC File Offset: 0x000028BC
		public static Guid ReadContainerId(this IDevicePropertySet propertySet)
		{
			return (Guid)propertySet.ReadProperty(new PropertyKey(2357121542U, 16266, 18471, 179, 171, 174, 158, 31, 174, 252, 108, 2), PropertyValueFormatter.Default);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004710 File Offset: 0x00002910
		public static Guid ReadModelId(this IDevicePropertySet propertySet)
		{
			return (Guid)propertySet.ReadProperty(new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 2), PropertyValueFormatter.Default);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004760 File Offset: 0x00002960
		public static int ReadFriendlyNameAttributes(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 3), PropertyValueFormatter.Default);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000047B0 File Offset: 0x000029B0
		public static int ReadManufacturerAttributes(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 4), PropertyValueFormatter.Default);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004800 File Offset: 0x00002A00
		public static bool ReadIsPresenceNotForDevice(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2161647270U, 29811, 19212, 130, 22, 239, 193, 26, 44, 76, 139, 5), PropertyValueFormatter.Default);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004850 File Offset: 0x00002A50
		public static int ReadNonUniformMemoryArchitectureProximityDomain(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 1), PropertyValueFormatter.Default);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000048A4 File Offset: 0x00002AA4
		public static int ReadDynamicHardwarePartitioningRebalancePolicy(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 2), PropertyValueFormatter.Default);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000048F8 File Offset: 0x00002AF8
		public static int ReadNonUniformMemoryArchitectureNode(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 3), PropertyValueFormatter.Default);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000494C File Offset: 0x00002B4C
		public static string ReadBusReportedDeviceDescription(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(1410045054U, 35648, 17852, 168, 162, 106, 11, 137, 76, 189, 162, 4), PropertyValueFormatter.Default);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000049A0 File Offset: 0x00002BA0
		public static int ReadSessionId(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 6), PropertyValueFormatter.Default);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000049EC File Offset: 0x00002BEC
		public static DateTime ReadInstallDate(this IDevicePropertySet propertySet)
		{
			return (DateTime)propertySet.ReadProperty(new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 100), PropertyValueFormatter.Default);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004A38 File Offset: 0x00002C38
		public static DateTime ReadFirstInstallDate(this IDevicePropertySet propertySet)
		{
			return (DateTime)propertySet.ReadProperty(new PropertyKey(2212127526U, 38822, 16520, 148, 83, 161, 146, 63, 87, 59, 41, 101), PropertyValueFormatter.Default);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004A84 File Offset: 0x00002C84
		public static DateTime ReadDriverDate(this IDevicePropertySet propertySet)
		{
			return (DateTime)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 2), PropertyValueFormatter.Default);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004AD8 File Offset: 0x00002CD8
		public static string ReadDriverVersion(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 3), PropertyValueFormatter.Default);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004B2C File Offset: 0x00002D2C
		public static string ReadDriverDescription(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 4), PropertyValueFormatter.Default);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004B80 File Offset: 0x00002D80
		public static string ReadDriverInfPath(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 5), PropertyValueFormatter.Default);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004BD4 File Offset: 0x00002DD4
		public static string ReadDriverInfSection(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 6), PropertyValueFormatter.Default);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004C28 File Offset: 0x00002E28
		public static string ReadDriverInfSectionExt(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 7), PropertyValueFormatter.Default);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004C7C File Offset: 0x00002E7C
		public static string ReadMatchingDeviceId(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 8), PropertyValueFormatter.Default);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public static string ReadDriverProvider(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 9), PropertyValueFormatter.Default);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004D28 File Offset: 0x00002F28
		public static string ReadDriverPropPageProvider(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 10), PropertyValueFormatter.Default);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004D80 File Offset: 0x00002F80
		public static string[] ReadDriverCoInstallers(this IDevicePropertySet propertySet)
		{
			return (string[])propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 11), PropertyValueFormatter.Default);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004DD8 File Offset: 0x00002FD8
		public static string ReadResourcePickerTags(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 12), PropertyValueFormatter.Default);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004E30 File Offset: 0x00003030
		public static string ReadResourcePickerExceptions(this IDevicePropertySet propertySet)
		{
			return (string)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 13), PropertyValueFormatter.Default);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004E88 File Offset: 0x00003088
		public static int ReadDriverRank(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 14), PropertyValueFormatter.Default);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004EE0 File Offset: 0x000030E0
		public static int ReadDriverLogoLevel(this IDevicePropertySet propertySet)
		{
			return (int)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 15), PropertyValueFormatter.Default);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004F38 File Offset: 0x00003138
		public static bool ReadIsNoConnectSound(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 17), PropertyValueFormatter.Default);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004F90 File Offset: 0x00003190
		public static bool ReadIsGenericDriverInstalled(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 18), PropertyValueFormatter.Default);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004FE8 File Offset: 0x000031E8
		public static bool ReadIsAdditionalSoftwareRequested(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2830656989U, 11837, 16532, 173, 151, 229, 147, 167, 12, 117, 214, 19), PropertyValueFormatter.Default);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005040 File Offset: 0x00003240
		public static bool ReadIsSafeRemovalRequired(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2950264384U, 34467, 16912, 182, 124, 40, 156, 65, 170, 190, 85, 2), PropertyValueFormatter.Default);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005090 File Offset: 0x00003290
		public static bool ReadIsSafeRemovalRequiredOverride(this IDevicePropertySet propertySet)
		{
			return (bool)propertySet.ReadProperty(new PropertyKey(2950264384U, 34467, 16912, 182, 124, 40, 156, 65, 170, 190, 85, 3), PropertyValueFormatter.Default);
		}
	}
}
