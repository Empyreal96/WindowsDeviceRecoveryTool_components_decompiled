using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a dialog box form that contains a <see cref="T:System.Windows.Forms.PrintPreviewControl" /> for printing from a Windows Forms application.</summary>
	// Token: 0x0200043F RID: 1087
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DesignTimeVisible(true)]
	[DefaultProperty("Document")]
	[ToolboxItemFilter("System.Windows.Forms.Control.TopLevel")]
	[ToolboxItem(true)]
	[SRDescription("DescriptionPrintPreviewDialog")]
	public partial class PrintPreviewDialog : Form
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PrintPreviewDialog" /> class.</summary>
		// Token: 0x06004BC8 RID: 19400 RVA: 0x00138F38 File Offset: 0x00137138
		public PrintPreviewDialog()
		{
			base.AutoScaleBaseSize = new Size(5, 13);
			this.previewControl = new PrintPreviewControl();
			this.imageList = new ImageList();
			Bitmap bitmap = new Bitmap(typeof(PrintPreviewDialog), "PrintPreviewStrip.bmp");
			bitmap.MakeTransparent();
			this.imageList.Images.AddStrip(bitmap);
			this.InitForm();
		}

		/// <summary>Gets or sets the button on the form that is clicked when the user presses the ENTER key.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.IButtonControl" /> that represents the button to use as the accept button for the form.</returns>
		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06004BC9 RID: 19401 RVA: 0x00138FA2 File Offset: 0x001371A2
		// (set) Token: 0x06004BCA RID: 19402 RVA: 0x00138FAA File Offset: 0x001371AA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new IButtonControl AcceptButton
		{
			get
			{
				return base.AcceptButton;
			}
			set
			{
				base.AcceptButton = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form adjusts its size to fit the height of the font used on the form and scales its controls.</summary>
		/// <returns>
		///     <see langword="true" /> if the form will automatically scale itself and its controls based on the current font assigned to the form; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06004BCB RID: 19403 RVA: 0x00138FB3 File Offset: 0x001371B3
		// (set) Token: 0x06004BCC RID: 19404 RVA: 0x00138FBB File Offset: 0x001371BB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool AutoScale
		{
			get
			{
				return base.AutoScale;
			}
			set
			{
				base.AutoScale = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form enables autoscrolling.</summary>
		/// <returns>Represents a Boolean value.</returns>
		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06004BCD RID: 19405 RVA: 0x00138FC4 File Offset: 0x001371C4
		// (set) Token: 0x06004BCE RID: 19406 RVA: 0x00138FCC File Offset: 0x001371CC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.PrintPreviewDialog" /> should automatically resize to fit its contents.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="T:System.Windows.Forms.PrintPreviewDialog" /> should resize to fit its contents; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06004BCF RID: 19407 RVA: 0x001025DE File Offset: 0x001007DE
		// (set) Token: 0x06004BD0 RID: 19408 RVA: 0x001025E6 File Offset: 0x001007E6
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.AutoSize" /> property changes.</summary>
		// Token: 0x140003EE RID: 1006
		// (add) Token: 0x06004BD1 RID: 19409 RVA: 0x001025EF File Offset: 0x001007EF
		// (remove) Token: 0x06004BD2 RID: 19410 RVA: 0x001025F8 File Offset: 0x001007F8
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

		/// <summary>Gets or sets how the control performs validation when the user changes focus to another control.</summary>
		/// <returns>Determines how a control validates its data when it loses user input focus.</returns>
		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06004BD3 RID: 19411 RVA: 0x00138FD5 File Offset: 0x001371D5
		// (set) Token: 0x06004BD4 RID: 19412 RVA: 0x00138FDD File Offset: 0x001371DD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override AutoValidate AutoValidate
		{
			get
			{
				return base.AutoValidate;
			}
			set
			{
				base.AutoValidate = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Form.AutoValidate" /> property changes.</summary>
		// Token: 0x140003EF RID: 1007
		// (add) Token: 0x06004BD5 RID: 19413 RVA: 0x00138FE6 File Offset: 0x001371E6
		// (remove) Token: 0x06004BD6 RID: 19414 RVA: 0x00138FEF File Offset: 0x001371EF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoValidateChanged
		{
			add
			{
				base.AutoValidateChanged += value;
			}
			remove
			{
				base.AutoValidateChanged -= value;
			}
		}

		/// <summary>Gets or sets the background color of the form.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06004BD7 RID: 19415 RVA: 0x00138FF8 File Offset: 0x001371F8
		// (set) Token: 0x06004BD8 RID: 19416 RVA: 0x00139000 File Offset: 0x00137200
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.BackColor" /> property changes.</summary>
		// Token: 0x140003F0 RID: 1008
		// (add) Token: 0x06004BD9 RID: 19417 RVA: 0x00050A7A File Offset: 0x0004EC7A
		// (remove) Token: 0x06004BDA RID: 19418 RVA: 0x00050A83 File Offset: 0x0004EC83
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

		/// <summary>Gets or sets the cancel button for the <see cref="T:System.Windows.Forms.PrintPreviewDialog" />.</summary>
		/// <returns>Allows a control to act like a button on a form.</returns>
		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06004BDB RID: 19419 RVA: 0x00139009 File Offset: 0x00137209
		// (set) Token: 0x06004BDC RID: 19420 RVA: 0x00139011 File Offset: 0x00137211
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new IButtonControl CancelButton
		{
			get
			{
				return base.CancelButton;
			}
			set
			{
				base.CancelButton = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a control box is displayed in the caption bar of the form.</summary>
		/// <returns>
		///     <see langword="true" /> if the form displays a control box in the upper-left corner of the form; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06004BDD RID: 19421 RVA: 0x0013901A File Offset: 0x0013721A
		// (set) Token: 0x06004BDE RID: 19422 RVA: 0x00139022 File Offset: 0x00137222
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool ControlBox
		{
			get
			{
				return base.ControlBox;
			}
			set
			{
				base.ControlBox = value;
			}
		}

		/// <summary>Gets or sets how the short cut menu for the control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> for this control, or <see langword="null" /> if there is no <see cref="T:System.Windows.Forms.ContextMenuStrip" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06004BDF RID: 19423 RVA: 0x0010C0FA File Offset: 0x0010A2FA
		// (set) Token: 0x06004BE0 RID: 19424 RVA: 0x0010C102 File Offset: 0x0010A302
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return base.ContextMenuStrip;
			}
			set
			{
				base.ContextMenuStrip = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.ContextMenuStrip" /> property changes.</summary>
		// Token: 0x140003F1 RID: 1009
		// (add) Token: 0x06004BE1 RID: 19425 RVA: 0x0010C10B File Offset: 0x0010A30B
		// (remove) Token: 0x06004BE2 RID: 19426 RVA: 0x0010C114 File Offset: 0x0010A314
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ContextMenuStripChanged
		{
			add
			{
				base.ContextMenuStripChanged += value;
			}
			remove
			{
				base.ContextMenuStripChanged -= value;
			}
		}

		/// <summary>Gets or sets the border style of the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FormBorderStyle" /> that represents the style of border to display for the form. The default is <see cref="F:System.Windows.Forms.FormBorderStyle.Sizable" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06004BE3 RID: 19427 RVA: 0x0013902B File Offset: 0x0013722B
		// (set) Token: 0x06004BE4 RID: 19428 RVA: 0x00139033 File Offset: 0x00137233
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FormBorderStyle FormBorderStyle
		{
			get
			{
				return base.FormBorderStyle;
			}
			set
			{
				base.FormBorderStyle = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a Help button should be displayed in the caption box of the form.</summary>
		/// <returns>
		///     <see langword="true" /> to display a Help button in the form's caption bar; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06004BE5 RID: 19429 RVA: 0x0013903C File Offset: 0x0013723C
		// (set) Token: 0x06004BE6 RID: 19430 RVA: 0x00139044 File Offset: 0x00137244
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool HelpButton
		{
			get
			{
				return base.HelpButton;
			}
			set
			{
				base.HelpButton = value;
			}
		}

		/// <summary>Gets or sets the icon for the form.</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> that represents the icon for the form.</returns>
		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x0013904D File Offset: 0x0013724D
		// (set) Token: 0x06004BE8 RID: 19432 RVA: 0x00139055 File Offset: 0x00137255
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Icon Icon
		{
			get
			{
				return base.Icon;
			}
			set
			{
				base.Icon = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form is a container for multiple document interface (MDI) child forms.</summary>
		/// <returns>
		///     <see langword="true" /> if the form is a container for MDI child forms; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x0013905E File Offset: 0x0013725E
		// (set) Token: 0x06004BEA RID: 19434 RVA: 0x00139066 File Offset: 0x00137266
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool IsMdiContainer
		{
			get
			{
				return base.IsMdiContainer;
			}
			set
			{
				base.IsMdiContainer = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form will receive key events before the event is passed to the control that has focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the form will receive all key events; <see langword="false" /> if the currently selected control on the form receives key events. The default is <see langword="false" />.</returns>
		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06004BEB RID: 19435 RVA: 0x0013906F File Offset: 0x0013726F
		// (set) Token: 0x06004BEC RID: 19436 RVA: 0x00139077 File Offset: 0x00137277
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool KeyPreview
		{
			get
			{
				return base.KeyPreview;
			}
			set
			{
				base.KeyPreview = value;
			}
		}

		/// <summary>Gets or sets the maximum size the form can be resized to.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the maximum size for the form.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The values of the height or width within the <see cref="T:System.Drawing.Size" /> are less than 0. </exception>
		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06004BED RID: 19437 RVA: 0x00139080 File Offset: 0x00137280
		// (set) Token: 0x06004BEE RID: 19438 RVA: 0x00139088 File Offset: 0x00137288
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.MaximumSize" /> property changes.</summary>
		// Token: 0x140003F2 RID: 1010
		// (add) Token: 0x06004BEF RID: 19439 RVA: 0x00139091 File Offset: 0x00137291
		// (remove) Token: 0x06004BF0 RID: 19440 RVA: 0x0013909A File Offset: 0x0013729A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MaximumSizeChanged
		{
			add
			{
				base.MaximumSizeChanged += value;
			}
			remove
			{
				base.MaximumSizeChanged -= value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the maximize button is displayed in the caption bar of the form.</summary>
		/// <returns>
		///     <see langword="true" /> to display a maximize button for the form; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06004BF1 RID: 19441 RVA: 0x001390A3 File Offset: 0x001372A3
		// (set) Token: 0x06004BF2 RID: 19442 RVA: 0x001390AB File Offset: 0x001372AB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool MaximizeBox
		{
			get
			{
				return base.MaximizeBox;
			}
			set
			{
				base.MaximizeBox = value;
			}
		}

		/// <summary>Gets or sets the margins for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the space between controls.</returns>
		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x001390B4 File Offset: 0x001372B4
		// (set) Token: 0x06004BF4 RID: 19444 RVA: 0x001390BC File Offset: 0x001372BC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Margin
		{
			get
			{
				return base.Margin;
			}
			set
			{
				base.Margin = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Margin" /> property changes.</summary>
		// Token: 0x140003F3 RID: 1011
		// (add) Token: 0x06004BF5 RID: 19445 RVA: 0x001390C5 File Offset: 0x001372C5
		// (remove) Token: 0x06004BF6 RID: 19446 RVA: 0x001390CE File Offset: 0x001372CE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MarginChanged
		{
			add
			{
				base.MarginChanged += value;
			}
			remove
			{
				base.MarginChanged -= value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.MainMenu" /> that is displayed in the form.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MainMenu" /> that represents the menu to display in the form.</returns>
		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06004BF7 RID: 19447 RVA: 0x001390D7 File Offset: 0x001372D7
		// (set) Token: 0x06004BF8 RID: 19448 RVA: 0x001390DF File Offset: 0x001372DF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new MainMenu Menu
		{
			get
			{
				return base.Menu;
			}
			set
			{
				base.Menu = value;
			}
		}

		/// <summary>Gets the minimum size the form can be resized to.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the minimum size for the form.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The values of the height or width within the <see cref="T:System.Drawing.Size" /> are less than 0. </exception>
		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06004BF9 RID: 19449 RVA: 0x001390E8 File Offset: 0x001372E8
		// (set) Token: 0x06004BFA RID: 19450 RVA: 0x001390F0 File Offset: 0x001372F0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.MinimumSize" /> property changes.</summary>
		// Token: 0x140003F4 RID: 1012
		// (add) Token: 0x06004BFB RID: 19451 RVA: 0x001390F9 File Offset: 0x001372F9
		// (remove) Token: 0x06004BFC RID: 19452 RVA: 0x00139102 File Offset: 0x00137302
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MinimumSizeChanged
		{
			add
			{
				base.MinimumSizeChanged += value;
			}
			remove
			{
				base.MinimumSizeChanged -= value;
			}
		}

		/// <summary>Gets or sets the padding for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the control's internal spacing characteristics.</returns>
		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06004BFD RID: 19453 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x06004BFE RID: 19454 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Padding" /> property changes.</summary>
		// Token: 0x140003F5 RID: 1013
		// (add) Token: 0x06004BFF RID: 19455 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x06004C00 RID: 19456 RVA: 0x000204B4 File Offset: 0x0001E6B4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Gets or sets the size of the form.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the form.</returns>
		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06004C01 RID: 19457 RVA: 0x0013910B File Offset: 0x0013730B
		// (set) Token: 0x06004C02 RID: 19458 RVA: 0x00139113 File Offset: 0x00137313
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Size" /> property changes.</summary>
		// Token: 0x140003F6 RID: 1014
		// (add) Token: 0x06004C03 RID: 19459 RVA: 0x0013911C File Offset: 0x0013731C
		// (remove) Token: 0x06004C04 RID: 19460 RVA: 0x00139125 File Offset: 0x00137325
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler SizeChanged
		{
			add
			{
				base.SizeChanged += value;
			}
			remove
			{
				base.SizeChanged -= value;
			}
		}

		/// <summary>Gets or sets the starting position of the dialog box at run time.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FormStartPosition" /> that represents the starting position of the dialog box.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06004C05 RID: 19461 RVA: 0x0013912E File Offset: 0x0013732E
		// (set) Token: 0x06004C06 RID: 19462 RVA: 0x00139136 File Offset: 0x00137336
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FormStartPosition StartPosition
		{
			get
			{
				return base.StartPosition;
			}
			set
			{
				base.StartPosition = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form should be displayed as the topmost form of your application.</summary>
		/// <returns>
		///     <see langword="true" /> to display the form as a topmost form; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06004C07 RID: 19463 RVA: 0x0013913F File Offset: 0x0013733F
		// (set) Token: 0x06004C08 RID: 19464 RVA: 0x00139147 File Offset: 0x00137347
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TopMost
		{
			get
			{
				return base.TopMost;
			}
			set
			{
				base.TopMost = value;
			}
		}

		/// <summary>Gets or sets the color that will represent transparent areas of the form.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color to display transparently on the form.</returns>
		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x00139150 File Offset: 0x00137350
		// (set) Token: 0x06004C0A RID: 19466 RVA: 0x00139158 File Offset: 0x00137358
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Color TransparencyKey
		{
			get
			{
				return base.TransparencyKey;
			}
			set
			{
				base.TransparencyKey = value;
			}
		}

		/// <summary>Gets the wait cursor, typically an hourglass shape.</summary>
		/// <returns>
		///     <see langword="true" /> to use the wait cursor for the current control and all child controls; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06004C0B RID: 19467 RVA: 0x00134B8B File Offset: 0x00132D8B
		// (set) Token: 0x06004C0C RID: 19468 RVA: 0x00139161 File Offset: 0x00137361
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool UseWaitCursor
		{
			get
			{
				return base.UseWaitCursor;
			}
			set
			{
				base.UseWaitCursor = value;
			}
		}

		/// <summary>Gets or sets the form's window state.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.FormWindowState" /> that represents the window state of the form. The default is <see cref="F:System.Windows.Forms.FormWindowState.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06004C0D RID: 19469 RVA: 0x0013916A File Offset: 0x0013736A
		// (set) Token: 0x06004C0E RID: 19470 RVA: 0x00139172 File Offset: 0x00137372
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FormWindowState WindowState
		{
			get
			{
				return base.WindowState;
			}
			set
			{
				base.WindowState = value;
			}
		}

		/// <summary>The accessible role of the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleRole" /> values. The default is <see cref="F:System.Windows.Forms.AccessibleRole.Default" />.</returns>
		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06004C0F RID: 19471 RVA: 0x0013917B File Offset: 0x0013737B
		// (set) Token: 0x06004C10 RID: 19472 RVA: 0x00139183 File Offset: 0x00137383
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new AccessibleRole AccessibleRole
		{
			get
			{
				return base.AccessibleRole;
			}
			set
			{
				base.AccessibleRole = value;
			}
		}

		/// <summary>Gets or sets the accessible description of the control.</summary>
		/// <returns>The accessible description of the control. The default is <see langword="null" />.</returns>
		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06004C11 RID: 19473 RVA: 0x0013918C File Offset: 0x0013738C
		// (set) Token: 0x06004C12 RID: 19474 RVA: 0x00139194 File Offset: 0x00137394
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string AccessibleDescription
		{
			get
			{
				return base.AccessibleDescription;
			}
			set
			{
				base.AccessibleDescription = value;
			}
		}

		/// <summary>Gets or sets the accessible name of the control.</summary>
		/// <returns>The accessible name of the control. The default is <see langword="null" />.</returns>
		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06004C13 RID: 19475 RVA: 0x0013919D File Offset: 0x0013739D
		// (set) Token: 0x06004C14 RID: 19476 RVA: 0x001391A5 File Offset: 0x001373A5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string AccessibleName
		{
			get
			{
				return base.AccessibleName;
			}
			set
			{
				base.AccessibleName = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether entering the control causes validation for all controls that require validation.</summary>
		/// <returns>
		///     <see langword="true" /> if entering the control causes validation to be performed on controls requiring validation; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06004C15 RID: 19477 RVA: 0x000DA227 File Offset: 0x000D8427
		// (set) Token: 0x06004C16 RID: 19478 RVA: 0x000DA22F File Offset: 0x000D842F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.CausesValidation" /> property changes.</summary>
		// Token: 0x140003F7 RID: 1015
		// (add) Token: 0x06004C17 RID: 19479 RVA: 0x000DA238 File Offset: 0x000D8438
		// (remove) Token: 0x06004C18 RID: 19480 RVA: 0x000DA241 File Offset: 0x000D8441
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		/// <summary>Gets the data bindings for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ControlBindingsCollection" /> that contains the <see cref="T:System.Windows.Forms.Binding" /> objects for the control.</returns>
		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x001391AE File Offset: 0x001373AE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ControlBindingsCollection DataBindings
		{
			get
			{
				return base.DataBindings;
			}
		}

		/// <summary>Gets the default minimum size, in pixels, of the <see cref="T:System.Windows.Forms.PrintPreviewDialog" /> control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" /> structure representing the default minimum size.</returns>
		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06004C1A RID: 19482 RVA: 0x001391B6 File Offset: 0x001373B6
		protected override Size DefaultMinimumSize
		{
			get
			{
				return new Size(375, 250);
			}
		}

		/// <summary>Get or sets a value indicating whether the control is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x00012060 File Offset: 0x00010260
		// (set) Token: 0x06004C1C RID: 19484 RVA: 0x00012068 File Offset: 0x00010268
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Enabled" /> property changes.</summary>
		// Token: 0x140003F8 RID: 1016
		// (add) Token: 0x06004C1D RID: 19485 RVA: 0x000FEA52 File Offset: 0x000FCC52
		// (remove) Token: 0x06004C1E RID: 19486 RVA: 0x000FEA5B File Offset: 0x000FCC5B
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

		/// <summary>Gets or sets the coordinates of the upper-left corner of the control relative to the upper-left corner of its container.</summary>
		/// <returns>The <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the control relative to the upper-left corner of its container.</returns>
		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06004C1F RID: 19487 RVA: 0x001391C7 File Offset: 0x001373C7
		// (set) Token: 0x06004C20 RID: 19488 RVA: 0x001391CF File Offset: 0x001373CF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Location" /> property changes.</summary>
		// Token: 0x140003F9 RID: 1017
		// (add) Token: 0x06004C21 RID: 19489 RVA: 0x000F7692 File Offset: 0x000F5892
		// (remove) Token: 0x06004C22 RID: 19490 RVA: 0x000F769B File Offset: 0x000F589B
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

		/// <summary>Gets or sets the object that contains data about the control.</summary>
		/// <returns>An object that contains data about the control. The default is <see langword="null" />.</returns>
		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06004C23 RID: 19491 RVA: 0x001391D8 File Offset: 0x001373D8
		// (set) Token: 0x06004C24 RID: 19492 RVA: 0x001391E0 File Offset: 0x001373E0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new object Tag
		{
			get
			{
				return base.Tag;
			}
			set
			{
				base.Tag = value;
			}
		}

		/// <summary>Gets or sets whether the control can accept data that the user drags onto it.</summary>
		/// <returns>
		///     <see langword="true" /> if drag-and-drop operations are allowed in the control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06004C25 RID: 19493 RVA: 0x000B0BBD File Offset: 0x000AEDBD
		// (set) Token: 0x06004C26 RID: 19494 RVA: 0x000B0BC5 File Offset: 0x000AEDC5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		/// <summary>Gets or sets the cursor for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor to display when the mouse pointer is over the control.</returns>
		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06004C27 RID: 19495 RVA: 0x00012033 File Offset: 0x00010233
		// (set) Token: 0x06004C28 RID: 19496 RVA: 0x0001203B File Offset: 0x0001023B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Cursor" /> property changes.</summary>
		// Token: 0x140003FA RID: 1018
		// (add) Token: 0x06004C29 RID: 19497 RVA: 0x0003E0B3 File Offset: 0x0003C2B3
		// (remove) Token: 0x06004C2A RID: 19498 RVA: 0x0003E0BC File Offset: 0x0003C2BC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CursorChanged
		{
			add
			{
				base.CursorChanged += value;
			}
			remove
			{
				base.CursorChanged -= value;
			}
		}

		/// <summary>Gets or sets the background image for the control.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the image to display in the background of the control.</returns>
		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06004C2B RID: 19499 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06004C2C RID: 19500 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.BackgroundImage" /> property changes.</summary>
		// Token: 0x140003FB RID: 1019
		// (add) Token: 0x06004C2D RID: 19501 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06004C2E RID: 19502 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>Gets or sets the layout of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.BackgroundImage" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x06004C2F RID: 19503 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06004C30 RID: 19504 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140003FC RID: 1020
		// (add) Token: 0x06004C31 RID: 19505 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06004C32 RID: 19506 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets the Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values. The default is <see cref="F:System.Windows.Forms.ImeMode.Inherit" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ImeMode" /> enumeration values. </exception>
		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x06004C33 RID: 19507 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06004C34 RID: 19508 RVA: 0x00011FEC File Offset: 0x000101EC
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.ImeMode" /> property changes.</summary>
		// Token: 0x140003FD RID: 1021
		// (add) Token: 0x06004C35 RID: 19509 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x06004C36 RID: 19510 RVA: 0x0001BF35 File Offset: 0x0001A135
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

		/// <summary>Gets or sets the size of the auto-scroll margin.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the height and width, in pixels, of the auto-scroll margin.</returns>
		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06004C37 RID: 19511 RVA: 0x000F3C48 File Offset: 0x000F1E48
		// (set) Token: 0x06004C38 RID: 19512 RVA: 0x000F3C50 File Offset: 0x000F1E50
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		/// <summary>Gets or sets the minimum size of the automatic scroll bars.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the minimum height and width, in pixels, of the scroll bars.</returns>
		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06004C39 RID: 19513 RVA: 0x000F3C37 File Offset: 0x000F1E37
		// (set) Token: 0x06004C3A RID: 19514 RVA: 0x000F3C3F File Offset: 0x000F1E3F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		/// <summary>Gets or sets the anchor style for the control.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.AnchorStyles" /> values. The default is <see langword="Top" /> and <see langword="Left" />.</returns>
		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06004C3B RID: 19515 RVA: 0x000F7554 File Offset: 0x000F5754
		// (set) Token: 0x06004C3C RID: 19516 RVA: 0x000F755C File Offset: 0x000F575C
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

		/// <summary>Gets or sets a value indicating whether the control is visible.</summary>
		/// <returns>This property is not relevant for this class.
		///     <see langword="true" /> if the control is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06004C3D RID: 19517 RVA: 0x000F7629 File Offset: 0x000F5829
		// (set) Token: 0x06004C3E RID: 19518 RVA: 0x000F7631 File Offset: 0x000F5831
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Visible" /> property changes.</summary>
		// Token: 0x140003FE RID: 1022
		// (add) Token: 0x06004C3F RID: 19519 RVA: 0x000F766E File Offset: 0x000F586E
		// (remove) Token: 0x06004C40 RID: 19520 RVA: 0x000F7677 File Offset: 0x000F5877
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

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.</returns>
		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06004C41 RID: 19521 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x06004C42 RID: 19522 RVA: 0x0001208A File Offset: 0x0001028A
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.ForeColor" /> property changes.</summary>
		// Token: 0x140003FF RID: 1023
		// (add) Token: 0x06004C43 RID: 19523 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x06004C44 RID: 19524 RVA: 0x0005276F File Offset: 0x0005096F
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

		/// <summary>Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft" /> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit" />.</returns>
		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x000DAB7B File Offset: 0x000D8D7B
		// (set) Token: 0x06004C46 RID: 19526 RVA: 0x000BDC35 File Offset: 0x000BBE35
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.PrintPreviewDialog" /> should be laid out from right to left.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate the <see cref="T:System.Windows.Forms.PrintPreviewDialog" /> contents should be laid out from right to left; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012C4 RID: 4804
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x001391E9 File Offset: 0x001373E9
		// (set) Token: 0x06004C48 RID: 19528 RVA: 0x001391F1 File Offset: 0x001373F1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool RightToLeftLayout
		{
			get
			{
				return base.RightToLeftLayout;
			}
			set
			{
				base.RightToLeftLayout = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.RightToLeft" /> property changes.</summary>
		// Token: 0x14000400 RID: 1024
		// (add) Token: 0x06004C49 RID: 19529 RVA: 0x000DAB83 File Offset: 0x000D8D83
		// (remove) Token: 0x06004C4A RID: 19530 RVA: 0x000DAB8C File Offset: 0x000D8D8C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				base.RightToLeftChanged += value;
			}
			remove
			{
				base.RightToLeftChanged -= value;
			}
		}

		/// <summary>Occurs when value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x14000401 RID: 1025
		// (add) Token: 0x06004C4B RID: 19531 RVA: 0x001391FA File Offset: 0x001373FA
		// (remove) Token: 0x06004C4C RID: 19532 RVA: 0x00139203 File Offset: 0x00137403
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.RightToLeftLayoutChanged += value;
			}
			remove
			{
				base.RightToLeftLayoutChanged -= value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to this control using the TAB key; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170012C5 RID: 4805
		// (get) Token: 0x06004C4D RID: 19533 RVA: 0x0013920C File Offset: 0x0013740C
		// (set) Token: 0x06004C4E RID: 19534 RVA: 0x00139214 File Offset: 0x00137414
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.TabStop" /> property changes.</summary>
		// Token: 0x14000402 RID: 1026
		// (add) Token: 0x06004C4F RID: 19535 RVA: 0x0013921D File Offset: 0x0013741D
		// (remove) Token: 0x06004C50 RID: 19536 RVA: 0x00139226 File Offset: 0x00137426
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

		/// <summary>Gets or sets the text displayed on the control.</summary>
		/// <returns>Represents text as a series of Unicode characters.</returns>
		// Token: 0x170012C6 RID: 4806
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x0013922F File Offset: 0x0013742F
		// (set) Token: 0x06004C52 RID: 19538 RVA: 0x00139237 File Offset: 0x00137437
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Text" /> property changes.</summary>
		// Token: 0x14000403 RID: 1027
		// (add) Token: 0x06004C53 RID: 19539 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06004C54 RID: 19540 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Gets or sets how the control should be docked in its parent control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see cref="F:System.Windows.Forms.DockStyle.None" />.</returns>
		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06004C55 RID: 19541 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x06004C56 RID: 19542 RVA: 0x000F7576 File Offset: 0x000F5776
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Dock" /> property changes.</summary>
		// Token: 0x14000404 RID: 1028
		// (add) Token: 0x06004C57 RID: 19543 RVA: 0x000F7680 File Offset: 0x000F5880
		// (remove) Token: 0x06004C58 RID: 19544 RVA: 0x000F7689 File Offset: 0x000F5889
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

		/// <summary>Gets or sets the font used for the control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06004C59 RID: 19545 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x06004C5A RID: 19546 RVA: 0x00012079 File Offset: 0x00010279
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.Font" /> property changes.</summary>
		// Token: 0x14000405 RID: 1029
		// (add) Token: 0x06004C5B RID: 19547 RVA: 0x00052778 File Offset: 0x00050978
		// (remove) Token: 0x06004C5C RID: 19548 RVA: 0x00052781 File Offset: 0x00050981
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

		/// <summary>Gets or sets the shortcut menu for the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ContextMenu" /> that represents the shortcut menu associated with the control.</returns>
		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06004C5D RID: 19549 RVA: 0x00012044 File Offset: 0x00010244
		// (set) Token: 0x06004C5E RID: 19550 RVA: 0x0001204C File Offset: 0x0001024C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ContextMenu ContextMenu
		{
			get
			{
				return base.ContextMenu;
			}
			set
			{
				base.ContextMenu = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewDialog.ContextMenu" /> property changes.</summary>
		// Token: 0x14000406 RID: 1030
		// (add) Token: 0x06004C5F RID: 19551 RVA: 0x0010C0E8 File Offset: 0x0010A2E8
		// (remove) Token: 0x06004C60 RID: 19552 RVA: 0x0010C0F1 File Offset: 0x0010A2F1
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ContextMenuChanged
		{
			add
			{
				base.ContextMenuChanged += value;
			}
			remove
			{
				base.ContextMenuChanged -= value;
			}
		}

		/// <summary>Overrides the <see cref="P:System.Windows.Forms.ScrollableControl.DockPadding" /> property.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ScrollableControl.DockPaddingEdges" /> that represents the padding for all the edges of a docked control.</returns>
		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06004C61 RID: 19553 RVA: 0x000F757F File Offset: 0x000F577F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}

		/// <summary>Gets or sets a value indicating whether printing uses the anti-aliasing features of the operating system.</summary>
		/// <returns>
		///     <see langword="true" /> if anti-aliasing is used; otherwise, <see langword="false" />.</returns>
		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06004C62 RID: 19554 RVA: 0x00139240 File Offset: 0x00137440
		// (set) Token: 0x06004C63 RID: 19555 RVA: 0x0013924D File Offset: 0x0013744D
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("PrintPreviewAntiAliasDescr")]
		public bool UseAntiAlias
		{
			get
			{
				return this.PrintPreviewControl.UseAntiAlias;
			}
			set
			{
				this.PrintPreviewControl.UseAntiAlias = value;
			}
		}

		/// <summary>The <see cref="T:System.Windows.Forms.PrintPreviewDialog" /> class does not support the <see cref="P:System.Windows.Forms.PrintPreviewDialog.AutoScaleBaseSize" /> property.</summary>
		/// <returns>Stores an ordered pair of integers, typically the width and height of a rectangle.</returns>
		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06004C64 RID: 19556 RVA: 0x0013925B File Offset: 0x0013745B
		// (set) Token: 0x06004C65 RID: 19557 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This property has been deprecated. Use the AutoScaleDimensions property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public override Size AutoScaleBaseSize
		{
			get
			{
				return base.AutoScaleBaseSize;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets the document to preview.</summary>
		/// <returns>The <see cref="T:System.Drawing.Printing.PrintDocument" /> representing the document to preview.</returns>
		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06004C66 RID: 19558 RVA: 0x00139263 File Offset: 0x00137463
		// (set) Token: 0x06004C67 RID: 19559 RVA: 0x00139270 File Offset: 0x00137470
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("PrintPreviewDocumentDescr")]
		public PrintDocument Document
		{
			get
			{
				return this.previewControl.Document;
			}
			set
			{
				this.previewControl.Document = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the minimize button is displayed in the caption bar of the form.</summary>
		/// <returns>
		///     <see langword="true" /> to display a minimize button for the form; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06004C68 RID: 19560 RVA: 0x0013927E File Offset: 0x0013747E
		// (set) Token: 0x06004C69 RID: 19561 RVA: 0x00139286 File Offset: 0x00137486
		[Browsable(false)]
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool MinimizeBox
		{
			get
			{
				return base.MinimizeBox;
			}
			set
			{
				base.MinimizeBox = value;
			}
		}

		/// <summary>Gets a value indicating the <see cref="T:System.Windows.Forms.PrintPreviewControl" /> contained in this form.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.PrintPreviewControl" /> contained in this form.</returns>
		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x0013928F File Offset: 0x0013748F
		[SRCategory("CatBehavior")]
		[SRDescription("PrintPreviewPrintPreviewControlDescr")]
		[Browsable(false)]
		public PrintPreviewControl PrintPreviewControl
		{
			get
			{
				return this.previewControl;
			}
		}

		/// <summary>Gets or sets the opacity level of the form.</summary>
		/// <returns>The level of opacity for the control.</returns>
		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x06004C6B RID: 19563 RVA: 0x00139297 File Offset: 0x00137497
		// (set) Token: 0x06004C6C RID: 19564 RVA: 0x0013929F File Offset: 0x0013749F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new double Opacity
		{
			get
			{
				return base.Opacity;
			}
			set
			{
				base.Opacity = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the form is displayed in the Windows taskbar.</summary>
		/// <returns>
		///     <see langword="true" /> to display the form in the Windows taskbar at run time; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06004C6D RID: 19565 RVA: 0x001392A8 File Offset: 0x001374A8
		// (set) Token: 0x06004C6E RID: 19566 RVA: 0x001392B0 File Offset: 0x001374B0
		[Browsable(false)]
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool ShowInTaskbar
		{
			get
			{
				return base.ShowInTaskbar;
			}
			set
			{
				base.ShowInTaskbar = value;
			}
		}

		/// <summary>Gets or sets the style of the size grip to display in the lower-right corner of the form.</summary>
		/// <returns>Gets or sets the style of the size grip to display in the lower-right corner of the form.</returns>
		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06004C6F RID: 19567 RVA: 0x001392B9 File Offset: 0x001374B9
		// (set) Token: 0x06004C70 RID: 19568 RVA: 0x001392C1 File Offset: 0x001374C1
		[Browsable(false)]
		[DefaultValue(SizeGripStyle.Hide)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new SizeGripStyle SizeGripStyle
		{
			get
			{
				return base.SizeGripStyle;
			}
			set
			{
				base.SizeGripStyle = value;
			}
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x001392CC File Offset: 0x001374CC
		private void InitForm()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PrintPreviewDialog));
			this.toolStrip1 = new ToolStrip();
			this.printToolStripButton = new ToolStripButton();
			this.zoomToolStripSplitButton = new ToolStripSplitButton();
			this.autoToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripMenuItem1 = new ToolStripMenuItem();
			this.toolStripMenuItem2 = new ToolStripMenuItem();
			this.toolStripMenuItem3 = new ToolStripMenuItem();
			this.toolStripMenuItem4 = new ToolStripMenuItem();
			this.toolStripMenuItem5 = new ToolStripMenuItem();
			this.toolStripMenuItem6 = new ToolStripMenuItem();
			this.toolStripMenuItem7 = new ToolStripMenuItem();
			this.toolStripMenuItem8 = new ToolStripMenuItem();
			this.separatorToolStripSeparator = new ToolStripSeparator();
			this.onepageToolStripButton = new ToolStripButton();
			this.twopagesToolStripButton = new ToolStripButton();
			this.threepagesToolStripButton = new ToolStripButton();
			this.fourpagesToolStripButton = new ToolStripButton();
			this.sixpagesToolStripButton = new ToolStripButton();
			this.separatorToolStripSeparator1 = new ToolStripSeparator();
			this.closeToolStripButton = new ToolStripButton();
			this.pageCounter = new NumericUpDown();
			this.pageToolStripLabel = new ToolStripLabel();
			this.toolStrip1.SuspendLayout();
			((ISupportInitialize)this.pageCounter).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.printToolStripButton,
				this.zoomToolStripSplitButton,
				this.separatorToolStripSeparator,
				this.onepageToolStripButton,
				this.twopagesToolStripButton,
				this.threepagesToolStripButton,
				this.fourpagesToolStripButton,
				this.sixpagesToolStripButton,
				this.separatorToolStripSeparator1,
				this.closeToolStripButton
			});
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = ToolStripRenderMode.System;
			this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
			this.printToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.printToolStripButton.Name = "printToolStripButton";
			componentResourceManager.ApplyResources(this.printToolStripButton, "printToolStripButton");
			this.zoomToolStripSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.zoomToolStripSplitButton.DoubleClickEnabled = true;
			this.zoomToolStripSplitButton.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.autoToolStripMenuItem,
				this.toolStripMenuItem1,
				this.toolStripMenuItem2,
				this.toolStripMenuItem3,
				this.toolStripMenuItem4,
				this.toolStripMenuItem5,
				this.toolStripMenuItem6,
				this.toolStripMenuItem7,
				this.toolStripMenuItem8
			});
			this.zoomToolStripSplitButton.Name = "zoomToolStripSplitButton";
			this.zoomToolStripSplitButton.SplitterWidth = 1;
			componentResourceManager.ApplyResources(this.zoomToolStripSplitButton, "zoomToolStripSplitButton");
			this.autoToolStripMenuItem.CheckOnClick = true;
			this.autoToolStripMenuItem.DoubleClickEnabled = true;
			this.autoToolStripMenuItem.Checked = true;
			this.autoToolStripMenuItem.Name = "autoToolStripMenuItem";
			componentResourceManager.ApplyResources(this.autoToolStripMenuItem, "autoToolStripMenuItem");
			this.toolStripMenuItem1.CheckOnClick = true;
			this.toolStripMenuItem1.DoubleClickEnabled = true;
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			componentResourceManager.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem2.CheckOnClick = true;
			this.toolStripMenuItem2.DoubleClickEnabled = true;
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			componentResourceManager.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			this.toolStripMenuItem3.CheckOnClick = true;
			this.toolStripMenuItem3.DoubleClickEnabled = true;
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			componentResourceManager.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			this.toolStripMenuItem4.CheckOnClick = true;
			this.toolStripMenuItem4.DoubleClickEnabled = true;
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			componentResourceManager.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
			this.toolStripMenuItem5.CheckOnClick = true;
			this.toolStripMenuItem5.DoubleClickEnabled = true;
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			componentResourceManager.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
			this.toolStripMenuItem6.CheckOnClick = true;
			this.toolStripMenuItem6.DoubleClickEnabled = true;
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			componentResourceManager.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
			this.toolStripMenuItem7.CheckOnClick = true;
			this.toolStripMenuItem7.DoubleClickEnabled = true;
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			componentResourceManager.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
			this.toolStripMenuItem8.CheckOnClick = true;
			this.toolStripMenuItem8.DoubleClickEnabled = true;
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			componentResourceManager.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
			this.separatorToolStripSeparator.Name = "separatorToolStripSeparator";
			this.onepageToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.onepageToolStripButton.Name = "onepageToolStripButton";
			componentResourceManager.ApplyResources(this.onepageToolStripButton, "onepageToolStripButton");
			this.twopagesToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.twopagesToolStripButton.Name = "twopagesToolStripButton";
			componentResourceManager.ApplyResources(this.twopagesToolStripButton, "twopagesToolStripButton");
			this.threepagesToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.threepagesToolStripButton.Name = "threepagesToolStripButton";
			componentResourceManager.ApplyResources(this.threepagesToolStripButton, "threepagesToolStripButton");
			this.fourpagesToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.fourpagesToolStripButton.Name = "fourpagesToolStripButton";
			componentResourceManager.ApplyResources(this.fourpagesToolStripButton, "fourpagesToolStripButton");
			this.sixpagesToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.sixpagesToolStripButton.Name = "sixpagesToolStripButton";
			componentResourceManager.ApplyResources(this.sixpagesToolStripButton, "sixpagesToolStripButton");
			this.separatorToolStripSeparator1.Name = "separatorToolStripSeparator1";
			this.closeToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.closeToolStripButton.Name = "closeToolStripButton";
			componentResourceManager.ApplyResources(this.closeToolStripButton, "closeToolStripButton");
			componentResourceManager.ApplyResources(this.pageCounter, "pageCounter");
			this.pageCounter.Text = "1";
			this.pageCounter.TextAlign = HorizontalAlignment.Right;
			this.pageCounter.DecimalPlaces = 0;
			this.pageCounter.Minimum = new decimal(0.0);
			this.pageCounter.Maximum = new decimal(1000.0);
			this.pageCounter.ValueChanged += this.UpdownMove;
			this.pageCounter.Name = "pageCounter";
			this.pageToolStripLabel.Alignment = ToolStripItemAlignment.Right;
			this.pageToolStripLabel.Name = "pageToolStripLabel";
			componentResourceManager.ApplyResources(this.pageToolStripLabel, "pageToolStripLabel");
			this.previewControl.Size = new Size(792, 610);
			this.previewControl.Location = new Point(0, 43);
			this.previewControl.Dock = DockStyle.Fill;
			this.previewControl.StartPageChanged += this.previewControl_StartPageChanged;
			this.printToolStripButton.Click += this.OnprintToolStripButtonClick;
			this.autoToolStripMenuItem.Click += this.ZoomAuto;
			this.toolStripMenuItem1.Click += this.Zoom500;
			this.toolStripMenuItem2.Click += this.Zoom250;
			this.toolStripMenuItem3.Click += this.Zoom150;
			this.toolStripMenuItem4.Click += this.Zoom100;
			this.toolStripMenuItem5.Click += this.Zoom75;
			this.toolStripMenuItem6.Click += this.Zoom50;
			this.toolStripMenuItem7.Click += this.Zoom25;
			this.toolStripMenuItem8.Click += this.Zoom10;
			this.onepageToolStripButton.Click += this.OnonepageToolStripButtonClick;
			this.twopagesToolStripButton.Click += this.OntwopagesToolStripButtonClick;
			this.threepagesToolStripButton.Click += this.OnthreepagesToolStripButtonClick;
			this.fourpagesToolStripButton.Click += this.OnfourpagesToolStripButtonClick;
			this.sixpagesToolStripButton.Click += this.OnsixpagesToolStripButtonClick;
			this.closeToolStripButton.Click += this.OncloseToolStripButtonClick;
			this.closeToolStripButton.Paint += this.OncloseToolStripButtonPaint;
			this.toolStrip1.ImageList = this.imageList;
			this.printToolStripButton.ImageIndex = 0;
			this.zoomToolStripSplitButton.ImageIndex = 1;
			this.onepageToolStripButton.ImageIndex = 2;
			this.twopagesToolStripButton.ImageIndex = 3;
			this.threepagesToolStripButton.ImageIndex = 4;
			this.fourpagesToolStripButton.ImageIndex = 5;
			this.sixpagesToolStripButton.ImageIndex = 6;
			this.previewControl.TabIndex = 0;
			this.toolStrip1.TabIndex = 1;
			this.zoomToolStripSplitButton.DefaultItem = this.autoToolStripMenuItem;
			ToolStripDropDownMenu toolStripDropDownMenu = this.zoomToolStripSplitButton.DropDown as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null)
			{
				toolStripDropDownMenu.ShowCheckMargin = true;
				toolStripDropDownMenu.ShowImageMargin = false;
				toolStripDropDownMenu.RenderMode = ToolStripRenderMode.System;
			}
			ToolStripControlHost toolStripControlHost = new ToolStripControlHost(this.pageCounter);
			toolStripControlHost.Alignment = ToolStripItemAlignment.Right;
			this.toolStrip1.Items.Add(toolStripControlHost);
			this.toolStrip1.Items.Add(this.pageToolStripLabel);
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.previewControl);
			base.Controls.Add(this.toolStrip1);
			base.ClientSize = new Size(400, 300);
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = SizeGripStyle.Hide;
			this.toolStrip1.ResumeLayout(false);
			((ISupportInitialize)this.pageCounter).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event.</summary>
		/// <param name="e">Provides data for a cancelable event.</param>
		// Token: 0x06004C72 RID: 19570 RVA: 0x00139CBC File Offset: 0x00137EBC
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			this.previewControl.InvalidatePreview();
		}

		/// <summary>Creates the handle for the form that encapsulates the <see cref="T:System.Windows.Forms.PrintPreviewDialog" />.</summary>
		/// <exception cref="T:System.Drawing.Printing.InvalidPrinterException">The printer settings in <see cref="P:System.Windows.Forms.PrintPreviewDialog.Document" /> are not valid. </exception>
		// Token: 0x06004C73 RID: 19571 RVA: 0x00139CD0 File Offset: 0x00137ED0
		protected override void CreateHandle()
		{
			if (this.Document != null && !this.Document.PrinterSettings.IsValid)
			{
				throw new InvalidPrinterException(this.Document.PrinterSettings);
			}
			base.CreateHandle();
		}

		/// <summary>Determines whether a key should be processed further.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values. </param>
		/// <returns>
		///     <see langword="true" /> to indicate the key should be processed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004C74 RID: 19572 RVA: 0x00139D04 File Offset: 0x00137F04
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys - Keys.Left <= 3)
				{
					return false;
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>Processes the TAB key.</summary>
		/// <param name="forward">
		///       <see langword="true" /> to cycle forward through the controls in the form; otherwise, <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> to indicate the TAB key was successfully processed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004C75 RID: 19573 RVA: 0x00139D32 File Offset: 0x00137F32
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessTabKey(bool forward)
		{
			if (base.ActiveControl == this.previewControl)
			{
				this.pageCounter.FocusInternal();
				return true;
			}
			return false;
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool ShouldSerializeAutoScaleBaseSize()
		{
			return false;
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x00139D51 File Offset: 0x00137F51
		internal override bool ShouldSerializeText()
		{
			return !this.Text.Equals(SR.GetString("PrintPreviewDialog_PrintPreview"));
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x00139D6B File Offset: 0x00137F6B
		private void OncloseToolStripButtonClick(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x00139D73 File Offset: 0x00137F73
		private void previewControl_StartPageChanged(object sender, EventArgs e)
		{
			this.pageCounter.Value = this.previewControl.StartPage + 1;
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x00139D94 File Offset: 0x00137F94
		private void CheckZoomMenu(ToolStripMenuItem toChecked)
		{
			foreach (object obj in this.zoomToolStripSplitButton.DropDownItems)
			{
				ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)obj;
				toolStripMenuItem.Checked = (toChecked == toolStripMenuItem);
			}
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x00139DF8 File Offset: 0x00137FF8
		private void ZoomAuto(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.AutoZoom = true;
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x00139E20 File Offset: 0x00138020
		private void Zoom500(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 5.0;
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x00139E50 File Offset: 0x00138050
		private void Zoom250(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 2.5;
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x00139E80 File Offset: 0x00138080
		private void Zoom150(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 1.5;
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x00139EB0 File Offset: 0x001380B0
		private void Zoom100(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 1.0;
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x00139EE0 File Offset: 0x001380E0
		private void Zoom75(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 0.75;
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x00139F10 File Offset: 0x00138110
		private void Zoom50(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 0.5;
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x00139F40 File Offset: 0x00138140
		private void Zoom25(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 0.25;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x00139F70 File Offset: 0x00138170
		private void Zoom10(object sender, EventArgs eventargs)
		{
			ToolStripMenuItem toChecked = sender as ToolStripMenuItem;
			this.CheckZoomMenu(toChecked);
			this.previewControl.Zoom = 0.1;
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x00139FA0 File Offset: 0x001381A0
		private void OncloseToolStripButtonPaint(object sender, PaintEventArgs e)
		{
			ToolStripItem toolStripItem = sender as ToolStripItem;
			if (toolStripItem != null && !toolStripItem.Selected)
			{
				Rectangle rect = new Rectangle(0, 0, toolStripItem.Bounds.Width - 1, toolStripItem.Bounds.Height - 1);
				using (Pen pen = new Pen(SystemColors.ControlDark))
				{
					e.Graphics.DrawRectangle(pen, rect);
				}
			}
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x0013A01C File Offset: 0x0013821C
		private void OnprintToolStripButtonClick(object sender, EventArgs e)
		{
			if (this.previewControl.Document != null)
			{
				this.previewControl.Document.Print();
			}
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x0013A03B File Offset: 0x0013823B
		private void OnzoomToolStripSplitButtonClick(object sender, EventArgs e)
		{
			this.ZoomAuto(null, EventArgs.Empty);
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x0013A049 File Offset: 0x00138249
		private void OnonepageToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 1;
			this.previewControl.Columns = 1;
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x0013A063 File Offset: 0x00138263
		private void OntwopagesToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 1;
			this.previewControl.Columns = 2;
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x0013A07D File Offset: 0x0013827D
		private void OnthreepagesToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 1;
			this.previewControl.Columns = 3;
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x0013A097 File Offset: 0x00138297
		private void OnfourpagesToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 2;
			this.previewControl.Columns = 2;
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x0013A0B1 File Offset: 0x001382B1
		private void OnsixpagesToolStripButtonClick(object sender, EventArgs e)
		{
			this.previewControl.Rows = 2;
			this.previewControl.Columns = 3;
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x0013A0CC File Offset: 0x001382CC
		private void UpdownMove(object sender, EventArgs eventargs)
		{
			int num = (int)this.pageCounter.Value - 1;
			if (num >= 0)
			{
				this.previewControl.StartPage = num;
				return;
			}
			this.pageCounter.Value = this.previewControl.StartPage + 1;
		}

		// Token: 0x040027B6 RID: 10166
		private PrintPreviewControl previewControl;

		// Token: 0x040027B7 RID: 10167
		private ToolStrip toolStrip1;

		// Token: 0x040027B8 RID: 10168
		private NumericUpDown pageCounter;

		// Token: 0x040027B9 RID: 10169
		private ToolStripButton printToolStripButton;

		// Token: 0x040027BA RID: 10170
		private ToolStripSplitButton zoomToolStripSplitButton;

		// Token: 0x040027BB RID: 10171
		private ToolStripMenuItem autoToolStripMenuItem;

		// Token: 0x040027BC RID: 10172
		private ToolStripMenuItem toolStripMenuItem1;

		// Token: 0x040027BD RID: 10173
		private ToolStripMenuItem toolStripMenuItem2;

		// Token: 0x040027BE RID: 10174
		private ToolStripMenuItem toolStripMenuItem3;

		// Token: 0x040027BF RID: 10175
		private ToolStripMenuItem toolStripMenuItem4;

		// Token: 0x040027C0 RID: 10176
		private ToolStripMenuItem toolStripMenuItem5;

		// Token: 0x040027C1 RID: 10177
		private ToolStripMenuItem toolStripMenuItem6;

		// Token: 0x040027C2 RID: 10178
		private ToolStripMenuItem toolStripMenuItem7;

		// Token: 0x040027C3 RID: 10179
		private ToolStripMenuItem toolStripMenuItem8;

		// Token: 0x040027C4 RID: 10180
		private ToolStripSeparator separatorToolStripSeparator;

		// Token: 0x040027C5 RID: 10181
		private ToolStripButton onepageToolStripButton;

		// Token: 0x040027C6 RID: 10182
		private ToolStripButton twopagesToolStripButton;

		// Token: 0x040027C7 RID: 10183
		private ToolStripButton threepagesToolStripButton;

		// Token: 0x040027C8 RID: 10184
		private ToolStripButton fourpagesToolStripButton;

		// Token: 0x040027C9 RID: 10185
		private ToolStripButton sixpagesToolStripButton;

		// Token: 0x040027CA RID: 10186
		private ToolStripSeparator separatorToolStripSeparator1;

		// Token: 0x040027CB RID: 10187
		private ToolStripButton closeToolStripButton;

		// Token: 0x040027CC RID: 10188
		private ToolStripLabel pageToolStripLabel;

		// Token: 0x040027CD RID: 10189
		private ImageList imageList;
	}
}
