using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.ColumnReordered" /> event. </summary>
	// Token: 0x02000148 RID: 328
	public class ColumnReorderedEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColumnReorderedEventArgs" /> class. </summary>
		/// <param name="oldDisplayIndex">The previous display position of the <see cref="T:System.Windows.Forms.ColumnHeader" />.</param>
		/// <param name="newDisplayIndex">The new display position for the <see cref="T:System.Windows.Forms.ColumnHeader" />.</param>
		/// <param name="header">The <see cref="T:System.Windows.Forms.ColumnHeader" /> that is being reordered.</param>
		// Token: 0x06000A78 RID: 2680 RVA: 0x0001FA50 File Offset: 0x0001DC50
		public ColumnReorderedEventArgs(int oldDisplayIndex, int newDisplayIndex, ColumnHeader header)
		{
			this.oldDisplayIndex = oldDisplayIndex;
			this.newDisplayIndex = newDisplayIndex;
			this.header = header;
		}

		/// <summary>Gets the previous display position of the <see cref="T:System.Windows.Forms.ColumnHeader" />.</summary>
		/// <returns>The previous display position of the <see cref="T:System.Windows.Forms.ColumnHeader" /></returns>
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0001FA6D File Offset: 0x0001DC6D
		public int OldDisplayIndex
		{
			get
			{
				return this.oldDisplayIndex;
			}
		}

		/// <summary>Gets the new display position of the <see cref="T:System.Windows.Forms.ColumnHeader" /></summary>
		/// <returns>The new display position of the <see cref="T:System.Windows.Forms.ColumnHeader" />.</returns>
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0001FA75 File Offset: 0x0001DC75
		public int NewDisplayIndex
		{
			get
			{
				return this.newDisplayIndex;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ColumnHeader" /> that is being reordered.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> that is being reordered.</returns>
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0001FA7D File Offset: 0x0001DC7D
		public ColumnHeader Header
		{
			get
			{
				return this.header;
			}
		}

		// Token: 0x040006ED RID: 1773
		private int oldDisplayIndex;

		// Token: 0x040006EE RID: 1774
		private int newDisplayIndex;

		// Token: 0x040006EF RID: 1775
		private ColumnHeader header;
	}
}
