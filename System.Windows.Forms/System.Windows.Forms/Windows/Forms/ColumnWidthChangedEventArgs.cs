using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.ColumnWidthChanged" /> event. </summary>
	// Token: 0x0200014B RID: 331
	public class ColumnWidthChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColumnWidthChangedEventArgs" /> class. </summary>
		/// <param name="columnIndex">The index of the column whose width is being changed.</param>
		// Token: 0x06000A80 RID: 2688 RVA: 0x0001FA85 File Offset: 0x0001DC85
		public ColumnWidthChangedEventArgs(int columnIndex)
		{
			this.columnIndex = columnIndex;
		}

		/// <summary>Gets the column index for the column whose width is being changed.</summary>
		/// <returns>The index of the column whose width is being changed.</returns>
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0001FA94 File Offset: 0x0001DC94
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x040006F4 RID: 1780
		private readonly int columnIndex;
	}
}
