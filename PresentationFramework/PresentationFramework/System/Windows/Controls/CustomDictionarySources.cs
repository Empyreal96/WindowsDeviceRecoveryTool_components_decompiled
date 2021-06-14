using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000490 RID: 1168
	internal class CustomDictionarySources : IList<Uri>, ICollection<Uri>, IEnumerable<Uri>, IEnumerable, IList, ICollection
	{
		// Token: 0x060044C7 RID: 17607 RVA: 0x0013852F File Offset: 0x0013672F
		internal CustomDictionarySources(TextBoxBase owner)
		{
			this._owner = owner;
			this._uriList = new List<Uri>();
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x00138549 File Offset: 0x00136749
		public IEnumerator<Uri> GetEnumerator()
		{
			return this._uriList.GetEnumerator();
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x00138549 File Offset: 0x00136749
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._uriList.GetEnumerator();
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x0013855B File Offset: 0x0013675B
		int IList<Uri>.IndexOf(Uri item)
		{
			return this._uriList.IndexOf(item);
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x0013856C File Offset: 0x0013676C
		void IList<Uri>.Insert(int index, Uri item)
		{
			if (this._uriList.Contains(item))
			{
				throw new ArgumentException(SR.Get("CustomDictionaryItemAlreadyExists"), "item");
			}
			CustomDictionarySources.ValidateUri(item);
			this._uriList.Insert(index, item);
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriAdded(item);
			}
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x001385C4 File Offset: 0x001367C4
		void IList<Uri>.RemoveAt(int index)
		{
			Uri uri = this._uriList[index];
			this._uriList.RemoveAt(index);
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriRemoved(uri);
			}
		}

		// Token: 0x170010E2 RID: 4322
		Uri IList<Uri>.this[int index]
		{
			get
			{
				return this._uriList[index];
			}
			set
			{
				CustomDictionarySources.ValidateUri(value);
				Uri uri = this._uriList[index];
				if (this.Speller != null)
				{
					this.Speller.OnDictionaryUriRemoved(uri);
				}
				this._uriList[index] = value;
				if (this.Speller != null)
				{
					this.Speller.OnDictionaryUriAdded(value);
				}
			}
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x00138661 File Offset: 0x00136861
		void ICollection<Uri>.Add(Uri item)
		{
			CustomDictionarySources.ValidateUri(item);
			if (!this._uriList.Contains(item))
			{
				this._uriList.Add(item);
			}
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriAdded(item);
			}
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x00138697 File Offset: 0x00136897
		void ICollection<Uri>.Clear()
		{
			this._uriList.Clear();
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriCollectionCleared();
			}
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x001386B7 File Offset: 0x001368B7
		bool ICollection<Uri>.Contains(Uri item)
		{
			return this._uriList.Contains(item);
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x001386C5 File Offset: 0x001368C5
		void ICollection<Uri>.CopyTo(Uri[] array, int arrayIndex)
		{
			this._uriList.CopyTo(array, arrayIndex);
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x060044D3 RID: 17619 RVA: 0x001386D4 File Offset: 0x001368D4
		int ICollection<Uri>.Count
		{
			get
			{
				return this._uriList.Count;
			}
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x060044D4 RID: 17620 RVA: 0x001386E1 File Offset: 0x001368E1
		bool ICollection<Uri>.IsReadOnly
		{
			get
			{
				return ((ICollection<Uri>)this._uriList).IsReadOnly;
			}
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x001386F0 File Offset: 0x001368F0
		bool ICollection<Uri>.Remove(Uri item)
		{
			bool flag = this._uriList.Remove(item);
			if (flag && this.Speller != null)
			{
				this.Speller.OnDictionaryUriRemoved(item);
			}
			return flag;
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x00138722 File Offset: 0x00136922
		int IList.Add(object value)
		{
			((ICollection<Uri>)this).Add((Uri)value);
			return this._uriList.IndexOf((Uri)value);
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x00138741 File Offset: 0x00136941
		void IList.Clear()
		{
			((ICollection<Uri>)this).Clear();
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x00138749 File Offset: 0x00136949
		bool IList.Contains(object value)
		{
			return ((IList)this._uriList).Contains(value);
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x00138757 File Offset: 0x00136957
		int IList.IndexOf(object value)
		{
			return ((IList)this._uriList).IndexOf(value);
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x00138765 File Offset: 0x00136965
		void IList.Insert(int index, object value)
		{
			((IList<Uri>)this).Insert(index, (Uri)value);
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x060044DB RID: 17627 RVA: 0x00138774 File Offset: 0x00136974
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._uriList).IsFixedSize;
			}
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x060044DC RID: 17628 RVA: 0x00138781 File Offset: 0x00136981
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._uriList).IsReadOnly;
			}
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x0013878E File Offset: 0x0013698E
		void IList.Remove(object value)
		{
			((ICollection<Uri>)this).Remove((Uri)value);
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x0013879D File Offset: 0x0013699D
		void IList.RemoveAt(int index)
		{
			((IList<Uri>)this).RemoveAt(index);
		}

		// Token: 0x170010E7 RID: 4327
		object IList.this[int index]
		{
			get
			{
				return this._uriList[index];
			}
			set
			{
				((IList<Uri>)this)[index] = (Uri)value;
			}
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x001387B5 File Offset: 0x001369B5
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this._uriList).CopyTo(array, index);
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x060044E2 RID: 17634 RVA: 0x001387C4 File Offset: 0x001369C4
		int ICollection.Count
		{
			get
			{
				return ((ICollection<Uri>)this).Count;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x060044E3 RID: 17635 RVA: 0x001387CC File Offset: 0x001369CC
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((ICollection)this._uriList).IsSynchronized;
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x060044E4 RID: 17636 RVA: 0x001387D9 File Offset: 0x001369D9
		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)this._uriList).SyncRoot;
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x060044E5 RID: 17637 RVA: 0x001387E6 File Offset: 0x001369E6
		private Speller Speller
		{
			get
			{
				if (this._owner.TextEditor == null)
				{
					return null;
				}
				return this._owner.TextEditor.Speller;
			}
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x00138808 File Offset: 0x00136A08
		private static void ValidateUri(Uri item)
		{
			if (item == null)
			{
				throw new ArgumentException(SR.Get("CustomDictionaryNullItem"));
			}
			if (item.IsAbsoluteUri && !item.IsUnc && !item.IsFile && !PackUriHelper.IsPackUri(item))
			{
				throw new NotSupportedException(SR.Get("CustomDictionarySourcesUnsupportedURI"));
			}
		}

		// Token: 0x040028B2 RID: 10418
		private readonly List<Uri> _uriList;

		// Token: 0x040028B3 RID: 10419
		private readonly TextBoxBase _owner;
	}
}
