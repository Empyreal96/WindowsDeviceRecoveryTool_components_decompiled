using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	/// <summary> Provides data for the <see cref="E:System.Windows.Documents.PageContent.GetPageRootCompleted" /> event. </summary>
	// Token: 0x0200039C RID: 924
	public sealed class GetPageRootCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06003255 RID: 12885 RVA: 0x000DCBA5 File Offset: 0x000DADA5
		internal GetPageRootCompletedEventArgs(FixedPage page, Exception error, bool cancelled, object userToken) : base(error, cancelled, userToken)
		{
			this._page = page;
		}

		/// <summary> Gets the <see cref="T:System.Windows.Documents.FixedPage" /> content asynchronously requested by <see cref="M:System.Windows.Documents.PageContent.GetPageRootAsync(System.Boolean)" />. </summary>
		/// <returns>The root element of the visual tree for the <see cref="T:System.Windows.Documents.PageContent" /> requested by <see cref="M:System.Windows.Documents.PageContent.GetPageRootAsync(System.Boolean)" />.</returns>
		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06003256 RID: 12886 RVA: 0x000DCBB8 File Offset: 0x000DADB8
		public FixedPage Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._page;
			}
		}

		// Token: 0x04001EB4 RID: 7860
		private FixedPage _page;
	}
}
