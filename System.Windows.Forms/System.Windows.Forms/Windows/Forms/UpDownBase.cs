using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Implements the basic functionality required by a spin box (also known as an up-down control).</summary>
	// Token: 0x0200041B RID: 1051
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.UpDownBaseDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public abstract class UpDownBase : ContainerControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.UpDownBase" /> class.</summary>
		// Token: 0x060048DC RID: 18652 RVA: 0x001315B0 File Offset: 0x0012F7B0
		public UpDownBase()
		{
			if (DpiHelper.IsScalingRequired)
			{
				this.defaultButtonsWidth = base.LogicalToDeviceUnits(16);
			}
			this.upDownButtons = new UpDownBase.UpDownButtons(this);
			this.upDownEdit = new UpDownBase.UpDownEdit(this);
			this.upDownEdit.BorderStyle = BorderStyle.None;
			this.upDownEdit.AutoSize = false;
			this.upDownEdit.KeyDown += this.OnTextBoxKeyDown;
			this.upDownEdit.KeyPress += this.OnTextBoxKeyPress;
			this.upDownEdit.TextChanged += this.OnTextBoxTextChanged;
			this.upDownEdit.LostFocus += this.OnTextBoxLostFocus;
			this.upDownEdit.Resize += this.OnTextBoxResize;
			this.upDownButtons.TabStop = false;
			this.upDownButtons.Size = new Size(this.defaultButtonsWidth, this.PreferredHeight);
			this.upDownButtons.UpDown += this.OnUpDown;
			base.Controls.AddRange(new Control[]
			{
				this.upDownButtons,
				this.upDownEdit
			});
			base.SetStyle(ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.FixedHeight, true);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
		}

		/// <summary>Gets a value indicating whether the container will allow the user to scroll to any controls placed outside of its visible boundaries.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x060048DD RID: 18653 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x060048DE RID: 18654 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoScroll
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets the size of the auto-scroll margin.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the height and width, in pixels, of the auto-scroll margin.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Size.Height" /> or <see cref="P:System.Drawing.Size.Width" /> is less than 0.</exception>
		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x060048DF RID: 18655 RVA: 0x000F3C48 File Offset: 0x000F1E48
		// (set) Token: 0x060048E0 RID: 18656 RVA: 0x000F3C50 File Offset: 0x000F1E50
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Gets or sets the minimum size of the auto-scroll area.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the minimum height and width, in pixels, of the scroll bars.</returns>
		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x060048E1 RID: 18657 RVA: 0x000F3C37 File Offset: 0x000F1E37
		// (set) Token: 0x060048E2 RID: 18658 RVA: 0x000F3C3F File Offset: 0x000F1E3F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Gets or sets a value indicating whether the control should automatically resize based on its contents.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate the control should automatically resize based on its contents; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x060048E3 RID: 18659 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x060048E4 RID: 18660 RVA: 0x000B0BCE File Offset: 0x000AEDCE
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.UpDownBase.AutoSize" /> property changes.</summary>
		// Token: 0x140003AC RID: 940
		// (add) Token: 0x060048E5 RID: 18661 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x060048E6 RID: 18662 RVA: 0x0001BA37 File Offset: 0x00019C37
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		/// <summary>Gets or sets the background color for the text box portion of the spin box (also known as an up-down control).</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the text box portion of the spin box.</returns>
		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x060048E7 RID: 18663 RVA: 0x00131724 File Offset: 0x0012F924
		// (set) Token: 0x060048E8 RID: 18664 RVA: 0x00131731 File Offset: 0x0012F931
		public override Color BackColor
		{
			get
			{
				return this.upDownEdit.BackColor;
			}
			set
			{
				base.BackColor = value;
				this.upDownEdit.BackColor = value;
				base.Invalidate();
			}
		}

		/// <summary>Gets or sets the background image for the <see cref="T:System.Windows.Forms.UpDownBase" />.</summary>
		/// <returns>The background image for the <see cref="T:System.Windows.Forms.UpDownBase" />.</returns>
		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x060048E9 RID: 18665 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x060048EA RID: 18666 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.UpDownBase.BackgroundImage" /> property changes.</summary>
		// Token: 0x140003AD RID: 941
		// (add) Token: 0x060048EB RID: 18667 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x060048EC RID: 18668 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>Gets or sets the layout of the <see cref="P:System.Windows.Forms.UpDownBase.BackgroundImage" /> of the <see cref="T:System.Windows.Forms.UpDownBase" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x060048ED RID: 18669 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x060048EE RID: 18670 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.UpDownBase.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140003AE RID: 942
		// (add) Token: 0x060048EF RID: 18671 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x060048F0 RID: 18672 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets the border style for the spin box (also known as an up-down control).</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default value is <see cref="F:System.Windows.Forms.BorderStyle.Fixed3D" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x060048F1 RID: 18673 RVA: 0x0013174C File Offset: 0x0012F94C
		// (set) Token: 0x060048F2 RID: 18674 RVA: 0x00131754 File Offset: 0x0012F954
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("UpDownBaseBorderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the text property is being changed internally by its parent class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.UpDownBase.Text" /> property is being changed internally by the <see cref="T:System.Windows.Forms.UpDownBase" /> class; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x060048F3 RID: 18675 RVA: 0x00131792 File Offset: 0x0012F992
		// (set) Token: 0x060048F4 RID: 18676 RVA: 0x0013179A File Offset: 0x0012F99A
		protected bool ChangingText
		{
			get
			{
				return this.changingText;
			}
			set
			{
				this.changingText = value;
			}
		}

		/// <summary>Gets or sets the shortcut menu associated with the spin box (also known as an up-down control).</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenu" /> associated with the spin box.</returns>
		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x060048F5 RID: 18677 RVA: 0x00012044 File Offset: 0x00010244
		// (set) Token: 0x060048F6 RID: 18678 RVA: 0x001317A3 File Offset: 0x0012F9A3
		public override ContextMenu ContextMenu
		{
			get
			{
				return base.ContextMenu;
			}
			set
			{
				base.ContextMenu = value;
				this.upDownEdit.ContextMenu = value;
			}
		}

		/// <summary>Gets or sets the shortcut menu for the spin box (also known as an up-down control).</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> associated with the control.</returns>
		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x060048F7 RID: 18679 RVA: 0x0010C0FA File Offset: 0x0010A2FA
		// (set) Token: 0x060048F8 RID: 18680 RVA: 0x001317B8 File Offset: 0x0012F9B8
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return base.ContextMenuStrip;
			}
			set
			{
				base.ContextMenuStrip = value;
				this.upDownEdit.ContextMenuStrip = value;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>The creation parameters.</returns>
		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x060048F9 RID: 18681 RVA: 0x001317D0 File Offset: 0x0012F9D0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style &= -8388609;
				if (!Application.RenderWithVisualStyles)
				{
					BorderStyle borderStyle = this.borderStyle;
					if (borderStyle != BorderStyle.FixedSingle)
					{
						if (borderStyle == BorderStyle.Fixed3D)
						{
							createParams.ExStyle |= 512;
						}
					}
					else
					{
						createParams.Style |= 8388608;
					}
				}
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x060048FA RID: 18682 RVA: 0x00131833 File Offset: 0x0012FA33
		protected override Size DefaultSize
		{
			get
			{
				return new Size(120, this.PreferredHeight);
			}
		}

		/// <summary>Gets the dock padding settings for all edges of the <see cref="T:System.Windows.Forms.UpDownBase" /> control.</summary>
		/// <returns>The dock paddings settings for this control.</returns>
		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x060048FB RID: 18683 RVA: 0x000F757F File Offset: 0x000F577F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}

		/// <summary>Gets a value indicating whether the control has input focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the control has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x060048FC RID: 18684 RVA: 0x00131842 File Offset: 0x0012FA42
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlFocusedDescr")]
		public override bool Focused
		{
			get
			{
				return this.upDownEdit.Focused;
			}
		}

		/// <summary>Gets or sets the foreground color of the spin box (also known as an up-down control).</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the spin box.</returns>
		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x060048FD RID: 18685 RVA: 0x0013184F File Offset: 0x0012FA4F
		// (set) Token: 0x060048FE RID: 18686 RVA: 0x0013185C File Offset: 0x0012FA5C
		public override Color ForeColor
		{
			get
			{
				return this.upDownEdit.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				this.upDownEdit.ForeColor = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can use the UP ARROW and DOWN ARROW keys to select values.</summary>
		/// <returns>
		///     <see langword="true" /> if the control allows the use of the UP ARROW and DOWN ARROW keys to select values; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x060048FF RID: 18687 RVA: 0x00131871 File Offset: 0x0012FA71
		// (set) Token: 0x06004900 RID: 18688 RVA: 0x00131879 File Offset: 0x0012FA79
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("UpDownBaseInterceptArrowKeysDescr")]
		public bool InterceptArrowKeys
		{
			get
			{
				return this.interceptArrowKeys;
			}
			set
			{
				this.interceptArrowKeys = value;
			}
		}

		/// <summary>Gets or sets the maximum size of the spin box (also known as an up-down control).</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" />, which is the maximum size of the spin box.</returns>
		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x06004901 RID: 18689 RVA: 0x000203D7 File Offset: 0x0001E5D7
		// (set) Token: 0x06004902 RID: 18690 RVA: 0x000203DF File Offset: 0x0001E5DF
		public override Size MaximumSize
		{
			get
			{
				return base.MaximumSize;
			}
			set
			{
				base.MaximumSize = new Size(value.Width, 0);
			}
		}

		/// <summary>Gets or sets the minimum size of the spin box (also known as an up-down control).</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" />, which is the minimum size of the spin box.</returns>
		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x06004903 RID: 18691 RVA: 0x000203F4 File Offset: 0x0001E5F4
		// (set) Token: 0x06004904 RID: 18692 RVA: 0x000203FC File Offset: 0x0001E5FC
		public override Size MinimumSize
		{
			get
			{
				return base.MinimumSize;
			}
			set
			{
				base.MinimumSize = new Size(value.Width, 0);
			}
		}

		/// <summary>Occurs when the mouse pointer enters the <see cref="T:System.Windows.Forms.UpDownBase" /> control.</summary>
		// Token: 0x140003AF RID: 943
		// (add) Token: 0x06004905 RID: 18693 RVA: 0x000B0EF8 File Offset: 0x000AF0F8
		// (remove) Token: 0x06004906 RID: 18694 RVA: 0x000B0F01 File Offset: 0x000AF101
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseEnter
		{
			add
			{
				base.MouseEnter += value;
			}
			remove
			{
				base.MouseEnter -= value;
			}
		}

		/// <summary>Occurs when the mouse pointer leaves the <see cref="T:System.Windows.Forms.UpDownBase" /> control.</summary>
		// Token: 0x140003B0 RID: 944
		// (add) Token: 0x06004907 RID: 18695 RVA: 0x000B0F0A File Offset: 0x000AF10A
		// (remove) Token: 0x06004908 RID: 18696 RVA: 0x000B0F13 File Offset: 0x000AF113
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseLeave
		{
			add
			{
				base.MouseLeave += value;
			}
			remove
			{
				base.MouseLeave -= value;
			}
		}

		/// <summary>Occurs when the mouse pointer rests on the <see cref="T:System.Windows.Forms.UpDownBase" /> control.</summary>
		// Token: 0x140003B1 RID: 945
		// (add) Token: 0x06004909 RID: 18697 RVA: 0x00131882 File Offset: 0x0012FA82
		// (remove) Token: 0x0600490A RID: 18698 RVA: 0x0013188B File Offset: 0x0012FA8B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MouseHover
		{
			add
			{
				base.MouseHover += value;
			}
			remove
			{
				base.MouseHover -= value;
			}
		}

		/// <summary>Occurs when the user moves the mouse pointer over the <see cref="T:System.Windows.Forms.UpDownBase" /> control.</summary>
		// Token: 0x140003B2 RID: 946
		// (add) Token: 0x0600490B RID: 18699 RVA: 0x000B0EE6 File Offset: 0x000AF0E6
		// (remove) Token: 0x0600490C RID: 18700 RVA: 0x000B0EEF File Offset: 0x000AF0EF
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

		/// <summary>Gets the height of the spin box (also known as an up-down control).</summary>
		/// <returns>The height, in pixels, of the spin box.</returns>
		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x0600490D RID: 18701 RVA: 0x00131894 File Offset: 0x0012FA94
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("UpDownBasePreferredHeightDescr")]
		public int PreferredHeight
		{
			get
			{
				int num = base.FontHeight;
				if (this.borderStyle != BorderStyle.None)
				{
					num += SystemInformation.BorderSize.Height * 4 + 3;
				}
				else
				{
					num += 3;
				}
				return num;
			}
		}

		/// <summary>Gets or sets a value indicating whether the text can be changed by the use of the up or down buttons only.</summary>
		/// <returns>
		///     <see langword="true" /> if the text can be changed by the use of the up or down buttons only; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x0600490E RID: 18702 RVA: 0x001318CB File Offset: 0x0012FACB
		// (set) Token: 0x0600490F RID: 18703 RVA: 0x001318D8 File Offset: 0x0012FAD8
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("UpDownBaseReadOnlyDescr")]
		public bool ReadOnly
		{
			get
			{
				return this.upDownEdit.ReadOnly;
			}
			set
			{
				this.upDownEdit.ReadOnly = value;
			}
		}

		/// <summary>Gets or sets the text displayed in the spin box (also known as an up-down control).</summary>
		/// <returns>The string value displayed in the spin box.</returns>
		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06004910 RID: 18704 RVA: 0x001318E6 File Offset: 0x0012FAE6
		// (set) Token: 0x06004911 RID: 18705 RVA: 0x001318F3 File Offset: 0x0012FAF3
		[Localizable(true)]
		public override string Text
		{
			get
			{
				return this.upDownEdit.Text;
			}
			set
			{
				this.upDownEdit.Text = value;
				this.ChangingText = false;
				if (this.UserEdit)
				{
					this.ValidateEditText();
				}
			}
		}

		/// <summary>Gets or sets the alignment of the text in the spin box (also known as an up-down control).</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. The default value is <see cref="F:System.Windows.Forms.HorizontalAlignment.Left" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. </exception>
		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06004912 RID: 18706 RVA: 0x00131916 File Offset: 0x0012FB16
		// (set) Token: 0x06004913 RID: 18707 RVA: 0x00131923 File Offset: 0x0012FB23
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(HorizontalAlignment.Left)]
		[SRDescription("UpDownBaseTextAlignDescr")]
		public HorizontalAlignment TextAlign
		{
			get
			{
				return this.upDownEdit.TextAlign;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				this.upDownEdit.TextAlign = value;
			}
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06004914 RID: 18708 RVA: 0x00131957 File Offset: 0x0012FB57
		internal TextBox TextBox
		{
			get
			{
				return this.upDownEdit;
			}
		}

		/// <summary>Gets or sets the alignment of the up and down buttons on the spin box (also known as an up-down control).</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.LeftRightAlignment" /> values. The default value is <see cref="F:System.Windows.Forms.LeftRightAlignment.Right" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.LeftRightAlignment" /> values. </exception>
		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06004915 RID: 18709 RVA: 0x0013195F File Offset: 0x0012FB5F
		// (set) Token: 0x06004916 RID: 18710 RVA: 0x00131968 File Offset: 0x0012FB68
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(LeftRightAlignment.Right)]
		[SRDescription("UpDownBaseAlignmentDescr")]
		public LeftRightAlignment UpDownAlign
		{
			get
			{
				return this.upDownAlign;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(LeftRightAlignment));
				}
				if (this.upDownAlign != value)
				{
					this.upDownAlign = value;
					this.PositionControls();
					base.Invalidate();
				}
			}
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06004917 RID: 18711 RVA: 0x001319B7 File Offset: 0x0012FBB7
		internal UpDownBase.UpDownButtons UpDownButtonsInternal
		{
			get
			{
				return this.upDownButtons;
			}
		}

		/// <summary>Gets or sets a value indicating whether a value has been entered by the user.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has changed the <see cref="P:System.Windows.Forms.UpDownBase.Text" /> property; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06004918 RID: 18712 RVA: 0x001319BF File Offset: 0x0012FBBF
		// (set) Token: 0x06004919 RID: 18713 RVA: 0x001319C7 File Offset: 0x0012FBC7
		protected bool UserEdit
		{
			get
			{
				return this.userEdit;
			}
			set
			{
				this.userEdit = value;
			}
		}

		/// <summary>When overridden in a derived class, handles the clicking of the down button on the spin box (also known as an up-down control).</summary>
		// Token: 0x0600491A RID: 18714
		public abstract void DownButton();

		// Token: 0x0600491B RID: 18715 RVA: 0x001319D0 File Offset: 0x0012FBD0
		internal override Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			return base.ApplyBoundsConstraints(suggestedX, suggestedY, proposedWidth, this.PreferredHeight);
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x001319E1 File Offset: 0x0012FBE1
		internal string GetAccessibleName(string baseName)
		{
			if (baseName == null)
			{
				if (AccessibilityImprovements.Level3)
				{
					return SR.GetString("SpinnerAccessibleName");
				}
				if (AccessibilityImprovements.Level1)
				{
					return base.GetType().Name;
				}
			}
			return baseName;
		}

		/// <summary>Provides constants for rescaling the control when a DPI change occurs.</summary>
		/// <param name="deviceDpiOld">The DPI value prior to the change.</param>
		/// <param name="deviceDpiNew">The DPI value after the change.</param>
		// Token: 0x0600491D RID: 18717 RVA: 0x00131A0C File Offset: 0x0012FC0C
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			this.defaultButtonsWidth = base.LogicalToDeviceUnits(16);
			this.upDownButtons.Width = this.defaultButtonsWidth;
		}

		/// <summary>When overridden in a derived class, raises the Changed event.</summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600491E RID: 18718 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnChanged(object source, EventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600491F RID: 18719 RVA: 0x00131A35 File Offset: 0x0012FC35
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.PositionControls();
			SystemEvents.UserPreferenceChanged += this.UserPreferenceChanged;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004920 RID: 18720 RVA: 0x00131A55 File Offset: 0x0012FC55
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.UserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" />  that contains the event data. </param>
		// Token: 0x06004921 RID: 18721 RVA: 0x00131A70 File Offset: 0x0012FC70
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Rectangle bounds = this.upDownEdit.Bounds;
			if (Application.RenderWithVisualStyles)
			{
				if (this.borderStyle == BorderStyle.None)
				{
					goto IL_249;
				}
				Rectangle clientRectangle = base.ClientRectangle;
				Rectangle clipRectangle = e.ClipRectangle;
				VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
				int num = 1;
				Rectangle clipRectangle2 = new Rectangle(clientRectangle.Left, clientRectangle.Top, num, clientRectangle.Height);
				Rectangle clipRectangle3 = new Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width, num);
				Rectangle clipRectangle4 = new Rectangle(clientRectangle.Right - num, clientRectangle.Top, num, clientRectangle.Height);
				Rectangle clipRectangle5 = new Rectangle(clientRectangle.Left, clientRectangle.Bottom - num, clientRectangle.Width, num);
				clipRectangle2.Intersect(clipRectangle);
				clipRectangle3.Intersect(clipRectangle);
				clipRectangle4.Intersect(clipRectangle);
				clipRectangle5.Intersect(clipRectangle);
				visualStyleRenderer.DrawBackground(e.Graphics, clientRectangle, clipRectangle2, base.HandleInternal);
				visualStyleRenderer.DrawBackground(e.Graphics, clientRectangle, clipRectangle3, base.HandleInternal);
				visualStyleRenderer.DrawBackground(e.Graphics, clientRectangle, clipRectangle4, base.HandleInternal);
				visualStyleRenderer.DrawBackground(e.Graphics, clientRectangle, clipRectangle5, base.HandleInternal);
				using (Pen pen = new Pen(this.BackColor))
				{
					Rectangle rect = bounds;
					int num2 = rect.X;
					rect.X = num2 - 1;
					num2 = rect.Y;
					rect.Y = num2 - 1;
					num2 = rect.Width;
					rect.Width = num2 + 1;
					num2 = rect.Height;
					rect.Height = num2 + 1;
					e.Graphics.DrawRectangle(pen, rect);
					goto IL_249;
				}
			}
			using (Pen pen2 = new Pen(this.BackColor, (float)(base.Enabled ? 2 : 1)))
			{
				Rectangle rect2 = bounds;
				rect2.Inflate(1, 1);
				if (!base.Enabled)
				{
					int num2 = rect2.X;
					rect2.X = num2 - 1;
					num2 = rect2.Y;
					rect2.Y = num2 - 1;
					num2 = rect2.Width;
					rect2.Width = num2 + 1;
					num2 = rect2.Height;
					rect2.Height = num2 + 1;
				}
				e.Graphics.DrawRectangle(pen2, rect2);
			}
			IL_249:
			if (!base.Enabled && this.BorderStyle != BorderStyle.None && !this.upDownEdit.ShouldSerializeBackColor())
			{
				bounds.Inflate(1, 1);
				ControlPaint.DrawBorder(e.Graphics, bounds, SystemColors.Control, ButtonBorderStyle.Solid);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.</summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06004922 RID: 18722 RVA: 0x00131D1C File Offset: 0x0012FF1C
		protected virtual void OnTextBoxKeyDown(object source, KeyEventArgs e)
		{
			this.OnKeyDown(e);
			if (this.interceptArrowKeys)
			{
				if (e.KeyData == Keys.Up)
				{
					this.UpButton();
					e.Handled = true;
				}
				else if (e.KeyData == Keys.Down)
				{
					this.DownButton();
					e.Handled = true;
				}
			}
			if (e.KeyCode == Keys.Return && this.UserEdit)
			{
				this.ValidateEditText();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data. </param>
		// Token: 0x06004923 RID: 18723 RVA: 0x00131D80 File Offset: 0x0012FF80
		protected virtual void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004924 RID: 18724 RVA: 0x00131D89 File Offset: 0x0012FF89
		protected virtual void OnTextBoxLostFocus(object source, EventArgs e)
		{
			if (this.UserEdit)
			{
				this.ValidateEditText();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004925 RID: 18725 RVA: 0x00131D99 File Offset: 0x0012FF99
		protected virtual void OnTextBoxResize(object source, EventArgs e)
		{
			base.Height = this.PreferredHeight;
			this.PositionControls();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004926 RID: 18726 RVA: 0x00131DAD File Offset: 0x0012FFAD
		protected virtual void OnTextBoxTextChanged(object source, EventArgs e)
		{
			if (this.changingText)
			{
				this.ChangingText = false;
			}
			else
			{
				this.UserEdit = true;
			}
			this.OnTextChanged(e);
			this.OnChanged(source, new EventArgs());
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnStartTimer()
		{
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnStopTimer()
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06004929 RID: 18729 RVA: 0x00131DDA File Offset: 0x0012FFDA
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Clicks == 2 && e.Button == MouseButtons.Left)
			{
				this.doubleClickFired = true;
			}
			base.OnMouseDown(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event. </summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x0600492A RID: 18730 RVA: 0x00131E00 File Offset: 0x00130000
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left)
			{
				Point point = base.PointToScreen(new Point(mevent.X, mevent.Y));
				if (UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle && !base.ValidationCancelled)
				{
					if (!this.doubleClickFired)
					{
						this.OnClick(mevent);
						this.OnMouseClick(mevent);
					}
					else
					{
						this.doubleClickFired = false;
						this.OnDoubleClick(mevent);
						this.OnMouseDoubleClick(mevent);
					}
				}
				this.doubleClickFired = false;
			}
			base.OnMouseUp(mevent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x0600492B RID: 18731 RVA: 0x00131E98 File Offset: 0x00130098
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			HandledMouseEventArgs handledMouseEventArgs = e as HandledMouseEventArgs;
			if (handledMouseEventArgs != null)
			{
				if (handledMouseEventArgs.Handled)
				{
					return;
				}
				handledMouseEventArgs.Handled = true;
			}
			if ((Control.ModifierKeys & (Keys.Shift | Keys.Alt)) != Keys.None || Control.MouseButtons != MouseButtons.None)
			{
				return;
			}
			int num = SystemInformation.MouseWheelScrollLines;
			if (num == 0)
			{
				return;
			}
			this.wheelDelta += e.Delta;
			float num2 = (float)this.wheelDelta / 120f;
			if (num == -1)
			{
				num = 1;
			}
			int num3 = (int)((float)num * num2);
			if (num3 != 0)
			{
				if (num3 > 0)
				{
					for (int i = num3; i > 0; i--)
					{
						this.UpButton();
					}
					this.wheelDelta -= (int)((float)num3 * (120f / (float)num));
					return;
				}
				for (int i = -num3; i > 0; i--)
				{
					this.DownButton();
				}
				this.wheelDelta -= (int)((float)num3 * (120f / (float)num));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
		// Token: 0x0600492C RID: 18732 RVA: 0x00131F79 File Offset: 0x00130179
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.PositionControls();
			base.OnLayout(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600492D RID: 18733 RVA: 0x00131F88 File Offset: 0x00130188
		protected override void OnFontChanged(EventArgs e)
		{
			base.FontHeight = -1;
			base.Height = this.PreferredHeight;
			this.PositionControls();
			base.OnFontChanged(e);
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x00131FAA File Offset: 0x001301AA
		private void OnUpDown(object source, UpDownEventArgs e)
		{
			if (e.ButtonID == 1)
			{
				this.UpButton();
				return;
			}
			if (e.ButtonID == 2)
			{
				this.DownButton();
			}
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x00131FCC File Offset: 0x001301CC
		private void PositionControls()
		{
			Rectangle bounds = Rectangle.Empty;
			Rectangle empty = Rectangle.Empty;
			Rectangle rectangle = new Rectangle(Point.Empty, base.ClientSize);
			int width = rectangle.Width;
			bool renderWithVisualStyles = Application.RenderWithVisualStyles;
			BorderStyle borderStyle = this.BorderStyle;
			int num = (borderStyle == BorderStyle.None) ? 0 : 2;
			rectangle.Inflate(-num, -num);
			if (this.upDownEdit != null)
			{
				bounds = rectangle;
				bounds.Size = new Size(rectangle.Width - this.defaultButtonsWidth, rectangle.Height);
			}
			if (this.upDownButtons != null)
			{
				int num2 = renderWithVisualStyles ? 1 : 2;
				if (borderStyle == BorderStyle.None)
				{
					num2 = 0;
				}
				empty = new Rectangle(rectangle.Right - this.defaultButtonsWidth + num2, rectangle.Top - num2, this.defaultButtonsWidth, rectangle.Height + num2 * 2);
			}
			LeftRightAlignment align = this.UpDownAlign;
			if (base.RtlTranslateLeftRight(align) == LeftRightAlignment.Left)
			{
				empty.X = width - empty.Right;
				bounds.X = width - bounds.Right;
			}
			if (this.upDownEdit != null)
			{
				this.upDownEdit.Bounds = bounds;
			}
			if (this.upDownButtons != null)
			{
				this.upDownButtons.Bounds = empty;
				this.upDownButtons.Invalidate();
			}
		}

		/// <summary>Selects a range of text in the spin box (also known as an up-down control) specifying the starting position and number of characters to select.</summary>
		/// <param name="start">The position of the first character to be selected. </param>
		/// <param name="length">The total number of characters to be selected. </param>
		// Token: 0x06004930 RID: 18736 RVA: 0x00132106 File Offset: 0x00130306
		public void Select(int start, int length)
		{
			this.upDownEdit.Select(start, length);
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x00132118 File Offset: 0x00130318
		private MouseEventArgs TranslateMouseEvent(Control child, MouseEventArgs e)
		{
			if (child != null && base.IsHandleCreated)
			{
				NativeMethods.POINT point = new NativeMethods.POINT(e.X, e.Y);
				UnsafeNativeMethods.MapWindowPoints(new HandleRef(child, child.Handle), new HandleRef(this, base.Handle), point, 1);
				return new MouseEventArgs(e.Button, e.Clicks, point.x, point.y, e.Delta);
			}
			return e;
		}

		/// <summary>When overridden in a derived class, handles the clicking of the up button on the spin box (also known as an up-down control).</summary>
		// Token: 0x06004932 RID: 18738
		public abstract void UpButton();

		/// <summary>When overridden in a derived class, updates the text displayed in the spin box (also known as an up-down control).</summary>
		// Token: 0x06004933 RID: 18739
		protected abstract void UpdateEditText();

		// Token: 0x06004934 RID: 18740 RVA: 0x00132187 File Offset: 0x00130387
		private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			if (pref.Category == UserPreferenceCategory.Locale)
			{
				this.UpdateEditText();
			}
		}

		/// <summary>When overridden in a derived class, validates the text displayed in the spin box (also known as an up-down control).</summary>
		// Token: 0x06004935 RID: 18741 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void ValidateEditText()
		{
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06004936 RID: 18742 RVA: 0x0013219C File Offset: 0x0013039C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 7)
			{
				if (msg != 8)
				{
					base.WndProc(ref m);
					return;
				}
				this.DefWndProc(ref m);
				return;
			}
			else
			{
				if (base.HostedInWin32DialogManager)
				{
					if (this.TextBox.CanFocus)
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(this.TextBox, this.TextBox.Handle));
					}
					base.WndProc(ref m);
					return;
				}
				if (base.ActiveControl == null)
				{
					base.SetActiveControlInternal(this.TextBox);
					return;
				}
				base.FocusActiveControlInternal();
				return;
			}
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x0013221E File Offset: 0x0013041E
		internal void SetToolTip(ToolTip toolTip, string caption)
		{
			toolTip.SetToolTip(this.upDownEdit, caption);
			toolTip.SetToolTip(this.upDownButtons, caption);
		}

		// Token: 0x040026CA RID: 9930
		private const int DefaultWheelScrollLinesPerPage = 1;

		// Token: 0x040026CB RID: 9931
		private const int DefaultButtonsWidth = 16;

		// Token: 0x040026CC RID: 9932
		private const int DefaultControlWidth = 120;

		// Token: 0x040026CD RID: 9933
		private const int ThemedBorderWidth = 1;

		// Token: 0x040026CE RID: 9934
		private const BorderStyle DefaultBorderStyle = BorderStyle.Fixed3D;

		// Token: 0x040026CF RID: 9935
		private static readonly bool DefaultInterceptArrowKeys = true;

		// Token: 0x040026D0 RID: 9936
		private const LeftRightAlignment DefaultUpDownAlign = LeftRightAlignment.Right;

		// Token: 0x040026D1 RID: 9937
		private const int DefaultTimerInterval = 500;

		// Token: 0x040026D2 RID: 9938
		internal UpDownBase.UpDownEdit upDownEdit;

		// Token: 0x040026D3 RID: 9939
		internal UpDownBase.UpDownButtons upDownButtons;

		// Token: 0x040026D4 RID: 9940
		private bool interceptArrowKeys = UpDownBase.DefaultInterceptArrowKeys;

		// Token: 0x040026D5 RID: 9941
		private LeftRightAlignment upDownAlign = LeftRightAlignment.Right;

		// Token: 0x040026D6 RID: 9942
		private bool userEdit;

		// Token: 0x040026D7 RID: 9943
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x040026D8 RID: 9944
		private int wheelDelta;

		// Token: 0x040026D9 RID: 9945
		private bool changingText;

		// Token: 0x040026DA RID: 9946
		private bool doubleClickFired;

		// Token: 0x040026DB RID: 9947
		internal int defaultButtonsWidth = 16;

		// Token: 0x020007FA RID: 2042
		internal class UpDownEdit : TextBox
		{
			// Token: 0x06006DE4 RID: 28132 RVA: 0x00192918 File Offset: 0x00190B18
			internal UpDownEdit(UpDownBase parent)
			{
				base.SetStyle(ControlStyles.FixedWidth | ControlStyles.FixedHeight, true);
				base.SetStyle(ControlStyles.Selectable, false);
				this.parent = parent;
			}

			// Token: 0x170017CE RID: 6094
			// (get) Token: 0x06006DE5 RID: 28133 RVA: 0x0019293C File Offset: 0x00190B3C
			// (set) Token: 0x06006DE6 RID: 28134 RVA: 0x00192944 File Offset: 0x00190B44
			public override string Text
			{
				get
				{
					return base.Text;
				}
				set
				{
					bool flag = value != base.Text;
					base.Text = value;
					if (flag && AccessibilityImprovements.Level1)
					{
						base.AccessibilityNotifyClients(AccessibleEvents.NameChange, -1);
					}
				}
			}

			// Token: 0x06006DE7 RID: 28135 RVA: 0x0019297B File Offset: 0x00190B7B
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				return new UpDownBase.UpDownEdit.UpDownEditAccessibleObject(this, this.parent);
			}

			// Token: 0x06006DE8 RID: 28136 RVA: 0x00192989 File Offset: 0x00190B89
			protected override void OnMouseDown(MouseEventArgs e)
			{
				if (e.Clicks == 2 && e.Button == MouseButtons.Left)
				{
					this.doubleClickFired = true;
				}
				this.parent.OnMouseDown(this.parent.TranslateMouseEvent(this, e));
			}

			// Token: 0x06006DE9 RID: 28137 RVA: 0x001929C0 File Offset: 0x00190BC0
			protected override void OnMouseUp(MouseEventArgs e)
			{
				Point p = new Point(e.X, e.Y);
				p = base.PointToScreen(p);
				MouseEventArgs e2 = this.parent.TranslateMouseEvent(this, e);
				if (e.Button == MouseButtons.Left)
				{
					if (!this.parent.ValidationCancelled && UnsafeNativeMethods.WindowFromPoint(p.X, p.Y) == base.Handle)
					{
						if (!this.doubleClickFired)
						{
							this.parent.OnClick(e2);
							this.parent.OnMouseClick(e2);
						}
						else
						{
							this.doubleClickFired = false;
							this.parent.OnDoubleClick(e2);
							this.parent.OnMouseDoubleClick(e2);
						}
					}
					this.doubleClickFired = false;
				}
				this.parent.OnMouseUp(e2);
			}

			// Token: 0x06006DEA RID: 28138 RVA: 0x00192A84 File Offset: 0x00190C84
			internal override void WmContextMenu(ref Message m)
			{
				if (this.ContextMenu == null && this.ContextMenuStrip != null)
				{
					base.WmContextMenu(ref m, this.parent);
					return;
				}
				base.WmContextMenu(ref m, this);
			}

			// Token: 0x06006DEB RID: 28139 RVA: 0x00192AAC File Offset: 0x00190CAC
			protected override void OnKeyUp(KeyEventArgs e)
			{
				this.parent.OnKeyUp(e);
			}

			// Token: 0x06006DEC RID: 28140 RVA: 0x00192ABA File Offset: 0x00190CBA
			protected override void OnGotFocus(EventArgs e)
			{
				this.parent.SetActiveControlInternal(this);
				this.parent.InvokeGotFocus(this.parent, e);
			}

			// Token: 0x06006DED RID: 28141 RVA: 0x00192ADA File Offset: 0x00190CDA
			protected override void OnLostFocus(EventArgs e)
			{
				this.parent.InvokeLostFocus(this.parent, e);
			}

			// Token: 0x0400421A RID: 16922
			private UpDownBase parent;

			// Token: 0x0400421B RID: 16923
			private bool doubleClickFired;

			// Token: 0x020008A8 RID: 2216
			internal class UpDownEditAccessibleObject : Control.ControlAccessibleObject
			{
				// Token: 0x060070F5 RID: 28917 RVA: 0x0019CAC7 File Offset: 0x0019ACC7
				public UpDownEditAccessibleObject(UpDownBase.UpDownEdit owner, UpDownBase parent) : base(owner)
				{
					this.parent = parent;
				}

				// Token: 0x17001885 RID: 6277
				// (get) Token: 0x060070F6 RID: 28918 RVA: 0x0019CAD7 File Offset: 0x0019ACD7
				// (set) Token: 0x060070F7 RID: 28919 RVA: 0x0019CAE9 File Offset: 0x0019ACE9
				public override string Name
				{
					get
					{
						return this.parent.AccessibilityObject.Name;
					}
					set
					{
						this.parent.AccessibilityObject.Name = value;
					}
				}

				// Token: 0x17001886 RID: 6278
				// (get) Token: 0x060070F8 RID: 28920 RVA: 0x0019CAFC File Offset: 0x0019ACFC
				public override string KeyboardShortcut
				{
					get
					{
						return this.parent.AccessibilityObject.KeyboardShortcut;
					}
				}

				// Token: 0x0400441C RID: 17436
				private UpDownBase parent;
			}
		}

		// Token: 0x020007FB RID: 2043
		internal class UpDownButtons : Control
		{
			// Token: 0x06006DEE RID: 28142 RVA: 0x00192AEE File Offset: 0x00190CEE
			internal UpDownButtons(UpDownBase parent)
			{
				base.SetStyle(ControlStyles.Opaque | ControlStyles.FixedWidth | ControlStyles.FixedHeight, true);
				base.SetStyle(ControlStyles.Selectable, false);
				this.parent = parent;
			}

			// Token: 0x14000415 RID: 1045
			// (add) Token: 0x06006DEF RID: 28143 RVA: 0x00192B12 File Offset: 0x00190D12
			// (remove) Token: 0x06006DF0 RID: 28144 RVA: 0x00192B2B File Offset: 0x00190D2B
			public event UpDownEventHandler UpDown
			{
				add
				{
					this.upDownEventHandler = (UpDownEventHandler)Delegate.Combine(this.upDownEventHandler, value);
				}
				remove
				{
					this.upDownEventHandler = (UpDownEventHandler)Delegate.Remove(this.upDownEventHandler, value);
				}
			}

			// Token: 0x06006DF1 RID: 28145 RVA: 0x00192B44 File Offset: 0x00190D44
			private void BeginButtonPress(MouseEventArgs e)
			{
				int num = base.Size.Height / 2;
				if (e.Y < num)
				{
					this.pushed = (this.captured = UpDownBase.ButtonID.Up);
					base.Invalidate();
				}
				else
				{
					this.pushed = (this.captured = UpDownBase.ButtonID.Down);
					base.Invalidate();
				}
				base.CaptureInternal = true;
				this.OnUpDown(new UpDownEventArgs((int)this.pushed));
				this.StartTimer();
			}

			// Token: 0x06006DF2 RID: 28146 RVA: 0x00192BB7 File Offset: 0x00190DB7
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				return new UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject(this);
			}

			// Token: 0x06006DF3 RID: 28147 RVA: 0x00192BBF File Offset: 0x00190DBF
			private void EndButtonPress()
			{
				this.pushed = UpDownBase.ButtonID.None;
				this.captured = UpDownBase.ButtonID.None;
				this.StopTimer();
				base.CaptureInternal = false;
				base.Invalidate();
			}

			// Token: 0x06006DF4 RID: 28148 RVA: 0x00192BE4 File Offset: 0x00190DE4
			protected override void OnMouseDown(MouseEventArgs e)
			{
				this.parent.FocusInternal();
				if (!this.parent.ValidationCancelled && e.Button == MouseButtons.Left)
				{
					this.BeginButtonPress(e);
				}
				if (e.Clicks == 2 && e.Button == MouseButtons.Left)
				{
					this.doubleClickFired = true;
				}
				this.parent.OnMouseDown(this.parent.TranslateMouseEvent(this, e));
			}

			// Token: 0x06006DF5 RID: 28149 RVA: 0x00192C54 File Offset: 0x00190E54
			protected override void OnMouseMove(MouseEventArgs e)
			{
				if (base.Capture)
				{
					Rectangle clientRectangle = base.ClientRectangle;
					clientRectangle.Height /= 2;
					if (this.captured == UpDownBase.ButtonID.Down)
					{
						clientRectangle.Y += clientRectangle.Height;
					}
					if (clientRectangle.Contains(e.X, e.Y))
					{
						if (this.pushed != this.captured)
						{
							this.StartTimer();
							this.pushed = this.captured;
							base.Invalidate();
						}
					}
					else if (this.pushed != UpDownBase.ButtonID.None)
					{
						this.StopTimer();
						this.pushed = UpDownBase.ButtonID.None;
						base.Invalidate();
					}
				}
				Rectangle clientRectangle2 = base.ClientRectangle;
				Rectangle clientRectangle3 = base.ClientRectangle;
				clientRectangle2.Height /= 2;
				clientRectangle3.Y += clientRectangle3.Height / 2;
				if (clientRectangle2.Contains(e.X, e.Y))
				{
					this.mouseOver = UpDownBase.ButtonID.Up;
					base.Invalidate();
				}
				else if (clientRectangle3.Contains(e.X, e.Y))
				{
					this.mouseOver = UpDownBase.ButtonID.Down;
					base.Invalidate();
				}
				this.parent.OnMouseMove(this.parent.TranslateMouseEvent(this, e));
			}

			// Token: 0x06006DF6 RID: 28150 RVA: 0x00192D8C File Offset: 0x00190F8C
			protected override void OnMouseUp(MouseEventArgs e)
			{
				if (!this.parent.ValidationCancelled && e.Button == MouseButtons.Left)
				{
					this.EndButtonPress();
				}
				Point p = new Point(e.X, e.Y);
				p = base.PointToScreen(p);
				MouseEventArgs e2 = this.parent.TranslateMouseEvent(this, e);
				if (e.Button == MouseButtons.Left)
				{
					if (!this.parent.ValidationCancelled && UnsafeNativeMethods.WindowFromPoint(p.X, p.Y) == base.Handle)
					{
						if (!this.doubleClickFired)
						{
							this.parent.OnClick(e2);
						}
						else
						{
							this.doubleClickFired = false;
							this.parent.OnDoubleClick(e2);
							this.parent.OnMouseDoubleClick(e2);
						}
					}
					this.doubleClickFired = false;
				}
				this.parent.OnMouseUp(e2);
			}

			// Token: 0x06006DF7 RID: 28151 RVA: 0x00192E64 File Offset: 0x00191064
			protected override void OnMouseLeave(EventArgs e)
			{
				this.mouseOver = UpDownBase.ButtonID.None;
				base.Invalidate();
				this.parent.OnMouseLeave(e);
			}

			// Token: 0x06006DF8 RID: 28152 RVA: 0x00192E80 File Offset: 0x00191080
			protected override void OnPaint(PaintEventArgs e)
			{
				int num = base.ClientSize.Height / 2;
				if (Application.RenderWithVisualStyles)
				{
					VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer((this.mouseOver == UpDownBase.ButtonID.Up) ? VisualStyleElement.Spin.Up.Hot : VisualStyleElement.Spin.Up.Normal);
					if (!base.Enabled)
					{
						visualStyleRenderer.SetParameters(VisualStyleElement.Spin.Up.Disabled);
					}
					else if (this.pushed == UpDownBase.ButtonID.Up)
					{
						visualStyleRenderer.SetParameters(VisualStyleElement.Spin.Up.Pressed);
					}
					visualStyleRenderer.DrawBackground(e.Graphics, new Rectangle(0, 0, this.parent.defaultButtonsWidth, num), base.HandleInternal);
					if (!base.Enabled)
					{
						visualStyleRenderer.SetParameters(VisualStyleElement.Spin.Down.Disabled);
					}
					else if (this.pushed == UpDownBase.ButtonID.Down)
					{
						visualStyleRenderer.SetParameters(VisualStyleElement.Spin.Down.Pressed);
					}
					else
					{
						visualStyleRenderer.SetParameters((this.mouseOver == UpDownBase.ButtonID.Down) ? VisualStyleElement.Spin.Down.Hot : VisualStyleElement.Spin.Down.Normal);
					}
					visualStyleRenderer.DrawBackground(e.Graphics, new Rectangle(0, num, this.parent.defaultButtonsWidth, num), base.HandleInternal);
				}
				else
				{
					ControlPaint.DrawScrollButton(e.Graphics, new Rectangle(0, 0, this.parent.defaultButtonsWidth, num), ScrollButton.Up, (this.pushed == UpDownBase.ButtonID.Up) ? ButtonState.Pushed : (base.Enabled ? ButtonState.Normal : ButtonState.Inactive));
					ControlPaint.DrawScrollButton(e.Graphics, new Rectangle(0, num, this.parent.defaultButtonsWidth, num), ScrollButton.Down, (this.pushed == UpDownBase.ButtonID.Down) ? ButtonState.Pushed : (base.Enabled ? ButtonState.Normal : ButtonState.Inactive));
				}
				if (num != (base.ClientSize.Height + 1) / 2)
				{
					using (Pen pen = new Pen(this.parent.BackColor))
					{
						Rectangle clientRectangle = base.ClientRectangle;
						e.Graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Bottom - 1, clientRectangle.Right - 1, clientRectangle.Bottom - 1);
					}
				}
				base.OnPaint(e);
			}

			// Token: 0x06006DF9 RID: 28153 RVA: 0x0019307C File Offset: 0x0019127C
			protected virtual void OnUpDown(UpDownEventArgs upevent)
			{
				if (this.upDownEventHandler != null)
				{
					this.upDownEventHandler(this, upevent);
				}
			}

			// Token: 0x06006DFA RID: 28154 RVA: 0x00193094 File Offset: 0x00191294
			protected void StartTimer()
			{
				this.parent.OnStartTimer();
				if (this.timer == null)
				{
					this.timer = new Timer();
					this.timer.Tick += this.TimerHandler;
				}
				this.timerInterval = 500;
				this.timer.Interval = this.timerInterval;
				this.timer.Start();
			}

			// Token: 0x06006DFB RID: 28155 RVA: 0x001930FD File Offset: 0x001912FD
			protected void StopTimer()
			{
				if (this.timer != null)
				{
					this.timer.Stop();
					this.timer.Dispose();
					this.timer = null;
				}
				this.parent.OnStopTimer();
			}

			// Token: 0x06006DFC RID: 28156 RVA: 0x00193130 File Offset: 0x00191330
			private void TimerHandler(object source, EventArgs args)
			{
				if (!base.Capture)
				{
					this.EndButtonPress();
					return;
				}
				this.OnUpDown(new UpDownEventArgs((int)this.pushed));
				if (this.timer != null)
				{
					this.timerInterval *= 7;
					this.timerInterval /= 10;
					if (this.timerInterval < 1)
					{
						this.timerInterval = 1;
					}
					this.timer.Interval = this.timerInterval;
				}
			}

			// Token: 0x0400421C RID: 16924
			private UpDownBase parent;

			// Token: 0x0400421D RID: 16925
			private UpDownBase.ButtonID pushed;

			// Token: 0x0400421E RID: 16926
			private UpDownBase.ButtonID captured;

			// Token: 0x0400421F RID: 16927
			private UpDownBase.ButtonID mouseOver;

			// Token: 0x04004220 RID: 16928
			private UpDownEventHandler upDownEventHandler;

			// Token: 0x04004221 RID: 16929
			private Timer timer;

			// Token: 0x04004222 RID: 16930
			private int timerInterval;

			// Token: 0x04004223 RID: 16931
			private bool doubleClickFired;

			// Token: 0x020008A9 RID: 2217
			internal class UpDownButtonsAccessibleObject : Control.ControlAccessibleObject
			{
				// Token: 0x060070F9 RID: 28921 RVA: 0x00093572 File Offset: 0x00091772
				public UpDownButtonsAccessibleObject(UpDownBase.UpDownButtons owner) : base(owner)
				{
				}

				// Token: 0x17001887 RID: 6279
				// (get) Token: 0x060070FA RID: 28922 RVA: 0x0019CB10 File Offset: 0x0019AD10
				// (set) Token: 0x060070FB RID: 28923 RVA: 0x000A0504 File Offset: 0x0009E704
				public override string Name
				{
					get
					{
						string name = base.Name;
						if (name != null && name.Length != 0)
						{
							return name;
						}
						if (AccessibilityImprovements.Level3)
						{
							return base.Owner.ParentInternal.GetType().Name;
						}
						return SR.GetString("SpinnerAccessibleName");
					}
					set
					{
						base.Name = value;
					}
				}

				// Token: 0x17001888 RID: 6280
				// (get) Token: 0x060070FC RID: 28924 RVA: 0x0019CB58 File Offset: 0x0019AD58
				public override AccessibleRole Role
				{
					get
					{
						AccessibleRole accessibleRole = base.Owner.AccessibleRole;
						if (accessibleRole != AccessibleRole.Default)
						{
							return accessibleRole;
						}
						return AccessibleRole.SpinButton;
					}
				}

				// Token: 0x17001889 RID: 6281
				// (get) Token: 0x060070FD RID: 28925 RVA: 0x0019CB79 File Offset: 0x0019AD79
				private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject UpButton
				{
					get
					{
						if (this.upButton == null)
						{
							this.upButton = new UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject(this, true);
						}
						return this.upButton;
					}
				}

				// Token: 0x1700188A RID: 6282
				// (get) Token: 0x060070FE RID: 28926 RVA: 0x0019CB96 File Offset: 0x0019AD96
				private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject DownButton
				{
					get
					{
						if (this.downButton == null)
						{
							this.downButton = new UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject(this, false);
						}
						return this.downButton;
					}
				}

				// Token: 0x060070FF RID: 28927 RVA: 0x0019CBB3 File Offset: 0x0019ADB3
				public override AccessibleObject GetChild(int index)
				{
					if (index == 0)
					{
						return this.UpButton;
					}
					if (index == 1)
					{
						return this.DownButton;
					}
					return null;
				}

				// Token: 0x06007100 RID: 28928 RVA: 0x0000E211 File Offset: 0x0000C411
				public override int GetChildCount()
				{
					return 2;
				}

				// Token: 0x0400441D RID: 17437
				private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject upButton;

				// Token: 0x0400441E RID: 17438
				private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject.DirectionButtonAccessibleObject downButton;

				// Token: 0x0200095D RID: 2397
				internal class DirectionButtonAccessibleObject : AccessibleObject
				{
					// Token: 0x06007381 RID: 29569 RVA: 0x001A141C File Offset: 0x0019F61C
					public DirectionButtonAccessibleObject(UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject parent, bool up)
					{
						this.parent = parent;
						this.up = up;
					}

					// Token: 0x17001A49 RID: 6729
					// (get) Token: 0x06007382 RID: 29570 RVA: 0x001A1434 File Offset: 0x0019F634
					public override Rectangle Bounds
					{
						get
						{
							Rectangle bounds = ((UpDownBase.UpDownButtons)this.parent.Owner).Bounds;
							bounds.Height /= 2;
							if (!this.up)
							{
								bounds.Y += bounds.Height;
							}
							return ((UpDownBase.UpDownButtons)this.parent.Owner).ParentInternal.RectangleToScreen(bounds);
						}
					}

					// Token: 0x17001A4A RID: 6730
					// (get) Token: 0x06007383 RID: 29571 RVA: 0x001A149E File Offset: 0x0019F69E
					// (set) Token: 0x06007384 RID: 29572 RVA: 0x0000701A File Offset: 0x0000521A
					public override string Name
					{
						get
						{
							if (this.up)
							{
								return SR.GetString("UpDownBaseUpButtonAccName");
							}
							return SR.GetString("UpDownBaseDownButtonAccName");
						}
						set
						{
						}
					}

					// Token: 0x17001A4B RID: 6731
					// (get) Token: 0x06007385 RID: 29573 RVA: 0x001A14BD File Offset: 0x0019F6BD
					public override AccessibleObject Parent
					{
						[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
						get
						{
							return this.parent;
						}
					}

					// Token: 0x17001A4C RID: 6732
					// (get) Token: 0x06007386 RID: 29574 RVA: 0x0013D791 File Offset: 0x0013B991
					public override AccessibleRole Role
					{
						get
						{
							return AccessibleRole.PushButton;
						}
					}

					// Token: 0x04004694 RID: 18068
					private bool up;

					// Token: 0x04004695 RID: 18069
					private UpDownBase.UpDownButtons.UpDownButtonsAccessibleObject parent;
				}
			}
		}

		// Token: 0x020007FC RID: 2044
		internal enum ButtonID
		{
			// Token: 0x04004225 RID: 16933
			None,
			// Token: 0x04004226 RID: 16934
			Up,
			// Token: 0x04004227 RID: 16935
			Down
		}
	}
}
