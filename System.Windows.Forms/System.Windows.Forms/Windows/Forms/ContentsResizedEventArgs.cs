using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.RichTextBox.ContentsResized" /> event.</summary>
	// Token: 0x02000331 RID: 817
	public class ContentsResizedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ContentsResizedEventArgs" /> class.</summary>
		/// <param name="newRectangle">A <see cref="T:System.Drawing.Rectangle" /> that specifies the requested dimensions of the <see cref="T:System.Windows.Forms.RichTextBox" /> control. </param>
		// Token: 0x06003277 RID: 12919 RVA: 0x000EAF5B File Offset: 0x000E915B
		public ContentsResizedEventArgs(Rectangle newRectangle)
		{
			this.newRectangle = newRectangle;
		}

		/// <summary>Represents the requested size of the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the requested size of the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</returns>
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x000EAF6A File Offset: 0x000E916A
		public Rectangle NewRectangle
		{
			get
			{
				return this.newRectangle;
			}
		}

		// Token: 0x04001E4D RID: 7757
		private readonly Rectangle newRectangle;
	}
}
