using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Automation;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a standard Windows label.</summary>
	// Token: 0x020002A9 RID: 681
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Text")]
	[DefaultBindingProperty("Text")]
	[Designer("System.Windows.Forms.Design.LabelDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionLabel")]
	public class Label : Control, IAutomationLiveRegion
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Label" /> class.</summary>
		// Token: 0x0600274B RID: 10059 RVA: 0x000B857C File Offset: 0x000B677C
		public Label()
		{
			base.SetState2(2048, true);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, this.IsOwnerDraw());
			base.SetStyle(ControlStyles.FixedHeight | ControlStyles.Selectable, false);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			CommonProperties.SetSelfAutoSizeInDefaultLayout(this, true);
			this.labelState[Label.StateFlatStyle] = 2;
			this.labelState[Label.StateUseMnemonic] = 1;
			this.labelState[Label.StateBorderStyle] = 0;
			this.TabStop = false;
			this.requestedHeight = base.Height;
			this.requestedWidth = base.Width;
		}

		/// <summary>Gets or sets a value indicating whether the control is automatically resized to display its entire contents.</summary>
		/// <returns>
		///     <see langword="true" /> if the control adjusts its width to closely fit its contents; otherwise, <see langword="false" />. When added to a form using the designer, the default value is <see langword="true" />. When instantiated from code, the default value is <see langword="false" />.</returns>
		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x0600274C RID: 10060 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x0600274D RID: 10061 RVA: 0x000B861A File Offset: 0x000B681A
		[SRCategory("CatLayout")]
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.All)]
		[Localizable(true)]
		[SRDescription("LabelAutoSizeDescr")]
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
				if (this.AutoSize != value)
				{
					base.AutoSize = value;
					this.AdjustSize();
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Label.AutoSize" /> property changes.</summary>
		// Token: 0x140001D0 RID: 464
		// (add) Token: 0x0600274E RID: 10062 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x0600274F RID: 10063 RVA: 0x0001BA37 File Offset: 0x00019C37
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

		/// <summary>Gets or sets a value indicating whether the ellipsis character (...) appears at the right edge of the <see cref="T:System.Windows.Forms.Label" />, denoting that the <see cref="T:System.Windows.Forms.Label" /> text extends beyond the specified length of the <see cref="T:System.Windows.Forms.Label" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the additional label text is to be indicated by an ellipsis; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06002750 RID: 10064 RVA: 0x000B8632 File Offset: 0x000B6832
		// (set) Token: 0x06002751 RID: 10065 RVA: 0x000B8648 File Offset: 0x000B6848
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRDescription("LabelAutoEllipsisDescr")]
		public bool AutoEllipsis
		{
			get
			{
				return this.labelState[Label.StateAutoEllipsis] != 0;
			}
			set
			{
				if (this.AutoEllipsis != value)
				{
					this.labelState[Label.StateAutoEllipsis] = (value ? 1 : 0);
					this.MeasureTextCache.InvalidateCache();
					this.OnAutoEllipsisChanged();
					if (value && this.textToolTip == null)
					{
						this.textToolTip = new ToolTip();
					}
					if (this.ParentInternal != null)
					{
						LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.AutoEllipsis);
					}
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the image rendered on the background of the control.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the background image of the control. The default is <see langword="null" />.</returns>
		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06002752 RID: 10066 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06002753 RID: 10067 RVA: 0x00011FCA File Offset: 0x000101CA
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("LabelBackgroundImageDescr")]
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Label.BackgroundImage" /> property changes. </summary>
		// Token: 0x140001D1 RID: 465
		// (add) Token: 0x06002754 RID: 10068 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06002755 RID: 10069 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageLayout" /> object.</returns>
		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06002756 RID: 10070 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06002757 RID: 10071 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Label.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140001D2 RID: 466
		// (add) Token: 0x06002758 RID: 10072 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06002759 RID: 10073 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets the border style for the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see langword="BorderStyle.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x0600275A RID: 10074 RVA: 0x000B86C1 File Offset: 0x000B68C1
		// (set) Token: 0x0600275B RID: 10075 RVA: 0x000B86D4 File Offset: 0x000B68D4
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.None)]
		[DispId(-504)]
		[SRDescription("LabelBorderDescr")]
		public virtual BorderStyle BorderStyle
		{
			get
			{
				return (BorderStyle)this.labelState[Label.StateBorderStyle];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.BorderStyle != value)
				{
					this.labelState[Label.StateBorderStyle] = (int)value;
					if (this.ParentInternal != null)
					{
						LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.BorderStyle);
					}
					if (this.AutoSize)
					{
						this.AdjustSize();
					}
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool CanUseTextRenderer
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x000B8754 File Offset: 0x000B6954
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "STATIC";
				if (this.OwnerDraw)
				{
					createParams.Style |= 13;
					createParams.ExStyle &= -4097;
				}
				if (!this.OwnerDraw)
				{
					ContentAlignment textAlign = this.TextAlign;
					if (textAlign <= ContentAlignment.MiddleCenter)
					{
						switch (textAlign)
						{
						case ContentAlignment.TopLeft:
							break;
						case ContentAlignment.TopCenter:
							goto IL_BF;
						case (ContentAlignment)3:
							goto IL_DD;
						case ContentAlignment.TopRight:
							goto IL_AF;
						default:
							if (textAlign != ContentAlignment.MiddleLeft)
							{
								if (textAlign != ContentAlignment.MiddleCenter)
								{
									goto IL_DD;
								}
								goto IL_BF;
							}
							break;
						}
					}
					else if (textAlign <= ContentAlignment.BottomLeft)
					{
						if (textAlign == ContentAlignment.MiddleRight)
						{
							goto IL_AF;
						}
						if (textAlign != ContentAlignment.BottomLeft)
						{
							goto IL_DD;
						}
					}
					else
					{
						if (textAlign == ContentAlignment.BottomCenter)
						{
							goto IL_BF;
						}
						if (textAlign != ContentAlignment.BottomRight)
						{
							goto IL_DD;
						}
						goto IL_AF;
					}
					createParams.Style |= 0;
					goto IL_DD;
					IL_AF:
					createParams.Style |= 2;
					goto IL_DD;
					IL_BF:
					createParams.Style |= 1;
				}
				else
				{
					createParams.Style |= 0;
				}
				IL_DD:
				BorderStyle borderStyle = this.BorderStyle;
				if (borderStyle != BorderStyle.FixedSingle)
				{
					if (borderStyle == BorderStyle.Fixed3D)
					{
						createParams.Style |= 4096;
					}
				}
				else
				{
					createParams.Style |= 8388608;
				}
				if (!this.UseMnemonic)
				{
					createParams.Style |= 128;
				}
				return createParams;
			}
		}

		/// <summary>Gets the default Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> supported by this control. The default is <see cref="F:System.Windows.Forms.ImeMode.Disable" />.</returns>
		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets the space, in pixels, that is specified by default between controls.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value that represents the default space between controls.</returns>
		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x000B8890 File Offset: 0x000B6A90
		protected override Padding DefaultMargin
		{
			get
			{
				return new Padding(3, 0, 3, 0);
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x000B889B File Offset: 0x000B6A9B
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, this.AutoSize ? this.PreferredHeight : 23);
			}
		}

		/// <summary>Gets or sets the flat style appearance of the label control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatStyle" /> values. The default value is <see langword="Standard" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.FlatStyle" /> values. </exception>
		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x000B88B6 File Offset: 0x000B6AB6
		// (set) Token: 0x06002762 RID: 10082 RVA: 0x000B88C8 File Offset: 0x000B6AC8
		[SRCategory("CatAppearance")]
		[DefaultValue(FlatStyle.Standard)]
		[SRDescription("ButtonFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				return (FlatStyle)this.labelState[Label.StateFlatStyle];
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				if (this.labelState[Label.StateFlatStyle] != (int)value)
				{
					bool flag = this.labelState[Label.StateFlatStyle] == 3 || value == FlatStyle.System;
					this.labelState[Label.StateFlatStyle] = (int)value;
					base.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, this.OwnerDraw);
					if (flag)
					{
						LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.BorderStyle);
						if (this.AutoSize)
						{
							this.AdjustSize();
						}
						base.RecreateHandle();
						return;
					}
					this.Refresh();
				}
			}
		}

		/// <summary>Gets or sets the image that is displayed on a <see cref="T:System.Windows.Forms.Label" />.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> displayed on the <see cref="T:System.Windows.Forms.Label" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x000B8980 File Offset: 0x000B6B80
		// (set) Token: 0x06002764 RID: 10084 RVA: 0x000B89D9 File Offset: 0x000B6BD9
		[Localizable(true)]
		[SRDescription("ButtonImageDescr")]
		[SRCategory("CatAppearance")]
		public Image Image
		{
			get
			{
				Image image = (Image)base.Properties.GetObject(Label.PropImage);
				if (image == null && this.ImageList != null && this.ImageIndexer.ActualIndex >= 0)
				{
					return this.ImageList.Images[this.ImageIndexer.ActualIndex];
				}
				return image;
			}
			set
			{
				if (this.Image != value)
				{
					this.StopAnimate();
					base.Properties.SetObject(Label.PropImage, value);
					if (value != null)
					{
						this.ImageIndex = -1;
						this.ImageList = null;
					}
					this.Animate();
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the index value of the image displayed on the <see cref="T:System.Windows.Forms.Label" />.</summary>
		/// <returns>A zero-based index that represents the position in the <see cref="T:System.Windows.Forms.ImageList" /> control (assigned to the <see cref="P:System.Windows.Forms.Label.ImageList" /> property) where the image is located. The default is -1.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value assigned is less than the lower bounds of the <see cref="P:System.Windows.Forms.Label.ImageIndex" /> property. </exception>
		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x000B8A18 File Offset: 0x000B6C18
		// (set) Token: 0x06002766 RID: 10086 RVA: 0x000B8A6C File Offset: 0x000B6C6C
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue(-1)]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ButtonImageIndexDescr")]
		[SRCategory("CatAppearance")]
		public int ImageIndex
		{
			get
			{
				if (this.ImageIndexer == null)
				{
					return -1;
				}
				int index = this.ImageIndexer.Index;
				if (this.ImageList != null && index >= this.ImageList.Images.Count)
				{
					return this.ImageList.Images.Count - 1;
				}
				return index;
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
				if (this.ImageIndex != value)
				{
					if (value != -1)
					{
						base.Properties.SetObject(Label.PropImage, null);
					}
					this.ImageIndexer.Index = value;
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the key accessor for the image in the <see cref="P:System.Windows.Forms.Label.ImageList" />.</summary>
		/// <returns>A string representing the key of the image.</returns>
		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x000B8AF0 File Offset: 0x000B6CF0
		// (set) Token: 0x06002768 RID: 10088 RVA: 0x000B8B07 File Offset: 0x000B6D07
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ButtonImageIndexDescr")]
		[SRCategory("CatAppearance")]
		public string ImageKey
		{
			get
			{
				if (this.ImageIndexer != null)
				{
					return this.ImageIndexer.Key;
				}
				return null;
			}
			set
			{
				if (this.ImageKey != value)
				{
					base.Properties.SetObject(Label.PropImage, null);
					this.ImageIndexer.Key = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x000B8B3C File Offset: 0x000B6D3C
		// (set) Token: 0x0600276A RID: 10090 RVA: 0x000B8B76 File Offset: 0x000B6D76
		internal LabelImageIndexer ImageIndexer
		{
			get
			{
				bool flag;
				LabelImageIndexer labelImageIndexer = base.Properties.GetObject(Label.PropImageIndex, out flag) as LabelImageIndexer;
				if (labelImageIndexer == null || !flag)
				{
					labelImageIndexer = new LabelImageIndexer(this);
					this.ImageIndexer = labelImageIndexer;
				}
				return labelImageIndexer;
			}
			set
			{
				base.Properties.SetObject(Label.PropImageIndex, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ImageList" /> that contains the images to display in the <see cref="T:System.Windows.Forms.Label" /> control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageList" /> that stores the collection of <see cref="T:System.Drawing.Image" /> objects. The default value is <see langword="null" />.</returns>
		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600276B RID: 10091 RVA: 0x000B8B89 File Offset: 0x000B6D89
		// (set) Token: 0x0600276C RID: 10092 RVA: 0x000B8BA0 File Offset: 0x000B6DA0
		[DefaultValue(null)]
		[SRDescription("ButtonImageListDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatAppearance")]
		public ImageList ImageList
		{
			get
			{
				return (ImageList)base.Properties.GetObject(Label.PropImageList);
			}
			set
			{
				if (this.ImageList != value)
				{
					EventHandler value2 = new EventHandler(this.ImageListRecreateHandle);
					EventHandler value3 = new EventHandler(this.DetachImageList);
					ImageList imageList = this.ImageList;
					if (imageList != null)
					{
						imageList.RecreateHandle -= value2;
						imageList.Disposed -= value3;
					}
					if (value != null)
					{
						base.Properties.SetObject(Label.PropImage, null);
					}
					base.Properties.SetObject(Label.PropImageList, value);
					if (value != null)
					{
						value.RecreateHandle += value2;
						value.Disposed += value3;
					}
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the alignment of an image that is displayed in the control.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default is <see langword="ContentAlignment.MiddleCenter" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values. </exception>
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x000B8C24 File Offset: 0x000B6E24
		// (set) Token: 0x0600276E RID: 10094 RVA: 0x000B8C4C File Offset: 0x000B6E4C
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[Localizable(true)]
		[SRDescription("ButtonImageAlignDescr")]
		[SRCategory("CatAppearance")]
		public ContentAlignment ImageAlign
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(Label.PropImageAlign, out flag);
				if (flag)
				{
					return (ContentAlignment)integer;
				}
				return ContentAlignment.MiddleCenter;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (value != this.ImageAlign)
				{
					base.Properties.SetInteger(Label.PropImageAlign, (int)value);
					LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.ImageAlign);
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x000B8CAE File Offset: 0x000B6EAE
		// (set) Token: 0x06002770 RID: 10096 RVA: 0x000B8CB6 File Offset: 0x000B6EB6
		[SRCategory("CatAccessibility")]
		[DefaultValue(AutomationLiveSetting.Off)]
		[SRDescription("LiveRegionAutomationLiveSettingDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutomationLiveSetting LiveSetting
		{
			get
			{
				return this.liveSetting;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutomationLiveSetting));
				}
				this.liveSetting = value;
			}
		}

		/// <summary>Gets or sets the Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to this property is not within the range of valid values specified in the enumeration. </exception>
		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06002772 RID: 10098 RVA: 0x00011FEC File Offset: 0x000101EC
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Label.ImeMode" /> property changes.</summary>
		// Token: 0x140001D3 RID: 467
		// (add) Token: 0x06002773 RID: 10099 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x06002774 RID: 10100 RVA: 0x0001BF35 File Offset: 0x0001A135
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

		/// <summary>Occurs when the user releases a key while the label has focus.</summary>
		// Token: 0x140001D4 RID: 468
		// (add) Token: 0x06002775 RID: 10101 RVA: 0x000B0E8C File Offset: 0x000AF08C
		// (remove) Token: 0x06002776 RID: 10102 RVA: 0x000B0E95 File Offset: 0x000AF095
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

		/// <summary>Occurs when the user presses a key while the label has focus.</summary>
		// Token: 0x140001D5 RID: 469
		// (add) Token: 0x06002777 RID: 10103 RVA: 0x000B0E9E File Offset: 0x000AF09E
		// (remove) Token: 0x06002778 RID: 10104 RVA: 0x000B0EA7 File Offset: 0x000AF0A7
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

		/// <summary>Occurs when the user presses a key while the label has focus.</summary>
		// Token: 0x140001D6 RID: 470
		// (add) Token: 0x06002779 RID: 10105 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		// (remove) Token: 0x0600277A RID: 10106 RVA: 0x000B0EB9 File Offset: 0x000AF0B9
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

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x000B8CE5 File Offset: 0x000B6EE5
		internal LayoutUtils.MeasureTextCache MeasureTextCache
		{
			get
			{
				if (this.textMeasurementCache == null)
				{
					this.textMeasurementCache = new LayoutUtils.MeasureTextCache();
				}
				return this.textMeasurementCache;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600277C RID: 10108 RVA: 0x000B8D00 File Offset: 0x000B6F00
		internal virtual bool OwnerDraw
		{
			get
			{
				return this.IsOwnerDraw();
			}
		}

		/// <summary>Gets the preferred height of the control.</summary>
		/// <returns>The height of the control (in pixels), assuming a single line of text is displayed.</returns>
		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x000B8D08 File Offset: 0x000B6F08
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("LabelPreferredHeightDescr")]
		public virtual int PreferredHeight
		{
			get
			{
				return base.PreferredSize.Height;
			}
		}

		/// <summary>Gets the preferred width of the control.</summary>
		/// <returns>The width of the control (in pixels), assuming a single line of text is displayed.</returns>
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x000B8D24 File Offset: 0x000B6F24
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("LabelPreferredWidthDescr")]
		public virtual int PreferredWidth
		{
			get
			{
				return base.PreferredSize.Width;
			}
		}

		/// <summary>Indicates whether the container control background is rendered on the <see cref="T:System.Windows.Forms.Label" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the background of the <see cref="T:System.Windows.Forms.Label" /> control's container is rendered on the <see cref="T:System.Windows.Forms.Label" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x0600277F RID: 10111 RVA: 0x000B8D3F File Offset: 0x000B6F3F
		// (set) Token: 0x06002780 RID: 10112 RVA: 0x0000701A File Offset: 0x0000521A
		[Obsolete("This property has been deprecated. Use BackColor instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected new virtual bool RenderTransparent
		{
			get
			{
				return base.RenderTransparent;
			}
			set
			{
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06002781 RID: 10113 RVA: 0x000B8D47 File Offset: 0x000B6F47
		private bool SelfSizing
		{
			get
			{
				return CommonProperties.ShouldSelfSize(this);
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can tab to the <see cref="T:System.Windows.Forms.Label" />. This property is not used by this class.</summary>
		/// <returns>This property is not used by this class. The default is <see langword="false" />.</returns>
		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06002783 RID: 10115 RVA: 0x000AA11D File Offset: 0x000A831D
		[DefaultValue(false)]
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.Label.TabStop" /> property changes.</summary>
		// Token: 0x140001D7 RID: 471
		// (add) Token: 0x06002784 RID: 10116 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x06002785 RID: 10117 RVA: 0x000AA12F File Offset: 0x000A832F
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

		/// <summary>Gets or sets the alignment of text in the label.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.ContentAlignment" /> values. The default is <see cref="F:System.Drawing.ContentAlignment.TopLeft" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment" /> values. </exception>
		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06002786 RID: 10118 RVA: 0x000B8D50 File Offset: 0x000B6F50
		// (set) Token: 0x06002787 RID: 10119 RVA: 0x000B8D78 File Offset: 0x000B6F78
		[SRDescription("LabelTextAlignDescr")]
		[Localizable(true)]
		[DefaultValue(ContentAlignment.TopLeft)]
		[SRCategory("CatAppearance")]
		public virtual ContentAlignment TextAlign
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(Label.PropTextAlign, out flag);
				if (flag)
				{
					return (ContentAlignment)integer;
				}
				return ContentAlignment.TopLeft;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (this.TextAlign != value)
				{
					base.Properties.SetInteger(Label.PropTextAlign, (int)value);
					base.Invalidate();
					if (!this.OwnerDraw)
					{
						base.RecreateHandle();
					}
					this.OnTextAlignChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06002789 RID: 10121 RVA: 0x0001BFAD File Offset: 0x0001A1AD
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Label.TextAlign" /> property has changed.</summary>
		// Token: 0x140001D8 RID: 472
		// (add) Token: 0x0600278A RID: 10122 RVA: 0x000B8DDC File Offset: 0x000B6FDC
		// (remove) Token: 0x0600278B RID: 10123 RVA: 0x000B8DEF File Offset: 0x000B6FEF
		[SRCategory("CatPropertyChanged")]
		[SRDescription("LabelOnTextAlignChangedDescr")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				base.Events.AddHandler(Label.EVENT_TEXTALIGNCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(Label.EVENT_TEXTALIGNCHANGED, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether to use the <see cref="T:System.Drawing.Graphics" /> class (GDI+) or the <see cref="T:System.Windows.Forms.TextRenderer" /> class (GDI) to render text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Drawing.Graphics" /> class should be used to perform text rendering for compatibility with versions 1.0 and 1.1. of the .NET Framework; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x000B8E02 File Offset: 0x000B7002
		// (set) Token: 0x0600278D RID: 10125 RVA: 0x000B8E14 File Offset: 0x000B7014
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("UseCompatibleTextRenderingDescr")]
		public bool UseCompatibleTextRendering
		{
			get
			{
				return !this.CanUseTextRenderer || base.UseCompatibleTextRenderingInt;
			}
			set
			{
				if (base.UseCompatibleTextRenderingInt != value)
				{
					base.UseCompatibleTextRenderingInt = value;
					this.AdjustSize();
				}
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets a value indicating whether the control interprets an ampersand character (&amp;) in the control's <see cref="P:System.Windows.Forms.Control.Text" /> property to be an access key prefix character.</summary>
		/// <returns>
		///     <see langword="true" /> if the label doesn't display the ampersand character and underlines the character after the ampersand in its displayed text and treats the underlined character as an access key; otherwise, <see langword="false" /> if the ampersand character is displayed in the text of the control. The default is <see langword="true" />.</returns>
		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000B8E2C File Offset: 0x000B702C
		// (set) Token: 0x06002790 RID: 10128 RVA: 0x000B8E44 File Offset: 0x000B7044
		[SRDescription("LabelUseMnemonicDescr")]
		[DefaultValue(true)]
		[SRCategory("CatAppearance")]
		public bool UseMnemonic
		{
			get
			{
				return this.labelState[Label.StateUseMnemonic] != 0;
			}
			set
			{
				if (this.UseMnemonic != value)
				{
					this.labelState[Label.StateUseMnemonic] = (value ? 1 : 0);
					this.MeasureTextCache.InvalidateCache();
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text))
					{
						this.AdjustSize();
						base.Invalidate();
					}
					if (base.IsHandleCreated)
					{
						int num = base.WindowStyle;
						if (!this.UseMnemonic)
						{
							num |= 128;
						}
						else
						{
							num &= -129;
						}
						base.WindowStyle = num;
					}
				}
			}
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000B8EF0 File Offset: 0x000B70F0
		internal void AdjustSize()
		{
			if (!this.SelfSizing)
			{
				return;
			}
			if (!this.AutoSize && ((this.Anchor & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right) || (this.Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom)))
			{
				return;
			}
			int height = this.requestedHeight;
			int width = this.requestedWidth;
			try
			{
				Size size = this.AutoSize ? base.PreferredSize : new Size(width, height);
				base.Size = size;
			}
			finally
			{
				this.requestedHeight = height;
				this.requestedWidth = width;
			}
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000B8F78 File Offset: 0x000B7178
		internal void Animate()
		{
			this.Animate(!base.DesignMode && base.Visible && base.Enabled && this.ParentInternal != null);
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000B8FA4 File Offset: 0x000B71A4
		internal void StopAnimate()
		{
			this.Animate(false);
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000B8FB0 File Offset: 0x000B71B0
		private void Animate(bool animate)
		{
			bool flag = this.labelState[Label.StateAnimating] != 0;
			if (animate != flag)
			{
				Image image = (Image)base.Properties.GetObject(Label.PropImage);
				if (animate)
				{
					if (image != null)
					{
						ImageAnimator.Animate(image, new EventHandler(this.OnFrameChanged));
						this.labelState[Label.StateAnimating] = (animate ? 1 : 0);
						return;
					}
				}
				else if (image != null)
				{
					ImageAnimator.StopAnimate(image, new EventHandler(this.OnFrameChanged));
					this.labelState[Label.StateAnimating] = (animate ? 1 : 0);
				}
			}
		}

		/// <summary>Determines the size and location of an image drawn within the <see cref="T:System.Windows.Forms.Label" /> control based on the alignment of the control.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> used to determine size and location when drawn within the control. </param>
		/// <param name="r">A <see cref="T:System.Drawing.Rectangle" /> that represents the area to draw the image in. </param>
		/// <param name="align">The alignment of content within the control. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the size and location of the specified image within the control.</returns>
		// Token: 0x06002795 RID: 10133 RVA: 0x000B9048 File Offset: 0x000B7248
		protected Rectangle CalcImageRenderBounds(Image image, Rectangle r, ContentAlignment align)
		{
			Size size = image.Size;
			int x = r.X + 2;
			int y = r.Y + 2;
			if ((align & WindowsFormsUtils.AnyRightAlign) != (ContentAlignment)0)
			{
				x = r.X + r.Width - 4 - size.Width;
			}
			else if ((align & WindowsFormsUtils.AnyCenterAlign) != (ContentAlignment)0)
			{
				x = r.X + (r.Width - size.Width) / 2;
			}
			if ((align & WindowsFormsUtils.AnyBottomAlign) != (ContentAlignment)0)
			{
				y = r.Y + r.Height - 4 - size.Height;
			}
			else if ((align & WindowsFormsUtils.AnyTopAlign) != (ContentAlignment)0)
			{
				y = r.Y + 2;
			}
			else
			{
				y = r.Y + (r.Height - size.Height) / 2;
			}
			return new Rectangle(x, y, size.Width, size.Height);
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x06002796 RID: 10134 RVA: 0x000B9121 File Offset: 0x000B7321
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new Label.LabelAccessibleObject(this);
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000B9129 File Offset: 0x000B7329
		internal virtual StringFormat CreateStringFormat()
		{
			return ControlPaint.CreateStringFormat(this, this.TextAlign, this.AutoEllipsis, this.UseMnemonic);
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000B9143 File Offset: 0x000B7343
		private TextFormatFlags CreateTextFormatFlags()
		{
			return this.CreateTextFormatFlags(base.Size - this.GetBordersAndPadding());
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000B915C File Offset: 0x000B735C
		internal virtual TextFormatFlags CreateTextFormatFlags(Size constrainingSize)
		{
			TextFormatFlags textFormatFlags = ControlPaint.CreateTextFormatFlags(this, this.TextAlign, this.AutoEllipsis, this.UseMnemonic);
			if (!this.MeasureTextCache.TextRequiresWordBreak(this.Text, this.Font, constrainingSize, textFormatFlags))
			{
				textFormatFlags &= ~(TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
			}
			return textFormatFlags;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000B91A6 File Offset: 0x000B73A6
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Label" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600279B RID: 10139 RVA: 0x000B91B0 File Offset: 0x000B73B0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopAnimate();
				if (this.ImageList != null)
				{
					this.ImageList.Disposed -= this.DetachImageList;
					this.ImageList.RecreateHandle -= this.ImageListRecreateHandle;
					base.Properties.SetObject(Label.PropImageList, null);
				}
				if (this.Image != null)
				{
					base.Properties.SetObject(Label.PropImage, null);
				}
				if (this.textToolTip != null)
				{
					this.textToolTip.Dispose();
					this.textToolTip = null;
				}
				this.controlToolTip = false;
			}
			base.Dispose(disposing);
		}

		/// <summary>Draws an <see cref="T:System.Drawing.Image" /> within the specified bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw. </param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="r">The <see cref="T:System.Drawing.Rectangle" /> bounds to draw within. </param>
		/// <param name="align">The alignment of the image to draw within the <see cref="T:System.Windows.Forms.Label" />. </param>
		// Token: 0x0600279C RID: 10140 RVA: 0x000B9254 File Offset: 0x000B7454
		protected void DrawImage(Graphics g, Image image, Rectangle r, ContentAlignment align)
		{
			Rectangle rectangle = this.CalcImageRenderBounds(image, r, align);
			if (!base.Enabled)
			{
				ControlPaint.DrawImageDisabled(g, image, rectangle.X, rectangle.Y, this.BackColor);
				return;
			}
			g.DrawImage(image, rectangle.X, rectangle.Y, image.Width, image.Height);
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000B92B4 File Offset: 0x000B74B4
		private Size GetBordersAndPadding()
		{
			Size size = base.Padding.Size;
			if (this.UseCompatibleTextRendering)
			{
				if (this.BorderStyle != BorderStyle.None)
				{
					size.Height += 6;
					size.Width += 2;
				}
				else
				{
					size.Height += 3;
				}
			}
			else
			{
				size += this.SizeFromClientSize(Size.Empty);
				if (this.BorderStyle == BorderStyle.Fixed3D)
				{
					size += new Size(2, 2);
				}
			}
			return size;
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fitted. </summary>
		/// <param name="proposedSize">The custom-sized area for a control.</param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x0600279E RID: 10142 RVA: 0x0001C3D6 File Offset: 0x0001A5D6
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

		// Token: 0x0600279F RID: 10143 RVA: 0x000B933B File Offset: 0x000B753B
		internal virtual bool UseGDIMeasuring()
		{
			return this.FlatStyle == FlatStyle.System || !this.UseCompatibleTextRendering;
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000B9354 File Offset: 0x000B7554
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size bordersAndPadding = this.GetBordersAndPadding();
			proposedConstraints -= bordersAndPadding;
			proposedConstraints = LayoutUtils.UnionSizes(proposedConstraints, Size.Empty);
			Size size;
			if (string.IsNullOrEmpty(this.Text))
			{
				using (WindowsFont windowsFont = WindowsFont.FromFont(this.Font))
				{
					size = WindowsGraphicsCacheManager.MeasurementGraphics.GetTextExtent("0", windowsFont);
					size.Width = 0;
					goto IL_111;
				}
			}
			if (this.UseGDIMeasuring())
			{
				TextFormatFlags flags = (this.FlatStyle == FlatStyle.System) ? TextFormatFlags.Default : this.CreateTextFormatFlags(proposedConstraints);
				size = this.MeasureTextCache.GetTextSize(this.Text, this.Font, proposedConstraints, flags);
			}
			else
			{
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					using (StringFormat stringFormat = this.CreateStringFormat())
					{
						SizeF layoutArea = (proposedConstraints.Width == 1) ? new SizeF(0f, (float)proposedConstraints.Height) : new SizeF((float)proposedConstraints.Width, (float)proposedConstraints.Height);
						size = Size.Ceiling(graphics.MeasureString(this.Text, this.Font, layoutArea, stringFormat));
					}
				}
			}
			IL_111:
			size += bordersAndPadding;
			return size;
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000B94A4 File Offset: 0x000B76A4
		private int GetLeadingTextPaddingFromTextFormatFlags()
		{
			if (!base.IsHandleCreated)
			{
				return 0;
			}
			if (this.UseCompatibleTextRendering && this.FlatStyle != FlatStyle.System)
			{
				return 0;
			}
			int iLeftMargin;
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHwnd(base.Handle))
			{
				TextFormatFlags textFormatFlags = this.CreateTextFormatFlags();
				if ((textFormatFlags & TextFormatFlags.NoPadding) == TextFormatFlags.NoPadding)
				{
					windowsGraphics.TextPadding = TextPaddingOptions.NoPadding;
				}
				else if ((textFormatFlags & TextFormatFlags.LeftAndRightPadding) == TextFormatFlags.LeftAndRightPadding)
				{
					windowsGraphics.TextPadding = TextPaddingOptions.LeftAndRightPadding;
				}
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(this.Font))
				{
					IntNativeMethods.DRAWTEXTPARAMS textMargins = windowsGraphics.GetTextMargins(windowsFont);
					iLeftMargin = textMargins.iLeftMargin;
				}
			}
			return iLeftMargin;
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x0001C1DB File Offset: 0x0001A3DB
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.Invalidate();
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060027A3 RID: 10147 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool IsMnemonicsListenerAxSourced
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000B9560 File Offset: 0x000B7760
		internal bool IsOwnerDraw()
		{
			return this.FlatStyle != FlatStyle.System;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027A5 RID: 10149 RVA: 0x000B9570 File Offset: 0x000B7770
		protected override void OnMouseEnter(EventArgs e)
		{
			if (!this.controlToolTip && !base.DesignMode && this.AutoEllipsis && this.showToolTip && this.textToolTip != null)
			{
				IntSecurity.AllWindows.Assert();
				try
				{
					this.controlToolTip = true;
					this.textToolTip.Show(WindowsFormsUtils.TextWithoutMnemonics(this.Text), this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
					this.controlToolTip = false;
				}
			}
			base.OnMouseEnter(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027A6 RID: 10150 RVA: 0x000B95F4 File Offset: 0x000B77F4
		protected override void OnMouseLeave(EventArgs e)
		{
			if (!this.controlToolTip && this.textToolTip != null && this.textToolTip.GetHandleCreated())
			{
				this.textToolTip.RemoveAll();
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
			base.OnMouseLeave(e);
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000B9660 File Offset: 0x000B7860
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

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027A8 RID: 10152 RVA: 0x000B96B6 File Offset: 0x000B78B6
		protected override void OnFontChanged(EventArgs e)
		{
			this.MeasureTextCache.InvalidateCache();
			base.OnFontChanged(e);
			this.AdjustSize();
			base.Invalidate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027A9 RID: 10153 RVA: 0x000B96D6 File Offset: 0x000B78D6
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if (this.textToolTip != null && this.textToolTip.GetHandleCreated())
			{
				this.textToolTip.DestroyHandle();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027AA RID: 10154 RVA: 0x000B9700 File Offset: 0x000B7900
		protected override void OnTextChanged(EventArgs e)
		{
			using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text))
			{
				this.MeasureTextCache.InvalidateCache();
				base.OnTextChanged(e);
				this.AdjustSize();
				base.Invalidate();
			}
			if (AccessibilityImprovements.Level3 && this.LiveSetting != AutomationLiveSetting.Off)
			{
				base.AccessibilityObject.RaiseLiveRegionChanged();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Label.TextAlignChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060027AB RID: 10155 RVA: 0x000B977C File Offset: 0x000B797C
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[Label.EVENT_TEXTALIGNCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.PaddingChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027AC RID: 10156 RVA: 0x000B97AA File Offset: 0x000B79AA
		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			this.AdjustSize();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x060027AD RID: 10157 RVA: 0x000B97BC File Offset: 0x000B79BC
		protected override void OnPaint(PaintEventArgs e)
		{
			this.Animate();
			ImageAnimator.UpdateFrames(this.Image);
			Rectangle rectangle = LayoutUtils.DeflateRect(base.ClientRectangle, base.Padding);
			Image image = this.Image;
			if (image != null)
			{
				this.DrawImage(e.Graphics, image, rectangle, base.RtlTranslateAlignment(this.ImageAlign));
			}
			IntPtr hdc = e.Graphics.GetHdc();
			Color nearestColor;
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					nearestColor = windowsGraphics.GetNearestColor(base.Enabled ? this.ForeColor : base.DisabledColor);
				}
			}
			finally
			{
				e.Graphics.ReleaseHdc();
			}
			if (this.AutoEllipsis)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				Size preferredSize = this.GetPreferredSize(new Size(clientRectangle.Width, clientRectangle.Height));
				this.showToolTip = (clientRectangle.Width < preferredSize.Width || clientRectangle.Height < preferredSize.Height);
			}
			else
			{
				this.showToolTip = false;
			}
			if (this.UseCompatibleTextRendering)
			{
				using (StringFormat stringFormat = this.CreateStringFormat())
				{
					if (base.Enabled)
					{
						using (Brush brush = new SolidBrush(nearestColor))
						{
							e.Graphics.DrawString(this.Text, this.Font, brush, rectangle, stringFormat);
							goto IL_1C6;
						}
					}
					ControlPaint.DrawStringDisabled(e.Graphics, this.Text, this.Font, nearestColor, rectangle, stringFormat);
					goto IL_1C6;
				}
			}
			TextFormatFlags flags = this.CreateTextFormatFlags();
			if (base.Enabled)
			{
				TextRenderer.DrawText(e.Graphics, this.Text, this.Font, rectangle, nearestColor, flags);
			}
			else
			{
				Color foreColor = TextRenderer.DisabledTextColor(this.BackColor);
				TextRenderer.DrawText(e.Graphics, this.Text, this.Font, rectangle, foreColor, flags);
			}
			IL_1C6:
			base.OnPaint(e);
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x0000701A File Offset: 0x0000521A
		internal virtual void OnAutoEllipsisChanged()
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027AF RID: 10159 RVA: 0x000B99CC File Offset: 0x000B7BCC
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Animate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027B0 RID: 10160 RVA: 0x000B99DB File Offset: 0x000B7BDB
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (this.SelfSizing)
			{
				this.AdjustSize();
			}
			this.Animate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027B1 RID: 10161 RVA: 0x000B99F8 File Offset: 0x000B7BF8
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			this.MeasureTextCache.InvalidateCache();
			base.OnRightToLeftChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060027B2 RID: 10162 RVA: 0x000B9A0C File Offset: 0x000B7C0C
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Animate();
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000B9A1C File Offset: 0x000B7C1C
		internal override void PrintToMetaFileRecursive(HandleRef hDC, IntPtr lParam, Rectangle bounds)
		{
			base.PrintToMetaFileRecursive(hDC, lParam, bounds);
			using (new WindowsFormsUtils.DCMapping(hDC, bounds))
			{
				using (Graphics graphics = Graphics.FromHdcInternal(hDC.Handle))
				{
					ControlPaint.PrintBorder(graphics, new Rectangle(Point.Empty, base.Size), this.BorderStyle, Border3DStyle.SunkenOuter);
				}
			}
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process.</param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060027B4 RID: 10164 RVA: 0x000B9A9C File Offset: 0x000B7C9C
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (this.UseMnemonic && Control.IsMnemonic(charCode, this.Text) && this.CanProcessMnemonic())
			{
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null)
				{
					IntSecurity.ModifyFocus.Assert();
					try
					{
						if (parentInternal.SelectNextControl(this, true, false, true, false) && !parentInternal.ContainsFocus)
						{
							parentInternal.Focus();
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>Sets the specified bounds of the label.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control. </param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control. </param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control. </param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control. </param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values. For any parameter not specified, the current value will be used. </param>
		// Token: 0x060027B5 RID: 10165 RVA: 0x000B9B14 File Offset: 0x000B7D14
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				this.requestedHeight = height;
			}
			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
			{
				this.requestedWidth = width;
			}
			if (this.AutoSize && this.SelfSizing)
			{
				Size preferredSize = base.PreferredSize;
				width = preferredSize.Width;
				height = preferredSize.Height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000B9B72 File Offset: 0x000B7D72
		private void ResetImage()
		{
			this.Image = null;
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000B9B7B File Offset: 0x000B7D7B
		private bool ShouldSerializeImage()
		{
			return base.Properties.GetObject(Label.PropImage) != null;
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000B9B90 File Offset: 0x000B7D90
		internal void SetToolTip(ToolTip toolTip)
		{
			if (toolTip != null && !this.controlToolTip)
			{
				this.controlToolTip = true;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x00020C1B File Offset: 0x0001EE1B
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3 && !base.DesignMode;
			}
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Windows.Forms.Label" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.Label" />.</returns>
		// Token: 0x060027BA RID: 10170 RVA: 0x000B9BA4 File Offset: 0x000B7DA4
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Text: " + this.Text;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x060027BB RID: 10171 RVA: 0x000B9BCC File Offset: 0x000B7DCC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 132)
			{
				Rectangle rectangle = base.RectangleToScreen(new Rectangle(0, 0, base.Width, base.Height));
				Point pt = new Point((int)((long)m.LParam));
				m.Result = (IntPtr)(rectangle.Contains(pt) ? 1 : 0);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000B9C36 File Offset: 0x000B7E36
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			if (!DpiHelper.EnableDpiChangedHighDpiImprovements)
			{
				return;
			}
			this.MeasureTextCache.InvalidateCache();
		}

		// Token: 0x04001155 RID: 4437
		private static readonly object EVENT_TEXTALIGNCHANGED = new object();

		// Token: 0x04001156 RID: 4438
		private static readonly BitVector32.Section StateUseMnemonic = BitVector32.CreateSection(1);

		// Token: 0x04001157 RID: 4439
		private static readonly BitVector32.Section StateAutoSize = BitVector32.CreateSection(1, Label.StateUseMnemonic);

		// Token: 0x04001158 RID: 4440
		private static readonly BitVector32.Section StateAnimating = BitVector32.CreateSection(1, Label.StateAutoSize);

		// Token: 0x04001159 RID: 4441
		private static readonly BitVector32.Section StateFlatStyle = BitVector32.CreateSection(3, Label.StateAnimating);

		// Token: 0x0400115A RID: 4442
		private static readonly BitVector32.Section StateBorderStyle = BitVector32.CreateSection(2, Label.StateFlatStyle);

		// Token: 0x0400115B RID: 4443
		private static readonly BitVector32.Section StateAutoEllipsis = BitVector32.CreateSection(1, Label.StateBorderStyle);

		// Token: 0x0400115C RID: 4444
		private static readonly int PropImageList = PropertyStore.CreateKey();

		// Token: 0x0400115D RID: 4445
		private static readonly int PropImage = PropertyStore.CreateKey();

		// Token: 0x0400115E RID: 4446
		private static readonly int PropTextAlign = PropertyStore.CreateKey();

		// Token: 0x0400115F RID: 4447
		private static readonly int PropImageAlign = PropertyStore.CreateKey();

		// Token: 0x04001160 RID: 4448
		private static readonly int PropImageIndex = PropertyStore.CreateKey();

		// Token: 0x04001161 RID: 4449
		private BitVector32 labelState;

		// Token: 0x04001162 RID: 4450
		private int requestedHeight;

		// Token: 0x04001163 RID: 4451
		private int requestedWidth;

		// Token: 0x04001164 RID: 4452
		private LayoutUtils.MeasureTextCache textMeasurementCache;

		// Token: 0x04001165 RID: 4453
		internal bool showToolTip;

		// Token: 0x04001166 RID: 4454
		private ToolTip textToolTip;

		// Token: 0x04001167 RID: 4455
		private bool controlToolTip;

		// Token: 0x04001168 RID: 4456
		private AutomationLiveSetting liveSetting;

		// Token: 0x020005FD RID: 1533
		[ComVisible(true)]
		internal class LabelAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06005BE0 RID: 23520 RVA: 0x00093572 File Offset: 0x00091772
			public LabelAccessibleObject(Label owner) : base(owner)
			{
			}

			// Token: 0x170015FF RID: 5631
			// (get) Token: 0x06005BE1 RID: 23521 RVA: 0x0017FD84 File Offset: 0x0017DF84
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.StaticText;
				}
			}

			// Token: 0x06005BE2 RID: 23522 RVA: 0x0009357B File Offset: 0x0009177B
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level3 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005BE3 RID: 23523 RVA: 0x0017DD0D File Offset: 0x0017BF0D
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level3 && patternId == 10018) || base.IsPatternSupported(patternId);
			}

			// Token: 0x06005BE4 RID: 23524 RVA: 0x0017FDA5 File Offset: 0x0017DFA5
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3 && propertyID == 30005)
				{
					return this.Name;
				}
				if (propertyID == 30003)
				{
					return 50020;
				}
				return base.GetPropertyValue(propertyID);
			}
		}
	}
}
