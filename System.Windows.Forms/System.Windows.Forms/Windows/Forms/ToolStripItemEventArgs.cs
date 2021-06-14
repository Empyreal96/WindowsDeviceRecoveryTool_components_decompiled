using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for <see cref="T:System.Windows.Forms.ToolStripItem" /> events.</summary>
	// Token: 0x020003C2 RID: 962
	public class ToolStripItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItemEventArgs" /> class, specifying a <see cref="T:System.Windows.Forms.ToolStripItem" />. </summary>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripItem" /> for which to specify events.</param>
		// Token: 0x0600405A RID: 16474 RVA: 0x001163D9 File Offset: 0x001145D9
		public ToolStripItemEventArgs(ToolStripItem item)
		{
			this.item = item;
		}

		/// <summary>Gets a <see cref="T:System.Windows.Forms.ToolStripItem" /> for which to handle events.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItem" /> for which to handle events.</returns>
		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x001163E8 File Offset: 0x001145E8
		public ToolStripItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x0400249E RID: 9374
		private ToolStripItem item;
	}
}
