using System;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000637 RID: 1591
	internal class ParagraphVisual : DrawingVisual
	{
		// Token: 0x06006900 RID: 26880 RVA: 0x001D9EA8 File Offset: 0x001D80A8
		internal ParagraphVisual()
		{
			this._renderBounds = Rect.Empty;
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x001D9EBC File Offset: 0x001D80BC
		internal void DrawBackgroundAndBorder(Brush backgroundBrush, Brush borderBrush, Thickness borderThickness, Rect renderBounds, bool isFirstChunk, bool isLastChunk)
		{
			if (this._backgroundBrush != backgroundBrush || this._renderBounds != renderBounds || this._borderBrush != borderBrush || !Thickness.AreClose(this._borderThickness, borderThickness))
			{
				using (DrawingContext drawingContext = base.RenderOpen())
				{
					this.DrawBackgroundAndBorderIntoContext(drawingContext, backgroundBrush, borderBrush, borderThickness, renderBounds, isFirstChunk, isLastChunk);
				}
			}
		}

		// Token: 0x06006902 RID: 26882 RVA: 0x001D9F2C File Offset: 0x001D812C
		internal void DrawBackgroundAndBorderIntoContext(DrawingContext dc, Brush backgroundBrush, Brush borderBrush, Thickness borderThickness, Rect renderBounds, bool isFirstChunk, bool isLastChunk)
		{
			this._backgroundBrush = (Brush)FreezableOperations.GetAsFrozenIfPossible(backgroundBrush);
			this._renderBounds = renderBounds;
			this._borderBrush = (Brush)FreezableOperations.GetAsFrozenIfPossible(borderBrush);
			this._borderThickness = borderThickness;
			if (!isFirstChunk)
			{
				this._borderThickness.Top = 0.0;
			}
			if (!isLastChunk)
			{
				this._borderThickness.Bottom = 0.0;
			}
			if (this._borderBrush != null)
			{
				Pen pen = new Pen();
				pen.Brush = this._borderBrush;
				pen.Thickness = this._borderThickness.Left;
				if (pen.CanFreeze)
				{
					pen.Freeze();
				}
				if (this._borderThickness.IsUniform)
				{
					dc.DrawRectangle(null, pen, new Rect(new Point(this._renderBounds.Left + pen.Thickness * 0.5, this._renderBounds.Bottom - pen.Thickness * 0.5), new Point(this._renderBounds.Right - pen.Thickness * 0.5, this._renderBounds.Top + pen.Thickness * 0.5)));
				}
				else
				{
					if (DoubleUtil.GreaterThan(this._borderThickness.Left, 0.0))
					{
						dc.DrawLine(pen, new Point(this._renderBounds.Left + pen.Thickness / 2.0, this._renderBounds.Top), new Point(this._renderBounds.Left + pen.Thickness / 2.0, this._renderBounds.Bottom));
					}
					if (DoubleUtil.GreaterThan(this._borderThickness.Right, 0.0))
					{
						pen = new Pen();
						pen.Brush = this._borderBrush;
						pen.Thickness = this._borderThickness.Right;
						if (pen.CanFreeze)
						{
							pen.Freeze();
						}
						dc.DrawLine(pen, new Point(this._renderBounds.Right - pen.Thickness / 2.0, this._renderBounds.Top), new Point(this._renderBounds.Right - pen.Thickness / 2.0, this._renderBounds.Bottom));
					}
					if (DoubleUtil.GreaterThan(this._borderThickness.Top, 0.0))
					{
						pen = new Pen();
						pen.Brush = this._borderBrush;
						pen.Thickness = this._borderThickness.Top;
						if (pen.CanFreeze)
						{
							pen.Freeze();
						}
						dc.DrawLine(pen, new Point(this._renderBounds.Left, this._renderBounds.Top + pen.Thickness / 2.0), new Point(this._renderBounds.Right, this._renderBounds.Top + pen.Thickness / 2.0));
					}
					if (DoubleUtil.GreaterThan(this._borderThickness.Bottom, 0.0))
					{
						pen = new Pen();
						pen.Brush = this._borderBrush;
						pen.Thickness = this._borderThickness.Bottom;
						if (pen.CanFreeze)
						{
							pen.Freeze();
						}
						dc.DrawLine(pen, new Point(this._renderBounds.Left, this._renderBounds.Bottom - pen.Thickness / 2.0), new Point(this._renderBounds.Right, this._renderBounds.Bottom - pen.Thickness / 2.0));
					}
				}
			}
			if (this._backgroundBrush != null)
			{
				dc.DrawRectangle(this._backgroundBrush, null, new Rect(new Point(this._renderBounds.Left + this._borderThickness.Left, this._renderBounds.Top + this._borderThickness.Top), new Point(this._renderBounds.Right - this._borderThickness.Right, this._renderBounds.Bottom - this._borderThickness.Bottom)));
			}
		}

		// Token: 0x04003405 RID: 13317
		private Brush _backgroundBrush;

		// Token: 0x04003406 RID: 13318
		private Brush _borderBrush;

		// Token: 0x04003407 RID: 13319
		private Thickness _borderThickness;

		// Token: 0x04003408 RID: 13320
		private Rect _renderBounds;
	}
}
