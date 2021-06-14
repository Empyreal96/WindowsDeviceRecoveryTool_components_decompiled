using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Windows.Documents
{
	/// <summary>Defines an ordered list of <see cref="T:System.Windows.Documents.DocumentReference" /> elements.</summary>
	// Token: 0x02000338 RID: 824
	[CLSCompliant(false)]
	public sealed class DocumentReferenceCollection : IEnumerable<DocumentReference>, IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x06002B63 RID: 11107 RVA: 0x0000326D File Offset: 0x0000146D
		internal DocumentReferenceCollection()
		{
		}

		/// <summary>Returns an enumerator for iterating through the collection.</summary>
		/// <returns>An enumerator that you can use to iterate through the collection.</returns>
		// Token: 0x06002B64 RID: 11108 RVA: 0x000C633F File Offset: 0x000C453F
		public IEnumerator<DocumentReference> GetEnumerator()
		{
			return this._InternalList.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.  Use the type-safe <see cref="M:System.Windows.Documents.DocumentReferenceCollection.GetEnumerator" /> method instead. </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06002B65 RID: 11109 RVA: 0x000C634C File Offset: 0x000C454C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DocumentReference>)this).GetEnumerator();
		}

		/// <summary>Adds an element to the end of the collection.</summary>
		/// <param name="item">The element to add to the end of the collection.</param>
		// Token: 0x06002B66 RID: 11110 RVA: 0x000C6354 File Offset: 0x000C4554
		public void Add(DocumentReference item)
		{
			int count = this._InternalList.Count;
			this._InternalList.Add(item);
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, count);
		}

		/// <summary>Copies the whole collection to an array that starts at a given array index.</summary>
		/// <param name="array">The destination array to which the elements from the collection should be copied.</param>
		/// <param name="arrayIndex">The zero-based starting index within the array where the collection elements are to be copied.</param>
		// Token: 0x06002B67 RID: 11111 RVA: 0x000C6382 File Offset: 0x000C4582
		public void CopyTo(DocumentReference[] array, int arrayIndex)
		{
			this._InternalList.CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of elements that are in the collection.</summary>
		/// <returns>The number of items that the collection contains.</returns>
		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06002B68 RID: 11112 RVA: 0x000C6391 File Offset: 0x000C4591
		public int Count
		{
			get
			{
				return this._InternalList.Count;
			}
		}

		/// <summary>Gets the element that is at the specified index.</summary>
		/// <param name="index">The zero-based index of the element in the collection to get.</param>
		/// <returns>The collection element that is at the specified <paramref name="index" />.</returns>
		// Token: 0x17000A86 RID: 2694
		public DocumentReference this[int index]
		{
			get
			{
				return this._InternalList[index];
			}
		}

		/// <summary>Occurs when an element is added or removed.</summary>
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06002B6A RID: 11114 RVA: 0x000C63AC File Offset: 0x000C45AC
		// (remove) Token: 0x06002B6B RID: 11115 RVA: 0x000C63E4 File Offset: 0x000C45E4
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06002B6C RID: 11116 RVA: 0x000C6419 File Offset: 0x000C4619
		private IList<DocumentReference> _InternalList
		{
			get
			{
				if (this._internalList == null)
				{
					this._internalList = new List<DocumentReference>();
				}
				return this._internalList;
			}
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000C6434 File Offset: 0x000C4634
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			if (this.CollectionChanged != null)
			{
				NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(action, item, index);
				this.CollectionChanged(this, e);
			}
		}

		// Token: 0x04001CA3 RID: 7331
		private List<DocumentReference> _internalList;
	}
}
