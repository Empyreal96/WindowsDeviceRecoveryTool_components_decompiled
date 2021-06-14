using System;

namespace System.Drawing.Printing
{
	/// <summary>Specifies the type of print operation occurring.</summary>
	// Token: 0x0200005F RID: 95
	public enum PrintAction
	{
		/// <summary>The print operation is printing to a file.</summary>
		// Token: 0x040006BA RID: 1722
		PrintToFile,
		/// <summary>The print operation is a print preview.</summary>
		// Token: 0x040006BB RID: 1723
		PrintToPreview,
		/// <summary>The print operation is printing to a printer.</summary>
		// Token: 0x040006BC RID: 1724
		PrintToPrinter
	}
}
