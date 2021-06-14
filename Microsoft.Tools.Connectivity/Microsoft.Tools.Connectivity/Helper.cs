using System;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x0200000E RID: 14
	internal static class Helper
	{
		// Token: 0x06000091 RID: 145 RVA: 0x000042E3 File Offset: 0x000024E3
		public static int GetHRForWin32Error(uint dwLastError)
		{
			if ((dwLastError & 2147483648U) != 0U)
			{
				return (int)dwLastError;
			}
			return (int)((dwLastError & 65535U) | 2147942400U);
		}
	}
}
