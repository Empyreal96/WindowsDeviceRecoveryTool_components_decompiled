using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.PropertyGrid.SelectedGridItemChanged" /> event of the <see cref="T:System.Windows.Forms.PropertyGrid" /> control.</summary>
	// Token: 0x02000352 RID: 850
	public class SelectedGridItemChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.SelectedGridItemChangedEventArgs" /> class.</summary>
		/// <param name="oldSel">The previously selected grid item. </param>
		/// <param name="newSel">The newly selected grid item. </param>
		// Token: 0x06003518 RID: 13592 RVA: 0x000F212C File Offset: 0x000F032C
		public SelectedGridItemChangedEventArgs(GridItem oldSel, GridItem newSel)
		{
			this.oldSelection = oldSel;
			this.newSelection = newSel;
		}

		/// <summary>Gets the newly selected <see cref="T:System.Windows.Forms.GridItem" />.</summary>
		/// <returns>The new <see cref="T:System.Windows.Forms.GridItem" />.</returns>
		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003519 RID: 13593 RVA: 0x000F2142 File Offset: 0x000F0342
		public GridItem NewSelection
		{
			get
			{
				return this.newSelection;
			}
		}

		/// <summary>Gets the previously selected <see cref="T:System.Windows.Forms.GridItem" />.</summary>
		/// <returns>The old <see cref="T:System.Windows.Forms.GridItem" />. This can be <see langword="null" />.</returns>
		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x0600351A RID: 13594 RVA: 0x000F214A File Offset: 0x000F034A
		public GridItem OldSelection
		{
			get
			{
				return this.oldSelection;
			}
		}

		// Token: 0x040020AA RID: 8362
		private GridItem oldSelection;

		// Token: 0x040020AB RID: 8363
		private GridItem newSelection;
	}
}
