using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000367 RID: 871
	internal sealed class FixedSOMTextRun : FixedSOMElement, IComparable
	{
		// Token: 0x06002E36 RID: 11830 RVA: 0x000D0DC8 File Offset: 0x000CEFC8
		private FixedSOMTextRun(Rect boundingRect, GeneralTransform trans, FixedNode fixedNode, int startIndex, int endIndex) : base(fixedNode, startIndex, endIndex, trans)
		{
			this._boundingRect = trans.TransformBounds(boundingRect);
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x000D0DE4 File Offset: 0x000CEFE4
		int IComparable.CompareTo(object comparedObj)
		{
			FixedSOMTextRun fixedSOMTextRun = comparedObj as FixedSOMTextRun;
			int result;
			if (this._fixedBlock.IsRTL)
			{
				Rect boundingRect = base.BoundingRect;
				Rect boundingRect2 = fixedSOMTextRun.BoundingRect;
				if (!base.Matrix.IsIdentity)
				{
					Matrix mat = this._mat;
					mat.Invert();
					boundingRect.Transform(mat);
					boundingRect.Offset(this._mat.OffsetX, this._mat.OffsetY);
					boundingRect2.Transform(mat);
					boundingRect2.Offset(this._mat.OffsetX, this._mat.OffsetY);
				}
				boundingRect.Offset(this._mat.OffsetX, this._mat.OffsetY);
				boundingRect2.Offset(fixedSOMTextRun.Matrix.OffsetX, fixedSOMTextRun.Matrix.OffsetY);
				if (FixedTextBuilder.IsSameLine(boundingRect2.Top - boundingRect.Top, boundingRect.Height, boundingRect2.Height))
				{
					result = ((boundingRect.Left < boundingRect2.Left) ? 1 : -1);
				}
				else
				{
					result = ((boundingRect.Top < boundingRect2.Top) ? -1 : 1);
				}
			}
			else
			{
				List<FixedNode> markupOrder = this.FixedBlock.FixedSOMPage.MarkupOrder;
				result = markupOrder.IndexOf(base.FixedNode) - markupOrder.IndexOf(fixedSOMTextRun.FixedNode);
			}
			return result;
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000D0F48 File Offset: 0x000CF148
		public static FixedSOMTextRun Create(Rect boundingRect, GeneralTransform transform, Glyphs glyphs, FixedNode fixedNode, int startIndex, int endIndex, bool allowReverseGlyphs)
		{
			if (string.IsNullOrEmpty(glyphs.UnicodeString) || glyphs.FontRenderingEmSize <= 0.0)
			{
				return null;
			}
			FixedSOMTextRun fixedSOMTextRun = new FixedSOMTextRun(boundingRect, transform, fixedNode, startIndex, endIndex);
			fixedSOMTextRun._fontUri = glyphs.FontUri;
			fixedSOMTextRun._cultureInfo = glyphs.Language.GetCompatibleCulture();
			fixedSOMTextRun._bidiLevel = glyphs.BidiLevel;
			fixedSOMTextRun._isSideways = glyphs.IsSideways;
			fixedSOMTextRun._fontSize = glyphs.FontRenderingEmSize;
			GlyphRun glyphRun = glyphs.ToGlyphRun();
			GlyphTypeface glyphTypeface = glyphRun.GlyphTypeface;
			glyphTypeface.FamilyNames.TryGetValue(fixedSOMTextRun._cultureInfo, out fixedSOMTextRun._fontFamily);
			if (fixedSOMTextRun._fontFamily == null)
			{
				glyphTypeface.FamilyNames.TryGetValue(TypeConverterHelper.InvariantEnglishUS, out fixedSOMTextRun._fontFamily);
			}
			fixedSOMTextRun._fontStyle = glyphTypeface.Style;
			fixedSOMTextRun._fontWeight = glyphTypeface.Weight;
			fixedSOMTextRun._fontStretch = glyphTypeface.Stretch;
			fixedSOMTextRun._defaultCharWidth = ((glyphTypeface.XHeight > 0.0) ? (glyphTypeface.XHeight * glyphs.FontRenderingEmSize) : glyphRun.AdvanceWidths[startIndex]);
			Transform affineTransform = transform.AffineTransform;
			if (affineTransform != null && !affineTransform.Value.IsIdentity)
			{
				Matrix value = affineTransform.Value;
				double num = Math.Sqrt(value.M12 * value.M12 + value.M22 * value.M22);
				double num2 = Math.Sqrt(value.M11 * value.M11 + value.M21 * value.M21);
				fixedSOMTextRun._fontSize *= num;
				fixedSOMTextRun._defaultCharWidth *= num2;
			}
			fixedSOMTextRun._foreground = glyphs.Fill;
			string unicodeString = glyphs.UnicodeString;
			fixedSOMTextRun.Text = unicodeString.Substring(startIndex, endIndex - startIndex);
			if (allowReverseGlyphs && fixedSOMTextRun._bidiLevel == 0 && !fixedSOMTextRun._isSideways && startIndex == 0 && endIndex == unicodeString.Length && string.IsNullOrEmpty(glyphs.CaretStops) && FixedTextBuilder.MostlyRTL(unicodeString))
			{
				char[] array = new char[fixedSOMTextRun.Text.Length];
				for (int i = 0; i < fixedSOMTextRun.Text.Length; i++)
				{
					array[i] = fixedSOMTextRun.Text[fixedSOMTextRun.Text.Length - 1 - i];
				}
				fixedSOMTextRun._isReversed = true;
				fixedSOMTextRun.Text = new string(array);
			}
			if (unicodeString == "" && glyphs.Indices != null && glyphs.Indices.Length > 0)
			{
				fixedSOMTextRun._isWhiteSpace = false;
			}
			else
			{
				fixedSOMTextRun._isWhiteSpace = true;
				for (int j = 0; j < unicodeString.Length; j++)
				{
					if (!char.IsWhiteSpace(unicodeString[j]))
					{
						fixedSOMTextRun._isWhiteSpace = false;
						break;
					}
				}
			}
			return fixedSOMTextRun;
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x000D1220 File Offset: 0x000CF420
		public bool HasSameRichProperties(FixedSOMTextRun run)
		{
			if (run.FontRenderingEmSize == this.FontRenderingEmSize && run.CultureInfo.Equals(this.CultureInfo) && run.FontStyle.Equals(this.FontStyle) && run.FontStretch.Equals(this.FontStretch) && run.FontWeight.Equals(this.FontWeight) && run.FontFamily == this.FontFamily && run.IsRTL == this.IsRTL)
			{
				SolidColorBrush solidColorBrush = this.Foreground as SolidColorBrush;
				SolidColorBrush solidColorBrush2 = run.Foreground as SolidColorBrush;
				if ((run.Foreground == null && this.Foreground == null) || (solidColorBrush != null && solidColorBrush2 != null && solidColorBrush.Color == solidColorBrush2.Color && solidColorBrush.Opacity == solidColorBrush2.Opacity))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000D1314 File Offset: 0x000CF514
		public override void SetRTFProperties(FixedElement element)
		{
			if (this._cultureInfo != null)
			{
				element.SetValue(FrameworkElement.LanguageProperty, XmlLanguage.GetLanguage(this._cultureInfo.IetfLanguageTag));
			}
			element.SetValue(TextElement.FontSizeProperty, this._fontSize);
			element.SetValue(TextElement.FontWeightProperty, this._fontWeight);
			element.SetValue(TextElement.FontStretchProperty, this._fontStretch);
			element.SetValue(TextElement.FontStyleProperty, this._fontStyle);
			if (this.IsRTL)
			{
				element.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft);
			}
			else
			{
				element.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
			}
			if (this._fontFamily != null)
			{
				element.SetValue(TextElement.FontFamilyProperty, new FontFamily(this._fontFamily));
			}
			element.SetValue(TextElement.ForegroundProperty, this._foreground);
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06002E3B RID: 11835 RVA: 0x000D13F7 File Offset: 0x000CF5F7
		public double DefaultCharWidth
		{
			get
			{
				return this._defaultCharWidth;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06002E3C RID: 11836 RVA: 0x000D13FF File Offset: 0x000CF5FF
		public bool IsSideways
		{
			get
			{
				return this._isSideways;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06002E3D RID: 11837 RVA: 0x000D1407 File Offset: 0x000CF607
		public bool IsWhiteSpace
		{
			get
			{
				return this._isWhiteSpace;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06002E3E RID: 11838 RVA: 0x000D140F File Offset: 0x000CF60F
		public CultureInfo CultureInfo
		{
			get
			{
				return this._cultureInfo;
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06002E3F RID: 11839 RVA: 0x000D1417 File Offset: 0x000CF617
		public bool IsLTR
		{
			get
			{
				return (this._bidiLevel & 1) == 0 && !this._isReversed;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06002E40 RID: 11840 RVA: 0x000D142E File Offset: 0x000CF62E
		public bool IsRTL
		{
			get
			{
				return !this.IsLTR;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06002E41 RID: 11841 RVA: 0x000D1439 File Offset: 0x000CF639
		// (set) Token: 0x06002E42 RID: 11842 RVA: 0x000D1441 File Offset: 0x000CF641
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06002E43 RID: 11843 RVA: 0x000D144A File Offset: 0x000CF64A
		// (set) Token: 0x06002E44 RID: 11844 RVA: 0x000D1452 File Offset: 0x000CF652
		public FixedSOMFixedBlock FixedBlock
		{
			get
			{
				return this._fixedBlock;
			}
			set
			{
				this._fixedBlock = value;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06002E45 RID: 11845 RVA: 0x000D145B File Offset: 0x000CF65B
		public string FontFamily
		{
			get
			{
				return this._fontFamily;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06002E46 RID: 11846 RVA: 0x000D1463 File Offset: 0x000CF663
		public FontStyle FontStyle
		{
			get
			{
				return this._fontStyle;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06002E47 RID: 11847 RVA: 0x000D146B File Offset: 0x000CF66B
		public FontWeight FontWeight
		{
			get
			{
				return this._fontWeight;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06002E48 RID: 11848 RVA: 0x000D1473 File Offset: 0x000CF673
		public FontStretch FontStretch
		{
			get
			{
				return this._fontStretch;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06002E49 RID: 11849 RVA: 0x000D147B File Offset: 0x000CF67B
		public double FontRenderingEmSize
		{
			get
			{
				return this._fontSize;
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06002E4A RID: 11850 RVA: 0x000D1483 File Offset: 0x000CF683
		public Brush Foreground
		{
			get
			{
				return this._foreground;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06002E4B RID: 11851 RVA: 0x000D148B File Offset: 0x000CF68B
		public bool IsReversed
		{
			get
			{
				return this._isReversed;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06002E4C RID: 11852 RVA: 0x000D1493 File Offset: 0x000CF693
		// (set) Token: 0x06002E4D RID: 11853 RVA: 0x000D149B File Offset: 0x000CF69B
		internal int LineIndex
		{
			get
			{
				return this._lineIndex;
			}
			set
			{
				this._lineIndex = value;
			}
		}

		// Token: 0x04001DF2 RID: 7666
		private double _defaultCharWidth;

		// Token: 0x04001DF3 RID: 7667
		private Uri _fontUri;

		// Token: 0x04001DF4 RID: 7668
		private CultureInfo _cultureInfo;

		// Token: 0x04001DF5 RID: 7669
		private bool _isSideways;

		// Token: 0x04001DF6 RID: 7670
		private int _bidiLevel;

		// Token: 0x04001DF7 RID: 7671
		private bool _isWhiteSpace;

		// Token: 0x04001DF8 RID: 7672
		private bool _isReversed;

		// Token: 0x04001DF9 RID: 7673
		private FixedSOMFixedBlock _fixedBlock;

		// Token: 0x04001DFA RID: 7674
		private int _lineIndex;

		// Token: 0x04001DFB RID: 7675
		private string _text;

		// Token: 0x04001DFC RID: 7676
		private Brush _foreground;

		// Token: 0x04001DFD RID: 7677
		private double _fontSize;

		// Token: 0x04001DFE RID: 7678
		private string _fontFamily;

		// Token: 0x04001DFF RID: 7679
		private FontStyle _fontStyle;

		// Token: 0x04001E00 RID: 7680
		private FontWeight _fontWeight;

		// Token: 0x04001E01 RID: 7681
		private FontStretch _fontStretch;
	}
}
