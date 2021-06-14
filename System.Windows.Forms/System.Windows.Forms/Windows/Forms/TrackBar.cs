using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a standard Windows track bar.</summary>
	// Token: 0x020003FE RID: 1022
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Value")]
	[DefaultEvent("Scroll")]
	[DefaultBindingProperty("Value")]
	[Designer("System.Windows.Forms.Design.TrackBarDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionTrackBar")]
	public class TrackBar : Control, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TrackBar" /> class.</summary>
		// Token: 0x06004566 RID: 17766 RVA: 0x0012912C File Offset: 0x0012732C
		public TrackBar()
		{
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
			this.requestedDim = this.PreferredDimension;
		}

		/// <summary>Gets or sets a value indicating whether the height or width of the track bar is being automatically sized.</summary>
		/// <returns>
		///     <see langword="true" /> if the track bar is being automatically sized; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x06004567 RID: 17767 RVA: 0x0012918A File Offset: 0x0012738A
		// (set) Token: 0x06004568 RID: 17768 RVA: 0x00129194 File Offset: 0x00127394
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TrackBarAutoSizeDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return this.autoSize;
			}
			set
			{
				if (this.autoSize != value)
				{
					this.autoSize = value;
					if (this.orientation == Orientation.Horizontal)
					{
						base.SetStyle(ControlStyles.FixedHeight, this.autoSize);
						base.SetStyle(ControlStyles.FixedWidth, false);
					}
					else
					{
						base.SetStyle(ControlStyles.FixedWidth, this.autoSize);
						base.SetStyle(ControlStyles.FixedHeight, false);
					}
					this.AdjustSize();
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TrackBar.AutoSize" /> property changes.</summary>
		// Token: 0x14000387 RID: 903
		// (add) Token: 0x06004569 RID: 17769 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x0600456A RID: 17770 RVA: 0x0001BA37 File Offset: 0x00019C37
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

		/// <summary>Gets or sets the background image for the <see cref="T:System.Windows.Forms.TrackBar" /> control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> that is the background image for the <see cref="T:System.Windows.Forms.TrackBar" />.</returns>
		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x0600456B RID: 17771 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x0600456C RID: 17772 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TrackBar.BackgroundImage" /> property changes.</summary>
		// Token: 0x14000388 RID: 904
		// (add) Token: 0x0600456D RID: 17773 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x0600456E RID: 17774 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>Gets or sets an <see cref="T:System.Windows.Forms.ImageLayout" /> value; however, setting this property has no effect on the <see cref="T:System.Windows.Forms.TrackBar" /> control. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x0600456F RID: 17775 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06004570 RID: 17776 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TrackBar.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x14000389 RID: 905
		// (add) Token: 0x06004571 RID: 17777 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06004572 RID: 17778 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Overrides the <see cref="P:System.Windows.Forms.Control.CreateParams" /> property.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x06004573 RID: 17779 RVA: 0x001291FC File Offset: 0x001273FC
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "msctls_trackbar32";
				switch (this.tickStyle)
				{
				case TickStyle.None:
					createParams.Style |= 16;
					break;
				case TickStyle.TopLeft:
					createParams.Style |= 5;
					break;
				case TickStyle.BottomRight:
					createParams.Style |= 1;
					break;
				case TickStyle.Both:
					createParams.Style |= 9;
					break;
				}
				if (this.orientation == Orientation.Vertical)
				{
					createParams.Style |= 2;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 5242880;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		/// <summary>Gets a value indicating the mode for the Input Method Editor (IME) for the <see cref="T:System.Windows.Forms.TrackBar" />.</summary>
		/// <returns>Always <see cref="F:System.Windows.Forms.ImeMode.Disable" />.</returns>
		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x06004574 RID: 17780 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets the default size of the <see cref="T:System.Windows.Forms.TrackBar" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> representing the default size of the control. </returns>
		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x06004575 RID: 17781 RVA: 0x001292C7 File Offset: 0x001274C7
		protected override Size DefaultSize
		{
			get
			{
				return new Size(104, this.PreferredDimension);
			}
		}

		/// <summary>Gets or sets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker; however, this property has no effect on the <see cref="T:System.Windows.Forms.TrackBar" /> control </summary>
		/// <returns>
		///     <see langword="true" /> if the control has a secondary buffer; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x06004576 RID: 17782 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x06004577 RID: 17783 RVA: 0x000A2CBA File Offset: 0x000A0EBA
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

		/// <summary>Overrides <see cref="P:System.Windows.Forms.Control.Font" /></summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x06004578 RID: 17784 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x06004579 RID: 17785 RVA: 0x00012079 File Offset: 0x00010279
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TrackBar.Font" /> property changes.</summary>
		// Token: 0x1400038A RID: 906
		// (add) Token: 0x0600457A RID: 17786 RVA: 0x00052778 File Offset: 0x00050978
		// (remove) Token: 0x0600457B RID: 17787 RVA: 0x00052781 File Offset: 0x00050981
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

		/// <summary>Gets the foreground color of the track bar.</summary>
		/// <returns>Always <see cref="P:System.Drawing.SystemColors.WindowText" />.</returns>
		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x0600457C RID: 17788 RVA: 0x001292D6 File Offset: 0x001274D6
		// (set) Token: 0x0600457D RID: 17789 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return SystemColors.WindowText;
			}
			set
			{
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TrackBar.ForeColor" /> property changes.</summary>
		// Token: 0x1400038B RID: 907
		// (add) Token: 0x0600457E RID: 17790 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x0600457F RID: 17791 RVA: 0x0005276F File Offset: 0x0005096F
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

		/// <summary>Gets or sets the Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x06004580 RID: 17792 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06004581 RID: 17793 RVA: 0x00011FEC File Offset: 0x000101EC
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TrackBar.ImeMode" /> property changes.</summary>
		// Token: 0x1400038C RID: 908
		// (add) Token: 0x06004582 RID: 17794 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x06004583 RID: 17795 RVA: 0x0001BF35 File Offset: 0x0001A135
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

		/// <summary>Gets or sets a value to be added to or subtracted from the <see cref="P:System.Windows.Forms.TrackBar.Value" /> property when the scroll box is moved a large distance.</summary>
		/// <returns>A numeric value. The default is 5.</returns>
		/// <exception cref="T:System.ArgumentException">The assigned value is less than 0. </exception>
		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x001292DD File Offset: 0x001274DD
		// (set) Token: 0x06004585 RID: 17797 RVA: 0x001292E8 File Offset: 0x001274E8
		[SRCategory("CatBehavior")]
		[DefaultValue(5)]
		[SRDescription("TrackBarLargeChangeDescr")]
		public int LargeChange
		{
			get
			{
				return this.largeChange;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("LargeChange", SR.GetString("TrackBarLargeChangeError", new object[]
					{
						value
					}));
				}
				if (this.largeChange != value)
				{
					this.largeChange = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(1045, 0, value);
					}
				}
			}
		}

		/// <summary>Gets or sets the upper limit of the range this <see cref="T:System.Windows.Forms.TrackBar" /> is working with.</summary>
		/// <returns>The maximum value for the <see cref="T:System.Windows.Forms.TrackBar" />. The default is 10.</returns>
		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x00129343 File Offset: 0x00127543
		// (set) Token: 0x06004587 RID: 17799 RVA: 0x0012934B File Offset: 0x0012754B
		[SRCategory("CatBehavior")]
		[DefaultValue(10)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("TrackBarMaximumDescr")]
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
					if (value < this.minimum)
					{
						this.minimum = value;
					}
					this.SetRange(this.minimum, value);
				}
			}
		}

		/// <summary>Gets or sets the lower limit of the range this <see cref="T:System.Windows.Forms.TrackBar" /> is working with.</summary>
		/// <returns>The minimum value for the <see cref="T:System.Windows.Forms.TrackBar" />. The default is 0.</returns>
		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x06004588 RID: 17800 RVA: 0x00129373 File Offset: 0x00127573
		// (set) Token: 0x06004589 RID: 17801 RVA: 0x0012937B File Offset: 0x0012757B
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("TrackBarMinimumDescr")]
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
					if (value > this.maximum)
					{
						this.maximum = value;
					}
					this.SetRange(value, this.maximum);
				}
			}
		}

		/// <summary>Gets or sets a value indicating the horizontal or vertical orientation of the track bar.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Orientation" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.Orientation" /> values. </exception>
		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x0600458A RID: 17802 RVA: 0x001293A3 File Offset: 0x001275A3
		// (set) Token: 0x0600458B RID: 17803 RVA: 0x001293AC File Offset: 0x001275AC
		[SRCategory("CatAppearance")]
		[DefaultValue(Orientation.Horizontal)]
		[Localizable(true)]
		[SRDescription("TrackBarOrientationDescr")]
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Orientation));
				}
				if (this.orientation != value)
				{
					this.orientation = value;
					if (this.orientation == Orientation.Horizontal)
					{
						base.SetStyle(ControlStyles.FixedHeight, this.autoSize);
						base.SetStyle(ControlStyles.FixedWidth, false);
						base.Width = this.requestedDim;
					}
					else
					{
						base.SetStyle(ControlStyles.FixedHeight, false);
						base.SetStyle(ControlStyles.FixedWidth, this.autoSize);
						base.Height = this.requestedDim;
					}
					if (base.IsHandleCreated)
					{
						Rectangle bounds = base.Bounds;
						base.RecreateHandle();
						base.SetBounds(bounds.X, bounds.Y, bounds.Height, bounds.Width, BoundsSpecified.All);
						this.AdjustSize();
					}
				}
			}
		}

		/// <summary>Gets or sets the space between the edges of a <see cref="T:System.Windows.Forms.TrackBar" /> control and its contents.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> object.</returns>
		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x0600458C RID: 17804 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x0600458D RID: 17805 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TrackBar.Padding" /> property changes.</summary>
		// Token: 0x1400038D RID: 909
		// (add) Token: 0x0600458E RID: 17806 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x0600458F RID: 17807 RVA: 0x000204B4 File Offset: 0x0001E6B4
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

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x06004590 RID: 17808 RVA: 0x00129484 File Offset: 0x00127684
		private int PreferredDimension
		{
			get
			{
				int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(3);
				return systemMetrics * 8 / 3;
			}
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x0012949D File Offset: 0x0012769D
		private void RedrawControl()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(1032, 1, this.maximum);
				base.Invalidate();
			}
		}

		/// <summary>Gets or sets a value indicating whether the contents of the <see cref="T:System.Windows.Forms.TrackBar" /> will be laid out from right to left.</summary>
		/// <returns>
		///     <see langword="true" /> if the contents of the <see cref="T:System.Windows.Forms.TrackBar" /> are laid out from right to left; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x06004592 RID: 17810 RVA: 0x001294C0 File Offset: 0x001276C0
		// (set) Token: 0x06004593 RID: 17811 RVA: 0x001294C8 File Offset: 0x001276C8
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Gets or sets the value added to or subtracted from the <see cref="P:System.Windows.Forms.TrackBar.Value" /> property when the scroll box is moved a small distance.</summary>
		/// <returns>A numeric value. The default value is 1.</returns>
		/// <exception cref="T:System.ArgumentException">The assigned value is less than 0. </exception>
		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x06004594 RID: 17812 RVA: 0x0012951C File Offset: 0x0012771C
		// (set) Token: 0x06004595 RID: 17813 RVA: 0x00129524 File Offset: 0x00127724
		[SRCategory("CatBehavior")]
		[DefaultValue(1)]
		[SRDescription("TrackBarSmallChangeDescr")]
		public int SmallChange
		{
			get
			{
				return this.smallChange;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SmallChange", SR.GetString("TrackBarSmallChangeError", new object[]
					{
						value
					}));
				}
				if (this.smallChange != value)
				{
					this.smallChange = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(1047, 0, value);
					}
				}
			}
		}

		/// <summary>Gets or sets the text of the <see cref="T:System.Windows.Forms.TrackBar" />.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x06004596 RID: 17814 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06004597 RID: 17815 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TrackBar.Text" /> property changes.</summary>
		// Token: 0x1400038E RID: 910
		// (add) Token: 0x06004598 RID: 17816 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06004599 RID: 17817 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Gets or sets a value indicating how to display the tick marks on the track bar.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TickStyle" /> values. The default is <see cref="F:System.Windows.Forms.TickStyle.BottomRight" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not a valid <see cref="T:System.Windows.Forms.TickStyle" />. </exception>
		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x0600459A RID: 17818 RVA: 0x0012957F File Offset: 0x0012777F
		// (set) Token: 0x0600459B RID: 17819 RVA: 0x00129587 File Offset: 0x00127787
		[SRCategory("CatAppearance")]
		[DefaultValue(TickStyle.BottomRight)]
		[SRDescription("TrackBarTickStyleDescr")]
		public TickStyle TickStyle
		{
			get
			{
				return this.tickStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TickStyle));
				}
				if (this.tickStyle != value)
				{
					this.tickStyle = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets a value that specifies the delta between ticks drawn on the control.</summary>
		/// <returns>The numeric value representing the delta between ticks. The default is 1.</returns>
		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x0600459C RID: 17820 RVA: 0x001295C5 File Offset: 0x001277C5
		// (set) Token: 0x0600459D RID: 17821 RVA: 0x001295CD File Offset: 0x001277CD
		[SRCategory("CatAppearance")]
		[DefaultValue(1)]
		[SRDescription("TrackBarTickFrequencyDescr")]
		public int TickFrequency
		{
			get
			{
				return this.tickFrequency;
			}
			set
			{
				if (this.tickFrequency != value)
				{
					this.tickFrequency = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(1044, value, 0);
						base.Invalidate();
					}
				}
			}
		}

		/// <summary>Gets or sets a numeric value that represents the current position of the scroll box on the track bar.</summary>
		/// <returns>A numeric value that is within the <see cref="P:System.Windows.Forms.TrackBar.Minimum" /> and <see cref="P:System.Windows.Forms.TrackBar.Maximum" /> range. The default value is 0.</returns>
		/// <exception cref="T:System.ArgumentException">The assigned value is less than the value of <see cref="P:System.Windows.Forms.TrackBar.Minimum" />.-or- The assigned value is greater than the value of <see cref="P:System.Windows.Forms.TrackBar.Maximum" />. </exception>
		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x0600459E RID: 17822 RVA: 0x001295FB File Offset: 0x001277FB
		// (set) Token: 0x0600459F RID: 17823 RVA: 0x0012960C File Offset: 0x0012780C
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Bindable(true)]
		[SRDescription("TrackBarValueDescr")]
		public int Value
		{
			get
			{
				this.GetTrackBarValue();
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					if (!this.initializing && (value < this.minimum || value > this.maximum))
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'Minimum'",
							"'Maximum'"
						}));
					}
					this.value = value;
					this.SetTrackBarPosition();
					this.OnValueChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.TrackBar" /> control.</summary>
		// Token: 0x1400038F RID: 911
		// (add) Token: 0x060045A0 RID: 17824 RVA: 0x000A2B72 File Offset: 0x000A0D72
		// (remove) Token: 0x060045A1 RID: 17825 RVA: 0x000A2B7B File Offset: 0x000A0D7B
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

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.TrackBar" /> control.</summary>
		// Token: 0x14000390 RID: 912
		// (add) Token: 0x060045A2 RID: 17826 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x060045A3 RID: 17827 RVA: 0x0001B704 File Offset: 0x00019904
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

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.TrackBar" /> control.</summary>
		// Token: 0x14000391 RID: 913
		// (add) Token: 0x060045A4 RID: 17828 RVA: 0x000A2FE9 File Offset: 0x000A11E9
		// (remove) Token: 0x060045A5 RID: 17829 RVA: 0x000A2FF2 File Offset: 0x000A11F2
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

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.TrackBar" /> control.</summary>
		// Token: 0x14000392 RID: 914
		// (add) Token: 0x060045A6 RID: 17830 RVA: 0x0001B70D File Offset: 0x0001990D
		// (remove) Token: 0x060045A7 RID: 17831 RVA: 0x0001B716 File Offset: 0x00019916
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TrackBar.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x14000393 RID: 915
		// (add) Token: 0x060045A8 RID: 17832 RVA: 0x00129696 File Offset: 0x00127896
		// (remove) Token: 0x060045A9 RID: 17833 RVA: 0x001296A9 File Offset: 0x001278A9
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(TrackBar.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TrackBar.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
		}

		/// <summary>Occurs when either a mouse or keyboard action moves the scroll box.</summary>
		// Token: 0x14000394 RID: 916
		// (add) Token: 0x060045AA RID: 17834 RVA: 0x001296BC File Offset: 0x001278BC
		// (remove) Token: 0x060045AB RID: 17835 RVA: 0x001296CF File Offset: 0x001278CF
		[SRCategory("CatBehavior")]
		[SRDescription("TrackBarOnScrollDescr")]
		public event EventHandler Scroll
		{
			add
			{
				base.Events.AddHandler(TrackBar.EVENT_SCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(TrackBar.EVENT_SCROLL, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.TrackBar" /> control is drawn.</summary>
		// Token: 0x14000395 RID: 917
		// (add) Token: 0x060045AC RID: 17836 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x060045AD RID: 17837 RVA: 0x00020D40 File Offset: 0x0001EF40
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TrackBar.Value" /> property of a track bar changes, either by movement of the scroll box or by manipulation in code.</summary>
		// Token: 0x14000396 RID: 918
		// (add) Token: 0x060045AE RID: 17838 RVA: 0x001296E2 File Offset: 0x001278E2
		// (remove) Token: 0x060045AF RID: 17839 RVA: 0x001296F5 File Offset: 0x001278F5
		[SRCategory("CatAction")]
		[SRDescription("valueChangedEventDescr")]
		public event EventHandler ValueChanged
		{
			add
			{
				base.Events.AddHandler(TrackBar.EVENT_VALUECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TrackBar.EVENT_VALUECHANGED, value);
			}
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x00129708 File Offset: 0x00127908
		private void AdjustSize()
		{
			if (base.IsHandleCreated)
			{
				int num = this.requestedDim;
				try
				{
					if (this.orientation == Orientation.Horizontal)
					{
						base.Height = (this.autoSize ? this.PreferredDimension : num);
					}
					else
					{
						base.Width = (this.autoSize ? this.PreferredDimension : num);
					}
				}
				finally
				{
					this.requestedDim = num;
				}
			}
		}

		/// <summary>Begins the initialization of a <see cref="T:System.Windows.Forms.TrackBar" /> that is used on a form or used by another component. The initialization occurs at run time.</summary>
		// Token: 0x060045B1 RID: 17841 RVA: 0x00129778 File Offset: 0x00127978
		public void BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x00129781 File Offset: 0x00127981
		private void ConstrainValue()
		{
			if (this.initializing)
			{
				return;
			}
			if (this.Value < this.minimum)
			{
				this.Value = this.minimum;
			}
			if (this.Value > this.maximum)
			{
				this.Value = this.maximum;
			}
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.CreateHandle" /> method.</summary>
		// Token: 0x060045B3 RID: 17843 RVA: 0x001297C0 File Offset: 0x001279C0
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

		/// <summary>Ends the initialization of a <see cref="T:System.Windows.Forms.TrackBar" /> that is used on a form or used by another component. The initialization occurs at run time.</summary>
		// Token: 0x060045B4 RID: 17844 RVA: 0x00129810 File Offset: 0x00127A10
		public void EndInit()
		{
			this.initializing = false;
			this.ConstrainValue();
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x00129820 File Offset: 0x00127A20
		private void GetTrackBarValue()
		{
			if (base.IsHandleCreated)
			{
				this.value = (int)((long)base.SendMessage(1024, 0, 0));
				if (this.orientation == Orientation.Vertical)
				{
					this.value = this.Minimum + this.Maximum - this.value;
				}
				if (this.orientation == Orientation.Horizontal && this.RightToLeft == RightToLeft.Yes && !base.IsMirrored)
				{
					this.value = this.Minimum + this.Maximum - this.value;
				}
			}
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x060045B6 RID: 17846 RVA: 0x001298A4 File Offset: 0x00127AA4
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			Keys keys = keyData & Keys.KeyCode;
			return keys - Keys.Prior <= 3 || base.IsInputKey(keyData);
		}

		/// <summary>Use the <see cref="M:System.Windows.Forms.Control.OnHandleCreated(System.EventArgs)" /> method.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060045B7 RID: 17847 RVA: 0x001298DC File Offset: 0x00127ADC
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(1031, 0, this.minimum);
			base.SendMessage(1032, 0, this.maximum);
			base.SendMessage(1044, this.tickFrequency, 0);
			base.SendMessage(1045, 0, this.largeChange);
			base.SendMessage(1047, 0, this.smallChange);
			this.SetTrackBarPosition();
			this.AdjustSize();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TrackBar.RightToLeftLayoutChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" />  that contains the event data. </param>
		// Token: 0x060045B8 RID: 17848 RVA: 0x0012995C File Offset: 0x00127B5C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				base.RecreateHandle();
			}
			EventHandler eventHandler = base.Events[TrackBar.EVENT_RIGHTTOLEFTLAYOUTCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TrackBar.Scroll" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060045B9 RID: 17849 RVA: 0x001299A4 File Offset: 0x00127BA4
		protected virtual void OnScroll(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TrackBar.EVENT_SCROLL];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x060045BA RID: 17850 RVA: 0x001299D4 File Offset: 0x00127BD4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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
			int mouseWheelScrollLines = SystemInformation.MouseWheelScrollLines;
			if (mouseWheelScrollLines == 0)
			{
				return;
			}
			this.cumulativeWheelData += e.Delta;
			float num = (float)this.cumulativeWheelData / 120f;
			if (mouseWheelScrollLines == -1)
			{
				mouseWheelScrollLines = this.TickFrequency;
			}
			int num2 = (int)((float)mouseWheelScrollLines * num);
			if (num2 != 0)
			{
				if (num2 > 0)
				{
					int num3 = num2;
					this.Value = Math.Min(num3 + this.Value, this.Maximum);
					this.cumulativeWheelData -= (int)((float)num2 * (120f / (float)mouseWheelScrollLines));
				}
				else
				{
					int num3 = -num2;
					this.Value = Math.Max(this.Value - num3, this.Minimum);
					this.cumulativeWheelData -= (int)((float)num2 * (120f / (float)mouseWheelScrollLines));
				}
			}
			if (e.Delta != this.Value)
			{
				this.OnScroll(EventArgs.Empty);
				this.OnValueChanged(EventArgs.Empty);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TrackBar.ValueChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060045BB RID: 17851 RVA: 0x00129AF0 File Offset: 0x00127CF0
		protected virtual void OnValueChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TrackBar.EVENT_VALUECHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060045BC RID: 17852 RVA: 0x00129B1E File Offset: 0x00127D1E
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.RedrawControl();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.SystemColorsChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060045BD RID: 17853 RVA: 0x00129B2D File Offset: 0x00127D2D
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			this.RedrawControl();
		}

		/// <summary>Performs the work of setting the specified bounds of this control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x060045BE RID: 17854 RVA: 0x00129B3C File Offset: 0x00127D3C
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			this.requestedDim = ((this.orientation == Orientation.Horizontal) ? height : width);
			if (this.autoSize)
			{
				if (this.orientation == Orientation.Horizontal)
				{
					if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
					{
						height = this.PreferredDimension;
					}
				}
				else if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
				{
					width = this.PreferredDimension;
				}
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// <summary>Sets the minimum and maximum values for a <see cref="T:System.Windows.Forms.TrackBar" />.</summary>
		/// <param name="minValue">The lower limit of the range of the track bar. </param>
		/// <param name="maxValue">The upper limit of the range of the track bar. </param>
		// Token: 0x060045BF RID: 17855 RVA: 0x00129B98 File Offset: 0x00127D98
		public void SetRange(int minValue, int maxValue)
		{
			if (this.minimum != minValue || this.maximum != maxValue)
			{
				if (minValue > maxValue)
				{
					maxValue = minValue;
				}
				this.minimum = minValue;
				this.maximum = maxValue;
				if (base.IsHandleCreated)
				{
					base.SendMessage(1031, 0, this.minimum);
					base.SendMessage(1032, 1, this.maximum);
					base.Invalidate();
				}
				if (this.value < this.minimum)
				{
					this.value = this.minimum;
				}
				if (this.value > this.maximum)
				{
					this.value = this.maximum;
				}
				this.SetTrackBarPosition();
			}
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x00129C40 File Offset: 0x00127E40
		private void SetTrackBarPosition()
		{
			if (base.IsHandleCreated)
			{
				int lparam = this.value;
				if (this.orientation == Orientation.Vertical)
				{
					lparam = this.Minimum + this.Maximum - this.value;
				}
				if (this.orientation == Orientation.Horizontal && this.RightToLeft == RightToLeft.Yes && !base.IsMirrored)
				{
					lparam = this.Minimum + this.Maximum - this.value;
				}
				base.SendMessage(1029, 1, lparam);
			}
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.TrackBar" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.TrackBar" />. </returns>
		// Token: 0x060045C1 RID: 17857 RVA: 0x00129CB8 File Offset: 0x00127EB8
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

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" /> method.</summary>
		/// <param name="m">A Windows Message object. </param>
		// Token: 0x060045C2 RID: 17858 RVA: 0x00129D38 File Offset: 0x00127F38
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int num = m.Msg;
			if (num - 8468 <= 1)
			{
				num = NativeMethods.Util.LOWORD(m.WParam);
				if ((num <= 3 || num - 5 <= 3) && this.value != this.Value)
				{
					this.OnScroll(EventArgs.Empty);
					this.OnValueChanged(EventArgs.Empty);
					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x04002615 RID: 9749
		private static readonly object EVENT_SCROLL = new object();

		// Token: 0x04002616 RID: 9750
		private static readonly object EVENT_VALUECHANGED = new object();

		// Token: 0x04002617 RID: 9751
		private static readonly object EVENT_RIGHTTOLEFTLAYOUTCHANGED = new object();

		// Token: 0x04002618 RID: 9752
		private bool autoSize = true;

		// Token: 0x04002619 RID: 9753
		private int largeChange = 5;

		// Token: 0x0400261A RID: 9754
		private int maximum = 10;

		// Token: 0x0400261B RID: 9755
		private int minimum;

		// Token: 0x0400261C RID: 9756
		private Orientation orientation;

		// Token: 0x0400261D RID: 9757
		private int value;

		// Token: 0x0400261E RID: 9758
		private int smallChange = 1;

		// Token: 0x0400261F RID: 9759
		private int tickFrequency = 1;

		// Token: 0x04002620 RID: 9760
		private TickStyle tickStyle = TickStyle.BottomRight;

		// Token: 0x04002621 RID: 9761
		private int requestedDim;

		// Token: 0x04002622 RID: 9762
		private int cumulativeWheelData;

		// Token: 0x04002623 RID: 9763
		private bool initializing;

		// Token: 0x04002624 RID: 9764
		private bool rightToLeftLayout;
	}
}
