using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal
{
	// Token: 0x020005EC RID: 1516
	internal class ListOfObject : IList<object>, ICollection<object>, IEnumerable<object>, IEnumerable
	{
		// Token: 0x06006524 RID: 25892 RVA: 0x001C68BC File Offset: 0x001C4ABC
		internal ListOfObject(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			this._list = list;
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x001C68D9 File Offset: 0x001C4AD9
		int IList<object>.IndexOf(object item)
		{
			return this._list.IndexOf(item);
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x0003E384 File Offset: 0x0003C584
		void IList<object>.Insert(int index, object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006527 RID: 25895 RVA: 0x0003E384 File Offset: 0x0003C584
		void IList<object>.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001844 RID: 6212
		object IList<object>.this[int index]
		{
			get
			{
				return this._list[index];
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600652A RID: 25898 RVA: 0x0003E384 File Offset: 0x0003C584
		void ICollection<object>.Add(object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x0003E384 File Offset: 0x0003C584
		void ICollection<object>.Clear()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x001C68F5 File Offset: 0x001C4AF5
		bool ICollection<object>.Contains(object item)
		{
			return this._list.Contains(item);
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x001C6903 File Offset: 0x001C4B03
		void ICollection<object>.CopyTo(object[] array, int arrayIndex)
		{
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x0600652E RID: 25902 RVA: 0x001C6912 File Offset: 0x001C4B12
		int ICollection<object>.Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x0600652F RID: 25903 RVA: 0x00016748 File Offset: 0x00014948
		bool ICollection<object>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x0003E384 File Offset: 0x0003C584
		bool ICollection<object>.Remove(object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x001C691F File Offset: 0x001C4B1F
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			return new ListOfObject.ObjectEnumerator(this._list);
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x001C692C File Offset: 0x001C4B2C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}

		// Token: 0x040032BC RID: 12988
		private IList _list;

		// Token: 0x02000A0A RID: 2570
		private class ObjectEnumerator : IEnumerator<object>, IDisposable, IEnumerator
		{
			// Token: 0x06008A0B RID: 35339 RVA: 0x00256B4D File Offset: 0x00254D4D
			public ObjectEnumerator(IList list)
			{
				this._ie = list.GetEnumerator();
			}

			// Token: 0x17001F29 RID: 7977
			// (get) Token: 0x06008A0C RID: 35340 RVA: 0x00256B61 File Offset: 0x00254D61
			object IEnumerator<object>.Current
			{
				get
				{
					return this._ie.Current;
				}
			}

			// Token: 0x06008A0D RID: 35341 RVA: 0x00256B6E File Offset: 0x00254D6E
			void IDisposable.Dispose()
			{
				this._ie = null;
			}

			// Token: 0x17001F2A RID: 7978
			// (get) Token: 0x06008A0E RID: 35342 RVA: 0x00256B61 File Offset: 0x00254D61
			object IEnumerator.Current
			{
				get
				{
					return this._ie.Current;
				}
			}

			// Token: 0x06008A0F RID: 35343 RVA: 0x00256B77 File Offset: 0x00254D77
			bool IEnumerator.MoveNext()
			{
				return this._ie.MoveNext();
			}

			// Token: 0x06008A10 RID: 35344 RVA: 0x00256B84 File Offset: 0x00254D84
			void IEnumerator.Reset()
			{
				this._ie.Reset();
			}

			// Token: 0x040046B9 RID: 18105
			private IEnumerator _ie;
		}
	}
}
