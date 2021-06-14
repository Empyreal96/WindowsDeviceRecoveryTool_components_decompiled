using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.HtmlWindow.Error" /> event. </summary>
	// Token: 0x0200026D RID: 621
	public sealed class HtmlElementErrorEventArgs : EventArgs
	{
		// Token: 0x0600259C RID: 9628 RVA: 0x000B4613 File Offset: 0x000B2813
		internal HtmlElementErrorEventArgs(string description, string urlString, int lineNumber)
		{
			this.description = description;
			this.urlString = urlString;
			this.lineNumber = lineNumber;
		}

		/// <summary>Gets the descriptive string corresponding to the error.</summary>
		/// <returns>The descriptive string corresponding to the error.</returns>
		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x0600259D RID: 9629 RVA: 0x000B4630 File Offset: 0x000B2830
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		/// <summary>Gets or sets whether this error has been handled by the application hosting the document.</summary>
		/// <returns>
		///     <see langword="true" /> if the event has been handled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x000B4638 File Offset: 0x000B2838
		// (set) Token: 0x0600259F RID: 9631 RVA: 0x000B4640 File Offset: 0x000B2840
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		/// <summary>Gets the line of HTML script code on which the error occurred.</summary>
		/// <returns>An <see cref="T:System.Int32" /> designating the script line number.</returns>
		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000B4649 File Offset: 0x000B2849
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		/// <summary>Gets the location of the document that generated the error.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that represents the location of the document that generated the error.</returns>
		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x000B4651 File Offset: 0x000B2851
		public Uri Url
		{
			get
			{
				if (this.url == null)
				{
					this.url = new Uri(this.urlString);
				}
				return this.url;
			}
		}

		// Token: 0x04001015 RID: 4117
		private string description;

		// Token: 0x04001016 RID: 4118
		private string urlString;

		// Token: 0x04001017 RID: 4119
		private Uri url;

		// Token: 0x04001018 RID: 4120
		private int lineNumber;

		// Token: 0x04001019 RID: 4121
		private bool handled;
	}
}
