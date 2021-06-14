using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007DE RID: 2014
	internal sealed class AnnotationAdorner : Adorner
	{
		// Token: 0x06007C83 RID: 31875 RVA: 0x00230504 File Offset: 0x0022E704
		public AnnotationAdorner(IAnnotationComponent component, UIElement annotatedElement) : base(annotatedElement)
		{
			if (component is UIElement)
			{
				this._annotationComponent = component;
				base.AddVisualChild((UIElement)this._annotationComponent);
				return;
			}
			throw new ArgumentException(SR.Get("AnnotationAdorner_NotUIElement"), "component");
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x00230544 File Offset: 0x0022E744
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (!(this._annotationComponent is UIElement))
			{
				return null;
			}
			transform = base.GetDesiredTransform(transform);
			GeneralTransform desiredTransform = this._annotationComponent.GetDesiredTransform(transform);
			if (this._annotationComponent.AnnotatedElement == null)
			{
				return null;
			}
			if (desiredTransform == null)
			{
				this._annotatedElement = this._annotationComponent.AnnotatedElement;
				this._annotatedElement.LayoutUpdated += this.OnLayoutUpdated;
				return transform;
			}
			return desiredTransform;
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x002305B3 File Offset: 0x0022E7B3
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0 || this._annotationComponent == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return (UIElement)this._annotationComponent;
		}

		// Token: 0x17001CEE RID: 7406
		// (get) Token: 0x06007C86 RID: 31878 RVA: 0x002305E6 File Offset: 0x0022E7E6
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._annotationComponent == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06007C87 RID: 31879 RVA: 0x002305F4 File Offset: 0x0022E7F4
		protected override Size MeasureOverride(Size availableSize)
		{
			Size availableSize2 = new Size(double.PositiveInfinity, double.PositiveInfinity);
			Invariant.Assert(this._annotationComponent != null, "AnnotationAdorner should only have one child - the annotation component.");
			((UIElement)this._annotationComponent).Measure(availableSize2);
			return new Size(0.0, 0.0);
		}

		// Token: 0x06007C88 RID: 31880 RVA: 0x00230655 File Offset: 0x0022E855
		protected override Size ArrangeOverride(Size finalSize)
		{
			Invariant.Assert(this._annotationComponent != null, "AnnotationAdorner should only have one child - the annotation component.");
			((UIElement)this._annotationComponent).Arrange(new Rect(((UIElement)this._annotationComponent).DesiredSize));
			return finalSize;
		}

		// Token: 0x06007C89 RID: 31881 RVA: 0x00230690 File Offset: 0x0022E890
		internal void RemoveChildren()
		{
			base.RemoveVisualChild((UIElement)this._annotationComponent);
			this._annotationComponent = null;
		}

		// Token: 0x06007C8A RID: 31882 RVA: 0x002306AC File Offset: 0x0022E8AC
		internal void InvalidateTransform()
		{
			AdornerLayer adornerLayer = (AdornerLayer)VisualTreeHelper.GetParent(this);
			base.InvalidateMeasure();
			adornerLayer.InvalidateVisual();
		}

		// Token: 0x17001CEF RID: 7407
		// (get) Token: 0x06007C8B RID: 31883 RVA: 0x002306D1 File Offset: 0x0022E8D1
		internal IAnnotationComponent AnnotationComponent
		{
			get
			{
				return this._annotationComponent;
			}
		}

		// Token: 0x06007C8C RID: 31884 RVA: 0x002306DC File Offset: 0x0022E8DC
		private void OnLayoutUpdated(object sender, EventArgs args)
		{
			this._annotatedElement.LayoutUpdated -= this.OnLayoutUpdated;
			this._annotatedElement = null;
			if (this._annotationComponent.AttachedAnnotations.Count > 0)
			{
				this._annotationComponent.PresentationContext.Host.InvalidateMeasure();
				base.InvalidateMeasure();
			}
		}

		// Token: 0x04003A68 RID: 14952
		private IAnnotationComponent _annotationComponent;

		// Token: 0x04003A69 RID: 14953
		private UIElement _annotatedElement;
	}
}
