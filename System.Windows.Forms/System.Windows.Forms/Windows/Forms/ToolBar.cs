using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows toolbar. Although <see cref="T:System.Windows.Forms.ToolStrip" /> replaces and adds functionality to the <see cref="T:System.Windows.Forms.ToolBar" /> control of previous versions, <see cref="T:System.Windows.Forms.ToolBar" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x02000398 RID: 920
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("ButtonClick")]
	[Designer("System.Windows.Forms.Design.ToolBarDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Buttons")]
	public class ToolBar : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolBar" /> class.</summary>
		// Token: 0x06003A25 RID: 14885 RVA: 0x00102940 File Offset: 0x00100B40
		public ToolBar()
		{
			this.toolBarState = new BitVector32(31);
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.FixedHeight, this.AutoSize);
			base.SetStyle(ControlStyles.FixedWidth, false);
			this.TabStop = false;
			this.Dock = DockStyle.Top;
			this.buttonsCollection = new ToolBar.ToolBarButtonCollection(this);
		}

		/// <summary>Gets or set the value that determines the appearance of a toolbar control and its buttons.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolBarAppearance" /> values. The default is <see langword="ToolBarAppearance.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ToolBarAppearance" /> values. </exception>
		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x001029C8 File Offset: 0x00100BC8
		// (set) Token: 0x06003A27 RID: 14887 RVA: 0x001029D0 File Offset: 0x00100BD0
		[SRCategory("CatBehavior")]
		[DefaultValue(ToolBarAppearance.Normal)]
		[Localizable(true)]
		[SRDescription("ToolBarAppearanceDescr")]
		public ToolBarAppearance Appearance
		{
			get
			{
				return this.appearance;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolBarAppearance));
				}
				if (value != this.appearance)
				{
					this.appearance = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the toolbar adjusts its size automatically, based on the size of the buttons and the dock style.</summary>
		/// <returns>
		///     <see langword="true" /> if the toolbar adjusts its size automatically, based on the size of the buttons and dock style; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x00102A0E File Offset: 0x00100C0E
		// (set) Token: 0x06003A29 RID: 14889 RVA: 0x00102A20 File Offset: 0x00100C20
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ToolBarAutoSizeDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return this.toolBarState[16];
			}
			set
			{
				if (this.AutoSize != value)
				{
					this.toolBarState[16] = value;
					if (this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
					{
						base.SetStyle(ControlStyles.FixedWidth, this.AutoSize);
						base.SetStyle(ControlStyles.FixedHeight, false);
					}
					else
					{
						base.SetStyle(ControlStyles.FixedHeight, this.AutoSize);
						base.SetStyle(ControlStyles.FixedWidth, false);
					}
					this.AdjustSize(this.Dock);
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolBar.AutoSize" /> property has changed.</summary>
		// Token: 0x140002DA RID: 730
		// (add) Token: 0x06003A2A RID: 14890 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x06003A2B RID: 14891 RVA: 0x0001BA37 File Offset: 0x00019C37
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

		/// <summary>Gets or sets the background color.</summary>
		/// <returns>The background color.</returns>
		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06003A2C RID: 14892 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x06003A2D RID: 14893 RVA: 0x00011FB9 File Offset: 0x000101B9
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolBar.BackColor" /> property changes.</summary>
		// Token: 0x140002DB RID: 731
		// (add) Token: 0x06003A2E RID: 14894 RVA: 0x00050A7A File Offset: 0x0004EC7A
		// (remove) Token: 0x06003A2F RID: 14895 RVA: 0x00050A83 File Offset: 0x0004EC83
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

		/// <summary>Gets or sets the background image.</summary>
		/// <returns>The background image.</returns>
		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06003A30 RID: 14896 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06003A31 RID: 14897 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolBar.BackgroundImage" /> property changes.</summary>
		// Token: 0x140002DC RID: 732
		// (add) Token: 0x06003A32 RID: 14898 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06003A33 RID: 14899 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>Gets or sets the layout for background image.</summary>
		/// <returns>The layout for background image.</returns>
		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06003A34 RID: 14900 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06003A35 RID: 14901 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolBar.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140002DD RID: 733
		// (add) Token: 0x06003A36 RID: 14902 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06003A37 RID: 14903 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets the border style of the toolbar control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see langword="BorderStyle.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06003A38 RID: 14904 RVA: 0x00102A9D File Offset: 0x00100C9D
		// (set) Token: 0x06003A39 RID: 14905 RVA: 0x00102AA5 File Offset: 0x00100CA5
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.None)]
		[DispId(-504)]
		[SRDescription("ToolBarBorderStyleDescr")]
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

		/// <summary>Gets the collection of <see cref="T:System.Windows.Forms.ToolBarButton" /> controls assigned to the toolbar control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolBar.ToolBarButtonCollection" /> that contains a collection of <see cref="T:System.Windows.Forms.ToolBarButton" /> controls.</returns>
		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06003A3A RID: 14906 RVA: 0x00102AE3 File Offset: 0x00100CE3
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ToolBarButtonsDescr")]
		[MergableProperty(false)]
		public ToolBar.ToolBarButtonCollection Buttons
		{
			get
			{
				return this.buttonsCollection;
			}
		}

		/// <summary>Gets or sets the size of the buttons on the toolbar control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> object that represents the size of the <see cref="T:System.Windows.Forms.ToolBarButton" /> controls on the toolbar. The default size has a width of 24 pixels and a height of 22 pixels, or large enough to accommodate the <see cref="T:System.Drawing.Image" /> and text, whichever is greater.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Size.Width" /> or <see cref="P:System.Drawing.Size.Height" /> property of the <see cref="T:System.Drawing.Size" /> object is less than 0. </exception>
		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06003A3B RID: 14907 RVA: 0x00102AEC File Offset: 0x00100CEC
		// (set) Token: 0x06003A3C RID: 14908 RVA: 0x00102B6C File Offset: 0x00100D6C
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.All)]
		[Localizable(true)]
		[SRDescription("ToolBarButtonSizeDescr")]
		public Size ButtonSize
		{
			get
			{
				if (!this.buttonSize.IsEmpty)
				{
					return this.buttonSize;
				}
				if (base.IsHandleCreated && this.buttons != null && this.buttonCount > 0)
				{
					int num = (int)((long)base.SendMessage(1082, 0, 0));
					if (num > 0)
					{
						return new Size(NativeMethods.Util.LOWORD(num), NativeMethods.Util.HIWORD(num));
					}
				}
				if (this.TextAlign == ToolBarTextAlign.Underneath)
				{
					return new Size(39, 36);
				}
				return new Size(23, 22);
			}
			set
			{
				if (value.Width < 0 || value.Height < 0)
				{
					throw new ArgumentOutOfRangeException("ButtonSize", SR.GetString("InvalidArgument", new object[]
					{
						"ButtonSize",
						value.ToString()
					}));
				}
				if (this.buttonSize != value)
				{
					this.buttonSize = value;
					this.maxWidth = -1;
					base.RecreateHandle();
					this.AdjustSize(this.Dock);
				}
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06003A3D RID: 14909 RVA: 0x00102BF0 File Offset: 0x00100DF0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "ToolbarWindow32";
				createParams.Style |= 12;
				if (!this.Divider)
				{
					createParams.Style |= 64;
				}
				if (this.Wrappable)
				{
					createParams.Style |= 512;
				}
				if (this.ShowToolTips && !base.DesignMode)
				{
					createParams.Style |= 256;
				}
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
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
				ToolBarAppearance toolBarAppearance = this.appearance;
				if (toolBarAppearance != ToolBarAppearance.Normal && toolBarAppearance == ToolBarAppearance.Flat)
				{
					createParams.Style |= 2048;
				}
				ToolBarTextAlign toolBarTextAlign = this.textAlign;
				if (toolBarTextAlign != ToolBarTextAlign.Underneath && toolBarTextAlign == ToolBarTextAlign.Right)
				{
					createParams.Style |= 4096;
				}
				return createParams;
			}
		}

		/// <summary>Gets the default Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06003A3E RID: 14910 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06003A3F RID: 14911 RVA: 0x000F782D File Offset: 0x000F5A2D
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 22);
			}
		}

		/// <summary>Gets or sets a value indicating whether the toolbar displays a divider.</summary>
		/// <returns>
		///     <see langword="true" /> if the toolbar displays a divider; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06003A40 RID: 14912 RVA: 0x00102D0B File Offset: 0x00100F0B
		// (set) Token: 0x06003A41 RID: 14913 RVA: 0x00102D19 File Offset: 0x00100F19
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		[SRDescription("ToolBarDividerDescr")]
		public bool Divider
		{
			get
			{
				return this.toolBarState[4];
			}
			set
			{
				if (this.Divider != value)
				{
					this.toolBarState[4] = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets which control borders are docked to its parent control and determines how a control is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see cref="F:System.Windows.Forms.DockStyle.None" />.</returns>
		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06003A42 RID: 14914 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x06003A43 RID: 14915 RVA: 0x00102D38 File Offset: 0x00100F38
		[Localizable(true)]
		[DefaultValue(DockStyle.Top)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DockStyle));
				}
				if (this.Dock != value)
				{
					if (value == DockStyle.Left || value == DockStyle.Right)
					{
						base.SetStyle(ControlStyles.FixedWidth, this.AutoSize);
						base.SetStyle(ControlStyles.FixedHeight, false);
					}
					else
					{
						base.SetStyle(ControlStyles.FixedHeight, this.AutoSize);
						base.SetStyle(ControlStyles.FixedWidth, false);
					}
					this.AdjustSize(value);
					base.Dock = value;
				}
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value.</returns>
		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x06003A45 RID: 14917 RVA: 0x000A2CBA File Offset: 0x000A0EBA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether drop-down buttons on a toolbar display down arrows.</summary>
		/// <returns>
		///     <see langword="true" /> if drop-down toolbar buttons display down arrows; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06003A46 RID: 14918 RVA: 0x00102DBA File Offset: 0x00100FBA
		// (set) Token: 0x06003A47 RID: 14919 RVA: 0x00102DC8 File Offset: 0x00100FC8
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ToolBarDropDownArrowsDescr")]
		public bool DropDownArrows
		{
			get
			{
				return this.toolBarState[2];
			}
			set
			{
				if (this.DropDownArrows != value)
				{
					this.toolBarState[2] = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets the forecolor .</summary>
		/// <returns>The forecolor.</returns>
		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06003A48 RID: 14920 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x06003A49 RID: 14921 RVA: 0x0001208A File Offset: 0x0001028A
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolBar.ForeColor" /> property changes.</summary>
		// Token: 0x140002DE RID: 734
		// (add) Token: 0x06003A4A RID: 14922 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x06003A4B RID: 14923 RVA: 0x0005276F File Offset: 0x0005096F
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

		/// <summary>Gets or sets the collection of images available to the toolbar button controls.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageList" /> that contains images available to the <see cref="T:System.Windows.Forms.ToolBarButton" /> controls. The default is <see langword="null" />.</returns>
		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06003A4C RID: 14924 RVA: 0x00102DE6 File Offset: 0x00100FE6
		// (set) Token: 0x06003A4D RID: 14925 RVA: 0x00102DF0 File Offset: 0x00100FF0
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("ToolBarImageListDescr")]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (value != this.imageList)
				{
					EventHandler value2 = new EventHandler(this.ImageListRecreateHandle);
					EventHandler value3 = new EventHandler(this.DetachImageList);
					if (this.imageList != null)
					{
						this.imageList.Disposed -= value3;
						this.imageList.RecreateHandle -= value2;
					}
					this.imageList = value;
					if (value != null)
					{
						value.Disposed += value3;
						value.RecreateHandle += value2;
					}
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets the size of the images in the image list assigned to the toolbar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the images (in the <see cref="T:System.Windows.Forms.ImageList" />) assigned to the <see cref="T:System.Windows.Forms.ToolBar" />.</returns>
		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06003A4E RID: 14926 RVA: 0x00102E66 File Offset: 0x00101066
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ToolBarImageSizeDescr")]
		public Size ImageSize
		{
			get
			{
				if (this.imageList != null)
				{
					return this.imageList.ImageSize;
				}
				return new Size(0, 0);
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06003A4F RID: 14927 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06003A50 RID: 14928 RVA: 0x00011FEC File Offset: 0x000101EC
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolBar.ImeMode" /> property changes.</summary>
		// Token: 0x140002DF RID: 735
		// (add) Token: 0x06003A51 RID: 14929 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x06003A52 RID: 14930 RVA: 0x0001BF35 File Offset: 0x0001A135
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

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06003A53 RID: 14931 RVA: 0x00102E84 File Offset: 0x00101084
		internal int PreferredHeight
		{
			get
			{
				int num;
				if (this.buttons == null || this.buttonCount == 0 || !base.IsHandleCreated)
				{
					num = this.ButtonSize.Height;
				}
				else
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					int num2 = 0;
					while (num2 < this.buttons.Length && (this.buttons[num2] == null || !this.buttons[num2].Visible))
					{
						num2++;
					}
					if (num2 == this.buttons.Length)
					{
						num2 = 0;
					}
					base.SendMessage(1075, num2, ref rect);
					num = rect.bottom - rect.top;
				}
				if (this.Wrappable && base.IsHandleCreated)
				{
					num *= (int)((long)base.SendMessage(1064, 0, 0));
				}
				num = ((num > 0) ? num : 1);
				BorderStyle borderStyle = this.borderStyle;
				if (borderStyle != BorderStyle.FixedSingle)
				{
					if (borderStyle == BorderStyle.Fixed3D)
					{
						num += SystemInformation.Border3DSize.Height;
					}
				}
				else
				{
					num += SystemInformation.BorderSize.Height;
				}
				if (this.Divider)
				{
					num += 2;
				}
				return num + 4;
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06003A54 RID: 14932 RVA: 0x00102F90 File Offset: 0x00101190
		internal int PreferredWidth
		{
			get
			{
				if (this.maxWidth == -1)
				{
					if (!base.IsHandleCreated || this.buttons == null)
					{
						this.maxWidth = this.ButtonSize.Width;
					}
					else
					{
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						for (int i = 0; i < this.buttonCount; i++)
						{
							base.SendMessage(1075, 0, ref rect);
							if (rect.right - rect.left > this.maxWidth)
							{
								this.maxWidth = rect.right - rect.left;
							}
						}
					}
				}
				int num = this.maxWidth;
				if (this.borderStyle != BorderStyle.None)
				{
					num += SystemInformation.BorderSize.Height * 4 + 3;
				}
				return num;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.RightToLeft" /> value.</returns>
		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06003A55 RID: 14933 RVA: 0x000DAB7B File Offset: 0x000D8D7B
		// (set) Token: 0x06003A56 RID: 14934 RVA: 0x000BDC35 File Offset: 0x000BBE35
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolBar.RightToLeft" /> property changes.</summary>
		// Token: 0x140002E0 RID: 736
		// (add) Token: 0x06003A57 RID: 14935 RVA: 0x000DAB83 File Offset: 0x000D8D83
		// (remove) Token: 0x06003A58 RID: 14936 RVA: 0x000DAB8C File Offset: 0x000D8D8C
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

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="dx">The horizontal scaling factor.</param>
		/// <param name="dy">The vertical scaling factor.</param>
		// Token: 0x06003A59 RID: 14937 RVA: 0x00103040 File Offset: 0x00101240
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			this.currentScaleDX = dx;
			this.currentScaleDY = dy;
			base.ScaleCore(dx, dy);
			this.UpdateButtons();
		}

		/// <summary>Scales a control's location, size, padding and margin.</summary>
		/// <param name="factor">The factor by which the height and width of the control will be scaled.</param>
		/// <param name="specified">A <see cref="T:System.Windows.Forms.BoundsSpecified" /> value that specifies the bounds of the control to use when defining its size and position.</param>
		// Token: 0x06003A5A RID: 14938 RVA: 0x0010305E File Offset: 0x0010125E
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			this.currentScaleDX = factor.Width;
			this.currentScaleDY = factor.Height;
			base.ScaleControl(factor, specified);
		}

		/// <summary>Gets or sets a value indicating whether the toolbar displays a ToolTip for each button.</summary>
		/// <returns>
		///     <see langword="true" /> if the toolbar display a ToolTip for each button; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x00103082 File Offset: 0x00101282
		// (set) Token: 0x06003A5C RID: 14940 RVA: 0x00103090 File Offset: 0x00101290
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("ToolBarShowToolTipsDescr")]
		public bool ShowToolTips
		{
			get
			{
				return this.toolBarState[8];
			}
			set
			{
				if (this.ShowToolTips != value)
				{
					this.toolBarState[8] = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>This property is not meaningful for this control.</summary>
		/// <returns>This property is not meaningful for this control.</returns>
		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06003A5D RID: 14941 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06003A5E RID: 14942 RVA: 0x000AA11D File Offset: 0x000A831D
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

		/// <summary>Gets or sets the text for the toolbar.</summary>
		/// <returns>The text for the toolbar.</returns>
		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06003A5F RID: 14943 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06003A60 RID: 14944 RVA: 0x0001BFAD File Offset: 0x0001A1AD
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolBar.Text" /> property changes.</summary>
		// Token: 0x140002E1 RID: 737
		// (add) Token: 0x06003A61 RID: 14945 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06003A62 RID: 14946 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Gets or sets the alignment of text in relation to each image displayed on the toolbar button controls.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolBarTextAlign" /> values. The default is <see langword="ToolBarTextAlign.Underneath" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ToolBarTextAlign" /> values. </exception>
		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06003A63 RID: 14947 RVA: 0x001030AE File Offset: 0x001012AE
		// (set) Token: 0x06003A64 RID: 14948 RVA: 0x001030B6 File Offset: 0x001012B6
		[SRCategory("CatAppearance")]
		[DefaultValue(ToolBarTextAlign.Underneath)]
		[Localizable(true)]
		[SRDescription("ToolBarTextAlignDescr")]
		public ToolBarTextAlign TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolBarTextAlign));
				}
				if (this.textAlign == value)
				{
					return;
				}
				this.textAlign = value;
				base.RecreateHandle();
			}
		}

		/// <summary>Gets or sets a value indicating whether the toolbar buttons wrap to the next line if the toolbar becomes too small to display all the buttons on the same line.</summary>
		/// <returns>
		///     <see langword="true" /> if the toolbar buttons wrap to another line if the toolbar becomes too small to display all the buttons on the same line; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06003A65 RID: 14949 RVA: 0x001030F5 File Offset: 0x001012F5
		// (set) Token: 0x06003A66 RID: 14950 RVA: 0x00103103 File Offset: 0x00101303
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ToolBarWrappableDescr")]
		public bool Wrappable
		{
			get
			{
				return this.toolBarState[1];
			}
			set
			{
				if (this.Wrappable != value)
				{
					this.toolBarState[1] = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Forms.ToolBarButton" /> on the <see cref="T:System.Windows.Forms.ToolBar" /> is clicked.</summary>
		// Token: 0x140002E2 RID: 738
		// (add) Token: 0x06003A67 RID: 14951 RVA: 0x00103121 File Offset: 0x00101321
		// (remove) Token: 0x06003A68 RID: 14952 RVA: 0x0010313A File Offset: 0x0010133A
		[SRCategory("CatBehavior")]
		[SRDescription("ToolBarButtonClickDescr")]
		public event ToolBarButtonClickEventHandler ButtonClick
		{
			add
			{
				this.onButtonClick = (ToolBarButtonClickEventHandler)Delegate.Combine(this.onButtonClick, value);
			}
			remove
			{
				this.onButtonClick = (ToolBarButtonClickEventHandler)Delegate.Remove(this.onButtonClick, value);
			}
		}

		/// <summary>Occurs when a drop-down style <see cref="T:System.Windows.Forms.ToolBarButton" /> or its down arrow is clicked.</summary>
		// Token: 0x140002E3 RID: 739
		// (add) Token: 0x06003A69 RID: 14953 RVA: 0x00103153 File Offset: 0x00101353
		// (remove) Token: 0x06003A6A RID: 14954 RVA: 0x0010316C File Offset: 0x0010136C
		[SRCategory("CatBehavior")]
		[SRDescription("ToolBarButtonDropDownDescr")]
		public event ToolBarButtonClickEventHandler ButtonDropDown
		{
			add
			{
				this.onButtonDropDown = (ToolBarButtonClickEventHandler)Delegate.Combine(this.onButtonDropDown, value);
			}
			remove
			{
				this.onButtonDropDown = (ToolBarButtonClickEventHandler)Delegate.Remove(this.onButtonDropDown, value);
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		// Token: 0x140002E4 RID: 740
		// (add) Token: 0x06003A6B RID: 14955 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x06003A6C RID: 14956 RVA: 0x00020D40 File Offset: 0x0001EF40
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

		// Token: 0x06003A6D RID: 14957 RVA: 0x00103188 File Offset: 0x00101388
		private void AdjustSize(DockStyle dock)
		{
			int num = this.requestedSize;
			try
			{
				if (dock == DockStyle.Left || dock == DockStyle.Right)
				{
					base.Width = (this.AutoSize ? this.PreferredWidth : num);
				}
				else
				{
					base.Height = (this.AutoSize ? this.PreferredHeight : num);
				}
			}
			finally
			{
				this.requestedSize = num;
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x000FB8A5 File Offset: 0x000F9AA5
		internal void BeginUpdate()
		{
			base.BeginUpdateInternal();
		}

		/// <summary>Creates a handle for the control.</summary>
		// Token: 0x06003A6F RID: 14959 RVA: 0x001031F0 File Offset: 0x001013F0
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 4
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x00103240 File Offset: 0x00101440
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ToolBar" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06003A71 RID: 14961 RVA: 0x0010324C File Offset: 0x0010144C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this)
				{
					bool state = base.GetState(4096);
					try
					{
						base.SetState(4096, true);
						if (this.imageList != null)
						{
							this.imageList.Disposed -= this.DetachImageList;
							this.imageList = null;
						}
						if (this.buttonsCollection != null)
						{
							ToolBarButton[] array = new ToolBarButton[this.buttonsCollection.Count];
							((ICollection)this.buttonsCollection).CopyTo(array, 0);
							this.buttonsCollection.Clear();
							foreach (ToolBarButton toolBarButton in array)
							{
								toolBarButton.Dispose();
							}
						}
					}
					finally
					{
						base.SetState(4096, state);
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x0010333C File Offset: 0x0010153C
		internal void EndUpdate()
		{
			base.EndUpdateInternal();
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00103348 File Offset: 0x00101548
		private void ForceButtonWidths()
		{
			if (this.buttons != null && this.buttonSize.IsEmpty && base.IsHandleCreated)
			{
				this.maxWidth = -1;
				for (int i = 0; i < this.buttonCount; i++)
				{
					NativeMethods.TBBUTTONINFO tbbuttoninfo = new NativeMethods.TBBUTTONINFO
					{
						cbSize = Marshal.SizeOf(typeof(NativeMethods.TBBUTTONINFO)),
						cx = this.buttons[i].Width
					};
					if ((int)tbbuttoninfo.cx > this.maxWidth)
					{
						this.maxWidth = (int)tbbuttoninfo.cx;
					}
					tbbuttoninfo.dwMask = 64;
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TB_SETBUTTONINFO, i, ref tbbuttoninfo);
				}
			}
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00103402 File Offset: 0x00101602
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00103414 File Offset: 0x00101614
		private void Insert(int index, ToolBarButton button)
		{
			button.parent = this;
			if (this.buttons == null)
			{
				this.buttons = new ToolBarButton[4];
			}
			else if (this.buttons.Length == this.buttonCount)
			{
				ToolBarButton[] destinationArray = new ToolBarButton[this.buttonCount + 4];
				Array.Copy(this.buttons, 0, destinationArray, 0, this.buttonCount);
				this.buttons = destinationArray;
			}
			if (index < this.buttonCount)
			{
				Array.Copy(this.buttons, index, this.buttons, index + 1, this.buttonCount - index);
			}
			this.buttons[index] = button;
			this.buttonCount++;
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x001034B4 File Offset: 0x001016B4
		private void InsertButton(int index, ToolBarButton value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (index < 0 || (this.buttons != null && index > this.buttonCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.Insert(index, value);
			if (base.IsHandleCreated)
			{
				NativeMethods.TBBUTTON tbbutton = value.GetTBBUTTON(index);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TB_INSERTBUTTON, index, ref tbbutton);
			}
			this.UpdateButtons();
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00103550 File Offset: 0x00101750
		private int InternalAddButton(ToolBarButton button)
		{
			if (button == null)
			{
				throw new ArgumentNullException("button");
			}
			int num = this.buttonCount;
			this.Insert(num, button);
			return num;
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x0010357C File Offset: 0x0010177C
		internal void InternalSetButton(int index, ToolBarButton value, bool recreate, bool updateText)
		{
			this.buttons[index].parent = null;
			this.buttons[index].stringIndex = (IntPtr)(-1);
			this.buttons[index] = value;
			this.buttons[index].parent = this;
			if (base.IsHandleCreated)
			{
				NativeMethods.TBBUTTONINFO tbbuttoninfo = value.GetTBBUTTONINFO(updateText, index);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TB_SETBUTTONINFO, index, ref tbbuttoninfo);
				if (tbbuttoninfo.pszText != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(tbbuttoninfo.pszText);
				}
				if (recreate)
				{
					this.UpdateButtons();
					return;
				}
				base.SendMessage(1057, 0, 0);
				this.ForceButtonWidths();
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolBar.ButtonClick" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolBarButtonClickEventArgs" /> that contains the event data. </param>
		// Token: 0x06003A79 RID: 14969 RVA: 0x0010362E File Offset: 0x0010182E
		protected virtual void OnButtonClick(ToolBarButtonClickEventArgs e)
		{
			if (this.onButtonClick != null)
			{
				this.onButtonClick(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolBar.ButtonDropDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ToolBarButtonClickEventArgs" /> that contains the event data. </param>
		// Token: 0x06003A7A RID: 14970 RVA: 0x00103645 File Offset: 0x00101845
		protected virtual void OnButtonDropDown(ToolBarButtonClickEventArgs e)
		{
			if (this.onButtonDropDown != null)
			{
				this.onButtonDropDown(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003A7B RID: 14971 RVA: 0x0010365C File Offset: 0x0010185C
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(1054, Marshal.SizeOf(typeof(NativeMethods.TBBUTTON)), 0);
			if (this.DropDownArrows)
			{
				base.SendMessage(1108, 0, 1);
			}
			if (this.imageList != null)
			{
				base.SendMessage(1072, 0, this.imageList.Handle);
			}
			this.RealizeButtons();
			this.BeginUpdate();
			try
			{
				Size size = base.Size;
				base.Size = new Size(size.Width + 1, size.Height);
				base.Size = size;
			}
			finally
			{
				this.EndUpdate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003A7C RID: 14972 RVA: 0x00103710 File Offset: 0x00101910
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.Wrappable)
			{
				this.AdjustSize(this.Dock);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003A7D RID: 14973 RVA: 0x0010372D File Offset: 0x0010192D
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (base.IsHandleCreated)
			{
				if (!this.buttonSize.IsEmpty)
				{
					this.SendToolbarButtonSizeMessage();
					return;
				}
				this.AdjustSize(this.Dock);
				this.ForceButtonWidths();
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x00103764 File Offset: 0x00101964
		private void RealizeButtons()
		{
			if (this.buttons != null)
			{
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					this.BeginUpdate();
					for (int i = 0; i < this.buttonCount; i++)
					{
						if (this.buttons[i].Text.Length > 0)
						{
							string lparam = this.buttons[i].Text + '\0'.ToString();
							this.buttons[i].stringIndex = base.SendMessage(NativeMethods.TB_ADDSTRING, 0, lparam);
						}
						else
						{
							this.buttons[i].stringIndex = (IntPtr)(-1);
						}
					}
					int num = Marshal.SizeOf(typeof(NativeMethods.TBBUTTON));
					int num2 = this.buttonCount;
					intPtr = Marshal.AllocHGlobal(checked(num * num2));
					for (int j = 0; j < num2; j++)
					{
						NativeMethods.TBBUTTON tbbutton = this.buttons[j].GetTBBUTTON(j);
						Marshal.StructureToPtr(tbbutton, (IntPtr)(checked((long)intPtr + unchecked((long)(checked(num * j))))), true);
						this.buttons[j].parent = this;
					}
					base.SendMessage(NativeMethods.TB_ADDBUTTONS, num2, intPtr);
					base.SendMessage(1057, 0, 0);
					if (!this.buttonSize.IsEmpty)
					{
						this.SendToolbarButtonSizeMessage();
					}
					else
					{
						this.ForceButtonWidths();
					}
					this.AdjustSize(this.Dock);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
					this.EndUpdate();
				}
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x001038DC File Offset: 0x00101ADC
		private void RemoveAt(int index)
		{
			this.buttons[index].parent = null;
			this.buttons[index].stringIndex = (IntPtr)(-1);
			this.buttonCount--;
			if (index < this.buttonCount)
			{
				Array.Copy(this.buttons, index + 1, this.buttons, index, this.buttonCount - index);
			}
			this.buttons[this.buttonCount] = null;
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x0010394C File Offset: 0x00101B4C
		private void ResetButtonSize()
		{
			this.buttonSize = Size.Empty;
			base.RecreateHandle();
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x0010395F File Offset: 0x00101B5F
		private void SendToolbarButtonSizeMessage()
		{
			base.SendMessage(1055, 0, NativeMethods.Util.MAKELPARAM((int)((float)this.buttonSize.Width * this.currentScaleDX), (int)((float)this.buttonSize.Height * this.currentScaleDY)));
		}

		/// <summary>Sets the specified bounds of the <see cref="T:System.Windows.Forms.ToolBar" /> control.</summary>
		/// <param name="x">The new <see langword="Left" /> property value of the control.</param>
		/// <param name="y">The new <see langword="Top" /> property value of the control.</param>
		/// <param name="width">The new <see langword="Width" /> property value of the control.</param>
		/// <param name="height">Not used.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x06003A82 RID: 14978 RVA: 0x0010399C File Offset: 0x00101B9C
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			int num = height;
			int num2 = width;
			base.SetBoundsCore(x, y, width, height, specified);
			Rectangle bounds = base.Bounds;
			if (this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
			{
				if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
				{
					this.requestedSize = width;
				}
				if (this.AutoSize)
				{
					width = this.PreferredWidth;
				}
				if (width != num2 && this.Dock == DockStyle.Right)
				{
					int num3 = num2 - width;
					x += num3;
				}
			}
			else
			{
				if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
				{
					this.requestedSize = height;
				}
				if (this.AutoSize)
				{
					height = this.PreferredHeight;
				}
				if (height != num && this.Dock == DockStyle.Bottom)
				{
					int num4 = num - height;
					y += num4;
				}
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x00103A4E File Offset: 0x00101C4E
		private bool ShouldSerializeButtonSize()
		{
			return !this.buttonSize.IsEmpty;
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x00103A5E File Offset: 0x00101C5E
		internal void SetToolTip(ToolTip toolTip)
		{
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1060, new HandleRef(toolTip, toolTip.Handle), 0);
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.ToolBar" /> control.</summary>
		/// <returns>A String that represents the current <see cref="T:System.Windows.Forms.ToolBar" />. </returns>
		// Token: 0x06003A85 RID: 14981 RVA: 0x00103A84 File Offset: 0x00101C84
		public override string ToString()
		{
			string text = base.ToString();
			text = text + ", Buttons.Count: " + this.buttonCount.ToString(CultureInfo.CurrentCulture);
			if (this.buttonCount > 0)
			{
				text = text + ", Buttons[0]: " + this.buttons[0].ToString();
			}
			return text;
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x00103402 File Offset: 0x00101602
		internal void UpdateButtons()
		{
			if (base.IsHandleCreated)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x00103AD8 File Offset: 0x00101CD8
		private void WmNotifyDropDown(ref Message m)
		{
			NativeMethods.NMTOOLBAR nmtoolbar = (NativeMethods.NMTOOLBAR)m.GetLParam(typeof(NativeMethods.NMTOOLBAR));
			ToolBarButton toolBarButton = this.buttons[nmtoolbar.iItem];
			if (toolBarButton == null)
			{
				throw new InvalidOperationException(SR.GetString("ToolBarButtonNotFound"));
			}
			this.OnButtonDropDown(new ToolBarButtonClickEventArgs(toolBarButton));
			Menu dropDownMenu = toolBarButton.DropDownMenu;
			if (dropDownMenu != null)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				NativeMethods.TPMPARAMS tpmparams = new NativeMethods.TPMPARAMS();
				base.SendMessage(1075, nmtoolbar.iItem, ref rect);
				if (dropDownMenu.GetType().IsAssignableFrom(typeof(ContextMenu)))
				{
					((ContextMenu)dropDownMenu).Show(this, new Point(rect.left, rect.bottom));
					return;
				}
				Menu mainMenu = dropDownMenu.GetMainMenu();
				if (mainMenu != null)
				{
					mainMenu.ProcessInitMenuPopup(dropDownMenu.Handle);
				}
				UnsafeNativeMethods.MapWindowPoints(new HandleRef(nmtoolbar.hdr, nmtoolbar.hdr.hwndFrom), NativeMethods.NullHandleRef, ref rect, 2);
				tpmparams.rcExclude_left = rect.left;
				tpmparams.rcExclude_top = rect.top;
				tpmparams.rcExclude_right = rect.right;
				tpmparams.rcExclude_bottom = rect.bottom;
				SafeNativeMethods.TrackPopupMenuEx(new HandleRef(dropDownMenu, dropDownMenu.Handle), 64, rect.left, rect.bottom, new HandleRef(this, base.Handle), tpmparams);
			}
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x00103C34 File Offset: 0x00101E34
		private void WmNotifyNeedText(ref Message m)
		{
			NativeMethods.TOOLTIPTEXT tooltiptext = (NativeMethods.TOOLTIPTEXT)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXT));
			int num = (int)tooltiptext.hdr.idFrom;
			ToolBarButton toolBarButton = this.buttons[num];
			if (toolBarButton != null && toolBarButton.ToolTipText != null)
			{
				tooltiptext.lpszText = toolBarButton.ToolTipText;
			}
			else
			{
				tooltiptext.lpszText = null;
			}
			tooltiptext.hinst = IntPtr.Zero;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				tooltiptext.uFlags |= 4;
			}
			Marshal.StructureToPtr(tooltiptext, m.LParam, false);
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x00103CC0 File Offset: 0x00101EC0
		private void WmNotifyNeedTextA(ref Message m)
		{
			NativeMethods.TOOLTIPTEXTA tooltiptexta = (NativeMethods.TOOLTIPTEXTA)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXTA));
			int num = (int)tooltiptexta.hdr.idFrom;
			ToolBarButton toolBarButton = this.buttons[num];
			if (toolBarButton != null && toolBarButton.ToolTipText != null)
			{
				tooltiptexta.lpszText = toolBarButton.ToolTipText;
			}
			else
			{
				tooltiptexta.lpszText = null;
			}
			tooltiptexta.hinst = IntPtr.Zero;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				tooltiptexta.uFlags |= 4;
			}
			Marshal.StructureToPtr(tooltiptexta, m.LParam, false);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x00103D4C File Offset: 0x00101F4C
		private void WmNotifyHotItemChange(ref Message m)
		{
			NativeMethods.NMTBHOTITEM nmtbhotitem = (NativeMethods.NMTBHOTITEM)m.GetLParam(typeof(NativeMethods.NMTBHOTITEM));
			if (16 == (nmtbhotitem.dwFlags & 16))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (32 == (nmtbhotitem.dwFlags & 32))
			{
				this.hotItem = -1;
				return;
			}
			if (1 == (nmtbhotitem.dwFlags & 1))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (2 == (nmtbhotitem.dwFlags & 2))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (4 == (nmtbhotitem.dwFlags & 4))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (8 == (nmtbhotitem.dwFlags & 8))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (64 == (nmtbhotitem.dwFlags & 64))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (128 == (nmtbhotitem.dwFlags & 128))
			{
				this.hotItem = nmtbhotitem.idNew;
				return;
			}
			if (256 == (nmtbhotitem.dwFlags & 256))
			{
				this.hotItem = nmtbhotitem.idNew;
			}
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x00103E58 File Offset: 0x00102058
		private void WmReflectCommand(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.WParam);
			ToolBarButton toolBarButton = this.buttons[num];
			if (toolBarButton != null)
			{
				ToolBarButtonClickEventArgs e = new ToolBarButtonClickEventArgs(toolBarButton);
				this.OnButtonClick(e);
			}
			base.WndProc(ref m);
			base.ResetMouseEventArgs();
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06003A8C RID: 14988 RVA: 0x00103E98 File Offset: 0x00102098
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int num = m.Msg;
			if (num != 78 && num != 8270)
			{
				if (num == 8465)
				{
					this.WmReflectCommand(ref m);
				}
			}
			else
			{
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
				num = nmhdr.code;
				if (num <= -706)
				{
					if (num != -713)
					{
						if (num != -710)
						{
							if (num == -706)
							{
								m.Result = (IntPtr)1;
							}
						}
						else
						{
							this.WmNotifyDropDown(ref m);
						}
					}
					else
					{
						this.WmNotifyHotItemChange(ref m);
					}
				}
				else if (num != -530)
				{
					if (num != -521)
					{
						if (num == -520)
						{
							this.WmNotifyNeedTextA(ref m);
							m.Result = (IntPtr)1;
							return;
						}
					}
					else
					{
						NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
						int windowPlacement = UnsafeNativeMethods.GetWindowPlacement(new HandleRef(null, nmhdr.hwndFrom), ref windowplacement);
						if (windowplacement.rcNormalPosition_left == 0 && windowplacement.rcNormalPosition_top == 0 && this.hotItem != -1)
						{
							int num2 = 0;
							for (int i = 0; i <= this.hotItem; i++)
							{
								num2 += this.buttonsCollection[i].GetButtonWidth();
							}
							int num3 = windowplacement.rcNormalPosition_right - windowplacement.rcNormalPosition_left;
							int num4 = windowplacement.rcNormalPosition_bottom - windowplacement.rcNormalPosition_top;
							int x = base.Location.X + num2 + 1;
							int y = base.Location.Y + this.ButtonSize.Height / 2;
							NativeMethods.POINT point = new NativeMethods.POINT(x, y);
							UnsafeNativeMethods.ClientToScreen(new HandleRef(this, base.Handle), point);
							if (point.y < SystemInformation.WorkingArea.Y)
							{
								point.y += this.ButtonSize.Height / 2 + 1;
							}
							if (point.y + num4 > SystemInformation.WorkingArea.Height)
							{
								point.y -= this.ButtonSize.Height / 2 + num4 + 1;
							}
							if (point.x + num3 > SystemInformation.WorkingArea.Right)
							{
								point.x -= this.ButtonSize.Width + num3 + 2;
							}
							SafeNativeMethods.SetWindowPos(new HandleRef(null, nmhdr.hwndFrom), NativeMethods.NullHandleRef, point.x, point.y, 0, 0, 21);
							m.Result = (IntPtr)1;
							return;
						}
					}
				}
				else if (Marshal.SystemDefaultCharSize == 2)
				{
					this.WmNotifyNeedText(ref m);
					m.Result = (IntPtr)1;
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x040022FA RID: 8954
		private ToolBar.ToolBarButtonCollection buttonsCollection;

		// Token: 0x040022FB RID: 8955
		internal Size buttonSize = Size.Empty;

		// Token: 0x040022FC RID: 8956
		private int requestedSize;

		// Token: 0x040022FD RID: 8957
		internal const int DDARROW_WIDTH = 15;

		// Token: 0x040022FE RID: 8958
		private ToolBarAppearance appearance;

		// Token: 0x040022FF RID: 8959
		private BorderStyle borderStyle;

		// Token: 0x04002300 RID: 8960
		private ToolBarButton[] buttons;

		// Token: 0x04002301 RID: 8961
		private int buttonCount;

		// Token: 0x04002302 RID: 8962
		private ToolBarTextAlign textAlign;

		// Token: 0x04002303 RID: 8963
		private ImageList imageList;

		// Token: 0x04002304 RID: 8964
		private int maxWidth = -1;

		// Token: 0x04002305 RID: 8965
		private int hotItem = -1;

		// Token: 0x04002306 RID: 8966
		private float currentScaleDX = 1f;

		// Token: 0x04002307 RID: 8967
		private float currentScaleDY = 1f;

		// Token: 0x04002308 RID: 8968
		private const int TOOLBARSTATE_wrappable = 1;

		// Token: 0x04002309 RID: 8969
		private const int TOOLBARSTATE_dropDownArrows = 2;

		// Token: 0x0400230A RID: 8970
		private const int TOOLBARSTATE_divider = 4;

		// Token: 0x0400230B RID: 8971
		private const int TOOLBARSTATE_showToolTips = 8;

		// Token: 0x0400230C RID: 8972
		private const int TOOLBARSTATE_autoSize = 16;

		// Token: 0x0400230D RID: 8973
		private BitVector32 toolBarState;

		// Token: 0x0400230E RID: 8974
		private ToolBarButtonClickEventHandler onButtonClick;

		// Token: 0x0400230F RID: 8975
		private ToolBarButtonClickEventHandler onButtonDropDown;

		/// <summary>Encapsulates a collection of <see cref="T:System.Windows.Forms.ToolBarButton" /> controls for use by the <see cref="T:System.Windows.Forms.ToolBar" /> class.</summary>
		// Token: 0x02000729 RID: 1833
		public class ToolBarButtonCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolBar.ToolBarButtonCollection" /> class and assigns it to the specified toolbar.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ToolBar" /> that is the parent of the collection of <see cref="T:System.Windows.Forms.ToolBarButton" /> controls. </param>
			// Token: 0x060060AD RID: 24749 RVA: 0x0018BEC2 File Offset: 0x0018A0C2
			public ToolBarButtonCollection(ToolBar owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets or sets the toolbar button at the specified indexed location in the toolbar button collection.</summary>
			/// <param name="index">The indexed location of the <see cref="T:System.Windows.Forms.ToolBarButton" /> in the collection. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.ToolBarButton" /> that represents the toolbar button at the specified indexed location.</returns>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="index" /> value is <see langword="null" />. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> value is less than zero.-or- The <paramref name="index" /> value is greater than the number of buttons in the collection, and the collection of buttons is not <see langword="null" />. </exception>
			// Token: 0x17001712 RID: 5906
			public virtual ToolBarButton this[int index]
			{
				get
				{
					if (index < 0 || (this.owner.buttons != null && index >= this.owner.buttonCount))
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.buttons[index];
				}
				set
				{
					if (index < 0 || (this.owner.buttons != null && index >= this.owner.buttonCount))
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}
					this.owner.InternalSetButton(index, value, true, true);
				}
			}

			/// <summary>Gets or sets the item at a specified index.</summary>
			/// <param name="index">The zero-based index of the element to get or set. </param>
			/// <returns>The element at the specified index.</returns>
			// Token: 0x17001713 RID: 5907
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is ToolBarButton)
					{
						this[index] = (ToolBarButton)value;
						return;
					}
					throw new ArgumentException(SR.GetString("ToolBarBadToolBarButton"), "value");
				}
			}

			/// <summary>Gets a <see cref="T:System.Windows.Forms.ToolBarButton" /> with the specified key from the collection.</summary>
			/// <param name="key">The name of the <see cref="T:System.Windows.Forms.ToolBarButton" /> to retrieve.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.ToolBarButton" /> whose <see cref="P:System.Windows.Forms.ToolBarButton.Name" /> property matches the specified key.</returns>
			// Token: 0x17001714 RID: 5908
			public virtual ToolBarButton this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			/// <summary>Gets the number of buttons in the toolbar button collection.</summary>
			/// <returns>The number of the <see cref="T:System.Windows.Forms.ToolBarButton" /> controls assigned to the toolbar.</returns>
			// Token: 0x17001715 RID: 5909
			// (get) Token: 0x060060B3 RID: 24755 RVA: 0x0018C025 File Offset: 0x0018A225
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.buttonCount;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection of buttons.</summary>
			// Token: 0x17001716 RID: 5910
			// (get) Token: 0x060060B4 RID: 24756 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001717 RID: 5911
			// (get) Token: 0x060060B5 RID: 24757 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001718 RID: 5912
			// (get) Token: 0x060060B6 RID: 24758 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
			// Token: 0x17001719 RID: 5913
			// (get) Token: 0x060060B7 RID: 24759 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds the specified toolbar button to the end of the toolbar button collection.</summary>
			/// <param name="button">The <see cref="T:System.Windows.Forms.ToolBarButton" /> to be added after all existing buttons. </param>
			/// <returns>The zero-based index value of the <see cref="T:System.Windows.Forms.ToolBarButton" /> added to the collection.</returns>
			// Token: 0x060060B8 RID: 24760 RVA: 0x0018C034 File Offset: 0x0018A234
			public int Add(ToolBarButton button)
			{
				int result = this.owner.InternalAddButton(button);
				if (!this.suspendUpdate)
				{
					this.owner.UpdateButtons();
				}
				return result;
			}

			/// <summary>Adds a new toolbar button to the end of the toolbar button collection with the specified <see cref="P:System.Windows.Forms.ToolBarButton.Text" /> property value.</summary>
			/// <param name="text">The text to display on the new <see cref="T:System.Windows.Forms.ToolBarButton" />. </param>
			/// <returns>The zero-based index value of the <see cref="T:System.Windows.Forms.ToolBarButton" /> added to the collection.</returns>
			// Token: 0x060060B9 RID: 24761 RVA: 0x0018C064 File Offset: 0x0018A264
			public int Add(string text)
			{
				ToolBarButton button = new ToolBarButton(text);
				return this.Add(button);
			}

			/// <summary>Adds the specified toolbar button to the end of the toolbar button collection.</summary>
			/// <param name="button">The <see cref="T:System.Windows.Forms.ToolBarButton" /> to be added after all existing buttons.</param>
			/// <returns>The zero-based index value of the <see cref="T:System.Windows.Forms.ToolBarButton" /> added to the collection.</returns>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="button" /> is not a <see cref="T:System.Windows.Forms.ToolBarButton" />.</exception>
			// Token: 0x060060BA RID: 24762 RVA: 0x0018C07F File Offset: 0x0018A27F
			int IList.Add(object button)
			{
				if (button is ToolBarButton)
				{
					return this.Add((ToolBarButton)button);
				}
				throw new ArgumentException(SR.GetString("ToolBarBadToolBarButton"), "button");
			}

			/// <summary>Adds a collection of toolbar buttons to this toolbar button collection.</summary>
			/// <param name="buttons">The collection of <see cref="T:System.Windows.Forms.ToolBarButton" /> controls to add to this <see cref="T:System.Windows.Forms.ToolBar.ToolBarButtonCollection" /> contained in an array. </param>
			// Token: 0x060060BB RID: 24763 RVA: 0x0018C0AC File Offset: 0x0018A2AC
			public void AddRange(ToolBarButton[] buttons)
			{
				if (buttons == null)
				{
					throw new ArgumentNullException("buttons");
				}
				try
				{
					this.suspendUpdate = true;
					foreach (ToolBarButton button in buttons)
					{
						this.Add(button);
					}
				}
				finally
				{
					this.suspendUpdate = false;
					this.owner.UpdateButtons();
				}
			}

			/// <summary>Removes all buttons from the toolbar button collection.</summary>
			// Token: 0x060060BC RID: 24764 RVA: 0x0018C110 File Offset: 0x0018A310
			public void Clear()
			{
				if (this.owner.buttons == null)
				{
					return;
				}
				for (int i = this.owner.buttonCount; i > 0; i--)
				{
					if (this.owner.IsHandleCreated)
					{
						this.owner.SendMessage(1046, i - 1, 0);
					}
					this.owner.RemoveAt(i - 1);
				}
				this.owner.buttons = null;
				this.owner.buttonCount = 0;
				if (!this.owner.Disposing)
				{
					this.owner.UpdateButtons();
				}
			}

			/// <summary>Determines if the specified toolbar button is a member of the collection.</summary>
			/// <param name="button">The <see cref="T:System.Windows.Forms.ToolBarButton" /> to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolBarButton" /> is a member of the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x060060BD RID: 24765 RVA: 0x0018C1A1 File Offset: 0x0018A3A1
			public bool Contains(ToolBarButton button)
			{
				return this.IndexOf(button) != -1;
			}

			/// <summary>Determines whether the collection contains a specific value.</summary>
			/// <param name="button">The item to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the item is found in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x060060BE RID: 24766 RVA: 0x0018C1B0 File Offset: 0x0018A3B0
			bool IList.Contains(object button)
			{
				return button is ToolBarButton && this.Contains((ToolBarButton)button);
			}

			/// <summary>Determines if a <see cref="T:System.Windows.Forms.ToolBarButton" /> with the specified key is contained in the collection.</summary>
			/// <param name="key">The name of the <see cref="T:System.Windows.Forms.ToolBarButton" /> to search for.</param>
			/// <returns>
			///     <see langword="true" /> to indicate a <see cref="T:System.Windows.Forms.ToolBarButton" /> with the specified key is found; otherwise, <see langword="false" />. </returns>
			// Token: 0x060060BF RID: 24767 RVA: 0x0018C1C8 File Offset: 0x0018A3C8
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
			/// <param name="dest">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
			/// <param name="index">The zero-based index in <paramref name="dest" /> at which copying begins. </param>
			// Token: 0x060060C0 RID: 24768 RVA: 0x0018C1D7 File Offset: 0x0018A3D7
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.owner.buttonCount > 0)
				{
					Array.Copy(this.owner.buttons, 0, dest, index, this.owner.buttonCount);
				}
			}

			/// <summary>Retrieves the index of the specified toolbar button in the collection.</summary>
			/// <param name="button">The <see cref="T:System.Windows.Forms.ToolBarButton" /> to locate in the collection. </param>
			/// <returns>The zero-based index of the item found in the collection; otherwise, -1.</returns>
			// Token: 0x060060C1 RID: 24769 RVA: 0x0018C208 File Offset: 0x0018A408
			public int IndexOf(ToolBarButton button)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == button)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Determines the index of a specific item in the collection.</summary>
			/// <param name="button">The item to locate in the collection. </param>
			/// <returns>The index of <paramref name="button" /> if found in the list; otherwise, -1.</returns>
			// Token: 0x060060C2 RID: 24770 RVA: 0x0018C233 File Offset: 0x0018A433
			int IList.IndexOf(object button)
			{
				if (button is ToolBarButton)
				{
					return this.IndexOf((ToolBarButton)button);
				}
				return -1;
			}

			/// <summary>Retrieves the index of the first occurrence of a <see cref="T:System.Windows.Forms.ToolBarButton" /> with the specified key.</summary>
			/// <param name="key">The name of the <see cref="T:System.Windows.Forms.ToolBarButton" /> to search for.</param>
			/// <returns>The index of the first occurrence of a <see cref="T:System.Windows.Forms.ToolBarButton" /> with the specified key, if found; otherwise, -1.</returns>
			// Token: 0x060060C3 RID: 24771 RVA: 0x0018C24C File Offset: 0x0018A44C
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			/// <summary>Inserts an existing toolbar button in the toolbar button collection at the specified location.</summary>
			/// <param name="index">The indexed location within the collection to insert the toolbar button. </param>
			/// <param name="button">The <see cref="T:System.Windows.Forms.ToolBarButton" /> to insert. </param>
			// Token: 0x060060C4 RID: 24772 RVA: 0x0018C2C9 File Offset: 0x0018A4C9
			public void Insert(int index, ToolBarButton button)
			{
				this.owner.InsertButton(index, button);
			}

			/// <summary>Inserts an existing toolbar button in the toolbar button collection at the specified location.</summary>
			/// <param name="index">The indexed location within the collection to insert the toolbar button. </param>
			/// <param name="button">The <see cref="T:System.Windows.Forms.ToolBarButton" /> to insert.</param>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="button" /> is not a <see cref="T:System.Windows.Forms.ToolBarButton" />.</exception>
			// Token: 0x060060C5 RID: 24773 RVA: 0x0018C2D8 File Offset: 0x0018A4D8
			void IList.Insert(int index, object button)
			{
				if (button is ToolBarButton)
				{
					this.Insert(index, (ToolBarButton)button);
					return;
				}
				throw new ArgumentException(SR.GetString("ToolBarBadToolBarButton"), "button");
			}

			// Token: 0x060060C6 RID: 24774 RVA: 0x0018C304 File Offset: 0x0018A504
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Removes a given button from the toolbar button collection.</summary>
			/// <param name="index">The indexed location of the <see cref="T:System.Windows.Forms.ToolBarButton" /> in the collection. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> value is less than 0, or it is greater than the number of buttons in the collection. </exception>
			// Token: 0x060060C7 RID: 24775 RVA: 0x0018C318 File Offset: 0x0018A518
			public void RemoveAt(int index)
			{
				int num = (this.owner.buttons == null) ? 0 : this.owner.buttonCount;
				if (index < 0 || index >= num)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.IsHandleCreated)
				{
					this.owner.SendMessage(1046, index, 0);
				}
				this.owner.RemoveAt(index);
				this.owner.UpdateButtons();
			}

			/// <summary>Removes the <see cref="T:System.Windows.Forms.ToolBarButton" /> with the specified key from the collection.</summary>
			/// <param name="key">The name of the <see cref="T:System.Windows.Forms.ToolBarButton" /> to remove from the collection.</param>
			// Token: 0x060060C8 RID: 24776 RVA: 0x0018C3B4 File Offset: 0x0018A5B4
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>Removes a given button from the toolbar button collection.</summary>
			/// <param name="button">The <see cref="T:System.Windows.Forms.ToolBarButton" /> to remove from the collection. </param>
			// Token: 0x060060C9 RID: 24777 RVA: 0x0018C3DC File Offset: 0x0018A5DC
			public void Remove(ToolBarButton button)
			{
				int num = this.IndexOf(button);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			/// <summary>Removes the first occurrence of an item from the collection.</summary>
			/// <param name="button">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />. </param>
			// Token: 0x060060CA RID: 24778 RVA: 0x0018C3FC File Offset: 0x0018A5FC
			void IList.Remove(object button)
			{
				if (button is ToolBarButton)
				{
					this.Remove((ToolBarButton)button);
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the toolbar button collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the tree node collection.</returns>
			// Token: 0x060060CB RID: 24779 RVA: 0x0018C412 File Offset: 0x0018A612
			public IEnumerator GetEnumerator()
			{
				return new WindowsFormsUtils.ArraySubsetEnumerator(this.owner.buttons, this.owner.buttonCount);
			}

			// Token: 0x04004162 RID: 16738
			private ToolBar owner;

			// Token: 0x04004163 RID: 16739
			private bool suspendUpdate;

			// Token: 0x04004164 RID: 16740
			private int lastAccessedIndex = -1;
		}
	}
}
