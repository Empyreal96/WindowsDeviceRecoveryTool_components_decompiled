using System;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200064C RID: 1612
	internal sealed class TextFormatterHost : TextSource
	{
		// Token: 0x06006AE2 RID: 27362 RVA: 0x001E97EA File Offset: 0x001E79EA
		internal TextFormatterHost(TextFormatter textFormatter, TextFormattingMode textFormattingMode, double pixelsPerDip)
		{
			if (textFormatter == null)
			{
				this.TextFormatter = TextFormatter.FromCurrentDispatcher(textFormattingMode);
			}
			else
			{
				this.TextFormatter = textFormatter;
			}
			base.PixelsPerDip = pixelsPerDip;
		}

		// Token: 0x06006AE3 RID: 27363 RVA: 0x001E9814 File Offset: 0x001E7A14
		public override TextRun GetTextRun(int textSourceCharacterIndex)
		{
			TextRun textRun = this.Context.GetTextRun(textSourceCharacterIndex);
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06006AE4 RID: 27364 RVA: 0x001E9848 File Offset: 0x001E7A48
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
		{
			return this.Context.GetPrecedingText(textSourceCharacterIndexLimit);
		}

		// Token: 0x06006AE5 RID: 27365 RVA: 0x001E9856 File Offset: 0x001E7A56
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return this.Context.GetTextEffectCharacterIndexFromTextSourceCharacterIndex(textSourceCharacterIndex);
		}

		// Token: 0x04003458 RID: 13400
		internal LineBase Context;

		// Token: 0x04003459 RID: 13401
		internal TextFormatter TextFormatter;
	}
}
