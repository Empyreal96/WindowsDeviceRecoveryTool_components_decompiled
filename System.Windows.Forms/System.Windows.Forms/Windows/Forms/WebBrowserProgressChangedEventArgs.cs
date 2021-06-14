using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.WebBrowser.ProgressChanged" /> event.</summary>
	// Token: 0x02000431 RID: 1073
	public class WebBrowserProgressChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.WebBrowserProgressChangedEventArgs" /> class.</summary>
		/// <param name="currentProgress">The number of bytes that are loaded already. </param>
		/// <param name="maximumProgress">The total number of bytes to be loaded. </param>
		// Token: 0x06004ACF RID: 19151 RVA: 0x001355B7 File Offset: 0x001337B7
		public WebBrowserProgressChangedEventArgs(long currentProgress, long maximumProgress)
		{
			this.currentProgress = currentProgress;
			this.maximumProgress = maximumProgress;
		}

		/// <summary>Gets the number of bytes that have been downloaded.</summary>
		/// <returns>The number of bytes that have been loaded or -1 to indicate that the download has completed.</returns>
		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06004AD0 RID: 19152 RVA: 0x001355CD File Offset: 0x001337CD
		public long CurrentProgress
		{
			get
			{
				return this.currentProgress;
			}
		}

		/// <summary>Gets the total number of bytes in the document being loaded.</summary>
		/// <returns>The total number of bytes to be loaded.</returns>
		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06004AD1 RID: 19153 RVA: 0x001355D5 File Offset: 0x001337D5
		public long MaximumProgress
		{
			get
			{
				return this.maximumProgress;
			}
		}

		// Token: 0x0400274F RID: 10063
		private long currentProgress;

		// Token: 0x04002750 RID: 10064
		private long maximumProgress;
	}
}
