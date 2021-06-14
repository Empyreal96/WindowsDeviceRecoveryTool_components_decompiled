using System;

namespace System.Spatial
{
	// Token: 0x0200008E RID: 142
	internal static class Error
	{
		// Token: 0x060003A5 RID: 933 RVA: 0x00009FEF File Offset: 0x000081EF
		internal static Exception ArgumentNull(string paramName)
		{
			return new ArgumentNullException(paramName);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00009FF7 File Offset: 0x000081F7
		internal static Exception ArgumentOutOfRange(string paramName)
		{
			return new ArgumentOutOfRangeException(paramName);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00009FFF File Offset: 0x000081FF
		internal static Exception NotImplemented()
		{
			return new NotImplementedException();
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000A006 File Offset: 0x00008206
		internal static Exception NotSupported()
		{
			return new NotSupportedException();
		}
	}
}
