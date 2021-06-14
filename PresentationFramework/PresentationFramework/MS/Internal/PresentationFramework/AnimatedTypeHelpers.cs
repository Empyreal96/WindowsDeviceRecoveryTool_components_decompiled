using System;
using System.Windows;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x02000800 RID: 2048
	internal static class AnimatedTypeHelpers
	{
		// Token: 0x06007DA2 RID: 32162 RVA: 0x002347F6 File Offset: 0x002329F6
		private static double InterpolateDouble(double from, double to, double progress)
		{
			return from + (to - from) * progress;
		}

		// Token: 0x06007DA3 RID: 32163 RVA: 0x00234800 File Offset: 0x00232A00
		internal static Thickness InterpolateThickness(Thickness from, Thickness to, double progress)
		{
			return new Thickness(AnimatedTypeHelpers.InterpolateDouble(from.Left, to.Left, progress), AnimatedTypeHelpers.InterpolateDouble(from.Top, to.Top, progress), AnimatedTypeHelpers.InterpolateDouble(from.Right, to.Right, progress), AnimatedTypeHelpers.InterpolateDouble(from.Bottom, to.Bottom, progress));
		}

		// Token: 0x06007DA4 RID: 32164 RVA: 0x00234862 File Offset: 0x00232A62
		private static double AddDouble(double value1, double value2)
		{
			return value1 + value2;
		}

		// Token: 0x06007DA5 RID: 32165 RVA: 0x00234868 File Offset: 0x00232A68
		internal static Thickness AddThickness(Thickness value1, Thickness value2)
		{
			return new Thickness(AnimatedTypeHelpers.AddDouble(value1.Left, value2.Left), AnimatedTypeHelpers.AddDouble(value1.Top, value2.Top), AnimatedTypeHelpers.AddDouble(value1.Right, value2.Right), AnimatedTypeHelpers.AddDouble(value1.Bottom, value2.Bottom));
		}

		// Token: 0x06007DA6 RID: 32166 RVA: 0x002348C8 File Offset: 0x00232AC8
		internal static Thickness SubtractThickness(Thickness value1, Thickness value2)
		{
			return new Thickness(value1.Left - value2.Left, value1.Top - value2.Top, value1.Right - value2.Right, value1.Bottom - value2.Bottom);
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x00234916 File Offset: 0x00232B16
		private static double GetSegmentLengthDouble(double from, double to)
		{
			return Math.Abs(to - from);
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x00234920 File Offset: 0x00232B20
		internal static double GetSegmentLengthThickness(Thickness from, Thickness to)
		{
			double d = Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Left, to.Left), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Top, to.Top), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Right, to.Right), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Bottom, to.Bottom), 2.0);
			return Math.Sqrt(d);
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x002349BB File Offset: 0x00232BBB
		private static double ScaleDouble(double value, double factor)
		{
			return value * factor;
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x002349C0 File Offset: 0x00232BC0
		internal static Thickness ScaleThickness(Thickness value, double factor)
		{
			return new Thickness(AnimatedTypeHelpers.ScaleDouble(value.Left, factor), AnimatedTypeHelpers.ScaleDouble(value.Top, factor), AnimatedTypeHelpers.ScaleDouble(value.Right, factor), AnimatedTypeHelpers.ScaleDouble(value.Bottom, factor));
		}

		// Token: 0x06007DAB RID: 32171 RVA: 0x002349FB File Offset: 0x00232BFB
		private static bool IsValidAnimationValueDouble(double value)
		{
			return !AnimatedTypeHelpers.IsInvalidDouble(value);
		}

		// Token: 0x06007DAC RID: 32172 RVA: 0x00234A08 File Offset: 0x00232C08
		internal static bool IsValidAnimationValueThickness(Thickness value)
		{
			return AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Left) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Top) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Right) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Bottom);
		}

		// Token: 0x06007DAD RID: 32173 RVA: 0x0018D6AE File Offset: 0x0018B8AE
		private static double GetZeroValueDouble(double baseValue)
		{
			return 0.0;
		}

		// Token: 0x06007DAE RID: 32174 RVA: 0x00234A45 File Offset: 0x00232C45
		internal static Thickness GetZeroValueThickness(Thickness baseValue)
		{
			return new Thickness(AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Left), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Top), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Right), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Bottom));
		}

		// Token: 0x06007DAF RID: 32175 RVA: 0x00234A7C File Offset: 0x00232C7C
		private static bool IsInvalidDouble(double value)
		{
			return double.IsInfinity(value) || DoubleUtil.IsNaN(value);
		}
	}
}
