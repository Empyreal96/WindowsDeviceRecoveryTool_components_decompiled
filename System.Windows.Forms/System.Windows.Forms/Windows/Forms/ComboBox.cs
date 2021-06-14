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
using System.Threading;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using Accessibility;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows combo box control. </summary>
	// Token: 0x0200014F RID: 335
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[DefaultBindingProperty("Text")]
	[Designer("System.Windows.Forms.Design.ComboBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionComboBox")]
	public class ComboBox : ListControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ComboBox" /> class.</summary>
		// Token: 0x06000A8F RID: 2703 RVA: 0x0001FAE4 File Offset: 0x0001DCE4
		public ComboBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick | ControlStyles.UseTextForAccessibility, false);
			this.requestedHeight = 150;
			base.SetState2(2048, true);
		}

		/// <summary>Gets or sets an option that controls how automatic completion works for the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.AutoCompleteMode" />. The values are <see cref="F:System.Windows.Forms.AutoCompleteMode.Append" />, <see cref="F:System.Windows.Forms.AutoCompleteMode.None" />, <see cref="F:System.Windows.Forms.AutoCompleteMode.Suggest" />, and <see cref="F:System.Windows.Forms.AutoCompleteMode.SuggestAppend" />. The default is <see cref="F:System.Windows.Forms.AutoCompleteMode.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the values of <see cref="T:System.Windows.Forms.AutoCompleteMode" />. </exception>
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0001FB73 File Offset: 0x0001DD73
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x0001FB7C File Offset: 0x0001DD7C
		[DefaultValue(AutoCompleteMode.None)]
		[SRDescription("ComboBoxAutoCompleteModeDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return this.autoCompleteMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoCompleteMode));
				}
				if (this.DropDownStyle == ComboBoxStyle.DropDownList && this.AutoCompleteSource != AutoCompleteSource.ListItems && value != AutoCompleteMode.None)
				{
					throw new NotSupportedException(SR.GetString("ComboBoxAutoCompleteModeOnlyNoneAllowed"));
				}
				if (Application.OleRequired() != ApartmentState.STA)
				{
					throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
				}
				bool reset = false;
				if (this.autoCompleteMode != AutoCompleteMode.None && value == AutoCompleteMode.None)
				{
					reset = true;
				}
				this.autoCompleteMode = value;
				this.SetAutoComplete(reset, true);
			}
		}

		/// <summary>Gets or sets a value specifying the source of complete strings used for automatic completion.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.AutoCompleteSource" />. The options are <see langword="AllSystemSources" />, <see langword="AllUrl" />, <see langword="FileSystem" />, <see langword="HistoryList" />, <see langword="RecentlyUsedList" />, <see langword="CustomSource" />, and <see langword="None" />. The default is <see langword="None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the values of <see cref="T:System.Windows.Forms.AutoCompleteSource" />. </exception>
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0001FC0D File Offset: 0x0001DE0D
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0001FC18 File Offset: 0x0001DE18
		[DefaultValue(AutoCompleteSource.None)]
		[SRDescription("ComboBoxAutoCompleteSourceDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return this.autoCompleteSource;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					128,
					7,
					6,
					64,
					1,
					32,
					2,
					256,
					4
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoCompleteSource));
				}
				if (this.DropDownStyle == ComboBoxStyle.DropDownList && this.AutoCompleteMode != AutoCompleteMode.None && value != AutoCompleteSource.ListItems)
				{
					throw new NotSupportedException(SR.GetString("ComboBoxAutoCompleteSourceOnlyListItemsAllowed"));
				}
				if (Application.OleRequired() != ApartmentState.STA)
				{
					throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
				}
				if (value != AutoCompleteSource.None && value != AutoCompleteSource.CustomSource && value != AutoCompleteSource.ListItems)
				{
					new FileIOPermission(PermissionState.Unrestricted)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Demand();
				}
				this.autoCompleteSource = value;
				this.SetAutoComplete(false, true);
			}
		}

		/// <summary>Gets or sets a custom <see cref="T:System.Collections.Specialized.StringCollection" /> to use when the <see cref="P:System.Windows.Forms.ComboBox.AutoCompleteSource" /> property is set to <see langword="CustomSource" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.StringCollection" /> to use with <see cref="P:System.Windows.Forms.ComboBox.AutoCompleteSource" />.</returns>
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0001FCD3 File Offset: 0x0001DED3
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x0001FD08 File Offset: 0x0001DF08
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ComboBoxAutoCompleteCustomSourceDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				if (this.autoCompleteCustomSource == null)
				{
					this.autoCompleteCustomSource = new AutoCompleteStringCollection();
					this.autoCompleteCustomSource.CollectionChanged += this.OnAutoCompleteCustomSourceChanged;
				}
				return this.autoCompleteCustomSource;
			}
			set
			{
				if (this.autoCompleteCustomSource != value)
				{
					if (this.autoCompleteCustomSource != null)
					{
						this.autoCompleteCustomSource.CollectionChanged -= this.OnAutoCompleteCustomSourceChanged;
					}
					this.autoCompleteCustomSource = value;
					if (this.autoCompleteCustomSource != null)
					{
						this.autoCompleteCustomSource.CollectionChanged += this.OnAutoCompleteCustomSourceChanged;
					}
					this.SetAutoComplete(false, true);
				}
			}
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A color object that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0001FD6B File Offset: 0x0001DF6B
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x00011FB9 File Offset: 0x000101B9
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
		/// <returns>The background image displayed in the control.</returns>
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>Gets or sets the background image layout as defined in the <see cref="T:System.Windows.Forms.ImageLayout" /> enumeration.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout" /> (<see langword="Center" />, <see langword="None" />, <see langword="Stretch" />, <see langword="Tile" />, or <see langword="Zoom" />).</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the values of <see cref="T:System.Windows.Forms.ImageLayout" />. </exception>
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ComboBox.BackgroundImage" /> property changes.</summary>
		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06000A9C RID: 2716 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06000A9D RID: 2717 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ComboBox.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06000A9E RID: 2718 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x06000A9F RID: 2719 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0001FDA5 File Offset: 0x0001DFA5
		internal ComboBox.ChildAccessibleObject ChildEditAccessibleObject
		{
			get
			{
				if (this.childEditAccessibleObject == null)
				{
					this.childEditAccessibleObject = new ComboBox.ComboBoxChildEditUiaProvider(this, this.childEdit.Handle);
				}
				return this.childEditAccessibleObject;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0001FDCC File Offset: 0x0001DFCC
		internal ComboBox.ChildAccessibleObject ChildListAccessibleObject
		{
			get
			{
				if (this.childListAccessibleObject == null)
				{
					this.childListAccessibleObject = new ComboBox.ComboBoxChildListUiaProvider(this, (this.DropDownStyle == ComboBoxStyle.Simple) ? this.childListBox.Handle : this.dropDownHandle);
				}
				return this.childListAccessibleObject;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0001FE03 File Offset: 0x0001E003
		internal AccessibleObject ChildTextAccessibleObject
		{
			get
			{
				if (this.childTextAccessibleObject == null)
				{
					this.childTextAccessibleObject = new ComboBox.ComboBoxChildTextUiaProvider(this);
				}
				return this.childTextAccessibleObject;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0001FE20 File Offset: 0x0001E020
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "COMBOBOX";
				createParams.Style |= 2097728;
				createParams.ExStyle |= 512;
				if (!this.integralHeight)
				{
					createParams.Style |= 1024;
				}
				switch (this.DropDownStyle)
				{
				case ComboBoxStyle.Simple:
					createParams.Style |= 1;
					break;
				case ComboBoxStyle.DropDown:
					createParams.Style |= 2;
					createParams.Height = this.PreferredHeight;
					break;
				case ComboBoxStyle.DropDownList:
					createParams.Style |= 3;
					createParams.Height = this.PreferredHeight;
					break;
				}
				DrawMode drawMode = this.DrawMode;
				if (drawMode != DrawMode.OwnerDrawFixed)
				{
					if (drawMode == DrawMode.OwnerDrawVariable)
					{
						createParams.Style |= 32;
					}
				}
				else
				{
					createParams.Style |= 16;
				}
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default size of the control.</returns>
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0001FF10 File Offset: 0x0001E110
		protected override Size DefaultSize
		{
			get
			{
				return new Size(121, this.PreferredHeight);
			}
		}

		/// <summary>Gets or sets the data source for this <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>An object that implements the <see cref="T:System.Collections.IList" /> interface or an <see cref="T:System.Array" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0001D967 File Offset: 0x0001BB67
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x0001D96F File Offset: 0x0001BB6F
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		[SRDescription("ListControlDataSourceDescr")]
		public new object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether your code or the operating system will handle drawing of elements in the list.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DrawMode" /> enumeration values. The default is <see cref="F:System.Windows.Forms.DrawMode.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value is not a valid <see cref="T:System.Windows.Forms.DrawMode" /> enumeration value. </exception>
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0001FF20 File Offset: 0x0001E120
		// (set) Token: 0x06000AA8 RID: 2728 RVA: 0x0001FF48 File Offset: 0x0001E148
		[SRCategory("CatBehavior")]
		[DefaultValue(DrawMode.Normal)]
		[SRDescription("ComboBoxDrawModeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public DrawMode DrawMode
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(ComboBox.PropDrawMode, out flag);
				if (flag)
				{
					return (DrawMode)integer;
				}
				return DrawMode.Normal;
			}
			set
			{
				if (this.DrawMode != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(DrawMode));
					}
					this.ResetHeightCache();
					base.Properties.SetInteger(ComboBox.PropDrawMode, (int)value);
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets the width of the of the drop-down portion of a combo box.</summary>
		/// <returns>The width, in pixels, of the drop-down box.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value is less than one. </exception>
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0001FFA4 File Offset: 0x0001E1A4
		// (set) Token: 0x06000AAA RID: 2730 RVA: 0x0001FFD0 File Offset: 0x0001E1D0
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxDropDownWidthDescr")]
		public int DropDownWidth
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(ComboBox.PropDropDownWidth, out flag);
				if (flag)
				{
					return integer;
				}
				return base.Width;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("DropDownWidth", SR.GetString("InvalidArgument", new object[]
					{
						"DropDownWidth",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (base.Properties.GetInteger(ComboBox.PropDropDownWidth) != value)
				{
					base.Properties.SetInteger(ComboBox.PropDropDownWidth, value);
					if (base.IsHandleCreated)
					{
						base.SendMessage(352, value, 0);
					}
				}
			}
		}

		/// <summary>Gets or sets the height in pixels of the drop-down portion of the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>The height, in pixels, of the drop-down box.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value is less than one. </exception>
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x00020050 File Offset: 0x0001E250
		// (set) Token: 0x06000AAC RID: 2732 RVA: 0x00020078 File Offset: 0x0001E278
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxDropDownHeightDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(106)]
		public int DropDownHeight
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(ComboBox.PropDropDownHeight, out flag);
				if (flag)
				{
					return integer;
				}
				return 106;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("DropDownHeight", SR.GetString("InvalidArgument", new object[]
					{
						"DropDownHeight",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (base.Properties.GetInteger(ComboBox.PropDropDownHeight) != value)
				{
					base.Properties.SetInteger(ComboBox.PropDropDownHeight, value);
					this.IntegralHeight = false;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the combo box is displaying its drop-down portion.</summary>
		/// <returns>
		///     <see langword="true" /> if the drop-down portion is displayed; otherwise, <see langword="false" />. The default is false.</returns>
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x000200E6 File Offset: 0x0001E2E6
		// (set) Token: 0x06000AAE RID: 2734 RVA: 0x00020108 File Offset: 0x0001E308
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxDroppedDownDescr")]
		public bool DroppedDown
		{
			get
			{
				return base.IsHandleCreated && (int)((long)base.SendMessage(343, 0, 0)) != 0;
			}
			set
			{
				if (!base.IsHandleCreated)
				{
					this.CreateHandle();
				}
				base.SendMessage(335, value ? -1 : 0, 0);
			}
		}

		/// <summary>Gets or sets the appearance of the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>One of the enumeration values that specifies the appearance of the control. The options are <see langword="Flat" />, <see langword="Popup" />, <see langword="Standard" />, and <see langword="System" />. The default is <see langword="Standard" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the values of <see cref="T:System.Windows.Forms.FlatStyle" />. </exception>
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0002012C File Offset: 0x0001E32C
		// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x00020134 File Offset: 0x0001E334
		[SRCategory("CatAppearance")]
		[DefaultValue(FlatStyle.Standard)]
		[Localizable(true)]
		[SRDescription("ComboBoxFlatStyleDescr")]
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
				base.Invalidate();
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ComboBox" /> has focus.</summary>
		/// <returns>
		///     <see langword="true" /> if this control has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0002016C File Offset: 0x0001E36C
		public override bool Focused
		{
			get
			{
				if (base.Focused)
				{
					return true;
				}
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				return focus != IntPtr.Zero && ((this.childEdit != null && focus == this.childEdit.Handle) || (this.childListBox != null && focus == this.childListBox.Handle));
			}
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.</returns>
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x000201D0 File Offset: 0x0001E3D0
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x0001208A File Offset: 0x0001028A
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

		/// <summary>Gets or sets a value indicating whether the control should resize to avoid showing partial items.</summary>
		/// <returns>
		///     <see langword="true" /> if the list portion can contain only complete items; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x000201E6 File Offset: 0x0001E3E6
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x000201EE File Offset: 0x0001E3EE
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ComboBoxIntegralHeightDescr")]
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
				}
			}
		}

		/// <summary>Gets or sets the height of an item in the combo box.</summary>
		/// <returns>The height, in pixels, of an item in the combo box.</returns>
		/// <exception cref="T:System.ArgumentException">The item height value is less than zero. </exception>
		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00020208 File Offset: 0x0001E408
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0002026C File Offset: 0x0001E46C
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ComboBoxItemHeightDescr")]
		public int ItemHeight
		{
			get
			{
				DrawMode drawMode = this.DrawMode;
				if (drawMode == DrawMode.OwnerDrawFixed || drawMode == DrawMode.OwnerDrawVariable || !base.IsHandleCreated)
				{
					bool flag;
					int integer = base.Properties.GetInteger(ComboBox.PropItemHeight, out flag);
					if (flag)
					{
						return integer;
					}
					return base.FontHeight + 2;
				}
				else
				{
					int num = (int)((long)base.SendMessage(340, 0, 0));
					if (num == -1)
					{
						throw new Win32Exception();
					}
					return num;
				}
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("ItemHeight", SR.GetString("InvalidArgument", new object[]
					{
						"ItemHeight",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.ResetHeightCache();
				if (base.Properties.GetInteger(ComboBox.PropItemHeight) != value)
				{
					base.Properties.SetInteger(ComboBox.PropItemHeight, value);
					if (this.DrawMode != DrawMode.Normal)
					{
						this.UpdateItemHeight();
					}
				}
			}
		}

		/// <summary>Gets an object representing the collection of the items contained in this <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ComboBox.ObjectCollection" /> representing the items in the <see cref="T:System.Windows.Forms.ComboBox" />.</returns>
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x000202E7 File Offset: 0x0001E4E7
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ComboBoxItemsDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		public ComboBox.ObjectCollection Items
		{
			get
			{
				if (this.itemsCollection == null)
				{
					this.itemsCollection = new ComboBox.ObjectCollection(this);
				}
				return this.itemsCollection;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00020304 File Offset: 0x0001E504
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00020331 File Offset: 0x0001E531
		private string MatchingText
		{
			get
			{
				string text = (string)base.Properties.GetObject(ComboBox.PropMatchingText);
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(ComboBox.PropMatchingText))
				{
					base.Properties.SetObject(ComboBox.PropMatchingText, value);
				}
			}
		}

		/// <summary>Gets or sets the maximum number of items to be shown in the drop-down portion of the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>The maximum number of items of in the drop-down portion. The minimum for this property is 1 and the maximum is 100.</returns>
		/// <exception cref="T:System.ArgumentException">The maximum number is set less than one or greater than 100. </exception>
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00020359 File Offset: 0x0001E559
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x00020364 File Offset: 0x0001E564
		[SRCategory("CatBehavior")]
		[DefaultValue(8)]
		[Localizable(true)]
		[SRDescription("ComboBoxMaxDropDownItemsDescr")]
		public int MaxDropDownItems
		{
			get
			{
				return (int)this.maxDropDownItems;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("MaxDropDownItems", SR.GetString("InvalidBoundArgument", new object[]
					{
						"MaxDropDownItems",
						value.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture),
						100.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.maxDropDownItems = (short)value;
			}
		}

		/// <summary>Gets or sets the size that is the upper limit that the <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> method can specify.</summary>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x000203D7 File Offset: 0x0001E5D7
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x000203DF File Offset: 0x0001E5DF
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

		/// <summary>Gets or sets the size that is the lower limit that the <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)" /> method can specify.</summary>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000203F4 File Offset: 0x0001E5F4
		// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x000203FC File Offset: 0x0001E5FC
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

		/// <summary>Gets or sets the number of characters a user can type into the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>The maximum number of characters a user can enter. Values of less than zero are reset to zero, which is the default value.</returns>
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00020411 File Offset: 0x0001E611
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x00020423 File Offset: 0x0001E623
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Localizable(true)]
		[SRDescription("ComboBoxMaxLengthDescr")]
		public int MaxLength
		{
			get
			{
				return base.Properties.GetInteger(ComboBox.PropMaxLength);
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (this.MaxLength != value)
				{
					base.Properties.SetInteger(ComboBox.PropMaxLength, value);
					if (base.IsHandleCreated)
					{
						base.SendMessage(321, value, 0);
					}
				}
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0002045C File Offset: 0x0001E65C
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x00020464 File Offset: 0x0001E664
		internal bool MouseIsOver
		{
			get
			{
				return this.mouseOver;
			}
			set
			{
				if (this.mouseOver != value)
				{
					this.mouseOver = value;
					if ((!base.ContainsFocus || !Application.RenderWithVisualStyles) && this.FlatStyle == FlatStyle.Popup)
					{
						base.Invalidate();
						base.Update();
					}
				}
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value.</returns>
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x000204A2 File Offset: 0x0001E6A2
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

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06000AC7 RID: 2759 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x06000AC8 RID: 2760 RVA: 0x000204B4 File Offset: 0x0001E6B4
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

		/// <summary>Gets the preferred height of the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>The preferred height, in pixels, of the item area of the combo box.</returns>
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x000204C0 File Offset: 0x0001E6C0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxPreferredHeightDescr")]
		public int PreferredHeight
		{
			get
			{
				if (!base.FormattingEnabled)
				{
					this.prefHeightCache = (short)(TextRenderer.MeasureText(LayoutUtils.TestString, this.Font, new Size(32767, (int)((double)base.FontHeight * 1.25)), TextFormatFlags.SingleLine).Height + SystemInformation.BorderSize.Height * 8 + this.Padding.Size.Height);
					return (int)this.prefHeightCache;
				}
				if (this.prefHeightCache < 0)
				{
					Size size = TextRenderer.MeasureText(LayoutUtils.TestString, this.Font, new Size(32767, (int)((double)base.FontHeight * 1.25)), TextFormatFlags.SingleLine);
					if (this.DropDownStyle == ComboBoxStyle.Simple)
					{
						int num = this.Items.Count + 1;
						this.prefHeightCache = (short)(size.Height * num + SystemInformation.BorderSize.Height * 16 + this.Padding.Size.Height);
					}
					else
					{
						this.prefHeightCache = (short)this.GetComboHeight();
					}
				}
				return (int)this.prefHeightCache;
			}
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x000205E0 File Offset: 0x0001E7E0
		private int GetComboHeight()
		{
			Size size = Size.Empty;
			using (WindowsFont windowsFont = WindowsFont.FromFont(this.Font))
			{
				size = WindowsGraphicsCacheManager.MeasurementGraphics.GetTextExtent("0", windowsFont);
			}
			int num = size.Height + SystemInformation.Border3DSize.Height;
			if (this.DrawMode != DrawMode.Normal)
			{
				num = this.ItemHeight;
			}
			return 2 * SystemInformation.FixedFrameBorderSize.Height + num;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002066C File Offset: 0x0001E86C
		private string[] GetStringsForAutoComplete(IList collection)
		{
			if (collection is AutoCompleteStringCollection)
			{
				string[] array = new string[this.AutoCompleteCustomSource.Count];
				for (int i = 0; i < this.AutoCompleteCustomSource.Count; i++)
				{
					array[i] = this.AutoCompleteCustomSource[i];
				}
				return array;
			}
			if (collection is ComboBox.ObjectCollection)
			{
				string[] array2 = new string[this.itemsCollection.Count];
				for (int j = 0; j < this.itemsCollection.Count; j++)
				{
					array2[j] = base.GetItemText(this.itemsCollection[j]);
				}
				return array2;
			}
			return new string[0];
		}

		/// <summary>Gets or sets the index specifying the currently selected item.</summary>
		/// <returns>A zero-based index of the currently selected item. A value of negative one (-1) is returned if no item is selected.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified index is less than or equal to -2.-or- The specified index is greater than or equal to the number of items in the combo box. </exception>
		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x00020705 File Offset: 0x0001E905
		// (set) Token: 0x06000ACD RID: 2765 RVA: 0x0002072C File Offset: 0x0001E92C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedIndexDescr")]
		public override int SelectedIndex
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)((long)base.SendMessage(327, 0, 0));
				}
				return this.selectedIndex;
			}
			set
			{
				if (this.SelectedIndex != value)
				{
					int num = 0;
					if (this.itemsCollection != null)
					{
						num = this.itemsCollection.Count;
					}
					if (value < -1 || value >= num)
					{
						throw new ArgumentOutOfRangeException("SelectedIndex", SR.GetString("InvalidArgument", new object[]
						{
							"SelectedIndex",
							value.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(334, value, 0);
					}
					else
					{
						this.selectedIndex = value;
					}
					this.UpdateText();
					if (base.IsHandleCreated)
					{
						this.OnTextChanged(EventArgs.Empty);
					}
					this.OnSelectedItemChanged(EventArgs.Empty);
					this.OnSelectedIndexChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets currently selected item in the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>The object that is the currently selected item or <see langword="null" /> if there is no currently selected item.</returns>
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x000207E4 File Offset: 0x0001E9E4
		// (set) Token: 0x06000ACF RID: 2767 RVA: 0x0002080C File Offset: 0x0001EA0C
		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedItemDescr")]
		public object SelectedItem
		{
			get
			{
				int num = this.SelectedIndex;
				if (num != -1)
				{
					return this.Items[num];
				}
				return null;
			}
			set
			{
				int num = -1;
				if (this.itemsCollection != null)
				{
					if (value != null)
					{
						num = this.itemsCollection.IndexOf(value);
					}
					else
					{
						this.SelectedIndex = -1;
					}
				}
				if (num != -1)
				{
					this.SelectedIndex = num;
				}
			}
		}

		/// <summary>Gets or sets the text that is selected in the editable portion of a <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <returns>A string that represents the currently selected text in the combo box. If <see cref="P:System.Windows.Forms.ComboBox.DropDownStyle" /> is set to <see cref="F:System.Windows.Forms.ComboBoxStyle.DropDownList" />, the return value is an empty string ("").</returns>
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x00020847 File Offset: 0x0001EA47
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x00020870 File Offset: 0x0001EA70
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectedTextDescr")]
		public string SelectedText
		{
			get
			{
				if (this.DropDownStyle == ComboBoxStyle.DropDownList)
				{
					return "";
				}
				return this.Text.Substring(this.SelectionStart, this.SelectionLength);
			}
			set
			{
				if (this.DropDownStyle != ComboBoxStyle.DropDownList)
				{
					string lParam = (value == null) ? "" : value;
					base.CreateControl();
					if (base.IsHandleCreated && this.childEdit != null)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.childEdit.Handle), 194, NativeMethods.InvalidIntPtr, lParam);
					}
				}
			}
		}

		/// <summary>Gets or sets the number of characters selected in the editable portion of the combo box.</summary>
		/// <returns>The number of characters selected in the combo box.</returns>
		/// <exception cref="T:System.ArgumentException">The value was less than zero. </exception>
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x000208CC File Offset: 0x0001EACC
		// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x00020907 File Offset: 0x0001EB07
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectionLengthDescr")]
		public int SelectionLength
		{
			get
			{
				int[] array = new int[1];
				int[] array2 = new int[1];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 320, array2, array);
				return array[0] - array2[0];
			}
			set
			{
				this.Select(this.SelectionStart, value);
			}
		}

		/// <summary>Gets or sets the starting index of text selected in the combo box.</summary>
		/// <returns>The zero-based index of the first character in the string of the current text selection.</returns>
		/// <exception cref="T:System.ArgumentException">The value is less than zero. </exception>
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00020918 File Offset: 0x0001EB18
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x00020948 File Offset: 0x0001EB48
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ComboBoxSelectionStartDescr")]
		public int SelectionStart
		{
			get
			{
				int[] array = new int[1];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 320, array, null);
				return array[0];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectionStart", SR.GetString("InvalidArgument", new object[]
					{
						"SelectionStart",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.Select(value, this.SelectionLength);
			}
		}

		/// <summary>Gets or sets a value indicating whether the items in the combo box are sorted.</summary>
		/// <returns>
		///     <see langword="true" /> if the combo box is sorted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt was made to sort a <see cref="T:System.Windows.Forms.ComboBox" /> that is attached to a data source. </exception>
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00020998 File Offset: 0x0001EB98
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x000209A0 File Offset: 0x0001EBA0
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ComboBoxSortedDescr")]
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
					if (this.DataSource != null && value)
					{
						throw new ArgumentException(SR.GetString("ComboBoxSortWithDataSource"));
					}
					this.sorted = value;
					this.RefreshItems();
					this.SelectedIndex = -1;
				}
			}
		}

		/// <summary>Gets or sets a value specifying the style of the combo box.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ComboBoxStyle" /> values. The default is <see langword="DropDown" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ComboBoxStyle" /> values. </exception>
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x000209DC File Offset: 0x0001EBDC
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x00020A04 File Offset: 0x0001EC04
		[SRCategory("CatAppearance")]
		[DefaultValue(ComboBoxStyle.DropDown)]
		[SRDescription("ComboBoxStyleDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public ComboBoxStyle DropDownStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(ComboBox.PropStyle, out flag);
				if (flag)
				{
					return (ComboBoxStyle)integer;
				}
				return ComboBoxStyle.DropDown;
			}
			set
			{
				if (this.DropDownStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ComboBoxStyle));
					}
					if (value == ComboBoxStyle.DropDownList && this.AutoCompleteSource != AutoCompleteSource.ListItems && this.AutoCompleteMode != AutoCompleteMode.None)
					{
						this.AutoCompleteMode = AutoCompleteMode.None;
					}
					this.ResetHeightCache();
					base.Properties.SetInteger(ComboBox.PropStyle, (int)value);
					if (base.IsHandleCreated)
					{
						base.RecreateHandle();
					}
					this.OnDropDownStyleChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x00020A90 File Offset: 0x0001EC90
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x00020AF8 File Offset: 0x0001ECF8
		[Localizable(true)]
		[Bindable(true)]
		public override string Text
		{
			get
			{
				if (this.SelectedItem != null && !base.BindingFieldEmpty)
				{
					if (!base.FormattingEnabled)
					{
						return base.FilterItemOnProperty(this.SelectedItem).ToString();
					}
					string itemText = base.GetItemText(this.SelectedItem);
					if (!string.IsNullOrEmpty(itemText) && string.Compare(itemText, base.Text, true, CultureInfo.CurrentCulture) == 0)
					{
						return itemText;
					}
				}
				return base.Text;
			}
			set
			{
				if (this.DropDownStyle == ComboBoxStyle.DropDownList && !base.IsHandleCreated && !string.IsNullOrEmpty(value) && this.FindStringExact(value) == -1)
				{
					return;
				}
				base.Text = value;
				object selectedItem = this.SelectedItem;
				if (!base.DesignMode)
				{
					if (value == null)
					{
						this.SelectedIndex = -1;
						return;
					}
					if (value != null && (selectedItem == null || string.Compare(value, base.GetItemText(selectedItem), false, CultureInfo.CurrentCulture) != 0))
					{
						int num = this.FindStringIgnoreCase(value);
						if (num != -1)
						{
							this.SelectedIndex = num;
						}
					}
				}
			}
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00020B7C File Offset: 0x0001ED7C
		private int FindStringIgnoreCase(string value)
		{
			int num = this.FindStringExact(value, -1, false);
			if (num == -1)
			{
				num = this.FindStringExact(value, -1, true);
			}
			return num;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00020BA2 File Offset: 0x0001EDA2
		private void NotifyAutoComplete()
		{
			this.NotifyAutoComplete(true);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00020BAC File Offset: 0x0001EDAC
		private void NotifyAutoComplete(bool setSelectedIndex)
		{
			string text = this.Text;
			bool flag = text != this.lastTextChangedValue;
			bool flag2 = false;
			if (setSelectedIndex)
			{
				int num = this.FindStringIgnoreCase(text);
				if (num != -1 && num != this.SelectedIndex)
				{
					this.SelectedIndex = num;
					this.SelectionStart = 0;
					this.SelectionLength = text.Length;
					flag2 = true;
				}
			}
			if (flag && !flag2)
			{
				this.OnTextChanged(EventArgs.Empty);
			}
			this.lastTextChangedValue = text;
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00020C1B File Offset: 0x0001EE1B
		internal override bool SupportsUiaProviders
		{
			get
			{
				return AccessibilityImprovements.Level3 && !base.DesignMode;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00020C2F File Offset: 0x0001EE2F
		private bool SystemAutoCompleteEnabled
		{
			get
			{
				return this.autoCompleteMode != AutoCompleteMode.None && this.DropDownStyle != ComboBoxStyle.DropDownList;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06000AE1 RID: 2785 RVA: 0x0001B6FB File Offset: 0x000198FB
		// (remove) Token: 0x06000AE2 RID: 2786 RVA: 0x0001B704 File Offset: 0x00019904
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

		/// <summary>Occurs when a visual aspect of an owner-drawn <see cref="T:System.Windows.Forms.ComboBox" /> changes.</summary>
		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06000AE3 RID: 2787 RVA: 0x00020C47 File Offset: 0x0001EE47
		// (remove) Token: 0x06000AE4 RID: 2788 RVA: 0x00020C5A File Offset: 0x0001EE5A
		[SRCategory("CatBehavior")]
		[SRDescription("drawItemEventDescr")]
		public event DrawItemEventHandler DrawItem
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_DRAWITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_DRAWITEM, value);
			}
		}

		/// <summary>Occurs when the drop-down portion of a <see cref="T:System.Windows.Forms.ComboBox" /> is shown.</summary>
		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06000AE5 RID: 2789 RVA: 0x00020C6D File Offset: 0x0001EE6D
		// (remove) Token: 0x06000AE6 RID: 2790 RVA: 0x00020C80 File Offset: 0x0001EE80
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnDropDownDescr")]
		public event EventHandler DropDown
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_DROPDOWN, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_DROPDOWN, value);
			}
		}

		/// <summary>Occurs each time an owner-drawn <see cref="T:System.Windows.Forms.ComboBox" /> item needs to be drawn and when the sizes of the list items are determined.</summary>
		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06000AE7 RID: 2791 RVA: 0x00020C93 File Offset: 0x0001EE93
		// (remove) Token: 0x06000AE8 RID: 2792 RVA: 0x00020CAC File Offset: 0x0001EEAC
		[SRCategory("CatBehavior")]
		[SRDescription("measureItemEventDescr")]
		public event MeasureItemEventHandler MeasureItem
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_MEASUREITEM, value);
				this.UpdateItemHeight();
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_MEASUREITEM, value);
				this.UpdateItemHeight();
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ComboBox.SelectedIndex" /> property has changed.</summary>
		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06000AE9 RID: 2793 RVA: 0x00020CC5 File Offset: 0x0001EEC5
		// (remove) Token: 0x06000AEA RID: 2794 RVA: 0x00020CD8 File Offset: 0x0001EED8
		[SRCategory("CatBehavior")]
		[SRDescription("selectedIndexChangedEventDescr")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_SELECTEDINDEXCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_SELECTEDINDEXCHANGED, value);
			}
		}

		/// <summary>Occurs when the user changes the selected item and that change is displayed in the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06000AEB RID: 2795 RVA: 0x00020CEB File Offset: 0x0001EEEB
		// (remove) Token: 0x06000AEC RID: 2796 RVA: 0x00020CFE File Offset: 0x0001EEFE
		[SRCategory("CatBehavior")]
		[SRDescription("selectionChangeCommittedEventDescr")]
		public event EventHandler SelectionChangeCommitted
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_SELECTIONCHANGECOMMITTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_SELECTIONCHANGECOMMITTED, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ComboBox.DropDownStyle" /> property has changed.</summary>
		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06000AED RID: 2797 RVA: 0x00020D11 File Offset: 0x0001EF11
		// (remove) Token: 0x06000AEE RID: 2798 RVA: 0x00020D24 File Offset: 0x0001EF24
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxDropDownStyleChangedDescr")]
		public event EventHandler DropDownStyleChanged
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_DROPDOWNSTYLE, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_DROPDOWNSTYLE, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.ComboBox" /> control is redrawn.</summary>
		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06000AEF RID: 2799 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x06000AF0 RID: 2800 RVA: 0x00020D40 File Offset: 0x0001EF40
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

		/// <summary>Occurs when the control has formatted the text, but before the text is displayed.</summary>
		// Token: 0x14000063 RID: 99
		// (add) Token: 0x06000AF1 RID: 2801 RVA: 0x00020D49 File Offset: 0x0001EF49
		// (remove) Token: 0x06000AF2 RID: 2802 RVA: 0x00020D5C File Offset: 0x0001EF5C
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnTextUpdateDescr")]
		public event EventHandler TextUpdate
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_TEXTUPDATE, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_TEXTUPDATE, value);
			}
		}

		/// <summary>Occurs when the drop-down portion of the <see cref="T:System.Windows.Forms.ComboBox" /> is no longer visible.</summary>
		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06000AF3 RID: 2803 RVA: 0x00020D6F File Offset: 0x0001EF6F
		// (remove) Token: 0x06000AF4 RID: 2804 RVA: 0x00020D82 File Offset: 0x0001EF82
		[SRCategory("CatBehavior")]
		[SRDescription("ComboBoxOnDropDownClosedDescr")]
		public event EventHandler DropDownClosed
		{
			add
			{
				base.Events.AddHandler(ComboBox.EVENT_DROPDOWNCLOSED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ComboBox.EVENT_DROPDOWNCLOSED, value);
			}
		}

		/// <summary>Adds the specified items to the combo box.</summary>
		/// <param name="value">The items to add.</param>
		// Token: 0x06000AF5 RID: 2805 RVA: 0x00020D98 File Offset: 0x0001EF98
		[Obsolete("This method has been deprecated.  There is no replacement.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void AddItemsCore(object[] value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			this.BeginUpdate();
			try
			{
				this.Items.AddRangeInternal(value);
			}
			finally
			{
				this.EndUpdate();
			}
		}

		/// <summary>Maintains performance when items are added to the <see cref="T:System.Windows.Forms.ComboBox" /> one at a time.</summary>
		// Token: 0x06000AF6 RID: 2806 RVA: 0x00020DE0 File Offset: 0x0001EFE0
		public void BeginUpdate()
		{
			this.updateCount++;
			base.BeginUpdateInternal();
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00020DF6 File Offset: 0x0001EFF6
		private void CheckNoDataSource()
		{
			if (this.DataSource != null)
			{
				throw new ArgumentException(SR.GetString("DataSourceLocksItems"));
			}
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new accessibility object for the control.</returns>
		// Token: 0x06000AF8 RID: 2808 RVA: 0x00020E10 File Offset: 0x0001F010
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ComboBox.ComboBoxUiaProvider(this);
			}
			if (AccessibilityImprovements.Level1)
			{
				return new ComboBox.ComboBoxExAccessibleObject(this);
			}
			return new ComboBox.ComboBoxAccessibleObject(this);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00020E34 File Offset: 0x0001F034
		internal bool UpdateNeeded()
		{
			return this.updateCount == 0;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00020E40 File Offset: 0x0001F040
		internal Point EditToComboboxMapping(Message m)
		{
			if (this.childEdit == null)
			{
				return new Point(0, 0);
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
			NativeMethods.RECT rect2 = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.childEdit.Handle), ref rect2);
			int x = NativeMethods.Util.SignedLOWORD(m.LParam) + (rect2.left - rect.left);
			int y = NativeMethods.Util.SignedHIWORD(m.LParam) + (rect2.top - rect.top);
			return new Point(x, y);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00020ED8 File Offset: 0x0001F0D8
		private void ChildWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 48)
			{
				if (msg <= 8)
				{
					if (msg != 7)
					{
						if (msg == 8)
						{
							if (!base.DesignMode)
							{
								base.OnImeContextStatusChanged(m.HWnd);
							}
							this.DefChildWndProc(ref m);
							if (this.fireLostFocus)
							{
								base.InvokeLostFocus(this, EventArgs.Empty);
							}
							if (this.FlatStyle == FlatStyle.Popup)
							{
								base.Invalidate();
								return;
							}
							return;
						}
					}
					else
					{
						if (!base.DesignMode)
						{
							ImeContext.SetImeStatus(base.CachedImeMode, m.HWnd);
						}
						if (!base.HostedInWin32DialogManager)
						{
							IContainerControl containerControlInternal = base.GetContainerControlInternal();
							if (containerControlInternal != null)
							{
								ContainerControl containerControl = containerControlInternal as ContainerControl;
								if (containerControl != null && !containerControl.ActivateControlInternal(this, false))
								{
									return;
								}
							}
						}
						this.DefChildWndProc(ref m);
						if (this.fireSetFocus)
						{
							if (!base.DesignMode && this.childEdit != null && m.HWnd == this.childEdit.Handle && !LocalAppContextSwitches.EnableLegacyIMEFocusInComboBox)
							{
								base.WmImeSetFocus();
							}
							base.InvokeGotFocus(this, EventArgs.Empty);
						}
						if (this.FlatStyle == FlatStyle.Popup)
						{
							base.Invalidate();
							return;
						}
						return;
					}
				}
				else if (msg != 32)
				{
					if (msg == 48)
					{
						this.DefChildWndProc(ref m);
						if (this.childEdit != null && m.HWnd == this.childEdit.Handle)
						{
							UnsafeNativeMethods.SendMessage(new HandleRef(this, this.childEdit.Handle), 211, 3, 0);
							return;
						}
						return;
					}
				}
				else
				{
					if (this.Cursor != this.DefaultCursor && this.childEdit != null && m.HWnd == this.childEdit.Handle && NativeMethods.Util.LOWORD(m.LParam) == 1)
					{
						Cursor.CurrentInternal = this.Cursor;
						return;
					}
					this.DefChildWndProc(ref m);
					return;
				}
			}
			else if (msg <= 123)
			{
				if (msg == 81)
				{
					this.DefChildWndProc(ref m);
					return;
				}
				if (msg == 123)
				{
					if (this.ContextMenu != null || this.ContextMenuStrip != null)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 123, m.WParam, m.LParam);
						return;
					}
					this.DefChildWndProc(ref m);
					return;
				}
			}
			else
			{
				switch (msg)
				{
				case 256:
				case 260:
					if (this.SystemAutoCompleteEnabled && !ComboBox.ACNativeWindow.AutoCompleteActive)
					{
						this.finder.FindDropDowns(false);
					}
					if (this.AutoCompleteMode != AutoCompleteMode.None)
					{
						char c = (char)((long)m.WParam);
						if (c == '\u001b')
						{
							this.DroppedDown = false;
						}
						else if (c == '\r' && this.DroppedDown)
						{
							this.UpdateText();
							this.OnSelectionChangeCommittedInternal(EventArgs.Empty);
							this.DroppedDown = false;
						}
					}
					if (this.DropDownStyle == ComboBoxStyle.Simple && m.HWnd == this.childListBox.Handle)
					{
						this.DefChildWndProc(ref m);
						return;
					}
					if (base.PreProcessControlMessage(ref m) == PreProcessControlState.MessageProcessed)
					{
						return;
					}
					if (this.ProcessKeyMessage(ref m))
					{
						return;
					}
					this.DefChildWndProc(ref m);
					return;
				case 257:
				case 261:
					if (this.DropDownStyle == ComboBoxStyle.Simple && m.HWnd == this.childListBox.Handle)
					{
						this.DefChildWndProc(ref m);
					}
					else if (base.PreProcessControlMessage(ref m) != PreProcessControlState.MessageProcessed)
					{
						if (this.ProcessKeyMessage(ref m))
						{
							return;
						}
						this.DefChildWndProc(ref m);
					}
					if (this.SystemAutoCompleteEnabled && !ComboBox.ACNativeWindow.AutoCompleteActive)
					{
						this.finder.FindDropDowns();
						return;
					}
					return;
				case 258:
					if (this.DropDownStyle == ComboBoxStyle.Simple && m.HWnd == this.childListBox.Handle)
					{
						this.DefChildWndProc(ref m);
						return;
					}
					if (base.PreProcessControlMessage(ref m) == PreProcessControlState.MessageProcessed)
					{
						return;
					}
					if (this.ProcessKeyMessage(ref m))
					{
						return;
					}
					this.DefChildWndProc(ref m);
					return;
				case 259:
					break;
				case 262:
					if (this.DropDownStyle == ComboBoxStyle.Simple && m.HWnd == this.childListBox.Handle)
					{
						this.DefChildWndProc(ref m);
						return;
					}
					if (base.PreProcessControlMessage(ref m) == PreProcessControlState.MessageProcessed)
					{
						return;
					}
					if (this.ProcessKeyEventArgs(ref m))
					{
						return;
					}
					this.DefChildWndProc(ref m);
					return;
				default:
					switch (msg)
					{
					case 512:
					{
						Point point = this.EditToComboboxMapping(m);
						this.DefChildWndProc(ref m);
						this.OnMouseEnterInternal(EventArgs.Empty);
						this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0));
						return;
					}
					case 513:
					{
						this.mousePressed = true;
						this.mouseEvents = true;
						base.CaptureInternal = true;
						this.DefChildWndProc(ref m);
						Point point2 = this.EditToComboboxMapping(m);
						this.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, point2.X, point2.Y, 0));
						return;
					}
					case 514:
					{
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
						Rectangle rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
						int x = NativeMethods.Util.SignedLOWORD(m.LParam);
						int y = NativeMethods.Util.SignedHIWORD(m.LParam);
						Point point3 = new Point(x, y);
						point3 = base.PointToScreen(point3);
						if (this.mouseEvents && !base.ValidationCancelled)
						{
							this.mouseEvents = false;
							if (this.mousePressed)
							{
								if (rectangle.Contains(point3))
								{
									this.mousePressed = false;
									this.OnClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
									this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								}
								else
								{
									this.mousePressed = false;
									this.mouseInEdit = false;
									this.OnMouseLeave(EventArgs.Empty);
								}
							}
						}
						this.DefChildWndProc(ref m);
						base.CaptureInternal = false;
						point3 = this.EditToComboboxMapping(m);
						this.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, point3.X, point3.Y, 0));
						return;
					}
					case 515:
					{
						this.mousePressed = true;
						this.mouseEvents = true;
						base.CaptureInternal = true;
						this.DefChildWndProc(ref m);
						Point point4 = this.EditToComboboxMapping(m);
						this.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, point4.X, point4.Y, 0));
						return;
					}
					case 516:
					{
						this.mousePressed = true;
						this.mouseEvents = true;
						if (this.ContextMenu != null || this.ContextMenuStrip != null)
						{
							base.CaptureInternal = true;
						}
						this.DefChildWndProc(ref m);
						Point point5 = this.EditToComboboxMapping(m);
						this.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, point5.X, point5.Y, 0));
						return;
					}
					case 517:
					{
						this.mousePressed = false;
						this.mouseEvents = false;
						if (this.ContextMenu != null)
						{
							base.CaptureInternal = false;
						}
						this.DefChildWndProc(ref m);
						Point point6 = this.EditToComboboxMapping(m);
						this.OnMouseUp(new MouseEventArgs(MouseButtons.Right, 1, point6.X, point6.Y, 0));
						return;
					}
					case 518:
					{
						this.mousePressed = true;
						this.mouseEvents = true;
						base.CaptureInternal = true;
						this.DefChildWndProc(ref m);
						Point point7 = this.EditToComboboxMapping(m);
						this.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, point7.X, point7.Y, 0));
						return;
					}
					case 519:
					{
						this.mousePressed = true;
						this.mouseEvents = true;
						base.CaptureInternal = true;
						this.DefChildWndProc(ref m);
						Point point8 = this.EditToComboboxMapping(m);
						this.OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 1, point8.X, point8.Y, 0));
						return;
					}
					case 520:
						this.mousePressed = false;
						this.mouseEvents = false;
						base.CaptureInternal = false;
						this.DefChildWndProc(ref m);
						this.OnMouseUp(new MouseEventArgs(MouseButtons.Middle, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						return;
					case 521:
					{
						this.mousePressed = true;
						this.mouseEvents = true;
						base.CaptureInternal = true;
						this.DefChildWndProc(ref m);
						Point point9 = this.EditToComboboxMapping(m);
						this.OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 1, point9.X, point9.Y, 0));
						return;
					}
					default:
						if (msg == 675)
						{
							this.DefChildWndProc(ref m);
							this.OnMouseLeaveInternal(EventArgs.Empty);
							return;
						}
						break;
					}
					break;
				}
			}
			this.DefChildWndProc(ref m);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0002172B File Offset: 0x0001F92B
		private void OnMouseEnterInternal(EventArgs args)
		{
			if (!this.mouseInEdit)
			{
				this.OnMouseEnter(args);
				this.mouseInEdit = true;
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00021744 File Offset: 0x0001F944
		private void OnMouseLeaveInternal(EventArgs args)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
			Rectangle rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
			Point mousePosition = Control.MousePosition;
			if (!rectangle.Contains(mousePosition))
			{
				this.OnMouseLeave(args);
				this.mouseInEdit = false;
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000217B8 File Offset: 0x0001F9B8
		private void DefChildWndProc(ref Message m)
		{
			if (this.childEdit != null)
			{
				NativeWindow nativeWindow;
				if (m.HWnd == this.childEdit.Handle)
				{
					nativeWindow = this.childEdit;
				}
				else if (AccessibilityImprovements.Level3 && m.HWnd == this.dropDownHandle)
				{
					nativeWindow = this.childDropDown;
				}
				else
				{
					nativeWindow = this.childListBox;
				}
				if (nativeWindow != null)
				{
					nativeWindow.DefWndProc(ref m);
				}
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ComboBox" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06000AFF RID: 2815 RVA: 0x00021824 File Offset: 0x0001FA24
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.autoCompleteCustomSource != null)
				{
					this.autoCompleteCustomSource.CollectionChanged -= this.OnAutoCompleteCustomSourceChanged;
				}
				if (this.stringSource != null)
				{
					this.stringSource.ReleaseAutoComplete();
					this.stringSource = null;
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>Resumes painting the <see cref="T:System.Windows.Forms.ComboBox" /> control after painting is suspended by the <see cref="M:System.Windows.Forms.ComboBox.BeginUpdate" /> method.</summary>
		// Token: 0x06000B00 RID: 2816 RVA: 0x00021874 File Offset: 0x0001FA74
		public void EndUpdate()
		{
			this.updateCount--;
			if (this.updateCount == 0 && this.AutoCompleteSource == AutoCompleteSource.ListItems)
			{
				this.SetAutoComplete(false, false);
			}
			if (base.EndUpdateInternal())
			{
				if (this.childEdit != null && this.childEdit.Handle != IntPtr.Zero)
				{
					SafeNativeMethods.InvalidateRect(new HandleRef(this, this.childEdit.Handle), null, false);
				}
				if (this.childListBox != null && this.childListBox.Handle != IntPtr.Zero)
				{
					SafeNativeMethods.InvalidateRect(new HandleRef(this, this.childListBox.Handle), null, false);
				}
			}
		}

		/// <summary>Returns the index of the first item in the <see cref="T:System.Windows.Forms.ComboBox" /> that starts with the specified string.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to search for. </param>
		/// <returns>The zero-based index of the first item found; returns -1 if no match is found.</returns>
		// Token: 0x06000B01 RID: 2817 RVA: 0x00021924 File Offset: 0x0001FB24
		public int FindString(string s)
		{
			return this.FindString(s, -1);
		}

		/// <summary>Returns the index of the first item in the <see cref="T:System.Windows.Forms.ComboBox" /> beyond the specified index that contains the specified string. The search is not case sensitive.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to search for. </param>
		/// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control. </param>
		/// <returns>The zero-based index of the first item found; returns -1 if no match is found, or 0 if the <paramref name="s" /> parameter specifies <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="startIndex" /> is less than -1.-or- The <paramref name="startIndex" /> is greater than the last index in the collection. </exception>
		// Token: 0x06000B02 RID: 2818 RVA: 0x00021930 File Offset: 0x0001FB30
		public int FindString(string s, int startIndex)
		{
			if (s == null)
			{
				return -1;
			}
			if (this.itemsCollection == null || this.itemsCollection.Count == 0)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= this.itemsCollection.Count)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return base.FindStringInternal(s, this.Items, startIndex, false);
		}

		/// <summary>Finds the first item in the combo box that matches the specified string.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to search for. </param>
		/// <returns>The zero-based index of the first item found; returns -1 if no match is found, or 0 if the <paramref name="s" /> parameter specifies <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x06000B03 RID: 2819 RVA: 0x00021985 File Offset: 0x0001FB85
		public int FindStringExact(string s)
		{
			return this.FindStringExact(s, -1, true);
		}

		/// <summary>Finds the first item after the specified index that matches the specified string.</summary>
		/// <param name="s">The <see cref="T:System.String" /> to search for. </param>
		/// <param name="startIndex">The zero-based index of the item before the first item to be searched. Set to -1 to search from the beginning of the control. </param>
		/// <returns>The zero-based index of the first item found; returns -1 if no match is found, or 0 if the <paramref name="s" /> parameter specifies <see cref="F:System.String.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="startIndex" /> is less than -1.-or- The <paramref name="startIndex" /> is equal to the last index in the collection. </exception>
		// Token: 0x06000B04 RID: 2820 RVA: 0x00021990 File Offset: 0x0001FB90
		public int FindStringExact(string s, int startIndex)
		{
			return this.FindStringExact(s, startIndex, true);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0002199C File Offset: 0x0001FB9C
		internal int FindStringExact(string s, int startIndex, bool ignorecase)
		{
			if (s == null)
			{
				return -1;
			}
			if (this.itemsCollection == null || this.itemsCollection.Count == 0)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= this.itemsCollection.Count)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return base.FindStringInternal(s, this.Items, startIndex, true, ignorecase);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x000219F2 File Offset: 0x0001FBF2
		internal override Rectangle ApplyBoundsConstraints(int suggestedX, int suggestedY, int proposedWidth, int proposedHeight)
		{
			if (this.DropDownStyle == ComboBoxStyle.DropDown || this.DropDownStyle == ComboBoxStyle.DropDownList)
			{
				proposedHeight = this.PreferredHeight;
			}
			return base.ApplyBoundsConstraints(suggestedX, suggestedY, proposedWidth, proposedHeight);
		}

		/// <summary>Scales a control's location, size, padding and margin.</summary>
		/// <param name="factor">The factor by which the height and width of the control will be scaled.</param>
		/// <param name="specified">A  value that specifies the bounds of the control to use when defining its size and position.</param>
		// Token: 0x06000B07 RID: 2823 RVA: 0x00021A19 File Offset: 0x0001FC19
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			if (factor.Width != 1f && factor.Height != 1f)
			{
				this.ResetHeightCache();
			}
			base.ScaleControl(factor, specified);
		}

		/// <summary>Returns the height of an item in the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <param name="index">The index of the item to return the height of. </param>
		/// <returns>The height, in pixels, of the item at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> is less than zero.-or- The <paramref name="index" /> is greater than count of items in the list. </exception>
		// Token: 0x06000B08 RID: 2824 RVA: 0x00021A48 File Offset: 0x0001FC48
		public int GetItemHeight(int index)
		{
			if (this.DrawMode != DrawMode.OwnerDrawVariable)
			{
				return this.ItemHeight;
			}
			if (index < 0 || this.itemsCollection == null || index >= this.itemsCollection.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!base.IsHandleCreated)
			{
				return this.ItemHeight;
			}
			int num = (int)((long)base.SendMessage(340, index, 0));
			if (num == -1)
			{
				throw new Win32Exception();
			}
			return num;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00021ADF File Offset: 0x0001FCDF
		internal IntPtr GetListHandle()
		{
			if (this.DropDownStyle != ComboBoxStyle.Simple)
			{
				return this.dropDownHandle;
			}
			return this.childListBox.Handle;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00021AFB File Offset: 0x0001FCFB
		internal NativeWindow GetListNativeWindow()
		{
			if (this.DropDownStyle != ComboBoxStyle.Simple)
			{
				return this.childDropDown;
			}
			return this.childListBox;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00021B14 File Offset: 0x0001FD14
		internal int GetListNativeWindowRuntimeIdPart()
		{
			NativeWindow listNativeWindow = this.GetListNativeWindow();
			if (listNativeWindow == null)
			{
				return 0;
			}
			return listNativeWindow.GetHashCode();
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00021B34 File Offset: 0x0001FD34
		internal override IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			if (msg == 312 && !this.ShouldSerializeBackColor())
			{
				return IntPtr.Zero;
			}
			if (msg == 308 && base.GetStyle(ControlStyles.UserPaint))
			{
				SafeNativeMethods.SetTextColor(new HandleRef(null, dc), ColorTranslator.ToWin32(this.ForeColor));
				SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(this.BackColor));
				return base.BackColorBrush;
			}
			return base.InitializeDCForWmCtlColor(dc, msg);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00021BA8 File Offset: 0x0001FDA8
		private bool InterceptAutoCompleteKeystroke(Message m)
		{
			if (m.Msg == 256)
			{
				if ((int)((long)m.WParam) == 46)
				{
					this.MatchingText = "";
					this.autoCompleteTimeStamp = DateTime.Now.Ticks;
					if (this.Items.Count > 0)
					{
						this.SelectedIndex = 0;
					}
					return false;
				}
			}
			else if (m.Msg == 258)
			{
				char c = (char)((long)m.WParam);
				if (c == '\b')
				{
					if (DateTime.Now.Ticks - this.autoCompleteTimeStamp > 10000000L || this.MatchingText.Length <= 1)
					{
						this.MatchingText = "";
						if (this.Items.Count > 0)
						{
							this.SelectedIndex = 0;
						}
					}
					else
					{
						this.MatchingText = this.MatchingText.Remove(this.MatchingText.Length - 1);
						this.SelectedIndex = this.FindString(this.MatchingText);
					}
					this.autoCompleteTimeStamp = DateTime.Now.Ticks;
					return false;
				}
				if (c == '\u001b')
				{
					this.MatchingText = "";
				}
				if (c != '\u001b' && c != '\r' && !this.DroppedDown && this.AutoCompleteMode != AutoCompleteMode.Append)
				{
					this.DroppedDown = true;
				}
				string text;
				if (DateTime.Now.Ticks - this.autoCompleteTimeStamp > 10000000L)
				{
					text = new string(c, 1);
					if (this.FindString(text) != -1)
					{
						this.MatchingText = text;
					}
					this.autoCompleteTimeStamp = DateTime.Now.Ticks;
					return false;
				}
				text = this.MatchingText + c.ToString();
				int num = this.FindString(text);
				if (num != -1)
				{
					this.MatchingText = text;
					if (num != this.SelectedIndex)
					{
						this.SelectedIndex = num;
					}
				}
				this.autoCompleteTimeStamp = DateTime.Now.Ticks;
				return true;
			}
			return false;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00021D8B File Offset: 0x0001FF8B
		private void InvalidateEverything()
		{
			SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), null, NativeMethods.NullHandleRef, 1157);
		}

		/// <summary>Determines whether the specified key is a regular input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000B0F RID: 2831 RVA: 0x00021DAC File Offset: 0x0001FFAC
		protected override bool IsInputKey(Keys keyData)
		{
			Keys keys = keyData & (Keys.KeyCode | Keys.Alt);
			if (keys == Keys.Return || keys == Keys.Escape)
			{
				if (this.DroppedDown || this.autoCompleteDroppedDown)
				{
					return true;
				}
				if (this.SystemAutoCompleteEnabled && ComboBox.ACNativeWindow.AutoCompleteActive)
				{
					this.autoCompleteDroppedDown = true;
					return true;
				}
			}
			return base.IsInputKey(keyData);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00021DFC File Offset: 0x0001FFFC
		private int NativeAdd(object item)
		{
			int num = (int)((long)base.SendMessage(323, 0, base.GetItemText(item)));
			if (num < 0)
			{
				throw new OutOfMemoryException(SR.GetString("ComboBoxItemOverflow"));
			}
			return num;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00021E38 File Offset: 0x00020038
		private void NativeClear()
		{
			string text = null;
			if (this.DropDownStyle != ComboBoxStyle.DropDownList)
			{
				text = this.WindowText;
			}
			base.SendMessage(331, 0, 0);
			if (text != null)
			{
				this.WindowText = text;
			}
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00021E70 File Offset: 0x00020070
		private string NativeGetItemText(int index)
		{
			int num = (int)((long)base.SendMessage(329, index, 0));
			StringBuilder stringBuilder = new StringBuilder(num + 1);
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 328, index, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00021EBC File Offset: 0x000200BC
		private int NativeInsert(int index, object item)
		{
			int num = (int)((long)base.SendMessage(330, index, base.GetItemText(item)));
			if (num < 0)
			{
				throw new OutOfMemoryException(SR.GetString("ComboBoxItemOverflow"));
			}
			return num;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00021EF8 File Offset: 0x000200F8
		private void NativeRemoveAt(int index)
		{
			if (this.DropDownStyle == ComboBoxStyle.DropDownList && this.SelectedIndex == index)
			{
				base.Invalidate();
			}
			base.SendMessage(324, index, 0);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00021F20 File Offset: 0x00020120
		internal override void RecreateHandleCore()
		{
			string windowText = this.WindowText;
			base.RecreateHandleCore();
			if (!string.IsNullOrEmpty(windowText) && string.IsNullOrEmpty(this.WindowText))
			{
				this.WindowText = windowText;
			}
		}

		/// <summary>Creates a handle for the control.</summary>
		// Token: 0x06000B16 RID: 2838 RVA: 0x00021F58 File Offset: 0x00020158
		protected override void CreateHandle()
		{
			using (new LayoutTransaction(this.ParentInternal, this, PropertyNames.Bounds))
			{
				base.CreateHandle();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000B17 RID: 2839 RVA: 0x00021F9C File Offset: 0x0002019C
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (this.MaxLength > 0)
			{
				base.SendMessage(321, this.MaxLength, 0);
			}
			bool flag = this.childEdit == null && this.childListBox == null;
			if (flag && this.DropDownStyle != ComboBoxStyle.DropDownList)
			{
				IntPtr window = UnsafeNativeMethods.GetWindow(new HandleRef(this, base.Handle), 5);
				if (window != IntPtr.Zero)
				{
					if (this.DropDownStyle == ComboBoxStyle.Simple)
					{
						this.childListBox = new ComboBox.ComboBoxChildNativeWindow(this, ComboBox.ChildWindowType.ListBox);
						this.childListBox.AssignHandle(window);
						window = UnsafeNativeMethods.GetWindow(new HandleRef(this, window), 2);
					}
					this.childEdit = new ComboBox.ComboBoxChildNativeWindow(this, ComboBox.ChildWindowType.Edit);
					this.childEdit.AssignHandle(window);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.childEdit.Handle), 211, 3, 0);
				}
			}
			bool flag2;
			int integer = base.Properties.GetInteger(ComboBox.PropDropDownWidth, out flag2);
			if (flag2)
			{
				base.SendMessage(352, integer, 0);
			}
			flag2 = false;
			int integer2 = base.Properties.GetInteger(ComboBox.PropItemHeight, out flag2);
			if (flag2)
			{
				this.UpdateItemHeight();
			}
			if (this.DropDownStyle == ComboBoxStyle.Simple)
			{
				base.Height = this.requestedHeight;
			}
			try
			{
				this.fromHandleCreate = true;
				this.SetAutoComplete(false, false);
			}
			finally
			{
				this.fromHandleCreate = false;
			}
			if (this.itemsCollection != null)
			{
				foreach (object item in this.itemsCollection)
				{
					this.NativeAdd(item);
				}
				if (this.selectedIndex >= 0)
				{
					base.SendMessage(334, this.selectedIndex, 0);
					this.UpdateText();
					this.selectedIndex = -1;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000B18 RID: 2840 RVA: 0x00022180 File Offset: 0x00020380
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.dropDownHandle = IntPtr.Zero;
			if (base.Disposing)
			{
				this.itemsCollection = null;
				this.selectedIndex = -1;
			}
			else
			{
				this.selectedIndex = this.SelectedIndex;
			}
			if (this.stringSource != null)
			{
				this.stringSource.ReleaseAutoComplete();
				this.stringSource = null;
			}
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.DrawItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06000B19 RID: 2841 RVA: 0x000221E0 File Offset: 0x000203E0
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{
			DrawItemEventHandler drawItemEventHandler = (DrawItemEventHandler)base.Events[ComboBox.EVENT_DRAWITEM];
			if (drawItemEventHandler != null)
			{
				drawItemEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.DropDown" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B1A RID: 2842 RVA: 0x00022210 File Offset: 0x00020410
		protected virtual void OnDropDown(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_DROPDOWN];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (AccessibilityImprovements.Level3 && base.IsHandleCreated)
			{
				base.AccessibilityObject.RaiseAutomationPropertyChangedEvent(30070, UnsafeNativeMethods.ExpandCollapseState.Collapsed, UnsafeNativeMethods.ExpandCollapseState.Expanded);
				ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = base.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
				if (comboBoxUiaProvider != null)
				{
					comboBoxUiaProvider.SetComboBoxItemFocus();
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06000B1B RID: 2843 RVA: 0x00022280 File Offset: 0x00020480
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (this.SystemAutoCompleteEnabled)
			{
				if (e.KeyCode == Keys.Return)
				{
					this.NotifyAutoComplete(true);
				}
				else if (e.KeyCode == Keys.Escape && this.autoCompleteDroppedDown)
				{
					this.NotifyAutoComplete(false);
				}
				this.autoCompleteDroppedDown = false;
			}
			base.OnKeyDown(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data.</param>
		// Token: 0x06000B1C RID: 2844 RVA: 0x000222D0 File Offset: 0x000204D0
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (!e.Handled && (e.KeyChar == '\r' || e.KeyChar == '\u001b') && this.DroppedDown)
			{
				this.dropDown = false;
				if (base.FormattingEnabled)
				{
					this.Text = this.WindowText;
					this.SelectAll();
					e.Handled = false;
					return;
				}
				this.DroppedDown = false;
				e.Handled = true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.MeasureItem" /> event.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.MeasureItemEventArgs" /> that was raised. </param>
		// Token: 0x06000B1D RID: 2845 RVA: 0x00022340 File Offset: 0x00020540
		protected virtual void OnMeasureItem(MeasureItemEventArgs e)
		{
			MeasureItemEventHandler measureItemEventHandler = (MeasureItemEventHandler)base.Events[ComboBox.EVENT_MEASUREITEM];
			if (measureItemEventHandler != null)
			{
				measureItemEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B1E RID: 2846 RVA: 0x0002236E File Offset: 0x0002056E
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.MouseIsOver = true;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B1F RID: 2847 RVA: 0x0002237E File Offset: 0x0002057E
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.MouseIsOver = false;
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00022390 File Offset: 0x00020590
		private void OnSelectionChangeCommittedInternal(EventArgs e)
		{
			if (this.allowCommit)
			{
				try
				{
					this.allowCommit = false;
					this.OnSelectionChangeCommitted(e);
				}
				finally
				{
					this.allowCommit = true;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectionChangeCommitted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B21 RID: 2849 RVA: 0x000223D0 File Offset: 0x000205D0
		protected virtual void OnSelectionChangeCommitted(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_SELECTIONCHANGECOMMITTED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.dropDown)
			{
				this.dropDownWillBeClosed = true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectedIndexChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B22 RID: 2850 RVA: 0x00022410 File Offset: 0x00020610
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_SELECTEDINDEXCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (this.dropDownWillBeClosed)
			{
				this.dropDownWillBeClosed = false;
			}
			else if (AccessibilityImprovements.Level3 && base.IsHandleCreated)
			{
				ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = base.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
				if (comboBoxUiaProvider != null && (this.DropDownStyle == ComboBoxStyle.DropDownList || this.DropDownStyle == ComboBoxStyle.DropDown))
				{
					if (this.dropDown)
					{
						comboBoxUiaProvider.SetComboBoxItemFocus();
					}
					comboBoxUiaProvider.SetComboBoxItemSelection();
				}
			}
			if (base.DataManager != null && base.DataManager.Position != this.SelectedIndex && (!base.FormattingEnabled || this.SelectedIndex != -1))
			{
				base.DataManager.Position = this.SelectedIndex;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.SelectedValueChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B23 RID: 2851 RVA: 0x000224D7 File Offset: 0x000206D7
		protected override void OnSelectedValueChanged(EventArgs e)
		{
			base.OnSelectedValueChanged(e);
			this.selectedValueChangedFired = true;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DomainUpDown.SelectedItemChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B24 RID: 2852 RVA: 0x000224E8 File Offset: 0x000206E8
		protected virtual void OnSelectedItemChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_SELECTEDITEMCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.DropDownStyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B25 RID: 2853 RVA: 0x00022518 File Offset: 0x00020718
		protected virtual void OnDropDownStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_DROPDOWNSTYLE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.  </param>
		// Token: 0x06000B26 RID: 2854 RVA: 0x00022546 File Offset: 0x00020746
		protected override void OnParentBackColorChanged(EventArgs e)
		{
			base.OnParentBackColorChanged(e);
			if (this.DropDownStyle == ComboBoxStyle.Simple)
			{
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06000B27 RID: 2855 RVA: 0x0002255D File Offset: 0x0002075D
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.ResetHeightCache();
			if (this.AutoCompleteMode == AutoCompleteMode.None)
			{
				this.UpdateControl(true);
			}
			else
			{
				base.RecreateHandle();
			}
			CommonProperties.xClearPreferredSizeCache(this);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00022589 File Offset: 0x00020789
		private void OnAutoCompleteCustomSourceChanged(object sender, CollectionChangeEventArgs e)
		{
			if (this.AutoCompleteSource == AutoCompleteSource.CustomSource)
			{
				if (this.AutoCompleteCustomSource.Count == 0)
				{
					this.SetAutoComplete(true, true);
					return;
				}
				this.SetAutoComplete(true, false);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06000B29 RID: 2857 RVA: 0x000225B3 File Offset: 0x000207B3
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.UpdateControl(false);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ForeColorChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06000B2A RID: 2858 RVA: 0x000225C3 File Offset: 0x000207C3
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.UpdateControl(false);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B2B RID: 2859 RVA: 0x000225D3 File Offset: 0x000207D3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnGotFocus(EventArgs e)
		{
			if (!this.canFireLostFocus)
			{
				base.OnGotFocus(e);
				this.canFireLostFocus = true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000B2C RID: 2860 RVA: 0x000225EC File Offset: 0x000207EC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnLostFocus(EventArgs e)
		{
			if (this.canFireLostFocus)
			{
				if (this.AutoCompleteMode != AutoCompleteMode.None && this.AutoCompleteSource == AutoCompleteSource.ListItems && this.DropDownStyle == ComboBoxStyle.DropDownList)
				{
					this.MatchingText = "";
				}
				base.OnLostFocus(e);
				this.canFireLostFocus = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000B2D RID: 2861 RVA: 0x00022638 File Offset: 0x00020838
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnTextChanged(EventArgs e)
		{
			if (this.SystemAutoCompleteEnabled)
			{
				string text = this.Text;
				if (text != this.lastTextChangedValue)
				{
					base.OnTextChanged(e);
					this.lastTextChangedValue = text;
					return;
				}
			}
			else
			{
				base.OnTextChanged(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Validating" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
		// Token: 0x06000B2E RID: 2862 RVA: 0x00022678 File Offset: 0x00020878
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnValidating(CancelEventArgs e)
		{
			if (this.SystemAutoCompleteEnabled)
			{
				this.NotifyAutoComplete();
			}
			base.OnValidating(e);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0002268F File Offset: 0x0002088F
		private void UpdateControl(bool recreate)
		{
			this.ResetHeightCache();
			if (base.IsHandleCreated)
			{
				if (this.DropDownStyle == ComboBoxStyle.Simple && recreate)
				{
					base.RecreateHandle();
					return;
				}
				this.UpdateItemHeight();
				this.InvalidateEverything();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B30 RID: 2864 RVA: 0x000226BF File Offset: 0x000208BF
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.DropDownStyle == ComboBoxStyle.Simple)
			{
				this.InvalidateEverything();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.DataSourceChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06000B31 RID: 2865 RVA: 0x000226D8 File Offset: 0x000208D8
		protected override void OnDataSourceChanged(EventArgs e)
		{
			if (this.Sorted && this.DataSource != null && base.Created)
			{
				this.DataSource = null;
				throw new InvalidOperationException(SR.GetString("ComboBoxDataSourceWithSort"));
			}
			if (this.DataSource == null)
			{
				this.BeginUpdate();
				this.SelectedIndex = -1;
				this.Items.ClearInternal();
				this.EndUpdate();
			}
			if (!this.Sorted && base.Created)
			{
				base.OnDataSourceChanged(e);
			}
			this.RefreshItems();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.DisplayMemberChanged" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06000B32 RID: 2866 RVA: 0x00022757 File Offset: 0x00020957
		protected override void OnDisplayMemberChanged(EventArgs e)
		{
			base.OnDisplayMemberChanged(e);
			this.RefreshItems();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.DropDownClosed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06000B33 RID: 2867 RVA: 0x00022768 File Offset: 0x00020968
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_DROPDOWNCLOSED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
			if (AccessibilityImprovements.Level3 && base.IsHandleCreated)
			{
				if (this.DropDownStyle == ComboBoxStyle.DropDown)
				{
					base.AccessibilityObject.RaiseAutomationEvent(20005);
				}
				base.AccessibilityObject.RaiseAutomationPropertyChangedEvent(30070, UnsafeNativeMethods.ExpandCollapseState.Expanded, UnsafeNativeMethods.ExpandCollapseState.Collapsed);
				this.dropDownWillBeClosed = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.TextUpdate" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06000B34 RID: 2868 RVA: 0x000227E4 File Offset: 0x000209E4
		protected virtual void OnTextUpdate(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ComboBox.EVENT_TEXTUPDATE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Processes a key message and generates the appropriate control events.</summary>
		/// <param name="m">A message object, passed by reference, that represents the window message to process.</param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000B35 RID: 2869 RVA: 0x00022812 File Offset: 0x00020A12
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyEventArgs(ref Message m)
		{
			return (this.AutoCompleteMode != AutoCompleteMode.None && this.AutoCompleteSource == AutoCompleteSource.ListItems && this.DropDownStyle == ComboBoxStyle.DropDownList && this.InterceptAutoCompleteKeystroke(m)) || base.ProcessKeyEventArgs(ref m);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00022849 File Offset: 0x00020A49
		private void ResetHeightCache()
		{
			this.prefHeightCache = -1;
		}

		/// <summary>Refreshes all <see cref="T:System.Windows.Forms.ComboBox" /> items.</summary>
		// Token: 0x06000B37 RID: 2871 RVA: 0x00022854 File Offset: 0x00020A54
		protected override void RefreshItems()
		{
			int num = this.SelectedIndex;
			ComboBox.ObjectCollection objectCollection = this.itemsCollection;
			this.itemsCollection = null;
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
			this.BeginUpdate();
			try
			{
				if (base.IsHandleCreated)
				{
					this.NativeClear();
				}
				if (array != null)
				{
					this.Items.AddRangeInternal(array);
				}
				if (base.DataManager != null)
				{
					this.SelectedIndex = base.DataManager.Position;
				}
				else
				{
					this.SelectedIndex = num;
				}
			}
			finally
			{
				this.EndUpdate();
			}
		}

		/// <summary>Refreshes the item contained at the specified location.</summary>
		/// <param name="index">The location of the item to refresh.</param>
		// Token: 0x06000B38 RID: 2872 RVA: 0x00022934 File Offset: 0x00020B34
		protected override void RefreshItem(int index)
		{
			this.Items.SetItemInternal(index, this.Items[index]);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00022950 File Offset: 0x00020B50
		private void ReleaseChildWindow()
		{
			if (this.childEdit != null)
			{
				this.childEdit.ReleaseHandle();
				this.childEdit = null;
			}
			if (this.childListBox != null)
			{
				if (AccessibilityImprovements.Level3)
				{
					this.ReleaseUiaProvider(this.childListBox.Handle);
				}
				this.childListBox.ReleaseHandle();
				this.childListBox = null;
			}
			if (this.childDropDown != null)
			{
				if (AccessibilityImprovements.Level3)
				{
					this.ReleaseUiaProvider(this.childDropDown.Handle);
				}
				this.childDropDown.ReleaseHandle();
				this.childDropDown = null;
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000229DC File Offset: 0x00020BDC
		internal override void ReleaseUiaProvider(IntPtr handle)
		{
			base.ReleaseUiaProvider(handle);
			ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = base.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
			if (comboBoxUiaProvider != null)
			{
				comboBoxUiaProvider.ResetListItemAccessibleObjects();
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00022A05 File Offset: 0x00020C05
		private void ResetAutoCompleteCustomSource()
		{
			this.AutoCompleteCustomSource = null;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00022A0E File Offset: 0x00020C0E
		private void ResetDropDownWidth()
		{
			base.Properties.RemoveInteger(ComboBox.PropDropDownWidth);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00022A20 File Offset: 0x00020C20
		private void ResetItemHeight()
		{
			base.Properties.RemoveInteger(ComboBox.PropItemHeight);
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.Text" /> property to its default value.</summary>
		// Token: 0x06000B3E RID: 2878 RVA: 0x00022A32 File Offset: 0x00020C32
		public override void ResetText()
		{
			base.ResetText();
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00022A3C File Offset: 0x00020C3C
		private void SetAutoComplete(bool reset, bool recreate)
		{
			if (!base.IsHandleCreated || this.childEdit == null)
			{
				return;
			}
			if (this.AutoCompleteMode != AutoCompleteMode.None)
			{
				if (!this.fromHandleCreate && recreate && base.IsHandleCreated)
				{
					AutoCompleteMode autoCompleteMode = this.AutoCompleteMode;
					this.autoCompleteMode = AutoCompleteMode.None;
					base.RecreateHandle();
					this.autoCompleteMode = autoCompleteMode;
				}
				if (this.AutoCompleteSource == AutoCompleteSource.CustomSource)
				{
					if (this.AutoCompleteCustomSource == null)
					{
						return;
					}
					if (this.AutoCompleteCustomSource.Count == 0)
					{
						int flags = -1610612736;
						SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), flags);
						return;
					}
					if (this.stringSource != null)
					{
						this.stringSource.RefreshList(this.GetStringsForAutoComplete(this.AutoCompleteCustomSource));
						return;
					}
					this.stringSource = new StringSource(this.GetStringsForAutoComplete(this.AutoCompleteCustomSource));
					if (!this.stringSource.Bind(new HandleRef(this, this.childEdit.Handle), (int)this.AutoCompleteMode))
					{
						throw new ArgumentException(SR.GetString("AutoCompleteFailure"));
					}
					return;
				}
				else if (this.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					if (this.DropDownStyle == ComboBoxStyle.DropDownList)
					{
						int flags2 = -1610612736;
						SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), flags2);
						return;
					}
					if (this.itemsCollection == null)
					{
						return;
					}
					if (this.itemsCollection.Count == 0)
					{
						int flags3 = -1610612736;
						SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), flags3);
						return;
					}
					if (this.stringSource != null)
					{
						this.stringSource.RefreshList(this.GetStringsForAutoComplete(this.Items));
						return;
					}
					this.stringSource = new StringSource(this.GetStringsForAutoComplete(this.Items));
					if (!this.stringSource.Bind(new HandleRef(this, this.childEdit.Handle), (int)this.AutoCompleteMode))
					{
						throw new ArgumentException(SR.GetString("AutoCompleteFailureListItems"));
					}
					return;
				}
				else
				{
					try
					{
						int num = 0;
						if (this.AutoCompleteMode == AutoCompleteMode.Suggest)
						{
							num |= -1879048192;
						}
						if (this.AutoCompleteMode == AutoCompleteMode.Append)
						{
							num |= 1610612736;
						}
						if (this.AutoCompleteMode == AutoCompleteMode.SuggestAppend)
						{
							num |= 268435456;
							num |= 1073741824;
						}
						int num2 = SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), (int)(this.AutoCompleteSource | (AutoCompleteSource)num));
						return;
					}
					catch (SecurityException)
					{
						return;
					}
				}
			}
			if (reset)
			{
				int flags4 = -1610612736;
				SafeNativeMethods.SHAutoComplete(new HandleRef(this, this.childEdit.Handle), flags4);
			}
		}

		/// <summary>Selects a range of text in the editable portion of the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <param name="start">The position of the first character in the current text selection within the text box. </param>
		/// <param name="length">The number of characters to select. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="start" /> is less than zero.-or- 
		///         <paramref name="start" /> plus <paramref name="length" /> is less than zero. </exception>
		// Token: 0x06000B40 RID: 2880 RVA: 0x00022CC8 File Offset: 0x00020EC8
		public void Select(int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("InvalidArgument", new object[]
				{
					"start",
					start.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = start + length;
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("length", SR.GetString("InvalidArgument", new object[]
				{
					"length",
					length.ToString(CultureInfo.CurrentCulture)
				}));
			}
			base.SendMessage(322, 0, NativeMethods.Util.MAKELPARAM(start, num));
		}

		/// <summary>Selects all the text in the editable portion of the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		// Token: 0x06000B41 RID: 2881 RVA: 0x00022D59 File Offset: 0x00020F59
		public void SelectAll()
		{
			this.Select(0, int.MaxValue);
		}

		/// <summary>Sets the size and location of the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		/// <param name="x">The horizontal location in pixels of the control. </param>
		/// <param name="y">The vertical location in pixels of the control. </param>
		/// <param name="width">The width in pixels of the control. </param>
		/// <param name="height">The height in pixels of the control. </param>
		/// <param name="specified">One of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values. </param>
		// Token: 0x06000B42 RID: 2882 RVA: 0x00022D67 File Offset: 0x00020F67
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				this.requestedHeight = height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// <summary>When overridden in a derived class, sets the specified array of objects in a collection in the derived class.</summary>
		/// <param name="value">An array of items.</param>
		// Token: 0x06000B43 RID: 2883 RVA: 0x00022D84 File Offset: 0x00020F84
		protected override void SetItemsCore(IList value)
		{
			this.BeginUpdate();
			this.Items.ClearInternal();
			this.Items.AddRangeInternal(value);
			if (base.DataManager != null)
			{
				if (this.DataSource is ICurrencyManagerProvider)
				{
					this.selectedValueChangedFired = false;
				}
				if (base.IsHandleCreated)
				{
					base.SendMessage(334, base.DataManager.Position, 0);
				}
				else
				{
					this.selectedIndex = base.DataManager.Position;
				}
				if (!this.selectedValueChangedFired)
				{
					this.OnSelectedValueChanged(EventArgs.Empty);
					this.selectedValueChangedFired = false;
				}
			}
			this.EndUpdate();
		}

		/// <summary>When overridden in a derived class, sets the object with the specified index in the derived class.</summary>
		/// <param name="index">The array index of the object.</param>
		/// <param name="value">The object.</param>
		// Token: 0x06000B44 RID: 2884 RVA: 0x00022E1D File Offset: 0x0002101D
		protected override void SetItemCore(int index, object value)
		{
			this.Items.SetItemInternal(index, value);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00022E2C File Offset: 0x0002102C
		private bool ShouldSerializeAutoCompleteCustomSource()
		{
			return this.autoCompleteCustomSource != null && this.autoCompleteCustomSource.Count > 0;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00022E46 File Offset: 0x00021046
		internal bool ShouldSerializeDropDownWidth()
		{
			return base.Properties.ContainsInteger(ComboBox.PropDropDownWidth);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00022E58 File Offset: 0x00021058
		internal bool ShouldSerializeItemHeight()
		{
			return base.Properties.ContainsInteger(ComboBox.PropItemHeight);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00022E6A File Offset: 0x0002106A
		internal override bool ShouldSerializeText()
		{
			return this.SelectedIndex == -1 && base.ShouldSerializeText();
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.ComboBox" /> control.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the current <see cref="T:System.Windows.Forms.ComboBox" />. The string includes the type and the number of items in the <see cref="T:System.Windows.Forms.ComboBox" /> control.</returns>
		// Token: 0x06000B49 RID: 2889 RVA: 0x00022E80 File Offset: 0x00021080
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Items.Count: " + ((this.itemsCollection == null) ? 0.ToString(CultureInfo.CurrentCulture) : this.itemsCollection.Count.ToString(CultureInfo.CurrentCulture));
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00022ED0 File Offset: 0x000210D0
		private void UpdateDropDownHeight()
		{
			if (this.dropDownHandle != IntPtr.Zero)
			{
				int num = this.DropDownHeight;
				if (num == 106)
				{
					int val = (this.itemsCollection == null) ? 0 : this.itemsCollection.Count;
					int num2 = Math.Min(Math.Max(val, 1), (int)this.maxDropDownItems);
					num = this.ItemHeight * num2 + 2;
				}
				SafeNativeMethods.SetWindowPos(new HandleRef(this, this.dropDownHandle), NativeMethods.NullHandleRef, 0, 0, this.DropDownWidth, num, 6);
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00022F54 File Offset: 0x00021154
		private void UpdateItemHeight()
		{
			if (!base.IsHandleCreated)
			{
				base.CreateControl();
			}
			if (this.DrawMode == DrawMode.OwnerDrawFixed)
			{
				base.SendMessage(339, -1, this.ItemHeight);
				base.SendMessage(339, 0, this.ItemHeight);
				return;
			}
			if (this.DrawMode == DrawMode.OwnerDrawVariable)
			{
				base.SendMessage(339, -1, this.ItemHeight);
				Graphics graphics = base.CreateGraphicsInternal();
				for (int i = 0; i < this.Items.Count; i++)
				{
					int num = (int)((long)base.SendMessage(340, i, 0));
					MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, i, num);
					this.OnMeasureItem(measureItemEventArgs);
					if (measureItemEventArgs.ItemHeight != num)
					{
						base.SendMessage(339, i, measureItemEventArgs.ItemHeight);
					}
				}
				graphics.Dispose();
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00023020 File Offset: 0x00021220
		private void UpdateText()
		{
			string text = null;
			if (this.SelectedIndex != -1)
			{
				object obj = this.Items[this.SelectedIndex];
				if (obj != null)
				{
					text = base.GetItemText(obj);
				}
			}
			this.Text = text;
			if (this.DropDownStyle == ComboBoxStyle.DropDown && this.childEdit != null && this.childEdit.Handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.childEdit.Handle), 12, IntPtr.Zero, text);
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x000230A4 File Offset: 0x000212A4
		private void WmEraseBkgnd(ref Message m)
		{
			if (this.DropDownStyle == ComboBoxStyle.Simple && this.ParentInternal != null)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				SafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref rect);
				Control parentInternal = this.ParentInternal;
				Graphics graphics = Graphics.FromHdcInternal(m.WParam);
				if (parentInternal != null)
				{
					Brush brush = new SolidBrush(parentInternal.BackColor);
					graphics.FillRectangle(brush, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
					brush.Dispose();
				}
				else
				{
					graphics.FillRectangle(SystemBrushes.Control, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
				}
				graphics.Dispose();
				m.Result = (IntPtr)1;
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00023188 File Offset: 0x00021388
		private void WmParentNotify(ref Message m)
		{
			base.WndProc(ref m);
			if ((int)((long)m.WParam) == 65536001)
			{
				this.dropDownHandle = m.LParam;
				if (AccessibilityImprovements.Level3)
				{
					if (this.childDropDown != null)
					{
						this.ReleaseUiaProvider(this.childDropDown.Handle);
						this.childDropDown.ReleaseHandle();
					}
					this.childDropDown = new ComboBox.ComboBoxChildNativeWindow(this, ComboBox.ChildWindowType.DropDownList);
					this.childDropDown.AssignHandle(this.dropDownHandle);
					this.childListAccessibleObject = null;
				}
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0002320C File Offset: 0x0002140C
		private void WmReflectCommand(ref Message m)
		{
			switch (NativeMethods.Util.HIWORD(m.WParam))
			{
			case 1:
				this.UpdateText();
				this.OnSelectedIndexChanged(EventArgs.Empty);
				return;
			case 2:
			case 3:
			case 4:
				break;
			case 5:
				this.OnTextChanged(EventArgs.Empty);
				return;
			case 6:
				this.OnTextUpdate(EventArgs.Empty);
				return;
			case 7:
				this.currentText = this.Text;
				this.dropDown = true;
				this.OnDropDown(EventArgs.Empty);
				this.UpdateDropDownHeight();
				return;
			case 8:
				this.OnDropDownClosed(EventArgs.Empty);
				if (base.FormattingEnabled && this.Text != this.currentText && this.dropDown)
				{
					this.OnTextChanged(EventArgs.Empty);
				}
				this.dropDown = false;
				return;
			case 9:
				this.OnSelectionChangeCommittedInternal(EventArgs.Empty);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x000232F0 File Offset: 0x000214F0
		private void WmReflectDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			IntPtr intPtr = Control.SetUpPalette(drawitemstruct.hDC, false, false);
			try
			{
				Graphics graphics = Graphics.FromHdcInternal(drawitemstruct.hDC);
				try
				{
					this.OnDrawItem(new DrawItemEventArgs(graphics, this.Font, Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom), drawitemstruct.itemID, (DrawItemState)drawitemstruct.itemState, this.ForeColor, this.BackColor));
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
					SafeNativeMethods.SelectPalette(new HandleRef(this, drawitemstruct.hDC), new HandleRef(null, intPtr), 0);
				}
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x000233E4 File Offset: 0x000215E4
		private void WmReflectMeasureItem(ref Message m)
		{
			NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
			if (this.DrawMode == DrawMode.OwnerDrawVariable && measureitemstruct.itemID >= 0)
			{
				Graphics graphics = base.CreateGraphicsInternal();
				MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, measureitemstruct.itemID, this.ItemHeight);
				this.OnMeasureItem(measureItemEventArgs);
				measureitemstruct.itemHeight = measureItemEventArgs.ItemHeight;
				graphics.Dispose();
			}
			else
			{
				measureitemstruct.itemHeight = this.ItemHeight;
			}
			Marshal.StructureToPtr(measureitemstruct, m.LParam, false);
			m.Result = (IntPtr)1;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process. </param>
		// Token: 0x06000B52 RID: 2898 RVA: 0x00023474 File Offset: 0x00021674
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 130)
			{
				if (msg <= 20)
				{
					if (msg <= 8)
					{
						if (msg != 7)
						{
							if (msg != 8)
							{
								goto IL_530;
							}
						}
						else
						{
							try
							{
								this.fireSetFocus = false;
								base.WndProc(ref m);
								return;
							}
							finally
							{
								this.fireSetFocus = true;
							}
						}
						try
						{
							this.fireLostFocus = false;
							base.WndProc(ref m);
							if (!Application.RenderWithVisualStyles && !base.GetStyle(ControlStyles.UserPaint) && this.DropDownStyle == ComboBoxStyle.DropDownList && (this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup))
							{
								UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 675, 0, 0);
							}
							return;
						}
						finally
						{
							this.fireLostFocus = true;
						}
					}
					else
					{
						if (msg == 15)
						{
							if (!base.GetStyle(ControlStyles.UserPaint) && (this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup))
							{
								using (WindowsRegion windowsRegion = new WindowsRegion(this.FlatComboBoxAdapter.dropDownRect))
								{
									using (WindowsRegion windowsRegion2 = new WindowsRegion(base.Bounds))
									{
										NativeMethods.RegionFlags updateRgn = (NativeMethods.RegionFlags)SafeNativeMethods.GetUpdateRgn(new HandleRef(this, base.Handle), new HandleRef(this, windowsRegion2.HRegion), true);
										windowsRegion.CombineRegion(windowsRegion2, windowsRegion, RegionCombineMode.DIFF);
										Rectangle updateRegionBox = windowsRegion2.ToRectangle();
										this.FlatComboBoxAdapter.ValidateOwnerDrawRegions(this, updateRegionBox);
										NativeMethods.PAINTSTRUCT paintstruct = default(NativeMethods.PAINTSTRUCT);
										bool flag = false;
										IntPtr intPtr;
										if (m.WParam == IntPtr.Zero)
										{
											intPtr = UnsafeNativeMethods.BeginPaint(new HandleRef(this, base.Handle), ref paintstruct);
											flag = true;
										}
										else
										{
											intPtr = m.WParam;
										}
										using (DeviceContext deviceContext = DeviceContext.FromHdc(intPtr))
										{
											using (WindowsGraphics windowsGraphics = new WindowsGraphics(deviceContext))
											{
												if (updateRgn != NativeMethods.RegionFlags.ERROR)
												{
													windowsGraphics.DeviceContext.SetClip(windowsRegion);
												}
												m.WParam = intPtr;
												this.DefWndProc(ref m);
												if (updateRgn != NativeMethods.RegionFlags.ERROR)
												{
													windowsGraphics.DeviceContext.SetClip(windowsRegion2);
												}
												using (Graphics graphics = Graphics.FromHdcInternal(intPtr))
												{
													this.FlatComboBoxAdapter.DrawFlatCombo(this, graphics);
												}
											}
										}
										if (flag)
										{
											UnsafeNativeMethods.EndPaint(new HandleRef(this, base.Handle), ref paintstruct);
										}
										return;
									}
								}
							}
							base.WndProc(ref m);
							return;
						}
						if (msg != 20)
						{
							goto IL_530;
						}
						this.WmEraseBkgnd(ref m);
						return;
					}
				}
				else if (msg <= 48)
				{
					if (msg == 32)
					{
						base.WndProc(ref m);
						return;
					}
					if (msg != 48)
					{
						goto IL_530;
					}
					if (base.Width == 0)
					{
						this.suppressNextWindosPos = true;
					}
					base.WndProc(ref m);
					return;
				}
				else
				{
					if (msg == 71)
					{
						if (!this.suppressNextWindosPos)
						{
							base.WndProc(ref m);
						}
						this.suppressNextWindosPos = false;
						return;
					}
					if (msg != 130)
					{
						goto IL_530;
					}
					base.WndProc(ref m);
					this.ReleaseChildWindow();
					return;
				}
			}
			else if (msg <= 528)
			{
				if (msg <= 513)
				{
					if (msg - 307 > 1)
					{
						if (msg != 513)
						{
							goto IL_530;
						}
						this.mouseEvents = true;
						base.WndProc(ref m);
						return;
					}
				}
				else if (msg != 514)
				{
					if (msg != 528)
					{
						goto IL_530;
					}
					this.WmParentNotify(ref m);
					return;
				}
				else
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
					Rectangle rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
					int x = NativeMethods.Util.SignedLOWORD(m.LParam);
					int y = NativeMethods.Util.SignedHIWORD(m.LParam);
					Point point = new Point(x, y);
					point = base.PointToScreen(point);
					if (this.mouseEvents && !base.ValidationCancelled)
					{
						this.mouseEvents = false;
						bool capture = base.Capture;
						if (capture && rectangle.Contains(point))
						{
							this.OnClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						}
						base.WndProc(ref m);
						return;
					}
					base.CaptureInternal = false;
					this.DefWndProc(ref m);
					return;
				}
			}
			else if (msg <= 792)
			{
				if (msg == 675)
				{
					this.DefWndProc(ref m);
					this.OnMouseLeaveInternal(EventArgs.Empty);
					return;
				}
				if (msg != 792)
				{
					goto IL_530;
				}
				if ((!base.GetStyle(ControlStyles.UserPaint) && this.FlatStyle == FlatStyle.Flat) || this.FlatStyle == FlatStyle.Popup)
				{
					this.DefWndProc(ref m);
					if (((int)((long)m.LParam) & 4) == 4)
					{
						if ((!base.GetStyle(ControlStyles.UserPaint) && this.FlatStyle == FlatStyle.Flat) || this.FlatStyle == FlatStyle.Popup)
						{
							using (Graphics graphics2 = Graphics.FromHdcInternal(m.WParam))
							{
								this.FlatComboBoxAdapter.DrawFlatCombo(this, graphics2);
							}
						}
						return;
					}
				}
				base.WndProc(ref m);
				return;
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
				if (msg != 8465)
				{
					goto IL_530;
				}
				this.WmReflectCommand(ref m);
				return;
			}
			m.Result = this.InitializeDCForWmCtlColor(m.WParam, m.Msg);
			return;
			IL_530:
			if (m.Msg == NativeMethods.WM_MOUSEENTER)
			{
				this.DefWndProc(ref m);
				this.OnMouseEnterInternal(EventArgs.Empty);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00023A9C File Offset: 0x00021C9C
		private ComboBox.FlatComboAdapter FlatComboBoxAdapter
		{
			get
			{
				ComboBox.FlatComboAdapter flatComboAdapter = base.Properties.GetObject(ComboBox.PropFlatComboAdapter) as ComboBox.FlatComboAdapter;
				if (flatComboAdapter == null || !flatComboAdapter.IsValid(this))
				{
					flatComboAdapter = this.CreateFlatComboAdapterInstance();
					base.Properties.SetObject(ComboBox.PropFlatComboAdapter, flatComboAdapter);
				}
				return flatComboAdapter;
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00023AE4 File Offset: 0x00021CE4
		internal virtual ComboBox.FlatComboAdapter CreateFlatComboAdapterInstance()
		{
			return new ComboBox.FlatComboAdapter(this, false);
		}

		// Token: 0x040006F7 RID: 1783
		private static readonly object EVENT_DROPDOWN = new object();

		// Token: 0x040006F8 RID: 1784
		private static readonly object EVENT_DRAWITEM = new object();

		// Token: 0x040006F9 RID: 1785
		private static readonly object EVENT_MEASUREITEM = new object();

		// Token: 0x040006FA RID: 1786
		private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

		// Token: 0x040006FB RID: 1787
		private static readonly object EVENT_SELECTIONCHANGECOMMITTED = new object();

		// Token: 0x040006FC RID: 1788
		private static readonly object EVENT_SELECTEDITEMCHANGED = new object();

		// Token: 0x040006FD RID: 1789
		private static readonly object EVENT_DROPDOWNSTYLE = new object();

		// Token: 0x040006FE RID: 1790
		private static readonly object EVENT_TEXTUPDATE = new object();

		// Token: 0x040006FF RID: 1791
		private static readonly object EVENT_DROPDOWNCLOSED = new object();

		// Token: 0x04000700 RID: 1792
		private static readonly int PropMaxLength = PropertyStore.CreateKey();

		// Token: 0x04000701 RID: 1793
		private static readonly int PropItemHeight = PropertyStore.CreateKey();

		// Token: 0x04000702 RID: 1794
		private static readonly int PropDropDownWidth = PropertyStore.CreateKey();

		// Token: 0x04000703 RID: 1795
		private static readonly int PropDropDownHeight = PropertyStore.CreateKey();

		// Token: 0x04000704 RID: 1796
		private static readonly int PropStyle = PropertyStore.CreateKey();

		// Token: 0x04000705 RID: 1797
		private static readonly int PropDrawMode = PropertyStore.CreateKey();

		// Token: 0x04000706 RID: 1798
		private static readonly int PropMatchingText = PropertyStore.CreateKey();

		// Token: 0x04000707 RID: 1799
		private static readonly int PropFlatComboAdapter = PropertyStore.CreateKey();

		// Token: 0x04000708 RID: 1800
		private const int DefaultSimpleStyleHeight = 150;

		// Token: 0x04000709 RID: 1801
		private const int DefaultDropDownHeight = 106;

		// Token: 0x0400070A RID: 1802
		private const int AutoCompleteTimeout = 10000000;

		// Token: 0x0400070B RID: 1803
		private bool autoCompleteDroppedDown;

		// Token: 0x0400070C RID: 1804
		private FlatStyle flatStyle = FlatStyle.Standard;

		// Token: 0x0400070D RID: 1805
		private int updateCount;

		// Token: 0x0400070E RID: 1806
		private long autoCompleteTimeStamp;

		// Token: 0x0400070F RID: 1807
		private int selectedIndex = -1;

		// Token: 0x04000710 RID: 1808
		private bool allowCommit = true;

		// Token: 0x04000711 RID: 1809
		private int requestedHeight;

		// Token: 0x04000712 RID: 1810
		private ComboBox.ComboBoxChildNativeWindow childDropDown;

		// Token: 0x04000713 RID: 1811
		private ComboBox.ComboBoxChildNativeWindow childEdit;

		// Token: 0x04000714 RID: 1812
		private ComboBox.ComboBoxChildNativeWindow childListBox;

		// Token: 0x04000715 RID: 1813
		private IntPtr dropDownHandle;

		// Token: 0x04000716 RID: 1814
		private ComboBox.ObjectCollection itemsCollection;

		// Token: 0x04000717 RID: 1815
		private short prefHeightCache = -1;

		// Token: 0x04000718 RID: 1816
		private short maxDropDownItems = 8;

		// Token: 0x04000719 RID: 1817
		private bool integralHeight = true;

		// Token: 0x0400071A RID: 1818
		private bool mousePressed;

		// Token: 0x0400071B RID: 1819
		private bool mouseEvents;

		// Token: 0x0400071C RID: 1820
		private bool mouseInEdit;

		// Token: 0x0400071D RID: 1821
		private bool sorted;

		// Token: 0x0400071E RID: 1822
		private bool fireSetFocus = true;

		// Token: 0x0400071F RID: 1823
		private bool fireLostFocus = true;

		// Token: 0x04000720 RID: 1824
		private bool mouseOver;

		// Token: 0x04000721 RID: 1825
		private bool suppressNextWindosPos;

		// Token: 0x04000722 RID: 1826
		private bool canFireLostFocus;

		// Token: 0x04000723 RID: 1827
		private string currentText = "";

		// Token: 0x04000724 RID: 1828
		private string lastTextChangedValue;

		// Token: 0x04000725 RID: 1829
		private bool dropDown;

		// Token: 0x04000726 RID: 1830
		private ComboBox.AutoCompleteDropDownFinder finder = new ComboBox.AutoCompleteDropDownFinder();

		// Token: 0x04000727 RID: 1831
		private bool selectedValueChangedFired;

		// Token: 0x04000728 RID: 1832
		private AutoCompleteMode autoCompleteMode;

		// Token: 0x04000729 RID: 1833
		private AutoCompleteSource autoCompleteSource = AutoCompleteSource.None;

		// Token: 0x0400072A RID: 1834
		private AutoCompleteStringCollection autoCompleteCustomSource;

		// Token: 0x0400072B RID: 1835
		private StringSource stringSource;

		// Token: 0x0400072C RID: 1836
		private bool fromHandleCreate;

		// Token: 0x0400072D RID: 1837
		private ComboBox.ComboBoxChildListUiaProvider childListAccessibleObject;

		// Token: 0x0400072E RID: 1838
		private ComboBox.ComboBoxChildEditUiaProvider childEditAccessibleObject;

		// Token: 0x0400072F RID: 1839
		private ComboBox.ComboBoxChildTextUiaProvider childTextAccessibleObject;

		// Token: 0x04000730 RID: 1840
		private bool dropDownWillBeClosed;

		// Token: 0x02000567 RID: 1383
		[ComVisible(true)]
		internal class ComboBoxChildNativeWindow : NativeWindow
		{
			// Token: 0x06005678 RID: 22136 RVA: 0x0016A16C File Offset: 0x0016836C
			public ComboBoxChildNativeWindow(ComboBox comboBox, ComboBox.ChildWindowType childWindowType)
			{
				this._owner = comboBox;
				this._childWindowType = childWindowType;
			}

			// Token: 0x06005679 RID: 22137 RVA: 0x0016A184 File Offset: 0x00168384
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg != 61)
				{
					if (msg != 512)
					{
						if (this._childWindowType == ComboBox.ChildWindowType.DropDownList)
						{
							base.DefWndProc(ref m);
							return;
						}
						this._owner.ChildWndProc(ref m);
					}
					else
					{
						if (this._childWindowType != ComboBox.ChildWindowType.DropDownList)
						{
							this._owner.ChildWndProc(ref m);
							return;
						}
						object selectedItem = this._owner.SelectedItem;
						base.DefWndProc(ref m);
						object selectedItem2 = this._owner.SelectedItem;
						if (selectedItem != selectedItem2)
						{
							(this._owner.AccessibilityObject as ComboBox.ComboBoxUiaProvider).SetComboBoxItemFocus();
							return;
						}
					}
					return;
				}
				this.WmGetObject(ref m);
			}

			// Token: 0x0600567A RID: 22138 RVA: 0x0016A21B File Offset: 0x0016841B
			private ComboBox.ChildAccessibleObject GetChildAccessibleObject(ComboBox.ChildWindowType childWindowType)
			{
				if (childWindowType == ComboBox.ChildWindowType.Edit)
				{
					return this._owner.ChildEditAccessibleObject;
				}
				if (childWindowType == ComboBox.ChildWindowType.ListBox || childWindowType == ComboBox.ChildWindowType.DropDownList)
				{
					return this._owner.ChildListAccessibleObject;
				}
				return new ComboBox.ChildAccessibleObject(this._owner, base.Handle);
			}

			// Token: 0x0600567B RID: 22139 RVA: 0x0016A254 File Offset: 0x00168454
			private void WmGetObject(ref Message m)
			{
				if (AccessibilityImprovements.Level3 && m.LParam == (IntPtr)(-25) && (this._childWindowType == ComboBox.ChildWindowType.ListBox || this._childWindowType == ComboBox.ChildWindowType.DropDownList))
				{
					AccessibleObject childAccessibleObject = this.GetChildAccessibleObject(this._childWindowType);
					IntSecurity.UnmanagedCode.Assert();
					InternalAccessibleObject el;
					try
					{
						el = new InternalAccessibleObject(childAccessibleObject);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					m.Result = UnsafeNativeMethods.UiaReturnRawElementProvider(new HandleRef(this, base.Handle), m.WParam, m.LParam, el);
					return;
				}
				if (-4 == (int)((long)m.LParam))
				{
					Guid guid = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");
					try
					{
						if (this._accessibilityObject == null)
						{
							IntSecurity.UnmanagedCode.Assert();
							try
							{
								AccessibleObject accessibleImplemention = AccessibilityImprovements.Level3 ? this.GetChildAccessibleObject(this._childWindowType) : new ComboBox.ChildAccessibleObject(this._owner, base.Handle);
								this._accessibilityObject = new InternalAccessibleObject(accessibleImplemention);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
						UnsafeNativeMethods.IAccessibleInternal accessibilityObject = this._accessibilityObject;
						IntPtr iunknownForObject = Marshal.GetIUnknownForObject(accessibilityObject);
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							m.Result = UnsafeNativeMethods.LresultFromObject(ref guid, m.WParam, new HandleRef(this, iunknownForObject));
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
							Marshal.Release(iunknownForObject);
						}
						return;
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException(SR.GetString("RichControlLresult"), innerException);
					}
				}
				base.DefWndProc(ref m);
			}

			// Token: 0x040037EE RID: 14318
			private ComboBox _owner;

			// Token: 0x040037EF RID: 14319
			private InternalAccessibleObject _accessibilityObject;

			// Token: 0x040037F0 RID: 14320
			private ComboBox.ChildWindowType _childWindowType;
		}

		// Token: 0x02000568 RID: 1384
		private sealed class ItemComparer : IComparer
		{
			// Token: 0x0600567C RID: 22140 RVA: 0x0016A3E0 File Offset: 0x001685E0
			public ItemComparer(ComboBox comboBox)
			{
				this.comboBox = comboBox;
			}

			// Token: 0x0600567D RID: 22141 RVA: 0x0016A3F0 File Offset: 0x001685F0
			public int Compare(object item1, object item2)
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
					string itemText = this.comboBox.GetItemText(item1);
					string itemText2 = this.comboBox.GetItemText(item2);
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					return compareInfo.Compare(itemText, itemText2, CompareOptions.StringSort);
				}
			}

			// Token: 0x040037F1 RID: 14321
			private ComboBox comboBox;
		}

		/// <summary>Represents the collection of items in a <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
		// Token: 0x02000569 RID: 1385
		[ListBindable(false)]
		public class ObjectCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of <see cref="T:System.Windows.Forms.ComboBox.ObjectCollection" />.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ComboBox" /> that owns this object collection. </param>
			// Token: 0x0600567E RID: 22142 RVA: 0x0016A43E File Offset: 0x0016863E
			public ObjectCollection(ComboBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x17001495 RID: 5269
			// (get) Token: 0x0600567F RID: 22143 RVA: 0x0016A44D File Offset: 0x0016864D
			private IComparer Comparer
			{
				get
				{
					if (this.comparer == null)
					{
						this.comparer = new ComboBox.ItemComparer(this.owner);
					}
					return this.comparer;
				}
			}

			// Token: 0x17001496 RID: 5270
			// (get) Token: 0x06005680 RID: 22144 RVA: 0x0016A46E File Offset: 0x0016866E
			private ArrayList InnerList
			{
				get
				{
					if (this.innerList == null)
					{
						this.innerList = new ArrayList();
					}
					return this.innerList;
				}
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of items in the collection.</returns>
			// Token: 0x17001497 RID: 5271
			// (get) Token: 0x06005681 RID: 22145 RVA: 0x0016A489 File Offset: 0x00168689
			public int Count
			{
				get
				{
					return this.InnerList.Count;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.ComboBox.ObjectCollection" />.</returns>
			// Token: 0x17001498 RID: 5272
			// (get) Token: 0x06005682 RID: 22146 RVA: 0x000069BD File Offset: 0x00004BBD
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
			// Token: 0x17001499 RID: 5273
			// (get) Token: 0x06005683 RID: 22147 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x1700149A RID: 5274
			// (get) Token: 0x06005684 RID: 22148 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether this collection can be modified.</summary>
			/// <returns>Always <see langword="false" />.</returns>
			// Token: 0x1700149B RID: 5275
			// (get) Token: 0x06005685 RID: 22149 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds an item to the list of items for a <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
			/// <param name="item">An object representing the item to add to the collection. </param>
			/// <returns>The zero-based index of the item in the collection.</returns>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="item" /> parameter was <see langword="null" />. </exception>
			// Token: 0x06005686 RID: 22150 RVA: 0x0016A498 File Offset: 0x00168698
			public int Add(object item)
			{
				this.owner.CheckNoDataSource();
				int result = this.AddInternal(item);
				if (this.owner.UpdateNeeded() && this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					this.owner.SetAutoComplete(false, false);
				}
				return result;
			}

			// Token: 0x06005687 RID: 22151 RVA: 0x0016A4E8 File Offset: 0x001686E8
			private int AddInternal(object item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				int num = -1;
				if (!this.owner.sorted)
				{
					this.InnerList.Add(item);
				}
				else
				{
					num = this.InnerList.BinarySearch(item, this.Comparer);
					if (num < 0)
					{
						num = ~num;
					}
					this.InnerList.Insert(num, item);
				}
				bool flag = false;
				try
				{
					if (this.owner.sorted)
					{
						if (this.owner.IsHandleCreated)
						{
							this.owner.NativeInsert(num, item);
						}
					}
					else
					{
						num = this.InnerList.Count - 1;
						if (this.owner.IsHandleCreated)
						{
							this.owner.NativeAdd(item);
						}
					}
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.InnerList.Remove(item);
					}
				}
				return num;
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			/// <param name="item">An object that represents the item to add to the collection.</param>
			/// <returns>The zero-based index of the item in the collection.</returns>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="item" /> parameter is <see langword="null" />.</exception>
			/// <exception cref="T:System.SystemException">There is insufficient space available to store the new item.</exception>
			// Token: 0x06005688 RID: 22152 RVA: 0x0016A5C4 File Offset: 0x001687C4
			int IList.Add(object item)
			{
				return this.Add(item);
			}

			/// <summary>Adds an array of items to the list of items for a <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
			/// <param name="items">An array of objects to add to the list. </param>
			/// <exception cref="T:System.ArgumentNullException">An item in the <paramref name="items" /> parameter was <see langword="null" />. </exception>
			// Token: 0x06005689 RID: 22153 RVA: 0x0016A5D0 File Offset: 0x001687D0
			public void AddRange(object[] items)
			{
				this.owner.CheckNoDataSource();
				this.owner.BeginUpdate();
				try
				{
					this.AddRangeInternal(items);
				}
				finally
				{
					this.owner.EndUpdate();
				}
			}

			// Token: 0x0600568A RID: 22154 RVA: 0x0016A618 File Offset: 0x00168818
			internal void AddRangeInternal(IList items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				foreach (object item in items)
				{
					this.AddInternal(item);
				}
				if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					this.owner.SetAutoComplete(false, false);
				}
			}

			/// <summary>Retrieves the item at the specified index within the collection.</summary>
			/// <param name="index">The index of the item in the collection to retrieve. </param>
			/// <returns>An object representing the item located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index was less than zero.-or- The <paramref name="index" /> was greater of equal to the count of items in the collection. </exception>
			// Token: 0x1700149C RID: 5276
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public virtual object this[int index]
			{
				get
				{
					if (index < 0 || index >= this.InnerList.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.InnerList[index];
				}
				set
				{
					this.owner.CheckNoDataSource();
					this.SetItemInternal(index, value);
				}
			}

			/// <summary>Removes all items from the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
			// Token: 0x0600568D RID: 22157 RVA: 0x0016A70A File Offset: 0x0016890A
			public void Clear()
			{
				this.owner.CheckNoDataSource();
				this.ClearInternal();
			}

			// Token: 0x0600568E RID: 22158 RVA: 0x0016A720 File Offset: 0x00168920
			internal void ClearInternal()
			{
				if (this.owner.IsHandleCreated)
				{
					this.owner.NativeClear();
				}
				this.InnerList.Clear();
				this.owner.selectedIndex = -1;
				if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					this.owner.SetAutoComplete(false, true);
				}
			}

			/// <summary>Determines if the specified item is located within the collection.</summary>
			/// <param name="value">An object representing the item to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the item is located within the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x0600568F RID: 22159 RVA: 0x0016A77B File Offset: 0x0016897B
			public bool Contains(object value)
			{
				return this.IndexOf(value) != -1;
			}

			/// <summary>Copies the entire collection into an existing array of objects at a specified location within the array.</summary>
			/// <param name="destination">The object array to copy the collection to. </param>
			/// <param name="arrayIndex">The location in the destination array to copy the collection to. </param>
			// Token: 0x06005690 RID: 22160 RVA: 0x0016A78A File Offset: 0x0016898A
			public void CopyTo(object[] destination, int arrayIndex)
			{
				this.InnerList.CopyTo(destination, arrayIndex);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
			/// <param name="destination">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in the array at which copying begins.</param>
			// Token: 0x06005691 RID: 22161 RVA: 0x0016A78A File Offset: 0x0016898A
			void ICollection.CopyTo(Array destination, int index)
			{
				this.InnerList.CopyTo(destination, index);
			}

			/// <summary>Returns an enumerator that can be used to iterate through the item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the item collection.</returns>
			// Token: 0x06005692 RID: 22162 RVA: 0x0016A799 File Offset: 0x00168999
			public IEnumerator GetEnumerator()
			{
				return this.InnerList.GetEnumerator();
			}

			/// <summary>Retrieves the index within the collection of the specified item.</summary>
			/// <param name="value">An object representing the item to locate in the collection. </param>
			/// <returns>The zero-based index where the item is located within the collection; otherwise, -1.</returns>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter was <see langword="null" />. </exception>
			// Token: 0x06005693 RID: 22163 RVA: 0x0016A7A6 File Offset: 0x001689A6
			public int IndexOf(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.InnerList.IndexOf(value);
			}

			/// <summary>Inserts an item into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the item is inserted. </param>
			/// <param name="item">An object representing the item to insert. </param>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="item" /> was <see langword="null" />. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> was less than zero.-or- The <paramref name="index" /> was greater than the count of items in the collection. </exception>
			// Token: 0x06005694 RID: 22164 RVA: 0x0016A7C4 File Offset: 0x001689C4
			public void Insert(int index, object item)
			{
				this.owner.CheckNoDataSource();
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (index < 0 || index > this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.sorted)
				{
					this.Add(item);
					return;
				}
				this.InnerList.Insert(index, item);
				if (this.owner.IsHandleCreated)
				{
					bool flag = false;
					try
					{
						this.owner.NativeInsert(index, item);
						flag = true;
					}
					finally
					{
						if (flag)
						{
							if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
							{
								this.owner.SetAutoComplete(false, false);
							}
						}
						else
						{
							this.InnerList.RemoveAt(index);
						}
					}
				}
			}

			/// <summary>Removes an item from the <see cref="T:System.Windows.Forms.ComboBox" /> at the specified index.</summary>
			/// <param name="index">The index of the item to remove. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="value" /> parameter was less than zero.-or- The <paramref name="value" /> parameter was greater than or equal to the count of items in the collection. </exception>
			// Token: 0x06005695 RID: 22165 RVA: 0x0016A8B4 File Offset: 0x00168AB4
			public void RemoveAt(int index)
			{
				this.owner.CheckNoDataSource();
				if (index < 0 || index >= this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.IsHandleCreated)
				{
					this.owner.NativeRemoveAt(index);
				}
				this.InnerList.RemoveAt(index);
				if (!this.owner.IsHandleCreated && index < this.owner.selectedIndex)
				{
					this.owner.selectedIndex--;
				}
				if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
				{
					this.owner.SetAutoComplete(false, false);
				}
			}

			/// <summary>Removes the specified item from the <see cref="T:System.Windows.Forms.ComboBox" />.</summary>
			/// <param name="value">The <see cref="T:System.Object" /> to remove from the list. </param>
			// Token: 0x06005696 RID: 22166 RVA: 0x0016A984 File Offset: 0x00168B84
			public void Remove(object value)
			{
				int num = this.InnerList.IndexOf(value);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06005697 RID: 22167 RVA: 0x0016A9AC File Offset: 0x00168BAC
			internal void SetItemInternal(int index, object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (index < 0 || index >= this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.InnerList[index] = value;
				if (this.owner.IsHandleCreated)
				{
					bool flag = index == this.owner.SelectedIndex;
					if (string.Compare(this.owner.GetItemText(value), this.owner.NativeGetItemText(index), true, CultureInfo.CurrentCulture) != 0)
					{
						this.owner.NativeRemoveAt(index);
						this.owner.NativeInsert(index, value);
						if (flag)
						{
							this.owner.SelectedIndex = index;
							this.owner.UpdateText();
						}
						if (this.owner.AutoCompleteSource == AutoCompleteSource.ListItems)
						{
							this.owner.SetAutoComplete(false, false);
							return;
						}
					}
					else if (flag)
					{
						this.owner.OnSelectedItemChanged(EventArgs.Empty);
						this.owner.OnSelectedIndexChanged(EventArgs.Empty);
					}
				}
			}

			// Token: 0x040037F2 RID: 14322
			private ComboBox owner;

			// Token: 0x040037F3 RID: 14323
			private ArrayList innerList;

			// Token: 0x040037F4 RID: 14324
			private IComparer comparer;
		}

		/// <summary>Provides information about the <see cref="T:System.Windows.Forms.ComboBox" /> control to accessibility client applications.</summary>
		// Token: 0x0200056A RID: 1386
		[ComVisible(true)]
		public class ChildAccessibleObject : AccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ComboBox.ChildAccessibleObject" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.ComboBox" /> control that owns the <see cref="T:System.Windows.Forms.ComboBox.ChildAccessibleObject" />.</param>
			/// <param name="handle">A handle to part of the <see cref="T:System.Windows.Forms.ComboBox" />.</param>
			// Token: 0x06005698 RID: 22168 RVA: 0x0016AAD3 File Offset: 0x00168CD3
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public ChildAccessibleObject(ComboBox owner, IntPtr handle)
			{
				this.owner = owner;
				base.UseStdAccessibleObjects(handle);
			}

			/// <summary>Gets the name of the object.</summary>
			/// <returns>The value of the <see cref="P:System.Windows.Forms.ComboBox.ChildAccessibleObject.Name" /> property is the same as the <see cref="P:System.Windows.Forms.AccessibleObject.Name" /> property for the <see cref="T:System.Windows.Forms.AccessibleObject" /> of the <see cref="T:System.Windows.Forms.ComboBox" />.</returns>
			// Token: 0x1700149D RID: 5277
			// (get) Token: 0x06005699 RID: 22169 RVA: 0x0016AAE9 File Offset: 0x00168CE9
			public override string Name
			{
				get
				{
					return this.owner.AccessibilityObject.Name;
				}
			}

			// Token: 0x040037F5 RID: 14325
			private ComboBox owner;
		}

		// Token: 0x0200056B RID: 1387
		[ComVisible(true)]
		internal class ComboBoxAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x0600569A RID: 22170 RVA: 0x00093572 File Offset: 0x00091772
			public ComboBoxAccessibleObject(Control ownerControl) : base(ownerControl)
			{
			}

			// Token: 0x0600569B RID: 22171 RVA: 0x0016AAFB File Offset: 0x00168CFB
			internal override string get_accNameInternal(object childID)
			{
				base.ValidateChildID(ref childID);
				if (childID != null && (int)childID == 1)
				{
					return this.Name;
				}
				return base.get_accNameInternal(childID);
			}

			// Token: 0x0600569C RID: 22172 RVA: 0x0016AB1F File Offset: 0x00168D1F
			internal override string get_accKeyboardShortcutInternal(object childID)
			{
				base.ValidateChildID(ref childID);
				if (childID != null && (int)childID == 1)
				{
					return this.KeyboardShortcut;
				}
				return base.get_accKeyboardShortcutInternal(childID);
			}

			// Token: 0x040037F6 RID: 14326
			private const int COMBOBOX_ACC_ITEM_INDEX = 1;
		}

		// Token: 0x0200056C RID: 1388
		[ComVisible(true)]
		internal class ComboBoxExAccessibleObject : ComboBox.ComboBoxAccessibleObject
		{
			// Token: 0x0600569D RID: 22173 RVA: 0x0016AB43 File Offset: 0x00168D43
			private void ComboBoxDefaultAction(bool expand)
			{
				if (this.ownerItem.DroppedDown != expand)
				{
					this.ownerItem.DroppedDown = expand;
				}
			}

			// Token: 0x0600569E RID: 22174 RVA: 0x0016AB5F File Offset: 0x00168D5F
			public ComboBoxExAccessibleObject(ComboBox ownerControl) : base(ownerControl)
			{
				this.ownerItem = ownerControl;
			}

			// Token: 0x0600569F RID: 22175 RVA: 0x0016AB6F File Offset: 0x00168D6F
			internal override bool IsIAccessibleExSupported()
			{
				return this.ownerItem != null || base.IsIAccessibleExSupported();
			}

			// Token: 0x060056A0 RID: 22176 RVA: 0x0016AB81 File Offset: 0x00168D81
			internal override bool IsPatternSupported(int patternId)
			{
				if (patternId == 10005)
				{
					return this.ownerItem.DropDownStyle != ComboBoxStyle.Simple;
				}
				if (patternId == 10002)
				{
					return this.ownerItem.DropDownStyle != ComboBoxStyle.DropDownList || AccessibilityImprovements.Level3;
				}
				return base.IsPatternSupported(patternId);
			}

			// Token: 0x1700149E RID: 5278
			// (get) Token: 0x060056A1 RID: 22177 RVA: 0x0016ABC4 File Offset: 0x00168DC4
			internal override int[] RuntimeId
			{
				get
				{
					if (this.ownerItem != null)
					{
						return new int[]
						{
							42,
							(int)((long)this.ownerItem.Handle),
							this.ownerItem.GetHashCode()
						};
					}
					return base.RuntimeId;
				}
			}

			// Token: 0x060056A2 RID: 22178 RVA: 0x0016AC10 File Offset: 0x00168E10
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30005)
				{
					return this.Name;
				}
				if (propertyID == 30028)
				{
					return this.IsPatternSupported(10005);
				}
				if (propertyID != 30043)
				{
					return base.GetPropertyValue(propertyID);
				}
				return this.IsPatternSupported(10002);
			}

			// Token: 0x060056A3 RID: 22179 RVA: 0x0016AC67 File Offset: 0x00168E67
			internal override void Expand()
			{
				this.ComboBoxDefaultAction(true);
			}

			// Token: 0x060056A4 RID: 22180 RVA: 0x0016AC70 File Offset: 0x00168E70
			internal override void Collapse()
			{
				this.ComboBoxDefaultAction(false);
			}

			// Token: 0x1700149F RID: 5279
			// (get) Token: 0x060056A5 RID: 22181 RVA: 0x0016AC79 File Offset: 0x00168E79
			internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
			{
				get
				{
					if (!this.ownerItem.DroppedDown)
					{
						return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
					}
					return UnsafeNativeMethods.ExpandCollapseState.Expanded;
				}
			}

			// Token: 0x040037F7 RID: 14327
			private ComboBox ownerItem;
		}

		// Token: 0x0200056D RID: 1389
		[ComVisible(true)]
		internal class ComboBoxItemAccessibleObject : AccessibleObject
		{
			// Token: 0x060056A6 RID: 22182 RVA: 0x0016AC8B File Offset: 0x00168E8B
			public ComboBoxItemAccessibleObject(ComboBox owningComboBox, object owningItem)
			{
				this._owningComboBox = owningComboBox;
				this._owningItem = owningItem;
				this._systemIAccessible = this._owningComboBox.ChildListAccessibleObject.GetSystemIAccessibleInternal();
			}

			// Token: 0x170014A0 RID: 5280
			// (get) Token: 0x060056A7 RID: 22183 RVA: 0x0016ACB8 File Offset: 0x00168EB8
			public override Rectangle Bounds
			{
				get
				{
					int currentIndex = this.GetCurrentIndex();
					IntPtr listHandle = this._owningComboBox.GetListHandle();
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					if ((int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(this, listHandle), 408, currentIndex, ref rect)) == -1)
					{
						return Rectangle.Empty;
					}
					UnsafeNativeMethods.MapWindowPoints(new HandleRef(this, listHandle), NativeMethods.NullHandleRef, ref rect, 2);
					return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
				}
			}

			// Token: 0x170014A1 RID: 5281
			// (get) Token: 0x060056A8 RID: 22184 RVA: 0x0016AD35 File Offset: 0x00168F35
			public override string DefaultAction
			{
				get
				{
					return this._systemIAccessible.get_accDefaultAction(this.GetChildId());
				}
			}

			// Token: 0x060056A9 RID: 22185 RVA: 0x0016AD50 File Offset: 0x00168F50
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return this._owningComboBox.ChildListAccessibleObject;
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
				{
					int currentIndex = this.GetCurrentIndex();
					ComboBox.ComboBoxChildListUiaProvider comboBoxChildListUiaProvider = this._owningComboBox.ChildListAccessibleObject as ComboBox.ComboBoxChildListUiaProvider;
					if (comboBoxChildListUiaProvider != null)
					{
						int childFragmentCount = comboBoxChildListUiaProvider.GetChildFragmentCount();
						int num = currentIndex + 1;
						if (childFragmentCount > num)
						{
							return comboBoxChildListUiaProvider.GetChildFragment(num);
						}
					}
					break;
				}
				case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
				{
					int currentIndex = this.GetCurrentIndex();
					ComboBox.ComboBoxChildListUiaProvider comboBoxChildListUiaProvider = this._owningComboBox.ChildListAccessibleObject as ComboBox.ComboBoxChildListUiaProvider;
					if (comboBoxChildListUiaProvider != null)
					{
						int childFragmentCount2 = comboBoxChildListUiaProvider.GetChildFragmentCount();
						int num2 = currentIndex - 1;
						if (num2 >= 0)
						{
							return comboBoxChildListUiaProvider.GetChildFragment(num2);
						}
					}
					break;
				}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x170014A2 RID: 5282
			// (get) Token: 0x060056AA RID: 22186 RVA: 0x0016ADEC File Offset: 0x00168FEC
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this._owningComboBox.AccessibilityObject;
				}
			}

			// Token: 0x060056AB RID: 22187 RVA: 0x0016ADF9 File Offset: 0x00168FF9
			private int GetCurrentIndex()
			{
				return this._owningComboBox.Items.IndexOf(this._owningItem);
			}

			// Token: 0x060056AC RID: 22188 RVA: 0x0016AE11 File Offset: 0x00169011
			internal override int GetChildId()
			{
				return this.GetCurrentIndex() + 1;
			}

			// Token: 0x060056AD RID: 22189 RVA: 0x0016AE1C File Offset: 0x0016901C
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID <= 30035)
				{
					switch (propertyID)
					{
					case 30000:
						return this.RuntimeId;
					case 30001:
						return this.BoundingRectangle;
					case 30002:
					case 30004:
					case 30006:
					case 30011:
					case 30012:
					case 30014:
					case 30015:
					case 30018:
					case 30020:
					case 30021:
						break;
					case 30003:
						return 50007;
					case 30005:
						return this.Name;
					case 30007:
						return this.KeyboardShortcut ?? string.Empty;
					case 30008:
						return this._owningComboBox.Focused && this._owningComboBox.SelectedIndex == this.GetCurrentIndex();
					case 30009:
						return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
					case 30010:
						return this._owningComboBox.Enabled;
					case 30013:
						return this.Help ?? string.Empty;
					case 30016:
						return true;
					case 30017:
						return true;
					case 30019:
						return false;
					case 30022:
						return (this.State & AccessibleStates.Offscreen) == AccessibleStates.Offscreen;
					default:
						if (propertyID == 30035)
						{
							return true;
						}
						break;
					}
				}
				else
				{
					if (propertyID == 30036)
					{
						return true;
					}
					if (propertyID == 30079)
					{
						return (this.State & AccessibleStates.Selected) > AccessibleStates.None;
					}
					if (propertyID == 30080)
					{
						return this._owningComboBox.ChildListAccessibleObject;
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x170014A3 RID: 5283
			// (get) Token: 0x060056AE RID: 22190 RVA: 0x0016AFC7 File Offset: 0x001691C7
			public override string Help
			{
				get
				{
					return this._systemIAccessible.get_accHelp(this.GetChildId());
				}
			}

			// Token: 0x060056AF RID: 22191 RVA: 0x0016AFDF File Offset: 0x001691DF
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId == 10018 || patternId == 10000 || patternId == 10017 || patternId == 10010 || base.IsPatternSupported(patternId);
			}

			// Token: 0x170014A4 RID: 5284
			// (get) Token: 0x060056B0 RID: 22192 RVA: 0x0016B00A File Offset: 0x0016920A
			// (set) Token: 0x060056B1 RID: 22193 RVA: 0x0016B02C File Offset: 0x0016922C
			public override string Name
			{
				get
				{
					if (this._owningComboBox != null)
					{
						return this._owningComboBox.GetItemText(this._owningItem);
					}
					return base.Name;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x170014A5 RID: 5285
			// (get) Token: 0x060056B2 RID: 22194 RVA: 0x0016B035 File Offset: 0x00169235
			public override AccessibleRole Role
			{
				get
				{
					return (AccessibleRole)this._systemIAccessible.get_accRole(this.GetChildId());
				}
			}

			// Token: 0x170014A6 RID: 5286
			// (get) Token: 0x060056B3 RID: 22195 RVA: 0x0016B054 File Offset: 0x00169254
			internal override int[] RuntimeId
			{
				get
				{
					return new int[]
					{
						42,
						(int)((long)this._owningComboBox.Handle),
						this._owningComboBox.GetListNativeWindowRuntimeIdPart(),
						this._owningItem.GetHashCode()
					};
				}
			}

			// Token: 0x060056B4 RID: 22196 RVA: 0x0016B0A0 File Offset: 0x001692A0
			internal override void ScrollIntoView()
			{
				if (!this._owningComboBox.IsHandleCreated || !this._owningComboBox.Enabled)
				{
					return;
				}
				if (this._owningComboBox.ChildListAccessibleObject.BoundingRectangle.IntersectsWith(this.Bounds))
				{
					return;
				}
				this._owningComboBox.SendMessage(348, this.GetCurrentIndex(), 0);
			}

			// Token: 0x170014A7 RID: 5287
			// (get) Token: 0x060056B5 RID: 22197 RVA: 0x0016B101 File Offset: 0x00169301
			public override AccessibleStates State
			{
				get
				{
					return (AccessibleStates)this._systemIAccessible.get_accState(this.GetChildId());
				}
			}

			// Token: 0x060056B6 RID: 22198 RVA: 0x0013D795 File Offset: 0x0013B995
			internal override void SetFocus()
			{
				base.RaiseAutomationEvent(20005);
				base.SetFocus();
			}

			// Token: 0x060056B7 RID: 22199 RVA: 0x0016B11E File Offset: 0x0016931E
			internal override void SelectItem()
			{
				this._owningComboBox.SelectedIndex = this.GetCurrentIndex();
				SafeNativeMethods.InvalidateRect(new HandleRef(this, this._owningComboBox.GetListHandle()), null, false);
			}

			// Token: 0x060056B8 RID: 22200 RVA: 0x0000F3C2 File Offset: 0x0000D5C2
			internal override void AddToSelection()
			{
				this.SelectItem();
			}

			// Token: 0x060056B9 RID: 22201 RVA: 0x0000701A File Offset: 0x0000521A
			internal override void RemoveFromSelection()
			{
			}

			// Token: 0x170014A8 RID: 5288
			// (get) Token: 0x060056BA RID: 22202 RVA: 0x0016B14A File Offset: 0x0016934A
			internal override bool IsItemSelected
			{
				get
				{
					return (this.State & AccessibleStates.Selected) > AccessibleStates.None;
				}
			}

			// Token: 0x170014A9 RID: 5289
			// (get) Token: 0x060056BB RID: 22203 RVA: 0x0016B157 File Offset: 0x00169357
			internal override UnsafeNativeMethods.IRawElementProviderSimple ItemSelectionContainer
			{
				get
				{
					return this._owningComboBox.ChildListAccessibleObject;
				}
			}

			// Token: 0x040037F8 RID: 14328
			private ComboBox _owningComboBox;

			// Token: 0x040037F9 RID: 14329
			private object _owningItem;

			// Token: 0x040037FA RID: 14330
			private IAccessible _systemIAccessible;
		}

		// Token: 0x0200056E RID: 1390
		internal class ComboBoxItemAccessibleObjectCollection : Hashtable
		{
			// Token: 0x060056BC RID: 22204 RVA: 0x0016B164 File Offset: 0x00169364
			public ComboBoxItemAccessibleObjectCollection(ComboBox owningComboBoxBox)
			{
				this._owningComboBoxBox = owningComboBoxBox;
			}

			// Token: 0x170014AA RID: 5290
			public override object this[object key]
			{
				get
				{
					if (!this.ContainsKey(key))
					{
						ComboBox.ComboBoxItemAccessibleObject value = new ComboBox.ComboBoxItemAccessibleObject(this._owningComboBoxBox, key);
						base[key] = value;
					}
					return base[key];
				}
				set
				{
					base[key] = value;
				}
			}

			// Token: 0x040037FB RID: 14331
			private ComboBox _owningComboBoxBox;
		}

		// Token: 0x0200056F RID: 1391
		[ComVisible(true)]
		internal class ComboBoxUiaProvider : ComboBox.ComboBoxExAccessibleObject
		{
			// Token: 0x060056BF RID: 22207 RVA: 0x0016B1B0 File Offset: 0x001693B0
			public ComboBoxUiaProvider(ComboBox owningComboBox) : base(owningComboBox)
			{
				this._owningComboBox = owningComboBox;
				this._itemAccessibleObjects = new ComboBox.ComboBoxItemAccessibleObjectCollection(owningComboBox);
			}

			// Token: 0x170014AB RID: 5291
			// (get) Token: 0x060056C0 RID: 22208 RVA: 0x0016B1CC File Offset: 0x001693CC
			public ComboBox.ComboBoxItemAccessibleObjectCollection ItemAccessibleObjects
			{
				get
				{
					return this._itemAccessibleObjects;
				}
			}

			// Token: 0x060056C1 RID: 22209 RVA: 0x0016B1D4 File Offset: 0x001693D4
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId == 10018 || base.IsPatternSupported(patternId);
			}

			// Token: 0x170014AC RID: 5292
			// (get) Token: 0x060056C2 RID: 22210 RVA: 0x0016B1E7 File Offset: 0x001693E7
			public ComboBox.ComboBoxChildDropDownButtonUiaProvider DropDownButtonUiaProvider
			{
				get
				{
					return this._dropDownButtonUiaProvider ?? new ComboBox.ComboBoxChildDropDownButtonUiaProvider(this._owningComboBox, this._owningComboBox.Handle);
				}
			}

			// Token: 0x060056C3 RID: 22211 RVA: 0x0016B20C File Offset: 0x0016940C
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild)
				{
					return this.GetChildFragment(0);
				}
				if (direction == UnsafeNativeMethods.NavigateDirection.LastChild)
				{
					int childFragmentCount = this.GetChildFragmentCount();
					if (childFragmentCount > 0)
					{
						return this.GetChildFragment(childFragmentCount - 1);
					}
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x170014AD RID: 5293
			// (get) Token: 0x060056C4 RID: 22212 RVA: 0x000069BD File Offset: 0x00004BBD
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x060056C5 RID: 22213 RVA: 0x0016B248 File Offset: 0x00169448
			internal override UnsafeNativeMethods.IRawElementProviderSimple GetOverrideProviderForHwnd(IntPtr hwnd)
			{
				if (hwnd == this._owningComboBox.childEdit.Handle)
				{
					return this._owningComboBox.ChildEditAccessibleObject;
				}
				if (hwnd == this._owningComboBox.childListBox.Handle || hwnd == this._owningComboBox.dropDownHandle)
				{
					return this._owningComboBox.ChildListAccessibleObject;
				}
				return null;
			}

			// Token: 0x060056C6 RID: 22214 RVA: 0x0016B2B1 File Offset: 0x001694B1
			internal AccessibleObject GetChildFragment(int index)
			{
				if (this._owningComboBox.DropDownStyle == ComboBoxStyle.DropDownList)
				{
					if (index == 0)
					{
						return this._owningComboBox.ChildTextAccessibleObject;
					}
					index--;
				}
				if (index == 0 && this._owningComboBox.DropDownStyle != ComboBoxStyle.Simple)
				{
					return this.DropDownButtonUiaProvider;
				}
				return null;
			}

			// Token: 0x060056C7 RID: 22215 RVA: 0x0016B2F0 File Offset: 0x001694F0
			internal int GetChildFragmentCount()
			{
				int num = 0;
				if (this._owningComboBox.DropDownStyle == ComboBoxStyle.DropDownList)
				{
					num++;
				}
				if (this._owningComboBox.DropDownStyle != ComboBoxStyle.Simple)
				{
					num++;
				}
				return num;
			}

			// Token: 0x060056C8 RID: 22216 RVA: 0x0016B324 File Offset: 0x00169524
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50003;
				}
				if (propertyID == 30008)
				{
					return this._owningComboBox.Focused;
				}
				if (propertyID != 30020)
				{
					return base.GetPropertyValue(propertyID);
				}
				return this._owningComboBox.Handle;
			}

			// Token: 0x060056C9 RID: 22217 RVA: 0x0016B37F File Offset: 0x0016957F
			internal void ResetListItemAccessibleObjects()
			{
				this._itemAccessibleObjects.Clear();
			}

			// Token: 0x060056CA RID: 22218 RVA: 0x0016B38C File Offset: 0x0016958C
			internal void SetComboBoxItemFocus()
			{
				object selectedItem = this._owningComboBox.SelectedItem;
				if (selectedItem == null)
				{
					return;
				}
				ComboBox.ComboBoxItemAccessibleObject comboBoxItemAccessibleObject = this.ItemAccessibleObjects[selectedItem] as ComboBox.ComboBoxItemAccessibleObject;
				if (comboBoxItemAccessibleObject != null)
				{
					comboBoxItemAccessibleObject.SetFocus();
				}
			}

			// Token: 0x060056CB RID: 22219 RVA: 0x0016B3C4 File Offset: 0x001695C4
			internal void SetComboBoxItemSelection()
			{
				object selectedItem = this._owningComboBox.SelectedItem;
				if (selectedItem == null)
				{
					return;
				}
				ComboBox.ComboBoxItemAccessibleObject comboBoxItemAccessibleObject = this.ItemAccessibleObjects[selectedItem] as ComboBox.ComboBoxItemAccessibleObject;
				if (comboBoxItemAccessibleObject != null)
				{
					comboBoxItemAccessibleObject.RaiseAutomationEvent(20012);
				}
			}

			// Token: 0x060056CC RID: 22220 RVA: 0x0016B402 File Offset: 0x00169602
			internal override void SetFocus()
			{
				base.SetFocus();
				base.RaiseAutomationEvent(20005);
			}

			// Token: 0x040037FC RID: 14332
			private ComboBox.ComboBoxChildDropDownButtonUiaProvider _dropDownButtonUiaProvider;

			// Token: 0x040037FD RID: 14333
			private ComboBox.ComboBoxItemAccessibleObjectCollection _itemAccessibleObjects;

			// Token: 0x040037FE RID: 14334
			private ComboBox _owningComboBox;
		}

		// Token: 0x02000570 RID: 1392
		[ComVisible(true)]
		internal class ComboBoxChildEditUiaProvider : ComboBox.ChildAccessibleObject
		{
			// Token: 0x060056CD RID: 22221 RVA: 0x0016B416 File Offset: 0x00169616
			public ComboBoxChildEditUiaProvider(ComboBox owner, IntPtr childEditControlhandle) : base(owner, childEditControlhandle)
			{
				this._owner = owner;
				this._handle = childEditControlhandle;
			}

			// Token: 0x060056CE RID: 22222 RVA: 0x0016B430 File Offset: 0x00169630
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return this._owner.AccessibilityObject;
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
				{
					if (this._owner.DropDownStyle == ComboBoxStyle.Simple)
					{
						return null;
					}
					ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = this._owner.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
					if (comboBoxUiaProvider != null)
					{
						int childFragmentCount = comboBoxUiaProvider.GetChildFragmentCount();
						if (childFragmentCount > 1)
						{
							return comboBoxUiaProvider.GetChildFragment(childFragmentCount - 1);
						}
					}
					return null;
				}
				case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
				{
					ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = this._owner.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
					if (comboBoxUiaProvider != null)
					{
						AccessibleObject childFragment = comboBoxUiaProvider.GetChildFragment(0);
						if (this.RuntimeId != childFragment.RuntimeId)
						{
							return childFragment;
						}
					}
					return null;
				}
				default:
					return base.FragmentNavigate(direction);
				}
			}

			// Token: 0x170014AE RID: 5294
			// (get) Token: 0x060056CF RID: 22223 RVA: 0x0016B4CC File Offset: 0x001696CC
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this._owner.AccessibilityObject;
				}
			}

			// Token: 0x060056D0 RID: 22224 RVA: 0x0016B4DC File Offset: 0x001696DC
			internal override object GetPropertyValue(int propertyID)
			{
				switch (propertyID)
				{
				case 30000:
					return this.RuntimeId;
				case 30001:
					return this.Bounds;
				case 30003:
					return 50004;
				case 30005:
					return this.Name;
				case 30007:
					return string.Empty;
				case 30008:
					return this._owner.Focused;
				case 30009:
					return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
				case 30010:
					return this._owner.Enabled;
				case 30011:
					return "1001";
				case 30013:
					return this.Help ?? string.Empty;
				case 30019:
					return false;
				case 30020:
					return this._handle;
				case 30022:
					return false;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x170014AF RID: 5295
			// (get) Token: 0x060056D1 RID: 22225 RVA: 0x0016B5F4 File Offset: 0x001697F4
			internal override UnsafeNativeMethods.IRawElementProviderSimple HostRawElementProvider
			{
				get
				{
					if (AccessibilityImprovements.Level3)
					{
						UnsafeNativeMethods.IRawElementProviderSimple result;
						UnsafeNativeMethods.UiaHostProviderFromHwnd(new HandleRef(this, this._handle), out result);
						return result;
					}
					return base.HostRawElementProvider;
				}
			}

			// Token: 0x060056D2 RID: 22226 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override bool IsIAccessibleExSupported()
			{
				return true;
			}

			// Token: 0x170014B0 RID: 5296
			// (get) Token: 0x060056D3 RID: 22227 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override int ProviderOptions
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x170014B1 RID: 5297
			// (get) Token: 0x060056D4 RID: 22228 RVA: 0x0016B624 File Offset: 0x00169824
			internal override int[] RuntimeId
			{
				get
				{
					return new int[]
					{
						42,
						this.GetHashCode()
					};
				}
			}

			// Token: 0x040037FF RID: 14335
			private const string COMBO_BOX_EDIT_AUTOMATION_ID = "1001";

			// Token: 0x04003800 RID: 14336
			private ComboBox _owner;

			// Token: 0x04003801 RID: 14337
			private IntPtr _handle;
		}

		// Token: 0x02000571 RID: 1393
		[ComVisible(true)]
		internal class ComboBoxChildListUiaProvider : ComboBox.ChildAccessibleObject
		{
			// Token: 0x060056D5 RID: 22229 RVA: 0x0016B647 File Offset: 0x00169847
			public ComboBoxChildListUiaProvider(ComboBox owningComboBox, IntPtr childListControlhandle) : base(owningComboBox, childListControlhandle)
			{
				this._owningComboBox = owningComboBox;
				this._childListControlhandle = childListControlhandle;
			}

			// Token: 0x060056D6 RID: 22230 RVA: 0x0016B660 File Offset: 0x00169860
			internal override UnsafeNativeMethods.IRawElementProviderFragment ElementProviderFromPoint(double x, double y)
			{
				if (AccessibilityImprovements.Level3)
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					if (systemIAccessibleInternal != null)
					{
						object obj = systemIAccessibleInternal.accHitTest((int)x, (int)y);
						if (obj is int)
						{
							int num = (int)obj;
							return this.GetChildFragment(num - 1);
						}
						return null;
					}
				}
				return base.ElementProviderFromPoint(x, y);
			}

			// Token: 0x060056D7 RID: 22231 RVA: 0x0016B6AC File Offset: 0x001698AC
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.FirstChild)
				{
					return this.GetChildFragment(0);
				}
				if (direction != UnsafeNativeMethods.NavigateDirection.LastChild)
				{
					return base.FragmentNavigate(direction);
				}
				int childFragmentCount = this.GetChildFragmentCount();
				if (childFragmentCount > 0)
				{
					return this.GetChildFragment(childFragmentCount - 1);
				}
				return null;
			}

			// Token: 0x170014B2 RID: 5298
			// (get) Token: 0x060056D8 RID: 22232 RVA: 0x0016B6E9 File Offset: 0x001698E9
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this._owningComboBox.AccessibilityObject;
				}
			}

			// Token: 0x060056D9 RID: 22233 RVA: 0x0016B6F8 File Offset: 0x001698F8
			public AccessibleObject GetChildFragment(int index)
			{
				if (index < 0 || index >= this._owningComboBox.Items.Count)
				{
					return null;
				}
				object key = this._owningComboBox.Items[index];
				ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = this._owningComboBox.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
				return comboBoxUiaProvider.ItemAccessibleObjects[key] as AccessibleObject;
			}

			// Token: 0x060056DA RID: 22234 RVA: 0x0016B752 File Offset: 0x00169952
			public int GetChildFragmentCount()
			{
				return this._owningComboBox.Items.Count;
			}

			// Token: 0x060056DB RID: 22235 RVA: 0x0016B764 File Offset: 0x00169964
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID <= 30037)
				{
					switch (propertyID)
					{
					case 30000:
						return this.RuntimeId;
					case 30001:
						return this.Bounds;
					case 30002:
					case 30004:
					case 30006:
					case 30012:
					case 30014:
					case 30015:
					case 30016:
					case 30017:
					case 30018:
					case 30021:
						break;
					case 30003:
						return 50008;
					case 30005:
						return this.Name;
					case 30007:
						return string.Empty;
					case 30008:
						return false;
					case 30009:
						return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
					case 30010:
						return this._owningComboBox.Enabled;
					case 30011:
						return "1000";
					case 30013:
						return this.Help ?? string.Empty;
					case 30019:
						return false;
					case 30020:
						return this._childListControlhandle;
					case 30022:
						return false;
					default:
						if (propertyID == 30037)
						{
							return true;
						}
						break;
					}
				}
				else
				{
					if (propertyID == 30060)
					{
						return this.CanSelectMultiple;
					}
					if (propertyID == 30061)
					{
						return this.IsSelectionRequired;
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x060056DC RID: 22236 RVA: 0x000E8E48 File Offset: 0x000E7048
			internal override UnsafeNativeMethods.IRawElementProviderFragment GetFocus()
			{
				return this.GetFocused();
			}

			// Token: 0x060056DD RID: 22237 RVA: 0x0016B8C0 File Offset: 0x00169AC0
			public override AccessibleObject GetFocused()
			{
				int selectedIndex = this._owningComboBox.SelectedIndex;
				return this.GetChildFragment(selectedIndex);
			}

			// Token: 0x060056DE RID: 22238 RVA: 0x0016B8E0 File Offset: 0x00169AE0
			internal override UnsafeNativeMethods.IRawElementProviderSimple[] GetSelection()
			{
				int selectedIndex = this._owningComboBox.SelectedIndex;
				AccessibleObject childFragment = this.GetChildFragment(selectedIndex);
				if (childFragment != null)
				{
					return new UnsafeNativeMethods.IRawElementProviderSimple[]
					{
						childFragment
					};
				}
				return new UnsafeNativeMethods.IRawElementProviderSimple[0];
			}

			// Token: 0x170014B3 RID: 5299
			// (get) Token: 0x060056DF RID: 22239 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			internal override bool CanSelectMultiple
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170014B4 RID: 5300
			// (get) Token: 0x060056E0 RID: 22240 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override bool IsSelectionRequired
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060056E1 RID: 22241 RVA: 0x0016B915 File Offset: 0x00169B15
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId == 10018 || patternId == 10001 || base.IsPatternSupported(patternId);
			}

			// Token: 0x170014B5 RID: 5301
			// (get) Token: 0x060056E2 RID: 22242 RVA: 0x0016B930 File Offset: 0x00169B30
			internal override UnsafeNativeMethods.IRawElementProviderSimple HostRawElementProvider
			{
				get
				{
					if (AccessibilityImprovements.Level3)
					{
						UnsafeNativeMethods.IRawElementProviderSimple result;
						UnsafeNativeMethods.UiaHostProviderFromHwnd(new HandleRef(this, this._childListControlhandle), out result);
						return result;
					}
					return base.HostRawElementProvider;
				}
			}

			// Token: 0x170014B6 RID: 5302
			// (get) Token: 0x060056E3 RID: 22243 RVA: 0x0016B960 File Offset: 0x00169B60
			internal override int[] RuntimeId
			{
				get
				{
					return new int[]
					{
						42,
						(int)((long)this._owningComboBox.Handle),
						this._owningComboBox.GetListNativeWindowRuntimeIdPart()
					};
				}
			}

			// Token: 0x170014B7 RID: 5303
			// (get) Token: 0x060056E4 RID: 22244 RVA: 0x0016B99C File Offset: 0x00169B9C
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable;
					if (this._owningComboBox.Focused)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					return accessibleStates;
				}
			}

			// Token: 0x04003802 RID: 14338
			private const string COMBO_BOX_LIST_AUTOMATION_ID = "1000";

			// Token: 0x04003803 RID: 14339
			private ComboBox _owningComboBox;

			// Token: 0x04003804 RID: 14340
			private IntPtr _childListControlhandle;
		}

		// Token: 0x02000572 RID: 1394
		[ComVisible(true)]
		internal class ComboBoxChildTextUiaProvider : AccessibleObject
		{
			// Token: 0x060056E5 RID: 22245 RVA: 0x0016B9C1 File Offset: 0x00169BC1
			public ComboBoxChildTextUiaProvider(ComboBox owner)
			{
				this._owner = owner;
			}

			// Token: 0x170014B8 RID: 5304
			// (get) Token: 0x060056E6 RID: 22246 RVA: 0x0016B9D0 File Offset: 0x00169BD0
			public override Rectangle Bounds
			{
				get
				{
					return this._owner.AccessibilityObject.Bounds;
				}
			}

			// Token: 0x060056E7 RID: 22247 RVA: 0x0000E214 File Offset: 0x0000C414
			internal override int GetChildId()
			{
				return 1;
			}

			// Token: 0x170014B9 RID: 5305
			// (get) Token: 0x060056E8 RID: 22248 RVA: 0x0016B9E2 File Offset: 0x00169BE2
			// (set) Token: 0x060056E9 RID: 22249 RVA: 0x0000701A File Offset: 0x0000521A
			public override string Name
			{
				get
				{
					return this._owner.AccessibilityObject.Name ?? string.Empty;
				}
				set
				{
				}
			}

			// Token: 0x060056EA RID: 22250 RVA: 0x0016BA00 File Offset: 0x00169C00
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return this._owner.AccessibilityObject;
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
				{
					ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = this._owner.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
					if (comboBoxUiaProvider != null)
					{
						int childFragmentCount = comboBoxUiaProvider.GetChildFragmentCount();
						if (childFragmentCount > 1)
						{
							return comboBoxUiaProvider.GetChildFragment(childFragmentCount - 1);
						}
					}
					return null;
				}
				case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
				{
					ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = this._owner.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
					if (comboBoxUiaProvider != null)
					{
						AccessibleObject childFragment = comboBoxUiaProvider.GetChildFragment(0);
						if (this.RuntimeId != childFragment.RuntimeId)
						{
							return childFragment;
						}
					}
					return null;
				}
				default:
					return base.FragmentNavigate(direction);
				}
			}

			// Token: 0x170014BA RID: 5306
			// (get) Token: 0x060056EB RID: 22251 RVA: 0x0016BA8D File Offset: 0x00169C8D
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this._owner.AccessibilityObject;
				}
			}

			// Token: 0x060056EC RID: 22252 RVA: 0x0016BA9C File Offset: 0x00169C9C
			internal override object GetPropertyValue(int propertyID)
			{
				switch (propertyID)
				{
				case 30000:
					return this.RuntimeId;
				case 30001:
					return this.Bounds;
				case 30002:
				case 30004:
				case 30006:
				case 30011:
				case 30012:
					break;
				case 30003:
					return 50020;
				case 30005:
					return this.Name;
				case 30007:
					return string.Empty;
				case 30008:
					return this._owner.Focused;
				case 30009:
					return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
				case 30010:
					return this._owner.Enabled;
				case 30013:
					return this.Help ?? string.Empty;
				default:
					if (propertyID == 30019 || propertyID == 30022)
					{
						return false;
					}
					break;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x170014BB RID: 5307
			// (get) Token: 0x060056ED RID: 22253 RVA: 0x0016BB88 File Offset: 0x00169D88
			internal override int[] RuntimeId
			{
				get
				{
					return new int[]
					{
						42,
						(int)((long)this._owner.Handle),
						this._owner.GetHashCode(),
						this.GetHashCode(),
						this.GetChildId()
					};
				}
			}

			// Token: 0x170014BC RID: 5308
			// (get) Token: 0x060056EE RID: 22254 RVA: 0x0016BBD8 File Offset: 0x00169DD8
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable;
					if (this._owner.Focused)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					return accessibleStates;
				}
			}

			// Token: 0x04003805 RID: 14341
			private const int COMBOBOX_TEXT_ACC_ITEM_INDEX = 1;

			// Token: 0x04003806 RID: 14342
			private ComboBox _owner;
		}

		// Token: 0x02000573 RID: 1395
		[ComVisible(true)]
		internal class ComboBoxChildDropDownButtonUiaProvider : AccessibleObject
		{
			// Token: 0x060056EF RID: 22255 RVA: 0x0016BBFD File Offset: 0x00169DFD
			public ComboBoxChildDropDownButtonUiaProvider(ComboBox owner, IntPtr comboBoxControlhandle)
			{
				this._owner = owner;
				base.UseStdAccessibleObjects(comboBoxControlhandle);
			}

			// Token: 0x170014BD RID: 5309
			// (get) Token: 0x060056F0 RID: 22256 RVA: 0x0016BC13 File Offset: 0x00169E13
			// (set) Token: 0x060056F1 RID: 22257 RVA: 0x0016BC24 File Offset: 0x00169E24
			public override string Name
			{
				get
				{
					return this.get_accNameInternal(2);
				}
				set
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					systemIAccessibleInternal.set_accName(2, value);
				}
			}

			// Token: 0x170014BE RID: 5310
			// (get) Token: 0x060056F2 RID: 22258 RVA: 0x0016BC48 File Offset: 0x00169E48
			public override Rectangle Bounds
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					int x;
					int y;
					int width;
					int height;
					systemIAccessibleInternal.accLocation(out x, out y, out width, out height, 2);
					return new Rectangle(x, y, width, height);
				}
			}

			// Token: 0x170014BF RID: 5311
			// (get) Token: 0x060056F3 RID: 22259 RVA: 0x0016BC7C File Offset: 0x00169E7C
			public override string DefaultAction
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return systemIAccessibleInternal.get_accDefaultAction(2);
				}
			}

			// Token: 0x060056F4 RID: 22260 RVA: 0x0016BC9C File Offset: 0x00169E9C
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				if (direction == UnsafeNativeMethods.NavigateDirection.Parent)
				{
					return this._owner.AccessibilityObject;
				}
				if (direction == UnsafeNativeMethods.NavigateDirection.PreviousSibling)
				{
					ComboBox.ComboBoxUiaProvider comboBoxUiaProvider = this._owner.AccessibilityObject as ComboBox.ComboBoxUiaProvider;
					if (comboBoxUiaProvider != null)
					{
						int childFragmentCount = comboBoxUiaProvider.GetChildFragmentCount();
						if (childFragmentCount > 1)
						{
							return comboBoxUiaProvider.GetChildFragment(childFragmentCount - 1);
						}
					}
					return null;
				}
				return base.FragmentNavigate(direction);
			}

			// Token: 0x170014C0 RID: 5312
			// (get) Token: 0x060056F5 RID: 22261 RVA: 0x0016BCEE File Offset: 0x00169EEE
			internal override UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot
			{
				get
				{
					return this._owner.AccessibilityObject;
				}
			}

			// Token: 0x060056F6 RID: 22262 RVA: 0x0000E211 File Offset: 0x0000C411
			internal override int GetChildId()
			{
				return 2;
			}

			// Token: 0x060056F7 RID: 22263 RVA: 0x0016BCFC File Offset: 0x00169EFC
			internal override object GetPropertyValue(int propertyID)
			{
				switch (propertyID)
				{
				case 30000:
					return this.RuntimeId;
				case 30001:
					return this.BoundingRectangle;
				case 30002:
				case 30004:
				case 30006:
				case 30011:
				case 30012:
					break;
				case 30003:
					return 50000;
				case 30005:
					return this.Name;
				case 30007:
					return this.KeyboardShortcut;
				case 30008:
					return this._owner.Focused;
				case 30009:
					return (this.State & AccessibleStates.Focusable) == AccessibleStates.Focusable;
				case 30010:
					return this._owner.Enabled;
				case 30013:
					return this.Help ?? string.Empty;
				default:
					if (propertyID == 30019)
					{
						return false;
					}
					if (propertyID == 30022)
					{
						return (this.State & AccessibleStates.Offscreen) == AccessibleStates.Offscreen;
					}
					break;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x170014C1 RID: 5313
			// (get) Token: 0x060056F8 RID: 22264 RVA: 0x0016BE08 File Offset: 0x0016A008
			public override string Help
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return systemIAccessibleInternal.get_accHelp(2);
				}
			}

			// Token: 0x170014C2 RID: 5314
			// (get) Token: 0x060056F9 RID: 22265 RVA: 0x0016BE28 File Offset: 0x0016A028
			public override string KeyboardShortcut
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return systemIAccessibleInternal.get_accKeyboardShortcut(2);
				}
			}

			// Token: 0x060056FA RID: 22266 RVA: 0x0016BE48 File Offset: 0x0016A048
			internal override bool IsPatternSupported(int patternId)
			{
				return patternId == 10018 || patternId == 10000 || base.IsPatternSupported(patternId);
			}

			// Token: 0x170014C3 RID: 5315
			// (get) Token: 0x060056FB RID: 22267 RVA: 0x0016BE64 File Offset: 0x0016A064
			public override AccessibleRole Role
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return (AccessibleRole)systemIAccessibleInternal.get_accRole(2);
				}
			}

			// Token: 0x170014C4 RID: 5316
			// (get) Token: 0x060056FC RID: 22268 RVA: 0x0016BE8C File Offset: 0x0016A08C
			internal override int[] RuntimeId
			{
				get
				{
					return new int[]
					{
						42,
						(int)((long)this._owner.Handle),
						this._owner.GetHashCode(),
						61453,
						2
					};
				}
			}

			// Token: 0x170014C5 RID: 5317
			// (get) Token: 0x060056FD RID: 22269 RVA: 0x0016BED4 File Offset: 0x0016A0D4
			public override AccessibleStates State
			{
				get
				{
					IAccessible systemIAccessibleInternal = base.GetSystemIAccessibleInternal();
					return (AccessibleStates)systemIAccessibleInternal.get_accState(2);
				}
			}

			// Token: 0x04003807 RID: 14343
			private const int COMBOBOX_DROPDOWN_BUTTON_ACC_ITEM_INDEX = 2;

			// Token: 0x04003808 RID: 14344
			private ComboBox _owner;
		}

		// Token: 0x02000574 RID: 1396
		private sealed class ACNativeWindow : NativeWindow
		{
			// Token: 0x060056FE RID: 22270 RVA: 0x0016BEF9 File Offset: 0x0016A0F9
			internal ACNativeWindow(IntPtr acHandle)
			{
				base.AssignHandle(acHandle);
				ComboBox.ACNativeWindow.ACWindows.Add(acHandle, this);
				UnsafeNativeMethods.EnumChildWindows(new HandleRef(this, acHandle), new NativeMethods.EnumChildrenCallback(ComboBox.ACNativeWindow.RegisterACWindowRecursive), NativeMethods.NullHandleRef);
			}

			// Token: 0x060056FF RID: 22271 RVA: 0x0016BF38 File Offset: 0x0016A138
			private static bool RegisterACWindowRecursive(IntPtr handle, IntPtr lparam)
			{
				if (!ComboBox.ACNativeWindow.ACWindows.ContainsKey(handle))
				{
					ComboBox.ACNativeWindow acnativeWindow = new ComboBox.ACNativeWindow(handle);
				}
				return true;
			}

			// Token: 0x170014C6 RID: 5318
			// (get) Token: 0x06005700 RID: 22272 RVA: 0x0016BF5F File Offset: 0x0016A15F
			internal bool Visible
			{
				get
				{
					return SafeNativeMethods.IsWindowVisible(new HandleRef(this, base.Handle));
				}
			}

			// Token: 0x170014C7 RID: 5319
			// (get) Token: 0x06005701 RID: 22273 RVA: 0x0016BF74 File Offset: 0x0016A174
			internal static bool AutoCompleteActive
			{
				get
				{
					if (ComboBox.ACNativeWindow.inWndProcCnt > 0)
					{
						return true;
					}
					foreach (object obj in ComboBox.ACNativeWindow.ACWindows.Values)
					{
						ComboBox.ACNativeWindow acnativeWindow = obj as ComboBox.ACNativeWindow;
						if (acnativeWindow != null && acnativeWindow.Visible)
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x06005702 RID: 22274 RVA: 0x0016BFEC File Offset: 0x0016A1EC
			protected override void WndProc(ref Message m)
			{
				ComboBox.ACNativeWindow.inWndProcCnt++;
				try
				{
					base.WndProc(ref m);
				}
				finally
				{
					ComboBox.ACNativeWindow.inWndProcCnt--;
				}
				if (m.Msg == 130)
				{
					ComboBox.ACNativeWindow.ACWindows.Remove(base.Handle);
				}
			}

			// Token: 0x06005703 RID: 22275 RVA: 0x0016C050 File Offset: 0x0016A250
			internal static void RegisterACWindow(IntPtr acHandle, bool subclass)
			{
				if (subclass && ComboBox.ACNativeWindow.ACWindows.ContainsKey(acHandle) && ComboBox.ACNativeWindow.ACWindows[acHandle] == null)
				{
					ComboBox.ACNativeWindow.ACWindows.Remove(acHandle);
				}
				if (!ComboBox.ACNativeWindow.ACWindows.ContainsKey(acHandle))
				{
					if (subclass)
					{
						ComboBox.ACNativeWindow acnativeWindow = new ComboBox.ACNativeWindow(acHandle);
						return;
					}
					ComboBox.ACNativeWindow.ACWindows.Add(acHandle, null);
				}
			}

			// Token: 0x06005704 RID: 22276 RVA: 0x0016C0C4 File Offset: 0x0016A2C4
			internal static void ClearNullACWindows()
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in ComboBox.ACNativeWindow.ACWindows)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (dictionaryEntry.Value == null)
					{
						arrayList.Add(dictionaryEntry.Key);
					}
				}
				foreach (object obj2 in arrayList)
				{
					IntPtr intPtr = (IntPtr)obj2;
					ComboBox.ACNativeWindow.ACWindows.Remove(intPtr);
				}
			}

			// Token: 0x04003809 RID: 14345
			internal static int inWndProcCnt;

			// Token: 0x0400380A RID: 14346
			private static Hashtable ACWindows = new Hashtable();
		}

		// Token: 0x02000575 RID: 1397
		private class AutoCompleteDropDownFinder
		{
			// Token: 0x06005706 RID: 22278 RVA: 0x0016C190 File Offset: 0x0016A390
			internal void FindDropDowns()
			{
				this.FindDropDowns(true);
			}

			// Token: 0x06005707 RID: 22279 RVA: 0x0016C199 File Offset: 0x0016A399
			internal void FindDropDowns(bool subclass)
			{
				if (!subclass)
				{
					ComboBox.ACNativeWindow.ClearNullACWindows();
				}
				this.shouldSubClass = subclass;
				UnsafeNativeMethods.EnumThreadWindows(SafeNativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWindowsCallback(this.Callback), new HandleRef(null, IntPtr.Zero));
			}

			// Token: 0x06005708 RID: 22280 RVA: 0x0016C1CC File Offset: 0x0016A3CC
			private bool Callback(IntPtr hWnd, IntPtr lParam)
			{
				HandleRef hRef = new HandleRef(null, hWnd);
				if (ComboBox.AutoCompleteDropDownFinder.GetClassName(hRef) == "Auto-Suggest Dropdown")
				{
					ComboBox.ACNativeWindow.RegisterACWindow(hRef.Handle, this.shouldSubClass);
				}
				return true;
			}

			// Token: 0x06005709 RID: 22281 RVA: 0x0016C208 File Offset: 0x0016A408
			private static string GetClassName(HandleRef hRef)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				UnsafeNativeMethods.GetClassName(hRef, stringBuilder, 256);
				return stringBuilder.ToString();
			}

			// Token: 0x0400380B RID: 14347
			private const int MaxClassName = 256;

			// Token: 0x0400380C RID: 14348
			private const string AutoCompleteClassName = "Auto-Suggest Dropdown";

			// Token: 0x0400380D RID: 14349
			private bool shouldSubClass;
		}

		// Token: 0x02000576 RID: 1398
		internal class FlatComboAdapter
		{
			// Token: 0x0600570B RID: 22283 RVA: 0x0016C234 File Offset: 0x0016A434
			public FlatComboAdapter(ComboBox comboBox, bool smallButton)
			{
				if ((!ComboBox.FlatComboAdapter.isScalingInitialized && DpiHelper.IsScalingRequired) || DpiHelper.EnableDpiChangedMessageHandling)
				{
					ComboBox.FlatComboAdapter.Offset2Pixels = comboBox.LogicalToDeviceUnits(ComboBox.FlatComboAdapter.OFFSET_2PIXELS);
					ComboBox.FlatComboAdapter.isScalingInitialized = true;
				}
				this.clientRect = comboBox.ClientRectangle;
				int horizontalScrollBarArrowWidthForDpi = SystemInformation.GetHorizontalScrollBarArrowWidthForDpi(comboBox.deviceDpi);
				this.outerBorder = new Rectangle(this.clientRect.Location, new Size(this.clientRect.Width - 1, this.clientRect.Height - 1));
				this.innerBorder = new Rectangle(this.outerBorder.X + 1, this.outerBorder.Y + 1, this.outerBorder.Width - horizontalScrollBarArrowWidthForDpi - 2, this.outerBorder.Height - 2);
				this.innerInnerBorder = new Rectangle(this.innerBorder.X + 1, this.innerBorder.Y + 1, this.innerBorder.Width - 2, this.innerBorder.Height - 2);
				this.dropDownRect = new Rectangle(this.innerBorder.Right + 1, this.innerBorder.Y, horizontalScrollBarArrowWidthForDpi, this.innerBorder.Height + 1);
				if (smallButton)
				{
					this.whiteFillRect = this.dropDownRect;
					this.whiteFillRect.Width = 5;
					this.dropDownRect.X = this.dropDownRect.X + 5;
					this.dropDownRect.Width = this.dropDownRect.Width - 5;
				}
				this.origRightToLeft = comboBox.RightToLeft;
				if (this.origRightToLeft == RightToLeft.Yes)
				{
					this.innerBorder.X = this.clientRect.Width - this.innerBorder.Right;
					this.innerInnerBorder.X = this.clientRect.Width - this.innerInnerBorder.Right;
					this.dropDownRect.X = this.clientRect.Width - this.dropDownRect.Right;
					this.whiteFillRect.X = this.clientRect.Width - this.whiteFillRect.Right + 1;
				}
			}

			// Token: 0x0600570C RID: 22284 RVA: 0x0016C453 File Offset: 0x0016A653
			public bool IsValid(ComboBox combo)
			{
				return combo.ClientRectangle == this.clientRect && combo.RightToLeft == this.origRightToLeft;
			}

			// Token: 0x0600570D RID: 22285 RVA: 0x0016C478 File Offset: 0x0016A678
			public virtual void DrawFlatCombo(ComboBox comboBox, Graphics g)
			{
				if (comboBox.DropDownStyle == ComboBoxStyle.Simple)
				{
					return;
				}
				Color outerBorderColor = this.GetOuterBorderColor(comboBox);
				Color innerBorderColor = this.GetInnerBorderColor(comboBox);
				bool flag = comboBox.RightToLeft == RightToLeft.Yes;
				this.DrawFlatComboDropDown(comboBox, g, this.dropDownRect);
				if (!LayoutUtils.IsZeroWidthOrHeight(this.whiteFillRect))
				{
					using (Brush brush = new SolidBrush(innerBorderColor))
					{
						g.FillRectangle(brush, this.whiteFillRect);
					}
				}
				if (outerBorderColor.IsSystemColor)
				{
					Pen pen = SystemPens.FromSystemColor(outerBorderColor);
					g.DrawRectangle(pen, this.outerBorder);
					if (flag)
					{
						g.DrawRectangle(pen, new Rectangle(this.outerBorder.X, this.outerBorder.Y, this.dropDownRect.Width + 1, this.outerBorder.Height));
					}
					else
					{
						g.DrawRectangle(pen, new Rectangle(this.dropDownRect.X, this.outerBorder.Y, this.outerBorder.Right - this.dropDownRect.X, this.outerBorder.Height));
					}
				}
				else
				{
					using (Pen pen2 = new Pen(outerBorderColor))
					{
						g.DrawRectangle(pen2, this.outerBorder);
						if (flag)
						{
							g.DrawRectangle(pen2, new Rectangle(this.outerBorder.X, this.outerBorder.Y, this.dropDownRect.Width + 1, this.outerBorder.Height));
						}
						else
						{
							g.DrawRectangle(pen2, new Rectangle(this.dropDownRect.X, this.outerBorder.Y, this.outerBorder.Right - this.dropDownRect.X, this.outerBorder.Height));
						}
					}
				}
				if (innerBorderColor.IsSystemColor)
				{
					Pen pen3 = SystemPens.FromSystemColor(innerBorderColor);
					g.DrawRectangle(pen3, this.innerBorder);
					g.DrawRectangle(pen3, this.innerInnerBorder);
				}
				else
				{
					using (Pen pen4 = new Pen(innerBorderColor))
					{
						g.DrawRectangle(pen4, this.innerBorder);
						g.DrawRectangle(pen4, this.innerInnerBorder);
					}
				}
				if (!comboBox.Enabled || comboBox.FlatStyle == FlatStyle.Popup)
				{
					bool focused = comboBox.ContainsFocus || comboBox.MouseIsOver;
					Color popupOuterBorderColor = this.GetPopupOuterBorderColor(comboBox, focused);
					using (Pen pen5 = new Pen(popupOuterBorderColor))
					{
						Pen pen6 = comboBox.Enabled ? pen5 : SystemPens.Control;
						if (flag)
						{
							g.DrawRectangle(pen6, new Rectangle(this.outerBorder.X, this.outerBorder.Y, this.dropDownRect.Width + 1, this.outerBorder.Height));
						}
						else
						{
							g.DrawRectangle(pen6, new Rectangle(this.dropDownRect.X, this.outerBorder.Y, this.outerBorder.Right - this.dropDownRect.X, this.outerBorder.Height));
						}
						g.DrawRectangle(pen5, this.outerBorder);
					}
				}
			}

			// Token: 0x0600570E RID: 22286 RVA: 0x0016C7C0 File Offset: 0x0016A9C0
			protected virtual void DrawFlatComboDropDown(ComboBox comboBox, Graphics g, Rectangle dropDownRect)
			{
				g.FillRectangle(SystemBrushes.Control, dropDownRect);
				Brush brush = comboBox.Enabled ? SystemBrushes.ControlText : SystemBrushes.ControlDark;
				Point point = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
				if (this.origRightToLeft == RightToLeft.Yes)
				{
					point.X -= dropDownRect.Width % 2;
				}
				else
				{
					point.X += dropDownRect.Width % 2;
				}
				g.FillPolygon(brush, new Point[]
				{
					new Point(point.X - ComboBox.FlatComboAdapter.Offset2Pixels, point.Y - 1),
					new Point(point.X + ComboBox.FlatComboAdapter.Offset2Pixels + 1, point.Y - 1),
					new Point(point.X, point.Y + ComboBox.FlatComboAdapter.Offset2Pixels)
				});
			}

			// Token: 0x0600570F RID: 22287 RVA: 0x0016C8C3 File Offset: 0x0016AAC3
			protected virtual Color GetOuterBorderColor(ComboBox comboBox)
			{
				if (!comboBox.Enabled)
				{
					return SystemColors.ControlDark;
				}
				return SystemColors.Window;
			}

			// Token: 0x06005710 RID: 22288 RVA: 0x0016C8D8 File Offset: 0x0016AAD8
			protected virtual Color GetPopupOuterBorderColor(ComboBox comboBox, bool focused)
			{
				if (!comboBox.Enabled)
				{
					return SystemColors.ControlDark;
				}
				if (!focused)
				{
					return SystemColors.Window;
				}
				return SystemColors.ControlDark;
			}

			// Token: 0x06005711 RID: 22289 RVA: 0x0016C8F6 File Offset: 0x0016AAF6
			protected virtual Color GetInnerBorderColor(ComboBox comboBox)
			{
				if (!comboBox.Enabled)
				{
					return SystemColors.Control;
				}
				return comboBox.BackColor;
			}

			// Token: 0x06005712 RID: 22290 RVA: 0x0016C90C File Offset: 0x0016AB0C
			public void ValidateOwnerDrawRegions(ComboBox comboBox, Rectangle updateRegionBox)
			{
				if (comboBox != null)
				{
					return;
				}
				Rectangle r = new Rectangle(0, 0, comboBox.Width, this.innerBorder.Top);
				Rectangle r2 = new Rectangle(0, this.innerBorder.Bottom, comboBox.Width, comboBox.Height - this.innerBorder.Bottom);
				Rectangle r3 = new Rectangle(0, 0, this.innerBorder.Left, comboBox.Height);
				Rectangle r4 = new Rectangle(this.innerBorder.Right, 0, comboBox.Width - this.innerBorder.Right, comboBox.Height);
				if (r.IntersectsWith(updateRegionBox))
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(r);
					SafeNativeMethods.ValidateRect(new HandleRef(comboBox, comboBox.Handle), ref rect);
				}
				if (r2.IntersectsWith(updateRegionBox))
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(r2);
					SafeNativeMethods.ValidateRect(new HandleRef(comboBox, comboBox.Handle), ref rect);
				}
				if (r3.IntersectsWith(updateRegionBox))
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(r3);
					SafeNativeMethods.ValidateRect(new HandleRef(comboBox, comboBox.Handle), ref rect);
				}
				if (r4.IntersectsWith(updateRegionBox))
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(r4);
					SafeNativeMethods.ValidateRect(new HandleRef(comboBox, comboBox.Handle), ref rect);
				}
			}

			// Token: 0x0400380E RID: 14350
			private Rectangle outerBorder;

			// Token: 0x0400380F RID: 14351
			private Rectangle innerBorder;

			// Token: 0x04003810 RID: 14352
			private Rectangle innerInnerBorder;

			// Token: 0x04003811 RID: 14353
			internal Rectangle dropDownRect;

			// Token: 0x04003812 RID: 14354
			private Rectangle whiteFillRect;

			// Token: 0x04003813 RID: 14355
			private Rectangle clientRect;

			// Token: 0x04003814 RID: 14356
			private RightToLeft origRightToLeft;

			// Token: 0x04003815 RID: 14357
			private const int WhiteFillRectWidth = 5;

			// Token: 0x04003816 RID: 14358
			private static bool isScalingInitialized = false;

			// Token: 0x04003817 RID: 14359
			private static int OFFSET_2PIXELS = 2;

			// Token: 0x04003818 RID: 14360
			protected static int Offset2Pixels = ComboBox.FlatComboAdapter.OFFSET_2PIXELS;
		}

		// Token: 0x02000577 RID: 1399
		internal enum ChildWindowType
		{
			// Token: 0x0400381A RID: 14362
			ListBox,
			// Token: 0x0400381B RID: 14363
			Edit,
			// Token: 0x0400381C RID: 14364
			DropDownList
		}
	}
}
