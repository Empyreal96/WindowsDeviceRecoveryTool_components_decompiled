using System;

namespace System.Drawing.Printing
{
	/// <summary>Provides data for the <see cref="E:System.Drawing.Printing.PrintDocument.PrintPage" /> event.</summary>
	// Token: 0x0200006C RID: 108
	public class PrintPageEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> class.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the item. </param>
		/// <param name="marginBounds">The area between the margins. </param>
		/// <param name="pageBounds">The total area of the paper. </param>
		/// <param name="pageSettings">The <see cref="T:System.Drawing.Printing.PageSettings" /> for the page. </param>
		// Token: 0x06000807 RID: 2055 RVA: 0x00020B17 File Offset: 0x0001ED17
		public PrintPageEventArgs(Graphics graphics, Rectangle marginBounds, Rectangle pageBounds, PageSettings pageSettings)
		{
			this.graphics = graphics;
			this.marginBounds = marginBounds;
			this.pageBounds = pageBounds;
			this.pageSettings = pageSettings;
		}

		/// <summary>Gets or sets a value indicating whether the print job should be canceled.</summary>
		/// <returns>
		///     <see langword="true" /> if the print job should be canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00020B43 File Offset: 0x0001ED43
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x00020B4B File Offset: 0x0001ED4B
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> used to paint the page.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> used to paint the page.</returns>
		// Token: 0x1700030E RID: 782
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x00020B54 File Offset: 0x0001ED54
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets or sets a value indicating whether an additional page should be printed.</summary>
		/// <returns>
		///     <see langword="true" /> if an additional page should be printed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00020B5C File Offset: 0x0001ED5C
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x00020B64 File Offset: 0x0001ED64
		public bool HasMorePages
		{
			get
			{
				return this.hasMorePages;
			}
			set
			{
				this.hasMorePages = value;
			}
		}

		/// <summary>Gets the rectangular area that represents the portion of the page inside the margins.</summary>
		/// <returns>The rectangular area, measured in hundredths of an inch, that represents the portion of the page inside the margins. </returns>
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x00020B6D File Offset: 0x0001ED6D
		public Rectangle MarginBounds
		{
			get
			{
				return this.marginBounds;
			}
		}

		/// <summary>Gets the rectangular area that represents the total area of the page.</summary>
		/// <returns>The rectangular area that represents the total area of the page.</returns>
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00020B75 File Offset: 0x0001ED75
		public Rectangle PageBounds
		{
			get
			{
				return this.pageBounds;
			}
		}

		/// <summary>Gets the page settings for the current page.</summary>
		/// <returns>The page settings for the current page.</returns>
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00020B7D File Offset: 0x0001ED7D
		public PageSettings PageSettings
		{
			get
			{
				return this.pageSettings;
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00020B85 File Offset: 0x0001ED85
		internal void Dispose()
		{
			this.graphics.Dispose();
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00020B92 File Offset: 0x0001ED92
		internal void SetGraphics(Graphics value)
		{
			this.graphics = value;
		}

		// Token: 0x040006F1 RID: 1777
		private bool hasMorePages;

		// Token: 0x040006F2 RID: 1778
		private bool cancel;

		// Token: 0x040006F3 RID: 1779
		private Graphics graphics;

		// Token: 0x040006F4 RID: 1780
		private readonly Rectangle marginBounds;

		// Token: 0x040006F5 RID: 1781
		private readonly Rectangle pageBounds;

		// Token: 0x040006F6 RID: 1782
		private readonly PageSettings pageSettings;

		// Token: 0x040006F7 RID: 1783
		internal bool CopySettingsToDevMode = true;
	}
}
