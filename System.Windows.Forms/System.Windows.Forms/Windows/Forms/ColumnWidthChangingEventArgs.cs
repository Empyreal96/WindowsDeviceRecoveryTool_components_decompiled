using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.ColumnWidthChanging" /> event. </summary>
	// Token: 0x0200014D RID: 333
	public class ColumnWidthChangingEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColumnWidthChangingEventArgs" /> class, specifying the column index and width and whether to cancel the event.</summary>
		/// <param name="columnIndex">The index of the column whose width is changing.</param>
		/// <param name="newWidth">The new width of the column.</param>
		/// <param name="cancel">
		///       <see langword="true" /> to cancel the width change; otherwise, <see langword="false" />.</param>
		// Token: 0x06000A86 RID: 2694 RVA: 0x0001FA9C File Offset: 0x0001DC9C
		public ColumnWidthChangingEventArgs(int columnIndex, int newWidth, bool cancel) : base(cancel)
		{
			this.columnIndex = columnIndex;
			this.newWidth = newWidth;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColumnWidthChangingEventArgs" /> class with the specified column index and width.</summary>
		/// <param name="columnIndex">The index of the column whose width is changing.</param>
		/// <param name="newWidth">The new width for the column.</param>
		// Token: 0x06000A87 RID: 2695 RVA: 0x0001FAB3 File Offset: 0x0001DCB3
		public ColumnWidthChangingEventArgs(int columnIndex, int newWidth)
		{
			this.columnIndex = columnIndex;
			this.newWidth = newWidth;
		}

		/// <summary>Gets the index of the column whose width is changing.</summary>
		/// <returns>The index of the column whose width is changing.</returns>
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0001FAC9 File Offset: 0x0001DCC9
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets or sets the new width for the column.</summary>
		/// <returns>The new width for the column.</returns>
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0001FAD1 File Offset: 0x0001DCD1
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x0001FAD9 File Offset: 0x0001DCD9
		public int NewWidth
		{
			get
			{
				return this.newWidth;
			}
			set
			{
				this.newWidth = value;
			}
		}

		// Token: 0x040006F5 RID: 1781
		private int columnIndex;

		// Token: 0x040006F6 RID: 1782
		private int newWidth;
	}
}
