using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Controls;

namespace System.Windows.Controls
{
	/// <summary>Represents the element used in a <see cref="T:System.Windows.Controls.ControlTemplate" /> to specify where a decorated control is placed relative to other elements in the <see cref="T:System.Windows.Controls.ControlTemplate" />.</summary>
	// Token: 0x0200046C RID: 1132
	[ContentProperty("Child")]
	public class AdornedElementPlaceholder : FrameworkElement, IAddChild
	{
		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value"> An object to add as a child.</param>
		// Token: 0x06004224 RID: 16932 RVA: 0x0012EB0C File Offset: 0x0012CD0C
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				return;
			}
			if (!(value is UIElement))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			if (this.Child != null)
			{
				throw new ArgumentException(SR.Get("CanOnlyHaveOneChild", new object[]
				{
					base.GetType(),
					value.GetType()
				}));
			}
			this.Child = (UIElement)value;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text"> A string to add to the object.</param>
		// Token: 0x06004225 RID: 16933 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets the <see cref="T:System.Windows.UIElement" /> that this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object is reserving space for.</summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> that this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object is reserving space for. The default is <see langword="null" />.</returns>
		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06004226 RID: 16934 RVA: 0x0012EB94 File Offset: 0x0012CD94
		public UIElement AdornedElement
		{
			get
			{
				TemplatedAdorner templatedAdorner = this.TemplatedAdorner;
				if (templatedAdorner != null)
				{
					return this.TemplatedAdorner.AdornedElement;
				}
				return null;
			}
		}

		/// <summary>Gets or sets the single child object of this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object.</summary>
		/// <returns>The single child object of this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object. The default is <see langword="null" />.</returns>
		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06004227 RID: 16935 RVA: 0x0012EBB8 File Offset: 0x0012CDB8
		// (set) Token: 0x06004228 RID: 16936 RVA: 0x0012EBC0 File Offset: 0x0012CDC0
		[DefaultValue(null)]
		public virtual UIElement Child
		{
			get
			{
				return this._child;
			}
			set
			{
				UIElement child = this._child;
				if (child != value)
				{
					base.RemoveVisualChild(child);
					base.RemoveLogicalChild(child);
					this._child = value;
					base.AddVisualChild(this._child);
					base.AddLogicalChild(value);
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets the number of visual child objects.</summary>
		/// <returns>Either 0 or 1. The default is 0.</returns>
		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06004229 RID: 16937 RVA: 0x0012EC06 File Offset: 0x0012CE06
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._child != null)
				{
					return 1;
				}
				return 0;
			}
		}

		/// <summary>Retrieves the <see cref="T:System.Windows.Media.Visual" /> child object at the specified index.</summary>
		/// <param name="index">The index that specifies the child object to retrieve.</param>
		/// <returns>The visual child object at the specified index.</returns>
		// Token: 0x0600422A RID: 16938 RVA: 0x0012EC13 File Offset: 0x0012CE13
		protected override Visual GetVisualChild(int index)
		{
			if (this._child == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._child;
		}

		/// <summary>Gets an enumerator for the logical child elements of this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object.</summary>
		/// <returns>An enumerator for the logical child elements of this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object. The default is <see langword="null" />.</returns>
		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x0600422B RID: 16939 RVA: 0x0012EC41 File Offset: 0x0012CE41
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new SingleChildEnumerator(this._child);
			}
		}

		/// <summary>Raises the <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> event. This method is called when <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> is set to <see langword="true" /> internally.</summary>
		/// <param name="e">Arguments of the event.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object is not part of a template.</exception>
		// Token: 0x0600422C RID: 16940 RVA: 0x0012EC4E File Offset: 0x0012CE4E
		protected override void OnInitialized(EventArgs e)
		{
			if (base.TemplatedParent == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementPlaceholderMustBeInTemplate"));
			}
			base.OnInitialized(e);
		}

		/// <summary>Determines the size of the <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object.</summary>
		/// <param name="constraint">An upper limit value that the return value should not exceed.</param>
		/// <returns>The desired size of this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object is not part of a template.</exception>
		// Token: 0x0600422D RID: 16941 RVA: 0x0012EC70 File Offset: 0x0012CE70
		protected override Size MeasureOverride(Size constraint)
		{
			if (base.TemplatedParent == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementPlaceholderMustBeInTemplate"));
			}
			if (this.AdornedElement == null)
			{
				return new Size(0.0, 0.0);
			}
			Size renderSize = this.AdornedElement.RenderSize;
			UIElement child = this.Child;
			if (child != null)
			{
				child.Measure(renderSize);
			}
			return renderSize;
		}

		/// <summary>Positions the first visual child object and returns the size in layout required by this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object.</summary>
		/// <param name="arrangeBounds">The size that this <see cref="T:System.Windows.Controls.AdornedElementPlaceholder" /> object should use to arrange its child element.</param>
		/// <returns>The actual size needed by the element.</returns>
		// Token: 0x0600422E RID: 16942 RVA: 0x0012ECD4 File Offset: 0x0012CED4
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			UIElement child = this.Child;
			if (child != null)
			{
				child.Arrange(new Rect(arrangeBounds));
			}
			return arrangeBounds;
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x0600422F RID: 16943 RVA: 0x0012ECF8 File Offset: 0x0012CEF8
		private TemplatedAdorner TemplatedAdorner
		{
			get
			{
				if (this._templatedAdorner == null)
				{
					FrameworkElement frameworkElement = base.TemplatedParent as FrameworkElement;
					if (frameworkElement != null)
					{
						this._templatedAdorner = (VisualTreeHelper.GetParent(frameworkElement) as TemplatedAdorner);
						if (this._templatedAdorner != null && this._templatedAdorner.ReferenceElement == null)
						{
							this._templatedAdorner.ReferenceElement = this;
						}
					}
				}
				return this._templatedAdorner;
			}
		}

		// Token: 0x040027D6 RID: 10198
		private UIElement _child;

		// Token: 0x040027D7 RID: 10199
		private TemplatedAdorner _templatedAdorner;
	}
}
