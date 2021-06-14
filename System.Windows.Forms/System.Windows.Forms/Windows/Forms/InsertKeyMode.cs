using System;

namespace System.Windows.Forms
{
	/// <summary>Represents the insertion mode used by text boxes.</summary>
	// Token: 0x020002A8 RID: 680
	public enum InsertKeyMode
	{
		/// <summary>Honors the current INSERT key mode of the keyboard.</summary>
		// Token: 0x04001152 RID: 4434
		Default,
		/// <summary>Indicates that the insertion mode is enabled regardless of the INSERT key mode of the keyboard.</summary>
		// Token: 0x04001153 RID: 4435
		Insert,
		/// <summary>Indicates that the overwrite mode is enabled regardless of the INSERT key mode of the keyboard.</summary>
		// Token: 0x04001154 RID: 4436
		Overwrite
	}
}
