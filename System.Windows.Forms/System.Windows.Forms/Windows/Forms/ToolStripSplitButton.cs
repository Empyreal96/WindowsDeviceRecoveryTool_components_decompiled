using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a combination of a standard button on the left and a drop-down button on the right, or the other way around if the value of <see cref="T:System.Windows.Forms.RightToLeft" /> is <see langword="Yes" />.</summary>
	// Token: 0x020003F3 RID: 1011
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	[DefaultEvent("ButtonClick")]
	public class ToolStripSplitButton : ToolStripDropDownItem
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> class.</summary>
		// Token: 0x06004403 RID: 17411 RVA: 0x00122B50 File Offset: 0x00120D50
		public ToolStripSplitButton()
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> class with the specified text. </summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		// Token: 0x06004404 RID: 17412 RVA: 0x00122B7B File Offset: 0x00120D7B
		public ToolStripSplitButton(string text) : base(text, null, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> class with the specified image. </summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		// Token: 0x06004405 RID: 17413 RVA: 0x00122BA9 File Offset: 0x00120DA9
		public ToolStripSplitButton(Image image) : base(null, image, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> class with the specified text and image.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		// Token: 0x06004406 RID: 17414 RVA: 0x00122BD7 File Offset: 0x00120DD7
		public ToolStripSplitButton(string text, Image image) : base(text, image, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> class with the specified display text, image, and <see cref="E:System.Windows.Forms.Control.Click" /> event handler.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		/// <param name="onClick">Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when the user clicks the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		// Token: 0x06004407 RID: 17415 RVA: 0x00122C05 File Offset: 0x00120E05
		public ToolStripSplitButton(string text, Image image, EventHandler onClick) : base(text, image, onClick)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> class with the specified display text, image, <see cref="E:System.Windows.Forms.Control.Click" /> event handler, and name.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		/// <param name="onClick">Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when the user clicks the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		// Token: 0x06004408 RID: 17416 RVA: 0x00122C33 File Offset: 0x00120E33
		public ToolStripSplitButton(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> class with the specified text, image, and <see cref="T:System.Windows.Forms.ToolStripItem" /> array.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</param>
		/// <param name="dropDownItems">A <see cref="T:System.Windows.Forms.ToolStripItem" /> array of controls.</param>
		// Token: 0x06004409 RID: 17417 RVA: 0x00122C63 File Offset: 0x00120E63
		public ToolStripSplitButton(string text, Image image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems)
		{
			this.Initialize();
		}

		/// <summary>Gets or sets a value indicating whether default or custom <see cref="T:System.Windows.Forms.ToolTip" /> text is displayed on the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</summary>
		/// <returns>
		///     <see langword="true" /> if default <see cref="T:System.Windows.Forms.ToolTip" /> text is displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x0600440A RID: 17418 RVA: 0x0010A478 File Offset: 0x00108678
		// (set) Token: 0x0600440B RID: 17419 RVA: 0x0010A480 File Offset: 0x00108680
		[DefaultValue(true)]
		public new bool AutoToolTip
		{
			get
			{
				return base.AutoToolTip;
			}
			set
			{
				base.AutoToolTip = value;
			}
		}

		/// <summary>Gets the size and location of the standard button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the size and location of the standard button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</returns>
		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x0600440C RID: 17420 RVA: 0x00122C91 File Offset: 0x00120E91
		[Browsable(false)]
		public Rectangle ButtonBounds
		{
			get
			{
				return this.SplitButtonButton.Bounds;
			}
		}

		/// <summary>Gets a value indicating whether the button portion of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is in the pressed state. </summary>
		/// <returns>
		///     <see langword="true" /> if the button portion of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is in the pressed state; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x0600440D RID: 17421 RVA: 0x00122C9E File Offset: 0x00120E9E
		[Browsable(false)]
		public bool ButtonPressed
		{
			get
			{
				return this.SplitButtonButton.Pressed;
			}
		}

		/// <summary>Gets a value indicating whether the standard button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is selected or the <see cref="P:System.Windows.Forms.ToolStripSplitButton.DropDownButtonPressed" /> property is <see langword="true" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is selected or whether <see cref="P:System.Windows.Forms.ToolStripSplitButton.DropDownButtonPressed" /> is <see langword="true" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x0600440E RID: 17422 RVA: 0x00122CAB File Offset: 0x00120EAB
		[Browsable(false)]
		public bool ButtonSelected
		{
			get
			{
				return this.SplitButtonButton.Selected || this.DropDownButtonPressed;
			}
		}

		/// <summary>Occurs when the standard button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is clicked.</summary>
		// Token: 0x1400037B RID: 891
		// (add) Token: 0x0600440F RID: 17423 RVA: 0x00122CC2 File Offset: 0x00120EC2
		// (remove) Token: 0x06004410 RID: 17424 RVA: 0x00122CD5 File Offset: 0x00120ED5
		[SRCategory("CatAction")]
		[SRDescription("ToolStripSplitButtonOnButtonClickDescr")]
		public event EventHandler ButtonClick
		{
			add
			{
				base.Events.AddHandler(ToolStripSplitButton.EventButtonClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripSplitButton.EventButtonClick, value);
			}
		}

		/// <summary>Occurs when the standard button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is double-clicked.</summary>
		// Token: 0x1400037C RID: 892
		// (add) Token: 0x06004411 RID: 17425 RVA: 0x00122CE8 File Offset: 0x00120EE8
		// (remove) Token: 0x06004412 RID: 17426 RVA: 0x00122CFB File Offset: 0x00120EFB
		[SRCategory("CatAction")]
		[SRDescription("ToolStripSplitButtonOnButtonDoubleClickDescr")]
		public event EventHandler ButtonDoubleClick
		{
			add
			{
				base.Events.AddHandler(ToolStripSplitButton.EventButtonDoubleClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripSplitButton.EventButtonDoubleClick, value);
			}
		}

		/// <summary>Gets a value indicating whether to display the <see cref="T:System.Windows.Forms.ToolTip" /> that is defined as the default. </summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x06004413 RID: 17427 RVA: 0x0000E214 File Offset: 0x0000C414
		protected override bool DefaultAutoToolTip
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets the portion of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> that is activated when the control is first selected.</summary>
		/// <returns>A <see langword="Forms.ToolStripItem" /> representing the portion of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> that is activated when first selected. The default value is <see langword="null" />.</returns>
		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x06004414 RID: 17428 RVA: 0x00122D0E File Offset: 0x00120F0E
		// (set) Token: 0x06004415 RID: 17429 RVA: 0x00122D16 File Offset: 0x00120F16
		[DefaultValue(null)]
		[Browsable(false)]
		public ToolStripItem DefaultItem
		{
			get
			{
				return this.defaultItem;
			}
			set
			{
				if (this.defaultItem != value)
				{
					this.OnDefaultItemChanged(new EventArgs());
					this.defaultItem = value;
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripSplitButton.DefaultItem" /> has changed.</summary>
		// Token: 0x1400037D RID: 893
		// (add) Token: 0x06004416 RID: 17430 RVA: 0x00122D33 File Offset: 0x00120F33
		// (remove) Token: 0x06004417 RID: 17431 RVA: 0x00122D46 File Offset: 0x00120F46
		[SRCategory("CatAction")]
		[SRDescription("ToolStripSplitButtonOnDefaultItemChangedDescr")]
		public event EventHandler DefaultItemChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripSplitButton.EventDefaultItemChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripSplitButton.EventDefaultItemChanged, value);
			}
		}

		/// <summary>Gets a value indicating whether items on a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> are hidden after they are clicked.</summary>
		/// <returns>
		///     <see langword="true" /> if the items are hidden after they are clicked; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x06004418 RID: 17432 RVA: 0x00122D59 File Offset: 0x00120F59
		protected internal override bool DismissWhenClicked
		{
			get
			{
				return !base.DropDown.Visible;
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06004419 RID: 17433 RVA: 0x00122D69 File Offset: 0x00120F69
		internal override Rectangle DropDownButtonArea
		{
			get
			{
				return this.DropDownButtonBounds;
			}
		}

		/// <summary>Gets the size and location, in screen coordinates, of the drop-down button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the size and location of the drop-down button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" />, in screen coordinates.</returns>
		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x0600441A RID: 17434 RVA: 0x00122D71 File Offset: 0x00120F71
		[Browsable(false)]
		public Rectangle DropDownButtonBounds
		{
			get
			{
				return this.dropDownButtonBounds;
			}
		}

		/// <summary>Gets a value indicating whether the drop-down portion of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is in the pressed state. </summary>
		/// <returns>
		///     <see langword="true" /> if the drop-down portion of the <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is in the pressed state; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x0600441B RID: 17435 RVA: 0x00122D79 File Offset: 0x00120F79
		[Browsable(false)]
		public bool DropDownButtonPressed
		{
			get
			{
				return base.DropDown.Visible;
			}
		}

		/// <summary>Gets a value indicating whether the drop-down button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the drop-down button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x0600441C RID: 17436 RVA: 0x00122D86 File Offset: 0x00120F86
		[Browsable(false)]
		public bool DropDownButtonSelected
		{
			get
			{
				return this.Selected;
			}
		}

		/// <summary>The width, in pixels, of the drop-down button portion of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the width in pixels. The default is 11. Starting with the .NET Framework 4.6, the default value is based on the DPI setting of the device running the app.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value is less than zero (0). </exception>
		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x0600441D RID: 17437 RVA: 0x00122D8E File Offset: 0x00120F8E
		// (set) Token: 0x0600441E RID: 17438 RVA: 0x00122D98 File Offset: 0x00120F98
		[SRCategory("CatLayout")]
		[SRDescription("ToolStripSplitButtonDropDownButtonWidthDescr")]
		public int DropDownButtonWidth
		{
			get
			{
				return this.dropDownButtonWidth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("DropDownButtonWidth", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"DropDownButtonWidth",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.dropDownButtonWidth != value)
				{
					this.dropDownButtonWidth = value;
					this.InvalidateSplitButtonLayout();
					base.InvalidateItemLayout(PropertyNames.DropDownButtonWidth, true);
				}
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x0600441F RID: 17439 RVA: 0x00122E0E File Offset: 0x0012100E
		private int DefaultDropDownButtonWidth
		{
			get
			{
				if (!ToolStripSplitButton.isScalingInitialized)
				{
					if (DpiHelper.IsScalingRequired)
					{
						ToolStripSplitButton.scaledDropDownButtonWidth = DpiHelper.LogicalToDeviceUnitsX(11);
					}
					ToolStripSplitButton.isScalingInitialized = true;
				}
				return ToolStripSplitButton.scaledDropDownButtonWidth;
			}
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x06004420 RID: 17440 RVA: 0x00122E38 File Offset: 0x00121038
		private ToolStripSplitButton.ToolStripSplitButtonButton SplitButtonButton
		{
			get
			{
				if (this.splitButtonButton == null)
				{
					this.splitButtonButton = new ToolStripSplitButton.ToolStripSplitButtonButton(this);
				}
				this.splitButtonButton.Image = this.Image;
				this.splitButtonButton.Text = this.Text;
				this.splitButtonButton.BackColor = this.BackColor;
				this.splitButtonButton.ForeColor = this.ForeColor;
				this.splitButtonButton.Font = this.Font;
				this.splitButtonButton.ImageAlign = base.ImageAlign;
				this.splitButtonButton.TextAlign = this.TextAlign;
				this.splitButtonButton.TextImageRelation = base.TextImageRelation;
				return this.splitButtonButton;
			}
		}

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x06004421 RID: 17441 RVA: 0x00122EE7 File Offset: 0x001210E7
		internal ToolStripItemInternalLayout SplitButtonButtonLayout
		{
			get
			{
				if (base.InternalLayout != null && this.splitButtonButtonLayout == null)
				{
					this.splitButtonButtonLayout = new ToolStripSplitButton.ToolStripSplitButtonButtonLayout(this);
				}
				return this.splitButtonButtonLayout;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x06004422 RID: 17442 RVA: 0x00122F0B File Offset: 0x0012110B
		// (set) Token: 0x06004423 RID: 17443 RVA: 0x00122F13 File Offset: 0x00121113
		[SRDescription("ToolStripSplitButtonSplitterWidthDescr")]
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal int SplitterWidth
		{
			get
			{
				return this.splitterWidth;
			}
			set
			{
				if (value < 0)
				{
					this.splitterWidth = 0;
				}
				else
				{
					this.splitterWidth = value;
				}
				this.InvalidateSplitButtonLayout();
			}
		}

		/// <summary>Gets the boundaries of the separator between the standard and drop-down button portions of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the size and location of the separator.</returns>
		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x06004424 RID: 17444 RVA: 0x00122F2F File Offset: 0x0012112F
		[Browsable(false)]
		public Rectangle SplitterBounds
		{
			get
			{
				return this.splitterBounds;
			}
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00122F38 File Offset: 0x00121138
		private void CalculateLayout()
		{
			Rectangle rectangle = new Rectangle(Point.Empty, this.Size);
			Rectangle empty = Rectangle.Empty;
			rectangle = new Rectangle(Point.Empty, new Size(Math.Min(base.Width, this.DropDownButtonWidth), base.Height));
			int width = Math.Max(0, base.Width - rectangle.Width);
			int height = Math.Max(0, base.Height);
			empty = new Rectangle(Point.Empty, new Size(width, height));
			empty.Width -= this.splitterWidth;
			if (this.RightToLeft == RightToLeft.No)
			{
				rectangle.Offset(empty.Right + this.splitterWidth, 0);
				this.splitterBounds = new Rectangle(empty.Right, empty.Top, this.splitterWidth, empty.Height);
			}
			else
			{
				empty.Offset(this.DropDownButtonWidth + this.splitterWidth, 0);
				this.splitterBounds = new Rectangle(rectangle.Right, rectangle.Top, this.splitterWidth, rectangle.Height);
			}
			this.SplitButtonButton.SetBounds(empty);
			this.SetDropDownButtonBounds(rectangle);
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</summary>
		/// <returns>A new accessibility object for the <see cref="T:System.Windows.Forms.ToolStripSplitButton" />.</returns>
		// Token: 0x06004426 RID: 17446 RVA: 0x00123062 File Offset: 0x00121262
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ToolStripSplitButton.ToolStripSplitButtonUiaProvider(this);
			}
			if (AccessibilityImprovements.Level1)
			{
				return new ToolStripSplitButton.ToolStripSplitButtonExAccessibleObject(this);
			}
			return new ToolStripSplitButton.ToolStripSplitButtonAccessibleObject(this);
		}

		/// <summary>Creates a generic <see cref="T:System.Windows.Forms.ToolStripDropDown" /> for which events can be defined.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripDropDown" />.</returns>
		// Token: 0x06004427 RID: 17447 RVA: 0x0010DEC0 File Offset: 0x0010C0C0
		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripDropDownMenu(this, true);
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x00123086 File Offset: 0x00121286
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			this.splitButtonButtonLayout = null;
			return new ToolStripItemInternalLayout(this);
		}

		/// <summary>Retrieves the size of a rectangular area into which a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> can be fitted.</summary>
		/// <param name="constrainingSize">The custom-sized area for a control. </param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" />, representing the width and height of a rectangle.</returns>
		// Token: 0x06004429 RID: 17449 RVA: 0x00123098 File Offset: 0x00121298
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size preferredSize = this.SplitButtonButtonLayout.GetPreferredSize(constrainingSize);
			preferredSize.Width += this.DropDownButtonWidth + this.SplitterWidth + this.Padding.Horizontal;
			return preferredSize;
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x001230DD File Offset: 0x001212DD
		private void InvalidateSplitButtonLayout()
		{
			this.splitButtonButtonLayout = null;
			this.CalculateLayout();
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x001230EC File Offset: 0x001212EC
		private void Initialize()
		{
			this.dropDownButtonWidth = this.DefaultDropDownButtonWidth;
			base.SupportsSpaceKey = true;
		}

		/// <summary>Processes a dialog key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the item; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600442C RID: 17452 RVA: 0x00123101 File Offset: 0x00121301
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessDialogKey(Keys keyData)
		{
			if (this.Enabled && (keyData == Keys.Return || (base.SupportsSpaceKey && keyData == Keys.Space)))
			{
				this.PerformButtonClick();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x0600442D RID: 17453 RVA: 0x0012312C File Offset: 0x0012132C
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			this.PerformButtonClick();
			return true;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripSplitButton.ButtonClick" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600442E RID: 17454 RVA: 0x00123138 File Offset: 0x00121338
		protected virtual void OnButtonClick(EventArgs e)
		{
			if (this.DefaultItem != null)
			{
				this.DefaultItem.FireEvent(ToolStripItemEventType.Click);
			}
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripSplitButton.EventButtonClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripSplitButton.ButtonDoubleClick" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600442F RID: 17455 RVA: 0x0012317C File Offset: 0x0012137C
		public virtual void OnButtonDoubleClick(EventArgs e)
		{
			if (this.DefaultItem != null)
			{
				this.DefaultItem.FireEvent(ToolStripItemEventType.DoubleClick);
			}
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripSplitButton.EventButtonDoubleClick];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripSplitButton.DefaultItemChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004430 RID: 17456 RVA: 0x001231C0 File Offset: 0x001213C0
		protected virtual void OnDefaultItemChanged(EventArgs e)
		{
			this.InvalidateSplitButtonLayout();
			if (this.CanRaiseEvents)
			{
				EventHandler eventHandler = base.Events[ToolStripSplitButton.EventDefaultItemChanged] as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06004431 RID: 17457 RVA: 0x001231FC File Offset: 0x001213FC
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (this.DropDownButtonBounds.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Left && !base.DropDown.Visible)
				{
					this.openMouseId = ((base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId());
					base.ShowDropDown(true);
					return;
				}
			}
			else
			{
				this.SplitButtonButton.Push(true);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06004432 RID: 17458 RVA: 0x0012326C File Offset: 0x0012146C
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!this.Enabled)
			{
				return;
			}
			this.SplitButtonButton.Push(false);
			if (this.DropDownButtonBounds.Contains(e.Location) && e.Button == MouseButtons.Left && base.DropDown.Visible)
			{
				byte b = (base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId();
				if (b != this.openMouseId)
				{
					this.openMouseId = 0;
					ToolStripManager.ModalMenuFilter.CloseActiveDropDown(base.DropDown, ToolStripDropDownCloseReason.AppClicked);
					base.Select();
				}
			}
			Point pt = new Point(e.X, e.Y);
			if (e.Button == MouseButtons.Left && this.SplitButtonButton.Bounds.Contains(pt))
			{
				bool flag = false;
				if (base.DoubleClickEnabled)
				{
					long ticks = DateTime.Now.Ticks;
					long num = ticks - this.lastClickTime;
					this.lastClickTime = ticks;
					if (num >= 0L && num < ToolStripItem.DoubleClickTicks)
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.OnButtonDoubleClick(new EventArgs());
					this.lastClickTime = 0L;
					return;
				}
				this.OnButtonClick(new EventArgs());
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004433 RID: 17459 RVA: 0x0012338D File Offset: 0x0012158D
		protected override void OnMouseLeave(EventArgs e)
		{
			this.openMouseId = 0;
			this.SplitButtonButton.Push(false);
			base.OnMouseLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004434 RID: 17460 RVA: 0x001233A9 File Offset: 0x001215A9
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			this.InvalidateSplitButtonLayout();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x06004435 RID: 17461 RVA: 0x001233B8 File Offset: 0x001215B8
		protected override void OnPaint(PaintEventArgs e)
		{
			ToolStripRenderer renderer = base.Renderer;
			if (renderer != null)
			{
				this.InvalidateSplitButtonLayout();
				Graphics graphics = e.Graphics;
				renderer.DrawSplitButton(new ToolStripItemRenderEventArgs(graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) != ToolStripItemDisplayStyle.None)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(graphics, this, this.SplitButtonButtonLayout.ImageRectangle));
				}
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) != ToolStripItemDisplayStyle.None)
				{
					renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.SplitButtonButton.Text, this.SplitButtonButtonLayout.TextRectangle, this.ForeColor, this.Font, this.SplitButtonButtonLayout.TextFormat));
				}
			}
		}

		/// <summary>If the <see cref="P:System.Windows.Forms.ToolStripItem.Enabled" /> property is <see langword="true" />, calls the <see cref="M:System.Windows.Forms.ToolStripSplitButton.OnButtonClick(System.EventArgs)" /> method.</summary>
		// Token: 0x06004436 RID: 17462 RVA: 0x00123452 File Offset: 0x00121652
		public void PerformButtonClick()
		{
			if (this.Enabled && base.Available)
			{
				base.PerformClick();
				this.OnButtonClick(EventArgs.Empty);
			}
		}

		/// <summary>This method is not relevant to this class.</summary>
		// Token: 0x06004437 RID: 17463 RVA: 0x00123475 File Offset: 0x00121675
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ResetDropDownButtonWidth()
		{
			this.DropDownButtonWidth = this.DefaultDropDownButtonWidth;
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x00123483 File Offset: 0x00121683
		private void SetDropDownButtonBounds(Rectangle rect)
		{
			this.dropDownButtonBounds = rect;
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x0012348C File Offset: 0x0012168C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeDropDownButtonWidth()
		{
			return this.DropDownButtonWidth != this.DefaultDropDownButtonWidth;
		}

		// Token: 0x040025B3 RID: 9651
		private ToolStripItem defaultItem;

		// Token: 0x040025B4 RID: 9652
		private ToolStripSplitButton.ToolStripSplitButtonButton splitButtonButton;

		// Token: 0x040025B5 RID: 9653
		private Rectangle dropDownButtonBounds = Rectangle.Empty;

		// Token: 0x040025B6 RID: 9654
		private ToolStripSplitButton.ToolStripSplitButtonButtonLayout splitButtonButtonLayout;

		// Token: 0x040025B7 RID: 9655
		private int dropDownButtonWidth;

		// Token: 0x040025B8 RID: 9656
		private int splitterWidth = 1;

		// Token: 0x040025B9 RID: 9657
		private Rectangle splitterBounds = Rectangle.Empty;

		// Token: 0x040025BA RID: 9658
		private byte openMouseId;

		// Token: 0x040025BB RID: 9659
		private long lastClickTime;

		// Token: 0x040025BC RID: 9660
		private const int DEFAULT_DROPDOWN_WIDTH = 11;

		// Token: 0x040025BD RID: 9661
		private static readonly object EventDefaultItemChanged = new object();

		// Token: 0x040025BE RID: 9662
		private static readonly object EventButtonClick = new object();

		// Token: 0x040025BF RID: 9663
		private static readonly object EventButtonDoubleClick = new object();

		// Token: 0x040025C0 RID: 9664
		private static readonly object EventDropDownOpened = new object();

		// Token: 0x040025C1 RID: 9665
		private static readonly object EventDropDownClosed = new object();

		// Token: 0x040025C2 RID: 9666
		private static bool isScalingInitialized = false;

		// Token: 0x040025C3 RID: 9667
		private static int scaledDropDownButtonWidth = 11;

		// Token: 0x02000750 RID: 1872
		private class ToolStripSplitButtonButton : ToolStripButton
		{
			// Token: 0x060061F0 RID: 25072 RVA: 0x00191435 File Offset: 0x0018F635
			public ToolStripSplitButtonButton(ToolStripSplitButton owner)
			{
				this.owner = owner;
			}

			// Token: 0x17001760 RID: 5984
			// (get) Token: 0x060061F1 RID: 25073 RVA: 0x00191444 File Offset: 0x0018F644
			// (set) Token: 0x060061F2 RID: 25074 RVA: 0x0000701A File Offset: 0x0000521A
			public override bool Enabled
			{
				get
				{
					return this.owner.Enabled;
				}
				set
				{
				}
			}

			// Token: 0x17001761 RID: 5985
			// (get) Token: 0x060061F3 RID: 25075 RVA: 0x00191451 File Offset: 0x0018F651
			// (set) Token: 0x060061F4 RID: 25076 RVA: 0x0000701A File Offset: 0x0000521A
			public override ToolStripItemDisplayStyle DisplayStyle
			{
				get
				{
					return this.owner.DisplayStyle;
				}
				set
				{
				}
			}

			// Token: 0x17001762 RID: 5986
			// (get) Token: 0x060061F5 RID: 25077 RVA: 0x0019145E File Offset: 0x0018F65E
			// (set) Token: 0x060061F6 RID: 25078 RVA: 0x0000701A File Offset: 0x0000521A
			public override Padding Padding
			{
				get
				{
					return this.owner.Padding;
				}
				set
				{
				}
			}

			// Token: 0x17001763 RID: 5987
			// (get) Token: 0x060061F7 RID: 25079 RVA: 0x0019146B File Offset: 0x0018F66B
			public override ToolStripTextDirection TextDirection
			{
				get
				{
					return this.owner.TextDirection;
				}
			}

			// Token: 0x17001764 RID: 5988
			// (get) Token: 0x060061F8 RID: 25080 RVA: 0x00191478 File Offset: 0x0018F678
			// (set) Token: 0x060061F9 RID: 25081 RVA: 0x0000701A File Offset: 0x0000521A
			public override Image Image
			{
				get
				{
					if ((this.owner.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
					{
						return this.owner.Image;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x17001765 RID: 5989
			// (get) Token: 0x060061FA RID: 25082 RVA: 0x00191497 File Offset: 0x0018F697
			public override bool Selected
			{
				get
				{
					if (this.owner != null)
					{
						return this.owner.Selected;
					}
					return base.Selected;
				}
			}

			// Token: 0x17001766 RID: 5990
			// (get) Token: 0x060061FB RID: 25083 RVA: 0x001914B3 File Offset: 0x0018F6B3
			// (set) Token: 0x060061FC RID: 25084 RVA: 0x0000701A File Offset: 0x0000521A
			public override string Text
			{
				get
				{
					if ((this.owner.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
					{
						return this.owner.Text;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x040041AE RID: 16814
			private ToolStripSplitButton owner;
		}

		// Token: 0x02000751 RID: 1873
		private class ToolStripSplitButtonButtonLayout : ToolStripItemInternalLayout
		{
			// Token: 0x060061FD RID: 25085 RVA: 0x001914D2 File Offset: 0x0018F6D2
			public ToolStripSplitButtonButtonLayout(ToolStripSplitButton owner) : base(owner.SplitButtonButton)
			{
				this.owner = owner;
			}

			// Token: 0x17001767 RID: 5991
			// (get) Token: 0x060061FE RID: 25086 RVA: 0x001914E7 File Offset: 0x0018F6E7
			protected override ToolStripItem Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x17001768 RID: 5992
			// (get) Token: 0x060061FF RID: 25087 RVA: 0x001914EF File Offset: 0x0018F6EF
			protected override ToolStrip ParentInternal
			{
				get
				{
					return this.owner.ParentInternal;
				}
			}

			// Token: 0x17001769 RID: 5993
			// (get) Token: 0x06006200 RID: 25088 RVA: 0x001914FC File Offset: 0x0018F6FC
			public override Rectangle ImageRectangle
			{
				get
				{
					Rectangle imageRectangle = base.ImageRectangle;
					imageRectangle.Offset(this.owner.SplitButtonButton.Bounds.Location);
					return imageRectangle;
				}
			}

			// Token: 0x1700176A RID: 5994
			// (get) Token: 0x06006201 RID: 25089 RVA: 0x00191530 File Offset: 0x0018F730
			public override Rectangle TextRectangle
			{
				get
				{
					Rectangle textRectangle = base.TextRectangle;
					textRectangle.Offset(this.owner.SplitButtonButton.Bounds.Location);
					return textRectangle;
				}
			}

			// Token: 0x040041AF RID: 16815
			private ToolStripSplitButton owner;
		}

		/// <summary>Provides information that accessibility applications use to adjust the user interface of a <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> for users with impairments.</summary>
		// Token: 0x02000752 RID: 1874
		public class ToolStripSplitButtonAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSplitButton.ToolStripSplitButtonAccessibleObject" /> class. </summary>
			/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripSplitButton" /> that owns this <see cref="T:System.Windows.Forms.ToolStripSplitButton.ToolStripSplitButtonAccessibleObject" />.</param>
			// Token: 0x06006202 RID: 25090 RVA: 0x00191564 File Offset: 0x0018F764
			public ToolStripSplitButtonAccessibleObject(ToolStripSplitButton item) : base(item)
			{
				this.owner = item;
			}

			/// <summary>Performs the default action associated with this <see cref="T:System.Windows.Forms.ToolStripSplitButton.ToolStripSplitButtonAccessibleObject" />.</summary>
			// Token: 0x06006203 RID: 25091 RVA: 0x00191574 File Offset: 0x0018F774
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.owner.PerformButtonClick();
			}

			// Token: 0x040041B0 RID: 16816
			private ToolStripSplitButton owner;
		}

		// Token: 0x02000753 RID: 1875
		internal class ToolStripSplitButtonExAccessibleObject : ToolStripSplitButton.ToolStripSplitButtonAccessibleObject
		{
			// Token: 0x06006204 RID: 25092 RVA: 0x00191581 File Offset: 0x0018F781
			public ToolStripSplitButtonExAccessibleObject(ToolStripSplitButton item) : base(item)
			{
				this.ownerItem = item;
			}

			// Token: 0x06006205 RID: 25093 RVA: 0x00191591 File Offset: 0x0018F791
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50000;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06006206 RID: 25094 RVA: 0x001915AD File Offset: 0x0018F7AD
			internal override bool IsIAccessibleExSupported()
			{
				return this.ownerItem != null || base.IsIAccessibleExSupported();
			}

			// Token: 0x06006207 RID: 25095 RVA: 0x001915BF File Offset: 0x0018F7BF
			internal override bool IsPatternSupported(int patternId)
			{
				return (patternId == 10005 && this.ownerItem.HasDropDownItems) || base.IsPatternSupported(patternId);
			}

			// Token: 0x06006208 RID: 25096 RVA: 0x0000E217 File Offset: 0x0000C417
			internal override void Expand()
			{
				this.DoDefaultAction();
			}

			// Token: 0x06006209 RID: 25097 RVA: 0x001915DF File Offset: 0x0018F7DF
			internal override void Collapse()
			{
				if (this.ownerItem != null && this.ownerItem.DropDown != null && this.ownerItem.DropDown.Visible)
				{
					this.ownerItem.DropDown.Close();
				}
			}

			// Token: 0x1700176B RID: 5995
			// (get) Token: 0x0600620A RID: 25098 RVA: 0x00191618 File Offset: 0x0018F818
			internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
			{
				get
				{
					if (!this.ownerItem.DropDown.Visible)
					{
						return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
					}
					return UnsafeNativeMethods.ExpandCollapseState.Expanded;
				}
			}

			// Token: 0x0600620B RID: 25099 RVA: 0x00191630 File Offset: 0x0018F830
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction != UnsafeNativeMethods.NavigateDirection.FirstChild)
				{
					if (direction != UnsafeNativeMethods.NavigateDirection.LastChild)
					{
						return base.FragmentNavigate(direction);
					}
					if (this.DropDownItemsCount <= 0)
					{
						return null;
					}
					return this.ownerItem.DropDown.Items[this.ownerItem.DropDown.Items.Count - 1].AccessibilityObject;
				}
				else
				{
					if (this.DropDownItemsCount <= 0)
					{
						return null;
					}
					return this.ownerItem.DropDown.Items[0].AccessibilityObject;
				}
			}

			// Token: 0x1700176C RID: 5996
			// (get) Token: 0x0600620C RID: 25100 RVA: 0x001916B2 File Offset: 0x0018F8B2
			private int DropDownItemsCount
			{
				get
				{
					if (AccessibilityImprovements.Level3 && this.ExpandCollapseState == UnsafeNativeMethods.ExpandCollapseState.Collapsed)
					{
						return 0;
					}
					return this.ownerItem.DropDownItems.Count;
				}
			}

			// Token: 0x040041B1 RID: 16817
			private ToolStripSplitButton ownerItem;
		}

		// Token: 0x02000754 RID: 1876
		internal class ToolStripSplitButtonUiaProvider : ToolStripDropDownItemAccessibleObject
		{
			// Token: 0x0600620D RID: 25101 RVA: 0x001916D5 File Offset: 0x0018F8D5
			public ToolStripSplitButtonUiaProvider(ToolStripSplitButton owner) : base(owner)
			{
				this._owner = owner;
				this._accessibleObject = new ToolStripSplitButton.ToolStripSplitButtonExAccessibleObject(owner);
			}

			// Token: 0x0600620E RID: 25102 RVA: 0x001916F1 File Offset: 0x0018F8F1
			public override void DoDefaultAction()
			{
				this._accessibleObject.DoDefaultAction();
			}

			// Token: 0x0600620F RID: 25103 RVA: 0x001916FE File Offset: 0x0018F8FE
			internal override object GetPropertyValue(int propertyID)
			{
				return this._accessibleObject.GetPropertyValue(propertyID);
			}

			// Token: 0x06006210 RID: 25104 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override bool IsIAccessibleExSupported()
			{
				return true;
			}

			// Token: 0x06006211 RID: 25105 RVA: 0x0019170C File Offset: 0x0018F90C
			internal override bool IsPatternSupported(int patternId)
			{
				return this._accessibleObject.IsPatternSupported(patternId);
			}

			// Token: 0x06006212 RID: 25106 RVA: 0x0000E217 File Offset: 0x0000C417
			internal override void Expand()
			{
				this.DoDefaultAction();
			}

			// Token: 0x06006213 RID: 25107 RVA: 0x0019171A File Offset: 0x0018F91A
			internal override void Collapse()
			{
				this._accessibleObject.Collapse();
			}

			// Token: 0x1700176D RID: 5997
			// (get) Token: 0x06006214 RID: 25108 RVA: 0x00191727 File Offset: 0x0018F927
			internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
			{
				get
				{
					return this._accessibleObject.ExpandCollapseState;
				}
			}

			// Token: 0x06006215 RID: 25109 RVA: 0x00191734 File Offset: 0x0018F934
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				return this._accessibleObject.FragmentNavigate(direction);
			}

			// Token: 0x040041B2 RID: 16818
			private ToolStripSplitButton _owner;

			// Token: 0x040041B3 RID: 16819
			private ToolStripSplitButton.ToolStripSplitButtonExAccessibleObject _accessibleObject;
		}
	}
}
