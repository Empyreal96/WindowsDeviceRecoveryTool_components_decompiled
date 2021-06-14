using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Creates a panel that is associated with a <see cref="T:System.Windows.Forms.SplitContainer" />.</summary>
	// Token: 0x02000362 RID: 866
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Docking(DockingBehavior.Never)]
	[Designer("System.Windows.Forms.Design.SplitterPanelDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItem(false)]
	public sealed class SplitterPanel : Panel
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.SplitterPanel" /> class with its specified <see cref="T:System.Windows.Forms.SplitContainer" />. </summary>
		/// <param name="owner">The <see cref="T:System.Windows.Forms.SplitContainer" /> that contains the <see cref="T:System.Windows.Forms.SplitterPanel" />.</param>
		// Token: 0x06003639 RID: 13881 RVA: 0x000F7508 File Offset: 0x000F5708
		public SplitterPanel(SplitContainer owner)
		{
			this.owner = owner;
			base.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x0600363A RID: 13882 RVA: 0x000F7520 File Offset: 0x000F5720
		// (set) Token: 0x0600363B RID: 13883 RVA: 0x000F7528 File Offset: 0x000F5728
		internal bool Collapsed
		{
			get
			{
				return this.collapsed;
			}
			set
			{
				this.collapsed = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.SplitterPanel" /> is automatically resized to display its entire contents. This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.SplitterPanel" /> is automatically resized; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x0600363C RID: 13884 RVA: 0x000F7531 File Offset: 0x000F5731
		// (set) Token: 0x0600363D RID: 13885 RVA: 0x000F7539 File Offset: 0x000F5739
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new bool AutoSize
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x140002A5 RID: 677
		// (add) Token: 0x0600363E RID: 13886 RVA: 0x000F7542 File Offset: 0x000F5742
		// (remove) Token: 0x0600363F RID: 13887 RVA: 0x000F754B File Offset: 0x000F574B
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

		/// <summary>Enables the <see cref="T:System.Windows.Forms.SplitterPanel" /> to shrink when <see cref="P:System.Windows.Forms.SplitterPanel.AutoSize" /> is <see langword="true" />. This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoSizeMode" /> values. The default is <see cref="F:System.Windows.Forms.AutoSizeMode.GrowOnly" />.</returns>
		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06003640 RID: 13888 RVA: 0x0000E214 File Offset: 0x0000C414
		// (set) Token: 0x06003641 RID: 13889 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[Localizable(false)]
		public override AutoSizeMode AutoSizeMode
		{
			get
			{
				return AutoSizeMode.GrowOnly;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets how a <see cref="T:System.Windows.Forms.SplitterPanel" /> attaches to the edges of the <see cref="T:System.Windows.Forms.SplitContainer" />. This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AnchorStyles" /> values.</returns>
		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003642 RID: 13890 RVA: 0x000F7554 File Offset: 0x000F5754
		// (set) Token: 0x06003643 RID: 13891 RVA: 0x000F755C File Offset: 0x000F575C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				base.Anchor = value;
			}
		}

		/// <summary>Gets or sets the border style for the <see cref="T:System.Windows.Forms.SplitterPanel" />. This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </returns>
		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06003644 RID: 13892 RVA: 0x000F7565 File Offset: 0x000F5765
		// (set) Token: 0x06003645 RID: 13893 RVA: 0x000F756D File Offset: 0x000F576D
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		/// <summary>Gets or sets which edge of the <see cref="T:System.Windows.Forms.SplitContainer" /> that the <see cref="T:System.Windows.Forms.SplitterPanel" /> is docked to. This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values.</returns>
		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06003646 RID: 13894 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x06003647 RID: 13895 RVA: 0x000F7576 File Offset: 0x000F5776
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new DockStyle Dock
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

		/// <summary>Gets the internal spacing between the <see cref="T:System.Windows.Forms.SplitterPanel" /> and its edges. This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ScrollableControl.DockPaddingEdges" /> that represents the padding for all the edges of a docked control.</returns>
		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06003648 RID: 13896 RVA: 0x000F757F File Offset: 0x000F577F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}

		/// <summary>Gets or sets the height of the <see cref="T:System.Windows.Forms.SplitterPanel" />.</summary>
		/// <returns>The height of the <see cref="T:System.Windows.Forms.SplitterPanel" />, in pixels.</returns>
		/// <exception cref="T:System.NotSupportedException">The height cannot be set.</exception>
		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x000F7587 File Offset: 0x000F5787
		// (set) Token: 0x0600364A RID: 13898 RVA: 0x000F7599 File Offset: 0x000F5799
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlHeightDescr")]
		public new int Height
		{
			get
			{
				if (this.Collapsed)
				{
					return 0;
				}
				return base.Height;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("SplitContainerPanelHeight"));
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x000F75AA File Offset: 0x000F57AA
		// (set) Token: 0x0600364C RID: 13900 RVA: 0x000F75B2 File Offset: 0x000F57B2
		internal int HeightInternal
		{
			get
			{
				return base.Height;
			}
			set
			{
				base.Height = value;
			}
		}

		/// <summary>Gets or sets the coordinates of the upper-left corner of the <see cref="T:System.Windows.Forms.SplitterPanel" /> relative to the upper-left corner of its <see cref="T:System.Windows.Forms.SplitContainer" />. This property is not relevant to this class.</summary>
		/// <returns>The <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the <see cref="T:System.Windows.Forms.SplitterPanel" /> relative to the upper-left corner of its <see cref="T:System.Windows.Forms.SplitContainer" />.</returns>
		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x0600364D RID: 13901 RVA: 0x000A9351 File Offset: 0x000A7551
		// (set) Token: 0x0600364E RID: 13902 RVA: 0x000A9359 File Offset: 0x000A7559
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x0600364F RID: 13903 RVA: 0x000F75BB File Offset: 0x000F57BB
		protected override Padding DefaultMargin
		{
			get
			{
				return new Padding(0, 0, 0, 0);
			}
		}

		/// <summary>Gets or sets the size that is the lower limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify. This property is not relevant to this class.</summary>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		/// <exception cref="T:System.NotSupportedException">The width cannot be set.</exception>
		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06003650 RID: 13904 RVA: 0x000203F4 File Offset: 0x0001E5F4
		// (set) Token: 0x06003651 RID: 13905 RVA: 0x000F75C6 File Offset: 0x000F57C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new Size MinimumSize
		{
			get
			{
				return base.MinimumSize;
			}
			set
			{
				base.MinimumSize = value;
			}
		}

		/// <summary>Gets or sets the size that is the upper limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> can specify. This property is not relevant to this class.</summary>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06003652 RID: 13906 RVA: 0x000203D7 File Offset: 0x0001E5D7
		// (set) Token: 0x06003653 RID: 13907 RVA: 0x000F75CF File Offset: 0x000F57CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new Size MaximumSize
		{
			get
			{
				return base.MaximumSize;
			}
			set
			{
				base.MaximumSize = value;
			}
		}

		/// <summary>The name of this <see cref="T:System.Windows.Forms.SplitterPanel" />. This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.String" /> representing the name of this <see cref="T:System.Windows.Forms.SplitterPanel" />.</returns>
		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06003654 RID: 13908 RVA: 0x000F75D8 File Offset: 0x000F57D8
		// (set) Token: 0x06003655 RID: 13909 RVA: 0x000F75E0 File Offset: 0x000F57E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06003656 RID: 13910 RVA: 0x000F75E9 File Offset: 0x000F57E9
		internal SplitContainer Owner
		{
			get
			{
				return this.owner;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.SplitContainer" /> that contains this <see cref="T:System.Windows.Forms.SplitterPanel" />. This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Control" /> representing the <see cref="T:System.Windows.Forms.SplitContainer" /> that contains this <see cref="T:System.Windows.Forms.SplitterPanel" />.</returns>
		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06003657 RID: 13911 RVA: 0x000F75F1 File Offset: 0x000F57F1
		// (set) Token: 0x06003658 RID: 13912 RVA: 0x000F75F9 File Offset: 0x000F57F9
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new Control Parent
		{
			get
			{
				return base.Parent;
			}
			set
			{
				base.Parent = value;
			}
		}

		/// <summary>Gets or sets the height and width of the <see cref="T:System.Windows.Forms.SplitterPanel" />. This property is not relevant to this class.</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" /> that represents the height and width of the <see cref="T:System.Windows.Forms.SplitterPanel" /> in pixels.</returns>
		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06003659 RID: 13913 RVA: 0x000F7602 File Offset: 0x000F5802
		// (set) Token: 0x0600365A RID: 13914 RVA: 0x000AA037 File Offset: 0x000A8237
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new Size Size
		{
			get
			{
				if (this.Collapsed)
				{
					return Size.Empty;
				}
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		/// <summary>Gets or sets the tab order of the <see cref="T:System.Windows.Forms.SplitterPanel" /> within its <see cref="T:System.Windows.Forms.SplitContainer" />. This property is not relevant to this class.</summary>
		/// <returns>The index value of the <see cref="T:System.Windows.Forms.SplitterPanel" /> within the set of other <see cref="T:System.Windows.Forms.SplitterPanel" /> objects within its <see cref="T:System.Windows.Forms.SplitContainer" /> that are included in the tab order.</returns>
		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x0600365B RID: 13915 RVA: 0x000AA0F2 File Offset: 0x000A82F2
		// (set) Token: 0x0600365C RID: 13916 RVA: 0x000AA0FA File Offset: 0x000A82FA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to this <see cref="T:System.Windows.Forms.SplitterPanel" /> using the TAB key. This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to this <see cref="T:System.Windows.Forms.SplitterPanel" /> using the TAB key; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x0600365D RID: 13917 RVA: 0x000F7618 File Offset: 0x000F5818
		// (set) Token: 0x0600365E RID: 13918 RVA: 0x000F7620 File Offset: 0x000F5820
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.SplitterPanel" /> is displayed. This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.SplitterPanel" /> is displayed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x0600365F RID: 13919 RVA: 0x000F7629 File Offset: 0x000F5829
		// (set) Token: 0x06003660 RID: 13920 RVA: 0x000F7631 File Offset: 0x000F5831
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		/// <summary>Gets or sets the width of the <see cref="T:System.Windows.Forms.SplitterPanel" />.</summary>
		/// <returns>The width of the <see cref="T:System.Windows.Forms.SplitterPanel" /> in pixels.</returns>
		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003661 RID: 13921 RVA: 0x000F763A File Offset: 0x000F583A
		// (set) Token: 0x06003662 RID: 13922 RVA: 0x000F764C File Offset: 0x000F584C
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlWidthDescr")]
		public new int Width
		{
			get
			{
				if (this.Collapsed)
				{
					return 0;
				}
				return base.Width;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("SplitContainerPanelWidth"));
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06003663 RID: 13923 RVA: 0x000F765D File Offset: 0x000F585D
		// (set) Token: 0x06003664 RID: 13924 RVA: 0x000F7665 File Offset: 0x000F5865
		internal int WidthInternal
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.SplitterPanel.Visible" /> property changes. This event is not relevant to this class.</summary>
		// Token: 0x140002A6 RID: 678
		// (add) Token: 0x06003665 RID: 13925 RVA: 0x000F766E File Offset: 0x000F586E
		// (remove) Token: 0x06003666 RID: 13926 RVA: 0x000F7677 File Offset: 0x000F5877
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler VisibleChanged
		{
			add
			{
				base.VisibleChanged += value;
			}
			remove
			{
				base.VisibleChanged -= value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.SplitterPanel.Dock" /> property changes. This event is not relevant to this class.</summary>
		// Token: 0x140002A7 RID: 679
		// (add) Token: 0x06003667 RID: 13927 RVA: 0x000F7680 File Offset: 0x000F5880
		// (remove) Token: 0x06003668 RID: 13928 RVA: 0x000F7689 File Offset: 0x000F5889
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler DockChanged
		{
			add
			{
				base.DockChanged += value;
			}
			remove
			{
				base.DockChanged -= value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.SplitterPanel.Location" /> property changes. This event is not relevant to this class.</summary>
		// Token: 0x140002A8 RID: 680
		// (add) Token: 0x06003669 RID: 13929 RVA: 0x000F7692 File Offset: 0x000F5892
		// (remove) Token: 0x0600366A RID: 13930 RVA: 0x000F769B File Offset: 0x000F589B
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler LocationChanged
		{
			add
			{
				base.LocationChanged += value;
			}
			remove
			{
				base.LocationChanged -= value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.SplitterPanel.TabIndex" /> property changes. This event is not relevant to this class.</summary>
		// Token: 0x140002A9 RID: 681
		// (add) Token: 0x0600366B RID: 13931 RVA: 0x000AA103 File Offset: 0x000A8303
		// (remove) Token: 0x0600366C RID: 13932 RVA: 0x000AA10C File Offset: 0x000A830C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler TabIndexChanged
		{
			add
			{
				base.TabIndexChanged += value;
			}
			remove
			{
				base.TabIndexChanged -= value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.SplitterPanel.TabStop" /> property changes. This event is not relevant to this class.</summary>
		// Token: 0x140002AA RID: 682
		// (add) Token: 0x0600366D RID: 13933 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x0600366E RID: 13934 RVA: 0x000AA12F File Offset: 0x000A832F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x040021B2 RID: 8626
		private SplitContainer owner;

		// Token: 0x040021B3 RID: 8627
		private bool collapsed;
	}
}
