using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.LinkLabel.LinkClicked" /> event.</summary>
	// Token: 0x020002B6 RID: 694
	[ComVisible(true)]
	public class LinkLabelLinkClickedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LinkLabelLinkClickedEventArgs" /> class with the specified link.</summary>
		/// <param name="link">The <see cref="T:System.Windows.Forms.LinkLabel.Link" /> that was clicked. </param>
		// Token: 0x06002854 RID: 10324 RVA: 0x000BC4C8 File Offset: 0x000BA6C8
		public LinkLabelLinkClickedEventArgs(LinkLabel.Link link)
		{
			this.link = link;
			this.button = MouseButtons.Left;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LinkLabelLinkClickedEventArgs" /> class with the specified link and the specified mouse button.</summary>
		/// <param name="link">The <see cref="T:System.Windows.Forms.LinkLabel.Link" /> that was clicked. </param>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values.</param>
		// Token: 0x06002855 RID: 10325 RVA: 0x000BC4E2 File Offset: 0x000BA6E2
		public LinkLabelLinkClickedEventArgs(LinkLabel.Link link, MouseButtons button) : this(link)
		{
			this.button = button;
		}

		/// <summary>Gets the mouse button that causes the link to be clicked.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values.</returns>
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06002856 RID: 10326 RVA: 0x000BC4F2 File Offset: 0x000BA6F2
		public MouseButtons Button
		{
			get
			{
				return this.button;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.LinkLabel.Link" /> that was clicked.</summary>
		/// <returns>A link on the <see cref="T:System.Windows.Forms.LinkLabel" />.</returns>
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x000BC4FA File Offset: 0x000BA6FA
		public LinkLabel.Link Link
		{
			get
			{
				return this.link;
			}
		}

		// Token: 0x0400118E RID: 4494
		private readonly LinkLabel.Link link;

		// Token: 0x0400118F RID: 4495
		private readonly MouseButtons button;
	}
}
