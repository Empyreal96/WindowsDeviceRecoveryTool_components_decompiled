using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides a common implementation of members for the <see cref="T:System.Windows.Forms.ListBox" /> and <see cref="T:System.Windows.Forms.ComboBox" /> classes.</summary>
	// Token: 0x020002BD RID: 701
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]
	public abstract class ListControl : Control
	{
		/// <summary>Gets or sets the data source for this <see cref="T:System.Windows.Forms.ListControl" />.</summary>
		/// <returns>An object that implements the <see cref="T:System.Collections.IList" /> or <see cref="T:System.ComponentModel.IListSource" /> interfaces, such as a <see cref="T:System.Data.DataSet" /> or an <see cref="T:System.Array" />. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">The assigned value does not implement the <see cref="T:System.Collections.IList" /> or <see cref="T:System.ComponentModel.IListSource" /> interfaces.</exception>
		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002915 RID: 10517 RVA: 0x000BF6A8 File Offset: 0x000BD8A8
		// (set) Token: 0x06002916 RID: 10518 RVA: 0x000BF6B0 File Offset: 0x000BD8B0
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		[SRDescription("ListControlDataSourceDescr")]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (value != null && !(value is IList) && !(value is IListSource))
				{
					throw new ArgumentException(SR.GetString("BadDataSourceForComplexBinding"));
				}
				if (this.dataSource == value)
				{
					return;
				}
				try
				{
					this.SetDataConnection(value, this.displayMember, false);
				}
				catch
				{
					this.DisplayMember = "";
				}
				if (value == null)
				{
					this.DisplayMember = "";
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListControl.DataSource" /> changes.</summary>
		// Token: 0x140001E5 RID: 485
		// (add) Token: 0x06002917 RID: 10519 RVA: 0x000BF728 File Offset: 0x000BD928
		// (remove) Token: 0x06002918 RID: 10520 RVA: 0x000BF73B File Offset: 0x000BD93B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlOnDataSourceChangedDescr")]
		public event EventHandler DataSourceChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_DATASOURCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_DATASOURCECHANGED, value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.CurrencyManager" /> associated with this control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.CurrencyManager" /> associated with this control. The default is <see langword="null" />.</returns>
		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06002919 RID: 10521 RVA: 0x000BF74E File Offset: 0x000BD94E
		protected CurrencyManager DataManager
		{
			get
			{
				return this.dataManager;
			}
		}

		/// <summary>Gets or sets the property to display for this <see cref="T:System.Windows.Forms.ListControl" />.</summary>
		/// <returns>A <see cref="T:System.String" /> specifying the name of an object property that is contained in the collection specified by the <see cref="P:System.Windows.Forms.ListControl.DataSource" /> property. The default is an empty string (""). </returns>
		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600291A RID: 10522 RVA: 0x000BF756 File Offset: 0x000BD956
		// (set) Token: 0x0600291B RID: 10523 RVA: 0x000BF764 File Offset: 0x000BD964
		[SRCategory("CatData")]
		[DefaultValue("")]
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListControlDisplayMemberDescr")]
		public string DisplayMember
		{
			get
			{
				return this.displayMember.BindingMember;
			}
			set
			{
				BindingMemberInfo bindingMemberInfo = this.displayMember;
				try
				{
					this.SetDataConnection(this.dataSource, new BindingMemberInfo(value), false);
				}
				catch
				{
					this.displayMember = bindingMemberInfo;
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListControl.DisplayMember" /> property changes.</summary>
		// Token: 0x140001E6 RID: 486
		// (add) Token: 0x0600291C RID: 10524 RVA: 0x000BF7A8 File Offset: 0x000BD9A8
		// (remove) Token: 0x0600291D RID: 10525 RVA: 0x000BF7BB File Offset: 0x000BD9BB
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlOnDisplayMemberChangedDescr")]
		public event EventHandler DisplayMemberChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_DISPLAYMEMBERCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_DISPLAYMEMBERCHANGED, value);
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x000BF7D0 File Offset: 0x000BD9D0
		private TypeConverter DisplayMemberConverter
		{
			get
			{
				if (this.displayMemberConverter == null && this.DataManager != null)
				{
					BindingMemberInfo bindingMemberInfo = this.displayMember;
					PropertyDescriptorCollection itemProperties = this.DataManager.GetItemProperties();
					if (itemProperties != null)
					{
						PropertyDescriptor propertyDescriptor = itemProperties.Find(this.displayMember.BindingField, true);
						if (propertyDescriptor != null)
						{
							this.displayMemberConverter = propertyDescriptor.Converter;
						}
					}
				}
				return this.displayMemberConverter;
			}
		}

		/// <summary>Occurs when the control is bound to a data value.</summary>
		// Token: 0x140001E7 RID: 487
		// (add) Token: 0x0600291F RID: 10527 RVA: 0x000BF82B File Offset: 0x000BDA2B
		// (remove) Token: 0x06002920 RID: 10528 RVA: 0x000BF844 File Offset: 0x000BDA44
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlFormatDescr")]
		public event ListControlConvertEventHandler Format
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_FORMAT, value);
				this.RefreshItems();
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_FORMAT, value);
				this.RefreshItems();
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.IFormatProvider" /> that provides custom formatting behavior. </summary>
		/// <returns>The <see cref="T:System.IFormatProvider" /> implementation that provides custom formatting behavior.</returns>
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x000BF85D File Offset: 0x000BDA5D
		// (set) Token: 0x06002922 RID: 10530 RVA: 0x000BF865 File Offset: 0x000BDA65
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(null)]
		public IFormatProvider FormatInfo
		{
			get
			{
				return this.formatInfo;
			}
			set
			{
				if (value != this.formatInfo)
				{
					this.formatInfo = value;
					this.RefreshItems();
					this.OnFormatInfoChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ListControl.FormatInfo" /> property changes.</summary>
		// Token: 0x140001E8 RID: 488
		// (add) Token: 0x06002923 RID: 10531 RVA: 0x000BF888 File Offset: 0x000BDA88
		// (remove) Token: 0x06002924 RID: 10532 RVA: 0x000BF89B File Offset: 0x000BDA9B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlFormatInfoChangedDescr")]
		public event EventHandler FormatInfoChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_FORMATINFOCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_FORMATINFOCHANGED, value);
			}
		}

		/// <summary>Gets or sets the format-specifier characters that indicate how a value is to be displayed.</summary>
		/// <returns>The string of format-specifier characters that indicates how a value is to be displayed.</returns>
		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x000BF8AE File Offset: 0x000BDAAE
		// (set) Token: 0x06002926 RID: 10534 RVA: 0x000BF8B6 File Offset: 0x000BDAB6
		[DefaultValue("")]
		[SRDescription("ListControlFormatStringDescr")]
		[Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		public string FormatString
		{
			get
			{
				return this.formatString;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (!value.Equals(this.formatString))
				{
					this.formatString = value;
					this.RefreshItems();
					this.OnFormatStringChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when value of the <see cref="P:System.Windows.Forms.ListControl.FormatString" /> property changes</summary>
		// Token: 0x140001E9 RID: 489
		// (add) Token: 0x06002927 RID: 10535 RVA: 0x000BF8E8 File Offset: 0x000BDAE8
		// (remove) Token: 0x06002928 RID: 10536 RVA: 0x000BF8FB File Offset: 0x000BDAFB
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlFormatStringChangedDescr")]
		public event EventHandler FormatStringChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_FORMATSTRINGCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_FORMATSTRINGCHANGED, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether formatting is applied to the <see cref="P:System.Windows.Forms.ListControl.DisplayMember" /> property of the <see cref="T:System.Windows.Forms.ListControl" />.</summary>
		/// <returns>
		///     <see langword="true" /> if formatting of the <see cref="P:System.Windows.Forms.ListControl.DisplayMember" /> property is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06002929 RID: 10537 RVA: 0x000BF90E File Offset: 0x000BDB0E
		// (set) Token: 0x0600292A RID: 10538 RVA: 0x000BF916 File Offset: 0x000BDB16
		[DefaultValue(false)]
		[SRDescription("ListControlFormattingEnabledDescr")]
		public bool FormattingEnabled
		{
			get
			{
				return this.formattingEnabled;
			}
			set
			{
				if (value != this.formattingEnabled)
				{
					this.formattingEnabled = value;
					this.RefreshItems();
					this.OnFormattingEnabledChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ListControl.FormattingEnabled" /> property changes.</summary>
		// Token: 0x140001EA RID: 490
		// (add) Token: 0x0600292B RID: 10539 RVA: 0x000BF939 File Offset: 0x000BDB39
		// (remove) Token: 0x0600292C RID: 10540 RVA: 0x000BF94C File Offset: 0x000BDB4C
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlFormattingEnabledChangedDescr")]
		public event EventHandler FormattingEnabledChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_FORMATTINGENABLEDCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_FORMATTINGENABLEDCHANGED, value);
			}
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000BF960 File Offset: 0x000BDB60
		private bool BindingMemberInfoInDataManager(BindingMemberInfo bindingMemberInfo)
		{
			if (this.dataManager == null)
			{
				return false;
			}
			PropertyDescriptorCollection itemProperties = this.dataManager.GetItemProperties();
			int count = itemProperties.Count;
			for (int i = 0; i < count; i++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(bindingMemberInfo.BindingField))
				{
					return true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && string.Compare(itemProperties[j].Name, bindingMemberInfo.BindingField, true, CultureInfo.CurrentCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Gets or sets the path of the property to use as the actual value for the items in the <see cref="T:System.Windows.Forms.ListControl" />.</summary>
		/// <returns>A <see cref="T:System.String" /> representing a single property name of the <see cref="P:System.Windows.Forms.ListControl.DataSource" /> property value, or a hierarchy of period-delimited property names that resolves to a property name of the final data-bound object. The default is an empty string ("").</returns>
		/// <exception cref="T:System.ArgumentException">The specified property path cannot be resolved through the object specified by the <see cref="P:System.Windows.Forms.ListControl.DataSource" /> property. </exception>
		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x000BFA1B File Offset: 0x000BDC1B
		// (set) Token: 0x0600292F RID: 10543 RVA: 0x000BFA28 File Offset: 0x000BDC28
		[SRCategory("CatData")]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListControlValueMemberDescr")]
		public string ValueMember
		{
			get
			{
				return this.valueMember.BindingMember;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(value);
				BindingMemberInfo bindingMemberInfo2 = this.valueMember;
				if (!bindingMemberInfo.Equals(this.valueMember))
				{
					if (this.DisplayMember.Length == 0)
					{
						this.SetDataConnection(this.DataSource, bindingMemberInfo, false);
					}
					if (this.dataManager != null && value != null && value.Length != 0 && !this.BindingMemberInfoInDataManager(bindingMemberInfo))
					{
						throw new ArgumentException(SR.GetString("ListControlWrongValueMember"), "value");
					}
					this.valueMember = bindingMemberInfo;
					this.OnValueMemberChanged(EventArgs.Empty);
					this.OnSelectedValueChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListControl.ValueMember" /> property changes.</summary>
		// Token: 0x140001EB RID: 491
		// (add) Token: 0x06002930 RID: 10544 RVA: 0x000BFAD1 File Offset: 0x000BDCD1
		// (remove) Token: 0x06002931 RID: 10545 RVA: 0x000BFAE4 File Offset: 0x000BDCE4
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlOnValueMemberChangedDescr")]
		public event EventHandler ValueMemberChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_VALUEMEMBERCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_VALUEMEMBERCHANGED, value);
			}
		}

		/// <summary>Gets a value indicating whether the list enables selection of list items.</summary>
		/// <returns>
		///     <see langword="true" /> if the list enables list item selection; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x0000E214 File Offset: 0x0000C414
		protected virtual bool AllowSelection
		{
			get
			{
				return true;
			}
		}

		/// <summary>When overridden in a derived class, gets or sets the zero-based index of the currently selected item.</summary>
		/// <returns>A zero-based index of the currently selected item. A value of negative one (-1) is returned if no item is selected.</returns>
		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06002933 RID: 10547
		// (set) Token: 0x06002934 RID: 10548
		public abstract int SelectedIndex { get; set; }

		/// <summary>Gets or sets the value of the member property specified by the <see cref="P:System.Windows.Forms.ListControl.ValueMember" /> property.</summary>
		/// <returns>An object containing the value of the member of the data source specified by the <see cref="P:System.Windows.Forms.ListControl.ValueMember" /> property.</returns>
		/// <exception cref="T:System.InvalidOperationException">The assigned value is <see langword="null" /> or the empty string ("").</exception>
		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06002935 RID: 10549 RVA: 0x000BFAF8 File Offset: 0x000BDCF8
		// (set) Token: 0x06002936 RID: 10550 RVA: 0x000BFB40 File Offset: 0x000BDD40
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListControlSelectedValueDescr")]
		[Bindable(true)]
		public object SelectedValue
		{
			get
			{
				if (this.SelectedIndex != -1 && this.dataManager != null)
				{
					object item = this.dataManager[this.SelectedIndex];
					return this.FilterItemOnProperty(item, this.valueMember.BindingField);
				}
				return null;
			}
			set
			{
				if (this.dataManager != null)
				{
					string bindingField = this.valueMember.BindingField;
					if (string.IsNullOrEmpty(bindingField))
					{
						throw new InvalidOperationException(SR.GetString("ListControlEmptyValueMemberInSettingSelectedValue"));
					}
					PropertyDescriptorCollection itemProperties = this.dataManager.GetItemProperties();
					PropertyDescriptor property = itemProperties.Find(bindingField, true);
					int selectedIndex = this.dataManager.Find(property, value, true);
					this.SelectedIndex = selectedIndex;
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ListControl.SelectedValue" /> property changes.</summary>
		// Token: 0x140001EC RID: 492
		// (add) Token: 0x06002937 RID: 10551 RVA: 0x000BFBA4 File Offset: 0x000BDDA4
		// (remove) Token: 0x06002938 RID: 10552 RVA: 0x000BFBB7 File Offset: 0x000BDDB7
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlOnSelectedValueChangedDescr")]
		public event EventHandler SelectedValueChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_SELECTEDVALUECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_SELECTEDVALUECHANGED, value);
			}
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000BFBCA File Offset: 0x000BDDCA
		private void DataManager_PositionChanged(object sender, EventArgs e)
		{
			if (this.dataManager != null && this.AllowSelection)
			{
				this.SelectedIndex = this.dataManager.Position;
			}
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000BFBF0 File Offset: 0x000BDDF0
		private void DataManager_ItemChanged(object sender, ItemChangedEventArgs e)
		{
			if (this.dataManager != null)
			{
				if (e.Index == -1)
				{
					this.SetItemsCore(this.dataManager.List);
					if (this.AllowSelection)
					{
						this.SelectedIndex = this.dataManager.Position;
						return;
					}
				}
				else
				{
					this.SetItemCore(e.Index, this.dataManager[e.Index]);
				}
			}
		}

		/// <summary>Retrieves the current value of the <see cref="T:System.Windows.Forms.ListControl" /> item, if it is a property of an object, given the item.</summary>
		/// <param name="item">The object the <see cref="T:System.Windows.Forms.ListControl" /> item is bound to.</param>
		/// <returns>The filtered object.</returns>
		// Token: 0x0600293B RID: 10555 RVA: 0x000BFC56 File Offset: 0x000BDE56
		protected object FilterItemOnProperty(object item)
		{
			return this.FilterItemOnProperty(item, this.displayMember.BindingField);
		}

		/// <summary>Returns the current value of the <see cref="T:System.Windows.Forms.ListControl" /> item, if it is a property of an object given the item and the property name.</summary>
		/// <param name="item">The object the <see cref="T:System.Windows.Forms.ListControl" /> item is bound to.</param>
		/// <param name="field">The property name of the item the <see cref="T:System.Windows.Forms.ListControl" /> is bound to.</param>
		/// <returns>The filtered object.</returns>
		// Token: 0x0600293C RID: 10556 RVA: 0x000BFC6C File Offset: 0x000BDE6C
		protected object FilterItemOnProperty(object item, string field)
		{
			if (item != null && field.Length > 0)
			{
				try
				{
					PropertyDescriptor propertyDescriptor;
					if (this.dataManager != null)
					{
						propertyDescriptor = this.dataManager.GetItemProperties().Find(field, true);
					}
					else
					{
						propertyDescriptor = TypeDescriptor.GetProperties(item).Find(field, true);
					}
					if (propertyDescriptor != null)
					{
						item = propertyDescriptor.GetValue(item);
					}
				}
				catch
				{
				}
			}
			return item;
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x000BFCD4 File Offset: 0x000BDED4
		internal bool BindingFieldEmpty
		{
			get
			{
				return this.displayMember.BindingField.Length <= 0;
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x000BFCEC File Offset: 0x000BDEEC
		internal int FindStringInternal(string str, IList items, int startIndex, bool exact)
		{
			return this.FindStringInternal(str, items, startIndex, exact, true);
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x000BFCFC File Offset: 0x000BDEFC
		internal int FindStringInternal(string str, IList items, int startIndex, bool exact, bool ignorecase)
		{
			if (str == null || items == null)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= items.Count)
			{
				return -1;
			}
			int length = str.Length;
			int i = 0;
			int num = (startIndex + 1) % items.Count;
			while (i < items.Count)
			{
				i++;
				bool flag;
				if (exact)
				{
					flag = (string.Compare(str, this.GetItemText(items[num]), ignorecase, CultureInfo.CurrentCulture) == 0);
				}
				else
				{
					flag = (string.Compare(str, 0, this.GetItemText(items[num]), 0, length, ignorecase, CultureInfo.CurrentCulture) == 0);
				}
				if (flag)
				{
					return num;
				}
				num = (num + 1) % items.Count;
			}
			return -1;
		}

		/// <summary>Returns the text representation of the specified item.</summary>
		/// <param name="item">The object from which to get the contents to display. </param>
		/// <returns>If the <see cref="P:System.Windows.Forms.ListControl.DisplayMember" /> property is not specified, the value returned by <see cref="M:System.Windows.Forms.ListControl.GetItemText(System.Object)" /> is the value of the item's <see langword="ToString" /> method. Otherwise, the method returns the string value of the member specified in the <see cref="P:System.Windows.Forms.ListControl.DisplayMember" /> property for the object specified in the <paramref name="item" /> parameter.</returns>
		// Token: 0x06002940 RID: 10560 RVA: 0x000BFD9C File Offset: 0x000BDF9C
		public string GetItemText(object item)
		{
			if (!this.formattingEnabled)
			{
				if (item == null)
				{
					return string.Empty;
				}
				item = this.FilterItemOnProperty(item, this.displayMember.BindingField);
				if (item == null)
				{
					return "";
				}
				return Convert.ToString(item, CultureInfo.CurrentCulture);
			}
			else
			{
				object obj = this.FilterItemOnProperty(item, this.displayMember.BindingField);
				ListControlConvertEventArgs listControlConvertEventArgs = new ListControlConvertEventArgs(obj, typeof(string), item);
				this.OnFormat(listControlConvertEventArgs);
				if (listControlConvertEventArgs.Value != item && listControlConvertEventArgs.Value is string)
				{
					return (string)listControlConvertEventArgs.Value;
				}
				if (ListControl.stringTypeConverter == null)
				{
					ListControl.stringTypeConverter = TypeDescriptor.GetConverter(typeof(string));
				}
				string result;
				try
				{
					result = (string)Formatter.FormatObject(obj, typeof(string), this.DisplayMemberConverter, ListControl.stringTypeConverter, this.formatString, this.formatInfo, null, DBNull.Value);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					result = ((obj != null) ? Convert.ToString(item, CultureInfo.CurrentCulture) : "");
				}
				return result;
			}
		}

		/// <summary>Handles special input keys, such as PAGE UP, PAGE DOWN, HOME, END, and so on.</summary>
		/// <param name="keyData">One of the values of <see cref="T:System.Windows.Forms.Keys" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="keyData" /> parameter specifies the <see cref="F:System.Windows.Forms.Keys.End" />, <see cref="F:System.Windows.Forms.Keys.Home" />, <see cref="F:System.Windows.Forms.Keys.PageUp" />, or <see cref="F:System.Windows.Forms.Keys.PageDown" /> key; <see langword="false" /> if the <paramref name="keyData" /> parameter specifies <see cref="F:System.Windows.Forms.Keys.Alt" />.</returns>
		// Token: 0x06002941 RID: 10561 RVA: 0x000BFEB8 File Offset: 0x000BE0B8
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			Keys keys = keyData & Keys.KeyCode;
			return keys - Keys.Prior <= 3 || base.IsInputKey(keyData);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BindingContextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002942 RID: 10562 RVA: 0x000BFEED File Offset: 0x000BE0ED
		protected override void OnBindingContextChanged(EventArgs e)
		{
			this.SetDataConnection(this.dataSource, this.displayMember, true);
			base.OnBindingContextChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.DataSourceChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002943 RID: 10563 RVA: 0x000BFF0C File Offset: 0x000BE10C
		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_DATASOURCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.DisplayMemberChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002944 RID: 10564 RVA: 0x000BFF3C File Offset: 0x000BE13C
		protected virtual void OnDisplayMemberChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_DISPLAYMEMBERCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.Format" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ListControlConvertEventArgs" /> that contains the event data. </param>
		// Token: 0x06002945 RID: 10565 RVA: 0x000BFF6C File Offset: 0x000BE16C
		protected virtual void OnFormat(ListControlConvertEventArgs e)
		{
			ListControlConvertEventHandler listControlConvertEventHandler = base.Events[ListControl.EVENT_FORMAT] as ListControlConvertEventHandler;
			if (listControlConvertEventHandler != null)
			{
				listControlConvertEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.FormatInfoChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002946 RID: 10566 RVA: 0x000BFF9C File Offset: 0x000BE19C
		protected virtual void OnFormatInfoChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_FORMATINFOCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.FormatStringChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002947 RID: 10567 RVA: 0x000BFFCC File Offset: 0x000BE1CC
		protected virtual void OnFormatStringChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_FORMATSTRINGCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.FormattingEnabledChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002948 RID: 10568 RVA: 0x000BFFFC File Offset: 0x000BE1FC
		protected virtual void OnFormattingEnabledChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_FORMATTINGENABLEDCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.SelectedValueChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002949 RID: 10569 RVA: 0x000C002A File Offset: 0x000BE22A
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			this.OnSelectedValueChanged(EventArgs.Empty);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.ValueMemberChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600294A RID: 10570 RVA: 0x000C0038 File Offset: 0x000BE238
		protected virtual void OnValueMemberChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_VALUEMEMBERCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ListControl.SelectedValueChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600294B RID: 10571 RVA: 0x000C0068 File Offset: 0x000BE268
		protected virtual void OnSelectedValueChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_SELECTEDVALUECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>When overridden in a derived class, resynchronizes the data of the object at the specified index with the contents of the data source.</summary>
		/// <param name="index">The zero-based index of the item whose data to refresh. </param>
		// Token: 0x0600294C RID: 10572
		protected abstract void RefreshItem(int index);

		/// <summary>When overridden in a derived class, resynchronizes the item data with the contents of the data source.</summary>
		// Token: 0x0600294D RID: 10573 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void RefreshItems()
		{
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000C0096 File Offset: 0x000BE296
		private void DataSourceDisposed(object sender, EventArgs e)
		{
			this.SetDataConnection(null, new BindingMemberInfo(""), true);
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000C00AC File Offset: 0x000BE2AC
		private void DataSourceInitialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
			this.SetDataConnection(this.dataSource, this.displayMember, true);
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000C00D8 File Offset: 0x000BE2D8
		private void SetDataConnection(object newDataSource, BindingMemberInfo newDisplayMember, bool force)
		{
			bool flag = this.dataSource != newDataSource;
			bool flag2 = !this.displayMember.Equals(newDisplayMember);
			if (this.inSetDataConnection)
			{
				return;
			}
			try
			{
				if (force || flag || flag2)
				{
					this.inSetDataConnection = true;
					IList list = (this.DataManager != null) ? this.DataManager.List : null;
					bool flag3 = this.DataManager == null;
					this.UnwireDataSource();
					this.dataSource = newDataSource;
					this.displayMember = newDisplayMember;
					this.WireDataSource();
					if (this.isDataSourceInitialized)
					{
						CurrencyManager currencyManager = null;
						if (newDataSource != null && this.BindingContext != null && newDataSource != Convert.DBNull)
						{
							currencyManager = (CurrencyManager)this.BindingContext[newDataSource, newDisplayMember.BindingPath];
						}
						if (this.dataManager != currencyManager)
						{
							if (this.dataManager != null)
							{
								this.dataManager.ItemChanged -= this.DataManager_ItemChanged;
								this.dataManager.PositionChanged -= this.DataManager_PositionChanged;
							}
							this.dataManager = currencyManager;
							if (this.dataManager != null)
							{
								this.dataManager.ItemChanged += this.DataManager_ItemChanged;
								this.dataManager.PositionChanged += this.DataManager_PositionChanged;
							}
						}
						if (this.dataManager != null && (flag2 || flag) && this.displayMember.BindingMember != null && this.displayMember.BindingMember.Length != 0 && !this.BindingMemberInfoInDataManager(this.displayMember))
						{
							throw new ArgumentException(SR.GetString("ListControlWrongDisplayMember"), "newDisplayMember");
						}
						if (this.dataManager != null && (flag || flag2 || force) && (flag2 || (force && (list != this.dataManager.List || flag3))))
						{
							this.DataManager_ItemChanged(this.dataManager, new ItemChangedEventArgs(-1));
						}
					}
					this.displayMemberConverter = null;
				}
				if (flag)
				{
					this.OnDataSourceChanged(EventArgs.Empty);
				}
				if (flag2)
				{
					this.OnDisplayMemberChanged(EventArgs.Empty);
				}
			}
			finally
			{
				this.inSetDataConnection = false;
			}
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000C02F0 File Offset: 0x000BE4F0
		private void UnwireDataSource()
		{
			if (this.dataSource is IComponent)
			{
				((IComponent)this.dataSource).Disposed -= this.DataSourceDisposed;
			}
			ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && this.isDataSourceInitEventHooked)
			{
				supportInitializeNotification.Initialized -= this.DataSourceInitialized;
				this.isDataSourceInitEventHooked = false;
			}
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000C0358 File Offset: 0x000BE558
		private void WireDataSource()
		{
			if (this.dataSource is IComponent)
			{
				((IComponent)this.dataSource).Disposed += this.DataSourceDisposed;
			}
			ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
			{
				supportInitializeNotification.Initialized += this.DataSourceInitialized;
				this.isDataSourceInitEventHooked = true;
				this.isDataSourceInitialized = false;
				return;
			}
			this.isDataSourceInitialized = true;
		}

		/// <summary>When overridden in a derived class, sets the specified array of objects in a collection in the derived class.</summary>
		/// <param name="items">An array of items.</param>
		// Token: 0x06002953 RID: 10579
		protected abstract void SetItemsCore(IList items);

		/// <summary>When overridden in a derived class, sets the object with the specified index in the derived class.</summary>
		/// <param name="index">The array index of the object.</param>
		/// <param name="value">The object.</param>
		// Token: 0x06002954 RID: 10580 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void SetItemCore(int index, object value)
		{
		}

		// Token: 0x040011C9 RID: 4553
		private static readonly object EVENT_DATASOURCECHANGED = new object();

		// Token: 0x040011CA RID: 4554
		private static readonly object EVENT_DISPLAYMEMBERCHANGED = new object();

		// Token: 0x040011CB RID: 4555
		private static readonly object EVENT_VALUEMEMBERCHANGED = new object();

		// Token: 0x040011CC RID: 4556
		private static readonly object EVENT_SELECTEDVALUECHANGED = new object();

		// Token: 0x040011CD RID: 4557
		private static readonly object EVENT_FORMATINFOCHANGED = new object();

		// Token: 0x040011CE RID: 4558
		private static readonly object EVENT_FORMATSTRINGCHANGED = new object();

		// Token: 0x040011CF RID: 4559
		private static readonly object EVENT_FORMATTINGENABLEDCHANGED = new object();

		// Token: 0x040011D0 RID: 4560
		private object dataSource;

		// Token: 0x040011D1 RID: 4561
		private CurrencyManager dataManager;

		// Token: 0x040011D2 RID: 4562
		private BindingMemberInfo displayMember;

		// Token: 0x040011D3 RID: 4563
		private BindingMemberInfo valueMember;

		// Token: 0x040011D4 RID: 4564
		private string formatString = string.Empty;

		// Token: 0x040011D5 RID: 4565
		private IFormatProvider formatInfo;

		// Token: 0x040011D6 RID: 4566
		private bool formattingEnabled;

		// Token: 0x040011D7 RID: 4567
		private static readonly object EVENT_FORMAT = new object();

		// Token: 0x040011D8 RID: 4568
		private TypeConverter displayMemberConverter;

		// Token: 0x040011D9 RID: 4569
		private static TypeConverter stringTypeConverter = null;

		// Token: 0x040011DA RID: 4570
		private bool isDataSourceInitialized;

		// Token: 0x040011DB RID: 4571
		private bool isDataSourceInitEventHooked;

		// Token: 0x040011DC RID: 4572
		private bool inSetDataConnection;
	}
}
