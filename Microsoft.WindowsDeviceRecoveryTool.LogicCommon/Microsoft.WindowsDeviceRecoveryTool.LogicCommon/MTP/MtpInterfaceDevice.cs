using System;
using System.Linq.Expressions;
using Nokia.Lucid;
using Nokia.Lucid.DeviceInformation;
using Nokia.Lucid.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.MTP
{
	// Token: 0x0200001D RID: 29
	public sealed class MtpInterfaceDevice
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00006D6D File Offset: 0x00004F6D
		public MtpInterfaceDevice(string devicePath)
		{
			this.devicePath = devicePath;
			this.deviceInfoSet = MtpInterfaceDevice.GetDeviceInfoSet();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006D8C File Offset: 0x00004F8C
		public string ReadDescriptionFromDriver()
		{
			DeviceInfo device = this.deviceInfoSet.GetDevice(this.devicePath);
			return device.ReadDescription();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public string ReadManufacturerFromDriver()
		{
			DeviceInfo device = this.deviceInfoSet.GetDevice(this.devicePath);
			return device.ReadManufacturer();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00006DE4 File Offset: 0x00004FE4
		private static DeviceInfoSet GetDeviceInfoSet()
		{
			return new DeviceInfoSet
			{
				DeviceTypeMap = MtpInterfaceDevice.GetDeviceTypeMap(),
				Filter = MtpInterfaceDevice.GetFilter()
			};
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006E18 File Offset: 0x00005018
		private static DeviceTypeMap GetDeviceTypeMap()
		{
			return new DeviceTypeMap(new Guid("6ac27878-a6fa-4155-ba85-f98f491d4f33"), DeviceType.Interface);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006E3C File Offset: 0x0000503C
		private static Expression<Func<DeviceIdentifier, bool>> GetFilter()
		{
			return (DeviceIdentifier identifier) => true;
		}

		// Token: 0x0400008C RID: 140
		private const string MtpInterfaceGuid = "6ac27878-a6fa-4155-ba85-f98f491d4f33";

		// Token: 0x0400008D RID: 141
		private readonly string devicePath;

		// Token: 0x0400008E RID: 142
		private readonly DeviceInfoSet deviceInfoSet;
	}
}
