using System;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000336 RID: 822
	internal class CompositionAdorner : Adorner
	{
		// Token: 0x06002B4D RID: 11085 RVA: 0x000C5B16 File Offset: 0x000C3D16
		static CompositionAdorner()
		{
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(CompositionAdorner), new FrameworkPropertyMetadata(false));
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x000C5B37 File Offset: 0x000C3D37
		internal CompositionAdorner(ITextView textView) : this(textView, new ArrayList())
		{
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x000C5B45 File Offset: 0x000C3D45
		internal CompositionAdorner(ITextView textView, ArrayList attributeRanges) : base(textView.RenderScope)
		{
			this._textView = textView;
			this._attributeRanges = attributeRanges;
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000C5B64 File Offset: 0x000C3D64
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			Transform transform2 = transform.AffineTransform;
			if (transform2 == null)
			{
				transform2 = Transform.Identity;
			}
			TranslateTransform value = new TranslateTransform(-transform2.Value.OffsetX, -transform2.Value.OffsetY);
			generalTransformGroup.Children.Add(value);
			if (transform != null)
			{
				generalTransformGroup.Children.Add(transform);
			}
			return generalTransformGroup;
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x000C5BC8 File Offset: 0x000C3DC8
		protected override void OnRender(DrawingContext drawingContext)
		{
			Visual visual = VisualTreeHelper.GetParent(base.AdornedElement) as Visual;
			if (visual == null)
			{
				return;
			}
			GeneralTransform generalTransform = base.AdornedElement.TransformToAncestor(visual);
			if (generalTransform == null)
			{
				return;
			}
			bool flag = "zh-CN".Equals(InputLanguageManager.Current.CurrentInputLanguage.IetfLanguageTag);
			for (int i = 0; i < this._attributeRanges.Count; i++)
			{
				CompositionAdorner.AttributeRange attributeRange = (CompositionAdorner.AttributeRange)this._attributeRanges[i];
				if (attributeRange.CompositionLines.Count != 0)
				{
					bool isBoldLine = attributeRange.TextServicesDisplayAttribute.IsBoldLine;
					bool flag2 = false;
					bool flag3 = (attributeRange.TextServicesDisplayAttribute.AttrInfo & UnsafeNativeMethods.TF_DA_ATTR_INFO.TF_ATTR_TARGET_CONVERTED) > UnsafeNativeMethods.TF_DA_ATTR_INFO.TF_ATTR_INPUT;
					Brush brush = null;
					double opacity = -1.0;
					Pen pen = null;
					if (flag && flag3)
					{
						DependencyObject parent = this._textView.TextContainer.Parent;
						brush = (Brush)parent.GetValue(TextBoxBase.SelectionBrushProperty);
						opacity = (double)parent.GetValue(TextBoxBase.SelectionOpacityProperty);
					}
					double height = attributeRange.Height;
					double num = height * (isBoldLine ? 0.08 : 0.06);
					double num2 = height * 0.09;
					Pen pen2 = new Pen(new SolidColorBrush(Colors.Black), num);
					switch (attributeRange.TextServicesDisplayAttribute.LineStyle)
					{
					case UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_SOLID:
						pen2.StartLineCap = PenLineCap.Round;
						pen2.EndLineCap = PenLineCap.Round;
						break;
					case UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_DOT:
						pen2.DashStyle = new DashStyle(new DoubleCollection
						{
							1.2,
							1.2
						}, 0.0);
						pen2.DashCap = PenLineCap.Round;
						pen2.StartLineCap = PenLineCap.Round;
						pen2.EndLineCap = PenLineCap.Round;
						num = height * (isBoldLine ? 0.1 : 0.08);
						break;
					case UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_DASH:
					{
						double value = height * (isBoldLine ? 0.39 : 0.27);
						double value2 = height * (isBoldLine ? 0.06 : 0.04);
						pen2.DashStyle = new DashStyle(new DoubleCollection
						{
							value,
							value2
						}, 0.0);
						pen2.DashCap = PenLineCap.Round;
						pen2.StartLineCap = PenLineCap.Round;
						pen2.EndLineCap = PenLineCap.Round;
						break;
					}
					case UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_SQUIGGLE:
						flag2 = true;
						break;
					}
					double num3 = num / 2.0;
					for (int j = 0; j < attributeRange.CompositionLines.Count; j++)
					{
						CompositionAdorner.CompositionLine compositionLine = (CompositionAdorner.CompositionLine)attributeRange.CompositionLines[j];
						Point point = new Point(compositionLine.StartPoint.X + num2, compositionLine.StartPoint.Y - num3);
						Point point2 = new Point(compositionLine.EndPoint.X - num2, compositionLine.EndPoint.Y - num3);
						pen2.Brush = new SolidColorBrush(compositionLine.LineColor);
						generalTransform.TryTransform(point, out point);
						generalTransform.TryTransform(point2, out point2);
						if (flag && flag3)
						{
							Rect rect = Rect.Union(compositionLine.StartRect, compositionLine.EndRect);
							rect = generalTransform.TransformBounds(rect);
							drawingContext.PushOpacity(opacity);
							drawingContext.DrawRectangle(brush, pen, rect);
							drawingContext.Pop();
						}
						if (flag2)
						{
							Point point3 = new Point(point.X, point.Y - num3);
							double num4 = num3;
							PathFigure pathFigure = new PathFigure();
							pathFigure.StartPoint = point3;
							int num5 = 0;
							while ((double)num5 < (point2.X - point.X) / num4)
							{
								if (num5 % 4 == 0 || num5 % 4 == 3)
								{
									point3 = new Point(point3.X + num4, point3.Y + num3);
									pathFigure.Segments.Add(new LineSegment(point3, true));
								}
								else if (num5 % 4 == 1 || num5 % 4 == 2)
								{
									point3 = new Point(point3.X + num4, point3.Y - num3);
									pathFigure.Segments.Add(new LineSegment(point3, true));
								}
								num5++;
							}
							drawingContext.DrawGeometry(null, pen2, new PathGeometry
							{
								Figures = 
								{
									pathFigure
								}
							});
						}
						else
						{
							drawingContext.DrawLine(pen2, point, point2);
						}
					}
				}
			}
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x000C605C File Offset: 0x000C425C
		internal void AddAttributeRange(ITextPointer start, ITextPointer end, TextServicesDisplayAttribute textServiceDisplayAttribute)
		{
			ITextPointer start2 = start.CreatePointer(LogicalDirection.Forward);
			ITextPointer end2 = end.CreatePointer(LogicalDirection.Backward);
			this._attributeRanges.Add(new CompositionAdorner.AttributeRange(this._textView, start2, end2, textServiceDisplayAttribute));
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000C6094 File Offset: 0x000C4294
		internal void InvalidateAdorner()
		{
			for (int i = 0; i < this._attributeRanges.Count; i++)
			{
				CompositionAdorner.AttributeRange attributeRange = (CompositionAdorner.AttributeRange)this._attributeRanges[i];
				attributeRange.AddCompositionLines();
			}
			AdornerLayer adornerLayer = VisualTreeHelper.GetParent(this) as AdornerLayer;
			if (adornerLayer != null)
			{
				adornerLayer.Update(base.AdornedElement);
				adornerLayer.InvalidateArrange();
			}
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000C60F0 File Offset: 0x000C42F0
		internal void Initialize(ITextView textView)
		{
			this._adornerLayer = AdornerLayer.GetAdornerLayer(textView.RenderScope);
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Add(this);
			}
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000C6117 File Offset: 0x000C4317
		internal void Uninitialize()
		{
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
				this._adornerLayer = null;
			}
		}

		// Token: 0x04001C91 RID: 7313
		private AdornerLayer _adornerLayer;

		// Token: 0x04001C92 RID: 7314
		private ITextView _textView;

		// Token: 0x04001C93 RID: 7315
		private readonly ArrayList _attributeRanges;

		// Token: 0x04001C94 RID: 7316
		private const double DotLength = 1.2;

		// Token: 0x04001C95 RID: 7317
		private const double NormalLineHeightRatio = 0.06;

		// Token: 0x04001C96 RID: 7318
		private const double BoldLineHeightRatio = 0.08;

		// Token: 0x04001C97 RID: 7319
		private const double NormalDotLineHeightRatio = 0.08;

		// Token: 0x04001C98 RID: 7320
		private const double BoldDotLineHeightRatio = 0.1;

		// Token: 0x04001C99 RID: 7321
		private const double NormalDashRatio = 0.27;

		// Token: 0x04001C9A RID: 7322
		private const double BoldDashRatio = 0.39;

		// Token: 0x04001C9B RID: 7323
		private const double ClauseGapRatio = 0.09;

		// Token: 0x04001C9C RID: 7324
		private const double NormalDashGapRatio = 0.04;

		// Token: 0x04001C9D RID: 7325
		private const double BoldDashGapRatio = 0.06;

		// Token: 0x04001C9E RID: 7326
		private const string chinesePinyin = "zh-CN";

		// Token: 0x020008C7 RID: 2247
		private class AttributeRange
		{
			// Token: 0x06008465 RID: 33893 RVA: 0x0024819E File Offset: 0x0024639E
			internal AttributeRange(ITextView textView, ITextPointer start, ITextPointer end, TextServicesDisplayAttribute textServicesDisplayAttribute)
			{
				this._textView = textView;
				this._startOffset = start.Offset;
				this._endOffset = end.Offset;
				this._textServicesDisplayAttribute = textServicesDisplayAttribute;
				this._compositionLines = new ArrayList(1);
			}

			// Token: 0x06008466 RID: 33894 RVA: 0x002481DC File Offset: 0x002463DC
			internal void AddCompositionLines()
			{
				this._compositionLines.Clear();
				ITextPointer textPointer = this._textView.TextContainer.Start.CreatePointer(this._startOffset, LogicalDirection.Forward);
				ITextPointer textPointer2 = this._textView.TextContainer.Start.CreatePointer(this._endOffset, LogicalDirection.Backward);
				while (textPointer.CompareTo(textPointer2) < 0 && textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
				Invariant.Assert(textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text);
				if (textPointer2.HasValidLayout)
				{
					this._startRect = this._textView.GetRectangleFromTextPosition(textPointer);
					this._endRect = this._textView.GetRectangleFromTextPosition(textPointer2);
					if (this._startRect.Top != this._endRect.Top)
					{
						this.AddMultipleCompositionLines(textPointer, textPointer2);
						return;
					}
					Color lineColor = this._textServicesDisplayAttribute.GetLineColor(textPointer);
					this._compositionLines.Add(new CompositionAdorner.CompositionLine(this._startRect, this._endRect, lineColor));
				}
			}

			// Token: 0x17001DF9 RID: 7673
			// (get) Token: 0x06008467 RID: 33895 RVA: 0x002482D3 File Offset: 0x002464D3
			internal double Height
			{
				get
				{
					return this._startRect.Bottom - this._startRect.Top;
				}
			}

			// Token: 0x17001DFA RID: 7674
			// (get) Token: 0x06008468 RID: 33896 RVA: 0x002482EC File Offset: 0x002464EC
			internal ArrayList CompositionLines
			{
				get
				{
					return this._compositionLines;
				}
			}

			// Token: 0x17001DFB RID: 7675
			// (get) Token: 0x06008469 RID: 33897 RVA: 0x002482F4 File Offset: 0x002464F4
			internal TextServicesDisplayAttribute TextServicesDisplayAttribute
			{
				get
				{
					return this._textServicesDisplayAttribute;
				}
			}

			// Token: 0x0600846A RID: 33898 RVA: 0x002482FC File Offset: 0x002464FC
			private void AddMultipleCompositionLines(ITextPointer start, ITextPointer end)
			{
				ITextPointer textPointer = start;
				ITextPointer textPointer2 = textPointer;
				while (textPointer2.CompareTo(end) < 0)
				{
					TextSegment lineRange = this._textView.GetLineRange(textPointer2);
					if (lineRange.IsNull)
					{
						textPointer = textPointer2;
					}
					else
					{
						if (textPointer.CompareTo(lineRange.Start) < 0)
						{
							textPointer = lineRange.Start;
						}
						if (textPointer2.CompareTo(lineRange.End) < 0)
						{
							if (end.CompareTo(lineRange.End) < 0)
							{
								textPointer2 = end.CreatePointer();
							}
							else
							{
								textPointer2 = lineRange.End.CreatePointer(LogicalDirection.Backward);
							}
						}
						Rect rectangleFromTextPosition = this._textView.GetRectangleFromTextPosition(textPointer);
						Rect rectangleFromTextPosition2 = this._textView.GetRectangleFromTextPosition(textPointer2);
						this._compositionLines.Add(new CompositionAdorner.CompositionLine(rectangleFromTextPosition, rectangleFromTextPosition2, this._textServicesDisplayAttribute.GetLineColor(textPointer)));
						textPointer = lineRange.End.CreatePointer(LogicalDirection.Forward);
					}
					while (textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.None && textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					textPointer2 = textPointer;
				}
			}

			// Token: 0x04004223 RID: 16931
			private ITextView _textView;

			// Token: 0x04004224 RID: 16932
			private Rect _startRect;

			// Token: 0x04004225 RID: 16933
			private Rect _endRect;

			// Token: 0x04004226 RID: 16934
			private readonly int _startOffset;

			// Token: 0x04004227 RID: 16935
			private readonly int _endOffset;

			// Token: 0x04004228 RID: 16936
			private readonly TextServicesDisplayAttribute _textServicesDisplayAttribute;

			// Token: 0x04004229 RID: 16937
			private readonly ArrayList _compositionLines;
		}

		// Token: 0x020008C8 RID: 2248
		private class CompositionLine
		{
			// Token: 0x0600846B RID: 33899 RVA: 0x002483F3 File Offset: 0x002465F3
			internal CompositionLine(Rect startRect, Rect endRect, Color lineColor)
			{
				this._startRect = startRect;
				this._endRect = endRect;
				this._color = lineColor;
			}

			// Token: 0x17001DFC RID: 7676
			// (get) Token: 0x0600846C RID: 33900 RVA: 0x00248410 File Offset: 0x00246610
			internal Point StartPoint
			{
				get
				{
					return this._startRect.BottomLeft;
				}
			}

			// Token: 0x17001DFD RID: 7677
			// (get) Token: 0x0600846D RID: 33901 RVA: 0x0024841D File Offset: 0x0024661D
			internal Point EndPoint
			{
				get
				{
					return this._endRect.BottomRight;
				}
			}

			// Token: 0x17001DFE RID: 7678
			// (get) Token: 0x0600846E RID: 33902 RVA: 0x0024842A File Offset: 0x0024662A
			internal Rect StartRect
			{
				get
				{
					return this._startRect;
				}
			}

			// Token: 0x17001DFF RID: 7679
			// (get) Token: 0x0600846F RID: 33903 RVA: 0x00248432 File Offset: 0x00246632
			internal Rect EndRect
			{
				get
				{
					return this._endRect;
				}
			}

			// Token: 0x17001E00 RID: 7680
			// (get) Token: 0x06008470 RID: 33904 RVA: 0x0024843A File Offset: 0x0024663A
			internal Color LineColor
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x0400422A RID: 16938
			private Rect _startRect;

			// Token: 0x0400422B RID: 16939
			private Rect _endRect;

			// Token: 0x0400422C RID: 16940
			private Color _color;
		}
	}
}
