using System;

namespace System.Drawing.Printing
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Drawing.Printing.PrintDocument.BeginPrint" /> or <see cref="E:System.Drawing.Printing.PrintDocument.EndPrint" /> event of a <see cref="T:System.Drawing.Printing.PrintDocument" />.</summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintEventArgs" /> that contains the event data. </param>
	// Token: 0x02000068 RID: 104
	// (Invoke) Token: 0x060007F4 RID: 2036
	public delegate void PrintEventHandler(object sender, PrintEventArgs e);
}
