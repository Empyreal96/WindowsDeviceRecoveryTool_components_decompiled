using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	/// <summary>Provides collection support for a collection of document pages. </summary>
	// Token: 0x0200039F RID: 927
	public sealed class PageContentCollection : IEnumerable<PageContent>, IEnumerable
	{
		// Token: 0x06003268 RID: 12904 RVA: 0x000DCD83 File Offset: 0x000DAF83
		internal PageContentCollection(FixedDocument logicalParent)
		{
			this._logicalParent = logicalParent;
			this._internalList = new List<PageContent>();
		}

		/// <summary>Adds a new page to the page collection.</summary>
		/// <param name="newPageContent">The new page to add to the collection. </param>
		/// <returns>The zero-based index within the collection where the page was added.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="newPageContent" /> was passed as <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The page passed as <paramref name="newPageContent" /> already existed in the collection.</exception>
		// Token: 0x06003269 RID: 12905 RVA: 0x000DCDA0 File Offset: 0x000DAFA0
		public int Add(PageContent newPageContent)
		{
			if (newPageContent == null)
			{
				throw new ArgumentNullException("newPageContent");
			}
			this._logicalParent.AddLogicalChild(newPageContent);
			this.InternalList.Add(newPageContent);
			int num = this.InternalList.Count - 1;
			this._logicalParent.OnPageContentAppended(num);
			return num;
		}

		/// <summary>Returns an enumerator for iterating through the page collection. </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		// Token: 0x0600326A RID: 12906 RVA: 0x000DCDEE File Offset: 0x000DAFEE
		public IEnumerator<PageContent> GetEnumerator()
		{
			return this.InternalList.GetEnumerator();
		}

		/// <summary>This member supports the Microsoft .NET Framework infrastructure and is not intended to be used directly from your code.  Use the type-safe <see cref="M:System.Windows.Documents.PageContentCollection.GetEnumerator" /> method instead. </summary>
		/// <returns>The enumerator.</returns>
		// Token: 0x0600326B RID: 12907 RVA: 0x000DCDFB File Offset: 0x000DAFFB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PageContent>)this).GetEnumerator();
		}

		/// <summary>Gets the <see cref="T:System.Windows.Documents.PageContent" /> element at the specified index within the collection. </summary>
		/// <param name="pageIndex">The zero-based index of the page to get. </param>
		/// <returns>The page content element at the specified index within the collection. </returns>
		// Token: 0x17000CB5 RID: 3253
		public PageContent this[int pageIndex]
		{
			get
			{
				return this.InternalList[pageIndex];
			}
		}

		/// <summary>Gets the number of elements contained in the page collection.</summary>
		/// <returns>The number of elements in the collection.</returns>
		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x0600326D RID: 12909 RVA: 0x000DCE11 File Offset: 0x000DB011
		public int Count
		{
			get
			{
				return this.InternalList.Count;
			}
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000DCE1E File Offset: 0x000DB01E
		internal int IndexOf(PageContent pc)
		{
			return this.InternalList.IndexOf(pc);
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x0600326F RID: 12911 RVA: 0x000DCE2C File Offset: 0x000DB02C
		private IList<PageContent> InternalList
		{
			get
			{
				return this._internalList;
			}
		}

		// Token: 0x04001EC2 RID: 7874
		private FixedDocument _logicalParent;

		// Token: 0x04001EC3 RID: 7875
		private List<PageContent> _internalList;
	}
}
