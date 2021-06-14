using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Implements the basic functionality of a scroll bar control.</summary>
	// Token: 0x02000345 RID: 837
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Value")]
	[DefaultEvent("Scroll")]
	public abstract class ScrollBar : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ScrollBar" /> class.</summary>
		// Token: 0x06003482 RID: 13442 RVA: 0x000F11E4 File Offset: 0x000EF3E4
		public ScrollBar()
		{
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
			this.TabStop = false;
			if ((this.CreateParams.Style & 1) != 0)
			{
				this.scrollOrientation = ScrollOrientation.VerticalScroll;
				return;
			}
			this.scrollOrientation = ScrollOrientation.HorizontalScroll;
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ScrollBar" /> is automatically resized to fit its contents.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ScrollBar" /> should be automatically resized to fit its contents; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06003483 RID: 13443 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x06003484 RID: 13444 RVA: 0x000B0BCE File Offset: 0x000AEDCE
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ScrollBar.AutoSize" /> property changes.</summary>
		// Token: 0x1400027C RID: 636
		// (add) Token: 0x06003485 RID: 13445 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x06003486 RID: 13446 RVA: 0x0001BA37 File Offset: 0x00019C37
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

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x06003488 RID: 13448 RVA: 0x00011FB9 File Offset: 0x000101B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ScrollBar.BackColor" /> property changes.</summary>
		// Token: 0x1400027D RID: 637
		// (add) Token: 0x06003489 RID: 13449 RVA: 0x00050A7A File Offset: 0x0004EC7A
		// (remove) Token: 0x0600348A RID: 13450 RVA: 0x00050A83 File Offset: 0x0004EC83
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				base.BackColorChanged += value;
			}
			remove
			{
				base.BackColorChanged -= value;
			}
		}

		/// <summary>Gets or sets the background image displayed in the control.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the image to display in the background of the control.</returns>
		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x0600348B RID: 13451 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x0600348C RID: 13452 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ScrollBar.BackgroundImage" /> property changes.</summary>
		// Token: 0x1400027E RID: 638
		// (add) Token: 0x0600348D RID: 13453 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x0600348E RID: 13454 RVA: 0x0001FD8A File Offset: 0x0001DF8A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Gets or sets the background image layout as defined in the <see cref="T:System.Windows.Forms.ImageLayout" /> enumeration.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout" /> (Center , None, Stretch, Tile, or Zoom). Tile is the default value.</returns>
		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06003490 RID: 13456 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ScrollBar.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x1400027F RID: 639
		// (add) Token: 0x06003491 RID: 13457 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06003492 RID: 13458 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06003493 RID: 13459 RVA: 0x000F125C File Offset: 0x000EF45C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SCROLLBAR";
				createParams.Style &= -8388609;
				return createParams;
			}
		}

		/// <summary>Gets the default Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06003494 RID: 13460 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets the default distance between the <see cref="T:System.Windows.Forms.ScrollBar" /> control edges and its contents.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Forms.Padding.Empty" /> in all cases.</returns>
		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06003495 RID: 13461 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000F128E File Offset: 0x000EF48E
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			if (DpiHelper.EnableDpiChangedHighDpiImprovements && this.ScaleScrollBarForDpiChange)
			{
				base.Scale((float)deviceDpiNew / (float)deviceDpiOld);
			}
		}

		/// <summary>Gets or sets the foreground color of the scroll bar control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color for this scroll bar control. The default is the foreground color of the parent control.</returns>
		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06003497 RID: 13463 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x06003498 RID: 13464 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ScrollBar.ForeColor" /> property changes.</summary>
		// Token: 0x14000280 RID: 640
		// (add) Token: 0x06003499 RID: 13465 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x0600349A RID: 13466 RVA: 0x0005276F File Offset: 0x0005096F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		/// <summary>Gets or sets the font of the text displayed by the control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x0600349B RID: 13467 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x0600349C RID: 13468 RVA: 0x00012079 File Offset: 0x00010279
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ScrollBar.Font" /> property changes.</summary>
		// Token: 0x14000281 RID: 641
		// (add) Token: 0x0600349D RID: 13469 RVA: 0x00052778 File Offset: 0x00050978
		// (remove) Token: 0x0600349E RID: 13470 RVA: 0x00052781 File Offset: 0x00050981
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler FontChanged
		{
			add
			{
				base.FontChanged += value;
			}
			remove
			{
				base.FontChanged -= value;
			}
		}

		/// <summary>Gets or sets the Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x060034A0 RID: 13472 RVA: 0x00011FEC File Offset: 0x000101EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ScrollBar.ImeMode" /> property changes.</summary>
		// Token: 0x14000282 RID: 642
		// (add) Token: 0x060034A1 RID: 13473 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x060034A2 RID: 13474 RVA: 0x0001BF35 File Offset: 0x0001A135
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		/// <summary>Gets or sets a value to be added to or subtracted from the <see cref="P:System.Windows.Forms.ScrollBar.Value" /> property when the scroll box is moved a large distance.</summary>
		/// <returns>A numeric value. The default value is 10.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than 0. </exception>
		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x060034A3 RID: 13475 RVA: 0x000F12B2 File Offset: 0x000EF4B2
		// (set) Token: 0x060034A4 RID: 13476 RVA: 0x000F12D0 File Offset: 0x000EF4D0
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
					this.UpdateScrollInfo();
				}
			}
		}

		/// <summary>Gets or sets the upper limit of values of the scrollable range.</summary>
		/// <returns>A numeric value. The default value is 100.</returns>
		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x060034A5 RID: 13477 RVA: 0x000F133A File Offset: 0x000EF53A
		// (set) Token: 0x060034A6 RID: 13478 RVA: 0x000F1342 File Offset: 0x000EF542
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
					this.UpdateScrollInfo();
				}
			}
		}

		/// <summary>Gets or sets the lower limit of values of the scrollable range.</summary>
		/// <returns>A numeric value. The default value is 0.</returns>
		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x060034A7 RID: 13479 RVA: 0x000F137A File Offset: 0x000EF57A
		// (set) Token: 0x060034A8 RID: 13480 RVA: 0x000F1382 File Offset: 0x000EF582
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
				if (this.minimum != value)
				{
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

		/// <summary>Gets or sets the value to be added to or subtracted from the <see cref="P:System.Windows.Forms.ScrollBar.Value" /> property when the scroll box is moved a small distance.</summary>
		/// <returns>A numeric value. The default value is 1.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than 0. </exception>
		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x060034A9 RID: 13481 RVA: 0x000F13BA File Offset: 0x000EF5BA
		// (set) Token: 0x060034AA RID: 13482 RVA: 0x000F13D0 File Offset: 0x000EF5D0
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
					this.UpdateScrollInfo();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to the <see cref="T:System.Windows.Forms.ScrollBar" /> control by using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to the control by using the TAB key; otherwise, false. The default is <see langword="false" />.</returns>
		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x060034AB RID: 13483 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x060034AC RID: 13484 RVA: 0x000AA11D File Offset: 0x000A831D
		[DefaultValue(false)]
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

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x060034AD RID: 13485 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060034AE RID: 13486 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ScrollBar.Text" /> property changes.</summary>
		// Token: 0x14000283 RID: 643
		// (add) Token: 0x060034AF RID: 13487 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x060034B0 RID: 13488 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Gets or sets a numeric value that represents the current position of the scroll box on the scroll bar control.</summary>
		/// <returns>A numeric value that is within the <see cref="P:System.Windows.Forms.ScrollBar.Minimum" /> and <see cref="P:System.Windows.Forms.ScrollBar.Maximum" /> range. The default value is 0.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than the <see cref="P:System.Windows.Forms.ScrollBar.Minimum" /> property value.-or- The assigned value is greater than the <see cref="P:System.Windows.Forms.ScrollBar.Maximum" /> property value. </exception>
		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x060034B1 RID: 13489 RVA: 0x000F143A File Offset: 0x000EF63A
		// (set) Token: 0x060034B2 RID: 13490 RVA: 0x000F1444 File Offset: 0x000EF644
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
					this.OnValueChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x060034B3 RID: 13491 RVA: 0x000F14C6 File Offset: 0x000EF6C6
		// (set) Token: 0x060034B4 RID: 13492 RVA: 0x000F14CE File Offset: 0x000EF6CE
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("ControlDpiChangeScale")]
		public bool ScaleScrollBarForDpiChange
		{
			get
			{
				return this.scaleScrollBarForDpiChange;
			}
			set
			{
				this.scaleScrollBarForDpiChange = value;
			}
		}

		/// <summary>Occurs when the control is clicked if the <see cref="F:System.Windows.Forms.ControlStyles.StandardClick" /> bit flag is set to <see langword="true" /> in a derived class.</summary>
		// Token: 0x14000284 RID: 644
		// (add) Token: 0x060034B5 RID: 13493 RVA: 0x000A2B72 File Offset: 0x000A0D72
		// (remove) Token: 0x060034B6 RID: 13494 RVA: 0x000A2B7B File Offset: 0x000A0D7B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		/// <summary>Occurs when the control is redrawn.</summary>
		// Token: 0x14000285 RID: 645
		// (add) Token: 0x060034B7 RID: 13495 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x060034B8 RID: 13496 RVA: 0x00020D40 File Offset: 0x0001EF40
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ScrollBar" /> control is double-clicked.</summary>
		// Token: 0x14000286 RID: 646
		// (add) Token: 0x060034B9 RID: 13497 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x060034BA RID: 13498 RVA: 0x0001B704 File Offset: 0x00019904
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DoubleClick
		{
			add
			{
				base.DoubleClick += value;
			}
			remove
			{
				base.DoubleClick -= value;
			}
		}

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.ScrollBar" /> control with the mouse.</summary>
		// Token: 0x14000287 RID: 647
		// (add) Token: 0x060034BB RID: 13499 RVA: 0x000A2FE9 File Offset: 0x000A11E9
		// (remove) Token: 0x060034BC RID: 13500 RVA: 0x000A2FF2 File Offset: 0x000A11F2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.ScrollBar" /> control with the mouse.</summary>
		// Token: 0x14000288 RID: 648
		// (add) Token: 0x060034BD RID: 13501 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x060034BE RID: 13502 RVA: 0x0001B716 File Offset: 0x00019916
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.MouseDoubleClick += value;
			}
			remove
			{
				base.MouseDoubleClick -= value;
			}
		}

		/// <summary>Occurs when the mouse pointer is over the control and the user presses a mouse button.</summary>
		// Token: 0x14000289 RID: 649
		// (add) Token: 0x060034BF RID: 13503 RVA: 0x000B0EC2 File Offset: 0x000AF0C2
		// (remove) Token: 0x060034C0 RID: 13504 RVA: 0x000B0ECB File Offset: 0x000AF0CB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				base.MouseDown += value;
			}
			remove
			{
				base.MouseDown -= value;
			}
		}

		/// <summary>Occurs when the user moves the mouse pointer over the control and releases a mouse button.</summary>
		// Token: 0x1400028A RID: 650
		// (add) Token: 0x060034C1 RID: 13505 RVA: 0x000B0ED4 File Offset: 0x000AF0D4
		// (remove) Token: 0x060034C2 RID: 13506 RVA: 0x000B0EDD File Offset: 0x000AF0DD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				base.MouseUp += value;
			}
			remove
			{
				base.MouseUp -= value;
			}
		}

		/// <summary>Occurs when the user moves the mouse pointer over the control.</summary>
		// Token: 0x1400028B RID: 651
		// (add) Token: 0x060034C3 RID: 13507 RVA: 0x000B0EE6 File Offset: 0x000AF0E6
		// (remove) Token: 0x060034C4 RID: 13508 RVA: 0x000B0EEF File Offset: 0x000AF0EF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				base.MouseMove += value;
			}
			remove
			{
				base.MouseMove -= value;
			}
		}

		/// <summary>Occurs when the scroll box has been moved by either a mouse or keyboard action.</summary>
		// Token: 0x1400028C RID: 652
		// (add) Token: 0x060034C5 RID: 13509 RVA: 0x000F14D7 File Offset: 0x000EF6D7
		// (remove) Token: 0x060034C6 RID: 13510 RVA: 0x000F14EA File Offset: 0x000EF6EA
		[SRCategory("CatAction")]
		[SRDescription("ScrollBarOnScrollDescr")]
		public event ScrollEventHandler Scroll
		{
			add
			{
				base.Events.AddHandler(ScrollBar.EVENT_SCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(ScrollBar.EVENT_SCROLL, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ScrollBar.Value" /> property is changed, either by a <see cref="E:System.Windows.Forms.ScrollBar.Scroll" /> event or programmatically.</summary>
		// Token: 0x1400028D RID: 653
		// (add) Token: 0x060034C7 RID: 13511 RVA: 0x000F14FD File Offset: 0x000EF6FD
		// (remove) Token: 0x060034C8 RID: 13512 RVA: 0x000F1510 File Offset: 0x000EF710
		[SRCategory("CatAction")]
		[SRDescription("valueChangedEventDescr")]
		public event EventHandler ValueChanged
		{
			add
			{
				base.Events.AddHandler(ScrollBar.EVENT_VALUECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ScrollBar.EVENT_VALUECHANGED, value);
			}
		}

		/// <summary>Returns the bounds to use when the <see cref="T:System.Windows.Forms.ScrollBar" /> is scaled by a specified amount.</summary>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the initial bounds.</param>
		/// <param name="factor">A <see cref="T:System.Drawing.SizeF" /> that indicates the amount the current bounds should be increased by.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values that indicate the how to define the control's size and position returned by <see cref="M:System.Windows.Forms.ScrollBar.GetScaledBounds(System.Drawing.Rectangle,System.Drawing.SizeF,System.Windows.Forms.BoundsSpecified)" />. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> specifying the scaled bounds.</returns>
		// Token: 0x060034C9 RID: 13513 RVA: 0x000F1523 File Offset: 0x000EF723
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			if (this.scrollOrientation == ScrollOrientation.VerticalScroll)
			{
				specified &= ~BoundsSpecified.Width;
			}
			else
			{
				specified &= ~BoundsSpecified.Height;
			}
			return base.GetScaledBounds(bounds, factor, specified);
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000F1545 File Offset: 0x000EF745
		internal override IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			return IntPtr.Zero;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060034CB RID: 13515 RVA: 0x000F154C File Offset: 0x000EF74C
		protected override void OnEnabledChanged(EventArgs e)
		{
			if (base.Enabled)
			{
				this.UpdateScrollInfo();
			}
			base.OnEnabledChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060034CC RID: 13516 RVA: 0x000F1563 File Offset: 0x000EF763
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.UpdateScrollInfo();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ScrollBar.Scroll" /> event.</summary>
		/// <param name="se">A <see cref="T:System.Windows.Forms.ScrollEventArgs" /> that contains the event data. </param>
		// Token: 0x060034CD RID: 13517 RVA: 0x000F1574 File Offset: 0x000EF774
		protected virtual void OnScroll(ScrollEventArgs se)
		{
			ScrollEventHandler scrollEventHandler = (ScrollEventHandler)base.Events[ScrollBar.EVENT_SCROLL];
			if (scrollEventHandler != null)
			{
				scrollEventHandler(this, se);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /></param>
		// Token: 0x060034CE RID: 13518 RVA: 0x000F15A4 File Offset: 0x000EF7A4
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.wheelDelta += e.Delta;
			bool flag = false;
			while (Math.Abs(this.wheelDelta) >= 120)
			{
				if (this.wheelDelta > 0)
				{
					this.wheelDelta -= 120;
					this.DoScroll(ScrollEventType.SmallDecrement);
					flag = true;
				}
				else
				{
					this.wheelDelta += 120;
					this.DoScroll(ScrollEventType.SmallIncrement);
					flag = true;
				}
			}
			if (flag)
			{
				this.DoScroll(ScrollEventType.EndScroll);
			}
			if (e is HandledMouseEventArgs)
			{
				((HandledMouseEventArgs)e).Handled = true;
			}
			base.OnMouseWheel(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ScrollBar.ValueChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060034CF RID: 13519 RVA: 0x000F1638 File Offset: 0x000EF838
		protected virtual void OnValueChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ScrollBar.EVENT_VALUECHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x000F1666 File Offset: 0x000EF866
		private int ReflectPosition(int position)
		{
			if (this is HScrollBar)
			{
				return this.minimum + (this.maximum - this.LargeChange + 1) - position;
			}
			return position;
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.ScrollBar" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.ScrollBar" />. </returns>
		// Token: 0x060034D1 RID: 13521 RVA: 0x000F168C File Offset: 0x000EF88C
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				", Minimum: ",
				this.Minimum.ToString(CultureInfo.CurrentCulture),
				", Maximum: ",
				this.Maximum.ToString(CultureInfo.CurrentCulture),
				", Value: ",
				this.Value.ToString(CultureInfo.CurrentCulture)
			});
		}

		/// <summary>Updates the <see cref="T:System.Windows.Forms.ScrollBar" /> control.</summary>
		// Token: 0x060034D2 RID: 13522 RVA: 0x000F170C File Offset: 0x000EF90C
		protected void UpdateScrollInfo()
		{
			if (base.IsHandleCreated && base.Enabled)
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
				scrollinfo.fMask = 23;
				scrollinfo.nMin = this.minimum;
				scrollinfo.nMax = this.maximum;
				scrollinfo.nPage = this.LargeChange;
				if (this.RightToLeft == RightToLeft.Yes)
				{
					scrollinfo.nPos = this.ReflectPosition(this.value);
				}
				else
				{
					scrollinfo.nPos = this.value;
				}
				scrollinfo.nTrackPos = 0;
				UnsafeNativeMethods.SetScrollInfo(new HandleRef(this, base.Handle), 2, scrollinfo, true);
			}
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000F17BC File Offset: 0x000EF9BC
		private void WmReflectScroll(ref Message m)
		{
			ScrollEventType type = (ScrollEventType)NativeMethods.Util.LOWORD(m.WParam);
			this.DoScroll(type);
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000F17DC File Offset: 0x000EF9DC
		private void DoScroll(ScrollEventType type)
		{
			if (this.RightToLeft == RightToLeft.Yes)
			{
				switch (type)
				{
				case ScrollEventType.SmallDecrement:
					type = ScrollEventType.SmallIncrement;
					break;
				case ScrollEventType.SmallIncrement:
					type = ScrollEventType.SmallDecrement;
					break;
				case ScrollEventType.LargeDecrement:
					type = ScrollEventType.LargeIncrement;
					break;
				case ScrollEventType.LargeIncrement:
					type = ScrollEventType.LargeDecrement;
					break;
				case ScrollEventType.First:
					type = ScrollEventType.Last;
					break;
				case ScrollEventType.Last:
					type = ScrollEventType.First;
					break;
				}
			}
			int newValue = this.value;
			int oldValue = this.value;
			switch (type)
			{
			case ScrollEventType.SmallDecrement:
				newValue = Math.Max(this.value - this.SmallChange, this.minimum);
				break;
			case ScrollEventType.SmallIncrement:
				newValue = Math.Min(this.value + this.SmallChange, this.maximum - this.LargeChange + 1);
				break;
			case ScrollEventType.LargeDecrement:
				newValue = Math.Max(this.value - this.LargeChange, this.minimum);
				break;
			case ScrollEventType.LargeIncrement:
				newValue = Math.Min(this.value + this.LargeChange, this.maximum - this.LargeChange + 1);
				break;
			case ScrollEventType.ThumbPosition:
			case ScrollEventType.ThumbTrack:
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.fMask = 16;
				SafeNativeMethods.GetScrollInfo(new HandleRef(this, base.Handle), 2, scrollinfo);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					newValue = this.ReflectPosition(scrollinfo.nTrackPos);
				}
				else
				{
					newValue = scrollinfo.nTrackPos;
				}
				break;
			}
			case ScrollEventType.First:
				newValue = this.minimum;
				break;
			case ScrollEventType.Last:
				newValue = this.maximum - this.LargeChange + 1;
				break;
			}
			ScrollEventArgs scrollEventArgs = new ScrollEventArgs(type, oldValue, newValue, this.scrollOrientation);
			this.OnScroll(scrollEventArgs);
			this.Value = scrollEventArgs.NewValue;
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" /> method.</summary>
		/// <param name="m">A Windows Message object.</param>
		// Token: 0x060034D5 RID: 13525 RVA: 0x000F1978 File Offset: 0x000EFB78
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 5)
			{
				if (msg != 20)
				{
					if (msg - 8468 <= 1)
					{
						this.WmReflectScroll(ref m);
						return;
					}
					base.WndProc(ref m);
				}
			}
			else if (UnsafeNativeMethods.GetFocus() == base.Handle)
			{
				this.DefWndProc(ref m);
				base.SendMessage(8, 0, 0);
				base.SendMessage(7, 0, 0);
				return;
			}
		}

		// Token: 0x0400205F RID: 8287
		private static readonly object EVENT_SCROLL = new object();

		// Token: 0x04002060 RID: 8288
		private static readonly object EVENT_VALUECHANGED = new object();

		// Token: 0x04002061 RID: 8289
		private int minimum;

		// Token: 0x04002062 RID: 8290
		private int maximum = 100;

		// Token: 0x04002063 RID: 8291
		private int smallChange = 1;

		// Token: 0x04002064 RID: 8292
		private int largeChange = 10;

		// Token: 0x04002065 RID: 8293
		private int value;

		// Token: 0x04002066 RID: 8294
		private ScrollOrientation scrollOrientation;

		// Token: 0x04002067 RID: 8295
		private int wheelDelta;

		// Token: 0x04002068 RID: 8296
		private bool scaleScrollBarForDpiChange = true;
	}
}
