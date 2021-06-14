using System;
using System.Linq.Expressions;
using Nokia.Lucid.Primitives;

namespace Nokia.Lucid
{
	// Token: 0x02000049 RID: 73
	public static class FilterExpression
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000D46B File Offset: 0x0000B66B
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000D472 File Offset: 0x0000B672
		public static Expression<Func<DeviceIdentifier, bool>> DefaultExpression
		{
			get
			{
				return FilterExpression.defaultExpression;
			}
			set
			{
				FilterExpression.defaultExpression = value;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000D47C File Offset: 0x0000B67C
		public static Expression<Func<DeviceIdentifier, bool>> CreateDefaultExpression()
		{
			return (DeviceIdentifier s) => (s.Vid("0421") && ((s.Pid("0660") && s.MI(new int[]
			{
				4,
				5,
				6
			})) || (s.Pid("0661") && s.MI(new int[]
			{
				2,
				3
			})) || s.Pid("066E"))) || s.Guid(WindowsPhoneIdentifiers.NcsdDeviceInterfaceGuid) || s.Guid(WindowsPhoneIdentifiers.TestServerDeviceInterfaceGuid) || s.Guid(WindowsPhoneIdentifiers.LabelAppDeviceInterfaceGuid) || s.Guid(WindowsPhoneIdentifiers.UefiDeviceInterfaceGuid) || s.Guid(WindowsPhoneIdentifiers.EdDeviceInterfaceGuid) || ((s.Vid("0421") || s.Vid("045E")) && s.Guid(WindowsPhoneIdentifiers.GenericUsbDeviceInterfaceGuid));
		}

		// Token: 0x0400013B RID: 315
		public static readonly Expression<Func<DeviceIdentifier, bool>> EmptyExpression = (DeviceIdentifier s) => false;

		// Token: 0x0400013C RID: 316
		public static readonly Expression<Func<DeviceIdentifier, bool>> NoFilter = (DeviceIdentifier s) => true;

		// Token: 0x0400013D RID: 317
		private static Expression<Func<DeviceIdentifier, bool>> defaultExpression = FilterExpression.CreateDefaultExpression();
	}
}
