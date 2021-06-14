using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents the base functionality for all menus. Although <see cref="T:System.Windows.Forms.ToolStripDropDown" /> and <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> replace and add functionality to the <see cref="T:System.Windows.Forms.Menu" /> control of previous versions, <see cref="T:System.Windows.Forms.Menu" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x020002E2 RID: 738
	[ToolboxItemFilter("System.Windows.Forms")]
	[ListBindable(false)]
	public abstract class Menu : Component
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Menu" /> class.</summary>
		/// <param name="items">An array of type <see cref="T:System.Windows.Forms.MenuItem" /> containing the objects to add to the menu.</param>
		// Token: 0x06002C4A RID: 11338 RVA: 0x000CE98F File Offset: 0x000CCB8F
		protected Menu(MenuItem[] items)
		{
			if (items != null)
			{
				this.MenuItems.AddRange(items);
			}
		}

		/// <summary>Gets a value representing the window handle for the menu.</summary>
		/// <returns>The HMENU value of the menu.</returns>
		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06002C4B RID: 11339 RVA: 0x000CE9A6 File Offset: 0x000CCBA6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlHandleDescr")]
		public IntPtr Handle
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					this.handle = this.CreateMenuHandle();
				}
				this.CreateMenuItems();
				return this.handle;
			}
		}

		/// <summary>Gets a value indicating whether this menu contains any menu items. This property is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if this menu contains <see cref="T:System.Windows.Forms.MenuItem" /> objects; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000CE9D2 File Offset: 0x000CCBD2
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("MenuIsParentDescr")]
		public virtual bool IsParent
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.items != null && this.ItemCount > 0;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06002C4D RID: 11341 RVA: 0x000CE9E7 File Offset: 0x000CCBE7
		internal int ItemCount
		{
			get
			{
				return this._itemCount;
			}
		}

		/// <summary>Gets a value indicating the <see cref="T:System.Windows.Forms.MenuItem" /> that is used to display a list of multiple document interface (MDI) child forms.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.MenuItem" /> that represents the menu item displaying a list of MDI child forms that are open in the application.</returns>
		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06002C4E RID: 11342 RVA: 0x000CE9F0 File Offset: 0x000CCBF0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("MenuMDIListItemDescr")]
		public MenuItem MdiListItem
		{
			get
			{
				for (int i = 0; i < this.ItemCount; i++)
				{
					MenuItem menuItem = this.items[i];
					if (menuItem.MdiList)
					{
						return menuItem;
					}
					if (menuItem.IsParent)
					{
						menuItem = menuItem.MdiListItem;
						if (menuItem != null)
						{
							return menuItem;
						}
					}
				}
				return null;
			}
		}

		/// <summary>Gets or sets the name of the <see cref="T:System.Windows.Forms.Menu" />.</summary>
		/// <returns>A string representing the name.</returns>
		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06002C4F RID: 11343 RVA: 0x000CEA36 File Offset: 0x000CCC36
		// (set) Token: 0x06002C50 RID: 11344 RVA: 0x000CEA44 File Offset: 0x000CCC44
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, this.name);
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.name = null;
				}
				else
				{
					this.name = value;
				}
				if (this.Site != null)
				{
					this.Site.Name = this.name;
				}
			}
		}

		/// <summary>Gets a value indicating the collection of <see cref="T:System.Windows.Forms.MenuItem" /> objects associated with the menu.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Menu.MenuItemCollection" /> that represents the list of <see cref="T:System.Windows.Forms.MenuItem" /> objects stored in the menu.</returns>
		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06002C51 RID: 11345 RVA: 0x000CEA7A File Offset: 0x000CCC7A
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("MenuMenuItemsDescr")]
		[MergableProperty(false)]
		public Menu.MenuItemCollection MenuItems
		{
			get
			{
				if (this.itemsCollection == null)
				{
					this.itemsCollection = new Menu.MenuItemCollection(this);
				}
				return this.itemsCollection;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06002C52 RID: 11346 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool RenderIsRightToLeft
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets user-defined data associated with the control.</summary>
		/// <returns>An object representing the data.</returns>
		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06002C53 RID: 11347 RVA: 0x000CEA96 File Offset: 0x000CCC96
		// (set) Token: 0x06002C54 RID: 11348 RVA: 0x000CEA9E File Offset: 0x000CCC9E
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
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

		// Token: 0x06002C55 RID: 11349 RVA: 0x000CEAA8 File Offset: 0x000CCCA8
		internal void ClearHandles()
		{
			if (this.handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.DestroyMenu(new HandleRef(this, this.handle));
			}
			this.handle = IntPtr.Zero;
			if (this.created)
			{
				for (int i = 0; i < this.ItemCount; i++)
				{
					this.items[i].ClearHandles();
				}
				this.created = false;
			}
		}

		/// <summary>Copies the <see cref="T:System.Windows.Forms.Menu" /> that is passed as a parameter to the current <see cref="T:System.Windows.Forms.Menu" />.</summary>
		/// <param name="menuSrc">The <see cref="T:System.Windows.Forms.Menu" /> to copy. </param>
		// Token: 0x06002C56 RID: 11350 RVA: 0x000CEB14 File Offset: 0x000CCD14
		protected internal void CloneMenu(Menu menuSrc)
		{
			MenuItem[] array = null;
			if (menuSrc.items != null)
			{
				int count = menuSrc.MenuItems.Count;
				array = new MenuItem[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = menuSrc.MenuItems[i].CloneMenu();
				}
			}
			this.MenuItems.Clear();
			if (array != null)
			{
				this.MenuItems.AddRange(array);
			}
		}

		/// <summary>Creates a new handle to the <see cref="T:System.Windows.Forms.Menu" />.</summary>
		/// <returns>A handle to the menu if the method succeeds; otherwise, <see langword="null" />.</returns>
		// Token: 0x06002C57 RID: 11351 RVA: 0x000CEB78 File Offset: 0x000CCD78
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual IntPtr CreateMenuHandle()
		{
			return UnsafeNativeMethods.CreatePopupMenu();
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x000CEB80 File Offset: 0x000CCD80
		internal void CreateMenuItems()
		{
			if (!this.created)
			{
				for (int i = 0; i < this.ItemCount; i++)
				{
					this.items[i].CreateMenuItem();
				}
				this.created = true;
			}
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x000CEBBC File Offset: 0x000CCDBC
		internal void DestroyMenuItems()
		{
			if (this.created)
			{
				for (int i = 0; i < this.ItemCount; i++)
				{
					this.items[i].ClearHandles();
				}
				while (UnsafeNativeMethods.GetMenuItemCount(new HandleRef(this, this.handle)) > 0)
				{
					UnsafeNativeMethods.RemoveMenu(new HandleRef(this, this.handle), 0, 1024);
				}
				this.created = false;
			}
		}

		/// <summary>Disposes of the resources, other than memory, used by the <see cref="T:System.Windows.Forms.Menu" />.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06002C5A RID: 11354 RVA: 0x000CEC24 File Offset: 0x000CCE24
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				while (this.ItemCount > 0)
				{
					MenuItem[] array = this.items;
					int num = this._itemCount - 1;
					this._itemCount = num;
					MenuItem menuItem = array[num];
					if (menuItem.Site != null && menuItem.Site.Container != null)
					{
						menuItem.Site.Container.Remove(menuItem);
					}
					menuItem.Menu = null;
					menuItem.Dispose();
				}
				this.items = null;
			}
			if (this.handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.DestroyMenu(new HandleRef(this, this.handle));
				this.handle = IntPtr.Zero;
				if (disposing)
				{
					this.ClearHandles();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.MenuItem" /> that contains the value specified. </summary>
		/// <param name="type">The type of item to use to find the <see cref="T:System.Windows.Forms.MenuItem" />.</param>
		/// <param name="value">The item to use to find the <see cref="T:System.Windows.Forms.MenuItem" />.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.MenuItem" /> that matches value; otherwise, <see langword="null" />.</returns>
		// Token: 0x06002C5B RID: 11355 RVA: 0x000CECD1 File Offset: 0x000CCED1
		public MenuItem FindMenuItem(int type, IntPtr value)
		{
			IntSecurity.ControlFromHandleOrLocation.Demand();
			return this.FindMenuItemInternal(type, value);
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x000CECE8 File Offset: 0x000CCEE8
		private MenuItem FindMenuItemInternal(int type, IntPtr value)
		{
			for (int i = 0; i < this.ItemCount; i++)
			{
				MenuItem menuItem = this.items[i];
				if (type != 0)
				{
					if (type == 1)
					{
						if (menuItem.Shortcut == (Shortcut)((int)value))
						{
							return menuItem;
						}
					}
				}
				else if (menuItem.handle == value)
				{
					return menuItem;
				}
				menuItem = menuItem.FindMenuItemInternal(type, value);
				if (menuItem != null)
				{
					return menuItem;
				}
			}
			return null;
		}

		/// <summary>Returns the position at which a menu item should be inserted into the menu.</summary>
		/// <param name="mergeOrder">The merge order position for the menu item to be merged.</param>
		/// <returns>The position at which a menu item should be inserted into the menu.</returns>
		// Token: 0x06002C5D RID: 11357 RVA: 0x000CED48 File Offset: 0x000CCF48
		protected int FindMergePosition(int mergeOrder)
		{
			int i = 0;
			int num = this.ItemCount;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (this.items[num2].MergeOrder <= mergeOrder)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			return i;
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x000CED84 File Offset: 0x000CCF84
		internal int xFindMergePosition(int mergeOrder)
		{
			int result = 0;
			int num = 0;
			while (num < this.ItemCount && this.items[num].MergeOrder <= mergeOrder)
			{
				if (this.items[num].MergeOrder < mergeOrder)
				{
					result = num + 1;
				}
				else if (mergeOrder == this.items[num].MergeOrder)
				{
					result = num;
					break;
				}
				num++;
			}
			return result;
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000CEDE0 File Offset: 0x000CCFE0
		internal void UpdateRtl(bool setRightToLeftBit)
		{
			foreach (object obj in this.MenuItems)
			{
				MenuItem menuItem = (MenuItem)obj;
				menuItem.UpdateItemRtl(setRightToLeftBit);
				menuItem.UpdateRtl(setRightToLeftBit);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ContextMenu" /> that contains this menu.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenu" /> that contains this menu. The default is <see langword="null" />.</returns>
		// Token: 0x06002C60 RID: 11360 RVA: 0x000CEE40 File Offset: 0x000CD040
		public ContextMenu GetContextMenu()
		{
			Menu menu = this;
			while (!(menu is ContextMenu))
			{
				if (!(menu is MenuItem))
				{
					return null;
				}
				menu = ((MenuItem)menu).Menu;
			}
			return (ContextMenu)menu;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.MainMenu" /> that contains this menu.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.MainMenu" /> that contains this menu.</returns>
		// Token: 0x06002C61 RID: 11361 RVA: 0x000CEE78 File Offset: 0x000CD078
		public MainMenu GetMainMenu()
		{
			Menu menu = this;
			while (!(menu is MainMenu))
			{
				if (!(menu is MenuItem))
				{
					return null;
				}
				menu = ((MenuItem)menu).Menu;
			}
			return (MainMenu)menu;
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x000CEEAD File Offset: 0x000CD0AD
		internal virtual void ItemsChanged(int change)
		{
			if (change <= 1)
			{
				this.DestroyMenuItems();
			}
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x000CEEBC File Offset: 0x000CD0BC
		private IntPtr MatchKeyToMenuItem(int startItem, char key, Menu.MenuItemKeyComparer comparer)
		{
			int num = -1;
			bool flag = false;
			int num2 = 0;
			while (num2 < this.items.Length && !flag)
			{
				int num3 = (startItem + num2) % this.items.Length;
				MenuItem menuItem = this.items[num3];
				if (menuItem != null && comparer(menuItem, key))
				{
					if (num < 0)
					{
						num = menuItem.MenuIndex;
					}
					else
					{
						flag = true;
					}
				}
				num2++;
			}
			if (num < 0)
			{
				return IntPtr.Zero;
			}
			int high = flag ? 3 : 2;
			return (IntPtr)NativeMethods.Util.MAKELONG(num, high);
		}

		/// <summary>Merges the <see cref="T:System.Windows.Forms.MenuItem" /> objects of one menu with the current menu.</summary>
		/// <param name="menuSrc">The <see cref="T:System.Windows.Forms.Menu" /> whose menu items are merged with the menu items of the current menu. </param>
		/// <exception cref="T:System.ArgumentException">It was attempted to merge the menu with itself. </exception>
		// Token: 0x06002C64 RID: 11364 RVA: 0x000CEF3C File Offset: 0x000CD13C
		public virtual void MergeMenu(Menu menuSrc)
		{
			if (menuSrc == this)
			{
				throw new ArgumentException(SR.GetString("MenuMergeWithSelf"), "menuSrc");
			}
			if (menuSrc.items != null && this.items == null)
			{
				this.MenuItems.Clear();
			}
			for (int i = 0; i < menuSrc.ItemCount; i++)
			{
				MenuItem menuItem = menuSrc.items[i];
				MenuMerge mergeType = menuItem.MergeType;
				if (mergeType != MenuMerge.Add)
				{
					if (mergeType - MenuMerge.Replace <= 1)
					{
						int mergeOrder = menuItem.MergeOrder;
						int j = this.xFindMergePosition(mergeOrder);
						while (j < this.ItemCount)
						{
							MenuItem menuItem2 = this.items[j];
							if (menuItem2.MergeOrder != mergeOrder)
							{
								this.MenuItems.Add(j, menuItem.MergeMenu());
								goto IL_11D;
							}
							if (menuItem2.MergeType != MenuMerge.Add)
							{
								if (menuItem.MergeType != MenuMerge.MergeItems || menuItem2.MergeType != MenuMerge.MergeItems)
								{
									menuItem2.Dispose();
									this.MenuItems.Add(j, menuItem.MergeMenu());
									goto IL_11D;
								}
								menuItem2.MergeMenu(menuItem);
								goto IL_11D;
							}
							else
							{
								j++;
							}
						}
						this.MenuItems.Add(j, menuItem.MergeMenu());
					}
				}
				else
				{
					this.MenuItems.Add(this.FindMergePosition(menuItem.MergeOrder), menuItem.MergeMenu());
				}
				IL_11D:;
			}
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000CF078 File Offset: 0x000CD278
		internal virtual bool ProcessInitMenuPopup(IntPtr handle)
		{
			MenuItem menuItem = this.FindMenuItemInternal(0, handle);
			if (menuItem != null)
			{
				menuItem._OnInitMenuPopup(EventArgs.Empty);
				menuItem.CreateMenuItems();
				return true;
			}
			return false;
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference that represents the window message to process.</param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
		/// <returns>
		///     <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002C66 RID: 11366 RVA: 0x000CF0A8 File Offset: 0x000CD2A8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal virtual bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			MenuItem menuItem = this.FindMenuItemInternal(1, (IntPtr)((int)keyData));
			return menuItem != null && menuItem.ShortcutClick();
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06002C67 RID: 11367 RVA: 0x000CF0D0 File Offset: 0x000CD2D0
		internal int SelectedMenuItemIndex
		{
			get
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					MenuItem menuItem = this.items[i];
					if (menuItem != null && menuItem.Selected)
					{
						return i;
					}
				}
				return -1;
			}
		}

		/// <summary>Returns a <see cref="T:System.String" /> that represents the <see cref="T:System.Windows.Forms.Menu" /> control.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the current <see cref="T:System.Windows.Forms.Menu" />.</returns>
		// Token: 0x06002C68 RID: 11368 RVA: 0x000CF108 File Offset: 0x000CD308
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Items.Count: " + this.ItemCount.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000CF13C File Offset: 0x000CD33C
		internal void WmMenuChar(ref Message m)
		{
			Menu menu = (m.LParam == this.handle) ? this : this.FindMenuItemInternal(0, m.LParam);
			if (menu == null)
			{
				return;
			}
			char key = char.ToUpper((char)NativeMethods.Util.LOWORD(m.WParam), CultureInfo.CurrentCulture);
			m.Result = menu.WmMenuCharInternal(key);
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000CF198 File Offset: 0x000CD398
		internal IntPtr WmMenuCharInternal(char key)
		{
			int startItem = (this.SelectedMenuItemIndex + 1) % this.items.Length;
			IntPtr intPtr = this.MatchKeyToMenuItem(startItem, key, new Menu.MenuItemKeyComparer(this.CheckOwnerDrawItemWithMnemonic));
			if (intPtr == IntPtr.Zero)
			{
				intPtr = this.MatchKeyToMenuItem(startItem, key, new Menu.MenuItemKeyComparer(this.CheckOwnerDrawItemNoMnemonic));
			}
			return intPtr;
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000CF1EF File Offset: 0x000CD3EF
		private bool CheckOwnerDrawItemWithMnemonic(MenuItem mi, char key)
		{
			return mi.OwnerDraw && mi.Mnemonic == key;
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x000CF204 File Offset: 0x000CD404
		private bool CheckOwnerDrawItemNoMnemonic(MenuItem mi, char key)
		{
			return mi.OwnerDraw && mi.Mnemonic == '\0' && mi.Text.Length > 0 && char.ToUpper(mi.Text[0], CultureInfo.CurrentCulture) == key;
		}

		// Token: 0x040012EE RID: 4846
		internal const int CHANGE_ITEMS = 0;

		// Token: 0x040012EF RID: 4847
		internal const int CHANGE_VISIBLE = 1;

		// Token: 0x040012F0 RID: 4848
		internal const int CHANGE_MDI = 2;

		// Token: 0x040012F1 RID: 4849
		internal const int CHANGE_MERGE = 3;

		// Token: 0x040012F2 RID: 4850
		internal const int CHANGE_ITEMADDED = 4;

		/// <summary>Specifies that the <see cref="M:System.Windows.Forms.Menu.FindMenuItem(System.Int32,System.IntPtr)" /> method should search for a handle.</summary>
		// Token: 0x040012F3 RID: 4851
		public const int FindHandle = 0;

		/// <summary>Specifies that the <see cref="M:System.Windows.Forms.Menu.FindMenuItem(System.Int32,System.IntPtr)" /> method should search for a shortcut.</summary>
		// Token: 0x040012F4 RID: 4852
		public const int FindShortcut = 1;

		// Token: 0x040012F5 RID: 4853
		private Menu.MenuItemCollection itemsCollection;

		// Token: 0x040012F6 RID: 4854
		internal MenuItem[] items;

		// Token: 0x040012F7 RID: 4855
		private int _itemCount;

		// Token: 0x040012F8 RID: 4856
		internal IntPtr handle;

		// Token: 0x040012F9 RID: 4857
		internal bool created;

		// Token: 0x040012FA RID: 4858
		private object userData;

		// Token: 0x040012FB RID: 4859
		private string name;

		// Token: 0x0200061A RID: 1562
		// (Invoke) Token: 0x06005DE2 RID: 24034
		private delegate bool MenuItemKeyComparer(MenuItem mi, char key);

		/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.MenuItem" /> objects.</summary>
		// Token: 0x0200061B RID: 1563
		[ListBindable(false)]
		public class MenuItemCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Menu.MenuItemCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.Menu" /> that owns this collection. </param>
			// Token: 0x06005DE5 RID: 24037 RVA: 0x00185BA6 File Offset: 0x00183DA6
			public MenuItemCollection(Menu owner)
			{
				this.owner = owner;
			}

			/// <summary>Retrieves the <see cref="T:System.Windows.Forms.MenuItem" /> at the specified indexed location in the collection.</summary>
			/// <param name="index">The indexed location of the <see cref="T:System.Windows.Forms.MenuItem" /> in the collection. </param>
			/// <returns>The <see cref="T:System.Windows.Forms.MenuItem" /> at the specified location.</returns>
			/// <exception cref="T:System.ArgumentException">The <paramref name="value" /> parameter is <see langword="null" />.or The <paramref name="index" /> parameter is less than zero.or The <paramref name="index" /> parameter is greater than the number of menu items in the collection, and the collection of menu items is not <see langword="null" />. </exception>
			// Token: 0x17001687 RID: 5767
			public virtual MenuItem this[int index]
			{
				get
				{
					if (index < 0 || index >= this.owner.ItemCount)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.items[index];
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the element to get.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.MenuItem" /> at the specified index.</returns>
			// Token: 0x17001688 RID: 5768
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			/// <summary>Gets an item with the specified key from the collection.</summary>
			/// <param name="key">The name of the item to retrieve from the collection.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.MenuItem" /> with the specified key.</returns>
			// Token: 0x17001689 RID: 5769
			public virtual MenuItem this[string key]
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

			/// <summary>Gets a value indicating the total number of <see cref="T:System.Windows.Forms.MenuItem" /> objects in the collection.</summary>
			/// <returns>The number of <see cref="T:System.Windows.Forms.MenuItem" /> objects in the collection.</returns>
			// Token: 0x1700168A RID: 5770
			// (get) Token: 0x06005DEA RID: 24042 RVA: 0x00185C55 File Offset: 0x00183E55
			public int Count
			{
				get
				{
					return this.owner.ItemCount;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.Menu.MenuItemCollection" />.</returns>
			// Token: 0x1700168B RID: 5771
			// (get) Token: 0x06005DEB RID: 24043 RVA: 0x000069BD File Offset: 0x00004BBD
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
			// Token: 0x1700168C RID: 5772
			// (get) Token: 0x06005DEC RID: 24044 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
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
			// Token: 0x1700168D RID: 5773
			// (get) Token: 0x06005DED RID: 24045 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
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
			// Token: 0x1700168E RID: 5774
			// (get) Token: 0x06005DEE RID: 24046 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds a new <see cref="T:System.Windows.Forms.MenuItem" />, to the end of the current menu, with a specified caption.</summary>
			/// <param name="caption">The caption of the menu item. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.MenuItem" /> that represents the menu item being added to the collection.</returns>
			// Token: 0x06005DEF RID: 24047 RVA: 0x00185C64 File Offset: 0x00183E64
			public virtual MenuItem Add(string caption)
			{
				MenuItem menuItem = new MenuItem(caption);
				this.Add(menuItem);
				return menuItem;
			}

			/// <summary>Adds a new <see cref="T:System.Windows.Forms.MenuItem" /> to the end of the current menu with a specified caption and a specified event handler for the <see cref="E:System.Windows.Forms.MenuItem.Click" /> event.</summary>
			/// <param name="caption">The caption of the menu item. </param>
			/// <param name="onClick">An <see cref="T:System.EventHandler" /> that represents the event handler that is called when the item is clicked by the user, or when a user presses an accelerator or shortcut key for the menu item. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.MenuItem" /> that represents the menu item being added to the collection.</returns>
			// Token: 0x06005DF0 RID: 24048 RVA: 0x00185C84 File Offset: 0x00183E84
			public virtual MenuItem Add(string caption, EventHandler onClick)
			{
				MenuItem menuItem = new MenuItem(caption, onClick);
				this.Add(menuItem);
				return menuItem;
			}

			/// <summary>Adds a new <see cref="T:System.Windows.Forms.MenuItem" /> to the end of this menu with the specified caption, <see cref="E:System.Windows.Forms.MenuItem.Click" /> event handler, and items.</summary>
			/// <param name="caption">The caption of the menu item. </param>
			/// <param name="items">An array of <see cref="T:System.Windows.Forms.MenuItem" /> objects that this <see cref="T:System.Windows.Forms.MenuItem" /> will contain. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.MenuItem" /> that represents the menu item being added to the collection.</returns>
			// Token: 0x06005DF1 RID: 24049 RVA: 0x00185CA4 File Offset: 0x00183EA4
			public virtual MenuItem Add(string caption, MenuItem[] items)
			{
				MenuItem menuItem = new MenuItem(caption, items);
				this.Add(menuItem);
				return menuItem;
			}

			/// <summary>Adds a previously created <see cref="T:System.Windows.Forms.MenuItem" /> to the end of the current menu.</summary>
			/// <param name="item">The <see cref="T:System.Windows.Forms.MenuItem" /> to add. </param>
			/// <returns>The zero-based index where the item is stored in the collection.</returns>
			// Token: 0x06005DF2 RID: 24050 RVA: 0x00185CC2 File Offset: 0x00183EC2
			public virtual int Add(MenuItem item)
			{
				return this.Add(this.owner.ItemCount, item);
			}

			/// <summary>Adds a previously created <see cref="T:System.Windows.Forms.MenuItem" /> at the specified index within the menu item collection.</summary>
			/// <param name="index">The position to add the new item. </param>
			/// <param name="item">The <see cref="T:System.Windows.Forms.MenuItem" /> to add. </param>
			/// <returns>The zero-based index where the item is stored in the collection.</returns>
			/// <exception cref="T:System.Exception">The <see cref="T:System.Windows.Forms.MenuItem" /> being added is already in use. </exception>
			/// <exception cref="T:System.ArgumentException">The index supplied in the <paramref name="index" /> parameter is larger than the size of the collection. </exception>
			// Token: 0x06005DF3 RID: 24051 RVA: 0x00185CD8 File Offset: 0x00183ED8
			public virtual int Add(int index, MenuItem item)
			{
				if (item.Menu != null)
				{
					if (this.owner is MenuItem)
					{
						for (MenuItem menuItem = (MenuItem)this.owner; menuItem != null; menuItem = (MenuItem)menuItem.Parent)
						{
							if (menuItem.Equals(item))
							{
								throw new ArgumentException(SR.GetString("MenuItemAlreadyExists", new object[]
								{
									item.Text
								}), "item");
							}
							if (!(menuItem.Parent is MenuItem))
							{
								break;
							}
						}
					}
					if (item.Menu.Equals(this.owner) && index > 0)
					{
						index--;
					}
					item.Menu.MenuItems.Remove(item);
				}
				if (index < 0 || index > this.owner.ItemCount)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.items == null || this.owner.items.Length == this.owner.ItemCount)
				{
					MenuItem[] array = new MenuItem[(this.owner.ItemCount < 2) ? 4 : (this.owner.ItemCount * 2)];
					if (this.owner.ItemCount > 0)
					{
						Array.Copy(this.owner.items, 0, array, 0, this.owner.ItemCount);
					}
					this.owner.items = array;
				}
				Array.Copy(this.owner.items, index, this.owner.items, index + 1, this.owner.ItemCount - index);
				this.owner.items[index] = item;
				this.owner._itemCount++;
				item.Menu = this.owner;
				this.owner.ItemsChanged(0);
				if (this.owner is MenuItem)
				{
					((MenuItem)this.owner).ItemsChanged(4, item);
				}
				return index;
			}

			/// <summary>Adds an array of previously created <see cref="T:System.Windows.Forms.MenuItem" /> objects to the collection.</summary>
			/// <param name="items">An array of <see cref="T:System.Windows.Forms.MenuItem" /> objects representing the menu items to add to the collection. </param>
			// Token: 0x06005DF4 RID: 24052 RVA: 0x00185ED0 File Offset: 0x001840D0
			public virtual void AddRange(MenuItem[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				foreach (MenuItem item in items)
				{
					this.Add(item);
				}
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.MenuItem" /> to add to the collection.</param>
			/// <returns>The position into which the <see cref="T:System.Windows.Forms.MenuItem" /> was inserted.</returns>
			// Token: 0x06005DF5 RID: 24053 RVA: 0x00185F07 File Offset: 0x00184107
			int IList.Add(object value)
			{
				if (value is MenuItem)
				{
					return this.Add((MenuItem)value);
				}
				throw new ArgumentException(SR.GetString("MenuBadMenuItem"), "value");
			}

			/// <summary>Determines if the specified <see cref="T:System.Windows.Forms.MenuItem" /> is a member of the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.MenuItem" /> to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.MenuItem" /> is a member of the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005DF6 RID: 24054 RVA: 0x00185F32 File Offset: 0x00184132
			public bool Contains(MenuItem value)
			{
				return this.IndexOf(value) != -1;
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
			/// <param name="value">The object to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the specified object is a <see cref="T:System.Windows.Forms.MenuItem" /> in the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005DF7 RID: 24055 RVA: 0x00185F41 File Offset: 0x00184141
			bool IList.Contains(object value)
			{
				return value is MenuItem && this.Contains((MenuItem)value);
			}

			/// <summary>Determines whether the collection contains an item with the specified key.</summary>
			/// <param name="key">The name of the item to look for.</param>
			/// <returns>
			///     <see langword="true" /> if the collection contains an item with the specified key, otherwise, <see langword="false" />. </returns>
			// Token: 0x06005DF8 RID: 24056 RVA: 0x00185F59 File Offset: 0x00184159
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Finds the items with the specified key, optionally searching the submenu items</summary>
			/// <param name="key">The name of the menu item to search for.</param>
			/// <param name="searchAllChildren">
			///       <see langword="true" /> to search child menu items; otherwise, <see langword="false" />. </param>
			/// <returns>An array of <see cref="T:System.Windows.Forms.MenuItem" /> objects whose <see cref="P:System.Windows.Forms.Menu.Name" /> property matches the specified <paramref name="key" />. </returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="key" /> is <see langword="null" /> or an empty string.</exception>
			// Token: 0x06005DF9 RID: 24057 RVA: 0x00185F68 File Offset: 0x00184168
			public MenuItem[] Find(string key, bool searchAllChildren)
			{
				if (key == null || key.Length == 0)
				{
					throw new ArgumentNullException("key", SR.GetString("FindKeyMayNotBeEmptyOrNull"));
				}
				ArrayList arrayList = this.FindInternal(key, searchAllChildren, this, new ArrayList());
				MenuItem[] array = new MenuItem[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}

			// Token: 0x06005DFA RID: 24058 RVA: 0x00185FBC File Offset: 0x001841BC
			private ArrayList FindInternal(string key, bool searchAllChildren, Menu.MenuItemCollection menuItemsToLookIn, ArrayList foundMenuItems)
			{
				if (menuItemsToLookIn == null || foundMenuItems == null)
				{
					return null;
				}
				for (int i = 0; i < menuItemsToLookIn.Count; i++)
				{
					if (menuItemsToLookIn[i] != null && WindowsFormsUtils.SafeCompareStrings(menuItemsToLookIn[i].Name, key, true))
					{
						foundMenuItems.Add(menuItemsToLookIn[i]);
					}
				}
				if (searchAllChildren)
				{
					for (int j = 0; j < menuItemsToLookIn.Count; j++)
					{
						if (menuItemsToLookIn[j] != null && menuItemsToLookIn[j].MenuItems != null && menuItemsToLookIn[j].MenuItems.Count > 0)
						{
							foundMenuItems = this.FindInternal(key, searchAllChildren, menuItemsToLookIn[j].MenuItems, foundMenuItems);
						}
					}
				}
				return foundMenuItems;
			}

			/// <summary>Retrieves the index of a specific item in the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.MenuItem" /> to locate in the collection. </param>
			/// <returns>The zero-based index of the item found in the collection; otherwise, -1.</returns>
			// Token: 0x06005DFB RID: 24059 RVA: 0x0018606C File Offset: 0x0018426C
			public int IndexOf(MenuItem value)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == value)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.MenuItem" /> to locate in the collection.</param>
			/// <returns>The zero-based index if <paramref name="value" /> is a <see cref="T:System.Windows.Forms.MenuItem" /> in the collection; otherwise -1.</returns>
			// Token: 0x06005DFC RID: 24060 RVA: 0x00186097 File Offset: 0x00184297
			int IList.IndexOf(object value)
			{
				if (value is MenuItem)
				{
					return this.IndexOf((MenuItem)value);
				}
				return -1;
			}

			/// <summary>Finds the index of the first occurrence of a menu item with the specified key.</summary>
			/// <param name="key">The name of the menu item to search for.</param>
			/// <returns>The zero-based index of the first menu item with the specified key.</returns>
			// Token: 0x06005DFD RID: 24061 RVA: 0x001860B0 File Offset: 0x001842B0
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

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
			/// <param name="index">The zero-based index at which the <see cref="T:System.Windows.Forms.MenuItem" /> should be inserted.</param>
			/// <param name="value">The <see cref="T:System.Windows.Forms.MenuItem" /> to insert into the <see cref="T:System.Windows.Forms.Menu.MenuItemCollection" />.</param>
			// Token: 0x06005DFE RID: 24062 RVA: 0x0018612D File Offset: 0x0018432D
			void IList.Insert(int index, object value)
			{
				if (value is MenuItem)
				{
					this.Add(index, (MenuItem)value);
					return;
				}
				throw new ArgumentException(SR.GetString("MenuBadMenuItem"), "value");
			}

			// Token: 0x06005DFF RID: 24063 RVA: 0x0018615A File Offset: 0x0018435A
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Removes all <see cref="T:System.Windows.Forms.MenuItem" /> objects from the menu item collection.</summary>
			// Token: 0x06005E00 RID: 24064 RVA: 0x0018616C File Offset: 0x0018436C
			public virtual void Clear()
			{
				if (this.owner.ItemCount > 0)
				{
					for (int i = 0; i < this.owner.ItemCount; i++)
					{
						this.owner.items[i].Menu = null;
					}
					this.owner._itemCount = 0;
					this.owner.items = null;
					this.owner.ItemsChanged(0);
					if (this.owner is MenuItem)
					{
						((MenuItem)this.owner).UpdateMenuItem(true);
					}
				}
			}

			/// <summary>Copies the entire collection into an existing array at a specified location within the array.</summary>
			/// <param name="dest">The destination array. </param>
			/// <param name="index">The index in the destination array at which storing begins. </param>
			// Token: 0x06005E01 RID: 24065 RVA: 0x001861F2 File Offset: 0x001843F2
			public void CopyTo(Array dest, int index)
			{
				if (this.owner.ItemCount > 0)
				{
					Array.Copy(this.owner.items, 0, dest, index, this.owner.ItemCount);
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the menu item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the menu item collection.</returns>
			// Token: 0x06005E02 RID: 24066 RVA: 0x00186220 File Offset: 0x00184420
			public IEnumerator GetEnumerator()
			{
				return new WindowsFormsUtils.ArraySubsetEnumerator(this.owner.items, this.owner.ItemCount);
			}

			/// <summary>Removes a <see cref="T:System.Windows.Forms.MenuItem" /> from the menu item collection at a specified index.</summary>
			/// <param name="index">The index of the <see cref="T:System.Windows.Forms.MenuItem" /> to remove. </param>
			// Token: 0x06005E03 RID: 24067 RVA: 0x00186240 File Offset: 0x00184440
			public virtual void RemoveAt(int index)
			{
				if (index < 0 || index >= this.owner.ItemCount)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				MenuItem menuItem = this.owner.items[index];
				menuItem.Menu = null;
				this.owner._itemCount--;
				Array.Copy(this.owner.items, index + 1, this.owner.items, index, this.owner.ItemCount - index);
				this.owner.items[this.owner.ItemCount] = null;
				this.owner.ItemsChanged(0);
				if (this.owner.ItemCount == 0)
				{
					this.Clear();
				}
			}

			/// <summary>Removes the menu item with the specified key from the collection.</summary>
			/// <param name="key">The name of the menu item to remove.</param>
			// Token: 0x06005E04 RID: 24068 RVA: 0x0018631C File Offset: 0x0018451C
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>Removes the specified <see cref="T:System.Windows.Forms.MenuItem" /> from the menu item collection.</summary>
			/// <param name="item">The <see cref="T:System.Windows.Forms.MenuItem" /> to remove. </param>
			// Token: 0x06005E05 RID: 24069 RVA: 0x00186341 File Offset: 0x00184541
			public virtual void Remove(MenuItem item)
			{
				if (item.Menu == this.owner)
				{
					this.RemoveAt(item.Index);
				}
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.MenuItem" /> to remove.</param>
			// Token: 0x06005E06 RID: 24070 RVA: 0x0018635D File Offset: 0x0018455D
			void IList.Remove(object value)
			{
				if (value is MenuItem)
				{
					this.Remove((MenuItem)value);
				}
			}

			// Token: 0x04003A17 RID: 14871
			private Menu owner;

			// Token: 0x04003A18 RID: 14872
			private int lastAccessedIndex = -1;
		}
	}
}
