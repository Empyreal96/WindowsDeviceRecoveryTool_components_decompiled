using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a control consisting of a movable bar that divides a container's display area into two resizable panels. </summary>
	// Token: 0x0200035C RID: 860
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("SplitterMoved")]
	[Docking(DockingBehavior.AutoDock)]
	[Designer("System.Windows.Forms.Design.SplitContainerDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionSplitContainer")]
	public class SplitContainer : ContainerControl, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.SplitContainer" /> class.</summary>
		// Token: 0x0600354F RID: 13647 RVA: 0x000F3B40 File Offset: 0x000F1D40
		public SplitContainer()
		{
			this.panel1 = new SplitterPanel(this);
			this.panel2 = new SplitterPanel(this);
			this.splitterRect = default(Rectangle);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			((WindowsFormsUtils.TypedControlCollection)this.Controls).AddInternal(this.panel1);
			((WindowsFormsUtils.TypedControlCollection)this.Controls).AddInternal(this.panel2);
			this.UpdateSplitter();
		}

		/// <summary>When overridden in a derived class, gets or sets a value indicating whether scroll bars automatically appear if controls are placed outside the <see cref="T:System.Windows.Forms.SplitContainer" /> client area. This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if scroll bars to automatically appear when controls are placed outside the <see cref="T:System.Windows.Forms.SplitContainer" /> client area; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06003550 RID: 13648 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x06003551 RID: 13649 RVA: 0x000E3A46 File Offset: 0x000E1C46
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("FormAutoScrollDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoScroll
		{
			get
			{
				return false;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> value.</returns>
		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06003552 RID: 13650 RVA: 0x000F3C26 File Offset: 0x000F1E26
		// (set) Token: 0x06003553 RID: 13651 RVA: 0x000F3C2E File Offset: 0x000F1E2E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(typeof(Point), "0, 0")]
		public override Point AutoScrollOffset
		{
			get
			{
				return base.AutoScrollOffset;
			}
			set
			{
				base.AutoScrollOffset = value;
			}
		}

		/// <summary>Gets or sets the minimum size of the scroll bar. This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the minimum height and width of the scroll bar, in pixels.</returns>
		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06003554 RID: 13652 RVA: 0x000F3C37 File Offset: 0x000F1E37
		// (set) Token: 0x06003555 RID: 13653 RVA: 0x000F3C3F File Offset: 0x000F1E3F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		/// <summary>Gets or sets the size of the auto-scroll margin. This property is not relevant to this class. This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> value that represents the height and width, in pixels, of the auto-scroll margin.</returns>
		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06003556 RID: 13654 RVA: 0x000F3C48 File Offset: 0x000F1E48
		// (set) Token: 0x06003557 RID: 13655 RVA: 0x000F3C50 File Offset: 0x000F1E50
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> value.</returns>
		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06003558 RID: 13656 RVA: 0x000F3C59 File Offset: 0x000F1E59
		// (set) Token: 0x06003559 RID: 13657 RVA: 0x000F3C61 File Offset: 0x000F1E61
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormAutoScrollPositionDescr")]
		public new Point AutoScrollPosition
		{
			get
			{
				return base.AutoScrollPosition;
			}
			set
			{
				base.AutoScrollPosition = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.SplitContainer" /> is automatically resized to display its entire contents. This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.SplitContainer" /> is automatically resized; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x0600355A RID: 13658 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x0600355B RID: 13659 RVA: 0x000B0BCE File Offset: 0x000AEDCE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.SplitContainer.AutoSize" /> property changes. This property is not relevant to this class.</summary>
		// Token: 0x1400028E RID: 654
		// (add) Token: 0x0600355C RID: 13660 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x0600355D RID: 13661 RVA: 0x0001BA37 File Offset: 0x00019C37
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		/// <summary>Gets or sets the background image displayed in the control.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the image to display in the background of the control.</returns>
		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x0600355E RID: 13662 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x0600355F RID: 13663 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageLayout" /> value.</returns>
		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06003560 RID: 13664 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06003561 RID: 13665 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.BindingContext" /> for the <see cref="T:System.Windows.Forms.SplitContainer" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.BindingContext" /> for the control.</returns>
		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003562 RID: 13666 RVA: 0x00027AB1 File Offset: 0x00025CB1
		// (set) Token: 0x06003563 RID: 13667 RVA: 0x00027AB9 File Offset: 0x00025CB9
		[Browsable(false)]
		[SRDescription("ContainerControlBindingContextDescr")]
		public override BindingContext BindingContext
		{
			get
			{
				return base.BindingContextInternal;
			}
			set
			{
				base.BindingContextInternal = value;
			}
		}

		/// <summary>Gets or sets the style of border for the <see cref="T:System.Windows.Forms.SplitContainer" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see cref="F:System.Windows.Forms.BorderStyle.Fixed3D" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value of the property is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003564 RID: 13668 RVA: 0x000F3C6A File Offset: 0x000F1E6A
		// (set) Token: 0x06003565 RID: 13669 RVA: 0x000F3C74 File Offset: 0x000F1E74
		[DefaultValue(BorderStyle.None)]
		[SRCategory("CatAppearance")]
		[DispId(-504)]
		[SRDescription("SplitterBorderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.Invalidate();
					this.SetInnerMostBorder(this);
					if (this.ParentInternal != null && this.ParentInternal is SplitterPanel)
					{
						SplitContainer owner = ((SplitterPanel)this.ParentInternal).Owner;
						owner.SetInnerMostBorder(owner);
					}
				}
				switch (this.BorderStyle)
				{
				case BorderStyle.None:
					this.BORDERSIZE = 0;
					return;
				case BorderStyle.FixedSingle:
					this.BORDERSIZE = 1;
					return;
				case BorderStyle.Fixed3D:
					this.BORDERSIZE = 4;
					return;
				default:
					return;
				}
			}
		}

		/// <summary>Gets a collection of child controls. This property is not relevant to this class.</summary>
		/// <returns>An object of type <see cref="T:System.Windows.Forms.Control.ControlCollection" /> that contains the child controls.</returns>
		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003566 RID: 13670 RVA: 0x000E3CDA File Offset: 0x000E1EDA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Control.ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400028F RID: 655
		// (add) Token: 0x06003567 RID: 13671 RVA: 0x000F3D22 File Offset: 0x000F1F22
		// (remove) Token: 0x06003568 RID: 13672 RVA: 0x000F3D2B File Offset: 0x000F1F2B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event ControlEventHandler ControlAdded
		{
			add
			{
				base.ControlAdded += value;
			}
			remove
			{
				base.ControlAdded -= value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000290 RID: 656
		// (add) Token: 0x06003569 RID: 13673 RVA: 0x000F3D34 File Offset: 0x000F1F34
		// (remove) Token: 0x0600356A RID: 13674 RVA: 0x000F3D3D File Offset: 0x000F1F3D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event ControlEventHandler ControlRemoved
		{
			add
			{
				base.ControlRemoved += value;
			}
			remove
			{
				base.ControlRemoved -= value;
			}
		}

		/// <summary>Gets or sets which <see cref="T:System.Windows.Forms.SplitContainer" /> borders are attached to the edges of the container.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default value is <see langword="None" />.</returns>
		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x0600356B RID: 13675 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x0600356C RID: 13676 RVA: 0x000F3D50 File Offset: 0x000F1F50
		public new DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
				if (this.ParentInternal != null && this.ParentInternal is SplitterPanel)
				{
					SplitContainer owner = ((SplitterPanel)this.ParentInternal).Owner;
					owner.SetInnerMostBorder(owner);
				}
				this.ResizeSplitContainer();
			}
		}

		/// <summary>Gets the default size of the <see cref="T:System.Windows.Forms.SplitContainer" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the <see cref="T:System.Windows.Forms.SplitContainer" />.</returns>
		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x0600356D RID: 13677 RVA: 0x000F3D97 File Offset: 0x000F1F97
		protected override Size DefaultSize
		{
			get
			{
				return new Size(150, 100);
			}
		}

		/// <summary>Gets or sets which <see cref="T:System.Windows.Forms.SplitContainer" /> panel remains the same size when the container is resized.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.FixedPanel" />. The default value is <see langword="None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.FixedPanel" /> values.</exception>
		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x0600356E RID: 13678 RVA: 0x000F3DA5 File Offset: 0x000F1FA5
		// (set) Token: 0x0600356F RID: 13679 RVA: 0x000F3DB0 File Offset: 0x000F1FB0
		[DefaultValue(FixedPanel.None)]
		[SRCategory("CatLayout")]
		[SRDescription("SplitContainerFixedPanelDescr")]
		public FixedPanel FixedPanel
		{
			get
			{
				return this.fixedPanel;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FixedPanel));
				}
				if (this.fixedPanel != value)
				{
					this.fixedPanel = value;
					FixedPanel fixedPanel = this.fixedPanel;
					if (fixedPanel == FixedPanel.Panel2)
					{
						if (this.Orientation == Orientation.Vertical)
						{
							this.panelSize = base.Width - this.SplitterDistanceInternal - this.SplitterWidthInternal;
							return;
						}
						this.panelSize = base.Height - this.SplitterDistanceInternal - this.SplitterWidthInternal;
						return;
					}
					else
					{
						this.panelSize = this.SplitterDistanceInternal;
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the splitter is fixed or movable.</summary>
		/// <returns>
		///     <see langword="true" /> if the splitter is fixed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003570 RID: 13680 RVA: 0x000F3E49 File Offset: 0x000F2049
		// (set) Token: 0x06003571 RID: 13681 RVA: 0x000F3E51 File Offset: 0x000F2051
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("SplitContainerIsSplitterFixedDescr")]
		public bool IsSplitterFixed
		{
			get
			{
				return this.splitterFixed;
			}
			set
			{
				this.splitterFixed = value;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003572 RID: 13682 RVA: 0x000F3E5C File Offset: 0x000F205C
		private bool IsSplitterMovable
		{
			get
			{
				if (this.Orientation == Orientation.Vertical)
				{
					return base.Width >= this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize;
				}
				return base.Height >= this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003573 RID: 13683 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool IsContainerControl
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a value indicating the horizontal or vertical orientation of the <see cref="T:System.Windows.Forms.SplitContainer" /> panels.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Orientation" /> values. The default is <see langword="Vertical" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.Orientation" /> values.</exception>
		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x000F3EB1 File Offset: 0x000F20B1
		// (set) Token: 0x06003575 RID: 13685 RVA: 0x000F3EBC File Offset: 0x000F20BC
		[SRCategory("CatBehavior")]
		[DefaultValue(Orientation.Vertical)]
		[Localizable(true)]
		[SRDescription("SplitContainerOrientationDescr")]
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Orientation));
				}
				if (this.orientation != value)
				{
					this.orientation = value;
					this.splitDistance = 0;
					this.SplitterDistance = this.SplitterDistanceInternal;
					this.UpdateSplitter();
				}
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x000F3F18 File Offset: 0x000F2118
		// (set) Token: 0x06003577 RID: 13687 RVA: 0x000F3F20 File Offset: 0x000F2120
		private Cursor OverrideCursor
		{
			get
			{
				return this.overrideCursor;
			}
			set
			{
				if (this.overrideCursor != value)
				{
					this.overrideCursor = value;
					if (base.IsHandleCreated)
					{
						NativeMethods.POINT point = new NativeMethods.POINT();
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						UnsafeNativeMethods.GetCursorPos(point);
						UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
						if ((rect.left <= point.x && point.x < rect.right && rect.top <= point.y && point.y < rect.bottom) || UnsafeNativeMethods.GetCapture() == base.Handle)
						{
							base.SendMessage(32, base.Handle, 1);
						}
					}
				}
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x000F3FD3 File Offset: 0x000F21D3
		private bool CollapsedMode
		{
			get
			{
				return this.Panel1Collapsed || this.Panel2Collapsed;
			}
		}

		/// <summary>Gets the left or top panel of the <see cref="T:System.Windows.Forms.SplitContainer" />, depending on <see cref="P:System.Windows.Forms.SplitContainer.Orientation" />.</summary>
		/// <returns>If <see cref="P:System.Windows.Forms.SplitContainer.Orientation" /> is <see langword="Vertical" />, the left panel of the <see cref="T:System.Windows.Forms.SplitContainer" />. If <see cref="P:System.Windows.Forms.SplitContainer.Orientation" /> is <see langword="Horizontal" />, the top panel of the <see cref="T:System.Windows.Forms.SplitContainer" />.</returns>
		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003579 RID: 13689 RVA: 0x000F3FE5 File Offset: 0x000F21E5
		[SRCategory("CatAppearance")]
		[SRDescription("SplitContainerPanel1Descr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SplitterPanel Panel1
		{
			get
			{
				return this.panel1;
			}
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000F3FED File Offset: 0x000F21ED
		private void CollapsePanel(SplitterPanel p, bool collapsing)
		{
			p.Collapsed = collapsing;
			if (collapsing)
			{
				p.Visible = false;
			}
			else
			{
				p.Visible = true;
			}
			this.UpdateSplitter();
		}

		/// <summary>Gets or sets the interior spacing, in pixels, between the edges of a <see cref="T:System.Windows.Forms.SplitterPanel" /> and its contents. This property is not relevant to this class.</summary>
		/// <returns>An object of type <see cref="T:System.Windows.Forms.Padding" /> representing the interior spacing.</returns>
		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x0600357B RID: 13691 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x0600357C RID: 13692 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000291 RID: 657
		// (add) Token: 0x0600357D RID: 13693 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x0600357E RID: 13694 RVA: 0x000204B4 File Offset: 0x0001E6B4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		/// <summary>Gets or sets a value determining whether <see cref="P:System.Windows.Forms.SplitContainer.Panel1" /> is collapsed or expanded.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="P:System.Windows.Forms.SplitContainer.Panel1" /> is collapsed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x0600357F RID: 13695 RVA: 0x000F400F File Offset: 0x000F220F
		// (set) Token: 0x06003580 RID: 13696 RVA: 0x000F401C File Offset: 0x000F221C
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		[SRDescription("SplitContainerPanel1CollapsedDescr")]
		public bool Panel1Collapsed
		{
			get
			{
				return this.panel1.Collapsed;
			}
			set
			{
				if (value != this.panel1.Collapsed)
				{
					if (value && this.panel2.Collapsed)
					{
						this.CollapsePanel(this.panel2, false);
					}
					this.CollapsePanel(this.panel1, value);
				}
			}
		}

		/// <summary>Gets or sets a value determining whether <see cref="P:System.Windows.Forms.SplitContainer.Panel2" /> is collapsed or expanded.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="P:System.Windows.Forms.SplitContainer.Panel2" /> is collapsed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06003581 RID: 13697 RVA: 0x000F4056 File Offset: 0x000F2256
		// (set) Token: 0x06003582 RID: 13698 RVA: 0x000F4063 File Offset: 0x000F2263
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		[SRDescription("SplitContainerPanel2CollapsedDescr")]
		public bool Panel2Collapsed
		{
			get
			{
				return this.panel2.Collapsed;
			}
			set
			{
				if (value != this.panel2.Collapsed)
				{
					if (value && this.panel1.Collapsed)
					{
						this.CollapsePanel(this.panel1, false);
					}
					this.CollapsePanel(this.panel2, value);
				}
			}
		}

		/// <summary>Gets or sets the minimum distance in pixels of the splitter from the left or top edge of <see cref="P:System.Windows.Forms.SplitContainer.Panel1" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the minimum distance in pixels of the splitter from the left or top edge of <see cref="P:System.Windows.Forms.SplitContainer.Panel1" />. The default value is 25 pixels, regardless of <see cref="P:System.Windows.Forms.SplitContainer.Orientation" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value is incompatible with the orientation. </exception>
		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06003583 RID: 13699 RVA: 0x000F409D File Offset: 0x000F229D
		// (set) Token: 0x06003584 RID: 13700 RVA: 0x000F40A5 File Offset: 0x000F22A5
		[SRCategory("CatLayout")]
		[DefaultValue(25)]
		[Localizable(true)]
		[SRDescription("SplitContainerPanel1MinSizeDescr")]
		[RefreshProperties(RefreshProperties.All)]
		public int Panel1MinSize
		{
			get
			{
				return this.panel1MinSize;
			}
			set
			{
				this.newPanel1MinSize = value;
				if (value != this.Panel1MinSize && !this.initializing)
				{
					this.ApplyPanel1MinSize(value);
				}
			}
		}

		/// <summary>Gets the right or bottom panel of the <see cref="T:System.Windows.Forms.SplitContainer" />, depending on <see cref="P:System.Windows.Forms.SplitContainer.Orientation" />.</summary>
		/// <returns>If <see cref="P:System.Windows.Forms.SplitContainer.Orientation" /> is <see langword="Vertical" />, the right panel of the <see cref="T:System.Windows.Forms.SplitContainer" />. If <see cref="P:System.Windows.Forms.SplitContainer.Orientation" /> is <see langword="Horizontal" />, the bottom panel of the <see cref="T:System.Windows.Forms.SplitContainer" />.</returns>
		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06003585 RID: 13701 RVA: 0x000F40C6 File Offset: 0x000F22C6
		[SRCategory("CatAppearance")]
		[SRDescription("SplitContainerPanel2Descr")]
		[Localizable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SplitterPanel Panel2
		{
			get
			{
				return this.panel2;
			}
		}

		/// <summary>Gets or sets the minimum distance in pixels of the splitter from the right or bottom edge of <see cref="P:System.Windows.Forms.SplitContainer.Panel2" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the minimum distance in pixels of the splitter from the right or bottom edge of <see cref="P:System.Windows.Forms.SplitContainer.Panel2" />. The default value is 25 pixels, regardless of <see cref="P:System.Windows.Forms.SplitContainer.Orientation" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value is incompatible with the orientation.</exception>
		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06003586 RID: 13702 RVA: 0x000F40CE File Offset: 0x000F22CE
		// (set) Token: 0x06003587 RID: 13703 RVA: 0x000F40D6 File Offset: 0x000F22D6
		[SRCategory("CatLayout")]
		[DefaultValue(25)]
		[Localizable(true)]
		[SRDescription("SplitContainerPanel2MinSizeDescr")]
		[RefreshProperties(RefreshProperties.All)]
		public int Panel2MinSize
		{
			get
			{
				return this.panel2MinSize;
			}
			set
			{
				this.newPanel2MinSize = value;
				if (value != this.Panel2MinSize && !this.initializing)
				{
					this.ApplyPanel2MinSize(value);
				}
			}
		}

		/// <summary>Gets or sets the location of the splitter, in pixels, from the left or top edge of the <see cref="T:System.Windows.Forms.SplitContainer" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the location of the splitter, in pixels, from the left or top edge of the <see cref="T:System.Windows.Forms.SplitContainer" />. The default value is 50 pixels.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than zero. </exception>
		/// <exception cref="T:System.InvalidOperationException">The value is incompatible with the orientation.</exception>
		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x000F40F7 File Offset: 0x000F22F7
		// (set) Token: 0x06003589 RID: 13705 RVA: 0x000F4100 File Offset: 0x000F2300
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SettingsBindable(true)]
		[SRDescription("SplitContainerSplitterDistanceDescr")]
		[DefaultValue(50)]
		public int SplitterDistance
		{
			get
			{
				return this.splitDistance;
			}
			set
			{
				if (value != this.SplitterDistance)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("SplitterDistance", SR.GetString("InvalidLowBoundArgument", new object[]
						{
							"SplitterDistance",
							value.ToString(CultureInfo.CurrentCulture),
							"0"
						}));
					}
					try
					{
						this.setSplitterDistance = true;
						if (this.Orientation == Orientation.Vertical)
						{
							if (value < this.Panel1MinSize)
							{
								value = this.Panel1MinSize;
							}
							if (value + this.SplitterWidthInternal > base.Width - this.Panel2MinSize)
							{
								value = base.Width - this.Panel2MinSize - this.SplitterWidthInternal;
							}
							if (value < 0)
							{
								throw new InvalidOperationException(SR.GetString("SplitterDistanceNotAllowed"));
							}
							this.splitDistance = value;
							this.splitterDistance = value;
							this.panel1.WidthInternal = this.SplitterDistance;
						}
						else
						{
							if (value < this.Panel1MinSize)
							{
								value = this.Panel1MinSize;
							}
							if (value + this.SplitterWidthInternal > base.Height - this.Panel2MinSize)
							{
								value = base.Height - this.Panel2MinSize - this.SplitterWidthInternal;
							}
							if (value < 0)
							{
								throw new InvalidOperationException(SR.GetString("SplitterDistanceNotAllowed"));
							}
							this.splitDistance = value;
							this.splitterDistance = value;
							this.panel1.HeightInternal = this.SplitterDistance;
						}
						FixedPanel fixedPanel = this.fixedPanel;
						if (fixedPanel != FixedPanel.Panel1)
						{
							if (fixedPanel == FixedPanel.Panel2)
							{
								if (this.Orientation == Orientation.Vertical)
								{
									this.panelSize = base.Width - this.SplitterDistance - this.SplitterWidthInternal;
								}
								else
								{
									this.panelSize = base.Height - this.SplitterDistance - this.SplitterWidthInternal;
								}
							}
						}
						else
						{
							this.panelSize = this.SplitterDistance;
						}
						this.UpdateSplitter();
					}
					finally
					{
						this.setSplitterDistance = false;
					}
					this.OnSplitterMoved(new SplitterEventArgs(this.SplitterRectangle.X + this.SplitterRectangle.Width / 2, this.SplitterRectangle.Y + this.SplitterRectangle.Height / 2, this.SplitterRectangle.X, this.SplitterRectangle.Y));
				}
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x0600358A RID: 13706 RVA: 0x000F433C File Offset: 0x000F253C
		// (set) Token: 0x0600358B RID: 13707 RVA: 0x000F4344 File Offset: 0x000F2544
		private int SplitterDistanceInternal
		{
			get
			{
				return this.splitterDistance;
			}
			set
			{
				this.SplitterDistance = value;
			}
		}

		/// <summary>Gets or sets a value representing the increment of splitter movement in pixels.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the increment of splitter movement in pixels. The default value is one pixel.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than one. </exception>
		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x0600358C RID: 13708 RVA: 0x000F434D File Offset: 0x000F254D
		// (set) Token: 0x0600358D RID: 13709 RVA: 0x000F4358 File Offset: 0x000F2558
		[SRCategory("CatLayout")]
		[DefaultValue(1)]
		[Localizable(true)]
		[SRDescription("SplitContainerSplitterIncrementDescr")]
		public int SplitterIncrement
		{
			get
			{
				return this.splitterInc;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("SplitterIncrement", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"SplitterIncrement",
						value.ToString(CultureInfo.CurrentCulture),
						"1"
					}));
				}
				this.splitterInc = value;
			}
		}

		/// <summary>Gets the size and location of the splitter relative to the <see cref="T:System.Windows.Forms.SplitContainer" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that specifies the size and location of the splitter relative to the <see cref="T:System.Windows.Forms.SplitContainer" />.</returns>
		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x0600358E RID: 13710 RVA: 0x000F43AC File Offset: 0x000F25AC
		[SRCategory("CatLayout")]
		[SRDescription("SplitContainerSplitterRectangleDescr")]
		[Browsable(false)]
		public Rectangle SplitterRectangle
		{
			get
			{
				Rectangle result = this.splitterRect;
				result.X = this.splitterRect.X - base.Left;
				result.Y = this.splitterRect.Y - base.Top;
				return result;
			}
		}

		/// <summary>Gets or sets the width of the splitter in pixels.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the width of the splitter, in pixels. The default is four pixels.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than one or is incompatible with the orientation. </exception>
		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x0600358F RID: 13711 RVA: 0x000F43F3 File Offset: 0x000F25F3
		// (set) Token: 0x06003590 RID: 13712 RVA: 0x000F43FB File Offset: 0x000F25FB
		[SRCategory("CatLayout")]
		[SRDescription("SplitContainerSplitterWidthDescr")]
		[Localizable(true)]
		[DefaultValue(4)]
		public int SplitterWidth
		{
			get
			{
				return this.splitterWidth;
			}
			set
			{
				this.newSplitterWidth = value;
				if (value != this.SplitterWidth && !this.initializing)
				{
					this.ApplySplitterWidth(value);
				}
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06003591 RID: 13713 RVA: 0x000F441C File Offset: 0x000F261C
		private int SplitterWidthInternal
		{
			get
			{
				if (!this.CollapsedMode)
				{
					return this.splitterWidth;
				}
				return 0;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to the splitter using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to the splitter using the TAB key; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06003592 RID: 13714 RVA: 0x000F442E File Offset: 0x000F262E
		// (set) Token: 0x06003593 RID: 13715 RVA: 0x000F4436 File Offset: 0x000F2636
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[DispId(-516)]
		[SRDescription("ControlTabStopDescr")]
		public new bool TabStop
		{
			get
			{
				return this.tabStop;
			}
			set
			{
				if (this.TabStop != value)
				{
					this.tabStop = value;
					this.OnTabStopChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>A string.</returns>
		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06003594 RID: 13716 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06003595 RID: 13717 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>Signals the object that initialization is started.</summary>
		// Token: 0x06003596 RID: 13718 RVA: 0x000F4453 File Offset: 0x000F2653
		public void BeginInit()
		{
			this.initializing = true;
		}

		/// <summary>Signals the object that initialization is complete.</summary>
		// Token: 0x06003597 RID: 13719 RVA: 0x000F445C File Offset: 0x000F265C
		public void EndInit()
		{
			this.initializing = false;
			if (this.newPanel1MinSize != this.panel1MinSize)
			{
				this.ApplyPanel1MinSize(this.newPanel1MinSize);
			}
			if (this.newPanel2MinSize != this.panel2MinSize)
			{
				this.ApplyPanel2MinSize(this.newPanel2MinSize);
			}
			if (this.newSplitterWidth != this.splitterWidth)
			{
				this.ApplySplitterWidth(this.newSplitterWidth);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.SplitContainer.BackgroundImage" /> property changes. </summary>
		// Token: 0x14000292 RID: 658
		// (add) Token: 0x06003598 RID: 13720 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06003599 RID: 13721 RVA: 0x0001FD8A File Offset: 0x0001DF8A
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.SplitContainer.BackgroundImageLayout" /> property changes. This event is not relevant to this class.</summary>
		// Token: 0x14000293 RID: 659
		// (add) Token: 0x0600359A RID: 13722 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x0600359B RID: 13723 RVA: 0x0001FD9C File Offset: 0x0001DF9C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		/// <summary>Occurs when the splitter control is in the process of moving.</summary>
		// Token: 0x14000294 RID: 660
		// (add) Token: 0x0600359C RID: 13724 RVA: 0x000F44BE File Offset: 0x000F26BE
		// (remove) Token: 0x0600359D RID: 13725 RVA: 0x000F44D1 File Offset: 0x000F26D1
		[SRCategory("CatBehavior")]
		[SRDescription("SplitterSplitterMovingDescr")]
		public event SplitterCancelEventHandler SplitterMoving
		{
			add
			{
				base.Events.AddHandler(SplitContainer.EVENT_MOVING, value);
			}
			remove
			{
				base.Events.RemoveHandler(SplitContainer.EVENT_MOVING, value);
			}
		}

		/// <summary>Occurs when the splitter control is moved.</summary>
		// Token: 0x14000295 RID: 661
		// (add) Token: 0x0600359E RID: 13726 RVA: 0x000F44E4 File Offset: 0x000F26E4
		// (remove) Token: 0x0600359F RID: 13727 RVA: 0x000F44F7 File Offset: 0x000F26F7
		[SRCategory("CatBehavior")]
		[SRDescription("SplitterSplitterMovedDescr")]
		public event SplitterEventHandler SplitterMoved
		{
			add
			{
				base.Events.AddHandler(SplitContainer.EVENT_MOVED, value);
			}
			remove
			{
				base.Events.RemoveHandler(SplitContainer.EVENT_MOVED, value);
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000296 RID: 662
		// (add) Token: 0x060035A0 RID: 13728 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x060035A1 RID: 13729 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060035A2 RID: 13730 RVA: 0x0001C1EB File Offset: 0x0001A3EB
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x060035A3 RID: 13731 RVA: 0x000F450C File Offset: 0x000F270C
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.IsSplitterMovable && !this.IsSplitterFixed)
			{
				if (e.KeyData == Keys.Escape && this.splitBegin)
				{
					this.splitBegin = false;
					this.splitBreak = true;
					return;
				}
				if (e.KeyData == Keys.Right || e.KeyData == Keys.Down || e.KeyData == Keys.Left || (e.KeyData == Keys.Up && this.splitterFocused))
				{
					if (this.splitBegin)
					{
						this.splitMove = true;
					}
					if (e.KeyData == Keys.Left || (e.KeyData == Keys.Up && this.splitterFocused))
					{
						this.splitterDistance -= this.SplitterIncrement;
						this.splitterDistance = ((this.splitterDistance < this.Panel1MinSize) ? (this.splitterDistance + this.SplitterIncrement) : Math.Max(this.splitterDistance, this.BORDERSIZE));
					}
					if (e.KeyData == Keys.Right || (e.KeyData == Keys.Down && this.splitterFocused))
					{
						this.splitterDistance += this.SplitterIncrement;
						if (this.Orientation == Orientation.Vertical)
						{
							this.splitterDistance = ((this.splitterDistance + this.SplitterWidth > base.Width - this.Panel2MinSize - this.BORDERSIZE) ? (this.splitterDistance - this.SplitterIncrement) : this.splitterDistance);
						}
						else
						{
							this.splitterDistance = ((this.splitterDistance + this.SplitterWidth > base.Height - this.Panel2MinSize - this.BORDERSIZE) ? (this.splitterDistance - this.SplitterIncrement) : this.splitterDistance);
						}
					}
					if (!this.splitBegin)
					{
						this.splitBegin = true;
					}
					if (this.splitBegin && !this.splitMove)
					{
						this.initialSplitterDistance = this.SplitterDistanceInternal;
						this.DrawSplitBar(1);
						return;
					}
					this.DrawSplitBar(2);
					Rectangle rectangle = this.CalcSplitLine(this.splitterDistance, 0);
					int x = rectangle.X;
					int y = rectangle.Y;
					SplitterCancelEventArgs splitterCancelEventArgs = new SplitterCancelEventArgs(base.Left + this.SplitterRectangle.X + this.SplitterRectangle.Width / 2, base.Top + this.SplitterRectangle.Y + this.SplitterRectangle.Height / 2, x, y);
					this.OnSplitterMoving(splitterCancelEventArgs);
					if (splitterCancelEventArgs.Cancel)
					{
						this.SplitEnd(false);
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x060035A4 RID: 13732 RVA: 0x000F4784 File Offset: 0x000F2984
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (this.splitBegin && this.IsSplitterMovable && (e.KeyData == Keys.Right || e.KeyData == Keys.Down || e.KeyData == Keys.Left || (e.KeyData == Keys.Up && this.splitterFocused)))
			{
				this.DrawSplitBar(3);
				this.ApplySplitterDistance();
				this.splitBegin = false;
				this.splitMove = false;
			}
			if (this.splitBreak)
			{
				this.splitBreak = false;
				this.SplitEnd(false);
			}
			using (Graphics graphics = base.CreateGraphicsInternal())
			{
				if (this.BackgroundImage == null)
				{
					using (SolidBrush solidBrush = new SolidBrush(this.BackColor))
					{
						graphics.FillRectangle(solidBrush, this.SplitterRectangle);
					}
				}
				this.DrawFocus(graphics, this.SplitterRectangle);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data. </param>
		// Token: 0x060035A5 RID: 13733 RVA: 0x000F4874 File Offset: 0x000F2A74
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.SetInnerMostBorder(this);
			if (this.IsSplitterMovable && !this.setSplitterDistance)
			{
				this.ResizeSplitContainer();
			}
			base.OnLayout(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060035A6 RID: 13734 RVA: 0x000F489A File Offset: 0x000F2A9A
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			base.Invalidate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x060035A7 RID: 13735 RVA: 0x000F48AC File Offset: 0x000F2AAC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!this.IsSplitterFixed && this.IsSplitterMovable)
			{
				if (this.Cursor == this.DefaultCursor && this.SplitterRectangle.Contains(e.Location))
				{
					if (this.Orientation == Orientation.Vertical)
					{
						this.OverrideCursor = Cursors.VSplit;
					}
					else
					{
						this.OverrideCursor = Cursors.HSplit;
					}
				}
				else
				{
					this.OverrideCursor = null;
				}
				if (this.splitterClick)
				{
					int num = e.X;
					int num2 = e.Y;
					this.splitterDrag = true;
					this.SplitMove(num, num2);
					if (this.Orientation == Orientation.Vertical)
					{
						num = Math.Max(Math.Min(num, base.Width - this.Panel2MinSize), this.Panel1MinSize);
						num2 = Math.Max(num2, 0);
					}
					else
					{
						num2 = Math.Max(Math.Min(num2, base.Height - this.Panel2MinSize), this.Panel1MinSize);
						num = Math.Max(num, 0);
					}
					Rectangle rectangle = this.CalcSplitLine(this.GetSplitterDistance(e.X, e.Y), 0);
					int x = rectangle.X;
					int y = rectangle.Y;
					SplitterCancelEventArgs splitterCancelEventArgs = new SplitterCancelEventArgs(num, num2, x, y);
					this.OnSplitterMoving(splitterCancelEventArgs);
					if (splitterCancelEventArgs.Cancel)
					{
						this.SplitEnd(false);
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060035A8 RID: 13736 RVA: 0x000F49FB File Offset: 0x000F2BFB
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!base.Enabled)
			{
				return;
			}
			this.OverrideCursor = null;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x060035A9 RID: 13737 RVA: 0x000F4A14 File Offset: 0x000F2C14
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.IsSplitterMovable && this.SplitterRectangle.Contains(e.Location))
			{
				if (!base.Enabled)
				{
					return;
				}
				if (e.Button == MouseButtons.Left && e.Clicks == 1 && !this.IsSplitterFixed)
				{
					this.splitterFocused = true;
					IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
					if (containerControlInternal != null)
					{
						ContainerControl containerControl = containerControlInternal as ContainerControl;
						if (containerControl == null)
						{
							containerControlInternal.ActiveControl = this;
						}
						else
						{
							containerControl.SetActiveControlInternal(this);
						}
					}
					base.SetActiveControlInternal(null);
					this.nextActiveControl = this.panel2;
					this.SplitBegin(e.X, e.Y);
					this.splitterClick = true;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x060035AA RID: 13738 RVA: 0x000F4AD0 File Offset: 0x000F2CD0
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!base.Enabled)
			{
				return;
			}
			if (!this.IsSplitterFixed && this.IsSplitterMovable && this.splitterClick)
			{
				base.CaptureInternal = false;
				if (this.splitterDrag)
				{
					this.CalcSplitLine(this.GetSplitterDistance(e.X, e.Y), 0);
					this.SplitEnd(true);
				}
				else
				{
					this.SplitEnd(false);
				}
				this.splitterClick = false;
				this.splitterDrag = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Move" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060035AB RID: 13739 RVA: 0x000F4B4C File Offset: 0x000F2D4C
		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			this.SetSplitterRect(this.Orientation == Orientation.Vertical);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x060035AC RID: 13740 RVA: 0x000F4B64 File Offset: 0x000F2D64
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this.Focused)
			{
				this.DrawFocus(e.Graphics, this.SplitterRectangle);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.SplitContainer.SplitterMoving" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.SplitterEventArgs" /> that contains the event data. </param>
		// Token: 0x060035AD RID: 13741 RVA: 0x000F4B88 File Offset: 0x000F2D88
		public void OnSplitterMoving(SplitterCancelEventArgs e)
		{
			SplitterCancelEventHandler splitterCancelEventHandler = (SplitterCancelEventHandler)base.Events[SplitContainer.EVENT_MOVING];
			if (splitterCancelEventHandler != null)
			{
				splitterCancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.SplitContainer.SplitterMoved" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.SplitterEventArgs" /> that contains the event data.</param>
		// Token: 0x060035AE RID: 13742 RVA: 0x000F4BB8 File Offset: 0x000F2DB8
		public void OnSplitterMoved(SplitterEventArgs e)
		{
			SplitterEventHandler splitterEventHandler = (SplitterEventHandler)base.Events[SplitContainer.EVENT_MOVED];
			if (splitterEventHandler != null)
			{
				splitterEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060035AF RID: 13743 RVA: 0x000F4BE6 File Offset: 0x000F2DE6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			this.panel1.RightToLeft = this.RightToLeft;
			this.panel2.RightToLeft = this.RightToLeft;
			this.UpdateSplitter();
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x000F4C18 File Offset: 0x000F2E18
		private void ApplyPanel1MinSize(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("Panel1MinSize", SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"Panel1MinSize",
					value.ToString(CultureInfo.CurrentCulture),
					"0"
				}));
			}
			if (this.Orientation == Orientation.Vertical)
			{
				if (base.DesignMode && base.Width != this.DefaultSize.Width && value + this.Panel2MinSize + this.SplitterWidth > base.Width)
				{
					throw new ArgumentOutOfRangeException("Panel1MinSize", SR.GetString("InvalidArgument", new object[]
					{
						"Panel1MinSize",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
			else if (this.Orientation == Orientation.Horizontal && base.DesignMode && base.Height != this.DefaultSize.Height && value + this.Panel2MinSize + this.SplitterWidth > base.Height)
			{
				throw new ArgumentOutOfRangeException("Panel1MinSize", SR.GetString("InvalidArgument", new object[]
				{
					"Panel1MinSize",
					value.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.panel1MinSize = value;
			if (value > this.SplitterDistanceInternal)
			{
				this.SplitterDistanceInternal = value;
			}
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000F4D64 File Offset: 0x000F2F64
		private void ApplyPanel2MinSize(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("Panel2MinSize", SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"Panel2MinSize",
					value.ToString(CultureInfo.CurrentCulture),
					"0"
				}));
			}
			if (this.Orientation == Orientation.Vertical)
			{
				if (base.DesignMode && base.Width != this.DefaultSize.Width && value + this.Panel1MinSize + this.SplitterWidth > base.Width)
				{
					throw new ArgumentOutOfRangeException("Panel2MinSize", SR.GetString("InvalidArgument", new object[]
					{
						"Panel2MinSize",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
			else if (this.Orientation == Orientation.Horizontal && base.DesignMode && base.Height != this.DefaultSize.Height && value + this.Panel1MinSize + this.SplitterWidth > base.Height)
			{
				throw new ArgumentOutOfRangeException("Panel2MinSize", SR.GetString("InvalidArgument", new object[]
				{
					"Panel2MinSize",
					value.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.panel2MinSize = value;
			if (value > this.Panel2.Width)
			{
				this.SplitterDistanceInternal = this.Panel2.Width + this.SplitterWidthInternal;
			}
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000F4EC4 File Offset: 0x000F30C4
		private void ApplySplitterWidth(int value)
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException("SplitterWidth", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"SplitterWidth",
					value.ToString(CultureInfo.CurrentCulture),
					"1"
				}));
			}
			if (this.Orientation == Orientation.Vertical)
			{
				if (base.DesignMode && value + this.Panel1MinSize + this.Panel2MinSize > base.Width)
				{
					throw new ArgumentOutOfRangeException("SplitterWidth", SR.GetString("InvalidArgument", new object[]
					{
						"SplitterWidth",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
			else if (this.Orientation == Orientation.Horizontal && base.DesignMode && value + this.Panel1MinSize + this.Panel2MinSize > base.Height)
			{
				throw new ArgumentOutOfRangeException("SplitterWidth", SR.GetString("InvalidArgument", new object[]
				{
					"SplitterWidth",
					value.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.splitterWidth = value;
			this.UpdateSplitter();
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x000F4FD8 File Offset: 0x000F31D8
		private void ApplySplitterDistance()
		{
			using (new LayoutTransaction(this, this, "SplitterDistance", false))
			{
				this.SplitterDistanceInternal = this.splitterDistance;
			}
			if (this.BackColor == Color.Transparent)
			{
				base.Invalidate();
			}
			if (this.Orientation != Orientation.Vertical)
			{
				this.splitterRect.Y = base.Location.Y + this.SplitterDistanceInternal;
				return;
			}
			if (this.RightToLeft == RightToLeft.No)
			{
				this.splitterRect.X = base.Location.X + this.SplitterDistanceInternal;
				return;
			}
			this.splitterRect.X = base.Right - this.SplitterDistanceInternal - this.SplitterWidthInternal;
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000F50A8 File Offset: 0x000F32A8
		private Rectangle CalcSplitLine(int splitSize, int minWeight)
		{
			Rectangle result = default(Rectangle);
			Orientation orientation = this.Orientation;
			if (orientation != Orientation.Horizontal)
			{
				if (orientation == Orientation.Vertical)
				{
					result.Width = this.SplitterWidthInternal;
					result.Height = base.Height;
					if (result.Width < minWeight)
					{
						result.Width = minWeight;
					}
					if (this.RightToLeft == RightToLeft.No)
					{
						result.X = this.panel1.Location.X + splitSize;
					}
					else
					{
						result.X = base.Width - splitSize - this.SplitterWidthInternal;
					}
				}
			}
			else
			{
				result.Width = base.Width;
				result.Height = this.SplitterWidthInternal;
				if (result.Width < minWeight)
				{
					result.Width = minWeight;
				}
				result.Y = this.panel1.Location.Y + splitSize;
			}
			return result;
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000F5184 File Offset: 0x000F3384
		private void DrawSplitBar(int mode)
		{
			if (mode != 1 && this.lastDrawSplit != -1)
			{
				this.DrawSplitHelper(this.lastDrawSplit);
				this.lastDrawSplit = -1;
			}
			else if (mode != 1 && this.lastDrawSplit == -1)
			{
				return;
			}
			if (mode == 3)
			{
				if (this.lastDrawSplit != -1)
				{
					this.DrawSplitHelper(this.lastDrawSplit);
				}
				this.lastDrawSplit = -1;
				return;
			}
			if (this.splitMove || this.splitBegin)
			{
				this.DrawSplitHelper(this.splitterDistance);
				this.lastDrawSplit = this.splitterDistance;
				return;
			}
			this.DrawSplitHelper(this.splitterDistance);
			this.lastDrawSplit = this.splitterDistance;
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000F5223 File Offset: 0x000F3423
		private void DrawFocus(Graphics g, Rectangle r)
		{
			r.Inflate(-1, -1);
			ControlPaint.DrawFocusRectangle(g, r, this.ForeColor, this.BackColor);
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000F5244 File Offset: 0x000F3444
		private void DrawSplitHelper(int splitSize)
		{
			Rectangle rectangle = this.CalcSplitLine(splitSize, 3);
			IntPtr handle = base.Handle;
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(this, handle), NativeMethods.NullHandleRef, 1026);
			IntPtr handle2 = ControlPaint.CreateHalftoneHBRUSH();
			IntPtr handle3 = SafeNativeMethods.SelectObject(new HandleRef(this, dcex), new HandleRef(null, handle2));
			SafeNativeMethods.PatBlt(new HandleRef(this, dcex), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 5898313);
			SafeNativeMethods.SelectObject(new HandleRef(this, dcex), new HandleRef(null, handle3));
			SafeNativeMethods.DeleteObject(new HandleRef(null, handle2));
			UnsafeNativeMethods.ReleaseDC(new HandleRef(this, handle), new HandleRef(null, dcex));
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000F52F8 File Offset: 0x000F34F8
		private int GetSplitterDistance(int x, int y)
		{
			int num;
			if (this.Orientation == Orientation.Vertical)
			{
				num = x - this.anchor.X;
			}
			else
			{
				num = y - this.anchor.Y;
			}
			int val = 0;
			Orientation orientation = this.Orientation;
			if (orientation != Orientation.Horizontal)
			{
				if (orientation == Orientation.Vertical)
				{
					if (this.RightToLeft == RightToLeft.No)
					{
						val = Math.Max(this.panel1.Width + num, this.BORDERSIZE);
					}
					else
					{
						val = Math.Max(this.panel1.Width - num, this.BORDERSIZE);
					}
				}
			}
			else
			{
				val = Math.Max(this.panel1.Height + num, this.BORDERSIZE);
			}
			if (this.Orientation == Orientation.Vertical)
			{
				return Math.Max(Math.Min(val, base.Width - this.Panel2MinSize), this.Panel1MinSize);
			}
			return Math.Max(Math.Min(val, base.Height - this.Panel2MinSize), this.Panel1MinSize);
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000F53DC File Offset: 0x000F35DC
		private bool ProcessArrowKey(bool forward)
		{
			Control control = this;
			if (base.ActiveControl != null)
			{
				control = base.ActiveControl.ParentInternal;
			}
			return control.SelectNextControl(base.ActiveControl, forward, false, false, true);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000F5410 File Offset: 0x000F3610
		private void RepaintSplitterRect()
		{
			if (base.IsHandleCreated)
			{
				Graphics graphics = base.CreateGraphicsInternal();
				if (this.BackgroundImage != null)
				{
					using (TextureBrush textureBrush = new TextureBrush(this.BackgroundImage, WrapMode.Tile))
					{
						graphics.FillRectangle(textureBrush, base.ClientRectangle);
						goto IL_62;
					}
				}
				using (SolidBrush solidBrush = new SolidBrush(this.BackColor))
				{
					graphics.FillRectangle(solidBrush, this.splitterRect);
				}
				IL_62:
				graphics.Dispose();
			}
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000F54A4 File Offset: 0x000F36A4
		private void SetSplitterRect(bool vertical)
		{
			if (vertical)
			{
				this.splitterRect.X = ((this.RightToLeft == RightToLeft.Yes) ? (base.Width - this.splitterDistance - this.SplitterWidthInternal) : (base.Location.X + this.splitterDistance));
				this.splitterRect.Y = base.Location.Y;
				this.splitterRect.Width = this.SplitterWidthInternal;
				this.splitterRect.Height = base.Height;
				return;
			}
			this.splitterRect.X = base.Location.X;
			this.splitterRect.Y = base.Location.Y + this.SplitterDistanceInternal;
			this.splitterRect.Width = base.Width;
			this.splitterRect.Height = this.SplitterWidthInternal;
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x000F558C File Offset: 0x000F378C
		private void ResizeSplitContainer()
		{
			if (this.splitContainerScaling)
			{
				return;
			}
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			if (base.Width == 0)
			{
				this.panel1.Size = new Size(0, this.panel1.Height);
				this.panel2.Size = new Size(0, this.panel2.Height);
			}
			else if (base.Height == 0)
			{
				this.panel1.Size = new Size(this.panel1.Width, 0);
				this.panel2.Size = new Size(this.panel2.Width, 0);
			}
			else
			{
				if (this.Orientation == Orientation.Vertical)
				{
					if (!this.CollapsedMode)
					{
						if (this.FixedPanel == FixedPanel.Panel1)
						{
							this.panel1.Size = new Size(this.panelSize, base.Height);
							this.panel2.Size = new Size(Math.Max(base.Width - this.panelSize - this.SplitterWidthInternal, this.Panel2MinSize), base.Height);
						}
						if (this.FixedPanel == FixedPanel.Panel2)
						{
							this.panel2.Size = new Size(this.panelSize, base.Height);
							this.splitterDistance = Math.Max(base.Width - this.panelSize - this.SplitterWidthInternal, this.Panel1MinSize);
							this.panel1.WidthInternal = this.splitterDistance;
							this.panel1.HeightInternal = base.Height;
						}
						if (this.FixedPanel == FixedPanel.None)
						{
							if (this.ratioWidth != 0.0)
							{
								this.splitterDistance = Math.Max((int)Math.Floor((double)base.Width / this.ratioWidth), this.Panel1MinSize);
							}
							this.panel1.WidthInternal = this.splitterDistance;
							this.panel1.HeightInternal = base.Height;
							this.panel2.Size = new Size(Math.Max(base.Width - this.splitterDistance - this.SplitterWidthInternal, this.Panel2MinSize), base.Height);
						}
						if (this.RightToLeft == RightToLeft.No)
						{
							this.panel2.Location = new Point(this.panel1.WidthInternal + this.SplitterWidthInternal, 0);
						}
						else
						{
							this.panel1.Location = new Point(base.Width - this.panel1.WidthInternal, 0);
						}
						this.RepaintSplitterRect();
						this.SetSplitterRect(true);
					}
					else if (this.Panel1Collapsed)
					{
						this.panel2.Size = base.Size;
						this.panel2.Location = new Point(0, 0);
					}
					else if (this.Panel2Collapsed)
					{
						this.panel1.Size = base.Size;
						this.panel1.Location = new Point(0, 0);
					}
				}
				else if (this.Orientation == Orientation.Horizontal)
				{
					if (!this.CollapsedMode)
					{
						if (this.FixedPanel == FixedPanel.Panel1)
						{
							this.panel1.Size = new Size(base.Width, this.panelSize);
							int num = this.panelSize + this.SplitterWidthInternal;
							this.panel2.Size = new Size(base.Width, Math.Max(base.Height - num, this.Panel2MinSize));
							this.panel2.Location = new Point(0, num);
						}
						if (this.FixedPanel == FixedPanel.Panel2)
						{
							this.panel2.Size = new Size(base.Width, this.panelSize);
							this.splitterDistance = Math.Max(base.Height - this.Panel2.Height - this.SplitterWidthInternal, this.Panel1MinSize);
							this.panel1.HeightInternal = this.splitterDistance;
							this.panel1.WidthInternal = base.Width;
							int y = this.splitterDistance + this.SplitterWidthInternal;
							this.panel2.Location = new Point(0, y);
						}
						if (this.FixedPanel == FixedPanel.None)
						{
							if (this.ratioHeight != 0.0)
							{
								this.splitterDistance = Math.Max((int)Math.Floor((double)base.Height / this.ratioHeight), this.Panel1MinSize);
							}
							this.panel1.HeightInternal = this.splitterDistance;
							this.panel1.WidthInternal = base.Width;
							int num2 = this.splitterDistance + this.SplitterWidthInternal;
							this.panel2.Size = new Size(base.Width, Math.Max(base.Height - num2, this.Panel2MinSize));
							this.panel2.Location = new Point(0, num2);
						}
						this.RepaintSplitterRect();
						this.SetSplitterRect(false);
					}
					else if (this.Panel1Collapsed)
					{
						this.panel2.Size = base.Size;
						this.panel2.Location = new Point(0, 0);
					}
					else if (this.Panel2Collapsed)
					{
						this.panel1.Size = base.Size;
						this.panel1.Location = new Point(0, 0);
					}
				}
				try
				{
					this.resizeCalled = true;
					this.ApplySplitterDistance();
				}
				finally
				{
					this.resizeCalled = false;
				}
			}
			this.panel1.ResumeLayout();
			this.panel2.ResumeLayout();
		}

		/// <summary>Scales the location, size, padding and margin.</summary>
		/// <param name="factor">The factor by which the height and width of the control will be scaled.</param>
		/// <param name="specified">A <see cref="T:System.Windows.Forms.BoundsSpecified" /> value that specifies the bounds of the control to use when defining its size and position.</param>
		// Token: 0x060035BD RID: 13757 RVA: 0x000F5AE8 File Offset: 0x000F3CE8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			try
			{
				this.splitContainerScaling = true;
				base.ScaleControl(factor, specified);
				float num;
				if (this.orientation == Orientation.Vertical)
				{
					num = factor.Width;
				}
				else
				{
					num = factor.Height;
				}
				this.SplitterWidth = (int)Math.Round((double)((float)this.SplitterWidth * num));
			}
			finally
			{
				this.splitContainerScaling = false;
			}
		}

		/// <summary>Activates a child control. Optionally specifies the direction in the tab order to select the control from.</summary>
		/// <param name="directed">
		///       <see langword="true" /> to specify the direction of the control to select; otherwise, <see langword="false" />. </param>
		/// <param name="forward">
		///       <see langword="true" /> to move forward in the tab order; <see langword="false" /> to move backward in the tab order. </param>
		// Token: 0x060035BE RID: 13758 RVA: 0x000F5B50 File Offset: 0x000F3D50
		protected override void Select(bool directed, bool forward)
		{
			if (this.selectNextControl)
			{
				return;
			}
			if (this.Panel1.Controls.Count > 0 || this.Panel2.Controls.Count > 0 || this.TabStop)
			{
				this.SelectNextControlInContainer(this, forward, true, true, false);
				return;
			}
			try
			{
				Control parentInternal = this.ParentInternal;
				this.selectNextControl = true;
				while (parentInternal != null)
				{
					if (parentInternal.SelectNextControl(this, forward, true, true, parentInternal.ParentInternal == null))
					{
						break;
					}
					parentInternal = parentInternal.ParentInternal;
				}
			}
			finally
			{
				this.selectNextControl = false;
			}
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000F5BF0 File Offset: 0x000F3DF0
		private bool SelectNextControlInContainer(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			if (!base.Contains(ctl) || (!nested && ctl.ParentInternal != this))
			{
				ctl = null;
			}
			SplitterPanel splitterPanel = null;
			for (;;)
			{
				ctl = base.GetNextControl(ctl, forward);
				SplitterPanel splitterPanel2 = ctl as SplitterPanel;
				if (splitterPanel2 != null && splitterPanel2.Visible)
				{
					if (splitterPanel != null)
					{
						goto IL_8D;
					}
					splitterPanel = splitterPanel2;
				}
				if (!forward && splitterPanel != null && ctl.ParentInternal != splitterPanel)
				{
					break;
				}
				if (ctl == null)
				{
					goto IL_8D;
				}
				if (ctl.CanSelect && ctl.TabStop)
				{
					goto Block_11;
				}
				if (ctl == null)
				{
					goto IL_8D;
				}
			}
			ctl = splitterPanel;
			goto IL_8D;
			Block_11:
			if (ctl is SplitContainer)
			{
				((SplitContainer)ctl).Select(forward, forward);
			}
			else
			{
				SplitContainer.SelectNextActiveControl(ctl, forward, tabStopOnly, nested, wrap);
			}
			return true;
			IL_8D:
			if (ctl != null && this.TabStop)
			{
				this.splitterFocused = true;
				IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
				if (containerControlInternal != null)
				{
					ContainerControl containerControl = containerControlInternal as ContainerControl;
					if (containerControl == null)
					{
						containerControlInternal.ActiveControl = this;
					}
					else
					{
						IntSecurity.ModifyFocus.Demand();
						containerControl.SetActiveControlInternal(this);
					}
				}
				base.SetActiveControlInternal(null);
				this.nextActiveControl = ctl;
				return true;
			}
			if (!this.SelectNextControlInPanel(ctl, forward, tabStopOnly, nested, wrap))
			{
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					try
					{
						this.selectNextControl = true;
						parentInternal.SelectNextControl(this, forward, true, true, true);
					}
					finally
					{
						this.selectNextControl = false;
					}
				}
			}
			return false;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000F5D30 File Offset: 0x000F3F30
		private bool SelectNextControlInPanel(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			if (!base.Contains(ctl) || (!nested && ctl.ParentInternal != this))
			{
				ctl = null;
			}
			for (;;)
			{
				ctl = base.GetNextControl(ctl, forward);
				if (ctl == null || (ctl is SplitterPanel && ctl.Visible))
				{
					goto IL_73;
				}
				if (ctl.CanSelect && (!tabStopOnly || ctl.TabStop))
				{
					break;
				}
				if (ctl == null)
				{
					goto IL_73;
				}
			}
			if (ctl is SplitContainer)
			{
				((SplitContainer)ctl).Select(forward, forward);
			}
			else
			{
				SplitContainer.SelectNextActiveControl(ctl, forward, tabStopOnly, nested, wrap);
			}
			return true;
			IL_73:
			if (ctl == null || (ctl is SplitterPanel && !ctl.Visible))
			{
				this.callBaseVersion = true;
			}
			else
			{
				ctl = base.GetNextControl(ctl, forward);
				if (forward)
				{
					this.nextActiveControl = this.panel2;
				}
				else if (ctl == null || !ctl.ParentInternal.Visible)
				{
					this.callBaseVersion = true;
				}
				else
				{
					this.nextActiveControl = this.panel2;
				}
			}
			return false;
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000F5E10 File Offset: 0x000F4010
		private static void SelectNextActiveControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
		{
			ContainerControl containerControl = ctl as ContainerControl;
			if (containerControl != null)
			{
				bool flag = true;
				if (containerControl.ParentInternal != null)
				{
					IContainerControl containerControlInternal = containerControl.ParentInternal.GetContainerControlInternal();
					if (containerControlInternal != null)
					{
						containerControlInternal.ActiveControl = containerControl;
						flag = (containerControlInternal.ActiveControl == containerControl);
					}
				}
				if (flag)
				{
					ctl.SelectNextControl(null, forward, tabStopOnly, nested, wrap);
					return;
				}
			}
			else
			{
				ctl.Select();
			}
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000F5E68 File Offset: 0x000F4068
		private void SetInnerMostBorder(SplitContainer sc)
		{
			foreach (object obj in sc.Controls)
			{
				Control control = (Control)obj;
				bool flag = false;
				if (control is SplitterPanel)
				{
					foreach (object obj2 in control.Controls)
					{
						Control control2 = (Control)obj2;
						SplitContainer splitContainer = control2 as SplitContainer;
						if (splitContainer != null && splitContainer.Dock == DockStyle.Fill)
						{
							if (splitContainer.BorderStyle != this.BorderStyle)
							{
								break;
							}
							((SplitterPanel)control).BorderStyle = BorderStyle.None;
							this.SetInnerMostBorder(splitContainer);
							flag = true;
						}
					}
					if (!flag)
					{
						((SplitterPanel)control).BorderStyle = this.BorderStyle;
					}
				}
			}
		}

		/// <summary>Performs the work of setting the specified bounds of this control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x060035C3 RID: 13763 RVA: 0x000F5F68 File Offset: 0x000F4168
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None && this.Orientation == Orientation.Horizontal && height < this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize)
			{
				height = this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize;
			}
			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None && this.Orientation == Orientation.Vertical && width < this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize)
			{
				width = this.Panel1MinSize + this.SplitterWidthInternal + this.Panel2MinSize;
			}
			base.SetBoundsCore(x, y, width, height, specified);
			this.SetSplitterRect(this.Orientation == Orientation.Vertical);
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000F600C File Offset: 0x000F420C
		private void SplitBegin(int x, int y)
		{
			this.anchor = new Point(x, y);
			this.splitterDistance = this.GetSplitterDistance(x, y);
			this.initialSplitterDistance = this.splitterDistance;
			this.initialSplitterRectangle = this.SplitterRectangle;
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				if (this.splitContainerMessageFilter == null)
				{
					this.splitContainerMessageFilter = new SplitContainer.SplitContainerMessageFilter(this);
				}
				Application.AddMessageFilter(this.splitContainerMessageFilter);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			base.CaptureInternal = true;
			this.DrawSplitBar(1);
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000F609C File Offset: 0x000F429C
		private void SplitMove(int x, int y)
		{
			int num = this.GetSplitterDistance(x, y);
			int num2 = num - this.initialSplitterDistance;
			int num3 = num2 % this.SplitterIncrement;
			if (this.splitterDistance != num)
			{
				if (this.Orientation == Orientation.Vertical)
				{
					if (num + this.SplitterWidthInternal <= base.Width - this.Panel2MinSize - this.BORDERSIZE)
					{
						this.splitterDistance = num - num3;
					}
				}
				else if (num + this.SplitterWidthInternal <= base.Height - this.Panel2MinSize - this.BORDERSIZE)
				{
					this.splitterDistance = num - num3;
				}
			}
			this.DrawSplitBar(2);
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000F6130 File Offset: 0x000F4330
		private void SplitEnd(bool accept)
		{
			this.DrawSplitBar(3);
			if (this.splitContainerMessageFilter != null)
			{
				Application.RemoveMessageFilter(this.splitContainerMessageFilter);
				this.splitContainerMessageFilter = null;
			}
			if (accept)
			{
				this.ApplySplitterDistance();
			}
			else if (this.splitterDistance != this.initialSplitterDistance)
			{
				this.splitterClick = false;
				this.splitterDistance = (this.SplitterDistanceInternal = this.initialSplitterDistance);
			}
			this.anchor = Point.Empty;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000F61A0 File Offset: 0x000F43A0
		private void UpdateSplitter()
		{
			if (this.splitContainerScaling)
			{
				return;
			}
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			if (this.Orientation == Orientation.Vertical)
			{
				bool flag = this.RightToLeft == RightToLeft.Yes;
				if (!this.CollapsedMode)
				{
					this.panel1.HeightInternal = base.Height;
					this.panel1.WidthInternal = this.splitterDistance;
					this.panel2.Size = new Size(base.Width - this.splitterDistance - this.SplitterWidthInternal, base.Height);
					if (!flag)
					{
						this.panel1.Location = new Point(0, 0);
						this.panel2.Location = new Point(this.splitterDistance + this.SplitterWidthInternal, 0);
					}
					else
					{
						this.panel1.Location = new Point(base.Width - this.splitterDistance, 0);
						this.panel2.Location = new Point(0, 0);
					}
					this.RepaintSplitterRect();
					this.SetSplitterRect(true);
					if (!this.resizeCalled)
					{
						this.ratioWidth = (((double)base.Width / (double)this.panel1.Width > 0.0) ? ((double)base.Width / (double)this.panel1.Width) : this.ratioWidth);
					}
				}
				else
				{
					if (this.Panel1Collapsed)
					{
						this.panel2.Size = base.Size;
						this.panel2.Location = new Point(0, 0);
					}
					else if (this.Panel2Collapsed)
					{
						this.panel1.Size = base.Size;
						this.panel1.Location = new Point(0, 0);
					}
					if (!this.resizeCalled)
					{
						this.ratioWidth = (((double)base.Width / (double)this.splitterDistance > 0.0) ? ((double)base.Width / (double)this.splitterDistance) : this.ratioWidth);
					}
				}
			}
			else if (!this.CollapsedMode)
			{
				this.panel1.Location = new Point(0, 0);
				this.panel1.WidthInternal = base.Width;
				this.panel1.HeightInternal = this.SplitterDistanceInternal;
				int num = this.splitterDistance + this.SplitterWidthInternal;
				this.panel2.Size = new Size(base.Width, base.Height - num);
				this.panel2.Location = new Point(0, num);
				this.RepaintSplitterRect();
				this.SetSplitterRect(false);
				if (!this.resizeCalled)
				{
					this.ratioHeight = (((double)base.Height / (double)this.panel1.Height > 0.0) ? ((double)base.Height / (double)this.panel1.Height) : this.ratioHeight);
				}
			}
			else
			{
				if (this.Panel1Collapsed)
				{
					this.panel2.Size = base.Size;
					this.panel2.Location = new Point(0, 0);
				}
				else if (this.Panel2Collapsed)
				{
					this.panel1.Size = base.Size;
					this.panel1.Location = new Point(0, 0);
				}
				if (!this.resizeCalled)
				{
					this.ratioHeight = (((double)base.Height / (double)this.splitterDistance > 0.0) ? ((double)base.Height / (double)this.splitterDistance) : this.ratioHeight);
				}
			}
			this.panel1.ResumeLayout();
			this.panel2.ResumeLayout();
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000F6524 File Offset: 0x000F4724
		private void WmSetCursor(ref Message m)
		{
			if (!(m.WParam == base.InternalHandle) || ((int)m.LParam & 65535) != 1)
			{
				this.DefWndProc(ref m);
				return;
			}
			if (this.OverrideCursor != null)
			{
				Cursor.CurrentInternal = this.OverrideCursor;
				return;
			}
			Cursor.CurrentInternal = this.Cursor;
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000F6588 File Offset: 0x000F4788
		internal override Rectangle GetToolNativeScreenRectangle()
		{
			Rectangle toolNativeScreenRectangle = base.GetToolNativeScreenRectangle();
			Rectangle splitterRectangle = this.SplitterRectangle;
			return new Rectangle(toolNativeScreenRectangle.X + splitterRectangle.X, toolNativeScreenRectangle.Y + splitterRectangle.Y, splitterRectangle.Width, splitterRectangle.Height);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000F65D4 File Offset: 0x000F47D4
		internal override void AfterControlRemoved(Control control, Control oldParent)
		{
			base.AfterControlRemoved(control, oldParent);
			if (control is SplitContainer && control.Dock == DockStyle.Fill)
			{
				this.SetInnerMostBorder(this);
			}
		}

		/// <summary>Processes a dialog box key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060035CB RID: 13771 RVA: 0x000F65F8 File Offset: 0x000F47F8
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys != Keys.Tab)
				{
					if (keys - Keys.Left <= 3)
					{
						if (this.splitterFocused)
						{
							return false;
						}
						if (this.ProcessArrowKey(keys == Keys.Right || keys == Keys.Down))
						{
							return true;
						}
					}
				}
				else if (this.ProcessTabKey((keyData & Keys.Shift) == Keys.None))
				{
					return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>Selects the next available control and makes it the active control.</summary>
		/// <param name="forward">
		///       <see langword="true" /> to cycle forward through the controls in the <see cref="T:System.Windows.Forms.ContainerControl" />; otherwise, <see langword="false" />. </param>
		/// <returns>
		///     <see langword="true" /> if a control is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x060035CC RID: 13772 RVA: 0x000F6660 File Offset: 0x000F4860
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessTabKey(bool forward)
		{
			if (!this.TabStop || this.IsSplitterFixed)
			{
				return base.ProcessTabKey(forward);
			}
			if (this.nextActiveControl != null)
			{
				base.SetActiveControlInternal(this.nextActiveControl);
				this.nextActiveControl = null;
			}
			if (this.SelectNextControlInPanel(base.ActiveControl, forward, true, true, true))
			{
				this.nextActiveControl = null;
				this.splitterFocused = false;
				return true;
			}
			if (this.callBaseVersion)
			{
				this.callBaseVersion = false;
				return base.ProcessTabKey(forward);
			}
			this.splitterFocused = true;
			IContainerControl containerControlInternal = this.ParentInternal.GetContainerControlInternal();
			if (containerControlInternal != null)
			{
				ContainerControl containerControl = containerControlInternal as ContainerControl;
				if (containerControl == null)
				{
					containerControlInternal.ActiveControl = this;
				}
				else
				{
					containerControl.SetActiveControlInternal(this);
				}
			}
			base.SetActiveControlInternal(null);
			return true;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseCaptureChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060035CD RID: 13773 RVA: 0x000F6711 File Offset: 0x000F4911
		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			base.OnMouseCaptureChanged(e);
			if (this.splitContainerMessageFilter != null)
			{
				Application.RemoveMessageFilter(this.splitContainerMessageFilter);
				this.splitContainerMessageFilter = null;
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="msg">The Windows <see cref="T:System.Windows.Forms.Message" /> to process. </param>
		// Token: 0x060035CE RID: 13774 RVA: 0x000F6734 File Offset: 0x000F4934
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			if (msg2 == 7)
			{
				this.splitterFocused = true;
				base.WndProc(ref msg);
				return;
			}
			if (msg2 == 8)
			{
				this.splitterFocused = false;
				base.WndProc(ref msg);
				return;
			}
			if (msg2 == 32)
			{
				this.WmSetCursor(ref msg);
				return;
			}
			base.WndProc(ref msg);
		}

		/// <summary>Creates a new instance of the control collection for the control.</summary>
		/// <returns>A new instance of <see cref="T:System.Windows.Forms.Control.ControlCollection" /> assigned to the control.</returns>
		// Token: 0x060035CF RID: 13775 RVA: 0x000F6782 File Offset: 0x000F4982
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new SplitContainer.SplitContainerTypedControlCollection(this, typeof(SplitterPanel), true);
		}

		// Token: 0x0400216A RID: 8554
		private const int DRAW_START = 1;

		// Token: 0x0400216B RID: 8555
		private const int DRAW_MOVE = 2;

		// Token: 0x0400216C RID: 8556
		private const int DRAW_END = 3;

		// Token: 0x0400216D RID: 8557
		private const int rightBorder = 5;

		// Token: 0x0400216E RID: 8558
		private const int leftBorder = 2;

		// Token: 0x0400216F RID: 8559
		private int BORDERSIZE;

		// Token: 0x04002170 RID: 8560
		private Orientation orientation = Orientation.Vertical;

		// Token: 0x04002171 RID: 8561
		private SplitterPanel panel1;

		// Token: 0x04002172 RID: 8562
		private SplitterPanel panel2;

		// Token: 0x04002173 RID: 8563
		private BorderStyle borderStyle;

		// Token: 0x04002174 RID: 8564
		private FixedPanel fixedPanel;

		// Token: 0x04002175 RID: 8565
		private int panel1MinSize = 25;

		// Token: 0x04002176 RID: 8566
		private int newPanel1MinSize = 25;

		// Token: 0x04002177 RID: 8567
		private int panel2MinSize = 25;

		// Token: 0x04002178 RID: 8568
		private int newPanel2MinSize = 25;

		// Token: 0x04002179 RID: 8569
		private bool tabStop = true;

		// Token: 0x0400217A RID: 8570
		private int panelSize;

		// Token: 0x0400217B RID: 8571
		private Rectangle splitterRect;

		// Token: 0x0400217C RID: 8572
		private int splitterInc = 1;

		// Token: 0x0400217D RID: 8573
		private bool splitterFixed;

		// Token: 0x0400217E RID: 8574
		private int splitterDistance = 50;

		// Token: 0x0400217F RID: 8575
		private int splitterWidth = 4;

		// Token: 0x04002180 RID: 8576
		private int newSplitterWidth = 4;

		// Token: 0x04002181 RID: 8577
		private int splitDistance = 50;

		// Token: 0x04002182 RID: 8578
		private int lastDrawSplit = 1;

		// Token: 0x04002183 RID: 8579
		private int initialSplitterDistance;

		// Token: 0x04002184 RID: 8580
		private Rectangle initialSplitterRectangle;

		// Token: 0x04002185 RID: 8581
		private Point anchor = Point.Empty;

		// Token: 0x04002186 RID: 8582
		private bool splitBegin;

		// Token: 0x04002187 RID: 8583
		private bool splitMove;

		// Token: 0x04002188 RID: 8584
		private bool splitBreak;

		// Token: 0x04002189 RID: 8585
		private Cursor overrideCursor;

		// Token: 0x0400218A RID: 8586
		private Control nextActiveControl;

		// Token: 0x0400218B RID: 8587
		private bool callBaseVersion;

		// Token: 0x0400218C RID: 8588
		private bool splitterFocused;

		// Token: 0x0400218D RID: 8589
		private bool splitterClick;

		// Token: 0x0400218E RID: 8590
		private bool splitterDrag;

		// Token: 0x0400218F RID: 8591
		private double ratioWidth;

		// Token: 0x04002190 RID: 8592
		private double ratioHeight;

		// Token: 0x04002191 RID: 8593
		private bool resizeCalled;

		// Token: 0x04002192 RID: 8594
		private bool splitContainerScaling;

		// Token: 0x04002193 RID: 8595
		private bool setSplitterDistance;

		// Token: 0x04002194 RID: 8596
		private static readonly object EVENT_MOVING = new object();

		// Token: 0x04002195 RID: 8597
		private static readonly object EVENT_MOVED = new object();

		// Token: 0x04002196 RID: 8598
		private SplitContainer.SplitContainerMessageFilter splitContainerMessageFilter;

		// Token: 0x04002197 RID: 8599
		private bool selectNextControl;

		// Token: 0x04002198 RID: 8600
		private bool initializing;

		// Token: 0x0200071A RID: 1818
		private class SplitContainerMessageFilter : IMessageFilter
		{
			// Token: 0x0600601E RID: 24606 RVA: 0x0018A235 File Offset: 0x00188435
			public SplitContainerMessageFilter(SplitContainer splitContainer)
			{
				this.owner = splitContainer;
			}

			// Token: 0x0600601F RID: 24607 RVA: 0x0018A244 File Offset: 0x00188444
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			bool IMessageFilter.PreFilterMessage(ref Message m)
			{
				if (m.Msg >= 256 && m.Msg <= 264)
				{
					if ((m.Msg == 256 && (int)m.WParam == 27) || m.Msg == 260)
					{
						this.owner.splitBegin = false;
						this.owner.SplitEnd(false);
						this.owner.splitterClick = false;
						this.owner.splitterDrag = false;
					}
					return true;
				}
				return false;
			}

			// Token: 0x04004145 RID: 16709
			private SplitContainer owner;
		}

		// Token: 0x0200071B RID: 1819
		internal class SplitContainerTypedControlCollection : WindowsFormsUtils.TypedControlCollection
		{
			// Token: 0x06006020 RID: 24608 RVA: 0x0018A2C7 File Offset: 0x001884C7
			public SplitContainerTypedControlCollection(Control c, Type type, bool isReadOnly) : base(c, type, isReadOnly)
			{
				this.owner = (c as SplitContainer);
			}

			// Token: 0x06006021 RID: 24609 RVA: 0x0018A2DE File Offset: 0x001884DE
			public override void Remove(Control value)
			{
				if (value is SplitterPanel && !this.owner.DesignMode && this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				base.Remove(value);
			}

			// Token: 0x06006022 RID: 24610 RVA: 0x0018A314 File Offset: 0x00188514
			internal override void SetChildIndexInternal(Control child, int newIndex)
			{
				if (child is SplitterPanel)
				{
					if (this.owner.DesignMode)
					{
						return;
					}
					if (this.IsReadOnly)
					{
						throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
					}
				}
				base.SetChildIndexInternal(child, newIndex);
			}

			// Token: 0x04004146 RID: 16710
			private SplitContainer owner;
		}
	}
}
