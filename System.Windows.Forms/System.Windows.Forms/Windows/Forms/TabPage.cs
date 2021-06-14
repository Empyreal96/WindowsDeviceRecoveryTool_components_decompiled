using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a single tab page in a <see cref="T:System.Windows.Forms.TabControl" />.</summary>
	// Token: 0x0200038C RID: 908
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.TabPageDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultEvent("Click")]
	[DefaultProperty("Text")]
	public class TabPage : Panel
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TabPage" /> class.</summary>
		// Token: 0x060038F6 RID: 14582 RVA: 0x000FE875 File Offset: 0x000FCA75
		public TabPage()
		{
			base.SetStyle(ControlStyles.CacheText, true);
			this.Text = null;
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The control grows as much as necessary to fit its contents but does not shrink smaller than the value of its size property</returns>
		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x0000E214 File Offset: 0x0000C414
		// (set) Token: 0x060038F8 RID: 14584 RVA: 0x0000701A File Offset: 0x0000521A
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

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The default value is <see langword="false" />.</returns>
		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000F7531 File Offset: 0x000F5731
		// (set) Token: 0x060038FA RID: 14586 RVA: 0x000F7539 File Offset: 0x000F5739
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabPage.AutoSize" /> property changes.</summary>
		// Token: 0x140002C2 RID: 706
		// (add) Token: 0x060038FB RID: 14587 RVA: 0x000F7542 File Offset: 0x000F5742
		// (remove) Token: 0x060038FC RID: 14588 RVA: 0x000F754B File Offset: 0x000F574B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
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

		/// <summary>Gets or sets the background color for the <see cref="T:System.Windows.Forms.TabPage" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the <see cref="T:System.Windows.Forms.TabPage" />. </returns>
		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x000FE89C File Offset: 0x000FCA9C
		// (set) Token: 0x060038FE RID: 14590 RVA: 0x000FE8EC File Offset: 0x000FCAEC
		[SRCategory("CatAppearance")]
		[SRDescription("ControlBackColorDescr")]
		public override Color BackColor
		{
			get
			{
				Color backColor = base.BackColor;
				if (backColor != Control.DefaultBackColor)
				{
					return backColor;
				}
				TabControl tabControl = this.ParentInternal as TabControl;
				if (Application.RenderWithVisualStyles && this.UseVisualStyleBackColor && tabControl != null && tabControl.Appearance == TabAppearance.Normal)
				{
					return Color.Transparent;
				}
				return backColor;
			}
			set
			{
				if (base.DesignMode)
				{
					if (value != Color.Empty)
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["UseVisualStyleBackColor"];
						if (propertyDescriptor != null)
						{
							propertyDescriptor.SetValue(this, false);
						}
					}
				}
				else
				{
					this.UseVisualStyleBackColor = false;
				}
				base.BackColor = value;
			}
		}

		/// <summary>Creates a new instance of the control collection for the control.</summary>
		/// <returns>A new instance of <see cref="T:System.Windows.Forms.Control.ControlCollection" /> assigned to the control.</returns>
		// Token: 0x060038FF RID: 14591 RVA: 0x000FE93F File Offset: 0x000FCB3F
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new TabPage.TabPageControlCollection(this);
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06003900 RID: 14592 RVA: 0x000FE947 File Offset: 0x000FCB47
		internal ImageList.Indexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ImageList.Indexer();
				}
				return this.imageIndexer;
			}
		}

		/// <summary>Gets or sets the index to the image displayed on this tab.</summary>
		/// <returns>The zero-based index to the image in the <see cref="P:System.Windows.Forms.TabControl.ImageList" /> that appears on the tab. The default is -1, which signifies no image.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Windows.Forms.TabPage.ImageIndex" /> value is less than -1. </exception>
		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06003901 RID: 14593 RVA: 0x000FE962 File Offset: 0x000FCB62
		// (set) Token: 0x06003902 RID: 14594 RVA: 0x000FE970 File Offset: 0x000FCB70
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(-1)]
		[SRDescription("TabItemImageIndexDescr")]
		public int ImageIndex
		{
			get
			{
				return this.ImageIndexer.Index;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"imageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						-1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				TabControl tabControl = this.ParentInternal as TabControl;
				if (tabControl != null)
				{
					this.ImageIndexer.ImageList = tabControl.ImageList;
				}
				this.ImageIndexer.Index = value;
				this.UpdateParent();
			}
		}

		/// <summary>Gets or sets the key accessor for the image in the <see cref="P:System.Windows.Forms.TabControl.ImageList" /> of the associated <see cref="T:System.Windows.Forms.TabControl" />.</summary>
		/// <returns>A string representing the key of the image.</returns>
		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06003903 RID: 14595 RVA: 0x000FE9F6 File Offset: 0x000FCBF6
		// (set) Token: 0x06003904 RID: 14596 RVA: 0x000FEA04 File Offset: 0x000FCC04
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("TabItemImageIndexDescr")]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				this.ImageIndexer.Key = value;
				TabControl tabControl = this.ParentInternal as TabControl;
				if (tabControl != null)
				{
					this.ImageIndexer.ImageList = tabControl.ImageList;
				}
				this.UpdateParent();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TabPage" /> class and specifies the text for the tab.</summary>
		/// <param name="text">The text for the tab. </param>
		// Token: 0x06003905 RID: 14597 RVA: 0x000FEA43 File Offset: 0x000FCC43
		public TabPage(string text) : this()
		{
			this.Text = text;
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AnchorStyles" /> value.</returns>
		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06003906 RID: 14598 RVA: 0x000F7554 File Offset: 0x000F5754
		// (set) Token: 0x06003907 RID: 14599 RVA: 0x000F755C File Offset: 0x000F575C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override AnchorStyles Anchor
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

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DockStyle" /> value.</returns>
		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06003908 RID: 14600 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x06003909 RID: 14601 RVA: 0x000F7576 File Offset: 0x000F5776
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabPage.Dock" /> property changes.</summary>
		// Token: 0x140002C3 RID: 707
		// (add) Token: 0x0600390A RID: 14602 RVA: 0x000F7680 File Offset: 0x000F5880
		// (remove) Token: 0x0600390B RID: 14603 RVA: 0x000F7689 File Offset: 0x000F5889
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The default is <see langword="true" />.</returns>
		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x0600390C RID: 14604 RVA: 0x00012060 File Offset: 0x00010260
		// (set) Token: 0x0600390D RID: 14605 RVA: 0x00012068 File Offset: 0x00010268
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabPage.Enabled" /> property changes.</summary>
		// Token: 0x140002C4 RID: 708
		// (add) Token: 0x0600390E RID: 14606 RVA: 0x000FEA52 File Offset: 0x000FCC52
		// (remove) Token: 0x0600390F RID: 14607 RVA: 0x000FEA5B File Offset: 0x000FCC5B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler EnabledChanged
		{
			add
			{
				base.EnabledChanged += value;
			}
			remove
			{
				base.EnabledChanged -= value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.TabPage" /> background renders using the current visual style when visual styles are enabled.</summary>
		/// <returns>
		///     <see langword="true" /> to render the background using the current visual style; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06003910 RID: 14608 RVA: 0x000FEA64 File Offset: 0x000FCC64
		// (set) Token: 0x06003911 RID: 14609 RVA: 0x000FEA6C File Offset: 0x000FCC6C
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("TabItemUseVisualStyleBackColorDescr")]
		public bool UseVisualStyleBackColor
		{
			get
			{
				return this.useVisualStyleBackColor;
			}
			set
			{
				this.useVisualStyleBackColor = value;
				base.Invalidate(true);
			}
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The x and y coordinates which specifies the location of the object.</returns>
		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06003912 RID: 14610 RVA: 0x000A9351 File Offset: 0x000A7551
		// (set) Token: 0x06003913 RID: 14611 RVA: 0x000A9359 File Offset: 0x000A7559
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabPage.Location" /> property changes.</summary>
		// Token: 0x140002C5 RID: 709
		// (add) Token: 0x06003914 RID: 14612 RVA: 0x000F7692 File Offset: 0x000F5892
		// (remove) Token: 0x06003915 RID: 14613 RVA: 0x000F769B File Offset: 0x000F589B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The upper limit of the size of the objet.</returns>
		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06003916 RID: 14614 RVA: 0x000203D7 File Offset: 0x0001E5D7
		// (set) Token: 0x06003917 RID: 14615 RVA: 0x000F75CF File Offset: 0x000F57CF
		[DefaultValue(typeof(Size), "0, 0")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Size MaximumSize
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

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The lower limit of the size of the objet.</returns>
		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06003918 RID: 14616 RVA: 0x000203F4 File Offset: 0x0001E5F4
		// (set) Token: 0x06003919 RID: 14617 RVA: 0x000F75C6 File Offset: 0x000F57C6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Size MinimumSize
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

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The size of a rectangular area into which the control can fit.</returns>
		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x0600391A RID: 14618 RVA: 0x000FEA7C File Offset: 0x000FCC7C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size PreferredSize
		{
			get
			{
				return base.PreferredSize;
			}
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>The tab order of the control.</returns>
		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x000AA0F2 File Offset: 0x000A82F2
		// (set) Token: 0x0600391C RID: 14620 RVA: 0x000AA0FA File Offset: 0x000A82FA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool RenderTransparencyWithVisualStyles
		{
			get
			{
				return true;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabPage.TabIndex" /> property changes.</summary>
		// Token: 0x140002C6 RID: 710
		// (add) Token: 0x0600391E RID: 14622 RVA: 0x000AA103 File Offset: 0x000A8303
		// (remove) Token: 0x0600391F RID: 14623 RVA: 0x000AA10C File Offset: 0x000A830C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The default is <see langword="true" />.</returns>
		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06003920 RID: 14624 RVA: 0x000F7618 File Offset: 0x000F5818
		// (set) Token: 0x06003921 RID: 14625 RVA: 0x000F7620 File Offset: 0x000F5820
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabPage.TabStop" /> property changes.</summary>
		// Token: 0x140002C7 RID: 711
		// (add) Token: 0x06003922 RID: 14626 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x06003923 RID: 14627 RVA: 0x000AA12F File Offset: 0x000A832F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Gets or sets the text to display on the tab.</summary>
		/// <returns>The text to display on the tab.</returns>
		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06003924 RID: 14628 RVA: 0x000FEA84 File Offset: 0x000FCC84
		// (set) Token: 0x06003925 RID: 14629 RVA: 0x000FEA8C File Offset: 0x000FCC8C
		[Localizable(true)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.UpdateParent();
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabControl.Text" /> property changes.</summary>
		// Token: 0x140002C8 RID: 712
		// (add) Token: 0x06003926 RID: 14630 RVA: 0x000FEA9B File Offset: 0x000FCC9B
		// (remove) Token: 0x06003927 RID: 14631 RVA: 0x000FEAA4 File Offset: 0x000FCCA4
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		/// <summary>Gets or sets the ToolTip text for this tab.</summary>
		/// <returns>The ToolTip text for this tab.</returns>
		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06003928 RID: 14632 RVA: 0x000FEAAD File Offset: 0x000FCCAD
		// (set) Token: 0x06003929 RID: 14633 RVA: 0x000FEAB5 File Offset: 0x000FCCB5
		[DefaultValue("")]
		[Localizable(true)]
		[SRDescription("TabItemToolTipTextDescr")]
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value == this.toolTipText)
				{
					return;
				}
				this.toolTipText = value;
				this.UpdateParent();
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The default is <see langword="true" />.</returns>
		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x0600392A RID: 14634 RVA: 0x000F7629 File Offset: 0x000F5829
		// (set) Token: 0x0600392B RID: 14635 RVA: 0x000F7631 File Offset: 0x000F5831
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabPage.Visible" /> property changes.</summary>
		// Token: 0x140002C9 RID: 713
		// (add) Token: 0x0600392C RID: 14636 RVA: 0x000F766E File Offset: 0x000F586E
		// (remove) Token: 0x0600392D RID: 14637 RVA: 0x000F7677 File Offset: 0x000F5877
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x0600392E RID: 14638 RVA: 0x000FEADD File Offset: 0x000FCCDD
		internal override void AssignParent(Control value)
		{
			if (value != null && !(value is TabControl))
			{
				throw new ArgumentException(SR.GetString("TABCONTROLTabPageNotOnTabControl", new object[]
				{
					value.GetType().FullName
				}));
			}
			base.AssignParent(value);
		}

		/// <summary>Retrieves the tab page that contains the specified object.</summary>
		/// <param name="comp">The object to look for. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.TabPage" /> that contains the specified object, or <see langword="null" /> if the object cannot be found.</returns>
		// Token: 0x0600392F RID: 14639 RVA: 0x000FEB18 File Offset: 0x000FCD18
		public static TabPage GetTabPageOfComponent(object comp)
		{
			if (!(comp is Control))
			{
				return null;
			}
			Control control = (Control)comp;
			while (control != null && !(control is TabPage))
			{
				control = control.ParentInternal;
			}
			return (TabPage)control;
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x000FEB50 File Offset: 0x000FCD50
		internal NativeMethods.TCITEM_T GetTCITEM()
		{
			NativeMethods.TCITEM_T tcitem_T = new NativeMethods.TCITEM_T();
			tcitem_T.mask = 0;
			tcitem_T.pszText = null;
			tcitem_T.cchTextMax = 0;
			tcitem_T.lParam = IntPtr.Zero;
			string text = this.Text;
			this.PrefixAmpersands(ref text);
			if (text != null)
			{
				tcitem_T.mask |= 1;
				tcitem_T.pszText = text;
				tcitem_T.cchTextMax = text.Length;
			}
			int imageIndex = this.ImageIndex;
			tcitem_T.mask |= 2;
			tcitem_T.iImage = this.ImageIndexer.ActualIndex;
			return tcitem_T;
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x000FEBE0 File Offset: 0x000FCDE0
		private void PrefixAmpersands(ref string value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			if (value.IndexOf('&') < 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == '&')
				{
					if (i < value.Length - 1 && value[i + 1] == '&')
					{
						i++;
					}
					stringBuilder.Append("&&");
				}
				else
				{
					stringBuilder.Append(value[i]);
				}
			}
			value = stringBuilder.ToString();
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x000FEC6F File Offset: 0x000FCE6F
		internal void FireLeave(EventArgs e)
		{
			this.leaveFired = true;
			this.OnLeave(e);
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x000FEC7F File Offset: 0x000FCE7F
		internal void FireEnter(EventArgs e)
		{
			this.enterFired = true;
			this.OnEnter(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event of the <see cref="T:System.Windows.Forms.TabPage" />. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003934 RID: 14644 RVA: 0x000FEC90 File Offset: 0x000FCE90
		protected override void OnEnter(EventArgs e)
		{
			TabControl tabControl = this.ParentInternal as TabControl;
			if (tabControl != null)
			{
				if (this.enterFired)
				{
					base.OnEnter(e);
				}
				this.enterFired = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event of the <see cref="T:System.Windows.Forms.TabPage" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003935 RID: 14645 RVA: 0x000FECC4 File Offset: 0x000FCEC4
		protected override void OnLeave(EventArgs e)
		{
			TabControl tabControl = this.ParentInternal as TabControl;
			if (tabControl != null)
			{
				if (this.leaveFired)
				{
					base.OnLeave(e);
				}
				this.leaveFired = false;
			}
		}

		/// <summary>Paints the background of the <see cref="T:System.Windows.Forms.TabPage" />.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains data useful for painting the background. </param>
		// Token: 0x06003936 RID: 14646 RVA: 0x000FECF8 File Offset: 0x000FCEF8
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			TabControl tabControl = this.ParentInternal as TabControl;
			if (Application.RenderWithVisualStyles && this.UseVisualStyleBackColor && tabControl != null && tabControl.Appearance == TabAppearance.Normal)
			{
				Color backColor = this.UseVisualStyleBackColor ? Color.Transparent : this.BackColor;
				Rectangle rectangle = LayoutUtils.InflateRect(this.DisplayRectangle, base.Padding);
				Rectangle bounds = new Rectangle(rectangle.X - 4, rectangle.Y - 2, rectangle.Width + 8, rectangle.Height + 6);
				TabRenderer.DrawTabPage(e.Graphics, bounds);
				if (this.BackgroundImage != null)
				{
					ControlPaint.DrawBackgroundImage(e.Graphics, this.BackgroundImage, backColor, this.BackgroundImageLayout, rectangle, rectangle, this.DisplayRectangle.Location);
					return;
				}
			}
			else
			{
				base.OnPaintBackground(e);
			}
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.SetBoundsCore(System.Int32,System.Int32,System.Int32,System.Int32,System.Windows.Forms.BoundsSpecified)" />.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x06003937 RID: 14647 RVA: 0x000FEDD0 File Offset: 0x000FCFD0
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal is TabControl && parentInternal.IsHandleCreated)
			{
				Rectangle displayRectangle = parentInternal.DisplayRectangle;
				base.SetBoundsCore(displayRectangle.X, displayRectangle.Y, displayRectangle.Width, displayRectangle.Height, (specified == BoundsSpecified.None) ? BoundsSpecified.None : BoundsSpecified.All);
				return;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x000AE30D File Offset: 0x000AC50D
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeLocation()
		{
			return base.Left != 0 || base.Top != 0;
		}

		/// <summary>Returns a string containing the value of the <see cref="P:System.Windows.Forms.TabPage.Text" /> property.</summary>
		/// <returns>A string containing the value of the <see cref="P:System.Windows.Forms.TabPage.Text" /> property.</returns>
		// Token: 0x06003939 RID: 14649 RVA: 0x000FEE34 File Offset: 0x000FD034
		public override string ToString()
		{
			return "TabPage: {" + this.Text + "}";
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x000FEE4C File Offset: 0x000FD04C
		internal void UpdateParent()
		{
			TabControl tabControl = this.ParentInternal as TabControl;
			if (tabControl != null)
			{
				tabControl.UpdateTab(this);
			}
		}

		// Token: 0x0400227C RID: 8828
		private ImageList.Indexer imageIndexer;

		// Token: 0x0400227D RID: 8829
		private string toolTipText = "";

		// Token: 0x0400227E RID: 8830
		private bool enterFired;

		// Token: 0x0400227F RID: 8831
		private bool leaveFired;

		// Token: 0x04002280 RID: 8832
		private bool useVisualStyleBackColor;

		/// <summary>Contains the collection of controls that the <see cref="T:System.Windows.Forms.TabPage" /> uses.</summary>
		// Token: 0x02000727 RID: 1831
		[ComVisible(false)]
		public class TabPageControlCollection : Control.ControlCollection
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TabPage.TabPageControlCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.TabPage" /> that contains this collection of controls. </param>
			// Token: 0x0600609D RID: 24733 RVA: 0x0018BBAB File Offset: 0x00189DAB
			public TabPageControlCollection(TabPage owner) : base(owner)
			{
			}

			/// <summary>Adds a control to the collection.</summary>
			/// <param name="value">The control to add. </param>
			/// <exception cref="T:System.ArgumentException">The specified control is a <see cref="T:System.Windows.Forms.TabPage" />. </exception>
			// Token: 0x0600609E RID: 24734 RVA: 0x0018BBB4 File Offset: 0x00189DB4
			public override void Add(Control value)
			{
				if (value is TabPage)
				{
					throw new ArgumentException(SR.GetString("TABCONTROLTabPageOnTabPage"));
				}
				base.Add(value);
			}
		}
	}
}
