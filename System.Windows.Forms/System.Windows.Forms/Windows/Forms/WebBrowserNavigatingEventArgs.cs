using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.WebBrowser.Navigating" /> event.</summary>
	// Token: 0x0200042F RID: 1071
	public class WebBrowserNavigatingEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.WebBrowserNavigatingEventArgs" /> class.</summary>
		/// <param name="url">A <see cref="T:System.Uri" /> representing the location of the document to which the <see cref="T:System.Windows.Forms.WebBrowser" /> control is navigating. </param>
		/// <param name="targetFrameName">The name of the Web page frame in which the new document will be loaded. </param>
		// Token: 0x06004AC8 RID: 19144 RVA: 0x0013557B File Offset: 0x0013377B
		public WebBrowserNavigatingEventArgs(Uri url, string targetFrameName)
		{
			this.url = url;
			this.targetFrameName = targetFrameName;
		}

		/// <summary>Gets the location of the document to which the <see cref="T:System.Windows.Forms.WebBrowser" /> control is navigating.</summary>
		/// <returns>A <see cref="T:System.Uri" /> representing the location of the document to which the <see cref="T:System.Windows.Forms.WebBrowser" /> control is navigating.</returns>
		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06004AC9 RID: 19145 RVA: 0x00135591 File Offset: 0x00133791
		public Uri Url
		{
			get
			{
				WebBrowser.EnsureUrlConnectPermission(this.url);
				return this.url;
			}
		}

		/// <summary>Gets the name of the Web page frame in which the new document will be loaded.</summary>
		/// <returns>The name of the frame in which the new document will be loaded.</returns>
		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06004ACA RID: 19146 RVA: 0x001355A4 File Offset: 0x001337A4
		public string TargetFrameName
		{
			get
			{
				WebBrowser.EnsureUrlConnectPermission(this.url);
				return this.targetFrameName;
			}
		}

		// Token: 0x0400274D RID: 10061
		private Uri url;

		// Token: 0x0400274E RID: 10062
		private string targetFrameName;
	}
}
