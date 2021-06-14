using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> objects in the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
	// Token: 0x02000171 RID: 369
	[Editor("System.Windows.Forms.Design.DataGridColumnCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[ListBindable(false)]
	public class GridColumnStylesCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		/// <summary>Adds an object to the collection.</summary>
		/// <param name="value">The object to be added to the collection. The value can be <see langword="null" />.</param>
		/// <returns>The index at which the value has been added.</returns>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> cannot be cast to a <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</exception>
		// Token: 0x06001356 RID: 4950 RVA: 0x00048E4F File Offset: 0x0004704F
		int IList.Add(object value)
		{
			return this.Add((DataGridColumnStyle)value);
		}

		/// <summary>Clears the collection of <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> objects.</summary>
		// Token: 0x06001357 RID: 4951 RVA: 0x00048E5D File Offset: 0x0004705D
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>Determines whether an element is in the collection.</summary>
		/// <param name="value">The object to locate in the collection. The value can be <see langword="null" />.</param>
		/// <returns>
		///     <see langword="true" /> if the element is in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001358 RID: 4952 RVA: 0x00048E65 File Offset: 0x00047065
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		/// <summary>Returns the zero-based index of the first occurrence of the specified object in the collection.</summary>
		/// <param name="value">The object to locate in the collection. The value can be <see langword="null" />.</param>
		/// <returns>The zero-based index of the first occurrence of the <paramref name="value" /> parameter within the collection, if found; otherwise, -1.</returns>
		// Token: 0x06001359 RID: 4953 RVA: 0x00048E73 File Offset: 0x00047073
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		/// <summary>This method is not supported by this control.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The object to insert into the collection.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600135A RID: 4954 RVA: 0x0000A2AB File Offset: 0x000084AB
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> from the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to remove from the collection.</param>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> cannot be cast to a <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</exception>
		// Token: 0x0600135B RID: 4955 RVA: 0x00048E81 File Offset: 0x00047081
		void IList.Remove(object value)
		{
			this.Remove((DataGridColumnStyle)value);
		}

		/// <summary>Removes the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> at the specified index from the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" />.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to remove.</param>
		// Token: 0x0600135C RID: 4956 RVA: 0x00048E8F File Offset: 0x0004708F
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
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
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.NotSupportedException">An operation attempts to set this property.</exception>
		// Token: 0x170004AB RID: 1195
		object IList.this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Copies the collection to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from collection. The array must have zero-based indexing.  </param>
		/// <param name="index">The zero-based index in the array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or-The number of elements in the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" /> is greater than the available space from <paramref name="index" /> to the end of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The type of the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" /> cannot be cast automatically to the type of <paramref name="array" />.</exception>
		// Token: 0x06001361 RID: 4961 RVA: 0x00048EA6 File Offset: 0x000470A6
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Gets the number of elements contained in the collection.</summary>
		/// <returns>The number of elements contained in the collection.</returns>
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001362 RID: 4962 RVA: 0x00048EB5 File Offset: 0x000470B5
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" />.</summary>
		/// <returns>The object used to synchronize access to the collection.</returns>
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns an enumerator for the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the collection.</returns>
		// Token: 0x06001365 RID: 4965 RVA: 0x00048EC2 File Offset: 0x000470C2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00048ECF File Offset: 0x000470CF
		internal GridColumnStylesCollection(DataGridTableStyle table)
		{
			this.owner = table;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00048EE9 File Offset: 0x000470E9
		internal GridColumnStylesCollection(DataGridTableStyle table, bool isDefault) : this(table)
		{
			this.isDefault = isDefault;
		}

		/// <summary>Gets the list of items in the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> containing the collection items.</returns>
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x00048EF9 File Offset: 0x000470F9
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> at a specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to return. </param>
		/// <returns>The specified <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</returns>
		// Token: 0x170004B0 RID: 1200
		public DataGridColumnStyle this[int index]
		{
			get
			{
				return (DataGridColumnStyle)this.items[index];
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified name.</summary>
		/// <param name="columnName">The <see cref="P:System.Windows.Forms.DataGridColumnStyle.MappingName" /> of the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to retrieve. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified column header.</returns>
		// Token: 0x170004B1 RID: 1201
		public DataGridColumnStyle this[string columnName]
		{
			get
			{
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[i];
					if (string.Equals(dataGridColumnStyle.MappingName, columnName, StringComparison.OrdinalIgnoreCase))
					{
						return dataGridColumnStyle;
					}
				}
				return null;
			}
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00048F60 File Offset: 0x00047160
		internal DataGridColumnStyle MapColumnStyleToPropertyName(string mappingName)
		{
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[i];
				if (string.Equals(dataGridColumnStyle.MappingName, mappingName, StringComparison.OrdinalIgnoreCase))
				{
					return dataGridColumnStyle;
				}
			}
			return null;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> associated with the specified <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
		/// <param name="propertyDesciptor">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> associated with the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> associated the specified <see cref="T:System.ComponentModel.PropertyDescriptor" />.</returns>
		// Token: 0x170004B2 RID: 1202
		public DataGridColumnStyle this[PropertyDescriptor propertyDesciptor]
		{
			get
			{
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[i];
					if (propertyDesciptor.Equals(dataGridColumnStyle.PropertyDescriptor))
					{
						return dataGridColumnStyle;
					}
				}
				return null;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00048FF4 File Offset: 0x000471F4
		internal DataGridTableStyle DataGridTableStyle
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00048FFC File Offset: 0x000471FC
		internal void CheckForMappingNameDuplicates(DataGridColumnStyle column)
		{
			if (string.IsNullOrEmpty(column.MappingName))
			{
				return;
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (((DataGridColumnStyle)this.items[i]).MappingName.Equals(column.MappingName) && column != this.items[i])
				{
					throw new ArgumentException(SR.GetString("DataGridColumnStyleDuplicateMappingName"), "column");
				}
			}
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00049074 File Offset: 0x00047274
		private void ColumnStyleMappingNameChanged(object sender, EventArgs pcea)
		{
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00049083 File Offset: 0x00047283
		private void ColumnStylePropDescChanged(object sender, EventArgs pcea)
		{
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, (DataGridColumnStyle)sender));
		}

		/// <summary>Adds a column style to the collection.</summary>
		/// <param name="column">The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to add. </param>
		/// <returns>The index of the new <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</returns>
		// Token: 0x06001371 RID: 4977 RVA: 0x00049098 File Offset: 0x00047298
		public virtual int Add(DataGridColumnStyle column)
		{
			if (this.isDefault)
			{
				throw new ArgumentException(SR.GetString("DataGridDefaultColumnCollectionChanged"));
			}
			this.CheckForMappingNameDuplicates(column);
			column.SetDataGridTableInColumn(this.owner, true);
			column.MappingNameChanged += this.ColumnStyleMappingNameChanged;
			column.PropertyDescriptorChanged += this.ColumnStylePropDescChanged;
			if (this.DataGridTableStyle != null && column.Width == -1)
			{
				column.width = this.DataGridTableStyle.PreferredColumnWidth;
			}
			int result = this.items.Add(column);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, column));
			return result;
		}

		/// <summary>Adds an array of column style objects to the collection.</summary>
		/// <param name="columns">An array of <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> objects to add to the collection. </param>
		// Token: 0x06001372 RID: 4978 RVA: 0x00049134 File Offset: 0x00047334
		public void AddRange(DataGridColumnStyle[] columns)
		{
			if (columns == null)
			{
				throw new ArgumentNullException("columns");
			}
			for (int i = 0; i < columns.Length; i++)
			{
				this.Add(columns[i]);
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00049167 File Offset: 0x00047367
		internal void AddDefaultColumn(DataGridColumnStyle column)
		{
			column.SetDataGridTableInColumn(this.owner, true);
			this.items.Add(column);
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00049184 File Offset: 0x00047384
		internal void ResetDefaultColumnCollection()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].ReleaseHostedControl();
			}
			this.items.Clear();
		}

		/// <summary>Occurs when a change is made to the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" />.</summary>
		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06001375 RID: 4981 RVA: 0x000491B9 File Offset: 0x000473B9
		// (remove) Token: 0x06001376 RID: 4982 RVA: 0x000491D2 File Offset: 0x000473D2
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanged, value);
			}
			remove
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanged, value);
			}
		}

		/// <summary>Clears the collection of <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> objects.</summary>
		// Token: 0x06001377 RID: 4983 RVA: 0x000491EC File Offset: 0x000473EC
		public void Clear()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].ReleaseHostedControl();
			}
			this.items.Clear();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" /> contains a <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> associated with the specified <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
		/// <param name="propertyDescriptor">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> associated with the desired <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <returns>
		///     <see langword="true" /> if the collection contains the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001378 RID: 4984 RVA: 0x0004922E File Offset: 0x0004742E
		public bool Contains(PropertyDescriptor propertyDescriptor)
		{
			return this[propertyDescriptor] != null;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" /> contains the specified <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</summary>
		/// <param name="column">The desired <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <returns>
		///     <see langword="true" /> if the collection contains the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001379 RID: 4985 RVA: 0x0004923C File Offset: 0x0004743C
		public bool Contains(DataGridColumnStyle column)
		{
			int num = this.items.IndexOf(column);
			return num != -1;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" /> contains the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified name.</summary>
		/// <param name="name">The <see cref="P:System.Windows.Forms.DataGridColumnStyle.MappingName" /> of the desired <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <returns>
		///     <see langword="true" /> if the collection contains the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600137A RID: 4986 RVA: 0x00049260 File Offset: 0x00047460
		public bool Contains(string name)
		{
			foreach (object obj in this.items)
			{
				DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)obj;
				if (string.Compare(dataGridColumnStyle.MappingName, name, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Gets the index of a specified <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</summary>
		/// <param name="element">The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to find. </param>
		/// <returns>The zero-based index of the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> within the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" /> or -1 if no corresponding <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> exists.</returns>
		// Token: 0x0600137B RID: 4987 RVA: 0x000492A8 File Offset: 0x000474A8
		public int IndexOf(DataGridColumnStyle element)
		{
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[i];
				if (element == dataGridColumnStyle)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.GridColumnStylesCollection.CollectionChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> that contains the event data event. </param>
		// Token: 0x0600137C RID: 4988 RVA: 0x000492E8 File Offset: 0x000474E8
		protected void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
			DataGrid dataGrid = this.owner.DataGrid;
			if (dataGrid != null)
			{
				dataGrid.checkHierarchy = true;
			}
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> from the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" />.</summary>
		/// <param name="column">The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to remove from the collection. </param>
		// Token: 0x0600137D RID: 4989 RVA: 0x00049320 File Offset: 0x00047520
		public void Remove(DataGridColumnStyle column)
		{
			if (this.isDefault)
			{
				throw new ArgumentException(SR.GetString("DataGridDefaultColumnCollectionChanged"));
			}
			int num = -1;
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.items[i] == column)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				throw new InvalidOperationException(SR.GetString("DataGridColumnCollectionMissing"));
			}
			this.RemoveAt(num);
		}

		/// <summary>Removes the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified index from the <see cref="T:System.Windows.Forms.GridColumnStylesCollection" />.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> to remove. </param>
		// Token: 0x0600137E RID: 4990 RVA: 0x00049390 File Offset: 0x00047590
		public void RemoveAt(int index)
		{
			if (this.isDefault)
			{
				throw new ArgumentException(SR.GetString("DataGridDefaultColumnCollectionChanged"));
			}
			DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)this.items[index];
			dataGridColumnStyle.SetDataGridTableInColumn(null, true);
			dataGridColumnStyle.MappingNameChanged -= this.ColumnStyleMappingNameChanged;
			dataGridColumnStyle.PropertyDescriptorChanged -= this.ColumnStylePropDescChanged;
			this.items.RemoveAt(index);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridColumnStyle));
		}

		/// <summary>Sets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> for each column style in the collection to <see langword="null" />.</summary>
		// Token: 0x0600137F RID: 4991 RVA: 0x0004940C File Offset: 0x0004760C
		public void ResetPropertyDescriptors()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].PropertyDescriptor = null;
			}
		}

		// Token: 0x0400099B RID: 2459
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x0400099C RID: 2460
		private ArrayList items = new ArrayList();

		// Token: 0x0400099D RID: 2461
		private DataGridTableStyle owner;

		// Token: 0x0400099E RID: 2462
		private bool isDefault;
	}
}
