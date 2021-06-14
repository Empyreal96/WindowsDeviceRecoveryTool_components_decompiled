using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost;

namespace MS.Internal.Text
{
	// Token: 0x02000602 RID: 1538
	internal class LineProperties : TextParagraphProperties
	{
		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x0600665B RID: 26203 RVA: 0x001CC2F9 File Offset: 0x001CA4F9
		public override FlowDirection FlowDirection
		{
			get
			{
				return this._flowDirection;
			}
		}

		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x0600665C RID: 26204 RVA: 0x001CC301 File Offset: 0x001CA501
		public override TextAlignment TextAlignment
		{
			get
			{
				if (!this.IgnoreTextAlignment)
				{
					return this._textAlignment;
				}
				return TextAlignment.Left;
			}
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x0600665D RID: 26205 RVA: 0x001CC313 File Offset: 0x001CA513
		public override double LineHeight
		{
			get
			{
				if (this.LineStackingStrategy == LineStackingStrategy.BlockLineHeight && !double.IsNaN(this._lineHeight))
				{
					return this._lineHeight;
				}
				return 0.0;
			}
		}

		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x0600665E RID: 26206 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool FirstLineInParagraph
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x0600665F RID: 26207 RVA: 0x001CC33A File Offset: 0x001CA53A
		public override TextRunProperties DefaultTextRunProperties
		{
			get
			{
				return this._defaultTextProperties;
			}
		}

		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x06006660 RID: 26208 RVA: 0x001CC342 File Offset: 0x001CA542
		public override TextDecorationCollection TextDecorations
		{
			get
			{
				return this._defaultTextProperties.TextDecorations;
			}
		}

		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x06006661 RID: 26209 RVA: 0x001CC34F File Offset: 0x001CA54F
		public override TextWrapping TextWrapping
		{
			get
			{
				return this._textWrapping;
			}
		}

		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x06006662 RID: 26210 RVA: 0x001CC357 File Offset: 0x001CA557
		public override TextMarkerProperties TextMarkerProperties
		{
			get
			{
				return this._markerProperties;
			}
		}

		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x06006663 RID: 26211 RVA: 0x0018D6AE File Offset: 0x0018B8AE
		public override double Indent
		{
			get
			{
				return 0.0;
			}
		}

		// Token: 0x06006664 RID: 26212 RVA: 0x001CC35F File Offset: 0x001CA55F
		internal LineProperties(DependencyObject element, DependencyObject contentHost, TextProperties defaultTextProperties, MarkerProperties markerProperties) : this(element, contentHost, defaultTextProperties, markerProperties, (TextAlignment)element.GetValue(Block.TextAlignmentProperty))
		{
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x001CC37C File Offset: 0x001CA57C
		internal LineProperties(DependencyObject element, DependencyObject contentHost, TextProperties defaultTextProperties, MarkerProperties markerProperties, TextAlignment textAlignment)
		{
			this._defaultTextProperties = defaultTextProperties;
			this._markerProperties = ((markerProperties != null) ? markerProperties.GetTextMarkerProperties(this) : null);
			this._flowDirection = (FlowDirection)element.GetValue(Block.FlowDirectionProperty);
			this._textAlignment = textAlignment;
			this._lineHeight = (double)element.GetValue(Block.LineHeightProperty);
			this._textIndent = (double)element.GetValue(Paragraph.TextIndentProperty);
			this._lineStackingStrategy = (LineStackingStrategy)element.GetValue(Block.LineStackingStrategyProperty);
			this._textWrapping = TextWrapping.Wrap;
			this._textTrimming = TextTrimming.None;
			if (contentHost is TextBlock || contentHost is ITextBoxViewHost)
			{
				this._textWrapping = (TextWrapping)contentHost.GetValue(TextBlock.TextWrappingProperty);
				this._textTrimming = (TextTrimming)contentHost.GetValue(TextBlock.TextTrimmingProperty);
				return;
			}
			if (contentHost is FlowDocument)
			{
				this._textWrapping = ((FlowDocument)contentHost).TextWrapping;
			}
		}

		// Token: 0x06006666 RID: 26214 RVA: 0x001CC470 File Offset: 0x001CA670
		internal double CalcLineAdvanceForTextParagraph(TextParagraph textParagraph, int dcp, double lineAdvance)
		{
			if (!DoubleUtil.IsNaN(this._lineHeight))
			{
				LineStackingStrategy lineStackingStrategy = this.LineStackingStrategy;
				if (lineStackingStrategy != LineStackingStrategy.BlockLineHeight)
				{
					if (lineStackingStrategy != LineStackingStrategy.MaxHeight)
					{
					}
					if (dcp == 0 && textParagraph.HasFiguresOrFloaters() && textParagraph.GetLastDcpAttachedObjectBeforeLine(0) + textParagraph.ParagraphStartCharacterPosition == textParagraph.ParagraphEndCharacterPosition)
					{
						lineAdvance = this._lineHeight;
					}
					else
					{
						lineAdvance = Math.Max(lineAdvance, this._lineHeight);
					}
				}
				else
				{
					lineAdvance = this._lineHeight;
				}
			}
			return lineAdvance;
		}

		// Token: 0x06006667 RID: 26215 RVA: 0x001CC4E0 File Offset: 0x001CA6E0
		internal double CalcLineAdvance(double lineAdvance)
		{
			if (!DoubleUtil.IsNaN(this._lineHeight))
			{
				LineStackingStrategy lineStackingStrategy = this.LineStackingStrategy;
				if (lineStackingStrategy != LineStackingStrategy.BlockLineHeight)
				{
					if (lineStackingStrategy != LineStackingStrategy.MaxHeight)
					{
					}
					lineAdvance = Math.Max(lineAdvance, this._lineHeight);
				}
				else
				{
					lineAdvance = this._lineHeight;
				}
			}
			return lineAdvance;
		}

		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06006668 RID: 26216 RVA: 0x001CC523 File Offset: 0x001CA723
		internal TextAlignment TextAlignmentInternal
		{
			get
			{
				return this._textAlignment;
			}
		}

		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x06006669 RID: 26217 RVA: 0x001CC52B File Offset: 0x001CA72B
		// (set) Token: 0x0600666A RID: 26218 RVA: 0x001CC533 File Offset: 0x001CA733
		internal bool IgnoreTextAlignment
		{
			get
			{
				return this._ignoreTextAlignment;
			}
			set
			{
				this._ignoreTextAlignment = value;
			}
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x0600666B RID: 26219 RVA: 0x001CC53C File Offset: 0x001CA73C
		internal LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return this._lineStackingStrategy;
			}
		}

		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x0600666C RID: 26220 RVA: 0x001CC544 File Offset: 0x001CA744
		internal TextTrimming TextTrimming
		{
			get
			{
				return this._textTrimming;
			}
		}

		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x0600666D RID: 26221 RVA: 0x001CC54C File Offset: 0x001CA74C
		internal bool HasFirstLineProperties
		{
			get
			{
				return this._markerProperties != null || !DoubleUtil.IsZero(this._textIndent);
			}
		}

		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x0600666E RID: 26222 RVA: 0x001CC566 File Offset: 0x001CA766
		internal TextParagraphProperties FirstLineProps
		{
			get
			{
				if (this._firstLineProperties == null)
				{
					this._firstLineProperties = new LineProperties.FirstLineProperties(this);
				}
				return this._firstLineProperties;
			}
		}

		// Token: 0x0600666F RID: 26223 RVA: 0x001CC582 File Offset: 0x001CA782
		internal TextParagraphProperties GetParaEllipsisLineProps(bool firstLine)
		{
			return new LineProperties.ParaEllipsisLineProperties(firstLine ? this.FirstLineProps : this);
		}

		// Token: 0x0400330A RID: 13066
		private TextRunProperties _defaultTextProperties;

		// Token: 0x0400330B RID: 13067
		private TextMarkerProperties _markerProperties;

		// Token: 0x0400330C RID: 13068
		private LineProperties.FirstLineProperties _firstLineProperties;

		// Token: 0x0400330D RID: 13069
		private bool _ignoreTextAlignment;

		// Token: 0x0400330E RID: 13070
		private FlowDirection _flowDirection;

		// Token: 0x0400330F RID: 13071
		private TextAlignment _textAlignment;

		// Token: 0x04003310 RID: 13072
		private TextWrapping _textWrapping;

		// Token: 0x04003311 RID: 13073
		private TextTrimming _textTrimming;

		// Token: 0x04003312 RID: 13074
		private double _lineHeight;

		// Token: 0x04003313 RID: 13075
		private double _textIndent;

		// Token: 0x04003314 RID: 13076
		private LineStackingStrategy _lineStackingStrategy;

		// Token: 0x02000A18 RID: 2584
		private sealed class FirstLineProperties : TextParagraphProperties
		{
			// Token: 0x17001F3B RID: 7995
			// (get) Token: 0x06008A94 RID: 35476 RVA: 0x002577DE File Offset: 0x002559DE
			public override FlowDirection FlowDirection
			{
				get
				{
					return this._lp.FlowDirection;
				}
			}

			// Token: 0x17001F3C RID: 7996
			// (get) Token: 0x06008A95 RID: 35477 RVA: 0x002577EB File Offset: 0x002559EB
			public override TextAlignment TextAlignment
			{
				get
				{
					return this._lp.TextAlignment;
				}
			}

			// Token: 0x17001F3D RID: 7997
			// (get) Token: 0x06008A96 RID: 35478 RVA: 0x002577F8 File Offset: 0x002559F8
			public override double LineHeight
			{
				get
				{
					return this._lp.LineHeight;
				}
			}

			// Token: 0x17001F3E RID: 7998
			// (get) Token: 0x06008A97 RID: 35479 RVA: 0x00016748 File Offset: 0x00014948
			public override bool FirstLineInParagraph
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001F3F RID: 7999
			// (get) Token: 0x06008A98 RID: 35480 RVA: 0x00257805 File Offset: 0x00255A05
			public override TextRunProperties DefaultTextRunProperties
			{
				get
				{
					return this._lp.DefaultTextRunProperties;
				}
			}

			// Token: 0x17001F40 RID: 8000
			// (get) Token: 0x06008A99 RID: 35481 RVA: 0x00257812 File Offset: 0x00255A12
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._lp.TextDecorations;
				}
			}

			// Token: 0x17001F41 RID: 8001
			// (get) Token: 0x06008A9A RID: 35482 RVA: 0x0025781F File Offset: 0x00255A1F
			public override TextWrapping TextWrapping
			{
				get
				{
					return this._lp.TextWrapping;
				}
			}

			// Token: 0x17001F42 RID: 8002
			// (get) Token: 0x06008A9B RID: 35483 RVA: 0x0025782C File Offset: 0x00255A2C
			public override TextMarkerProperties TextMarkerProperties
			{
				get
				{
					return this._lp.TextMarkerProperties;
				}
			}

			// Token: 0x17001F43 RID: 8003
			// (get) Token: 0x06008A9C RID: 35484 RVA: 0x00257839 File Offset: 0x00255A39
			public override double Indent
			{
				get
				{
					return this._lp._textIndent;
				}
			}

			// Token: 0x06008A9D RID: 35485 RVA: 0x00257846 File Offset: 0x00255A46
			internal FirstLineProperties(LineProperties lp)
			{
				this._lp = lp;
				this.Hyphenator = lp.Hyphenator;
			}

			// Token: 0x040046CE RID: 18126
			private LineProperties _lp;
		}

		// Token: 0x02000A19 RID: 2585
		private sealed class ParaEllipsisLineProperties : TextParagraphProperties
		{
			// Token: 0x17001F44 RID: 8004
			// (get) Token: 0x06008A9E RID: 35486 RVA: 0x00257861 File Offset: 0x00255A61
			public override FlowDirection FlowDirection
			{
				get
				{
					return this._lp.FlowDirection;
				}
			}

			// Token: 0x17001F45 RID: 8005
			// (get) Token: 0x06008A9F RID: 35487 RVA: 0x0025786E File Offset: 0x00255A6E
			public override TextAlignment TextAlignment
			{
				get
				{
					return this._lp.TextAlignment;
				}
			}

			// Token: 0x17001F46 RID: 8006
			// (get) Token: 0x06008AA0 RID: 35488 RVA: 0x0025787B File Offset: 0x00255A7B
			public override double LineHeight
			{
				get
				{
					return this._lp.LineHeight;
				}
			}

			// Token: 0x17001F47 RID: 8007
			// (get) Token: 0x06008AA1 RID: 35489 RVA: 0x00257888 File Offset: 0x00255A88
			public override bool FirstLineInParagraph
			{
				get
				{
					return this._lp.FirstLineInParagraph;
				}
			}

			// Token: 0x17001F48 RID: 8008
			// (get) Token: 0x06008AA2 RID: 35490 RVA: 0x00257895 File Offset: 0x00255A95
			public override bool AlwaysCollapsible
			{
				get
				{
					return this._lp.AlwaysCollapsible;
				}
			}

			// Token: 0x17001F49 RID: 8009
			// (get) Token: 0x06008AA3 RID: 35491 RVA: 0x002578A2 File Offset: 0x00255AA2
			public override TextRunProperties DefaultTextRunProperties
			{
				get
				{
					return this._lp.DefaultTextRunProperties;
				}
			}

			// Token: 0x17001F4A RID: 8010
			// (get) Token: 0x06008AA4 RID: 35492 RVA: 0x002578AF File Offset: 0x00255AAF
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._lp.TextDecorations;
				}
			}

			// Token: 0x17001F4B RID: 8011
			// (get) Token: 0x06008AA5 RID: 35493 RVA: 0x00016748 File Offset: 0x00014948
			public override TextWrapping TextWrapping
			{
				get
				{
					return TextWrapping.NoWrap;
				}
			}

			// Token: 0x17001F4C RID: 8012
			// (get) Token: 0x06008AA6 RID: 35494 RVA: 0x002578BC File Offset: 0x00255ABC
			public override TextMarkerProperties TextMarkerProperties
			{
				get
				{
					return this._lp.TextMarkerProperties;
				}
			}

			// Token: 0x17001F4D RID: 8013
			// (get) Token: 0x06008AA7 RID: 35495 RVA: 0x002578C9 File Offset: 0x00255AC9
			public override double Indent
			{
				get
				{
					return this._lp.Indent;
				}
			}

			// Token: 0x06008AA8 RID: 35496 RVA: 0x002578D6 File Offset: 0x00255AD6
			internal ParaEllipsisLineProperties(TextParagraphProperties lp)
			{
				this._lp = lp;
			}

			// Token: 0x040046CF RID: 18127
			private TextParagraphProperties _lp;
		}
	}
}
