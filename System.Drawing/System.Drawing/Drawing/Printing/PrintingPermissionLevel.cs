using System;

namespace System.Drawing.Printing
{
	/// <summary>Specifies the type of printing that code is allowed to do.</summary>
	// Token: 0x0200006B RID: 107
	[Serializable]
	public enum PrintingPermissionLevel
	{
		/// <summary>Prevents access to printers. <see cref="F:System.Drawing.Printing.PrintingPermissionLevel.NoPrinting" /> is a subset of <see cref="F:System.Drawing.Printing.PrintingPermissionLevel.SafePrinting" />.</summary>
		// Token: 0x040006ED RID: 1773
		NoPrinting,
		/// <summary>Provides printing only from a restricted dialog box. <see cref="F:System.Drawing.Printing.PrintingPermissionLevel.SafePrinting" /> is a subset of <see cref="F:System.Drawing.Printing.PrintingPermissionLevel.DefaultPrinting" />.</summary>
		// Token: 0x040006EE RID: 1774
		SafePrinting,
		/// <summary>Provides printing programmatically to the default printer, along with safe printing through semirestricted dialog box. <see cref="F:System.Drawing.Printing.PrintingPermissionLevel.DefaultPrinting" /> is a subset of <see cref="F:System.Drawing.Printing.PrintingPermissionLevel.AllPrinting" />.</summary>
		// Token: 0x040006EF RID: 1775
		DefaultPrinting,
		/// <summary>Provides full access to all printers.</summary>
		// Token: 0x040006F0 RID: 1776
		AllPrinting
	}
}
