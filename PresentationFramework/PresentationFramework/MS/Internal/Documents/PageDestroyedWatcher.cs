using System;
using System.Collections;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006D8 RID: 1752
	internal class PageDestroyedWatcher
	{
		// Token: 0x0600715C RID: 29020 RVA: 0x0020728E File Offset: 0x0020548E
		public PageDestroyedWatcher()
		{
			this._table = new Hashtable(16);
		}

		// Token: 0x0600715D RID: 29021 RVA: 0x002072A4 File Offset: 0x002054A4
		public void AddPage(DocumentPage page)
		{
			if (!this._table.Contains(page))
			{
				this._table.Add(page, false);
				page.PageDestroyed += this.OnPageDestroyed;
				return;
			}
			this._table[page] = false;
		}

		// Token: 0x0600715E RID: 29022 RVA: 0x002072F6 File Offset: 0x002054F6
		public void RemovePage(DocumentPage page)
		{
			if (this._table.Contains(page))
			{
				this._table.Remove(page);
				page.PageDestroyed -= this.OnPageDestroyed;
			}
		}

		// Token: 0x0600715F RID: 29023 RVA: 0x00207324 File Offset: 0x00205524
		public bool IsDestroyed(DocumentPage page)
		{
			return !this._table.Contains(page) || (bool)this._table[page];
		}

		// Token: 0x06007160 RID: 29024 RVA: 0x00207348 File Offset: 0x00205548
		private void OnPageDestroyed(object sender, EventArgs e)
		{
			DocumentPage documentPage = sender as DocumentPage;
			Invariant.Assert(documentPage != null, "Invalid type in PageDestroyedWatcher");
			if (this._table.Contains(documentPage))
			{
				this._table[documentPage] = true;
			}
		}

		// Token: 0x04003716 RID: 14102
		private Hashtable _table;
	}
}
