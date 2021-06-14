using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020003D7 RID: 983
	internal static class SelectionHighlightInfo
	{
		// Token: 0x0600352F RID: 13615 RVA: 0x000F0F64 File Offset: 0x000EF164
		static SelectionHighlightInfo()
		{
			SelectionHighlightInfo._objectMaskBrush.Opacity = 0.5;
			SelectionHighlightInfo._objectMaskBrush.Freeze();
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06003530 RID: 13616 RVA: 0x000F0F92 File Offset: 0x000EF192
		internal static Brush ForegroundBrush
		{
			get
			{
				return SystemColors.HighlightTextBrush;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06003531 RID: 13617 RVA: 0x000F0F99 File Offset: 0x000EF199
		internal static Brush BackgroundBrush
		{
			get
			{
				return SystemColors.HighlightBrush;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003532 RID: 13618 RVA: 0x000F0FA0 File Offset: 0x000EF1A0
		internal static Brush ObjectMaskBrush
		{
			get
			{
				return SelectionHighlightInfo._objectMaskBrush;
			}
		}

		// Token: 0x04002501 RID: 9473
		private static readonly Brush _objectMaskBrush = new SolidColorBrush(SystemColors.HighlightColor);
	}
}
