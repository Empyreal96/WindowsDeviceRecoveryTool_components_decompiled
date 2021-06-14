using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies constants that define the state of the <see cref="T:System.Windows.Forms.WebBrowser" /> control.</summary>
	// Token: 0x02000425 RID: 1061
	public enum WebBrowserReadyState
	{
		/// <summary>No document is currently loaded.</summary>
		// Token: 0x04002712 RID: 10002
		Uninitialized,
		/// <summary>The control is loading a new document.</summary>
		// Token: 0x04002713 RID: 10003
		Loading,
		/// <summary>The control has loaded and initialized the new document, but has not yet received all the document data.</summary>
		// Token: 0x04002714 RID: 10004
		Loaded,
		/// <summary>The control has loaded enough of the document to allow limited user interaction, such as clicking hyperlinks that have been displayed.</summary>
		// Token: 0x04002715 RID: 10005
		Interactive,
		/// <summary>The control has finished loading the new document and all its contents.</summary>
		// Token: 0x04002716 RID: 10006
		Complete
	}
}
