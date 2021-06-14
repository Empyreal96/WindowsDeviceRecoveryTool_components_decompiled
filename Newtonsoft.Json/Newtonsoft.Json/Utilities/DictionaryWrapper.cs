using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000E0 RID: 224
	internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IWrappedDictionary, IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002B537 File Offset: 0x00029737
		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002B551 File Offset: 0x00029751
		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002B56B File Offset: 0x0002976B
		public DictionaryWrapper(IReadOnlyDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._readOnlyDictionary = dictionary;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002B585 File Offset: 0x00029785
		public void Add(TKey key, TValue value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(key, value);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002B5C2 File Offset: 0x000297C2
		public bool ContainsKey(TKey key)
		{
			if (this._dictionary != null)
			{
				return this._dictionary.Contains(key);
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.ContainsKey(key);
			}
			return this._genericDictionary.ContainsKey(key);
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0002B600 File Offset: 0x00029800
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Keys.Cast<TKey>().ToList<TKey>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Keys.ToList<TKey>();
				}
				return this._genericDictionary.Keys;
			}
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002B650 File Offset: 0x00029850
		public bool Remove(TKey key)
		{
			if (this._dictionary != null)
			{
				if (this._dictionary.Contains(key))
				{
					this._dictionary.Remove(key);
					return true;
				}
				return false;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.Remove(key);
			}
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002B6A8 File Offset: 0x000298A8
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._dictionary != null)
			{
				if (!this._dictionary.Contains(key))
				{
					value = default(TValue);
					return false;
				}
				value = (TValue)((object)this._dictionary[key]);
				return true;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.TryGetValue(key, out value);
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0002B714 File Offset: 0x00029914
		public ICollection<TValue> Values
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Values.Cast<TValue>().ToList<TValue>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Values.ToList<TValue>();
				}
				return this._genericDictionary.Values;
			}
		}

		// Token: 0x17000249 RID: 585
		public TValue this[TKey key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return (TValue)((object)this._dictionary[key]);
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary[key];
				}
				return this._genericDictionary[key];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this._genericDictionary[key] = value;
			}
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002B7F0 File Offset: 0x000299F0
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				((IList)this._dictionary).Add(item);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(item);
			}
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002B83F File Offset: 0x00029A3F
		public void Clear()
		{
			if (this._dictionary != null)
			{
				this._dictionary.Clear();
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Clear();
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002B870 File Offset: 0x00029A70
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				return ((IList)this._dictionary).Contains(item);
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.Contains(item);
			}
			return this._genericDictionary.Contains(item);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002B8C0 File Offset: 0x00029AC0
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._dictionary != null)
			{
				using (IDictionaryEnumerator enumerator = this._dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)dictionaryEntry.Key), (TValue)((object)dictionaryEntry.Value));
					}
					return;
				}
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0002B964 File Offset: 0x00029B64
		public int Count
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Count;
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Count;
				}
				return this._genericDictionary.Count;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0002B999 File Offset: 0x00029B99
		public bool IsReadOnly
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.IsReadOnly;
				}
				return this._readOnlyDictionary != null || this._genericDictionary.IsReadOnly;
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002B9C4 File Offset: 0x00029BC4
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				if (!this._dictionary.Contains(item.Key))
				{
					return true;
				}
				object objA = this._dictionary[item.Key];
				if (object.Equals(objA, item.Value))
				{
					this._dictionary.Remove(item.Key);
					return true;
				}
				return false;
			}
			else
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.Remove(item);
			}
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002BA74 File Offset: 0x00029C74
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return (from DictionaryEntry de in this._dictionary
				select new KeyValuePair<TKey, TValue>((TKey)((object)de.Key), (TValue)((object)de.Value))).GetEnumerator();
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.GetEnumerator();
			}
			return this._genericDictionary.GetEnumerator();
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0002BADB File Offset: 0x00029CDB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002BAE3 File Offset: 0x00029CE3
		void IDictionary.Add(object key, object value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x1700024C RID: 588
		object IDictionary.this[object key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary[(TKey)((object)key)];
				}
				return this._genericDictionary[(TKey)((object)key)];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this._genericDictionary[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002BBB4 File Offset: 0x00029DB4
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return this._dictionary.GetEnumerator();
			}
			if (this._readOnlyDictionary != null)
			{
				return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._readOnlyDictionary.GetEnumerator());
			}
			return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002BC08 File Offset: 0x00029E08
		bool IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)((object)key));
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.ContainsKey((TKey)((object)key));
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0002BC55 File Offset: 0x00029E55
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._genericDictionary == null && (this._readOnlyDictionary != null || this._dictionary.IsFixedSize);
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0002BC76 File Offset: 0x00029E76
		ICollection IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Keys.ToList<TKey>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Keys.ToList<TKey>();
				}
				return this._dictionary.Keys;
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002BCB5 File Offset: 0x00029EB5
		public void Remove(object key)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Remove(key);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Remove((TKey)((object)key));
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0002BCEC File Offset: 0x00029EEC
		ICollection IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Values.ToList<TValue>();
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Values.ToList<TValue>();
				}
				return this._dictionary.Values;
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0002BD2B File Offset: 0x00029F2B
		void ICollection.CopyTo(Array array, int index)
		{
			if (this._dictionary != null)
			{
				this._dictionary.CopyTo(array, index);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0002BD63 File Offset: 0x00029F63
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._dictionary != null && this._dictionary.IsSynchronized;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0002BD7A File Offset: 0x00029F7A
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0002BD9C File Offset: 0x00029F9C
		public object UnderlyingDictionary
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary;
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary;
				}
				return this._genericDictionary;
			}
		}

		// Token: 0x040003F1 RID: 1009
		private readonly IDictionary _dictionary;

		// Token: 0x040003F2 RID: 1010
		private readonly IDictionary<TKey, TValue> _genericDictionary;

		// Token: 0x040003F3 RID: 1011
		private readonly IReadOnlyDictionary<TKey, TValue> _readOnlyDictionary;

		// Token: 0x040003F4 RID: 1012
		private object _syncRoot;

		// Token: 0x020000E1 RID: 225
		private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06000AE9 RID: 2793 RVA: 0x0002BDC2 File Offset: 0x00029FC2
			public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			// Token: 0x17000253 RID: 595
			// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0002BDD6 File Offset: 0x00029FD6
			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			// Token: 0x17000254 RID: 596
			// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0002BDE4 File Offset: 0x00029FE4
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x17000255 RID: 597
			// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0002BE00 File Offset: 0x0002A000
			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x17000256 RID: 598
			// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002BE1C File Offset: 0x0002A01C
			public object Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair = this._e.Current;
					object key = keyValuePair.Key;
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> keyValuePair2 = this._e.Current;
					return new DictionaryEntry(key, keyValuePair2.Value);
				}
			}

			// Token: 0x06000AEE RID: 2798 RVA: 0x0002BE63 File Offset: 0x0002A063
			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			// Token: 0x06000AEF RID: 2799 RVA: 0x0002BE70 File Offset: 0x0002A070
			public void Reset()
			{
				this._e.Reset();
			}

			// Token: 0x040003F6 RID: 1014
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;
		}
	}
}
