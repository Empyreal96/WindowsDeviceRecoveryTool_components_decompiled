using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020002B3 RID: 691
	internal static class Error
	{
		// Token: 0x06001B4B RID: 6987 RVA: 0x0005B273 File Offset: 0x00059473
		internal static Exception ArgumentNull(string paramName)
		{
			return new ArgumentNullException(paramName);
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0005B27B File Offset: 0x0005947B
		internal static Exception ArgumentOutOfRange(string paramName)
		{
			return new ArgumentOutOfRangeException(paramName);
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0005B283 File Offset: 0x00059483
		internal static Exception NotImplemented()
		{
			return new NotImplementedException();
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x0005B28A File Offset: 0x0005948A
		internal static Exception NotSupported()
		{
			return new NotSupportedException();
		}
	}
}
