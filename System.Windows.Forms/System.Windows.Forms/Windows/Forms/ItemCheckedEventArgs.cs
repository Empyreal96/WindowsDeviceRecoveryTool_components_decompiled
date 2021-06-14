using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.ItemChecked" /> event of the <see cref="T:System.Windows.Forms.ListView" /> control. </summary>
	// Token: 0x02000299 RID: 665
	public class ItemCheckedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ItemCheckedEventArgs" /> class. </summary>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ListViewItem" /> that is being checked or unchecked.</param>
		// Token: 0x060026EE RID: 9966 RVA: 0x000B76B5 File Offset: 0x000B58B5
		public ItemCheckedEventArgs(ListViewItem item)
		{
			this.lvi = item;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ListViewItem" /> whose checked state is changing.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> whose checked state is changing.</returns>
		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x000B76C4 File Offset: 0x000B58C4
		public ListViewItem Item
		{
			get
			{
				return this.lvi;
			}
		}

		// Token: 0x04001072 RID: 4210
		private ListViewItem lvi;
	}
}
