using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies a location in a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001ED RID: 493
	public enum DataGridViewHitTestType
	{
		/// <summary>An empty part of the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x04000D26 RID: 3366
		None,
		/// <summary>A cell in the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x04000D27 RID: 3367
		Cell,
		/// <summary>A column header in the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x04000D28 RID: 3368
		ColumnHeader,
		/// <summary>A row header in the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x04000D29 RID: 3369
		RowHeader,
		/// <summary>The top left column header in the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x04000D2A RID: 3370
		TopLeftHeader,
		/// <summary>The horizontal scroll bar of the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x04000D2B RID: 3371
		HorizontalScrollBar,
		/// <summary>The vertical scroll bar of the <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x04000D2C RID: 3372
		VerticalScrollBar
	}
}
