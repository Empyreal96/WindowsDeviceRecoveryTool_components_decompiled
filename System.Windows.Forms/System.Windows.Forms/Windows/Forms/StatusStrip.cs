using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows status bar control. </summary>
	// Token: 0x0200036C RID: 876
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionStatusStrip")]
	public class StatusStrip : ToolStrip
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.StatusStrip" /> class. </summary>
		// Token: 0x060036EF RID: 14063 RVA: 0x000F8D78 File Offset: 0x000F6F78
		public StatusStrip()
		{
			base.SuspendLayout();
			this.CanOverflow = false;
			this.LayoutStyle = ToolStripLayoutStyle.Table;
			base.RenderMode = ToolStripRenderMode.System;
			this.GripStyle = ToolStripGripStyle.Hidden;
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			this.Stretch = true;
			this.state[StatusStrip.stateSizingGrip] = true;
			base.ResumeLayout(true);
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.StatusStrip" /> supports overflow functionality.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.StatusStrip" /> supports overflow functionality; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x060036F0 RID: 14064 RVA: 0x000D09CC File Offset: 0x000CEBCC
		// (set) Token: 0x060036F1 RID: 14065 RVA: 0x000D09D4 File Offset: 0x000CEBD4
		[DefaultValue(false)]
		[SRDescription("ToolStripCanOverflowDescr")]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		public new bool CanOverflow
		{
			get
			{
				return base.CanOverflow;
			}
			set
			{
				base.CanOverflow = value;
			}
		}

		/// <summary>Gets a value indicating whether ToolTips are shown for the <see cref="T:System.Windows.Forms.StatusStrip" /> by default.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x060036F2 RID: 14066 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected override bool DefaultShowItemToolTips
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the size, in pixels, of the <see cref="T:System.Windows.Forms.StatusStrip" /> when it is first created.</summary>
		/// <returns>A <see cref="M:System.Drawing.Point.#ctor(System.Drawing.Size)" /> constructor representing the size of the <see cref="T:System.Windows.Forms.StatusStrip" />, in pixels.</returns>
		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x060036F3 RID: 14067 RVA: 0x000F8DD5 File Offset: 0x000F6FD5
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 22);
			}
		}

		/// <summary>Gets the spacing, in pixels, between the left, right, top, and bottom edges of the <see cref="T:System.Windows.Forms.StatusStrip" /> from the edges of the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the spacing. The default is {Left=6, Top=2, Right=0, Bottom=2}.</returns>
		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x060036F4 RID: 14068 RVA: 0x000F8DE4 File Offset: 0x000F6FE4
		protected override Padding DefaultPadding
		{
			get
			{
				if (base.Orientation != Orientation.Horizontal)
				{
					return new Padding(1, 3, 1, this.DefaultSize.Height);
				}
				if (this.RightToLeft == RightToLeft.No)
				{
					return new Padding(1, 0, 14, 0);
				}
				return new Padding(14, 0, 1, 0);
			}
		}

		/// <summary>Gets which borders of the <see cref="T:System.Windows.Forms.StatusStrip" /> are docked to the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see cref="F:System.Windows.Forms.DockStyle.Bottom" />.</returns>
		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x060036F5 RID: 14069 RVA: 0x0000E211 File Offset: 0x0000C411
		protected override DockStyle DefaultDock
		{
			get
			{
				return DockStyle.Bottom;
			}
		}

		/// <summary>Gets or sets which <see cref="T:System.Windows.Forms.StatusStrip" /> borders are docked to its parent control and determines how a <see cref="T:System.Windows.Forms.StatusStrip" /> is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see cref="F:System.Windows.Forms.DockStyle.Bottom" />.</returns>
		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x060036F6 RID: 14070 RVA: 0x000F8E2D File Offset: 0x000F702D
		// (set) Token: 0x060036F7 RID: 14071 RVA: 0x000F8E35 File Offset: 0x000F7035
		[DefaultValue(DockStyle.Bottom)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}

		/// <summary>Gets or sets the visibility of the grip used to reposition the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripGripStyle" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripGripStyle.Hidden" />.</returns>
		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x060036F8 RID: 14072 RVA: 0x000D0A95 File Offset: 0x000CEC95
		// (set) Token: 0x060036F9 RID: 14073 RVA: 0x000D0A9D File Offset: 0x000CEC9D
		[DefaultValue(ToolStripGripStyle.Hidden)]
		public new ToolStripGripStyle GripStyle
		{
			get
			{
				return base.GripStyle;
			}
			set
			{
				base.GripStyle = value;
			}
		}

		/// <summary>Gets or sets a value indicating how the <see cref="T:System.Windows.Forms.StatusStrip" /> lays out the items collection.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripLayoutStyle" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripLayoutStyle.Table" />.</returns>
		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x000F8E3E File Offset: 0x000F703E
		// (set) Token: 0x060036FB RID: 14075 RVA: 0x000F8E46 File Offset: 0x000F7046
		[DefaultValue(ToolStripLayoutStyle.Table)]
		public new ToolStripLayoutStyle LayoutStyle
		{
			get
			{
				return base.LayoutStyle;
			}
			set
			{
				base.LayoutStyle = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value.</returns>
		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x060036FC RID: 14076 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x060036FD RID: 14077 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x140002B3 RID: 691
		// (add) Token: 0x060036FE RID: 14078 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x060036FF RID: 14079 RVA: 0x000204B4 File Offset: 0x0001E6B4
		[Browsable(false)]
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

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x000F8E4F File Offset: 0x000F704F
		private Control RTLGrip
		{
			get
			{
				if (this.rtlLayoutGrip == null)
				{
					this.rtlLayoutGrip = new StatusStrip.RightToLeftLayoutGrip();
				}
				return this.rtlLayoutGrip;
			}
		}

		/// <summary>Gets or sets a value indicating whether ToolTips are shown for the <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
		/// <returns>
		///     <see langword="true" /> if ToolTips are shown for the <see cref="T:System.Windows.Forms.StatusStrip" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06003701 RID: 14081 RVA: 0x000D0AF2 File Offset: 0x000CECF2
		// (set) Token: 0x06003702 RID: 14082 RVA: 0x000D0AFA File Offset: 0x000CECFA
		[DefaultValue(false)]
		[SRDescription("ToolStripShowItemToolTipsDescr")]
		[SRCategory("CatBehavior")]
		public new bool ShowItemToolTips
		{
			get
			{
				return base.ShowItemToolTips;
			}
			set
			{
				base.ShowItemToolTips = value;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06003703 RID: 14083 RVA: 0x000F8E6C File Offset: 0x000F706C
		private bool ShowSizingGrip
		{
			get
			{
				if (this.SizingGrip && base.IsHandleCreated)
				{
					if (base.DesignMode)
					{
						return true;
					}
					HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this);
					if (rootHWnd.Handle != IntPtr.Zero)
					{
						return !UnsafeNativeMethods.IsZoomed(rootHWnd);
					}
				}
				return false;
			}
		}

		/// <summary>Gets or sets a value indicating whether a sizing handle (grip) is displayed in the lower-right corner of the control.</summary>
		/// <returns>
		///     <see langword="true" /> if a grip is displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06003704 RID: 14084 RVA: 0x000F8EB8 File Offset: 0x000F70B8
		// (set) Token: 0x06003705 RID: 14085 RVA: 0x000F8ECA File Offset: 0x000F70CA
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		[SRDescription("StatusStripSizingGripDescr")]
		public bool SizingGrip
		{
			get
			{
				return this.state[StatusStrip.stateSizingGrip];
			}
			set
			{
				if (value != this.state[StatusStrip.stateSizingGrip])
				{
					this.state[StatusStrip.stateSizingGrip] = value;
					this.EnsureRightToLeftGrip();
					base.Invalidate(true);
				}
			}
		}

		/// <summary>Gets the boundaries of the sizing handle (grip) for a <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> representing the grip boundaries.</returns>
		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06003706 RID: 14086 RVA: 0x000F8F00 File Offset: 0x000F7100
		[Browsable(false)]
		public Rectangle SizeGripBounds
		{
			get
			{
				if (!this.SizingGrip)
				{
					return Rectangle.Empty;
				}
				Size size = base.Size;
				int num = Math.Min(this.DefaultSize.Height, size.Height);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					return new Rectangle(0, size.Height - num, 12, num);
				}
				return new Rectangle(size.Width - 12, size.Height - num, 12, num);
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.StatusStrip" /> stretches from end to end in its container.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.StatusStrip" /> stretches from end to end in its <see cref="T:System.Windows.Forms.ToolStripContainer" />; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06003707 RID: 14087 RVA: 0x000D0B03 File Offset: 0x000CED03
		// (set) Token: 0x06003708 RID: 14088 RVA: 0x000D0B0B File Offset: 0x000CED0B
		[DefaultValue(true)]
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripStretchDescr")]
		public new bool Stretch
		{
			get
			{
				return base.Stretch;
			}
			set
			{
				base.Stretch = value;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06003709 RID: 14089 RVA: 0x000F8F74 File Offset: 0x000F7174
		private TableLayoutSettings TableLayoutSettings
		{
			get
			{
				return base.LayoutSettings as TableLayoutSettings;
			}
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x0600370A RID: 14090 RVA: 0x000F8F81 File Offset: 0x000F7181
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new StatusStrip.StatusStripAccessibleObject(this);
		}

		/// <summary>Creates a default <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> with the specified text, image, and event handler on a new <see cref="T:System.Windows.Forms.StatusStrip" /> instance.</summary>
		/// <param name="text">The text to use for the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />. If the <paramref name="text" /> parameter is a hyphen (-), this method creates a <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> is clicked.</param>
		/// <returns>A <see cref="M:System.Windows.Forms.ToolStripStatusLabel.#ctor(System.String,System.Drawing.Image,System.EventHandler)" />, or a <see cref="T:System.Windows.Forms.ToolStripSeparator" /> if the <paramref name="text" /> parameter is a hyphen (-).</returns>
		// Token: 0x0600370B RID: 14091 RVA: 0x000F8F89 File Offset: 0x000F7189
		protected internal override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
		{
			return new ToolStripStatusLabel(text, image, onClick);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.StatusStrip" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600370C RID: 14092 RVA: 0x000F8F93 File Offset: 0x000F7193
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.rtlLayoutGrip != null)
			{
				this.rtlLayoutGrip.Dispose();
				this.rtlLayoutGrip = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x000F8FBC File Offset: 0x000F71BC
		private void EnsureRightToLeftGrip()
		{
			if (this.SizingGrip && this.RightToLeft == RightToLeft.Yes)
			{
				this.RTLGrip.Bounds = this.SizeGripBounds;
				if (!base.Controls.Contains(this.RTLGrip))
				{
					WindowsFormsUtils.ReadOnlyControlCollection readOnlyControlCollection = base.Controls as WindowsFormsUtils.ReadOnlyControlCollection;
					if (readOnlyControlCollection != null)
					{
						readOnlyControlCollection.AddInternal(this.RTLGrip);
						return;
					}
				}
			}
			else if (this.rtlLayoutGrip != null && base.Controls.Contains(this.rtlLayoutGrip))
			{
				WindowsFormsUtils.ReadOnlyControlCollection readOnlyControlCollection2 = base.Controls as WindowsFormsUtils.ReadOnlyControlCollection;
				if (readOnlyControlCollection2 != null)
				{
					readOnlyControlCollection2.RemoveInternal(this.rtlLayoutGrip);
				}
				this.rtlLayoutGrip.Dispose();
				this.rtlLayoutGrip = null;
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x000F9064 File Offset: 0x000F7264
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (this.LayoutStyle != ToolStripLayoutStyle.Table)
			{
				return base.GetPreferredSizeCore(proposedSize);
			}
			if (proposedSize.Width == 1)
			{
				proposedSize.Width = int.MaxValue;
			}
			if (proposedSize.Height == 1)
			{
				proposedSize.Height = int.MaxValue;
			}
			if (base.Orientation == Orientation.Horizontal)
			{
				return ToolStrip.GetPreferredSizeHorizontal(this, proposedSize) + this.Padding.Size;
			}
			return ToolStrip.GetPreferredSizeVertical(this, proposedSize) + this.Padding.Size;
		}

		/// <summary>Paints the background of the control.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains information about the <see cref="T:System.Windows.Forms.StatusStrip" /> to paint.</param>
		// Token: 0x0600370F RID: 14095 RVA: 0x000F90EB File Offset: 0x000F72EB
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			if (this.ShowSizingGrip)
			{
				base.Renderer.DrawStatusStripSizingGrip(new ToolStripRenderEventArgs(e.Graphics, this));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="levent">The event data.</param>
		// Token: 0x06003710 RID: 14096 RVA: 0x000F9114 File Offset: 0x000F7314
		protected override void OnLayout(LayoutEventArgs levent)
		{
			this.state[StatusStrip.stateCalledSpringTableLayout] = false;
			bool flag = false;
			ToolStripItem toolStripItem = levent.AffectedComponent as ToolStripItem;
			int count = this.DisplayedItems.Count;
			if (toolStripItem != null)
			{
				flag = this.DisplayedItems.Contains(toolStripItem);
			}
			if (this.LayoutStyle == ToolStripLayoutStyle.Table)
			{
				this.OnSpringTableLayoutCore();
			}
			base.OnLayout(levent);
			if ((count != this.DisplayedItems.Count || (toolStripItem != null && flag != this.DisplayedItems.Contains(toolStripItem))) && this.LayoutStyle == ToolStripLayoutStyle.Table)
			{
				this.OnSpringTableLayoutCore();
				base.OnLayout(levent);
			}
			this.EnsureRightToLeftGrip();
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06003711 RID: 14097 RVA: 0x00020C1B File Offset: 0x0001EE1B
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3 && !base.DesignMode;
			}
		}

		/// <summary>Resets the collection of displayed and overflow items after a layout is done.</summary>
		// Token: 0x06003712 RID: 14098 RVA: 0x000F91B0 File Offset: 0x000F73B0
		protected override void SetDisplayedItems()
		{
			if (this.state[StatusStrip.stateCalledSpringTableLayout])
			{
				bool flag = base.Orientation == Orientation.Horizontal && this.RightToLeft == RightToLeft.Yes;
				Point location = this.DisplayRectangle.Location;
				location.X += base.ClientSize.Width + 1;
				location.Y += base.ClientSize.Height + 1;
				bool flag2 = false;
				Rectangle rectangle = Rectangle.Empty;
				ToolStripItem toolStripItem = null;
				for (int i = 0; i < this.Items.Count; i++)
				{
					ToolStripItem toolStripItem2 = this.Items[i];
					if (flag2 || ((IArrangedElement)toolStripItem2).ParticipatesInLayout)
					{
						if (flag2 || (this.SizingGrip && toolStripItem2.Bounds.IntersectsWith(this.SizeGripBounds)))
						{
							base.SetItemLocation(toolStripItem2, location);
							toolStripItem2.SetPlacement(ToolStripItemPlacement.None);
						}
					}
					else if (toolStripItem != null && rectangle.IntersectsWith(toolStripItem2.Bounds))
					{
						base.SetItemLocation(toolStripItem2, location);
						toolStripItem2.SetPlacement(ToolStripItemPlacement.None);
					}
					else if (toolStripItem2.Bounds.Width == 1)
					{
						ToolStripStatusLabel toolStripStatusLabel = toolStripItem2 as ToolStripStatusLabel;
						if (toolStripStatusLabel != null && toolStripStatusLabel.Spring)
						{
							base.SetItemLocation(toolStripItem2, location);
							toolStripItem2.SetPlacement(ToolStripItemPlacement.None);
						}
					}
					if (toolStripItem2.Bounds.Location != location)
					{
						toolStripItem = toolStripItem2;
						rectangle = toolStripItem.Bounds;
					}
					else if (((IArrangedElement)toolStripItem2).ParticipatesInLayout)
					{
						flag2 = true;
					}
				}
			}
			base.SetDisplayedItems();
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000F934D File Offset: 0x000F754D
		internal override void ResetRenderMode()
		{
			base.RenderMode = ToolStripRenderMode.System;
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000F9356 File Offset: 0x000F7556
		internal override bool ShouldSerializeRenderMode()
		{
			return base.RenderMode != ToolStripRenderMode.System && base.RenderMode > ToolStripRenderMode.Custom;
		}

		/// <summary>Provides custom table layout for a <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
		// Token: 0x06003715 RID: 14101 RVA: 0x000F936C File Offset: 0x000F756C
		protected virtual void OnSpringTableLayoutCore()
		{
			if (this.LayoutStyle == ToolStripLayoutStyle.Table)
			{
				this.state[StatusStrip.stateCalledSpringTableLayout] = true;
				base.SuspendLayout();
				if (this.lastOrientation != base.Orientation)
				{
					TableLayoutSettings tableLayoutSettings = this.TableLayoutSettings;
					tableLayoutSettings.RowCount = 0;
					tableLayoutSettings.ColumnCount = 0;
					tableLayoutSettings.ColumnStyles.Clear();
					tableLayoutSettings.RowStyles.Clear();
				}
				this.lastOrientation = base.Orientation;
				if (base.Orientation == Orientation.Horizontal)
				{
					this.TableLayoutSettings.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
					int count = this.TableLayoutSettings.ColumnStyles.Count;
					for (int i = 0; i < this.DisplayedItems.Count; i++)
					{
						if (i >= count)
						{
							this.TableLayoutSettings.ColumnStyles.Add(new ColumnStyle());
						}
						ToolStripStatusLabel toolStripStatusLabel = this.DisplayedItems[i] as ToolStripStatusLabel;
						bool flag = toolStripStatusLabel != null && toolStripStatusLabel.Spring;
						this.DisplayedItems[i].Anchor = (flag ? (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right) : (AnchorStyles.Top | AnchorStyles.Bottom));
						ColumnStyle columnStyle = this.TableLayoutSettings.ColumnStyles[i];
						columnStyle.Width = 100f;
						columnStyle.SizeType = (flag ? SizeType.Percent : SizeType.AutoSize);
					}
					if (this.TableLayoutSettings.RowStyles.Count > 1 || this.TableLayoutSettings.RowStyles.Count == 0)
					{
						this.TableLayoutSettings.RowStyles.Clear();
						this.TableLayoutSettings.RowStyles.Add(new RowStyle());
					}
					this.TableLayoutSettings.RowCount = 1;
					this.TableLayoutSettings.RowStyles[0].SizeType = SizeType.Absolute;
					this.TableLayoutSettings.RowStyles[0].Height = (float)Math.Max(0, this.DisplayRectangle.Height);
					this.TableLayoutSettings.ColumnCount = this.DisplayedItems.Count + 1;
					for (int j = this.DisplayedItems.Count; j < this.TableLayoutSettings.ColumnStyles.Count; j++)
					{
						this.TableLayoutSettings.ColumnStyles[j].SizeType = SizeType.AutoSize;
					}
				}
				else
				{
					this.TableLayoutSettings.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
					int count2 = this.TableLayoutSettings.RowStyles.Count;
					for (int k = 0; k < this.DisplayedItems.Count; k++)
					{
						if (k >= count2)
						{
							this.TableLayoutSettings.RowStyles.Add(new RowStyle());
						}
						ToolStripStatusLabel toolStripStatusLabel2 = this.DisplayedItems[k] as ToolStripStatusLabel;
						bool flag2 = toolStripStatusLabel2 != null && toolStripStatusLabel2.Spring;
						this.DisplayedItems[k].Anchor = (flag2 ? (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right) : (AnchorStyles.Left | AnchorStyles.Right));
						RowStyle rowStyle = this.TableLayoutSettings.RowStyles[k];
						rowStyle.Height = 100f;
						rowStyle.SizeType = (flag2 ? SizeType.Percent : SizeType.AutoSize);
					}
					this.TableLayoutSettings.ColumnCount = 1;
					if (this.TableLayoutSettings.ColumnStyles.Count > 1 || this.TableLayoutSettings.ColumnStyles.Count == 0)
					{
						this.TableLayoutSettings.ColumnStyles.Clear();
						this.TableLayoutSettings.ColumnStyles.Add(new ColumnStyle());
					}
					this.TableLayoutSettings.ColumnCount = 1;
					this.TableLayoutSettings.ColumnStyles[0].SizeType = SizeType.Absolute;
					this.TableLayoutSettings.ColumnStyles[0].Width = (float)Math.Max(0, this.DisplayRectangle.Width);
					this.TableLayoutSettings.RowCount = this.DisplayedItems.Count + 1;
					for (int l = this.DisplayedItems.Count; l < this.TableLayoutSettings.RowStyles.Count; l++)
					{
						this.TableLayoutSettings.RowStyles[l].SizeType = SizeType.AutoSize;
					}
				}
				base.ResumeLayout(false);
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06003716 RID: 14102 RVA: 0x000F9770 File Offset: 0x000F7970
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 132 && this.SizingGrip)
			{
				Rectangle sizeGripBounds = this.SizeGripBounds;
				int x = NativeMethods.Util.LOWORD(m.LParam);
				int y = NativeMethods.Util.HIWORD(m.LParam);
				if (sizeGripBounds.Contains(base.PointToClient(new Point(x, y))))
				{
					HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(this);
					if (rootHWnd.Handle != IntPtr.Zero && !UnsafeNativeMethods.IsZoomed(rootHWnd))
					{
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						UnsafeNativeMethods.GetClientRect(rootHWnd, ref rect);
						NativeMethods.POINT point;
						if (this.RightToLeft == RightToLeft.Yes)
						{
							point = new NativeMethods.POINT(this.SizeGripBounds.Left, this.SizeGripBounds.Bottom);
						}
						else
						{
							point = new NativeMethods.POINT(this.SizeGripBounds.Right, this.SizeGripBounds.Bottom);
						}
						UnsafeNativeMethods.MapWindowPoints(new HandleRef(this, base.Handle), rootHWnd, point, 1);
						int num = Math.Abs(rect.bottom - point.y);
						int num2 = Math.Abs(rect.right - point.x);
						if (this.RightToLeft != RightToLeft.Yes && num2 + num < 2)
						{
							m.Result = (IntPtr)17;
							return;
						}
					}
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x040021E4 RID: 8676
		private const AnchorStyles AllAnchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

		// Token: 0x040021E5 RID: 8677
		private const AnchorStyles HorizontalAnchor = AnchorStyles.Left | AnchorStyles.Right;

		// Token: 0x040021E6 RID: 8678
		private const AnchorStyles VerticalAnchor = AnchorStyles.Top | AnchorStyles.Bottom;

		// Token: 0x040021E7 RID: 8679
		private BitVector32 state;

		// Token: 0x040021E8 RID: 8680
		private static readonly int stateSizingGrip = BitVector32.CreateMask();

		// Token: 0x040021E9 RID: 8681
		private static readonly int stateCalledSpringTableLayout = BitVector32.CreateMask(StatusStrip.stateSizingGrip);

		// Token: 0x040021EA RID: 8682
		private const int gripWidth = 12;

		// Token: 0x040021EB RID: 8683
		private StatusStrip.RightToLeftLayoutGrip rtlLayoutGrip;

		// Token: 0x040021EC RID: 8684
		private Orientation lastOrientation;

		// Token: 0x02000720 RID: 1824
		private class RightToLeftLayoutGrip : Control
		{
			// Token: 0x06006056 RID: 24662 RVA: 0x0018AEA1 File Offset: 0x001890A1
			public RightToLeftLayoutGrip()
			{
				base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
				this.BackColor = Color.Transparent;
			}

			// Token: 0x17001704 RID: 5892
			// (get) Token: 0x06006057 RID: 24663 RVA: 0x0018AEC0 File Offset: 0x001890C0
			protected override CreateParams CreateParams
			{
				[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.ExStyle |= 4194304;
					return createParams;
				}
			}

			// Token: 0x06006058 RID: 24664 RVA: 0x0018AEE8 File Offset: 0x001890E8
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 132)
				{
					int x = NativeMethods.Util.LOWORD(m.LParam);
					int y = NativeMethods.Util.HIWORD(m.LParam);
					if (base.ClientRectangle.Contains(base.PointToClient(new Point(x, y))))
					{
						m.Result = (IntPtr)16;
						return;
					}
				}
				base.WndProc(ref m);
			}
		}

		// Token: 0x02000721 RID: 1825
		[ComVisible(true)]
		internal class StatusStripAccessibleObject : ToolStrip.ToolStripAccessibleObject
		{
			// Token: 0x06006059 RID: 24665 RVA: 0x001868E4 File Offset: 0x00184AE4
			public StatusStripAccessibleObject(StatusStrip owner) : base(owner)
			{
			}

			// Token: 0x17001705 RID: 5893
			// (get) Token: 0x0600605A RID: 24666 RVA: 0x0018AF4C File Offset: 0x0018914C
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.StatusBar;
				}
			}

			// Token: 0x0600605B RID: 24667 RVA: 0x0018AF6D File Offset: 0x0018916D
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3 && propertyID == 30003)
				{
					return 50017;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x0600605C RID: 24668 RVA: 0x0018AF90 File Offset: 0x00189190
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				StatusStrip statusStrip = base.Owner as StatusStrip;
				if (statusStrip == null || statusStrip.Items.Count == 0)
				{
					return null;
				}
				if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild)
				{
					for (int i = 0; i < this.GetChildCount(); i++)
					{
						AccessibleObject child = this.GetChild(i);
						if (child != null && !(child is Control.ControlAccessibleObject))
						{
							return child;
						}
					}
					return null;
				}
				if (direction != UnsafeNativeMethods.NavigateDirection.LastChild)
				{
					return base.FragmentNavigate(direction);
				}
				for (int j = this.GetChildCount() - 1; j >= 0; j--)
				{
					AccessibleObject child2 = this.GetChild(j);
					if (child2 != null && !(child2 is Control.ControlAccessibleObject))
					{
						return child2;
					}
				}
				return null;
			}

			// Token: 0x0600605D RID: 24669 RVA: 0x00173EC5 File Offset: 0x001720C5
			internal override UnsafeNativeMethods.IRawElementProviderFragment ElementProviderFromPoint(double x, double y)
			{
				return this.HitTest((int)x, (int)y);
			}

			// Token: 0x0600605E RID: 24670 RVA: 0x000E8E48 File Offset: 0x000E7048
			internal override UnsafeNativeMethods.IRawElementProviderFragment GetFocus()
			{
				return this.GetFocused();
			}
		}
	}
}
