using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	/// <summary>Represents an individual item that is displayed within a <see cref="T:System.Windows.Forms.MainMenu" /> or <see cref="T:System.Windows.Forms.ContextMenu" />. Although <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> replaces and adds functionality to the <see cref="T:System.Windows.Forms.MenuItem" /> control of previous versions, <see cref="T:System.Windows.Forms.MenuItem" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x020002E4 RID: 740
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultEvent("Click")]
	[DefaultProperty("Text")]
	public class MenuItem : Menu
	{
		/// <summary>Initializes a <see cref="T:System.Windows.Forms.MenuItem" /> with a blank caption.</summary>
		// Token: 0x06002C6D RID: 11373 RVA: 0x000CF240 File Offset: 0x000CD440
		public MenuItem() : this(MenuMerge.Add, 0, Shortcut.None, null, null, null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MenuItem" /> class with a specified caption for the menu item.</summary>
		/// <param name="text">The caption for the menu item. </param>
		// Token: 0x06002C6E RID: 11374 RVA: 0x000CF25C File Offset: 0x000CD45C
		public MenuItem(string text) : this(MenuMerge.Add, 0, Shortcut.None, text, null, null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the class with a specified caption and event handler for the <see cref="E:System.Windows.Forms.MenuItem.Click" /> event of the menu item.</summary>
		/// <param name="text">The caption for the menu item. </param>
		/// <param name="onClick">The <see cref="T:System.EventHandler" /> that handles the <see cref="E:System.Windows.Forms.MenuItem.Click" /> event for this menu item. </param>
		// Token: 0x06002C6F RID: 11375 RVA: 0x000CF278 File Offset: 0x000CD478
		public MenuItem(string text, EventHandler onClick) : this(MenuMerge.Add, 0, Shortcut.None, text, onClick, null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the class with a specified caption, event handler, and associated shortcut key for the menu item.</summary>
		/// <param name="text">The caption for the menu item. </param>
		/// <param name="onClick">The <see cref="T:System.EventHandler" /> that handles the <see cref="E:System.Windows.Forms.MenuItem.Click" /> event for this menu item. </param>
		/// <param name="shortcut">One of the <see cref="T:System.Windows.Forms.Shortcut" /> values. </param>
		// Token: 0x06002C70 RID: 11376 RVA: 0x000CF294 File Offset: 0x000CD494
		public MenuItem(string text, EventHandler onClick, Shortcut shortcut) : this(MenuMerge.Add, 0, shortcut, text, onClick, null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the class with a specified caption and an array of submenu items defined for the menu item.</summary>
		/// <param name="text">The caption for the menu item. </param>
		/// <param name="items">An array of <see cref="T:System.Windows.Forms.MenuItem" /> objects that contains the submenu items for this menu item. </param>
		// Token: 0x06002C71 RID: 11377 RVA: 0x000CF2B0 File Offset: 0x000CD4B0
		public MenuItem(string text, MenuItem[] items) : this(MenuMerge.Add, 0, Shortcut.None, text, null, null, null, items)
		{
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x000CF2CB File Offset: 0x000CD4CB
		internal MenuItem(MenuItem.MenuItemData data)
		{
			this.msaaMenuInfoPtr = IntPtr.Zero;
			base..ctor(null);
			data.AddItem(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MenuItem" /> class with a specified caption; defined event-handlers for the <see cref="E:System.Windows.Forms.MenuItem.Click" />, <see cref="E:System.Windows.Forms.MenuItem.Select" /> and <see cref="E:System.Windows.Forms.MenuItem.Popup" /> events; a shortcut key; a merge type; and order specified for the menu item.</summary>
		/// <param name="mergeType">One of the <see cref="T:System.Windows.Forms.MenuMerge" /> values. </param>
		/// <param name="mergeOrder">The relative position that this menu item will take in a merged menu. </param>
		/// <param name="shortcut">One of the <see cref="T:System.Windows.Forms.Shortcut" /> values. </param>
		/// <param name="text">The caption for the menu item. </param>
		/// <param name="onClick">The <see cref="T:System.EventHandler" /> that handles the <see cref="E:System.Windows.Forms.MenuItem.Click" /> event for this menu item. </param>
		/// <param name="onPopup">The <see cref="T:System.EventHandler" /> that handles the <see cref="E:System.Windows.Forms.MenuItem.Popup" /> event for this menu item. </param>
		/// <param name="onSelect">The <see cref="T:System.EventHandler" /> that handles the <see cref="E:System.Windows.Forms.MenuItem.Select" /> event for this menu item. </param>
		/// <param name="items">An array of <see cref="T:System.Windows.Forms.MenuItem" /> objects that contains the submenu items for this menu item. </param>
		// Token: 0x06002C73 RID: 11379 RVA: 0x000CF2E8 File Offset: 0x000CD4E8
		public MenuItem(MenuMerge mergeType, int mergeOrder, Shortcut shortcut, string text, EventHandler onClick, EventHandler onPopup, EventHandler onSelect, MenuItem[] items)
		{
			this.msaaMenuInfoPtr = IntPtr.Zero;
			base..ctor(items);
			new MenuItem.MenuItemData(this, mergeType, mergeOrder, shortcut, true, text, onClick, onPopup, onSelect, null, null);
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.MenuItem" /> is placed on a new line (for a menu item added to a <see cref="T:System.Windows.Forms.MainMenu" /> object) or in a new column (for a submenu item or menu item displayed in a <see cref="T:System.Windows.Forms.ContextMenu" />).</summary>
		/// <returns>
		///     <see langword="true" /> if the menu item is placed on a new line or in a new column; <see langword="false" /> if the menu item is left in its default placement. The default is <see langword="false" />.</returns>
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06002C74 RID: 11380 RVA: 0x000CF31D File Offset: 0x000CD51D
		// (set) Token: 0x06002C75 RID: 11381 RVA: 0x000CF330 File Offset: 0x000CD530
		[Browsable(false)]
		[DefaultValue(false)]
		public bool BarBreak
		{
			get
			{
				return (this.data.State & 32) != 0;
			}
			set
			{
				this.data.SetState(32, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the item is placed on a new line (for a menu item added to a <see cref="T:System.Windows.Forms.MainMenu" /> object) or in a new column (for a menu item or submenu item displayed in a <see cref="T:System.Windows.Forms.ContextMenu" />).</summary>
		/// <returns>
		///     <see langword="true" /> if the menu item is placed on a new line or in a new column; <see langword="false" /> if the menu item is left in its default placement. The default is <see langword="false" />.</returns>
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06002C76 RID: 11382 RVA: 0x000CF340 File Offset: 0x000CD540
		// (set) Token: 0x06002C77 RID: 11383 RVA: 0x000CF353 File Offset: 0x000CD553
		[Browsable(false)]
		[DefaultValue(false)]
		public bool Break
		{
			get
			{
				return (this.data.State & 64) != 0;
			}
			set
			{
				this.data.SetState(64, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether a check mark appears next to the text of the menu item.</summary>
		/// <returns>
		///     <see langword="true" /> if there is a check mark next to the menu item; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Windows.Forms.MenuItem" /> is a top-level menu or has children.</exception>
		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06002C78 RID: 11384 RVA: 0x000CF363 File Offset: 0x000CD563
		// (set) Token: 0x06002C79 RID: 11385 RVA: 0x000CF375 File Offset: 0x000CD575
		[DefaultValue(false)]
		[SRDescription("MenuItemCheckedDescr")]
		public bool Checked
		{
			get
			{
				return (this.data.State & 8) != 0;
			}
			set
			{
				if (value && (base.ItemCount != 0 || (this.Parent != null && this.Parent is MainMenu)))
				{
					throw new ArgumentException(SR.GetString("MenuItemInvalidCheckProperty"));
				}
				this.data.SetState(8, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the menu item is the default menu item.</summary>
		/// <returns>
		///     <see langword="true" /> if the menu item is the default item in a menu; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06002C7A RID: 11386 RVA: 0x000CF3B4 File Offset: 0x000CD5B4
		// (set) Token: 0x06002C7B RID: 11387 RVA: 0x000CF3CC File Offset: 0x000CD5CC
		[DefaultValue(false)]
		[SRDescription("MenuItemDefaultDescr")]
		public bool DefaultItem
		{
			get
			{
				return (this.data.State & 4096) != 0;
			}
			set
			{
				if (this.menu != null)
				{
					if (value)
					{
						UnsafeNativeMethods.SetMenuDefaultItem(new HandleRef(this.menu, this.menu.handle), this.MenuID, false);
					}
					else if (this.DefaultItem)
					{
						UnsafeNativeMethods.SetMenuDefaultItem(new HandleRef(this.menu, this.menu.handle), -1, false);
					}
				}
				this.data.SetState(4096, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the code that you provide draws the menu item or Windows draws the menu item.</summary>
		/// <returns>
		///     <see langword="true" /> if the menu item is to be drawn using code; <see langword="false" /> if the menu item is to be drawn by Windows. The default is <see langword="false" />.</returns>
		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06002C7C RID: 11388 RVA: 0x000CF440 File Offset: 0x000CD640
		// (set) Token: 0x06002C7D RID: 11389 RVA: 0x000CF456 File Offset: 0x000CD656
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("MenuItemOwnerDrawDescr")]
		public bool OwnerDraw
		{
			get
			{
				return (this.data.State & 256) != 0;
			}
			set
			{
				this.data.SetState(256, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the menu item is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the menu item is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06002C7E RID: 11390 RVA: 0x000CF469 File Offset: 0x000CD669
		// (set) Token: 0x06002C7F RID: 11391 RVA: 0x000CF47B File Offset: 0x000CD67B
		[Localizable(true)]
		[DefaultValue(true)]
		[SRDescription("MenuItemEnabledDescr")]
		public bool Enabled
		{
			get
			{
				return (this.data.State & 3) == 0;
			}
			set
			{
				this.data.SetState(3, !value);
			}
		}

		/// <summary>Gets or sets a value indicating the position of the menu item in its parent menu.</summary>
		/// <returns>The zero-based index representing the position of the menu item in its parent menu.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than zero or greater than the item count.</exception>
		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06002C80 RID: 11392 RVA: 0x000CF490 File Offset: 0x000CD690
		// (set) Token: 0x06002C81 RID: 11393 RVA: 0x000CF4D0 File Offset: 0x000CD6D0
		[Browsable(false)]
		public int Index
		{
			get
			{
				if (this.menu != null)
				{
					for (int i = 0; i < this.menu.ItemCount; i++)
					{
						if (this.menu.items[i] == this)
						{
							return i;
						}
					}
				}
				return -1;
			}
			set
			{
				int index = this.Index;
				if (index >= 0)
				{
					if (value < 0 || value >= this.menu.ItemCount)
					{
						throw new ArgumentOutOfRangeException("Index", SR.GetString("InvalidArgument", new object[]
						{
							"Index",
							value.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value != index)
					{
						Menu menu = this.menu;
						menu.MenuItems.RemoveAt(index);
						menu.MenuItems.Add(value, this);
					}
				}
			}
		}

		/// <summary>Gets a value indicating whether the menu item contains child menu items.</summary>
		/// <returns>
		///     <see langword="true" /> if the menu item contains child menu items; <see langword="false" /> if the menu is a standalone menu item.</returns>
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06002C82 RID: 11394 RVA: 0x000CF554 File Offset: 0x000CD754
		[Browsable(false)]
		public override bool IsParent
		{
			get
			{
				bool flag = false;
				if (this.data != null && this.MdiList)
				{
					for (int i = 0; i < base.ItemCount; i++)
					{
						if (!(this.items[i].data.UserData is MenuItem.MdiListUserData))
						{
							flag = true;
							break;
						}
					}
					if (!flag && this.FindMdiForms().Length != 0)
					{
						flag = true;
					}
					if (!flag && this.menu != null && !(this.menu is MenuItem))
					{
						flag = true;
					}
				}
				else
				{
					flag = base.IsParent;
				}
				return flag;
			}
		}

		/// <summary>Gets or sets a value indicating whether the menu item will be populated with a list of the Multiple Document Interface (MDI) child windows that are displayed within the associated form.</summary>
		/// <returns>
		///     <see langword="true" /> if a list of the MDI child windows is displayed in this menu item; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06002C83 RID: 11395 RVA: 0x000CF5D3 File Offset: 0x000CD7D3
		// (set) Token: 0x06002C84 RID: 11396 RVA: 0x000CF5E9 File Offset: 0x000CD7E9
		[DefaultValue(false)]
		[SRDescription("MenuItemMDIListDescr")]
		public bool MdiList
		{
			get
			{
				return (this.data.State & 131072) != 0;
			}
			set
			{
				this.data.MdiList = value;
				MenuItem.CleanListItems(this);
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x000CF5FD File Offset: 0x000CD7FD
		// (set) Token: 0x06002C86 RID: 11398 RVA: 0x000CF605 File Offset: 0x000CD805
		internal Menu Menu
		{
			get
			{
				return this.menu;
			}
			set
			{
				this.menu = value;
			}
		}

		/// <summary>Gets a value indicating the Windows identifier for this menu item.</summary>
		/// <returns>The Windows identifier for this menu item.</returns>
		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06002C87 RID: 11399 RVA: 0x000CF60E File Offset: 0x000CD80E
		protected int MenuID
		{
			get
			{
				return this.data.GetMenuID();
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06002C88 RID: 11400 RVA: 0x000CF61C File Offset: 0x000CD81C
		internal bool Selected
		{
			get
			{
				if (this.menu == null)
				{
					return false;
				}
				NativeMethods.MENUITEMINFO_T menuiteminfo_T = new NativeMethods.MENUITEMINFO_T();
				menuiteminfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T));
				menuiteminfo_T.fMask = 1;
				UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), this.MenuID, false, menuiteminfo_T);
				return (menuiteminfo_T.fState & 128) != 0;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x000CF688 File Offset: 0x000CD888
		internal int MenuIndex
		{
			get
			{
				if (this.menu == null)
				{
					return -1;
				}
				int menuItemCount = UnsafeNativeMethods.GetMenuItemCount(new HandleRef(this.menu, this.menu.Handle));
				int menuID = this.MenuID;
				NativeMethods.MENUITEMINFO_T menuiteminfo_T = new NativeMethods.MENUITEMINFO_T();
				menuiteminfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T));
				menuiteminfo_T.fMask = 6;
				for (int i = 0; i < menuItemCount; i++)
				{
					UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), i, true, menuiteminfo_T);
					if ((menuiteminfo_T.hSubMenu == IntPtr.Zero || menuiteminfo_T.hSubMenu == base.Handle) && menuiteminfo_T.wID == menuID)
					{
						return i;
					}
				}
				return -1;
			}
		}

		/// <summary>Gets or sets a value indicating the behavior of this menu item when its menu is merged with another.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MenuMerge" /> value that represents the menu item's merge type.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.MenuMerge" /> values.</exception>
		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06002C8A RID: 11402 RVA: 0x000CF740 File Offset: 0x000CD940
		// (set) Token: 0x06002C8B RID: 11403 RVA: 0x000CF74D File Offset: 0x000CD94D
		[DefaultValue(MenuMerge.Add)]
		[SRDescription("MenuItemMergeTypeDescr")]
		public MenuMerge MergeType
		{
			get
			{
				return this.data.mergeType;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(MenuMerge));
				}
				this.data.MergeType = value;
			}
		}

		/// <summary>Gets or sets a value indicating the relative position of the menu item when it is merged with another.</summary>
		/// <returns>A zero-based index representing the merge order position for this menu item. The default is 0.</returns>
		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06002C8C RID: 11404 RVA: 0x000CF781 File Offset: 0x000CD981
		// (set) Token: 0x06002C8D RID: 11405 RVA: 0x000CF78E File Offset: 0x000CD98E
		[DefaultValue(0)]
		[SRDescription("MenuItemMergeOrderDescr")]
		public int MergeOrder
		{
			get
			{
				return this.data.mergeOrder;
			}
			set
			{
				this.data.MergeOrder = value;
			}
		}

		/// <summary>Gets a value indicating the mnemonic character that is associated with this menu item.</summary>
		/// <returns>A character that represents the mnemonic character associated with this menu item. Returns the NUL character (ASCII value 0) if no mnemonic character is specified in the text of the <see cref="T:System.Windows.Forms.MenuItem" />.</returns>
		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x000CF79C File Offset: 0x000CD99C
		[Browsable(false)]
		public char Mnemonic
		{
			get
			{
				return this.data.Mnemonic;
			}
		}

		/// <summary>Gets a value indicating the menu that contains this menu item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Menu" /> that represents the menu that contains this menu item.</returns>
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06002C8F RID: 11407 RVA: 0x000CF5FD File Offset: 0x000CD7FD
		[Browsable(false)]
		public Menu Parent
		{
			get
			{
				return this.menu;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.MenuItem" />, if checked, displays a radio-button instead of a check mark.</summary>
		/// <returns>
		///     <see langword="true" /> if a radio-button is to be used instead of a check mark; <see langword="false" /> if the standard check mark is to be displayed when the menu item is checked. The default is <see langword="false" />.</returns>
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000CF7A9 File Offset: 0x000CD9A9
		// (set) Token: 0x06002C91 RID: 11409 RVA: 0x000CF7BF File Offset: 0x000CD9BF
		[DefaultValue(false)]
		[SRDescription("MenuItemRadioCheckDescr")]
		public bool RadioCheck
		{
			get
			{
				return (this.data.State & 512) != 0;
			}
			set
			{
				this.data.SetState(512, value);
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x000CF7D2 File Offset: 0x000CD9D2
		internal override bool RenderIsRightToLeft
		{
			get
			{
				return this.Parent != null && this.Parent.RenderIsRightToLeft;
			}
		}

		/// <summary>Gets or sets a value indicating the caption of the menu item.</summary>
		/// <returns>The text caption of the menu item.</returns>
		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06002C93 RID: 11411 RVA: 0x000CF7E9 File Offset: 0x000CD9E9
		// (set) Token: 0x06002C94 RID: 11412 RVA: 0x000CF7F6 File Offset: 0x000CD9F6
		[Localizable(true)]
		[SRDescription("MenuItemTextDescr")]
		public string Text
		{
			get
			{
				return this.data.caption;
			}
			set
			{
				this.data.SetCaption(value);
			}
		}

		/// <summary>Gets or sets a value indicating the shortcut key associated with the menu item.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Shortcut" /> values. The default is <see langword="Shortcut.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.Shortcut" /> values.</exception>
		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06002C95 RID: 11413 RVA: 0x000CF804 File Offset: 0x000CDA04
		// (set) Token: 0x06002C96 RID: 11414 RVA: 0x000CF814 File Offset: 0x000CDA14
		[Localizable(true)]
		[DefaultValue(Shortcut.None)]
		[SRDescription("MenuItemShortCutDescr")]
		public Shortcut Shortcut
		{
			get
			{
				return this.data.shortcut;
			}
			set
			{
				if (!Enum.IsDefined(typeof(Shortcut), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Shortcut));
				}
				this.data.shortcut = value;
				this.UpdateMenuItem(true);
			}
		}

		/// <summary>Gets or sets a value indicating whether the shortcut key that is associated with the menu item is displayed next to the menu item caption.</summary>
		/// <returns>
		///     <see langword="true" /> if the shortcut key combination is displayed next to the menu item caption; <see langword="false" /> if the shortcut key combination is not to be displayed. The default is <see langword="true" />.</returns>
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06002C97 RID: 11415 RVA: 0x000CF861 File Offset: 0x000CDA61
		// (set) Token: 0x06002C98 RID: 11416 RVA: 0x000CF86E File Offset: 0x000CDA6E
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("MenuItemShowShortCutDescr")]
		public bool ShowShortcut
		{
			get
			{
				return this.data.showShortcut;
			}
			set
			{
				if (value != this.data.showShortcut)
				{
					this.data.showShortcut = value;
					this.UpdateMenuItem(true);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the menu item is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the menu item will be made visible on the menu; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06002C99 RID: 11417 RVA: 0x000CF891 File Offset: 0x000CDA91
		// (set) Token: 0x06002C9A RID: 11418 RVA: 0x000CF8A7 File Offset: 0x000CDAA7
		[Localizable(true)]
		[DefaultValue(true)]
		[SRDescription("MenuItemVisibleDescr")]
		public bool Visible
		{
			get
			{
				return (this.data.State & 65536) == 0;
			}
			set
			{
				this.data.Visible = value;
			}
		}

		/// <summary>Occurs when the menu item is clicked or selected using a shortcut key or access key defined for the menu item.</summary>
		// Token: 0x1400020E RID: 526
		// (add) Token: 0x06002C9B RID: 11419 RVA: 0x000CF8B5 File Offset: 0x000CDAB5
		// (remove) Token: 0x06002C9C RID: 11420 RVA: 0x000CF8D3 File Offset: 0x000CDAD3
		[SRDescription("MenuItemOnClickDescr")]
		public event EventHandler Click
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onClick = (EventHandler)Delegate.Combine(menuItemData.onClick, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onClick = (EventHandler)Delegate.Remove(menuItemData.onClick, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.MenuItem.OwnerDraw" /> property of a menu item is set to <see langword="true" /> and a request is made to draw the menu item.</summary>
		// Token: 0x1400020F RID: 527
		// (add) Token: 0x06002C9D RID: 11421 RVA: 0x000CF8F1 File Offset: 0x000CDAF1
		// (remove) Token: 0x06002C9E RID: 11422 RVA: 0x000CF90F File Offset: 0x000CDB0F
		[SRCategory("CatBehavior")]
		[SRDescription("drawItemEventDescr")]
		public event DrawItemEventHandler DrawItem
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onDrawItem = (DrawItemEventHandler)Delegate.Combine(menuItemData.onDrawItem, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onDrawItem = (DrawItemEventHandler)Delegate.Remove(menuItemData.onDrawItem, value);
			}
		}

		/// <summary>Occurs when the menu needs to know the size of a menu item before drawing it.</summary>
		// Token: 0x14000210 RID: 528
		// (add) Token: 0x06002C9F RID: 11423 RVA: 0x000CF92D File Offset: 0x000CDB2D
		// (remove) Token: 0x06002CA0 RID: 11424 RVA: 0x000CF94B File Offset: 0x000CDB4B
		[SRCategory("CatBehavior")]
		[SRDescription("measureItemEventDescr")]
		public event MeasureItemEventHandler MeasureItem
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onMeasureItem = (MeasureItemEventHandler)Delegate.Combine(menuItemData.onMeasureItem, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onMeasureItem = (MeasureItemEventHandler)Delegate.Remove(menuItemData.onMeasureItem, value);
			}
		}

		/// <summary>Occurs before a menu item's list of menu items is displayed.</summary>
		// Token: 0x14000211 RID: 529
		// (add) Token: 0x06002CA1 RID: 11425 RVA: 0x000CF969 File Offset: 0x000CDB69
		// (remove) Token: 0x06002CA2 RID: 11426 RVA: 0x000CF987 File Offset: 0x000CDB87
		[SRDescription("MenuItemOnInitDescr")]
		public event EventHandler Popup
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onPopup = (EventHandler)Delegate.Combine(menuItemData.onPopup, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onPopup = (EventHandler)Delegate.Remove(menuItemData.onPopup, value);
			}
		}

		/// <summary>Occurs when the user places the pointer over a menu item.</summary>
		// Token: 0x14000212 RID: 530
		// (add) Token: 0x06002CA3 RID: 11427 RVA: 0x000CF9A5 File Offset: 0x000CDBA5
		// (remove) Token: 0x06002CA4 RID: 11428 RVA: 0x000CF9C3 File Offset: 0x000CDBC3
		[SRDescription("MenuItemOnSelectDescr")]
		public event EventHandler Select
		{
			add
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onSelect = (EventHandler)Delegate.Combine(menuItemData.onSelect, value);
			}
			remove
			{
				MenuItem.MenuItemData menuItemData = this.data;
				menuItemData.onSelect = (EventHandler)Delegate.Remove(menuItemData.onSelect, value);
			}
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000CF9E4 File Offset: 0x000CDBE4
		private static void CleanListItems(MenuItem senderMenu)
		{
			for (int i = senderMenu.MenuItems.Count - 1; i >= 0; i--)
			{
				MenuItem menuItem = senderMenu.MenuItems[i];
				if (menuItem.data.UserData is MenuItem.MdiListUserData)
				{
					menuItem.Dispose();
				}
			}
		}

		/// <summary>Creates a copy of the current <see cref="T:System.Windows.Forms.MenuItem" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MenuItem" /> that represents the duplicated menu item.</returns>
		// Token: 0x06002CA6 RID: 11430 RVA: 0x000CFA30 File Offset: 0x000CDC30
		public virtual MenuItem CloneMenu()
		{
			MenuItem menuItem = new MenuItem();
			menuItem.CloneMenu(this);
			return menuItem;
		}

		/// <summary>Creates a copy of the specified <see cref="T:System.Windows.Forms.MenuItem" />.</summary>
		/// <param name="itemSrc">The <see cref="T:System.Windows.Forms.MenuItem" /> that represents the menu item to copy. </param>
		// Token: 0x06002CA7 RID: 11431 RVA: 0x000CFA4C File Offset: 0x000CDC4C
		protected void CloneMenu(MenuItem itemSrc)
		{
			base.CloneMenu(itemSrc);
			int state = itemSrc.data.State;
			new MenuItem.MenuItemData(this, itemSrc.MergeType, itemSrc.MergeOrder, itemSrc.Shortcut, itemSrc.ShowShortcut, itemSrc.Text, itemSrc.data.onClick, itemSrc.data.onPopup, itemSrc.data.onSelect, itemSrc.data.onDrawItem, itemSrc.data.onMeasureItem);
			this.data.SetState(state & 201579, true);
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000CFADC File Offset: 0x000CDCDC
		internal virtual void CreateMenuItem()
		{
			if ((this.data.State & 65536) == 0)
			{
				NativeMethods.MENUITEMINFO_T menuiteminfo_T = this.CreateMenuItemInfo();
				UnsafeNativeMethods.InsertMenuItem(new HandleRef(this.menu, this.menu.handle), -1, true, menuiteminfo_T);
				this.hasHandle = (menuiteminfo_T.hSubMenu != IntPtr.Zero);
				this.dataVersion = this.data.version;
				this.menuItemIsCreated = true;
				if (this.RenderIsRightToLeft)
				{
					this.Menu.UpdateRtl(true);
				}
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000CFB64 File Offset: 0x000CDD64
		private NativeMethods.MENUITEMINFO_T CreateMenuItemInfo()
		{
			NativeMethods.MENUITEMINFO_T menuiteminfo_T = new NativeMethods.MENUITEMINFO_T();
			menuiteminfo_T.fMask = 55;
			menuiteminfo_T.fType = (this.data.State & 864);
			bool flag = false;
			if (this.menu == base.GetMainMenu())
			{
				flag = true;
			}
			if (this.data.caption.Equals("-"))
			{
				if (flag)
				{
					this.data.caption = " ";
					menuiteminfo_T.fType |= 64;
				}
				else
				{
					menuiteminfo_T.fType |= 2048;
				}
			}
			menuiteminfo_T.fState = (this.data.State & 4107);
			menuiteminfo_T.wID = this.MenuID;
			if (this.IsParent)
			{
				menuiteminfo_T.hSubMenu = base.Handle;
			}
			menuiteminfo_T.hbmpChecked = IntPtr.Zero;
			menuiteminfo_T.hbmpUnchecked = IntPtr.Zero;
			if (this.uniqueID == 0U)
			{
				Hashtable obj = MenuItem.allCreatedMenuItems;
				lock (obj)
				{
					this.uniqueID = (uint)Interlocked.Increment(ref MenuItem.nextUniqueID);
					MenuItem.allCreatedMenuItems.Add(this.uniqueID, new WeakReference(this));
				}
			}
			if (IntPtr.Size == 4)
			{
				if (this.data.OwnerDraw)
				{
					menuiteminfo_T.dwItemData = this.AllocMsaaMenuInfo();
				}
				else
				{
					menuiteminfo_T.dwItemData = (IntPtr)((int)this.uniqueID);
				}
			}
			else
			{
				menuiteminfo_T.dwItemData = this.AllocMsaaMenuInfo();
			}
			if (this.data.showShortcut && this.data.shortcut != Shortcut.None && !this.IsParent && !flag)
			{
				menuiteminfo_T.dwTypeData = this.data.caption + "\t" + TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys)this.data.shortcut);
			}
			else
			{
				menuiteminfo_T.dwTypeData = ((this.data.caption.Length == 0) ? " " : this.data.caption);
			}
			menuiteminfo_T.cch = 0;
			return menuiteminfo_T;
		}

		/// <summary>Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.MenuItem" />.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06002CAA RID: 11434 RVA: 0x000CFD7C File Offset: 0x000CDF7C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.menu != null)
				{
					this.menu.MenuItems.Remove(this);
				}
				if (this.data != null)
				{
					this.data.RemoveItem(this);
				}
				Hashtable obj = MenuItem.allCreatedMenuItems;
				lock (obj)
				{
					MenuItem.allCreatedMenuItems.Remove(this.uniqueID);
				}
				this.uniqueID = 0U;
			}
			this.FreeMsaaMenuInfo();
			base.Dispose(disposing);
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000CFE10 File Offset: 0x000CE010
		internal static MenuItem GetMenuItemFromUniqueID(uint uniqueID)
		{
			WeakReference weakReference = (WeakReference)MenuItem.allCreatedMenuItems[uniqueID];
			if (weakReference != null && weakReference.IsAlive)
			{
				return (MenuItem)weakReference.Target;
			}
			return null;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000CFE4C File Offset: 0x000CE04C
		internal static MenuItem GetMenuItemFromItemData(IntPtr itemData)
		{
			uint num = (uint)((long)itemData);
			if (num == 0U)
			{
				return null;
			}
			if (IntPtr.Size == 4)
			{
				if (num < 3221225472U)
				{
					MenuItem.MsaaMenuInfoWithId msaaMenuInfoWithId = (MenuItem.MsaaMenuInfoWithId)Marshal.PtrToStructure(itemData, typeof(MenuItem.MsaaMenuInfoWithId));
					num = msaaMenuInfoWithId.uniqueID;
				}
			}
			else
			{
				MenuItem.MsaaMenuInfoWithId msaaMenuInfoWithId2 = (MenuItem.MsaaMenuInfoWithId)Marshal.PtrToStructure(itemData, typeof(MenuItem.MsaaMenuInfoWithId));
				num = msaaMenuInfoWithId2.uniqueID;
			}
			return MenuItem.GetMenuItemFromUniqueID(num);
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000CFEB8 File Offset: 0x000CE0B8
		private IntPtr AllocMsaaMenuInfo()
		{
			this.FreeMsaaMenuInfo();
			this.msaaMenuInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MenuItem.MsaaMenuInfoWithId)));
			int size = IntPtr.Size;
			MenuItem.MsaaMenuInfoWithId msaaMenuInfoWithId = new MenuItem.MsaaMenuInfoWithId(this.data.caption, this.uniqueID);
			Marshal.StructureToPtr(msaaMenuInfoWithId, this.msaaMenuInfoPtr, false);
			return this.msaaMenuInfoPtr;
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000CFF1D File Offset: 0x000CE11D
		private void FreeMsaaMenuInfo()
		{
			if (this.msaaMenuInfoPtr != IntPtr.Zero)
			{
				Marshal.DestroyStructure(this.msaaMenuInfoPtr, typeof(MenuItem.MsaaMenuInfoWithId));
				Marshal.FreeHGlobal(this.msaaMenuInfoPtr);
				this.msaaMenuInfoPtr = IntPtr.Zero;
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000CFF5C File Offset: 0x000CE15C
		internal override void ItemsChanged(int change)
		{
			base.ItemsChanged(change);
			if (change == 0)
			{
				if (this.menu != null && this.menu.created)
				{
					this.UpdateMenuItem(true);
					base.CreateMenuItems();
					return;
				}
			}
			else
			{
				if (!this.hasHandle && this.IsParent)
				{
					this.UpdateMenuItem(true);
				}
				MainMenu mainMenu = base.GetMainMenu();
				if (mainMenu != null && (this.data.State & 512) == 0)
				{
					mainMenu.ItemsChanged(change, this);
				}
			}
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000CFFD4 File Offset: 0x000CE1D4
		internal void ItemsChanged(int change, MenuItem item)
		{
			if (change == 4 && this.data != null && this.data.baseItem != null && this.data.baseItem.MenuItems.Contains(item))
			{
				if (this.menu != null && this.menu.created)
				{
					this.UpdateMenuItem(true);
					base.CreateMenuItems();
					return;
				}
				if (this.data != null)
				{
					for (MenuItem firstItem = this.data.firstItem; firstItem != null; firstItem = firstItem.nextLinkedItem)
					{
						if (firstItem.created)
						{
							MenuItem item2 = item.CloneMenu();
							item.data.AddItem(item2);
							firstItem.MenuItems.Add(item2);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000D0088 File Offset: 0x000CE288
		internal Form[] FindMdiForms()
		{
			Form[] array = null;
			MainMenu mainMenu = base.GetMainMenu();
			Form form = null;
			if (mainMenu != null)
			{
				form = mainMenu.GetFormUnsafe();
			}
			if (form != null)
			{
				array = form.MdiChildren;
			}
			if (array == null)
			{
				array = new Form[0];
			}
			return array;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000D00C0 File Offset: 0x000CE2C0
		private void PopulateMdiList()
		{
			this.data.SetState(512, true);
			try
			{
				MenuItem.CleanListItems(this);
				Form[] array = this.FindMdiForms();
				if (array != null && array.Length != 0)
				{
					Form activeMdiChild = base.GetMainMenu().GetFormUnsafe().ActiveMdiChild;
					if (this.MenuItems.Count > 0)
					{
						MenuItem menuItem = (MenuItem)Activator.CreateInstance(base.GetType());
						menuItem.data.UserData = new MenuItem.MdiListUserData();
						menuItem.Text = "-";
						this.MenuItems.Add(menuItem);
					}
					int num = 0;
					int num2 = 1;
					int num3 = 0;
					bool flag = false;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].Visible)
						{
							num++;
							if ((flag && num3 < 9) || (!flag && num3 < 8) || array[i].Equals(activeMdiChild))
							{
								MenuItem menuItem2 = (MenuItem)Activator.CreateInstance(base.GetType());
								menuItem2.data.UserData = new MenuItem.MdiListFormData(this, i);
								if (array[i].Equals(activeMdiChild))
								{
									menuItem2.Checked = true;
									flag = true;
								}
								menuItem2.Text = string.Format(CultureInfo.CurrentUICulture, "&{0} {1}", new object[]
								{
									num2,
									array[i].Text
								});
								num2++;
								num3++;
								this.MenuItems.Add(menuItem2);
							}
						}
					}
					if (num > 9)
					{
						MenuItem menuItem3 = (MenuItem)Activator.CreateInstance(base.GetType());
						menuItem3.data.UserData = new MenuItem.MdiListMoreWindowsData(this);
						menuItem3.Text = SR.GetString("MDIMenuMoreWindows");
						this.MenuItems.Add(menuItem3);
					}
				}
			}
			finally
			{
				this.data.SetState(512, false);
			}
		}

		/// <summary>Merges this <see cref="T:System.Windows.Forms.MenuItem" /> with another <see cref="T:System.Windows.Forms.MenuItem" /> and returns the resulting merged <see cref="T:System.Windows.Forms.MenuItem" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MenuItem" /> that represents the merged menu item.</returns>
		// Token: 0x06002CB3 RID: 11443 RVA: 0x000D02B0 File Offset: 0x000CE4B0
		public virtual MenuItem MergeMenu()
		{
			MenuItem menuItem = (MenuItem)Activator.CreateInstance(base.GetType());
			this.data.AddItem(menuItem);
			menuItem.MergeMenu(this);
			return menuItem;
		}

		/// <summary>Merges another menu item with this menu item.</summary>
		/// <param name="itemSrc">A <see cref="T:System.Windows.Forms.MenuItem" /> that specifies the menu item to merge with this one. </param>
		// Token: 0x06002CB4 RID: 11444 RVA: 0x000D02E2 File Offset: 0x000CE4E2
		public void MergeMenu(MenuItem itemSrc)
		{
			base.MergeMenu(itemSrc);
			itemSrc.data.AddItem(this);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuItem.Click" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002CB5 RID: 11445 RVA: 0x000D02F8 File Offset: 0x000CE4F8
		protected virtual void OnClick(EventArgs e)
		{
			if (this.data.UserData is MenuItem.MdiListUserData)
			{
				((MenuItem.MdiListUserData)this.data.UserData).OnClick(e);
				return;
			}
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnClick(e);
				return;
			}
			if (this.data.onClick != null)
			{
				this.data.onClick(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuItem.DrawItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06002CB6 RID: 11446 RVA: 0x000D0370 File Offset: 0x000CE570
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnDrawItem(e);
				return;
			}
			if (this.data.onDrawItem != null)
			{
				this.data.onDrawItem(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuItem.MeasureItem" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MeasureItemEventArgs" /> that contains the event data. </param>
		// Token: 0x06002CB7 RID: 11447 RVA: 0x000D03BC File Offset: 0x000CE5BC
		protected virtual void OnMeasureItem(MeasureItemEventArgs e)
		{
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnMeasureItem(e);
				return;
			}
			if (this.data.onMeasureItem != null)
			{
				this.data.onMeasureItem(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuItem.Popup" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002CB8 RID: 11448 RVA: 0x000D0408 File Offset: 0x000CE608
		protected virtual void OnPopup(EventArgs e)
		{
			bool flag = false;
			for (int i = 0; i < base.ItemCount; i++)
			{
				if (this.items[i].MdiList)
				{
					flag = true;
					this.items[i].UpdateMenuItem(true);
				}
			}
			if (flag || (this.hasHandle && !this.IsParent))
			{
				this.UpdateMenuItem(true);
			}
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnPopup(e);
			}
			else if (this.data.onPopup != null)
			{
				this.data.onPopup(this, e);
			}
			for (int j = 0; j < base.ItemCount; j++)
			{
				this.items[j].UpdateMenuItemIfDirty();
			}
			if (this.MdiList)
			{
				this.PopulateMdiList();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuItem.Select" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002CB9 RID: 11449 RVA: 0x000D04D0 File Offset: 0x000CE6D0
		protected virtual void OnSelect(EventArgs e)
		{
			if (this.data.baseItem != this)
			{
				this.data.baseItem.OnSelect(e);
				return;
			}
			if (this.data.onSelect != null)
			{
				this.data.onSelect(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuItem.Popup" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002CBA RID: 11450 RVA: 0x000D051C File Offset: 0x000CE71C
		protected virtual void OnInitMenuPopup(EventArgs e)
		{
			this.OnPopup(e);
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000D0525 File Offset: 0x000CE725
		internal virtual void _OnInitMenuPopup(EventArgs e)
		{
			this.OnInitMenuPopup(e);
		}

		/// <summary>Generates a <see cref="E:System.Windows.Forms.Control.Click" /> event for the <see cref="T:System.Windows.Forms.MenuItem" />, simulating a click by a user.</summary>
		// Token: 0x06002CBC RID: 11452 RVA: 0x000D052E File Offset: 0x000CE72E
		public void PerformClick()
		{
			this.OnClick(EventArgs.Empty);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MenuItem.Select" /> event for this menu item.</summary>
		// Token: 0x06002CBD RID: 11453 RVA: 0x000D053B File Offset: 0x000CE73B
		public virtual void PerformSelect()
		{
			this.OnSelect(EventArgs.Empty);
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000D0548 File Offset: 0x000CE748
		internal virtual bool ShortcutClick()
		{
			if (this.menu is MenuItem)
			{
				MenuItem menuItem = (MenuItem)this.menu;
				if (!menuItem.ShortcutClick() || this.menu != menuItem)
				{
					return false;
				}
			}
			if ((this.data.State & 3) != 0)
			{
				return false;
			}
			if (base.ItemCount > 0)
			{
				this.OnPopup(EventArgs.Empty);
			}
			else
			{
				this.OnClick(EventArgs.Empty);
			}
			return true;
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.MenuItem" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.MenuItem" />. The string includes the type and the <see cref="P:System.Windows.Forms.MenuItem.Text" /> property of the control.</returns>
		// Token: 0x06002CBF RID: 11455 RVA: 0x000D05B4 File Offset: 0x000CE7B4
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = string.Empty;
			if (this.data != null && this.data.caption != null)
			{
				str2 = this.data.caption;
			}
			return str + ", Text: " + str2;
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x000D05FB File Offset: 0x000CE7FB
		internal void UpdateMenuItemIfDirty()
		{
			if (this.dataVersion != this.data.version)
			{
				this.UpdateMenuItem(true);
			}
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000D0618 File Offset: 0x000CE818
		internal void UpdateItemRtl(bool setRightToLeftBit)
		{
			if (!this.menuItemIsCreated)
			{
				return;
			}
			NativeMethods.MENUITEMINFO_T menuiteminfo_T = new NativeMethods.MENUITEMINFO_T();
			menuiteminfo_T.fMask = 21;
			menuiteminfo_T.dwTypeData = new string('\0', this.Text.Length + 2);
			menuiteminfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T));
			menuiteminfo_T.cch = menuiteminfo_T.dwTypeData.Length - 1;
			UnsafeNativeMethods.GetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), this.MenuID, false, menuiteminfo_T);
			if (setRightToLeftBit)
			{
				menuiteminfo_T.fType |= 24576;
			}
			else
			{
				menuiteminfo_T.fType &= -24577;
			}
			UnsafeNativeMethods.SetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), this.MenuID, false, menuiteminfo_T);
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x000D06F0 File Offset: 0x000CE8F0
		internal void UpdateMenuItem(bool force)
		{
			if (this.menu == null || !this.menu.created)
			{
				return;
			}
			if (force || this.menu is MainMenu || this.menu is ContextMenu)
			{
				NativeMethods.MENUITEMINFO_T menuiteminfo_T = this.CreateMenuItemInfo();
				UnsafeNativeMethods.SetMenuItemInfo(new HandleRef(this.menu, this.menu.handle), this.MenuID, false, menuiteminfo_T);
				if (this.hasHandle && menuiteminfo_T.hSubMenu == IntPtr.Zero)
				{
					base.ClearHandles();
				}
				this.hasHandle = (menuiteminfo_T.hSubMenu != IntPtr.Zero);
				this.dataVersion = this.data.version;
				if (this.menu is MainMenu)
				{
					Form formUnsafe = ((MainMenu)this.menu).GetFormUnsafe();
					if (formUnsafe != null)
					{
						SafeNativeMethods.DrawMenuBar(new HandleRef(formUnsafe, formUnsafe.Handle));
					}
				}
			}
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x000D07D8 File Offset: 0x000CE9D8
		internal void WmDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			IntPtr intPtr = Control.SetUpPalette(drawitemstruct.hDC, false, false);
			try
			{
				Graphics graphics = Graphics.FromHdcInternal(drawitemstruct.hDC);
				try
				{
					this.OnDrawItem(new DrawItemEventArgs(graphics, SystemInformation.MenuFont, Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom), this.Index, (DrawItemState)drawitemstruct.itemState));
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
					SafeNativeMethods.SelectPalette(new HandleRef(null, drawitemstruct.hDC), new HandleRef(null, intPtr), 0);
				}
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000D08BC File Offset: 0x000CEABC
		internal void WmMeasureItem(ref Message m)
		{
			NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			Graphics graphics = Graphics.FromHdcInternal(dc);
			MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, this.Index);
			try
			{
				this.OnMeasureItem(measureItemEventArgs);
			}
			finally
			{
				graphics.Dispose();
			}
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			measureitemstruct.itemHeight = measureItemEventArgs.ItemHeight;
			measureitemstruct.itemWidth = measureItemEventArgs.ItemWidth;
			Marshal.StructureToPtr(measureitemstruct, m.LParam, false);
			m.Result = (IntPtr)1;
		}

		// Token: 0x04001302 RID: 4866
		internal const int STATE_BARBREAK = 32;

		// Token: 0x04001303 RID: 4867
		internal const int STATE_BREAK = 64;

		// Token: 0x04001304 RID: 4868
		internal const int STATE_CHECKED = 8;

		// Token: 0x04001305 RID: 4869
		internal const int STATE_DEFAULT = 4096;

		// Token: 0x04001306 RID: 4870
		internal const int STATE_DISABLED = 3;

		// Token: 0x04001307 RID: 4871
		internal const int STATE_RADIOCHECK = 512;

		// Token: 0x04001308 RID: 4872
		internal const int STATE_HIDDEN = 65536;

		// Token: 0x04001309 RID: 4873
		internal const int STATE_MDILIST = 131072;

		// Token: 0x0400130A RID: 4874
		internal const int STATE_CLONE_MASK = 201579;

		// Token: 0x0400130B RID: 4875
		internal const int STATE_OWNERDRAW = 256;

		// Token: 0x0400130C RID: 4876
		internal const int STATE_INMDIPOPUP = 512;

		// Token: 0x0400130D RID: 4877
		internal const int STATE_HILITE = 128;

		// Token: 0x0400130E RID: 4878
		private Menu menu;

		// Token: 0x0400130F RID: 4879
		private bool hasHandle;

		// Token: 0x04001310 RID: 4880
		private MenuItem.MenuItemData data;

		// Token: 0x04001311 RID: 4881
		private int dataVersion;

		// Token: 0x04001312 RID: 4882
		private MenuItem nextLinkedItem;

		// Token: 0x04001313 RID: 4883
		private static Hashtable allCreatedMenuItems = new Hashtable();

		// Token: 0x04001314 RID: 4884
		private const uint firstUniqueID = 3221225472U;

		// Token: 0x04001315 RID: 4885
		private static long nextUniqueID = (long)((ulong)-1073741824);

		// Token: 0x04001316 RID: 4886
		private uint uniqueID;

		// Token: 0x04001317 RID: 4887
		private IntPtr msaaMenuInfoPtr;

		// Token: 0x04001318 RID: 4888
		private bool menuItemIsCreated;

		// Token: 0x0200061C RID: 1564
		private struct MsaaMenuInfoWithId
		{
			// Token: 0x06005E07 RID: 24071 RVA: 0x00186373 File Offset: 0x00184573
			public MsaaMenuInfoWithId(string text, uint uniqueID)
			{
				this.msaaMenuInfo = new NativeMethods.MSAAMENUINFO(text);
				this.uniqueID = uniqueID;
			}

			// Token: 0x04003A19 RID: 14873
			public NativeMethods.MSAAMENUINFO msaaMenuInfo;

			// Token: 0x04003A1A RID: 14874
			public uint uniqueID;
		}

		// Token: 0x0200061D RID: 1565
		internal class MenuItemData : ICommandExecutor
		{
			// Token: 0x06005E08 RID: 24072 RVA: 0x00186388 File Offset: 0x00184588
			internal MenuItemData(MenuItem baseItem, MenuMerge mergeType, int mergeOrder, Shortcut shortcut, bool showShortcut, string caption, EventHandler onClick, EventHandler onPopup, EventHandler onSelect, DrawItemEventHandler onDrawItem, MeasureItemEventHandler onMeasureItem)
			{
				this.AddItem(baseItem);
				this.mergeType = mergeType;
				this.mergeOrder = mergeOrder;
				this.shortcut = shortcut;
				this.showShortcut = showShortcut;
				this.caption = ((caption == null) ? "" : caption);
				this.onClick = onClick;
				this.onPopup = onPopup;
				this.onSelect = onSelect;
				this.onDrawItem = onDrawItem;
				this.onMeasureItem = onMeasureItem;
				this.version = 1;
				this.mnemonic = -1;
			}

			// Token: 0x1700168F RID: 5775
			// (get) Token: 0x06005E09 RID: 24073 RVA: 0x00186409 File Offset: 0x00184609
			// (set) Token: 0x06005E0A RID: 24074 RVA: 0x0018641A File Offset: 0x0018461A
			internal bool OwnerDraw
			{
				get
				{
					return (this.State & 256) != 0;
				}
				set
				{
					this.SetState(256, value);
				}
			}

			// Token: 0x17001690 RID: 5776
			// (get) Token: 0x06005E0B RID: 24075 RVA: 0x00186428 File Offset: 0x00184628
			// (set) Token: 0x06005E0C RID: 24076 RVA: 0x00186438 File Offset: 0x00184638
			internal bool MdiList
			{
				get
				{
					return this.HasState(131072);
				}
				set
				{
					if ((this.state & 131072) != 0 != value)
					{
						this.SetState(131072, value);
						for (MenuItem nextLinkedItem = this.firstItem; nextLinkedItem != null; nextLinkedItem = nextLinkedItem.nextLinkedItem)
						{
							nextLinkedItem.ItemsChanged(2);
						}
					}
				}
			}

			// Token: 0x17001691 RID: 5777
			// (get) Token: 0x06005E0D RID: 24077 RVA: 0x0018647D File Offset: 0x0018467D
			// (set) Token: 0x06005E0E RID: 24078 RVA: 0x00186485 File Offset: 0x00184685
			internal MenuMerge MergeType
			{
				get
				{
					return this.mergeType;
				}
				set
				{
					if (this.mergeType != value)
					{
						this.mergeType = value;
						this.ItemsChanged(3);
					}
				}
			}

			// Token: 0x17001692 RID: 5778
			// (get) Token: 0x06005E0F RID: 24079 RVA: 0x0018649E File Offset: 0x0018469E
			// (set) Token: 0x06005E10 RID: 24080 RVA: 0x001864A6 File Offset: 0x001846A6
			internal int MergeOrder
			{
				get
				{
					return this.mergeOrder;
				}
				set
				{
					if (this.mergeOrder != value)
					{
						this.mergeOrder = value;
						this.ItemsChanged(3);
					}
				}
			}

			// Token: 0x17001693 RID: 5779
			// (get) Token: 0x06005E11 RID: 24081 RVA: 0x001864BF File Offset: 0x001846BF
			internal char Mnemonic
			{
				get
				{
					if (this.mnemonic == -1)
					{
						this.mnemonic = (short)WindowsFormsUtils.GetMnemonic(this.caption, true);
					}
					return (char)this.mnemonic;
				}
			}

			// Token: 0x17001694 RID: 5780
			// (get) Token: 0x06005E12 RID: 24082 RVA: 0x001864E4 File Offset: 0x001846E4
			internal int State
			{
				get
				{
					return this.state;
				}
			}

			// Token: 0x17001695 RID: 5781
			// (get) Token: 0x06005E13 RID: 24083 RVA: 0x001864EC File Offset: 0x001846EC
			// (set) Token: 0x06005E14 RID: 24084 RVA: 0x001864FD File Offset: 0x001846FD
			internal bool Visible
			{
				get
				{
					return (this.state & 65536) == 0;
				}
				set
				{
					if ((this.state & 65536) == 0 != value)
					{
						this.state = (value ? (this.state & -65537) : (this.state | 65536));
						this.ItemsChanged(1);
					}
				}
			}

			// Token: 0x17001696 RID: 5782
			// (get) Token: 0x06005E15 RID: 24085 RVA: 0x0018653B File Offset: 0x0018473B
			// (set) Token: 0x06005E16 RID: 24086 RVA: 0x00186543 File Offset: 0x00184743
			internal object UserData
			{
				get
				{
					return this.userData;
				}
				set
				{
					this.userData = value;
				}
			}

			// Token: 0x06005E17 RID: 24087 RVA: 0x0018654C File Offset: 0x0018474C
			internal void AddItem(MenuItem item)
			{
				if (item.data != this)
				{
					if (item.data != null)
					{
						item.data.RemoveItem(item);
					}
					item.nextLinkedItem = this.firstItem;
					this.firstItem = item;
					if (this.baseItem == null)
					{
						this.baseItem = item;
					}
					item.data = this;
					item.dataVersion = 0;
					item.UpdateMenuItem(false);
				}
			}

			// Token: 0x06005E18 RID: 24088 RVA: 0x001865AD File Offset: 0x001847AD
			public void Execute()
			{
				if (this.baseItem != null)
				{
					this.baseItem.OnClick(EventArgs.Empty);
				}
			}

			// Token: 0x06005E19 RID: 24089 RVA: 0x001865C7 File Offset: 0x001847C7
			internal int GetMenuID()
			{
				if (this.cmd == null)
				{
					this.cmd = new Command(this);
				}
				return this.cmd.ID;
			}

			// Token: 0x06005E1A RID: 24090 RVA: 0x001865E8 File Offset: 0x001847E8
			internal void ItemsChanged(int change)
			{
				for (MenuItem nextLinkedItem = this.firstItem; nextLinkedItem != null; nextLinkedItem = nextLinkedItem.nextLinkedItem)
				{
					if (nextLinkedItem.menu != null)
					{
						nextLinkedItem.menu.ItemsChanged(change);
					}
				}
			}

			// Token: 0x06005E1B RID: 24091 RVA: 0x0018661C File Offset: 0x0018481C
			internal void RemoveItem(MenuItem item)
			{
				if (item == this.firstItem)
				{
					this.firstItem = item.nextLinkedItem;
				}
				else
				{
					MenuItem nextLinkedItem = this.firstItem;
					while (item != nextLinkedItem.nextLinkedItem)
					{
						nextLinkedItem = nextLinkedItem.nextLinkedItem;
					}
					nextLinkedItem.nextLinkedItem = item.nextLinkedItem;
				}
				item.nextLinkedItem = null;
				item.data = null;
				item.dataVersion = 0;
				if (item == this.baseItem)
				{
					this.baseItem = this.firstItem;
				}
				if (this.firstItem == null)
				{
					this.onClick = null;
					this.onPopup = null;
					this.onSelect = null;
					this.onDrawItem = null;
					this.onMeasureItem = null;
					if (this.cmd != null)
					{
						this.cmd.Dispose();
						this.cmd = null;
					}
				}
			}

			// Token: 0x06005E1C RID: 24092 RVA: 0x001866D4 File Offset: 0x001848D4
			internal void SetCaption(string value)
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.caption.Equals(value))
				{
					this.caption = value;
					this.UpdateMenuItems();
				}
			}

			// Token: 0x06005E1D RID: 24093 RVA: 0x001866FB File Offset: 0x001848FB
			internal bool HasState(int flag)
			{
				return (this.State & flag) == flag;
			}

			// Token: 0x06005E1E RID: 24094 RVA: 0x00186708 File Offset: 0x00184908
			internal void SetState(int flag, bool value)
			{
				if ((this.state & flag) != 0 != value)
				{
					this.state = (value ? (this.state | flag) : (this.state & ~flag));
					this.UpdateMenuItems();
				}
			}

			// Token: 0x06005E1F RID: 24095 RVA: 0x0018673C File Offset: 0x0018493C
			internal void UpdateMenuItems()
			{
				this.version++;
				for (MenuItem nextLinkedItem = this.firstItem; nextLinkedItem != null; nextLinkedItem = nextLinkedItem.nextLinkedItem)
				{
					nextLinkedItem.UpdateMenuItem(true);
				}
			}

			// Token: 0x04003A1B RID: 14875
			internal MenuItem baseItem;

			// Token: 0x04003A1C RID: 14876
			internal MenuItem firstItem;

			// Token: 0x04003A1D RID: 14877
			private int state;

			// Token: 0x04003A1E RID: 14878
			internal int version;

			// Token: 0x04003A1F RID: 14879
			internal MenuMerge mergeType;

			// Token: 0x04003A20 RID: 14880
			internal int mergeOrder;

			// Token: 0x04003A21 RID: 14881
			internal string caption;

			// Token: 0x04003A22 RID: 14882
			internal short mnemonic;

			// Token: 0x04003A23 RID: 14883
			internal Shortcut shortcut;

			// Token: 0x04003A24 RID: 14884
			internal bool showShortcut;

			// Token: 0x04003A25 RID: 14885
			internal EventHandler onClick;

			// Token: 0x04003A26 RID: 14886
			internal EventHandler onPopup;

			// Token: 0x04003A27 RID: 14887
			internal EventHandler onSelect;

			// Token: 0x04003A28 RID: 14888
			internal DrawItemEventHandler onDrawItem;

			// Token: 0x04003A29 RID: 14889
			internal MeasureItemEventHandler onMeasureItem;

			// Token: 0x04003A2A RID: 14890
			private object userData;

			// Token: 0x04003A2B RID: 14891
			internal Command cmd;
		}

		// Token: 0x0200061E RID: 1566
		private class MdiListUserData
		{
			// Token: 0x06005E20 RID: 24096 RVA: 0x0000701A File Offset: 0x0000521A
			public virtual void OnClick(EventArgs e)
			{
			}
		}

		// Token: 0x0200061F RID: 1567
		private class MdiListFormData : MenuItem.MdiListUserData
		{
			// Token: 0x06005E22 RID: 24098 RVA: 0x00186771 File Offset: 0x00184971
			public MdiListFormData(MenuItem parentItem, int boundFormIndex)
			{
				this.boundIndex = boundFormIndex;
				this.parent = parentItem;
			}

			// Token: 0x06005E23 RID: 24099 RVA: 0x00186788 File Offset: 0x00184988
			public override void OnClick(EventArgs e)
			{
				if (this.boundIndex != -1)
				{
					IntSecurity.ModifyFocus.Assert();
					try
					{
						Form[] array = this.parent.FindMdiForms();
						if (array != null && array.Length > this.boundIndex)
						{
							Form form = array[this.boundIndex];
							form.Activate();
							if (form.ActiveControl != null && !form.ActiveControl.Focused)
							{
								form.ActiveControl.Focus();
							}
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}

			// Token: 0x04003A2C RID: 14892
			private MenuItem parent;

			// Token: 0x04003A2D RID: 14893
			private int boundIndex;
		}

		// Token: 0x02000620 RID: 1568
		private class MdiListMoreWindowsData : MenuItem.MdiListUserData
		{
			// Token: 0x06005E24 RID: 24100 RVA: 0x0018680C File Offset: 0x00184A0C
			public MdiListMoreWindowsData(MenuItem parent)
			{
				this.parent = parent;
			}

			// Token: 0x06005E25 RID: 24101 RVA: 0x0018681C File Offset: 0x00184A1C
			public override void OnClick(EventArgs e)
			{
				Form[] array = this.parent.FindMdiForms();
				Form activeMdiChild = this.parent.GetMainMenu().GetFormUnsafe().ActiveMdiChild;
				if (array != null && array.Length != 0 && activeMdiChild != null)
				{
					IntSecurity.AllWindows.Assert();
					try
					{
						using (MdiWindowDialog mdiWindowDialog = new MdiWindowDialog())
						{
							mdiWindowDialog.SetItems(activeMdiChild, array);
							DialogResult dialogResult = mdiWindowDialog.ShowDialog();
							if (dialogResult == DialogResult.OK)
							{
								mdiWindowDialog.ActiveChildForm.Activate();
								if (mdiWindowDialog.ActiveChildForm.ActiveControl != null && !mdiWindowDialog.ActiveChildForm.ActiveControl.Focused)
								{
									mdiWindowDialog.ActiveChildForm.ActiveControl.Focus();
								}
							}
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}

			// Token: 0x04003A2E RID: 14894
			private MenuItem parent;
		}
	}
}
