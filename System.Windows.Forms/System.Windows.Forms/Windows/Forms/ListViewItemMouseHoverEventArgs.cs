using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.ItemMouseHover" /> event. </summary>
	// Token: 0x020002CD RID: 717
	[ComVisible(true)]
	public class ListViewItemMouseHoverEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItemMouseHoverEventArgs" /> class. </summary>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ListViewItem" /> the mouse pointer is currently hovering over.</param>
		// Token: 0x06002B43 RID: 11075 RVA: 0x000CAE21 File Offset: 0x000C9021
		public ListViewItemMouseHoverEventArgs(ListViewItem item)
		{
			this.item = item;
		}

		/// <summary>Gets the item the mouse pointer is currently hovering over.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> that the mouse pointer is currently hovering over.</returns>
		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06002B44 RID: 11076 RVA: 0x000CAE30 File Offset: 0x000C9030
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x04001283 RID: 4739
		private readonly ListViewItem item;
	}
}
