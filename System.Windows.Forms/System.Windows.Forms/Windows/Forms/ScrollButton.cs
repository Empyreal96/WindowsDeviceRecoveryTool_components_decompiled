using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the type of scroll arrow to draw on a scroll bar.</summary>
	// Token: 0x02000348 RID: 840
	public enum ScrollButton
	{
		/// <summary>A down-scroll arrow.</summary>
		// Token: 0x04002070 RID: 8304
		Down = 1,
		/// <summary>A left-scroll arrow.</summary>
		// Token: 0x04002071 RID: 8305
		Left,
		/// <summary>A right-scroll arrow.</summary>
		// Token: 0x04002072 RID: 8306
		Right,
		/// <summary>An up-scroll arrow.</summary>
		// Token: 0x04002073 RID: 8307
		Up = 0,
		/// <summary>A minimum-scroll arrow.</summary>
		// Token: 0x04002074 RID: 8308
		Min = 0,
		/// <summary>A maximum-scroll arrow.</summary>
		// Token: 0x04002075 RID: 8309
		Max = 3
	}
}
