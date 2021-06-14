using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies whether all the pages or only a limited range will be processed by an operation, usually printing.</summary>
	// Token: 0x0200050B RID: 1291
	public enum PageRangeSelection
	{
		/// <summary>All pages.</summary>
		// Token: 0x04002CD4 RID: 11476
		AllPages,
		/// <summary>A user-specified range of pages.</summary>
		// Token: 0x04002CD5 RID: 11477
		UserPages,
		/// <summary>The current page.</summary>
		// Token: 0x04002CD6 RID: 11478
		CurrentPage,
		/// <summary>The selected pages.</summary>
		// Token: 0x04002CD7 RID: 11479
		SelectedPages
	}
}
