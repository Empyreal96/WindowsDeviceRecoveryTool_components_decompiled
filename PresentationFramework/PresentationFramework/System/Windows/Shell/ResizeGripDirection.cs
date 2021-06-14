using System;

namespace System.Windows.Shell
{
	/// <summary>Specifies constants that indicate the direction of the resize grip behavior on an input element.</summary>
	// Token: 0x0200014F RID: 335
	public enum ResizeGripDirection
	{
		/// <summary>No resize behavior is specified.</summary>
		// Token: 0x04001152 RID: 4434
		None,
		/// <summary>The window resizes from its top-left corner.</summary>
		// Token: 0x04001153 RID: 4435
		TopLeft,
		/// <summary>The window resizes from its top edge.</summary>
		// Token: 0x04001154 RID: 4436
		Top,
		/// <summary>The window resizes from its top-right corner.</summary>
		// Token: 0x04001155 RID: 4437
		TopRight,
		/// <summary>The window resizes from its right edge.</summary>
		// Token: 0x04001156 RID: 4438
		Right,
		/// <summary>The window resizes from its bottom-right corner.</summary>
		// Token: 0x04001157 RID: 4439
		BottomRight,
		/// <summary>The window resizes from its bottom edge.</summary>
		// Token: 0x04001158 RID: 4440
		Bottom,
		/// <summary>The window resizes from its bottom-left corner.</summary>
		// Token: 0x04001159 RID: 4441
		BottomLeft,
		/// <summary>The windows resizes from its left edge.</summary>
		// Token: 0x0400115A RID: 4442
		Left
	}
}
