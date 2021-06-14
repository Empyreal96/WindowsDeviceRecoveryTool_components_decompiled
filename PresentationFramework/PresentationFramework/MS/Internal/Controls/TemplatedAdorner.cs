using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Controls
{
	// Token: 0x02000763 RID: 1891
	internal sealed class TemplatedAdorner : Adorner
	{
		// Token: 0x06007847 RID: 30791 RVA: 0x00224382 File Offset: 0x00222582
		public void ClearChild()
		{
			base.RemoveVisualChild(this._child);
			this._child = null;
		}

		// Token: 0x06007848 RID: 30792 RVA: 0x00224398 File Offset: 0x00222598
		public TemplatedAdorner(UIElement adornedElement, ControlTemplate adornerTemplate) : base(adornedElement)
		{
			this._child = new Control
			{
				DataContext = Validation.GetErrors(adornedElement),
				IsTabStop = false,
				Template = adornerTemplate
			};
			base.AddVisualChild(this._child);
		}

		// Token: 0x06007849 RID: 30793 RVA: 0x002243E0 File Offset: 0x002225E0
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (this.ReferenceElement == null)
			{
				return transform;
			}
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			generalTransformGroup.Children.Add(transform);
			GeneralTransform generalTransform = base.TransformToDescendant(this.ReferenceElement);
			if (generalTransform != null)
			{
				generalTransformGroup.Children.Add(generalTransform);
			}
			return generalTransformGroup;
		}

		// Token: 0x17001C83 RID: 7299
		// (get) Token: 0x0600784A RID: 30794 RVA: 0x00224426 File Offset: 0x00222626
		// (set) Token: 0x0600784B RID: 30795 RVA: 0x0022442E File Offset: 0x0022262E
		public FrameworkElement ReferenceElement
		{
			get
			{
				return this._referenceElement;
			}
			set
			{
				this._referenceElement = value;
			}
		}

		// Token: 0x0600784C RID: 30796 RVA: 0x00224437 File Offset: 0x00222637
		protected override Visual GetVisualChild(int index)
		{
			if (this._child == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._child;
		}

		// Token: 0x17001C84 RID: 7300
		// (get) Token: 0x0600784D RID: 30797 RVA: 0x00224465 File Offset: 0x00222665
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._child == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x0600784E RID: 30798 RVA: 0x00224474 File Offset: 0x00222674
		protected override Size MeasureOverride(Size constraint)
		{
			if (this.ReferenceElement != null && base.AdornedElement != null && base.AdornedElement.IsMeasureValid && !DoubleUtil.AreClose(this.ReferenceElement.DesiredSize, base.AdornedElement.DesiredSize))
			{
				this.ReferenceElement.InvalidateMeasure();
			}
			this._child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			return this._child.DesiredSize;
		}

		// Token: 0x0600784F RID: 30799 RVA: 0x002244F4 File Offset: 0x002226F4
		protected override Size ArrangeOverride(Size size)
		{
			Size size2 = base.ArrangeOverride(size);
			if (this._child != null)
			{
				this._child.Arrange(new Rect(default(Point), size2));
			}
			return size2;
		}

		// Token: 0x06007850 RID: 30800 RVA: 0x0022452C File Offset: 0x0022272C
		internal override bool NeedsUpdate(Size oldSize)
		{
			bool result = base.NeedsUpdate(oldSize);
			Visibility visibility = base.AdornedElement.IsVisible ? Visibility.Visible : Visibility.Collapsed;
			if (visibility != base.Visibility)
			{
				base.Visibility = visibility;
				result = true;
			}
			return result;
		}

		// Token: 0x040038F0 RID: 14576
		private Control _child;

		// Token: 0x040038F1 RID: 14577
		private FrameworkElement _referenceElement;
	}
}
