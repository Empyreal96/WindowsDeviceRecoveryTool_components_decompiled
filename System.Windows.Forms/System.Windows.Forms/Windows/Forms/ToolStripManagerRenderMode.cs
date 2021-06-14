using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Specifies the painting style applied to multiple <see cref="T:System.Windows.Forms.ToolStrip" /> objects contained in a form.</summary>
	// Token: 0x020003D5 RID: 981
	public enum ToolStripManagerRenderMode
	{
		/// <summary>Indicates the use of a <see cref="T:System.Windows.Forms.ToolStripRenderer" /> other than <see cref="T:System.Windows.Forms.ToolStripProfessionalRenderer" /> or <see cref="T:System.Windows.Forms.ToolStripSystemRenderer" />.</summary>
		// Token: 0x040024F2 RID: 9458
		[Browsable(false)]
		Custom,
		/// <summary>Indicates the use of a <see cref="T:System.Windows.Forms.ToolStripSystemRenderer" /> to paint.</summary>
		// Token: 0x040024F3 RID: 9459
		System,
		/// <summary>Indicates the use of a <see cref="T:System.Windows.Forms.ToolStripProfessionalRenderer" /> to paint.</summary>
		// Token: 0x040024F4 RID: 9460
		Professional
	}
}
