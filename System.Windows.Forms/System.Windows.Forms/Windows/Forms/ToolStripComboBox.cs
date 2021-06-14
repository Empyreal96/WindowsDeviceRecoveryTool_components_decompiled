using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	/// <summary>Represents a <see cref="T:System.Windows.Forms.ToolStripComboBox" /> that is properly rendered in a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
	// Token: 0x020003A6 RID: 934
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	[DefaultProperty("Items")]
	public class ToolStripComboBox : ToolStripControlHost
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> class.</summary>
		// Token: 0x06003C2D RID: 15405 RVA: 0x0010A744 File Offset: 0x00108944
		public ToolStripComboBox() : base(ToolStripComboBox.CreateControlInstance())
		{
			ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl = base.Control as ToolStripComboBox.ToolStripComboBoxControl;
			toolStripComboBoxControl.Owner = this;
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.scaledPadding = DpiHelper.LogicalToDeviceUnits(ToolStripComboBox.padding, 0);
				this.scaledDropDownPadding = DpiHelper.LogicalToDeviceUnits(ToolStripComboBox.dropDownPadding, 0);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> class with the specified name. </summary>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</param>
		// Token: 0x06003C2E RID: 15406 RVA: 0x0010A7AE File Offset: 0x001089AE
		public ToolStripComboBox(string name) : this()
		{
			base.Name = name;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> class derived from a base control.</summary>
		/// <param name="c">The base control. </param>
		/// <exception cref="T:System.NotSupportedException">The operation is not supported. </exception>
		// Token: 0x06003C2F RID: 15407 RVA: 0x0010A7BD File Offset: 0x001089BD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ToolStripComboBox(Control c) : base(c)
		{
			throw new NotSupportedException(SR.GetString("ToolStripMustSupplyItsOwnComboBox"));
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x0010A7EB File Offset: 0x001089EB
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ToolStripComboBox.ToolStripComboBoxAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x0010A804 File Offset: 0x00108A04
		private static Control CreateControlInstance()
		{
			return new ToolStripComboBox.ToolStripComboBoxControl
			{
				FlatStyle = FlatStyle.Popup,
				Font = ToolStripManager.DefaultFont
			};
		}

		/// <summary>Gets or sets the custom string collection to use when the <see cref="P:System.Windows.Forms.ToolStripComboBox.AutoCompleteSource" /> property is set to <see cref="F:System.Windows.Forms.AutoCompleteSource.CustomSource" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" /> that contains the strings.</returns>
		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06003C32 RID: 15410 RVA: 0x0010A82A File Offset: 0x00108A2A
		// (set) Token: 0x06003C33 RID: 15411 RVA: 0x0010A837 File Offset: 0x00108A37
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ComboBoxAutoCompleteCustomSourceDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				return this.ComboBox.AutoCompleteCustomSource;
			}
			set
			{
				this.ComboBox.AutoCompleteCustomSource = value;
			}
		}

		/// <summary>Gets or sets a value that indicates the text completion behavior of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoCompleteMode" /> values. The default is <see cref="F:System.Windows.Forms.AutoCompleteMode.None" />.</returns>
		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06003C34 RID: 15412 RVA: 0x0010A845 File Offset: 0x00108A45
		// (set) Token: 0x06003C35 RID: 15413 RVA: 0x0010A852 File Offset: 0x00108A52
		[DefaultValue(AutoCompleteMode.None)]
		[SRDescription("ComboBoxAutoCompleteModeDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return this.ComboBox.AutoCompleteMode;
			}
			set
			{
				this.ComboBox.AutoCompleteMode = value;
			}
		}

		/// <summary>Gets or sets the source of complete strings used for automatic completion.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoCompleteSource" /> values. The default is <see cref="F:System.Windows.Forms.AutoCompleteSource.None" />.</returns>
		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06003C36 RID: 15414 RVA: 0x0010A860 File Offset: 0x00108A60
		// (set) Token: 0x06003C37 RID: 15415 RVA: 0x0010A86D File Offset: 0x00108A6D
		[DefaultValue(AutoCompleteSource.None)]
		[SRDescription("ComboBoxAutoCompleteSourceDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return this.ComboBox.AutoCompleteSource;
			}
			set
			{
				this.ComboBox.AutoCompleteSource = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The background image displayed in the control.</returns>
		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x0010A87B File Offset: 0x00108A7B
		// (set) Token: 0x06003C39 RID: 15417 RVA: 0x0010A883 File Offset: 0x00108A83
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
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout" />.</returns>
		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06003C3A RID: 15418 RVA: 0x0010A88C File Offset: 0x00108A8C
		// (set) Token: 0x06003C3B RID: 15419 RVA: 0x0010A894 File Offset: 0x00108A94
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

		/// <summary>Gets a <see cref="T:System.Windows.Forms.ComboBox" /> in which the user can enter text, along with a list from which the user can select.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ComboBox" /> for a <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06003C3C RID: 15420 RVA: 0x0010A89D File Offset: 0x00108A9D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComboBox ComboBox
		{
			get
			{
				return base.Control as ComboBox;
			}
		}

		/// <summary>Gets the default size of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the <see cref="T:System.Windows.Forms.ToolStripTextBox" /> in pixels. The default size is 100 x 20 pixels.</returns>
		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x000F782D File Offset: 0x000F5A2D
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 22);
			}
		}

		/// <summary>Gets the default spacing, in pixels, between the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> and an adjacent item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value.</returns>
		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06003C3E RID: 15422 RVA: 0x0010A8AA File Offset: 0x00108AAA
		protected internal override Padding DefaultMargin
		{
			get
			{
				if (base.IsOnDropDown)
				{
					return this.scaledDropDownPadding;
				}
				return this.scaledPadding;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x140002F7 RID: 759
		// (add) Token: 0x06003C3F RID: 15423 RVA: 0x0010A8C1 File Offset: 0x00108AC1
		// (remove) Token: 0x06003C40 RID: 15424 RVA: 0x0010A8CA File Offset: 0x00108ACA
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

		/// <summary>Occurs when the drop-down portion of a <see cref="T:System.Windows.Forms.ToolStripComboBox" /> is shown.</summary>
		// Token: 0x140002F8 RID: 760
		// (add) Token: 0x06003C41 RID: 15425 RVA: 0x0010A8D3 File Offset: 0x00108AD3
		// (remove) Token: 0x06003C42 RID: 15426 RVA: 0x0010A8E6 File Offset: 0x00108AE6
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnDropDownDescr")]
		public event EventHandler DropDown
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventDropDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventDropDown, value);
			}
		}

		/// <summary>Occurs when the drop-down portion of the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> has closed.</summary>
		// Token: 0x140002F9 RID: 761
		// (add) Token: 0x06003C43 RID: 15427 RVA: 0x0010A8F9 File Offset: 0x00108AF9
		// (remove) Token: 0x06003C44 RID: 15428 RVA: 0x0010A90C File Offset: 0x00108B0C
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnDropDownClosedDescr")]
		public event EventHandler DropDownClosed
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventDropDownClosed, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventDropDownClosed, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripComboBox.DropDownStyle" /> property has changed.</summary>
		// Token: 0x140002FA RID: 762
		// (add) Token: 0x06003C45 RID: 15429 RVA: 0x0010A91F File Offset: 0x00108B1F
		// (remove) Token: 0x06003C46 RID: 15430 RVA: 0x0010A932 File Offset: 0x00108B32
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxDropDownStyleChangedDescr")]
		public event EventHandler DropDownStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventDropDownStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventDropDownStyleChanged, value);
			}
		}

		/// <summary>Gets or sets the height, in pixels, of the drop-down portion box of a <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>The height, in pixels, of the drop-down box.</returns>
		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06003C47 RID: 15431 RVA: 0x0010A945 File Offset: 0x00108B45
		// (set) Token: 0x06003C48 RID: 15432 RVA: 0x0010A952 File Offset: 0x00108B52
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxDropDownHeightDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(106)]
		public int DropDownHeight
		{
			get
			{
				return this.ComboBox.DropDownHeight;
			}
			set
			{
				this.ComboBox.DropDownHeight = value;
			}
		}

		/// <summary>Gets or sets a value specifying the style of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ComboBoxStyle" /> values. The default is <see cref="F:System.Windows.Forms.ComboBoxStyle.DropDown" />.</returns>
		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06003C49 RID: 15433 RVA: 0x0010A960 File Offset: 0x00108B60
		// (set) Token: 0x06003C4A RID: 15434 RVA: 0x0010A96D File Offset: 0x00108B6D
		[SRCategory("CatAppearance")]
		[DefaultValue(ComboBoxStyle.DropDown)]
		[SRDescription("ComboBoxStyleDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public ComboBoxStyle DropDownStyle
		{
			get
			{
				return this.ComboBox.DropDownStyle;
			}
			set
			{
				this.ComboBox.DropDownStyle = value;
			}
		}

		/// <summary>Gets or sets the width, in pixels, of the drop-down portion of a <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>The width, in pixels, of the drop-down box.</returns>
		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06003C4B RID: 15435 RVA: 0x0010A97B File Offset: 0x00108B7B
		// (set) Token: 0x06003C4C RID: 15436 RVA: 0x0010A988 File Offset: 0x00108B88
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxDropDownWidthDescr")]
		public int DropDownWidth
		{
			get
			{
				return this.ComboBox.DropDownWidth;
			}
			set
			{
				this.ComboBox.DropDownWidth = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> currently displays its drop-down portion.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> currently displays its drop-down portion; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06003C4D RID: 15437 RVA: 0x0010A996 File Offset: 0x00108B96
		// (set) Token: 0x06003C4E RID: 15438 RVA: 0x0010A9A3 File Offset: 0x00108BA3
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxDroppedDownDescr")]
		public bool DroppedDown
		{
			get
			{
				return this.ComboBox.DroppedDown;
			}
			set
			{
				this.ComboBox.DroppedDown = value;
			}
		}

		/// <summary>Gets or sets the appearance of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.FlatStyle" />. The options are <see cref="F:System.Windows.Forms.FlatStyle.Flat" />, <see cref="F:System.Windows.Forms.FlatStyle.Popup" />, <see cref="F:System.Windows.Forms.FlatStyle.Standard" />, and <see cref="F:System.Windows.Forms.FlatStyle.System" />. The default is <see cref="F:System.Windows.Forms.FlatStyle.Popup" />.</returns>
		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06003C4F RID: 15439 RVA: 0x0010A9B1 File Offset: 0x00108BB1
		// (set) Token: 0x06003C50 RID: 15440 RVA: 0x0010A9BE File Offset: 0x00108BBE
		[SRCategory("CatAppearance")]
		[DefaultValue(FlatStyle.Popup)]
		[Localizable(true)]
		[SRDescription("ComboBoxFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				return this.ComboBox.FlatStyle;
			}
			set
			{
				this.ComboBox.FlatStyle = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> should resize to avoid showing partial items.</summary>
		/// <returns>
		///     <see langword="true" /> if the list portion can contain only complete items; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06003C51 RID: 15441 RVA: 0x0010A9CC File Offset: 0x00108BCC
		// (set) Token: 0x06003C52 RID: 15442 RVA: 0x0010A9D9 File Offset: 0x00108BD9
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ComboBoxIntegralHeightDescr")]
		public bool IntegralHeight
		{
			get
			{
				return this.ComboBox.IntegralHeight;
			}
			set
			{
				this.ComboBox.IntegralHeight = value;
			}
		}

		/// <summary>Gets a collection of the items contained in this <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>A collection of items.</returns>
		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06003C53 RID: 15443 RVA: 0x0010A9E7 File Offset: 0x00108BE7
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ComboBoxItemsDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public ComboBox.ObjectCollection Items
		{
			get
			{
				return this.ComboBox.Items;
			}
		}

		/// <summary>Gets or sets the maximum number of items to be shown in the drop-down portion of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>The maximum number of items in the drop-down portion. The minimum for this property is 1 and the maximum is 100.</returns>
		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06003C54 RID: 15444 RVA: 0x0010A9F4 File Offset: 0x00108BF4
		// (set) Token: 0x06003C55 RID: 15445 RVA: 0x0010AA01 File Offset: 0x00108C01
		[SRCategory("CatBehavior")]
		[DefaultValue(8)]
		[Localizable(true)]
		[SRDescription("ComboBoxMaxDropDownItemsDescr")]
		public int MaxDropDownItems
		{
			get
			{
				return this.ComboBox.MaxDropDownItems;
			}
			set
			{
				this.ComboBox.MaxDropDownItems = value;
			}
		}

		/// <summary>Gets or sets the maximum number of characters allowed in the editable portion of a combo box.</summary>
		/// <returns>The maximum number of characters the user can enter. Values of less than zero are reset to zero, which is the default value.</returns>
		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06003C56 RID: 15446 RVA: 0x0010AA0F File Offset: 0x00108C0F
		// (set) Token: 0x06003C57 RID: 15447 RVA: 0x0010AA1C File Offset: 0x00108C1C
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Localizable(true)]
		[SRDescription("ComboBoxMaxLengthDescr")]
		public int MaxLength
		{
			get
			{
				return this.ComboBox.MaxLength;
			}
			set
			{
				this.ComboBox.MaxLength = value;
			}
		}

		/// <summary>Gets or sets the index specifying the currently selected item.</summary>
		/// <returns>A zero-based index of the currently selected item. A value of negative one (-1) is returned if no item is selected.</returns>
		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06003C58 RID: 15448 RVA: 0x0010AA2A File Offset: 0x00108C2A
		// (set) Token: 0x06003C59 RID: 15449 RVA: 0x0010AA37 File Offset: 0x00108C37
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedIndexDescr")]
		public int SelectedIndex
		{
			get
			{
				return this.ComboBox.SelectedIndex;
			}
			set
			{
				this.ComboBox.SelectedIndex = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripComboBox.SelectedIndex" /> property has changed.</summary>
		// Token: 0x140002FB RID: 763
		// (add) Token: 0x06003C5A RID: 15450 RVA: 0x0010AA45 File Offset: 0x00108C45
		// (remove) Token: 0x06003C5B RID: 15451 RVA: 0x0010AA58 File Offset: 0x00108C58
		[SRCategory("CatBehavior")]
		[SRDescription("selectedIndexChangedEventDescr")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventSelectedIndexChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventSelectedIndexChanged, value);
			}
		}

		/// <summary>Gets or sets currently selected item in the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>The object that is the currently selected item or <see langword="null" /> if there is no currently selected item.</returns>
		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06003C5C RID: 15452 RVA: 0x0010AA6B File Offset: 0x00108C6B
		// (set) Token: 0x06003C5D RID: 15453 RVA: 0x0010AA78 File Offset: 0x00108C78
		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedItemDescr")]
		public object SelectedItem
		{
			get
			{
				return this.ComboBox.SelectedItem;
			}
			set
			{
				this.ComboBox.SelectedItem = value;
			}
		}

		/// <summary>Gets or sets the text that is selected in the editable portion of a <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>A string that represents the currently selected text in the combo box. If <see cref="P:System.Windows.Forms.ToolStripComboBox.DropDownStyle" /> is set to <see langword="DropDownList" />, the return value is an empty string ("").</returns>
		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06003C5E RID: 15454 RVA: 0x0010AA86 File Offset: 0x00108C86
		// (set) Token: 0x06003C5F RID: 15455 RVA: 0x0010AA93 File Offset: 0x00108C93
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedTextDescr")]
		public string SelectedText
		{
			get
			{
				return this.ComboBox.SelectedText;
			}
			set
			{
				this.ComboBox.SelectedText = value;
			}
		}

		/// <summary>Gets or sets the number of characters selected in the editable portion of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>The number of characters selected in the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</returns>
		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06003C60 RID: 15456 RVA: 0x0010AAA1 File Offset: 0x00108CA1
		// (set) Token: 0x06003C61 RID: 15457 RVA: 0x0010AAAE File Offset: 0x00108CAE
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectionLengthDescr")]
		public int SelectionLength
		{
			get
			{
				return this.ComboBox.SelectionLength;
			}
			set
			{
				this.ComboBox.SelectionLength = value;
			}
		}

		/// <summary>Gets or sets the starting index of text selected in the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>The zero-based index of the first character in the string of the current text selection.</returns>
		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06003C62 RID: 15458 RVA: 0x0010AABC File Offset: 0x00108CBC
		// (set) Token: 0x06003C63 RID: 15459 RVA: 0x0010AAC9 File Offset: 0x00108CC9
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectionStartDescr")]
		public int SelectionStart
		{
			get
			{
				return this.ComboBox.SelectionStart;
			}
			set
			{
				this.ComboBox.SelectionStart = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the items in the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> are sorted.</summary>
		/// <returns>
		///     <see langword="true" /> if the combo box is sorted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06003C64 RID: 15460 RVA: 0x0010AAD7 File Offset: 0x00108CD7
		// (set) Token: 0x06003C65 RID: 15461 RVA: 0x0010AAE4 File Offset: 0x00108CE4
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ComboBoxSortedDescr")]
		public bool Sorted
		{
			get
			{
				return this.ComboBox.Sorted;
			}
			set
			{
				this.ComboBox.Sorted = value;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> text has changed.</summary>
		// Token: 0x140002FC RID: 764
		// (add) Token: 0x06003C66 RID: 15462 RVA: 0x0010AAF2 File Offset: 0x00108CF2
		// (remove) Token: 0x06003C67 RID: 15463 RVA: 0x0010AB05 File Offset: 0x00108D05
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnTextUpdateDescr")]
		public event EventHandler TextUpdate
		{
			add
			{
				base.Events.AddHandler(ToolStripComboBox.EventTextUpdate, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripComboBox.EventTextUpdate, value);
			}
		}

		/// <summary>Maintains performance when items are added to the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> one at a time.</summary>
		// Token: 0x06003C68 RID: 15464 RVA: 0x0010AB18 File Offset: 0x00108D18
		public void BeginUpdate()
		{
			this.ComboBox.BeginUpdate();
		}

		/// <summary>Resumes painting the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> control after painting is suspended by the <see cref="M:System.Windows.Forms.ToolStripComboBox.BeginUpdate" /> method.</summary>
		// Token: 0x06003C69 RID: 15465 RVA: 0x0010AB25 File Offset: 0x00108D25
		public void EndUpdate()
		{
			this.ComboBox.EndUpdate();
		}

		/// <summary>Finds the first item in the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> that starts with the specified string.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to search for.</param>
		/// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
		// Token: 0x06003C6A RID: 15466 RVA: 0x0010AB32 File Offset: 0x00108D32
		public int FindString(string s)
		{
			return this.ComboBox.FindString(s);
		}

		/// <summary>Finds the first item after the given index which starts with the given string. </summary>
		/// <param name="s">The <see cref="T:System.String" /> to search for.</param>
		/// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control.</param>
		/// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
		// Token: 0x06003C6B RID: 15467 RVA: 0x0010AB40 File Offset: 0x00108D40
		public int FindString(string s, int startIndex)
		{
			return this.ComboBox.FindString(s, startIndex);
		}

		/// <summary>Finds the first item in the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> that exactly matches the specified string.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to search for.</param>
		/// <returns>The zero-based index of the first item found; -1 if no match is found.</returns>
		// Token: 0x06003C6C RID: 15468 RVA: 0x0010AB4F File Offset: 0x00108D4F
		public int FindStringExact(string s)
		{
			return this.ComboBox.FindStringExact(s);
		}

		/// <summary>Finds the first item after the specified index that exactly matches the specified string.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to search for.</param>
		/// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control.</param>
		/// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
		// Token: 0x06003C6D RID: 15469 RVA: 0x0010AB5D File Offset: 0x00108D5D
		public int FindStringExact(string s, int startIndex)
		{
			return this.ComboBox.FindStringExact(s, startIndex);
		}

		/// <summary>Returns the height, in pixels, of an item in the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <param name="index">The index of the item to return the height of.</param>
		/// <returns>The height, in pixels, of the item at the specified index.</returns>
		// Token: 0x06003C6E RID: 15470 RVA: 0x0010AB6C File Offset: 0x00108D6C
		public int GetItemHeight(int index)
		{
			return this.ComboBox.GetItemHeight(index);
		}

		/// <summary>Selects a range of text in the editable portion of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <param name="start">The position of the first character in the current text selection within the text box.</param>
		/// <param name="length">The number of characters to select.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="start" /> is less than zero.-or- 
		///         <paramref name="start" /> minus <paramref name="length" /> is less than zero. </exception>
		// Token: 0x06003C6F RID: 15471 RVA: 0x0010AB7A File Offset: 0x00108D7A
		public void Select(int start, int length)
		{
			this.ComboBox.Select(start, length);
		}

		/// <summary>Selects all the text in the editable portion of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		// Token: 0x06003C70 RID: 15472 RVA: 0x0010AB89 File Offset: 0x00108D89
		public void SelectAll()
		{
			this.ComboBox.SelectAll();
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fitted.</summary>
		/// <param name="constrainingSize">The custom-sized area for a control. </param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x06003C71 RID: 15473 RVA: 0x0010AB98 File Offset: 0x00108D98
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size preferredSize = base.GetPreferredSize(constrainingSize);
			preferredSize.Width = Math.Max(preferredSize.Width, 75);
			return preferredSize;
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x0010ABC3 File Offset: 0x00108DC3
		private void HandleDropDown(object sender, EventArgs e)
		{
			this.OnDropDown(e);
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x0010ABCC File Offset: 0x00108DCC
		private void HandleDropDownClosed(object sender, EventArgs e)
		{
			this.OnDropDownClosed(e);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x0010ABD5 File Offset: 0x00108DD5
		private void HandleDropDownStyleChanged(object sender, EventArgs e)
		{
			this.OnDropDownStyleChanged(e);
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x0010ABDE File Offset: 0x00108DDE
		private void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			this.OnSelectedIndexChanged(e);
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x0010ABE7 File Offset: 0x00108DE7
		private void HandleSelectionChangeCommitted(object sender, EventArgs e)
		{
			this.OnSelectionChangeCommitted(e);
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x0010ABF0 File Offset: 0x00108DF0
		private void HandleTextUpdate(object sender, EventArgs e)
		{
			this.OnTextUpdate(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripComboBox.DropDown" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003C78 RID: 15480 RVA: 0x0010ABF9 File Offset: 0x00108DF9
		protected virtual void OnDropDown(EventArgs e)
		{
			if (base.ParentInternal != null)
			{
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(base.ParentInternal.RestoreFocusFilter);
				ToolStripManager.ModalMenuFilter.SuspendMenuMode();
			}
			base.RaiseEvent(ToolStripComboBox.EventDropDown, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripComboBox.DropDownClosed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003C79 RID: 15481 RVA: 0x0010AC29 File Offset: 0x00108E29
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			if (base.ParentInternal != null)
			{
				Application.ThreadContext.FromCurrent().RemoveMessageFilter(base.ParentInternal.RestoreFocusFilter);
				ToolStripManager.ModalMenuFilter.ResumeMenuMode();
			}
			base.RaiseEvent(ToolStripComboBox.EventDropDownClosed, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripComboBox.DropDownStyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003C7A RID: 15482 RVA: 0x0010AC59 File Offset: 0x00108E59
		protected virtual void OnDropDownStyleChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripComboBox.EventDropDownStyleChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripComboBox.SelectedIndexChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003C7B RID: 15483 RVA: 0x0010AC67 File Offset: 0x00108E67
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripComboBox.EventSelectedIndexChanged, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectionChangeCommitted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003C7C RID: 15484 RVA: 0x0010AC75 File Offset: 0x00108E75
		protected virtual void OnSelectionChangeCommitted(EventArgs e)
		{
			base.RaiseEvent(ToolStripComboBox.EventSelectionChangeCommitted, e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripComboBox.TextUpdate" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003C7D RID: 15485 RVA: 0x0010AC83 File Offset: 0x00108E83
		protected virtual void OnTextUpdate(EventArgs e)
		{
			base.RaiseEvent(ToolStripComboBox.EventTextUpdate, e);
		}

		/// <summary>Subscribes events from the specified control.</summary>
		/// <param name="control">The control from which to subscribe events.</param>
		// Token: 0x06003C7E RID: 15486 RVA: 0x0010AC94 File Offset: 0x00108E94
		protected override void OnSubscribeControlEvents(Control control)
		{
			ComboBox comboBox = control as ComboBox;
			if (comboBox != null)
			{
				comboBox.DropDown += this.HandleDropDown;
				comboBox.DropDownClosed += this.HandleDropDownClosed;
				comboBox.DropDownStyleChanged += this.HandleDropDownStyleChanged;
				comboBox.SelectedIndexChanged += this.HandleSelectedIndexChanged;
				comboBox.SelectionChangeCommitted += this.HandleSelectionChangeCommitted;
				comboBox.TextUpdate += this.HandleTextUpdate;
			}
			base.OnSubscribeControlEvents(control);
		}

		/// <summary>Unsubscribes events from the specified control.</summary>
		/// <param name="control">The control from which to unsubscribe events.</param>
		// Token: 0x06003C7F RID: 15487 RVA: 0x0010AD20 File Offset: 0x00108F20
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			ComboBox comboBox = control as ComboBox;
			if (comboBox != null)
			{
				comboBox.DropDown -= this.HandleDropDown;
				comboBox.DropDownClosed -= this.HandleDropDownClosed;
				comboBox.DropDownStyleChanged -= this.HandleDropDownStyleChanged;
				comboBox.SelectedIndexChanged -= this.HandleSelectedIndexChanged;
				comboBox.SelectionChangeCommitted -= this.HandleSelectionChangeCommitted;
				comboBox.TextUpdate -= this.HandleTextUpdate;
			}
			base.OnUnsubscribeControlEvents(control);
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0010ADAA File Offset: 0x00108FAA
		private bool ShouldSerializeDropDownWidth()
		{
			return this.ComboBox.ShouldSerializeDropDownWidth();
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x0010ADB7 File Offset: 0x00108FB7
		internal override bool ShouldSerializeFont()
		{
			return !object.Equals(this.Font, ToolStripManager.DefaultFont);
		}

		/// <summary>Returns a string representation of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
		/// <returns>A string that represents the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</returns>
		// Token: 0x06003C82 RID: 15490 RVA: 0x0010ADCC File Offset: 0x00108FCC
		public override string ToString()
		{
			return base.ToString() + ", Items.Count: " + this.Items.Count.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x04002399 RID: 9113
		internal static readonly object EventDropDown = new object();

		// Token: 0x0400239A RID: 9114
		internal static readonly object EventDropDownClosed = new object();

		// Token: 0x0400239B RID: 9115
		internal static readonly object EventDropDownStyleChanged = new object();

		// Token: 0x0400239C RID: 9116
		internal static readonly object EventSelectedIndexChanged = new object();

		// Token: 0x0400239D RID: 9117
		internal static readonly object EventSelectionChangeCommitted = new object();

		// Token: 0x0400239E RID: 9118
		internal static readonly object EventTextUpdate = new object();

		// Token: 0x0400239F RID: 9119
		private static readonly Padding dropDownPadding = new Padding(2);

		// Token: 0x040023A0 RID: 9120
		private static readonly Padding padding = new Padding(1, 0, 1, 0);

		// Token: 0x040023A1 RID: 9121
		private Padding scaledDropDownPadding = ToolStripComboBox.dropDownPadding;

		// Token: 0x040023A2 RID: 9122
		private Padding scaledPadding = ToolStripComboBox.padding;

		// Token: 0x02000731 RID: 1841
		[ComVisible(true)]
		internal class ToolStripComboBoxAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x060060EB RID: 24811 RVA: 0x0018CDE9 File Offset: 0x0018AFE9
			public ToolStripComboBoxAccessibleObject(ToolStripComboBox ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x17001720 RID: 5920
			// (get) Token: 0x060060EC RID: 24812 RVA: 0x00179039 File Offset: 0x00177239
			public override string DefaultAction
			{
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x060060ED RID: 24813 RVA: 0x0000701A File Offset: 0x0000521A
			public override void DoDefaultAction()
			{
			}

			// Token: 0x17001721 RID: 5921
			// (get) Token: 0x060060EE RID: 24814 RVA: 0x0018CDFC File Offset: 0x0018AFFC
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.ComboBox;
				}
			}

			// Token: 0x060060EF RID: 24815 RVA: 0x0018CE1D File Offset: 0x0018B01D
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild || direction == UnsafeNativeMethods.NavigateDirection.LastChild)
				{
					return this.ownerItem.ComboBox.AccessibilityObject;
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x17001722 RID: 5922
			// (get) Token: 0x060060F0 RID: 24816 RVA: 0x0018CE3F File Offset: 0x0018B03F
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this.ownerItem.RootToolStrip.AccessibilityObject;
				}
			}

			// Token: 0x0400416E RID: 16750
			private ToolStripComboBox ownerItem;
		}

		// Token: 0x02000732 RID: 1842
		internal class ToolStripComboBoxControl : ComboBox
		{
			// Token: 0x060060F1 RID: 24817 RVA: 0x0018CE51 File Offset: 0x0018B051
			public ToolStripComboBoxControl()
			{
				base.FlatStyle = FlatStyle.Popup;
				base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			}

			// Token: 0x17001723 RID: 5923
			// (get) Token: 0x060060F2 RID: 24818 RVA: 0x0018CE6C File Offset: 0x0018B06C
			// (set) Token: 0x060060F3 RID: 24819 RVA: 0x0018CE74 File Offset: 0x0018B074
			public ToolStripComboBox Owner
			{
				get
				{
					return this.owner;
				}
				set
				{
					this.owner = value;
				}
			}

			// Token: 0x17001724 RID: 5924
			// (get) Token: 0x060060F4 RID: 24820 RVA: 0x0018CE80 File Offset: 0x0018B080
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

			// Token: 0x060060F5 RID: 24821 RVA: 0x0018CEB5 File Offset: 0x0018B0B5
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				if (AccessibilityImprovements.Level3)
				{
					return new ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxControlAccessibleObject(this);
				}
				return base.CreateAccessibilityInstance();
			}

			// Token: 0x060060F6 RID: 24822 RVA: 0x0018CECB File Offset: 0x0018B0CB
			internal override ComboBox.FlatComboAdapter CreateFlatComboAdapterInstance()
			{
				return new ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter(this);
			}

			// Token: 0x060060F7 RID: 24823 RVA: 0x0018CED3 File Offset: 0x0018B0D3
			protected override bool IsInputKey(Keys keyData)
			{
				return ((keyData & Keys.Alt) == Keys.Alt && ((keyData & Keys.Down) == Keys.Down || (keyData & Keys.Up) == Keys.Up)) || base.IsInputKey(keyData);
			}

			// Token: 0x060060F8 RID: 24824 RVA: 0x0018CEFC File Offset: 0x0018B0FC
			protected override void OnDropDownClosed(EventArgs e)
			{
				base.OnDropDownClosed(e);
				base.Invalidate();
				base.Update();
			}

			// Token: 0x17001725 RID: 5925
			// (get) Token: 0x060060F9 RID: 24825 RVA: 0x000A010F File Offset: 0x0009E30F
			internal override bool SupportsUiaProviders
			{
				get
				{
					return AccessibilityImprovements.Level3;
				}
			}

			// Token: 0x0400416F RID: 16751
			private ToolStripComboBox owner;

			// Token: 0x0200089E RID: 2206
			internal class ToolStripComboBoxFlatComboAdapter : ComboBox.FlatComboAdapter
			{
				// Token: 0x060070D4 RID: 28884 RVA: 0x0019C271 File Offset: 0x0019A471
				public ToolStripComboBoxFlatComboAdapter(ComboBox comboBox) : base(comboBox, true)
				{
				}

				// Token: 0x060070D5 RID: 28885 RVA: 0x0019C27C File Offset: 0x0019A47C
				private static bool UseBaseAdapter(ComboBox comboBox)
				{
					ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl = comboBox as ToolStripComboBox.ToolStripComboBoxControl;
					return toolStripComboBoxControl == null || !(toolStripComboBoxControl.Owner.Renderer is ToolStripProfessionalRenderer);
				}

				// Token: 0x060070D6 RID: 28886 RVA: 0x0019C2A8 File Offset: 0x0019A4A8
				private static ProfessionalColorTable GetColorTable(ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl)
				{
					if (toolStripComboBoxControl != null)
					{
						return toolStripComboBoxControl.ColorTable;
					}
					return ProfessionalColors.ColorTable;
				}

				// Token: 0x060070D7 RID: 28887 RVA: 0x0019C2B9 File Offset: 0x0019A4B9
				protected override Color GetOuterBorderColor(ComboBox comboBox)
				{
					if (ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.UseBaseAdapter(comboBox))
					{
						return base.GetOuterBorderColor(comboBox);
					}
					if (!comboBox.Enabled)
					{
						return ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.GetColorTable(comboBox as ToolStripComboBox.ToolStripComboBoxControl).ComboBoxBorder;
					}
					return SystemColors.Window;
				}

				// Token: 0x060070D8 RID: 28888 RVA: 0x0019C2E9 File Offset: 0x0019A4E9
				protected override Color GetPopupOuterBorderColor(ComboBox comboBox, bool focused)
				{
					if (ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.UseBaseAdapter(comboBox))
					{
						return base.GetPopupOuterBorderColor(comboBox, focused);
					}
					if (!comboBox.Enabled)
					{
						return SystemColors.ControlDark;
					}
					if (!focused)
					{
						return SystemColors.Window;
					}
					return ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.GetColorTable(comboBox as ToolStripComboBox.ToolStripComboBoxControl).ComboBoxBorder;
				}

				// Token: 0x060070D9 RID: 28889 RVA: 0x0019C324 File Offset: 0x0019A524
				protected override void DrawFlatComboDropDown(ComboBox comboBox, Graphics g, Rectangle dropDownRect)
				{
					if (ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.UseBaseAdapter(comboBox))
					{
						base.DrawFlatComboDropDown(comboBox, g, dropDownRect);
						return;
					}
					if (!comboBox.Enabled || !ToolStripManager.VisualStylesEnabled)
					{
						g.FillRectangle(SystemBrushes.Control, dropDownRect);
					}
					else
					{
						ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl = comboBox as ToolStripComboBox.ToolStripComboBoxControl;
						ProfessionalColorTable colorTable = ToolStripComboBox.ToolStripComboBoxControl.ToolStripComboBoxFlatComboAdapter.GetColorTable(toolStripComboBoxControl);
						if (!comboBox.DroppedDown)
						{
							bool flag = comboBox.ContainsFocus || comboBox.MouseIsOver;
							if (flag)
							{
								using (Brush brush = new LinearGradientBrush(dropDownRect, colorTable.ComboBoxButtonSelectedGradientBegin, colorTable.ComboBoxButtonSelectedGradientEnd, LinearGradientMode.Vertical))
								{
									g.FillRectangle(brush, dropDownRect);
									goto IL_11A;
								}
							}
							if (toolStripComboBoxControl.Owner.IsOnOverflow)
							{
								using (Brush brush2 = new SolidBrush(colorTable.ComboBoxButtonOnOverflow))
								{
									g.FillRectangle(brush2, dropDownRect);
									goto IL_11A;
								}
							}
							using (Brush brush3 = new LinearGradientBrush(dropDownRect, colorTable.ComboBoxButtonGradientBegin, colorTable.ComboBoxButtonGradientEnd, LinearGradientMode.Vertical))
							{
								g.FillRectangle(brush3, dropDownRect);
								goto IL_11A;
							}
						}
						using (Brush brush4 = new LinearGradientBrush(dropDownRect, colorTable.ComboBoxButtonPressedGradientBegin, colorTable.ComboBoxButtonPressedGradientEnd, LinearGradientMode.Vertical))
						{
							g.FillRectangle(brush4, dropDownRect);
						}
					}
					IL_11A:
					Brush brush5;
					if (comboBox.Enabled)
					{
						if (AccessibilityImprovements.Level2 && SystemInformation.HighContrast && (comboBox.ContainsFocus || comboBox.MouseIsOver) && ToolStripManager.VisualStylesEnabled)
						{
							brush5 = SystemBrushes.HighlightText;
						}
						else
						{
							brush5 = SystemBrushes.ControlText;
						}
					}
					else
					{
						brush5 = SystemBrushes.GrayText;
					}
					Point point = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
					point.X += dropDownRect.Width % 2;
					g.FillPolygon(brush5, new Point[]
					{
						new Point(point.X - ComboBox.FlatComboAdapter.Offset2Pixels, point.Y - 1),
						new Point(point.X + ComboBox.FlatComboAdapter.Offset2Pixels + 1, point.Y - 1),
						new Point(point.X, point.Y + ComboBox.FlatComboAdapter.Offset2Pixels)
					});
				}
			}

			// Token: 0x0200089F RID: 2207
			internal class ToolStripComboBoxControlAccessibleObject : ComboBox.ComboBoxUiaProvider
			{
				// Token: 0x060070DA RID: 28890 RVA: 0x0019C578 File Offset: 0x0019A778
				public ToolStripComboBoxControlAccessibleObject(ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl) : base(toolStripComboBoxControl)
				{
					this.childAccessibleObject = new ComboBox.ChildAccessibleObject(toolStripComboBoxControl, toolStripComboBoxControl.Handle);
				}

				// Token: 0x060070DB RID: 28891 RVA: 0x0019C594 File Offset: 0x0019A794
				internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
				{
					if (direction <= UnsafeNativeMethods.NavigateDirection.PreviousSibling)
					{
						ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl = base.Owner as ToolStripComboBox.ToolStripComboBoxControl;
						if (toolStripComboBoxControl != null)
						{
							return toolStripComboBoxControl.Owner.AccessibilityObject.FragmentNavigate(direction);
						}
					}
					return base.FragmentNavigate(direction);
				}

				// Token: 0x17001882 RID: 6274
				// (get) Token: 0x060070DC RID: 28892 RVA: 0x0019C5D0 File Offset: 0x0019A7D0
				internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
				{
					get
					{
						ToolStripComboBox.ToolStripComboBoxControl toolStripComboBoxControl = base.Owner as ToolStripComboBox.ToolStripComboBoxControl;
						if (toolStripComboBoxControl != null)
						{
							return toolStripComboBoxControl.Owner.Owner.AccessibilityObject;
						}
						return base.FragmentRoot;
					}
				}

				// Token: 0x060070DD RID: 28893 RVA: 0x0019C603 File Offset: 0x0019A803
				internal override object GetPropertyValue(int propertyID)
				{
					if (propertyID == 30003)
					{
						return 50003;
					}
					if (propertyID != 30022)
					{
						return base.GetPropertyValue(propertyID);
					}
					return (this.State & AccessibleStates.Offscreen) == AccessibleStates.Offscreen;
				}

				// Token: 0x060070DE RID: 28894 RVA: 0x0019C642 File Offset: 0x0019A842
				internal override bool IsPatternSupported(int patternId)
				{
					return patternId == 10005 || patternId == 10002 || base.IsPatternSupported(patternId);
				}

				// Token: 0x04004405 RID: 17413
				private ComboBox.ChildAccessibleObject childAccessibleObject;
			}
		}
	}
}
