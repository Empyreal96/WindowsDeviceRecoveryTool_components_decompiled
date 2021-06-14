using System;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Used to arrange overflow <see cref="T:System.Windows.Controls.ToolBar" /> items.</summary>
	// Token: 0x020005B2 RID: 1458
	public class ToolBarOverflowPanel : Panel
	{
		/// <summary>Gets or sets the recommended width for an overflow <see cref="T:System.Windows.Controls.ToolBar" /> before items flow to the next line.  </summary>
		/// <returns>A double value that represents the recommended width of the <see cref="T:System.Windows.Controls.ToolBar" />.</returns>
		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x060060FD RID: 24829 RVA: 0x001B3491 File Offset: 0x001B1691
		// (set) Token: 0x060060FE RID: 24830 RVA: 0x001B34A3 File Offset: 0x001B16A3
		public double WrapWidth
		{
			get
			{
				return (double)base.GetValue(ToolBarOverflowPanel.WrapWidthProperty);
			}
			set
			{
				base.SetValue(ToolBarOverflowPanel.WrapWidthProperty, value);
			}
		}

		// Token: 0x060060FF RID: 24831 RVA: 0x001B34B8 File Offset: 0x001B16B8
		private static bool IsWrapWidthValid(object value)
		{
			double num = (double)value;
			return DoubleUtil.IsNaN(num) || (DoubleUtil.GreaterThanOrClose(num, 0.0) && !double.IsPositiveInfinity(num));
		}

		/// <summary>Remeasures the <see cref="T:System.Windows.Controls.Primitives.ToolBarOverflowPanel" />. </summary>
		/// <param name="constraint">Constraint size is an upper limit. The return value should not exceed this size.</param>
		/// <returns>The desired size.</returns>
		// Token: 0x06006100 RID: 24832 RVA: 0x001B34F4 File Offset: 0x001B16F4
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			this._panelSize = default(Size);
			this._wrapWidth = (double.IsNaN(this.WrapWidth) ? constraint.Width : this.WrapWidth);
			UIElementCollection internalChildren = base.InternalChildren;
			int num = internalChildren.Count;
			ToolBarPanel toolBarPanel = this.ToolBarPanel;
			if (toolBarPanel != null)
			{
				List<UIElement> generatedItemsCollection = toolBarPanel.GeneratedItemsCollection;
				int num2 = (generatedItemsCollection != null) ? generatedItemsCollection.Count : 0;
				int num3 = 0;
				for (int i = 0; i < num2; i++)
				{
					UIElement uielement = generatedItemsCollection[i];
					if (uielement != null && ToolBar.GetIsOverflowItem(uielement) && !(uielement is Separator))
					{
						if (num3 < num)
						{
							if (internalChildren[num3] != uielement)
							{
								internalChildren.Insert(num3, uielement);
								num++;
							}
						}
						else
						{
							internalChildren.Add(uielement);
							num++;
						}
						num3++;
					}
				}
			}
			for (int j = 0; j < num; j++)
			{
				UIElement uielement2 = internalChildren[j];
				uielement2.Measure(constraint);
				Size desiredSize = uielement2.DesiredSize;
				if (DoubleUtil.GreaterThan(desiredSize.Width, this._wrapWidth))
				{
					this._wrapWidth = desiredSize.Width;
				}
			}
			this._wrapWidth = Math.Min(this._wrapWidth, constraint.Width);
			for (int k = 0; k < internalChildren.Count; k++)
			{
				UIElement uielement3 = internalChildren[k];
				Size desiredSize2 = uielement3.DesiredSize;
				if (DoubleUtil.GreaterThan(size.Width + desiredSize2.Width, this._wrapWidth))
				{
					this._panelSize.Width = Math.Max(size.Width, this._panelSize.Width);
					this._panelSize.Height = this._panelSize.Height + size.Height;
					size = desiredSize2;
					if (DoubleUtil.GreaterThan(desiredSize2.Width, this._wrapWidth))
					{
						this._panelSize.Width = Math.Max(desiredSize2.Width, this._panelSize.Width);
						this._panelSize.Height = this._panelSize.Height + desiredSize2.Height;
						size = default(Size);
					}
				}
				else
				{
					size.Width += desiredSize2.Width;
					size.Height = Math.Max(desiredSize2.Height, size.Height);
				}
			}
			this._panelSize.Width = Math.Max(size.Width, this._panelSize.Width);
			this._panelSize.Height = this._panelSize.Height + size.Height;
			return this._panelSize;
		}

		/// <summary>Arranges and sizes the content of a <see cref="T:System.Windows.Controls.Primitives.ToolBarOverflowPanel" /> object.</summary>
		/// <param name="arrangeBounds">Size that a toolbar overflow panel assumes to position child elements.</param>
		/// <returns>The size of the <see cref="T:System.Windows.Controls.Primitives.ToolBarOverflowPanel" />.</returns>
		// Token: 0x06006101 RID: 24833 RVA: 0x001B378C File Offset: 0x001B198C
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			int start = 0;
			Size size = default(Size);
			double num = 0.0;
			this._wrapWidth = Math.Min(this._wrapWidth, arrangeBounds.Width);
			UIElementCollection children = base.Children;
			for (int i = 0; i < children.Count; i++)
			{
				Size desiredSize = children[i].DesiredSize;
				if (DoubleUtil.GreaterThan(size.Width + desiredSize.Width, this._wrapWidth))
				{
					this.arrangeLine(num, size.Height, start, i);
					num += size.Height;
					start = i;
					size = desiredSize;
				}
				else
				{
					size.Width += desiredSize.Width;
					size.Height = Math.Max(desiredSize.Height, size.Height);
				}
			}
			this.arrangeLine(num, size.Height, start, children.Count);
			return this._panelSize;
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Controls.UIElementCollection" />.</summary>
		/// <param name="logicalParent">Logical parent of the new collection.</param>
		/// <returns>A new collection.</returns>
		// Token: 0x06006102 RID: 24834 RVA: 0x001B387F File Offset: 0x001B1A7F
		protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return new UIElementCollection(this, (base.TemplatedParent == null) ? logicalParent : null);
		}

		// Token: 0x06006103 RID: 24835 RVA: 0x001B3894 File Offset: 0x001B1A94
		private void arrangeLine(double y, double lineHeight, int start, int end)
		{
			double num = 0.0;
			UIElementCollection children = base.Children;
			for (int i = start; i < end; i++)
			{
				UIElement uielement = children[i];
				uielement.Arrange(new Rect(num, y, uielement.DesiredSize.Width, lineHeight));
				num += uielement.DesiredSize.Width;
			}
		}

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x06006104 RID: 24836 RVA: 0x001B38F6 File Offset: 0x001B1AF6
		private ToolBar ToolBar
		{
			get
			{
				return base.TemplatedParent as ToolBar;
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x06006105 RID: 24837 RVA: 0x001B3904 File Offset: 0x001B1B04
		private ToolBarPanel ToolBarPanel
		{
			get
			{
				ToolBar toolBar = this.ToolBar;
				if (toolBar != null)
				{
					return toolBar.ToolBarPanel;
				}
				return null;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ToolBarOverflowPanel.WrapWidth" /> dependency property.</summary>
		// Token: 0x04003135 RID: 12597
		public static readonly DependencyProperty WrapWidthProperty = DependencyProperty.Register("WrapWidth", typeof(double), typeof(ToolBarOverflowPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ToolBarOverflowPanel.IsWrapWidthValid));

		// Token: 0x04003136 RID: 12598
		private double _wrapWidth;

		// Token: 0x04003137 RID: 12599
		private Size _panelSize;
	}
}
