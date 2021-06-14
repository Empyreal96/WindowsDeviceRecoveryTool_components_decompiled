using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a control that when clicked displays an associated <see cref="T:System.Windows.Forms.ToolStripDropDown" /> from which the user can select a single item.</summary>
	// Token: 0x020003AF RID: 943
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripDropDownButton : ToolStripDropDownItem
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> class. </summary>
		// Token: 0x06003DF1 RID: 15857 RVA: 0x0010DDD6 File Offset: 0x0010BFD6
		public ToolStripDropDownButton()
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> class that displays the specified text.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		// Token: 0x06003DF2 RID: 15858 RVA: 0x0010DDEB File Offset: 0x0010BFEB
		public ToolStripDropDownButton(string text) : base(text, null, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> class that displays the specified image.</summary>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		// Token: 0x06003DF3 RID: 15859 RVA: 0x0010DE03 File Offset: 0x0010C003
		public ToolStripDropDownButton(Image image) : base(null, image, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> class that displays the specified text and image.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		// Token: 0x06003DF4 RID: 15860 RVA: 0x0010DE1B File Offset: 0x0010C01B
		public ToolStripDropDownButton(string text, Image image) : base(text, image, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> class that displays the specified text and image and raises the <see langword="Click" /> event.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		/// <param name="onClick">The event handler for the <see cref="E:System.Windows.Forms.Control.Click" /> event.</param>
		// Token: 0x06003DF5 RID: 15861 RVA: 0x0010DE33 File Offset: 0x0010C033
		public ToolStripDropDownButton(string text, Image image, EventHandler onClick) : base(text, image, onClick)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> class that has the specified name, displays the specified text and image, and raises the <see langword="Click" /> event.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		/// <param name="onClick">The event handler for the <see cref="E:System.Windows.Forms.Control.Click" /> event.</param>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		// Token: 0x06003DF6 RID: 15862 RVA: 0x0010DE4B File Offset: 0x0010C04B
		public ToolStripDropDownButton(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> class.</summary>
		/// <param name="text">The text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> to be displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		/// <param name="dropDownItems">An array of type <see cref="T:System.Windows.Forms.ToolStripItem" /> containing the items of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />.</param>
		// Token: 0x06003DF7 RID: 15863 RVA: 0x0010DE65 File Offset: 0x0010C065
		public ToolStripDropDownButton(string text, Image image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems)
		{
			this.Initialize();
		}

		/// <summary>Creates a new accessibility object for this <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> instance. </summary>
		/// <returns>A new accessibility object for this <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> instance. </returns>
		// Token: 0x06003DF8 RID: 15864 RVA: 0x0010DE7D File Offset: 0x0010C07D
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level1)
			{
				return new ToolStripDropDownButton.ToolStripDropDownButtonAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		/// <summary>Gets or sets a value indicating whether to use the <see langword="Text" /> property or the <see cref="P:System.Windows.Forms.ToolStripItem.ToolTipText" /> property for the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> ToolTip.</summary>
		/// <returns>
		///     <see langword="true" /> to use the <see cref="P:System.Windows.Forms.Control.Text" /> property for the ToolTip; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x0010A478 File Offset: 0x00108678
		// (set) Token: 0x06003DFA RID: 15866 RVA: 0x0010A480 File Offset: 0x00108680
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

		/// <summary>Gets a value indicating whether to display the <see cref="T:System.Windows.Forms.ToolTip" /> that is defined as the default.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06003DFB RID: 15867 RVA: 0x0000E214 File Offset: 0x0000C414
		protected override bool DefaultAutoToolTip
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a value indicating whether an arrow is displayed on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />, which indicates that further options are available in a drop-down list.</summary>
		/// <returns>
		///     <see langword="true" /> to show an arrow on the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" />; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x0010DE93 File Offset: 0x0010C093
		// (set) Token: 0x06003DFD RID: 15869 RVA: 0x0010DE9B File Offset: 0x0010C09B
		[DefaultValue(true)]
		[SRDescription("ToolStripDropDownButtonShowDropDownArrowDescr")]
		[SRCategory("CatAppearance")]
		public bool ShowDropDownArrow
		{
			get
			{
				return this.showDropDownArrow;
			}
			set
			{
				if (this.showDropDownArrow != value)
				{
					this.showDropDownArrow = value;
					base.InvalidateItemLayout(PropertyNames.ShowDropDownArrow);
				}
			}
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x0010DEB8 File Offset: 0x0010C0B8
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout(this);
		}

		/// <summary>Creates a generic <see cref="T:System.Windows.Forms.ToolStripDropDown" /> for which events can be defined.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripDropDown" /> for which events can be defined.</returns>
		// Token: 0x06003DFF RID: 15871 RVA: 0x0010DEC0 File Offset: 0x0010C0C0
		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripDropDownMenu(this, true);
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0010DEC9 File Offset: 0x0010C0C9
		private void Initialize()
		{
			base.SupportsSpaceKey = true;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06003E01 RID: 15873 RVA: 0x0010DED4 File Offset: 0x0010C0D4
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (Control.ModifierKeys != Keys.Alt && e.Button == MouseButtons.Left)
			{
				if (base.DropDown.Visible)
				{
					ToolStripManager.ModalMenuFilter.CloseActiveDropDown(base.DropDown, ToolStripDropDownCloseReason.AppClicked);
				}
				else
				{
					this.openMouseId = ((base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId());
					base.ShowDropDown(true);
				}
			}
			base.OnMouseDown(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" />  that contains the event data. </param>
		// Token: 0x06003E02 RID: 15874 RVA: 0x0010DF40 File Offset: 0x0010C140
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (Control.ModifierKeys != Keys.Alt && e.Button == MouseButtons.Left)
			{
				byte b = (base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId();
				if (b != this.openMouseId)
				{
					this.openMouseId = 0;
					ToolStripManager.ModalMenuFilter.CloseActiveDropDown(base.DropDown, ToolStripDropDownCloseReason.AppClicked);
					base.Select();
				}
			}
			base.OnMouseUp(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003E03 RID: 15875 RVA: 0x0010DFA6 File Offset: 0x0010C1A6
		protected override void OnMouseLeave(EventArgs e)
		{
			this.openMouseId = 0;
			base.OnMouseLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x06003E04 RID: 15876 RVA: 0x0010DFB8 File Offset: 0x0010C1B8
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				Graphics graphics = e.Graphics;
				renderer.DrawDropDownButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(graphics, this, base.InternalLayout.ImageRectangle));
				}
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
				{
					renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.Text, base.InternalLayout.TextRectangle, this.ForeColor, this.Font, base.InternalLayout.TextFormat));
				}
				if (this.ShowDropDownArrow)
				{
					ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout toolStripDropDownButtonInternalLayout = base.InternalLayout as ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout;
					Rectangle arrowRectangle = (toolStripDropDownButtonInternalLayout != null) ? toolStripDropDownButtonInternalLayout.DropDownArrowRect : Rectangle.Empty;
					Color arrowColor;
					if (this.Selected && !this.Pressed && AccessibilityImprovements.Level2 && SystemInformation.HighContrast)
					{
						arrowColor = (this.Enabled ? SystemColors.HighlightText : SystemColors.ControlDark);
					}
					else
					{
						arrowColor = (this.Enabled ? SystemColors.ControlText : SystemColors.ControlDark);
					}
					renderer.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, this, arrowRectangle, arrowColor, ArrowDirection.Down));
				}
			}
		}

		/// <summary>Retrieves a value indicating whether the drop-down list of the <see cref="T:System.Windows.Forms.ToolStripDropDownButton" /> has items.</summary>
		/// <param name="charCode">The character to process.</param>
		/// <returns>
		///     <see langword="true" /> if the drop-down list has items; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003E05 RID: 15877 RVA: 0x0010E0D6 File Offset: 0x0010C2D6
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (this.HasDropDownItems)
			{
				base.Select();
				base.ShowDropDown();
				return true;
			}
			return false;
		}

		// Token: 0x040023DF RID: 9183
		private bool showDropDownArrow = true;

		// Token: 0x040023E0 RID: 9184
		private byte openMouseId;

		// Token: 0x02000735 RID: 1845
		[ComVisible(true)]
		internal class ToolStripDropDownButtonAccessibleObject : ToolStripDropDownItemAccessibleObject
		{
			// Token: 0x06006108 RID: 24840 RVA: 0x0018D141 File Offset: 0x0018B341
			public ToolStripDropDownButtonAccessibleObject(ToolStripDropDownButton ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x06006109 RID: 24841 RVA: 0x0018D151 File Offset: 0x0018B351
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50000;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x04004174 RID: 16756
			private ToolStripDropDownButton ownerItem;
		}

		// Token: 0x02000736 RID: 1846
		internal class ToolStripDropDownButtonInternalLayout : ToolStripItemInternalLayout
		{
			// Token: 0x0600610A RID: 24842 RVA: 0x0018D170 File Offset: 0x0018B370
			public ToolStripDropDownButtonInternalLayout(ToolStripDropDownButton ownerItem) : base(ownerItem)
			{
				if (DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSizeUnscaled, ownerItem.DeviceDpi);
					this.scaledDropDownArrowPadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding, ownerItem.DeviceDpi);
				}
				else if (DpiHelper.IsScalingRequired)
				{
					ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSizeUnscaled, 0);
					this.scaledDropDownArrowPadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding, 0);
				}
				this.ownerItem = ownerItem;
			}

			// Token: 0x0600610B RID: 24843 RVA: 0x0018D200 File Offset: 0x0018B400
			public override Size GetPreferredSize(Size constrainingSize)
			{
				Size preferredSize = base.GetPreferredSize(constrainingSize);
				if (this.ownerItem.ShowDropDownArrow)
				{
					if (this.ownerItem.TextDirection == ToolStripTextDirection.Horizontal)
					{
						preferredSize.Width += this.DropDownArrowRect.Width + this.scaledDropDownArrowPadding.Horizontal;
					}
					else
					{
						preferredSize.Height += this.DropDownArrowRect.Height + this.scaledDropDownArrowPadding.Vertical;
					}
				}
				return preferredSize;
			}

			// Token: 0x0600610C RID: 24844 RVA: 0x0018D284 File Offset: 0x0018B484
			protected override ToolStripItemInternalLayout.ToolStripItemLayoutOptions CommonLayoutOptions()
			{
				ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions = base.CommonLayoutOptions();
				if (this.ownerItem.ShowDropDownArrow)
				{
					if (this.ownerItem.TextDirection == ToolStripTextDirection.Horizontal)
					{
						int num = ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Width + this.scaledDropDownArrowPadding.Horizontal;
						ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions2 = toolStripItemLayoutOptions;
						toolStripItemLayoutOptions2.client.Width = toolStripItemLayoutOptions2.client.Width - num;
						if (this.ownerItem.RightToLeft == RightToLeft.Yes)
						{
							toolStripItemLayoutOptions.client.Offset(num, 0);
							this.dropDownArrowRect = new Rectangle(this.scaledDropDownArrowPadding.Left, 0, ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Width, this.ownerItem.Bounds.Height);
						}
						else
						{
							this.dropDownArrowRect = new Rectangle(toolStripItemLayoutOptions.client.Right, 0, ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Width, this.ownerItem.Bounds.Height);
						}
					}
					else
					{
						int num2 = ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Height + this.scaledDropDownArrowPadding.Vertical;
						ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions3 = toolStripItemLayoutOptions;
						toolStripItemLayoutOptions3.client.Height = toolStripItemLayoutOptions3.client.Height - num2;
						this.dropDownArrowRect = new Rectangle(0, toolStripItemLayoutOptions.client.Bottom + this.scaledDropDownArrowPadding.Top, this.ownerItem.Bounds.Width - 1, ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Height);
					}
				}
				return toolStripItemLayoutOptions;
			}

			// Token: 0x1700172C RID: 5932
			// (get) Token: 0x0600610D RID: 24845 RVA: 0x0018D3DA File Offset: 0x0018B5DA
			public Rectangle DropDownArrowRect
			{
				get
				{
					return this.dropDownArrowRect;
				}
			}

			// Token: 0x04004175 RID: 16757
			private ToolStripDropDownButton ownerItem;

			// Token: 0x04004176 RID: 16758
			private static readonly Size dropDownArrowSizeUnscaled = new Size(5, 3);

			// Token: 0x04004177 RID: 16759
			private static Size dropDownArrowSize = ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSizeUnscaled;

			// Token: 0x04004178 RID: 16760
			private const int DROP_DOWN_ARROW_PADDING = 2;

			// Token: 0x04004179 RID: 16761
			private static Padding dropDownArrowPadding = new Padding(2);

			// Token: 0x0400417A RID: 16762
			private Padding scaledDropDownArrowPadding = ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding;

			// Token: 0x0400417B RID: 16763
			private Rectangle dropDownArrowRect = Rectangle.Empty;
		}
	}
}
