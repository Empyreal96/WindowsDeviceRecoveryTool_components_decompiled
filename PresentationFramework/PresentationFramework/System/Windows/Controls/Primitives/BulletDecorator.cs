using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a layout control that aligns a bullet and another visual object.</summary>
	// Token: 0x02000575 RID: 1397
	public class BulletDecorator : Decorator
	{
		/// <summary>Gets or sets the background color for a <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control. </summary>
		/// <returns>The background color for the <see cref="P:System.Windows.Controls.Primitives.BulletDecorator.Bullet" /> and <see cref="P:System.Windows.Controls.Decorator.Child" /> of a <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x06005BF5 RID: 23541 RVA: 0x0019DA2D File Offset: 0x0019BC2D
		// (set) Token: 0x06005BF6 RID: 23542 RVA: 0x0019DA3F File Offset: 0x0019BC3F
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(BulletDecorator.BackgroundProperty);
			}
			set
			{
				base.SetValue(BulletDecorator.BackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the object to use as the bullet in a <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" />.</summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> to use as the bullet. The default is <see langword="null" />.</returns>
		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x06005BF7 RID: 23543 RVA: 0x0019DA4D File Offset: 0x0019BC4D
		// (set) Token: 0x06005BF8 RID: 23544 RVA: 0x0019DA58 File Offset: 0x0019BC58
		public UIElement Bullet
		{
			get
			{
				return this._bullet;
			}
			set
			{
				if (this._bullet != value)
				{
					if (this._bullet != null)
					{
						base.RemoveVisualChild(this._bullet);
						base.RemoveLogicalChild(this._bullet);
					}
					this._bullet = value;
					base.AddLogicalChild(value);
					base.AddVisualChild(value);
					UIElement child = this.Child;
					if (child != null)
					{
						base.RemoveVisualChild(child);
						base.AddVisualChild(child);
					}
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets an enumerator for the logical child elements of the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control.</summary>
		/// <returns>The enumerator that provides access to the logical child elements of the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control. The default is <see langword="null" />.</returns>
		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x0019DAC1 File Offset: 0x0019BCC1
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this._bullet == null)
				{
					return base.LogicalChildren;
				}
				if (this.Child == null)
				{
					return new SingleChildEnumerator(this._bullet);
				}
				return new BulletDecorator.DoubleChildEnumerator(this._bullet, this.Child);
			}
		}

		/// <summary>Renders the content of a <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control.</summary>
		/// <param name="dc">The <see cref="T:System.Windows.Media.DrawingContext" /> for the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" />. </param>
		// Token: 0x06005BFA RID: 23546 RVA: 0x0019DAF8 File Offset: 0x0019BCF8
		protected override void OnRender(DrawingContext dc)
		{
			Brush background = this.Background;
			if (background != null)
			{
				dc.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			}
		}

		/// <summary>Gets the number of visual child elements for the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control.</summary>
		/// <returns>The number of visual elements that are defined for the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" />. The default is 0.</returns>
		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x06005BFB RID: 23547 RVA: 0x0019DB4A File Offset: 0x0019BD4A
		protected override int VisualChildrenCount
		{
			get
			{
				return ((this.Child == null) ? 0 : 1) + ((this._bullet == null) ? 0 : 1);
			}
		}

		/// <summary>Gets the child element that is at the specified index.</summary>
		/// <param name="index">The specified index for the child element.</param>
		/// <returns>The <see cref="T:System.Windows.Media.Visual" /> child element that is at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0. -or-
		///         <paramref name="index" /> is greater than or equal to <see cref="P:System.Windows.Controls.Primitives.BulletDecorator.VisualChildrenCount" />.</exception>
		// Token: 0x06005BFC RID: 23548 RVA: 0x0019DB68 File Offset: 0x0019BD68
		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index > this.VisualChildrenCount - 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0 && this._bullet != null)
			{
				return this._bullet;
			}
			return this.Child;
		}

		/// <summary>Overrides the default measurement behavior for the objects of a <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control.</summary>
		/// <param name="constraint">The upper <see cref="T:System.Windows.Size" /> limit of the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" />.</param>
		/// <returns>The required <see cref="T:System.Windows.Size" /> for the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control, based on the size of its <see cref="P:System.Windows.Controls.Primitives.BulletDecorator.Bullet" /> and <see cref="P:System.Windows.Controls.Decorator.Child" /> objects.</returns>
		// Token: 0x06005BFD RID: 23549 RVA: 0x0019DBB8 File Offset: 0x0019BDB8
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			Size size2 = default(Size);
			UIElement bullet = this.Bullet;
			UIElement child = this.Child;
			if (bullet != null)
			{
				bullet.Measure(constraint);
				size = bullet.DesiredSize;
			}
			if (child != null)
			{
				Size availableSize = constraint;
				availableSize.Width = Math.Max(0.0, availableSize.Width - size.Width);
				child.Measure(availableSize);
				size2 = child.DesiredSize;
			}
			Size result = new Size(size.Width + size2.Width, Math.Max(size.Height, size2.Height));
			return result;
		}

		/// <summary>Overrides the default content arrangement behavior for the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control.</summary>
		/// <param name="arrangeSize">The available <see cref="T:System.Windows.Size" /> to use to lay out the content of the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control.</param>
		/// <returns>The computed size of the <see cref="T:System.Windows.Controls.Primitives.BulletDecorator" /> control. </returns>
		// Token: 0x06005BFE RID: 23550 RVA: 0x0019DC58 File Offset: 0x0019BE58
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElement bullet = this.Bullet;
			UIElement child = this.Child;
			double x = 0.0;
			double num = 0.0;
			Size size = default(Size);
			if (bullet != null)
			{
				bullet.Arrange(new Rect(bullet.DesiredSize));
				size = bullet.RenderSize;
				x = size.Width;
			}
			if (child != null)
			{
				Size size2 = arrangeSize;
				if (bullet != null)
				{
					size2.Width = Math.Max(child.DesiredSize.Width, arrangeSize.Width - bullet.DesiredSize.Width);
					size2.Height = Math.Max(child.DesiredSize.Height, arrangeSize.Height);
				}
				child.Arrange(new Rect(x, 0.0, size2.Width, size2.Height));
				double num2 = this.GetFirstLineHeight(child) * 0.5;
				num += Math.Max(0.0, num2 - size.Height * 0.5);
			}
			if (bullet != null && !DoubleUtil.IsZero(num))
			{
				bullet.Arrange(new Rect(0.0, num, bullet.DesiredSize.Width, bullet.DesiredSize.Height));
			}
			return arrangeSize;
		}

		// Token: 0x06005BFF RID: 23551 RVA: 0x0019DDB0 File Offset: 0x0019BFB0
		private double GetFirstLineHeight(UIElement element)
		{
			UIElement uielement = this.FindText(element);
			ReadOnlyCollection<LineResult> readOnlyCollection = null;
			if (uielement != null)
			{
				TextBlock textBlock = (TextBlock)uielement;
				if (textBlock.IsLayoutDataValid)
				{
					readOnlyCollection = textBlock.GetLineResults();
				}
			}
			else
			{
				uielement = this.FindFlowDocumentScrollViewer(element);
				if (uielement != null)
				{
					TextDocumentView textDocumentView = ((IServiceProvider)uielement).GetService(typeof(ITextView)) as TextDocumentView;
					if (textDocumentView != null && textDocumentView.IsValid)
					{
						ReadOnlyCollection<ColumnResult> columns = textDocumentView.Columns;
						if (columns != null && columns.Count > 0)
						{
							ColumnResult columnResult = columns[0];
							ReadOnlyCollection<ParagraphResult> paragraphs = columnResult.Paragraphs;
							if (paragraphs != null && paragraphs.Count > 0)
							{
								ContainerParagraphResult containerParagraphResult = paragraphs[0] as ContainerParagraphResult;
								if (containerParagraphResult != null)
								{
									TextParagraphResult textParagraphResult = containerParagraphResult.Paragraphs[0] as TextParagraphResult;
									if (textParagraphResult != null)
									{
										readOnlyCollection = textParagraphResult.Lines;
									}
								}
							}
						}
					}
				}
			}
			if (readOnlyCollection != null && readOnlyCollection.Count > 0)
			{
				Point inPoint = default(Point);
				uielement.TransformToAncestor(element).TryTransform(inPoint, out inPoint);
				return readOnlyCollection[0].LayoutBox.Height + inPoint.Y * 2.0;
			}
			return element.RenderSize.Height;
		}

		// Token: 0x06005C00 RID: 23552 RVA: 0x0019DEE4 File Offset: 0x0019C0E4
		private TextBlock FindText(Visual root)
		{
			TextBlock textBlock = root as TextBlock;
			if (textBlock != null)
			{
				return textBlock;
			}
			ContentPresenter contentPresenter = root as ContentPresenter;
			if (contentPresenter != null)
			{
				if (VisualTreeHelper.GetChildrenCount(contentPresenter) == 1)
				{
					DependencyObject child = VisualTreeHelper.GetChild(contentPresenter, 0);
					TextBlock textBlock2 = child as TextBlock;
					if (textBlock2 == null)
					{
						AccessText accessText = child as AccessText;
						if (accessText != null && VisualTreeHelper.GetChildrenCount(accessText) == 1)
						{
							textBlock2 = (VisualTreeHelper.GetChild(accessText, 0) as TextBlock);
						}
					}
					return textBlock2;
				}
			}
			else
			{
				AccessText accessText2 = root as AccessText;
				if (accessText2 != null && VisualTreeHelper.GetChildrenCount(accessText2) == 1)
				{
					return VisualTreeHelper.GetChild(accessText2, 0) as TextBlock;
				}
			}
			return null;
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x0019DF70 File Offset: 0x0019C170
		private FlowDocumentScrollViewer FindFlowDocumentScrollViewer(Visual root)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = root as FlowDocumentScrollViewer;
			if (flowDocumentScrollViewer != null)
			{
				return flowDocumentScrollViewer;
			}
			ContentPresenter contentPresenter = root as ContentPresenter;
			if (contentPresenter != null && VisualTreeHelper.GetChildrenCount(contentPresenter) == 1)
			{
				return VisualTreeHelper.GetChild(contentPresenter, 0) as FlowDocumentScrollViewer;
			}
			return null;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.BulletDecorator.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.BulletDecorator.Background" /> dependency property. </returns>
		// Token: 0x04002FA1 RID: 12193
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(BulletDecorator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002FA2 RID: 12194
		private UIElement _bullet;

		// Token: 0x020009E3 RID: 2531
		private class DoubleChildEnumerator : IEnumerator
		{
			// Token: 0x0600895F RID: 35167 RVA: 0x00254E09 File Offset: 0x00253009
			internal DoubleChildEnumerator(object child1, object child2)
			{
				this._child1 = child1;
				this._child2 = child2;
			}

			// Token: 0x17001F10 RID: 7952
			// (get) Token: 0x06008960 RID: 35168 RVA: 0x00254E28 File Offset: 0x00253028
			object IEnumerator.Current
			{
				get
				{
					int index = this._index;
					if (index == 0)
					{
						return this._child1;
					}
					if (index != 1)
					{
						return null;
					}
					return this._child2;
				}
			}

			// Token: 0x06008961 RID: 35169 RVA: 0x00254E54 File Offset: 0x00253054
			bool IEnumerator.MoveNext()
			{
				this._index++;
				return this._index < 2;
			}

			// Token: 0x06008962 RID: 35170 RVA: 0x00254E6D File Offset: 0x0025306D
			void IEnumerator.Reset()
			{
				this._index = -1;
			}

			// Token: 0x04004650 RID: 18000
			private int _index = -1;

			// Token: 0x04004651 RID: 18001
			private object _child1;

			// Token: 0x04004652 RID: 18002
			private object _child2;
		}
	}
}
