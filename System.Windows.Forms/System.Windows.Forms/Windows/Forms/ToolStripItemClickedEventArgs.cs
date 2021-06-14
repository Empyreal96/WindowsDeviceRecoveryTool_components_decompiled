using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStrip.ItemClicked" /> event.</summary>
	// Token: 0x020003BE RID: 958
	public class ToolStripItemClickedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItemClickedEventArgs" /> class, specifying the <see cref="T:System.Windows.Forms.ToolStripItem" /> that was clicked. </summary>
		/// <param name="clickedItem">The <see cref="T:System.Windows.Forms.ToolStripItem" /> that was clicked.</param>
		// Token: 0x0600402B RID: 16427 RVA: 0x00115B19 File Offset: 0x00113D19
		public ToolStripItemClickedEventArgs(ToolStripItem clickedItem)
		{
			this.clickedItem = clickedItem;
		}

		/// <summary>Gets the item that was clicked on the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripItem" /> that was clicked.</returns>
		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x0600402C RID: 16428 RVA: 0x00115B28 File Offset: 0x00113D28
		public ToolStripItem ClickedItem
		{
			get
			{
				return this.clickedItem;
			}
		}

		// Token: 0x04002494 RID: 9364
		private ToolStripItem clickedItem;
	}
}
