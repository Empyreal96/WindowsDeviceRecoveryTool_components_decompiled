using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies constants that define which mouse button was pressed.</summary>
	// Token: 0x020002F1 RID: 753
	[Flags]
	[ComVisible(true)]
	public enum MouseButtons
	{
		/// <summary>The left mouse button was pressed.</summary>
		// Token: 0x04001382 RID: 4994
		Left = 1048576,
		/// <summary>No mouse button was pressed.</summary>
		// Token: 0x04001383 RID: 4995
		None = 0,
		/// <summary>The right mouse button was pressed.</summary>
		// Token: 0x04001384 RID: 4996
		Right = 2097152,
		/// <summary>The middle mouse button was pressed.</summary>
		// Token: 0x04001385 RID: 4997
		Middle = 4194304,
		/// <summary>The first XButton was pressed.</summary>
		// Token: 0x04001386 RID: 4998
		XButton1 = 8388608,
		/// <summary>The second XButton was pressed.</summary>
		// Token: 0x04001387 RID: 4999
		XButton2 = 16777216
	}
}
