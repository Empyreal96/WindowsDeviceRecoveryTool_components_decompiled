using System;
using System.Collections.Generic;
using MS.Internal;
using MS.Internal.Automation;
using MS.Internal.Hashing.PresentationFramework;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002C6 RID: 710
	internal class ItemPeersStorage<T> where T : class
	{
		// Token: 0x0600274C RID: 10060 RVA: 0x000B9BB3 File Offset: 0x000B7DB3
		public void Clear()
		{
			this._usesHashCode = false;
			this._count = 0;
			if (this._hashtable != null)
			{
				this._hashtable.Clear();
			}
			if (this._list != null)
			{
				this._list.Clear();
			}
		}

		// Token: 0x1700099B RID: 2459
		public T this[object item]
		{
			get
			{
				if (this._count == 0 || item == null)
				{
					return default(T);
				}
				if (this._usesHashCode)
				{
					if (this._hashtable == null || !this._hashtable.ContainsKey(item))
					{
						return default(T);
					}
					return this._hashtable[item];
				}
				else
				{
					if (this._list == null)
					{
						return default(T);
					}
					for (int i = 0; i < this._list.Count; i++)
					{
						KeyValuePair<object, T> keyValuePair = this._list[i];
						if (object.Equals(item, keyValuePair.Key))
						{
							return keyValuePair.Value;
						}
					}
					return default(T);
				}
			}
			set
			{
				if (item == null)
				{
					return;
				}
				if (this._count == 0)
				{
					this._usesHashCode = (item != null && HashHelper.HasReliableHashCode(item));
				}
				if (this._usesHashCode)
				{
					if (this._hashtable == null)
					{
						this._hashtable = new WeakDictionary<object, T>();
					}
					if (!this._hashtable.ContainsKey(item) && value != null)
					{
						this._hashtable[item] = value;
					}
				}
				else
				{
					if (this._list == null)
					{
						this._list = new List<KeyValuePair<object, T>>();
					}
					if (value != null)
					{
						this._list.Add(new KeyValuePair<object, T>(item, value));
					}
				}
				this._count++;
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000B9D40 File Offset: 0x000B7F40
		public void Remove(object item)
		{
			if (this._usesHashCode)
			{
				if (item != null && this._hashtable.ContainsKey(item))
				{
					this._hashtable.Remove(item);
					if (!this._hashtable.ContainsKey(item))
					{
						this._count--;
						return;
					}
				}
			}
			else if (this._list != null)
			{
				int num = 0;
				while (num < this._list.Count && !object.Equals(item, this._list[num].Key))
				{
					num++;
				}
				if (num < this._list.Count)
				{
					this._list.RemoveAt(num);
					this._count--;
				}
			}
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000B9DFC File Offset: 0x000B7FFC
		public void PurgeWeakRefCollection()
		{
			if (!typeof(T).IsAssignableFrom(typeof(WeakReference)))
			{
				return;
			}
			List<object> list = new List<object>();
			if (this._usesHashCode)
			{
				if (this._hashtable == null)
				{
					return;
				}
				using (IEnumerator<KeyValuePair<object, T>> enumerator = this._hashtable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<object, T> keyValuePair = enumerator.Current;
						WeakReference weakReference = keyValuePair.Value as WeakReference;
						if (weakReference == null)
						{
							list.Add(keyValuePair.Key);
						}
						else
						{
							ElementProxy elementProxy = weakReference.Target as ElementProxy;
							if (elementProxy == null)
							{
								list.Add(keyValuePair.Key);
							}
							else if (!(elementProxy.Peer is ItemAutomationPeer))
							{
								list.Add(keyValuePair.Key);
							}
						}
					}
					goto IL_165;
				}
			}
			if (this._list == null)
			{
				return;
			}
			foreach (KeyValuePair<object, T> keyValuePair2 in this._list)
			{
				WeakReference weakReference2 = keyValuePair2.Value as WeakReference;
				if (weakReference2 == null)
				{
					list.Add(keyValuePair2.Key);
				}
				else
				{
					ElementProxy elementProxy2 = weakReference2.Target as ElementProxy;
					if (elementProxy2 == null)
					{
						list.Add(keyValuePair2.Key);
					}
					else if (!(elementProxy2.Peer is ItemAutomationPeer))
					{
						list.Add(keyValuePair2.Key);
					}
				}
			}
			IL_165:
			foreach (object item in list)
			{
				this.Remove(item);
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x000B9FCC File Offset: 0x000B81CC
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x04001B8A RID: 7050
		private WeakDictionary<object, T> _hashtable;

		// Token: 0x04001B8B RID: 7051
		private List<KeyValuePair<object, T>> _list;

		// Token: 0x04001B8C RID: 7052
		private int _count;

		// Token: 0x04001B8D RID: 7053
		private bool _usesHashCode;
	}
}
