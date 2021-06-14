using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Manages all <see cref="T:System.Windows.Forms.Binding" /> objects that are bound to the same data source and data member. This class is abstract.</summary>
	// Token: 0x02000125 RID: 293
	public abstract class BindingManagerBase
	{
		/// <summary>Gets the collection of bindings being managed.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.BindingsCollection" /> that contains the <see cref="T:System.Windows.Forms.Binding" /> objects managed by this <see cref="T:System.Windows.Forms.BindingManagerBase" />.</returns>
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00017B1C File Offset: 0x00015D1C
		public BindingsCollection Bindings
		{
			get
			{
				if (this.bindings == null)
				{
					this.bindings = new ListManagerBindingsCollection(this);
					this.bindings.CollectionChanging += this.OnBindingsCollectionChanging;
					this.bindings.CollectionChanged += this.OnBindingsCollectionChanged;
				}
				return this.bindings;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.BindingComplete" /> event. </summary>
		/// <param name="args">A <see cref="T:System.Windows.Forms.BindingCompleteEventArgs" />  that contains the event data. </param>
		// Token: 0x060007D4 RID: 2004 RVA: 0x00017B71 File Offset: 0x00015D71
		protected internal void OnBindingComplete(BindingCompleteEventArgs args)
		{
			if (this.onBindingCompleteHandler != null)
			{
				this.onBindingCompleteHandler(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060007D5 RID: 2005
		protected internal abstract void OnCurrentChanged(EventArgs e);

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentItemChanged" /> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060007D6 RID: 2006
		protected internal abstract void OnCurrentItemChanged(EventArgs e);

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingManagerBase.DataError" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Exception" /> that caused the <see cref="E:System.Windows.Forms.BindingManagerBase.DataError" /> event to occur.</param>
		// Token: 0x060007D7 RID: 2007 RVA: 0x00017B88 File Offset: 0x00015D88
		protected internal void OnDataError(Exception e)
		{
			if (this.onDataErrorHandler != null)
			{
				this.onDataErrorHandler(this, new BindingManagerDataErrorEventArgs(e));
			}
		}

		/// <summary>When overridden in a derived class, gets the current object.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the current object.</returns>
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060007D8 RID: 2008
		public abstract object Current { get; }

		// Token: 0x060007D9 RID: 2009
		internal abstract void SetDataSource(object dataSource);

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingManagerBase" /> class.</summary>
		// Token: 0x060007DA RID: 2010 RVA: 0x000027DB File Offset: 0x000009DB
		public BindingManagerBase()
		{
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00017BA4 File Offset: 0x00015DA4
		internal BindingManagerBase(object dataSource)
		{
			this.SetDataSource(dataSource);
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060007DC RID: 2012
		internal abstract Type BindType { get; }

		// Token: 0x060007DD RID: 2013
		internal abstract PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors);

		/// <summary>When overridden in a derived class, gets the collection of property descriptors for the binding.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the property descriptors for the binding.</returns>
		// Token: 0x060007DE RID: 2014 RVA: 0x00017BB3 File Offset: 0x00015DB3
		public virtual PropertyDescriptorCollection GetItemProperties()
		{
			return this.GetItemProperties(null);
		}

		/// <summary>Gets the collection of property descriptors for the binding using the specified <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <param name="dataSources">An <see cref="T:System.Collections.ArrayList" /> containing the data sources. </param>
		/// <param name="listAccessors">An <see cref="T:System.Collections.ArrayList" /> containing the table's bound properties. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the property descriptors for the binding.</returns>
		// Token: 0x060007DF RID: 2015 RVA: 0x00017BBC File Offset: 0x00015DBC
		protected internal virtual PropertyDescriptorCollection GetItemProperties(ArrayList dataSources, ArrayList listAccessors)
		{
			IList list = null;
			if (this is CurrencyManager)
			{
				list = ((CurrencyManager)this).List;
			}
			if (list is ITypedList)
			{
				PropertyDescriptor[] array = new PropertyDescriptor[listAccessors.Count];
				listAccessors.CopyTo(array, 0);
				return ((ITypedList)list).GetItemProperties(array);
			}
			return this.GetItemProperties(this.BindType, 0, dataSources, listAccessors);
		}

		/// <summary>Gets the list of properties of the items managed by this <see cref="T:System.Windows.Forms.BindingManagerBase" />.</summary>
		/// <param name="listType">The <see cref="T:System.Type" /> of the bound list. </param>
		/// <param name="offset">A counter used to recursively call the method. </param>
		/// <param name="dataSources">An <see cref="T:System.Collections.ArrayList" /> containing the data sources. </param>
		/// <param name="listAccessors">An <see cref="T:System.Collections.ArrayList" /> containing the table's bound properties. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the property descriptors for the binding.</returns>
		// Token: 0x060007E0 RID: 2016 RVA: 0x00017C18 File Offset: 0x00015E18
		protected virtual PropertyDescriptorCollection GetItemProperties(Type listType, int offset, ArrayList dataSources, ArrayList listAccessors)
		{
			if (listAccessors.Count < offset)
			{
				return null;
			}
			if (listAccessors.Count != offset)
			{
				PropertyInfo[] properties = listType.GetProperties();
				if (typeof(IList).IsAssignableFrom(listType))
				{
					PropertyDescriptorCollection propertyDescriptorCollection = null;
					for (int i = 0; i < properties.Length; i++)
					{
						if ("Item".Equals(properties[i].Name) && properties[i].PropertyType != typeof(object))
						{
							propertyDescriptorCollection = TypeDescriptor.GetProperties(properties[i].PropertyType, new Attribute[]
							{
								new BrowsableAttribute(true)
							});
						}
					}
					if (propertyDescriptorCollection == null)
					{
						IList list;
						if (offset == 0)
						{
							list = (this.DataSource as IList);
						}
						else
						{
							list = (dataSources[offset - 1] as IList);
						}
						if (list != null && list.Count > 0)
						{
							propertyDescriptorCollection = TypeDescriptor.GetProperties(list[0]);
						}
					}
					if (propertyDescriptorCollection != null)
					{
						for (int j = 0; j < propertyDescriptorCollection.Count; j++)
						{
							if (propertyDescriptorCollection[j].Equals(listAccessors[offset]))
							{
								return this.GetItemProperties(propertyDescriptorCollection[j].PropertyType, offset + 1, dataSources, listAccessors);
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < properties.Length; k++)
					{
						if (properties[k].Name.Equals(((PropertyDescriptor)listAccessors[offset]).Name))
						{
							return this.GetItemProperties(properties[k].PropertyType, offset + 1, dataSources, listAccessors);
						}
					}
				}
				return null;
			}
			if (!typeof(IList).IsAssignableFrom(listType))
			{
				return TypeDescriptor.GetProperties(listType);
			}
			PropertyInfo[] properties2 = listType.GetProperties();
			for (int l = 0; l < properties2.Length; l++)
			{
				if ("Item".Equals(properties2[l].Name) && properties2[l].PropertyType != typeof(object))
				{
					return TypeDescriptor.GetProperties(properties2[l].PropertyType, new Attribute[]
					{
						new BrowsableAttribute(true)
					});
				}
			}
			IList list2 = dataSources[offset - 1] as IList;
			if (list2 != null && list2.Count > 0)
			{
				return TypeDescriptor.GetProperties(list2[0]);
			}
			return null;
		}

		/// <summary>Occurs at the completion of a data-binding operation.</summary>
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060007E1 RID: 2017 RVA: 0x00017E42 File Offset: 0x00016042
		// (remove) Token: 0x060007E2 RID: 2018 RVA: 0x00017E5B File Offset: 0x0001605B
		public event BindingCompleteEventHandler BindingComplete
		{
			add
			{
				this.onBindingCompleteHandler = (BindingCompleteEventHandler)Delegate.Combine(this.onBindingCompleteHandler, value);
			}
			remove
			{
				this.onBindingCompleteHandler = (BindingCompleteEventHandler)Delegate.Remove(this.onBindingCompleteHandler, value);
			}
		}

		/// <summary>Occurs when the currently bound item changes.</summary>
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060007E3 RID: 2019 RVA: 0x00017E74 File Offset: 0x00016074
		// (remove) Token: 0x060007E4 RID: 2020 RVA: 0x00017E8D File Offset: 0x0001608D
		public event EventHandler CurrentChanged
		{
			add
			{
				this.onCurrentChangedHandler = (EventHandler)Delegate.Combine(this.onCurrentChangedHandler, value);
			}
			remove
			{
				this.onCurrentChangedHandler = (EventHandler)Delegate.Remove(this.onCurrentChangedHandler, value);
			}
		}

		/// <summary>Occurs when the state of the currently bound item changes.</summary>
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060007E5 RID: 2021 RVA: 0x00017EA6 File Offset: 0x000160A6
		// (remove) Token: 0x060007E6 RID: 2022 RVA: 0x00017EBF File Offset: 0x000160BF
		public event EventHandler CurrentItemChanged
		{
			add
			{
				this.onCurrentItemChangedHandler = (EventHandler)Delegate.Combine(this.onCurrentItemChangedHandler, value);
			}
			remove
			{
				this.onCurrentItemChangedHandler = (EventHandler)Delegate.Remove(this.onCurrentItemChangedHandler, value);
			}
		}

		/// <summary>Occurs when an <see cref="T:System.Exception" /> is silently handled by the <see cref="T:System.Windows.Forms.BindingManagerBase" />. </summary>
		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060007E7 RID: 2023 RVA: 0x00017ED8 File Offset: 0x000160D8
		// (remove) Token: 0x060007E8 RID: 2024 RVA: 0x00017EF1 File Offset: 0x000160F1
		public event BindingManagerDataErrorEventHandler DataError
		{
			add
			{
				this.onDataErrorHandler = (BindingManagerDataErrorEventHandler)Delegate.Combine(this.onDataErrorHandler, value);
			}
			remove
			{
				this.onDataErrorHandler = (BindingManagerDataErrorEventHandler)Delegate.Remove(this.onDataErrorHandler, value);
			}
		}

		// Token: 0x060007E9 RID: 2025
		internal abstract string GetListName();

		/// <summary>When overridden in a derived class, cancels the current edit.</summary>
		// Token: 0x060007EA RID: 2026
		public abstract void CancelCurrentEdit();

		/// <summary>When overridden in a derived class, ends the current edit.</summary>
		// Token: 0x060007EB RID: 2027
		public abstract void EndCurrentEdit();

		/// <summary>When overridden in a derived class, adds a new item to the underlying list.</summary>
		// Token: 0x060007EC RID: 2028
		public abstract void AddNew();

		/// <summary>When overridden in a derived class, deletes the row at the specified index from the underlying list.</summary>
		/// <param name="index">The index of the row to delete. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">There is no row at the specified <paramref name="index" />. </exception>
		// Token: 0x060007ED RID: 2029
		public abstract void RemoveAt(int index);

		/// <summary>When overridden in a derived class, gets or sets the position in the underlying list that controls bound to this data source point to.</summary>
		/// <returns>A zero-based index that specifies a position in the underlying list.</returns>
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060007EE RID: 2030
		// (set) Token: 0x060007EF RID: 2031
		public abstract int Position { get; set; }

		/// <summary>Occurs after the value of the <see cref="P:System.Windows.Forms.BindingManagerBase.Position" /> property has changed.</summary>
		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060007F0 RID: 2032 RVA: 0x00017F0A File Offset: 0x0001610A
		// (remove) Token: 0x060007F1 RID: 2033 RVA: 0x00017F23 File Offset: 0x00016123
		public event EventHandler PositionChanged
		{
			add
			{
				this.onPositionChangedHandler = (EventHandler)Delegate.Combine(this.onPositionChangedHandler, value);
			}
			remove
			{
				this.onPositionChangedHandler = (EventHandler)Delegate.Remove(this.onPositionChangedHandler, value);
			}
		}

		/// <summary>When overridden in a derived class, updates the binding.</summary>
		// Token: 0x060007F2 RID: 2034
		protected abstract void UpdateIsBinding();

		/// <summary>When overridden in a derived class, gets the name of the list supplying the data for the binding.</summary>
		/// <param name="listAccessors">An <see cref="T:System.Collections.ArrayList" /> containing the table's bound properties. </param>
		/// <returns>The name of the list supplying the data for the binding.</returns>
		// Token: 0x060007F3 RID: 2035
		protected internal abstract string GetListName(ArrayList listAccessors);

		/// <summary>When overridden in a derived class, suspends data binding.</summary>
		// Token: 0x060007F4 RID: 2036
		public abstract void SuspendBinding();

		/// <summary>When overridden in a derived class, resumes data binding.</summary>
		// Token: 0x060007F5 RID: 2037
		public abstract void ResumeBinding();

		/// <summary>Pulls data from the data-bound control into the data source, returning no information.</summary>
		// Token: 0x060007F6 RID: 2038 RVA: 0x00017F3C File Offset: 0x0001613C
		protected void PullData()
		{
			bool flag;
			this.PullData(out flag);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00017F54 File Offset: 0x00016154
		internal void PullData(out bool success)
		{
			success = true;
			this.pullingData = true;
			try
			{
				this.UpdateIsBinding();
				int count = this.Bindings.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.Bindings[i].PullData())
					{
						success = false;
					}
				}
			}
			finally
			{
				this.pullingData = false;
			}
		}

		/// <summary>Pushes data from the data source into the data-bound control, returning no information.</summary>
		// Token: 0x060007F8 RID: 2040 RVA: 0x00017FBC File Offset: 0x000161BC
		protected void PushData()
		{
			bool flag;
			this.PushData(out flag);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00017FD4 File Offset: 0x000161D4
		internal void PushData(out bool success)
		{
			success = true;
			if (this.pullingData)
			{
				return;
			}
			this.UpdateIsBinding();
			int count = this.Bindings.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.Bindings[i].PushData())
				{
					success = false;
				}
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060007FA RID: 2042
		internal abstract object DataSource { get; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060007FB RID: 2043
		internal abstract bool IsBinding { get; }

		/// <summary>Gets a value indicating whether binding is suspended.</summary>
		/// <returns>
		///     <see langword="true" /> if binding is suspended; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00018021 File Offset: 0x00016221
		public bool IsBindingSuspended
		{
			get
			{
				return !this.IsBinding;
			}
		}

		/// <summary>When overridden in a derived class, gets the number of rows managed by the <see cref="T:System.Windows.Forms.BindingManagerBase" />.</summary>
		/// <returns>The number of rows managed by the <see cref="T:System.Windows.Forms.BindingManagerBase" />.</returns>
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060007FD RID: 2045
		public abstract int Count { get; }

		// Token: 0x060007FE RID: 2046 RVA: 0x0001802C File Offset: 0x0001622C
		private void OnBindingsCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			Binding binding = e.Element as Binding;
			switch (e.Action)
			{
			case CollectionChangeAction.Add:
				binding.BindingComplete += this.Binding_BindingComplete;
				return;
			case CollectionChangeAction.Remove:
				binding.BindingComplete -= this.Binding_BindingComplete;
				return;
			case CollectionChangeAction.Refresh:
				foreach (object obj in this.bindings)
				{
					Binding binding2 = (Binding)obj;
					binding2.BindingComplete += this.Binding_BindingComplete;
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000180E4 File Offset: 0x000162E4
		private void OnBindingsCollectionChanging(object sender, CollectionChangeEventArgs e)
		{
			if (e.Action == CollectionChangeAction.Refresh)
			{
				foreach (object obj in this.bindings)
				{
					Binding binding = (Binding)obj;
					binding.BindingComplete -= this.Binding_BindingComplete;
				}
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00018154 File Offset: 0x00016354
		internal void Binding_BindingComplete(object sender, BindingCompleteEventArgs args)
		{
			this.OnBindingComplete(args);
		}

		// Token: 0x04000608 RID: 1544
		private BindingsCollection bindings;

		// Token: 0x04000609 RID: 1545
		private bool pullingData;

		/// <summary>Specifies the event handler for the <see cref="E:System.Windows.Forms.BindingManagerBase.CurrentChanged" /> event.</summary>
		// Token: 0x0400060A RID: 1546
		protected EventHandler onCurrentChangedHandler;

		/// <summary>Specifies the event handler for the <see cref="E:System.Windows.Forms.BindingManagerBase.PositionChanged" /> event.</summary>
		// Token: 0x0400060B RID: 1547
		protected EventHandler onPositionChangedHandler;

		// Token: 0x0400060C RID: 1548
		private BindingCompleteEventHandler onBindingCompleteHandler;

		// Token: 0x0400060D RID: 1549
		internal EventHandler onCurrentItemChangedHandler;

		// Token: 0x0400060E RID: 1550
		internal BindingManagerDataErrorEventHandler onDataErrorHandler;
	}
}
