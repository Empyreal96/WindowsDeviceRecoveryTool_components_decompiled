using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006C6 RID: 1734
	internal class FixedDocumentSequencePaginator : DynamicDocumentPaginator, IServiceProvider
	{
		// Token: 0x06006FF0 RID: 28656 RVA: 0x00202A90 File Offset: 0x00200C90
		internal FixedDocumentSequencePaginator(FixedDocumentSequence document)
		{
			this._document = document;
		}

		// Token: 0x06006FF1 RID: 28657 RVA: 0x00202A9F File Offset: 0x00200C9F
		public override DocumentPage GetPage(int pageNumber)
		{
			return this._document.GetPage(pageNumber);
		}

		// Token: 0x06006FF2 RID: 28658 RVA: 0x00202AAD File Offset: 0x00200CAD
		public override void GetPageAsync(int pageNumber, object userState)
		{
			this._document.GetPageAsync(pageNumber, userState);
		}

		// Token: 0x06006FF3 RID: 28659 RVA: 0x00202ABC File Offset: 0x00200CBC
		public override void CancelAsync(object userState)
		{
			this._document.CancelAsync(userState);
		}

		// Token: 0x06006FF4 RID: 28660 RVA: 0x00202ACA File Offset: 0x00200CCA
		public override int GetPageNumber(ContentPosition contentPosition)
		{
			return this._document.GetPageNumber(contentPosition);
		}

		// Token: 0x06006FF5 RID: 28661 RVA: 0x00202AD8 File Offset: 0x00200CD8
		public override ContentPosition GetPagePosition(DocumentPage page)
		{
			return this._document.GetPagePosition(page);
		}

		// Token: 0x06006FF6 RID: 28662 RVA: 0x00202AE6 File Offset: 0x00200CE6
		public override ContentPosition GetObjectPosition(object o)
		{
			return this._document.GetObjectPosition(o);
		}

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x06006FF7 RID: 28663 RVA: 0x00202AF4 File Offset: 0x00200CF4
		public override bool IsPageCountValid
		{
			get
			{
				return this._document.IsPageCountValid;
			}
		}

		// Token: 0x17001A98 RID: 6808
		// (get) Token: 0x06006FF8 RID: 28664 RVA: 0x00202B01 File Offset: 0x00200D01
		public override int PageCount
		{
			get
			{
				return this._document.PageCount;
			}
		}

		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x06006FF9 RID: 28665 RVA: 0x00202B0E File Offset: 0x00200D0E
		// (set) Token: 0x06006FFA RID: 28666 RVA: 0x00202B1B File Offset: 0x00200D1B
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

		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x06006FFB RID: 28667 RVA: 0x00202B29 File Offset: 0x00200D29
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x06006FFC RID: 28668 RVA: 0x00202A5E File Offset: 0x00200C5E
		internal void NotifyGetPageCompleted(GetPageCompletedEventArgs e)
		{
			this.OnGetPageCompleted(e);
		}

		// Token: 0x06006FFD RID: 28669 RVA: 0x00202A67 File Offset: 0x00200C67
		internal void NotifyPaginationCompleted(EventArgs e)
		{
			this.OnPaginationCompleted(e);
		}

		// Token: 0x06006FFE RID: 28670 RVA: 0x00202A70 File Offset: 0x00200C70
		internal void NotifyPaginationProgress(PaginationProgressEventArgs e)
		{
			this.OnPaginationProgress(e);
		}

		// Token: 0x06006FFF RID: 28671 RVA: 0x00202A79 File Offset: 0x00200C79
		internal void NotifyPagesChanged(PagesChangedEventArgs e)
		{
			this.OnPagesChanged(e);
		}

		// Token: 0x06007000 RID: 28672 RVA: 0x00202B31 File Offset: 0x00200D31
		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._document).GetService(serviceType);
		}

		// Token: 0x040036DD RID: 14045
		private readonly FixedDocumentSequence _document;
	}
}
