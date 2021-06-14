using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006C5 RID: 1733
	internal class FixedDocumentPaginator : DynamicDocumentPaginator, IServiceProvider
	{
		// Token: 0x06006FDF RID: 28639 RVA: 0x002029BD File Offset: 0x00200BBD
		internal FixedDocumentPaginator(FixedDocument document)
		{
			this._document = document;
		}

		// Token: 0x06006FE0 RID: 28640 RVA: 0x002029CC File Offset: 0x00200BCC
		public override DocumentPage GetPage(int pageNumber)
		{
			return this._document.GetPage(pageNumber);
		}

		// Token: 0x06006FE1 RID: 28641 RVA: 0x002029DA File Offset: 0x00200BDA
		public override void GetPageAsync(int pageNumber, object userState)
		{
			this._document.GetPageAsync(pageNumber, userState);
		}

		// Token: 0x06006FE2 RID: 28642 RVA: 0x002029E9 File Offset: 0x00200BE9
		public override void CancelAsync(object userState)
		{
			this._document.CancelAsync(userState);
		}

		// Token: 0x06006FE3 RID: 28643 RVA: 0x002029F7 File Offset: 0x00200BF7
		public override int GetPageNumber(ContentPosition contentPosition)
		{
			return this._document.GetPageNumber(contentPosition);
		}

		// Token: 0x06006FE4 RID: 28644 RVA: 0x00202A05 File Offset: 0x00200C05
		public override ContentPosition GetPagePosition(DocumentPage page)
		{
			return this._document.GetPagePosition(page);
		}

		// Token: 0x06006FE5 RID: 28645 RVA: 0x00202A13 File Offset: 0x00200C13
		public override ContentPosition GetObjectPosition(object o)
		{
			return this._document.GetObjectPosition(o);
		}

		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x06006FE6 RID: 28646 RVA: 0x00202A21 File Offset: 0x00200C21
		public override bool IsPageCountValid
		{
			get
			{
				return this._document.IsPageCountValid;
			}
		}

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x06006FE7 RID: 28647 RVA: 0x00202A2E File Offset: 0x00200C2E
		public override int PageCount
		{
			get
			{
				return this._document.PageCount;
			}
		}

		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x06006FE8 RID: 28648 RVA: 0x00202A3B File Offset: 0x00200C3B
		// (set) Token: 0x06006FE9 RID: 28649 RVA: 0x00202A48 File Offset: 0x00200C48
		public override Size PageSize
		{
			get
			{
				return this._document.PageSize;
			}
			set
			{
				this._document.PageSize = value;
			}
		}

		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x06006FEA RID: 28650 RVA: 0x00202A56 File Offset: 0x00200C56
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x06006FEB RID: 28651 RVA: 0x00202A5E File Offset: 0x00200C5E
		internal void NotifyGetPageCompleted(GetPageCompletedEventArgs e)
		{
			this.OnGetPageCompleted(e);
		}

		// Token: 0x06006FEC RID: 28652 RVA: 0x00202A67 File Offset: 0x00200C67
		internal void NotifyPaginationCompleted(EventArgs e)
		{
			this.OnPaginationCompleted(e);
		}

		// Token: 0x06006FED RID: 28653 RVA: 0x00202A70 File Offset: 0x00200C70
		internal void NotifyPaginationProgress(PaginationProgressEventArgs e)
		{
			this.OnPaginationProgress(e);
		}

		// Token: 0x06006FEE RID: 28654 RVA: 0x00202A79 File Offset: 0x00200C79
		internal void NotifyPagesChanged(PagesChangedEventArgs e)
		{
			this.OnPagesChanged(e);
		}

		// Token: 0x06006FEF RID: 28655 RVA: 0x00202A82 File Offset: 0x00200C82
		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._document).GetService(serviceType);
		}

		// Token: 0x040036DC RID: 14044
		private readonly FixedDocument _document;
	}
}
