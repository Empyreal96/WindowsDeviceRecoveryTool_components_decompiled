using System;

namespace System.Windows.Forms
{
	/// <summary>Contains information about an area of a <see cref="T:System.Windows.Forms.ListView" /> control or a <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
	// Token: 0x020002C9 RID: 713
	public class ListViewHitTestInfo
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewHitTestInfo" /> class. </summary>
		/// <param name="hitItem">The <see cref="T:System.Windows.Forms.ListViewItem" /> located at the position indicated by the hit test.</param>
		/// <param name="hitSubItem">The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> located at the position indicated by the hit test.</param>
		/// <param name="hitLocation">One of the <see cref="T:System.Windows.Forms.ListViewHitTestLocations" /> values.</param>
		// Token: 0x06002AD5 RID: 10965 RVA: 0x000C944A File Offset: 0x000C764A
		public ListViewHitTestInfo(ListViewItem hitItem, ListViewItem.ListViewSubItem hitSubItem, ListViewHitTestLocations hitLocation)
		{
			this.item = hitItem;
			this.subItem = hitSubItem;
			this.loc = hitLocation;
		}

		/// <summary>Gets the location of a hit test on a <see cref="T:System.Windows.Forms.ListView" /> control, in relation to the <see cref="T:System.Windows.Forms.ListView" /> and the items it contains.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ListViewHitTestLocations" /> values. </returns>
		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06002AD6 RID: 10966 RVA: 0x000C9467 File Offset: 0x000C7667
		public ListViewHitTestLocations Location
		{
			get
			{
				return this.loc;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ListViewItem" /> at the position indicated by a hit test on a <see cref="T:System.Windows.Forms.ListView" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> at the position indicated by a hit test on a <see cref="T:System.Windows.Forms.ListView" />.</returns>
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000C946F File Offset: 0x000C766F
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> at the position indicated by a hit test on a <see cref="T:System.Windows.Forms.ListView" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> at the position indicated by a hit test on a <see cref="T:System.Windows.Forms.ListView" />.</returns>
		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x000C9477 File Offset: 0x000C7677
		public ListViewItem.ListViewSubItem SubItem
		{
			get
			{
				return this.subItem;
			}
		}

		// Token: 0x04001260 RID: 4704
		private ListViewHitTestLocations loc;

		// Token: 0x04001261 RID: 4705
		private ListViewItem item;

		// Token: 0x04001262 RID: 4706
		private ListViewItem.ListViewSubItem subItem;
	}
}
