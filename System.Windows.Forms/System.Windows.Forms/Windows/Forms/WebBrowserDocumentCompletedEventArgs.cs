using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.WebBrowser.DocumentCompleted" /> event.</summary>
	// Token: 0x0200042A RID: 1066
	public class WebBrowserDocumentCompletedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.WebBrowserDocumentCompletedEventArgs" /> class.</summary>
		/// <param name="url">A <see cref="T:System.Uri" /> representing the location of the document that was loaded. </param>
		// Token: 0x06004AB3 RID: 19123 RVA: 0x00135311 File Offset: 0x00133511
		public WebBrowserDocumentCompletedEventArgs(Uri url)
		{
			this.url = url;
		}

		/// <summary>Gets the location of the document to which the <see cref="T:System.Windows.Forms.WebBrowser" /> control has navigated.</summary>
		/// <returns>A <see cref="T:System.Uri" /> representing the location of the document that was loaded.</returns>
		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06004AB4 RID: 19124 RVA: 0x00135320 File Offset: 0x00133520
		public Uri Url
		{
			get
			{
				WebBrowser.EnsureUrlConnectPermission(this.url);
				return this.url;
			}
		}

		// Token: 0x04002739 RID: 10041
		private Uri url;
	}
}
