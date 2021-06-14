using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Represents the navigation and manipulation user interface (UI) for controls on a form that are bound to data.</summary>
	// Token: 0x02000129 RID: 297
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("BindingSource")]
	[DefaultEvent("RefreshItems")]
	[Designer("System.Windows.Forms.Design.BindingNavigatorDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionBindingNavigator")]
	public class BindingNavigator : ToolStrip, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingNavigator" /> class.</summary>
		// Token: 0x0600080F RID: 2063 RVA: 0x00018294 File Offset: 0x00016494
		[EditorBrowsable(EditorBrowsableState.Never)]
		public BindingNavigator() : this(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingNavigator" /> class with the specified <see cref="T:System.Windows.Forms.BindingSource" /> as the data source.</summary>
		/// <param name="bindingSource">The <see cref="T:System.Windows.Forms.BindingSource" /> used as a data source.</param>
		// Token: 0x06000810 RID: 2064 RVA: 0x0001829D File Offset: 0x0001649D
		public BindingNavigator(BindingSource bindingSource) : this(true)
		{
			this.BindingSource = bindingSource;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingNavigator" /> class and adds this new instance to the specified container.</summary>
		/// <param name="container">The <see cref="T:System.ComponentModel.IContainer" /> to add the new <see cref="T:System.Windows.Forms.BindingNavigator" /> control to.</param>
		// Token: 0x06000811 RID: 2065 RVA: 0x000182AD File Offset: 0x000164AD
		[EditorBrowsable(EditorBrowsableState.Never)]
		public BindingNavigator(IContainer container) : this(false)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingNavigator" /> class, indicating whether to display the standard navigation user interface (UI).</summary>
		/// <param name="addStandardItems">
		///       <see langword="true" /> to show the standard navigational UI; otherwise, <see langword="false" />.</param>
		// Token: 0x06000812 RID: 2066 RVA: 0x000182CB File Offset: 0x000164CB
		public BindingNavigator(bool addStandardItems)
		{
			if (addStandardItems)
			{
				this.AddStandardItems();
			}
		}

		/// <summary>Disables updates to the <see cref="T:System.Windows.Forms.ToolStripItem" /> controls of the <see cref="T:System.Windows.Forms.BindingNavigator" /> during the component's initialization.</summary>
		// Token: 0x06000813 RID: 2067 RVA: 0x000182FA File Offset: 0x000164FA
		public void BeginInit()
		{
			this.initializing = true;
		}

		/// <summary>Enables updates to the <see cref="T:System.Windows.Forms.ToolStripItem" /> controls of the <see cref="T:System.Windows.Forms.BindingNavigator" /> after the component's initialization has concluded.</summary>
		// Token: 0x06000814 RID: 2068 RVA: 0x00018303 File Offset: 0x00016503
		public void EndInit()
		{
			this.initializing = false;
			this.RefreshItemsInternal();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.BindingNavigator" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06000815 RID: 2069 RVA: 0x00018312 File Offset: 0x00016512
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.BindingSource = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>Adds the standard set of navigation items to the <see cref="T:System.Windows.Forms.BindingNavigator" /> control.</summary>
		// Token: 0x06000816 RID: 2070 RVA: 0x00018328 File Offset: 0x00016528
		public virtual void AddStandardItems()
		{
			this.MoveFirstItem = new ToolStripButton();
			this.MovePreviousItem = new ToolStripButton();
			this.MoveNextItem = new ToolStripButton();
			this.MoveLastItem = new ToolStripButton();
			this.PositionItem = new ToolStripTextBox();
			this.CountItem = new ToolStripLabel();
			this.AddNewItem = new ToolStripButton();
			this.DeleteItem = new ToolStripButton();
			ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
			ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
			ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
			char c = (string.IsNullOrEmpty(base.Name) || char.IsLower(base.Name[0])) ? 'b' : 'B';
			this.MoveFirstItem.Name = c.ToString() + "indingNavigatorMoveFirstItem";
			this.MovePreviousItem.Name = c.ToString() + "indingNavigatorMovePreviousItem";
			this.MoveNextItem.Name = c.ToString() + "indingNavigatorMoveNextItem";
			this.MoveLastItem.Name = c.ToString() + "indingNavigatorMoveLastItem";
			this.PositionItem.Name = c.ToString() + "indingNavigatorPositionItem";
			this.CountItem.Name = c.ToString() + "indingNavigatorCountItem";
			this.AddNewItem.Name = c.ToString() + "indingNavigatorAddNewItem";
			this.DeleteItem.Name = c.ToString() + "indingNavigatorDeleteItem";
			toolStripSeparator.Name = c.ToString() + "indingNavigatorSeparator";
			toolStripSeparator2.Name = c.ToString() + "indingNavigatorSeparator";
			toolStripSeparator3.Name = c.ToString() + "indingNavigatorSeparator";
			this.MoveFirstItem.Text = SR.GetString("BindingNavigatorMoveFirstItemText");
			this.MovePreviousItem.Text = SR.GetString("BindingNavigatorMovePreviousItemText");
			this.MoveNextItem.Text = SR.GetString("BindingNavigatorMoveNextItemText");
			this.MoveLastItem.Text = SR.GetString("BindingNavigatorMoveLastItemText");
			this.AddNewItem.Text = SR.GetString("BindingNavigatorAddNewItemText");
			this.DeleteItem.Text = SR.GetString("BindingNavigatorDeleteItemText");
			this.CountItem.ToolTipText = SR.GetString("BindingNavigatorCountItemTip");
			this.PositionItem.ToolTipText = SR.GetString("BindingNavigatorPositionItemTip");
			this.CountItem.AutoToolTip = false;
			this.PositionItem.AutoToolTip = false;
			this.PositionItem.AccessibleName = SR.GetString("BindingNavigatorPositionAccessibleName");
			Bitmap bitmap = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveFirst.bmp");
			Bitmap bitmap2 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MovePrevious.bmp");
			Bitmap bitmap3 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveNext.bmp");
			Bitmap bitmap4 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveLast.bmp");
			Bitmap bitmap5 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.AddNew.bmp");
			Bitmap bitmap6 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.Delete.bmp");
			bitmap.MakeTransparent(Color.Magenta);
			bitmap2.MakeTransparent(Color.Magenta);
			bitmap3.MakeTransparent(Color.Magenta);
			bitmap4.MakeTransparent(Color.Magenta);
			bitmap5.MakeTransparent(Color.Magenta);
			bitmap6.MakeTransparent(Color.Magenta);
			this.MoveFirstItem.Image = bitmap;
			this.MovePreviousItem.Image = bitmap2;
			this.MoveNextItem.Image = bitmap3;
			this.MoveLastItem.Image = bitmap4;
			this.AddNewItem.Image = bitmap5;
			this.DeleteItem.Image = bitmap6;
			this.MoveFirstItem.RightToLeftAutoMirrorImage = true;
			this.MovePreviousItem.RightToLeftAutoMirrorImage = true;
			this.MoveNextItem.RightToLeftAutoMirrorImage = true;
			this.MoveLastItem.RightToLeftAutoMirrorImage = true;
			this.AddNewItem.RightToLeftAutoMirrorImage = true;
			this.DeleteItem.RightToLeftAutoMirrorImage = true;
			this.MoveFirstItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.MovePreviousItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.MoveNextItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.MoveLastItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.AddNewItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.DeleteItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.PositionItem.AutoSize = false;
			this.PositionItem.Width = 50;
			this.Items.AddRange(new ToolStripItem[]
			{
				this.MoveFirstItem,
				this.MovePreviousItem,
				toolStripSeparator,
				this.PositionItem,
				this.CountItem,
				toolStripSeparator2,
				this.MoveNextItem,
				this.MoveLastItem,
				toolStripSeparator3,
				this.AddNewItem,
				this.DeleteItem
			});
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.BindingSource" /> component that is the source of data.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.BindingSource" /> component associated with this <see cref="T:System.Windows.Forms.BindingNavigator" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x000187EB File Offset: 0x000169EB
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x000187F3 File Offset: 0x000169F3
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[SRDescription("BindingNavigatorBindingSourcePropDescr")]
		[TypeConverter(typeof(ReferenceConverter))]
		public BindingSource BindingSource
		{
			get
			{
				return this.bindingSource;
			}
			set
			{
				this.WireUpBindingSource(ref this.bindingSource, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that is associated with the Move First functionality.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> that represents the Move First button for the <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00018802 File Offset: 0x00016A02
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x00018826 File Offset: 0x00016A26
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorMoveFirstItemPropDescr")]
		public ToolStripItem MoveFirstItem
		{
			get
			{
				if (this.moveFirstItem != null && this.moveFirstItem.IsDisposed)
				{
					this.moveFirstItem = null;
				}
				return this.moveFirstItem;
			}
			set
			{
				this.WireUpButton(ref this.moveFirstItem, value, new EventHandler(this.OnMoveFirst));
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that is associated with the Move Previous functionality.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> that represents the Move Previous button for the <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x00018841 File Offset: 0x00016A41
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x00018865 File Offset: 0x00016A65
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorMovePreviousItemPropDescr")]
		public ToolStripItem MovePreviousItem
		{
			get
			{
				if (this.movePreviousItem != null && this.movePreviousItem.IsDisposed)
				{
					this.movePreviousItem = null;
				}
				return this.movePreviousItem;
			}
			set
			{
				this.WireUpButton(ref this.movePreviousItem, value, new EventHandler(this.OnMovePrevious));
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that is associated with the Move Next functionality.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> that represents the Move Next button for the <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x00018880 File Offset: 0x00016A80
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x000188A4 File Offset: 0x00016AA4
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorMoveNextItemPropDescr")]
		public ToolStripItem MoveNextItem
		{
			get
			{
				if (this.moveNextItem != null && this.moveNextItem.IsDisposed)
				{
					this.moveNextItem = null;
				}
				return this.moveNextItem;
			}
			set
			{
				this.WireUpButton(ref this.moveNextItem, value, new EventHandler(this.OnMoveNext));
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that is associated with the Move Last functionality.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> that represents the Move Last button for the <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x000188BF File Offset: 0x00016ABF
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x000188E3 File Offset: 0x00016AE3
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorMoveLastItemPropDescr")]
		public ToolStripItem MoveLastItem
		{
			get
			{
				if (this.moveLastItem != null && this.moveLastItem.IsDisposed)
				{
					this.moveLastItem = null;
				}
				return this.moveLastItem;
			}
			set
			{
				this.WireUpButton(ref this.moveLastItem, value, new EventHandler(this.OnMoveLast));
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that represents the Add New button.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> that represents the Add New button for the <see cref="T:System.Windows.Forms.BindingSource" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x000188FE File Offset: 0x00016AFE
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x00018924 File Offset: 0x00016B24
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorAddNewItemPropDescr")]
		public ToolStripItem AddNewItem
		{
			get
			{
				if (this.addNewItem != null && this.addNewItem.IsDisposed)
				{
					this.addNewItem = null;
				}
				return this.addNewItem;
			}
			set
			{
				if (this.addNewItem != value && value != null)
				{
					value.InternalEnabledChanged += this.OnAddNewItemEnabledChanged;
					this.addNewItemUserEnabled = value.Enabled;
				}
				this.WireUpButton(ref this.addNewItem, value, new EventHandler(this.OnAddNew));
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that is associated with the Delete functionality.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> that represents the Delete button for the <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00018974 File Offset: 0x00016B74
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x00018998 File Offset: 0x00016B98
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorDeleteItemPropDescr")]
		public ToolStripItem DeleteItem
		{
			get
			{
				if (this.deleteItem != null && this.deleteItem.IsDisposed)
				{
					this.deleteItem = null;
				}
				return this.deleteItem;
			}
			set
			{
				if (this.deleteItem != value && value != null)
				{
					value.InternalEnabledChanged += this.OnDeleteItemEnabledChanged;
					this.deleteItemUserEnabled = value.Enabled;
				}
				this.WireUpButton(ref this.deleteItem, value, new EventHandler(this.OnDelete));
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that displays the current position within the <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripItem" /> that displays the current position.</returns>
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x000189E8 File Offset: 0x00016BE8
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x00018A0C File Offset: 0x00016C0C
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorPositionItemPropDescr")]
		public ToolStripItem PositionItem
		{
			get
			{
				if (this.positionItem != null && this.positionItem.IsDisposed)
				{
					this.positionItem = null;
				}
				return this.positionItem;
			}
			set
			{
				this.WireUpTextBox(ref this.positionItem, value, new KeyEventHandler(this.OnPositionKey), new EventHandler(this.OnPositionLostFocus));
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolStripItem" /> that displays the total number of items in the associated <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> that displays the total number of items in the associated <see cref="T:System.Windows.Forms.BindingSource" />. </returns>
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x00018A33 File Offset: 0x00016C33
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x00018A57 File Offset: 0x00016C57
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorCountItemPropDescr")]
		public ToolStripItem CountItem
		{
			get
			{
				if (this.countItem != null && this.countItem.IsDisposed)
				{
					this.countItem = null;
				}
				return this.countItem;
			}
			set
			{
				this.WireUpLabel(ref this.countItem, value);
			}
		}

		/// <summary>Gets or sets a string used to format the information displayed in the <see cref="P:System.Windows.Forms.BindingNavigator.CountItem" /> control. </summary>
		/// <returns>The format <see cref="T:System.String" /> used to format the item count. The default is the string "of {0}".</returns>
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00018A66 File Offset: 0x00016C66
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x00018A6E File Offset: 0x00016C6E
		[SRCategory("CatAppearance")]
		[SRDescription("BindingNavigatorCountItemFormatPropDescr")]
		public string CountItemFormat
		{
			get
			{
				return this.countItemFormat;
			}
			set
			{
				if (this.countItemFormat != value)
				{
					this.countItemFormat = value;
					this.RefreshItemsInternal();
				}
			}
		}

		/// <summary>Occurs when the state of the navigational user interface (UI) needs to be refreshed to reflect the current state of the underlying data.</summary>
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x0600082B RID: 2091 RVA: 0x00018A8B File Offset: 0x00016C8B
		// (remove) Token: 0x0600082C RID: 2092 RVA: 0x00018AA4 File Offset: 0x00016CA4
		[SRCategory("CatBehavior")]
		[SRDescription("BindingNavigatorRefreshItemsEventDescr")]
		public event EventHandler RefreshItems
		{
			add
			{
				this.onRefreshItems = (EventHandler)Delegate.Combine(this.onRefreshItems, value);
			}
			remove
			{
				this.onRefreshItems = (EventHandler)Delegate.Remove(this.onRefreshItems, value);
			}
		}

		/// <summary>Refreshes the state of the standard items to reflect the current state of the data.</summary>
		// Token: 0x0600082D RID: 2093 RVA: 0x00018AC0 File Offset: 0x00016CC0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void RefreshItemsCore()
		{
			int num;
			int num2;
			bool flag;
			bool flag2;
			if (this.bindingSource == null)
			{
				num = 0;
				num2 = 0;
				flag = false;
				flag2 = false;
			}
			else
			{
				num = this.bindingSource.Count;
				num2 = this.bindingSource.Position + 1;
				flag = ((IBindingList)this.bindingSource).AllowNew;
				flag2 = ((IBindingList)this.bindingSource).AllowRemove;
			}
			if (!base.DesignMode)
			{
				if (this.MoveFirstItem != null)
				{
					this.moveFirstItem.Enabled = (num2 > 1);
				}
				if (this.MovePreviousItem != null)
				{
					this.movePreviousItem.Enabled = (num2 > 1);
				}
				if (this.MoveNextItem != null)
				{
					this.moveNextItem.Enabled = (num2 < num);
				}
				if (this.MoveLastItem != null)
				{
					this.moveLastItem.Enabled = (num2 < num);
				}
				if (this.AddNewItem != null)
				{
					EventHandler value = new EventHandler(this.OnAddNewItemEnabledChanged);
					this.addNewItem.InternalEnabledChanged -= value;
					this.addNewItem.Enabled = (this.addNewItemUserEnabled && flag);
					this.addNewItem.InternalEnabledChanged += value;
				}
				if (this.DeleteItem != null)
				{
					EventHandler value2 = new EventHandler(this.OnDeleteItemEnabledChanged);
					this.deleteItem.InternalEnabledChanged -= value2;
					this.deleteItem.Enabled = (this.deleteItemUserEnabled && flag2 && num > 0);
					this.deleteItem.InternalEnabledChanged += value2;
				}
				if (this.PositionItem != null)
				{
					this.positionItem.Enabled = (num2 > 0 && num > 0);
				}
				if (this.CountItem != null)
				{
					this.countItem.Enabled = (num > 0);
				}
			}
			if (this.positionItem != null)
			{
				this.positionItem.Text = num2.ToString(CultureInfo.CurrentCulture);
			}
			if (this.countItem != null)
			{
				this.countItem.Text = (base.DesignMode ? this.CountItemFormat : string.Format(CultureInfo.CurrentCulture, this.CountItemFormat, new object[]
				{
					num
				}));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingNavigator.RefreshItems" /> event.</summary>
		// Token: 0x0600082E RID: 2094 RVA: 0x00018C9D File Offset: 0x00016E9D
		protected virtual void OnRefreshItems()
		{
			this.RefreshItemsCore();
			if (this.onRefreshItems != null)
			{
				this.onRefreshItems(this, EventArgs.Empty);
			}
		}

		/// <summary>Causes form validation to occur and returns whether validation was successful.</summary>
		/// <returns>
		///     <see langword="true" /> if validation was successful and focus can shift to the <see cref="T:System.Windows.Forms.BindingNavigator" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600082F RID: 2095 RVA: 0x00018CC0 File Offset: 0x00016EC0
		public bool Validate()
		{
			bool flag;
			return base.ValidateActiveControl(out flag);
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00018CD8 File Offset: 0x00016ED8
		private void AcceptNewPosition()
		{
			if (this.positionItem == null || this.bindingSource == null)
			{
				return;
			}
			int num = this.bindingSource.Position;
			try
			{
				num = Convert.ToInt32(this.positionItem.Text, CultureInfo.CurrentCulture) - 1;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			if (num != this.bindingSource.Position)
			{
				this.bindingSource.Position = num;
			}
			this.RefreshItemsInternal();
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00018D60 File Offset: 0x00016F60
		private void CancelNewPosition()
		{
			this.RefreshItemsInternal();
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00018D68 File Offset: 0x00016F68
		private void OnMoveFirst(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.MoveFirst();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00018D8B File Offset: 0x00016F8B
		private void OnMovePrevious(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.MovePrevious();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00018DAE File Offset: 0x00016FAE
		private void OnMoveNext(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.MoveNext();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00018DD1 File Offset: 0x00016FD1
		private void OnMoveLast(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.MoveLast();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00018DF4 File Offset: 0x00016FF4
		private void OnAddNew(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.AddNew();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00018E18 File Offset: 0x00017018
		private void OnDelete(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.RemoveCurrent();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00018E3C File Offset: 0x0001703C
		private void OnPositionKey(object sender, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode == Keys.Return)
			{
				this.AcceptNewPosition();
				return;
			}
			if (keyCode != Keys.Escape)
			{
				return;
			}
			this.CancelNewPosition();
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00018E68 File Offset: 0x00017068
		private void OnPositionLostFocus(object sender, EventArgs e)
		{
			this.AcceptNewPosition();
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00018D60 File Offset: 0x00016F60
		private void OnBindingSourceStateChanged(object sender, EventArgs e)
		{
			this.RefreshItemsInternal();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00018D60 File Offset: 0x00016F60
		private void OnBindingSourceListChanged(object sender, ListChangedEventArgs e)
		{
			this.RefreshItemsInternal();
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00018E70 File Offset: 0x00017070
		private void RefreshItemsInternal()
		{
			if (this.initializing)
			{
				return;
			}
			this.OnRefreshItems();
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00018E81 File Offset: 0x00017081
		private void ResetCountItemFormat()
		{
			this.countItemFormat = SR.GetString("BindingNavigatorCountItemFormat");
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00018E93 File Offset: 0x00017093
		private bool ShouldSerializeCountItemFormat()
		{
			return this.countItemFormat != SR.GetString("BindingNavigatorCountItemFormat");
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00018EAA File Offset: 0x000170AA
		private void OnAddNewItemEnabledChanged(object sender, EventArgs e)
		{
			if (this.AddNewItem != null)
			{
				this.addNewItemUserEnabled = this.addNewItem.Enabled;
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00018EC5 File Offset: 0x000170C5
		private void OnDeleteItemEnabledChanged(object sender, EventArgs e)
		{
			if (this.DeleteItem != null)
			{
				this.deleteItemUserEnabled = this.deleteItem.Enabled;
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00018EE0 File Offset: 0x000170E0
		private void WireUpButton(ref ToolStripItem oldButton, ToolStripItem newButton, EventHandler clickHandler)
		{
			if (oldButton == newButton)
			{
				return;
			}
			if (oldButton != null)
			{
				oldButton.Click -= clickHandler;
			}
			if (newButton != null)
			{
				newButton.Click += clickHandler;
			}
			oldButton = newButton;
			this.RefreshItemsInternal();
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00018F08 File Offset: 0x00017108
		private void WireUpTextBox(ref ToolStripItem oldTextBox, ToolStripItem newTextBox, KeyEventHandler keyUpHandler, EventHandler lostFocusHandler)
		{
			if (oldTextBox != newTextBox)
			{
				ToolStripControlHost toolStripControlHost = oldTextBox as ToolStripControlHost;
				ToolStripControlHost toolStripControlHost2 = newTextBox as ToolStripControlHost;
				if (toolStripControlHost != null)
				{
					toolStripControlHost.KeyUp -= keyUpHandler;
					toolStripControlHost.LostFocus -= lostFocusHandler;
				}
				if (toolStripControlHost2 != null)
				{
					toolStripControlHost2.KeyUp += keyUpHandler;
					toolStripControlHost2.LostFocus += lostFocusHandler;
				}
				oldTextBox = newTextBox;
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00018F56 File Offset: 0x00017156
		private void WireUpLabel(ref ToolStripItem oldLabel, ToolStripItem newLabel)
		{
			if (oldLabel != newLabel)
			{
				oldLabel = newLabel;
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00018F68 File Offset: 0x00017168
		private void WireUpBindingSource(ref BindingSource oldBindingSource, BindingSource newBindingSource)
		{
			if (oldBindingSource != newBindingSource)
			{
				if (oldBindingSource != null)
				{
					oldBindingSource.PositionChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.CurrentChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.CurrentItemChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.DataSourceChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.DataMemberChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.ListChanged -= this.OnBindingSourceListChanged;
				}
				if (newBindingSource != null)
				{
					newBindingSource.PositionChanged += this.OnBindingSourceStateChanged;
					newBindingSource.CurrentChanged += this.OnBindingSourceStateChanged;
					newBindingSource.CurrentItemChanged += this.OnBindingSourceStateChanged;
					newBindingSource.DataSourceChanged += this.OnBindingSourceStateChanged;
					newBindingSource.DataMemberChanged += this.OnBindingSourceStateChanged;
					newBindingSource.ListChanged += this.OnBindingSourceListChanged;
				}
				oldBindingSource = newBindingSource;
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x04000612 RID: 1554
		private BindingSource bindingSource;

		// Token: 0x04000613 RID: 1555
		private ToolStripItem moveFirstItem;

		// Token: 0x04000614 RID: 1556
		private ToolStripItem movePreviousItem;

		// Token: 0x04000615 RID: 1557
		private ToolStripItem moveNextItem;

		// Token: 0x04000616 RID: 1558
		private ToolStripItem moveLastItem;

		// Token: 0x04000617 RID: 1559
		private ToolStripItem addNewItem;

		// Token: 0x04000618 RID: 1560
		private ToolStripItem deleteItem;

		// Token: 0x04000619 RID: 1561
		private ToolStripItem positionItem;

		// Token: 0x0400061A RID: 1562
		private ToolStripItem countItem;

		// Token: 0x0400061B RID: 1563
		private string countItemFormat = SR.GetString("BindingNavigatorCountItemFormat");

		// Token: 0x0400061C RID: 1564
		private EventHandler onRefreshItems;

		// Token: 0x0400061D RID: 1565
		private bool initializing;

		// Token: 0x0400061E RID: 1566
		private bool addNewItemUserEnabled = true;

		// Token: 0x0400061F RID: 1567
		private bool deleteItemUserEnabled = true;
	}
}
