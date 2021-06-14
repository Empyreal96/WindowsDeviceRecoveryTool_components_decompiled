using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies which child controls to skip.</summary>
	// Token: 0x0200025E RID: 606
	[Flags]
	public enum GetChildAtPointSkip
	{
		/// <summary>Does not skip any child windows.</summary>
		// Token: 0x04000FAD RID: 4013
		None = 0,
		/// <summary>Skips invisible child windows.</summary>
		// Token: 0x04000FAE RID: 4014
		Invisible = 1,
		/// <summary>Skips disabled child windows.</summary>
		// Token: 0x04000FAF RID: 4015
		Disabled = 2,
		/// <summary>Skips transparent child windows.</summary>
		// Token: 0x04000FB0 RID: 4016
		Transparent = 4
	}
}
