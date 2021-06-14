using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200062B RID: 1579
	internal class ListMarkerLine : LineBase
	{
		// Token: 0x060068A1 RID: 26785 RVA: 0x001D8460 File Offset: 0x001D6660
		internal ListMarkerLine(TextFormatterHost host, ListParaClient paraClient) : base(paraClient)
		{
			this._host = host;
		}

		// Token: 0x060068A2 RID: 26786 RVA: 0x001D8470 File Offset: 0x001D6670
		internal override TextRun GetTextRun(int dcp)
		{
			return new ParagraphBreakRun(1, PTS.FSFLRES.fsflrEndOfParagraph);
		}

		// Token: 0x060068A3 RID: 26787 RVA: 0x001D8479 File Offset: 0x001D6679
		internal override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			return new TextSpan<CultureSpecificCharacterBufferRange>(0, new CultureSpecificCharacterBufferRange(null, CharacterBufferRange.Empty));
		}

		// Token: 0x060068A4 RID: 26788 RVA: 0x00012630 File Offset: 0x00010830
		internal override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp)
		{
			return dcp;
		}

		// Token: 0x060068A5 RID: 26789 RVA: 0x001D848C File Offset: 0x001D668C
		internal void FormatAndDrawVisual(DrawingContext ctx, LineProperties lineProps, int ur, int vrBaseline)
		{
			bool flag = lineProps.FlowDirection == FlowDirection.RightToLeft;
			this._host.Context = this;
			try
			{
				TextLine textLine = this._host.TextFormatter.FormatLine(this._host, 0, 0.0, lineProps.FirstLineProps, null, new TextRunCache());
				Point origin = new Point(TextDpi.FromTextDpi(ur), TextDpi.FromTextDpi(vrBaseline) - textLine.Baseline);
				textLine.Draw(ctx, origin, flag ? InvertAxes.Horizontal : InvertAxes.None);
				textLine.Dispose();
			}
			finally
			{
				this._host.Context = null;
			}
		}

		// Token: 0x040033E4 RID: 13284
		private readonly TextFormatterHost _host;
	}
}
