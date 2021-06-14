using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Documents
{
	/// <summary>Abstract class that represents a <see cref="T:System.Windows.FrameworkElement" /> that decorates a <see cref="T:System.Windows.UIElement" />.</summary>
	// Token: 0x02000328 RID: 808
	public abstract class Adorner : FrameworkElement
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Adorner" /> class.</summary>
		/// <param name="adornedElement">The element to bind the adorner to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         adornedElement is <see langword="null" />.</exception>
		// Token: 0x06002A91 RID: 10897 RVA: 0x000C2B62 File Offset: 0x000C0D62
		protected Adorner(UIElement adornedElement)
		{
			if (adornedElement == null)
			{
				throw new ArgumentNullException("adornedElement");
			}
			this._adornedElement = adornedElement;
			this._isClipEnabled = false;
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(Adorner.CreateFlowDirectionBinding), this);
		}

		/// <summary>Implements any custom measuring behavior for the adorner.</summary>
		/// <param name="constraint">A size to constrain the adorner to.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> object representing the amount of layout space needed by the adorner.</returns>
		// Token: 0x06002A92 RID: 10898 RVA: 0x000C2BA0 File Offset: 0x000C0DA0
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = new Size(this.AdornedElement.RenderSize.Width, this.AdornedElement.RenderSize.Height);
			int visualChildrenCount = this.VisualChildrenCount;
			for (int i = 0; i < visualChildrenCount; i++)
			{
				UIElement uielement = this.GetVisualChild(i) as UIElement;
				if (uielement != null)
				{
					uielement.Measure(size);
				}
			}
			return size;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Windows.UIElement.GetLayoutClip(System.Windows.Size)" />.</summary>
		/// <param name="layoutSlotSize">The available size provided by the element.</param>
		/// <returns>The potential clipping geometry. See Remarks.</returns>
		// Token: 0x06002A93 RID: 10899 RVA: 0x0000C238 File Offset: 0x0000A438
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			return null;
		}

		/// <summary>Returns a <see cref="T:System.Windows.Media.Transform" /> for the adorner, based on the transform that is currently applied to the adorned element.</summary>
		/// <param name="transform">The transform that is currently applied to the adorned element.</param>
		/// <returns>A transform to apply to the adorner.</returns>
		// Token: 0x06002A94 RID: 10900 RVA: 0x00012630 File Offset: 0x00010830
		public virtual GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			return transform;
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002A95 RID: 10901 RVA: 0x000C2C08 File Offset: 0x000C0E08
		// (set) Token: 0x06002A96 RID: 10902 RVA: 0x000C2C10 File Offset: 0x000C0E10
		internal Geometry AdornerClip
		{
			get
			{
				return base.Clip;
			}
			set
			{
				base.Clip = value;
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06002A97 RID: 10903 RVA: 0x000C2C19 File Offset: 0x000C0E19
		// (set) Token: 0x06002A98 RID: 10904 RVA: 0x000C2C21 File Offset: 0x000C0E21
		internal Transform AdornerTransform
		{
			get
			{
				return base.RenderTransform;
			}
			set
			{
				base.RenderTransform = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.UIElement" /> that this adorner is bound to.</summary>
		/// <returns>The element that this adorner is bound to. The default value is <see langword="null" />.</returns>
		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06002A99 RID: 10905 RVA: 0x000C2C2A File Offset: 0x000C0E2A
		public UIElement AdornedElement
		{
			get
			{
				return this._adornedElement;
			}
		}

		/// <summary>Gets or sets a value that indicates whether clipping of the adorner is enabled.</summary>
		/// <returns>A <see langword="Boolean" /> value indicating whether clipping of the adorner is enabled.If this property is <see langword="false" />, the adorner is not clipped.If this property is <see langword="true" />, the adorner is clipped using the same clipping geometry as the adorned element.The default value is <see langword="false" />.</returns>
		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06002A9A RID: 10906 RVA: 0x000C2C32 File Offset: 0x000C0E32
		// (set) Token: 0x06002A9B RID: 10907 RVA: 0x000C2C3A File Offset: 0x000C0E3A
		public bool IsClipEnabled
		{
			get
			{
				return this._isClipEnabled;
			}
			set
			{
				this._isClipEnabled = value;
				base.InvalidateArrange();
				AdornerLayer.GetAdornerLayer(this._adornedElement).InvalidateArrange();
			}
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000C2C5C File Offset: 0x000C0E5C
		private static object CreateFlowDirectionBinding(object o)
		{
			Adorner adorner = (Adorner)o;
			Binding binding = new Binding("FlowDirection");
			binding.Mode = BindingMode.OneWay;
			binding.Source = adorner.AdornedElement;
			adorner.SetBinding(FrameworkElement.FlowDirectionProperty, binding);
			return null;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000C2C9C File Offset: 0x000C0E9C
		internal virtual bool NeedsUpdate(Size oldSize)
		{
			return !DoubleUtil.AreClose(this.AdornedElement.RenderSize, oldSize);
		}

		// Token: 0x04001C48 RID: 7240
		private readonly UIElement _adornedElement;

		// Token: 0x04001C49 RID: 7241
		private bool _isClipEnabled;
	}
}
