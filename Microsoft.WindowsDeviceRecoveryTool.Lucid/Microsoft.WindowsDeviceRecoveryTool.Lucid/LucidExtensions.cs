using System;
using Nokia.Lucid.DeviceInformation;

namespace Microsoft.WindowsDeviceRecoveryTool.Lucid
{
	// Token: 0x02000007 RID: 7
	internal static class LucidExtensions
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000022A6 File Offset: 0x000004A6
		internal static Guid ReadClassGuid(this IDevicePropertySet propertySet)
		{
			return (Guid)propertySet.ReadProperty(LucidExtensions.DEVPKEY_Device_ClassGuid, PropertyValueFormatter.Default);
		}

		// Token: 0x04000006 RID: 6
		private static readonly PropertyKey DEVPKEY_Device_ClassGuid = new PropertyKey(2757502286U, 57116, 20221, 128, 32, 103, 209, 70, 168, 80, 224, 10);
	}
}
