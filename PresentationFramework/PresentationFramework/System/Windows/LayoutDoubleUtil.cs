using System;

namespace System.Windows
{
	// Token: 0x020000C4 RID: 196
	internal static class LayoutDoubleUtil
	{
		// Token: 0x06000670 RID: 1648 RVA: 0x00013F6C File Offset: 0x0001216C
		internal static bool AreClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = value1 - value2;
			return num < 1.53E-06 && num > -1.53E-06;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00013F9D File Offset: 0x0001219D
		internal static bool LessThan(double value1, double value2)
		{
			return value1 < value2 && !LayoutDoubleUtil.AreClose(value1, value2);
		}

		// Token: 0x0400069D RID: 1693
		private const double eps = 1.53E-06;
	}
}
