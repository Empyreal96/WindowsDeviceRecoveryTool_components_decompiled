using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how rows or columns of user interface (UI) elements should be sized relative to their container.</summary>
	// Token: 0x02000383 RID: 899
	public enum SizeType
	{
		/// <summary>The row or column should be automatically sized to share space with its peers.</summary>
		// Token: 0x04002271 RID: 8817
		AutoSize,
		/// <summary>The row or column should be sized to an exact number of pixels.</summary>
		// Token: 0x04002272 RID: 8818
		Absolute,
		/// <summary>The row or column should be sized as a percentage of the parent container.</summary>
		// Token: 0x04002273 RID: 8819
		Percent
	}
}
