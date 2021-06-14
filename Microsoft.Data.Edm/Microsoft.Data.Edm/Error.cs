using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000243 RID: 579
	internal static class Error
	{
		// Token: 0x06000E4E RID: 3662 RVA: 0x0002C1D3 File Offset: 0x0002A3D3
		internal static Exception ArgumentNull(string paramName)
		{
			return new ArgumentNullException(paramName);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0002C1DB File Offset: 0x0002A3DB
		internal static Exception ArgumentOutOfRange(string paramName)
		{
			return new ArgumentOutOfRangeException(paramName);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0002C1E3 File Offset: 0x0002A3E3
		internal static Exception NotImplemented()
		{
			return new NotImplementedException();
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0002C1EA File Offset: 0x0002A3EA
		internal static Exception NotSupported()
		{
			return new NotSupportedException();
		}
	}
}
