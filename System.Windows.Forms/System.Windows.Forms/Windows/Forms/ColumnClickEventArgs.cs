using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.ColumnClick" /> event.</summary>
	// Token: 0x02000143 RID: 323
	public class ColumnClickEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColumnClickEventArgs" /> class.</summary>
		/// <param name="column">The zero-based index of the column that is clicked. </param>
		// Token: 0x06000A4A RID: 2634 RVA: 0x0001F193 File Offset: 0x0001D393
		public ColumnClickEventArgs(int column)
		{
			this.column = column;
		}

		/// <summary>Gets the zero-based index of the column that is clicked.</summary>
		/// <returns>The zero-based index within the <see cref="T:System.Windows.Forms.ListView.ColumnHeaderCollection" /> of the column that is clicked.</returns>
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0001F1A2 File Offset: 0x0001D3A2
		public int Column
		{
			get
			{
				return this.column;
			}
		}

		// Token: 0x040006DE RID: 1758
		private readonly int column;
	}
}
