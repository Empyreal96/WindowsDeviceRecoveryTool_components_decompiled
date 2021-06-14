using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.Text
{
	// Token: 0x02000605 RID: 1541
	internal static class TextDpi
	{
		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x06006676 RID: 26230 RVA: 0x001CC6E4 File Offset: 0x001CA8E4
		internal static double MinWidth
		{
			get
			{
				return 0.0033333333333333335;
			}
		}

		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x06006677 RID: 26231 RVA: 0x001CC6EF File Offset: 0x001CA8EF
		internal static double MaxWidth
		{
			get
			{
				return 3579139.4066666667;
			}
		}

		// Token: 0x06006678 RID: 26232 RVA: 0x001CC6FC File Offset: 0x001CA8FC
		internal static int ToTextDpi(double d)
		{
			if (DoubleUtil.IsZero(d))
			{
				return 0;
			}
			if (d > 0.0)
			{
				if (d > 3579139.4066666667)
				{
					d = 3579139.4066666667;
				}
				else if (d < 0.0033333333333333335)
				{
					d = 0.0033333333333333335;
				}
			}
			else if (d < -3579139.4066666667)
			{
				d = -3579139.4066666667;
			}
			else if (d > -0.0033333333333333335)
			{
				d = -0.0033333333333333335;
			}
			return (int)Math.Round(d * 300.0);
		}

		// Token: 0x06006679 RID: 26233 RVA: 0x001CC792 File Offset: 0x001CA992
		internal static double FromTextDpi(int i)
		{
			return (double)i / 300.0;
		}

		// Token: 0x0600667A RID: 26234 RVA: 0x001CC7A0 File Offset: 0x001CA9A0
		internal static PTS.FSPOINT ToTextPoint(Point point)
		{
			return new PTS.FSPOINT
			{
				u = TextDpi.ToTextDpi(point.X),
				v = TextDpi.ToTextDpi(point.Y)
			};
		}

		// Token: 0x0600667B RID: 26235 RVA: 0x001CC7DC File Offset: 0x001CA9DC
		internal static PTS.FSVECTOR ToTextSize(Size size)
		{
			return new PTS.FSVECTOR
			{
				du = TextDpi.ToTextDpi(size.Width),
				dv = TextDpi.ToTextDpi(size.Height)
			};
		}

		// Token: 0x0600667C RID: 26236 RVA: 0x001CC818 File Offset: 0x001CAA18
		internal static Rect FromTextRect(PTS.FSRECT fsrect)
		{
			return new Rect(TextDpi.FromTextDpi(fsrect.u), TextDpi.FromTextDpi(fsrect.v), TextDpi.FromTextDpi(fsrect.du), TextDpi.FromTextDpi(fsrect.dv));
		}

		// Token: 0x0600667D RID: 26237 RVA: 0x001CC84B File Offset: 0x001CAA4B
		internal static void EnsureValidLineOffset(ref double offset)
		{
			if (offset > 3579139.4066666667)
			{
				offset = 3579139.4066666667;
				return;
			}
			if (offset < -3579139.4066666667)
			{
				offset = -3579139.4066666667;
			}
		}

		// Token: 0x0600667E RID: 26238 RVA: 0x001CC87E File Offset: 0x001CAA7E
		internal static void SnapToTextDpi(ref Size size)
		{
			size = new Size(TextDpi.FromTextDpi(TextDpi.ToTextDpi(size.Width)), TextDpi.FromTextDpi(TextDpi.ToTextDpi(size.Height)));
		}

		// Token: 0x0600667F RID: 26239 RVA: 0x001CC8AB File Offset: 0x001CAAAB
		internal static void EnsureValidLineWidth(ref double width)
		{
			if (width > 3579139.4066666667)
			{
				width = 3579139.4066666667;
				return;
			}
			if (width < 0.0033333333333333335)
			{
				width = 0.0033333333333333335;
			}
		}

		// Token: 0x06006680 RID: 26240 RVA: 0x001CC8E0 File Offset: 0x001CAAE0
		internal static void EnsureValidLineWidth(ref Size size)
		{
			if (size.Width > 3579139.4066666667)
			{
				size.Width = 3579139.4066666667;
				return;
			}
			if (size.Width < 0.0033333333333333335)
			{
				size.Width = 0.0033333333333333335;
			}
		}

		// Token: 0x06006681 RID: 26241 RVA: 0x001CC92E File Offset: 0x001CAB2E
		internal static void EnsureValidLineWidth(ref int width)
		{
			if (width > 1073741822)
			{
				width = 1073741822;
				return;
			}
			if (width < 1)
			{
				width = 1;
			}
		}

		// Token: 0x06006682 RID: 26242 RVA: 0x001CC94C File Offset: 0x001CAB4C
		internal static void EnsureValidPageSize(ref Size size)
		{
			if (size.Width > 3579139.4066666667)
			{
				size.Width = 3579139.4066666667;
			}
			else if (size.Width < 0.0033333333333333335)
			{
				size.Width = 0.0033333333333333335;
			}
			if (size.Height > 3579139.4066666667)
			{
				size.Height = 3579139.4066666667;
				return;
			}
			if (size.Height < 0.0033333333333333335)
			{
				size.Height = 0.0033333333333333335;
			}
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x001CC8AB File Offset: 0x001CAAAB
		internal static void EnsureValidPageWidth(ref double width)
		{
			if (width > 3579139.4066666667)
			{
				width = 3579139.4066666667;
				return;
			}
			if (width < 0.0033333333333333335)
			{
				width = 0.0033333333333333335;
			}
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x001CC9DC File Offset: 0x001CABDC
		internal static void EnsureValidPageMargin(ref Thickness pageMargin, Size pageSize)
		{
			if (pageMargin.Left >= pageSize.Width)
			{
				pageMargin.Right = 0.0;
			}
			if (pageMargin.Left + pageMargin.Right >= pageSize.Width)
			{
				pageMargin.Right = Math.Max(0.0, pageSize.Width - pageMargin.Left - 0.0033333333333333335);
				if (pageMargin.Left + pageMargin.Right >= pageSize.Width)
				{
					pageMargin.Left = pageSize.Width - 0.0033333333333333335;
				}
			}
			if (pageMargin.Top >= pageSize.Height)
			{
				pageMargin.Bottom = 0.0;
			}
			if (pageMargin.Top + pageMargin.Bottom >= pageSize.Height)
			{
				pageMargin.Bottom = Math.Max(0.0, pageSize.Height - pageMargin.Top - 0.0033333333333333335);
				if (pageMargin.Top + pageMargin.Bottom >= pageSize.Height)
				{
					pageMargin.Top = pageSize.Height - 0.0033333333333333335;
				}
			}
		}

		// Token: 0x06006685 RID: 26245 RVA: 0x001CCB04 File Offset: 0x001CAD04
		internal static void EnsureValidObjSize(ref Size size)
		{
			if (size.Width > 1193046.4688888888)
			{
				size.Width = 1193046.4688888888;
			}
			if (size.Height > 1193046.4688888888)
			{
				size.Height = 1193046.4688888888;
			}
		}

		// Token: 0x0400331A RID: 13082
		private const double _scale = 300.0;

		// Token: 0x0400331B RID: 13083
		private const int _maxSizeInt = 1073741822;

		// Token: 0x0400331C RID: 13084
		private const double _maxSize = 3579139.4066666667;

		// Token: 0x0400331D RID: 13085
		private const int _minSizeInt = 1;

		// Token: 0x0400331E RID: 13086
		private const double _minSize = 0.0033333333333333335;

		// Token: 0x0400331F RID: 13087
		private const double _maxObjSize = 1193046.4688888888;
	}
}
