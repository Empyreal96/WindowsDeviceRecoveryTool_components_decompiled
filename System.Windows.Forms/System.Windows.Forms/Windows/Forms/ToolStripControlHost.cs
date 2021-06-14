using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Hosts custom controls or Windows Forms controls.</summary>
	// Token: 0x020003A7 RID: 935
	public class ToolStripControlHost : ToolStripItem
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripControlHost" /> class that hosts the specified control.</summary>
		/// <param name="c">The <see cref="T:System.Windows.Forms.Control" /> hosted by this <see cref="T:System.Windows.Forms.ToolStripControlHost" /> class. </param>
		/// <exception cref="T:System.ArgumentNullException">The control referred to by the <paramref name="c" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C84 RID: 15492 RVA: 0x0010AE68 File Offset: 0x00109068
		public ToolStripControlHost(Control c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c", "ControlCannotBeNull");
			}
			this.control = c;
			this.SyncControlParent();
			c.Visible = true;
			this.SetBounds(c.Bounds);
			Rectangle bounds = this.Bounds;
			CommonProperties.UpdateSpecifiedBounds(c, bounds.X, bounds.Y, bounds.Width, bounds.Height);
			if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
			{
				c.ToolStripControlHost = this;
			}
			this.OnSubscribeControlEvents(c);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripControlHost" /> class that hosts the specified control and that has the specified name.</summary>
		/// <param name="c">The <see cref="T:System.Windows.Forms.Control" /> hosted by this <see cref="T:System.Windows.Forms.ToolStripControlHost" /> class.</param>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripControlHost" />.</param>
		// Token: 0x06003C85 RID: 15493 RVA: 0x0010AEF4 File Offset: 0x001090F4
		public ToolStripControlHost(Control c, string name) : this(c)
		{
			base.Name = name;
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06003C86 RID: 15494 RVA: 0x0010AF04 File Offset: 0x00109104
		// (set) Token: 0x06003C87 RID: 15495 RVA: 0x0010AF11 File Offset: 0x00109111
		public override Color BackColor
		{
			get
			{
				return this.Control.BackColor;
			}
			set
			{
				this.Control.BackColor = value;
			}
		}

		/// <summary>Gets or sets the background image displayed in the control.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the image to display in the background of the control.</returns>
		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06003C88 RID: 15496 RVA: 0x0010AF1F File Offset: 0x0010911F
		// (set) Token: 0x06003C89 RID: 15497 RVA: 0x0010AF2C File Offset: 0x0010912C
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripItemImageDescr")]
		[DefaultValue(null)]
		public override Image BackgroundImage
		{
			get
			{
				return this.Control.BackgroundImage;
			}
			set
			{
				this.Control.BackgroundImage = value;
			}
		}

		/// <summary>Gets or sets the background image layout as defined in the <see langword="ImageLayout" /> enumeration.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout" />:
		///         <see cref="F:System.Windows.Forms.ImageLayout.Center" />
		///
		///         <see cref="F:System.Windows.Forms.ImageLayout.None" />
		///
		///         <see cref="F:System.Windows.Forms.ImageLayout.Stretch" />
		///
		///         <see cref="F:System.Windows.Forms.ImageLayout.Tile" /> (default)
		///         <see cref="F:System.Windows.Forms.ImageLayout.Zoom" />
		///       </returns>
		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x0010AF3A File Offset: 0x0010913A
		// (set) Token: 0x06003C8B RID: 15499 RVA: 0x0010AF47 File Offset: 0x00109147
		[SRCategory("CatAppearance")]
		[DefaultValue(ImageLayout.Tile)]
		[Localizable(true)]
		[SRDescription("ControlBackgroundImageLayoutDescr")]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return this.Control.BackgroundImageLayout;
			}
			set
			{
				this.Control.BackgroundImageLayout = value;
			}
		}

		/// <summary>Gets a value indicating whether the control can be selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can be selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06003C8C RID: 15500 RVA: 0x0010AF55 File Offset: 0x00109155
		public override bool CanSelect
		{
			get
			{
				return this.control != null && (base.DesignMode || this.Control.CanSelect);
			}
		}

		/// <summary>Gets or sets a value indicating whether the hosted control causes and raises validation events on other controls when the hosted control receives focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the hosted control causes and raises validation events on other controls when the hosted control receives focus; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06003C8D RID: 15501 RVA: 0x0010AF76 File Offset: 0x00109176
		// (set) Token: 0x06003C8E RID: 15502 RVA: 0x0010AF83 File Offset: 0x00109183
		[SRCategory("CatFocus")]
		[DefaultValue(true)]
		[SRDescription("ControlCausesValidationDescr")]
		public bool CausesValidation
		{
			get
			{
				return this.Control.CausesValidation;
			}
			set
			{
				this.Control.CausesValidation = value;
			}
		}

		/// <summary>Gets or sets the alignment of the control on the form.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default is <see cref="F:System.Drawing.ContentAlignment.MiddleCenter" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The <see cref="P:System.Windows.Forms.ToolStripControlHost.ControlAlign" /> property is set to a value that is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values.</exception>
		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06003C8F RID: 15503 RVA: 0x0010AF91 File Offset: 0x00109191
		// (set) Token: 0x06003C90 RID: 15504 RVA: 0x0010AF99 File Offset: 0x00109199
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Browsable(false)]
		public ContentAlignment ControlAlign
		{
			get
			{
				return this.controlAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (this.controlAlign != value)
				{
					this.controlAlign = value;
					this.OnBoundsChanged();
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.Control" /> that this <see cref="T:System.Windows.Forms.ToolStripControlHost" /> is hosting.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that this <see cref="T:System.Windows.Forms.ToolStripControlHost" /> is hosting.</returns>
		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x06003C91 RID: 15505 RVA: 0x0010AFCF File Offset: 0x001091CF
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control Control
		{
			get
			{
				return this.control;
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06003C92 RID: 15506 RVA: 0x0010AFD7 File Offset: 0x001091D7
		internal AccessibleObject ControlAccessibilityObject
		{
			get
			{
				Control control = this.Control;
				if (control == null)
				{
					return null;
				}
				return control.AccessibilityObject;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06003C93 RID: 15507 RVA: 0x0010AFEA File Offset: 0x001091EA
		protected override Size DefaultSize
		{
			get
			{
				if (this.Control != null)
				{
					return this.Control.Size;
				}
				return base.DefaultSize;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The display style of the object.</returns>
		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06003C94 RID: 15508 RVA: 0x0010B006 File Offset: 0x00109206
		// (set) Token: 0x06003C95 RID: 15509 RVA: 0x0010B00E File Offset: 0x0010920E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ToolStripItemDisplayStyle DisplayStyle
		{
			get
			{
				return base.DisplayStyle;
			}
			set
			{
				base.DisplayStyle = value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x140002FD RID: 765
		// (add) Token: 0x06003C96 RID: 15510 RVA: 0x0010B017 File Offset: 0x00109217
		// (remove) Token: 0x06003C97 RID: 15511 RVA: 0x0010B02A File Offset: 0x0010922A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DisplayStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDisplayStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDisplayStyleChanged, value);
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if double clicking is enabled; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x06003C98 RID: 15512 RVA: 0x0010B03D File Offset: 0x0010923D
		// (set) Token: 0x06003C99 RID: 15513 RVA: 0x0010B045 File Offset: 0x00109245
		[DefaultValue(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool DoubleClickEnabled
		{
			get
			{
				return base.DoubleClickEnabled;
			}
			set
			{
				base.DoubleClickEnabled = value;
			}
		}

		/// <summary>Gets or sets the font to be used on the hosted control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> for the hosted control.</returns>
		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06003C9A RID: 15514 RVA: 0x0010B04E File Offset: 0x0010924E
		// (set) Token: 0x06003C9B RID: 15515 RVA: 0x0010B05B File Offset: 0x0010925B
		public override Font Font
		{
			get
			{
				return this.Control.Font;
			}
			set
			{
				this.Control.Font = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the parent control of the <see cref="T:System.Windows.Forms.ToolStripItem" /> is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the parent control of the <see cref="T:System.Windows.Forms.ToolStripItem" /> is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06003C9C RID: 15516 RVA: 0x0010B069 File Offset: 0x00109269
		// (set) Token: 0x06003C9D RID: 15517 RVA: 0x0010B076 File Offset: 0x00109276
		public override bool Enabled
		{
			get
			{
				return this.Control.Enabled;
			}
			set
			{
				this.Control.Enabled = value;
			}
		}

		/// <summary>Occurs when the hosted control is entered.</summary>
		// Token: 0x140002FE RID: 766
		// (add) Token: 0x06003C9E RID: 15518 RVA: 0x0010B084 File Offset: 0x00109284
		// (remove) Token: 0x06003C9F RID: 15519 RVA: 0x0010B097 File Offset: 0x00109297
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnEnterDescr")]
		public event EventHandler Enter
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventEnter, value);
			}
		}

		/// <summary>Gets a value indicating whether the control has input focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the control has input focus; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06003CA0 RID: 15520 RVA: 0x0010B0AA File Offset: 0x001092AA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public virtual bool Focused
		{
			get
			{
				return this.Control.Focused;
			}
		}

		/// <summary>Gets or sets the foreground color of the hosted control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing the foreground color of the hosted control.</returns>
		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x0010B0B7 File Offset: 0x001092B7
		// (set) Token: 0x06003CA2 RID: 15522 RVA: 0x0010B0C4 File Offset: 0x001092C4
		public override Color ForeColor
		{
			get
			{
				return this.Control.ForeColor;
			}
			set
			{
				this.Control.ForeColor = value;
			}
		}

		/// <summary>Occurs when the hosted control receives focus.</summary>
		// Token: 0x140002FF RID: 767
		// (add) Token: 0x06003CA3 RID: 15523 RVA: 0x0010B0D2 File Offset: 0x001092D2
		// (remove) Token: 0x06003CA4 RID: 15524 RVA: 0x0010B0E5 File Offset: 0x001092E5
		[SRCategory("CatFocus")]
		[SRDescription("ToolStripItemOnGotFocusDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler GotFocus
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventGotFocus, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventGotFocus, value);
			}
		}

		/// <summary>The image associated with the object.</summary>
		/// <returns>The image of the hosted control.</returns>
		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06003CA5 RID: 15525 RVA: 0x0010B0F8 File Offset: 0x001092F8
		// (set) Token: 0x06003CA6 RID: 15526 RVA: 0x0010B100 File Offset: 0x00109300
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if an image on a ToolStripItem is automatically resized to fit in a container; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06003CA7 RID: 15527 RVA: 0x0010B109 File Offset: 0x00109309
		// (set) Token: 0x06003CA8 RID: 15528 RVA: 0x0010B111 File Offset: 0x00109311
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ToolStripItemImageScaling ImageScaling
		{
			get
			{
				return base.ImageScaling;
			}
			set
			{
				base.ImageScaling = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The transparent color of the image.</returns>
		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06003CA9 RID: 15529 RVA: 0x0010B11A File Offset: 0x0010931A
		// (set) Token: 0x06003CAA RID: 15530 RVA: 0x0010B122 File Offset: 0x00109322
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ImageTransparentColor
		{
			get
			{
				return base.ImageTransparentColor;
			}
			set
			{
				base.ImageTransparentColor = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The image alignment for the object.</returns>
		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06003CAB RID: 15531 RVA: 0x0010B12B File Offset: 0x0010932B
		// (set) Token: 0x06003CAC RID: 15532 RVA: 0x0010B133 File Offset: 0x00109333
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
			set
			{
				base.ImageAlign = value;
			}
		}

		/// <summary>Occurs when the input focus leaves the hosted control.</summary>
		// Token: 0x14000300 RID: 768
		// (add) Token: 0x06003CAD RID: 15533 RVA: 0x0010B13C File Offset: 0x0010933C
		// (remove) Token: 0x06003CAE RID: 15534 RVA: 0x0010B14F File Offset: 0x0010934F
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnLeaveDescr")]
		public event EventHandler Leave
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventLeave, value);
			}
		}

		/// <summary>Occurs when the hosted control loses focus.</summary>
		// Token: 0x14000301 RID: 769
		// (add) Token: 0x06003CAF RID: 15535 RVA: 0x0010B162 File Offset: 0x00109362
		// (remove) Token: 0x06003CB0 RID: 15536 RVA: 0x0010B175 File Offset: 0x00109375
		[SRCategory("CatFocus")]
		[SRDescription("ToolStripItemOnLostFocusDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler LostFocus
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventLostFocus, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventLostFocus, value);
			}
		}

		/// <summary>Occurs when a key is pressed and held down while the hosted control has focus.</summary>
		// Token: 0x14000302 RID: 770
		// (add) Token: 0x06003CB1 RID: 15537 RVA: 0x0010B188 File Offset: 0x00109388
		// (remove) Token: 0x06003CB2 RID: 15538 RVA: 0x0010B19B File Offset: 0x0010939B
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyDownDescr")]
		public event KeyEventHandler KeyDown
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventKeyDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventKeyDown, value);
			}
		}

		/// <summary>Occurs when a key is pressed while the hosted control has focus.</summary>
		// Token: 0x14000303 RID: 771
		// (add) Token: 0x06003CB3 RID: 15539 RVA: 0x0010B1AE File Offset: 0x001093AE
		// (remove) Token: 0x06003CB4 RID: 15540 RVA: 0x0010B1C1 File Offset: 0x001093C1
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyPressDescr")]
		public event KeyPressEventHandler KeyPress
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventKeyPress, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventKeyPress, value);
			}
		}

		/// <summary>Occurs when a key is released while the hosted control has focus.</summary>
		// Token: 0x14000304 RID: 772
		// (add) Token: 0x06003CB5 RID: 15541 RVA: 0x0010B1D4 File Offset: 0x001093D4
		// (remove) Token: 0x06003CB6 RID: 15542 RVA: 0x0010B1E7 File Offset: 0x001093E7
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyUpDescr")]
		public event KeyEventHandler KeyUp
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventKeyUp, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventKeyUp, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft" /> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit" />.</returns>
		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x0010B1FA File Offset: 0x001093FA
		// (set) Token: 0x06003CB8 RID: 15544 RVA: 0x0010B216 File Offset: 0x00109416
		public override RightToLeft RightToLeft
		{
			get
			{
				if (this.control != null)
				{
					return this.control.RightToLeft;
				}
				return base.RightToLeft;
			}
			set
			{
				if (this.control != null)
				{
					this.control.RightToLeft = value;
				}
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the image is mirrored; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x0010B22C File Offset: 0x0010942C
		// (set) Token: 0x06003CBA RID: 15546 RVA: 0x0010B234 File Offset: 0x00109434
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool RightToLeftAutoMirrorImage
		{
			get
			{
				return base.RightToLeftAutoMirrorImage;
			}
			set
			{
				base.RightToLeftAutoMirrorImage = value;
			}
		}

		/// <summary>Gets a value indicating whether the item is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripItem" /> is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06003CBB RID: 15547 RVA: 0x0010B23D File Offset: 0x0010943D
		public override bool Selected
		{
			get
			{
				return this.Control != null && this.Control.Focused;
			}
		}

		/// <summary>Gets or sets the size of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06003CBC RID: 15548 RVA: 0x0010B254 File Offset: 0x00109454
		// (set) Token: 0x06003CBD RID: 15549 RVA: 0x0010B25C File Offset: 0x0010945C
		public override Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				Rectangle right = Rectangle.Empty;
				if (this.control != null)
				{
					right = this.control.Bounds;
					right.Size = value;
					CommonProperties.UpdateSpecifiedBounds(this.control, right.X, right.Y, right.Width, right.Height);
				}
				base.Size = value;
				if (this.control != null)
				{
					Rectangle bounds = this.control.Bounds;
					if (bounds != right)
					{
						CommonProperties.UpdateSpecifiedBounds(this.control, bounds.X, bounds.Y, bounds.Width, bounds.Height);
					}
				}
			}
		}

		/// <summary>Gets or sets the site of the hosted control.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the control.</returns>
		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06003CBE RID: 15550 RVA: 0x0002981A File Offset: 0x00027A1A
		// (set) Token: 0x06003CBF RID: 15551 RVA: 0x0010B2FD File Offset: 0x001094FD
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if (value != null)
				{
					this.Control.Site = new ToolStripControlHost.StubSite(this.Control, this);
					return;
				}
				this.Control.Site = null;
			}
		}

		/// <summary>Gets or sets the text to be displayed on the hosted control.</summary>
		/// <returns>A <see cref="T:System.String" /> representing the text.</returns>
		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06003CC0 RID: 15552 RVA: 0x0010B32D File Offset: 0x0010952D
		// (set) Token: 0x06003CC1 RID: 15553 RVA: 0x0010B33A File Offset: 0x0010953A
		[DefaultValue("")]
		public override string Text
		{
			get
			{
				return this.Control.Text;
			}
			set
			{
				this.Control.Text = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The text alignment property for the object.</returns>
		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06003CC2 RID: 15554 RVA: 0x0010B348 File Offset: 0x00109548
		// (set) Token: 0x06003CC3 RID: 15555 RVA: 0x0010B350 File Offset: 0x00109550
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The text direction of the tool strip.</returns>
		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06003CC4 RID: 15556 RVA: 0x0010B359 File Offset: 0x00109559
		// (set) Token: 0x06003CC5 RID: 15557 RVA: 0x0010B361 File Offset: 0x00109561
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(ToolStripTextDirection.Horizontal)]
		public override ToolStripTextDirection TextDirection
		{
			get
			{
				return base.TextDirection;
			}
			set
			{
				base.TextDirection = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The relation of a text image with the object.</returns>
		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x0010B36A File Offset: 0x0010956A
		// (set) Token: 0x06003CC7 RID: 15559 RVA: 0x0010B372 File Offset: 0x00109572
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new TextImageRelation TextImageRelation
		{
			get
			{
				return base.TextImageRelation;
			}
			set
			{
				base.TextImageRelation = value;
			}
		}

		/// <summary>Occurs while the hosted control is validating.</summary>
		// Token: 0x14000305 RID: 773
		// (add) Token: 0x06003CC8 RID: 15560 RVA: 0x0010B37B File Offset: 0x0010957B
		// (remove) Token: 0x06003CC9 RID: 15561 RVA: 0x0010B38E File Offset: 0x0010958E
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnValidatingDescr")]
		public event CancelEventHandler Validating
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventValidating, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventValidating, value);
			}
		}

		/// <summary>Occurs after the hosted control has been successfully validated.</summary>
		// Token: 0x14000306 RID: 774
		// (add) Token: 0x06003CCA RID: 15562 RVA: 0x0010B3A1 File Offset: 0x001095A1
		// (remove) Token: 0x06003CCB RID: 15563 RVA: 0x0010B3B4 File Offset: 0x001095B4
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnValidatedDescr")]
		public event EventHandler Validated
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventValidated, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventValidated, value);
			}
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x06003CCC RID: 15564 RVA: 0x0010B3C7 File Offset: 0x001095C7
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return this.Control.AccessibilityObject;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ToolStripControlHost" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06003CCD RID: 15565 RVA: 0x0010B3D4 File Offset: 0x001095D4
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing && this.Control != null)
			{
				this.OnUnsubscribeControlEvents(this.Control);
				this.Control.Dispose();
				this.control = null;
			}
		}

		/// <summary>Gives the focus to a control.</summary>
		// Token: 0x06003CCE RID: 15566 RVA: 0x0010B406 File Offset: 0x00109606
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void Focus()
		{
			this.Control.Focus();
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fitted.</summary>
		/// <param name="constrainingSize">The custom-sized area for a control. </param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x06003CCF RID: 15567 RVA: 0x0010B414 File Offset: 0x00109614
		public override Size GetPreferredSize(Size constrainingSize)
		{
			if (this.control != null)
			{
				return this.Control.GetPreferredSize(constrainingSize - this.Padding.Size) + this.Padding.Size;
			}
			return base.GetPreferredSize(constrainingSize);
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x0010B463 File Offset: 0x00109663
		private void HandleClick(object sender, EventArgs e)
		{
			this.OnClick(e);
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x0010B46C File Offset: 0x0010966C
		private void HandleBackColorChanged(object sender, EventArgs e)
		{
			this.OnBackColorChanged(e);
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x0010B475 File Offset: 0x00109675
		private void HandleDoubleClick(object sender, EventArgs e)
		{
			this.OnDoubleClick(e);
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x0010B47E File Offset: 0x0010967E
		private void HandleDragDrop(object sender, DragEventArgs e)
		{
			this.OnDragDrop(e);
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x0010B487 File Offset: 0x00109687
		private void HandleDragEnter(object sender, DragEventArgs e)
		{
			this.OnDragEnter(e);
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x0010B490 File Offset: 0x00109690
		private void HandleDragLeave(object sender, EventArgs e)
		{
			this.OnDragLeave(e);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0010B499 File Offset: 0x00109699
		private void HandleDragOver(object sender, DragEventArgs e)
		{
			this.OnDragOver(e);
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0010B4A2 File Offset: 0x001096A2
		private void HandleEnter(object sender, EventArgs e)
		{
			this.OnEnter(e);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x0010B4AB File Offset: 0x001096AB
		private void HandleEnabledChanged(object sender, EventArgs e)
		{
			this.OnEnabledChanged(e);
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x0010B4B4 File Offset: 0x001096B4
		private void HandleForeColorChanged(object sender, EventArgs e)
		{
			this.OnForeColorChanged(e);
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x0010B4BD File Offset: 0x001096BD
		private void HandleGiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			this.OnGiveFeedback(e);
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x0010B4C6 File Offset: 0x001096C6
		private void HandleGotFocus(object sender, EventArgs e)
		{
			this.OnGotFocus(e);
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x0010B4CF File Offset: 0x001096CF
		private void HandleLocationChanged(object sender, EventArgs e)
		{
			this.OnLocationChanged(e);
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x0010B4D8 File Offset: 0x001096D8
		private void HandleLostFocus(object sender, EventArgs e)
		{
			this.OnLostFocus(e);
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x0010B4E1 File Offset: 0x001096E1
		private void HandleKeyDown(object sender, KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x0010B4EA File Offset: 0x001096EA
		private void HandleKeyPress(object sender, KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x0010B4F3 File Offset: 0x001096F3
		private void HandleKeyUp(object sender, KeyEventArgs e)
		{
			this.OnKeyUp(e);
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x0010B4FC File Offset: 0x001096FC
		private void HandleLeave(object sender, EventArgs e)
		{
			this.OnLeave(e);
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x0010B505 File Offset: 0x00109705
		private void HandleMouseDown(object sender, MouseEventArgs e)
		{
			this.OnMouseDown(e);
			base.RaiseMouseEvent(ToolStripItem.EventMouseDown, e);
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x0010B51A File Offset: 0x0010971A
		private void HandleMouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
			base.RaiseEvent(ToolStripItem.EventMouseEnter, e);
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x0010B52F File Offset: 0x0010972F
		private void HandleMouseLeave(object sender, EventArgs e)
		{
			this.OnMouseLeave(e);
			base.RaiseEvent(ToolStripItem.EventMouseLeave, e);
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x0010B544 File Offset: 0x00109744
		private void HandleMouseHover(object sender, EventArgs e)
		{
			this.OnMouseHover(e);
			base.RaiseEvent(ToolStripItem.EventMouseHover, e);
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x0010B559 File Offset: 0x00109759
		private void HandleMouseMove(object sender, MouseEventArgs e)
		{
			this.OnMouseMove(e);
			base.RaiseMouseEvent(ToolStripItem.EventMouseMove, e);
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x0010B56E File Offset: 0x0010976E
		private void HandleMouseUp(object sender, MouseEventArgs e)
		{
			this.OnMouseUp(e);
			base.RaiseMouseEvent(ToolStripItem.EventMouseUp, e);
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x0010B583 File Offset: 0x00109783
		private void HandlePaint(object sender, PaintEventArgs e)
		{
			this.OnPaint(e);
			base.RaisePaintEvent(ToolStripItem.EventPaint, e);
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x0010B598 File Offset: 0x00109798
		private void HandleQueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
		{
			QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)base.Events[ToolStripItem.EventQueryAccessibilityHelp];
			if (queryAccessibilityHelpEventHandler != null)
			{
				queryAccessibilityHelpEventHandler(this, e);
			}
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0010B5C6 File Offset: 0x001097C6
		private void HandleQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			this.OnQueryContinueDrag(e);
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x0010B5CF File Offset: 0x001097CF
		private void HandleRightToLeftChanged(object sender, EventArgs e)
		{
			this.OnRightToLeftChanged(e);
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0010B5D8 File Offset: 0x001097D8
		private void HandleResize(object sender, EventArgs e)
		{
			if (this.suspendSyncSizeCount == 0)
			{
				this.OnHostedControlResize(e);
			}
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x0010B5E9 File Offset: 0x001097E9
		private void HandleTextChanged(object sender, EventArgs e)
		{
			this.OnTextChanged(e);
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0010B5F4 File Offset: 0x001097F4
		private void HandleControlVisibleChanged(object sender, EventArgs e)
		{
			bool participatesInLayout = ((IArrangedElement)this.Control).ParticipatesInLayout;
			bool participatesInLayout2 = ((IArrangedElement)this).ParticipatesInLayout;
			if (participatesInLayout2 != participatesInLayout)
			{
				base.Visible = this.Control.Visible;
			}
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x0010B629 File Offset: 0x00109829
		private void HandleValidating(object sender, CancelEventArgs e)
		{
			this.OnValidating(e);
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x0010B632 File Offset: 0x00109832
		private void HandleValidated(object sender, EventArgs e)
		{
			this.OnValidated(e);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0010B63B File Offset: 0x0010983B
		internal override void OnAccessibleDescriptionChanged(EventArgs e)
		{
			this.Control.AccessibleDescription = base.AccessibleDescription;
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x0010B64E File Offset: 0x0010984E
		internal override void OnAccessibleNameChanged(EventArgs e)
		{
			this.Control.AccessibleName = base.AccessibleName;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x0010B661 File Offset: 0x00109861
		internal override void OnAccessibleDefaultActionDescriptionChanged(EventArgs e)
		{
			this.Control.AccessibleDefaultActionDescription = base.AccessibleDefaultActionDescription;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x0010B674 File Offset: 0x00109874
		internal override void OnAccessibleRoleChanged(EventArgs e)
		{
			this.Control.AccessibleRole = base.AccessibleRole;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.Enter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003CF5 RID: 15605 RVA: 0x0010B687 File Offset: 0x00109887
		protected virtual void OnEnter(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventEnter, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003CF6 RID: 15606 RVA: 0x0010B695 File Offset: 0x00109895
		protected virtual void OnGotFocus(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventGotFocus, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.Leave" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003CF7 RID: 15607 RVA: 0x0010B6A3 File Offset: 0x001098A3
		protected virtual void OnLeave(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventLeave, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.LostFocus" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003CF8 RID: 15608 RVA: 0x0010B6B1 File Offset: 0x001098B1
		protected virtual void OnLostFocus(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventLostFocus, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.KeyDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06003CF9 RID: 15609 RVA: 0x0010B6BF File Offset: 0x001098BF
		protected virtual void OnKeyDown(KeyEventArgs e)
		{
			base.RaiseKeyEvent(ToolStripControlHost.EventKeyDown, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.KeyPress" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data.</param>
		// Token: 0x06003CFA RID: 15610 RVA: 0x0010B6CD File Offset: 0x001098CD
		protected virtual void OnKeyPress(KeyPressEventArgs e)
		{
			base.RaiseKeyPressEvent(ToolStripControlHost.EventKeyPress, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.KeyUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06003CFB RID: 15611 RVA: 0x0010B6DB File Offset: 0x001098DB
		protected virtual void OnKeyUp(KeyEventArgs e)
		{
			base.RaiseKeyEvent(ToolStripControlHost.EventKeyUp, e);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripItem.Bounds" /> property changes.</summary>
		// Token: 0x06003CFC RID: 15612 RVA: 0x0010B6EC File Offset: 0x001098EC
		protected override void OnBoundsChanged()
		{
			if (this.control != null)
			{
				this.SuspendSizeSync();
				IArrangedElement arrangedElement = this.control;
				if (arrangedElement == null)
				{
					return;
				}
				Size size = LayoutUtils.DeflateRect(this.Bounds, this.Padding).Size;
				Rectangle rectangle = LayoutUtils.Align(size, this.Bounds, this.ControlAlign);
				arrangedElement.SetBounds(rectangle, BoundsSpecified.None);
				if (rectangle != this.control.Bounds)
				{
					rectangle = LayoutUtils.Align(this.control.Size, this.Bounds, this.ControlAlign);
					arrangedElement.SetBounds(rectangle, BoundsSpecified.None);
				}
				this.ResumeSizeSync();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x06003CFD RID: 15613 RVA: 0x0000701A File Offset: 0x0000521A
		protected override void OnPaint(PaintEventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
		// Token: 0x06003CFE RID: 15614 RVA: 0x0000701A File Offset: 0x0000521A
		protected internal override void OnLayout(LayoutEventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="oldParent">The original parent of the item.</param>
		/// <param name="newParent">The new parent of the item.</param>
		// Token: 0x06003CFF RID: 15615 RVA: 0x0010B788 File Offset: 0x00109988
		protected override void OnParentChanged(ToolStrip oldParent, ToolStrip newParent)
		{
			if (oldParent != null && base.Owner == null && newParent == null && this.Control != null)
			{
				WindowsFormsUtils.ReadOnlyControlCollection controlCollection = ToolStripControlHost.GetControlCollection(this.Control.ParentInternal as ToolStrip);
				if (controlCollection != null)
				{
					controlCollection.RemoveInternal(this.Control);
				}
			}
			else
			{
				this.SyncControlParent();
			}
			base.OnParentChanged(oldParent, newParent);
		}

		/// <summary>Subscribes events from the hosted control.</summary>
		/// <param name="control">The control from which to subscribe events.</param>
		// Token: 0x06003D00 RID: 15616 RVA: 0x0010B7E0 File Offset: 0x001099E0
		protected virtual void OnSubscribeControlEvents(Control control)
		{
			if (control != null)
			{
				control.Click += this.HandleClick;
				control.BackColorChanged += this.HandleBackColorChanged;
				control.DoubleClick += this.HandleDoubleClick;
				control.DragDrop += this.HandleDragDrop;
				control.DragEnter += this.HandleDragEnter;
				control.DragLeave += this.HandleDragLeave;
				control.DragOver += this.HandleDragOver;
				control.Enter += this.HandleEnter;
				control.EnabledChanged += this.HandleEnabledChanged;
				control.ForeColorChanged += this.HandleForeColorChanged;
				control.GiveFeedback += this.HandleGiveFeedback;
				control.GotFocus += this.HandleGotFocus;
				control.Leave += this.HandleLeave;
				control.LocationChanged += this.HandleLocationChanged;
				control.LostFocus += this.HandleLostFocus;
				control.KeyDown += this.HandleKeyDown;
				control.KeyPress += this.HandleKeyPress;
				control.KeyUp += this.HandleKeyUp;
				control.MouseDown += this.HandleMouseDown;
				control.MouseEnter += this.HandleMouseEnter;
				control.MouseHover += this.HandleMouseHover;
				control.MouseLeave += this.HandleMouseLeave;
				control.MouseMove += this.HandleMouseMove;
				control.MouseUp += this.HandleMouseUp;
				control.Paint += this.HandlePaint;
				control.QueryAccessibilityHelp += this.HandleQueryAccessibilityHelp;
				control.QueryContinueDrag += this.HandleQueryContinueDrag;
				control.Resize += this.HandleResize;
				control.RightToLeftChanged += this.HandleRightToLeftChanged;
				control.TextChanged += this.HandleTextChanged;
				control.VisibleChanged += this.HandleControlVisibleChanged;
				control.Validating += this.HandleValidating;
				control.Validated += this.HandleValidated;
			}
		}

		/// <summary>Unsubscribes events from the hosted control.</summary>
		/// <param name="control">The control from which to unsubscribe events.</param>
		// Token: 0x06003D01 RID: 15617 RVA: 0x0010BA48 File Offset: 0x00109C48
		protected virtual void OnUnsubscribeControlEvents(Control control)
		{
			if (control != null)
			{
				control.Click -= this.HandleClick;
				control.BackColorChanged -= this.HandleBackColorChanged;
				control.DoubleClick -= this.HandleDoubleClick;
				control.DragDrop -= this.HandleDragDrop;
				control.DragEnter -= this.HandleDragEnter;
				control.DragLeave -= this.HandleDragLeave;
				control.DragOver -= this.HandleDragOver;
				control.Enter -= this.HandleEnter;
				control.EnabledChanged -= this.HandleEnabledChanged;
				control.ForeColorChanged -= this.HandleForeColorChanged;
				control.GiveFeedback -= this.HandleGiveFeedback;
				control.GotFocus -= this.HandleGotFocus;
				control.Leave -= this.HandleLeave;
				control.LocationChanged -= this.HandleLocationChanged;
				control.LostFocus -= this.HandleLostFocus;
				control.KeyDown -= this.HandleKeyDown;
				control.KeyPress -= this.HandleKeyPress;
				control.KeyUp -= this.HandleKeyUp;
				control.MouseDown -= this.HandleMouseDown;
				control.MouseEnter -= this.HandleMouseEnter;
				control.MouseHover -= this.HandleMouseHover;
				control.MouseLeave -= this.HandleMouseLeave;
				control.MouseMove -= this.HandleMouseMove;
				control.MouseUp -= this.HandleMouseUp;
				control.Paint -= this.HandlePaint;
				control.QueryAccessibilityHelp -= this.HandleQueryAccessibilityHelp;
				control.QueryContinueDrag -= this.HandleQueryContinueDrag;
				control.Resize -= this.HandleResize;
				control.RightToLeftChanged -= this.HandleRightToLeftChanged;
				control.TextChanged -= this.HandleTextChanged;
				control.VisibleChanged -= this.HandleControlVisibleChanged;
				control.Validating -= this.HandleValidating;
				control.Validated -= this.HandleValidated;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.Validating" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
		// Token: 0x06003D02 RID: 15618 RVA: 0x0010BCAD File Offset: 0x00109EAD
		protected virtual void OnValidating(CancelEventArgs e)
		{
			base.RaiseCancelEvent(ToolStripControlHost.EventValidating, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripControlHost.Validated" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003D03 RID: 15619 RVA: 0x0010BCBB File Offset: 0x00109EBB
		protected virtual void OnValidated(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventValidated, e);
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x0010BCCC File Offset: 0x00109ECC
		private static WindowsFormsUtils.ReadOnlyControlCollection GetControlCollection(ToolStrip toolStrip)
		{
			return (toolStrip != null) ? ((WindowsFormsUtils.ReadOnlyControlCollection)toolStrip.Controls) : null;
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x0010BCEC File Offset: 0x00109EEC
		private void SyncControlParent()
		{
			WindowsFormsUtils.ReadOnlyControlCollection controlCollection = ToolStripControlHost.GetControlCollection(base.ParentInternal);
			if (controlCollection != null)
			{
				controlCollection.AddInternal(this.Control);
			}
		}

		/// <summary>Synchronizes the resizing of the control host with the resizing of the hosted control.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003D06 RID: 15622 RVA: 0x0010BD14 File Offset: 0x00109F14
		protected virtual void OnHostedControlResize(EventArgs e)
		{
			this.Size = this.Control.Size;
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x06003D07 RID: 15623 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			return false;
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process.</param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003D08 RID: 15624 RVA: 0x0010BD27 File Offset: 0x00109F27
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (this.control != null)
			{
				return this.control.ProcessMnemonic(charCode);
			}
			return base.ProcessMnemonic(charCode);
		}

		/// <summary>Processes a dialog key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the item; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003D09 RID: 15625 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessDialogKey(Keys keyData)
		{
			return false;
		}

		/// <summary>Sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> to the specified visible state. </summary>
		/// <param name="visible">
		///       <see langword="true" /> to make the ToolStripItem visible; otherwise, <see langword="false" />.</param>
		// Token: 0x06003D0A RID: 15626 RVA: 0x0010BD48 File Offset: 0x00109F48
		protected override void SetVisibleCore(bool visible)
		{
			if (this.inSetVisibleCore)
			{
				return;
			}
			this.inSetVisibleCore = true;
			this.Control.SuspendLayout();
			try
			{
				this.Control.Visible = visible;
			}
			finally
			{
				this.Control.ResumeLayout(false);
				base.SetVisibleCore(visible);
				this.inSetVisibleCore = false;
			}
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06003D0B RID: 15627 RVA: 0x0010BDAC File Offset: 0x00109FAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetBackColor()
		{
			this.Control.ResetBackColor();
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06003D0C RID: 15628 RVA: 0x0010BDB9 File Offset: 0x00109FB9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetForeColor()
		{
			this.Control.ResetForeColor();
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x0010BDC6 File Offset: 0x00109FC6
		private void SuspendSizeSync()
		{
			this.suspendSyncSizeCount++;
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x0010BDD6 File Offset: 0x00109FD6
		private void ResumeSizeSync()
		{
			this.suspendSyncSizeCount--;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x0010BDE6 File Offset: 0x00109FE6
		internal override bool ShouldSerializeBackColor()
		{
			if (this.control != null)
			{
				return this.control.ShouldSerializeBackColor();
			}
			return base.ShouldSerializeBackColor();
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x0010BE02 File Offset: 0x0010A002
		internal override bool ShouldSerializeForeColor()
		{
			if (this.control != null)
			{
				return this.control.ShouldSerializeForeColor();
			}
			return base.ShouldSerializeForeColor();
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x0010BE1E File Offset: 0x0010A01E
		internal override bool ShouldSerializeFont()
		{
			if (this.control != null)
			{
				return this.control.ShouldSerializeFont();
			}
			return base.ShouldSerializeFont();
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x0010BE3A File Offset: 0x0010A03A
		internal override bool ShouldSerializeRightToLeft()
		{
			if (this.control != null)
			{
				return this.control.ShouldSerializeRightToLeft();
			}
			return base.ShouldSerializeRightToLeft();
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x0010BE56 File Offset: 0x0010A056
		internal override void OnKeyboardToolTipHook(ToolTip toolTip)
		{
			base.OnKeyboardToolTipHook(toolTip);
			KeyboardToolTipStateMachine.Instance.Hook(this.Control, toolTip);
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x0010BE70 File Offset: 0x0010A070
		internal override void OnKeyboardToolTipUnhook(ToolTip toolTip)
		{
			base.OnKeyboardToolTipUnhook(toolTip);
			KeyboardToolTipStateMachine.Instance.Unhook(this.Control, toolTip);
		}

		// Token: 0x040023A3 RID: 9123
		private Control control;

		// Token: 0x040023A4 RID: 9124
		private int suspendSyncSizeCount;

		// Token: 0x040023A5 RID: 9125
		private ContentAlignment controlAlign = ContentAlignment.MiddleCenter;

		// Token: 0x040023A6 RID: 9126
		private bool inSetVisibleCore;

		// Token: 0x040023A7 RID: 9127
		internal static readonly object EventGotFocus = new object();

		// Token: 0x040023A8 RID: 9128
		internal static readonly object EventLostFocus = new object();

		// Token: 0x040023A9 RID: 9129
		internal static readonly object EventKeyDown = new object();

		// Token: 0x040023AA RID: 9130
		internal static readonly object EventKeyPress = new object();

		// Token: 0x040023AB RID: 9131
		internal static readonly object EventKeyUp = new object();

		// Token: 0x040023AC RID: 9132
		internal static readonly object EventEnter = new object();

		// Token: 0x040023AD RID: 9133
		internal static readonly object EventLeave = new object();

		// Token: 0x040023AE RID: 9134
		internal static readonly object EventValidated = new object();

		// Token: 0x040023AF RID: 9135
		internal static readonly object EventValidating = new object();

		// Token: 0x02000733 RID: 1843
		private class StubSite : ISite, IServiceProvider, IDictionaryService
		{
			// Token: 0x060060FA RID: 24826 RVA: 0x0018CF11 File Offset: 0x0018B111
			public StubSite(Component control, Component host)
			{
				this.comp = control;
				this.owner = host;
			}

			// Token: 0x17001726 RID: 5926
			// (get) Token: 0x060060FB RID: 24827 RVA: 0x0018CF27 File Offset: 0x0018B127
			IComponent ISite.Component
			{
				get
				{
					return this.comp;
				}
			}

			// Token: 0x17001727 RID: 5927
			// (get) Token: 0x060060FC RID: 24828 RVA: 0x0018CF2F File Offset: 0x0018B12F
			IContainer ISite.Container
			{
				get
				{
					return this.owner.Site.Container;
				}
			}

			// Token: 0x17001728 RID: 5928
			// (get) Token: 0x060060FD RID: 24829 RVA: 0x0018CF41 File Offset: 0x0018B141
			bool ISite.DesignMode
			{
				get
				{
					return this.owner.Site.DesignMode;
				}
			}

			// Token: 0x17001729 RID: 5929
			// (get) Token: 0x060060FE RID: 24830 RVA: 0x0018CF53 File Offset: 0x0018B153
			// (set) Token: 0x060060FF RID: 24831 RVA: 0x0018CF65 File Offset: 0x0018B165
			string ISite.Name
			{
				get
				{
					return this.owner.Site.Name;
				}
				set
				{
					this.owner.Site.Name = value;
				}
			}

			// Token: 0x06006100 RID: 24832 RVA: 0x0018CF78 File Offset: 0x0018B178
			object IServiceProvider.GetService(Type service)
			{
				if (service == null)
				{
					throw new ArgumentNullException("service");
				}
				if (service == typeof(IDictionaryService))
				{
					return this;
				}
				if (this.owner.Site != null)
				{
					return this.owner.Site.GetService(service);
				}
				return null;
			}

			// Token: 0x06006101 RID: 24833 RVA: 0x0018CFD0 File Offset: 0x0018B1D0
			object IDictionaryService.GetKey(object value)
			{
				if (this._dictionary != null)
				{
					foreach (object obj in this._dictionary)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						object value2 = dictionaryEntry.Value;
						if (value != null && value.Equals(value2))
						{
							return dictionaryEntry.Key;
						}
					}
				}
				return null;
			}

			// Token: 0x06006102 RID: 24834 RVA: 0x0018D050 File Offset: 0x0018B250
			object IDictionaryService.GetValue(object key)
			{
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				return null;
			}

			// Token: 0x06006103 RID: 24835 RVA: 0x0018D068 File Offset: 0x0018B268
			void IDictionaryService.SetValue(object key, object value)
			{
				if (this._dictionary == null)
				{
					this._dictionary = new Hashtable();
				}
				if (value == null)
				{
					this._dictionary.Remove(key);
					return;
				}
				this._dictionary[key] = value;
			}

			// Token: 0x04004170 RID: 16752
			private Hashtable _dictionary;

			// Token: 0x04004171 RID: 16753
			private IComponent comp;

			// Token: 0x04004172 RID: 16754
			private IComponent owner;
		}
	}
}
