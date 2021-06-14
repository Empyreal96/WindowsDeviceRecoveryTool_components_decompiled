using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStripDropDown.Closed" /> event. </summary>
	// Token: 0x020003AA RID: 938
	public class ToolStripDropDownClosedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownClosedEventArgs" /> class. </summary>
		/// <param name="reason">One of the <see cref="T:System.Windows.Forms.ToolStripDropDownCloseReason" /> values.</param>
		// Token: 0x06003DE5 RID: 15845 RVA: 0x0010DDA8 File Offset: 0x0010BFA8
		public ToolStripDropDownClosedEventArgs(ToolStripDropDownCloseReason reason)
		{
			this.closeReason = reason;
		}

		/// <summary>Gets the reason that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> closed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripDropDownCloseReason" /> values.</returns>
		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x0010DDB7 File Offset: 0x0010BFB7
		public ToolStripDropDownCloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
		}

		// Token: 0x040023D5 RID: 9173
		private ToolStripDropDownCloseReason closeReason;
	}
}
