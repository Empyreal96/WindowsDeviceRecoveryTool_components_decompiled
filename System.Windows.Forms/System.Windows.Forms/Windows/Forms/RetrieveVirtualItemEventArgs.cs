using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> event. </summary>
	// Token: 0x02000333 RID: 819
	public class RetrieveVirtualItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.RetrieveVirtualItemEventArgs" /> class. </summary>
		/// <param name="itemIndex">The index of the item to retrieve.</param>
		// Token: 0x0600327D RID: 12925 RVA: 0x000EAF72 File Offset: 0x000E9172
		public RetrieveVirtualItemEventArgs(int itemIndex)
		{
			this.itemIndex = itemIndex;
		}

		/// <summary>Gets the index of the item to retrieve from the cache.</summary>
		/// <returns>The index of the item to retrieve from the cache.</returns>
		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x0600327E RID: 12926 RVA: 0x000EAF81 File Offset: 0x000E9181
		public int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		/// <summary>Gets or sets the item retrieved from the cache.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> retrieved from the cache.</returns>
		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x0600327F RID: 12927 RVA: 0x000EAF89 File Offset: 0x000E9189
		// (set) Token: 0x06003280 RID: 12928 RVA: 0x000EAF91 File Offset: 0x000E9191
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
			set
			{
				this.item = value;
			}
		}

		// Token: 0x04001E4E RID: 7758
		private int itemIndex;

		// Token: 0x04001E4F RID: 7759
		private ListViewItem item;
	}
}
