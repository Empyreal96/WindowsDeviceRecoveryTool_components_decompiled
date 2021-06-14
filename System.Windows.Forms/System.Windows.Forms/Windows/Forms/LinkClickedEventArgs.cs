using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.RichTextBox.LinkClicked" /> event.</summary>
	// Token: 0x020002B2 RID: 690
	[ComVisible(true)]
	public class LinkClickedEventArgs : EventArgs
	{
		/// <summary>Gets the text of the link being clicked.</summary>
		/// <returns>The text of the link that is clicked in the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</returns>
		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x060027E4 RID: 10212 RVA: 0x000B9F5D File Offset: 0x000B815D
		public string LinkText
		{
			get
			{
				return this.linkText;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LinkClickedEventArgs" /> class.</summary>
		/// <param name="linkText">The text of the link that is clicked in the <see cref="T:System.Windows.Forms.RichTextBox" /> control. </param>
		// Token: 0x060027E5 RID: 10213 RVA: 0x000B9F65 File Offset: 0x000B8165
		public LinkClickedEventArgs(string linkText)
		{
			this.linkText = linkText;
		}

		// Token: 0x0400117A RID: 4474
		private string linkText;
	}
}
