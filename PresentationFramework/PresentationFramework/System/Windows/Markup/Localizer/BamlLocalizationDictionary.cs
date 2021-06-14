using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Contains all the localizable resources in a BAML record. </summary>
	// Token: 0x02000291 RID: 657
	public sealed class BamlLocalizationDictionary : IDictionary, ICollection, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> class.</summary>
		// Token: 0x060024EE RID: 9454 RVA: 0x000B2BB1 File Offset: 0x000B0DB1
		public BamlLocalizationDictionary()
		{
			this._dictionary = new Dictionary<BamlLocalizableResourceKey, BamlLocalizableResource>();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> object has a fixed size. </summary>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060024EF RID: 9455 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> object is read-only. </summary>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the key of the root element, if it is localizable. </summary>
		/// <returns>The key of the root element, if it is localizable. Otherwise, the value is set to <see langword="null" />.</returns>
		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000B2BC4 File Offset: 0x000B0DC4
		public BamlLocalizableResourceKey RootElementKey
		{
			get
			{
				return this._rootElementKey;
			}
		}

		/// <summary>Gets a collection that contains all the keys in the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> object. </summary>
		/// <returns>A collection that contains all the keys in the object.</returns>
		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060024F2 RID: 9458 RVA: 0x000B2BCC File Offset: 0x000B0DCC
		public ICollection Keys
		{
			get
			{
				return ((IDictionary)this._dictionary).Keys;
			}
		}

		/// <summary>Gets a collection that contains all the values in the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />. </summary>
		/// <returns>A collection that contains all the values in the object.</returns>
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x000B2BDE File Offset: 0x000B0DDE
		public ICollection Values
		{
			get
			{
				return ((IDictionary)this._dictionary).Values;
			}
		}

		/// <summary>Gets or sets a localizable resource specified by its key.</summary>
		/// <param name="key">The key value of the resource.</param>
		/// <returns>The value of the resource.</returns>
		// Token: 0x17000928 RID: 2344
		public BamlLocalizableResource this[BamlLocalizableResourceKey key]
		{
			get
			{
				this.CheckNonNullParam(key, "key");
				return this._dictionary[key];
			}
			set
			{
				this.CheckNonNullParam(key, "key");
				this._dictionary[key] = value;
			}
		}

		/// <summary>Adds an item with the provided key and value to the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</summary>
		/// <param name="key">A key for the resource.</param>
		/// <param name="value">An object that contains the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="key" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">An item with the same key already exists.</exception>
		// Token: 0x060024F6 RID: 9462 RVA: 0x000B2C25 File Offset: 0x000B0E25
		public void Add(BamlLocalizableResourceKey key, BamlLocalizableResource value)
		{
			this.CheckNonNullParam(key, "key");
			this._dictionary.Add(key, value);
		}

		/// <summary>Deletes all resources from the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> object.</summary>
		// Token: 0x060024F7 RID: 9463 RVA: 0x000B2C40 File Offset: 0x000B0E40
		public void Clear()
		{
			this._dictionary.Clear();
		}

		/// <summary>Removes a specified localizable resource from the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</summary>
		/// <param name="key">The key for the resource to be removed.</param>
		/// <exception cref="T:System.ArgumentNullException">key is <see langword="null" />.</exception>
		// Token: 0x060024F8 RID: 9464 RVA: 0x000B2C4D File Offset: 0x000B0E4D
		public void Remove(BamlLocalizableResourceKey key)
		{
			this._dictionary.Remove(key);
		}

		/// <summary>Determines whether a <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> object contains a resource with a specified key.</summary>
		/// <param name="key">The resource key to find.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> object contains a resource with the specified key; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">key is <see langword="null" />.</exception>
		// Token: 0x060024F9 RID: 9465 RVA: 0x000B2C5C File Offset: 0x000B0E5C
		public bool Contains(BamlLocalizableResourceKey key)
		{
			this.CheckNonNullParam(key, "key");
			return this._dictionary.ContainsKey(key);
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />. </summary>
		/// <returns>A specialized <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionaryEnumerator" /> that can iterate the contents of the dictionary. </returns>
		// Token: 0x060024FA RID: 9466 RVA: 0x000B2C76 File Offset: 0x000B0E76
		public BamlLocalizationDictionaryEnumerator GetEnumerator()
		{
			return new BamlLocalizationDictionaryEnumerator(((IDictionary)this._dictionary).GetEnumerator());
		}

		/// <summary>Gets the number of localizable resources in the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</summary>
		/// <returns>The number of localizable resources.</returns>
		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x000B2C8D File Offset: 0x000B0E8D
		public int Count
		{
			get
			{
				return this._dictionary.Count;
			}
		}

		/// <summary>Copies the contents of a <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> object to a one-dimensional array of <see cref="T:System.Collections.DictionaryEntry" /> objects, starting at a specified index. </summary>
		/// <param name="array">An array of objects to hold the data.</param>
		/// <param name="arrayIndex">The starting index value.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="arrayIndex" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="arrayIndex" /> exceeds the destination array length.-or-Copy cannot fit in the remaining array space between <paramref name="arrayIndex" /> and the destination array length.</exception>
		// Token: 0x060024FC RID: 9468 RVA: 0x000B2C9C File Offset: 0x000B0E9C
		public void CopyTo(DictionaryEntry[] array, int arrayIndex)
		{
			this.CheckNonNullParam(array, "array");
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", SR.Get("ParameterCannotBeNegative"));
			}
			if (arrayIndex >= array.Length)
			{
				throw new ArgumentException(SR.Get("Collection_CopyTo_IndexGreaterThanOrEqualToArrayLength", new object[]
				{
					"arrayIndex",
					"array"
				}), "arrayIndex");
			}
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException(SR.Get("Collection_CopyTo_NumberOfElementsExceedsArrayLength", new object[]
				{
					"arrayIndex",
					"array"
				}));
			}
			foreach (KeyValuePair<BamlLocalizableResourceKey, BamlLocalizableResource> keyValuePair in this._dictionary)
			{
				DictionaryEntry dictionaryEntry = new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
				array[arrayIndex++] = dictionaryEntry;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IDictionary.Contains(System.Object)" />.</summary>
		/// <param name="key">The key  to locate in the dictionary.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060024FD RID: 9469 RVA: 0x000B2D90 File Offset: 0x000B0F90
		bool IDictionary.Contains(object key)
		{
			this.CheckNonNullParam(key, "key");
			return ((IDictionary)this._dictionary).Contains(key);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IDictionary.Add(System.Object,System.Object)" />.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The object value to add to the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</param>
		// Token: 0x060024FE RID: 9470 RVA: 0x000B2DAF File Offset: 0x000B0FAF
		void IDictionary.Add(object key, object value)
		{
			this.CheckNonNullParam(key, "key");
			((IDictionary)this._dictionary).Add(key, value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IDictionary.Remove(System.Object)" />.</summary>
		/// <param name="key">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</param>
		// Token: 0x060024FF RID: 9471 RVA: 0x000B2DCF File Offset: 0x000B0FCF
		void IDictionary.Remove(object key)
		{
			this.CheckNonNullParam(key, "key");
			((IDictionary)this._dictionary).Remove(key);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IDictionary.Item(System.Object)" />.</summary>
		/// <param name="key">The key of the item to get or set. </param>
		/// <returns>The item with the specified key.</returns>
		// Token: 0x1700092A RID: 2346
		object IDictionary.this[object key]
		{
			get
			{
				this.CheckNonNullParam(key, "key");
				return ((IDictionary)this._dictionary)[key];
			}
			set
			{
				this.CheckNonNullParam(key, "key");
				((IDictionary)this._dictionary)[key] = value;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IDictionary.GetEnumerator" />.</summary>
		/// <returns>An enumerator object that can be used to iterate through the collection.</returns>
		// Token: 0x06002502 RID: 9474 RVA: 0x000B2E2D File Offset: 0x000B102D
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
		/// <param name="array">A zero-based <see cref="T:System.Array" /> that receives the copied items.</param>
		/// <param name="index">The first position in the specified <see cref="T:System.Array" /> to receive the copied contents.</param>
		// Token: 0x06002503 RID: 9475 RVA: 0x000B2E35 File Offset: 0x000B1035
		void ICollection.CopyTo(Array array, int index)
		{
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException(SR.Get("Collection_CopyTo_ArrayCannotBeMultidimensional"), "array");
			}
			this.CopyTo(array as DictionaryEntry[], index);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.Count" />.</summary>
		/// <returns>The number of items in the collection.</returns>
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x000B2E65 File Offset: 0x000B1065
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" />.</returns>
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06002505 RID: 9477 RVA: 0x000B2E6D File Offset: 0x000B106D
		object ICollection.SyncRoot
		{
			get
			{
				return ((IDictionary)this._dictionary).SyncRoot;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
		/// <returns>
		///     <see langword="true" /> if access to the <see cref="T:System.Windows.Markup.Localizer.BamlLocalizationDictionary" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x000B2E7F File Offset: 0x000B107F
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((IDictionary)this._dictionary).IsSynchronized;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06002507 RID: 9479 RVA: 0x000B2E2D File Offset: 0x000B102D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x000B2E94 File Offset: 0x000B1094
		internal BamlLocalizationDictionary Copy()
		{
			BamlLocalizationDictionary bamlLocalizationDictionary = new BamlLocalizationDictionary();
			foreach (KeyValuePair<BamlLocalizableResourceKey, BamlLocalizableResource> keyValuePair in this._dictionary)
			{
				BamlLocalizableResource value = (keyValuePair.Value == null) ? null : new BamlLocalizableResource(keyValuePair.Value);
				bamlLocalizationDictionary.Add(keyValuePair.Key, value);
			}
			bamlLocalizationDictionary._rootElementKey = this._rootElementKey;
			return bamlLocalizationDictionary;
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000B2F14 File Offset: 0x000B1114
		internal void SetRootElementKey(BamlLocalizableResourceKey key)
		{
			this._rootElementKey = key;
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000B2F1D File Offset: 0x000B111D
		private void CheckNonNullParam(object param, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		// Token: 0x04001B51 RID: 6993
		private IDictionary<BamlLocalizableResourceKey, BamlLocalizableResource> _dictionary;

		// Token: 0x04001B52 RID: 6994
		private BamlLocalizableResourceKey _rootElementKey;
	}
}
