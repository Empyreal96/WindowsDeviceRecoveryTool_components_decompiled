using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.DataGridTableStyle" /> objects in the <see cref="T:System.Windows.Forms.DataGrid" /> control.</summary>
	// Token: 0x0200017A RID: 378
	[ListBindable(false)]
	public class GridTableStylesCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		/// <summary>Adds a <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to this collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to add to the collection.</param>
		/// <returns>The index of the newly added object.</returns>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> cannot be cast to a <see cref="T:System.Windows.Forms.DataGridTableStyle" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> has already been assigned to a <see cref="T:System.Windows.Forms.GridTableStylesCollection" />.-or-A <see cref="T:System.Windows.Forms.DataGridTableStyle" /> in <see cref="T:System.Windows.Forms.GridTableStylesCollection" /> has the same <see cref="P:System.Windows.Forms.DataGridTableStyle.MappingName" /> property value as <paramref name="value" />.</exception>
		// Token: 0x060014CD RID: 5325 RVA: 0x0004E831 File Offset: 0x0004CA31
		int IList.Add(object value)
		{
			return this.Add((DataGridTableStyle)value);
		}

		/// <summary>Clears the collection.</summary>
		// Token: 0x060014CE RID: 5326 RVA: 0x0004E83F File Offset: 0x0004CA3F
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>Determines whether an element is in the collection.</summary>
		/// <param name="value">The object to locate in the collection. The value can be <see langword="null" />.</param>
		/// <returns>
		///     <see langword="true" /> if value is found in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x060014CF RID: 5327 RVA: 0x0004E847 File Offset: 0x0004CA47
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		/// <summary>Returns the zero-based index of the first occurrence of the specified object in the collection.</summary>
		/// <param name="value">The object to locate in the collection. The value can be <see langword="null" />.</param>
		/// <returns>The zero-based index of the first occurrence of value within the entire collection, if found; otherwise, -1.</returns>
		// Token: 0x060014D0 RID: 5328 RVA: 0x0004E855 File Offset: 0x0004CA55
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The object to insert into the collection.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x060014D1 RID: 5329 RVA: 0x0000A2AB File Offset: 0x000084AB
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Forms.DataGridTableStyle" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to remove from the collection.</param>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> cannot be cast to a <see cref="T:System.Windows.Forms.DataGridTableStyle" />.</exception>
		// Token: 0x060014D2 RID: 5330 RVA: 0x0004E863 File Offset: 0x0004CA63
		void IList.Remove(object value)
		{
			this.Remove((DataGridTableStyle)value);
		}

		/// <summary>Removes the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified index from the collection.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to remove.</param>
		// Token: 0x060014D3 RID: 5331 RVA: 0x0004E871 File Offset: 0x0004CA71
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
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
		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.NotSupportedException">The item property cannot be set.</exception>
		// Token: 0x170004FA RID: 1274
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
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the collection. The array must have zero-based indexing.  </param>
		/// <param name="index">The zero-based index in the array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or-The number of elements in the <see cref="T:System.Windows.Forms.GridTableStylesCollection" /> is greater than the available space from index to the end of the destination array.</exception>
		/// <exception cref="T:System.InvalidCastException">The type in the collection cannot be cast automatically to the type of the destination array.</exception>
		// Token: 0x060014D8 RID: 5336 RVA: 0x0004E888 File Offset: 0x0004CA88
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Gets the number of items in the collection.</summary>
		/// <returns>The number of items contained in the collection.</returns>
		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x0004E897 File Offset: 0x0004CA97
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Windows.Forms.GridTableStylesCollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>The <see cref="T:System.Object" /> used to synchronize access to the collection.</returns>
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns an enumerator for the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the collection.</returns>
		// Token: 0x060014DC RID: 5340 RVA: 0x0004E8A4 File Offset: 0x0004CAA4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0004E8B1 File Offset: 0x0004CAB1
		internal GridTableStylesCollection(DataGrid grid)
		{
			this.owner = grid;
		}

		/// <summary>Gets the underlying list.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> that contains the table data.</returns>
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x0004E8CB File Offset: 0x0004CACB
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> specified by index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to get. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridTableStyle" /> at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">No item exists at the specified index. </exception>
		// Token: 0x170004FF RID: 1279
		public DataGridTableStyle this[int index]
		{
			get
			{
				return (DataGridTableStyle)this.items[index];
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> with the specified name.</summary>
		/// <param name="tableName">The <see cref="P:System.Windows.Forms.DataGridTableStyle.MappingName" /> of the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to retrieve. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridTableStyle" /> with the specified <see cref="P:System.Windows.Forms.DataGridTableStyle.MappingName" />.</returns>
		// Token: 0x17000500 RID: 1280
		public DataGridTableStyle this[string tableName]
		{
			get
			{
				if (tableName == null)
				{
					throw new ArgumentNullException("tableName");
				}
				int count = this.items.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)this.items[i];
					if (string.Equals(dataGridTableStyle.MappingName, tableName, StringComparison.OrdinalIgnoreCase))
					{
						return dataGridTableStyle;
					}
				}
				return null;
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0004E940 File Offset: 0x0004CB40
		internal void CheckForMappingNameDuplicates(DataGridTableStyle table)
		{
			if (string.IsNullOrEmpty(table.MappingName))
			{
				return;
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (((DataGridTableStyle)this.items[i]).MappingName.Equals(table.MappingName) && table != this.items[i])
				{
					throw new ArgumentException(SR.GetString("DataGridTableStyleDuplicateMappingName"), "table");
				}
			}
		}

		/// <summary>Adds a <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to this collection.</summary>
		/// <param name="table">The <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to add to the collection. </param>
		/// <returns>The index of the newly added object.</returns>
		// Token: 0x060014E2 RID: 5346 RVA: 0x0004E9B8 File Offset: 0x0004CBB8
		public virtual int Add(DataGridTableStyle table)
		{
			if (this.owner != null && this.owner.MinimumRowHeaderWidth() > table.RowHeaderWidth)
			{
				table.RowHeaderWidth = this.owner.MinimumRowHeaderWidth();
			}
			if (table.DataGrid != this.owner && table.DataGrid != null)
			{
				throw new ArgumentException(SR.GetString("DataGridTableStyleCollectionAddedParentedTableStyle"), "table");
			}
			table.DataGrid = this.owner;
			this.CheckForMappingNameDuplicates(table);
			table.MappingNameChanged += this.TableStyleMappingNameChanged;
			int result = this.items.Add(table);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, table));
			return result;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0004EA5C File Offset: 0x0004CC5C
		private void TableStyleMappingNameChanged(object sender, EventArgs pcea)
		{
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Adds an array of table styles to the collection.</summary>
		/// <param name="tables">An array of <see cref="T:System.Windows.Forms.DataGridTableStyle" /> objects. </param>
		// Token: 0x060014E4 RID: 5348 RVA: 0x0004EA6C File Offset: 0x0004CC6C
		public virtual void AddRange(DataGridTableStyle[] tables)
		{
			if (tables == null)
			{
				throw new ArgumentNullException("tables");
			}
			foreach (DataGridTableStyle dataGridTableStyle in tables)
			{
				dataGridTableStyle.DataGrid = this.owner;
				dataGridTableStyle.MappingNameChanged += this.TableStyleMappingNameChanged;
				this.items.Add(dataGridTableStyle);
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Occurs when the collection has changed.</summary>
		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x060014E5 RID: 5349 RVA: 0x0004EAD3 File Offset: 0x0004CCD3
		// (remove) Token: 0x060014E6 RID: 5350 RVA: 0x0004EAEC File Offset: 0x0004CCEC
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

		/// <summary>Clears the collection.</summary>
		// Token: 0x060014E7 RID: 5351 RVA: 0x0004EB08 File Offset: 0x0004CD08
		public void Clear()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)this.items[i];
				dataGridTableStyle.MappingNameChanged -= this.TableStyleMappingNameChanged;
			}
			this.items.Clear();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.GridTableStylesCollection" /> contains the specified <see cref="T:System.Windows.Forms.DataGridTableStyle" />.</summary>
		/// <param name="table">The <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to look for. </param>
		/// <returns>
		///     <see langword="true" /> if the specified table style exists in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x060014E8 RID: 5352 RVA: 0x0004EB68 File Offset: 0x0004CD68
		public bool Contains(DataGridTableStyle table)
		{
			int num = this.items.IndexOf(table);
			return num != -1;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.GridTableStylesCollection" /> contains the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> specified by name.</summary>
		/// <param name="name">The <see cref="P:System.Windows.Forms.DataGridTableStyle.MappingName" /> of the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to look for. </param>
		/// <returns>
		///     <see langword="true" /> if the specified table style exists in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x060014E9 RID: 5353 RVA: 0x0004EB8C File Offset: 0x0004CD8C
		public bool Contains(string name)
		{
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)this.items[i];
				if (string.Compare(dataGridTableStyle.MappingName, name, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.GridTableStylesCollection.CollectionChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> containing the event data. </param>
		// Token: 0x060014EA RID: 5354 RVA: 0x0004EBDC File Offset: 0x0004CDDC
		protected void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
			DataGrid dataGrid = this.owner;
			if (dataGrid != null)
			{
				dataGrid.checkHierarchy = true;
			}
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Forms.DataGridTableStyle" />.</summary>
		/// <param name="table">The <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to remove. </param>
		// Token: 0x060014EB RID: 5355 RVA: 0x0004EC10 File Offset: 0x0004CE10
		public void Remove(DataGridTableStyle table)
		{
			int num = -1;
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.items[i] == table)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				throw new ArgumentException(SR.GetString("DataGridTableCollectionMissingTable"), "table");
			}
			this.RemoveAt(num);
		}

		/// <summary>Removes a <see cref="T:System.Windows.Forms.DataGridTableStyle" /> at the specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> to remove. </param>
		// Token: 0x060014EC RID: 5356 RVA: 0x0004EC6C File Offset: 0x0004CE6C
		public void RemoveAt(int index)
		{
			DataGridTableStyle dataGridTableStyle = (DataGridTableStyle)this.items[index];
			dataGridTableStyle.MappingNameChanged -= this.TableStyleMappingNameChanged;
			this.items.RemoveAt(index);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridTableStyle));
		}

		// Token: 0x04000A13 RID: 2579
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04000A14 RID: 2580
		private ArrayList items = new ArrayList();

		// Token: 0x04000A15 RID: 2581
		private DataGrid owner;
	}
}
