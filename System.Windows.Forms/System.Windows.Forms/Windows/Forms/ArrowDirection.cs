using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the direction to move when getting items with the <see cref="M:System.Windows.Forms.ToolStrip.GetNextItem(System.Windows.Forms.ToolStripItem,System.Windows.Forms.ArrowDirection)" /> method.</summary>
	// Token: 0x02000113 RID: 275
	public enum ArrowDirection
	{
		/// <summary>The direction is up (<see cref="F:System.Windows.Forms.Orientation.Vertical" />).</summary>
		// Token: 0x0400053A RID: 1338
		Up = 1,
		/// <summary>The direction is down (<see cref="F:System.Windows.Forms.Orientation.Vertical" />).</summary>
		// Token: 0x0400053B RID: 1339
		Down = 17,
		/// <summary>The direction is left (<see cref="F:System.Windows.Forms.Orientation.Horizontal" />).</summary>
		// Token: 0x0400053C RID: 1340
		Left = 0,
		/// <summary>The direction is right (<see cref="F:System.Windows.Forms.Orientation.Horizontal" />).</summary>
		// Token: 0x0400053D RID: 1341
		Right = 16
	}
}
