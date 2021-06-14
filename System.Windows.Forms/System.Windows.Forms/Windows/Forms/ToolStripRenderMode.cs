using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Specifies the painting style applied to one <see cref="T:System.Windows.Forms.ToolStrip" /> contained in a form.</summary>
	// Token: 0x020003EC RID: 1004
	public enum ToolStripRenderMode
	{
		/// <summary>Indicates that the <see cref="P:System.Windows.Forms.ToolStrip.RenderMode" /> is not determined by the <see cref="T:System.Windows.Forms.ToolStripManager" /> or the use of a <see cref="T:System.Windows.Forms.ToolStripRenderer" /> other than <see cref="T:System.Windows.Forms.ToolStripProfessionalRenderer" />, <see cref="T:System.Windows.Forms.ToolStripSystemRenderer" /></summary>
		// Token: 0x040025A4 RID: 9636
		[Browsable(false)]
		Custom,
		/// <summary>Indicates the use of a <see cref="T:System.Windows.Forms.ToolStripSystemRenderer" /> to paint.</summary>
		// Token: 0x040025A5 RID: 9637
		System,
		/// <summary>Indicates the use of a <see cref="T:System.Windows.Forms.ToolStripProfessionalRenderer" /> to paint.</summary>
		// Token: 0x040025A6 RID: 9638
		Professional,
		/// <summary>Indicates that the <see cref="P:System.Windows.Forms.ToolStripManager.RenderMode" /> or <see cref="P:System.Windows.Forms.ToolStripManager.Renderer" /> determines the painting style.</summary>
		// Token: 0x040025A7 RID: 9639
		ManagerRenderMode
	}
}
