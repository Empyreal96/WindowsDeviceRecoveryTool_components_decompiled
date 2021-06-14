using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.ItemSelectionChanged" /> event. </summary>
	// Token: 0x020002CF RID: 719
	public class ListViewItemSelectionChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ListViewItemSelectionChangedEventArgs" /> class. </summary>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ListViewItem" /> whose selection state has changed.</param>
		/// <param name="itemIndex">The index of the <see cref="T:System.Windows.Forms.ListViewItem" /> whose selection state has changed.</param>
		/// <param name="isSelected">
		///       <see langword="true" /> to indicate the item's state has changed to selected; <see langword="false" /> to indicate the item's state has changed to deselected.</param>
		// Token: 0x06002B49 RID: 11081 RVA: 0x000CAE38 File Offset: 0x000C9038
		public ListViewItemSelectionChangedEventArgs(ListViewItem item, int itemIndex, bool isSelected)
		{
			this.item = item;
			this.itemIndex = itemIndex;
			this.isSelected = isSelected;
		}

		/// <summary>Gets a value indicating whether the item's state has changed to selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the item's state has changed to selected; <see langword="false" /> if the item's state has changed to deselected.</returns>
		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06002B4A RID: 11082 RVA: 0x000CAE55 File Offset: 0x000C9055
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
		}

		/// <summary>Gets the item whose selection state has changed.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> whose selection state has changed.</returns>
		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06002B4B RID: 11083 RVA: 0x000CAE5D File Offset: 0x000C905D
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		/// <summary>Gets the index of the item whose selection state has changed.</summary>
		/// <returns>The index of the <see cref="T:System.Windows.Forms.ListViewItem" /> whose selection state has changed.</returns>
		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06002B4C RID: 11084 RVA: 0x000CAE65 File Offset: 0x000C9065
		public int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		// Token: 0x04001284 RID: 4740
		private ListViewItem item;

		// Token: 0x04001285 RID: 4741
		private int itemIndex;

		// Token: 0x04001286 RID: 4742
		private bool isSelected;
	}
}
