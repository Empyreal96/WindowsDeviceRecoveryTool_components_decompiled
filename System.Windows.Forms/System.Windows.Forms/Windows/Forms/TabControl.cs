using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Manages a related set of tab pages.</summary>
	// Token: 0x02000375 RID: 885
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("TabPages")]
	[DefaultEvent("SelectedIndexChanged")]
	[Designer("System.Windows.Forms.Design.TabControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionTabControl")]
	public class TabControl : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TabControl" /> class.</summary>
		// Token: 0x060037AA RID: 14250 RVA: 0x000FACEC File Offset: 0x000F8EEC
		public TabControl()
		{
			this.tabControlState = new BitVector32(0);
			this.tabCollection = new TabControl.TabPageCollection(this);
			base.SetStyle(ControlStyles.UserPaint, false);
		}

		/// <summary>Gets or sets the area of the control (for example, along the top) where the tabs are aligned.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TabAlignment" /> values. The default is <see langword="Top" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The property value is not a valid <see cref="T:System.Windows.Forms.TabAlignment" /> value. </exception>
		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x060037AB RID: 14251 RVA: 0x000FAD77 File Offset: 0x000F8F77
		// (set) Token: 0x060037AC RID: 14252 RVA: 0x000FAD80 File Offset: 0x000F8F80
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(TabAlignment.Top)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("TabBaseAlignmentDescr")]
		public TabAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				if (this.alignment != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(TabAlignment));
					}
					this.alignment = value;
					if (this.alignment == TabAlignment.Left || this.alignment == TabAlignment.Right)
					{
						this.Multiline = true;
					}
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets the visual appearance of the control's tabs.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TabAppearance" /> values. The default is <see langword="Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The property value is not a valid <see cref="T:System.Windows.Forms.TabAppearance" /> value. </exception>
		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x060037AD RID: 14253 RVA: 0x000FADE2 File Offset: 0x000F8FE2
		// (set) Token: 0x060037AE RID: 14254 RVA: 0x000FAE00 File Offset: 0x000F9000
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(TabAppearance.Normal)]
		[SRDescription("TabBaseAppearanceDescr")]
		public TabAppearance Appearance
		{
			get
			{
				if (this.appearance == TabAppearance.FlatButtons && this.alignment != TabAlignment.Top)
				{
					return TabAppearance.Buttons;
				}
				return this.appearance;
			}
			set
			{
				if (this.appearance != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(TabAppearance));
					}
					this.appearance = value;
					base.RecreateHandle();
					this.OnStyleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The background color for the control.</returns>
		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x060037AF RID: 14255 RVA: 0x0002849B File Offset: 0x0002669B
		// (set) Token: 0x060037B0 RID: 14256 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return SystemColors.Control;
			}
			set
			{
			}
		}

		/// <summary>This event is not meaningful for this control.</summary>
		// Token: 0x140002B4 RID: 692
		// (add) Token: 0x060037B1 RID: 14257 RVA: 0x00050A7A File Offset: 0x0004EC7A
		// (remove) Token: 0x060037B2 RID: 14258 RVA: 0x00050A83 File Offset: 0x0004EC83
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

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The background image displayed in the control.</returns>
		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x060037B3 RID: 14259 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x060037B4 RID: 14260 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabControl.BackgroundImage" /> property changes.</summary>
		// Token: 0x140002B5 RID: 693
		// (add) Token: 0x060037B5 RID: 14261 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x060037B6 RID: 14262 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout" />. The default value is Tile.</returns>
		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x060037B8 RID: 14264 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabControl.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140002B6 RID: 694
		// (add) Token: 0x060037B9 RID: 14265 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x060037BA RID: 14266 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x060037BB RID: 14267 RVA: 0x000B0CC4 File Offset: 0x000AEEC4
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 100);
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value.</returns>
		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x060037BC RID: 14268 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x060037BD RID: 14269 RVA: 0x000A2CBA File Offset: 0x000A0EBA
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

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The foreground color of the control.</returns>
		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x060037BF RID: 14271 RVA: 0x0001208A File Offset: 0x0001028A
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabControl.ForeColor" /> property changes.</summary>
		// Token: 0x140002B7 RID: 695
		// (add) Token: 0x060037C0 RID: 14272 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x060037C1 RID: 14273 RVA: 0x0005276F File Offset: 0x0005096F
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

		/// <summary>This member overrides <see cref="P:System.Windows.Forms.Control.CreateParams" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x060037C2 RID: 14274 RVA: 0x000FAE54 File Offset: 0x000F9054
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysTabControl32";
				if (this.Multiline)
				{
					createParams.Style |= 512;
				}
				if (this.drawMode == TabDrawMode.OwnerDrawFixed)
				{
					createParams.Style |= 8192;
				}
				if (this.ShowToolTips && !base.DesignMode)
				{
					createParams.Style |= 16384;
				}
				if (this.alignment == TabAlignment.Bottom || this.alignment == TabAlignment.Right)
				{
					createParams.Style |= 2;
				}
				if (this.alignment == TabAlignment.Left || this.alignment == TabAlignment.Right)
				{
					createParams.Style |= 640;
				}
				if (this.tabControlState[1])
				{
					createParams.Style |= 64;
				}
				if (this.appearance == TabAppearance.Normal)
				{
					createParams.Style |= 0;
				}
				else
				{
					createParams.Style |= 256;
					if (this.appearance == TabAppearance.FlatButtons && this.alignment == TabAlignment.Top)
					{
						createParams.Style |= 8;
					}
				}
				switch (this.sizeMode)
				{
				case TabSizeMode.Normal:
					createParams.Style |= 2048;
					break;
				case TabSizeMode.FillToRight:
					createParams.Style |= 0;
					break;
				case TabSizeMode.Fixed:
					createParams.Style |= 1024;
					break;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 5242880;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		/// <summary>Gets the display area of the control's tab pages.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the display area of the tab pages.</returns>
		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x060037C3 RID: 14275 RVA: 0x000FAFFC File Offset: 0x000F91FC
		public override Rectangle DisplayRectangle
		{
			get
			{
				if (!this.cachedDisplayRect.IsEmpty)
				{
					return this.cachedDisplayRect;
				}
				Rectangle bounds = base.Bounds;
				NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(bounds.X, bounds.Y, bounds.Width, bounds.Height);
				if (!base.IsDisposed)
				{
					if (!base.IsActiveX && !base.IsHandleCreated)
					{
						this.CreateHandle();
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(4904, 0, ref rect);
					}
				}
				Rectangle result = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
				Point location = base.Location;
				result.X -= location.X;
				result.Y -= location.Y;
				this.cachedDisplayRect = result;
				return result;
			}
		}

		/// <summary>Gets or sets the way that the control's tabs are drawn.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TabDrawMode" /> values. The default is <see langword="Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The property value is not a valid <see cref="T:System.Windows.Forms.TabDrawMode" /> value. </exception>
		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x060037C4 RID: 14276 RVA: 0x000FB0D2 File Offset: 0x000F92D2
		// (set) Token: 0x060037C5 RID: 14277 RVA: 0x000FB0DA File Offset: 0x000F92DA
		[SRCategory("CatBehavior")]
		[DefaultValue(TabDrawMode.Normal)]
		[SRDescription("TabBaseDrawModeDescr")]
		public TabDrawMode DrawMode
		{
			get
			{
				return this.drawMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TabDrawMode));
				}
				if (this.drawMode != value)
				{
					this.drawMode = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the control's tabs change in appearance when the mouse passes over them.</summary>
		/// <returns>
		///     <see langword="true" /> if the tabs change in appearance when the mouse passes over them; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x060037C6 RID: 14278 RVA: 0x000FB118 File Offset: 0x000F9318
		// (set) Token: 0x060037C7 RID: 14279 RVA: 0x000FB126 File Offset: 0x000F9326
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TabBaseHotTrackDescr")]
		public bool HotTrack
		{
			get
			{
				return this.tabControlState[1];
			}
			set
			{
				if (this.HotTrack != value)
				{
					this.tabControlState[1] = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets the images to display on the control's tabs.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ImageList" /> that specifies the images to display on the tabs.</returns>
		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x060037C8 RID: 14280 RVA: 0x000FB14C File Offset: 0x000F934C
		// (set) Token: 0x060037C9 RID: 14281 RVA: 0x000FB154 File Offset: 0x000F9354
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[SRDescription("TabBaseImageListDescr")]
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
					this.imageList = value;
					IntPtr lparam = (value != null) ? value.Handle : IntPtr.Zero;
					if (base.IsHandleCreated)
					{
						base.SendMessage(4867, IntPtr.Zero, lparam);
					}
					foreach (object obj in this.TabPages)
					{
						TabPage tabPage = (TabPage)obj;
						tabPage.ImageIndexer.ImageList = value;
					}
					if (value != null)
					{
						value.RecreateHandle += value2;
						value.Disposed += value3;
					}
				}
			}
		}

		/// <summary>Gets or sets the size of the control's tabs.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the tabs. The default automatically sizes the tabs to fit the icons and labels on the tabs.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The width or height of the <see cref="T:System.Drawing.Size" /> is less than 0. </exception>
		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x060037CA RID: 14282 RVA: 0x000FB240 File Offset: 0x000F9440
		// (set) Token: 0x060037CB RID: 14283 RVA: 0x000FB28C File Offset: 0x000F948C
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("TabBaseItemSizeDescr")]
		public Size ItemSize
		{
			get
			{
				if (!this.itemSize.IsEmpty)
				{
					return this.itemSize;
				}
				if (base.IsHandleCreated)
				{
					this.tabControlState[8] = true;
					return this.GetTabRect(0).Size;
				}
				return TabControl.DEFAULT_ITEMSIZE;
			}
			set
			{
				if (value.Width < 0 || value.Height < 0)
				{
					throw new ArgumentOutOfRangeException("ItemSize", SR.GetString("InvalidArgument", new object[]
					{
						"ItemSize",
						value.ToString()
					}));
				}
				this.itemSize = value;
				this.ApplyItemSize();
				this.UpdateSize();
				base.Invalidate();
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x060037CC RID: 14284 RVA: 0x000FB2F9 File Offset: 0x000F94F9
		// (set) Token: 0x060037CD RID: 14285 RVA: 0x000FB30B File Offset: 0x000F950B
		private bool InsertingItem
		{
			get
			{
				return this.tabControlState[128];
			}
			set
			{
				this.tabControlState[128] = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether more than one row of tabs can be displayed.</summary>
		/// <returns>
		///     <see langword="true" /> if more than one row of tabs can be displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x060037CE RID: 14286 RVA: 0x000FB31E File Offset: 0x000F951E
		// (set) Token: 0x060037CF RID: 14287 RVA: 0x000FB32C File Offset: 0x000F952C
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TabBaseMultilineDescr")]
		public bool Multiline
		{
			get
			{
				return this.tabControlState[2];
			}
			set
			{
				if (this.Multiline != value)
				{
					this.tabControlState[2] = value;
					if (!this.Multiline && (this.alignment == TabAlignment.Left || this.alignment == TabAlignment.Right))
					{
						this.alignment = TabAlignment.Top;
					}
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets the amount of space around each item on the control's tab pages.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that specifies the amount of space around each item. The default is (6,3).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The width or height of the <see cref="T:System.Drawing.Point" /> is less than 0. </exception>
		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x060037D0 RID: 14288 RVA: 0x000FB36B File Offset: 0x000F956B
		// (set) Token: 0x060037D1 RID: 14289 RVA: 0x000FB374 File Offset: 0x000F9574
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("TabBasePaddingDescr")]
		public new Point Padding
		{
			get
			{
				return this.padding;
			}
			set
			{
				if (value.X < 0 || value.Y < 0)
				{
					throw new ArgumentOutOfRangeException("Padding", SR.GetString("InvalidArgument", new object[]
					{
						"Padding",
						value.ToString()
					}));
				}
				if (this.padding != value)
				{
					this.padding = value;
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether right-to-left mirror placement is turned on.</summary>
		/// <returns>
		///     <see langword="true" /> if right-to-left mirror placement is turned on; <see langword="false" /> for standard child control placement. The default is <see langword="false" />.</returns>
		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x060037D2 RID: 14290 RVA: 0x000FB3EB File Offset: 0x000F95EB
		// (set) Token: 0x060037D3 RID: 14291 RVA: 0x000FB3F4 File Offset: 0x000F95F4
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

		/// <summary>Gets the number of rows that are currently being displayed in the control's tab strip.</summary>
		/// <returns>The number of rows that are currently being displayed in the tab strip.</returns>
		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x060037D4 RID: 14292 RVA: 0x000FB448 File Offset: 0x000F9648
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TabBaseRowCountDescr")]
		public int RowCount
		{
			get
			{
				return (int)((long)base.SendMessage(4908, 0, 0));
			}
		}

		/// <summary>Gets or sets the index of the currently selected tab page.</summary>
		/// <returns>The zero-based index of the currently selected tab page. The default is -1, which is also the value if no tab page is selected.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than -1. </exception>
		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x060037D5 RID: 14293 RVA: 0x000FB46C File Offset: 0x000F966C
		// (set) Token: 0x060037D6 RID: 14294 RVA: 0x000FB4A0 File Offset: 0x000F96A0
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[DefaultValue(-1)]
		[SRDescription("selectedIndexDescr")]
		public int SelectedIndex
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)((long)base.SendMessage(4875, 0, 0));
				}
				return this.selectedIndex;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("SelectedIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"SelectedIndex",
						value.ToString(CultureInfo.CurrentCulture),
						-1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.SelectedIndex != value)
				{
					if (base.IsHandleCreated)
					{
						if (!this.tabControlState[16] && !this.tabControlState[64])
						{
							this.tabControlState[32] = true;
							if (this.WmSelChanging())
							{
								this.tabControlState[32] = false;
								return;
							}
							if (base.ValidationCancelled)
							{
								this.tabControlState[32] = false;
								return;
							}
						}
						base.SendMessage(4876, value, 0);
						if (!this.tabControlState[16] && !this.tabControlState[64])
						{
							this.tabControlState[64] = true;
							if (this.WmSelChange())
							{
								this.tabControlState[32] = false;
								this.tabControlState[64] = false;
								return;
							}
							this.tabControlState[64] = false;
							return;
						}
					}
					else
					{
						this.selectedIndex = value;
					}
				}
			}
		}

		/// <summary>Gets or sets the currently selected tab page.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TabPage" /> that represents the selected tab page. If no tab page is selected, the value is <see langword="null" />.</returns>
		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x060037D7 RID: 14295 RVA: 0x000FB5DA File Offset: 0x000F97DA
		// (set) Token: 0x060037D8 RID: 14296 RVA: 0x000FB5E2 File Offset: 0x000F97E2
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TabControlSelectedTabDescr")]
		public TabPage SelectedTab
		{
			get
			{
				return this.SelectedTabInternal;
			}
			set
			{
				this.SelectedTabInternal = value;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x060037D9 RID: 14297 RVA: 0x000FB5EC File Offset: 0x000F97EC
		// (set) Token: 0x060037DA RID: 14298 RVA: 0x000FB610 File Offset: 0x000F9810
		internal TabPage SelectedTabInternal
		{
			get
			{
				int num = this.SelectedIndex;
				if (num == -1)
				{
					return null;
				}
				return this.tabPages[num];
			}
			set
			{
				int num = this.FindTabPage(value);
				this.SelectedIndex = num;
			}
		}

		/// <summary>Gets or sets the way that the control's tabs are sized.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TabSizeMode" /> values. The default is <see langword="Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The property value is not a valid <see cref="T:System.Windows.Forms.TabSizeMode" /> value. </exception>
		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x060037DB RID: 14299 RVA: 0x000FB62C File Offset: 0x000F982C
		// (set) Token: 0x060037DC RID: 14300 RVA: 0x000FB634 File Offset: 0x000F9834
		[SRCategory("CatBehavior")]
		[DefaultValue(TabSizeMode.Normal)]
		[SRDescription("TabBaseSizeModeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public TabSizeMode SizeMode
		{
			get
			{
				return this.sizeMode;
			}
			set
			{
				if (this.sizeMode == value)
				{
					return;
				}
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TabSizeMode));
				}
				this.sizeMode = value;
				base.RecreateHandle();
			}
		}

		/// <summary>Gets or sets a value indicating whether a tab's ToolTip is shown when the mouse passes over the tab.</summary>
		/// <returns>
		///     <see langword="true" /> if ToolTips are shown for the tabs that have them; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x060037DD RID: 14301 RVA: 0x000FB673 File Offset: 0x000F9873
		// (set) Token: 0x060037DE RID: 14302 RVA: 0x000FB681 File Offset: 0x000F9881
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("TabBaseShowToolTipsDescr")]
		public bool ShowToolTips
		{
			get
			{
				return this.tabControlState[4];
			}
			set
			{
				if (this.ShowToolTips != value)
				{
					this.tabControlState[4] = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets the number of tabs in the tab strip.</summary>
		/// <returns>The number of tabs in the tab strip.</returns>
		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x060037DF RID: 14303 RVA: 0x000FB69F File Offset: 0x000F989F
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TabBaseTabCountDescr")]
		public int TabCount
		{
			get
			{
				return this.tabPageCount;
			}
		}

		/// <summary>Gets the collection of tab pages in this tab control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" /> that contains the <see cref="T:System.Windows.Forms.TabPage" /> objects in this <see cref="T:System.Windows.Forms.TabControl" />.</returns>
		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x000FB6A7 File Offset: 0x000F98A7
		[SRCategory("CatBehavior")]
		[SRDescription("TabControlTabsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Editor("System.Windows.Forms.Design.TabPageCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		public TabControl.TabPageCollection TabPages
		{
			get
			{
				return this.tabCollection;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x060037E1 RID: 14305 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060037E2 RID: 14306 RVA: 0x0001BFAD File Offset: 0x0001A1AD
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabControl.Text" /> property changes.</summary>
		// Token: 0x140002B8 RID: 696
		// (add) Token: 0x060037E3 RID: 14307 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x060037E4 RID: 14308 RVA: 0x0003E43E File Offset: 0x0003C63E
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

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.TabControl" /> needs to paint each of its tabs if the <see cref="P:System.Windows.Forms.TabControl.DrawMode" /> property is set to <see cref="F:System.Windows.Forms.TabDrawMode.OwnerDrawFixed" />.</summary>
		// Token: 0x140002B9 RID: 697
		// (add) Token: 0x060037E5 RID: 14309 RVA: 0x000FB6AF File Offset: 0x000F98AF
		// (remove) Token: 0x060037E6 RID: 14310 RVA: 0x000FB6C8 File Offset: 0x000F98C8
		[SRCategory("CatBehavior")]
		[SRDescription("drawItemEventDescr")]
		public event DrawItemEventHandler DrawItem
		{
			add
			{
				this.onDrawItem = (DrawItemEventHandler)Delegate.Combine(this.onDrawItem, value);
			}
			remove
			{
				this.onDrawItem = (DrawItemEventHandler)Delegate.Remove(this.onDrawItem, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TabControl.RightToLeftLayout" /> property changes.</summary>
		// Token: 0x140002BA RID: 698
		// (add) Token: 0x060037E7 RID: 14311 RVA: 0x000FB6E1 File Offset: 0x000F98E1
		// (remove) Token: 0x060037E8 RID: 14312 RVA: 0x000FB6F4 File Offset: 0x000F98F4
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.TabControl.SelectedIndex" /> property has changed.</summary>
		// Token: 0x140002BB RID: 699
		// (add) Token: 0x060037E9 RID: 14313 RVA: 0x000FB707 File Offset: 0x000F9907
		// (remove) Token: 0x060037EA RID: 14314 RVA: 0x000FB720 File Offset: 0x000F9920
		[SRCategory("CatBehavior")]
		[SRDescription("selectedIndexChangedEventDescr")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				this.onSelectedIndexChanged = (EventHandler)Delegate.Combine(this.onSelectedIndexChanged, value);
			}
			remove
			{
				this.onSelectedIndexChanged = (EventHandler)Delegate.Remove(this.onSelectedIndexChanged, value);
			}
		}

		/// <summary>Occurs before a tab is selected, enabling a handler to cancel the tab change.</summary>
		// Token: 0x140002BC RID: 700
		// (add) Token: 0x060037EB RID: 14315 RVA: 0x000FB739 File Offset: 0x000F9939
		// (remove) Token: 0x060037EC RID: 14316 RVA: 0x000FB74C File Offset: 0x000F994C
		[SRCategory("CatAction")]
		[SRDescription("TabControlSelectingEventDescr")]
		public event TabControlCancelEventHandler Selecting
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_SELECTING, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_SELECTING, value);
			}
		}

		/// <summary>Occurs when a tab is selected.</summary>
		// Token: 0x140002BD RID: 701
		// (add) Token: 0x060037ED RID: 14317 RVA: 0x000FB75F File Offset: 0x000F995F
		// (remove) Token: 0x060037EE RID: 14318 RVA: 0x000FB772 File Offset: 0x000F9972
		[SRCategory("CatAction")]
		[SRDescription("TabControlSelectedEventDescr")]
		public event TabControlEventHandler Selected
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_SELECTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_SELECTED, value);
			}
		}

		/// <summary>Occurs before a tab is deselected, enabling a handler to cancel the tab change.</summary>
		// Token: 0x140002BE RID: 702
		// (add) Token: 0x060037EF RID: 14319 RVA: 0x000FB785 File Offset: 0x000F9985
		// (remove) Token: 0x060037F0 RID: 14320 RVA: 0x000FB798 File Offset: 0x000F9998
		[SRCategory("CatAction")]
		[SRDescription("TabControlDeselectingEventDescr")]
		public event TabControlCancelEventHandler Deselecting
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_DESELECTING, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_DESELECTING, value);
			}
		}

		/// <summary>Occurs when a tab is deselected. </summary>
		// Token: 0x140002BF RID: 703
		// (add) Token: 0x060037F1 RID: 14321 RVA: 0x000FB7AB File Offset: 0x000F99AB
		// (remove) Token: 0x060037F2 RID: 14322 RVA: 0x000FB7BE File Offset: 0x000F99BE
		[SRCategory("CatAction")]
		[SRDescription("TabControlDeselectedEventDescr")]
		public event TabControlEventHandler Deselected
		{
			add
			{
				base.Events.AddHandler(TabControl.EVENT_DESELECTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TabControl.EVENT_DESELECTED, value);
			}
		}

		/// <summary>This event is not meaningful for this control.</summary>
		// Token: 0x140002C0 RID: 704
		// (add) Token: 0x060037F3 RID: 14323 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x060037F4 RID: 14324 RVA: 0x00020D40 File Offset: 0x0001EF40
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

		// Token: 0x060037F5 RID: 14325 RVA: 0x000FB7D4 File Offset: 0x000F99D4
		internal int AddTabPage(TabPage tabPage, NativeMethods.TCITEM_T tcitem)
		{
			int num = this.AddNativeTabPage(tcitem);
			if (num >= 0)
			{
				this.Insert(num, tabPage);
			}
			return num;
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x000FB7F8 File Offset: 0x000F99F8
		internal int AddNativeTabPage(NativeMethods.TCITEM_T tcitem)
		{
			int result = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TCM_INSERTITEM, this.tabPageCount + 1, tcitem);
			UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), this.tabBaseReLayoutMessage, IntPtr.Zero, IntPtr.Zero);
			return result;
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x000FB850 File Offset: 0x000F9A50
		internal void ApplyItemSize()
		{
			if (base.IsHandleCreated && this.ShouldSerializeItemSize())
			{
				base.SendMessage(4905, 0, (int)NativeMethods.Util.MAKELPARAM(this.itemSize.Width, this.itemSize.Height));
			}
			this.cachedDisplayRect = Rectangle.Empty;
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x000FB8A5 File Offset: 0x000F9AA5
		internal void BeginUpdate()
		{
			base.BeginUpdateInternal();
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.CreateControlsInstance" />.</summary>
		/// <returns>A new instance of <see cref="T:System.Windows.Forms.Control.ControlCollection" /> assigned to the control.</returns>
		// Token: 0x060037F9 RID: 14329 RVA: 0x000FB8AD File Offset: 0x000F9AAD
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new TabControl.ControlCollection(this);
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.CreateHandle" />.</summary>
		// Token: 0x060037FA RID: 14330 RVA: 0x000FB8B8 File Offset: 0x000F9AB8
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 8
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x000FB908 File Offset: 0x000F9B08
		private void DetachImageList(object sender, EventArgs e)
		{
			this.ImageList = null;
		}

		/// <summary>Makes the tab following the tab with the specified index the current tab.</summary>
		/// <param name="index">The index in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection of the tab to deselect.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than the number of <see cref="T:System.Windows.Forms.TabPage" /> controls in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection minus 1.</exception>
		// Token: 0x060037FC RID: 14332 RVA: 0x000FB914 File Offset: 0x000F9B14
		public void DeselectTab(int index)
		{
			TabPage tabPage = this.GetTabPage(index);
			if (this.SelectedTab == tabPage)
			{
				if (0 <= index && index < this.TabPages.Count - 1)
				{
					this.SelectedTab = this.GetTabPage(++index);
					return;
				}
				this.SelectedTab = this.GetTabPage(0);
			}
		}

		/// <summary>Makes the tab following the specified <see cref="T:System.Windows.Forms.TabPage" /> the current tab.</summary>
		/// <param name="tabPage">The <see cref="T:System.Windows.Forms.TabPage" /> to deselect.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than the number of <see cref="T:System.Windows.Forms.TabPage" /> controls in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection minus 1.-or-
		///         <paramref name="tabPage" /> is not in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="tabPage" /> is <see langword="null" />.</exception>
		// Token: 0x060037FD RID: 14333 RVA: 0x000FB968 File Offset: 0x000F9B68
		public void DeselectTab(TabPage tabPage)
		{
			if (tabPage == null)
			{
				throw new ArgumentNullException("tabPage");
			}
			int index = this.FindTabPage(tabPage);
			this.DeselectTab(index);
		}

		/// <summary>Makes the tab following the tab with the specified name the current tab.</summary>
		/// <param name="tabPageName">The <see cref="P:System.Windows.Forms.Control.Name" /> of the tab to deselect.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="tabPageName" /> is <see langword="null" />.-or-
		///         <paramref name="tabPageName" /> does not match the <see cref="P:System.Windows.Forms.Control.Name" /> property of any <see cref="T:System.Windows.Forms.TabPage" /> in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</exception>
		// Token: 0x060037FE RID: 14334 RVA: 0x000FB994 File Offset: 0x000F9B94
		public void DeselectTab(string tabPageName)
		{
			if (tabPageName == null)
			{
				throw new ArgumentNullException("tabPageName");
			}
			TabPage tabPage = this.TabPages[tabPageName];
			this.DeselectTab(tabPage);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x060037FF RID: 14335 RVA: 0x000FB9C3 File Offset: 0x000F9BC3
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.imageList != null)
			{
				this.imageList.Disposed -= this.DetachImageList;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x000FB9EE File Offset: 0x000F9BEE
		internal void EndUpdate()
		{
			this.EndUpdate(true);
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x000FB9F7 File Offset: 0x000F9BF7
		internal void EndUpdate(bool invalidate)
		{
			base.EndUpdateInternal(invalidate);
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000FBA04 File Offset: 0x000F9C04
		internal int FindTabPage(TabPage tabPage)
		{
			if (this.tabPages != null)
			{
				for (int i = 0; i < this.tabPageCount; i++)
				{
					if (this.tabPages[i].Equals(tabPage))
					{
						return i;
					}
				}
			}
			return -1;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.TabPage" /> control at the specified location.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.TabPage" /> to get.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TabPage" /> at the specified location.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than the <see cref="P:System.Windows.Forms.TabControl.TabCount" />.</exception>
		// Token: 0x06003803 RID: 14339 RVA: 0x000FBA3D File Offset: 0x000F9C3D
		public Control GetControl(int index)
		{
			return this.GetTabPage(index);
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000FBA48 File Offset: 0x000F9C48
		internal TabPage GetTabPage(int index)
		{
			if (index < 0 || index >= this.tabPageCount)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return this.tabPages[index];
		}

		/// <summary>Gets an array of <see cref="T:System.Windows.Forms.TabPage" /> controls that belong to the <see cref="T:System.Windows.Forms.TabControl" /> control.</summary>
		/// <returns>An array of <see cref="T:System.Windows.Forms.TabPage" /> controls that belong to the <see cref="T:System.Windows.Forms.TabControl" />.</returns>
		// Token: 0x06003805 RID: 14341 RVA: 0x000FBA9C File Offset: 0x000F9C9C
		protected virtual object[] GetItems()
		{
			TabPage[] array = new TabPage[this.tabPageCount];
			if (this.tabPageCount > 0)
			{
				Array.Copy(this.tabPages, 0, array, 0, this.tabPageCount);
			}
			return array;
		}

		/// <summary>Copies the <see cref="T:System.Windows.Forms.TabPage" /> controls in the <see cref="T:System.Windows.Forms.TabControl" /> to an array of the specified type.</summary>
		/// <param name="baseType">The <see cref="T:System.Type" /> of the array to create.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.TabPage" /> controls that belong to the <see cref="T:System.Windows.Forms.TabControl" /> as an array of the specified type.</returns>
		/// <exception cref="T:System.ArrayTypeMismatchException">The type <see cref="T:System.Windows.Forms.TabPage" /> cannot be converted to <paramref name="baseType" />.</exception>
		// Token: 0x06003806 RID: 14342 RVA: 0x000FBAD4 File Offset: 0x000F9CD4
		protected virtual object[] GetItems(Type baseType)
		{
			object[] array = (object[])Array.CreateInstance(baseType, this.tabPageCount);
			if (this.tabPageCount > 0)
			{
				Array.Copy(this.tabPages, 0, array, 0, this.tabPageCount);
			}
			return array;
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x000FBB11 File Offset: 0x000F9D11
		internal TabPage[] GetTabPages()
		{
			return (TabPage[])this.GetItems();
		}

		/// <summary>Returns the bounding rectangle for a specified tab in this tab control.</summary>
		/// <param name="index">The zero-based index of the tab you want. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the specified tab.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index is less than zero.-or- The index is greater than or equal to <see cref="P:System.Windows.Forms.TabControl.TabPageCollection.Count" />. </exception>
		// Token: 0x06003808 RID: 14344 RVA: 0x000FBB20 File Offset: 0x000F9D20
		public Rectangle GetTabRect(int index)
		{
			if (index < 0 || (index >= this.tabPageCount && !this.tabControlState[8]))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.tabControlState[8] = false;
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			base.SendMessage(4874, index, ref rect);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		/// <summary>Gets the ToolTip for the specified <see cref="T:System.Windows.Forms.TabPage" />.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Forms.TabPage" /> that owns the desired ToolTip.</param>
		/// <returns>The ToolTip text.</returns>
		// Token: 0x06003809 RID: 14345 RVA: 0x000FBBC9 File Offset: 0x000F9DC9
		protected string GetToolTipText(object item)
		{
			return ((TabPage)item).ToolTipText;
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x000FBBD6 File Offset: 0x000F9DD6
		private void ImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4867, 0, this.ImageList.Handle);
			}
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x000FBBF8 File Offset: 0x000F9DF8
		internal void Insert(int index, TabPage tabPage)
		{
			if (this.tabPages == null)
			{
				this.tabPages = new TabPage[4];
			}
			else if (this.tabPages.Length == this.tabPageCount)
			{
				TabPage[] destinationArray = new TabPage[this.tabPageCount * 2];
				Array.Copy(this.tabPages, 0, destinationArray, 0, this.tabPageCount);
				this.tabPages = destinationArray;
			}
			if (index < this.tabPageCount)
			{
				Array.Copy(this.tabPages, index, this.tabPages, index + 1, this.tabPageCount - index);
			}
			this.tabPages[index] = tabPage;
			this.tabPageCount++;
			this.cachedDisplayRect = Rectangle.Empty;
			this.ApplyItemSize();
			if (this.Appearance == TabAppearance.FlatButtons)
			{
				base.Invalidate();
			}
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x000FBCB4 File Offset: 0x000F9EB4
		private void InsertItem(int index, TabPage tabPage)
		{
			if (index < 0 || (this.tabPages != null && index > this.tabPageCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (tabPage == null)
			{
				throw new ArgumentNullException("tabPage");
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.TCITEM_T tcitem = tabPage.GetTCITEM();
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TCM_INSERTITEM, index, tcitem);
				if (num >= 0)
				{
					this.Insert(num, tabPage);
				}
			}
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values. </param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600380D RID: 14349 RVA: 0x000FBD50 File Offset: 0x000F9F50
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			Keys keys = keyData & Keys.KeyCode;
			return keys - Keys.Prior <= 3 || base.IsInputKey(keyData);
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.OnHandleCreated(System.EventArgs)" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600380E RID: 14350 RVA: 0x000FBD88 File Offset: 0x000F9F88
		protected override void OnHandleCreated(EventArgs e)
		{
			NativeWindow.AddWindowToIDTable(this, base.Handle);
			this.handleInTable = true;
			if (!this.padding.IsEmpty)
			{
				base.SendMessage(4907, 0, NativeMethods.Util.MAKELPARAM(this.padding.X, this.padding.Y));
			}
			base.OnHandleCreated(e);
			this.cachedDisplayRect = Rectangle.Empty;
			this.ApplyItemSize();
			if (this.imageList != null)
			{
				base.SendMessage(4867, 0, this.imageList.Handle);
			}
			if (this.ShowToolTips)
			{
				IntPtr intPtr = base.SendMessage(4909, 0, 0);
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.SetWindowPos(new HandleRef(this, intPtr), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
				}
			}
			foreach (object obj in this.TabPages)
			{
				TabPage tabPage = (TabPage)obj;
				this.AddNativeTabPage(tabPage.GetTCITEM());
			}
			this.ResizePages();
			if (this.selectedIndex != -1)
			{
				try
				{
					this.tabControlState[16] = true;
					this.SelectedIndex = this.selectedIndex;
				}
				finally
				{
					this.tabControlState[16] = false;
				}
				this.selectedIndex = -1;
			}
			this.UpdateTabSelection(false);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600380F RID: 14351 RVA: 0x000FBEF8 File Offset: 0x000FA0F8
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (!base.Disposing)
			{
				this.selectedIndex = this.SelectedIndex;
			}
			if (this.handleInTable)
			{
				this.handleInTable = false;
				NativeWindow.RemoveWindowFromIDTable(base.Handle);
			}
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TabControl.DrawItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06003810 RID: 14352 RVA: 0x000FBF2F File Offset: 0x000FA12F
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{
			if (this.onDrawItem != null)
			{
				this.onDrawItem(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event of the <see cref="T:System.Windows.Forms.TabControl" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003811 RID: 14353 RVA: 0x000FBF46 File Offset: 0x000FA146
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if (this.SelectedTab != null)
			{
				this.SelectedTab.FireEnter(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event of the <see cref="T:System.Windows.Forms.TabControl" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003812 RID: 14354 RVA: 0x000FBF63 File Offset: 0x000FA163
		protected override void OnLeave(EventArgs e)
		{
			if (this.SelectedTab != null)
			{
				this.SelectedTab.FireLeave(e);
			}
			base.OnLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event. </summary>
		/// <param name="ke">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06003813 RID: 14355 RVA: 0x000FBF80 File Offset: 0x000FA180
		protected override void OnKeyDown(KeyEventArgs ke)
		{
			if (ke.KeyCode == Keys.Tab && (ke.KeyData & Keys.Control) != Keys.None)
			{
				bool forward = (ke.KeyData & Keys.Shift) == Keys.None;
				this.SelectNextTab(ke, forward);
			}
			if (ke.KeyCode == Keys.Next && (ke.KeyData & Keys.Control) != Keys.None)
			{
				this.SelectNextTab(ke, true);
			}
			if (ke.KeyCode == Keys.Prior && (ke.KeyData & Keys.Control) != Keys.None)
			{
				this.SelectNextTab(ke, false);
			}
			base.OnKeyDown(ke);
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x000FC004 File Offset: 0x000FA204
		internal override void OnParentHandleRecreated()
		{
			this.skipUpdateSize = true;
			try
			{
				base.OnParentHandleRecreated();
			}
			finally
			{
				this.skipUpdateSize = false;
			}
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.OnResize(System.EventArgs)" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003815 RID: 14357 RVA: 0x000FC038 File Offset: 0x000FA238
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(false);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TabControl.RightToLeftLayoutChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003816 RID: 14358 RVA: 0x000FC054 File Offset: 0x000FA254
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
			EventHandler eventHandler = base.Events[TabControl.EVENT_RIGHTTOLEFTLAYOUTCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TabControl.SelectedIndexChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003817 RID: 14359 RVA: 0x000FC09C File Offset: 0x000FA29C
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			int num = this.SelectedIndex;
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(this.tabControlState[32]);
			this.tabControlState[32] = false;
			if (this.onSelectedIndexChanged != null)
			{
				this.onSelectedIndexChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TabControl.Selecting" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TabControlCancelEventArgs" /> that contains the event data. </param>
		// Token: 0x06003818 RID: 14360 RVA: 0x000FC0F4 File Offset: 0x000FA2F4
		protected virtual void OnSelecting(TabControlCancelEventArgs e)
		{
			TabControlCancelEventHandler tabControlCancelEventHandler = (TabControlCancelEventHandler)base.Events[TabControl.EVENT_SELECTING];
			if (tabControlCancelEventHandler != null)
			{
				tabControlCancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TabControl.Selected" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TabControlEventArgs" /> that contains the event data. </param>
		// Token: 0x06003819 RID: 14361 RVA: 0x000FC124 File Offset: 0x000FA324
		protected virtual void OnSelected(TabControlEventArgs e)
		{
			TabControlEventHandler tabControlEventHandler = (TabControlEventHandler)base.Events[TabControl.EVENT_SELECTED];
			if (tabControlEventHandler != null)
			{
				tabControlEventHandler(this, e);
			}
			if (this.SelectedTab != null)
			{
				this.SelectedTab.FireEnter(EventArgs.Empty);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TabControl.Deselecting" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TabControlCancelEventArgs" /> that contains the event data. </param>
		// Token: 0x0600381A RID: 14362 RVA: 0x000FC16C File Offset: 0x000FA36C
		protected virtual void OnDeselecting(TabControlCancelEventArgs e)
		{
			TabControlCancelEventHandler tabControlCancelEventHandler = (TabControlCancelEventHandler)base.Events[TabControl.EVENT_DESELECTING];
			if (tabControlCancelEventHandler != null)
			{
				tabControlCancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TabControl.Deselected" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TabControlEventArgs" /> that contains the event data. </param>
		// Token: 0x0600381B RID: 14363 RVA: 0x000FC19C File Offset: 0x000FA39C
		protected virtual void OnDeselected(TabControlEventArgs e)
		{
			TabControlEventHandler tabControlEventHandler = (TabControlEventHandler)base.Events[TabControl.EVENT_DESELECTED];
			if (tabControlEventHandler != null)
			{
				tabControlEventHandler(this, e);
			}
			if (this.SelectedTab != null)
			{
				this.SelectedTab.FireLeave(EventArgs.Empty);
			}
		}

		/// <summary>Previews a keyboard message.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process.</param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600381C RID: 14364 RVA: 0x000FC1E2 File Offset: 0x000FA3E2
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyPreview(ref Message m)
		{
			return this.ProcessKeyEventArgs(ref m) || base.ProcessKeyPreview(ref m);
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x000FC1F8 File Offset: 0x000FA3F8
		internal void UpdateSize()
		{
			if (this.skipUpdateSize)
			{
				return;
			}
			this.BeginUpdate();
			Size size = base.Size;
			base.Size = new Size(size.Width + 1, size.Height);
			base.Size = size;
			this.EndUpdate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600381E RID: 14366 RVA: 0x000FC243 File Offset: 0x000FA443
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateSize();
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x000FC260 File Offset: 0x000FA460
		internal override void RecreateHandleCore()
		{
			TabPage[] array = this.GetTabPages();
			int num = (array.Length != 0 && this.SelectedIndex == -1) ? 0 : this.SelectedIndex;
			if (base.IsHandleCreated)
			{
				base.SendMessage(4873, 0, 0);
			}
			this.tabPages = null;
			this.tabPageCount = 0;
			base.RecreateHandleCore();
			for (int i = 0; i < array.Length; i++)
			{
				this.TabPages.Add(array[i]);
			}
			try
			{
				this.tabControlState[16] = true;
				this.SelectedIndex = num;
			}
			finally
			{
				this.tabControlState[16] = false;
			}
			this.UpdateSize();
		}

		/// <summary>Removes all the tab pages and additional controls from this tab control.</summary>
		// Token: 0x06003820 RID: 14368 RVA: 0x000FC310 File Offset: 0x000FA510
		protected void RemoveAll()
		{
			base.Controls.Clear();
			base.SendMessage(4873, 0, 0);
			this.tabPages = null;
			this.tabPageCount = 0;
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x000FC33C File Offset: 0x000FA53C
		internal void RemoveTabPage(int index)
		{
			if (index < 0 || index >= this.tabPageCount)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.tabPageCount--;
			if (index < this.tabPageCount)
			{
				Array.Copy(this.tabPages, index + 1, this.tabPages, index, this.tabPageCount - index);
			}
			this.tabPages[this.tabPageCount] = null;
			if (base.IsHandleCreated)
			{
				base.SendMessage(4872, index, 0);
			}
			this.cachedDisplayRect = Rectangle.Empty;
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000FC3EB File Offset: 0x000FA5EB
		private void ResetItemSize()
		{
			this.ItemSize = TabControl.DEFAULT_ITEMSIZE;
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000FC3F8 File Offset: 0x000FA5F8
		private void ResetPadding()
		{
			this.Padding = TabControl.DEFAULT_PADDING;
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x000FC408 File Offset: 0x000FA608
		private void ResizePages()
		{
			Rectangle displayRectangle = this.DisplayRectangle;
			TabPage[] array = this.GetTabPages();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Bounds = displayRectangle;
			}
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000FC43A File Offset: 0x000FA63A
		internal void SetToolTip(ToolTip toolTip, string controlToolTipText)
		{
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4910, new HandleRef(toolTip, toolTip.Handle), 0);
			this.controlTipText = controlToolTipText;
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x000FC468 File Offset: 0x000FA668
		internal void SetTabPage(int index, TabPage tabPage, NativeMethods.TCITEM_T tcitem)
		{
			if (index < 0 || index >= this.tabPageCount)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.TCM_SETITEM, index, tcitem);
			}
			if (base.DesignMode && base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4876, (IntPtr)index, IntPtr.Zero);
			}
			this.tabPages[index] = tabPage;
		}

		/// <summary>Makes the tab with the specified index the current tab.</summary>
		/// <param name="index">The index in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection of the tab to select.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than the number of <see cref="T:System.Windows.Forms.TabPage" /> controls in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection minus 1.</exception>
		// Token: 0x06003827 RID: 14375 RVA: 0x000FC510 File Offset: 0x000FA710
		public void SelectTab(int index)
		{
			TabPage tabPage = this.GetTabPage(index);
			if (tabPage != null)
			{
				this.SelectedTab = tabPage;
			}
		}

		/// <summary>Makes the specified <see cref="T:System.Windows.Forms.TabPage" /> the current tab.</summary>
		/// <param name="tabPage">The <see cref="T:System.Windows.Forms.TabPage" /> to select.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than the number of <see cref="T:System.Windows.Forms.TabPage" /> controls in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection minus 1.-or-
		///         <paramref name="tabPage" /> is not in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="tabPage" /> is <see langword="null" />.</exception>
		// Token: 0x06003828 RID: 14376 RVA: 0x000FC530 File Offset: 0x000FA730
		public void SelectTab(TabPage tabPage)
		{
			if (tabPage == null)
			{
				throw new ArgumentNullException("tabPage");
			}
			int index = this.FindTabPage(tabPage);
			this.SelectTab(index);
		}

		/// <summary>Makes the tab with the specified name the current tab.</summary>
		/// <param name="tabPageName">The <see cref="P:System.Windows.Forms.Control.Name" /> of the tab to select.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="tabPageName" /> is <see langword="null" />.-or-
		///         <paramref name="tabPageName" /> does not match the <see cref="P:System.Windows.Forms.Control.Name" /> property of any <see cref="T:System.Windows.Forms.TabPage" /> in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</exception>
		// Token: 0x06003829 RID: 14377 RVA: 0x000FC55C File Offset: 0x000FA75C
		public void SelectTab(string tabPageName)
		{
			if (tabPageName == null)
			{
				throw new ArgumentNullException("tabPageName");
			}
			TabPage tabPage = this.TabPages[tabPageName];
			this.SelectTab(tabPage);
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x000FC58C File Offset: 0x000FA78C
		private void SelectNextTab(KeyEventArgs ke, bool forward)
		{
			bool focused = this.Focused;
			if (this.WmSelChanging())
			{
				this.tabControlState[32] = false;
				return;
			}
			if (base.ValidationCancelled)
			{
				this.tabControlState[32] = false;
				return;
			}
			int num = this.SelectedIndex;
			if (num != -1)
			{
				int tabCount = this.TabCount;
				if (forward)
				{
					num = (num + 1) % tabCount;
				}
				else
				{
					num = (num + tabCount - 1) % tabCount;
				}
				try
				{
					this.tabControlState[32] = true;
					this.tabControlState[64] = true;
					this.SelectedIndex = num;
					this.tabControlState[64] = !focused;
					this.WmSelChange();
				}
				finally
				{
					this.tabControlState[64] = false;
					ke.Handled = true;
				}
			}
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool ShouldPerformContainerValidation()
		{
			return true;
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x000FC658 File Offset: 0x000FA858
		private bool ShouldSerializeItemSize()
		{
			return !this.itemSize.Equals(TabControl.DEFAULT_ITEMSIZE);
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x000FC678 File Offset: 0x000FA878
		private new bool ShouldSerializePadding()
		{
			return !this.padding.Equals(TabControl.DEFAULT_PADDING);
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.TabControl" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.TabControl" />. </returns>
		// Token: 0x0600382E RID: 14382 RVA: 0x000FC698 File Offset: 0x000FA898
		public override string ToString()
		{
			string text = base.ToString();
			if (this.TabPages != null)
			{
				text = text + ", TabPages.Count: " + this.TabPages.Count.ToString(CultureInfo.CurrentCulture);
				if (this.TabPages.Count > 0)
				{
					text = text + ", TabPages[0]: " + this.TabPages[0].ToString();
				}
			}
			return text;
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.ScaleCore(System.Single,System.Single)" />.</summary>
		/// <param name="dx">The horizontal scaling factor. </param>
		/// <param name="dy">The vertical scaling factor. </param>
		// Token: 0x0600382F RID: 14383 RVA: 0x000FC704 File Offset: 0x000FA904
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			this.currentlyScaling = true;
			base.ScaleCore(dx, dy);
			this.currentlyScaling = false;
		}

		/// <summary>Sets the <see cref="P:System.Windows.Forms.TabPage.Visible" /> property to <see langword="true" /> for the appropriate <see cref="T:System.Windows.Forms.TabPage" /> control in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</summary>
		/// <param name="updateFocus">
		///       <see langword="true" /> to change focus to the next <see cref="T:System.Windows.Forms.TabPage" />; otherwise, <see langword="false" />.</param>
		// Token: 0x06003830 RID: 14384 RVA: 0x000FC71C File Offset: 0x000FA91C
		protected void UpdateTabSelection(bool updateFocus)
		{
			if (base.IsHandleCreated)
			{
				int num = this.SelectedIndex;
				TabPage[] array = this.GetTabPages();
				if (num != -1)
				{
					if (this.currentlyScaling)
					{
						array[num].SuspendLayout();
					}
					array[num].Bounds = this.DisplayRectangle;
					array[num].Invalidate();
					if (this.currentlyScaling)
					{
						array[num].ResumeLayout(false);
					}
					array[num].Visible = true;
					if (updateFocus && (!this.Focused || this.tabControlState[64]))
					{
						this.tabControlState[32] = false;
						bool flag = false;
						IntSecurity.ModifyFocus.Assert();
						try
						{
							flag = array[num].SelectNextControl(null, true, true, false, false);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						if (flag)
						{
							if (!base.ContainsFocus)
							{
								IContainerControl containerControl = base.GetContainerControlInternal();
								if (containerControl != null)
								{
									while (containerControl.ActiveControl is ContainerControl)
									{
										containerControl = (IContainerControl)containerControl.ActiveControl;
									}
									if (containerControl.ActiveControl != null)
									{
										containerControl.ActiveControl.FocusInternal();
									}
								}
							}
						}
						else
						{
							IContainerControl containerControlInternal = base.GetContainerControlInternal();
							if (containerControlInternal != null && !base.DesignMode)
							{
								if (containerControlInternal is ContainerControl)
								{
									((ContainerControl)containerControlInternal).SetActiveControlInternal(this);
								}
								else
								{
									IntSecurity.ModifyFocus.Assert();
									try
									{
										containerControlInternal.ActiveControl = this;
									}
									finally
									{
										CodeAccessPermission.RevertAssert();
									}
								}
							}
						}
					}
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (i != this.SelectedIndex)
					{
						array[i].Visible = false;
					}
				}
			}
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.OnStyleChanged(System.EventArgs)" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003831 RID: 14385 RVA: 0x000FC8AC File Offset: 0x000FAAAC
		protected override void OnStyleChanged(EventArgs e)
		{
			base.OnStyleChanged(e);
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(false);
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x000FC8C8 File Offset: 0x000FAAC8
		internal void UpdateTab(TabPage tabPage)
		{
			int index = this.FindTabPage(tabPage);
			this.SetTabPage(index, tabPage, tabPage.GetTCITEM());
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(false);
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x000FC900 File Offset: 0x000FAB00
		private void WmNeedText(ref Message m)
		{
			NativeMethods.TOOLTIPTEXT tooltiptext = (NativeMethods.TOOLTIPTEXT)m.GetLParam(typeof(NativeMethods.TOOLTIPTEXT));
			int index = (int)tooltiptext.hdr.idFrom;
			string toolTipText = this.GetToolTipText(this.GetTabPage(index));
			if (!string.IsNullOrEmpty(toolTipText))
			{
				tooltiptext.lpszText = toolTipText;
			}
			else
			{
				tooltiptext.lpszText = this.controlTipText;
			}
			tooltiptext.hinst = IntPtr.Zero;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				tooltiptext.uFlags |= 4;
			}
			Marshal.StructureToPtr(tooltiptext, m.LParam, false);
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x000FC990 File Offset: 0x000FAB90
		private void WmReflectDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			IntPtr intPtr = Control.SetUpPalette(drawitemstruct.hDC, false, false);
			using (Graphics graphics = Graphics.FromHdcInternal(drawitemstruct.hDC))
			{
				this.OnDrawItem(new DrawItemEventArgs(graphics, this.Font, Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom), drawitemstruct.itemID, (DrawItemState)drawitemstruct.itemState));
			}
			if (intPtr != IntPtr.Zero)
			{
				SafeNativeMethods.SelectPalette(new HandleRef(null, drawitemstruct.hDC), new HandleRef(null, intPtr), 0);
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x000FCA6C File Offset: 0x000FAC6C
		private bool WmSelChange()
		{
			TabControlCancelEventArgs tabControlCancelEventArgs = new TabControlCancelEventArgs(this.SelectedTab, this.SelectedIndex, false, TabControlAction.Selecting);
			this.OnSelecting(tabControlCancelEventArgs);
			if (!tabControlCancelEventArgs.Cancel)
			{
				this.OnSelected(new TabControlEventArgs(this.SelectedTab, this.SelectedIndex, TabControlAction.Selected));
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
			else
			{
				base.SendMessage(4876, this.lastSelection, 0);
				this.UpdateTabSelection(true);
			}
			return tabControlCancelEventArgs.Cancel;
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x000FCAE4 File Offset: 0x000FACE4
		private bool WmSelChanging()
		{
			IContainerControl containerControlInternal = base.GetContainerControlInternal();
			if (containerControlInternal != null && !base.DesignMode)
			{
				if (containerControlInternal is ContainerControl)
				{
					((ContainerControl)containerControlInternal).SetActiveControlInternal(this);
				}
				else
				{
					IntSecurity.ModifyFocus.Assert();
					try
					{
						containerControlInternal.ActiveControl = this;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			this.lastSelection = this.SelectedIndex;
			TabControlCancelEventArgs tabControlCancelEventArgs = new TabControlCancelEventArgs(this.SelectedTab, this.SelectedIndex, false, TabControlAction.Deselecting);
			this.OnDeselecting(tabControlCancelEventArgs);
			if (!tabControlCancelEventArgs.Cancel)
			{
				this.OnDeselected(new TabControlEventArgs(this.SelectedTab, this.SelectedIndex, TabControlAction.Deselected));
			}
			return tabControlCancelEventArgs.Cancel;
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x000FCB90 File Offset: 0x000FAD90
		private void WmTabBaseReLayout(ref Message m)
		{
			this.BeginUpdate();
			this.cachedDisplayRect = Rectangle.Empty;
			this.UpdateTabSelection(false);
			this.EndUpdate();
			base.Invalidate(true);
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			IntPtr handle = base.Handle;
			while (UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(this, handle), this.tabBaseReLayoutMessage, this.tabBaseReLayoutMessage, 1))
			{
			}
		}

		/// <summary>This member overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />.</summary>
		/// <param name="m">A Windows Message Object. </param>
		// Token: 0x06003838 RID: 14392 RVA: 0x000FCBF0 File Offset: 0x000FADF0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int num = m.Msg;
			if (num <= 8235)
			{
				if (num != 78)
				{
					if (num != 8235)
					{
						goto IL_161;
					}
					this.WmReflectDrawItem(ref m);
					goto IL_161;
				}
			}
			else
			{
				if (num == 8236)
				{
					goto IL_161;
				}
				if (num != 8270)
				{
					goto IL_161;
				}
			}
			NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
			num = nmhdr.code;
			if (num <= -551)
			{
				if (num != -552)
				{
					if (num == -551)
					{
						if (this.WmSelChange())
						{
							m.Result = (IntPtr)1;
							this.tabControlState[32] = false;
							return;
						}
						this.tabControlState[32] = true;
					}
				}
				else
				{
					if (this.WmSelChanging())
					{
						m.Result = (IntPtr)1;
						this.tabControlState[32] = false;
						return;
					}
					if (base.ValidationCancelled)
					{
						m.Result = (IntPtr)1;
						this.tabControlState[32] = false;
						return;
					}
					this.tabControlState[32] = true;
				}
			}
			else if (num == -530 || num == -520)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(nmhdr, nmhdr.hwndFrom), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
				this.WmNeedText(ref m);
				m.Result = (IntPtr)1;
				return;
			}
			IL_161:
			if (m.Msg == this.tabBaseReLayoutMessage)
			{
				this.WmTabBaseReLayout(ref m);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x04002227 RID: 8743
		private static readonly Size DEFAULT_ITEMSIZE = Size.Empty;

		// Token: 0x04002228 RID: 8744
		private static readonly Point DEFAULT_PADDING = new Point(6, 3);

		// Token: 0x04002229 RID: 8745
		private TabControl.TabPageCollection tabCollection;

		// Token: 0x0400222A RID: 8746
		private TabAlignment alignment;

		// Token: 0x0400222B RID: 8747
		private TabDrawMode drawMode;

		// Token: 0x0400222C RID: 8748
		private ImageList imageList;

		// Token: 0x0400222D RID: 8749
		private Size itemSize = TabControl.DEFAULT_ITEMSIZE;

		// Token: 0x0400222E RID: 8750
		private Point padding = TabControl.DEFAULT_PADDING;

		// Token: 0x0400222F RID: 8751
		private TabSizeMode sizeMode;

		// Token: 0x04002230 RID: 8752
		private TabAppearance appearance;

		// Token: 0x04002231 RID: 8753
		private Rectangle cachedDisplayRect = Rectangle.Empty;

		// Token: 0x04002232 RID: 8754
		private bool currentlyScaling;

		// Token: 0x04002233 RID: 8755
		private int selectedIndex = -1;

		// Token: 0x04002234 RID: 8756
		private Size cachedSize = Size.Empty;

		// Token: 0x04002235 RID: 8757
		private string controlTipText = string.Empty;

		// Token: 0x04002236 RID: 8758
		private bool handleInTable;

		// Token: 0x04002237 RID: 8759
		private EventHandler onSelectedIndexChanged;

		// Token: 0x04002238 RID: 8760
		private DrawItemEventHandler onDrawItem;

		// Token: 0x04002239 RID: 8761
		private static readonly object EVENT_DESELECTING = new object();

		// Token: 0x0400223A RID: 8762
		private static readonly object EVENT_DESELECTED = new object();

		// Token: 0x0400223B RID: 8763
		private static readonly object EVENT_SELECTING = new object();

		// Token: 0x0400223C RID: 8764
		private static readonly object EVENT_SELECTED = new object();

		// Token: 0x0400223D RID: 8765
		private static readonly object EVENT_RIGHTTOLEFTLAYOUTCHANGED = new object();

		// Token: 0x0400223E RID: 8766
		private const int TABCONTROLSTATE_hotTrack = 1;

		// Token: 0x0400223F RID: 8767
		private const int TABCONTROLSTATE_multiline = 2;

		// Token: 0x04002240 RID: 8768
		private const int TABCONTROLSTATE_showToolTips = 4;

		// Token: 0x04002241 RID: 8769
		private const int TABCONTROLSTATE_getTabRectfromItemSize = 8;

		// Token: 0x04002242 RID: 8770
		private const int TABCONTROLSTATE_fromCreateHandles = 16;

		// Token: 0x04002243 RID: 8771
		private const int TABCONTROLSTATE_UISelection = 32;

		// Token: 0x04002244 RID: 8772
		private const int TABCONTROLSTATE_selectFirstControl = 64;

		// Token: 0x04002245 RID: 8773
		private const int TABCONTROLSTATE_insertingItem = 128;

		// Token: 0x04002246 RID: 8774
		private const int TABCONTROLSTATE_autoSize = 256;

		// Token: 0x04002247 RID: 8775
		private BitVector32 tabControlState;

		// Token: 0x04002248 RID: 8776
		private readonly int tabBaseReLayoutMessage = SafeNativeMethods.RegisterWindowMessage(Application.WindowMessagesVersion + "_TabBaseReLayout");

		// Token: 0x04002249 RID: 8777
		private TabPage[] tabPages;

		// Token: 0x0400224A RID: 8778
		private int tabPageCount;

		// Token: 0x0400224B RID: 8779
		private int lastSelection;

		// Token: 0x0400224C RID: 8780
		private bool rightToLeftLayout;

		// Token: 0x0400224D RID: 8781
		private bool skipUpdateSize;

		/// <summary>Contains a collection of <see cref="T:System.Windows.Forms.TabPage" /> objects.</summary>
		// Token: 0x02000722 RID: 1826
		public class TabPageCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.TabControl" /> that this collection belongs to. </param>
			/// <exception cref="T:System.ArgumentNullException">The specified <see cref="T:System.Windows.Forms.TabControl" /> is <see langword="null" />. </exception>
			// Token: 0x0600605F RID: 24671 RVA: 0x0018B027 File Offset: 0x00189227
			public TabPageCollection(TabControl owner)
			{
				if (owner == null)
				{
					throw new ArgumentNullException("owner");
				}
				this.owner = owner;
			}

			/// <summary>Gets or sets a <see cref="T:System.Windows.Forms.TabPage" /> in the collection.</summary>
			/// <param name="index">The zero-based index of the tab page to get or set. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.TabPage" /> at the specified index.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than zero or greater than the highest available index. </exception>
			// Token: 0x17001706 RID: 5894
			public virtual TabPage this[int index]
			{
				get
				{
					return this.owner.GetTabPage(index);
				}
				set
				{
					this.owner.SetTabPage(index, value, value.GetTCITEM());
				}
			}

			/// <summary>Gets or sets a <see cref="T:System.Windows.Forms.TabPage" /> in the collection.</summary>
			/// <param name="index">The zero-based index of the element to get.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.TabPage" /> at the specified index.</returns>
			/// <exception cref="T:System.ArgumentException">The value is not a <see cref="T:System.Windows.Forms.TabPage" />.</exception>
			// Token: 0x17001707 RID: 5895
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is TabPage)
					{
						this[index] = (TabPage)value;
						return;
					}
					throw new ArgumentException("value");
				}
			}

			/// <summary>Gets a tab page with the specified key from the collection.</summary>
			/// <param name="key">The name of the tab page to retrieve.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.TabPage" /> with the specified key.</returns>
			// Token: 0x17001708 RID: 5896
			public virtual TabPage this[string key]
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

			/// <summary>Gets the number of tab pages in the collection.</summary>
			/// <returns>The number of tab pages in the collection.</returns>
			// Token: 0x17001709 RID: 5897
			// (get) Token: 0x06006065 RID: 24677 RVA: 0x0018B0CD File Offset: 0x001892CD
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.tabPageCount;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" />.</returns>
			// Token: 0x1700170A RID: 5898
			// (get) Token: 0x06006066 RID: 24678 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" /> is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700170B RID: 5899
			// (get) Token: 0x06006067 RID: 24679 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" /> has a fixed size.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700170C RID: 5900
			// (get) Token: 0x06006068 RID: 24680 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>This property always returns <see langword="false" />.</returns>
			// Token: 0x1700170D RID: 5901
			// (get) Token: 0x06006069 RID: 24681 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds a <see cref="T:System.Windows.Forms.TabPage" /> to the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.TabPage" /> to add. </param>
			/// <exception cref="T:System.ArgumentNullException">The specified <paramref name="value" /> is <see langword="null" />. </exception>
			// Token: 0x0600606A RID: 24682 RVA: 0x0018B0DA File Offset: 0x001892DA
			public void Add(TabPage value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.owner.Controls.Add(value);
			}

			/// <summary>Adds a <see cref="T:System.Windows.Forms.TabPage" /> control to the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.TabPage" /> to add to the collection.</param>
			/// <returns>The position into which the <see cref="T:System.Windows.Forms.TabPage" /> was inserted.</returns>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.TabPage" />.</exception>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			// Token: 0x0600606B RID: 24683 RVA: 0x0018B0FB File Offset: 0x001892FB
			int IList.Add(object value)
			{
				if (value is TabPage)
				{
					this.Add((TabPage)value);
					return this.IndexOf((TabPage)value);
				}
				throw new ArgumentException("value");
			}

			/// <summary>Creates a tab page with the specified text, and adds it to the collection.</summary>
			/// <param name="text">The text to display on the tab page.</param>
			// Token: 0x0600606C RID: 24684 RVA: 0x0018B128 File Offset: 0x00189328
			public void Add(string text)
			{
				this.Add(new TabPage
				{
					Text = text
				});
			}

			/// <summary>Creates a tab page with the specified text and key, and adds it to the collection.</summary>
			/// <param name="key">The name of the tab page.</param>
			/// <param name="text">The text to display on the tab page.</param>
			// Token: 0x0600606D RID: 24685 RVA: 0x0018B14C File Offset: 0x0018934C
			public void Add(string key, string text)
			{
				this.Add(new TabPage
				{
					Name = key,
					Text = text
				});
			}

			/// <summary>Creates a tab page with the specified key, text, and image, and adds it to the collection.</summary>
			/// <param name="key">The name of the tab page.</param>
			/// <param name="text">The text to display on the tab page.</param>
			/// <param name="imageIndex">The index of the image to display on the tab page.</param>
			// Token: 0x0600606E RID: 24686 RVA: 0x0018B174 File Offset: 0x00189374
			public void Add(string key, string text, int imageIndex)
			{
				this.Add(new TabPage
				{
					Name = key,
					Text = text,
					ImageIndex = imageIndex
				});
			}

			/// <summary>Creates a tab page with the specified key, text, and image, and adds it to the collection.</summary>
			/// <param name="key">The name of the tab page.</param>
			/// <param name="text">The text to display on the tab page.</param>
			/// <param name="imageKey">The key of the image to display on the tab page.</param>
			// Token: 0x0600606F RID: 24687 RVA: 0x0018B1A4 File Offset: 0x001893A4
			public void Add(string key, string text, string imageKey)
			{
				this.Add(new TabPage
				{
					Name = key,
					Text = text,
					ImageKey = imageKey
				});
			}

			/// <summary>Adds a set of tab pages to the collection.</summary>
			/// <param name="pages">An array of type <see cref="T:System.Windows.Forms.TabPage" /> that contains the tab pages to add. </param>
			/// <exception cref="T:System.ArgumentNullException">The value of pages equals <see langword="null" />. </exception>
			// Token: 0x06006070 RID: 24688 RVA: 0x0018B1D4 File Offset: 0x001893D4
			public void AddRange(TabPage[] pages)
			{
				if (pages == null)
				{
					throw new ArgumentNullException("pages");
				}
				foreach (TabPage value in pages)
				{
					this.Add(value);
				}
			}

			/// <summary>Determines whether a specified tab page is in the collection.</summary>
			/// <param name="page">The <see cref="T:System.Windows.Forms.TabPage" /> to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified <see cref="T:System.Windows.Forms.TabPage" /> is in the collection; otherwise, <see langword="false" />.</returns>
			/// <exception cref="T:System.ArgumentNullException">The value of <paramref name="page" /> is <see langword="null" />. </exception>
			// Token: 0x06006071 RID: 24689 RVA: 0x0018B20A File Offset: 0x0018940A
			public bool Contains(TabPage page)
			{
				if (page == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.IndexOf(page) != -1;
			}

			/// <summary>Determines whether the specified <see cref="T:System.Windows.Forms.TabPage" /> control is in the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" />.</summary>
			/// <param name="page">The object to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the specified object is a <see cref="T:System.Windows.Forms.TabPage" /> in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06006072 RID: 24690 RVA: 0x0018B227 File Offset: 0x00189427
			bool IList.Contains(object page)
			{
				return page is TabPage && this.Contains((TabPage)page);
			}

			/// <summary>Determines whether the collection contains a tab page with the specified key.</summary>
			/// <param name="key">The name of the tab page to search for.</param>
			/// <returns>
			///     <see langword="true" /> to indicate a tab page with the specified key was found in the collection; otherwise, <see langword="false" />. </returns>
			// Token: 0x06006073 RID: 24691 RVA: 0x0018B23F File Offset: 0x0018943F
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Returns the index of the specified tab page in the collection.</summary>
			/// <param name="page">The <see cref="T:System.Windows.Forms.TabPage" /> to locate in the collection. </param>
			/// <returns>The zero-based index of the tab page; -1 if it cannot be found.</returns>
			/// <exception cref="T:System.ArgumentNullException">The value of <paramref name="page" /> is <see langword="null" />. </exception>
			// Token: 0x06006074 RID: 24692 RVA: 0x0018B250 File Offset: 0x00189450
			public int IndexOf(TabPage page)
			{
				if (page == null)
				{
					throw new ArgumentNullException("value");
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == page)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Returns the index of the specified <see cref="T:System.Windows.Forms.TabPage" /> control in the collection.</summary>
			/// <param name="page">The <see cref="T:System.Windows.Forms.TabPage" /> to locate in the collection.</param>
			/// <returns>The zero-based index if page is a <see cref="T:System.Windows.Forms.TabPage" /> in the collection; otherwise -1.</returns>
			// Token: 0x06006075 RID: 24693 RVA: 0x0018B289 File Offset: 0x00189489
			int IList.IndexOf(object page)
			{
				if (page is TabPage)
				{
					return this.IndexOf((TabPage)page);
				}
				return -1;
			}

			/// <summary>Returns the index of the first occurrence of the <see cref="T:System.Windows.Forms.TabPage" /> with the specified key.</summary>
			/// <param name="key">The name of the tab page to find in the collection.</param>
			/// <returns>The zero-based index of the first occurrence of a tab page with the specified key, if found; otherwise, -1.</returns>
			// Token: 0x06006076 RID: 24694 RVA: 0x0018B2A4 File Offset: 0x001894A4
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

			/// <summary>Inserts an existing tab page into the collection at the specified index. </summary>
			/// <param name="index">The zero-based index location where the tab page is inserted.</param>
			/// <param name="tabPage">The <see cref="T:System.Windows.Forms.TabPage" /> to insert in the collection.</param>
			// Token: 0x06006077 RID: 24695 RVA: 0x0018B324 File Offset: 0x00189524
			public void Insert(int index, TabPage tabPage)
			{
				this.owner.InsertItem(index, tabPage);
				try
				{
					this.owner.InsertingItem = true;
					this.owner.Controls.Add(tabPage);
				}
				finally
				{
					this.owner.InsertingItem = false;
				}
				this.owner.Controls.SetChildIndex(tabPage, index);
			}

			/// <summary>Inserts a <see cref="T:System.Windows.Forms.TabPage" /> control into the collection.</summary>
			/// <param name="index">The zero-based index at which the <see cref="T:System.Windows.Forms.TabPage" /> should be inserted.</param>
			/// <param name="tabPage">The <see cref="T:System.Windows.Forms.TabPage" /> to insert into the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" />.</param>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="tabPage" /> is not a <see cref="T:System.Windows.Forms.TabPage" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0, or index is greater than or equal to <see cref="P:System.Windows.Forms.TabControl.TabPageCollection.Count" />.</exception>
			// Token: 0x06006078 RID: 24696 RVA: 0x0018B38C File Offset: 0x0018958C
			void IList.Insert(int index, object tabPage)
			{
				if (tabPage is TabPage)
				{
					this.Insert(index, (TabPage)tabPage);
					return;
				}
				throw new ArgumentException("tabPage");
			}

			/// <summary>Creates a new tab page with the specified text and inserts it into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the tab page is inserted.</param>
			/// <param name="text">The text to display in the tab page.</param>
			// Token: 0x06006079 RID: 24697 RVA: 0x0018B3B0 File Offset: 0x001895B0
			public void Insert(int index, string text)
			{
				this.Insert(index, new TabPage
				{
					Text = text
				});
			}

			/// <summary>Creates a new tab page with the specified key and text, and inserts it into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the tab page is inserted.</param>
			/// <param name="key">The name of the tab page.</param>
			/// <param name="text">The text to display on the tab page.</param>
			// Token: 0x0600607A RID: 24698 RVA: 0x0018B3D4 File Offset: 0x001895D4
			public void Insert(int index, string key, string text)
			{
				this.Insert(index, new TabPage
				{
					Name = key,
					Text = text
				});
			}

			/// <summary>Creates a new tab page with the specified key, text, and image, and inserts it into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the tab page is inserted</param>
			/// <param name="key">The name of the tab page.</param>
			/// <param name="text">The text to display on the tab page</param>
			/// <param name="imageIndex">The zero-based index of the image to display on the tab page.</param>
			// Token: 0x0600607B RID: 24699 RVA: 0x0018B400 File Offset: 0x00189600
			public void Insert(int index, string key, string text, int imageIndex)
			{
				TabPage tabPage = new TabPage();
				tabPage.Name = key;
				tabPage.Text = text;
				this.Insert(index, tabPage);
				tabPage.ImageIndex = imageIndex;
			}

			/// <summary>Creates a tab page with the specified key, text, and image, and inserts it into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the tab page is inserted.</param>
			/// <param name="key">The name of the tab page.</param>
			/// <param name="text">The text to display on the tab page.</param>
			/// <param name="imageKey">The key of the image to display on the tab page.</param>
			// Token: 0x0600607C RID: 24700 RVA: 0x0018B434 File Offset: 0x00189634
			public void Insert(int index, string key, string text, string imageKey)
			{
				TabPage tabPage = new TabPage();
				tabPage.Name = key;
				tabPage.Text = text;
				this.Insert(index, tabPage);
				tabPage.ImageKey = imageKey;
			}

			// Token: 0x0600607D RID: 24701 RVA: 0x0018B465 File Offset: 0x00189665
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Removes all the tab pages from the collection.</summary>
			// Token: 0x0600607E RID: 24702 RVA: 0x0018B476 File Offset: 0x00189676
			public virtual void Clear()
			{
				this.owner.RemoveAll();
			}

			/// <summary>Copies the elements of the collection to the specified array, starting at the specified index.</summary>
			/// <param name="dest">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in the array at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="dest" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="dest" /> is multidimensional.-or-The number of elements in the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" /> is greater than the available space from index to the end of <paramref name="dest" />.</exception>
			/// <exception cref="T:System.InvalidCastException">The items in the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" /> cannot be cast automatically to the type of <paramref name="dest" />.</exception>
			// Token: 0x0600607F RID: 24703 RVA: 0x0018B483 File Offset: 0x00189683
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.owner.GetTabPages(), 0, dest, index, this.Count);
				}
			}

			/// <summary>Returns an enumeration of all the tab pages in the collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Windows.Forms.TabControl.TabPageCollection" />.</returns>
			// Token: 0x06006080 RID: 24704 RVA: 0x0018B4A8 File Offset: 0x001896A8
			public IEnumerator GetEnumerator()
			{
				TabPage[] tabPages = this.owner.GetTabPages();
				if (tabPages != null)
				{
					return tabPages.GetEnumerator();
				}
				return new TabPage[0].GetEnumerator();
			}

			/// <summary>Removes a <see cref="T:System.Windows.Forms.TabPage" /> from the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.TabPage" /> to remove. </param>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />. </exception>
			// Token: 0x06006081 RID: 24705 RVA: 0x0018B4D6 File Offset: 0x001896D6
			public void Remove(TabPage value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.owner.Controls.Remove(value);
			}

			/// <summary>Removes a <see cref="T:System.Windows.Forms.TabPage" /> from the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.TabPage" /> to remove.</param>
			// Token: 0x06006082 RID: 24706 RVA: 0x0018B4F7 File Offset: 0x001896F7
			void IList.Remove(object value)
			{
				if (value is TabPage)
				{
					this.Remove((TabPage)value);
				}
			}

			/// <summary>Removes the tab page at the specified index from the collection.</summary>
			/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.TabPage" /> to remove. </param>
			// Token: 0x06006083 RID: 24707 RVA: 0x0018B50D File Offset: 0x0018970D
			public void RemoveAt(int index)
			{
				this.owner.Controls.RemoveAt(index);
			}

			/// <summary>Removes the tab page with the specified key from the collection.</summary>
			/// <param name="key">The name of the tab page to remove.</param>
			// Token: 0x06006084 RID: 24708 RVA: 0x0018B520 File Offset: 0x00189720
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			// Token: 0x04004151 RID: 16721
			private TabControl owner;

			// Token: 0x04004152 RID: 16722
			private int lastAccessedIndex = -1;
		}

		/// <summary>Contains a collection of <see cref="T:System.Windows.Forms.Control" /> objects.</summary>
		// Token: 0x02000723 RID: 1827
		[ComVisible(false)]
		public new class ControlCollection : Control.ControlCollection
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TabControl.ControlCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.TabControl" /> that this collection belongs to. </param>
			// Token: 0x06006085 RID: 24709 RVA: 0x0018B545 File Offset: 0x00189745
			public ControlCollection(TabControl owner) : base(owner)
			{
				this.owner = owner;
			}

			/// <summary>Adds a <see cref="T:System.Windows.Forms.Control" /> to the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.Control" /> to add. </param>
			/// <exception cref="T:System.Exception">The specified <see cref="T:System.Windows.Forms.Control" /> is a <see cref="T:System.Windows.Forms.TabPage" />. </exception>
			// Token: 0x06006086 RID: 24710 RVA: 0x0018B558 File Offset: 0x00189758
			public override void Add(Control value)
			{
				if (!(value is TabPage))
				{
					throw new ArgumentException(SR.GetString("TabControlInvalidTabPageType", new object[]
					{
						value.GetType().Name
					}));
				}
				TabPage tabPage = (TabPage)value;
				if (!this.owner.InsertingItem)
				{
					if (this.owner.IsHandleCreated)
					{
						this.owner.AddTabPage(tabPage, tabPage.GetTCITEM());
					}
					else
					{
						this.owner.Insert(this.owner.TabCount, tabPage);
					}
				}
				base.Add(tabPage);
				tabPage.Visible = false;
				if (this.owner.IsHandleCreated)
				{
					tabPage.Bounds = this.owner.DisplayRectangle;
				}
				ISite site = this.owner.Site;
				if (site != null && tabPage.Site == null)
				{
					IContainer container = site.Container;
					if (container != null)
					{
						container.Add(tabPage);
					}
				}
				this.owner.ApplyItemSize();
				this.owner.UpdateTabSelection(false);
			}

			/// <summary>Removes a <see cref="T:System.Windows.Forms.Control" /> from the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.Control" /> to remove. </param>
			// Token: 0x06006087 RID: 24711 RVA: 0x0018B64C File Offset: 0x0018984C
			public override void Remove(Control value)
			{
				base.Remove(value);
				if (!(value is TabPage))
				{
					return;
				}
				int num = this.owner.FindTabPage((TabPage)value);
				int selectedIndex = this.owner.SelectedIndex;
				if (num != -1)
				{
					this.owner.RemoveTabPage(num);
					if (num == selectedIndex)
					{
						this.owner.SelectedIndex = 0;
					}
				}
				this.owner.UpdateTabSelection(false);
			}

			// Token: 0x04004153 RID: 16723
			private TabControl owner;
		}
	}
}
