using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows <see cref="T:System.Windows.Forms.CheckBox" />.</summary>
	// Token: 0x0200013B RID: 315
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Checked")]
	[DefaultEvent("CheckedChanged")]
	[DefaultBindingProperty("CheckState")]
	[ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionCheckBox")]
	public class CheckBox : ButtonBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.CheckBox" /> class.</summary>
		// Token: 0x0600098E RID: 2446 RVA: 0x0001CCE0 File Offset: 0x0001AEE0
		public CheckBox()
		{
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.flatSystemStylePaddingWidth = base.LogicalToDeviceUnits(25);
				this.flatSystemStyleMinimumHeight = base.LogicalToDeviceUnits(13);
			}
			base.SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
			base.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
			this.autoCheck = true;
			this.TextAlign = ContentAlignment.MiddleLeft;
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0001CD50 File Offset: 0x0001AF50
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0001CD58 File Offset: 0x0001AF58
		private bool AccObjDoDefaultAction
		{
			get
			{
				return this.accObjDoDefaultAction;
			}
			set
			{
				this.accObjDoDefaultAction = value;
			}
		}

		/// <summary>Gets or sets the value that determines the appearance of a <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Appearance" /> values. The default value is <see cref="F:System.Windows.Forms.Appearance.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.Appearance" /> values. </exception>
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0001CD61 File Offset: 0x0001AF61
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x0001CD6C File Offset: 0x0001AF6C
		[DefaultValue(Appearance.Normal)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("CheckBoxAppearanceDescr")]
		public Appearance Appearance
		{
			get
			{
				return this.appearance;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Appearance));
				}
				if (this.appearance != value)
				{
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Appearance))
					{
						this.appearance = value;
						if (base.OwnerDraw)
						{
							this.Refresh();
						}
						else
						{
							base.UpdateStyles();
						}
						this.OnAppearanceChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.CheckBox.Appearance" /> property changes.</summary>
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06000993 RID: 2451 RVA: 0x0001CE04 File Offset: 0x0001B004
		// (remove) Token: 0x06000994 RID: 2452 RVA: 0x0001CE17 File Offset: 0x0001B017
		[SRCategory("CatPropertyChanged")]
		[SRDescription("CheckBoxOnAppearanceChangedDescr")]
		public event EventHandler AppearanceChanged
		{
			add
			{
				base.Events.AddHandler(CheckBox.EVENT_APPEARANCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(CheckBox.EVENT_APPEARANCECHANGED, value);
			}
		}

		/// <summary>Gets or set a value indicating whether the <see cref="P:System.Windows.Forms.CheckBox.Checked" /> or <see cref="P:System.Windows.Forms.CheckBox.CheckState" /> values and the <see cref="T:System.Windows.Forms.CheckBox" />'s appearance are automatically changed when the <see cref="T:System.Windows.Forms.CheckBox" /> is clicked.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.CheckBox.Checked" /> value or <see cref="P:System.Windows.Forms.CheckBox.CheckState" /> value and the appearance of the control are automatically changed on the <see cref="E:System.Windows.Forms.Control.Click" /> event; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0001CE2A File Offset: 0x0001B02A
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x0001CE32 File Offset: 0x0001B032
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("CheckBoxAutoCheckDescr")]
		public bool AutoCheck
		{
			get
			{
				return this.autoCheck;
			}
			set
			{
				this.autoCheck = value;
			}
		}

		/// <summary>Gets or sets the horizontal and vertical alignment of the check mark on a <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default value is <see langword="MiddleLeft" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment" /> enumeration values. </exception>
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0001CE3B File Offset: 0x0001B03B
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0001CE44 File Offset: 0x0001B044
		[Bindable(true)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[SRDescription("CheckBoxCheckAlignDescr")]
		public ContentAlignment CheckAlign
		{
			get
			{
				return this.checkAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (this.checkAlign != value)
				{
					this.checkAlign = value;
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.CheckAlign);
					if (base.OwnerDraw)
					{
						base.Invalidate();
						return;
					}
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or set a value indicating whether the <see cref="T:System.Windows.Forms.CheckBox" /> is in the checked state.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.CheckBox" /> is in the checked state; otherwise, <see langword="false" />. The default value is <see langword="false" />.If the <see cref="P:System.Windows.Forms.CheckBox.ThreeState" /> property is set to <see langword="true" />, the <see cref="P:System.Windows.Forms.CheckBox.Checked" /> property will return <see langword="true" /> for either a <see langword="Checked" /> or <see langword="Indeterminate" /><see cref="P:System.Windows.Forms.CheckBox.CheckState" />.</returns>
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0001CEAB File Offset: 0x0001B0AB
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0001CEB6 File Offset: 0x0001B0B6
		[Bindable(true)]
		[SettingsBindable(true)]
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("CheckBoxCheckedDescr")]
		public bool Checked
		{
			get
			{
				return this.checkState > CheckState.Unchecked;
			}
			set
			{
				if (value != this.Checked)
				{
					this.CheckState = (value ? CheckState.Checked : CheckState.Unchecked);
				}
			}
		}

		/// <summary>Gets or sets the state of the <see cref="T:System.Windows.Forms.CheckBox" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CheckState" /> enumeration values. The default value is <see langword="Unchecked" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.CheckState" /> enumeration values. </exception>
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0001CECE File Offset: 0x0001B0CE
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x0001CED8 File Offset: 0x0001B0D8
		[Bindable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(CheckState.Unchecked)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("CheckBoxCheckStateDescr")]
		public CheckState CheckState
		{
			get
			{
				return this.checkState;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
				}
				if (this.checkState != value)
				{
					bool @checked = this.Checked;
					this.checkState = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(241, (int)this.checkState, 0);
					}
					if (@checked != this.Checked)
					{
						this.OnCheckedChanged(EventArgs.Empty);
					}
					this.OnCheckStateChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
		// Token: 0x1400004C RID: 76
		// (add) Token: 0x0600099D RID: 2461 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x0600099E RID: 2462 RVA: 0x0001B704 File Offset: 0x00019904
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

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x0600099F RID: 2463 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x060009A0 RID: 2464 RVA: 0x0001B716 File Offset: 0x00019916
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

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0001CF5C File Offset: 0x0001B15C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "BUTTON";
				if (base.OwnerDraw)
				{
					createParams.Style |= 11;
				}
				else
				{
					createParams.Style |= 5;
					if (this.Appearance == Appearance.Button)
					{
						createParams.Style |= 4096;
					}
					ContentAlignment contentAlignment = base.RtlTranslateContent(this.CheckAlign);
					if ((contentAlignment & CheckBox.anyRight) != (ContentAlignment)0)
					{
						createParams.Style |= 32;
					}
				}
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default size.</returns>
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0001CFE3 File Offset: 0x0001B1E3
		protected override Size DefaultSize
		{
			get
			{
				return new Size(104, 24);
			}
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001CFEE File Offset: 0x0001B1EE
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			if (DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				this.flatSystemStylePaddingWidth = base.LogicalToDeviceUnits(25);
				this.flatSystemStyleMinimumHeight = base.LogicalToDeviceUnits(13);
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001D01C File Offset: 0x0001B21C
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			if (this.Appearance == Appearance.Button)
			{
				ButtonStandardAdapter buttonStandardAdapter = new ButtonStandardAdapter(this);
				return buttonStandardAdapter.GetPreferredSizeCore(proposedConstraints);
			}
			if (base.FlatStyle != FlatStyle.System)
			{
				return base.GetPreferredSizeCore(proposedConstraints);
			}
			Size clientSize = TextRenderer.MeasureText(this.Text, this.Font);
			Size sz = this.SizeFromClientSize(clientSize);
			sz.Width += this.flatSystemStylePaddingWidth;
			sz.Height = (DpiHelper.EnableDpiChangedHighDpiImprovements ? Math.Max(sz.Height + 5, this.flatSystemStyleMinimumHeight) : (sz.Height + 5));
			return sz + base.Padding.Size;
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0001D0C1 File Offset: 0x0001B2C1
		internal override Rectangle OverChangeRectangle
		{
			get
			{
				if (this.Appearance == Appearance.Button)
				{
					return base.OverChangeRectangle;
				}
				if (base.FlatStyle == FlatStyle.Standard)
				{
					return new Rectangle(-1, -1, 1, 1);
				}
				return base.Adapter.CommonLayout().Layout().checkBounds;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0001D0FB File Offset: 0x0001B2FB
		internal override Rectangle DownChangeRectangle
		{
			get
			{
				if (this.Appearance == Appearance.Button || base.FlatStyle == FlatStyle.System)
				{
					return base.DownChangeRectangle;
				}
				return base.Adapter.CommonLayout().Layout().checkBounds;
			}
		}

		/// <summary>Gets or sets the alignment of the text on the <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default is <see cref="F:System.Drawing.ContentAlignment.MiddleLeft" />.</returns>
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0001D12B File Offset: 0x0001B32B
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0001D133 File Offset: 0x0001B333
		[Localizable(true)]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		public override ContentAlignment TextAlign
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

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.CheckBox" /> will allow three check states rather than two.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.CheckBox" /> is able to display three check states; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0001D13C File Offset: 0x0001B33C
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0001D144 File Offset: 0x0001B344
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("CheckBoxThreeStateDescr")]
		public bool ThreeState
		{
			get
			{
				return this.threeState;
			}
			set
			{
				this.threeState = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.CheckBox.Checked" /> property changes.</summary>
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060009AB RID: 2475 RVA: 0x0001D14D File Offset: 0x0001B34D
		// (remove) Token: 0x060009AC RID: 2476 RVA: 0x0001D160 File Offset: 0x0001B360
		[SRDescription("CheckBoxOnCheckedChangedDescr")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(CheckBox.EVENT_CHECKEDCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(CheckBox.EVENT_CHECKEDCHANGED, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.CheckBox.CheckState" /> property changes.</summary>
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x060009AD RID: 2477 RVA: 0x0001D173 File Offset: 0x0001B373
		// (remove) Token: 0x060009AE RID: 2478 RVA: 0x0001D186 File Offset: 0x0001B386
		[SRDescription("CheckBoxOnCheckStateChangedDescr")]
		public event EventHandler CheckStateChanged
		{
			add
			{
				base.Events.AddHandler(CheckBox.EVENT_CHECKSTATECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(CheckBox.EVENT_CHECKSTATECHANGED, value);
			}
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.CheckBox.CheckBoxAccessibleObject" /> for the control.</returns>
		// Token: 0x060009AF RID: 2479 RVA: 0x0001D199 File Offset: 0x0001B399
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new CheckBox.CheckBoxAccessibleObject(this);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CheckBox.AppearanceChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060009B0 RID: 2480 RVA: 0x0001D1A4 File Offset: 0x0001B3A4
		protected virtual void OnAppearanceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[CheckBox.EVENT_APPEARANCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CheckBox.CheckedChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060009B1 RID: 2481 RVA: 0x0001D1D4 File Offset: 0x0001B3D4
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			if (base.FlatStyle == FlatStyle.System)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.SystemCaptureStart, -1);
			}
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
			base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
			if (base.FlatStyle == FlatStyle.System)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.SystemCaptureEnd, -1);
			}
			EventHandler eventHandler = (EventHandler)base.Events[CheckBox.EVENT_CHECKEDCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CheckBox.CheckStateChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060009B2 RID: 2482 RVA: 0x0001D240 File Offset: 0x0001B440
		protected virtual void OnCheckStateChanged(EventArgs e)
		{
			if (base.OwnerDraw)
			{
				this.Refresh();
			}
			EventHandler eventHandler = (EventHandler)base.Events[CheckBox.EVENT_CHECKSTATECHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060009B3 RID: 2483 RVA: 0x0001D27C File Offset: 0x0001B47C
		protected override void OnClick(EventArgs e)
		{
			if (this.autoCheck)
			{
				CheckState checkState = this.CheckState;
				if (checkState != CheckState.Unchecked)
				{
					if (checkState != CheckState.Checked)
					{
						this.CheckState = CheckState.Unchecked;
					}
					else if (this.threeState)
					{
						this.CheckState = CheckState.Indeterminate;
						if (this.AccObjDoDefaultAction)
						{
							base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
						}
					}
					else
					{
						this.CheckState = CheckState.Unchecked;
					}
				}
				else
				{
					this.CheckState = CheckState.Checked;
				}
			}
			base.OnClick(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060009B4 RID: 2484 RVA: 0x0001D2E6 File Offset: 0x0001B4E6
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (base.IsHandleCreated)
			{
				base.SendMessage(241, (int)this.checkState, 0);
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x060009B5 RID: 2485 RVA: 0x0001D30A File Offset: 0x0001B50A
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
		}

		/// <summary>Raises the OnMouseUp event.</summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x060009B6 RID: 2486 RVA: 0x0001D314 File Offset: 0x0001B514
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left && base.MouseIsPressed && base.MouseIsDown)
			{
				Point point = base.PointToScreen(new Point(mevent.X, mevent.Y));
				if (UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
				{
					base.ResetFlagsandPaint();
					if (!base.ValidationCancelled)
					{
						if (base.Capture)
						{
							this.OnClick(mevent);
						}
						this.OnMouseClick(mevent);
					}
				}
			}
			base.OnMouseUp(mevent);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001D3A1 File Offset: 0x0001B5A1
		internal override ButtonBaseAdapter CreateFlatAdapter()
		{
			return new CheckBoxFlatAdapter(this);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001D3A9 File Offset: 0x0001B5A9
		internal override ButtonBaseAdapter CreatePopupAdapter()
		{
			return new CheckBoxPopupAdapter(this);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001D3B1 File Offset: 0x0001B5B1
		internal override ButtonBaseAdapter CreateStandardAdapter()
		{
			return new CheckBoxStandardAdapter(this);
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process.</param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009BA RID: 2490 RVA: 0x0001D3BC File Offset: 0x0001B5BC
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.UseMnemonic && Control.IsMnemonic(charCode, this.Text) && base.CanSelect)
			{
				if (this.FocusInternal())
				{
					base.ResetFlagsandPaint();
					if (!base.ValidationCancelled)
					{
						this.OnClick(EventArgs.Empty);
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
		/// <returns>A string that states the control type and the state of the <see cref="P:System.Windows.Forms.CheckBox.CheckState" /> property.</returns>
		// Token: 0x060009BB RID: 2491 RVA: 0x0001D40C File Offset: 0x0001B60C
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", CheckState: " + ((int)this.CheckState).ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x040006AA RID: 1706
		private static readonly object EVENT_CHECKEDCHANGED = new object();

		// Token: 0x040006AB RID: 1707
		private static readonly object EVENT_CHECKSTATECHANGED = new object();

		// Token: 0x040006AC RID: 1708
		private static readonly object EVENT_APPEARANCECHANGED = new object();

		// Token: 0x040006AD RID: 1709
		private static readonly ContentAlignment anyRight = (ContentAlignment)1092;

		// Token: 0x040006AE RID: 1710
		private bool autoCheck;

		// Token: 0x040006AF RID: 1711
		private bool threeState;

		// Token: 0x040006B0 RID: 1712
		private bool accObjDoDefaultAction;

		// Token: 0x040006B1 RID: 1713
		private ContentAlignment checkAlign = ContentAlignment.MiddleLeft;

		// Token: 0x040006B2 RID: 1714
		private CheckState checkState;

		// Token: 0x040006B3 RID: 1715
		private Appearance appearance;

		// Token: 0x040006B4 RID: 1716
		private const int FlatSystemStylePaddingWidth = 25;

		// Token: 0x040006B5 RID: 1717
		private const int FlatSystemStyleMinimumHeight = 13;

		// Token: 0x040006B6 RID: 1718
		internal int flatSystemStylePaddingWidth = 25;

		// Token: 0x040006B7 RID: 1719
		internal int flatSystemStyleMinimumHeight = 13;

		/// <summary>Provides information about the <see cref="T:System.Windows.Forms.CheckBox" /> control to accessibility client applications.</summary>
		// Token: 0x02000560 RID: 1376
		[ComVisible(true)]
		public class CheckBoxAccessibleObject : ButtonBase.ButtonBaseAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.CheckBox.CheckBoxAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.CheckBox" /> that owns the <see cref="T:System.Windows.Forms.CheckBox.CheckBoxAccessibleObject" />.</param>
			// Token: 0x0600562D RID: 22061 RVA: 0x001698E9 File Offset: 0x00167AE9
			public CheckBoxAccessibleObject(Control owner) : base(owner)
			{
			}

			/// <summary>Gets a string that describes the default action of the <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
			/// <returns>The description of the default action of the <see cref="T:System.Windows.Forms.CheckBox" /> control.</returns>
			// Token: 0x17001479 RID: 5241
			// (get) Token: 0x0600562E RID: 22062 RVA: 0x001698F4 File Offset: 0x00167AF4
			public override string DefaultAction
			{
				get
				{
					string accessibleDefaultActionDescription = base.Owner.AccessibleDefaultActionDescription;
					if (accessibleDefaultActionDescription != null)
					{
						return accessibleDefaultActionDescription;
					}
					if (((CheckBox)base.Owner).Checked)
					{
						return SR.GetString("AccessibleActionUncheck");
					}
					return SR.GetString("AccessibleActionCheck");
				}
			}

			/// <summary>Gets the role of this accessible object.</summary>
			/// <returns>The <see cref="F:System.Windows.Forms.AccessibleRole.CheckButton" /> value.</returns>
			// Token: 0x1700147A RID: 5242
			// (get) Token: 0x0600562F RID: 22063 RVA: 0x0016993C File Offset: 0x00167B3C
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.CheckButton;
				}
			}

			/// <summary>Gets the state of the <see cref="T:System.Windows.Forms.CheckBox" /> control.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleStates" /> values. If the <see cref="P:System.Windows.Forms.CheckBox.CheckState" /> property is set to <see cref="F:System.Windows.Forms.CheckState.Checked" />, this property returns <see cref="F:System.Windows.Forms.AccessibleStates.Checked" />. If <see cref="P:System.Windows.Forms.CheckBox.CheckState" /> is set to <see cref="F:System.Windows.Forms.CheckState.Indeterminate" />, this property returns <see cref="F:System.Windows.Forms.AccessibleStates.Indeterminate" />.</returns>
			// Token: 0x1700147B RID: 5243
			// (get) Token: 0x06005630 RID: 22064 RVA: 0x00169960 File Offset: 0x00167B60
			public override AccessibleStates State
			{
				get
				{
					CheckState checkState = ((CheckBox)base.Owner).CheckState;
					if (checkState == CheckState.Checked)
					{
						return AccessibleStates.Checked | base.State;
					}
					if (checkState != CheckState.Indeterminate)
					{
						return base.State;
					}
					return AccessibleStates.Mixed | base.State;
				}
			}

			/// <summary>Performs the default action associated with this accessible object.</summary>
			// Token: 0x06005631 RID: 22065 RVA: 0x001699A4 File Offset: 0x00167BA4
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				CheckBox checkBox = base.Owner as CheckBox;
				if (checkBox != null)
				{
					checkBox.AccObjDoDefaultAction = true;
				}
				try
				{
					base.DoDefaultAction();
				}
				finally
				{
					if (checkBox != null)
					{
						checkBox.AccObjDoDefaultAction = false;
					}
				}
			}
		}
	}
}
