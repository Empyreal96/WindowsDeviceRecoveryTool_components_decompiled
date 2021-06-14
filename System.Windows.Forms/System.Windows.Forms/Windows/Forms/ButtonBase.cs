using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Implements the basic functionality common to button controls.</summary>
	// Token: 0x02000133 RID: 307
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ButtonBaseDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public abstract class ButtonBase : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ButtonBase" /> class.</summary>
		// Token: 0x06000919 RID: 2329 RVA: 0x0001B95C File Offset: 0x00019B5C
		protected ButtonBase()
		{
			base.SetStyle(ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.StandardClick | ControlStyles.SupportsTransparentBackColor | ControlStyles.CacheText | ControlStyles.OptimizedDoubleBuffer, true);
			base.SetState2(2048, true);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.UserMouse, this.OwnerDraw);
			this.SetFlag(128, true);
			this.SetFlag(256, false);
		}

		/// <summary>Gets or sets a value indicating whether the ellipsis character (...) appears at the right edge of the control, denoting that the control text extends beyond the specified length of the control.</summary>
		/// <returns>
		///     <see langword="true" /> if the additional label text is to be indicated by an ellipsis; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0001B9D9 File Offset: 0x00019BD9
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x0001B9E3 File Offset: 0x00019BE3
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("ButtonAutoEllipsisDescr")]
		public bool AutoEllipsis
		{
			get
			{
				return this.GetFlag(32);
			}
			set
			{
				if (this.AutoEllipsis != value)
				{
					this.SetFlag(32, value);
					if (value && this.textToolTip == null)
					{
						this.textToolTip = new ToolTip();
					}
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether the control resizes based on its contents.</summary>
		/// <returns>
		///     <see langword="true" /> if the control automatically resizes based on its contents; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x0001BA1B File Offset: 0x00019C1B
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
				if (value)
				{
					this.AutoEllipsis = false;
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ButtonBase.AutoSize" /> property changes.</summary>
		// Token: 0x14000049 RID: 73
		// (add) Token: 0x0600091E RID: 2334 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x0600091F RID: 2335 RVA: 0x0001BA37 File Offset: 0x00019C37
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

		/// <summary>Gets or sets the background color of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value representing the background color.</returns>
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x0001BA40 File Offset: 0x00019C40
		[SRCategory("CatAppearance")]
		[SRDescription("ControlBackColorDescr")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				if (base.DesignMode)
				{
					if (value != Color.Empty)
					{
						PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["UseVisualStyleBackColor"];
						if (propertyDescriptor != null)
						{
							propertyDescriptor.SetValue(this, false);
						}
					}
				}
				else
				{
					this.UseVisualStyleBackColor = false;
				}
				base.BackColor = value;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00012055 File Offset: 0x00010255
		protected override Size DefaultSize
		{
			get
			{
				return new Size(75, 23);
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0001BA94 File Offset: 0x00019C94
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (!this.OwnerDraw)
				{
					createParams.ExStyle &= -4097;
					createParams.Style |= 8192;
					if (this.IsDefault)
					{
						createParams.Style |= 1;
					}
					ContentAlignment contentAlignment = base.RtlTranslateContent(this.TextAlign);
					if ((contentAlignment & WindowsFormsUtils.AnyLeftAlign) != (ContentAlignment)0)
					{
						createParams.Style |= 256;
					}
					else if ((contentAlignment & WindowsFormsUtils.AnyRightAlign) != (ContentAlignment)0)
					{
						createParams.Style |= 512;
					}
					else
					{
						createParams.Style |= 768;
					}
					if ((contentAlignment & WindowsFormsUtils.AnyTopAlign) != (ContentAlignment)0)
					{
						createParams.Style |= 1024;
					}
					else if ((contentAlignment & WindowsFormsUtils.AnyBottomAlign) != (ContentAlignment)0)
					{
						createParams.Style |= 2048;
					}
					else
					{
						createParams.Style |= 3072;
					}
				}
				return createParams;
			}
		}

		/// <summary>Gets the default Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets or sets a value indicating whether the button control is the default button.</summary>
		/// <returns>
		///     <see langword="true" /> if the button control is the default button; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0001BB96 File Offset: 0x00019D96
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x0001BBA0 File Offset: 0x00019DA0
		protected internal bool IsDefault
		{
			get
			{
				return this.GetFlag(64);
			}
			set
			{
				if (this.GetFlag(64) != value)
				{
					this.SetFlag(64, value);
					if (base.IsHandleCreated)
					{
						if (this.OwnerDraw)
						{
							base.Invalidate();
							return;
						}
						base.UpdateStyles();
					}
				}
			}
		}

		/// <summary>Gets or sets the flat style appearance of the button control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatStyle" /> values. The default value is <see langword="Standard" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.FlatStyle" /> values. </exception>
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0001BBD3 File Offset: 0x00019DD3
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0001BBDC File Offset: 0x00019DDC
		[SRCategory("CatAppearance")]
		[DefaultValue(FlatStyle.Standard)]
		[Localizable(true)]
		[SRDescription("ButtonFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				return this.flatStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				this.flatStyle = value;
				LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.FlatStyle);
				base.Invalidate();
				this.UpdateOwnerDraw();
			}
		}

		/// <summary>Gets the appearance of the border and the colors used to indicate check state and mouse state.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatButtonAppearance" /> values.</returns>
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0001BC39 File Offset: 0x00019E39
		[Browsable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonFlatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public FlatButtonAppearance FlatAppearance
		{
			get
			{
				if (this.flatAppearance == null)
				{
					this.flatAppearance = new FlatButtonAppearance(this);
				}
				return this.flatAppearance;
			}
		}

		/// <summary>Gets or sets the image that is displayed on a button control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> displayed on the button control. The default value is <see langword="null" />.</returns>
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0001BC58 File Offset: 0x00019E58
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0001BCC4 File Offset: 0x00019EC4
		[SRDescription("ButtonImageDescr")]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		public Image Image
		{
			get
			{
				if (this.image == null && this.imageList != null)
				{
					int num = this.imageIndex.ActualIndex;
					if (num >= this.imageList.Images.Count)
					{
						num = this.imageList.Images.Count - 1;
					}
					if (num >= 0)
					{
						return this.imageList.Images[num];
					}
				}
				return this.image;
			}
			set
			{
				if (this.Image != value)
				{
					this.StopAnimate();
					this.image = value;
					if (this.image != null)
					{
						this.ImageIndex = -1;
						this.ImageList = null;
					}
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Image);
					this.Animate();
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the alignment of the image on the button control.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default value is <see langword="MiddleCenter" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values. </exception>
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0001BD20 File Offset: 0x00019F20
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0001BD28 File Offset: 0x00019F28
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Localizable(true)]
		[SRDescription("ButtonImageAlignDescr")]
		[SRCategory("CatAppearance")]
		public ContentAlignment ImageAlign
		{
			get
			{
				return this.imageAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (value != this.imageAlign)
				{
					this.imageAlign = value;
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.ImageAlign);
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the image list index value of the image displayed on the button control.</summary>
		/// <returns>A zero-based index, which represents the image position in an <see cref="T:System.Windows.Forms.ImageList" />. The default is -1.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than the lower bounds of the <see cref="P:System.Windows.Forms.ButtonBase.ImageIndex" />. </exception>
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0001BD80 File Offset: 0x00019F80
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x0001BDE0 File Offset: 0x00019FE0
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[DefaultValue(-1)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ButtonImageIndexDescr")]
		[SRCategory("CatAppearance")]
		public int ImageIndex
		{
			get
			{
				if (this.imageIndex.Index != -1 && this.imageList != null && this.imageIndex.Index >= this.imageList.Images.Count)
				{
					return this.imageList.Images.Count - 1;
				}
				return this.imageIndex.Index;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						-1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.imageIndex.Index != value)
				{
					if (value != -1)
					{
						this.image = null;
					}
					this.imageIndex.Index = value;
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the key accessor for the image in the <see cref="P:System.Windows.Forms.ButtonBase.ImageList" />.</summary>
		/// <returns>A string representing the key of the image.</returns>
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0001BE5F File Offset: 0x0001A05F
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x0001BE6C File Offset: 0x0001A06C
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ButtonImageIndexDescr")]
		[SRCategory("CatAppearance")]
		public string ImageKey
		{
			get
			{
				return this.imageIndex.Key;
			}
			set
			{
				if (this.imageIndex.Key != value)
				{
					if (value != null)
					{
						this.image = null;
					}
					this.imageIndex.Key = value;
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ImageList" /> that contains the <see cref="T:System.Drawing.Image" /> displayed on a button control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageList" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0001BE9D File Offset: 0x0001A09D
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x0001BEA8 File Offset: 0x0001A0A8
		[DefaultValue(null)]
		[SRDescription("ButtonImageListDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatAppearance")]
		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				if (this.imageList != value)
				{
					EventHandler value2 = new EventHandler(this.ImageListRecreateHandle);
					EventHandler value3 = new EventHandler(this.DetachImageList);
					if (this.imageList != null)
					{
						this.imageList.RecreateHandle -= value2;
						this.imageList.Disposed -= value3;
					}
					if (value != null)
					{
						this.image = null;
					}
					this.imageList = value;
					this.imageIndex.ImageList = value;
					if (value != null)
					{
						value.RecreateHandle += value2;
						value.Disposed += value3;
					}
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the Input Method Editor (IME) mode supported by this control. This property is not relevant for this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x00011FEC File Offset: 0x000101EC
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ButtonBase.ImeMode" /> property is changed. This event is not relevant for this class.</summary>
		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06000936 RID: 2358 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x06000937 RID: 2359 RVA: 0x0001BF35 File Offset: 0x0001A135
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

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0001BF3E File Offset: 0x0001A13E
		internal virtual Rectangle OverChangeRectangle
		{
			get
			{
				if (this.FlatStyle == FlatStyle.Standard)
				{
					return new Rectangle(-1, -1, 1, 1);
				}
				return base.ClientRectangle;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0001BF59 File Offset: 0x0001A159
		internal bool OwnerDraw
		{
			get
			{
				return this.FlatStyle != FlatStyle.System;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0001BF67 File Offset: 0x0001A167
		internal virtual Rectangle DownChangeRectangle
		{
			get
			{
				return base.ClientRectangle;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0001BF6F File Offset: 0x0001A16F
		internal bool MouseIsPressed
		{
			get
			{
				return this.GetFlag(4);
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0001BF78 File Offset: 0x0001A178
		internal bool MouseIsDown
		{
			get
			{
				return this.GetFlag(2);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0001BF81 File Offset: 0x0001A181
		internal bool MouseIsOver
		{
			get
			{
				return this.GetFlag(1);
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0001BF8A File Offset: 0x0001A18A
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0001BF97 File Offset: 0x0001A197
		internal bool ShowToolTip
		{
			get
			{
				return this.GetFlag(256);
			}
			set
			{
				this.SetFlag(256, value);
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SettingsBindable(true)]
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

		/// <summary>Gets or sets the alignment of the text on the button control.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default is <see langword="MiddleCenter" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values. </exception>
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x0001BFB6 File Offset: 0x0001A1B6
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x0001BFC0 File Offset: 0x0001A1C0
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Localizable(true)]
		[SRDescription("ButtonTextAlignDescr")]
		[SRCategory("CatAppearance")]
		public virtual ContentAlignment TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (value != this.textAlign)
				{
					this.textAlign = value;
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.TextAlign);
					if (this.OwnerDraw)
					{
						base.Invalidate();
						return;
					}
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets or sets the position of text and image relative to each other.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.TextImageRelation" />. The default is <see cref="F:System.Windows.Forms.TextImageRelation.Overlay" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value is not one of the <see cref="T:System.Windows.Forms.TextImageRelation" /> values.</exception>
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0001C027 File Offset: 0x0001A227
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x0001C030 File Offset: 0x0001A230
		[DefaultValue(TextImageRelation.Overlay)]
		[Localizable(true)]
		[SRDescription("ButtonTextImageRelationDescr")]
		[SRCategory("CatAppearance")]
		public TextImageRelation TextImageRelation
		{
			get
			{
				return this.textImageRelation;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidTextImageRelation(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TextImageRelation));
				}
				if (value != this.TextImageRelation)
				{
					this.textImageRelation = value;
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.TextImageRelation);
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the first character that is preceded by an ampersand (&amp;) is used as the mnemonic key of the control.</summary>
		/// <returns>
		///     <see langword="true" /> if the first character that is preceded by an ampersand (&amp;) is used as the mnemonic key of the control; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0001C088 File Offset: 0x0001A288
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x0001C095 File Offset: 0x0001A295
		[SRDescription("ButtonUseMnemonicDescr")]
		[DefaultValue(true)]
		[SRCategory("CatAppearance")]
		public bool UseMnemonic
		{
			get
			{
				return this.GetFlag(128);
			}
			set
			{
				this.SetFlag(128, value);
				LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text);
				base.Invalidate();
			}
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001C0C0 File Offset: 0x0001A2C0
		private void Animate()
		{
			this.Animate(!base.DesignMode && base.Visible && base.Enabled && this.ParentInternal != null);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001C0EC File Offset: 0x0001A2EC
		private void StopAnimate()
		{
			this.Animate(false);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
		private void Animate(bool animate)
		{
			if (animate != this.GetFlag(16))
			{
				if (animate)
				{
					if (this.image != null)
					{
						ImageAnimator.Animate(this.image, new EventHandler(this.OnFrameChanged));
						this.SetFlag(16, animate);
						return;
					}
				}
				else if (this.image != null)
				{
					ImageAnimator.StopAnimate(this.image, new EventHandler(this.OnFrameChanged));
					this.SetFlag(16, animate);
				}
			}
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x0600094C RID: 2380 RVA: 0x0001C164 File Offset: 0x0001A364
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ButtonBase.ButtonBaseAccessibleObject(this);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0001C16C File Offset: 0x0001A36C
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ButtonBase" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600094E RID: 2382 RVA: 0x0001C178 File Offset: 0x0001A378
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopAnimate();
				if (this.imageList != null)
				{
					this.imageList.Disposed -= this.DetachImageList;
				}
				if (this.textToolTip != null)
				{
					this.textToolTip.Dispose();
					this.textToolTip = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001C1CE File Offset: 0x0001A3CE
		private bool GetFlag(int flag)
		{
			return (this.state & flag) == flag;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001C1DB File Offset: 0x0001A3DB
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000951 RID: 2385 RVA: 0x0001C1EB File Offset: 0x0001A3EB
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			base.Invalidate();
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnLostFocus(System.EventArgs)" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000952 RID: 2386 RVA: 0x0001C1FA File Offset: 0x0001A3FA
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.SetFlag(2, false);
			base.CaptureInternal = false;
			base.Invalidate();
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseEnter(System.EventArgs)" /> event.</summary>
		/// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000953 RID: 2387 RVA: 0x0001C218 File Offset: 0x0001A418
		protected override void OnMouseEnter(EventArgs eventargs)
		{
			this.SetFlag(1, true);
			base.Invalidate();
			if (!base.DesignMode && this.AutoEllipsis && this.ShowToolTip && this.textToolTip != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					this.textToolTip.Show(WindowsFormsUtils.TextWithoutMnemonics(this.Text), this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			base.OnMouseEnter(eventargs);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseLeave(System.EventArgs)" /> event.</summary>
		/// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000954 RID: 2388 RVA: 0x0001C294 File Offset: 0x0001A494
		protected override void OnMouseLeave(EventArgs eventargs)
		{
			this.SetFlag(1, false);
			if (this.textToolTip != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					this.textToolTip.Hide(this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			base.Invalidate();
			base.OnMouseLeave(eventargs);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseMove(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000955 RID: 2389 RVA: 0x0001C2EC File Offset: 0x0001A4EC
		protected override void OnMouseMove(MouseEventArgs mevent)
		{
			if (mevent.Button != MouseButtons.None && this.GetFlag(4))
			{
				if (!base.ClientRectangle.Contains(mevent.X, mevent.Y))
				{
					if (this.GetFlag(2))
					{
						this.SetFlag(2, false);
						base.Invalidate(this.DownChangeRectangle);
					}
				}
				else if (!this.GetFlag(2))
				{
					this.SetFlag(2, true);
					base.Invalidate(this.DownChangeRectangle);
				}
			}
			base.OnMouseMove(mevent);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseDown(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000956 RID: 2390 RVA: 0x0001C369 File Offset: 0x0001A569
		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			if (mevent.Button == MouseButtons.Left)
			{
				this.SetFlag(2, true);
				this.SetFlag(4, true);
				base.Invalidate(this.DownChangeRectangle);
			}
			base.OnMouseDown(mevent);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnMouseUp(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06000957 RID: 2391 RVA: 0x0001C39B File Offset: 0x0001A59B
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
		}

		/// <summary>Resets the <see cref="T:System.Windows.Forms.Button" /> control to the state before it is pressed and redraws it.</summary>
		// Token: 0x06000958 RID: 2392 RVA: 0x0001C3A4 File Offset: 0x0001A5A4
		protected void ResetFlagsandPaint()
		{
			this.SetFlag(4, false);
			this.SetFlag(2, false);
			base.Invalidate(this.DownChangeRectangle);
			base.Update();
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0001C3C8 File Offset: 0x0001A5C8
		private void PaintControl(PaintEventArgs pevent)
		{
			this.Adapter.Paint(pevent);
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fitted.</summary>
		/// <param name="proposedSize">The custom-sized area for a control.</param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x0600095A RID: 2394 RVA: 0x0001C3D6 File Offset: 0x0001A5D6
		public override Size GetPreferredSize(Size proposedSize)
		{
			if (proposedSize.Width == 1)
			{
				proposedSize.Width = 0;
			}
			if (proposedSize.Height == 1)
			{
				proposedSize.Height = 0;
			}
			return base.GetPreferredSize(proposedSize);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0001C404 File Offset: 0x0001A604
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size preferredSizeCore = this.Adapter.GetPreferredSizeCore(proposedConstraints);
			return LayoutUtils.UnionSizes(preferredSizeCore + base.Padding.Size, this.MinimumSize);
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0001C440 File Offset: 0x0001A640
		internal ButtonBaseAdapter Adapter
		{
			get
			{
				if (this._adapter == null || this.FlatStyle != this._cachedAdapterType)
				{
					switch (this.FlatStyle)
					{
					case FlatStyle.Flat:
						this._adapter = this.CreateFlatAdapter();
						break;
					case FlatStyle.Popup:
						this._adapter = this.CreatePopupAdapter();
						break;
					case FlatStyle.Standard:
						this._adapter = this.CreateStandardAdapter();
						break;
					}
					this._cachedAdapterType = this.FlatStyle;
				}
				return this._adapter;
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual ButtonBaseAdapter CreateFlatAdapter()
		{
			return null;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual ButtonBaseAdapter CreatePopupAdapter()
		{
			return null;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual ButtonBaseAdapter CreateStandardAdapter()
		{
			return null;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001C4B8 File Offset: 0x0001A6B8
		internal virtual StringFormat CreateStringFormat()
		{
			if (this.Adapter == null)
			{
				return new StringFormat();
			}
			return this.Adapter.CreateStringFormat();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001C4D3 File Offset: 0x0001A6D3
		internal virtual TextFormatFlags CreateTextFormatFlags()
		{
			if (this.Adapter == null)
			{
				return TextFormatFlags.Default;
			}
			return this.Adapter.CreateTextFormatFlags();
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0001C4EC File Offset: 0x0001A6EC
		private void OnFrameChanged(object o, EventArgs e)
		{
			if (base.Disposing || base.IsDisposed)
			{
				return;
			}
			if (base.IsHandleCreated && base.InvokeRequired)
			{
				base.BeginInvoke(new EventHandler(this.OnFrameChanged), new object[]
				{
					o,
					e
				});
				return;
			}
			base.Invalidate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000963 RID: 2403 RVA: 0x0001C542 File Offset: 0x0001A742
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Animate();
			if (!base.Enabled)
			{
				this.SetFlag(2, false);
				this.SetFlag(1, false);
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000964 RID: 2404 RVA: 0x0001C570 File Offset: 0x0001A770
		protected override void OnTextChanged(EventArgs e)
		{
			using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text))
			{
				base.OnTextChanged(e);
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)" /> event.</summary>
		/// <param name="kevent">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06000965 RID: 2405 RVA: 0x0001C5C0 File Offset: 0x0001A7C0
		protected override void OnKeyDown(KeyEventArgs kevent)
		{
			if (kevent.KeyData == Keys.Space)
			{
				if (!this.GetFlag(2))
				{
					this.SetFlag(2, true);
					if (!this.OwnerDraw)
					{
						base.SendMessage(243, 1, 0);
					}
					base.Invalidate(this.DownChangeRectangle);
				}
				kevent.Handled = true;
			}
			base.OnKeyDown(kevent);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)" /> event.</summary>
		/// <param name="kevent">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06000966 RID: 2406 RVA: 0x0001C618 File Offset: 0x0001A818
		protected override void OnKeyUp(KeyEventArgs kevent)
		{
			if (this.GetFlag(2) && !base.ValidationCancelled)
			{
				if (this.OwnerDraw)
				{
					this.ResetFlagsandPaint();
				}
				else
				{
					this.SetFlag(4, false);
					this.SetFlag(2, false);
					base.SendMessage(243, 0, 0);
				}
				if (kevent.KeyCode == Keys.Return || kevent.KeyCode == Keys.Space)
				{
					this.OnClick(EventArgs.Empty);
				}
				kevent.Handled = true;
			}
			base.OnKeyUp(kevent);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.</summary>
		/// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06000967 RID: 2407 RVA: 0x0001C694 File Offset: 0x0001A894
		protected override void OnPaint(PaintEventArgs pevent)
		{
			if (this.AutoEllipsis)
			{
				Size preferredSize = base.PreferredSize;
				this.ShowToolTip = (base.ClientRectangle.Width < preferredSize.Width || base.ClientRectangle.Height < preferredSize.Height);
			}
			else
			{
				this.ShowToolTip = false;
			}
			if (base.GetStyle(ControlStyles.UserPaint))
			{
				this.Animate();
				ImageAnimator.UpdateFrames(this.Image);
				this.PaintControl(pevent);
			}
			base.OnPaint(pevent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000968 RID: 2408 RVA: 0x0001C718 File Offset: 0x0001A918
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			this.Animate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data. </param>
		// Token: 0x06000969 RID: 2409 RVA: 0x0001C727 File Offset: 0x0001A927
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Animate();
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001C736 File Offset: 0x0001A936
		private void ResetImage()
		{
			this.Image = null;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001C740 File Offset: 0x0001A940
		private void SetFlag(int flag, bool value)
		{
			bool flag2 = (this.state & flag) != 0;
			if (value)
			{
				this.state |= flag;
			}
			else
			{
				this.state &= ~flag;
			}
			if (this.OwnerDraw && (flag & 2) != 0 && value != flag2)
			{
				base.AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001C798 File Offset: 0x0001A998
		private bool ShouldSerializeImage()
		{
			return this.image != null;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001C7A3 File Offset: 0x0001A9A3
		private void UpdateOwnerDraw()
		{
			if (this.OwnerDraw != base.GetStyle(ControlStyles.UserPaint))
			{
				base.SetStyle(ControlStyles.UserPaint | ControlStyles.UserMouse, this.OwnerDraw);
				base.RecreateHandle();
			}
		}

		/// <summary>Gets or sets a value that determines whether to use the <see cref="T:System.Drawing.Graphics" /> class (GDI+) or the <see cref="T:System.Windows.Forms.TextRenderer" /> class (GDI) to render text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Drawing.Graphics" /> class should be used to perform text rendering for compatibility with versions 1.0 and 1.1. of the .NET Framework; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0001C7CB File Offset: 0x0001A9CB
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0001C7D3 File Offset: 0x0001A9D3
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("UseCompatibleTextRenderingDescr")]
		public bool UseCompatibleTextRendering
		{
			get
			{
				return base.UseCompatibleTextRenderingInt;
			}
			set
			{
				base.UseCompatibleTextRenderingInt = value;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a value that determines if the background is drawn using visual styles, if supported.</summary>
		/// <returns>
		///     <see langword="true" /> if the background is drawn using visual styles; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0001C7DC File Offset: 0x0001A9DC
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0001C81B File Offset: 0x0001AA1B
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonUseVisualStyleBackColorDescr")]
		public bool UseVisualStyleBackColor
		{
			get
			{
				return (this.isEnableVisualStyleBackgroundSet || (base.RawBackColor.IsEmpty && this.BackColor == SystemColors.Control)) && this.enableVisualStyleBackground;
			}
			set
			{
				this.isEnableVisualStyleBackgroundSet = true;
				this.enableVisualStyleBackground = value;
				base.Invalidate();
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001C831 File Offset: 0x0001AA31
		private void ResetUseVisualStyleBackColor()
		{
			this.isEnableVisualStyleBackgroundSet = false;
			this.enableVisualStyleBackground = true;
			base.Invalidate();
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0001C847 File Offset: 0x0001AA47
		private bool ShouldSerializeUseVisualStyleBackColor()
		{
			return this.isEnableVisualStyleBackgroundSet;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06000975 RID: 2421 RVA: 0x0001C850 File Offset: 0x0001AA50
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 245)
			{
				if (this.OwnerDraw)
				{
					msg = m.Msg;
					if (msg > 243)
					{
						if (msg <= 517)
						{
							if (msg != 514 && msg != 517)
							{
								goto IL_E6;
							}
						}
						else if (msg != 520)
						{
							if (msg == 533)
							{
								goto IL_8C;
							}
							goto IL_E6;
						}
						try
						{
							this.SetFlag(8, true);
							base.WndProc(ref m);
							return;
						}
						finally
						{
							this.SetFlag(8, false);
						}
						goto IL_E6;
					}
					if (msg != 8 && msg != 31)
					{
						if (msg != 243)
						{
							goto IL_E6;
						}
						return;
					}
					IL_8C:
					if (!this.GetFlag(8) && this.GetFlag(4))
					{
						this.SetFlag(4, false);
						if (this.GetFlag(2))
						{
							this.SetFlag(2, false);
							base.Invalidate(this.DownChangeRectangle);
						}
					}
					base.WndProc(ref m);
					return;
					IL_E6:
					base.WndProc(ref m);
					return;
				}
				msg = m.Msg;
				if (msg == 8465)
				{
					if (NativeMethods.Util.HIWORD(m.WParam) == 0 && !base.ValidationCancelled)
					{
						this.OnClick(EventArgs.Empty);
						return;
					}
				}
				else
				{
					base.WndProc(ref m);
				}
				return;
			}
			if (this is IButtonControl)
			{
				((IButtonControl)this).PerformClick();
				return;
			}
			this.OnClick(EventArgs.Empty);
		}

		// Token: 0x04000676 RID: 1654
		private FlatStyle flatStyle = FlatStyle.Standard;

		// Token: 0x04000677 RID: 1655
		private ContentAlignment imageAlign = ContentAlignment.MiddleCenter;

		// Token: 0x04000678 RID: 1656
		private ContentAlignment textAlign = ContentAlignment.MiddleCenter;

		// Token: 0x04000679 RID: 1657
		private TextImageRelation textImageRelation;

		// Token: 0x0400067A RID: 1658
		private ImageList.Indexer imageIndex = new ImageList.Indexer();

		// Token: 0x0400067B RID: 1659
		private FlatButtonAppearance flatAppearance;

		// Token: 0x0400067C RID: 1660
		private ImageList imageList;

		// Token: 0x0400067D RID: 1661
		private Image image;

		// Token: 0x0400067E RID: 1662
		private const int FlagMouseOver = 1;

		// Token: 0x0400067F RID: 1663
		private const int FlagMouseDown = 2;

		// Token: 0x04000680 RID: 1664
		private const int FlagMousePressed = 4;

		// Token: 0x04000681 RID: 1665
		private const int FlagInButtonUp = 8;

		// Token: 0x04000682 RID: 1666
		private const int FlagCurrentlyAnimating = 16;

		// Token: 0x04000683 RID: 1667
		private const int FlagAutoEllipsis = 32;

		// Token: 0x04000684 RID: 1668
		private const int FlagIsDefault = 64;

		// Token: 0x04000685 RID: 1669
		private const int FlagUseMnemonic = 128;

		// Token: 0x04000686 RID: 1670
		private const int FlagShowToolTip = 256;

		// Token: 0x04000687 RID: 1671
		private int state;

		// Token: 0x04000688 RID: 1672
		private ToolTip textToolTip;

		// Token: 0x04000689 RID: 1673
		private bool enableVisualStyleBackground = true;

		// Token: 0x0400068A RID: 1674
		private bool isEnableVisualStyleBackgroundSet;

		// Token: 0x0400068B RID: 1675
		private ButtonBaseAdapter _adapter;

		// Token: 0x0400068C RID: 1676
		private FlatStyle _cachedAdapterType;

		/// <summary>Provides information that accessibility applications use to adjust an application's user interface for users with disabilities.</summary>
		// Token: 0x0200055F RID: 1375
		[ComVisible(true)]
		public class ButtonBaseAccessibleObject : Control.ControlAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ButtonBase.ButtonBaseAccessibleObject" /> class. </summary>
			/// <param name="owner">The owner of this <see cref="T:System.Windows.Forms.ButtonBase.ButtonBaseAccessibleObject" />.</param>
			// Token: 0x0600562A RID: 22058 RVA: 0x00093572 File Offset: 0x00091772
			public ButtonBaseAccessibleObject(Control owner) : base(owner)
			{
			}

			/// <summary>Performs the default action associated with this accessible object.</summary>
			// Token: 0x0600562B RID: 22059 RVA: 0x0016989D File Offset: 0x00167A9D
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				((ButtonBase)base.Owner).OnClick(EventArgs.Empty);
			}

			/// <summary>Gets the state of this accessible object.</summary>
			/// <returns>One of the <see cref="T:System.Windows.Forms.AccessibleStates" /> values.</returns>
			// Token: 0x17001478 RID: 5240
			// (get) Token: 0x0600562C RID: 22060 RVA: 0x001698B4 File Offset: 0x00167AB4
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = base.State;
					ButtonBase buttonBase = (ButtonBase)base.Owner;
					if (buttonBase.OwnerDraw && buttonBase.MouseIsDown)
					{
						accessibleStates |= AccessibleStates.Pressed;
					}
					return accessibleStates;
				}
			}
		}
	}
}
