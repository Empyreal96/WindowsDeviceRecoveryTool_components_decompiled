using System;

namespace Standard
{
	// Token: 0x02000006 RID: 6
	internal static class DoubleUtilities
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002320 File Offset: 0x00000520
		public static bool AreClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = value1 - value2;
			return num < 1.53E-06 && num > -1.53E-06;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002351 File Offset: 0x00000551
		public static bool LessThan(double value1, double value2)
		{
			return value1 < value2 && !DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002363 File Offset: 0x00000563
		public static bool GreaterThan(double value1, double value2)
		{
			return value1 > value2 && !DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002375 File Offset: 0x00000575
		public static bool LessThanOrClose(double value1, double value2)
		{
			return value1 < value2 || DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002384 File Offset: 0x00000584
		public static bool GreaterThanOrClose(double value1, double value2)
		{
			return value1 > value2 || DoubleUtilities.AreClose(value1, value2);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002393 File Offset: 0x00000593
		public static bool IsFinite(double value)
		{
			return !double.IsNaN(value) && !double.IsInfinity(value);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000023A8 File Offset: 0x000005A8
		public static bool IsValidSize(double value)
		{
			return DoubleUtilities.IsFinite(value) && DoubleUtilities.GreaterThanOrClose(value, 0.0);
		}

		// Token: 0x04000020 RID: 32
		private const double Epsilon = 1.53E-06;
	}
}
