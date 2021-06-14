using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowHeightInfoPushed" /> event of a <see cref="T:System.Windows.Forms.DataGridView" />. </summary>
	// Token: 0x02000201 RID: 513
	public class DataGridViewRowHeightInfoPushedEventArgs : HandledEventArgs
	{
		// Token: 0x06001F4E RID: 8014 RVA: 0x0009E3F5 File Offset: 0x0009C5F5
		internal DataGridViewRowHeightInfoPushedEventArgs(int rowIndex, int height, int minimumHeight) : base(false)
		{
			this.rowIndex = rowIndex;
			this.height = height;
			this.minimumHeight = minimumHeight;
		}

		/// <summary>Gets the height of the row the event occurred for.</summary>
		/// <returns>The row height, in pixels.</returns>
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x0009E413 File Offset: 0x0009C613
		public int Height
		{
			get
			{
				return this.height;
			}
		}

		/// <summary>Gets the minimum height of the row the event occurred for.</summary>
		/// <returns>The minimum row height, in pixels.</returns>
		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x0009E41B File Offset: 0x0009C61B
		public int MinimumHeight
		{
			get
			{
				return this.minimumHeight;
			}
		}

		/// <summary>Gets the index of the row the event occurred for.</summary>
		/// <returns>The zero-based index of the row.</returns>
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x0009E423 File Offset: 0x0009C623
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04000D91 RID: 3473
		private int rowIndex;

		// Token: 0x04000D92 RID: 3474
		private int height;

		// Token: 0x04000D93 RID: 3475
		private int minimumHeight;
	}
}
