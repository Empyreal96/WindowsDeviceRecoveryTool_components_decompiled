using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Represents a text box in a <see cref="T:System.Windows.Forms.ToolStrip" /> that allows the user to enter text.</summary>
	// Token: 0x020003F8 RID: 1016
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	public class ToolStripTextBox : ToolStripControlHost
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> class.</summary>
		// Token: 0x06004489 RID: 17545 RVA: 0x001254F8 File Offset: 0x001236F8
		public ToolStripTextBox() : base(ToolStripTextBox.CreateControlInstance())
		{
			ToolStripTextBox.ToolStripTextBoxControl toolStripTextBoxControl = base.Control as ToolStripTextBox.ToolStripTextBoxControl;
			toolStripTextBoxControl.Owner = this;
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.scaledDefaultMargin = DpiHelper.LogicalToDeviceUnits(ToolStripTextBox.defaultMargin, 0);
				this.scaledDefaultDropDownMargin = DpiHelper.LogicalToDeviceUnits(ToolStripTextBox.defaultDropDownMargin, 0);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> class with the specified name. </summary>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripTextBox" />.</param>
		// Token: 0x0600448A RID: 17546 RVA: 0x00125562 File Offset: 0x00123762
		public ToolStripTextBox(string name) : this()
		{
			base.Name = name;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> class derived from a base control.</summary>
		/// <param name="c">The control from which to derive the <see cref="T:System.Windows.Forms.ToolStripTextBox" />. </param>
		// Token: 0x0600448B RID: 17547 RVA: 0x00125571 File Offset: 0x00123771
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ToolStripTextBox(Control c) : base(c)
		{
			throw new NotSupportedException(SR.GetString("ToolStripMustSupplyItsOwnTextBox"));
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The background image displayed in the control.</returns>
		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x0600448C RID: 17548 RVA: 0x0010A87B File Offset: 0x00108A7B
		// (set) Token: 0x0600448D RID: 17549 RVA: 0x0010A883 File Offset: 0x00108A83
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x0600448E RID: 17550 RVA: 0x0010A88C File Offset: 0x00108A8C
		// (set) Token: 0x0600448F RID: 17551 RVA: 0x0010A894 File Offset: 0x00108A94
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Gets the spacing, in pixels, between the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> and adjacent items.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value representing the spacing.</returns>
		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x06004490 RID: 17552 RVA: 0x0012559F File Offset: 0x0012379F
		protected internal override Padding DefaultMargin
		{
			get
			{
				if (base.IsOnDropDown)
				{
					return this.scaledDefaultDropDownMargin;
				}
				return this.scaledDefaultMargin;
			}
		}

		/// <summary>Gets the default size of the <see cref="T:System.Windows.Forms.ToolStripTextBox" />.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> in pixels. The default size is 100 pixels by 25 pixels.</returns>
		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x06004491 RID: 17553 RVA: 0x000F782D File Offset: 0x000F5A2D
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 22);
			}
		}

		/// <summary>Gets the hosted <see cref="T:System.Windows.Forms.TextBox" /> control.</summary>
		/// <returns>The hosted <see cref="T:System.Windows.Forms.TextBox" />.</returns>
		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06004492 RID: 17554 RVA: 0x001255B6 File Offset: 0x001237B6
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextBox TextBox
		{
			get
			{
				return base.Control as TextBox;
			}
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x001255C3 File Offset: 0x001237C3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ToolStripTextBox.ToolStripTextBoxAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x001255DC File Offset: 0x001237DC
		private static Control CreateControlInstance()
		{
			return new ToolStripTextBox.ToolStripTextBoxControl
			{
				BorderStyle = BorderStyle.Fixed3D,
				AutoSize = true
			};
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fitted.</summary>
		/// <param name="constrainingSize">The custom-sized area for a control.</param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x06004495 RID: 17557 RVA: 0x00125600 File Offset: 0x00123800
		public override Size GetPreferredSize(Size constrainingSize)
		{
			return new Size(CommonProperties.GetSpecifiedBounds(this.TextBox).Width, this.TextBox.PreferredHeight);
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x00125630 File Offset: 0x00123830
		private void HandleAcceptsTabChanged(object sender, EventArgs e)
		{
			this.OnAcceptsTabChanged(e);
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x00125639 File Offset: 0x00123839
		private void HandleBorderStyleChanged(object sender, EventArgs e)
		{
			this.OnBorderStyleChanged(e);
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x00125642 File Offset: 0x00123842
		private void HandleHideSelectionChanged(object sender, EventArgs e)
		{
			this.OnHideSelectionChanged(e);
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x0012564B File Offset: 0x0012384B
		private void HandleModifiedChanged(object sender, EventArgs e)
		{
			this.OnModifiedChanged(e);
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x00125654 File Offset: 0x00123854
		private void HandleMultilineChanged(object sender, EventArgs e)
		{
			this.OnMultilineChanged(e);
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x0012565D File Offset: 0x0012385D
		private void HandleReadOnlyChanged(object sender, EventArgs e)
		{
			this.OnReadOnlyChanged(e);
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x00125666 File Offset: 0x00123866
		private void HandleTextBoxTextAlignChanged(object sender, EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventTextBoxTextAlignChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripTextBox.AcceptsTabChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600449D RID: 17565 RVA: 0x00125674 File Offset: 0x00123874
		protected virtual void OnAcceptsTabChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventAcceptsTabChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripTextBox.BorderStyleChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600449E RID: 17566 RVA: 0x00125682 File Offset: 0x00123882
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventBorderStyleChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripTextBox.HideSelectionChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600449F RID: 17567 RVA: 0x00125690 File Offset: 0x00123890
		protected virtual void OnHideSelectionChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventHideSelectionChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripTextBox.ModifiedChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060044A0 RID: 17568 RVA: 0x0012569E File Offset: 0x0012389E
		protected virtual void OnModifiedChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventModifiedChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripTextBox.MultilineChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060044A1 RID: 17569 RVA: 0x001256AC File Offset: 0x001238AC
		protected virtual void OnMultilineChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventMultilineChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripTextBox.ReadOnlyChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060044A2 RID: 17570 RVA: 0x001256BA File Offset: 0x001238BA
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripTextBox.EventReadOnlyChanged, e);
		}

		/// <summary>Subscribes events from the hosted control.</summary>
		/// <param name="control">The control from which to subscribe events.</param>
		// Token: 0x060044A3 RID: 17571 RVA: 0x001256C8 File Offset: 0x001238C8
		protected override void OnSubscribeControlEvents(Control control)
		{
			TextBox textBox = control as TextBox;
			if (textBox != null)
			{
				textBox.AcceptsTabChanged += this.HandleAcceptsTabChanged;
				textBox.BorderStyleChanged += this.HandleBorderStyleChanged;
				textBox.HideSelectionChanged += this.HandleHideSelectionChanged;
				textBox.ModifiedChanged += this.HandleModifiedChanged;
				textBox.MultilineChanged += this.HandleMultilineChanged;
				textBox.ReadOnlyChanged += this.HandleReadOnlyChanged;
				textBox.TextAlignChanged += this.HandleTextBoxTextAlignChanged;
			}
			base.OnSubscribeControlEvents(control);
		}

		/// <summary>Unsubscribes events from the hosted control.</summary>
		/// <param name="control">The control from which to unsubscribe events.</param>
		// Token: 0x060044A4 RID: 17572 RVA: 0x00125764 File Offset: 0x00123964
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			TextBox textBox = control as TextBox;
			if (textBox != null)
			{
				textBox.AcceptsTabChanged -= this.HandleAcceptsTabChanged;
				textBox.BorderStyleChanged -= this.HandleBorderStyleChanged;
				textBox.HideSelectionChanged -= this.HandleHideSelectionChanged;
				textBox.ModifiedChanged -= this.HandleModifiedChanged;
				textBox.MultilineChanged -= this.HandleMultilineChanged;
				textBox.ReadOnlyChanged -= this.HandleReadOnlyChanged;
				textBox.TextAlignChanged -= this.HandleTextBoxTextAlignChanged;
			}
			base.OnUnsubscribeControlEvents(control);
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x00125800 File Offset: 0x00123A00
		internal override bool ShouldSerializeFont()
		{
			return this.Font != ToolStripManager.DefaultFont;
		}

		/// <summary>Gets or sets a value indicating whether pressing the TAB key in a multiline text box control types a TAB character in the control instead of moving the focus to the next control in the tab order.</summary>
		/// <returns>
		///     <see langword="true" /> if users can enter tabs in a multiline text box using the TAB key; <see langword="false" /> if pressing the TAB key moves the focus. The default is <see langword="false" />.</returns>
		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x060044A6 RID: 17574 RVA: 0x00125812 File Offset: 0x00123A12
		// (set) Token: 0x060044A7 RID: 17575 RVA: 0x0012581F File Offset: 0x00123A1F
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TextBoxAcceptsTabDescr")]
		public bool AcceptsTab
		{
			get
			{
				return this.TextBox.AcceptsTab;
			}
			set
			{
				this.TextBox.AcceptsTab = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether pressing ENTER in a multiline <see cref="T:System.Windows.Forms.TextBox" /> control creates a new line of text in the control or activates the default button for the form.</summary>
		/// <returns>
		///     <see langword="true" /> if the ENTER key creates a new line of text in a multiline version of the control; <see langword="false" /> if the ENTER key activates the default button for the form. The default is <see langword="false" />.</returns>
		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x0012582D File Offset: 0x00123A2D
		// (set) Token: 0x060044A9 RID: 17577 RVA: 0x0012583A File Offset: 0x00123A3A
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TextBoxAcceptsReturnDescr")]
		public bool AcceptsReturn
		{
			get
			{
				return this.TextBox.AcceptsReturn;
			}
			set
			{
				this.TextBox.AcceptsReturn = value;
			}
		}

		/// <summary>Gets or sets a custom string collection to use when the <see cref="P:System.Windows.Forms.ToolStripTextBox.AutoCompleteSource" /> property is set to <see langword="CustomSource" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" /> to use with <see cref="P:System.Windows.Forms.TextBox.AutoCompleteSource" />.</returns>
		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x060044AA RID: 17578 RVA: 0x00125848 File Offset: 0x00123A48
		// (set) Token: 0x060044AB RID: 17579 RVA: 0x00125855 File Offset: 0x00123A55
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("TextBoxAutoCompleteCustomSourceDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				return this.TextBox.AutoCompleteCustomSource;
			}
			set
			{
				this.TextBox.AutoCompleteCustomSource = value;
			}
		}

		/// <summary>Gets or sets an option that controls how automatic completion works for the <see cref="T:System.Windows.Forms.ToolStripTextBox" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoCompleteMode" /> values. The default is <see cref="F:System.Windows.Forms.AutoCompleteMode.None" />.</returns>
		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x060044AC RID: 17580 RVA: 0x00125863 File Offset: 0x00123A63
		// (set) Token: 0x060044AD RID: 17581 RVA: 0x00125870 File Offset: 0x00123A70
		[DefaultValue(AutoCompleteMode.None)]
		[SRDescription("TextBoxAutoCompleteModeDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return this.TextBox.AutoCompleteMode;
			}
			set
			{
				this.TextBox.AutoCompleteMode = value;
			}
		}

		/// <summary>Gets or sets a value specifying the source of complete strings used for automatic completion.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoCompleteSource" /> values. The default is <see cref="F:System.Windows.Forms.AutoCompleteSource.None" />.</returns>
		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x060044AE RID: 17582 RVA: 0x0012587E File Offset: 0x00123A7E
		// (set) Token: 0x060044AF RID: 17583 RVA: 0x0012588B File Offset: 0x00123A8B
		[DefaultValue(AutoCompleteSource.None)]
		[SRDescription("TextBoxAutoCompleteSourceDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return this.TextBox.AutoCompleteSource;
			}
			set
			{
				this.TextBox.AutoCompleteSource = value;
			}
		}

		/// <summary>Gets or sets the border type of the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see cref="F:System.Windows.Forms.BorderStyle.Fixed3D" />.</returns>
		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x060044B0 RID: 17584 RVA: 0x00125899 File Offset: 0x00123A99
		// (set) Token: 0x060044B1 RID: 17585 RVA: 0x001258A6 File Offset: 0x00123AA6
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("TextBoxBorderDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.TextBox.BorderStyle;
			}
			set
			{
				this.TextBox.BorderStyle = value;
			}
		}

		/// <summary>Gets a value indicating whether the user can undo the previous operation in a <see cref="T:System.Windows.Forms.ToolStripTextBox" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can undo the previous operation performed in a text box control; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x060044B2 RID: 17586 RVA: 0x001258B4 File Offset: 0x00123AB4
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxCanUndoDescr")]
		public bool CanUndo
		{
			get
			{
				return this.TextBox.CanUndo;
			}
		}

		/// <summary>Gets or sets whether the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> control modifies the case of characters as they are typed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CharacterCasing" /> values. The default is <see cref="F:System.Windows.Forms.CharacterCasing.Normal" />.</returns>
		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x060044B3 RID: 17587 RVA: 0x001258C1 File Offset: 0x00123AC1
		// (set) Token: 0x060044B4 RID: 17588 RVA: 0x001258CE File Offset: 0x00123ACE
		[SRCategory("CatBehavior")]
		[DefaultValue(CharacterCasing.Normal)]
		[SRDescription("TextBoxCharacterCasingDescr")]
		public CharacterCasing CharacterCasing
		{
			get
			{
				return this.TextBox.CharacterCasing;
			}
			set
			{
				this.TextBox.CharacterCasing = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the selected text does not appear highlighted when the text box control loses focus; <see langword="false" />, if the selected text remains highlighted when the text box control loses focus. The default is <see langword="true" />.</returns>
		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x060044B5 RID: 17589 RVA: 0x001258DC File Offset: 0x00123ADC
		// (set) Token: 0x060044B6 RID: 17590 RVA: 0x001258E9 File Offset: 0x00123AE9
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TextBoxHideSelectionDescr")]
		public bool HideSelection
		{
			get
			{
				return this.TextBox.HideSelection;
			}
			set
			{
				this.TextBox.HideSelection = value;
			}
		}

		/// <summary>Gets or sets the lines of text in a <see cref="T:System.Windows.Forms.ToolStripTextBox" /> control.</summary>
		/// <returns>An array of strings that contains the text in a text box control.</returns>
		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x060044B7 RID: 17591 RVA: 0x001258F7 File Offset: 0x00123AF7
		// (set) Token: 0x060044B8 RID: 17592 RVA: 0x00125904 File Offset: 0x00123B04
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizable(true)]
		[SRDescription("TextBoxLinesDescr")]
		[Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string[] Lines
		{
			get
			{
				return this.TextBox.Lines;
			}
			set
			{
				this.TextBox.Lines = value;
			}
		}

		/// <summary>Gets or sets the maximum number of characters the user can type or paste into the text box control.</summary>
		/// <returns>The number of characters that can be entered into the control. The default is 32767 characters.</returns>
		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x060044B9 RID: 17593 RVA: 0x00125912 File Offset: 0x00123B12
		// (set) Token: 0x060044BA RID: 17594 RVA: 0x0012591F File Offset: 0x00123B1F
		[SRCategory("CatBehavior")]
		[DefaultValue(32767)]
		[Localizable(true)]
		[SRDescription("TextBoxMaxLengthDescr")]
		public int MaxLength
		{
			get
			{
				return this.TextBox.MaxLength;
			}
			set
			{
				this.TextBox.MaxLength = value;
			}
		}

		/// <summary>Gets or sets a value that indicates that the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> control has been modified by the user since the control was created or its contents were last set.</summary>
		/// <returns>
		///     <see langword="true" /> if the control's contents have been modified; otherwise, <see langword="false" />. </returns>
		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x060044BB RID: 17595 RVA: 0x0012592D File Offset: 0x00123B2D
		// (set) Token: 0x060044BC RID: 17596 RVA: 0x0012593A File Offset: 0x00123B3A
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxModifiedDescr")]
		public bool Modified
		{
			get
			{
				return this.TextBox.Modified;
			}
			set
			{
				this.TextBox.Modified = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x060044BD RID: 17597 RVA: 0x00125948 File Offset: 0x00123B48
		// (set) Token: 0x060044BE RID: 17598 RVA: 0x00125955 File Offset: 0x00123B55
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("TextBoxMultilineDescr")]
		[RefreshProperties(RefreshProperties.All)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool Multiline
		{
			get
			{
				return this.TextBox.Multiline;
			}
			set
			{
				this.TextBox.Multiline = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether text in the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> is read-only; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x060044BF RID: 17599 RVA: 0x00125963 File Offset: 0x00123B63
		// (set) Token: 0x060044C0 RID: 17600 RVA: 0x00125970 File Offset: 0x00123B70
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TextBoxReadOnlyDescr")]
		public bool ReadOnly
		{
			get
			{
				return this.TextBox.ReadOnly;
			}
			set
			{
				this.TextBox.ReadOnly = value;
			}
		}

		/// <summary>Gets or sets a value indicating the currently selected text in the control.</summary>
		/// <returns>A string that represents the currently selected text in the text box.</returns>
		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060044C1 RID: 17601 RVA: 0x0012597E File Offset: 0x00123B7E
		// (set) Token: 0x060044C2 RID: 17602 RVA: 0x0012598B File Offset: 0x00123B8B
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectedTextDescr")]
		public string SelectedText
		{
			get
			{
				return this.TextBox.SelectedText;
			}
			set
			{
				this.TextBox.SelectedText = value;
			}
		}

		/// <summary>Gets or sets the number of characters selected in the<see cref="T:System.Windows.Forms.ToolStripTextBox" />.</summary>
		/// <returns>The number of characters selected in the<see cref="T:System.Windows.Forms.ToolStripTextBox" />.</returns>
		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x060044C3 RID: 17603 RVA: 0x00125999 File Offset: 0x00123B99
		// (set) Token: 0x060044C4 RID: 17604 RVA: 0x001259A6 File Offset: 0x00123BA6
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectionLengthDescr")]
		public int SelectionLength
		{
			get
			{
				return this.TextBox.SelectionLength;
			}
			set
			{
				this.TextBox.SelectionLength = value;
			}
		}

		/// <summary>Gets or sets the starting point of text selected in the<see cref="T:System.Windows.Forms.ToolStripTextBox" />.</summary>
		/// <returns>The starting position of text selected in the<see cref="T:System.Windows.Forms.ToolStripTextBox" />.</returns>
		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x060044C5 RID: 17605 RVA: 0x001259B4 File Offset: 0x00123BB4
		// (set) Token: 0x060044C6 RID: 17606 RVA: 0x001259C1 File Offset: 0x00123BC1
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectionStartDescr")]
		public int SelectionStart
		{
			get
			{
				return this.TextBox.SelectionStart;
			}
			set
			{
				this.TextBox.SelectionStart = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the defined shortcuts are enabled.</summary>
		/// <returns>
		///     <see langword="true" /> to enable the shortcuts; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x060044C7 RID: 17607 RVA: 0x001259CF File Offset: 0x00123BCF
		// (set) Token: 0x060044C8 RID: 17608 RVA: 0x001259DC File Offset: 0x00123BDC
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TextBoxShortcutsEnabledDescr")]
		public bool ShortcutsEnabled
		{
			get
			{
				return this.TextBox.ShortcutsEnabled;
			}
			set
			{
				this.TextBox.ShortcutsEnabled = value;
			}
		}

		/// <summary>Gets the length of text in the control.</summary>
		/// <returns>The number of characters contained in the text of the <see cref="T:System.Windows.Forms.ToolStripTextBox" />.</returns>
		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x060044C9 RID: 17609 RVA: 0x001259EA File Offset: 0x00123BEA
		[Browsable(false)]
		public int TextLength
		{
			get
			{
				return this.TextBox.TextLength;
			}
		}

		/// <summary>Gets or sets how text is aligned in a <see cref="T:System.Windows.Forms.TextBox" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> enumeration values that specifies how text is aligned in the control. The default is <see cref="F:System.Windows.Forms.HorizontalAlignment.Left" />.</returns>
		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x060044CA RID: 17610 RVA: 0x001259F7 File Offset: 0x00123BF7
		// (set) Token: 0x060044CB RID: 17611 RVA: 0x00125A04 File Offset: 0x00123C04
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(HorizontalAlignment.Left)]
		[SRDescription("TextBoxTextAlignDescr")]
		public HorizontalAlignment TextBoxTextAlign
		{
			get
			{
				return this.TextBox.TextAlign;
			}
			set
			{
				this.TextBox.TextAlign = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x060044CC RID: 17612 RVA: 0x00125A12 File Offset: 0x00123C12
		// (set) Token: 0x060044CD RID: 17613 RVA: 0x00125A1F File Offset: 0x00123C1F
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(true)]
		[SRDescription("TextBoxWordWrapDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool WordWrap
		{
			get
			{
				return this.TextBox.WordWrap;
			}
			set
			{
				this.TextBox.WordWrap = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripTextBox.AcceptsTab" /> property changes.</summary>
		// Token: 0x1400037E RID: 894
		// (add) Token: 0x060044CE RID: 17614 RVA: 0x00125A2D File Offset: 0x00123C2D
		// (remove) Token: 0x060044CF RID: 17615 RVA: 0x00125A40 File Offset: 0x00123C40
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnAcceptsTabChangedDescr")]
		public event EventHandler AcceptsTabChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventAcceptsTabChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventAcceptsTabChanged, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripTextBox.BorderStyle" /> property changes.</summary>
		// Token: 0x1400037F RID: 895
		// (add) Token: 0x060044D0 RID: 17616 RVA: 0x00125A53 File Offset: 0x00123C53
		// (remove) Token: 0x060044D1 RID: 17617 RVA: 0x00125A66 File Offset: 0x00123C66
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnBorderStyleChangedDescr")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventBorderStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventBorderStyleChanged, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripTextBox.HideSelection" /> property changes.</summary>
		// Token: 0x14000380 RID: 896
		// (add) Token: 0x060044D2 RID: 17618 RVA: 0x00125A79 File Offset: 0x00123C79
		// (remove) Token: 0x060044D3 RID: 17619 RVA: 0x00125A8C File Offset: 0x00123C8C
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnHideSelectionChangedDescr")]
		public event EventHandler HideSelectionChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventHideSelectionChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventHideSelectionChanged, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripTextBox.Modified" /> property changes.</summary>
		// Token: 0x14000381 RID: 897
		// (add) Token: 0x060044D4 RID: 17620 RVA: 0x00125A9F File Offset: 0x00123C9F
		// (remove) Token: 0x060044D5 RID: 17621 RVA: 0x00125AB2 File Offset: 0x00123CB2
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnModifiedChangedDescr")]
		public event EventHandler ModifiedChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventModifiedChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventModifiedChanged, value);
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000382 RID: 898
		// (add) Token: 0x060044D6 RID: 17622 RVA: 0x00125AC5 File Offset: 0x00123CC5
		// (remove) Token: 0x060044D7 RID: 17623 RVA: 0x00125AD8 File Offset: 0x00123CD8
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnMultilineChangedDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler MultilineChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventMultilineChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventMultilineChanged, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripTextBox.ReadOnly" /> property changes.</summary>
		// Token: 0x14000383 RID: 899
		// (add) Token: 0x060044D8 RID: 17624 RVA: 0x00125AEB File Offset: 0x00123CEB
		// (remove) Token: 0x060044D9 RID: 17625 RVA: 0x00125AFE File Offset: 0x00123CFE
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnReadOnlyChangedDescr")]
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventReadOnlyChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventReadOnlyChanged, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripTextBox.TextBoxTextAlign" /> property changes.</summary>
		// Token: 0x14000384 RID: 900
		// (add) Token: 0x060044DA RID: 17626 RVA: 0x00125B11 File Offset: 0x00123D11
		// (remove) Token: 0x060044DB RID: 17627 RVA: 0x00125B24 File Offset: 0x00123D24
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ToolStripTextBoxTextBoxTextAlignChangedDescr")]
		public event EventHandler TextBoxTextAlignChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripTextBox.EventTextBoxTextAlignChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripTextBox.EventTextBoxTextAlignChanged, value);
			}
		}

		/// <summary>Appends text to the current text of the <see cref="T:System.Windows.Forms.ToolStripTextBox" />.</summary>
		/// <param name="text">The text to append to the current contents of the <see cref="T:System.Windows.Forms.ToolStripTextBox" />.</param>
		// Token: 0x060044DC RID: 17628 RVA: 0x00125B37 File Offset: 0x00123D37
		public void AppendText(string text)
		{
			this.TextBox.AppendText(text);
		}

		/// <summary>Clears all text from the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> control.</summary>
		// Token: 0x060044DD RID: 17629 RVA: 0x00125B45 File Offset: 0x00123D45
		public void Clear()
		{
			this.TextBox.Clear();
		}

		/// <summary>Clears information about the most recent operation from the undo buffer of the <see cref="T:System.Windows.Forms.ToolStripTextBox" />.</summary>
		// Token: 0x060044DE RID: 17630 RVA: 0x00125B52 File Offset: 0x00123D52
		public void ClearUndo()
		{
			this.TextBox.ClearUndo();
		}

		/// <summary>Copies the current selection in the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> to the Clipboard.</summary>
		// Token: 0x060044DF RID: 17631 RVA: 0x00125B5F File Offset: 0x00123D5F
		public void Copy()
		{
			this.TextBox.Copy();
		}

		/// <summary>Moves the current selection in the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> to the Clipboard.</summary>
		// Token: 0x060044E0 RID: 17632 RVA: 0x00125B5F File Offset: 0x00123D5F
		public void Cut()
		{
			this.TextBox.Copy();
		}

		/// <summary>Specifies that the value of the <see cref="P:System.Windows.Forms.ToolStripTextBox.SelectionLength" /> property is zero so that no characters are selected in the control.</summary>
		// Token: 0x060044E1 RID: 17633 RVA: 0x00125B6C File Offset: 0x00123D6C
		public void DeselectAll()
		{
			this.TextBox.DeselectAll();
		}

		/// <summary>Retrieves the character that is closest to the specified location within the control.</summary>
		/// <param name="pt">The location from which to seek the nearest character.</param>
		/// <returns>The character at the specified location.</returns>
		// Token: 0x060044E2 RID: 17634 RVA: 0x00125B79 File Offset: 0x00123D79
		public char GetCharFromPosition(Point pt)
		{
			return this.TextBox.GetCharFromPosition(pt);
		}

		/// <summary>Retrieves the index of the character nearest to the specified location.</summary>
		/// <param name="pt">The location to search.</param>
		/// <returns>The zero-based character index at the specified location.</returns>
		// Token: 0x060044E3 RID: 17635 RVA: 0x00125B87 File Offset: 0x00123D87
		public int GetCharIndexFromPosition(Point pt)
		{
			return this.TextBox.GetCharIndexFromPosition(pt);
		}

		/// <summary>Retrieves the index of the first character of a given line.</summary>
		/// <param name="lineNumber">The line for which to get the index of its first character.</param>
		/// <returns>The zero-based character index in the specified line.</returns>
		// Token: 0x060044E4 RID: 17636 RVA: 0x00125B95 File Offset: 0x00123D95
		public int GetFirstCharIndexFromLine(int lineNumber)
		{
			return this.TextBox.GetFirstCharIndexFromLine(lineNumber);
		}

		/// <summary>Retrieves the index of the first character of the current line.</summary>
		/// <returns>The zero-based character index in the current line.</returns>
		// Token: 0x060044E5 RID: 17637 RVA: 0x00125BA3 File Offset: 0x00123DA3
		public int GetFirstCharIndexOfCurrentLine()
		{
			return this.TextBox.GetFirstCharIndexOfCurrentLine();
		}

		/// <summary>Retrieves the line number from the specified character position within the text of the control.</summary>
		/// <param name="index">The character index position to search.</param>
		/// <returns>The zero-based line number in which the character index is located.</returns>
		// Token: 0x060044E6 RID: 17638 RVA: 0x00125BB0 File Offset: 0x00123DB0
		public int GetLineFromCharIndex(int index)
		{
			return this.TextBox.GetLineFromCharIndex(index);
		}

		/// <summary>Retrieves the location within the control at the specified character index.</summary>
		/// <param name="index">The index of the character for which to retrieve the location.</param>
		/// <returns>The location of the specified character.</returns>
		// Token: 0x060044E7 RID: 17639 RVA: 0x00125BBE File Offset: 0x00123DBE
		public Point GetPositionFromCharIndex(int index)
		{
			return this.TextBox.GetPositionFromCharIndex(index);
		}

		/// <summary>Replaces the current selection in the text box with the contents of the Clipboard.</summary>
		// Token: 0x060044E8 RID: 17640 RVA: 0x00125BCC File Offset: 0x00123DCC
		public void Paste()
		{
			this.TextBox.Paste();
		}

		/// <summary>Scrolls the contents of the control to the current caret position.</summary>
		// Token: 0x060044E9 RID: 17641 RVA: 0x00125BD9 File Offset: 0x00123DD9
		public void ScrollToCaret()
		{
			this.TextBox.ScrollToCaret();
		}

		/// <summary>Selects a range of text in the text box.</summary>
		/// <param name="start">The position of the first character in the current text selection within the text box.</param>
		/// <param name="length">The number of characters to select.</param>
		// Token: 0x060044EA RID: 17642 RVA: 0x00125BE6 File Offset: 0x00123DE6
		public void Select(int start, int length)
		{
			this.TextBox.Select(start, length);
		}

		/// <summary>Selects all text in the text box.</summary>
		// Token: 0x060044EB RID: 17643 RVA: 0x00125BF5 File Offset: 0x00123DF5
		public void SelectAll()
		{
			this.TextBox.SelectAll();
		}

		/// <summary>Undoes the last edit operation in the text box.</summary>
		// Token: 0x060044EC RID: 17644 RVA: 0x00125C02 File Offset: 0x00123E02
		public void Undo()
		{
			this.TextBox.Undo();
		}

		// Token: 0x040025DB RID: 9691
		internal static readonly object EventTextBoxTextAlignChanged = new object();

		// Token: 0x040025DC RID: 9692
		internal static readonly object EventAcceptsTabChanged = new object();

		// Token: 0x040025DD RID: 9693
		internal static readonly object EventBorderStyleChanged = new object();

		// Token: 0x040025DE RID: 9694
		internal static readonly object EventHideSelectionChanged = new object();

		// Token: 0x040025DF RID: 9695
		internal static readonly object EventReadOnlyChanged = new object();

		// Token: 0x040025E0 RID: 9696
		internal static readonly object EventMultilineChanged = new object();

		// Token: 0x040025E1 RID: 9697
		internal static readonly object EventModifiedChanged = new object();

		// Token: 0x040025E2 RID: 9698
		private static readonly Padding defaultMargin = new Padding(1, 0, 1, 0);

		// Token: 0x040025E3 RID: 9699
		private static readonly Padding defaultDropDownMargin = new Padding(1);

		// Token: 0x040025E4 RID: 9700
		private Padding scaledDefaultMargin = ToolStripTextBox.defaultMargin;

		// Token: 0x040025E5 RID: 9701
		private Padding scaledDefaultDropDownMargin = ToolStripTextBox.defaultDropDownMargin;

		// Token: 0x02000757 RID: 1879
		[ComVisible(true)]
		internal class ToolStripTextBoxAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x0600621B RID: 25115 RVA: 0x001917C0 File Offset: 0x0018F9C0
			public ToolStripTextBoxAccessibleObject(ToolStripTextBox ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x1700176E RID: 5998
			// (get) Token: 0x0600621C RID: 25116 RVA: 0x001917D0 File Offset: 0x0018F9D0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Text;
				}
			}

			// Token: 0x0600621D RID: 25117 RVA: 0x001917F1 File Offset: 0x0018F9F1
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild || direction == UnsafeNativeMethods.NavigateDirection.LastChild)
				{
					return this.ownerItem.TextBox.AccessibilityObject;
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x040041B6 RID: 16822
			private ToolStripTextBox ownerItem;
		}

		// Token: 0x02000758 RID: 1880
		private class ToolStripTextBoxControl : TextBox
		{
			// Token: 0x0600621E RID: 25118 RVA: 0x00191813 File Offset: 0x0018FA13
			public ToolStripTextBoxControl()
			{
				this.Font = ToolStripManager.DefaultFont;
				this.isFontSet = false;
			}

			// Token: 0x1700176F RID: 5999
			// (get) Token: 0x0600621F RID: 25119 RVA: 0x00191834 File Offset: 0x0018FA34
			private NativeMethods.RECT AbsoluteClientRECT
			{
				get
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					CreateParams createParams = this.CreateParams;
					base.AdjustWindowRectEx(ref rect, createParams.Style, this.HasMenu, createParams.ExStyle);
					int num = -rect.left;
					int num2 = -rect.top;
					UnsafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref rect);
					rect.left += num;
					rect.right += num;
					rect.top += num2;
					rect.bottom += num2;
					return rect;
				}
			}

			// Token: 0x17001770 RID: 6000
			// (get) Token: 0x06006220 RID: 25120 RVA: 0x001918C0 File Offset: 0x0018FAC0
			private Rectangle AbsoluteClientRectangle
			{
				get
				{
					NativeMethods.RECT absoluteClientRECT = this.AbsoluteClientRECT;
					return Rectangle.FromLTRB(absoluteClientRECT.top, absoluteClientRECT.top, absoluteClientRECT.right, absoluteClientRECT.bottom);
				}
			}

			// Token: 0x17001771 RID: 6001
			// (get) Token: 0x06006221 RID: 25121 RVA: 0x001918F4 File Offset: 0x0018FAF4
			private ProfessionalColorTable ColorTable
			{
				get
				{
					if (this.Owner != null)
					{
						ToolStripProfessionalRenderer toolStripProfessionalRenderer = this.Owner.Renderer as ToolStripProfessionalRenderer;
						if (toolStripProfessionalRenderer != null)
						{
							return toolStripProfessionalRenderer.ColorTable;
						}
					}
					return ProfessionalColors.ColorTable;
				}
			}

			// Token: 0x17001772 RID: 6002
			// (get) Token: 0x06006222 RID: 25122 RVA: 0x00191929 File Offset: 0x0018FB29
			private bool IsPopupTextBox
			{
				get
				{
					return base.BorderStyle == BorderStyle.Fixed3D && this.Owner != null && this.Owner.Renderer is ToolStripProfessionalRenderer;
				}
			}

			// Token: 0x17001773 RID: 6003
			// (get) Token: 0x06006223 RID: 25123 RVA: 0x00191953 File Offset: 0x0018FB53
			// (set) Token: 0x06006224 RID: 25124 RVA: 0x0019195B File Offset: 0x0018FB5B
			internal bool MouseIsOver
			{
				get
				{
					return this.mouseIsOver;
				}
				set
				{
					if (this.mouseIsOver != value)
					{
						this.mouseIsOver = value;
						if (!this.Focused)
						{
							this.InvalidateNonClient();
						}
					}
				}
			}

			// Token: 0x17001774 RID: 6004
			// (get) Token: 0x06006225 RID: 25125 RVA: 0x00012071 File Offset: 0x00010271
			// (set) Token: 0x06006226 RID: 25126 RVA: 0x0019197B File Offset: 0x0018FB7B
			public override Font Font
			{
				get
				{
					return base.Font;
				}
				set
				{
					base.Font = value;
					this.isFontSet = this.ShouldSerializeFont();
				}
			}

			// Token: 0x17001775 RID: 6005
			// (get) Token: 0x06006227 RID: 25127 RVA: 0x00191990 File Offset: 0x0018FB90
			// (set) Token: 0x06006228 RID: 25128 RVA: 0x00191998 File Offset: 0x0018FB98
			public ToolStripTextBox Owner
			{
				get
				{
					return this.ownerItem;
				}
				set
				{
					this.ownerItem = value;
				}
			}

			// Token: 0x17001776 RID: 6006
			// (get) Token: 0x06006229 RID: 25129 RVA: 0x000A010F File Offset: 0x0009E30F
			internal override bool SupportsUiaProviders
			{
				get
				{
					return AccessibilityImprovements.Level3;
				}
			}

			// Token: 0x0600622A RID: 25130 RVA: 0x001919A4 File Offset: 0x0018FBA4
			private void InvalidateNonClient()
			{
				if (!this.IsPopupTextBox)
				{
					return;
				}
				NativeMethods.RECT absoluteClientRECT = this.AbsoluteClientRECT;
				HandleRef handleRef = NativeMethods.NullHandleRef;
				HandleRef handleRef2 = NativeMethods.NullHandleRef;
				HandleRef handleRef3 = NativeMethods.NullHandleRef;
				try
				{
					handleRef3 = new HandleRef(this, SafeNativeMethods.CreateRectRgn(0, 0, base.Width, base.Height));
					handleRef2 = new HandleRef(this, SafeNativeMethods.CreateRectRgn(absoluteClientRECT.left, absoluteClientRECT.top, absoluteClientRECT.right, absoluteClientRECT.bottom));
					handleRef = new HandleRef(this, SafeNativeMethods.CreateRectRgn(0, 0, 0, 0));
					SafeNativeMethods.CombineRgn(handleRef, handleRef3, handleRef2, 3);
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), ref rect, handleRef, 1797);
				}
				finally
				{
					try
					{
						if (handleRef.Handle != IntPtr.Zero)
						{
							SafeNativeMethods.DeleteObject(handleRef);
						}
					}
					finally
					{
						try
						{
							if (handleRef2.Handle != IntPtr.Zero)
							{
								SafeNativeMethods.DeleteObject(handleRef2);
							}
						}
						finally
						{
							if (handleRef3.Handle != IntPtr.Zero)
							{
								SafeNativeMethods.DeleteObject(handleRef3);
							}
						}
					}
				}
			}

			// Token: 0x0600622B RID: 25131 RVA: 0x00191AD0 File Offset: 0x0018FCD0
			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.InvalidateNonClient();
			}

			// Token: 0x0600622C RID: 25132 RVA: 0x00191ADF File Offset: 0x0018FCDF
			protected override void OnLostFocus(EventArgs e)
			{
				base.OnLostFocus(e);
				this.InvalidateNonClient();
			}

			// Token: 0x0600622D RID: 25133 RVA: 0x00191AEE File Offset: 0x0018FCEE
			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter(e);
				this.MouseIsOver = true;
			}

			// Token: 0x0600622E RID: 25134 RVA: 0x00191AFE File Offset: 0x0018FCFE
			protected override void OnMouseLeave(EventArgs e)
			{
				base.OnMouseLeave(e);
				this.MouseIsOver = false;
			}

			// Token: 0x0600622F RID: 25135 RVA: 0x00191B10 File Offset: 0x0018FD10
			private void HookStaticEvents(bool hook)
			{
				if (hook)
				{
					if (this.alreadyHooked)
					{
						return;
					}
					try
					{
						SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
						return;
					}
					finally
					{
						this.alreadyHooked = true;
					}
				}
				if (this.alreadyHooked)
				{
					try
					{
						SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
					}
					finally
					{
						this.alreadyHooked = false;
					}
				}
			}

			// Token: 0x06006230 RID: 25136 RVA: 0x00191B84 File Offset: 0x0018FD84
			private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
			{
				if (e.Category == UserPreferenceCategory.Window && !this.isFontSet)
				{
					this.Font = ToolStripManager.DefaultFont;
				}
			}

			// Token: 0x06006231 RID: 25137 RVA: 0x00191BA3 File Offset: 0x0018FDA3
			protected override void OnVisibleChanged(EventArgs e)
			{
				base.OnVisibleChanged(e);
				if (!base.Disposing && !base.IsDisposed)
				{
					this.HookStaticEvents(base.Visible);
				}
			}

			// Token: 0x06006232 RID: 25138 RVA: 0x00191BC8 File Offset: 0x0018FDC8
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				if (AccessibilityImprovements.Level3)
				{
					return new ToolStripTextBox.ToolStripTextBoxControlAccessibleObject(this);
				}
				return base.CreateAccessibilityInstance();
			}

			// Token: 0x06006233 RID: 25139 RVA: 0x00191BDE File Offset: 0x0018FDDE
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					this.HookStaticEvents(false);
				}
				base.Dispose(disposing);
			}

			// Token: 0x06006234 RID: 25140 RVA: 0x00191BF4 File Offset: 0x0018FDF4
			private void WmNCPaint(ref Message m)
			{
				if (!this.IsPopupTextBox)
				{
					base.WndProc(ref m);
					return;
				}
				HandleRef hDC = new HandleRef(this, UnsafeNativeMethods.GetWindowDC(new HandleRef(this, m.HWnd)));
				if (hDC.Handle == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				try
				{
					Color color = (this.MouseIsOver || this.Focused) ? this.ColorTable.TextBoxBorder : this.BackColor;
					Color color2 = this.BackColor;
					if (!base.Enabled)
					{
						color = SystemColors.ControlDark;
						color2 = SystemColors.Control;
					}
					using (Graphics graphics = Graphics.FromHdcInternal(hDC.Handle))
					{
						Rectangle absoluteClientRectangle = this.AbsoluteClientRectangle;
						using (Brush brush = new SolidBrush(color2))
						{
							graphics.FillRectangle(brush, 0, 0, base.Width, absoluteClientRectangle.Top);
							graphics.FillRectangle(brush, 0, 0, absoluteClientRectangle.Left, base.Height);
							graphics.FillRectangle(brush, 0, absoluteClientRectangle.Bottom, base.Width, base.Height - absoluteClientRectangle.Height);
							graphics.FillRectangle(brush, absoluteClientRectangle.Right, 0, base.Width - absoluteClientRectangle.Right, base.Height);
						}
						using (Pen pen = new Pen(color))
						{
							graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
						}
					}
				}
				finally
				{
					UnsafeNativeMethods.ReleaseDC(new HandleRef(this, base.Handle), hDC);
				}
				m.Result = IntPtr.Zero;
			}

			// Token: 0x06006235 RID: 25141 RVA: 0x00191DE4 File Offset: 0x0018FFE4
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 133)
				{
					this.WmNCPaint(ref m);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x040041B7 RID: 16823
			private bool mouseIsOver;

			// Token: 0x040041B8 RID: 16824
			private ToolStripTextBox ownerItem;

			// Token: 0x040041B9 RID: 16825
			private bool isFontSet = true;

			// Token: 0x040041BA RID: 16826
			private bool alreadyHooked;
		}

		// Token: 0x02000759 RID: 1881
		private class ToolStripTextBoxControlAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06006236 RID: 25142 RVA: 0x00093572 File Offset: 0x00091772
			public ToolStripTextBoxControlAccessibleObject(ToolStripTextBox.ToolStripTextBoxControl toolStripTextBoxControl) : base(toolStripTextBoxControl)
			{
			}

			// Token: 0x17001777 RID: 6007
			// (get) Token: 0x06006237 RID: 25143 RVA: 0x00191E04 File Offset: 0x00190004
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					ToolStripTextBox.ToolStripTextBoxControl toolStripTextBoxControl = base.Owner as ToolStripTextBox.ToolStripTextBoxControl;
					if (toolStripTextBoxControl != null)
					{
						return toolStripTextBoxControl.Owner.Owner.AccessibilityObject;
					}
					return base.FragmentRoot;
				}
			}

			// Token: 0x06006238 RID: 25144 RVA: 0x00191E38 File Offset: 0x00190038
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction <= UnsafeNativeMethods.NavigateDirection.PreviousSibling)
				{
					ToolStripTextBox.ToolStripTextBoxControl toolStripTextBoxControl = base.Owner as ToolStripTextBox.ToolStripTextBoxControl;
					if (toolStripTextBoxControl != null)
					{
						return toolStripTextBoxControl.Owner.AccessibilityObject.FragmentNavigate(direction);
					}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x06006239 RID: 25145 RVA: 0x00191E74 File Offset: 0x00190074
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50004;
				}
				if (propertyID == 30005)
				{
					return this.Name;
				}
				if (propertyID != 30008)
				{
					return base.GetPropertyValue(propertyID);
				}
				return (this.State & AccessibleStates.Focused) == AccessibleStates.Focused;
			}

			// Token: 0x0600623A RID: 25146 RVA: 0x00191EC5 File Offset: 0x001900C5
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId == 10002 || patternId == 10018 || base.IsPatternSupported(patternId);
			}
		}
	}
}
