using System;
using System.Windows;
using System.Windows.Xps;

namespace MS.Internal.Documents
{
	// Token: 0x020006C4 RID: 1732
	internal class FlowDocumentPrintingState
	{
		// Token: 0x040036D7 RID: 14039
		internal XpsDocumentWriter XpsDocumentWriter;

		// Token: 0x040036D8 RID: 14040
		internal Size PageSize;

		// Token: 0x040036D9 RID: 14041
		internal Thickness PagePadding;

		// Token: 0x040036DA RID: 14042
		internal double ColumnWidth;

		// Token: 0x040036DB RID: 14043
		internal bool IsSelectionEnabled;
	}
}
