using System;
using System.Collections;

namespace System.Windows
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Window" /> objects. This class cannot be inherited.</summary>
	// Token: 0x02000141 RID: 321
	public sealed class WindowCollection : ICollection, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.WindowCollection" /> class.</summary>
		// Token: 0x06000E3D RID: 3645 RVA: 0x00036AC8 File Offset: 0x00034CC8
		public WindowCollection()
		{
			this._list = new ArrayList(1);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00036ADC File Offset: 0x00034CDC
		internal WindowCollection(int count)
		{
			this._list = new ArrayList(count);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Window" /> or <see cref="T:System.Windows.Navigation.NavigationWindow" /> object at the specified index.</summary>
		/// <param name="index">The index of the specified <see cref="T:System.Windows.Window" /> or <see cref="T:System.Windows.Navigation.NavigationWindow" />.</param>
		/// <returns>A <see cref="T:System.Windows.Window" /> or <see cref="T:System.Windows.Navigation.NavigationWindow" /> object.</returns>
		// Token: 0x17000456 RID: 1110
		public Window this[int index]
		{
			get
			{
				return this._list[index] as Window;
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> that you can use to enumerate the <see cref="T:System.Windows.Window" /> objects in the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that you can use to enumerate the <see cref="T:System.Windows.Window" /> objects in the collection.</returns>
		// Token: 0x06000E40 RID: 3648 RVA: 0x00036B03 File Offset: 0x00034D03
		public IEnumerator GetEnumerator()
		{
			return this._list.GetEnumerator();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
		/// <param name="array">A zero-based <see cref="T:System.Array" /> that receives the copied items from the <see cref="T:System.Windows.WindowCollection" />.</param>
		/// <param name="index">The first position in the specified <see cref="T:System.Array" /> to receive the copied contents.</param>
		// Token: 0x06000E41 RID: 3649 RVA: 0x00036B10 File Offset: 0x00034D10
		void ICollection.CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		/// <summary>Copies each <see cref="T:System.Windows.Window" /> object in the collection to an array, starting from the specified index.</summary>
		/// <param name="array">An array of type <see cref="T:System.Windows.Window" /> that the <see cref="T:System.Windows.Window" /> objects in the collection are copied to.</param>
		/// <param name="index">The position in the collection to start copying from.</param>
		// Token: 0x06000E42 RID: 3650 RVA: 0x00036B10 File Offset: 0x00034D10
		public void CopyTo(Window[] array, int index)
		{
			this._list.CopyTo(array, index);
		}

		/// <summary>Gets the number of <see cref="T:System.Windows.Window" /> objects contained in the <see cref="T:System.Windows.WindowCollection" />.</summary>
		/// <returns>The number of <see cref="T:System.Windows.Window" /> objects contained in the <see cref="T:System.Windows.WindowCollection" />.</returns>
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x00036B1F File Offset: 0x00034D1F
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.WindowCollection" /> object is thread safe. </summary>
		/// <returns>
		///     true if the object is thread safe; otherwise, false.</returns>
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x00036B2C File Offset: 0x00034D2C
		public bool IsSynchronized
		{
			get
			{
				return this._list.IsSynchronized;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x00036B39 File Offset: 0x00034D39
		public object SyncRoot
		{
			get
			{
				return this._list.SyncRoot;
			}
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00036B48 File Offset: 0x00034D48
		internal WindowCollection Clone()
		{
			object syncRoot = this._list.SyncRoot;
			WindowCollection windowCollection;
			lock (syncRoot)
			{
				windowCollection = new WindowCollection(this._list.Count);
				for (int i = 0; i < this._list.Count; i++)
				{
					windowCollection._list.Add(this._list[i]);
				}
			}
			return windowCollection;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00036BC8 File Offset: 0x00034DC8
		internal void Remove(Window win)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Remove(win);
			}
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00036C14 File Offset: 0x00034E14
		internal void RemoveAt(int index)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Remove(index);
			}
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00036C64 File Offset: 0x00034E64
		internal int Add(Window win)
		{
			object syncRoot = this._list.SyncRoot;
			int result;
			lock (syncRoot)
			{
				result = this._list.Add(win);
			}
			return result;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00036CB4 File Offset: 0x00034EB4
		internal bool HasItem(Window win)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					if (this._list[i] == win)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000BA5 RID: 2981
		private ArrayList _list;
	}
}
