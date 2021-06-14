using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies how elements with a bitmap background will adjust to fill a bounds.</summary>
	// Token: 0x0200045A RID: 1114
	public enum SizingType
	{
		/// <summary>The element cannot be resized.</summary>
		// Token: 0x040031DB RID: 12763
		FixedSize,
		/// <summary>The background image stretches to fill the bounds.</summary>
		// Token: 0x040031DC RID: 12764
		Stretch,
		/// <summary>The background image repeats the pattern to fill the bounds.</summary>
		// Token: 0x040031DD RID: 12765
		Tile
	}
}
