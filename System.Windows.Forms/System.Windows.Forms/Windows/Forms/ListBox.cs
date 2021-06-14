using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using Accessibility;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows control to display a list of items. </summary>
	// Token: 0x020002BC RID: 700
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ListBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[DefaultBindingProperty("SelectedValue")]
	[SRDescription("DescriptionListBox")]
	public class ListBox : ListControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListBox" /> class.</summary>
		// Token: 0x06002884 RID: 10372 RVA: 0x000BD470 File Offset: 0x000BB670
		public ListBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick | ControlStyles.UseTextForAccessibility, false);
			base.SetState2(2048, true);
			base.SetBounds(0, 0, 120, 96);
			this.requestedHeight = base.Height;
			this.PrepareForDrawing();
		}

		/// <summary>Provides constants for rescaling the control when a DPI change occurs.</summary>
		/// <param name="deviceDpiOld">The DPI value prior to the change.</param>
		/// <param name="deviceDpiNew">The DPI value after the change.</param>
		// Token: 0x06002885 RID: 10373 RVA: 0x000BD500 File Offset: 0x000BB700
		protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
		{
			base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
			this.PrepareForDrawing();
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000BD510 File Offset: 0x000BB710
		private void PrepareForDrawing()
		{
			if (DpiHelper.EnableCheckedListBoxHighDpiImprovements)
			{
				this.scaledListItemStartPosition = base.LogicalToDeviceUnits(1);
				this.scaledListItemBordersHeight = 2 * base.LogicalToDeviceUnits(1);
				this.scaledListItemPaddingBuffer = base.LogicalToDeviceUnits(3);
			}
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06002887 RID: 10375 RVA: 0x0001FD6B File Offset: 0x0001DF6B
		// (set) Token: 0x06002888 RID: 10376 RVA: 0x00011FB9 File Offset: 0x000101B9
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The background image of the form.</returns>
		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x0600288A RID: 10378 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListBox.BackgroundImage" /> property of the label changes.</summary>
		// Token: 0x140001DB RID: 475
		// (add) Token: 0x0600288B RID: 10379 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x0600288C RID: 10380 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>Gets or sets the background image layout for a <see cref="T:System.Windows.Forms.ListBox" /> as defined in the <see cref="T:System.Windows.Forms.ImageLayout" /> enumeration.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout" />. The values are <see langword="Center" />, <see langword="None" />, <see langword="Stretch" />, <see langword="Tile" />, or <see langword="Zoom" />. <see langword="Center" /> is the default value.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified enumeration value does not exist. </exception>
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x0600288E RID: 10382 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListBox.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140001DC RID: 476
		// (add) Token: 0x0600288F RID: 10383 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06002890 RID: 10384 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets the type of border that is drawn around the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see cref="F:System.Windows.Forms.BorderStyle.Fixed3D" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values.</exception>
		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06002891 RID: 10385 RVA: 0x000BD542 File Offset: 0x000BB742
		// (set) Token: 0x06002892 RID: 10386 RVA: 0x000BD54C File Offset: 0x000BB74C
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("ListBoxBorderDescr")]
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
				if (value != this.borderStyle)
				{
					this.borderStyle = value;
					base.RecreateHandle();
					this.integralHeightAdjust = true;
					try
					{
						base.Height = this.requestedHeight;
					}
					finally
					{
						this.integralHeightAdjust = false;
					}
				}
			}
		}

		/// <summary>Gets or sets the width of columns in a multicolumn <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>The width, in pixels, of each column in the control. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentException">A value less than zero is assigned to the property. </exception>
		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06002893 RID: 10387 RVA: 0x000BD5C4 File Offset: 0x000BB7C4
		// (set) Token: 0x06002894 RID: 10388 RVA: 0x000BD5CC File Offset: 0x000BB7CC
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(0)]
		[SRDescription("ListBoxColumnWidthDescr")]
		public int ColumnWidth
		{
			get
			{
				return this.columnWidth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.columnWidth != value)
				{
					this.columnWidth = value;
					if (this.columnWidth == 0)
					{
						base.RecreateHandle();
						return;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(405, this.columnWidth, 0);
					}
				}
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x000BD658 File Offset: 0x000BB858
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "LISTBOX";
				createParams.Style |= 2097217;
				if (this.scrollAlwaysVisible)
				{
					createParams.Style |= 4096;
				}
				if (!this.integralHeight)
				{
					createParams.Style |= 256;
				}
				if (this.useTabStops)
				{
					createParams.Style |= 128;
				}
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
				if (this.multiColumn)
				{
					createParams.Style |= 1049088;
				}
				else if (this.horizontalScrollbar)
				{
					createParams.Style |= 1048576;
				}
				switch (this.selectionMode)
				{
				case SelectionMode.None:
					createParams.Style |= 16384;
					break;
				case SelectionMode.MultiSimple:
					createParams.Style |= 8;
					break;
				case SelectionMode.MultiExtended:
					createParams.Style |= 2048;
					break;
				}
				switch (this.drawMode)
				{
				case DrawMode.OwnerDrawFixed:
					createParams.Style |= 16;
					break;
				case DrawMode.OwnerDrawVariable:
					createParams.Style |= 32;
					break;
				}
				return createParams;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ListBox" /> recognizes and expands tab characters when it draws its strings by using the <see cref="P:System.Windows.Forms.ListBox.CustomTabOffsets" /> integer array.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ListBox" /> recognizes and expands tab characters; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06002896 RID: 10390 RVA: 0x000BD7D3 File Offset: 0x000BB9D3
		// (set) Token: 0x06002897 RID: 10391 RVA: 0x000BD7DB File Offset: 0x000BB9DB
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Browsable(false)]
		public bool UseCustomTabOffsets
		{
			get
			{
				return this.useCustomTabOffsets;
			}
			set
			{
				if (this.useCustomTabOffsets != value)
				{
					this.useCustomTabOffsets = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06002898 RID: 10392 RVA: 0x000BD7F3 File Offset: 0x000BB9F3
		protected override Size DefaultSize
		{
			get
			{
				return new Size(120, 96);
			}
		}

		/// <summary>Gets or sets the drawing mode for the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DrawMode" /> values representing the mode for drawing the items of the control. The default is <see langword="DrawMode.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to the property is not a member of the <see cref="T:System.Windows.Forms.DrawMode" /> enumeration. </exception>
		/// <exception cref="T:System.ArgumentException">A multicolumn <see cref="T:System.Windows.Forms.ListBox" /> cannot have a variable-sized height. </exception>
		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x000BD7FE File Offset: 0x000BB9FE
		// (set) Token: 0x0600289A RID: 10394 RVA: 0x000BD808 File Offset: 0x000BBA08
		[SRCategory("CatBehavior")]
		[DefaultValue(DrawMode.Normal)]
		[SRDescription("ListBoxDrawModeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual DrawMode DrawMode
		{
			get
			{
				return this.drawMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DrawMode));
				}
				if (this.drawMode != value)
				{
					if (this.MultiColumn && value == DrawMode.OwnerDrawVariable)
					{
						throw new ArgumentException(SR.GetString("ListBoxVarHeightMultiCol"), "value");
					}
					this.drawMode = value;
					base.RecreateHandle();
					if (this.drawMode == DrawMode.OwnerDrawVariable)
					{
						LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.DrawMode);
					}
				}
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x000BD892 File Offset: 0x000BBA92
		internal int FocusedIndex
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)((long)base.SendMessage(415, 0, 0));
				}
				return -1;
			}
		}

		/// <summary>Gets or sets the font of the text displayed by the control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x0600289D RID: 10397 RVA: 0x000BD8B1 File Offset: 0x000BBAB1
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				if (!this.integralHeight)
				{
					this.RefreshItems();
				}
			}
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.</returns>
		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x000201D0 File Offset: 0x0001E3D0
		// (set) Token: 0x0600289F RID: 10399 RVA: 0x0001208A File Offset: 0x0001028A
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Gets or sets the width by which the horizontal scroll bar of a <see cref="T:System.Windows.Forms.ListBox" /> can scroll.</summary>
		/// <returns>The width, in pixels, that the horizontal scroll bar can scroll the control. The default is zero.</returns>
		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x000BD8C8 File Offset: 0x000BBAC8
		// (set) Token: 0x060028A1 RID: 10401 RVA: 0x000BD8D0 File Offset: 0x000BBAD0
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Localizable(true)]
		[SRDescription("ListBoxHorizontalExtentDescr")]
		public int HorizontalExtent
		{
			get
			{
				return this.horizontalExtent;
			}
			set
			{
				if (value != this.horizontalExtent)
				{
					this.horizontalExtent = value;
					this.UpdateHorizontalExtent();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a horizontal scroll bar is displayed in the control.</summary>
		/// <returns>
		///     <see langword="true" /> to display a horizontal scroll bar in the control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000BD8E8 File Offset: 0x000BBAE8
		// (set) Token: 0x060028A3 RID: 10403 RVA: 0x000BD8F0 File Offset: 0x000BBAF0
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("ListBoxHorizontalScrollbarDescr")]
		public bool HorizontalScrollbar
		{
			get
			{
				return this.horizontalScrollbar;
			}
			set
			{
				if (value != this.horizontalScrollbar)
				{
					this.horizontalScrollbar = value;
					this.RefreshItems();
					if (!this.MultiColumn)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the control should resize to avoid showing partial items.</summary>
		/// <returns>
		///     <see langword="true" /> if the control resizes so that it does not display partial items; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060028A4 RID: 10404 RVA: 0x000BD916 File Offset: 0x000BBB16
		// (set) Token: 0x060028A5 RID: 10405 RVA: 0x000BD920 File Offset: 0x000BBB20
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ListBoxIntegralHeightDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public bool IntegralHeight
		{
			get
			{
				return this.integralHeight;
			}
			set
			{
				if (this.integralHeight != value)
				{
					this.integralHeight = value;
					base.RecreateHandle();
					this.integralHeightAdjust = true;
					try
					{
						base.Height = this.requestedHeight;
					}
					finally
					{
						this.integralHeightAdjust = false;
					}
				}
			}
		}

		/// <summary>Gets or sets the height of an item in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>The height, in pixels, of an item in the control.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <see cref="P:System.Windows.Forms.ListBox.ItemHeight" /> property was set to less than 0 or more than 255 pixels. </exception>
		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060028A6 RID: 10406 RVA: 0x000BD970 File Offset: 0x000BBB70
		// (set) Token: 0x060028A7 RID: 10407 RVA: 0x000BD994 File Offset: 0x000BBB94
		[SRCategory("CatBehavior")]
		[DefaultValue(13)]
		[Localizable(true)]
		[SRDescription("ListBoxItemHeightDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual int ItemHeight
		{
			get
			{
				if (this.drawMode == DrawMode.OwnerDrawFixed || this.drawMode == DrawMode.OwnerDrawVariable)
				{
					return this.itemHeight;
				}
				return this.GetItemHeight(0);
			}
			set
			{
				if (value < 1 || value > 255)
				{
					throw new ArgumentOutOfRangeException("ItemHeight", SR.GetString("InvalidExBoundArgument", new object[]
					{
						"ItemHeight",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture),
						"256"
					}));
				}
				if (this.itemHeight != value)
				{
					this.itemHeight = value;
					if (this.drawMode == DrawMode.OwnerDrawFixed && base.IsHandleCreated)
					{
						this.BeginUpdate();
						base.SendMessage(416, 0, value);
						if (this.IntegralHeight)
						{
							Size size = base.Size;
							base.Size = new Size(size.Width + 1, size.Height);
							base.Size = size;
						}
						this.EndUpdate();
					}
				}
			}
		}

		/// <summary>Gets the items of the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> representing the items in the <see cref="T:System.Windows.Forms.ListBox" />.</returns>
		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x000BDA64 File Offset: 0x000BBC64
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ListBoxItemsDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		public ListBox.ObjectCollection Items
		{
			get
			{
				if (this.itemsCollection == null)
				{
					this.itemsCollection = this.CreateItemCollection();
				}
				return this.itemsCollection;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x000BDA80 File Offset: 0x000BBC80
		internal virtual int MaxItemWidth
		{
			get
			{
				if (this.horizontalExtent > 0)
				{
					return this.horizontalExtent;
				}
				if (this.DrawMode != DrawMode.Normal)
				{
					return -1;
				}
				if (this.maxWidth > -1)
				{
					return this.maxWidth;
				}
				this.maxWidth = this.ComputeMaxItemWidth(this.maxWidth);
				return this.maxWidth;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ListBox" /> supports multiple columns.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ListBox" /> supports multiple columns; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">A multicolumn <see cref="T:System.Windows.Forms.ListBox" /> cannot have a variable-sized height. </exception>
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x060028AA RID: 10410 RVA: 0x000BDACF File Offset: 0x000BBCCF
		// (set) Token: 0x060028AB RID: 10411 RVA: 0x000BDAD7 File Offset: 0x000BBCD7
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListBoxMultiColumnDescr")]
		public bool MultiColumn
		{
			get
			{
				return this.multiColumn;
			}
			set
			{
				if (this.multiColumn != value)
				{
					if (value && this.drawMode == DrawMode.OwnerDrawVariable)
					{
						throw new ArgumentException(SR.GetString("ListBoxVarHeightMultiCol"), "value");
					}
					this.multiColumn = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets the combined height of all items in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>The combined height, in pixels, of all items in the control.</returns>
		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x000BDB10 File Offset: 0x000BBD10
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListBoxPreferredHeightDescr")]
		public int PreferredHeight
		{
			get
			{
				int num = 0;
				if (this.drawMode == DrawMode.OwnerDrawVariable)
				{
					if (base.RecreatingHandle || base.GetState(262144))
					{
						num = base.Height;
					}
					else if (this.itemsCollection != null)
					{
						int count = this.itemsCollection.Count;
						for (int i = 0; i < count; i++)
						{
							num += this.GetItemHeight(i);
						}
					}
				}
				else
				{
					int num2 = (this.itemsCollection == null || this.itemsCollection.Count == 0) ? 1 : this.itemsCollection.Count;
					num = this.GetItemHeight(0) * num2;
				}
				if (this.borderStyle != BorderStyle.None)
				{
					num += SystemInformation.BorderSize.Height * 4 + 3;
				}
				return num;
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000BDBC0 File Offset: 0x000BBDC0
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			int preferredHeight = this.PreferredHeight;
			if (base.IsHandleCreated)
			{
				int num = this.SizeFromClientSize(new Size(this.MaxItemWidth, preferredHeight)).Width;
				num += SystemInformation.VerticalScrollBarWidth + 4;
				return new Size(num, preferredHeight) + this.Padding.Size;
			}
			return this.DefaultSize;
		}

		/// <summary>Gets or sets a value indicating whether text displayed by the control is displayed from right to left.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft" /> values.</returns>
		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x060028AE RID: 10414 RVA: 0x000BDC24 File Offset: 0x000BBE24
		// (set) Token: 0x060028AF RID: 10415 RVA: 0x000BDC35 File Offset: 0x000BBE35
		public override RightToLeft RightToLeft
		{
			get
			{
				if (!ListBox.RunningOnWin2K)
				{
					return RightToLeft.No;
				}
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060028B0 RID: 10416 RVA: 0x000BDC3E File Offset: 0x000BBE3E
		private static bool RunningOnWin2K
		{
			get
			{
				if (!ListBox.checkedOS && (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5))
				{
					ListBox.runningOnWin2K = false;
					ListBox.checkedOS = true;
				}
				return ListBox.runningOnWin2K;
			}
		}

		/// <summary>Gets or sets a value indicating whether the vertical scroll bar is shown at all times.</summary>
		/// <returns>
		///     <see langword="true" /> if the vertical scroll bar should always be displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x060028B1 RID: 10417 RVA: 0x000BDC77 File Offset: 0x000BBE77
		// (set) Token: 0x060028B2 RID: 10418 RVA: 0x000BDC7F File Offset: 0x000BBE7F
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("ListBoxScrollIsVisibleDescr")]
		public bool ScrollAlwaysVisible
		{
			get
			{
				return this.scrollAlwaysVisible;
			}
			set
			{
				if (this.scrollAlwaysVisible != value)
				{
					this.scrollAlwaysVisible = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ListBox" /> currently enables selection of list items.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="T:System.Windows.Forms.SelectionMode" /> is not <see cref="F:System.Windows.Forms.SelectionMode.None" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x000BDC97 File Offset: 0x000BBE97
		protected override bool AllowSelection
		{
			get
			{
				return this.selectionMode > SelectionMode.None;
			}
		}

		/// <summary>Gets or sets the zero-based index of the currently selected item in a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>A zero-based index of the currently selected item. A value of negative one (-1) is returned if no item is selected.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than -1 or greater than or equal to the item count.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Windows.Forms.ListBox.SelectionMode" /> property is set to <see langword="None" />.</exception>
		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x000BDCA4 File Offset: 0x000BBEA4
		// (set) Token: 0x060028B5 RID: 10421 RVA: 0x000BDD1C File Offset: 0x000BBF1C
		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListBoxSelectedIndexDescr")]
		public override int SelectedIndex
		{
			get
			{
				SelectionMode selectionMode = this.selectionModeChanging ? this.cachedSelectionMode : this.selectionMode;
				if (selectionMode == SelectionMode.None)
				{
					return -1;
				}
				if (selectionMode == SelectionMode.One && base.IsHandleCreated)
				{
					return (int)((long)base.SendMessage(392, 0, 0));
				}
				if (this.itemsCollection != null && this.SelectedItems.Count > 0)
				{
					return this.Items.IndexOfIdentifier(this.SelectedItems.GetObjectAt(0));
				}
				return -1;
			}
			set
			{
				int num = (this.itemsCollection == null) ? 0 : this.itemsCollection.Count;
				if (value < -1 || value >= num)
				{
					throw new ArgumentOutOfRangeException("SelectedIndex", SR.GetString("InvalidArgument", new object[]
					{
						"SelectedIndex",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.selectionMode == SelectionMode.None)
				{
					throw new ArgumentException(SR.GetString("ListBoxInvalidSelectionMode"), "SelectedIndex");
				}
				if (this.selectionMode == SelectionMode.One && value != -1)
				{
					int selectedIndex = this.SelectedIndex;
					if (selectedIndex != value)
					{
						if (selectedIndex != -1)
						{
							this.SelectedItems.SetSelected(selectedIndex, false);
						}
						this.SelectedItems.SetSelected(value, true);
						if (base.IsHandleCreated)
						{
							this.NativeSetSelected(value, true);
						}
						this.OnSelectedIndexChanged(EventArgs.Empty);
						return;
					}
				}
				else if (value == -1)
				{
					if (this.SelectedIndex != -1)
					{
						this.ClearSelected();
						return;
					}
				}
				else if (!this.SelectedItems.GetSelected(value))
				{
					this.SelectedItems.SetSelected(value, true);
					if (base.IsHandleCreated)
					{
						this.NativeSetSelected(value, true);
					}
					this.OnSelectedIndexChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets a collection that contains the zero-based indexes of all currently selected items in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" /> containing the indexes of the currently selected items in the control. If no items are currently selected, an empty <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" /> is returned.</returns>
		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x000BDE36 File Offset: 0x000BC036
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListBoxSelectedIndicesDescr")]
		public ListBox.SelectedIndexCollection SelectedIndices
		{
			get
			{
				if (this.selectedIndices == null)
				{
					this.selectedIndices = new ListBox.SelectedIndexCollection(this);
				}
				return this.selectedIndices;
			}
		}

		/// <summary>Gets or sets the currently selected item in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>An object that represents the current selection in the control.</returns>
		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x000BDE52 File Offset: 0x000BC052
		// (set) Token: 0x060028B8 RID: 10424 RVA: 0x000BDE70 File Offset: 0x000BC070
		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListBoxSelectedItemDescr")]
		public object SelectedItem
		{
			get
			{
				if (this.SelectedItems.Count > 0)
				{
					return this.SelectedItems[0];
				}
				return null;
			}
			set
			{
				if (this.itemsCollection != null)
				{
					if (value != null)
					{
						int num = this.itemsCollection.IndexOf(value);
						if (num != -1)
						{
							this.SelectedIndex = num;
							return;
						}
					}
					else
					{
						this.SelectedIndex = -1;
					}
				}
			}
		}

		/// <summary>Gets a collection containing the currently selected items in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListBox.SelectedObjectCollection" /> containing the currently selected items in the control.</returns>
		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060028B9 RID: 10425 RVA: 0x000BDEA8 File Offset: 0x000BC0A8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListBoxSelectedItemsDescr")]
		public ListBox.SelectedObjectCollection SelectedItems
		{
			get
			{
				if (this.selectedItems == null)
				{
					this.selectedItems = new ListBox.SelectedObjectCollection(this);
				}
				return this.selectedItems;
			}
		}

		/// <summary>Gets or sets the method in which items are selected in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.SelectionMode" /> values. The default is <see langword="SelectionMode.One" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.SelectionMode" /> values.</exception>
		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x060028BA RID: 10426 RVA: 0x000BDEC4 File Offset: 0x000BC0C4
		// (set) Token: 0x060028BB RID: 10427 RVA: 0x000BDECC File Offset: 0x000BC0CC
		[SRCategory("CatBehavior")]
		[DefaultValue(SelectionMode.One)]
		[SRDescription("ListBoxSelectionModeDescr")]
		public virtual SelectionMode SelectionMode
		{
			get
			{
				return this.selectionMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SelectionMode));
				}
				if (this.selectionMode != value)
				{
					this.SelectedItems.EnsureUpToDate();
					this.selectionMode = value;
					try
					{
						this.selectionModeChanging = true;
						base.RecreateHandle();
					}
					finally
					{
						this.selectionModeChanging = false;
						this.cachedSelectionMode = this.selectionMode;
						if (base.IsHandleCreated)
						{
							this.NativeUpdateSelection();
						}
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the items in the <see cref="T:System.Windows.Forms.ListBox" /> are sorted alphabetically.</summary>
		/// <returns>
		///     <see langword="true" /> if items in the control are sorted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x060028BC RID: 10428 RVA: 0x000BDF5C File Offset: 0x000BC15C
		// (set) Token: 0x060028BD RID: 10429 RVA: 0x000BDF64 File Offset: 0x000BC164
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListBoxSortedDescr")]
		public bool Sorted
		{
			get
			{
				return this.sorted;
			}
			set
			{
				if (this.sorted != value)
				{
					this.sorted = value;
					if (this.sorted && this.itemsCollection != null && this.itemsCollection.Count >= 1)
					{
						this.Sort();
					}
				}
			}
		}

		/// <summary>Gets or searches for the text of the currently selected item in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>The text of the currently selected item in the control.</returns>
		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x060028BE RID: 10430 RVA: 0x000BDF9A File Offset: 0x000BC19A
		// (set) Token: 0x060028BF RID: 10431 RVA: 0x000BDFDC File Offset: 0x000BC1DC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				if (this.SelectionMode == SelectionMode.None || this.SelectedItem == null)
				{
					return base.Text;
				}
				if (base.FormattingEnabled)
				{
					return base.GetItemText(this.SelectedItem);
				}
				return base.FilterItemOnProperty(this.SelectedItem).ToString();
			}
			set
			{
				base.Text = value;
				if (this.SelectionMode != SelectionMode.None && value != null && (this.SelectedItem == null || !value.Equals(base.GetItemText(this.SelectedItem))))
				{
					int count = this.Items.Count;
					for (int i = 0; i < count; i++)
					{
						if (string.Compare(value, base.GetItemText(this.Items[i]), true, CultureInfo.CurrentCulture) == 0)
						{
							this.SelectedIndex = i;
							return;
						}
					}
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListBox.Text" /> property is changed.</summary>
		// Token: 0x140001DD RID: 477
		// (add) Token: 0x060028C0 RID: 10432 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x060028C1 RID: 10433 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		/// <summary>Gets or sets the index of the first visible item in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>The zero-based index of the first visible item in the control.</returns>
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000BE057 File Offset: 0x000BC257
		// (set) Token: 0x060028C3 RID: 10435 RVA: 0x000BE07B File Offset: 0x000BC27B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListBoxTopIndexDescr")]
		public int TopIndex
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)((long)base.SendMessage(398, 0, 0));
				}
				return this.topIndex;
			}
			set
			{
				if (base.IsHandleCreated)
				{
					base.SendMessage(407, value, 0);
					return;
				}
				this.topIndex = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ListBox" /> can recognize and expand tab characters when drawing its strings.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can expand tab characters; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x060028C4 RID: 10436 RVA: 0x000BE09B File Offset: 0x000BC29B
		// (set) Token: 0x060028C5 RID: 10437 RVA: 0x000BE0A3 File Offset: 0x000BC2A3
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListBoxUseTabStopsDescr")]
		public bool UseTabStops
		{
			get
			{
				return this.useTabStops;
			}
			set
			{
				if (this.useTabStops != value)
				{
					this.useTabStops = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets the width of the tabs between the items in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>A collection of integers representing the tab widths.</returns>
		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x060028C6 RID: 10438 RVA: 0x000BE0BB File Offset: 0x000BC2BB
		[SRCategory("CatBehavior")]
		[SRDescription("ListBoxCustomTabOffsetsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Browsable(false)]
		public ListBox.IntegerCollection CustomTabOffsets
		{
			get
			{
				if (this.customTabOffsets == null)
				{
					this.customTabOffsets = new ListBox.IntegerCollection(this);
				}
				return this.customTabOffsets;
			}
		}

		/// <summary>This member is obsolete, and there is no replacement.</summary>
		/// <param name="value">An array of objects.</param>
		// Token: 0x060028C7 RID: 10439 RVA: 0x000BE0D8 File Offset: 0x000BC2D8
		[Obsolete("This method has been deprecated.  There is no replacement.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void AddItemsCore(object[] value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			this.Items.AddRangeInternal(value);
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ListBox" /> control is clicked.</summary>
		// Token: 0x140001DE RID: 478
		// (add) Token: 0x060028C8 RID: 10440 RVA: 0x000A2B72 File Offset: 0x000A0D72
		// (remove) Token: 0x060028C9 RID: 10441 RVA: 0x000A2B7B File Offset: 0x000A0D7B
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		/// <summary>Occurs when the user clicks the <see cref="T:System.Windows.Forms.ListBox" /> control with the mouse pointer.</summary>
		// Token: 0x140001DF RID: 479
		// (add) Token: 0x060028CA RID: 10442 RVA: 0x000A2FE9 File Offset: 0x000A11E9
		// (remove) Token: 0x060028CB RID: 10443 RVA: 0x000A2FF2 File Offset: 0x000A11F2
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value.</returns>
		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x060028CD RID: 10445 RVA: 0x000204A2 File Offset: 0x0001E6A2
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ListBox.Padding" /> property changes.</summary>
		// Token: 0x140001E0 RID: 480
		// (add) Token: 0x060028CE RID: 10446 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x060028CF RID: 10447 RVA: 0x000204B4 File Offset: 0x0001E6B4
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

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ListBox" /> control is painted.</summary>
		// Token: 0x140001E1 RID: 481
		// (add) Token: 0x060028D0 RID: 10448 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x060028D1 RID: 10449 RVA: 0x00020D40 File Offset: 0x0001EF40
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

		/// <summary>Occurs when a visual aspect of an owner-drawn <see cref="T:System.Windows.Forms.ListBox" /> changes.</summary>
		// Token: 0x140001E2 RID: 482
		// (add) Token: 0x060028D2 RID: 10450 RVA: 0x000BE0FF File Offset: 0x000BC2FF
		// (remove) Token: 0x060028D3 RID: 10451 RVA: 0x000BE112 File Offset: 0x000BC312
		[SRCategory("CatBehavior")]
		[SRDescription("drawItemEventDescr")]
		public event DrawItemEventHandler DrawItem
		{
			add
			{
				base.Events.AddHandler(ListBox.EVENT_DRAWITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListBox.EVENT_DRAWITEM, value);
			}
		}

		/// <summary>Occurs when an owner-drawn <see cref="T:System.Windows.Forms.ListBox" /> is created and the sizes of the list items are determined.</summary>
		// Token: 0x140001E3 RID: 483
		// (add) Token: 0x060028D4 RID: 10452 RVA: 0x000BE125 File Offset: 0x000BC325
		// (remove) Token: 0x060028D5 RID: 10453 RVA: 0x000BE138 File Offset: 0x000BC338
		[SRCategory("CatBehavior")]
		[SRDescription("measureItemEventDescr")]
		public event MeasureItemEventHandler MeasureItem
		{
			add
			{
				base.Events.AddHandler(ListBox.EVENT_MEASUREITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListBox.EVENT_MEASUREITEM, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListBox.SelectedIndex" /> property or the <see cref="P:System.Windows.Forms.ListBox.SelectedIndices" /> collection has changed. </summary>
		// Token: 0x140001E4 RID: 484
		// (add) Token: 0x060028D6 RID: 10454 RVA: 0x000BE14B File Offset: 0x000BC34B
		// (remove) Token: 0x060028D7 RID: 10455 RVA: 0x000BE15E File Offset: 0x000BC35E
		[SRCategory("CatBehavior")]
		[SRDescription("selectedIndexChangedEventDescr")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ListBox.EVENT_SELECTEDINDEXCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListBox.EVENT_SELECTEDINDEXCHANGED, value);
			}
		}

		/// <summary>Maintains performance while items are added to the <see cref="T:System.Windows.Forms.ListBox" /> one at a time by preventing the control from drawing until the <see cref="M:System.Windows.Forms.ListBox.EndUpdate" /> method is called.</summary>
		// Token: 0x060028D8 RID: 10456 RVA: 0x000BE171 File Offset: 0x000BC371
		public void BeginUpdate()
		{
			base.BeginUpdateInternal();
			this.updateCount++;
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000BE187 File Offset: 0x000BC387
		private void CheckIndex(int index)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[]
				{
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000BE1C5 File Offset: 0x000BC3C5
		private void CheckNoDataSource()
		{
			if (base.DataSource != null)
			{
				throw new ArgumentException(SR.GetString("DataSourceLocksItems"));
			}
		}

		/// <summary>Creates a new instance of the item collection.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> that represents the new item collection.</returns>
		// Token: 0x060028DB RID: 10459 RVA: 0x000BE1DF File Offset: 0x000BC3DF
		protected virtual ListBox.ObjectCollection CreateItemCollection()
		{
			return new ListBox.ObjectCollection(this);
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000BE1E8 File Offset: 0x000BC3E8
		internal virtual int ComputeMaxItemWidth(int oldMax)
		{
			string[] array = new string[this.Items.Count];
			for (int i = 0; i < this.Items.Count; i++)
			{
				array[i] = base.GetItemText(this.Items[i]);
			}
			return Math.Max(oldMax, LayoutUtils.OldGetLargestStringSizeInCollection(this.Font, array).Width);
		}

		/// <summary>Unselects all items in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		// Token: 0x060028DD RID: 10461 RVA: 0x000BE24C File Offset: 0x000BC44C
		public void ClearSelected()
		{
			bool flag = false;
			int num = (this.itemsCollection == null) ? 0 : this.itemsCollection.Count;
			for (int i = 0; i < num; i++)
			{
				if (this.SelectedItems.GetSelected(i))
				{
					flag = true;
					this.SelectedItems.SetSelected(i, false);
					if (base.IsHandleCreated)
					{
						this.NativeSetSelected(i, false);
					}
				}
			}
			if (flag)
			{
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
		}

		/// <summary>Resumes painting the <see cref="T:System.Windows.Forms.ListBox" /> control after painting is suspended by the <see cref="M:System.Windows.Forms.ListBox.BeginUpdate" /> method.</summary>
		// Token: 0x060028DE RID: 10462 RVA: 0x000BE2B9 File Offset: 0x000BC4B9
		public void EndUpdate()
		{
			base.EndUpdateInternal();
			this.updateCount--;
		}

		/// <summary>Finds the first item in the <see cref="T:System.Windows.Forms.ListBox" /> that starts with the specified string.</summary>
		/// <param name="s">The text to search for. </param>
		/// <returns>The zero-based index of the first item found; returns <see langword="ListBox.NoMatches" /> if no match is found.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="s" /> parameter is less than -1 or greater than or equal to the item count.</exception>
		// Token: 0x060028DF RID: 10463 RVA: 0x000BE2D0 File Offset: 0x000BC4D0
		public int FindString(string s)
		{
			return this.FindString(s, -1);
		}

		/// <summary>Finds the first item in the <see cref="T:System.Windows.Forms.ListBox" /> that starts with the specified string. The search starts at a specific starting index.</summary>
		/// <param name="s">The text to search for. </param>
		/// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to negative one (-1) to search from the beginning of the control. </param>
		/// <returns>The zero-based index of the first item found; returns <see langword="ListBox.NoMatches" /> if no match is found.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="startIndex" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListBox.ObjectCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> class. </exception>
		// Token: 0x060028E0 RID: 10464 RVA: 0x000BE2DC File Offset: 0x000BC4DC
		public int FindString(string s, int startIndex)
		{
			if (s == null)
			{
				return -1;
			}
			int num = (this.itemsCollection == null) ? 0 : this.itemsCollection.Count;
			if (num == 0)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= num)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return base.FindStringInternal(s, this.Items, startIndex, false);
		}

		/// <summary>Finds the first item in the <see cref="T:System.Windows.Forms.ListBox" /> that exactly matches the specified string.</summary>
		/// <param name="s">The text to search for. </param>
		/// <returns>The zero-based index of the first item found; returns <see langword="ListBox.NoMatches" /> if no match is found.</returns>
		// Token: 0x060028E1 RID: 10465 RVA: 0x000BE32C File Offset: 0x000BC52C
		public int FindStringExact(string s)
		{
			return this.FindStringExact(s, -1);
		}

		/// <summary>Finds the first item in the <see cref="T:System.Windows.Forms.ListBox" /> that exactly matches the specified string. The search starts at a specific starting index.</summary>
		/// <param name="s">The text to search for. </param>
		/// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to negative one (-1) to search from the beginning of the control. </param>
		/// <returns>The zero-based index of the first item found; returns <see langword="ListBox.NoMatches" /> if no match is found.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="startIndex" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListBox.ObjectCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> class. </exception>
		// Token: 0x060028E2 RID: 10466 RVA: 0x000BE338 File Offset: 0x000BC538
		public int FindStringExact(string s, int startIndex)
		{
			if (s == null)
			{
				return -1;
			}
			int num = (this.itemsCollection == null) ? 0 : this.itemsCollection.Count;
			if (num == 0)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= num)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return base.FindStringInternal(s, this.Items, startIndex, true);
		}

		/// <summary>Returns the height of an item in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <param name="index">The zero-based index of the item to return the height for. </param>
		/// <returns>The height, in pixels, of the specified item.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value of the <paramref name="index" /> parameter is less than zero or greater than the item count. </exception>
		// Token: 0x060028E3 RID: 10467 RVA: 0x000BE388 File Offset: 0x000BC588
		public int GetItemHeight(int index)
		{
			int num = (this.itemsCollection == null) ? 0 : this.itemsCollection.Count;
			if (index < 0 || (index > 0 && index >= num))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.drawMode != DrawMode.OwnerDrawVariable)
			{
				index = 0;
			}
			if (!base.IsHandleCreated)
			{
				return this.itemHeight;
			}
			int num2 = (int)((long)base.SendMessage(417, index, 0));
			if (num2 == -1)
			{
				throw new Win32Exception();
			}
			return num2;
		}

		/// <summary>Returns the bounding rectangle for an item in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <param name="index">The zero-based index of item whose bounding rectangle you want to return. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding rectangle for the specified item.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListBox.ObjectCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> class. </exception>
		// Token: 0x060028E4 RID: 10468 RVA: 0x000BE424 File Offset: 0x000BC624
		public Rectangle GetItemRectangle(int index)
		{
			this.CheckIndex(index);
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			base.SendMessage(408, index, ref rect);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		/// <summary>Retrieves the bounds within which the <see cref="T:System.Windows.Forms.ListBox" /> is scaled.</summary>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that specifies the area for which to retrieve the display bounds.</param>
		/// <param name="factor">The height and width of the control's bounds.</param>
		/// <param name="specified">One of the values of <see cref="T:System.Windows.Forms.BoundsSpecified" /> that specifies the bounds of the control to use when defining its size and position.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> representing the bounds within which the control is scaled.</returns>
		// Token: 0x060028E5 RID: 10469 RVA: 0x000BE46C File Offset: 0x000BC66C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			bounds.Height = this.requestedHeight;
			return base.GetScaledBounds(bounds, factor, specified);
		}

		/// <summary>Returns a value indicating whether the specified item is selected.</summary>
		/// <param name="index">The zero-based index of the item that determines whether it is selected. </param>
		/// <returns>
		///     <see langword="true" /> if the specified item is currently selected in the <see cref="T:System.Windows.Forms.ListBox" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListBox.ObjectCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> class. </exception>
		// Token: 0x060028E6 RID: 10470 RVA: 0x000BE484 File Offset: 0x000BC684
		public bool GetSelected(int index)
		{
			this.CheckIndex(index);
			return this.GetSelectedInternal(index);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000BE494 File Offset: 0x000BC694
		private bool GetSelectedInternal(int index)
		{
			if (!base.IsHandleCreated)
			{
				return this.itemsCollection != null && this.SelectedItems.GetSelected(index);
			}
			int num = (int)((long)base.SendMessage(391, index, 0));
			if (num == -1)
			{
				throw new Win32Exception();
			}
			return num > 0;
		}

		/// <summary>Returns the zero-based index of the item at the specified coordinates.</summary>
		/// <param name="p">A <see cref="T:System.Drawing.Point" /> object containing the coordinates used to obtain the item index. </param>
		/// <returns>The zero-based index of the item found at the specified coordinates; returns <see langword="ListBox.NoMatches" /> if no match is found.</returns>
		// Token: 0x060028E8 RID: 10472 RVA: 0x000BE4E5 File Offset: 0x000BC6E5
		public int IndexFromPoint(Point p)
		{
			return this.IndexFromPoint(p.X, p.Y);
		}

		/// <summary>Returns the zero-based index of the item at the specified coordinates.</summary>
		/// <param name="x">The x-coordinate of the location to search. </param>
		/// <param name="y">The y-coordinate of the location to search. </param>
		/// <returns>The zero-based index of the item found at the specified coordinates; returns <see langword="ListBox.NoMatches" /> if no match is found.</returns>
		// Token: 0x060028E9 RID: 10473 RVA: 0x000BE4FC File Offset: 0x000BC6FC
		public int IndexFromPoint(int x, int y)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref rect);
			if (rect.left <= x && x < rect.right && rect.top <= y && y < rect.bottom)
			{
				int n = (int)((long)base.SendMessage(425, 0, (int)((long)NativeMethods.Util.MAKELPARAM(x, y))));
				if (NativeMethods.Util.HIWORD(n) == 0)
				{
					return NativeMethods.Util.LOWORD(n);
				}
			}
			return -1;
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x000BE57C File Offset: 0x000BC77C
		private int NativeAdd(object item)
		{
			int num = (int)((long)base.SendMessage(384, 0, base.GetItemText(item)));
			if (num == -2)
			{
				throw new OutOfMemoryException();
			}
			if (num == -1)
			{
				throw new OutOfMemoryException(SR.GetString("ListBoxItemOverflow"));
			}
			return num;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x000BE5C3 File Offset: 0x000BC7C3
		private void NativeClear()
		{
			base.SendMessage(388, 0, 0);
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x000BE5D4 File Offset: 0x000BC7D4
		internal string NativeGetItemText(int index)
		{
			int num = (int)((long)base.SendMessage(394, index, 0));
			StringBuilder stringBuilder = new StringBuilder(num + 1);
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 393, index, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000BE620 File Offset: 0x000BC820
		private int NativeInsert(int index, object item)
		{
			int num = (int)((long)base.SendMessage(385, index, base.GetItemText(item)));
			if (num == -2)
			{
				throw new OutOfMemoryException();
			}
			if (num == -1)
			{
				throw new OutOfMemoryException(SR.GetString("ListBoxItemOverflow"));
			}
			return num;
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x000BE668 File Offset: 0x000BC868
		private void NativeRemoveAt(int index)
		{
			bool flag = (int)((long)base.SendMessage(391, (IntPtr)index, IntPtr.Zero)) > 0;
			base.SendMessage(386, index, 0);
			if (flag)
			{
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000BE6B1 File Offset: 0x000BC8B1
		private void NativeSetSelected(int index, bool value)
		{
			if (this.selectionMode == SelectionMode.One)
			{
				base.SendMessage(390, value ? index : -1, 0);
				return;
			}
			base.SendMessage(389, value ? -1 : 0, index);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000BE6E8 File Offset: 0x000BC8E8
		private void NativeUpdateSelection()
		{
			int count = this.Items.Count;
			for (int i = 0; i < count; i++)
			{
				this.SelectedItems.SetSelected(i, false);
			}
			int[] array = null;
			SelectionMode selectionMode = this.selectionMode;
			if (selectionMode != SelectionMode.One)
			{
				if (selectionMode - SelectionMode.MultiSimple <= 1)
				{
					int num = (int)((long)base.SendMessage(400, 0, 0));
					if (num > 0)
					{
						array = new int[num];
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 401, num, array);
					}
				}
			}
			else
			{
				int num2 = (int)((long)base.SendMessage(392, 0, 0));
				if (num2 >= 0)
				{
					array = new int[]
					{
						num2
					};
				}
			}
			if (array != null)
			{
				foreach (int index in array)
				{
					this.SelectedItems.SetSelected(index, true);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ChangeUICues" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.UICuesEventArgs" /> that contains the event data.</param>
		// Token: 0x060028F1 RID: 10481 RVA: 0x000BE7C1 File Offset: 0x000BC9C1
		protected override void OnChangeUICues(UICuesEventArgs e)
		{
			base.Invalidate();
			base.OnChangeUICues(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListBox.DrawItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> that contains the event data. </param>
		// Token: 0x060028F2 RID: 10482 RVA: 0x000BE7D0 File Offset: 0x000BC9D0
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{
			DrawItemEventHandler drawItemEventHandler = (DrawItemEventHandler)base.Events[ListBox.EVENT_DRAWITEM];
			if (drawItemEventHandler != null)
			{
				drawItemEventHandler(this, e);
			}
		}

		/// <summary>Specifies when the window handle has been created so that column width and other characteristics can be set. Inheriting classes should call <see langword="base.OnHandleCreated" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028F3 RID: 10483 RVA: 0x000BE800 File Offset: 0x000BCA00
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(421, CultureInfo.CurrentCulture.LCID, 0);
			if (this.columnWidth != 0)
			{
				base.SendMessage(405, this.columnWidth, 0);
			}
			if (this.drawMode == DrawMode.OwnerDrawFixed)
			{
				base.SendMessage(416, 0, this.ItemHeight);
			}
			if (this.topIndex != 0)
			{
				base.SendMessage(407, this.topIndex, 0);
			}
			if (this.UseCustomTabOffsets && this.CustomTabOffsets != null)
			{
				int count = this.CustomTabOffsets.Count;
				int[] array = new int[count];
				this.CustomTabOffsets.CopyTo(array, 0);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 402, count, array);
			}
			if (this.itemsCollection != null)
			{
				int count2 = this.itemsCollection.Count;
				for (int i = 0; i < count2; i++)
				{
					this.NativeAdd(this.itemsCollection[i]);
					if (this.selectionMode != SelectionMode.None && this.selectedItems != null)
					{
						this.selectedItems.PushSelectionIntoNativeListBox(i);
					}
				}
			}
			if (this.selectedItems != null && this.selectedItems.Count > 0 && this.selectionMode == SelectionMode.One)
			{
				this.SelectedItems.Dirty();
				this.SelectedItems.EnsureUpToDate();
			}
			this.UpdateHorizontalExtent();
		}

		/// <summary>Overridden to be sure that items are set up and cleared out correctly. Inheriting controls should call <see langword="base.OnHandleDestroyed" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028F4 RID: 10484 RVA: 0x000BE950 File Offset: 0x000BCB50
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.SelectedItems.EnsureUpToDate();
			if (base.Disposing)
			{
				this.itemsCollection = null;
			}
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListBox.MeasureItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MeasureItemEventArgs" /> that contains the event data. </param>
		// Token: 0x060028F5 RID: 10485 RVA: 0x000BE974 File Offset: 0x000BCB74
		protected virtual void OnMeasureItem(MeasureItemEventArgs e)
		{
			MeasureItemEventHandler measureItemEventHandler = (MeasureItemEventHandler)base.Events[ListBox.EVENT_MEASUREITEM];
			if (measureItemEventHandler != null)
			{
				measureItemEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028F6 RID: 10486 RVA: 0x000BE9A2 File Offset: 0x000BCBA2
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFontCache();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028F7 RID: 10487 RVA: 0x000BE9B1 File Offset: 0x000BCBB1
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (this.ParentInternal != null)
			{
				base.RecreateHandle();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028F8 RID: 10488 RVA: 0x000BE9C8 File Offset: 0x000BCBC8
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.RightToLeft == RightToLeft.Yes || this.HorizontalScrollbar)
			{
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.SelectedValueChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028F9 RID: 10489 RVA: 0x000BE9E8 File Offset: 0x000BCBE8
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			if (base.DataManager != null && base.DataManager.Position != this.SelectedIndex && (!base.FormattingEnabled || this.SelectedIndex != -1))
			{
				base.DataManager.Position = this.SelectedIndex;
			}
			EventHandler eventHandler = (EventHandler)base.Events[ListBox.EVENT_SELECTEDINDEXCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.SelectedValueChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028FA RID: 10490 RVA: 0x000BEA5A File Offset: 0x000BCC5A
		protected override void OnSelectedValueChanged(EventArgs e)
		{
			base.OnSelectedValueChanged(e);
			this.selectedValueChangedFired = true;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.DataSourceChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028FB RID: 10491 RVA: 0x000BEA6A File Offset: 0x000BCC6A
		protected override void OnDataSourceChanged(EventArgs e)
		{
			if (base.DataSource == null)
			{
				this.BeginUpdate();
				this.SelectedIndex = -1;
				this.Items.ClearInternal();
				this.EndUpdate();
			}
			base.OnDataSourceChanged(e);
			this.RefreshItems();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.DisplayMemberChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060028FC RID: 10492 RVA: 0x000BEA9F File Offset: 0x000BCC9F
		protected override void OnDisplayMemberChanged(EventArgs e)
		{
			base.OnDisplayMemberChanged(e);
			this.RefreshItems();
			if (this.SelectionMode != SelectionMode.None && base.DataManager != null)
			{
				this.SelectedIndex = base.DataManager.Position;
			}
		}

		/// <summary>Forces the control to invalidate its client area and immediately redraw itself and any child controls.</summary>
		// Token: 0x060028FD RID: 10493 RVA: 0x000BEAD0 File Offset: 0x000BCCD0
		public override void Refresh()
		{
			if (this.drawMode == DrawMode.OwnerDrawVariable)
			{
				int count = this.Items.Count;
				Graphics graphics = base.CreateGraphicsInternal();
				try
				{
					for (int i = 0; i < count; i++)
					{
						MeasureItemEventArgs e = new MeasureItemEventArgs(graphics, i, this.ItemHeight);
						this.OnMeasureItem(e);
					}
				}
				finally
				{
					graphics.Dispose();
				}
			}
			base.Refresh();
		}

		/// <summary>Refreshes all <see cref="T:System.Windows.Forms.ListBox" /> items and retrieves new strings for them.</summary>
		// Token: 0x060028FE RID: 10494 RVA: 0x000BEB3C File Offset: 0x000BCD3C
		protected override void RefreshItems()
		{
			ListBox.ObjectCollection objectCollection = this.itemsCollection;
			this.itemsCollection = null;
			this.selectedIndices = null;
			if (base.IsHandleCreated)
			{
				this.NativeClear();
			}
			object[] array = null;
			if (base.DataManager != null && base.DataManager.Count != -1)
			{
				array = new object[base.DataManager.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = base.DataManager[i];
				}
			}
			else if (objectCollection != null)
			{
				array = new object[objectCollection.Count];
				objectCollection.CopyTo(array, 0);
			}
			if (array != null)
			{
				this.Items.AddRangeInternal(array);
			}
			if (this.SelectionMode != SelectionMode.None)
			{
				if (base.DataManager != null)
				{
					this.SelectedIndex = base.DataManager.Position;
					return;
				}
				if (objectCollection != null)
				{
					int count = objectCollection.Count;
					for (int j = 0; j < count; j++)
					{
						if (objectCollection.InnerArray.GetState(j, ListBox.SelectedObjectCollection.SelectedObjectMask))
						{
							this.SelectedItem = objectCollection[j];
						}
					}
				}
			}
		}

		/// <summary>Refreshes the item contained at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to refresh.</param>
		// Token: 0x060028FF RID: 10495 RVA: 0x000BEC38 File Offset: 0x000BCE38
		protected override void RefreshItem(int index)
		{
			this.Items.SetItemInternal(index, this.Items[index]);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.BackColor" /> property to its default value.</summary>
		// Token: 0x06002900 RID: 10496 RVA: 0x000BEC52 File Offset: 0x000BCE52
		public override void ResetBackColor()
		{
			base.ResetBackColor();
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.ForeColor" /> property to its default value.</summary>
		// Token: 0x06002901 RID: 10497 RVA: 0x000BEC5A File Offset: 0x000BCE5A
		public override void ResetForeColor()
		{
			base.ResetForeColor();
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000BEC62 File Offset: 0x000BCE62
		private void ResetItemHeight()
		{
			this.itemHeight = 13;
		}

		/// <summary>Scales a control's location, size, padding and margin.</summary>
		/// <param name="factor">The factor by which the height and width of the control will be scaled.</param>
		/// <param name="specified">A <see cref="T:System.Windows.Forms.BoundsSpecified" /> value that specifies the bounds of the control to use when defining its size and position.</param>
		// Token: 0x06002903 RID: 10499 RVA: 0x000BEC6C File Offset: 0x000BCE6C
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			if (factor.Width != 1f && factor.Height != 1f)
			{
				this.UpdateFontCache();
			}
			base.ScaleControl(factor, specified);
		}

		/// <summary>Sets the specified bounds of the <see cref="T:System.Windows.Forms.ListBox" /> control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x06002904 RID: 10500 RVA: 0x000BEC98 File Offset: 0x000BCE98
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (!this.integralHeightAdjust && height != base.Height)
			{
				this.requestedHeight = height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// <summary>Clears the contents of the <see cref="T:System.Windows.Forms.ListBox" /> and adds the specified items to the control.</summary>
		/// <param name="value">An array of objects to insert into the control. </param>
		// Token: 0x06002905 RID: 10501 RVA: 0x000BECC4 File Offset: 0x000BCEC4
		protected override void SetItemsCore(IList value)
		{
			this.BeginUpdate();
			this.Items.ClearInternal();
			this.Items.AddRangeInternal(value);
			this.SelectedItems.Dirty();
			if (base.DataManager != null)
			{
				if (base.DataSource is ICurrencyManagerProvider)
				{
					this.selectedValueChangedFired = false;
				}
				if (base.IsHandleCreated)
				{
					base.SendMessage(390, base.DataManager.Position, 0);
				}
				if (!this.selectedValueChangedFired)
				{
					this.OnSelectedValueChanged(EventArgs.Empty);
					this.selectedValueChangedFired = false;
				}
			}
			this.EndUpdate();
		}

		/// <summary>Sets the object with the specified index in the derived class.</summary>
		/// <param name="index">The array index of the object.</param>
		/// <param name="value">The object.</param>
		// Token: 0x06002906 RID: 10502 RVA: 0x000BED55 File Offset: 0x000BCF55
		protected override void SetItemCore(int index, object value)
		{
			this.Items.SetItemInternal(index, value);
		}

		/// <summary>Selects or clears the selection for the specified item in a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <param name="index">The zero-based index of the item in a <see cref="T:System.Windows.Forms.ListBox" /> to select or clear the selection for. </param>
		/// <param name="value">
		///       <see langword="true" /> to select the specified item; otherwise, <see langword="false" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified index was outside the range of valid values. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.ListBox.SelectionMode" /> property was set to <see langword="None" />.</exception>
		// Token: 0x06002907 RID: 10503 RVA: 0x000BED64 File Offset: 0x000BCF64
		public void SetSelected(int index, bool value)
		{
			int num = (this.itemsCollection == null) ? 0 : this.itemsCollection.Count;
			if (index < 0 || index >= num)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.selectionMode == SelectionMode.None)
			{
				throw new InvalidOperationException(SR.GetString("ListBoxInvalidSelectionMode"));
			}
			this.SelectedItems.SetSelected(index, value);
			if (base.IsHandleCreated)
			{
				this.NativeSetSelected(index, value);
			}
			this.SelectedItems.Dirty();
			this.OnSelectedIndexChanged(EventArgs.Empty);
		}

		/// <summary>Sorts the items in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		// Token: 0x06002908 RID: 10504 RVA: 0x000BEE10 File Offset: 0x000BD010
		protected virtual void Sort()
		{
			this.CheckNoDataSource();
			ListBox.SelectedObjectCollection selectedObjectCollection = this.SelectedItems;
			selectedObjectCollection.EnsureUpToDate();
			if (this.sorted && this.itemsCollection != null)
			{
				this.itemsCollection.InnerArray.Sort();
				if (base.IsHandleCreated)
				{
					this.NativeClear();
					int count = this.itemsCollection.Count;
					for (int i = 0; i < count; i++)
					{
						this.NativeAdd(this.itemsCollection[i]);
						if (selectedObjectCollection.GetSelected(i))
						{
							this.NativeSetSelected(i, true);
						}
					}
				}
			}
		}

		/// <summary>Returns a string representation of the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		/// <returns>A string that states the control type, the count of items in the <see cref="T:System.Windows.Forms.ListBox" /> control, and the Text property of the first item in the <see cref="T:System.Windows.Forms.ListBox" />, if the count is not 0.</returns>
		// Token: 0x06002909 RID: 10505 RVA: 0x000BEE9C File Offset: 0x000BD09C
		public override string ToString()
		{
			string text = base.ToString();
			if (this.itemsCollection != null)
			{
				text = text + ", Items.Count: " + this.Items.Count.ToString(CultureInfo.CurrentCulture);
				if (this.Items.Count > 0)
				{
					string itemText = base.GetItemText(this.Items[0]);
					string str = (itemText.Length > 40) ? itemText.Substring(0, 40) : itemText;
					text = text + ", Items[0]: " + str;
				}
			}
			return text;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000BEF24 File Offset: 0x000BD124
		private void UpdateFontCache()
		{
			this.fontIsChanged = true;
			this.integralHeightAdjust = true;
			try
			{
				base.Height = this.requestedHeight;
			}
			finally
			{
				this.integralHeightAdjust = false;
			}
			this.maxWidth = -1;
			this.UpdateHorizontalExtent();
			CommonProperties.xClearPreferredSizeCache(this);
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000BEF78 File Offset: 0x000BD178
		private void UpdateHorizontalExtent()
		{
			if (!this.multiColumn && this.horizontalScrollbar && base.IsHandleCreated)
			{
				int maxItemWidth = this.horizontalExtent;
				if (maxItemWidth == 0)
				{
					maxItemWidth = this.MaxItemWidth;
				}
				base.SendMessage(404, maxItemWidth, 0);
			}
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000BEFBC File Offset: 0x000BD1BC
		private void UpdateMaxItemWidth(object item, bool removing)
		{
			if (!this.horizontalScrollbar || this.horizontalExtent > 0)
			{
				this.maxWidth = -1;
				return;
			}
			if (this.maxWidth > -1)
			{
				int num;
				using (Graphics graphics = base.CreateGraphicsInternal())
				{
					num = (int)Math.Ceiling((double)graphics.MeasureString(base.GetItemText(item), this.Font).Width);
				}
				if (removing)
				{
					if (num >= this.maxWidth)
					{
						this.maxWidth = -1;
						return;
					}
				}
				else if (num > this.maxWidth)
				{
					this.maxWidth = num;
				}
			}
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000BF058 File Offset: 0x000BD258
		private void UpdateCustomTabOffsets()
		{
			if (base.IsHandleCreated && this.UseCustomTabOffsets && this.CustomTabOffsets != null)
			{
				int count = this.CustomTabOffsets.Count;
				int[] array = new int[count];
				this.CustomTabOffsets.CopyTo(array, 0);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 402, count, array);
				base.Invalidate();
			}
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000BF0BC File Offset: 0x000BD2BC
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)m.LParam) != 0 && Application.RenderWithVisualStyles && this.BorderStyle == BorderStyle.Fixed3D)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					using (Graphics graphics = Graphics.FromHdc(m.WParam))
					{
						Rectangle rect = new Rectangle(0, 0, base.Size.Width - 1, base.Size.Height - 1);
						using (Pen pen = new Pen(VisualStyleInformation.TextControlBorder))
						{
							graphics.DrawRectangle(pen, rect);
						}
						rect.Inflate(-1, -1);
						graphics.DrawRectangle(SystemPens.Window, rect);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		/// <summary>Processes the command message the <see cref="T:System.Windows.Forms.ListView" /> control receives from the top-level window.</summary>
		/// <param name="m">The <see cref="T:System.Windows.Forms.Message" /> the top-level window sent to the <see cref="T:System.Windows.Forms.ListBox" /> control.</param>
		// Token: 0x0600290F RID: 10511 RVA: 0x000BF1A8 File Offset: 0x000BD3A8
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void WmReflectCommand(ref Message m)
		{
			int num = NativeMethods.Util.HIWORD(m.WParam);
			if (num != 1)
			{
				return;
			}
			if (this.selectedItems != null)
			{
				this.selectedItems.Dirty();
			}
			this.OnSelectedIndexChanged(EventArgs.Empty);
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x000BF1E8 File Offset: 0x000BD3E8
		private void WmReflectDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			IntPtr hDC = drawitemstruct.hDC;
			IntPtr intPtr = Control.SetUpPalette(hDC, false, false);
			try
			{
				Graphics graphics = Graphics.FromHdcInternal(hDC);
				try
				{
					Rectangle rect = Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom);
					if (this.HorizontalScrollbar)
					{
						if (this.MultiColumn)
						{
							rect.Width = Math.Max(this.ColumnWidth, rect.Width);
						}
						else
						{
							rect.Width = Math.Max(this.MaxItemWidth, rect.Width);
						}
					}
					this.OnDrawItem(new DrawItemEventArgs(graphics, this.Font, rect, drawitemstruct.itemID, (DrawItemState)drawitemstruct.itemState, this.ForeColor, this.BackColor));
				}
				finally
				{
					graphics.Dispose();
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.SelectPalette(new HandleRef(null, hDC), new HandleRef(null, intPtr), 0);
				}
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000BF31C File Offset: 0x000BD51C
		private void WmReflectMeasureItem(ref Message m)
		{
			NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
			if (this.drawMode == DrawMode.OwnerDrawVariable && measureitemstruct.itemID >= 0)
			{
				Graphics graphics = base.CreateGraphicsInternal();
				MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, measureitemstruct.itemID, this.ItemHeight);
				try
				{
					this.OnMeasureItem(measureItemEventArgs);
					measureitemstruct.itemHeight = measureItemEventArgs.ItemHeight;
					goto IL_6A;
				}
				finally
				{
					graphics.Dispose();
				}
			}
			measureitemstruct.itemHeight = this.ItemHeight;
			IL_6A:
			Marshal.StructureToPtr(measureitemstruct, m.LParam, false);
			m.Result = (IntPtr)1;
		}

		/// <summary>The list's window procedure. </summary>
		/// <param name="m">A Windows Message Object. </param>
		// Token: 0x06002912 RID: 10514 RVA: 0x000BF3BC File Offset: 0x000BD5BC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 791)
			{
				if (msg != 71)
				{
					switch (msg)
					{
					case 513:
						if (this.selectedItems != null)
						{
							this.selectedItems.Dirty();
						}
						base.WndProc(ref m);
						return;
					case 514:
					{
						int x = NativeMethods.Util.SignedLOWORD(m.LParam);
						int y = NativeMethods.Util.SignedHIWORD(m.LParam);
						Point p = new Point(x, y);
						p = base.PointToScreen(p);
						bool capture = base.Capture;
						if (capture && UnsafeNativeMethods.WindowFromPoint(p.X, p.Y) == base.Handle)
						{
							if (!this.doubleClickFired && !base.ValidationCancelled)
							{
								this.OnClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							}
							else
							{
								this.doubleClickFired = false;
								if (!base.ValidationCancelled)
								{
									this.OnDoubleClick(new MouseEventArgs(MouseButtons.Left, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
									this.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								}
							}
						}
						if (base.GetState(2048))
						{
							base.DefWndProc(ref m);
						}
						else
						{
							base.WndProc(ref m);
						}
						this.doubleClickFired = false;
						return;
					}
					case 515:
						this.doubleClickFired = true;
						base.WndProc(ref m);
						return;
					case 516:
						break;
					case 517:
					{
						int x2 = NativeMethods.Util.SignedLOWORD(m.LParam);
						int y2 = NativeMethods.Util.SignedHIWORD(m.LParam);
						Point p2 = new Point(x2, y2);
						p2 = base.PointToScreen(p2);
						bool capture2 = base.Capture;
						if (capture2 && UnsafeNativeMethods.WindowFromPoint(p2.X, p2.Y) == base.Handle && this.selectedItems != null)
						{
							this.selectedItems.Dirty();
						}
						base.WndProc(ref m);
						return;
					}
					default:
						if (msg == 791)
						{
							this.WmPrint(ref m);
							return;
						}
						break;
					}
				}
				else
				{
					base.WndProc(ref m);
					if (this.integralHeight && this.fontIsChanged)
					{
						base.Height = Math.Max(base.Height, this.ItemHeight);
						this.fontIsChanged = false;
						return;
					}
					return;
				}
			}
			else
			{
				if (msg == 8235)
				{
					this.WmReflectDrawItem(ref m);
					return;
				}
				if (msg == 8236)
				{
					this.WmReflectMeasureItem(ref m);
					return;
				}
				if (msg == 8465)
				{
					this.WmReflectCommand(ref m);
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x000BF666 File Offset: 0x000BD866
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ListBox.ListBoxAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		/// <summary>Specifies that no matches are found during a search.</summary>
		// Token: 0x040011A0 RID: 4512
		public const int NoMatches = -1;

		/// <summary>Specifies the default item height for an owner-drawn <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		// Token: 0x040011A1 RID: 4513
		public const int DefaultItemHeight = 13;

		// Token: 0x040011A2 RID: 4514
		private const int maxWin9xHeight = 32767;

		// Token: 0x040011A3 RID: 4515
		private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

		// Token: 0x040011A4 RID: 4516
		private static readonly object EVENT_DRAWITEM = new object();

		// Token: 0x040011A5 RID: 4517
		private static readonly object EVENT_MEASUREITEM = new object();

		// Token: 0x040011A6 RID: 4518
		private static bool checkedOS = false;

		// Token: 0x040011A7 RID: 4519
		private static bool runningOnWin2K = true;

		// Token: 0x040011A8 RID: 4520
		private ListBox.SelectedObjectCollection selectedItems;

		// Token: 0x040011A9 RID: 4521
		private ListBox.SelectedIndexCollection selectedIndices;

		// Token: 0x040011AA RID: 4522
		private ListBox.ObjectCollection itemsCollection;

		// Token: 0x040011AB RID: 4523
		private int itemHeight = 13;

		// Token: 0x040011AC RID: 4524
		private int columnWidth;

		// Token: 0x040011AD RID: 4525
		private int requestedHeight;

		// Token: 0x040011AE RID: 4526
		private int topIndex;

		// Token: 0x040011AF RID: 4527
		private int horizontalExtent;

		// Token: 0x040011B0 RID: 4528
		private int maxWidth = -1;

		// Token: 0x040011B1 RID: 4529
		private int updateCount;

		// Token: 0x040011B2 RID: 4530
		private bool sorted;

		// Token: 0x040011B3 RID: 4531
		private bool scrollAlwaysVisible;

		// Token: 0x040011B4 RID: 4532
		private bool integralHeight = true;

		// Token: 0x040011B5 RID: 4533
		private bool integralHeightAdjust;

		// Token: 0x040011B6 RID: 4534
		private bool multiColumn;

		// Token: 0x040011B7 RID: 4535
		private bool horizontalScrollbar;

		// Token: 0x040011B8 RID: 4536
		private bool useTabStops = true;

		// Token: 0x040011B9 RID: 4537
		private bool useCustomTabOffsets;

		// Token: 0x040011BA RID: 4538
		private bool fontIsChanged;

		// Token: 0x040011BB RID: 4539
		private bool doubleClickFired;

		// Token: 0x040011BC RID: 4540
		private bool selectedValueChangedFired;

		// Token: 0x040011BD RID: 4541
		private DrawMode drawMode;

		// Token: 0x040011BE RID: 4542
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x040011BF RID: 4543
		private SelectionMode selectionMode = SelectionMode.One;

		// Token: 0x040011C0 RID: 4544
		private SelectionMode cachedSelectionMode = SelectionMode.One;

		// Token: 0x040011C1 RID: 4545
		private bool selectionModeChanging;

		// Token: 0x040011C2 RID: 4546
		private ListBox.IntegerCollection customTabOffsets;

		// Token: 0x040011C3 RID: 4547
		private const int defaultListItemStartPos = 1;

		// Token: 0x040011C4 RID: 4548
		private const int defaultListItemBorderHeight = 1;

		// Token: 0x040011C5 RID: 4549
		private const int defaultListItemPaddingBuffer = 3;

		// Token: 0x040011C6 RID: 4550
		internal int scaledListItemStartPosition = 1;

		// Token: 0x040011C7 RID: 4551
		internal int scaledListItemBordersHeight = 2;

		// Token: 0x040011C8 RID: 4552
		internal int scaledListItemPaddingBuffer = 3;

		// Token: 0x02000604 RID: 1540
		internal class ItemArray : IComparer
		{
			// Token: 0x06005C3C RID: 23612 RVA: 0x00180C8D File Offset: 0x0017EE8D
			public ItemArray(ListControl listControl)
			{
				this.listControl = listControl;
			}

			// Token: 0x1700161C RID: 5660
			// (get) Token: 0x06005C3D RID: 23613 RVA: 0x00180C9C File Offset: 0x0017EE9C
			public int Version
			{
				get
				{
					return this.version;
				}
			}

			// Token: 0x06005C3E RID: 23614 RVA: 0x00180CA4 File Offset: 0x0017EEA4
			public object Add(object item)
			{
				this.EnsureSpace(1);
				this.version++;
				this.entries[this.count] = new ListBox.ItemArray.Entry(item);
				ListBox.ItemArray.Entry[] array = this.entries;
				int num = this.count;
				this.count = num + 1;
				return array[num];
			}

			// Token: 0x06005C3F RID: 23615 RVA: 0x00180CF4 File Offset: 0x0017EEF4
			public void AddRange(ICollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.EnsureSpace(items.Count);
				foreach (object item in items)
				{
					ListBox.ItemArray.Entry[] array = this.entries;
					int num = this.count;
					this.count = num + 1;
					array[num] = new ListBox.ItemArray.Entry(item);
				}
				this.version++;
			}

			// Token: 0x06005C40 RID: 23616 RVA: 0x00180D84 File Offset: 0x0017EF84
			public void Clear()
			{
				if (this.count > 0)
				{
					Array.Clear(this.entries, 0, this.count);
				}
				this.count = 0;
				this.version++;
			}

			// Token: 0x06005C41 RID: 23617 RVA: 0x00180DB8 File Offset: 0x0017EFB8
			public static int CreateMask()
			{
				int result = ListBox.ItemArray.lastMask;
				ListBox.ItemArray.lastMask <<= 1;
				return result;
			}

			// Token: 0x06005C42 RID: 23618 RVA: 0x00180DD8 File Offset: 0x0017EFD8
			private void EnsureSpace(int elements)
			{
				if (this.entries == null)
				{
					this.entries = new ListBox.ItemArray.Entry[Math.Max(elements, 4)];
					return;
				}
				if (this.count + elements >= this.entries.Length)
				{
					int num = Math.Max(this.entries.Length * 2, this.entries.Length + elements);
					ListBox.ItemArray.Entry[] array = new ListBox.ItemArray.Entry[num];
					this.entries.CopyTo(array, 0);
					this.entries = array;
				}
			}

			// Token: 0x06005C43 RID: 23619 RVA: 0x00180E48 File Offset: 0x0017F048
			public int GetActualIndex(int virtualIndex, int stateMask)
			{
				if (stateMask == 0)
				{
					return virtualIndex;
				}
				int num = -1;
				for (int i = 0; i < this.count; i++)
				{
					if ((this.entries[i].state & stateMask) != 0)
					{
						num++;
						if (num == virtualIndex)
						{
							return i;
						}
					}
				}
				return -1;
			}

			// Token: 0x06005C44 RID: 23620 RVA: 0x00180E8C File Offset: 0x0017F08C
			public int GetCount(int stateMask)
			{
				if (stateMask == 0)
				{
					return this.count;
				}
				int num = 0;
				for (int i = 0; i < this.count; i++)
				{
					if ((this.entries[i].state & stateMask) != 0)
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x06005C45 RID: 23621 RVA: 0x00180ECC File Offset: 0x0017F0CC
			public IEnumerator GetEnumerator(int stateMask)
			{
				return this.GetEnumerator(stateMask, false);
			}

			// Token: 0x06005C46 RID: 23622 RVA: 0x00180ED6 File Offset: 0x0017F0D6
			public IEnumerator GetEnumerator(int stateMask, bool anyBit)
			{
				return new ListBox.ItemArray.EntryEnumerator(this, stateMask, anyBit);
			}

			// Token: 0x06005C47 RID: 23623 RVA: 0x00180EE0 File Offset: 0x0017F0E0
			public object GetItem(int virtualIndex, int stateMask)
			{
				int actualIndex = this.GetActualIndex(virtualIndex, stateMask);
				if (actualIndex == -1)
				{
					throw new IndexOutOfRangeException();
				}
				return this.entries[actualIndex].item;
			}

			// Token: 0x06005C48 RID: 23624 RVA: 0x00180F10 File Offset: 0x0017F110
			internal object GetEntryObject(int virtualIndex, int stateMask)
			{
				int actualIndex = this.GetActualIndex(virtualIndex, stateMask);
				if (actualIndex == -1)
				{
					throw new IndexOutOfRangeException();
				}
				return this.entries[actualIndex];
			}

			// Token: 0x06005C49 RID: 23625 RVA: 0x00180F38 File Offset: 0x0017F138
			public bool GetState(int index, int stateMask)
			{
				return (this.entries[index].state & stateMask) == stateMask;
			}

			// Token: 0x06005C4A RID: 23626 RVA: 0x00180F4C File Offset: 0x0017F14C
			public int IndexOf(object item, int stateMask)
			{
				int num = -1;
				for (int i = 0; i < this.count; i++)
				{
					if (stateMask == 0 || (this.entries[i].state & stateMask) != 0)
					{
						num++;
						if (this.entries[i].item.Equals(item))
						{
							return num;
						}
					}
				}
				return -1;
			}

			// Token: 0x06005C4B RID: 23627 RVA: 0x00180F9C File Offset: 0x0017F19C
			public int IndexOfIdentifier(object identifier, int stateMask)
			{
				int num = -1;
				for (int i = 0; i < this.count; i++)
				{
					if (stateMask == 0 || (this.entries[i].state & stateMask) != 0)
					{
						num++;
						if (this.entries[i] == identifier)
						{
							return num;
						}
					}
				}
				return -1;
			}

			// Token: 0x06005C4C RID: 23628 RVA: 0x00180FE4 File Offset: 0x0017F1E4
			public void Insert(int index, object item)
			{
				this.EnsureSpace(1);
				if (index < this.count)
				{
					Array.Copy(this.entries, index, this.entries, index + 1, this.count - index);
				}
				this.entries[index] = new ListBox.ItemArray.Entry(item);
				this.count++;
				this.version++;
			}

			// Token: 0x06005C4D RID: 23629 RVA: 0x00181048 File Offset: 0x0017F248
			public void Remove(object item)
			{
				int num = this.IndexOf(item, 0);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06005C4E RID: 23630 RVA: 0x0018106C File Offset: 0x0017F26C
			public void RemoveAt(int index)
			{
				this.count--;
				for (int i = index; i < this.count; i++)
				{
					this.entries[i] = this.entries[i + 1];
				}
				this.entries[this.count] = null;
				this.version++;
			}

			// Token: 0x06005C4F RID: 23631 RVA: 0x001810C6 File Offset: 0x0017F2C6
			public void SetItem(int index, object item)
			{
				this.entries[index].item = item;
			}

			// Token: 0x06005C50 RID: 23632 RVA: 0x001810D6 File Offset: 0x0017F2D6
			public void SetState(int index, int stateMask, bool value)
			{
				if (value)
				{
					this.entries[index].state |= stateMask;
				}
				else
				{
					this.entries[index].state &= ~stateMask;
				}
				this.version++;
			}

			// Token: 0x06005C51 RID: 23633 RVA: 0x00181116 File Offset: 0x0017F316
			public int BinarySearch(object element)
			{
				return Array.BinarySearch(this.entries, 0, this.count, element, this);
			}

			// Token: 0x06005C52 RID: 23634 RVA: 0x0018112C File Offset: 0x0017F32C
			public void Sort()
			{
				Array.Sort(this.entries, 0, this.count, this);
			}

			// Token: 0x06005C53 RID: 23635 RVA: 0x00181141 File Offset: 0x0017F341
			public void Sort(Array externalArray)
			{
				Array.Sort(externalArray, this);
			}

			// Token: 0x06005C54 RID: 23636 RVA: 0x0018114C File Offset: 0x0017F34C
			int IComparer.Compare(object item1, object item2)
			{
				if (item1 == null)
				{
					if (item2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (item2 == null)
					{
						return 1;
					}
					if (item1 is ListBox.ItemArray.Entry)
					{
						item1 = ((ListBox.ItemArray.Entry)item1).item;
					}
					if (item2 is ListBox.ItemArray.Entry)
					{
						item2 = ((ListBox.ItemArray.Entry)item2).item;
					}
					string itemText = this.listControl.GetItemText(item1);
					string itemText2 = this.listControl.GetItemText(item2);
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					return compareInfo.Compare(itemText, itemText2, CompareOptions.StringSort);
				}
			}

			// Token: 0x040039EF RID: 14831
			private static int lastMask = 1;

			// Token: 0x040039F0 RID: 14832
			private ListControl listControl;

			// Token: 0x040039F1 RID: 14833
			private ListBox.ItemArray.Entry[] entries;

			// Token: 0x040039F2 RID: 14834
			private int count;

			// Token: 0x040039F3 RID: 14835
			private int version;

			// Token: 0x02000895 RID: 2197
			private class Entry
			{
				// Token: 0x060070B0 RID: 28848 RVA: 0x0019BEDC File Offset: 0x0019A0DC
				public Entry(object item)
				{
					this.item = item;
					this.state = 0;
				}

				// Token: 0x040043F1 RID: 17393
				public object item;

				// Token: 0x040043F2 RID: 17394
				public int state;
			}

			// Token: 0x02000896 RID: 2198
			private class EntryEnumerator : IEnumerator
			{
				// Token: 0x060070B1 RID: 28849 RVA: 0x0019BEF2 File Offset: 0x0019A0F2
				public EntryEnumerator(ListBox.ItemArray items, int state, bool anyBit)
				{
					this.items = items;
					this.state = state;
					this.anyBit = anyBit;
					this.version = items.version;
					this.current = -1;
				}

				// Token: 0x060070B2 RID: 28850 RVA: 0x0019BF24 File Offset: 0x0019A124
				bool IEnumerator.MoveNext()
				{
					if (this.version != this.items.version)
					{
						throw new InvalidOperationException(SR.GetString("ListEnumVersionMismatch"));
					}
					while (this.current < this.items.count - 1)
					{
						this.current++;
						if (this.anyBit)
						{
							if ((this.items.entries[this.current].state & this.state) != 0)
							{
								return true;
							}
						}
						else if ((this.items.entries[this.current].state & this.state) == this.state)
						{
							return true;
						}
					}
					this.current = this.items.count;
					return false;
				}

				// Token: 0x060070B3 RID: 28851 RVA: 0x0019BFDB File Offset: 0x0019A1DB
				void IEnumerator.Reset()
				{
					if (this.version != this.items.version)
					{
						throw new InvalidOperationException(SR.GetString("ListEnumVersionMismatch"));
					}
					this.current = -1;
				}

				// Token: 0x1700187B RID: 6267
				// (get) Token: 0x060070B4 RID: 28852 RVA: 0x0019C008 File Offset: 0x0019A208
				object IEnumerator.Current
				{
					get
					{
						if (this.current == -1 || this.current == this.items.count)
						{
							throw new InvalidOperationException(SR.GetString("ListEnumCurrentOutOfRange"));
						}
						return this.items.entries[this.current].item;
					}
				}

				// Token: 0x040043F3 RID: 17395
				private ListBox.ItemArray items;

				// Token: 0x040043F4 RID: 17396
				private bool anyBit;

				// Token: 0x040043F5 RID: 17397
				private int state;

				// Token: 0x040043F6 RID: 17398
				private int current;

				// Token: 0x040043F7 RID: 17399
				private int version;
			}
		}

		/// <summary>Represents the collection of items in a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		// Token: 0x02000605 RID: 1541
		[ListBindable(false)]
		public class ObjectCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" />.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListBox" /> that owns the collection. </param>
			// Token: 0x06005C56 RID: 23638 RVA: 0x001811CC File Offset: 0x0017F3CC
			public ObjectCollection(ListBox owner)
			{
				this.owner = owner;
			}

			/// <summary>Initializes a new instance of <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> based on another <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" />.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListBox" /> that owns the collection. </param>
			/// <param name="value">A <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> from which the contents are copied to this collection. </param>
			// Token: 0x06005C57 RID: 23639 RVA: 0x001811DB File Offset: 0x0017F3DB
			public ObjectCollection(ListBox owner, ListBox.ObjectCollection value)
			{
				this.owner = owner;
				this.AddRange(value);
			}

			/// <summary>Initializes a new instance of <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> containing an array of objects.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListBox" /> that owns the collection. </param>
			/// <param name="value">An array of objects to add to the collection. </param>
			// Token: 0x06005C58 RID: 23640 RVA: 0x001811F1 File Offset: 0x0017F3F1
			public ObjectCollection(ListBox owner, object[] value)
			{
				this.owner = owner;
				this.AddRange(value);
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection </returns>
			// Token: 0x1700161D RID: 5661
			// (get) Token: 0x06005C59 RID: 23641 RVA: 0x00181207 File Offset: 0x0017F407
			public int Count
			{
				get
				{
					return this.InnerArray.GetCount(0);
				}
			}

			// Token: 0x1700161E RID: 5662
			// (get) Token: 0x06005C5A RID: 23642 RVA: 0x00181215 File Offset: 0x0017F415
			internal ListBox.ItemArray InnerArray
			{
				get
				{
					if (this.items == null)
					{
						this.items = new ListBox.ItemArray(this.owner);
					}
					return this.items;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" />.</returns>
			// Token: 0x1700161F RID: 5663
			// (get) Token: 0x06005C5B RID: 23643 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001620 RID: 5664
			// (get) Token: 0x06005C5C RID: 23644 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001621 RID: 5665
			// (get) Token: 0x06005C5D RID: 23645 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if this collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001622 RID: 5666
			// (get) Token: 0x06005C5E RID: 23646 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds an item to the list of items for a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
			/// <param name="item">An object representing the item to add to the collection. </param>
			/// <returns>The zero-based index of the item in the collection, or -1 if <see cref="M:System.Windows.Forms.ListBox.BeginUpdate" /> has been called.</returns>
			/// <exception cref="T:System.SystemException">There is insufficient space available to add the new item to the list. </exception>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="item" /> is <see langword="null" />.</exception>
			// Token: 0x06005C5F RID: 23647 RVA: 0x00181238 File Offset: 0x0017F438
			public int Add(object item)
			{
				this.owner.CheckNoDataSource();
				int result = this.AddInternal(item);
				this.owner.UpdateHorizontalExtent();
				return result;
			}

			// Token: 0x06005C60 RID: 23648 RVA: 0x00181264 File Offset: 0x0017F464
			private int AddInternal(object item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				int num = -1;
				if (!this.owner.sorted)
				{
					this.InnerArray.Add(item);
				}
				else
				{
					if (this.Count > 0)
					{
						num = this.InnerArray.BinarySearch(item);
						if (num < 0)
						{
							num = ~num;
						}
					}
					else
					{
						num = 0;
					}
					this.InnerArray.Insert(num, item);
				}
				bool flag = false;
				try
				{
					if (this.owner.sorted)
					{
						if (this.owner.IsHandleCreated)
						{
							this.owner.NativeInsert(num, item);
							this.owner.UpdateMaxItemWidth(item, false);
							if (this.owner.selectedItems != null)
							{
								this.owner.selectedItems.Dirty();
							}
						}
					}
					else
					{
						num = this.Count - 1;
						if (this.owner.IsHandleCreated)
						{
							this.owner.NativeAdd(item);
							this.owner.UpdateMaxItemWidth(item, false);
						}
					}
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.InnerArray.Remove(item);
					}
				}
				return num;
			}

			/// <summary>Adds an object to the <see cref="T:System.Windows.Forms.ListBox" /> class.</summary>
			/// <param name="item">The object to be added to the <see cref="T:System.Windows.Forms.ListBox" />.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="item" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Windows.Forms.ListBox" /> has a data source.</exception>
			/// <exception cref="T:System.SystemException">There is insufficient space available to store the new item.</exception>
			// Token: 0x06005C61 RID: 23649 RVA: 0x00181378 File Offset: 0x0017F578
			int IList.Add(object item)
			{
				return this.Add(item);
			}

			/// <summary>Adds the items of an existing <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> to the list of items in a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> to load into this collection. </param>
			// Token: 0x06005C62 RID: 23650 RVA: 0x00181381 File Offset: 0x0017F581
			public void AddRange(ListBox.ObjectCollection value)
			{
				this.owner.CheckNoDataSource();
				this.AddRangeInternal(value);
			}

			/// <summary>Adds an array of items to the list of items for a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
			/// <param name="items">An array of objects to add to the list. </param>
			// Token: 0x06005C63 RID: 23651 RVA: 0x00181381 File Offset: 0x0017F581
			public void AddRange(object[] items)
			{
				this.owner.CheckNoDataSource();
				this.AddRangeInternal(items);
			}

			// Token: 0x06005C64 RID: 23652 RVA: 0x00181398 File Offset: 0x0017F598
			internal void AddRangeInternal(ICollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.owner.BeginUpdate();
				try
				{
					foreach (object item in items)
					{
						this.AddInternal(item);
					}
				}
				finally
				{
					this.owner.UpdateHorizontalExtent();
					this.owner.EndUpdate();
				}
			}

			/// <summary>Gets or sets the item at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to get or set. </param>
			/// <returns>An object representing the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListBox.ObjectCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> class. </exception>
			// Token: 0x17001623 RID: 5667
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public virtual object this[int index]
			{
				get
				{
					if (index < 0 || index >= this.InnerArray.GetCount(0))
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.InnerArray.GetItem(index, 0);
				}
				set
				{
					this.owner.CheckNoDataSource();
					this.SetItemInternal(index, value);
				}
			}

			/// <summary>Removes all items from the collection.</summary>
			// Token: 0x06005C67 RID: 23655 RVA: 0x00181498 File Offset: 0x0017F698
			public virtual void Clear()
			{
				this.owner.CheckNoDataSource();
				this.ClearInternal();
			}

			// Token: 0x06005C68 RID: 23656 RVA: 0x001814AC File Offset: 0x0017F6AC
			internal void ClearInternal()
			{
				int count = this.owner.Items.Count;
				for (int i = 0; i < count; i++)
				{
					this.owner.UpdateMaxItemWidth(this.InnerArray.GetItem(i, 0), true);
				}
				if (this.owner.IsHandleCreated)
				{
					this.owner.NativeClear();
				}
				this.InnerArray.Clear();
				this.owner.maxWidth = -1;
				this.owner.UpdateHorizontalExtent();
			}

			/// <summary>Determines whether the specified item is located within the collection.</summary>
			/// <param name="value">An object representing the item to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the item is located within the collection; otherwise, <see langword="false" />.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			// Token: 0x06005C69 RID: 23657 RVA: 0x00181529 File Offset: 0x0017F729
			public bool Contains(object value)
			{
				return this.IndexOf(value) != -1;
			}

			/// <summary>Copies the entire collection into an existing array of objects at a specified location within the array.</summary>
			/// <param name="destination">The object array in which the items from the collection are copied to. </param>
			/// <param name="arrayIndex">The location within the destination array to copy the items from the collection to. </param>
			// Token: 0x06005C6A RID: 23658 RVA: 0x00181538 File Offset: 0x0017F738
			public void CopyTo(object[] destination, int arrayIndex)
			{
				int count = this.InnerArray.GetCount(0);
				for (int i = 0; i < count; i++)
				{
					destination[i + arrayIndex] = this.InnerArray.GetItem(i, 0);
				}
			}

			/// <summary>Copies the elements of the collection to an array, starting at a particular array index.</summary>
			/// <param name="destination">The one-dimensional array that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in the array at which copying begins.</param>
			/// <exception cref="T:System.ArrayTypeMismatchException">The array type is not compatible with the items in the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" />.</exception>
			// Token: 0x06005C6B RID: 23659 RVA: 0x00181570 File Offset: 0x0017F770
			void ICollection.CopyTo(Array destination, int index)
			{
				int count = this.InnerArray.GetCount(0);
				for (int i = 0; i < count; i++)
				{
					destination.SetValue(this.InnerArray.GetItem(i, 0), i + index);
				}
			}

			/// <summary>Returns an enumerator to use to iterate through the item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the item collection.</returns>
			// Token: 0x06005C6C RID: 23660 RVA: 0x001815AC File Offset: 0x0017F7AC
			public IEnumerator GetEnumerator()
			{
				return this.InnerArray.GetEnumerator(0);
			}

			/// <summary>Returns the index within the collection of the specified item.</summary>
			/// <param name="value">An object representing the item to locate in the collection. </param>
			/// <returns>The zero-based index where the item is located within the collection; otherwise, negative one (-1).</returns>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is null. </exception>
			// Token: 0x06005C6D RID: 23661 RVA: 0x001815BA File Offset: 0x0017F7BA
			public int IndexOf(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.InnerArray.IndexOf(value, 0);
			}

			// Token: 0x06005C6E RID: 23662 RVA: 0x001815D7 File Offset: 0x0017F7D7
			internal int IndexOfIdentifier(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.InnerArray.IndexOfIdentifier(value, 0);
			}

			/// <summary>Inserts an item into the list box at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted. </param>
			/// <param name="item">An object representing the item to insert. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than value of the <see cref="P:System.Windows.Forms.ListBox.ObjectCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> class. </exception>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="item" /> is <see langword="null" />.</exception>
			// Token: 0x06005C6F RID: 23663 RVA: 0x001815F4 File Offset: 0x0017F7F4
			public void Insert(int index, object item)
			{
				this.owner.CheckNoDataSource();
				if (index < 0 || index > this.InnerArray.GetCount(0))
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (this.owner.sorted)
				{
					this.Add(item);
				}
				else
				{
					this.InnerArray.Insert(index, item);
					if (this.owner.IsHandleCreated)
					{
						bool flag = false;
						try
						{
							this.owner.NativeInsert(index, item);
							this.owner.UpdateMaxItemWidth(item, false);
							flag = true;
						}
						finally
						{
							if (!flag)
							{
								this.InnerArray.RemoveAt(index);
							}
						}
					}
				}
				this.owner.UpdateHorizontalExtent();
			}

			/// <summary>Removes the specified object from the collection.</summary>
			/// <param name="value">An object representing the item to remove from the collection. </param>
			// Token: 0x06005C70 RID: 23664 RVA: 0x001816DC File Offset: 0x0017F8DC
			public void Remove(object value)
			{
				int num = this.InnerArray.IndexOf(value, 0);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			/// <summary>Removes the item at the specified index within the collection.</summary>
			/// <param name="index">The zero-based index of the item to remove. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListBox.ObjectCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> class. </exception>
			// Token: 0x06005C71 RID: 23665 RVA: 0x00181704 File Offset: 0x0017F904
			public void RemoveAt(int index)
			{
				this.owner.CheckNoDataSource();
				if (index < 0 || index >= this.InnerArray.GetCount(0))
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.owner.UpdateMaxItemWidth(this.InnerArray.GetItem(index, 0), true);
				this.InnerArray.RemoveAt(index);
				if (this.owner.IsHandleCreated)
				{
					this.owner.NativeRemoveAt(index);
				}
				this.owner.UpdateHorizontalExtent();
			}

			// Token: 0x06005C72 RID: 23666 RVA: 0x001817AC File Offset: 0x0017F9AC
			internal void SetItemInternal(int index, object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (index < 0 || index >= this.InnerArray.GetCount(0))
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.owner.UpdateMaxItemWidth(this.InnerArray.GetItem(index, 0), true);
				this.InnerArray.SetItem(index, value);
				if (this.owner.IsHandleCreated)
				{
					bool flag = this.owner.SelectedIndex == index;
					if (string.Compare(this.owner.GetItemText(value), this.owner.NativeGetItemText(index), true, CultureInfo.CurrentCulture) != 0)
					{
						this.owner.NativeRemoveAt(index);
						this.owner.SelectedItems.SetSelected(index, false);
						this.owner.NativeInsert(index, value);
						this.owner.UpdateMaxItemWidth(value, false);
						if (flag)
						{
							this.owner.SelectedIndex = index;
						}
					}
					else if (flag)
					{
						this.owner.OnSelectedIndexChanged(EventArgs.Empty);
					}
				}
				this.owner.UpdateHorizontalExtent();
			}

			// Token: 0x040039F4 RID: 14836
			private ListBox owner;

			// Token: 0x040039F5 RID: 14837
			private ListBox.ItemArray items;
		}

		/// <summary>Represents a collection of integers in a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		// Token: 0x02000606 RID: 1542
		public class IntegerCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ListBox" /> that owns the collection.</param>
			// Token: 0x06005C73 RID: 23667 RVA: 0x001818DE File Offset: 0x0017FADE
			public IntegerCollection(ListBox owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of selected items in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
			/// <returns>The number of selected items in the <see cref="T:System.Windows.Forms.ListBox" />.</returns>
			// Token: 0x17001624 RID: 5668
			// (get) Token: 0x06005C74 RID: 23668 RVA: 0x001818ED File Offset: 0x0017FAED
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.count;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection of controls.</summary>
			/// <returns>The object used to synchronize to the collection.</returns>
			// Token: 0x17001625 RID: 5669
			// (get) Token: 0x06005C75 RID: 23669 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001626 RID: 5670
			// (get) Token: 0x06005C76 RID: 23670 RVA: 0x0000E214 File Offset: 0x0000C414
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x17001627 RID: 5671
			// (get) Token: 0x06005C77 RID: 23671 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x17001628 RID: 5672
			// (get) Token: 0x06005C78 RID: 23672 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Determines whether the specified integer is in the collection.</summary>
			/// <param name="item">The integer to search for in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the specified integer is in the collection; otherwise, <see langword="false" />. </returns>
			// Token: 0x06005C79 RID: 23673 RVA: 0x001818F5 File Offset: 0x0017FAF5
			public bool Contains(int item)
			{
				return this.IndexOf(item) != -1;
			}

			/// <summary>Determines whether the specified tab stop is in the collection.</summary>
			/// <param name="item">The tab stop to locate in the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</param>
			/// <returns>
			///     <see langword="true" /> if item is an integer located in the IntegerCollection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005C7A RID: 23674 RVA: 0x00181904 File Offset: 0x0017FB04
			bool IList.Contains(object item)
			{
				return item is int && this.Contains((int)item);
			}

			/// <summary>Removes all integers from the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</summary>
			// Token: 0x06005C7B RID: 23675 RVA: 0x0018191C File Offset: 0x0017FB1C
			public void Clear()
			{
				this.count = 0;
				this.innerArray = null;
			}

			/// <summary>Retrieves the index within the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" /> of the specified integer.</summary>
			/// <param name="item">The integer for which to retrieve the index.</param>
			/// <returns>The zero-based index of the integer in the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />; otherwise, negative one (-1).</returns>
			// Token: 0x06005C7C RID: 23676 RVA: 0x0018192C File Offset: 0x0017FB2C
			public int IndexOf(int item)
			{
				int num = -1;
				if (this.innerArray != null)
				{
					num = Array.IndexOf<int>(this.innerArray, item);
					if (num >= this.count)
					{
						num = -1;
					}
				}
				return num;
			}

			/// <summary>Returns the index of the specified tab stop in the collection.</summary>
			/// <param name="item">The tab stop to locate in the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</param>
			/// <returns>The zero-based index of item if it was found in the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />; otherwise, -1.</returns>
			// Token: 0x06005C7D RID: 23677 RVA: 0x0018195C File Offset: 0x0017FB5C
			int IList.IndexOf(object item)
			{
				if (item is int)
				{
					return this.IndexOf((int)item);
				}
				return -1;
			}

			// Token: 0x06005C7E RID: 23678 RVA: 0x00181974 File Offset: 0x0017FB74
			private int AddInternal(int item)
			{
				this.EnsureSpace(1);
				int num = this.IndexOf(item);
				if (num == -1)
				{
					int[] array = this.innerArray;
					int num2 = this.count;
					this.count = num2 + 1;
					array[num2] = item;
					Array.Sort<int>(this.innerArray, 0, this.count);
					num = this.IndexOf(item);
				}
				return num;
			}

			/// <summary>Adds a unique integer to the collection in sorted order.</summary>
			/// <param name="item">The integer to add to the collection.</param>
			/// <returns>The index of the added item.</returns>
			/// <exception cref="T:System.SystemException">There is insufficient space available to store the new item.</exception>
			// Token: 0x06005C7F RID: 23679 RVA: 0x001819C8 File Offset: 0x0017FBC8
			public int Add(int item)
			{
				int result = this.AddInternal(item);
				this.owner.UpdateCustomTabOffsets();
				return result;
			}

			/// <summary>Adds a tab stop to the collection.</summary>
			/// <param name="item">The tab stop to add to the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</param>
			/// <returns>The index at which the integer was added to the collection.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="item" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="item" /> is not an 32-bit signed integer.</exception>
			/// <exception cref="T:System.SystemException">There is insufficient space to store the new item in the collection.</exception>
			// Token: 0x06005C80 RID: 23680 RVA: 0x001819E9 File Offset: 0x0017FBE9
			int IList.Add(object item)
			{
				if (!(item is int))
				{
					throw new ArgumentException("item");
				}
				return this.Add((int)item);
			}

			/// <summary>Adds an array of integers to the collection.</summary>
			/// <param name="items">The array of integers to add to the collection.</param>
			// Token: 0x06005C81 RID: 23681 RVA: 0x00181A0A File Offset: 0x0017FC0A
			public void AddRange(int[] items)
			{
				this.AddRangeInternal(items);
			}

			/// <summary>Adds the contents of an existing <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" /> to another collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" /> to add to another collection.</param>
			// Token: 0x06005C82 RID: 23682 RVA: 0x00181A0A File Offset: 0x0017FC0A
			public void AddRange(ListBox.IntegerCollection value)
			{
				this.AddRangeInternal(value);
			}

			// Token: 0x06005C83 RID: 23683 RVA: 0x00181A14 File Offset: 0x0017FC14
			private void AddRangeInternal(ICollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.owner.BeginUpdate();
				try
				{
					this.EnsureSpace(items.Count);
					foreach (object obj in items)
					{
						if (!(obj is int))
						{
							throw new ArgumentException("item");
						}
						this.AddInternal((int)obj);
					}
					this.owner.UpdateCustomTabOffsets();
				}
				finally
				{
					this.owner.EndUpdate();
				}
			}

			// Token: 0x06005C84 RID: 23684 RVA: 0x00181AC8 File Offset: 0x0017FCC8
			private void EnsureSpace(int elements)
			{
				if (this.innerArray == null)
				{
					this.innerArray = new int[Math.Max(elements, 4)];
					return;
				}
				if (this.count + elements >= this.innerArray.Length)
				{
					int num = Math.Max(this.innerArray.Length * 2, this.innerArray.Length + elements);
					int[] array = new int[num];
					this.innerArray.CopyTo(array, 0);
					this.innerArray = array;
				}
			}

			/// <summary>Clears all the tab stops from the collection.</summary>
			// Token: 0x06005C85 RID: 23685 RVA: 0x00181B37 File Offset: 0x0017FD37
			void IList.Clear()
			{
				this.Clear();
			}

			/// <summary>Inserts an item into the collection at a specified index.</summary>
			/// <param name="index">The zero-based index at which value should be inserted.</param>
			/// <param name="value">The object to insert into the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005C86 RID: 23686 RVA: 0x00181B3F File Offset: 0x0017FD3F
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxCantInsertIntoIntegerCollection"));
			}

			/// <summary>Removes the first occurrence of an item from the collection.</summary>
			/// <param name="value">The object to add to the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005C87 RID: 23687 RVA: 0x00181B50 File Offset: 0x0017FD50
			void IList.Remove(object value)
			{
				if (!(value is int))
				{
					throw new ArgumentException("value");
				}
				this.Remove((int)value);
			}

			/// <summary>Removes the item at a specified index.</summary>
			/// <param name="index">The index of the item to remove.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005C88 RID: 23688 RVA: 0x00181B71 File Offset: 0x0017FD71
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			/// <summary>Removes the specified integer from the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</summary>
			/// <param name="item">The integer to remove from the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</param>
			// Token: 0x06005C89 RID: 23689 RVA: 0x00181B7C File Offset: 0x0017FD7C
			public void Remove(int item)
			{
				int num = this.IndexOf(item);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			/// <summary>Removes the integer at the specified index from the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</summary>
			/// <param name="index">The zero-based index of the integer to remove.</param>
			// Token: 0x06005C8A RID: 23690 RVA: 0x00181B9C File Offset: 0x0017FD9C
			public void RemoveAt(int index)
			{
				if (index < 0 || index >= this.count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.count--;
				for (int i = index; i < this.count; i++)
				{
					this.innerArray[i] = this.innerArray[i + 1];
				}
			}

			/// <summary>Gets or sets the <see cref="P:System.Windows.Forms.ListBox.IntegerCollection.Item(System.Int32)" /> having the specified index.</summary>
			/// <param name="index">The position of the <see cref="P:System.Windows.Forms.ListBox.IntegerCollection.Item(System.Int32)" /> in the collection.</param>
			/// <returns>The selected <see cref="P:System.Windows.Forms.ListBox.IntegerCollection.Item(System.Int32)" /> at the specified position.</returns>
			// Token: 0x17001629 RID: 5673
			public int this[int index]
			{
				get
				{
					return this.innerArray[index];
				}
				set
				{
					if (index < 0 || index >= this.count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.innerArray[index] = value;
					this.owner.UpdateCustomTabOffsets();
				}
			}

			/// <summary>Gets or sets the tab stop at the specified index.</summary>
			/// <param name="index">The zero-based index that specifies which tab stop to get.</param>
			/// <returns>The tab stop that is stored at the specified location in the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</returns>
			/// <exception cref="T:System.ArgumentException">The object is not an integer.</exception>
			// Token: 0x1700162A RID: 5674
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (!(value is int))
					{
						throw new ArgumentException("value");
					}
					this[index] = (int)value;
				}
			}

			/// <summary>Copies the entire <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" /> into an existing array of integers at a specified location within the array.</summary>
			/// <param name="destination">The array into which the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" /> is copied.</param>
			/// <param name="index">The location within the destination array to which to copy the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</param>
			// Token: 0x06005C8F RID: 23695 RVA: 0x00181CB4 File Offset: 0x0017FEB4
			public void CopyTo(Array destination, int index)
			{
				int num = this.Count;
				for (int i = 0; i < num; i++)
				{
					destination.SetValue(this[i], i + index);
				}
			}

			/// <summary>Retrieves an enumeration of all the integers in the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Windows.Forms.ListBox.IntegerCollection" />.</returns>
			// Token: 0x06005C90 RID: 23696 RVA: 0x00181CE9 File Offset: 0x0017FEE9
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ListBox.IntegerCollection.CustomTabOffsetsEnumerator(this);
			}

			// Token: 0x040039F6 RID: 14838
			private ListBox owner;

			// Token: 0x040039F7 RID: 14839
			private int[] innerArray;

			// Token: 0x040039F8 RID: 14840
			private int count;

			// Token: 0x02000897 RID: 2199
			private class CustomTabOffsetsEnumerator : IEnumerator
			{
				// Token: 0x060070B5 RID: 28853 RVA: 0x0019C058 File Offset: 0x0019A258
				public CustomTabOffsetsEnumerator(ListBox.IntegerCollection items)
				{
					this.items = items;
					this.current = -1;
				}

				// Token: 0x060070B6 RID: 28854 RVA: 0x0019C06E File Offset: 0x0019A26E
				bool IEnumerator.MoveNext()
				{
					if (this.current < this.items.Count - 1)
					{
						this.current++;
						return true;
					}
					this.current = this.items.Count;
					return false;
				}

				// Token: 0x060070B7 RID: 28855 RVA: 0x0019C0A7 File Offset: 0x0019A2A7
				void IEnumerator.Reset()
				{
					this.current = -1;
				}

				// Token: 0x1700187C RID: 6268
				// (get) Token: 0x060070B8 RID: 28856 RVA: 0x0019C0B0 File Offset: 0x0019A2B0
				object IEnumerator.Current
				{
					get
					{
						if (this.current == -1 || this.current == this.items.Count)
						{
							throw new InvalidOperationException(SR.GetString("ListEnumCurrentOutOfRange"));
						}
						return this.items[this.current];
					}
				}

				// Token: 0x040043F8 RID: 17400
				private ListBox.IntegerCollection items;

				// Token: 0x040043F9 RID: 17401
				private int current;
			}
		}

		/// <summary>Represents the collection containing the indexes to the selected items in a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		// Token: 0x02000607 RID: 1543
		public class SelectedIndexCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" /> class.</summary>
			/// <param name="owner">A <see cref="T:System.Windows.Forms.ListBox" /> representing the owner of the collection. </param>
			// Token: 0x06005C91 RID: 23697 RVA: 0x00181CF1 File Offset: 0x0017FEF1
			public SelectedIndexCollection(ListBox owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x1700162B RID: 5675
			// (get) Token: 0x06005C92 RID: 23698 RVA: 0x00181D00 File Offset: 0x0017FF00
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.SelectedItems.Count;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" />.</returns>
			// Token: 0x1700162C RID: 5676
			// (get) Token: 0x06005C93 RID: 23699 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700162D RID: 5677
			// (get) Token: 0x06005C94 RID: 23700 RVA: 0x0000E214 File Offset: 0x0000C414
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x1700162E RID: 5678
			// (get) Token: 0x06005C95 RID: 23701 RVA: 0x0000E214 File Offset: 0x0000C414
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x1700162F RID: 5679
			// (get) Token: 0x06005C96 RID: 23702 RVA: 0x0000E214 File Offset: 0x0000C414
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Determines whether the specified index is located within the collection.</summary>
			/// <param name="selectedIndex">The index to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified index from the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> for the <see cref="T:System.Windows.Forms.ListBox" /> is an item in this collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005C97 RID: 23703 RVA: 0x00181D12 File Offset: 0x0017FF12
			public bool Contains(int selectedIndex)
			{
				return this.IndexOf(selectedIndex) != -1;
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
			/// <param name="selectedIndex">The index to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the specified index from the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> for the <see cref="T:System.Windows.Forms.ListBox" /> is an item in this collection; otherwise, false.</returns>
			// Token: 0x06005C98 RID: 23704 RVA: 0x00181D21 File Offset: 0x0017FF21
			bool IList.Contains(object selectedIndex)
			{
				return selectedIndex is int && this.Contains((int)selectedIndex);
			}

			/// <summary>Returns the index within the <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" /> of the specified index from the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> of the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
			/// <param name="selectedIndex">The zero-based index from the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> to locate in this collection. </param>
			/// <returns>The zero-based index in the collection where the specified index of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> was located within the <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" />; otherwise, negative one (-1).</returns>
			// Token: 0x06005C99 RID: 23705 RVA: 0x00181D3C File Offset: 0x0017FF3C
			public int IndexOf(int selectedIndex)
			{
				if (selectedIndex >= 0 && selectedIndex < this.InnerArray.GetCount(0) && this.InnerArray.GetState(selectedIndex, ListBox.SelectedObjectCollection.SelectedObjectMask))
				{
					return this.InnerArray.IndexOf(this.InnerArray.GetItem(selectedIndex, 0), ListBox.SelectedObjectCollection.SelectedObjectMask);
				}
				return -1;
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
			/// <param name="selectedIndex">The zero-based index from the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> to locate in this collection.</param>
			/// <returns>The zero-based index in the collection where the specified index of the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> is located if it is in the <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" />; otherwise, -1.</returns>
			// Token: 0x06005C9A RID: 23706 RVA: 0x00181D8E File Offset: 0x0017FF8E
			int IList.IndexOf(object selectedIndex)
			{
				if (selectedIndex is int)
				{
					return this.IndexOf((int)selectedIndex);
				}
				return -1;
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			/// <param name="value">The index to add to the collection.</param>
			/// <returns>The position into which the index was inserted.</returns>
			// Token: 0x06005C9B RID: 23707 RVA: 0x00181DA6 File Offset: 0x0017FFA6
			int IList.Add(object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Clear" />.</summary>
			// Token: 0x06005C9C RID: 23708 RVA: 0x00181DA6 File Offset: 0x0017FFA6
			void IList.Clear()
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
			/// <param name="index">The index at which value should be inserted.</param>
			/// <param name="value">The object to be added to the <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005C9D RID: 23709 RVA: 0x00181DA6 File Offset: 0x0017FFA6
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
			/// <param name="value">The object to be removed from the <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005C9E RID: 23710 RVA: 0x00181DA6 File Offset: 0x0017FFA6
			void IList.Remove(object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
			// Token: 0x06005C9F RID: 23711 RVA: 0x00181DA6 File Offset: 0x0017FFA6
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			/// <summary>Gets the index value at the specified index within this collection.</summary>
			/// <param name="index">The index of the item in the collection to get. </param>
			/// <returns>The index value from the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> that is stored at the specified location.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListBox.SelectedIndexCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.SelectedIndexCollection" /> class. </exception>
			// Token: 0x17001630 RID: 5680
			public int this[int index]
			{
				get
				{
					object entryObject = this.InnerArray.GetEntryObject(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
					return this.InnerArray.IndexOfIdentifier(entryObject, 0);
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the element to get.</param>
			/// <returns>The index value from the <see cref="T:System.Windows.Forms.ListBox.ObjectCollection" /> that is stored at the specified location.</returns>
			// Token: 0x17001631 RID: 5681
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
				}
			}

			// Token: 0x17001632 RID: 5682
			// (get) Token: 0x06005CA3 RID: 23715 RVA: 0x00181DF2 File Offset: 0x0017FFF2
			private ListBox.ItemArray InnerArray
			{
				get
				{
					this.owner.SelectedItems.EnsureUpToDate();
					return this.owner.Items.InnerArray;
				}
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="destination">The destination array. </param>
			/// <param name="index">The index in the destination array at which storing begins. </param>
			// Token: 0x06005CA4 RID: 23716 RVA: 0x00181E14 File Offset: 0x00180014
			public void CopyTo(Array destination, int index)
			{
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					destination.SetValue(this[i], i + index);
				}
			}

			/// <summary>Removes all controls from the collection.</summary>
			// Token: 0x06005CA5 RID: 23717 RVA: 0x00181E49 File Offset: 0x00180049
			public void Clear()
			{
				if (this.owner != null)
				{
					this.owner.ClearSelected();
				}
			}

			/// <summary>Adds the <see cref="T:System.Windows.Forms.ListBox" /> at the specified index location.</summary>
			/// <param name="index">The location in the array at which to add the <see cref="T:System.Windows.Forms.ListBox" />.</param>
			// Token: 0x06005CA6 RID: 23718 RVA: 0x00181E60 File Offset: 0x00180060
			public void Add(int index)
			{
				if (this.owner != null)
				{
					ListBox.ObjectCollection items = this.owner.Items;
					if (items != null && index != -1 && !this.Contains(index))
					{
						this.owner.SetSelected(index, true);
					}
				}
			}

			/// <summary>Removes the specified control from the collection.</summary>
			/// <param name="index">The control to be removed.</param>
			// Token: 0x06005CA7 RID: 23719 RVA: 0x00181EA0 File Offset: 0x001800A0
			public void Remove(int index)
			{
				if (this.owner != null)
				{
					ListBox.ObjectCollection items = this.owner.Items;
					if (items != null && index != -1 && this.Contains(index))
					{
						this.owner.SetSelected(index, false);
					}
				}
			}

			/// <summary>Returns an enumerator to use to iterate through the selected indexes collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the selected indexes collection.</returns>
			// Token: 0x06005CA8 RID: 23720 RVA: 0x00181EDE File Offset: 0x001800DE
			public IEnumerator GetEnumerator()
			{
				return new ListBox.SelectedIndexCollection.SelectedIndexEnumerator(this);
			}

			// Token: 0x040039F9 RID: 14841
			private ListBox owner;

			// Token: 0x02000898 RID: 2200
			private class SelectedIndexEnumerator : IEnumerator
			{
				// Token: 0x060070B9 RID: 28857 RVA: 0x0019C0FF File Offset: 0x0019A2FF
				public SelectedIndexEnumerator(ListBox.SelectedIndexCollection items)
				{
					this.items = items;
					this.current = -1;
				}

				// Token: 0x060070BA RID: 28858 RVA: 0x0019C115 File Offset: 0x0019A315
				bool IEnumerator.MoveNext()
				{
					if (this.current < this.items.Count - 1)
					{
						this.current++;
						return true;
					}
					this.current = this.items.Count;
					return false;
				}

				// Token: 0x060070BB RID: 28859 RVA: 0x0019C14E File Offset: 0x0019A34E
				void IEnumerator.Reset()
				{
					this.current = -1;
				}

				// Token: 0x1700187D RID: 6269
				// (get) Token: 0x060070BC RID: 28860 RVA: 0x0019C158 File Offset: 0x0019A358
				object IEnumerator.Current
				{
					get
					{
						if (this.current == -1 || this.current == this.items.Count)
						{
							throw new InvalidOperationException(SR.GetString("ListEnumCurrentOutOfRange"));
						}
						return this.items[this.current];
					}
				}

				// Token: 0x040043FA RID: 17402
				private ListBox.SelectedIndexCollection items;

				// Token: 0x040043FB RID: 17403
				private int current;
			}
		}

		/// <summary>Represents the collection of selected items in the <see cref="T:System.Windows.Forms.ListBox" />.</summary>
		// Token: 0x02000608 RID: 1544
		public class SelectedObjectCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListBox.SelectedObjectCollection" /> class.</summary>
			/// <param name="owner">A <see cref="T:System.Windows.Forms.ListBox" /> representing the owner of the collection. </param>
			// Token: 0x06005CA9 RID: 23721 RVA: 0x00181EE6 File Offset: 0x001800E6
			public SelectedObjectCollection(ListBox owner)
			{
				this.owner = owner;
				this.stateDirty = true;
				this.lastVersion = -1;
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x17001633 RID: 5683
			// (get) Token: 0x06005CAA RID: 23722 RVA: 0x00181F04 File Offset: 0x00180104
			public int Count
			{
				get
				{
					if (!this.owner.IsHandleCreated)
					{
						if (this.lastVersion != this.InnerArray.Version)
						{
							this.lastVersion = this.InnerArray.Version;
							this.count = this.InnerArray.GetCount(ListBox.SelectedObjectCollection.SelectedObjectMask);
						}
						return this.count;
					}
					switch (this.owner.selectionModeChanging ? this.owner.cachedSelectionMode : this.owner.selectionMode)
					{
					case SelectionMode.None:
						return 0;
					case SelectionMode.One:
					{
						int selectedIndex = this.owner.SelectedIndex;
						if (selectedIndex >= 0)
						{
							return 1;
						}
						return 0;
					}
					case SelectionMode.MultiSimple:
					case SelectionMode.MultiExtended:
						return (int)((long)this.owner.SendMessage(400, 0, 0));
					default:
						return 0;
					}
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>An object that can be used to synchronize access to the underlying list.</returns>
			// Token: 0x17001634 RID: 5684
			// (get) Token: 0x06005CAB RID: 23723 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
			/// <returns>
			///     <see langword="true" /> if the list is synchronized; otherwise <see langword="false" />.</returns>
			// Token: 0x17001635 RID: 5685
			// (get) Token: 0x06005CAC RID: 23724 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
			/// <returns>
			///     <see langword="true" /> if the underlying list has a fixed size; otherwise <see langword="false" />.</returns>
			// Token: 0x17001636 RID: 5686
			// (get) Token: 0x06005CAD RID: 23725 RVA: 0x0000E214 File Offset: 0x0000C414
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06005CAE RID: 23726 RVA: 0x00181FCD File Offset: 0x001801CD
			internal void Dirty()
			{
				this.stateDirty = true;
			}

			// Token: 0x17001637 RID: 5687
			// (get) Token: 0x06005CAF RID: 23727 RVA: 0x00181FD6 File Offset: 0x001801D6
			private ListBox.ItemArray InnerArray
			{
				get
				{
					this.EnsureUpToDate();
					return this.owner.Items.InnerArray;
				}
			}

			// Token: 0x06005CB0 RID: 23728 RVA: 0x00181FEE File Offset: 0x001801EE
			internal void EnsureUpToDate()
			{
				if (this.stateDirty)
				{
					this.stateDirty = false;
					if (this.owner.IsHandleCreated)
					{
						this.owner.NativeUpdateSelection();
					}
				}
			}

			/// <summary>Gets a value indicating whether the collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001638 RID: 5688
			// (get) Token: 0x06005CB1 RID: 23729 RVA: 0x0000E214 File Offset: 0x0000C414
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Determines whether the specified item is located within the collection.</summary>
			/// <param name="selectedObject">An object representing the item to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified item is located in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005CB2 RID: 23730 RVA: 0x00182017 File Offset: 0x00180217
			public bool Contains(object selectedObject)
			{
				return this.IndexOf(selectedObject) != -1;
			}

			/// <summary>Returns the index within the collection of the specified item.</summary>
			/// <param name="selectedObject">An object representing the item to locate in the collection. </param>
			/// <returns>The zero-based index of the item in the collection; otherwise, -1.</returns>
			// Token: 0x06005CB3 RID: 23731 RVA: 0x00182026 File Offset: 0x00180226
			public int IndexOf(object selectedObject)
			{
				return this.InnerArray.IndexOf(selectedObject, ListBox.SelectedObjectCollection.SelectedObjectMask);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			/// <param name="value">The object to add to the collection.</param>
			/// <returns>The position into which the object was inserted.</returns>
			// Token: 0x06005CB4 RID: 23732 RVA: 0x00182039 File Offset: 0x00180239
			int IList.Add(object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Clear" />.</summary>
			// Token: 0x06005CB5 RID: 23733 RVA: 0x00182039 File Offset: 0x00180239
			void IList.Clear()
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
			/// <param name="index">The zero-based index at which the object should be inserted.</param>
			/// <param name="value">The object to insert into the <see cref="T:System.Windows.Forms.ListBox.SelectedObjectCollection" />.</param>
			// Token: 0x06005CB6 RID: 23734 RVA: 0x00182039 File Offset: 0x00180239
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
			/// <param name="value">The object to remove.</param>
			// Token: 0x06005CB7 RID: 23735 RVA: 0x00182039 File Offset: 0x00180239
			void IList.Remove(object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the object to remove from the <see cref="T:System.Windows.Forms.ListBox.SelectedObjectCollection" />.</param>
			// Token: 0x06005CB8 RID: 23736 RVA: 0x00182039 File Offset: 0x00180239
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			// Token: 0x06005CB9 RID: 23737 RVA: 0x0018204A File Offset: 0x0018024A
			internal object GetObjectAt(int index)
			{
				return this.InnerArray.GetEntryObject(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
			}

			/// <summary>Gets the item at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to retrieve. </param>
			/// <returns>An object representing the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.ListBox.ObjectCollection.Count" /> property of the <see cref="T:System.Windows.Forms.ListBox.SelectedObjectCollection" /> class. </exception>
			// Token: 0x17001639 RID: 5689
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public object this[int index]
			{
				get
				{
					return this.InnerArray.GetItem(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
				}
				set
				{
					throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
				}
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="destination">An <see cref="T:System.Array" /> representing the array to copy the contents of the collection to. </param>
			/// <param name="index">The location within the destination array to copy the items from the collection to. </param>
			// Token: 0x06005CBC RID: 23740 RVA: 0x00182070 File Offset: 0x00180270
			public void CopyTo(Array destination, int index)
			{
				int num = this.InnerArray.GetCount(ListBox.SelectedObjectCollection.SelectedObjectMask);
				for (int i = 0; i < num; i++)
				{
					destination.SetValue(this.InnerArray.GetItem(i, ListBox.SelectedObjectCollection.SelectedObjectMask), i + index);
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the selected item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the item collection.</returns>
			// Token: 0x06005CBD RID: 23741 RVA: 0x001820B4 File Offset: 0x001802B4
			public IEnumerator GetEnumerator()
			{
				return this.InnerArray.GetEnumerator(ListBox.SelectedObjectCollection.SelectedObjectMask);
			}

			// Token: 0x06005CBE RID: 23742 RVA: 0x001820C6 File Offset: 0x001802C6
			internal bool GetSelected(int index)
			{
				return this.InnerArray.GetState(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
			}

			// Token: 0x06005CBF RID: 23743 RVA: 0x001820DC File Offset: 0x001802DC
			internal void PushSelectionIntoNativeListBox(int index)
			{
				bool state = this.owner.Items.InnerArray.GetState(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
				if (state)
				{
					this.owner.NativeSetSelected(index, true);
				}
			}

			// Token: 0x06005CC0 RID: 23744 RVA: 0x00182115 File Offset: 0x00180315
			internal void SetSelected(int index, bool value)
			{
				this.InnerArray.SetState(index, ListBox.SelectedObjectCollection.SelectedObjectMask, value);
			}

			/// <summary>Removes all items from the collection of selected items.</summary>
			// Token: 0x06005CC1 RID: 23745 RVA: 0x00182129 File Offset: 0x00180329
			public void Clear()
			{
				if (this.owner != null)
				{
					this.owner.ClearSelected();
				}
			}

			/// <summary>Adds an item to the list of selected items for a <see cref="T:System.Windows.Forms.ListBox" />.</summary>
			/// <param name="value">An object representing the item to add to the collection of selected items.</param>
			// Token: 0x06005CC2 RID: 23746 RVA: 0x00182140 File Offset: 0x00180340
			public void Add(object value)
			{
				if (this.owner != null)
				{
					ListBox.ObjectCollection items = this.owner.Items;
					if (items != null && value != null)
					{
						int num = items.IndexOf(value);
						if (num != -1 && !this.GetSelected(num))
						{
							this.owner.SelectedIndex = num;
						}
					}
				}
			}

			/// <summary>Removes the specified object from the collection of selected items.</summary>
			/// <param name="value">An object representing the item to remove from the collection.</param>
			// Token: 0x06005CC3 RID: 23747 RVA: 0x00182188 File Offset: 0x00180388
			public void Remove(object value)
			{
				if (this.owner != null)
				{
					ListBox.ObjectCollection items = this.owner.Items;
					if (items != null & value != null)
					{
						int num = items.IndexOf(value);
						if (num != -1 && this.GetSelected(num))
						{
							this.owner.SetSelected(num, false);
						}
					}
				}
			}

			// Token: 0x040039FA RID: 14842
			internal static int SelectedObjectMask = ListBox.ItemArray.CreateMask();

			// Token: 0x040039FB RID: 14843
			private ListBox owner;

			// Token: 0x040039FC RID: 14844
			private bool stateDirty;

			// Token: 0x040039FD RID: 14845
			private int lastVersion;

			// Token: 0x040039FE RID: 14846
			private int count;
		}

		// Token: 0x02000609 RID: 1545
		private sealed class ListBoxAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06005CC5 RID: 23749 RVA: 0x001821E2 File Offset: 0x001803E2
			public ListBoxAccessibleObject(ListBox control) : base(control)
			{
				this.owner = control;
			}

			// Token: 0x06005CC6 RID: 23750 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override bool IsIAccessibleExSupported()
			{
				return true;
			}

			// Token: 0x06005CC7 RID: 23751 RVA: 0x001821F4 File Offset: 0x001803F4
			internal override object GetObjectForChild(int childId)
			{
				IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
				if (ListBox.ListBoxAccessibleObject.IsChildIdValid(childId, systemIAccessibleInternal) && (AccessibleRole)systemIAccessibleInternal.get_accRole(childId) == AccessibleRole.ListItem)
				{
					return new ListBox.ListBoxAccessibleObject.ListBoxItemAccessibleObject(this.owner, childId);
				}
				return base.GetObjectForChild(childId);
			}

			// Token: 0x06005CC8 RID: 23752 RVA: 0x0018223A File Offset: 0x0018043A
			private static bool IsChildIdValid(int childId, IAccessible systemIAccessible)
			{
				return childId > 0 && childId <= systemIAccessible.accChildCount;
			}

			// Token: 0x040039FF RID: 14847
			private readonly ListBox owner;

			// Token: 0x02000899 RID: 2201
			private sealed class ListBoxItemAccessibleObject : AccessibleObject
			{
				// Token: 0x060070BD RID: 28861 RVA: 0x0019C1A7 File Offset: 0x0019A3A7
				public ListBoxItemAccessibleObject(ListBox owner, int childId)
				{
					this.owner = owner;
					this.childId = childId;
				}

				// Token: 0x060070BE RID: 28862 RVA: 0x0000E214 File Offset: 0x0000C414
				internal override bool IsIAccessibleExSupported()
				{
					return true;
				}

				// Token: 0x060070BF RID: 28863 RVA: 0x0019C1BD File Offset: 0x0019A3BD
				internal override bool IsPatternSupported(int patternId)
				{
					return patternId == 10017 || base.IsPatternSupported(patternId);
				}

				// Token: 0x060070C0 RID: 28864 RVA: 0x0019C1D0 File Offset: 0x0019A3D0
				internal override void ScrollIntoView()
				{
					if (this.owner.IsHandleCreated && ListBox.ListBoxAccessibleObject.IsChildIdValid(this.childId, this.owner.AccessibilityObject.GetSystemIAccessibleInternal()))
					{
						this.owner.TopIndex = this.childId - 1;
					}
				}

				// Token: 0x040043FC RID: 17404
				private readonly int childId;

				// Token: 0x040043FD RID: 17405
				private readonly ListBox owner;
			}
		}
	}
}
