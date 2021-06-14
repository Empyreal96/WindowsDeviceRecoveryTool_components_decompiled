using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace MS.Internal.Annotations
{
	// Token: 0x020007CC RID: 1996
	internal class ObservableDictionary : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable, INotifyPropertyChanged
	{
		// Token: 0x06007B9C RID: 31644 RVA: 0x0022BCAD File Offset: 0x00229EAD
		public ObservableDictionary()
		{
			this._nameValues = new Dictionary<string, string>();
		}

		// Token: 0x06007B9D RID: 31645 RVA: 0x0022BCC0 File Offset: 0x00229EC0
		public void Add(string key, string val)
		{
			if (key == null || val == null)
			{
				throw new ArgumentNullException((key == null) ? "key" : "val");
			}
			this._nameValues.Add(key, val);
			this.FireDictionaryChanged();
		}

		// Token: 0x06007B9E RID: 31646 RVA: 0x0022BCF0 File Offset: 0x00229EF0
		public void Clear()
		{
			int count = this._nameValues.Count;
			if (count > 0)
			{
				this._nameValues.Clear();
				this.FireDictionaryChanged();
			}
		}

		// Token: 0x06007B9F RID: 31647 RVA: 0x0022BD1E File Offset: 0x00229F1E
		public bool ContainsKey(string key)
		{
			return this._nameValues.ContainsKey(key);
		}

		// Token: 0x06007BA0 RID: 31648 RVA: 0x0022BD2C File Offset: 0x00229F2C
		public bool Remove(string key)
		{
			bool flag = this._nameValues.Remove(key);
			if (flag)
			{
				this.FireDictionaryChanged();
			}
			return flag;
		}

		// Token: 0x06007BA1 RID: 31649 RVA: 0x0022BD50 File Offset: 0x00229F50
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._nameValues.GetEnumerator();
		}

		// Token: 0x06007BA2 RID: 31650 RVA: 0x0022BD62 File Offset: 0x00229F62
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, string>>)this._nameValues).GetEnumerator();
		}

		// Token: 0x06007BA3 RID: 31651 RVA: 0x0022BD6F File Offset: 0x00229F6F
		public bool TryGetValue(string key, out string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this._nameValues.TryGetValue(key, out value);
		}

		// Token: 0x06007BA4 RID: 31652 RVA: 0x0022BD8C File Offset: 0x00229F8C
		void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> pair)
		{
			((ICollection<KeyValuePair<string, string>>)this._nameValues).Add(pair);
		}

		// Token: 0x06007BA5 RID: 31653 RVA: 0x0022BD9A File Offset: 0x00229F9A
		bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> pair)
		{
			return ((ICollection<KeyValuePair<string, string>>)this._nameValues).Contains(pair);
		}

		// Token: 0x06007BA6 RID: 31654 RVA: 0x0022BDA8 File Offset: 0x00229FA8
		bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> pair)
		{
			return ((ICollection<KeyValuePair<string, string>>)this._nameValues).Remove(pair);
		}

		// Token: 0x06007BA7 RID: 31655 RVA: 0x0022BDB6 File Offset: 0x00229FB6
		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] target, int startIndex)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (startIndex < 0 || startIndex > target.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			((ICollection<KeyValuePair<string, string>>)this._nameValues).CopyTo(target, startIndex);
		}

		// Token: 0x17001CD8 RID: 7384
		// (get) Token: 0x06007BA8 RID: 31656 RVA: 0x0022BDE8 File Offset: 0x00229FE8
		public int Count
		{
			get
			{
				return this._nameValues.Count;
			}
		}

		// Token: 0x17001CD9 RID: 7385
		public string this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				string result = null;
				this._nameValues.TryGetValue(key, out result);
				return result;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				string text = null;
				this._nameValues.TryGetValue(key, out text);
				if (text == null || text != value)
				{
					this._nameValues[key] = value;
					this.FireDictionaryChanged();
				}
			}
		}

		// Token: 0x17001CDA RID: 7386
		// (get) Token: 0x06007BAB RID: 31659 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001CDB RID: 7387
		// (get) Token: 0x06007BAC RID: 31660 RVA: 0x0022BE81 File Offset: 0x0022A081
		public ICollection<string> Keys
		{
			get
			{
				return this._nameValues.Keys;
			}
		}

		// Token: 0x17001CDC RID: 7388
		// (get) Token: 0x06007BAD RID: 31661 RVA: 0x0022BE8E File Offset: 0x0022A08E
		public ICollection<string> Values
		{
			get
			{
				return this._nameValues.Values;
			}
		}

		// Token: 0x1400016D RID: 365
		// (add) Token: 0x06007BAE RID: 31662 RVA: 0x0022BE9C File Offset: 0x0022A09C
		// (remove) Token: 0x06007BAF RID: 31663 RVA: 0x0022BED4 File Offset: 0x0022A0D4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06007BB0 RID: 31664 RVA: 0x0022BF09 File Offset: 0x0022A109
		private void FireDictionaryChanged()
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(null));
			}
		}

		// Token: 0x04003A32 RID: 14898
		private Dictionary<string, string> _nameValues;
	}
}
