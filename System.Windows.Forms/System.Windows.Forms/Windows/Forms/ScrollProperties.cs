using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Encapsulates properties related to scrolling. </summary>
	// Token: 0x0200034D RID: 845
	public abstract class ScrollProperties
	{
		/// <summary>Gets the control to which this scroll information applies.</summary>
		/// <returns>The control to which this scroll information applies.</returns>
		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x060034F4 RID: 13556 RVA: 0x000F1C10 File Offset: 0x000EFE10
		protected ScrollableControl ParentControl
		{
			get
			{
				return this.parent;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ScrollProperties" /> class. </summary>
		/// <param name="container">The <see cref="T:System.Windows.Forms.ScrollableControl" /> whose scrolling properties this object describes.</param>
		// Token: 0x060034F5 RID: 13557 RVA: 0x000F1C18 File Offset: 0x000EFE18
		protected ScrollProperties(ScrollableControl container)
		{
			this.parent = container;
		}

		/// <summary>Gets or sets whether the scroll bar can be used on the container.</summary>
		/// <returns>
		///     <see langword="true" /> if the scroll bar can be used; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x060034F6 RID: 13558 RVA: 0x000F1C45 File Offset: 0x000EFE45
		// (set) Token: 0x060034F7 RID: 13559 RVA: 0x000F1C4D File Offset: 0x000EFE4D
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ScrollBarEnableDescr")]
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.parent.AutoScroll)
				{
					return;
				}
				if (value != this.enabled)
				{
					this.enabled = value;
					this.EnableScroll(value);
				}
			}
		}

		/// <summary>Gets or sets the distance to move a scroll bar in response to a large scroll command. </summary>
		/// <returns>An <see cref="T:System.Int32" /> describing how far, in pixels, to move the scroll bar in response to a large change.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <see cref="P:System.Windows.Forms.ScrollProperties.LargeChange" /> cannot be less than zero. </exception>
		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x060034F8 RID: 13560 RVA: 0x000F1C74 File Offset: 0x000EFE74
		// (set) Token: 0x060034F9 RID: 13561 RVA: 0x000F1C90 File Offset: 0x000EFE90
		[SRCategory("CatBehavior")]
		[DefaultValue(10)]
		[SRDescription("ScrollBarLargeChangeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int LargeChange
		{
			get
			{
				return Math.Min(this.largeChange, this.maximum - this.minimum + 1);
			}
			set
			{
				if (this.largeChange != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("LargeChange", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"LargeChange",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.largeChange = value;
					this.largeChangeSetExternally = true;
					this.UpdateScrollInfo();
				}
			}
		}

		/// <summary>Gets or sets the upper limit of the scrollable range. </summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the maximum range of the scroll bar.</returns>
		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x060034FA RID: 13562 RVA: 0x000F1D01 File Offset: 0x000EFF01
		// (set) Token: 0x060034FB RID: 13563 RVA: 0x000F1D0C File Offset: 0x000EFF0C
		[SRCategory("CatBehavior")]
		[DefaultValue(100)]
		[SRDescription("ScrollBarMaximumDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (this.parent.AutoScroll)
				{
					return;
				}
				if (this.maximum != value)
				{
					if (this.minimum > value)
					{
						this.minimum = value;
					}
					if (value < this.value)
					{
						this.Value = value;
					}
					this.maximum = value;
					this.maximumSetExternally = true;
					this.UpdateScrollInfo();
				}
			}
		}

		/// <summary>Gets or sets the lower limit of the scrollable range. </summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the lower range of the scroll bar.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <see cref="P:System.Windows.Forms.ScrollProperties.Minimum" /> cannot be less than zero. </exception>
		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x060034FC RID: 13564 RVA: 0x000F1D64 File Offset: 0x000EFF64
		// (set) Token: 0x060034FD RID: 13565 RVA: 0x000F1D6C File Offset: 0x000EFF6C
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[SRDescription("ScrollBarMinimumDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				if (this.parent.AutoScroll)
				{
					return;
				}
				if (this.minimum != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Minimum", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"Minimum",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.maximum < value)
					{
						this.maximum = value;
					}
					if (value > this.value)
					{
						this.value = value;
					}
					this.minimum = value;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x060034FE RID: 13566
		internal abstract int PageSize { get; }

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x060034FF RID: 13567
		internal abstract int Orientation { get; }

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06003500 RID: 13568
		internal abstract int HorizontalDisplayPosition { get; }

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06003501 RID: 13569
		internal abstract int VerticalDisplayPosition { get; }

		/// <summary>Gets or sets the distance to move a scroll bar in response to a small scroll command. </summary>
		/// <returns>An <see cref="T:System.Int32" /> representing how far, in pixels, to move the scroll bar.</returns>
		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06003502 RID: 13570 RVA: 0x000F1E04 File Offset: 0x000F0004
		// (set) Token: 0x06003503 RID: 13571 RVA: 0x000F1E18 File Offset: 0x000F0018
		[SRCategory("CatBehavior")]
		[DefaultValue(1)]
		[SRDescription("ScrollBarSmallChangeDescr")]
		public int SmallChange
		{
			get
			{
				return Math.Min(this.smallChange, this.LargeChange);
			}
			set
			{
				if (this.smallChange != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("SmallChange", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"SmallChange",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.smallChange = value;
					this.smallChangeSetExternally = true;
					this.UpdateScrollInfo();
				}
			}
		}

		/// <summary>Gets or sets a numeric value that represents the current position of the scroll bar box.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the position of the scroll bar box, in pixels. </returns>
		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06003504 RID: 13572 RVA: 0x000F1E89 File Offset: 0x000F0089
		// (set) Token: 0x06003505 RID: 13573 RVA: 0x000F1E94 File Offset: 0x000F0094
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Bindable(true)]
		[SRDescription("ScrollBarValueDescr")]
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					if (value < this.minimum || value > this.maximum)
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'minimum'",
							"'maximum'"
						}));
					}
					this.value = value;
					this.UpdateScrollInfo();
					this.parent.SetDisplayFromScrollProps(this.HorizontalDisplayPosition, this.VerticalDisplayPosition);
				}
			}
		}

		/// <summary>Gets or sets whether the scroll bar can be seen by the user.</summary>
		/// <returns>
		///     <see langword="true" /> if it can be seen; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06003506 RID: 13574 RVA: 0x000F1F22 File Offset: 0x000F0122
		// (set) Token: 0x06003507 RID: 13575 RVA: 0x000F1F2C File Offset: 0x000F012C
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ScrollBarVisibleDescr")]
		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (this.parent.AutoScroll)
				{
					return;
				}
				if (value != this.visible)
				{
					this.visible = value;
					this.parent.UpdateStylesCore();
					this.UpdateScrollInfo();
					this.parent.SetDisplayFromScrollProps(this.HorizontalDisplayPosition, this.VerticalDisplayPosition);
				}
			}
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000F1F80 File Offset: 0x000F0180
		internal void UpdateScrollInfo()
		{
			if (this.parent.IsHandleCreated && this.visible)
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
				scrollinfo.fMask = 23;
				scrollinfo.nMin = this.minimum;
				scrollinfo.nMax = this.maximum;
				scrollinfo.nPage = (this.parent.AutoScroll ? this.PageSize : this.LargeChange);
				scrollinfo.nPos = this.value;
				scrollinfo.nTrackPos = 0;
				UnsafeNativeMethods.SetScrollInfo(new HandleRef(this.parent, this.parent.Handle), this.Orientation, scrollinfo, true);
			}
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000F203C File Offset: 0x000F023C
		private void EnableScroll(bool enable)
		{
			if (enable)
			{
				UnsafeNativeMethods.EnableScrollBar(new HandleRef(this.parent, this.parent.Handle), this.Orientation, 0);
				return;
			}
			UnsafeNativeMethods.EnableScrollBar(new HandleRef(this.parent, this.parent.Handle), this.Orientation, 3);
		}

		// Token: 0x04002087 RID: 8327
		internal int minimum;

		// Token: 0x04002088 RID: 8328
		internal int maximum = 100;

		// Token: 0x04002089 RID: 8329
		internal int smallChange = 1;

		// Token: 0x0400208A RID: 8330
		internal int largeChange = 10;

		// Token: 0x0400208B RID: 8331
		internal int value;

		// Token: 0x0400208C RID: 8332
		internal bool maximumSetExternally;

		// Token: 0x0400208D RID: 8333
		internal bool smallChangeSetExternally;

		// Token: 0x0400208E RID: 8334
		internal bool largeChangeSetExternally;

		// Token: 0x0400208F RID: 8335
		private ScrollableControl parent;

		// Token: 0x04002090 RID: 8336
		private const int SCROLL_LINE = 5;

		// Token: 0x04002091 RID: 8337
		internal bool visible;

		// Token: 0x04002092 RID: 8338
		private bool enabled = true;
	}
}
