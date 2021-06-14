using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a splitter control that enables the user to resize docked controls. <see cref="T:System.Windows.Forms.Splitter" /> has been replaced by <see cref="T:System.Windows.Forms.SplitContainer" /> and is provided only for compatibility with previous versions.</summary>
	// Token: 0x0200035D RID: 861
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("SplitterMoved")]
	[DefaultProperty("Dock")]
	[SRDescription("DescriptionSplitter")]
	[Designer("System.Windows.Forms.Design.SplitterDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class Splitter : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Splitter" /> class. <see cref="T:System.Windows.Forms.Splitter" /> has been replaced by <see cref="T:System.Windows.Forms.SplitContainer" />, and is provided only for compatibility with previous versions.</summary>
		// Token: 0x060035D1 RID: 13777 RVA: 0x000F67AC File Offset: 0x000F49AC
		public Splitter()
		{
			base.SetStyle(ControlStyles.Selectable, false);
			this.TabStop = false;
			this.minSize = 25;
			this.minExtra = 25;
			this.Dock = DockStyle.Left;
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AnchorStyles" /> values.</returns>
		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x060035D2 RID: 13778 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x060035D3 RID: 13779 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(AnchorStyles.None)]
		public override AnchorStyles Anchor
		{
			get
			{
				return AnchorStyles.None;
			}
			set
			{
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x060035D4 RID: 13780 RVA: 0x000B0BBD File Offset: 0x000AEDBD
		// (set) Token: 0x060035D5 RID: 13781 RVA: 0x000B0BC5 File Offset: 0x000AEDC5
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

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the default size of the control.</returns>
		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x060035D6 RID: 13782 RVA: 0x000F6819 File Offset: 0x000F4A19
		protected override Size DefaultSize
		{
			get
			{
				return new Size(3, 3);
			}
		}

		/// <summary>Gets or sets the default cursor for the control.</summary>
		/// <returns>An object of type <see cref="T:System.Windows.Forms.Cursor" /> representing the current default cursor.</returns>
		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x060035D7 RID: 13783 RVA: 0x000F6824 File Offset: 0x000F4A24
		protected override Cursor DefaultCursor
		{
			get
			{
				DockStyle dock = this.Dock;
				if (dock - DockStyle.Top <= 1)
				{
					return Cursors.HSplit;
				}
				if (dock - DockStyle.Left > 1)
				{
					return base.DefaultCursor;
				}
				return Cursors.VSplit;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The foreground color of the control.</returns>
		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x060035D8 RID: 13784 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x060035D9 RID: 13785 RVA: 0x0001208A File Offset: 0x0001028A
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000297 RID: 663
		// (add) Token: 0x060035DA RID: 13786 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x060035DB RID: 13787 RVA: 0x0005276F File Offset: 0x0005096F
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The background image displayed in the control.</returns>
		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x060035DC RID: 13788 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x060035DD RID: 13789 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000298 RID: 664
		// (add) Token: 0x060035DE RID: 13790 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x060035DF RID: 13791 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x060035E0 RID: 13792 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x060035E1 RID: 13793 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000299 RID: 665
		// (add) Token: 0x060035E2 RID: 13794 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x060035E3 RID: 13795 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The font of the text displayed by the control.</returns>
		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x060035E4 RID: 13796 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x060035E5 RID: 13797 RVA: 0x00012079 File Offset: 0x00010279
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400029A RID: 666
		// (add) Token: 0x060035E6 RID: 13798 RVA: 0x00052778 File Offset: 0x00050978
		// (remove) Token: 0x060035E7 RID: 13799 RVA: 0x00052781 File Offset: 0x00050981
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

		/// <summary>Gets or sets the style of border for the control. <see cref="P:System.Windows.Forms.Splitter.BorderStyle" /> has been replaced by <see cref="P:System.Windows.Forms.SplitContainer.BorderStyle" /> and is provided only for compatibility with previous versions.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see langword="BorderStyle.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value of the property is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x060035E8 RID: 13800 RVA: 0x000F6858 File Offset: 0x000F4A58
		// (set) Token: 0x060035E9 RID: 13801 RVA: 0x000F6860 File Offset: 0x000F4A60
		[DefaultValue(BorderStyle.None)]
		[SRCategory("CatAppearance")]
		[DispId(-504)]
		[SRDescription("SplitterBorderStyleDescr")]
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
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Returns the parameters needed to create the handle. </summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x060035EA RID: 13802 RVA: 0x000F68A0 File Offset: 0x000F4AA0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
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
				return createParams;
			}
		}

		/// <summary>Gets the default Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x060035EB RID: 13803 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets or sets which <see cref="T:System.Windows.Forms.Splitter" /> borders are docked to its parent control and determines how a <see cref="T:System.Windows.Forms.Splitter" /> is resized with its parent.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see cref="F:System.Windows.Forms.DockStyle.Left" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Forms.Splitter.Dock" /> is not set to one of the valid <see cref="T:System.Windows.Forms.DockStyle" /> values.</exception>
		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x060035EC RID: 13804 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x060035ED RID: 13805 RVA: 0x000F6910 File Offset: 0x000F4B10
		[Localizable(true)]
		[DefaultValue(DockStyle.Left)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				if (value != DockStyle.Top && value != DockStyle.Bottom && value != DockStyle.Left && value != DockStyle.Right)
				{
					throw new ArgumentException(SR.GetString("SplitterInvalidDockEnum"));
				}
				int num = this.splitterThickness;
				base.Dock = value;
				DockStyle dock = this.Dock;
				if (dock - DockStyle.Top > 1)
				{
					if (dock - DockStyle.Left > 1)
					{
						return;
					}
					if (this.splitterThickness != -1)
					{
						base.Width = num;
					}
				}
				else if (this.splitterThickness != -1)
				{
					base.Height = num;
					return;
				}
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x060035EE RID: 13806 RVA: 0x000F6980 File Offset: 0x000F4B80
		private bool Horizontal
		{
			get
			{
				DockStyle dock = this.Dock;
				return dock == DockStyle.Left || dock == DockStyle.Right;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x060035EF RID: 13807 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x060035F0 RID: 13808 RVA: 0x00011FEC File Offset: 0x000101EC
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400029B RID: 667
		// (add) Token: 0x060035F1 RID: 13809 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x060035F2 RID: 13810 RVA: 0x0001BF35 File Offset: 0x0001A135
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

		/// <summary>Gets or sets the minimum distance that must remain between the splitter control and the edge of the opposite side of the container (or the closest control docked to that side). <see cref="P:System.Windows.Forms.Splitter.MinExtra" /> has been replaced by similar properties in <see cref="T:System.Windows.Forms.SplitContainer" /> and is provided only for compatibility with previous versions.</summary>
		/// <returns>The minimum distance, in pixels, between the <see cref="T:System.Windows.Forms.Splitter" /> control and the edge of the opposite side of the container (or the closest control docked to that side). The default is 25.</returns>
		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x060035F3 RID: 13811 RVA: 0x000F699E File Offset: 0x000F4B9E
		// (set) Token: 0x060035F4 RID: 13812 RVA: 0x000F69A6 File Offset: 0x000F4BA6
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(25)]
		[SRDescription("SplitterMinExtraDescr")]
		public int MinExtra
		{
			get
			{
				return this.minExtra;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.minExtra = value;
			}
		}

		/// <summary>Gets or sets the minimum distance that must remain between the splitter control and the container edge that the control is docked to. <see cref="P:System.Windows.Forms.Splitter.MinSize" /> has been replaced by <see cref="P:System.Windows.Forms.SplitContainer.Panel1MinSize" /> and <see cref="P:System.Windows.Forms.SplitContainer.Panel2MinSize" /> and is provided only for compatibility with previous versions.</summary>
		/// <returns>The minimum distance, in pixels, between the <see cref="T:System.Windows.Forms.Splitter" /> control and the container edge that the control is docked to. The default is 25.</returns>
		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x060035F5 RID: 13813 RVA: 0x000F69B6 File Offset: 0x000F4BB6
		// (set) Token: 0x060035F6 RID: 13814 RVA: 0x000F69BE File Offset: 0x000F4BBE
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(25)]
		[SRDescription("SplitterMinSizeDescr")]
		public int MinSize
		{
			get
			{
				return this.minSize;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.minSize = value;
			}
		}

		/// <summary>Gets or sets the distance between the splitter control and the container edge that the control is docked to. <see cref="P:System.Windows.Forms.Splitter.SplitPosition" /> has been replaced by <see cref="P:System.Windows.Forms.SplitContainer.Panel1MinSize" /> and <see cref="P:System.Windows.Forms.SplitContainer.Panel2MinSize" /> and is provided only for compatibility with previous versions.</summary>
		/// <returns>The distance, in pixels, between the <see cref="T:System.Windows.Forms.Splitter" /> control and the container edge that the control is docked to. If the <see cref="T:System.Windows.Forms.Splitter" /> control is not bound to a control, the value is -1.</returns>
		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x060035F7 RID: 13815 RVA: 0x000F69CE File Offset: 0x000F4BCE
		// (set) Token: 0x060035F8 RID: 13816 RVA: 0x000F69EC File Offset: 0x000F4BEC
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("SplitterSplitPositionDescr")]
		public int SplitPosition
		{
			get
			{
				if (this.splitSize == -1)
				{
					this.splitSize = this.CalcSplitSize();
				}
				return this.splitSize;
			}
			set
			{
				Splitter.SplitData splitData = this.CalcSplitBounds();
				if (value > this.maxSize)
				{
					value = this.maxSize;
				}
				if (value < this.minSize)
				{
					value = this.minSize;
				}
				this.splitSize = value;
				this.DrawSplitBar(3);
				if (splitData.target == null)
				{
					this.splitSize = -1;
					return;
				}
				Rectangle bounds = splitData.target.Bounds;
				switch (this.Dock)
				{
				case DockStyle.Top:
					bounds.Height = value;
					break;
				case DockStyle.Bottom:
					bounds.Y += bounds.Height - this.splitSize;
					bounds.Height = value;
					break;
				case DockStyle.Left:
					bounds.Width = value;
					break;
				case DockStyle.Right:
					bounds.X += bounds.Width - this.splitSize;
					bounds.Width = value;
					break;
				}
				splitData.target.Bounds = bounds;
				Application.DoEvents();
				this.OnSplitterMoved(new SplitterEventArgs(base.Left, base.Top, base.Left + bounds.Width / 2, base.Top + bounds.Height / 2));
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x060035F9 RID: 13817 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x060035FA RID: 13818 RVA: 0x000AA11D File Offset: 0x000A831D
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400029C RID: 668
		// (add) Token: 0x060035FB RID: 13819 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x060035FC RID: 13820 RVA: 0x000AA12F File Offset: 0x000A832F
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>A string.</returns>
		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x060035FD RID: 13821 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060035FE RID: 13822 RVA: 0x0001BFAD File Offset: 0x0001A1AD
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400029D RID: 669
		// (add) Token: 0x060035FF RID: 13823 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06003600 RID: 13824 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400029E RID: 670
		// (add) Token: 0x06003601 RID: 13825 RVA: 0x000DAC88 File Offset: 0x000D8E88
		// (remove) Token: 0x06003602 RID: 13826 RVA: 0x000DAC91 File Offset: 0x000D8E91
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Enter
		{
			add
			{
				base.Enter += value;
			}
			remove
			{
				base.Enter -= value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400029F RID: 671
		// (add) Token: 0x06003603 RID: 13827 RVA: 0x000B0E8C File Offset: 0x000AF08C
		// (remove) Token: 0x06003604 RID: 13828 RVA: 0x000B0E95 File Offset: 0x000AF095
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x140002A0 RID: 672
		// (add) Token: 0x06003605 RID: 13829 RVA: 0x000B0E9E File Offset: 0x000AF09E
		// (remove) Token: 0x06003606 RID: 13830 RVA: 0x000B0EA7 File Offset: 0x000AF0A7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x140002A1 RID: 673
		// (add) Token: 0x06003607 RID: 13831 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		// (remove) Token: 0x06003608 RID: 13832 RVA: 0x000B0EB9 File Offset: 0x000AF0B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x140002A2 RID: 674
		// (add) Token: 0x06003609 RID: 13833 RVA: 0x000DAC9A File Offset: 0x000D8E9A
		// (remove) Token: 0x0600360A RID: 13834 RVA: 0x000DACA3 File Offset: 0x000D8EA3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Leave
		{
			add
			{
				base.Leave += value;
			}
			remove
			{
				base.Leave -= value;
			}
		}

		/// <summary>Occurs when the splitter control is in the process of moving. <see cref="E:System.Windows.Forms.Splitter.SplitterMoving" /> has been replaced by <see cref="E:System.Windows.Forms.SplitContainer.SplitterMoving" /> and is provided only for compatibility with previous versions.</summary>
		// Token: 0x140002A3 RID: 675
		// (add) Token: 0x0600360B RID: 13835 RVA: 0x000F6B13 File Offset: 0x000F4D13
		// (remove) Token: 0x0600360C RID: 13836 RVA: 0x000F6B26 File Offset: 0x000F4D26
		[SRCategory("CatBehavior")]
		[SRDescription("SplitterSplitterMovingDescr")]
		public event SplitterEventHandler SplitterMoving
		{
			add
			{
				base.Events.AddHandler(Splitter.EVENT_MOVING, value);
			}
			remove
			{
				base.Events.RemoveHandler(Splitter.EVENT_MOVING, value);
			}
		}

		/// <summary>Occurs when the splitter control is moved. <see cref="E:System.Windows.Forms.Splitter.SplitterMoved" /> has been replaced by <see cref="E:System.Windows.Forms.SplitContainer.SplitterMoved" /> and is provided only for compatibility with previous versions.</summary>
		// Token: 0x140002A4 RID: 676
		// (add) Token: 0x0600360D RID: 13837 RVA: 0x000F6B39 File Offset: 0x000F4D39
		// (remove) Token: 0x0600360E RID: 13838 RVA: 0x000F6B4C File Offset: 0x000F4D4C
		[SRCategory("CatBehavior")]
		[SRDescription("SplitterSplitterMovedDescr")]
		public event SplitterEventHandler SplitterMoved
		{
			add
			{
				base.Events.AddHandler(Splitter.EVENT_MOVED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Splitter.EVENT_MOVED, value);
			}
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000F6B60 File Offset: 0x000F4D60
		private void DrawSplitBar(int mode)
		{
			if (mode != 1 && this.lastDrawSplit != -1)
			{
				this.DrawSplitHelper(this.lastDrawSplit);
				this.lastDrawSplit = -1;
			}
			else if (mode != 1 && this.lastDrawSplit == -1)
			{
				return;
			}
			if (mode != 3)
			{
				this.DrawSplitHelper(this.splitSize);
				this.lastDrawSplit = this.splitSize;
				return;
			}
			if (this.lastDrawSplit != -1)
			{
				this.DrawSplitHelper(this.lastDrawSplit);
			}
			this.lastDrawSplit = -1;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000F6BD8 File Offset: 0x000F4DD8
		private Rectangle CalcSplitLine(int splitSize, int minWeight)
		{
			Rectangle bounds = base.Bounds;
			Rectangle bounds2 = this.splitTarget.Bounds;
			switch (this.Dock)
			{
			case DockStyle.Top:
				if (bounds.Height < minWeight)
				{
					bounds.Height = minWeight;
				}
				bounds.Y = bounds2.Y + splitSize;
				break;
			case DockStyle.Bottom:
				if (bounds.Height < minWeight)
				{
					bounds.Height = minWeight;
				}
				bounds.Y = bounds2.Y + bounds2.Height - splitSize - bounds.Height;
				break;
			case DockStyle.Left:
				if (bounds.Width < minWeight)
				{
					bounds.Width = minWeight;
				}
				bounds.X = bounds2.X + splitSize;
				break;
			case DockStyle.Right:
				if (bounds.Width < minWeight)
				{
					bounds.Width = minWeight;
				}
				bounds.X = bounds2.X + bounds2.Width - splitSize - bounds.Width;
				break;
			}
			return bounds;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000F6CD0 File Offset: 0x000F4ED0
		private int CalcSplitSize()
		{
			Control control = this.FindTarget();
			if (control == null)
			{
				return -1;
			}
			Rectangle bounds = control.Bounds;
			DockStyle dock = this.Dock;
			if (dock - DockStyle.Top <= 1)
			{
				return bounds.Height;
			}
			if (dock - DockStyle.Left > 1)
			{
				return -1;
			}
			return bounds.Width;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000F6D18 File Offset: 0x000F4F18
		private Splitter.SplitData CalcSplitBounds()
		{
			Splitter.SplitData splitData = new Splitter.SplitData();
			Control control = this.FindTarget();
			splitData.target = control;
			if (control != null)
			{
				DockStyle dock = control.Dock;
				if (dock - DockStyle.Top > 1)
				{
					if (dock - DockStyle.Left <= 1)
					{
						this.initTargetSize = control.Bounds.Width;
					}
				}
				else
				{
					this.initTargetSize = control.Bounds.Height;
				}
				Control parentInternal = this.ParentInternal;
				Control.ControlCollection controls = parentInternal.Controls;
				int count = controls.Count;
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < count; i++)
				{
					Control control2 = controls[i];
					if (control2 != control)
					{
						dock = control2.Dock;
						if (dock - DockStyle.Top > 1)
						{
							if (dock - DockStyle.Left <= 1)
							{
								num += control2.Width;
							}
						}
						else
						{
							num2 += control2.Height;
						}
					}
				}
				Size clientSize = parentInternal.ClientSize;
				if (this.Horizontal)
				{
					this.maxSize = clientSize.Width - num - this.minExtra;
				}
				else
				{
					this.maxSize = clientSize.Height - num2 - this.minExtra;
				}
				splitData.dockWidth = num;
				splitData.dockHeight = num2;
			}
			return splitData;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000F6E40 File Offset: 0x000F5040
		private void DrawSplitHelper(int splitSize)
		{
			if (this.splitTarget == null)
			{
				return;
			}
			Rectangle rectangle = this.CalcSplitLine(splitSize, 3);
			IntPtr handle = this.ParentInternal.Handle;
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(this.ParentInternal, handle), NativeMethods.NullHandleRef, 1026);
			IntPtr handle2 = ControlPaint.CreateHalftoneHBRUSH();
			IntPtr handle3 = SafeNativeMethods.SelectObject(new HandleRef(this.ParentInternal, dcex), new HandleRef(null, handle2));
			SafeNativeMethods.PatBlt(new HandleRef(this.ParentInternal, dcex), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 5898313);
			SafeNativeMethods.SelectObject(new HandleRef(this.ParentInternal, dcex), new HandleRef(null, handle3));
			SafeNativeMethods.DeleteObject(new HandleRef(null, handle2));
			UnsafeNativeMethods.ReleaseDC(new HandleRef(this.ParentInternal, handle), new HandleRef(null, dcex));
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x000F6F1C File Offset: 0x000F511C
		private Control FindTarget()
		{
			Control parentInternal = this.ParentInternal;
			if (parentInternal == null)
			{
				return null;
			}
			Control.ControlCollection controls = parentInternal.Controls;
			int count = controls.Count;
			DockStyle dock = this.Dock;
			for (int i = 0; i < count; i++)
			{
				Control control = controls[i];
				if (control != this)
				{
					switch (dock)
					{
					case DockStyle.Top:
						if (control.Bottom == base.Top)
						{
							return control;
						}
						break;
					case DockStyle.Bottom:
						if (control.Top == base.Bottom)
						{
							return control;
						}
						break;
					case DockStyle.Left:
						if (control.Right == base.Left)
						{
							return control;
						}
						break;
					case DockStyle.Right:
						if (control.Left == base.Right)
						{
							return control;
						}
						break;
					}
				}
			}
			return null;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x000F6FCC File Offset: 0x000F51CC
		private int GetSplitSize(int x, int y)
		{
			int num;
			if (this.Horizontal)
			{
				num = x - this.anchor.X;
			}
			else
			{
				num = y - this.anchor.Y;
			}
			int val = 0;
			switch (this.Dock)
			{
			case DockStyle.Top:
				val = this.splitTarget.Height + num;
				break;
			case DockStyle.Bottom:
				val = this.splitTarget.Height - num;
				break;
			case DockStyle.Left:
				val = this.splitTarget.Width + num;
				break;
			case DockStyle.Right:
				val = this.splitTarget.Width - num;
				break;
			}
			return Math.Max(Math.Min(val, this.maxSize), this.minSize);
		}

		/// <summary>This method is not relevant to this class.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06003616 RID: 13846 RVA: 0x000F7077 File Offset: 0x000F5277
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.splitTarget != null && e.KeyCode == Keys.Escape)
			{
				this.SplitEnd(false);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06003617 RID: 13847 RVA: 0x000F7099 File Offset: 0x000F5299
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && e.Clicks == 1)
			{
				this.SplitBegin(e.X, e.Y);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06003618 RID: 13848 RVA: 0x000F70CC File Offset: 0x000F52CC
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.splitTarget != null)
			{
				int x = e.X + base.Left;
				int y = e.Y + base.Top;
				Rectangle rectangle = this.CalcSplitLine(this.GetSplitSize(e.X, e.Y), 0);
				int x2 = rectangle.X;
				int y2 = rectangle.Y;
				this.OnSplitterMoving(new SplitterEventArgs(x, y, x2, y2));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06003619 RID: 13849 RVA: 0x000F7140 File Offset: 0x000F5340
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this.splitTarget != null)
			{
				int num = e.X + base.Left;
				int num2 = e.Y + base.Top;
				Rectangle rectangle = this.CalcSplitLine(this.GetSplitSize(e.X, e.Y), 0);
				int x = rectangle.X;
				int y = rectangle.Y;
				this.SplitEnd(true);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Splitter.SplitterMoving" /> event. <see cref="M:System.Windows.Forms.Splitter.OnSplitterMoving(System.Windows.Forms.SplitterEventArgs)" /> has been replaced by <see cref="M:System.Windows.Forms.SplitContainer.OnSplitterMoving(System.Windows.Forms.SplitterCancelEventArgs)" /> and is provided only for compatibility with previous versions.</summary>
		/// <param name="sevent">A <see cref="T:System.Windows.Forms.SplitterEventArgs" /> that contains the event data. </param>
		// Token: 0x0600361A RID: 13850 RVA: 0x000F71AC File Offset: 0x000F53AC
		protected virtual void OnSplitterMoving(SplitterEventArgs sevent)
		{
			SplitterEventHandler splitterEventHandler = (SplitterEventHandler)base.Events[Splitter.EVENT_MOVING];
			if (splitterEventHandler != null)
			{
				splitterEventHandler(this, sevent);
			}
			if (this.splitTarget != null)
			{
				this.SplitMove(sevent.SplitX, sevent.SplitY);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Splitter.SplitterMoved" /> event. <see cref="M:System.Windows.Forms.Splitter.OnSplitterMoved(System.Windows.Forms.SplitterEventArgs)" /> has been replaced by <see cref="M:System.Windows.Forms.SplitContainer.OnSplitterMoved(System.Windows.Forms.SplitterEventArgs)" /> and is provided only for compatibility with previous versions.</summary>
		/// <param name="sevent">A <see cref="T:System.Windows.Forms.SplitterEventArgs" /> that contains the event data. </param>
		// Token: 0x0600361B RID: 13851 RVA: 0x000F71F4 File Offset: 0x000F53F4
		protected virtual void OnSplitterMoved(SplitterEventArgs sevent)
		{
			SplitterEventHandler splitterEventHandler = (SplitterEventHandler)base.Events[Splitter.EVENT_MOVED];
			if (splitterEventHandler != null)
			{
				splitterEventHandler(this, sevent);
			}
			if (this.splitTarget != null)
			{
				this.SplitMove(sevent.SplitX, sevent.SplitY);
			}
		}

		/// <summary>Performs the work of setting the specified bounds of this control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x0600361C RID: 13852 RVA: 0x000F723C File Offset: 0x000F543C
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.Horizontal)
			{
				if (width < 1)
				{
					width = 3;
				}
				this.splitterThickness = width;
			}
			else
			{
				if (height < 1)
				{
					height = 3;
				}
				this.splitterThickness = height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000F7274 File Offset: 0x000F5474
		private void SplitBegin(int x, int y)
		{
			Splitter.SplitData splitData = this.CalcSplitBounds();
			if (splitData.target != null && this.minSize < this.maxSize)
			{
				this.anchor = new Point(x, y);
				this.splitTarget = splitData.target;
				this.splitSize = this.GetSplitSize(x, y);
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					if (this.splitterMessageFilter != null)
					{
						this.splitterMessageFilter = new Splitter.SplitterMessageFilter(this);
					}
					Application.AddMessageFilter(this.splitterMessageFilter);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				base.CaptureInternal = true;
				this.DrawSplitBar(1);
			}
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000F7314 File Offset: 0x000F5514
		private void SplitEnd(bool accept)
		{
			this.DrawSplitBar(3);
			this.splitTarget = null;
			base.CaptureInternal = false;
			if (this.splitterMessageFilter != null)
			{
				Application.RemoveMessageFilter(this.splitterMessageFilter);
				this.splitterMessageFilter = null;
			}
			if (accept)
			{
				this.ApplySplitPosition();
			}
			else if (this.splitSize != this.initTargetSize)
			{
				this.SplitPosition = this.initTargetSize;
			}
			this.anchor = Point.Empty;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000F7380 File Offset: 0x000F5580
		private void ApplySplitPosition()
		{
			this.SplitPosition = this.splitSize;
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000F7390 File Offset: 0x000F5590
		private void SplitMove(int x, int y)
		{
			int num = this.GetSplitSize(x - base.Left + this.anchor.X, y - base.Top + this.anchor.Y);
			if (this.splitSize != num)
			{
				this.splitSize = num;
				this.DrawSplitBar(2);
			}
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.Splitter" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.Splitter" />. </returns>
		// Token: 0x06003621 RID: 13857 RVA: 0x000F73E4 File Offset: 0x000F55E4
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				", MinExtra: ",
				this.MinExtra.ToString(CultureInfo.CurrentCulture),
				", MinSize: ",
				this.MinSize.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x04002199 RID: 8601
		private const int DRAW_START = 1;

		// Token: 0x0400219A RID: 8602
		private const int DRAW_MOVE = 2;

		// Token: 0x0400219B RID: 8603
		private const int DRAW_END = 3;

		// Token: 0x0400219C RID: 8604
		private const int defaultWidth = 3;

		// Token: 0x0400219D RID: 8605
		private BorderStyle borderStyle;

		// Token: 0x0400219E RID: 8606
		private int minSize = 25;

		// Token: 0x0400219F RID: 8607
		private int minExtra = 25;

		// Token: 0x040021A0 RID: 8608
		private Point anchor = Point.Empty;

		// Token: 0x040021A1 RID: 8609
		private Control splitTarget;

		// Token: 0x040021A2 RID: 8610
		private int splitSize = -1;

		// Token: 0x040021A3 RID: 8611
		private int splitterThickness = 3;

		// Token: 0x040021A4 RID: 8612
		private int initTargetSize;

		// Token: 0x040021A5 RID: 8613
		private int lastDrawSplit = -1;

		// Token: 0x040021A6 RID: 8614
		private int maxSize;

		// Token: 0x040021A7 RID: 8615
		private static readonly object EVENT_MOVING = new object();

		// Token: 0x040021A8 RID: 8616
		private static readonly object EVENT_MOVED = new object();

		// Token: 0x040021A9 RID: 8617
		private Splitter.SplitterMessageFilter splitterMessageFilter;

		// Token: 0x0200071C RID: 1820
		private class SplitData
		{
			// Token: 0x04004147 RID: 16711
			public int dockWidth = -1;

			// Token: 0x04004148 RID: 16712
			public int dockHeight = -1;

			// Token: 0x04004149 RID: 16713
			internal Control target;
		}

		// Token: 0x0200071D RID: 1821
		private class SplitterMessageFilter : IMessageFilter
		{
			// Token: 0x06006024 RID: 24612 RVA: 0x0018A362 File Offset: 0x00188562
			public SplitterMessageFilter(Splitter splitter)
			{
				this.owner = splitter;
			}

			// Token: 0x06006025 RID: 24613 RVA: 0x0018A374 File Offset: 0x00188574
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public bool PreFilterMessage(ref Message m)
			{
				if (m.Msg >= 256 && m.Msg <= 264)
				{
					if (m.Msg == 256 && (int)((long)m.WParam) == 27)
					{
						this.owner.SplitEnd(false);
					}
					return true;
				}
				return false;
			}

			// Token: 0x0400414A RID: 16714
			private Splitter owner;
		}
	}
}
