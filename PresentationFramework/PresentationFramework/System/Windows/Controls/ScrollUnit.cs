using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the type of unit that is used by the <see cref="P:System.Windows.Controls.VirtualizingPanel.ScrollUnit" /> attached property.</summary>
	// Token: 0x02000523 RID: 1315
	public enum ScrollUnit
	{
		/// <summary>The <see cref="P:System.Windows.Controls.VirtualizingPanel.ScrollUnit" /> is measured in terms of device-independent units (1/96th inch per unit).</summary>
		// Token: 0x04002DBB RID: 11707
		Pixel,
		/// <summary>The <see cref="P:System.Windows.Controls.VirtualizingPanel.ScrollUnit" /> is measured in terms of the items that are displayed in the panel.</summary>
		// Token: 0x04002DBC RID: 11708
		Item
	}
}
