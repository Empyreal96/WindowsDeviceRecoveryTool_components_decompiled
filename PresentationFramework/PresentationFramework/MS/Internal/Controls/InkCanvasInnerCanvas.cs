using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MS.Internal.Controls
{
	// Token: 0x02000758 RID: 1880
	internal class InkCanvasInnerCanvas : Panel
	{
		// Token: 0x060077B0 RID: 30640 RVA: 0x002227E0 File Offset: 0x002209E0
		internal InkCanvasInnerCanvas(InkCanvas inkCanvas)
		{
			this._inkCanvas = inkCanvas;
		}

		// Token: 0x060077B1 RID: 30641 RVA: 0x00133750 File Offset: 0x00131950
		private InkCanvasInnerCanvas()
		{
		}

		// Token: 0x060077B2 RID: 30642 RVA: 0x002227F0 File Offset: 0x002209F0
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			UIElement uielement = visualRemoved as UIElement;
			if (uielement != null)
			{
				this.InkCanvas.InkCanvasSelection.RemoveElement(uielement);
			}
			this.InkCanvas.RaiseOnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		// Token: 0x060077B3 RID: 30643 RVA: 0x00222830 File Offset: 0x00220A30
		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			Size result = default(Size);
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					uielement.Measure(availableSize);
					double num = InkCanvas.GetLeft(uielement);
					if (!DoubleUtil.IsNaN(num))
					{
						result.Width = Math.Max(result.Width, num + uielement.DesiredSize.Width);
					}
					else
					{
						result.Width = Math.Max(result.Width, uielement.DesiredSize.Width);
					}
					double num2 = InkCanvas.GetTop(uielement);
					if (!DoubleUtil.IsNaN(num2))
					{
						result.Height = Math.Max(result.Height, num2 + uielement.DesiredSize.Height);
					}
					else
					{
						result.Height = Math.Max(result.Height, uielement.DesiredSize.Height);
					}
				}
			}
			return result;
		}

		// Token: 0x060077B4 RID: 30644 RVA: 0x00222970 File Offset: 0x00220B70
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					double x = 0.0;
					double y = 0.0;
					double num = InkCanvas.GetLeft(uielement);
					if (!DoubleUtil.IsNaN(num))
					{
						x = num;
					}
					else
					{
						double num2 = InkCanvas.GetRight(uielement);
						if (!DoubleUtil.IsNaN(num2))
						{
							x = arrangeSize.Width - uielement.DesiredSize.Width - num2;
						}
					}
					double num3 = InkCanvas.GetTop(uielement);
					if (!DoubleUtil.IsNaN(num3))
					{
						y = num3;
					}
					else
					{
						double num4 = InkCanvas.GetBottom(uielement);
						if (!DoubleUtil.IsNaN(num4))
						{
							y = arrangeSize.Height - uielement.DesiredSize.Height - num4;
						}
					}
					uielement.Arrange(new Rect(new Point(x, y), uielement.DesiredSize));
				}
			}
			return arrangeSize;
		}

		// Token: 0x060077B5 RID: 30645 RVA: 0x00222A88 File Offset: 0x00220C88
		protected override void OnChildDesiredSizeChanged(UIElement child)
		{
			base.OnChildDesiredSizeChanged(child);
			base.InvalidateMeasure();
		}

		// Token: 0x060077B6 RID: 30646 RVA: 0x00222A97 File Offset: 0x00220C97
		protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return base.CreateUIElementCollection(this._inkCanvas);
		}

		// Token: 0x17001C61 RID: 7265
		// (get) Token: 0x060077B7 RID: 30647 RVA: 0x00222AA5 File Offset: 0x00220CA5
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x060077B8 RID: 30648 RVA: 0x00163D2C File Offset: 0x00161F2C
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			if (base.ClipToBounds)
			{
				return base.GetLayoutClip(layoutSlotSize);
			}
			return null;
		}

		// Token: 0x060077B9 RID: 30649 RVA: 0x00222AAC File Offset: 0x00220CAC
		internal UIElement HitTestOnElements(Point point)
		{
			UIElement result = null;
			HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);
			if (hitTestResult != null)
			{
				Visual visual = hitTestResult.VisualHit as Visual;
				Visual3D visual3D = hitTestResult.VisualHit as Visual3D;
				DependencyObject dependencyObject = null;
				if (visual != null)
				{
					dependencyObject = visual;
				}
				else if (visual3D != null)
				{
					dependencyObject = visual3D;
				}
				while (dependencyObject != null)
				{
					DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
					if (parent == this.InkCanvas.InnerCanvas)
					{
						result = (dependencyObject as UIElement);
						break;
					}
					dependencyObject = parent;
				}
			}
			return result;
		}

		// Token: 0x17001C62 RID: 7266
		// (get) Token: 0x060077BA RID: 30650 RVA: 0x00222B1C File Offset: 0x00220D1C
		internal IEnumerator PrivateLogicalChildren
		{
			get
			{
				return base.LogicalChildren;
			}
		}

		// Token: 0x17001C63 RID: 7267
		// (get) Token: 0x060077BB RID: 30651 RVA: 0x00222B24 File Offset: 0x00220D24
		internal InkCanvas InkCanvas
		{
			get
			{
				return this._inkCanvas;
			}
		}

		// Token: 0x040038D3 RID: 14547
		private InkCanvas _inkCanvas;
	}
}
