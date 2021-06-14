using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a selectable option displayed on a <see cref="T:System.Windows.Forms.MenuStrip" /> or <see cref="T:System.Windows.Forms.ContextMenuStrip" />. Although <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> replaces and adds functionality to the <see cref="T:System.Windows.Forms.MenuItem" /> control of previous versions, <see cref="T:System.Windows.Forms.MenuItem" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x020003D6 RID: 982
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	[DesignerSerializer("System.Windows.Forms.Design.ToolStripMenuItemCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ToolStripMenuItem : ToolStripDropDownItem
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> class.</summary>
		// Token: 0x060040FA RID: 16634 RVA: 0x00118360 File Offset: 0x00116560
		public ToolStripMenuItem()
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> class that displays the specified text.</summary>
		/// <param name="text">The text to display on the menu item.</param>
		// Token: 0x060040FB RID: 16635 RVA: 0x001183CC File Offset: 0x001165CC
		public ToolStripMenuItem(string text) : base(text, null, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> class that displays the specified <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
		// Token: 0x060040FC RID: 16636 RVA: 0x00118438 File Offset: 0x00116638
		public ToolStripMenuItem(Image image) : base(null, image, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> class that displays the specified text and image.</summary>
		/// <param name="text">The text to display on the menu item.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
		// Token: 0x060040FD RID: 16637 RVA: 0x001184A4 File Offset: 0x001166A4
		public ToolStripMenuItem(string text, Image image) : base(text, image, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> class that displays the specified text and image and that does the specified action when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is clicked.</summary>
		/// <param name="text">The text to display on the menu item.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when the control is clicked.</param>
		// Token: 0x060040FE RID: 16638 RVA: 0x00118510 File Offset: 0x00116710
		public ToolStripMenuItem(string text, Image image, EventHandler onClick) : base(text, image, onClick)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> class with the specified name that displays the specified text and image that does the specified action when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is clicked.</summary>
		/// <param name="text">The text to display on the menu item.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when the control is clicked.</param>
		/// <param name="name">The name of the menu item.</param>
		// Token: 0x060040FF RID: 16639 RVA: 0x0011857C File Offset: 0x0011677C
		public ToolStripMenuItem(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> class that displays the specified text and image and that contains the specified <see cref="T:System.Windows.Forms.ToolStripItem" /> collection.</summary>
		/// <param name="text">The text to display on the menu item.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
		/// <param name="dropDownItems">The menu items to display when the control is clicked.</param>
		// Token: 0x06004100 RID: 16640 RVA: 0x001185EC File Offset: 0x001167EC
		public ToolStripMenuItem(string text, Image image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> class that displays the specified text and image, does the specified action when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is clicked, and displays the specified shortcut keys.</summary>
		/// <param name="text">The text to display on the menu item.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when the control is clicked.</param>
		/// <param name="shortcutKeys">One of the values of <see cref="T:System.Windows.Forms.Keys" /> that represents the shortcut key for the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</param>
		// Token: 0x06004101 RID: 16641 RVA: 0x00118658 File Offset: 0x00116858
		public ToolStripMenuItem(string text, Image image, EventHandler onClick, Keys shortcutKeys) : base(text, image, onClick)
		{
			this.Initialize();
			this.ShortcutKeys = shortcutKeys;
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x001186CC File Offset: 0x001168CC
		internal ToolStripMenuItem(Form mdiForm)
		{
			this.Initialize();
			base.Properties.SetObject(ToolStripMenuItem.PropMdiForm, mdiForm);
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00118748 File Offset: 0x00116948
		internal ToolStripMenuItem(IntPtr hMenu, int nativeMenuCommandId, IWin32Window targetWindow)
		{
			this.Initialize();
			this.Overflow = ToolStripItemOverflow.Never;
			this.nativeMenuCommandID = nativeMenuCommandId;
			this.targetWindowHandle = Control.GetSafeHandle(targetWindow);
			this.nativeMenuHandle = hMenu;
			this.Image = this.GetNativeMenuItemImage();
			base.ImageScaling = ToolStripItemImageScaling.None;
			string nativeMenuItemTextAndShortcut = this.GetNativeMenuItemTextAndShortcut();
			if (nativeMenuItemTextAndShortcut != null)
			{
				string[] array = nativeMenuItemTextAndShortcut.Split(new char[]
				{
					'\t'
				});
				if (array.Length >= 1)
				{
					this.Text = array[0];
				}
				if (array.Length >= 2)
				{
					this.ShowShortcutKeys = true;
					this.ShortcutKeyDisplayString = array[1];
				}
			}
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00118826 File Offset: 0x00116A26
		internal override void AutoHide(ToolStripItem otherItemBeingSelected)
		{
			if (base.IsOnDropDown)
			{
				ToolStripMenuItem.MenuTimer.Transition(this, otherItemBeingSelected as ToolStripMenuItem);
				return;
			}
			base.AutoHide(otherItemBeingSelected);
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x00118849 File Offset: 0x00116A49
		private void ClearShortcutCache()
		{
			this.cachedShortcutSize = Size.Empty;
			this.cachedShortcutText = null;
		}

		/// <summary>Creates a generic <see cref="T:System.Windows.Forms.ToolStripDropDown" /> for which events can be defined.</summary>
		/// <returns>A generic <see cref="T:System.Windows.Forms.ToolStripDropDown" /> for which can be defined.</returns>
		// Token: 0x06004106 RID: 16646 RVA: 0x0010DEC0 File Offset: 0x0010C0C0
		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripDropDownMenu(this, true);
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x0011885D File Offset: 0x00116A5D
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripMenuItemInternalLayout(this);
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</returns>
		// Token: 0x06004108 RID: 16648 RVA: 0x00118865 File Offset: 0x00116A65
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripMenuItem.ToolStripMenuItemAccessibleObject(this);
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x00118870 File Offset: 0x00116A70
		private void Initialize()
		{
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.scaledDefaultPadding = DpiHelper.LogicalToDeviceUnits(ToolStripMenuItem.defaultPadding, 0);
				this.scaledDefaultDropDownPadding = DpiHelper.LogicalToDeviceUnits(ToolStripMenuItem.defaultDropDownPadding, 0);
				this.scaledCheckMarkBitmapSize = DpiHelper.LogicalToDeviceUnits(ToolStripMenuItem.checkMarkBitmapSize, 0);
			}
			this.Overflow = ToolStripItemOverflow.Never;
			base.MouseDownAndUpMustBeInSameItem = false;
			base.SupportsDisabledHotTracking = true;
		}

		/// <summary>Gets the default size of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" /> of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />, measured in pixels. The default is 100 pixels horizontally.</returns>
		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x0600410A RID: 16650 RVA: 0x001188CC File Offset: 0x00116ACC
		protected override Size DefaultSize
		{
			get
			{
				if (!DpiHelper.EnableToolStripPerMonitorV2HighDpiImprovements)
				{
					return new Size(32, 19);
				}
				return DpiHelper.LogicalToDeviceUnits(new Size(32, 19), this.DeviceDpi);
			}
		}

		/// <summary>Gets the spacing between the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> and an adjacent item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value representing the spacing.</returns>
		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x0600410B RID: 16651 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected internal override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		/// <summary>Gets the internal spacing within the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value representing the spacing.</returns>
		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x0600410C RID: 16652 RVA: 0x001188F3 File Offset: 0x00116AF3
		protected override Padding DefaultPadding
		{
			get
			{
				if (base.IsOnDropDown)
				{
					return this.scaledDefaultDropDownPadding;
				}
				return this.scaledDefaultPadding;
			}
		}

		/// <summary>Gets or sets a value indicating whether the control is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the control is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x0600410D RID: 16653 RVA: 0x0011890C File Offset: 0x00116B0C
		// (set) Token: 0x0600410E RID: 16654 RVA: 0x0011895D File Offset: 0x00116B5D
		public override bool Enabled
		{
			get
			{
				if (this.nativeMenuCommandID != -1)
				{
					return base.Enabled && this.nativeMenuHandle != IntPtr.Zero && this.targetWindowHandle != IntPtr.Zero && this.GetNativeMenuItemEnabled();
				}
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is checked.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is checked or is in an indeterminate state; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x0600410F RID: 16655 RVA: 0x00118966 File Offset: 0x00116B66
		// (set) Token: 0x06004110 RID: 16656 RVA: 0x00118971 File Offset: 0x00116B71
		[Bindable(true)]
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("CheckBoxCheckedDescr")]
		public bool Checked
		{
			get
			{
				return this.CheckState > CheckState.Unchecked;
			}
			set
			{
				if (value != this.Checked)
				{
					this.CheckState = (value ? CheckState.Checked : CheckState.Unchecked);
					base.InvokePaint();
				}
			}
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x00118990 File Offset: 0x00116B90
		internal Image CheckedImage
		{
			get
			{
				CheckState checkState = this.CheckState;
				if (checkState == CheckState.Indeterminate)
				{
					if (ToolStripMenuItem.indeterminateCheckedImage == null)
					{
						if (DpiHelper.EnableToolStripHighDpiImprovements)
						{
							ToolStripMenuItem.indeterminateCheckedImage = ToolStripMenuItem.GetBitmapFromIcon("IndeterminateChecked.ico", this.scaledCheckMarkBitmapSize);
						}
						else
						{
							Bitmap bitmap = new Bitmap(typeof(ToolStripMenuItem), "IndeterminateChecked.bmp");
							if (bitmap != null)
							{
								bitmap.MakeTransparent(bitmap.GetPixel(1, 1));
								if (DpiHelper.IsScalingRequired)
								{
									DpiHelper.ScaleBitmapLogicalToDevice(ref bitmap, 0);
								}
								ToolStripMenuItem.indeterminateCheckedImage = bitmap;
							}
						}
					}
					return ToolStripMenuItem.indeterminateCheckedImage;
				}
				if (checkState == CheckState.Checked)
				{
					if (ToolStripMenuItem.checkedImage == null)
					{
						if (DpiHelper.EnableToolStripHighDpiImprovements)
						{
							ToolStripMenuItem.checkedImage = ToolStripMenuItem.GetBitmapFromIcon("Checked.ico", this.scaledCheckMarkBitmapSize);
						}
						else
						{
							Bitmap bitmap2 = new Bitmap(typeof(ToolStripMenuItem), "Checked.bmp");
							if (bitmap2 != null)
							{
								bitmap2.MakeTransparent(bitmap2.GetPixel(1, 1));
								if (DpiHelper.IsScalingRequired)
								{
									DpiHelper.ScaleBitmapLogicalToDevice(ref bitmap2, 0);
								}
								ToolStripMenuItem.checkedImage = bitmap2;
							}
						}
					}
					return ToolStripMenuItem.checkedImage;
				}
				return null;
			}
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x00118A7C File Offset: 0x00116C7C
		private static Bitmap GetBitmapFromIcon(string iconName, Size desiredIconSize)
		{
			Bitmap bitmap = null;
			Icon icon = new Icon(typeof(ToolStripMenuItem), iconName);
			if (icon != null)
			{
				Icon icon2 = new Icon(icon, desiredIconSize);
				if (icon2 != null)
				{
					try
					{
						bitmap = icon2.ToBitmap();
						if (bitmap != null)
						{
							bitmap.MakeTransparent(bitmap.GetPixel(1, 1));
							if (DpiHelper.IsScalingRequired && (bitmap.Size.Width != desiredIconSize.Width || bitmap.Size.Height != desiredIconSize.Height))
							{
								Bitmap bitmap2 = DpiHelper.CreateResizedBitmap(bitmap, desiredIconSize);
								if (bitmap2 != null)
								{
									bitmap.Dispose();
									bitmap = bitmap2;
								}
							}
						}
					}
					finally
					{
						icon.Dispose();
						icon2.Dispose();
					}
				}
			}
			return bitmap;
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> should automatically appear checked and unchecked when clicked.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> should automatically appear checked when clicked; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06004113 RID: 16659 RVA: 0x00118B30 File Offset: 0x00116D30
		// (set) Token: 0x06004114 RID: 16660 RVA: 0x00118B38 File Offset: 0x00116D38
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripButtonCheckOnClickDescr")]
		public bool CheckOnClick
		{
			get
			{
				return this.checkOnClick;
			}
			set
			{
				this.checkOnClick = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is in the checked, unchecked, or indeterminate state.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CheckState" /> values. The default is <see langword="Unchecked" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The <see cref="P:System.Windows.Forms.ToolStripMenuItem.CheckState" /> property is not set to one of the <see cref="T:System.Windows.Forms.CheckState" /> values. </exception>
		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x00118B44 File Offset: 0x00116D44
		// (set) Token: 0x06004116 RID: 16662 RVA: 0x00118B78 File Offset: 0x00116D78
		[Bindable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(CheckState.Unchecked)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("CheckBoxCheckStateDescr")]
		public CheckState CheckState
		{
			get
			{
				bool flag = false;
				object obj = base.Properties.GetInteger(ToolStripMenuItem.PropCheckState, out flag);
				if (!flag)
				{
					return CheckState.Unchecked;
				}
				return (CheckState)obj;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
				}
				if (value != this.CheckState)
				{
					base.Properties.SetInteger(ToolStripMenuItem.PropCheckState, (int)value);
					this.OnCheckedChanged(EventArgs.Empty);
					this.OnCheckStateChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripMenuItem.Checked" /> property changes.</summary>
		// Token: 0x14000344 RID: 836
		// (add) Token: 0x06004117 RID: 16663 RVA: 0x00118BDB File Offset: 0x00116DDB
		// (remove) Token: 0x06004118 RID: 16664 RVA: 0x00118BEE File Offset: 0x00116DEE
		[SRDescription("CheckBoxOnCheckedChangedDescr")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripMenuItem.EventCheckedChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripMenuItem.EventCheckedChanged, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripMenuItem.CheckState" /> property changes.</summary>
		// Token: 0x14000345 RID: 837
		// (add) Token: 0x06004119 RID: 16665 RVA: 0x00118C01 File Offset: 0x00116E01
		// (remove) Token: 0x0600411A RID: 16666 RVA: 0x00118C14 File Offset: 0x00116E14
		[SRDescription("CheckBoxOnCheckStateChangedDescr")]
		public event EventHandler CheckStateChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripMenuItem.EventCheckStateChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripMenuItem.EventCheckStateChanged, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is attached to the <see cref="T:System.Windows.Forms.ToolStrip" /> or the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" /> or whether it can float between the two.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemOverflow" /> values. The default is <see langword="Never" />.</returns>
		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x0600411B RID: 16667 RVA: 0x00118C27 File Offset: 0x00116E27
		// (set) Token: 0x0600411C RID: 16668 RVA: 0x00118C2F File Offset: 0x00116E2F
		[DefaultValue(ToolStripItemOverflow.Never)]
		[SRDescription("ToolStripItemOverflowDescr")]
		[SRCategory("CatLayout")]
		public new ToolStripItemOverflow Overflow
		{
			get
			{
				return base.Overflow;
			}
			set
			{
				base.Overflow = value;
			}
		}

		/// <summary>Gets or sets the shortcut keys associated with the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Keys" /> values. The default is <see cref="F:System.Windows.Forms.Keys.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The property was not set to one of the <see cref="T:System.Windows.Forms.Keys" /> values.</exception>
		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x0600411D RID: 16669 RVA: 0x00118C38 File Offset: 0x00116E38
		// (set) Token: 0x0600411E RID: 16670 RVA: 0x00118C6C File Offset: 0x00116E6C
		[Localizable(true)]
		[DefaultValue(Keys.None)]
		[SRDescription("MenuItemShortCutDescr")]
		public Keys ShortcutKeys
		{
			get
			{
				bool flag = false;
				object obj = base.Properties.GetInteger(ToolStripMenuItem.PropShortcutKeys, out flag);
				if (!flag)
				{
					return Keys.None;
				}
				return (Keys)obj;
			}
			set
			{
				if (value != Keys.None && !ToolStripManager.IsValidShortcut(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Keys));
				}
				Keys shortcutKeys = this.ShortcutKeys;
				if (shortcutKeys != value)
				{
					this.ClearShortcutCache();
					ToolStrip owner = base.Owner;
					if (owner != null)
					{
						if (shortcutKeys != Keys.None)
						{
							owner.Shortcuts.Remove(shortcutKeys);
						}
						if (owner.Shortcuts.Contains(value))
						{
							owner.Shortcuts[value] = this;
						}
						else
						{
							owner.Shortcuts.Add(value, this);
						}
					}
					base.Properties.SetInteger(ToolStripMenuItem.PropShortcutKeys, (int)value);
					if (this.ShowShortcutKeys && base.IsOnDropDown)
					{
						ToolStripDropDownMenu toolStripDropDownMenu = base.GetCurrentParentDropDown() as ToolStripDropDownMenu;
						if (toolStripDropDownMenu != null)
						{
							LayoutTransaction.DoLayout(base.ParentInternal, this, "ShortcutKeys");
							toolStripDropDownMenu.AdjustSize();
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the shortcut key text.</summary>
		/// <returns>A <see cref="T:System.String" /> representing the shortcut key.</returns>
		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x0600411F RID: 16671 RVA: 0x00118D4C File Offset: 0x00116F4C
		// (set) Token: 0x06004120 RID: 16672 RVA: 0x00118D54 File Offset: 0x00116F54
		[SRDescription("ToolStripMenuItemShortcutKeyDisplayStringDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		[Localizable(true)]
		public string ShortcutKeyDisplayString
		{
			get
			{
				return this.shortcutKeyDisplayString;
			}
			set
			{
				if (this.shortcutKeyDisplayString != value)
				{
					this.shortcutKeyDisplayString = value;
					this.ClearShortcutCache();
					if (this.ShowShortcutKeys)
					{
						ToolStripDropDown toolStripDropDown = base.ParentInternal as ToolStripDropDown;
						if (toolStripDropDown != null)
						{
							LayoutTransaction.DoLayout(toolStripDropDown, this, "ShortcutKeyDisplayString");
							toolStripDropDown.AdjustSize();
						}
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the shortcut keys that are associated with the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> are displayed next to the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. </summary>
		/// <returns>
		///     <see langword="true" /> if the shortcut keys are shown; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06004121 RID: 16673 RVA: 0x00118DA5 File Offset: 0x00116FA5
		// (set) Token: 0x06004122 RID: 16674 RVA: 0x00118DB0 File Offset: 0x00116FB0
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("MenuItemShowShortCutDescr")]
		public bool ShowShortcutKeys
		{
			get
			{
				return this.showShortcutKeys;
			}
			set
			{
				if (value != this.showShortcutKeys)
				{
					this.ClearShortcutCache();
					this.showShortcutKeys = value;
					ToolStripDropDown toolStripDropDown = base.ParentInternal as ToolStripDropDown;
					if (toolStripDropDown != null)
					{
						LayoutTransaction.DoLayout(toolStripDropDown, this, "ShortcutKeys");
						toolStripDropDown.AdjustSize();
					}
				}
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06004123 RID: 16675 RVA: 0x00118DF4 File Offset: 0x00116FF4
		internal bool IsTopLevel
		{
			get
			{
				return !(base.ParentInternal is ToolStripDropDown);
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> appears on a multiple document interface (MDI) window list.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> appears on a MDI window list; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06004124 RID: 16676 RVA: 0x00118E04 File Offset: 0x00117004
		[Browsable(false)]
		public bool IsMdiWindowListEntry
		{
			get
			{
				return this.MdiForm != null;
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06004125 RID: 16677 RVA: 0x00118E0F File Offset: 0x0011700F
		internal static MenuTimer MenuTimer
		{
			get
			{
				return ToolStripMenuItem.menuTimer;
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06004126 RID: 16678 RVA: 0x00118E16 File Offset: 0x00117016
		internal Form MdiForm
		{
			get
			{
				if (base.Properties.ContainsObject(ToolStripMenuItem.PropMdiForm))
				{
					return base.Properties.GetObject(ToolStripMenuItem.PropMdiForm) as Form;
				}
				return null;
			}
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x00118E44 File Offset: 0x00117044
		internal ToolStripMenuItem Clone()
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
			toolStripMenuItem.Events.AddHandlers(base.Events);
			toolStripMenuItem.AccessibleName = base.AccessibleName;
			toolStripMenuItem.AccessibleRole = base.AccessibleRole;
			toolStripMenuItem.Alignment = base.Alignment;
			toolStripMenuItem.AllowDrop = this.AllowDrop;
			toolStripMenuItem.Anchor = base.Anchor;
			toolStripMenuItem.AutoSize = base.AutoSize;
			toolStripMenuItem.AutoToolTip = base.AutoToolTip;
			toolStripMenuItem.BackColor = this.BackColor;
			toolStripMenuItem.BackgroundImage = this.BackgroundImage;
			toolStripMenuItem.BackgroundImageLayout = this.BackgroundImageLayout;
			toolStripMenuItem.Checked = this.Checked;
			toolStripMenuItem.CheckOnClick = this.CheckOnClick;
			toolStripMenuItem.CheckState = this.CheckState;
			toolStripMenuItem.DisplayStyle = this.DisplayStyle;
			toolStripMenuItem.Dock = base.Dock;
			toolStripMenuItem.DoubleClickEnabled = base.DoubleClickEnabled;
			toolStripMenuItem.Enabled = this.Enabled;
			toolStripMenuItem.Font = this.Font;
			toolStripMenuItem.ForeColor = this.ForeColor;
			toolStripMenuItem.Image = this.Image;
			toolStripMenuItem.ImageAlign = base.ImageAlign;
			toolStripMenuItem.ImageScaling = base.ImageScaling;
			toolStripMenuItem.ImageTransparentColor = base.ImageTransparentColor;
			toolStripMenuItem.Margin = base.Margin;
			toolStripMenuItem.MergeAction = base.MergeAction;
			toolStripMenuItem.MergeIndex = base.MergeIndex;
			toolStripMenuItem.Name = base.Name;
			toolStripMenuItem.Overflow = this.Overflow;
			toolStripMenuItem.Padding = this.Padding;
			toolStripMenuItem.RightToLeft = this.RightToLeft;
			toolStripMenuItem.ShortcutKeys = this.ShortcutKeys;
			toolStripMenuItem.ShowShortcutKeys = this.ShowShortcutKeys;
			toolStripMenuItem.Tag = base.Tag;
			toolStripMenuItem.Text = this.Text;
			toolStripMenuItem.TextAlign = this.TextAlign;
			toolStripMenuItem.TextDirection = this.TextDirection;
			toolStripMenuItem.TextImageRelation = base.TextImageRelation;
			toolStripMenuItem.ToolTipText = base.ToolTipText;
			toolStripMenuItem.Visible = ((IArrangedElement)this).ParticipatesInLayout;
			if (!base.AutoSize)
			{
				toolStripMenuItem.Size = this.Size;
			}
			return toolStripMenuItem;
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06004128 RID: 16680 RVA: 0x0010A577 File Offset: 0x00108777
		// (set) Token: 0x06004129 RID: 16681 RVA: 0x00119051 File Offset: 0x00117251
		internal override int DeviceDpi
		{
			get
			{
				return base.DeviceDpi;
			}
			set
			{
				base.DeviceDpi = value;
				this.scaledDefaultPadding = DpiHelper.LogicalToDeviceUnits(ToolStripMenuItem.defaultPadding, value);
				this.scaledDefaultDropDownPadding = DpiHelper.LogicalToDeviceUnits(ToolStripMenuItem.defaultDropDownPadding, value);
				this.scaledCheckMarkBitmapSize = DpiHelper.LogicalToDeviceUnits(ToolStripMenuItem.checkMarkBitmapSize, value);
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600412A RID: 16682 RVA: 0x00119090 File Offset: 0x00117290
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.lastOwner != null)
			{
				Keys shortcutKeys = this.ShortcutKeys;
				if (shortcutKeys != Keys.None && this.lastOwner.Shortcuts.ContainsKey(shortcutKeys))
				{
					this.lastOwner.Shortcuts.Remove(shortcutKeys);
				}
				this.lastOwner = null;
				if (this.MdiForm != null)
				{
					base.Properties.SetObject(ToolStripMenuItem.PropMdiForm, null);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x00119108 File Offset: 0x00117308
		private bool GetNativeMenuItemEnabled()
		{
			if (this.nativeMenuCommandID == -1 || this.nativeMenuHandle == IntPtr.Zero)
			{
				return false;
			}
			NativeMethods.MENUITEMINFO_T_RW menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
			menuiteminfo_T_RW.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T_RW));
			menuiteminfo_T_RW.fMask = 1;
			menuiteminfo_T_RW.fType = 1;
			menuiteminfo_T_RW.wID = this.nativeMenuCommandID;
			UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this, this.nativeMenuHandle), this.nativeMenuCommandID, false, menuiteminfo_T_RW);
			return (menuiteminfo_T_RW.fState & 3) == 0;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x0011918C File Offset: 0x0011738C
		private string GetNativeMenuItemTextAndShortcut()
		{
			if (this.nativeMenuCommandID == -1 || this.nativeMenuHandle == IntPtr.Zero)
			{
				return null;
			}
			string result = null;
			NativeMethods.MENUITEMINFO_T_RW menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
			menuiteminfo_T_RW.fMask = 64;
			menuiteminfo_T_RW.fType = 64;
			menuiteminfo_T_RW.wID = this.nativeMenuCommandID;
			menuiteminfo_T_RW.dwTypeData = IntPtr.Zero;
			UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this, this.nativeMenuHandle), this.nativeMenuCommandID, false, menuiteminfo_T_RW);
			if (menuiteminfo_T_RW.cch > 0)
			{
				menuiteminfo_T_RW.cch++;
				menuiteminfo_T_RW.wID = this.nativeMenuCommandID;
				IntPtr intPtr = Marshal.AllocCoTaskMem(menuiteminfo_T_RW.cch * Marshal.SystemDefaultCharSize);
				menuiteminfo_T_RW.dwTypeData = intPtr;
				try
				{
					UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this, this.nativeMenuHandle), this.nativeMenuCommandID, false, menuiteminfo_T_RW);
					if (menuiteminfo_T_RW.dwTypeData != IntPtr.Zero)
					{
						result = Marshal.PtrToStringAuto(menuiteminfo_T_RW.dwTypeData, menuiteminfo_T_RW.cch);
					}
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
				}
			}
			return result;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x001192A4 File Offset: 0x001174A4
		private Image GetNativeMenuItemImage()
		{
			if (this.nativeMenuCommandID == -1 || this.nativeMenuHandle == IntPtr.Zero)
			{
				return null;
			}
			NativeMethods.MENUITEMINFO_T_RW menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
			menuiteminfo_T_RW.fMask = 128;
			menuiteminfo_T_RW.fType = 128;
			menuiteminfo_T_RW.wID = this.nativeMenuCommandID;
			UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this, this.nativeMenuHandle), this.nativeMenuCommandID, false, menuiteminfo_T_RW);
			if (menuiteminfo_T_RW.hbmpItem != IntPtr.Zero && menuiteminfo_T_RW.hbmpItem.ToInt32() > 11)
			{
				return Image.FromHbitmap(menuiteminfo_T_RW.hbmpItem);
			}
			int num = -1;
			switch (menuiteminfo_T_RW.hbmpItem.ToInt32())
			{
			case 2:
			case 9:
				num = 3;
				break;
			case 3:
			case 7:
			case 11:
				num = 1;
				break;
			case 5:
			case 6:
			case 8:
				num = 0;
				break;
			case 10:
				num = 2;
				break;
			}
			if (num > -1)
			{
				Bitmap bitmap = new Bitmap(16, 16);
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					ControlPaint.DrawCaptionButton(graphics, new Rectangle(Point.Empty, bitmap.Size), (CaptionButton)num, ButtonState.Flat);
					graphics.DrawRectangle(SystemPens.Control, 0, 0, bitmap.Width - 1, bitmap.Height - 1);
				}
				bitmap.MakeTransparent(SystemColors.Control);
				return bitmap;
			}
			return null;
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00119414 File Offset: 0x00117614
		internal Size GetShortcutTextSize()
		{
			if (!this.ShowShortcutKeys)
			{
				return Size.Empty;
			}
			string shortcutText = this.GetShortcutText();
			if (string.IsNullOrEmpty(shortcutText))
			{
				return Size.Empty;
			}
			if (this.cachedShortcutSize == Size.Empty)
			{
				this.cachedShortcutSize = TextRenderer.MeasureText(shortcutText, this.Font);
			}
			return this.cachedShortcutSize;
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0011946E File Offset: 0x0011766E
		internal string GetShortcutText()
		{
			if (this.cachedShortcutText == null)
			{
				this.cachedShortcutText = ToolStripMenuItem.ShortcutToText(this.ShortcutKeys, this.ShortcutKeyDisplayString);
			}
			return this.cachedShortcutText;
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x00119498 File Offset: 0x00117698
		internal void HandleAutoExpansion()
		{
			if (this.Enabled && base.ParentInternal != null && base.ParentInternal.MenuAutoExpand && this.HasDropDownItems)
			{
				base.ShowDropDown();
				if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
				{
					KeyboardToolTipStateMachine.Instance.NotifyAboutLostFocus(this);
				}
				base.DropDown.SelectNextToolStripItem(null, true);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004131 RID: 16689 RVA: 0x001194F0 File Offset: 0x001176F0
		protected override void OnClick(EventArgs e)
		{
			if (this.checkOnClick)
			{
				this.Checked = !this.Checked;
			}
			base.OnClick(e);
			if (this.nativeMenuCommandID != -1)
			{
				if ((this.nativeMenuCommandID & 61440) != 0)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, this.targetWindowHandle), 274, this.nativeMenuCommandID, 0);
				}
				else
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, this.targetWindowHandle), 273, this.nativeMenuCommandID, 0);
				}
				base.Invalidate();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripMenuItem.CheckedChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004132 RID: 16690 RVA: 0x00119578 File Offset: 0x00117778
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripMenuItem.EventCheckedChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripMenuItem.CheckStateChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004133 RID: 16691 RVA: 0x001195A8 File Offset: 0x001177A8
		protected virtual void OnCheckStateChanged(EventArgs e)
		{
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripMenuItem.EventCheckStateChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raised in response to the <see cref="M:System.Windows.Forms.ToolStripDropDownItem.HideDropDown" /> method.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004134 RID: 16692 RVA: 0x001195E1 File Offset: 0x001177E1
		protected override void OnDropDownHide(EventArgs e)
		{
			ToolStripMenuItem.MenuTimer.Cancel(this);
			base.OnDropDownHide(e);
		}

		/// <summary>Raised in response to the <see cref="M:System.Windows.Forms.ToolStripDropDownItem.ShowDropDown" /> method.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004135 RID: 16693 RVA: 0x001195F5 File Offset: 0x001177F5
		protected override void OnDropDownShow(EventArgs e)
		{
			ToolStripMenuItem.MenuTimer.Cancel(this);
			if (base.ParentInternal != null)
			{
				base.ParentInternal.MenuAutoExpand = true;
			}
			base.OnDropDownShow(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004136 RID: 16694 RVA: 0x0011961D File Offset: 0x0011781D
		protected override void OnFontChanged(EventArgs e)
		{
			this.ClearShortcutCache();
			base.OnFontChanged(e);
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x0011962C File Offset: 0x0011782C
		internal void OnMenuAutoExpand()
		{
			base.ShowDropDown();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06004138 RID: 16696 RVA: 0x00119634 File Offset: 0x00117834
		protected override void OnMouseDown(MouseEventArgs e)
		{
			ToolStripMenuItem.MenuTimer.Cancel(this);
			this.OnMouseButtonStateChange(e, true);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06004139 RID: 16697 RVA: 0x00119649 File Offset: 0x00117849
		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.OnMouseButtonStateChange(e, false);
			base.OnMouseUp(e);
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x0011965C File Offset: 0x0011785C
		private void OnMouseButtonStateChange(MouseEventArgs e, bool isMouseDown)
		{
			bool flag = true;
			if (base.IsOnDropDown)
			{
				ToolStripDropDown currentParentDropDown = base.GetCurrentParentDropDown();
				base.SupportsRightClick = (currentParentDropDown.GetFirstDropDown() is ContextMenuStrip);
			}
			else
			{
				flag = !base.DropDown.Visible;
				base.SupportsRightClick = false;
			}
			if (e.Button == MouseButtons.Left || (e.Button == MouseButtons.Right && base.SupportsRightClick))
			{
				if (isMouseDown && flag)
				{
					this.openMouseId = ((base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId());
					base.ShowDropDown(true);
					return;
				}
				if (!isMouseDown && !flag)
				{
					byte b = (base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId();
					int num = (int)this.openMouseId;
					if ((int)b != num)
					{
						this.openMouseId = 0;
						ToolStripManager.ModalMenuFilter.CloseActiveDropDown(base.DropDown, ToolStripDropDownCloseReason.AppClicked);
						base.Select();
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600413B RID: 16699 RVA: 0x00119732 File Offset: 0x00117932
		protected override void OnMouseEnter(EventArgs e)
		{
			if (base.ParentInternal != null && base.ParentInternal.MenuAutoExpand && this.Selected)
			{
				ToolStripMenuItem.MenuTimer.Cancel(this);
				ToolStripMenuItem.MenuTimer.Start(this);
			}
			base.OnMouseEnter(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600413C RID: 16700 RVA: 0x0011976E File Offset: 0x0011796E
		protected override void OnMouseLeave(EventArgs e)
		{
			ToolStripMenuItem.MenuTimer.Cancel(this);
			base.OnMouseLeave(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.OwnerChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600413D RID: 16701 RVA: 0x00119784 File Offset: 0x00117984
		protected override void OnOwnerChanged(EventArgs e)
		{
			Keys shortcutKeys = this.ShortcutKeys;
			if (shortcutKeys != Keys.None)
			{
				if (this.lastOwner != null)
				{
					this.lastOwner.Shortcuts.Remove(shortcutKeys);
				}
				if (base.Owner != null)
				{
					if (base.Owner.Shortcuts.Contains(shortcutKeys))
					{
						base.Owner.Shortcuts[shortcutKeys] = this;
					}
					else
					{
						base.Owner.Shortcuts.Add(shortcutKeys, this);
					}
					this.lastOwner = base.Owner;
				}
			}
			base.OnOwnerChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600413E RID: 16702 RVA: 0x0011981C File Offset: 0x00117A1C
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				Graphics graphics = e.Graphics;
				renderer.DrawMenuItemBackground(new ToolStripItemRenderEventArgs(graphics, this));
				Color textColor = SystemColors.MenuText;
				if (base.IsForeColorSet)
				{
					textColor = this.ForeColor;
				}
				else if (!this.IsTopLevel || ToolStripManager.VisualStylesEnabled)
				{
					if (this.Selected || this.Pressed)
					{
						textColor = SystemColors.HighlightText;
					}
					else
					{
						textColor = SystemColors.MenuText;
					}
				}
				bool flag = this.RightToLeft == RightToLeft.Yes;
				ToolStripMenuItemInternalLayout toolStripMenuItemInternalLayout = base.InternalLayout as ToolStripMenuItemInternalLayout;
				if (toolStripMenuItemInternalLayout != null && toolStripMenuItemInternalLayout.UseMenuLayout)
				{
					if (this.CheckState != CheckState.Unchecked && toolStripMenuItemInternalLayout.PaintCheck)
					{
						Rectangle imageRectangle = toolStripMenuItemInternalLayout.CheckRectangle;
						if (!toolStripMenuItemInternalLayout.ShowCheckMargin)
						{
							imageRectangle = toolStripMenuItemInternalLayout.ImageRectangle;
						}
						if (imageRectangle.Width != 0)
						{
							renderer.DrawItemCheck(new ToolStripItemImageRenderEventArgs(graphics, this, this.CheckedImage, imageRectangle));
						}
					}
					if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
					{
						renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.Text, base.InternalLayout.TextRectangle, textColor, this.Font, flag ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft));
						bool flag2 = this.ShowShortcutKeys;
						if (!base.DesignMode)
						{
							flag2 = (flag2 && !this.HasDropDownItems);
						}
						if (flag2)
						{
							renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.GetShortcutText(), base.InternalLayout.TextRectangle, textColor, this.Font, flag ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleRight));
						}
					}
					if (this.HasDropDownItems)
					{
						ArrowDirection arrowDirection = flag ? ArrowDirection.Left : ArrowDirection.Right;
						Color color = (this.Selected || this.Pressed) ? SystemColors.HighlightText : SystemColors.MenuText;
						color = (this.Enabled ? color : SystemColors.ControlDark);
						renderer.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, this, toolStripMenuItemInternalLayout.ArrowRectangle, color, arrowDirection));
					}
					if (toolStripMenuItemInternalLayout.PaintImage && (this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image && this.Image != null)
					{
						renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(graphics, this, base.InternalLayout.ImageRectangle));
						return;
					}
				}
				else
				{
					if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
					{
						renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.Text, base.InternalLayout.TextRectangle, textColor, this.Font, base.InternalLayout.TextFormat));
					}
					if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image && this.Image != null)
					{
						renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(graphics, this, base.InternalLayout.ImageRectangle));
					}
				}
			}
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, which represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600413F RID: 16703 RVA: 0x00119A93 File Offset: 0x00117C93
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (this.Enabled && this.ShortcutKeys == keyData && !this.HasDropDownItems)
			{
				base.FireEvent(ToolStripItemEventType.Click);
				return true;
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004140 RID: 16704 RVA: 0x00119ABF File Offset: 0x00117CBF
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (this.HasDropDownItems)
			{
				base.Select();
				base.ShowDropDown();
				if (!AccessibilityImprovements.UseLegacyToolTipDisplay)
				{
					KeyboardToolTipStateMachine.Instance.NotifyAboutLostFocus(this);
				}
				base.DropDown.SelectNextToolStripItem(null, true);
				return true;
			}
			return base.ProcessMnemonic(charCode);
		}

		/// <summary>Sets the size and location of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> that represents the size and location of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</param>
		// Token: 0x06004141 RID: 16705 RVA: 0x00119B00 File Offset: 0x00117D00
		protected internal override void SetBounds(Rectangle rect)
		{
			ToolStripMenuItemInternalLayout toolStripMenuItemInternalLayout = base.InternalLayout as ToolStripMenuItemInternalLayout;
			if (toolStripMenuItemInternalLayout != null && toolStripMenuItemInternalLayout.UseMenuLayout)
			{
				ToolStripDropDownMenu toolStripDropDownMenu = base.Owner as ToolStripDropDownMenu;
				if (toolStripDropDownMenu != null)
				{
					rect.X -= toolStripDropDownMenu.Padding.Left;
					rect.X = Math.Max(rect.X, 0);
				}
			}
			base.SetBounds(rect);
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x00119B6A File Offset: 0x00117D6A
		internal void SetNativeTargetWindow(IWin32Window window)
		{
			this.targetWindowHandle = Control.GetSafeHandle(window);
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00119B78 File Offset: 0x00117D78
		internal void SetNativeTargetMenu(IntPtr hMenu)
		{
			this.nativeMenuHandle = hMenu;
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x00119B81 File Offset: 0x00117D81
		internal static string ShortcutToText(Keys shortcutKeys, string shortcutKeyDisplayString)
		{
			if (!string.IsNullOrEmpty(shortcutKeyDisplayString))
			{
				return shortcutKeyDisplayString;
			}
			if (shortcutKeys == Keys.None)
			{
				return string.Empty;
			}
			return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(shortcutKeys);
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x00119BB0 File Offset: 0x00117DB0
		internal override bool IsBeingTabbedTo()
		{
			return base.IsBeingTabbedTo() || ToolStripManager.ModalMenuFilter.InMenuMode;
		}

		// Token: 0x040024F5 RID: 9461
		private static MenuTimer menuTimer = new MenuTimer();

		// Token: 0x040024F6 RID: 9462
		private static readonly int PropShortcutKeys = PropertyStore.CreateKey();

		// Token: 0x040024F7 RID: 9463
		private static readonly int PropCheckState = PropertyStore.CreateKey();

		// Token: 0x040024F8 RID: 9464
		private static readonly int PropMdiForm = PropertyStore.CreateKey();

		// Token: 0x040024F9 RID: 9465
		private bool checkOnClick;

		// Token: 0x040024FA RID: 9466
		private bool showShortcutKeys = true;

		// Token: 0x040024FB RID: 9467
		private ToolStrip lastOwner;

		// Token: 0x040024FC RID: 9468
		private int nativeMenuCommandID = -1;

		// Token: 0x040024FD RID: 9469
		private IntPtr targetWindowHandle = IntPtr.Zero;

		// Token: 0x040024FE RID: 9470
		private IntPtr nativeMenuHandle = IntPtr.Zero;

		// Token: 0x040024FF RID: 9471
		[ThreadStatic]
		private static Image indeterminateCheckedImage;

		// Token: 0x04002500 RID: 9472
		[ThreadStatic]
		private static Image checkedImage;

		// Token: 0x04002501 RID: 9473
		private string shortcutKeyDisplayString;

		// Token: 0x04002502 RID: 9474
		private string cachedShortcutText;

		// Token: 0x04002503 RID: 9475
		private Size cachedShortcutSize = Size.Empty;

		// Token: 0x04002504 RID: 9476
		private static readonly Padding defaultPadding = new Padding(4, 0, 4, 0);

		// Token: 0x04002505 RID: 9477
		private static readonly Padding defaultDropDownPadding = new Padding(0, 1, 0, 1);

		// Token: 0x04002506 RID: 9478
		private static readonly Size checkMarkBitmapSize = new Size(16, 16);

		// Token: 0x04002507 RID: 9479
		private Padding scaledDefaultPadding = ToolStripMenuItem.defaultPadding;

		// Token: 0x04002508 RID: 9480
		private Padding scaledDefaultDropDownPadding = ToolStripMenuItem.defaultDropDownPadding;

		// Token: 0x04002509 RID: 9481
		private Size scaledCheckMarkBitmapSize = ToolStripMenuItem.checkMarkBitmapSize;

		// Token: 0x0400250A RID: 9482
		private byte openMouseId;

		// Token: 0x0400250B RID: 9483
		private static readonly object EventCheckedChanged = new object();

		// Token: 0x0400250C RID: 9484
		private static readonly object EventCheckStateChanged = new object();

		// Token: 0x0200073F RID: 1855
		[ComVisible(true)]
		internal class ToolStripMenuItemAccessibleObject : ToolStripDropDownItemAccessibleObject
		{
			// Token: 0x06006160 RID: 24928 RVA: 0x0018E9D4 File Offset: 0x0018CBD4
			public ToolStripMenuItemAccessibleObject(ToolStripMenuItem ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x17001745 RID: 5957
			// (get) Token: 0x06006161 RID: 24929 RVA: 0x0018E9E4 File Offset: 0x0018CBE4
			public override AccessibleStates State
			{
				get
				{
					if (this.ownerItem.Enabled)
					{
						AccessibleStates accessibleStates = base.State;
						if ((accessibleStates & AccessibleStates.Pressed) == AccessibleStates.Pressed)
						{
							accessibleStates &= ~AccessibleStates.Pressed;
						}
						if (this.ownerItem.Checked)
						{
							accessibleStates |= AccessibleStates.Checked;
						}
						return accessibleStates;
					}
					return base.State;
				}
			}

			// Token: 0x06006162 RID: 24930 RVA: 0x0018EA2A File Offset: 0x0018CC2A
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50011;
				}
				if (AccessibilityImprovements.Level2 && propertyID == 30006)
				{
					return this.ownerItem.GetShortcutText();
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x04004196 RID: 16790
			private ToolStripMenuItem ownerItem;
		}
	}
}
