using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the possible alignments with which the items of a <see cref="T:System.Windows.Forms.ToolStrip" /> can be displayed.</summary>
	// Token: 0x020003D0 RID: 976
	public enum ToolStripLayoutStyle
	{
		/// <summary>Specifies that items are laid out automatically.</summary>
		// Token: 0x040024D5 RID: 9429
		StackWithOverflow,
		/// <summary>Specifies that items are laid out horizontally and overflow as necessary.</summary>
		// Token: 0x040024D6 RID: 9430
		HorizontalStackWithOverflow,
		/// <summary>Specifies that items are laid out vertically, are centered within the control, and overflow as necessary.</summary>
		// Token: 0x040024D7 RID: 9431
		VerticalStackWithOverflow,
		/// <summary>Specifies that items flow horizontally or vertically as necessary.</summary>
		// Token: 0x040024D8 RID: 9432
		Flow,
		/// <summary>Specifies that items are laid out flush left.</summary>
		// Token: 0x040024D9 RID: 9433
		Table
	}
}
