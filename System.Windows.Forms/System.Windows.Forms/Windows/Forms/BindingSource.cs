using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Encapsulates the data source for a form.</summary>
	// Token: 0x0200012B RID: 299
	[DefaultProperty("DataSource")]
	[DefaultEvent("CurrentChanged")]
	[ComplexBindingProperties("DataSource", "DataMember")]
	[Designer("System.Windows.Forms.Design.BindingSourceDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionBindingSource")]
	public class BindingSource : Component, IBindingListView, IBindingList, IList, ICollection, IEnumerable, ITypedList, ICancelAddNew, ISupportInitializeNotification, ISupportInitialize, ICurrencyManagerProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingSource" /> class to the default property values.</summary>
		// Token: 0x06000857 RID: 2135 RVA: 0x00019216 File Offset: 0x00017416
		public BindingSource() : this(null, string.Empty)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingSource" /> class with the specified data source and data member.</summary>
		/// <param name="dataSource">The data source for the <see cref="T:System.Windows.Forms.BindingSource" />.</param>
		/// <param name="dataMember">The specific column or list name within the data source to bind to.</param>
		// Token: 0x06000858 RID: 2136 RVA: 0x00019224 File Offset: 0x00017424
		public BindingSource(object dataSource, string dataMember)
		{
			this.dataSource = dataSource;
			this.dataMember = dataMember;
			this._innerList = new ArrayList();
			this.currencyManager = new CurrencyManager(this);
			this.WireCurrencyManager(this.currencyManager);
			this.listItemPropertyChangedHandler = new EventHandler(this.ListItem_PropertyChanged);
			this.ResetList();
			this.WireDataSource();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingSource" /> class and adds the <see cref="T:System.Windows.Forms.BindingSource" /> to the specified container.</summary>
		/// <param name="container">The <see cref="T:System.ComponentModel.IContainer" /> to add the current <see cref="T:System.Windows.Forms.BindingSource" /> to.</param>
		// Token: 0x06000859 RID: 2137 RVA: 0x000192A6 File Offset: 0x000174A6
		public BindingSource(IContainer container) : this()
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000192C4 File Offset: 0x000174C4
		private bool AllowNewInternal(bool checkconstructor)
		{
			if (this.disposedOrFinalized)
			{
				return false;
			}
			if (this.allowNewIsSet)
			{
				return this.allowNewSetValue;
			}
			if (this.listExtractedFromEnumerable)
			{
				return false;
			}
			if (this.isBindingList)
			{
				return ((IBindingList)this.List).AllowNew;
			}
			return this.IsListWriteable(checkconstructor);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00019314 File Offset: 0x00017514
		private bool IsListWriteable(bool checkconstructor)
		{
			return !this.List.IsReadOnly && !this.List.IsFixedSize && (!checkconstructor || this.itemConstructor != null);
		}

		/// <summary>Gets the currency manager associated with this <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.CurrencyManager" /> associated with this <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x00019343 File Offset: 0x00017543
		[Browsable(false)]
		public virtual CurrencyManager CurrencyManager
		{
			get
			{
				return ((ICurrencyManagerProvider)this).GetRelatedCurrencyManager(null);
			}
		}

		/// <summary>Gets the related currency manager for the specified data member.</summary>
		/// <param name="dataMember">The name of column or list, within the data source to retrieve the currency manager for.</param>
		/// <returns>The related <see cref="T:System.Windows.Forms.CurrencyManager" /> for the specified data member.</returns>
		// Token: 0x0600085D RID: 2141 RVA: 0x0001934C File Offset: 0x0001754C
		public virtual CurrencyManager GetRelatedCurrencyManager(string dataMember)
		{
			this.EnsureInnerList();
			if (string.IsNullOrEmpty(dataMember))
			{
				return this.currencyManager;
			}
			if (dataMember.IndexOf(".") != -1)
			{
				return null;
			}
			BindingSource relatedBindingSource = this.GetRelatedBindingSource(dataMember);
			return ((ICurrencyManagerProvider)relatedBindingSource).CurrencyManager;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001938C File Offset: 0x0001758C
		private BindingSource GetRelatedBindingSource(string dataMember)
		{
			if (this.relatedBindingSources == null)
			{
				this.relatedBindingSources = new Dictionary<string, BindingSource>();
			}
			foreach (string text in this.relatedBindingSources.Keys)
			{
				if (string.Equals(text, dataMember, StringComparison.OrdinalIgnoreCase))
				{
					return this.relatedBindingSources[text];
				}
			}
			BindingSource bindingSource = new BindingSource(this, dataMember);
			this.relatedBindingSources[dataMember] = bindingSource;
			return bindingSource;
		}

		/// <summary>Gets the current item in the list.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the current item in the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property, or <see langword="null" /> if the list has no items.</returns>
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00019424 File Offset: 0x00017624
		[Browsable(false)]
		public object Current
		{
			get
			{
				if (this.currencyManager.Count <= 0)
				{
					return null;
				}
				return this.currencyManager.Current;
			}
		}

		/// <summary>Gets or sets the specific list in the data source to which the connector currently binds to.</summary>
		/// <returns>The name of a list (or row) in the <see cref="P:System.Windows.Forms.BindingSource.DataSource" />. The default is an empty string ("").</returns>
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x00019441 File Offset: 0x00017641
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x00019449 File Offset: 0x00017649
		[SRCategory("CatData")]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("BindingSourceDataMemberDescr")]
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (!this.dataMember.Equals(value))
				{
					this.dataMember = value;
					this.ResetList();
					this.OnDataMemberChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets the data source that the connector binds to.</summary>
		/// <returns>An <see cref="T:System.Object" /> that acts as a data source. The default is <see langword="null" />.</returns>
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0001947B File Offset: 0x0001767B
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x00019483 File Offset: 0x00017683
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		[SRDescription("BindingSourceDataSourceDescr")]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (this.dataSource != value)
				{
					this.ThrowIfBindingSourceRecursionDetected(value);
					this.UnwireDataSource();
					this.dataSource = value;
					this.ClearInvalidDataMember();
					this.ResetList();
					this.WireDataSource();
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x000194C0 File Offset: 0x000176C0
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x000194F0 File Offset: 0x000176F0
		private string InnerListFilter
		{
			get
			{
				IBindingListView bindingListView = this.List as IBindingListView;
				if (bindingListView != null && bindingListView.SupportsFiltering)
				{
					return bindingListView.Filter;
				}
				return string.Empty;
			}
			set
			{
				if (this.initializing || base.DesignMode)
				{
					return;
				}
				if (string.Equals(value, this.InnerListFilter, StringComparison.Ordinal))
				{
					return;
				}
				IBindingListView bindingListView = this.List as IBindingListView;
				if (bindingListView != null && bindingListView.SupportsFiltering)
				{
					bindingListView.Filter = value;
				}
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0001953C File Offset: 0x0001773C
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x000195B4 File Offset: 0x000177B4
		private string InnerListSort
		{
			get
			{
				ListSortDescriptionCollection sortsColln = null;
				IBindingListView bindingListView = this.List as IBindingListView;
				IBindingList bindingList = this.List as IBindingList;
				if (bindingListView != null && bindingListView.SupportsAdvancedSorting)
				{
					sortsColln = bindingListView.SortDescriptions;
				}
				else if (bindingList != null && bindingList.SupportsSorting && bindingList.IsSorted)
				{
					sortsColln = new ListSortDescriptionCollection(new ListSortDescription[]
					{
						new ListSortDescription(bindingList.SortProperty, bindingList.SortDirection)
					});
				}
				return BindingSource.BuildSortString(sortsColln);
			}
			set
			{
				if (this.initializing || base.DesignMode)
				{
					return;
				}
				if (string.Compare(value, this.InnerListSort, false, CultureInfo.InvariantCulture) == 0)
				{
					return;
				}
				ListSortDescriptionCollection listSortDescriptionCollection = this.ParseSortString(value);
				IBindingListView bindingListView = this.List as IBindingListView;
				IBindingList bindingList = this.List as IBindingList;
				if (bindingListView == null || !bindingListView.SupportsAdvancedSorting)
				{
					if (bindingList != null && bindingList.SupportsSorting)
					{
						if (listSortDescriptionCollection.Count == 0)
						{
							bindingList.RemoveSort();
							return;
						}
						if (listSortDescriptionCollection.Count == 1)
						{
							bindingList.ApplySort(listSortDescriptionCollection[0].PropertyDescriptor, listSortDescriptionCollection[0].SortDirection);
							return;
						}
					}
					return;
				}
				if (listSortDescriptionCollection.Count == 0)
				{
					bindingListView.RemoveSort();
					return;
				}
				bindingListView.ApplySort(listSortDescriptionCollection);
			}
		}

		/// <summary>Gets a value indicating whether the list binding is suspended.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate the binding is suspended; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0001966B File Offset: 0x0001786B
		[Browsable(false)]
		public bool IsBindingSuspended
		{
			get
			{
				return this.currencyManager.IsBindingSuspended;
			}
		}

		/// <summary>Gets the list that the connector is bound to.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> that represents the list, or <see langword="null" /> if there is no underlying list associated with this <see cref="T:System.Windows.Forms.BindingSource" />.</returns>
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x00019678 File Offset: 0x00017878
		[Browsable(false)]
		public IList List
		{
			get
			{
				this.EnsureInnerList();
				return this._innerList;
			}
		}

		/// <summary>Gets or sets the index of the current item in the underlying list.</summary>
		/// <returns>A zero-based index that specifies the position of the current item in the underlying list.</returns>
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x00019686 File Offset: 0x00017886
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x00019693 File Offset: 0x00017893
		[DefaultValue(-1)]
		[Browsable(false)]
		public int Position
		{
			get
			{
				return this.currencyManager.Position;
			}
			set
			{
				if (this.currencyManager.Position != value)
				{
					this.currencyManager.Position = value;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether <see cref="E:System.Windows.Forms.BindingSource.ListChanged" /> events should be raised.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="E:System.Windows.Forms.BindingSource.ListChanged" /> events should be raised; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x000196AF File Offset: 0x000178AF
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x000196B7 File Offset: 0x000178B7
		[DefaultValue(true)]
		[Browsable(false)]
		public bool RaiseListChangedEvents
		{
			get
			{
				return this.raiseListChangedEvents;
			}
			set
			{
				if (this.raiseListChangedEvents != value)
				{
					this.raiseListChangedEvents = value;
				}
			}
		}

		/// <summary>Gets or sets the column names used for sorting, and the sort order for viewing the rows in the data source.</summary>
		/// <returns>A case-sensitive string containing the column name followed by "ASC" (for ascending) or "DESC" (for descending). The default is <see langword="null" />.</returns>
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x000196C9 File Offset: 0x000178C9
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x000196D1 File Offset: 0x000178D1
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[SRDescription("BindingSourceSortDescr")]
		public string Sort
		{
			get
			{
				return this.sort;
			}
			set
			{
				this.sort = value;
				this.InnerListSort = value;
			}
		}

		/// <summary>Occurs before an item is added to the underlying list.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.ComponentModel.AddingNewEventArgs.NewObject" /> is not the same type as the type contained in the list.</exception>
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000870 RID: 2160 RVA: 0x000196E1 File Offset: 0x000178E1
		// (remove) Token: 0x06000871 RID: 2161 RVA: 0x000196F4 File Offset: 0x000178F4
		[SRCategory("CatData")]
		[SRDescription("BindingSourceAddingNewEventHandlerDescr")]
		public event AddingNewEventHandler AddingNew
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_ADDINGNEW, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_ADDINGNEW, value);
			}
		}

		/// <summary>Occurs when all the clients have been bound to this <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06000872 RID: 2162 RVA: 0x00019707 File Offset: 0x00017907
		// (remove) Token: 0x06000873 RID: 2163 RVA: 0x0001971A File Offset: 0x0001791A
		[SRCategory("CatData")]
		[SRDescription("BindingSourceBindingCompleteEventHandlerDescr")]
		public event BindingCompleteEventHandler BindingComplete
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_BINDINGCOMPLETE, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_BINDINGCOMPLETE, value);
			}
		}

		/// <summary>Occurs when a currency-related exception is silently handled by the <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06000874 RID: 2164 RVA: 0x0001972D File Offset: 0x0001792D
		// (remove) Token: 0x06000875 RID: 2165 RVA: 0x00019740 File Offset: 0x00017940
		[SRCategory("CatData")]
		[SRDescription("BindingSourceDataErrorEventHandlerDescr")]
		public event BindingManagerDataErrorEventHandler DataError
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_DATAERROR, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_DATAERROR, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.BindingSource.DataSource" /> property value has changed.</summary>
		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06000876 RID: 2166 RVA: 0x00019753 File Offset: 0x00017953
		// (remove) Token: 0x06000877 RID: 2167 RVA: 0x00019766 File Offset: 0x00017966
		[SRCategory("CatData")]
		[SRDescription("BindingSourceDataSourceChangedEventHandlerDescr")]
		public event EventHandler DataSourceChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_DATASOURCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_DATASOURCECHANGED, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.BindingSource.DataMember" /> property value has changed.</summary>
		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06000878 RID: 2168 RVA: 0x00019779 File Offset: 0x00017979
		// (remove) Token: 0x06000879 RID: 2169 RVA: 0x0001978C File Offset: 0x0001798C
		[SRCategory("CatData")]
		[SRDescription("BindingSourceDataMemberChangedEventHandlerDescr")]
		public event EventHandler DataMemberChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_DATAMEMBERCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_DATAMEMBERCHANGED, value);
			}
		}

		/// <summary>Occurs when the currently bound item changes.</summary>
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x0600087A RID: 2170 RVA: 0x0001979F File Offset: 0x0001799F
		// (remove) Token: 0x0600087B RID: 2171 RVA: 0x000197B2 File Offset: 0x000179B2
		[SRCategory("CatData")]
		[SRDescription("BindingSourceCurrentChangedEventHandlerDescr")]
		public event EventHandler CurrentChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_CURRENTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_CURRENTCHANGED, value);
			}
		}

		/// <summary>Occurs when a property value of the <see cref="P:System.Windows.Forms.BindingSource.Current" /> property has changed.</summary>
		// Token: 0x14000043 RID: 67
		// (add) Token: 0x0600087C RID: 2172 RVA: 0x000197C5 File Offset: 0x000179C5
		// (remove) Token: 0x0600087D RID: 2173 RVA: 0x000197D8 File Offset: 0x000179D8
		[SRCategory("CatData")]
		[SRDescription("BindingSourceCurrentItemChangedEventHandlerDescr")]
		public event EventHandler CurrentItemChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_CURRENTITEMCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_CURRENTITEMCHANGED, value);
			}
		}

		/// <summary>Occurs when the underlying list changes or an item in the list changes.</summary>
		// Token: 0x14000044 RID: 68
		// (add) Token: 0x0600087E RID: 2174 RVA: 0x000197EB File Offset: 0x000179EB
		// (remove) Token: 0x0600087F RID: 2175 RVA: 0x000197FE File Offset: 0x000179FE
		[SRCategory("CatData")]
		[SRDescription("BindingSourceListChangedEventHandlerDescr")]
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_LISTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_LISTCHANGED, value);
			}
		}

		/// <summary>Occurs after the value of the <see cref="P:System.Windows.Forms.BindingSource.Position" /> property has changed.</summary>
		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000880 RID: 2176 RVA: 0x00019811 File Offset: 0x00017A11
		// (remove) Token: 0x06000881 RID: 2177 RVA: 0x00019824 File Offset: 0x00017A24
		[SRCategory("CatData")]
		[SRDescription("BindingSourcePositionChangedEventHandlerDescr")]
		public event EventHandler PositionChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_POSITIONCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_POSITIONCHANGED, value);
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00019838 File Offset: 0x00017A38
		private static string BuildSortString(ListSortDescriptionCollection sortsColln)
		{
			if (sortsColln == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(sortsColln.Count);
			for (int i = 0; i < sortsColln.Count; i++)
			{
				stringBuilder.Append(sortsColln[i].PropertyDescriptor.Name + ((sortsColln[i].SortDirection == ListSortDirection.Ascending) ? " ASC" : " DESC") + ((i < sortsColln.Count - 1) ? "," : string.Empty));
			}
			return stringBuilder.ToString();
		}

		/// <summary>Cancels the current edit operation.</summary>
		// Token: 0x06000883 RID: 2179 RVA: 0x000198BF File Offset: 0x00017ABF
		public void CancelEdit()
		{
			this.currencyManager.CancelCurrentEdit();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000198CC File Offset: 0x00017ACC
		private void ThrowIfBindingSourceRecursionDetected(object newDataSource)
		{
			for (BindingSource bindingSource = newDataSource as BindingSource; bindingSource != null; bindingSource = (bindingSource.DataSource as BindingSource))
			{
				if (bindingSource == this)
				{
					throw new InvalidOperationException(SR.GetString("BindingSourceRecursionDetected"));
				}
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00019905 File Offset: 0x00017B05
		private void ClearInvalidDataMember()
		{
			if (!this.IsDataMemberValid())
			{
				this.dataMember = "";
				this.OnDataMemberChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00019928 File Offset: 0x00017B28
		private static IList CreateBindingList(Type type)
		{
			Type typeFromHandle = typeof(BindingList<>);
			Type type2 = typeFromHandle.MakeGenericType(new Type[]
			{
				type
			});
			return (IList)SecurityUtils.SecureCreateInstance(type2);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001995C File Offset: 0x00017B5C
		private static object CreateInstanceOfType(Type type)
		{
			object result = null;
			Exception ex = null;
			try
			{
				result = SecurityUtils.SecureCreateInstance(type);
			}
			catch (TargetInvocationException ex2)
			{
				ex = ex2;
			}
			catch (MethodAccessException ex3)
			{
				ex = ex3;
			}
			catch (MissingMethodException ex4)
			{
				ex = ex4;
			}
			if (ex != null)
			{
				throw new NotSupportedException(SR.GetString("BindingSourceInstanceError"), ex);
			}
			return result;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000199C4 File Offset: 0x00017BC4
		private void CurrencyManager_PositionChanged(object sender, EventArgs e)
		{
			this.OnPositionChanged(e);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x000199CD File Offset: 0x00017BCD
		private void CurrencyManager_CurrentChanged(object sender, EventArgs e)
		{
			this.OnCurrentChanged(EventArgs.Empty);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x000199DA File Offset: 0x00017BDA
		private void CurrencyManager_CurrentItemChanged(object sender, EventArgs e)
		{
			this.OnCurrentItemChanged(EventArgs.Empty);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x000199E7 File Offset: 0x00017BE7
		private void CurrencyManager_BindingComplete(object sender, BindingCompleteEventArgs e)
		{
			this.OnBindingComplete(e);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x000199F0 File Offset: 0x00017BF0
		private void CurrencyManager_DataError(object sender, BindingManagerDataErrorEventArgs e)
		{
			this.OnDataError(e);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.BindingSource" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x0600088D RID: 2189 RVA: 0x000199FC File Offset: 0x00017BFC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.UnwireDataSource();
				this.UnwireInnerList();
				this.UnhookItemChangedEventsForOldCurrent();
				this.UnwireCurrencyManager(this.currencyManager);
				this.dataSource = null;
				this.sort = null;
				this.dataMember = null;
				this._innerList = null;
				this.isBindingList = false;
				this.needToSetList = true;
				this.raiseListChangedEvents = false;
			}
			this.disposedOrFinalized = true;
			base.Dispose(disposing);
		}

		/// <summary>Applies pending changes to the underlying data source.</summary>
		// Token: 0x0600088E RID: 2190 RVA: 0x00019A6C File Offset: 0x00017C6C
		public void EndEdit()
		{
			if (this.endingEdit)
			{
				return;
			}
			try
			{
				this.endingEdit = true;
				this.currencyManager.EndCurrentEdit();
			}
			finally
			{
				this.endingEdit = false;
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00019AB0 File Offset: 0x00017CB0
		private void EnsureInnerList()
		{
			if (!this.initializing && this.needToSetList)
			{
				this.needToSetList = false;
				this.ResetList();
			}
		}

		/// <summary>Returns the index of the item in the list with the specified property name and value.</summary>
		/// <param name="propertyName">The name of the property to search for.</param>
		/// <param name="key">The value of the item with the specified <paramref name="propertyName" /> to find.</param>
		/// <returns>The zero-based index of the item with the specified property name and value. </returns>
		/// <exception cref="T:System.InvalidOperationException">The underlying list is not a <see cref="T:System.ComponentModel.IBindingList" /> with searching functionality implemented.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="propertyName" /> does not match a property in the list.</exception>
		// Token: 0x06000890 RID: 2192 RVA: 0x00019AD0 File Offset: 0x00017CD0
		public int Find(string propertyName, object key)
		{
			PropertyDescriptor propertyDescriptor = (this.itemShape == null) ? null : this.itemShape.Find(propertyName, true);
			if (propertyDescriptor == null)
			{
				throw new ArgumentException(SR.GetString("DataSourceDataMemberPropNotFound", new object[]
				{
					propertyName
				}));
			}
			return ((IBindingList)this).Find(propertyDescriptor, key);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00019B1C File Offset: 0x00017D1C
		private static IList GetListFromType(Type type)
		{
			IList result;
			if (typeof(ITypedList).IsAssignableFrom(type) && typeof(IList).IsAssignableFrom(type))
			{
				result = (BindingSource.CreateInstanceOfType(type) as IList);
			}
			else if (typeof(IListSource).IsAssignableFrom(type))
			{
				result = (BindingSource.CreateInstanceOfType(type) as IListSource).GetList();
			}
			else
			{
				result = BindingSource.CreateBindingList(ListBindingHelper.GetListItemType(type));
			}
			return result;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00019B90 File Offset: 0x00017D90
		private static IList GetListFromEnumerable(IEnumerable enumerable)
		{
			IList list = null;
			foreach (object obj in enumerable)
			{
				if (list == null)
				{
					list = BindingSource.CreateBindingList(obj.GetType());
				}
				list.Add(obj);
			}
			return list;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00019BF4 File Offset: 0x00017DF4
		private bool IsDataMemberValid()
		{
			if (this.initializing)
			{
				return true;
			}
			if (string.IsNullOrEmpty(this.dataMember))
			{
				return true;
			}
			PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(this.dataSource);
			return listItemProperties[this.dataMember] != null;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00019C3C File Offset: 0x00017E3C
		private void InnerList_ListChanged(object sender, ListChangedEventArgs e)
		{
			if (!this.innerListChanging)
			{
				try
				{
					this.innerListChanging = true;
					this.OnListChanged(e);
				}
				finally
				{
					this.innerListChanging = false;
				}
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00019C7C File Offset: 0x00017E7C
		private void ListItem_PropertyChanged(object sender, EventArgs e)
		{
			int newIndex;
			if (sender == this.currentItemHookedForItemChange)
			{
				newIndex = this.Position;
			}
			else
			{
				newIndex = ((IList)this).IndexOf(sender);
			}
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, newIndex));
		}

		/// <summary>Moves to the first item in the list.</summary>
		// Token: 0x06000896 RID: 2198 RVA: 0x00019CB0 File Offset: 0x00017EB0
		public void MoveFirst()
		{
			this.Position = 0;
		}

		/// <summary>Moves to the last item in the list.</summary>
		// Token: 0x06000897 RID: 2199 RVA: 0x00019CB9 File Offset: 0x00017EB9
		public void MoveLast()
		{
			this.Position = this.Count - 1;
		}

		/// <summary>Moves to the next item in the list.</summary>
		// Token: 0x06000898 RID: 2200 RVA: 0x00019CCC File Offset: 0x00017ECC
		public void MoveNext()
		{
			int position = this.Position + 1;
			this.Position = position;
		}

		/// <summary>Moves to the previous item in the list.</summary>
		// Token: 0x06000899 RID: 2201 RVA: 0x00019CEC File Offset: 0x00017EEC
		public void MovePrevious()
		{
			int position = this.Position - 1;
			this.Position = position;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00019D09 File Offset: 0x00017F09
		private void OnSimpleListChanged(ListChangedType listChangedType, int newIndex)
		{
			if (!this.isBindingList)
			{
				this.OnListChanged(new ListChangedEventArgs(listChangedType, newIndex));
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.AddingNew" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600089B RID: 2203 RVA: 0x00019D20 File Offset: 0x00017F20
		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			AddingNewEventHandler addingNewEventHandler = (AddingNewEventHandler)base.Events[BindingSource.EVENT_ADDINGNEW];
			if (addingNewEventHandler != null)
			{
				addingNewEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.BindingComplete" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" />  that contains the event data. </param>
		// Token: 0x0600089C RID: 2204 RVA: 0x00019D50 File Offset: 0x00017F50
		protected virtual void OnBindingComplete(BindingCompleteEventArgs e)
		{
			BindingCompleteEventHandler bindingCompleteEventHandler = (BindingCompleteEventHandler)base.Events[BindingSource.EVENT_BINDINGCOMPLETE];
			if (bindingCompleteEventHandler != null)
			{
				bindingCompleteEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.CurrentChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600089D RID: 2205 RVA: 0x00019D80 File Offset: 0x00017F80
		protected virtual void OnCurrentChanged(EventArgs e)
		{
			this.UnhookItemChangedEventsForOldCurrent();
			this.HookItemChangedEventsForNewCurrent();
			EventHandler eventHandler = (EventHandler)base.Events[BindingSource.EVENT_CURRENTCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.CurrentItemChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600089E RID: 2206 RVA: 0x00019DBC File Offset: 0x00017FBC
		protected virtual void OnCurrentItemChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[BindingSource.EVENT_CURRENTITEMCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.DataError" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.BindingManagerDataErrorEventArgs" /> that contains the event data. </param>
		// Token: 0x0600089F RID: 2207 RVA: 0x00019DEC File Offset: 0x00017FEC
		protected virtual void OnDataError(BindingManagerDataErrorEventArgs e)
		{
			BindingManagerDataErrorEventHandler bindingManagerDataErrorEventHandler = base.Events[BindingSource.EVENT_DATAERROR] as BindingManagerDataErrorEventHandler;
			if (bindingManagerDataErrorEventHandler != null)
			{
				bindingManagerDataErrorEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.DataMemberChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060008A0 RID: 2208 RVA: 0x00019E1C File Offset: 0x0001801C
		protected virtual void OnDataMemberChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[BindingSource.EVENT_DATAMEMBERCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.DataSourceChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060008A1 RID: 2209 RVA: 0x00019E4C File Offset: 0x0001804C
		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[BindingSource.EVENT_DATASOURCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.ListChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060008A2 RID: 2210 RVA: 0x00019E7C File Offset: 0x0001807C
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if (!this.raiseListChangedEvents || this.initializing)
			{
				return;
			}
			ListChangedEventHandler listChangedEventHandler = (ListChangedEventHandler)base.Events[BindingSource.EVENT_LISTCHANGED];
			if (listChangedEventHandler != null)
			{
				listChangedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingSource.PositionChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.ListChangedEventArgs" /> that contains the event data.</param>
		// Token: 0x060008A3 RID: 2211 RVA: 0x00019EBC File Offset: 0x000180BC
		protected virtual void OnPositionChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[BindingSource.EVENT_POSITIONCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00019EEC File Offset: 0x000180EC
		private void ParentCurrencyManager_CurrentItemChanged(object sender, EventArgs e)
		{
			if (this.initializing)
			{
				return;
			}
			if (this.parentsCurrentItemChanging)
			{
				return;
			}
			try
			{
				this.parentsCurrentItemChanging = true;
				bool flag;
				this.currencyManager.PullData(out flag);
			}
			finally
			{
				this.parentsCurrentItemChanging = false;
			}
			CurrencyManager currencyManager = (CurrencyManager)sender;
			if (!string.IsNullOrEmpty(this.dataMember))
			{
				object obj = null;
				IList list = null;
				if (currencyManager.Count > 0)
				{
					PropertyDescriptorCollection itemProperties = currencyManager.GetItemProperties();
					PropertyDescriptor propertyDescriptor = itemProperties[this.dataMember];
					if (propertyDescriptor != null)
					{
						obj = ListBindingHelper.GetList(propertyDescriptor.GetValue(currencyManager.Current));
						list = (obj as IList);
					}
				}
				if (list != null)
				{
					this.SetList(list, false, true);
				}
				else if (obj != null)
				{
					this.SetList(BindingSource.WrapObjectInBindingList(obj), false, false);
				}
				else
				{
					this.SetList(BindingSource.CreateBindingList(this.itemType), false, false);
				}
				bool flag2 = this.lastCurrentItem == null || currencyManager.Count == 0 || this.lastCurrentItem != currencyManager.Current || this.Position >= this.Count;
				this.lastCurrentItem = ((currencyManager.Count > 0) ? currencyManager.Current : null);
				if (flag2)
				{
					this.Position = ((this.Count > 0) ? 0 : -1);
				}
			}
			this.OnCurrentItemChanged(EventArgs.Empty);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001A03C File Offset: 0x0001823C
		private void ParentCurrencyManager_MetaDataChanged(object sender, EventArgs e)
		{
			this.ClearInvalidDataMember();
			this.ResetList();
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001A04C File Offset: 0x0001824C
		private ListSortDescriptionCollection ParseSortString(string sortString)
		{
			if (string.IsNullOrEmpty(sortString))
			{
				return new ListSortDescriptionCollection();
			}
			ArrayList arrayList = new ArrayList();
			PropertyDescriptorCollection itemProperties = this.currencyManager.GetItemProperties();
			string[] array = sortString.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				int length = text.Length;
				bool flag = true;
				if (length >= 5 && string.Compare(text, length - 4, " ASC", 0, 4, true, CultureInfo.InvariantCulture) == 0)
				{
					text = text.Substring(0, length - 4).Trim();
				}
				else if (length >= 6 && string.Compare(text, length - 5, " DESC", 0, 5, true, CultureInfo.InvariantCulture) == 0)
				{
					flag = false;
					text = text.Substring(0, length - 5).Trim();
				}
				if (text.StartsWith("["))
				{
					if (!text.EndsWith("]"))
					{
						throw new ArgumentException(SR.GetString("BindingSourceBadSortString"));
					}
					text = text.Substring(1, text.Length - 2);
				}
				PropertyDescriptor propertyDescriptor = itemProperties.Find(text, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("BindingSourceSortStringPropertyNotInIBindingList"));
				}
				arrayList.Add(new ListSortDescription(propertyDescriptor, flag ? ListSortDirection.Ascending : ListSortDirection.Descending));
			}
			ListSortDescription[] array2 = new ListSortDescription[arrayList.Count];
			arrayList.CopyTo(array2);
			return new ListSortDescriptionCollection(array2);
		}

		/// <summary>Removes the current item from the list.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.BindingSource.AllowRemove" /> property is <see langword="false" />.-or-
		///         <see cref="P:System.Windows.Forms.BindingSource.Position" /> is less than zero or greater than <see cref="P:System.Windows.Forms.BindingSource.Count" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property is read-only or has a fixed size.</exception>
		// Token: 0x060008A7 RID: 2215 RVA: 0x0001A1B4 File Offset: 0x000183B4
		public void RemoveCurrent()
		{
			if (!((IBindingList)this).AllowRemove)
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceRemoveCurrentNotAllowed"));
			}
			if (this.Position < 0 || this.Position >= this.Count)
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceRemoveCurrentNoCurrentItem"));
			}
			this.RemoveAt(this.Position);
		}

		/// <summary>Reinitializes the <see cref="P:System.Windows.Forms.BindingSource.AllowNew" /> property.</summary>
		// Token: 0x060008A8 RID: 2216 RVA: 0x0001A20C File Offset: 0x0001840C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void ResetAllowNew()
		{
			this.allowNewIsSet = false;
			this.allowNewSetValue = true;
		}

		/// <summary>Causes a control bound to the <see cref="T:System.Windows.Forms.BindingSource" /> to reread all the items in the list and refresh their displayed values. </summary>
		/// <param name="metadataChanged">
		///       <see langword="true" /> if the data schema has changed; <see langword="false" /> if only values have changed.</param>
		// Token: 0x060008A9 RID: 2217 RVA: 0x0001A21C File Offset: 0x0001841C
		public void ResetBindings(bool metadataChanged)
		{
			if (metadataChanged)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, null));
			}
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		/// <summary>Causes a control bound to the <see cref="T:System.Windows.Forms.BindingSource" /> to reread the currently selected item and refresh its displayed value.</summary>
		// Token: 0x060008AA RID: 2218 RVA: 0x0001A23B File Offset: 0x0001843B
		public void ResetCurrentItem()
		{
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.Position));
		}

		/// <summary>Causes a control bound to the <see cref="T:System.Windows.Forms.BindingSource" /> to reread the item at the specified index, and refresh its displayed value. </summary>
		/// <param name="itemIndex">The zero-based index of the item that has changed.</param>
		// Token: 0x060008AB RID: 2219 RVA: 0x0001A24F File Offset: 0x0001844F
		public void ResetItem(int itemIndex)
		{
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, itemIndex));
		}

		/// <summary>Resumes data binding.</summary>
		// Token: 0x060008AC RID: 2220 RVA: 0x0001A25E File Offset: 0x0001845E
		public void ResumeBinding()
		{
			this.currencyManager.ResumeBinding();
		}

		/// <summary>Suspends data binding to prevent changes from updating the bound data source.</summary>
		// Token: 0x060008AD RID: 2221 RVA: 0x0001A26B File Offset: 0x0001846B
		public void SuspendBinding()
		{
			this.currencyManager.SuspendBinding();
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001A278 File Offset: 0x00018478
		private void ResetList()
		{
			if (this.initializing)
			{
				this.needToSetList = true;
				return;
			}
			this.needToSetList = false;
			object obj = (this.dataSource is Type) ? BindingSource.GetListFromType(this.dataSource as Type) : this.dataSource;
			object list = ListBindingHelper.GetList(obj, this.dataMember);
			this.listExtractedFromEnumerable = false;
			IList list2 = null;
			if (list is IList)
			{
				list2 = (list as IList);
			}
			else
			{
				if (list is IListSource)
				{
					list2 = (list as IListSource).GetList();
				}
				else if (list is IEnumerable)
				{
					list2 = BindingSource.GetListFromEnumerable(list as IEnumerable);
					if (list2 != null)
					{
						this.listExtractedFromEnumerable = true;
					}
				}
				if (list2 == null)
				{
					if (list != null)
					{
						list2 = BindingSource.WrapObjectInBindingList(list);
					}
					else
					{
						Type listItemType = ListBindingHelper.GetListItemType(this.dataSource, this.dataMember);
						list2 = BindingSource.GetListFromType(listItemType);
						if (list2 == null)
						{
							list2 = BindingSource.CreateBindingList(listItemType);
						}
					}
				}
			}
			this.SetList(list2, true, true);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001A358 File Offset: 0x00018558
		private void SetList(IList list, bool metaDataChanged, bool applySortAndFilter)
		{
			if (list == null)
			{
				list = BindingSource.CreateBindingList(this.itemType);
			}
			this.UnwireInnerList();
			this.UnhookItemChangedEventsForOldCurrent();
			IList list2 = ListBindingHelper.GetList(list) as IList;
			if (list2 == null)
			{
				list2 = list;
			}
			this._innerList = list2;
			this.isBindingList = (list2 is IBindingList);
			if (list2 is IRaiseItemChangedEvents)
			{
				this.listRaisesItemChangedEvents = (list2 as IRaiseItemChangedEvents).RaisesItemChangedEvents;
			}
			else
			{
				this.listRaisesItemChangedEvents = this.isBindingList;
			}
			if (metaDataChanged)
			{
				this.itemType = ListBindingHelper.GetListItemType(this.List);
				this.itemShape = ListBindingHelper.GetListItemProperties(this.List);
				this.itemConstructor = this.itemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new Type[0], null);
			}
			this.WireInnerList();
			this.HookItemChangedEventsForNewCurrent();
			this.ResetBindings(metaDataChanged);
			if (applySortAndFilter)
			{
				if (this.Sort != null)
				{
					this.InnerListSort = this.Sort;
				}
				if (this.Filter != null)
				{
					this.InnerListFilter = this.Filter;
				}
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001A450 File Offset: 0x00018650
		private static IList WrapObjectInBindingList(object obj)
		{
			IList list = BindingSource.CreateBindingList(obj.GetType());
			list.Add(obj);
			return list;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001A472 File Offset: 0x00018672
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeAllowNew()
		{
			return this.allowNewIsSet;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001A47C File Offset: 0x0001867C
		private void HookItemChangedEventsForNewCurrent()
		{
			if (!this.listRaisesItemChangedEvents)
			{
				if (this.Position >= 0 && this.Position <= this.Count - 1)
				{
					this.currentItemHookedForItemChange = this.Current;
					this.WirePropertyChangedEvents(this.currentItemHookedForItemChange);
					return;
				}
				this.currentItemHookedForItemChange = null;
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001A4CA File Offset: 0x000186CA
		private void UnhookItemChangedEventsForOldCurrent()
		{
			if (!this.listRaisesItemChangedEvents)
			{
				this.UnwirePropertyChangedEvents(this.currentItemHookedForItemChange);
				this.currentItemHookedForItemChange = null;
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001A4E8 File Offset: 0x000186E8
		private void WireCurrencyManager(CurrencyManager cm)
		{
			if (cm != null)
			{
				cm.PositionChanged += this.CurrencyManager_PositionChanged;
				cm.CurrentChanged += this.CurrencyManager_CurrentChanged;
				cm.CurrentItemChanged += this.CurrencyManager_CurrentItemChanged;
				cm.BindingComplete += this.CurrencyManager_BindingComplete;
				cm.DataError += this.CurrencyManager_DataError;
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001A554 File Offset: 0x00018754
		private void UnwireCurrencyManager(CurrencyManager cm)
		{
			if (cm != null)
			{
				cm.PositionChanged -= this.CurrencyManager_PositionChanged;
				cm.CurrentChanged -= this.CurrencyManager_CurrentChanged;
				cm.CurrentItemChanged -= this.CurrencyManager_CurrentItemChanged;
				cm.BindingComplete -= this.CurrencyManager_BindingComplete;
				cm.DataError -= this.CurrencyManager_DataError;
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0001A5C0 File Offset: 0x000187C0
		private void WireDataSource()
		{
			if (this.dataSource is ICurrencyManagerProvider)
			{
				CurrencyManager currencyManager = (this.dataSource as ICurrencyManagerProvider).CurrencyManager;
				currencyManager.CurrentItemChanged += this.ParentCurrencyManager_CurrentItemChanged;
				currencyManager.MetaDataChanged += this.ParentCurrencyManager_MetaDataChanged;
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001A610 File Offset: 0x00018810
		private void UnwireDataSource()
		{
			if (this.dataSource is ICurrencyManagerProvider)
			{
				CurrencyManager currencyManager = (this.dataSource as ICurrencyManagerProvider).CurrencyManager;
				currencyManager.CurrentItemChanged -= this.ParentCurrencyManager_CurrentItemChanged;
				currencyManager.MetaDataChanged -= this.ParentCurrencyManager_MetaDataChanged;
			}
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001A660 File Offset: 0x00018860
		private void WireInnerList()
		{
			if (this._innerList is IBindingList)
			{
				IBindingList bindingList = this._innerList as IBindingList;
				bindingList.ListChanged += this.InnerList_ListChanged;
			}
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001A698 File Offset: 0x00018898
		private void UnwireInnerList()
		{
			if (this._innerList is IBindingList)
			{
				IBindingList bindingList = this._innerList as IBindingList;
				bindingList.ListChanged -= this.InnerList_ListChanged;
			}
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0001A6D0 File Offset: 0x000188D0
		private void WirePropertyChangedEvents(object item)
		{
			if (item != null && this.itemShape != null)
			{
				for (int i = 0; i < this.itemShape.Count; i++)
				{
					this.itemShape[i].AddValueChanged(item, this.listItemPropertyChangedHandler);
				}
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001A718 File Offset: 0x00018918
		private void UnwirePropertyChangedEvents(object item)
		{
			if (item != null && this.itemShape != null)
			{
				for (int i = 0; i < this.itemShape.Count; i++)
				{
					this.itemShape[i].RemoveValueChanged(item, this.listItemPropertyChangedHandler);
				}
			}
		}

		/// <summary>Signals the <see cref="T:System.Windows.Forms.BindingSource" /> that initialization is starting.</summary>
		// Token: 0x060008BC RID: 2236 RVA: 0x0001A75E File Offset: 0x0001895E
		void ISupportInitialize.BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0001A767 File Offset: 0x00018967
		private void EndInitCore()
		{
			this.initializing = false;
			this.EnsureInnerList();
			this.OnInitialized();
		}

		/// <summary>Signals the <see cref="T:System.Windows.Forms.BindingSource" /> that initialization is complete. </summary>
		// Token: 0x060008BE RID: 2238 RVA: 0x0001A77C File Offset: 0x0001897C
		void ISupportInitialize.EndInit()
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
			{
				supportInitializeNotification.Initialized += this.DataSource_Initialized;
				return;
			}
			this.EndInitCore();
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001A7BC File Offset: 0x000189BC
		private void DataSource_Initialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
			}
			this.EndInitCore();
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.BindingSource" /> is initialized.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate the <see cref="T:System.Windows.Forms.BindingSource" /> is initialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0001A7F0 File Offset: 0x000189F0
		bool ISupportInitializeNotification.IsInitialized
		{
			get
			{
				return !this.initializing;
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.BindingSource" /> is initialized.</summary>
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060008C1 RID: 2241 RVA: 0x0001A7FB File Offset: 0x000189FB
		// (remove) Token: 0x060008C2 RID: 2242 RVA: 0x0001A80E File Offset: 0x00018A0E
		event EventHandler ISupportInitializeNotification.Initialized
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_INITIALIZED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_INITIALIZED, value);
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001A824 File Offset: 0x00018A24
		private void OnInitialized()
		{
			EventHandler eventHandler = (EventHandler)base.Events[BindingSource.EVENT_INITIALIZED];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		/// <summary>Retrieves an enumerator for the <see cref="P:System.Windows.Forms.BindingSource.List" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="P:System.Windows.Forms.BindingSource.List" />. </returns>
		// Token: 0x060008C4 RID: 2244 RVA: 0x0001A856 File Offset: 0x00018A56
		public virtual IEnumerator GetEnumerator()
		{
			return this.List.GetEnumerator();
		}

		/// <summary>Copies the contents of the <see cref="P:System.Windows.Forms.BindingSource.List" /> to the specified array, starting at the specified index value.</summary>
		/// <param name="arr">The destination array.</param>
		/// <param name="index">The index in the destination array at which to start the copy operation.</param>
		// Token: 0x060008C5 RID: 2245 RVA: 0x0001A863 File Offset: 0x00018A63
		public virtual void CopyTo(Array arr, int index)
		{
			this.List.CopyTo(arr, index);
		}

		/// <summary>Gets the total number of items in the underlying list, taking the current <see cref="P:System.Windows.Forms.BindingSource.Filter" /> value into consideration.</summary>
		/// <returns>The total number of filtered items in the underlying list.</returns>
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0001A874 File Offset: 0x00018A74
		[Browsable(false)]
		public virtual int Count
		{
			get
			{
				int result;
				try
				{
					if (this.disposedOrFinalized)
					{
						result = 0;
					}
					else
					{
						if (this.recursionDetectionFlag)
						{
							throw new InvalidOperationException(SR.GetString("BindingSourceRecursionDetected"));
						}
						this.recursionDetectionFlag = true;
						result = this.List.Count;
					}
				}
				finally
				{
					this.recursionDetectionFlag = false;
				}
				return result;
			}
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="true" /> to indicate the list is synchronized; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0001A8D4 File Offset: 0x00018AD4
		[Browsable(false)]
		public virtual bool IsSynchronized
		{
			get
			{
				return this.List.IsSynchronized;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the underlying list.</summary>
		/// <returns>An object that can be used to synchronize access to the underlying list.</returns>
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0001A8E1 File Offset: 0x00018AE1
		[Browsable(false)]
		public virtual object SyncRoot
		{
			get
			{
				return this.List.SyncRoot;
			}
		}

		/// <summary>Adds an existing item to the internal list.</summary>
		/// <param name="value">An <see cref="T:System.Object" /> to be added to the internal list.</param>
		/// <returns>The zero-based index at which <paramref name="value" /> was added to the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property. </returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="value" /> differs in type from the existing items in the underlying list.</exception>
		// Token: 0x060008C9 RID: 2249 RVA: 0x0001A8F0 File Offset: 0x00018AF0
		public virtual int Add(object value)
		{
			if (this.dataSource == null && this.List.Count == 0)
			{
				this.SetList(BindingSource.CreateBindingList((value == null) ? typeof(object) : value.GetType()), true, true);
			}
			if (value != null && !this.itemType.IsAssignableFrom(value.GetType()))
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceItemTypeMismatchOnAdd"));
			}
			if (value == null && this.itemType.IsValueType)
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceItemTypeIsValueType"));
			}
			int num = this.List.Add(value);
			this.OnSimpleListChanged(ListChangedType.ItemAdded, num);
			return num;
		}

		/// <summary>Removes all elements from the list.</summary>
		// Token: 0x060008CA RID: 2250 RVA: 0x0001A992 File Offset: 0x00018B92
		public virtual void Clear()
		{
			this.UnhookItemChangedEventsForOldCurrent();
			this.List.Clear();
			this.OnSimpleListChanged(ListChangedType.Reset, -1);
		}

		/// <summary>Determines whether an object is an item in the list.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property. The value can be <see langword="null" />. </param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="value" /> parameter is found in the <see cref="P:System.Windows.Forms.BindingSource.List" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060008CB RID: 2251 RVA: 0x0001A9AD File Offset: 0x00018BAD
		public virtual bool Contains(object value)
		{
			return this.List.Contains(value);
		}

		/// <summary>Searches for the specified object and returns the index of the first occurrence within the entire list.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property. The value can be <see langword="null" />. </param>
		/// <returns>The zero-based index of the first occurrence of the <paramref name="value" /> parameter; otherwise, -1 if <paramref name="value" /> is not in the list.</returns>
		// Token: 0x060008CC RID: 2252 RVA: 0x0001A9BB File Offset: 0x00018BBB
		public virtual int IndexOf(object value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>Inserts an item into the list at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert. The value can be <see langword="null" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero or greater than <see cref="P:System.Windows.Forms.BindingSource.Count" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The list is read-only or has a fixed size.</exception>
		// Token: 0x060008CD RID: 2253 RVA: 0x0001A9C9 File Offset: 0x00018BC9
		public virtual void Insert(int index, object value)
		{
			this.List.Insert(index, value);
			this.OnSimpleListChanged(ListChangedType.ItemAdded, index);
		}

		/// <summary>Removes the specified item from the list.</summary>
		/// <param name="value">The item to remove from the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property.</param>
		/// <exception cref="T:System.NotSupportedException">The underlying list has a fixed size or is read-only. </exception>
		// Token: 0x060008CE RID: 2254 RVA: 0x0001A9E0 File Offset: 0x00018BE0
		public virtual void Remove(object value)
		{
			int num = ((IList)this).IndexOf(value);
			this.List.Remove(value);
			if (num != -1)
			{
				this.OnSimpleListChanged(ListChangedType.ItemDeleted, num);
			}
		}

		/// <summary>Removes the item at the specified index in the list.</summary>
		/// <param name="index">The zero-based index of the item to remove. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero or greater than the value of the <see cref="P:System.Windows.Forms.BindingSource.Count" /> property.</exception>
		/// <exception cref="T:System.NotSupportedException">The underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property is read-only or has a fixed size.</exception>
		// Token: 0x060008CF RID: 2255 RVA: 0x0001AA10 File Offset: 0x00018C10
		public virtual void RemoveAt(int index)
		{
			object obj = ((IList)this)[index];
			this.List.RemoveAt(index);
			this.OnSimpleListChanged(ListChangedType.ItemDeleted, index);
		}

		/// <summary>Gets or sets the list element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to retrieve.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero or is equal to or greater than <see cref="P:System.Windows.Forms.BindingSource.Count" />.</exception>
		// Token: 0x1700027C RID: 636
		[Browsable(false)]
		public virtual object this[int index]
		{
			get
			{
				return this.List[index];
			}
			set
			{
				this.List[index] = value;
				if (!this.isBindingList)
				{
					this.OnSimpleListChanged(ListChangedType.ItemChanged, index);
				}
			}
		}

		/// <summary>Gets a value indicating whether the underlying list has a fixed size.</summary>
		/// <returns>
		///     <see langword="true" /> if the underlying list has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0001AA66 File Offset: 0x00018C66
		[Browsable(false)]
		public virtual bool IsFixedSize
		{
			get
			{
				return this.List.IsFixedSize;
			}
		}

		/// <summary>Gets a value indicating whether the underlying list is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the list is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0001AA73 File Offset: 0x00018C73
		[Browsable(false)]
		public virtual bool IsReadOnly
		{
			get
			{
				return this.List.IsReadOnly;
			}
		}

		/// <summary>Gets the name of the list supplying data for the binding.</summary>
		/// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to find in the list as bindable.</param>
		/// <returns>The name of the list supplying the data for binding.</returns>
		// Token: 0x060008D4 RID: 2260 RVA: 0x0001AA80 File Offset: 0x00018C80
		public virtual string GetListName(PropertyDescriptor[] listAccessors)
		{
			return ListBindingHelper.GetListName(this.List, listAccessors);
		}

		/// <summary>Retrieves an array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects representing the bindable properties of the data source list type.</summary>
		/// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to find in the list as bindable.</param>
		/// <returns>An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects that represents the properties on this list type used to bind data.</returns>
		// Token: 0x060008D5 RID: 2261 RVA: 0x0001AA90 File Offset: 0x00018C90
		public virtual PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			object list = ListBindingHelper.GetList(this.dataSource);
			if (list is ITypedList && !string.IsNullOrEmpty(this.dataMember))
			{
				return ListBindingHelper.GetListItemProperties(list, this.dataMember, listAccessors);
			}
			return ListBindingHelper.GetListItemProperties(this.List, listAccessors);
		}

		/// <summary>Adds a new item to the underlying list.</summary>
		/// <returns>The <see cref="T:System.Object" /> that was created and added to the list.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.BindingSource.AllowNew" /> property is set to <see langword="false" />. -or-A public default constructor could not be found for the current item type.</exception>
		// Token: 0x060008D6 RID: 2262 RVA: 0x0001AAD8 File Offset: 0x00018CD8
		public virtual object AddNew()
		{
			if (!this.AllowNewInternal(false))
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceBindingListWrapperAddToReadOnlyList"));
			}
			if (!this.AllowNewInternal(true))
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceBindingListWrapperNeedToSetAllowNew", new object[]
				{
					(this.itemType == null) ? "(null)" : this.itemType.FullName
				}));
			}
			int num = this.addNewPos;
			this.EndEdit();
			if (num != -1)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, num));
			}
			AddingNewEventArgs addingNewEventArgs = new AddingNewEventArgs();
			int count = this.List.Count;
			this.OnAddingNew(addingNewEventArgs);
			object obj = addingNewEventArgs.NewObject;
			if (obj == null)
			{
				if (this.isBindingList)
				{
					obj = (this.List as IBindingList).AddNew();
					this.Position = this.Count - 1;
					return obj;
				}
				if (this.itemConstructor == null)
				{
					throw new InvalidOperationException(SR.GetString("BindingSourceBindingListWrapperNeedAParameterlessConstructor", new object[]
					{
						(this.itemType == null) ? "(null)" : this.itemType.FullName
					}));
				}
				obj = this.itemConstructor.Invoke(null);
			}
			if (this.List.Count > count)
			{
				this.addNewPos = this.Position;
			}
			else
			{
				this.addNewPos = this.Add(obj);
				this.Position = this.addNewPos;
			}
			return obj;
		}

		/// <summary>Gets a value indicating whether items in the underlying list can be edited.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate list items can be edited; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001AC34 File Offset: 0x00018E34
		[Browsable(false)]
		public virtual bool AllowEdit
		{
			get
			{
				if (this.isBindingList)
				{
					return ((IBindingList)this.List).AllowEdit;
				}
				return !this.List.IsReadOnly;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="M:System.Windows.Forms.BindingSource.AddNew" /> method can be used to add items to the list.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="M:System.Windows.Forms.BindingSource.AddNew" /> can be used to add items to the list; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">This property is set to <see langword="true" /> when the underlying list represented by the <see cref="P:System.Windows.Forms.BindingSource.List" /> property has a fixed size or is read-only.</exception>
		/// <exception cref="T:System.MissingMethodException">The property is set to <see langword="true" /> and the <see cref="E:System.Windows.Forms.BindingSource.AddingNew" /> event is not handled when the underlying list type does not have a default constructor.</exception>
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0001AC5D File Offset: 0x00018E5D
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x0001AC68 File Offset: 0x00018E68
		[SRCategory("CatBehavior")]
		[SRDescription("BindingSourceAllowNewDescr")]
		public virtual bool AllowNew
		{
			get
			{
				return this.AllowNewInternal(true);
			}
			set
			{
				if (this.allowNewIsSet && value == this.allowNewSetValue)
				{
					return;
				}
				if (value && !this.isBindingList && !this.IsListWriteable(false))
				{
					throw new InvalidOperationException(SR.GetString("NoAllowNewOnReadOnlyList"));
				}
				this.allowNewIsSet = true;
				this.allowNewSetValue = value;
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
			}
		}

		/// <summary>Gets a value indicating whether items can be removed from the underlying list.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate list items can be removed from the list; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0001ACC6 File Offset: 0x00018EC6
		[Browsable(false)]
		public virtual bool AllowRemove
		{
			get
			{
				if (this.isBindingList)
				{
					return ((IBindingList)this.List).AllowRemove;
				}
				return !this.List.IsReadOnly && !this.List.IsFixedSize;
			}
		}

		/// <summary>Gets a value indicating whether the data source supports change notification.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0000E214 File Offset: 0x0000C414
		[Browsable(false)]
		public virtual bool SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the data source supports searching with the <see cref="M:System.Windows.Forms.BindingSource.Find(System.ComponentModel.PropertyDescriptor,System.Object)" /> method.</summary>
		/// <returns>
		///     <see langword="true" /> if the list is a <see cref="T:System.ComponentModel.IBindingList" /> and supports the searching with the <see cref="Overload:System.Windows.Forms.BindingSource.Find" /> method; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0001ACFE File Offset: 0x00018EFE
		[Browsable(false)]
		public virtual bool SupportsSearching
		{
			get
			{
				return this.isBindingList && ((IBindingList)this.List).SupportsSearching;
			}
		}

		/// <summary>Gets a value indicating whether the data source supports sorting.</summary>
		/// <returns>
		///     <see langword="true" /> if the data source is an <see cref="T:System.ComponentModel.IBindingList" /> and supports sorting; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001AD1A File Offset: 0x00018F1A
		[Browsable(false)]
		public virtual bool SupportsSorting
		{
			get
			{
				return this.isBindingList && ((IBindingList)this.List).SupportsSorting;
			}
		}

		/// <summary>Gets a value indicating whether the items in the underlying list are sorted. </summary>
		/// <returns>
		///     <see langword="true" /> if the list is an <see cref="T:System.ComponentModel.IBindingList" /> and is sorted; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0001AD36 File Offset: 0x00018F36
		[Browsable(false)]
		public virtual bool IsSorted
		{
			get
			{
				return this.isBindingList && ((IBindingList)this.List).IsSorted;
			}
		}

		/// <summary>Gets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is being used for sorting the list.</summary>
		/// <returns>If the list is an <see cref="T:System.ComponentModel.IBindingList" />, the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is being used for sorting; otherwise, <see langword="null" />.</returns>
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0001AD52 File Offset: 0x00018F52
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual PropertyDescriptor SortProperty
		{
			get
			{
				if (this.isBindingList)
				{
					return ((IBindingList)this.List).SortProperty;
				}
				return null;
			}
		}

		/// <summary>Gets the direction the items in the list are sorted.</summary>
		/// <returns>One of the <see cref="T:System.ComponentModel.ListSortDirection" /> values indicating the direction the list is sorted.</returns>
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0001AD6E File Offset: 0x00018F6E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual ListSortDirection SortDirection
		{
			get
			{
				if (this.isBindingList)
				{
					return ((IBindingList)this.List).SortDirection;
				}
				return ListSortDirection.Ascending;
			}
		}

		/// <summary>Adds the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to the indexes used for searching.</summary>
		/// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to add to the indexes used for searching. </param>
		/// <exception cref="T:System.NotSupportedException">The underlying list is not an <see cref="T:System.ComponentModel.IBindingList" />.</exception>
		// Token: 0x060008E1 RID: 2273 RVA: 0x0001AD8A File Offset: 0x00018F8A
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			if (this.isBindingList)
			{
				((IBindingList)this.List).AddIndex(property);
				return;
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingList"));
		}

		/// <summary>Sorts the data source using the specified property descriptor and sort direction.</summary>
		/// <param name="property">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that describes the property by which to sort the data source.</param>
		/// <param name="sort">A <see cref="T:System.ComponentModel.ListSortDirection" /> indicating how the list should be sorted.</param>
		/// <exception cref="T:System.NotSupportedException">The data source is not an <see cref="T:System.ComponentModel.IBindingList" />.</exception>
		// Token: 0x060008E2 RID: 2274 RVA: 0x0001ADB5 File Offset: 0x00018FB5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ApplySort(PropertyDescriptor property, ListSortDirection sort)
		{
			if (this.isBindingList)
			{
				((IBindingList)this.List).ApplySort(property, sort);
				return;
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingList"));
		}

		/// <summary>Searches for the index of the item that has the given property descriptor.</summary>
		/// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to search for. </param>
		/// <param name="key">The value of <paramref name="prop" /> to match. </param>
		/// <returns>The zero-based index of the item that has the given value for <see cref="T:System.ComponentModel.PropertyDescriptor" />.</returns>
		/// <exception cref="T:System.NotSupportedException">The underlying list is not of type <see cref="T:System.ComponentModel.IBindingList" />.</exception>
		// Token: 0x060008E3 RID: 2275 RVA: 0x0001ADE1 File Offset: 0x00018FE1
		public virtual int Find(PropertyDescriptor prop, object key)
		{
			if (this.isBindingList)
			{
				return ((IBindingList)this.List).Find(prop, key);
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingList"));
		}

		/// <summary>Removes the <see cref="T:System.ComponentModel.PropertyDescriptor" /> from the indexes used for searching.</summary>
		/// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to remove from the indexes used for searching.  </param>
		// Token: 0x060008E4 RID: 2276 RVA: 0x0001AE0D File Offset: 0x0001900D
		void IBindingList.RemoveIndex(PropertyDescriptor prop)
		{
			if (this.isBindingList)
			{
				((IBindingList)this.List).RemoveIndex(prop);
				return;
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingList"));
		}

		/// <summary>Removes the sort associated with the <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The underlying list does not support sorting.</exception>
		// Token: 0x060008E5 RID: 2277 RVA: 0x0001AE38 File Offset: 0x00019038
		public virtual void RemoveSort()
		{
			this.sort = null;
			if (this.isBindingList)
			{
				((IBindingList)this.List).RemoveSort();
			}
		}

		/// <summary>Sorts the data source with the specified sort descriptions.</summary>
		/// <param name="sorts">A <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> containing the sort descriptions to apply to the data source.</param>
		/// <exception cref="T:System.NotSupportedException">The data source is not an <see cref="T:System.ComponentModel.IBindingListView" />.</exception>
		// Token: 0x060008E6 RID: 2278 RVA: 0x0001AE5C File Offset: 0x0001905C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ApplySort(ListSortDescriptionCollection sorts)
		{
			IBindingListView bindingListView = this.List as IBindingListView;
			if (bindingListView != null)
			{
				bindingListView.ApplySort(sorts);
				return;
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingListView"));
		}

		/// <summary>Gets the collection of sort descriptions applied to the data source.</summary>
		/// <returns>If the data source is an <see cref="T:System.ComponentModel.IBindingListView" />, a <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> that contains the sort descriptions applied to the list; otherwise, <see langword="null" />.</returns>
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0001AE90 File Offset: 0x00019090
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual ListSortDescriptionCollection SortDescriptions
		{
			get
			{
				IBindingListView bindingListView = this.List as IBindingListView;
				if (bindingListView != null)
				{
					return bindingListView.SortDescriptions;
				}
				return null;
			}
		}

		/// <summary>Gets or sets the expression used to filter which rows are viewed.</summary>
		/// <returns>A string that specifies how rows are to be filtered. The default is <see langword="null" />.</returns>
		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0001AEB4 File Offset: 0x000190B4
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x0001AEBC File Offset: 0x000190BC
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[SRDescription("BindingSourceFilterDescr")]
		public virtual string Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
				this.InnerListFilter = value;
			}
		}

		/// <summary>Removes the filter associated with the <see cref="T:System.Windows.Forms.BindingSource" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The underlying list does not support filtering.</exception>
		// Token: 0x060008EA RID: 2282 RVA: 0x0001AECC File Offset: 0x000190CC
		public virtual void RemoveFilter()
		{
			this.filter = null;
			IBindingListView bindingListView = this.List as IBindingListView;
			if (bindingListView != null)
			{
				bindingListView.RemoveFilter();
			}
		}

		/// <summary>Gets a value indicating whether the data source supports multi-column sorting.</summary>
		/// <returns>
		///     <see langword="true" /> if the list is an <see cref="T:System.ComponentModel.IBindingListView" /> and supports multi-column sorting; otherwise, <see langword="false" />. </returns>
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0001AEF8 File Offset: 0x000190F8
		[Browsable(false)]
		public virtual bool SupportsAdvancedSorting
		{
			get
			{
				IBindingListView bindingListView = this.List as IBindingListView;
				return bindingListView != null && bindingListView.SupportsAdvancedSorting;
			}
		}

		/// <summary>Gets a value indicating whether the data source supports filtering.</summary>
		/// <returns>
		///     <see langword="true" /> if the list is an <see cref="T:System.ComponentModel.IBindingListView" /> and supports filtering; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0001AF1C File Offset: 0x0001911C
		[Browsable(false)]
		public virtual bool SupportsFiltering
		{
			get
			{
				IBindingListView bindingListView = this.List as IBindingListView;
				return bindingListView != null && bindingListView.SupportsFiltering;
			}
		}

		/// <summary>Discards a pending new item from the collection.</summary>
		/// <param name="position">The index of the item that was added to the collection. </param>
		// Token: 0x060008ED RID: 2285 RVA: 0x0001AF40 File Offset: 0x00019140
		void ICancelAddNew.CancelNew(int position)
		{
			if (this.addNewPos >= 0 && this.addNewPos == position)
			{
				this.RemoveAt(this.addNewPos);
				this.addNewPos = -1;
				return;
			}
			ICancelAddNew cancelAddNew = this.List as ICancelAddNew;
			if (cancelAddNew != null)
			{
				cancelAddNew.CancelNew(position);
			}
		}

		/// <summary>Commits a pending new item to the collection.</summary>
		/// <param name="position">The index of the item that was added to the collection. </param>
		// Token: 0x060008EE RID: 2286 RVA: 0x0001AF8C File Offset: 0x0001918C
		void ICancelAddNew.EndNew(int position)
		{
			if (this.addNewPos >= 0 && this.addNewPos == position)
			{
				this.addNewPos = -1;
				return;
			}
			ICancelAddNew cancelAddNew = this.List as ICancelAddNew;
			if (cancelAddNew != null)
			{
				cancelAddNew.EndNew(position);
			}
		}

		// Token: 0x04000623 RID: 1571
		private static readonly object EVENT_ADDINGNEW = new object();

		// Token: 0x04000624 RID: 1572
		private static readonly object EVENT_BINDINGCOMPLETE = new object();

		// Token: 0x04000625 RID: 1573
		private static readonly object EVENT_CURRENTCHANGED = new object();

		// Token: 0x04000626 RID: 1574
		private static readonly object EVENT_CURRENTITEMCHANGED = new object();

		// Token: 0x04000627 RID: 1575
		private static readonly object EVENT_DATAERROR = new object();

		// Token: 0x04000628 RID: 1576
		private static readonly object EVENT_DATAMEMBERCHANGED = new object();

		// Token: 0x04000629 RID: 1577
		private static readonly object EVENT_DATASOURCECHANGED = new object();

		// Token: 0x0400062A RID: 1578
		private static readonly object EVENT_LISTCHANGED = new object();

		// Token: 0x0400062B RID: 1579
		private static readonly object EVENT_POSITIONCHANGED = new object();

		// Token: 0x0400062C RID: 1580
		private static readonly object EVENT_INITIALIZED = new object();

		// Token: 0x0400062D RID: 1581
		private object dataSource;

		// Token: 0x0400062E RID: 1582
		private string dataMember = string.Empty;

		// Token: 0x0400062F RID: 1583
		private string sort;

		// Token: 0x04000630 RID: 1584
		private string filter;

		// Token: 0x04000631 RID: 1585
		private CurrencyManager currencyManager;

		// Token: 0x04000632 RID: 1586
		private bool raiseListChangedEvents = true;

		// Token: 0x04000633 RID: 1587
		private bool parentsCurrentItemChanging;

		// Token: 0x04000634 RID: 1588
		private bool disposedOrFinalized;

		// Token: 0x04000635 RID: 1589
		private IList _innerList;

		// Token: 0x04000636 RID: 1590
		private bool isBindingList;

		// Token: 0x04000637 RID: 1591
		private bool listRaisesItemChangedEvents;

		// Token: 0x04000638 RID: 1592
		private bool listExtractedFromEnumerable;

		// Token: 0x04000639 RID: 1593
		private Type itemType;

		// Token: 0x0400063A RID: 1594
		private ConstructorInfo itemConstructor;

		// Token: 0x0400063B RID: 1595
		private PropertyDescriptorCollection itemShape;

		// Token: 0x0400063C RID: 1596
		private Dictionary<string, BindingSource> relatedBindingSources;

		// Token: 0x0400063D RID: 1597
		private bool allowNewIsSet;

		// Token: 0x0400063E RID: 1598
		private bool allowNewSetValue = true;

		// Token: 0x0400063F RID: 1599
		private object currentItemHookedForItemChange;

		// Token: 0x04000640 RID: 1600
		private object lastCurrentItem;

		// Token: 0x04000641 RID: 1601
		private EventHandler listItemPropertyChangedHandler;

		// Token: 0x04000642 RID: 1602
		private int addNewPos = -1;

		// Token: 0x04000643 RID: 1603
		private bool initializing;

		// Token: 0x04000644 RID: 1604
		private bool needToSetList;

		// Token: 0x04000645 RID: 1605
		private bool recursionDetectionFlag;

		// Token: 0x04000646 RID: 1606
		private bool innerListChanging;

		// Token: 0x04000647 RID: 1607
		private bool endingEdit;
	}
}
