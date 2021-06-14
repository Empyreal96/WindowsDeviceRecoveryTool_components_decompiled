using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Manages the collection of <see cref="T:System.Windows.Forms.BindingManagerBase" /> objects for any object that inherits from the <see cref="T:System.Windows.Forms.Control" /> class.</summary>
	// Token: 0x02000124 RID: 292
	[DefaultEvent("CollectionChanged")]
	public class BindingContext : ICollection, IEnumerable
	{
		/// <summary>Gets the total number of <see cref="T:System.Windows.Forms.CurrencyManager" /> objects managed by the <see cref="T:System.Windows.Forms.BindingContext" />.</summary>
		/// <returns>The number of data sources managed by the <see cref="T:System.Windows.Forms.BindingContext" />.</returns>
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001765C File Offset: 0x0001585C
		int ICollection.Count
		{
			get
			{
				this.ScrubWeakRefs();
				return this.listManagers.Count;
			}
		}

		/// <summary>Copies the elements of the collection into a specified array, starting at the collection index.</summary>
		/// <param name="ar">An <see cref="T:System.Array" /> to copy into. </param>
		/// <param name="index">The collection index to begin copying from. </param>
		// Token: 0x060007BB RID: 1979 RVA: 0x0001766F File Offset: 0x0001586F
		void ICollection.CopyTo(Array ar, int index)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.ScrubWeakRefs();
			this.listManagers.CopyTo(ar, index);
		}

		/// <summary>Gets an enumerator for the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the collection.</returns>
		// Token: 0x060007BC RID: 1980 RVA: 0x0001768E File Offset: 0x0001588E
		IEnumerator IEnumerable.GetEnumerator()
		{
			IntSecurity.UnmanagedCode.Demand();
			this.ScrubWeakRefs();
			return this.listManagers.GetEnumerator();
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the collection is synchronized.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is thread safe; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object to use for synchronization (thread safety).</summary>
		/// <returns>This property is derived from <see cref="T:System.Collections.ICollection" />, and is overridden to always return <see langword="null" />.</returns>
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0000DE5C File Offset: 0x0000C05C
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingContext" /> class.</summary>
		// Token: 0x060007C0 RID: 1984 RVA: 0x000176AB File Offset: 0x000158AB
		public BindingContext()
		{
			this.listManagers = new Hashtable();
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.BindingManagerBase" /> that is associated with the specified data source.</summary>
		/// <param name="dataSource">The data source associated with a particular <see cref="T:System.Windows.Forms.BindingManagerBase" />. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.BindingManagerBase" /> for the specified data source.</returns>
		// Token: 0x17000252 RID: 594
		public BindingManagerBase this[object dataSource]
		{
			get
			{
				return this[dataSource, ""];
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Forms.BindingManagerBase" /> that is associated with the specified data source and data member.</summary>
		/// <param name="dataSource">The data source associated with a particular <see cref="T:System.Windows.Forms.BindingManagerBase" />. </param>
		/// <param name="dataMember">A navigation path containing the information that resolves to a specific <see cref="T:System.Windows.Forms.BindingManagerBase" />. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.BindingManagerBase" /> for the specified data source and data member.</returns>
		/// <exception cref="T:System.Exception">The specified <paramref name="dataMember" /> does not exist within the data source. </exception>
		// Token: 0x17000253 RID: 595
		public BindingManagerBase this[object dataSource, string dataMember]
		{
			get
			{
				return this.EnsureListManager(dataSource, dataMember);
			}
		}

		/// <summary>Adds the <see cref="T:System.Windows.Forms.BindingManagerBase" /> associated with a specific data source to the collection.</summary>
		/// <param name="dataSource">The <see cref="T:System.Object" /> associated with the <see cref="T:System.Windows.Forms.BindingManagerBase" />. </param>
		/// <param name="listManager">The <see cref="T:System.Windows.Forms.BindingManagerBase" /> to add. </param>
		// Token: 0x060007C3 RID: 1987 RVA: 0x000176D6 File Offset: 0x000158D6
		protected internal void Add(object dataSource, BindingManagerBase listManager)
		{
			this.AddCore(dataSource, listManager);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataSource));
		}

		/// <summary>Adds the <see cref="T:System.Windows.Forms.BindingManagerBase" /> associated with a specific data source to the collection.</summary>
		/// <param name="dataSource">The object associated with the <see cref="T:System.Windows.Forms.BindingManagerBase" />. </param>
		/// <param name="listManager">The <see cref="T:System.Windows.Forms.BindingManagerBase" /> to add.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataSource" /> is <see langword="null" />.-or-
		///         <paramref name="listManager" /> is <see langword="null" />.</exception>
		// Token: 0x060007C4 RID: 1988 RVA: 0x000176ED File Offset: 0x000158ED
		protected virtual void AddCore(object dataSource, BindingManagerBase listManager)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			if (listManager == null)
			{
				throw new ArgumentNullException("listManager");
			}
			this.listManagers[this.GetKey(dataSource, "")] = new WeakReference(listManager, false);
		}

		/// <summary>Always raises a <see cref="T:System.NotImplementedException" /> when handled.</summary>
		/// <exception cref="T:System.NotImplementedException">Occurs in all cases.</exception>
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x060007C5 RID: 1989 RVA: 0x00017729 File Offset: 0x00015929
		// (remove) Token: 0x060007C6 RID: 1990 RVA: 0x0000701A File Offset: 0x0000521A
		[SRDescription("collectionChangedEventDescr")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				throw new NotImplementedException();
			}
			remove
			{
			}
		}

		/// <summary>Clears the collection of any <see cref="T:System.Windows.Forms.BindingManagerBase" /> objects.</summary>
		// Token: 0x060007C7 RID: 1991 RVA: 0x00017730 File Offset: 0x00015930
		protected internal void Clear()
		{
			this.ClearCore();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Clears the collection.</summary>
		// Token: 0x060007C8 RID: 1992 RVA: 0x00017745 File Offset: 0x00015945
		protected virtual void ClearCore()
		{
			this.listManagers.Clear();
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.BindingContext" /> contains the <see cref="T:System.Windows.Forms.BindingManagerBase" /> associated with the specified data source.</summary>
		/// <param name="dataSource">An <see cref="T:System.Object" /> that represents the data source. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.BindingContext" /> contains the specified <see cref="T:System.Windows.Forms.BindingManagerBase" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060007C9 RID: 1993 RVA: 0x00017752 File Offset: 0x00015952
		public bool Contains(object dataSource)
		{
			return this.Contains(dataSource, "");
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.BindingContext" /> contains the <see cref="T:System.Windows.Forms.BindingManagerBase" /> associated with the specified data source and data member.</summary>
		/// <param name="dataSource">An <see cref="T:System.Object" /> that represents the data source. </param>
		/// <param name="dataMember">The information needed to resolve to a specific <see cref="T:System.Windows.Forms.BindingManagerBase" />. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.BindingContext" /> contains the specified <see cref="T:System.Windows.Forms.BindingManagerBase" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060007CA RID: 1994 RVA: 0x00017760 File Offset: 0x00015960
		public bool Contains(object dataSource, string dataMember)
		{
			return this.listManagers.ContainsKey(this.GetKey(dataSource, dataMember));
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00017775 File Offset: 0x00015975
		internal BindingContext.HashKey GetKey(object dataSource, string dataMember)
		{
			return new BindingContext.HashKey(dataSource, dataMember);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingContext.CollectionChanged" /> event.</summary>
		/// <param name="ccevent">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> that contains the event data.</param>
		// Token: 0x060007CC RID: 1996 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs ccevent)
		{
		}

		/// <summary>Deletes the <see cref="T:System.Windows.Forms.BindingManagerBase" /> associated with the specified data source.</summary>
		/// <param name="dataSource">The data source associated with the <see cref="T:System.Windows.Forms.BindingManagerBase" /> to remove. </param>
		// Token: 0x060007CD RID: 1997 RVA: 0x0001777E File Offset: 0x0001597E
		protected internal void Remove(object dataSource)
		{
			this.RemoveCore(dataSource);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataSource));
		}

		/// <summary>Removes the <see cref="T:System.Windows.Forms.BindingManagerBase" /> associated with the specified data source.</summary>
		/// <param name="dataSource">The data source associated with the <see cref="T:System.Windows.Forms.BindingManagerBase" /> to remove.</param>
		// Token: 0x060007CE RID: 1998 RVA: 0x00017794 File Offset: 0x00015994
		protected virtual void RemoveCore(object dataSource)
		{
			this.listManagers.Remove(this.GetKey(dataSource, ""));
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x000177B0 File Offset: 0x000159B0
		internal BindingManagerBase EnsureListManager(object dataSource, string dataMember)
		{
			BindingManagerBase bindingManagerBase = null;
			if (dataMember == null)
			{
				dataMember = "";
			}
			if (dataSource is ICurrencyManagerProvider)
			{
				bindingManagerBase = (dataSource as ICurrencyManagerProvider).GetRelatedCurrencyManager(dataMember);
				if (bindingManagerBase != null)
				{
					return bindingManagerBase;
				}
			}
			BindingContext.HashKey key = this.GetKey(dataSource, dataMember);
			WeakReference weakReference = this.listManagers[key] as WeakReference;
			if (weakReference != null)
			{
				bindingManagerBase = (BindingManagerBase)weakReference.Target;
			}
			if (bindingManagerBase != null)
			{
				return bindingManagerBase;
			}
			if (dataMember.Length == 0)
			{
				if (dataSource is IList || dataSource is IListSource)
				{
					bindingManagerBase = new CurrencyManager(dataSource);
				}
				else
				{
					bindingManagerBase = new PropertyManager(dataSource);
				}
			}
			else
			{
				int num = dataMember.LastIndexOf(".");
				string dataMember2 = (num == -1) ? "" : dataMember.Substring(0, num);
				string text = dataMember.Substring(num + 1);
				BindingManagerBase bindingManagerBase2 = this.EnsureListManager(dataSource, dataMember2);
				PropertyDescriptor propertyDescriptor = bindingManagerBase2.GetItemProperties().Find(text, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("RelatedListManagerChild", new object[]
					{
						text
					}));
				}
				if (typeof(IList).IsAssignableFrom(propertyDescriptor.PropertyType))
				{
					bindingManagerBase = new RelatedCurrencyManager(bindingManagerBase2, text);
				}
				else
				{
					bindingManagerBase = new RelatedPropertyManager(bindingManagerBase2, text);
				}
			}
			if (weakReference == null)
			{
				this.listManagers.Add(key, new WeakReference(bindingManagerBase, false));
			}
			else
			{
				weakReference.Target = bindingManagerBase;
			}
			IntSecurity.UnmanagedCode.Demand();
			this.ScrubWeakRefs();
			return bindingManagerBase;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00017908 File Offset: 0x00015B08
		private static void CheckPropertyBindingCycles(BindingContext newBindingContext, Binding propBinding)
		{
			if (newBindingContext == null || propBinding == null)
			{
				return;
			}
			if (newBindingContext.Contains(propBinding.BindableComponent, ""))
			{
				BindingManagerBase bindingManagerBase = newBindingContext.EnsureListManager(propBinding.BindableComponent, "");
				for (int i = 0; i < bindingManagerBase.Bindings.Count; i++)
				{
					Binding binding = bindingManagerBase.Bindings[i];
					if (binding.DataSource == propBinding.BindableComponent)
					{
						if (propBinding.BindToObject.BindingMemberInfo.BindingMember.Equals(binding.PropertyName))
						{
							throw new ArgumentException(SR.GetString("DataBindingCycle", new object[]
							{
								binding.PropertyName
							}), "propBinding");
						}
					}
					else if (propBinding.BindToObject.BindingManagerBase is PropertyManager)
					{
						BindingContext.CheckPropertyBindingCycles(newBindingContext, binding);
					}
				}
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000179D8 File Offset: 0x00015BD8
		private void ScrubWeakRefs()
		{
			ArrayList arrayList = null;
			foreach (object obj in this.listManagers)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				WeakReference weakReference = (WeakReference)dictionaryEntry.Value;
				if (weakReference.Target == null)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(dictionaryEntry.Key);
				}
			}
			if (arrayList != null)
			{
				foreach (object key in arrayList)
				{
					this.listManagers.Remove(key);
				}
			}
		}

		/// <summary>Associates a <see cref="T:System.Windows.Forms.Binding" /> with a new <see cref="T:System.Windows.Forms.BindingContext" />.</summary>
		/// <param name="newBindingContext">The new <see cref="T:System.Windows.Forms.BindingContext" /> to associate with the <see cref="T:System.Windows.Forms.Binding" />.</param>
		/// <param name="binding">The <see cref="T:System.Windows.Forms.Binding" /> to associate with the new <see cref="T:System.Windows.Forms.BindingContext" />.</param>
		// Token: 0x060007D2 RID: 2002 RVA: 0x00017AAC File Offset: 0x00015CAC
		public static void UpdateBinding(BindingContext newBindingContext, Binding binding)
		{
			BindingManagerBase bindingManagerBase = binding.BindingManagerBase;
			if (bindingManagerBase != null)
			{
				bindingManagerBase.Bindings.Remove(binding);
			}
			if (newBindingContext != null)
			{
				if (binding.BindToObject.BindingManagerBase is PropertyManager)
				{
					BindingContext.CheckPropertyBindingCycles(newBindingContext, binding);
				}
				BindToObject bindToObject = binding.BindToObject;
				BindingManagerBase bindingManagerBase2 = newBindingContext.EnsureListManager(bindToObject.DataSource, bindToObject.BindingMemberInfo.BindingPath);
				bindingManagerBase2.Bindings.Add(binding);
			}
		}

		// Token: 0x04000607 RID: 1543
		private Hashtable listManagers;

		// Token: 0x0200055E RID: 1374
		internal class HashKey
		{
			// Token: 0x06005627 RID: 22055 RVA: 0x001697E8 File Offset: 0x001679E8
			internal HashKey(object dataSource, string dataMember)
			{
				if (dataSource == null)
				{
					throw new ArgumentNullException("dataSource");
				}
				if (dataMember == null)
				{
					dataMember = "";
				}
				this.wRef = new WeakReference(dataSource, false);
				this.dataSourceHashCode = dataSource.GetHashCode();
				this.dataMember = dataMember.ToLower(CultureInfo.InvariantCulture);
			}

			// Token: 0x06005628 RID: 22056 RVA: 0x0016983D File Offset: 0x00167A3D
			public override int GetHashCode()
			{
				return this.dataSourceHashCode * this.dataMember.GetHashCode();
			}

			// Token: 0x06005629 RID: 22057 RVA: 0x00169854 File Offset: 0x00167A54
			public override bool Equals(object target)
			{
				if (target is BindingContext.HashKey)
				{
					BindingContext.HashKey hashKey = (BindingContext.HashKey)target;
					return this.wRef.Target == hashKey.wRef.Target && this.dataMember == hashKey.dataMember;
				}
				return false;
			}

			// Token: 0x040037E1 RID: 14305
			private WeakReference wRef;

			// Token: 0x040037E2 RID: 14306
			private int dataSourceHashCode;

			// Token: 0x040037E3 RID: 14307
			private string dataMember;
		}
	}
}
