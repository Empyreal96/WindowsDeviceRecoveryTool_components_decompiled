using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies that <see cref="P:System.Windows.Forms.SplitContainer.Panel1" />, <see cref="P:System.Windows.Forms.SplitContainer.Panel2" />, or neither panel is fixed.</summary>
	// Token: 0x02000242 RID: 578
	public enum FixedPanel
	{
		/// <summary>Specifies that neither <see cref="P:System.Windows.Forms.SplitContainer.Panel1" />, <see cref="P:System.Windows.Forms.SplitContainer.Panel2" /> is fixed. A <see cref="E:System.Windows.Forms.Control.Resize" /> event affects both panels.</summary>
		// Token: 0x04000EEA RID: 3818
		None,
		/// <summary>Specifies that <see cref="P:System.Windows.Forms.SplitContainer.Panel1" /> is fixed. A <see cref="E:System.Windows.Forms.Control.Resize" /> event affects only <see cref="P:System.Windows.Forms.SplitContainer.Panel2" />.</summary>
		// Token: 0x04000EEB RID: 3819
		Panel1,
		/// <summary>Specifies that <see cref="P:System.Windows.Forms.SplitContainer.Panel2" /> is fixed. A <see cref="E:System.Windows.Forms.Control.Resize" /> event affects only <see cref="P:System.Windows.Forms.SplitContainer.Panel1" />.</summary>
		// Token: 0x04000EEC RID: 3820
		Panel2
	}
}
