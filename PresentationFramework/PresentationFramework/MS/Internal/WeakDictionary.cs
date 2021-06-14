using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal
{
	// Token: 0x020005F3 RID: 1523
	internal class WeakDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TKey : class
	{
		// Token: 0x06006555 RID: 25941 RVA: 0x001C6EFE File Offset: 0x001C50FE
		public void Add(TKey key, TValue value)
		{
			this._hashTable.SetWeak(key, value);
		}

		// Token: 0x06006556 RID: 25942 RVA: 0x001C6F17 File Offset: 0x001C5117
		public bool ContainsKey(TKey key)
		{
			return this._hashTable.ContainsKey(key);
		}

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x06006557 RID: 25943 RVA: 0x001C6F2A File Offset: 0x001C512A
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new WeakDictionary<TKey, TValue>.KeyCollection<TKey, TValue>(this);
				}
				return this._keys;
			}
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x001C6F46 File Offset: 0x001C5146
		public bool Remove(TKey key)
		{
			if (this._hashTable.ContainsKey(key))
			{
				this._hashTable.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x001C6F6F File Offset: 0x001C516F
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._hashTable.ContainsKey(key))
			{
				value = (TValue)((object)this._hashTable[key]);
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x0600655A RID: 25946 RVA: 0x001C6FAA File Offset: 0x001C51AA
		public ICollection<TValue> Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new WeakDictionary<TKey, TValue>.ValueCollection<TKey, TValue>(this);
				}
				return this._values;
			}
		}

		// Token: 0x17001849 RID: 6217
		public TValue this[TKey key]
		{
			get
			{
				if (!this._hashTable.ContainsKey(key))
				{
					throw new KeyNotFoundException();
				}
				return (TValue)((object)this._hashTable[key]);
			}
			set
			{
				this._hashTable.SetWeak(key, value);
			}
		}

		// Token: 0x0600655D RID: 25949 RVA: 0x001C6FF7 File Offset: 0x001C51F7
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x0600655E RID: 25950 RVA: 0x001C700D File Offset: 0x001C520D
		public void Clear()
		{
			this._hashTable.Clear();
		}

		// Token: 0x0600655F RID: 25951 RVA: 0x001C701C File Offset: 0x001C521C
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this._hashTable.ContainsKey(item.Key) && object.Equals(this._hashTable[item.Key], item.Value);
		}

		// Token: 0x06006560 RID: 25952 RVA: 0x001C7074 File Offset: 0x001C5274
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = 0;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
			{
				num++;
			}
			if (num + arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair2 in this)
			{
				array[arrayIndex++] = keyValuePair2;
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x06006561 RID: 25953 RVA: 0x001C712C File Offset: 0x001C532C
		public int Count
		{
			get
			{
				return this._hashTable.Count;
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x06006562 RID: 25954 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006563 RID: 25955 RVA: 0x001C7139 File Offset: 0x001C5339
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x06006564 RID: 25956 RVA: 0x001C7153 File Offset: 0x001C5353
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (object key in this._hashTable.Keys)
			{
				TKey tkey = this._hashTable.UnwrapKey(key) as TKey;
				if (tkey != null)
				{
					yield return new KeyValuePair<TKey, TValue>(tkey, (TValue)((object)this._hashTable[key]));
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006565 RID: 25957 RVA: 0x001C7162 File Offset: 0x001C5362
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040032C0 RID: 12992
		private IWeakHashtable _hashTable = WeakHashtable.FromKeyType(typeof(TKey));

		// Token: 0x040032C1 RID: 12993
		private WeakDictionary<TKey, TValue>.KeyCollection<TKey, TValue> _keys;

		// Token: 0x040032C2 RID: 12994
		private WeakDictionary<TKey, TValue>.ValueCollection<TKey, TValue> _values;

		// Token: 0x02000A0B RID: 2571
		private class KeyCollection<KeyType, ValueType> : ICollection<KeyType>, IEnumerable<KeyType>, IEnumerable where KeyType : class
		{
			// Token: 0x06008A11 RID: 35345 RVA: 0x00256B91 File Offset: 0x00254D91
			public KeyCollection(WeakDictionary<KeyType, ValueType> dict)
			{
				this.Dict = dict;
			}

			// Token: 0x17001F2B RID: 7979
			// (get) Token: 0x06008A12 RID: 35346 RVA: 0x00256BA0 File Offset: 0x00254DA0
			// (set) Token: 0x06008A13 RID: 35347 RVA: 0x00256BA8 File Offset: 0x00254DA8
			public WeakDictionary<KeyType, ValueType> Dict { get; private set; }

			// Token: 0x06008A14 RID: 35348 RVA: 0x0003E384 File Offset: 0x0003C584
			public void Add(KeyType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A15 RID: 35349 RVA: 0x0003E384 File Offset: 0x0003C584
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A16 RID: 35350 RVA: 0x00256BB1 File Offset: 0x00254DB1
			public bool Contains(KeyType item)
			{
				return this.Dict.ContainsKey(item);
			}

			// Token: 0x06008A17 RID: 35351 RVA: 0x0003E384 File Offset: 0x0003C584
			public void CopyTo(KeyType[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x17001F2C RID: 7980
			// (get) Token: 0x06008A18 RID: 35352 RVA: 0x00256BBF File Offset: 0x00254DBF
			public int Count
			{
				get
				{
					return this.Dict.Count;
				}
			}

			// Token: 0x17001F2D RID: 7981
			// (get) Token: 0x06008A19 RID: 35353 RVA: 0x00016748 File Offset: 0x00014948
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06008A1A RID: 35354 RVA: 0x0003E384 File Offset: 0x0003C584
			public bool Remove(KeyType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A1B RID: 35355 RVA: 0x00256BCC File Offset: 0x00254DCC
			public IEnumerator<KeyType> GetEnumerator()
			{
				IWeakHashtable hashTable = this.Dict._hashTable;
				foreach (object key in hashTable.Keys)
				{
					KeyType keyType = hashTable.UnwrapKey(key) as KeyType;
					if (keyType != null)
					{
						yield return keyType;
					}
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x06008A1C RID: 35356 RVA: 0x00256BDB File Offset: 0x00254DDB
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}

		// Token: 0x02000A0C RID: 2572
		private class ValueCollection<KeyType, ValueType> : ICollection<ValueType>, IEnumerable<ValueType>, IEnumerable where KeyType : class
		{
			// Token: 0x06008A1D RID: 35357 RVA: 0x00256BE3 File Offset: 0x00254DE3
			public ValueCollection(WeakDictionary<KeyType, ValueType> dict)
			{
				this.Dict = dict;
			}

			// Token: 0x17001F2E RID: 7982
			// (get) Token: 0x06008A1E RID: 35358 RVA: 0x00256BF2 File Offset: 0x00254DF2
			// (set) Token: 0x06008A1F RID: 35359 RVA: 0x00256BFA File Offset: 0x00254DFA
			public WeakDictionary<KeyType, ValueType> Dict { get; private set; }

			// Token: 0x06008A20 RID: 35360 RVA: 0x0003E384 File Offset: 0x0003C584
			public void Add(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A21 RID: 35361 RVA: 0x0003E384 File Offset: 0x0003C584
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A22 RID: 35362 RVA: 0x0003E384 File Offset: 0x0003C584
			public bool Contains(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A23 RID: 35363 RVA: 0x0003E384 File Offset: 0x0003C584
			public void CopyTo(ValueType[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x17001F2F RID: 7983
			// (get) Token: 0x06008A24 RID: 35364 RVA: 0x00256C03 File Offset: 0x00254E03
			public int Count
			{
				get
				{
					return this.Dict.Count;
				}
			}

			// Token: 0x17001F30 RID: 7984
			// (get) Token: 0x06008A25 RID: 35365 RVA: 0x00016748 File Offset: 0x00014948
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06008A26 RID: 35366 RVA: 0x0003E384 File Offset: 0x0003C584
			public bool Remove(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A27 RID: 35367 RVA: 0x00256C10 File Offset: 0x00254E10
			public IEnumerator<ValueType> GetEnumerator()
			{
				IWeakHashtable hashTable = this.Dict._hashTable;
				foreach (object key in hashTable.Keys)
				{
					KeyType keyType = hashTable.UnwrapKey(key) as KeyType;
					if (keyType != null)
					{
						yield return (ValueType)((object)hashTable[key]);
					}
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x06008A28 RID: 35368 RVA: 0x00256C1F File Offset: 0x00254E1F
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}
