using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.WebBrowser.Navigated" /> event.</summary>
	// Token: 0x0200042D RID: 1069
	public class WebBrowserNavigatedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.WebBrowserNavigatedEventArgs" /> class.</summary>
		/// <param name="url">A <see cref="T:System.Uri" /> representing the location of the document to which the <see cref="T:System.Windows.Forms.WebBrowser" /> control has navigated. </param>
		// Token: 0x06004AC2 RID: 19138 RVA: 0x00135559 File Offset: 0x00133759
		public WebBrowserNavigatedEventArgs(Uri url)
		{
			this.url = url;
		}

		/// <summary>Gets the location of the document to which the <see cref="T:System.Windows.Forms.WebBrowser" /> control has navigated.</summary>
		/// <returns>A <see cref="T:System.Uri" /> representing the location of the document to which the <see cref="T:System.Windows.Forms.WebBrowser" /> control has navigated.</returns>
		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06004AC3 RID: 19139 RVA: 0x00135568 File Offset: 0x00133768
		public Uri Url
		{
			get
			{
				WebBrowser.EnsureUrlConnectPermission(this.url);
				return this.url;
			}
		}

		// Token: 0x0400274C RID: 10060
		private Uri url;
	}
}
