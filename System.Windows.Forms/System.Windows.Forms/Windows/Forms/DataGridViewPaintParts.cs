using System;

namespace System.Windows.Forms
{
	/// <summary>Defines values for specifying the parts of a <see cref="T:System.Windows.Forms.DataGridViewCell" /> that are to be painted.</summary>
	// Token: 0x020001F6 RID: 502
	[Flags]
	public enum DataGridViewPaintParts
	{
		/// <summary>Nothing should be painted.</summary>
		// Token: 0x04000D5F RID: 3423
		None = 0,
		/// <summary>All parts of the cell should be painted.</summary>
		// Token: 0x04000D60 RID: 3424
		All = 127,
		/// <summary>The background of the cell should be painted.</summary>
		// Token: 0x04000D61 RID: 3425
		Background = 1,
		/// <summary>The border of the cell should be painted.</summary>
		// Token: 0x04000D62 RID: 3426
		Border = 2,
		/// <summary>The background of the cell content should be painted.</summary>
		// Token: 0x04000D63 RID: 3427
		ContentBackground = 4,
		/// <summary>The foreground of the cell content should be painted.</summary>
		// Token: 0x04000D64 RID: 3428
		ContentForeground = 8,
		/// <summary>The cell error icon should be painted.</summary>
		// Token: 0x04000D65 RID: 3429
		ErrorIcon = 16,
		/// <summary>The focus rectangle should be painted around the cell.</summary>
		// Token: 0x04000D66 RID: 3430
		Focus = 32,
		/// <summary>The background of the cell should be painted when the cell is selected.</summary>
		// Token: 0x04000D67 RID: 3431
		SelectionBackground = 64
	}
}
