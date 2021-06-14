using System;

namespace System.Windows.Documents
{
	// Token: 0x020003B9 RID: 953
	internal static class Validators
	{
		// Token: 0x060032D7 RID: 13015 RVA: 0x000E5232 File Offset: 0x000E3432
		internal static bool IsValidFontSize(long fs)
		{
			return fs >= 0L && fs <= 32767L;
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000E5247 File Offset: 0x000E3447
		internal static bool IsValidWidthType(long wt)
		{
			return wt >= 0L && wt <= 3L;
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x000E5258 File Offset: 0x000E3458
		internal static long MakeValidShading(long s)
		{
			if (s > 10000L)
			{
				s = 10000L;
			}
			return s;
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x000E526C File Offset: 0x000E346C
		internal static long MakeValidBorderWidth(long w)
		{
			if (w < 0L)
			{
				w = 0L;
			}
			if (w > 1440L)
			{
				w = 1440L;
			}
			return w;
		}
	}
}
