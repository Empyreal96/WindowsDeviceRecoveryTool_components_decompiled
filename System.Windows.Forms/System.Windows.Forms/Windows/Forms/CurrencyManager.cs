using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Manages a list of <see cref="T:System.Windows.Forms.Binding" /> objects.</summary>
	// Token: 0x02000165 RID: 357
	public class CurrencyManager : BindingManagerBase
	{
		/// <summary>Occurs when the current item has been altered.</summary>
		// Token: 0x140000AF RID: 175
		// (add) Token: 0x0600104E RID: 4174 RVA: 0x000393E5 File Offset: 0x000375E5
		// (remove) Token: 0x0600104F RID: 4175 RVA: 0x000393FE File Offset: 0x000375FE
		[SRCategory("CatData")]
		public event ItemChangedEventHandler ItemChanged
		{
			add
			{
				this.onItemChanged = (ItemChangedEventHandler)Delegate.Combine(this.onItemChanged, value);
			}
			remove
			{
				this.onItemChanged = (ItemChangedEventHandler)Delegate.Remove(this.onItemChanged, value);
			}
		}

		/// <summary>Occurs when the list changes or an item in the list changes.</summary>
		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06001050 RID: 4176 RVA: 0x00039417 File Offset: 0x00037617
		// (remove) Token: 0x06001051 RID: 4177 RVA: 0x00039430 File Offset: 0x00037630
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Combine(this.onListChanged, value);
			}
			remove
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Remove(this.onListChanged, value);
			}
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00039449 File Offset: 0x00037649
		internal CurrencyManager(object dataSource)
		{
			this.SetDataSource(dataSource);
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0003947C File Offset: 0x0003767C
		internal bool AllowAdd
		{
			get
			{
				if (this.list is IBindingList)
				{
					return ((IBindingList)this.list).AllowNew;
				}
				return this.list != null && !this.list.IsReadOnly && !this.list.IsFixedSize;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x000394CE File Offset: 0x000376CE
		internal bool AllowEdit
		{
			get
			{
				if (this.list is IBindingList)
				{
					return ((IBindingList)this.list).AllowEdit;
				}
				return this.list != null && !this.list.IsReadOnly;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x00039508 File Offset: 0x00037708
		internal bool AllowRemove
		{
			get
			{
				if (this.list is IBindingList)
				{
					return ((IBindingList)this.list).AllowRemove;
				}
				return this.list != null && !this.list.IsReadOnly && !this.list.IsFixedSize;
			}
		}

		/// <summary>Gets the number of items in the list.</summary>
		/// <returns>The number of items in the list.</returns>
		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0003955A File Offset: 0x0003775A
		public override int Count
		{
			get
			{
				if (this.list == null)
				{
					return 0;
				}
				return this.list.Count;
			}
		}

		/// <summary>Gets the current item in the list.</summary>
		/// <returns>A list item of type <see cref="T:System.Object" />.</returns>
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x00039571 File Offset: 0x00037771
		public override object Current
		{
			get
			{
				return this[this.Position];
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0003957F File Offset: 0x0003777F
		internal override Type BindType
		{
			get
			{
				return ListBindingHelper.GetListItemType(this.List);
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0003958C File Offset: 0x0003778C
		internal override object DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00039594 File Offset: 0x00037794
		internal override void SetDataSource(object dataSource)
		{
			if (this.dataSource == dataSource)
			{
				return;
			}
			this.Release();
			this.dataSource = dataSource;
			this.list = null;
			this.finalType = null;
			object obj = dataSource;
			if (obj is Array)
			{
				this.finalType = obj.GetType();
				obj = (Array)obj;
			}
			if (obj is IListSource)
			{
				obj = ((IListSource)obj).GetList();
			}
			if (obj is IList)
			{
				if (this.finalType == null)
				{
					this.finalType = obj.GetType();
				}
				this.list = (IList)obj;
				this.WireEvents(this.list);
				if (this.list.Count > 0)
				{
					this.listposition = 0;
				}
				else
				{
					this.listposition = -1;
				}
				this.OnItemChanged(this.resetEvent);
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1, -1));
				this.UpdateIsBinding();
				return;
			}
			if (obj == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			throw new ArgumentException(SR.GetString("ListManagerSetDataSource", new object[]
			{
				obj.GetType().FullName
			}), "dataSource");
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x000396A9 File Offset: 0x000378A9
		internal override bool IsBinding
		{
			get
			{
				return this.bound;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x000396B1 File Offset: 0x000378B1
		internal bool ShouldBind
		{
			get
			{
				return this.shouldBind;
			}
		}

		/// <summary>Gets the list for this <see cref="T:System.Windows.Forms.CurrencyManager" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> that contains the list.</returns>
		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x000396B9 File Offset: 0x000378B9
		public IList List
		{
			get
			{
				return this.list;
			}
		}

		/// <summary>Gets or sets the position you are at within the list.</summary>
		/// <returns>A number between 0 and <see cref="P:System.Windows.Forms.CurrencyManager.Count" /> minus 1.</returns>
		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x000396C1 File Offset: 0x000378C1
		// (set) Token: 0x0600105F RID: 4191 RVA: 0x000396CC File Offset: 0x000378CC
		public override int Position
		{
			get
			{
				return this.listposition;
			}
			set
			{
				if (this.listposition == -1)
				{
					return;
				}
				if (value < 0)
				{
					value = 0;
				}
				int count = this.list.Count;
				if (value >= count)
				{
					value = count - 1;
				}
				this.ChangeRecordState(value, this.listposition != value, true, true, false);
			}
		}

		// Token: 0x17000401 RID: 1025
		internal object this[int index]
		{
			get
			{
				if (index < 0 || index >= this.list.Count)
				{
					throw new IndexOutOfRangeException(SR.GetString("ListManagerNoValue", new object[]
					{
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				return this.list[index];
			}
			set
			{
				if (index < 0 || index >= this.list.Count)
				{
					throw new IndexOutOfRangeException(SR.GetString("ListManagerNoValue", new object[]
					{
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.list[index] = value;
			}
		}

		/// <summary>Adds a new item to the underlying list.</summary>
		/// <exception cref="T:System.NotSupportedException">The underlying data source does not implement <see cref="T:System.ComponentModel.IBindingList" />, or the data source has thrown an exception because the user has attempted to add a row to a read-only or fixed-size <see cref="T:System.Data.DataView" />. </exception>
		// Token: 0x06001062 RID: 4194 RVA: 0x000397BC File Offset: 0x000379BC
		public override void AddNew()
		{
			IBindingList bindingList = this.list as IBindingList;
			if (bindingList != null)
			{
				bindingList.AddNew();
				this.ChangeRecordState(this.list.Count - 1, this.Position != this.list.Count - 1, this.Position != this.list.Count - 1, true, true);
				return;
			}
			throw new NotSupportedException(SR.GetString("CurrencyManagerCantAddNew"));
		}

		/// <summary>Cancels the current edit operation.</summary>
		// Token: 0x06001063 RID: 4195 RVA: 0x00039838 File Offset: 0x00037A38
		public override void CancelCurrentEdit()
		{
			if (this.Count > 0)
			{
				object obj = (this.Position >= 0 && this.Position < this.list.Count) ? this.list[this.Position] : null;
				IEditableObject editableObject = obj as IEditableObject;
				if (editableObject != null)
				{
					editableObject.CancelEdit();
				}
				ICancelAddNew cancelAddNew = this.list as ICancelAddNew;
				if (cancelAddNew != null)
				{
					cancelAddNew.CancelNew(this.Position);
				}
				this.OnItemChanged(new ItemChangedEventArgs(this.Position));
				if (this.Position != -1)
				{
					this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.Position));
				}
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000398DC File Offset: 0x00037ADC
		private void ChangeRecordState(int newPosition, bool validating, bool endCurrentEdit, bool firePositionChange, bool pullData)
		{
			if (newPosition == -1 && this.list.Count == 0)
			{
				if (this.listposition != -1)
				{
					this.listposition = -1;
					this.OnPositionChanged(EventArgs.Empty);
				}
				return;
			}
			if ((newPosition < 0 || newPosition >= this.Count) && this.IsBinding)
			{
				throw new IndexOutOfRangeException(SR.GetString("ListManagerBadPosition"));
			}
			int num = this.listposition;
			if (endCurrentEdit)
			{
				this.inChangeRecordState = true;
				try
				{
					this.EndCurrentEdit();
				}
				finally
				{
					this.inChangeRecordState = false;
				}
			}
			if (validating && pullData)
			{
				this.CurrencyManager_PullData();
			}
			this.listposition = Math.Min(newPosition, this.Count - 1);
			if (validating)
			{
				this.OnCurrentChanged(EventArgs.Empty);
			}
			bool flag = num != this.listposition;
			if (flag && firePositionChange)
			{
				this.OnPositionChanged(EventArgs.Empty);
			}
		}

		/// <summary>Throws an exception if there is no list, or the list is empty.</summary>
		/// <exception cref="T:System.Exception">There is no list, or the list is empty. </exception>
		// Token: 0x06001065 RID: 4197 RVA: 0x000399BC File Offset: 0x00037BBC
		protected void CheckEmpty()
		{
			if (this.dataSource == null || this.list == null || this.list.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("ListManagerEmptyList"));
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000399EC File Offset: 0x00037BEC
		private bool CurrencyManager_PushData()
		{
			if (this.pullingData)
			{
				return false;
			}
			int num = this.listposition;
			if (this.lastGoodKnownRow == -1)
			{
				try
				{
					base.PushData();
				}
				catch (Exception e)
				{
					base.OnDataError(e);
					this.FindGoodRow();
				}
				this.lastGoodKnownRow = this.listposition;
			}
			else
			{
				try
				{
					base.PushData();
				}
				catch (Exception e2)
				{
					base.OnDataError(e2);
					this.listposition = this.lastGoodKnownRow;
					base.PushData();
				}
				this.lastGoodKnownRow = this.listposition;
			}
			return num != this.listposition;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00039A94 File Offset: 0x00037C94
		private bool CurrencyManager_PullData()
		{
			bool result = true;
			this.pullingData = true;
			try
			{
				base.PullData(out result);
			}
			finally
			{
				this.pullingData = false;
			}
			return result;
		}

		/// <summary>Removes the item at the specified index.</summary>
		/// <param name="index">The index of the item to remove from the list. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">There is no row at the specified <paramref name="index" />. </exception>
		// Token: 0x06001068 RID: 4200 RVA: 0x00039AD0 File Offset: 0x00037CD0
		public override void RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		/// <summary>Ends the current edit operation.</summary>
		// Token: 0x06001069 RID: 4201 RVA: 0x00039AE0 File Offset: 0x00037CE0
		public override void EndCurrentEdit()
		{
			if (this.Count > 0)
			{
				bool flag = this.CurrencyManager_PullData();
				if (flag)
				{
					object obj = (this.Position >= 0 && this.Position < this.list.Count) ? this.list[this.Position] : null;
					IEditableObject editableObject = obj as IEditableObject;
					if (editableObject != null)
					{
						editableObject.EndEdit();
					}
					ICancelAddNew cancelAddNew = this.list as ICancelAddNew;
					if (cancelAddNew != null)
					{
						cancelAddNew.EndNew(this.Position);
					}
				}
			}
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00039B5C File Offset: 0x00037D5C
		private void FindGoodRow()
		{
			int count = this.list.Count;
			int i = 0;
			while (i < count)
			{
				this.listposition = i;
				try
				{
					base.PushData();
				}
				catch (Exception e)
				{
					base.OnDataError(e);
					goto IL_31;
				}
				goto IL_29;
				IL_31:
				i++;
				continue;
				IL_29:
				this.listposition = i;
				return;
			}
			this.SuspendBinding();
			throw new InvalidOperationException(SR.GetString("DataBindingPushDataException"));
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00039BC8 File Offset: 0x00037DC8
		internal void SetSort(PropertyDescriptor property, ListSortDirection sortDirection)
		{
			if (this.list is IBindingList && ((IBindingList)this.list).SupportsSorting)
			{
				((IBindingList)this.list).ApplySort(property, sortDirection);
			}
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x00039BFB File Offset: 0x00037DFB
		internal PropertyDescriptor GetSortProperty()
		{
			if (this.list is IBindingList && ((IBindingList)this.list).SupportsSorting)
			{
				return ((IBindingList)this.list).SortProperty;
			}
			return null;
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00039C2E File Offset: 0x00037E2E
		internal ListSortDirection GetSortDirection()
		{
			if (this.list is IBindingList && ((IBindingList)this.list).SupportsSorting)
			{
				return ((IBindingList)this.list).SortDirection;
			}
			return ListSortDirection.Ascending;
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00039C64 File Offset: 0x00037E64
		internal int Find(PropertyDescriptor property, object key, bool keepIndex)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (property != null && this.list is IBindingList && ((IBindingList)this.list).SupportsSearching)
			{
				return ((IBindingList)this.list).Find(property, key);
			}
			if (property != null)
			{
				for (int i = 0; i < this.list.Count; i++)
				{
					object value = property.GetValue(this.list[i]);
					if (key.Equals(value))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00039CEC File Offset: 0x00037EEC
		internal override string GetListName()
		{
			if (this.list is ITypedList)
			{
				return ((ITypedList)this.list).GetListName(null);
			}
			return this.finalType.Name;
		}

		/// <summary>Gets the name of the list supplying the data for the binding using the specified set of bound properties.</summary>
		/// <param name="listAccessors">An <see cref="T:System.Collections.ArrayList" /> of properties to be found in the data source.</param>
		/// <returns>If successful, a <see cref="T:System.String" /> containing name of the list supplying the data for the binding; otherwise, an <see cref="F:System.String.Empty" /> string.</returns>
		// Token: 0x06001070 RID: 4208 RVA: 0x00039D18 File Offset: 0x00037F18
		protected internal override string GetListName(ArrayList listAccessors)
		{
			if (this.list is ITypedList)
			{
				PropertyDescriptor[] array = new PropertyDescriptor[listAccessors.Count];
				listAccessors.CopyTo(array, 0);
				return ((ITypedList)this.list).GetListName(array);
			}
			return "";
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00039D5D File Offset: 0x00037F5D
		internal override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return ListBindingHelper.GetListItemProperties(this.list, listAccessors);
		}

		/// <summary>Gets the property descriptor collection for the underlying list.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> for the list.</returns>
		// Token: 0x06001072 RID: 4210 RVA: 0x00017BB3 File Offset: 0x00015DB3
		public override PropertyDescriptorCollection GetItemProperties()
		{
			return this.GetItemProperties(null);
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00039D6C File Offset: 0x00037F6C
		private void List_ListChanged(object sender, ListChangedEventArgs e)
		{
			ListChangedEventArgs listChangedEventArgs;
			if (e.ListChangedType == ListChangedType.ItemMoved && e.OldIndex < 0)
			{
				listChangedEventArgs = new ListChangedEventArgs(ListChangedType.ItemAdded, e.NewIndex, e.OldIndex);
			}
			else if (e.ListChangedType == ListChangedType.ItemMoved && e.NewIndex < 0)
			{
				listChangedEventArgs = new ListChangedEventArgs(ListChangedType.ItemDeleted, e.OldIndex, e.NewIndex);
			}
			else
			{
				listChangedEventArgs = e;
			}
			int num = this.listposition;
			this.UpdateLastGoodKnownRow(listChangedEventArgs);
			this.UpdateIsBinding();
			if (this.list.Count == 0)
			{
				this.listposition = -1;
				if (num != -1)
				{
					this.OnPositionChanged(EventArgs.Empty);
					this.OnCurrentChanged(EventArgs.Empty);
				}
				if (listChangedEventArgs.ListChangedType == ListChangedType.Reset && e.NewIndex == -1)
				{
					this.OnItemChanged(this.resetEvent);
				}
				if (listChangedEventArgs.ListChangedType == ListChangedType.ItemDeleted)
				{
					this.OnItemChanged(this.resetEvent);
				}
				if (e.ListChangedType == ListChangedType.PropertyDescriptorAdded || e.ListChangedType == ListChangedType.PropertyDescriptorDeleted || e.ListChangedType == ListChangedType.PropertyDescriptorChanged)
				{
					this.OnMetaDataChanged(EventArgs.Empty);
				}
				this.OnListChanged(listChangedEventArgs);
				return;
			}
			this.suspendPushDataInCurrentChanged = true;
			try
			{
				switch (listChangedEventArgs.ListChangedType)
				{
				case ListChangedType.Reset:
					if (this.listposition == -1 && this.list.Count > 0)
					{
						this.ChangeRecordState(0, true, false, true, false);
					}
					else
					{
						this.ChangeRecordState(Math.Min(this.listposition, this.list.Count - 1), true, false, true, false);
					}
					this.UpdateIsBinding(false);
					this.OnItemChanged(this.resetEvent);
					break;
				case ListChangedType.ItemAdded:
					if (listChangedEventArgs.NewIndex <= this.listposition && this.listposition < this.list.Count - 1)
					{
						this.ChangeRecordState(this.listposition + 1, true, true, this.listposition != this.list.Count - 2, false);
						this.UpdateIsBinding();
						this.OnItemChanged(this.resetEvent);
						if (this.listposition == this.list.Count - 1)
						{
							this.OnPositionChanged(EventArgs.Empty);
						}
					}
					else
					{
						if (listChangedEventArgs.NewIndex == this.listposition && this.listposition == this.list.Count - 1 && this.listposition != -1)
						{
							this.OnCurrentItemChanged(EventArgs.Empty);
						}
						if (this.listposition == -1)
						{
							this.ChangeRecordState(0, false, false, true, false);
						}
						this.UpdateIsBinding();
						this.OnItemChanged(this.resetEvent);
					}
					break;
				case ListChangedType.ItemDeleted:
					if (listChangedEventArgs.NewIndex == this.listposition)
					{
						this.ChangeRecordState(Math.Min(this.listposition, this.Count - 1), true, false, true, false);
						this.OnItemChanged(this.resetEvent);
					}
					else if (listChangedEventArgs.NewIndex < this.listposition)
					{
						this.ChangeRecordState(this.listposition - 1, true, false, true, false);
						this.OnItemChanged(this.resetEvent);
					}
					else
					{
						this.OnItemChanged(this.resetEvent);
					}
					break;
				case ListChangedType.ItemMoved:
					if (listChangedEventArgs.OldIndex == this.listposition)
					{
						this.ChangeRecordState(listChangedEventArgs.NewIndex, true, this.Position > -1 && this.Position < this.list.Count, true, false);
					}
					else if (listChangedEventArgs.NewIndex == this.listposition)
					{
						this.ChangeRecordState(listChangedEventArgs.OldIndex, true, this.Position > -1 && this.Position < this.list.Count, true, false);
					}
					this.OnItemChanged(this.resetEvent);
					break;
				case ListChangedType.ItemChanged:
					if (listChangedEventArgs.NewIndex == this.listposition)
					{
						this.OnCurrentItemChanged(EventArgs.Empty);
					}
					this.OnItemChanged(new ItemChangedEventArgs(listChangedEventArgs.NewIndex));
					break;
				case ListChangedType.PropertyDescriptorAdded:
				case ListChangedType.PropertyDescriptorDeleted:
				case ListChangedType.PropertyDescriptorChanged:
					this.lastGoodKnownRow = -1;
					if (this.listposition == -1 && this.list.Count > 0)
					{
						this.ChangeRecordState(0, true, false, true, false);
					}
					else if (this.listposition > this.list.Count - 1)
					{
						this.ChangeRecordState(this.list.Count - 1, true, false, true, false);
					}
					this.OnMetaDataChanged(EventArgs.Empty);
					break;
				}
				this.OnListChanged(listChangedEventArgs);
			}
			finally
			{
				this.suspendPushDataInCurrentChanged = false;
			}
		}

		/// <summary>Occurs when the metadata of the <see cref="P:System.Windows.Forms.CurrencyManager.List" /> has changed.</summary>
		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06001074 RID: 4212 RVA: 0x0003A1B4 File Offset: 0x000383B4
		// (remove) Token: 0x06001075 RID: 4213 RVA: 0x0003A1CD File Offset: 0x000383CD
		[SRCategory("CatData")]
		public event EventHandler MetaDataChanged
		{
			add
			{
				this.onMetaDataChangedHandler = (EventHandler)Delegate.Combine(this.onMetaDataChangedHandler, value);
			}
			remove
			{
				this.onMetaDataChangedHandler = (EventHandler)Delegate.Remove(this.onMetaDataChangedHandler, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06001076 RID: 4214 RVA: 0x0003A1E8 File Offset: 0x000383E8
		protected internal override void OnCurrentChanged(EventArgs e)
		{
			if (!this.inChangeRecordState)
			{
				int num = this.lastGoodKnownRow;
				bool flag = false;
				if (!this.suspendPushDataInCurrentChanged)
				{
					flag = this.CurrencyManager_PushData();
				}
				if (this.Count > 0)
				{
					object obj = this.list[this.Position];
					if (obj is IEditableObject)
					{
						((IEditableObject)obj).BeginEdit();
					}
				}
				try
				{
					if (!flag || (flag && num != -1))
					{
						if (this.onCurrentChangedHandler != null)
						{
							this.onCurrentChangedHandler(this, e);
						}
						if (this.onCurrentItemChangedHandler != null)
						{
							this.onCurrentItemChangedHandler(this, e);
						}
					}
				}
				catch (Exception e2)
				{
					base.OnDataError(e2);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentItemChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06001077 RID: 4215 RVA: 0x0003A298 File Offset: 0x00038498
		protected internal override void OnCurrentItemChanged(EventArgs e)
		{
			if (this.onCurrentItemChangedHandler != null)
			{
				this.onCurrentItemChangedHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CurrencyManager.ItemChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.ItemChangedEventArgs" /> that contains the event data. </param>
		// Token: 0x06001078 RID: 4216 RVA: 0x0003A2B0 File Offset: 0x000384B0
		protected virtual void OnItemChanged(ItemChangedEventArgs e)
		{
			bool flag = false;
			if ((e.Index == this.listposition || (e.Index == -1 && this.Position < this.Count)) && !this.inChangeRecordState)
			{
				flag = this.CurrencyManager_PushData();
			}
			try
			{
				if (this.onItemChanged != null)
				{
					this.onItemChanged(this, e);
				}
			}
			catch (Exception e2)
			{
				base.OnDataError(e2);
			}
			if (flag)
			{
				this.OnPositionChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0003A334 File Offset: 0x00038534
		private void OnListChanged(ListChangedEventArgs e)
		{
			if (this.onListChanged != null)
			{
				this.onListChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.CurrencyManager.MetaDataChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600107A RID: 4218 RVA: 0x0003A34B File Offset: 0x0003854B
		protected internal void OnMetaDataChanged(EventArgs e)
		{
			if (this.onMetaDataChangedHandler != null)
			{
				this.onMetaDataChangedHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.PositionChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600107B RID: 4219 RVA: 0x0003A364 File Offset: 0x00038564
		protected virtual void OnPositionChanged(EventArgs e)
		{
			try
			{
				if (this.onPositionChangedHandler != null)
				{
					this.onPositionChangedHandler(this, e);
				}
			}
			catch (Exception e2)
			{
				base.OnDataError(e2);
			}
		}

		/// <summary>Forces a repopulation of the data-bound list.</summary>
		// Token: 0x0600107C RID: 4220 RVA: 0x0003A3A4 File Offset: 0x000385A4
		public void Refresh()
		{
			if (this.list.Count > 0)
			{
				if (this.listposition >= this.list.Count)
				{
					this.lastGoodKnownRow = -1;
					this.listposition = 0;
				}
			}
			else
			{
				this.listposition = -1;
			}
			this.List_ListChanged(this.list, new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0003A3FC File Offset: 0x000385FC
		internal void Release()
		{
			this.UnwireEvents(this.list);
		}

		/// <summary>Resumes data binding.</summary>
		// Token: 0x0600107E RID: 4222 RVA: 0x0003A40C File Offset: 0x0003860C
		public override void ResumeBinding()
		{
			this.lastGoodKnownRow = -1;
			try
			{
				if (!this.shouldBind)
				{
					this.shouldBind = true;
					this.listposition = ((this.list != null && this.list.Count != 0) ? 0 : -1);
					this.UpdateIsBinding();
				}
			}
			catch
			{
				this.shouldBind = false;
				this.UpdateIsBinding();
				throw;
			}
		}

		/// <summary>Suspends data binding to prevents changes from updating the bound data source.</summary>
		// Token: 0x0600107F RID: 4223 RVA: 0x0003A478 File Offset: 0x00038678
		public override void SuspendBinding()
		{
			this.lastGoodKnownRow = -1;
			if (this.shouldBind)
			{
				this.shouldBind = false;
				this.UpdateIsBinding();
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0003A496 File Offset: 0x00038696
		internal void UnwireEvents(IList list)
		{
			if (list is IBindingList && ((IBindingList)list).SupportsChangeNotification)
			{
				((IBindingList)list).ListChanged -= this.List_ListChanged;
			}
		}

		/// <summary>Updates the status of the binding.</summary>
		// Token: 0x06001081 RID: 4225 RVA: 0x0003A4C4 File Offset: 0x000386C4
		protected override void UpdateIsBinding()
		{
			this.UpdateIsBinding(true);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0003A4D0 File Offset: 0x000386D0
		private void UpdateIsBinding(bool raiseItemChangedEvent)
		{
			bool flag = this.list != null && this.list.Count > 0 && this.shouldBind && this.listposition != -1;
			if (this.list != null && this.bound != flag)
			{
				this.bound = flag;
				int num = flag ? 0 : -1;
				this.ChangeRecordState(num, this.bound, this.Position != num, true, false);
				int count = base.Bindings.Count;
				for (int i = 0; i < count; i++)
				{
					base.Bindings[i].UpdateIsBinding();
				}
				if (raiseItemChangedEvent)
				{
					this.OnItemChanged(this.resetEvent);
				}
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0003A580 File Offset: 0x00038780
		private void UpdateLastGoodKnownRow(ListChangedEventArgs e)
		{
			switch (e.ListChangedType)
			{
			case ListChangedType.Reset:
				this.lastGoodKnownRow = -1;
				return;
			case ListChangedType.ItemAdded:
				if (e.NewIndex <= this.lastGoodKnownRow && this.lastGoodKnownRow < this.List.Count - 1)
				{
					this.lastGoodKnownRow++;
					return;
				}
				break;
			case ListChangedType.ItemDeleted:
				if (e.NewIndex == this.lastGoodKnownRow)
				{
					this.lastGoodKnownRow = -1;
					return;
				}
				break;
			case ListChangedType.ItemMoved:
				if (e.OldIndex == this.lastGoodKnownRow)
				{
					this.lastGoodKnownRow = e.NewIndex;
					return;
				}
				break;
			case ListChangedType.ItemChanged:
				if (e.NewIndex == this.lastGoodKnownRow)
				{
					this.lastGoodKnownRow = -1;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0003A62F File Offset: 0x0003882F
		internal void WireEvents(IList list)
		{
			if (list is IBindingList && ((IBindingList)list).SupportsChangeNotification)
			{
				((IBindingList)list).ListChanged += this.List_ListChanged;
			}
		}

		// Token: 0x0400088B RID: 2187
		private object dataSource;

		// Token: 0x0400088C RID: 2188
		private IList list;

		// Token: 0x0400088D RID: 2189
		private bool bound;

		// Token: 0x0400088E RID: 2190
		private bool shouldBind = true;

		/// <summary>Specifies the current position of the <see cref="T:System.Windows.Forms.CurrencyManager" /> in the list.</summary>
		// Token: 0x0400088F RID: 2191
		protected int listposition = -1;

		// Token: 0x04000890 RID: 2192
		private int lastGoodKnownRow = -1;

		// Token: 0x04000891 RID: 2193
		private bool pullingData;

		// Token: 0x04000892 RID: 2194
		private bool inChangeRecordState;

		// Token: 0x04000893 RID: 2195
		private bool suspendPushDataInCurrentChanged;

		// Token: 0x04000894 RID: 2196
		private ItemChangedEventHandler onItemChanged;

		// Token: 0x04000895 RID: 2197
		private ListChangedEventHandler onListChanged;

		// Token: 0x04000896 RID: 2198
		private ItemChangedEventArgs resetEvent = new ItemChangedEventArgs(-1);

		// Token: 0x04000897 RID: 2199
		private EventHandler onMetaDataChangedHandler;

		/// <summary>Specifies the data type of the list.</summary>
		// Token: 0x04000898 RID: 2200
		protected Type finalType;
	}
}
