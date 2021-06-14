using System;

namespace System.Drawing.Printing
{
	/// <summary>Specifies the part of the document to print.</summary>
	// Token: 0x0200006E RID: 110
	[Serializable]
	public enum PrintRange
	{
		/// <summary>All pages are printed.</summary>
		// Token: 0x040006F9 RID: 1785
		AllPages,
		/// <summary>The pages between <see cref="P:System.Drawing.Printing.PrinterSettings.FromPage" /> and <see cref="P:System.Drawing.Printing.PrinterSettings.ToPage" /> are printed.</summary>
		// Token: 0x040006FA RID: 1786
		SomePages = 2,
		/// <summary>The selected pages are printed.</summary>
		// Token: 0x040006FB RID: 1787
		Selection = 1,
		/// <summary>The currently displayed page is printed</summary>
		// Token: 0x040006FC RID: 1788
		CurrentPage = 4194304
	}
}
