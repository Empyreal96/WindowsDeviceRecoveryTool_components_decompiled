using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000607 RID: 1543
	internal sealed class TextProperties : TextRunProperties
	{
		// Token: 0x170018A5 RID: 6309
		// (get) Token: 0x06006697 RID: 26263 RVA: 0x001CCD37 File Offset: 0x001CAF37
		public override Typeface Typeface
		{
			get
			{
				return this._typeface;
			}
		}

		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x06006698 RID: 26264 RVA: 0x001CCD40 File Offset: 0x001CAF40
		public override double FontRenderingEmSize
		{
			get
			{
				double fontSize = this._fontSize;
				TextDpi.EnsureValidLineOffset(ref fontSize);
				return fontSize;
			}
		}

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x06006699 RID: 26265 RVA: 0x001CCD5C File Offset: 0x001CAF5C
		public override double FontHintingEmSize
		{
			get
			{
				return 12.0;
			}
		}

		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x0600669A RID: 26266 RVA: 0x001CCD67 File Offset: 0x001CAF67
		public override TextDecorationCollection TextDecorations
		{
			get
			{
				return this._textDecorations;
			}
		}

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x0600669B RID: 26267 RVA: 0x001CCD6F File Offset: 0x001CAF6F
		public override Brush ForegroundBrush
		{
			get
			{
				return this._foreground;
			}
		}

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x0600669C RID: 26268 RVA: 0x001CCD77 File Offset: 0x001CAF77
		public override Brush BackgroundBrush
		{
			get
			{
				return this._backgroundBrush;
			}
		}

		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x0600669D RID: 26269 RVA: 0x001CCD7F File Offset: 0x001CAF7F
		public override BaselineAlignment BaselineAlignment
		{
			get
			{
				return this._baselineAlignment;
			}
		}

		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x0600669E RID: 26270 RVA: 0x001CCD87 File Offset: 0x001CAF87
		public override CultureInfo CultureInfo
		{
			get
			{
				return this._cultureInfo;
			}
		}

		// Token: 0x170018AD RID: 6317
		// (get) Token: 0x0600669F RID: 26271 RVA: 0x001CCD8F File Offset: 0x001CAF8F
		public override NumberSubstitution NumberSubstitution
		{
			get
			{
				return this._numberSubstitution;
			}
		}

		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x060066A0 RID: 26272 RVA: 0x001CCD97 File Offset: 0x001CAF97
		public override TextRunTypographyProperties TypographyProperties
		{
			get
			{
				return this._typographyProperties;
			}
		}

		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x060066A1 RID: 26273 RVA: 0x001CCD9F File Offset: 0x001CAF9F
		public override TextEffectCollection TextEffects
		{
			get
			{
				return this._textEffects;
			}
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x001CCDA8 File Offset: 0x001CAFA8
		internal TextProperties(FrameworkElement target, bool isTypographyDefaultValue)
		{
			if (!target.HasNumberSubstitutionChanged)
			{
				this._numberSubstitution = FrameworkElement.DefaultNumberSubstitution;
			}
			base.PixelsPerDip = target.GetDpi().PixelsPerDip;
			this.InitCommon(target);
			if (!isTypographyDefaultValue)
			{
				this._typographyProperties = TextElement.GetTypographyProperties(target);
			}
			else
			{
				this._typographyProperties = Typography.Default;
			}
			this._baselineAlignment = BaselineAlignment.Baseline;
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x001CCE0C File Offset: 0x001CB00C
		internal TextProperties(DependencyObject target, StaticTextPointer position, bool inlineObjects, bool getBackground, double pixelsPerDip)
		{
			FrameworkContentElement frameworkContentElement = target as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				if (!frameworkContentElement.HasNumberSubstitutionChanged)
				{
					this._numberSubstitution = FrameworkContentElement.DefaultNumberSubstitution;
				}
			}
			else
			{
				FrameworkElement frameworkElement = target as FrameworkElement;
				if (frameworkElement != null && !frameworkElement.HasNumberSubstitutionChanged)
				{
					this._numberSubstitution = FrameworkElement.DefaultNumberSubstitution;
				}
			}
			base.PixelsPerDip = pixelsPerDip;
			this.InitCommon(target);
			this._typographyProperties = TextProperties.GetTypographyProperties(target);
			if (!inlineObjects)
			{
				this._baselineAlignment = DynamicPropertyReader.GetBaselineAlignment(target);
				if (!position.IsNull)
				{
					TextDecorationCollection highlightTextDecorations = TextProperties.GetHighlightTextDecorations(position);
					if (highlightTextDecorations != null)
					{
						this._textDecorations = highlightTextDecorations;
					}
				}
				if (getBackground)
				{
					this._backgroundBrush = DynamicPropertyReader.GetBackgroundBrush(target);
					return;
				}
			}
			else
			{
				this._baselineAlignment = DynamicPropertyReader.GetBaselineAlignmentForInlineObject(target);
				this._textDecorations = DynamicPropertyReader.GetTextDecorationsForInlineObject(target, this._textDecorations);
				if (getBackground)
				{
					this._backgroundBrush = DynamicPropertyReader.GetBackgroundBrushForInlineObject(position);
				}
			}
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x001CCEE0 File Offset: 0x001CB0E0
		internal TextProperties(TextProperties source, TextDecorationCollection textDecorations)
		{
			this._backgroundBrush = source._backgroundBrush;
			this._typeface = source._typeface;
			this._fontSize = source._fontSize;
			this._foreground = source._foreground;
			this._textEffects = source._textEffects;
			this._cultureInfo = source._cultureInfo;
			this._numberSubstitution = source._numberSubstitution;
			this._typographyProperties = source._typographyProperties;
			this._baselineAlignment = source._baselineAlignment;
			base.PixelsPerDip = source.PixelsPerDip;
			this._textDecorations = textDecorations;
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x001CCF74 File Offset: 0x001CB174
		private void InitCommon(DependencyObject target)
		{
			this._typeface = DynamicPropertyReader.GetTypeface(target);
			this._fontSize = (double)target.GetValue(TextElement.FontSizeProperty);
			this._foreground = (Brush)target.GetValue(TextElement.ForegroundProperty);
			this._textEffects = DynamicPropertyReader.GetTextEffects(target);
			this._cultureInfo = DynamicPropertyReader.GetCultureInfo(target);
			this._textDecorations = DynamicPropertyReader.GetTextDecorations(target);
			if (this._numberSubstitution == null)
			{
				this._numberSubstitution = DynamicPropertyReader.GetNumberSubstitution(target);
			}
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x001CCFF4 File Offset: 0x001CB1F4
		private static TextDecorationCollection GetHighlightTextDecorations(StaticTextPointer highlightPosition)
		{
			TextDecorationCollection result = null;
			Highlights highlights = highlightPosition.TextContainer.Highlights;
			if (highlights == null)
			{
				return result;
			}
			return highlights.GetHighlightValue(highlightPosition, LogicalDirection.Forward, typeof(SpellerHighlightLayer)) as TextDecorationCollection;
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x001CD030 File Offset: 0x001CB230
		private static TypographyProperties GetTypographyProperties(DependencyObject element)
		{
			TextBlock textBlock = element as TextBlock;
			if (textBlock != null)
			{
				if (!textBlock.IsTypographyDefaultValue)
				{
					return TextElement.GetTypographyProperties(element);
				}
				return Typography.Default;
			}
			else
			{
				TextBox textBox = element as TextBox;
				if (textBox != null)
				{
					if (!textBox.IsTypographyDefaultValue)
					{
						return TextElement.GetTypographyProperties(element);
					}
					return Typography.Default;
				}
				else
				{
					TextElement textElement = element as TextElement;
					if (textElement != null)
					{
						return textElement.TypographyPropertiesGroup;
					}
					FlowDocument flowDocument = element as FlowDocument;
					if (flowDocument != null)
					{
						return flowDocument.TypographyPropertiesGroup;
					}
					return Typography.Default;
				}
			}
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x001CD0A2 File Offset: 0x001CB2A2
		internal void SetBackgroundBrush(Brush backgroundBrush)
		{
			this._backgroundBrush = backgroundBrush;
		}

		// Token: 0x060066A9 RID: 26281 RVA: 0x001CD0AB File Offset: 0x001CB2AB
		internal void SetForegroundBrush(Brush foregroundBrush)
		{
			this._foreground = foregroundBrush;
		}

		// Token: 0x0400332A RID: 13098
		private Typeface _typeface;

		// Token: 0x0400332B RID: 13099
		private double _fontSize;

		// Token: 0x0400332C RID: 13100
		private Brush _foreground;

		// Token: 0x0400332D RID: 13101
		private TextEffectCollection _textEffects;

		// Token: 0x0400332E RID: 13102
		private TextDecorationCollection _textDecorations;

		// Token: 0x0400332F RID: 13103
		private BaselineAlignment _baselineAlignment;

		// Token: 0x04003330 RID: 13104
		private Brush _backgroundBrush;

		// Token: 0x04003331 RID: 13105
		private CultureInfo _cultureInfo;

		// Token: 0x04003332 RID: 13106
		private NumberSubstitution _numberSubstitution;

		// Token: 0x04003333 RID: 13107
		private TextRunTypographyProperties _typographyProperties;
	}
}
