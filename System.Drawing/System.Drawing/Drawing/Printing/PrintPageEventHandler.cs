using System;

namespace System.Drawing.Printing
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Drawing.Printing.PrintDocument.PrintPage" /> event of a <see cref="T:System.Drawing.Printing.PrintDocument" />.</summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains the event data. </param>
	// Token: 0x0200006D RID: 109
	// (Invoke) Token: 0x06000813 RID: 2067
	public delegate void PrintPageEventHandler(object sender, PrintPageEventArgs e);
}
