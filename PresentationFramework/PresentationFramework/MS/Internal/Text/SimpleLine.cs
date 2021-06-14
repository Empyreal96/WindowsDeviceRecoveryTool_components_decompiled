using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000604 RID: 1540
	internal sealed class SimpleLine : Line
	{
		// Token: 0x06006672 RID: 26226 RVA: 0x001CC618 File Offset: 0x001CA818
		public override TextRun GetTextRun(int dcp)
		{
			TextRun textRun;
			if (dcp < this._content.Length)
			{
				textRun = new TextCharacters(this._content, dcp, this._content.Length - dcp, this._textProps);
			}
			else
			{
				textRun = new TextEndOfParagraph(Line._syntheticCharacterLength);
			}
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06006673 RID: 26227 RVA: 0x001CC67C File Offset: 0x001CA87C
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				empty = new CharacterBufferRange(this._content, 0, Math.Min(dcp, this._content.Length));
				culture = this._textProps.CultureInfo;
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(dcp, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x06006674 RID: 26228 RVA: 0x00012630 File Offset: 0x00010830
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return textSourceCharacterIndex;
		}

		// Token: 0x06006675 RID: 26229 RVA: 0x001CC6CD File Offset: 0x001CA8CD
		internal SimpleLine(TextBlock owner, string content, TextRunProperties textProps) : base(owner)
		{
			this._content = content;
			this._textProps = textProps;
		}

		// Token: 0x04003318 RID: 13080
		private readonly string _content;

		// Token: 0x04003319 RID: 13081
		private readonly TextRunProperties _textProps;
	}
}
