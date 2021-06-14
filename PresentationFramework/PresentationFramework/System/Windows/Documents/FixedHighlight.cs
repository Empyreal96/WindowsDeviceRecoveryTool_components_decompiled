using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace System.Windows.Documents
{
	// Token: 0x0200034F RID: 847
	internal sealed class FixedHighlight
	{
		// Token: 0x06002D39 RID: 11577 RVA: 0x000CC3D3 File Offset: 0x000CA5D3
		internal FixedHighlight(UIElement element, int beginOffset, int endOffset, FixedHighlightType t, Brush foreground, Brush background)
		{
			this._element = element;
			this._gBeginOffset = beginOffset;
			this._gEndOffset = endOffset;
			this._type = t;
			this._foregroundBrush = foreground;
			this._backgroundBrush = background;
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000CC408 File Offset: 0x000CA608
		public override bool Equals(object oCompare)
		{
			FixedHighlight fixedHighlight = oCompare as FixedHighlight;
			return fixedHighlight != null && (fixedHighlight._element == this._element && fixedHighlight._gBeginOffset == this._gBeginOffset && fixedHighlight._gEndOffset == this._gEndOffset) && fixedHighlight._type == this._type;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000CC45B File Offset: 0x000CA65B
		public override int GetHashCode()
		{
			if (this._element != null)
			{
				return (int)(this._element.GetHashCode() + this._gBeginOffset + this._gEndOffset + this._type);
			}
			return 0;
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000CC488 File Offset: 0x000CA688
		internal Rect ComputeDesignRect()
		{
			Glyphs glyphs = this._element as Glyphs;
			if (glyphs == null)
			{
				Image image = this._element as Image;
				if (image != null && image.Source != null)
				{
					return new Rect(0.0, 0.0, image.Width, image.Height);
				}
				Path path = this._element as Path;
				if (path != null)
				{
					return path.Data.Bounds;
				}
				return Rect.Empty;
			}
			else
			{
				GlyphRun measurementGlyphRun = glyphs.MeasurementGlyphRun;
				if (measurementGlyphRun == null || this._gBeginOffset >= this._gEndOffset)
				{
					return Rect.Empty;
				}
				Rect result = measurementGlyphRun.ComputeAlignmentBox();
				result.Offset(glyphs.OriginX, glyphs.OriginY);
				int num = (measurementGlyphRun.Characters == null) ? 0 : measurementGlyphRun.Characters.Count;
				double num2 = measurementGlyphRun.GetDistanceFromCaretCharacterHit(new CharacterHit(this._gBeginOffset, 0));
				double num3;
				if (this._gEndOffset == num)
				{
					num3 = measurementGlyphRun.GetDistanceFromCaretCharacterHit(new CharacterHit(num - 1, 1));
				}
				else
				{
					num3 = measurementGlyphRun.GetDistanceFromCaretCharacterHit(new CharacterHit(this._gEndOffset, 0));
				}
				if (num3 < num2)
				{
					double num4 = num2;
					num2 = num3;
					num3 = num4;
				}
				double width = num3 - num2;
				if ((measurementGlyphRun.BidiLevel & 1) != 0)
				{
					result.X = glyphs.OriginX - num3;
				}
				else
				{
					result.X = glyphs.OriginX + num2;
				}
				result.Width = width;
				return result;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06002D3D RID: 11581 RVA: 0x000CC5EA File Offset: 0x000CA7EA
		internal FixedHighlightType HighlightType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06002D3E RID: 11582 RVA: 0x000CC5F2 File Offset: 0x000CA7F2
		internal Glyphs Glyphs
		{
			get
			{
				return this._element as Glyphs;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000CC5FF File Offset: 0x000CA7FF
		internal UIElement Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06002D40 RID: 11584 RVA: 0x000CC607 File Offset: 0x000CA807
		internal Brush ForegroundBrush
		{
			get
			{
				return this._foregroundBrush;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x000CC60F File Offset: 0x000CA80F
		internal Brush BackgroundBrush
		{
			get
			{
				return this._backgroundBrush;
			}
		}

		// Token: 0x04001D99 RID: 7577
		private readonly UIElement _element;

		// Token: 0x04001D9A RID: 7578
		private readonly int _gBeginOffset;

		// Token: 0x04001D9B RID: 7579
		private readonly int _gEndOffset;

		// Token: 0x04001D9C RID: 7580
		private readonly FixedHighlightType _type;

		// Token: 0x04001D9D RID: 7581
		private readonly Brush _backgroundBrush;

		// Token: 0x04001D9E RID: 7582
		private readonly Brush _foregroundBrush;
	}
}
