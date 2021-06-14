using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000074 RID: 116
	internal class JPropertyKeyedCollection : Collection<JToken>
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x00018D09 File Offset: 0x00016F09
		private void AddKey(string key, JToken item)
		{
			this.EnsureDictionary();
			this._dictionary[key] = item;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00018D20 File Offset: 0x00016F20
		protected void ChangeItemKey(JToken item, string newKey)
		{
			if (!this.ContainsItem(item))
			{
				throw new ArgumentException("The specified item does not exist in this KeyedCollection.");
			}
			string keyForItem = this.GetKeyForItem(item);
			if (!JPropertyKeyedCollection.Comparer.Equals(keyForItem, newKey))
			{
				if (newKey != null)
				{
					this.AddKey(newKey, item);
				}
				if (keyForItem != null)
				{
					this.RemoveKey(keyForItem);
				}
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00018D6C File Offset: 0x00016F6C
		protected override void ClearItems()
		{
			base.ClearItems();
			if (this._dictionary != null)
			{
				this._dictionary.Clear();
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00018D87 File Offset: 0x00016F87
		public bool Contains(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this._dictionary != null && this._dictionary.ContainsKey(key);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00018DB0 File Offset: 0x00016FB0
		private bool ContainsItem(JToken item)
		{
			if (this._dictionary == null)
			{
				return false;
			}
			string keyForItem = this.GetKeyForItem(item);
			JToken jtoken;
			return this._dictionary.TryGetValue(keyForItem, out jtoken);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00018DDD File Offset: 0x00016FDD
		private void EnsureDictionary()
		{
			if (this._dictionary == null)
			{
				this._dictionary = new Dictionary<string, JToken>(JPropertyKeyedCollection.Comparer);
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00018DF7 File Offset: 0x00016FF7
		private string GetKeyForItem(JToken item)
		{
			return ((JProperty)item).Name;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00018E04 File Offset: 0x00017004
		protected override void InsertItem(int index, JToken item)
		{
			this.AddKey(this.GetKeyForItem(item), item);
			base.InsertItem(index, item);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00018E1C File Offset: 0x0001701C
		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this._dictionary != null && this._dictionary.ContainsKey(key) && base.Remove(this._dictionary[key]);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00018E58 File Offset: 0x00017058
		protected override void RemoveItem(int index)
		{
			string keyForItem = this.GetKeyForItem(base.Items[index]);
			this.RemoveKey(keyForItem);
			base.RemoveItem(index);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00018E86 File Offset: 0x00017086
		private void RemoveKey(string key)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Remove(key);
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00018EA0 File Offset: 0x000170A0
		protected override void SetItem(int index, JToken item)
		{
			string keyForItem = this.GetKeyForItem(item);
			string keyForItem2 = this.GetKeyForItem(base.Items[index]);
			if (JPropertyKeyedCollection.Comparer.Equals(keyForItem2, keyForItem))
			{
				if (this._dictionary != null)
				{
					this._dictionary[keyForItem] = item;
				}
			}
			else
			{
				this.AddKey(keyForItem, item);
				if (keyForItem2 != null)
				{
					this.RemoveKey(keyForItem2);
				}
			}
			base.SetItem(index, item);
		}

		// Token: 0x1700014C RID: 332
		public JToken this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				throw new KeyNotFoundException();
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00018F31 File Offset: 0x00017131
		public bool TryGetValue(string key, out JToken value)
		{
			if (this._dictionary == null)
			{
				value = null;
				return false;
			}
			return this._dictionary.TryGetValue(key, out value);
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00018F4D File Offset: 0x0001714D
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this._dictionary.Keys;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00018F60 File Offset: 0x00017160
		public ICollection<JToken> Values
		{
			get
			{
				this.EnsureDictionary();
				return this._dictionary.Values;
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00018F74 File Offset: 0x00017174
		public bool Compare(JPropertyKeyedCollection other)
		{
			if (this == other)
			{
				return true;
			}
			Dictionary<string, JToken> dictionary = this._dictionary;
			Dictionary<string, JToken> dictionary2 = other._dictionary;
			if (dictionary == null && dictionary2 == null)
			{
				return true;
			}
			if (dictionary == null)
			{
				return dictionary2.Count == 0;
			}
			if (dictionary2 == null)
			{
				return dictionary.Count == 0;
			}
			if (dictionary.Count != dictionary2.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in dictionary)
			{
				JToken jtoken;
				if (!dictionary2.TryGetValue(keyValuePair.Key, out jtoken))
				{
					return false;
				}
				JProperty jproperty = (JProperty)keyValuePair.Value;
				JProperty jproperty2 = (JProperty)jtoken;
				if (jproperty.Value == null)
				{
					return jproperty2.Value == null;
				}
				if (!jproperty.Value.DeepEquals(jproperty2.Value))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040001D0 RID: 464
		private static readonly IEqualityComparer<string> Comparer = StringComparer.Ordinal;

		// Token: 0x040001D1 RID: 465
		private Dictionary<string, JToken> _dictionary;
	}
}
