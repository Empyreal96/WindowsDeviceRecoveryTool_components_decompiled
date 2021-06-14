using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000633 RID: 1587
	internal sealed class OptimalTextSource : LineBase
	{
		// Token: 0x060068E9 RID: 26857 RVA: 0x001D987F File Offset: 0x001D7A7F
		internal OptimalTextSource(TextFormatterHost host, int cpPara, int durTrack, TextParaClient paraClient, TextRunCache runCache) : base(paraClient)
		{
			this._host = host;
			this._durTrack = durTrack;
			this._runCache = runCache;
			this._cpPara = cpPara;
		}

		// Token: 0x060068EA RID: 26858 RVA: 0x001D98A6 File Offset: 0x001D7AA6
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x060068EB RID: 26859 RVA: 0x001D98B0 File Offset: 0x001D7AB0
		internal override TextRun GetTextRun(int dcp)
		{
			TextRun textRun = null;
			ITextContainer textContainer = this._paraClient.Paragraph.StructuralCache.TextContainer;
			StaticTextPointer position = textContainer.CreateStaticPointerAtOffset(this._cpPara + dcp);
			switch (position.GetPointerContext(LogicalDirection.Forward))
			{
			case TextPointerContext.None:
				textRun = new ParagraphBreakRun(LineBase._syntheticCharacterLength, PTS.FSFLRES.fsflrEndOfParagraph);
				break;
			case TextPointerContext.Text:
				textRun = base.HandleText(position);
				break;
			case TextPointerContext.EmbeddedElement:
				textRun = base.HandleEmbeddedObject(dcp, position);
				break;
			case TextPointerContext.ElementStart:
				textRun = base.HandleElementStartEdge(position);
				break;
			case TextPointerContext.ElementEnd:
				textRun = base.HandleElementEndEdge(position);
				break;
			}
			Invariant.Assert(textRun != null, "TextRun has not been created.");
			Invariant.Assert(textRun.Length > 0, "TextRun has to have positive length.");
			return textRun;
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x001D9960 File Offset: 0x001D7B60
		internal override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			Invariant.Assert(dcp >= 0);
			int num = 0;
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, this._cpPara, LogicalDirection.Forward);
				ITextPointer textPointerFromCP2 = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, this._cpPara + dcp, LogicalDirection.Forward);
				while (textPointerFromCP2.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text && textPointerFromCP2.CompareTo(textPointerFromCP) != 0)
				{
					textPointerFromCP2.MoveByOffset(-1);
					num++;
				}
				string textInRun = textPointerFromCP2.GetTextInRun(LogicalDirection.Backward);
				empty = new CharacterBufferRange(textInRun, 0, textInRun.Length);
				StaticTextPointer staticTextPointer = textPointerFromCP2.CreateStaticPointer();
				DependencyObject element = (staticTextPointer.Parent != null) ? staticTextPointer.Parent : this._paraClient.Paragraph.Element;
				culture = DynamicPropertyReader.GetCultureInfo(element);
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(num + empty.Length, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x060068ED RID: 26861 RVA: 0x001D9A5C File Offset: 0x001D7C5C
		internal override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp)
		{
			ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, this._cpPara + dcp, LogicalDirection.Forward);
			return textPointerFromCP.TextContainer.Start.GetOffsetToPosition(textPointerFromCP);
		}

		// Token: 0x060068EE RID: 26862 RVA: 0x001D9AA0 File Offset: 0x001D7CA0
		internal PTS.FSFLRES GetFormatResultForBreakpoint(int dcp, TextBreakpoint textBreakpoint)
		{
			int num = 0;
			PTS.FSFLRES result = PTS.FSFLRES.fsflrOutOfSpace;
			foreach (TextSpan<TextRun> textSpan in this._runCache.GetTextRunSpans())
			{
				TextRun value = textSpan.Value;
				if (value != null && num + value.Length >= dcp + textBreakpoint.Length)
				{
					if (value is ParagraphBreakRun)
					{
						result = ((ParagraphBreakRun)value).BreakReason;
						break;
					}
					if (value is LineBreakRun)
					{
						result = ((LineBreakRun)value).BreakReason;
						break;
					}
					break;
				}
				else
				{
					num += textSpan.Length;
				}
			}
			return result;
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x001D9B48 File Offset: 0x001D7D48
		internal Size MeasureChild(InlineObjectRun inlineObject)
		{
			double height = this._paraClient.Paragraph.StructuralCache.CurrentFormatContext.DocumentPageSize.Height;
			if (!this._paraClient.Paragraph.StructuralCache.CurrentFormatContext.FinitePage)
			{
				height = double.PositiveInfinity;
			}
			return inlineObject.UIElementIsland.DoLayout(new Size(TextDpi.FromTextDpi(this._durTrack), height), true, true);
		}

		// Token: 0x040033FA RID: 13306
		private readonly TextFormatterHost _host;

		// Token: 0x040033FB RID: 13307
		private TextRunCache _runCache;

		// Token: 0x040033FC RID: 13308
		private int _durTrack;

		// Token: 0x040033FD RID: 13309
		private int _cpPara;
	}
}
