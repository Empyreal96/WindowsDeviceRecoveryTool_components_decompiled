using System;
using System.ComponentModel;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Positions child elements in sequential position from left to right, breaking content to the next line at the edge of the containing box. Subsequent ordering happens sequentially from top to bottom or from right to left, depending on the value of the <see cref="P:System.Windows.Controls.WrapPanel.Orientation" /> property.</summary>
	// Token: 0x02000560 RID: 1376
	public class WrapPanel : Panel
	{
		// Token: 0x06005B4D RID: 23373 RVA: 0x0019BD28 File Offset: 0x00199F28
		static WrapPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.WrapPanel);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.WrapPanel" /> class.</summary>
		// Token: 0x06005B4E RID: 23374 RVA: 0x0019BDFA File Offset: 0x00199FFA
		public WrapPanel()
		{
			this._orientation = (Orientation)WrapPanel.OrientationProperty.GetDefaultValue(base.DependencyObjectType);
		}

		// Token: 0x06005B4F RID: 23375 RVA: 0x0019BE20 File Offset: 0x0019A020
		private static bool IsWidthHeightValid(object value)
		{
			double num = (double)value;
			return DoubleUtil.IsNaN(num) || (num >= 0.0 && !double.IsPositiveInfinity(num));
		}

		/// <summary>Gets or sets a value that specifies the width of all items that are contained within a <see cref="T:System.Windows.Controls.WrapPanel" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the uniform width of all items that are contained within the <see cref="T:System.Windows.Controls.WrapPanel" />. The default value is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x06005B50 RID: 23376 RVA: 0x0019BE55 File Offset: 0x0019A055
		// (set) Token: 0x06005B51 RID: 23377 RVA: 0x0019BE67 File Offset: 0x0019A067
		[TypeConverter(typeof(LengthConverter))]
		public double ItemWidth
		{
			get
			{
				return (double)base.GetValue(WrapPanel.ItemWidthProperty);
			}
			set
			{
				base.SetValue(WrapPanel.ItemWidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the height of all items that are contained within a <see cref="T:System.Windows.Controls.WrapPanel" />. </summary>
		/// <returns>The <see cref="T:System.Double" /> that represents the uniform height of all items that are contained within the <see cref="T:System.Windows.Controls.WrapPanel" />. The default value is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06005B52 RID: 23378 RVA: 0x0019BE7A File Offset: 0x0019A07A
		// (set) Token: 0x06005B53 RID: 23379 RVA: 0x0019BE8C File Offset: 0x0019A08C
		[TypeConverter(typeof(LengthConverter))]
		public double ItemHeight
		{
			get
			{
				return (double)base.GetValue(WrapPanel.ItemHeightProperty);
			}
			set
			{
				base.SetValue(WrapPanel.ItemHeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the dimension in which child content is arranged. </summary>
		/// <returns>An <see cref="T:System.Windows.Controls.Orientation" /> value that represents the physical orientation of content within the <see cref="T:System.Windows.Controls.WrapPanel" /> as horizontal or vertical. The default value is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x06005B54 RID: 23380 RVA: 0x0019BE9F File Offset: 0x0019A09F
		// (set) Token: 0x06005B55 RID: 23381 RVA: 0x0019BEA7 File Offset: 0x0019A0A7
		public Orientation Orientation
		{
			get
			{
				return this._orientation;
			}
			set
			{
				base.SetValue(WrapPanel.OrientationProperty, value);
			}
		}

		// Token: 0x06005B56 RID: 23382 RVA: 0x0019BEBC File Offset: 0x0019A0BC
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WrapPanel wrapPanel = (WrapPanel)d;
			wrapPanel._orientation = (Orientation)e.NewValue;
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.WrapPanel" /> in anticipation of arranging them during the <see cref="M:System.Windows.Controls.WrapPanel.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the desired size of the element.</returns>
		// Token: 0x06005B57 RID: 23383 RVA: 0x0019BEE4 File Offset: 0x0019A0E4
		protected override Size MeasureOverride(Size constraint)
		{
			WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize2 = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize3 = new WrapPanel.UVSize(this.Orientation, constraint.Width, constraint.Height);
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			bool flag = !DoubleUtil.IsNaN(itemWidth);
			bool flag2 = !DoubleUtil.IsNaN(itemHeight);
			Size availableSize = new Size(flag ? itemWidth : constraint.Width, flag2 ? itemHeight : constraint.Height);
			UIElementCollection internalChildren = base.InternalChildren;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					uielement.Measure(availableSize);
					WrapPanel.UVSize uvsize4 = new WrapPanel.UVSize(this.Orientation, flag ? itemWidth : uielement.DesiredSize.Width, flag2 ? itemHeight : uielement.DesiredSize.Height);
					if (DoubleUtil.GreaterThan(uvsize.U + uvsize4.U, uvsize3.U))
					{
						uvsize2.U = Math.Max(uvsize.U, uvsize2.U);
						uvsize2.V += uvsize.V;
						uvsize = uvsize4;
						if (DoubleUtil.GreaterThan(uvsize4.U, uvsize3.U))
						{
							uvsize2.U = Math.Max(uvsize4.U, uvsize2.U);
							uvsize2.V += uvsize4.V;
							uvsize = new WrapPanel.UVSize(this.Orientation);
						}
					}
					else
					{
						uvsize.U += uvsize4.U;
						uvsize.V = Math.Max(uvsize4.V, uvsize.V);
					}
				}
				i++;
			}
			uvsize2.U = Math.Max(uvsize.U, uvsize2.U);
			uvsize2.V += uvsize.V;
			return new Size(uvsize2.Width, uvsize2.Height);
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.WrapPanel" /> element.</summary>
		/// <param name="finalSize">The <see cref="T:System.Windows.Size" /> that this element should use to arrange its child elements.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.WrapPanel" /> element and its children.</returns>
		// Token: 0x06005B58 RID: 23384 RVA: 0x0019C0EC File Offset: 0x0019A2EC
		protected override Size ArrangeOverride(Size finalSize)
		{
			int num = 0;
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			double num2 = 0.0;
			double itemU = (this.Orientation == Orientation.Horizontal) ? itemWidth : itemHeight;
			WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize2 = new WrapPanel.UVSize(this.Orientation, finalSize.Width, finalSize.Height);
			bool flag = !DoubleUtil.IsNaN(itemWidth);
			bool flag2 = !DoubleUtil.IsNaN(itemHeight);
			bool useItemU = (this.Orientation == Orientation.Horizontal) ? flag : flag2;
			UIElementCollection internalChildren = base.InternalChildren;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					WrapPanel.UVSize uvsize3 = new WrapPanel.UVSize(this.Orientation, flag ? itemWidth : uielement.DesiredSize.Width, flag2 ? itemHeight : uielement.DesiredSize.Height);
					if (DoubleUtil.GreaterThan(uvsize.U + uvsize3.U, uvsize2.U))
					{
						this.arrangeLine(num2, uvsize.V, num, i, useItemU, itemU);
						num2 += uvsize.V;
						uvsize = uvsize3;
						if (DoubleUtil.GreaterThan(uvsize3.U, uvsize2.U))
						{
							this.arrangeLine(num2, uvsize3.V, i, ++i, useItemU, itemU);
							num2 += uvsize3.V;
							uvsize = new WrapPanel.UVSize(this.Orientation);
						}
						num = i;
					}
					else
					{
						uvsize.U += uvsize3.U;
						uvsize.V = Math.Max(uvsize3.V, uvsize.V);
					}
				}
				i++;
			}
			if (num < internalChildren.Count)
			{
				this.arrangeLine(num2, uvsize.V, num, internalChildren.Count, useItemU, itemU);
			}
			return finalSize;
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x0019C2C4 File Offset: 0x0019A4C4
		private void arrangeLine(double v, double lineV, int start, int end, bool useItemU, double itemU)
		{
			double num = 0.0;
			bool flag = this.Orientation == Orientation.Horizontal;
			UIElementCollection internalChildren = base.InternalChildren;
			for (int i = start; i < end; i++)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation, uielement.DesiredSize.Width, uielement.DesiredSize.Height);
					double num2 = useItemU ? itemU : uvsize.U;
					uielement.Arrange(new Rect(flag ? num : v, flag ? v : num, flag ? num2 : lineV, flag ? lineV : num2));
					num += num2;
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.WrapPanel.ItemWidth" />  dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.WrapPanel.ItemWidth" />  dependency property.</returns>
		// Token: 0x04002F71 RID: 12145
		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(WrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(WrapPanel.IsWidthHeightValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.WrapPanel.ItemHeight" />  dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.WrapPanel.ItemHeight" />  dependency property.</returns>
		// Token: 0x04002F72 RID: 12146
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(WrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(WrapPanel.IsWidthHeightValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.WrapPanel.Orientation" />  dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.WrapPanel.Orientation" />  dependency property.</returns>
		// Token: 0x04002F73 RID: 12147
		public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(WrapPanel), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(WrapPanel.OnOrientationChanged)));

		// Token: 0x04002F74 RID: 12148
		private Orientation _orientation;

		// Token: 0x020009DF RID: 2527
		private struct UVSize
		{
			// Token: 0x06008943 RID: 35139 RVA: 0x002548F0 File Offset: 0x00252AF0
			internal UVSize(Orientation orientation, double width, double height)
			{
				this.U = (this.V = 0.0);
				this._orientation = orientation;
				this.Width = width;
				this.Height = height;
			}

			// Token: 0x06008944 RID: 35140 RVA: 0x0025492C File Offset: 0x00252B2C
			internal UVSize(Orientation orientation)
			{
				this.U = (this.V = 0.0);
				this._orientation = orientation;
			}

			// Token: 0x17001F08 RID: 7944
			// (get) Token: 0x06008945 RID: 35141 RVA: 0x00254958 File Offset: 0x00252B58
			// (set) Token: 0x06008946 RID: 35142 RVA: 0x0025496F File Offset: 0x00252B6F
			internal double Width
			{
				get
				{
					if (this._orientation != Orientation.Horizontal)
					{
						return this.V;
					}
					return this.U;
				}
				set
				{
					if (this._orientation == Orientation.Horizontal)
					{
						this.U = value;
						return;
					}
					this.V = value;
				}
			}

			// Token: 0x17001F09 RID: 7945
			// (get) Token: 0x06008947 RID: 35143 RVA: 0x00254988 File Offset: 0x00252B88
			// (set) Token: 0x06008948 RID: 35144 RVA: 0x0025499F File Offset: 0x00252B9F
			internal double Height
			{
				get
				{
					if (this._orientation != Orientation.Horizontal)
					{
						return this.U;
					}
					return this.V;
				}
				set
				{
					if (this._orientation == Orientation.Horizontal)
					{
						this.V = value;
						return;
					}
					this.U = value;
				}
			}

			// Token: 0x0400463F RID: 17983
			internal double U;

			// Token: 0x04004640 RID: 17984
			internal double V;

			// Token: 0x04004641 RID: 17985
			private Orientation _orientation;
		}
	}
}
