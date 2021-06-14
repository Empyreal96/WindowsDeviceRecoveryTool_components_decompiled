using System;
using System.Collections;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000379 RID: 889
	internal sealed class HighlightVisual : Adorner
	{
		// Token: 0x06003013 RID: 12307 RVA: 0x000D8448 File Offset: 0x000D6648
		internal HighlightVisual(FixedDocument panel, FixedPage page) : base(page)
		{
			this._panel = panel;
			this._page = page;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x0000C238 File Offset: 0x0000A438
		protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
		{
			return null;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x0000C238 File Offset: 0x0000A438
		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			return null;
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000D8460 File Offset: 0x000D6660
		protected override void OnRender(DrawingContext dc)
		{
			if (this._panel.Highlights.ContainsKey(this._page))
			{
				ArrayList arrayList = this._panel.Highlights[this._page];
				Size size = this._panel.ComputePageSize(this._page);
				Rect rect = new Rect(new Point(0.0, 0.0), size);
				dc.PushClip(new RectangleGeometry(rect));
				if (arrayList != null)
				{
					this._UpdateHighlightBackground(dc, arrayList);
					this._UpdateHighlightForeground(dc, arrayList);
				}
				dc.Pop();
			}
			if (this._rubberbandSelector != null && this._rubberbandSelector.Page == this._page)
			{
				Rect selectionRect = this._rubberbandSelector.SelectionRect;
				if (!selectionRect.IsEmpty)
				{
					dc.DrawRectangle(SelectionHighlightInfo.ObjectMaskBrush, null, selectionRect);
				}
			}
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000D8530 File Offset: 0x000D6730
		internal void InvalidateHighlights()
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._page);
			if (adornerLayer == null)
			{
				return;
			}
			adornerLayer.Update(this._page);
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000D8559 File Offset: 0x000D6759
		internal void UpdateRubberbandSelection(RubberbandSelector selector)
		{
			this._rubberbandSelector = selector;
			this.InvalidateHighlights();
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x000D8568 File Offset: 0x000D6768
		internal static HighlightVisual GetHighlightVisual(FixedPage page)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(page);
			if (adornerLayer == null)
			{
				return null;
			}
			Adorner[] adorners = adornerLayer.GetAdorners(page);
			if (adorners != null)
			{
				foreach (Adorner adorner in adorners)
				{
					HighlightVisual highlightVisual = adorner as HighlightVisual;
					if (highlightVisual != null)
					{
						return highlightVisual;
					}
				}
			}
			return null;
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x000D85B4 File Offset: 0x000D67B4
		private void _UpdateHighlightBackground(DrawingContext dc, ArrayList highlights)
		{
			PathGeometry pathGeometry = null;
			Brush brush = null;
			Rect rect = Rect.Empty;
			foreach (object obj in highlights)
			{
				FixedHighlight fixedHighlight = (FixedHighlight)obj;
				Brush brush2 = null;
				if (fixedHighlight.HighlightType != FixedHighlightType.None)
				{
					Rect rect2 = fixedHighlight.ComputeDesignRect();
					if (!(rect2 == Rect.Empty))
					{
						GeneralTransform generalTransform = fixedHighlight.Element.TransformToAncestor(this._page);
						Transform transform = generalTransform.AffineTransform;
						if (transform == null)
						{
							transform = Transform.Identity;
						}
						Glyphs glyphs = fixedHighlight.Glyphs;
						if (fixedHighlight.HighlightType == FixedHighlightType.TextSelection)
						{
							brush2 = ((glyphs == null) ? SelectionHighlightInfo.ObjectMaskBrush : SelectionHighlightInfo.BackgroundBrush);
						}
						else if (fixedHighlight.HighlightType == FixedHighlightType.AnnotationHighlight)
						{
							brush2 = fixedHighlight.BackgroundBrush;
						}
						if (fixedHighlight.Element.Clip != null)
						{
							Rect bounds = fixedHighlight.Element.Clip.Bounds;
							rect2.Intersect(bounds);
						}
						Geometry geometry = new RectangleGeometry(rect2);
						geometry.Transform = transform;
						rect2 = generalTransform.TransformBounds(rect2);
						if (brush2 != brush || rect2.Top > rect.Bottom + 0.1 || rect2.Bottom + 0.1 < rect.Top || rect2.Left > rect.Right + 0.1 || rect2.Right + 0.1 < rect.Left)
						{
							if (brush != null)
							{
								pathGeometry.FillRule = FillRule.Nonzero;
								dc.DrawGeometry(brush, null, pathGeometry);
							}
							brush = brush2;
							pathGeometry = new PathGeometry();
							pathGeometry.AddGeometry(geometry);
							rect = rect2;
						}
						else
						{
							pathGeometry.AddGeometry(geometry);
							rect.Union(rect2);
						}
					}
				}
			}
			if (brush != null)
			{
				pathGeometry.FillRule = FillRule.Nonzero;
				dc.DrawGeometry(brush, null, pathGeometry);
			}
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x000D87B0 File Offset: 0x000D69B0
		private void _UpdateHighlightForeground(DrawingContext dc, ArrayList highlights)
		{
			foreach (object obj in highlights)
			{
				FixedHighlight fixedHighlight = (FixedHighlight)obj;
				Brush brush = null;
				if (fixedHighlight.HighlightType != FixedHighlightType.None)
				{
					Glyphs glyphs = fixedHighlight.Glyphs;
					if (glyphs != null)
					{
						Rect rect = fixedHighlight.ComputeDesignRect();
						if (!(rect == Rect.Empty))
						{
							GeneralTransform generalTransform = fixedHighlight.Element.TransformToAncestor(this._page);
							Transform affineTransform = generalTransform.AffineTransform;
							if (affineTransform != null)
							{
								dc.PushTransform(affineTransform);
							}
							else
							{
								dc.PushTransform(Transform.Identity);
							}
							dc.PushClip(new RectangleGeometry(rect));
							if (fixedHighlight.HighlightType == FixedHighlightType.TextSelection)
							{
								brush = SelectionHighlightInfo.ForegroundBrush;
							}
							else if (fixedHighlight.HighlightType == FixedHighlightType.AnnotationHighlight)
							{
								brush = fixedHighlight.ForegroundBrush;
							}
							GlyphRun glyphRun = glyphs.ToGlyphRun();
							if (brush == null)
							{
								brush = glyphs.Fill;
							}
							dc.PushGuidelineY1(glyphRun.BaselineOrigin.Y);
							dc.PushClip(glyphs.Clip);
							dc.DrawGlyphRun(brush, glyphRun);
							dc.Pop();
							dc.Pop();
							dc.Pop();
							dc.Pop();
						}
					}
				}
			}
		}

		// Token: 0x04001E65 RID: 7781
		private FixedDocument _panel;

		// Token: 0x04001E66 RID: 7782
		private RubberbandSelector _rubberbandSelector;

		// Token: 0x04001E67 RID: 7783
		private FixedPage _page;
	}
}
