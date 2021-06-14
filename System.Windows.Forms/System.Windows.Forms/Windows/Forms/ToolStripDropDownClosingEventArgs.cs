using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStripDropDown.Closing" /> event.</summary>
	// Token: 0x020003AC RID: 940
	public class ToolStripDropDownClosingEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownClosingEventArgs" /> class with the specified reason for closing. </summary>
		/// <param name="reason">One of the <see cref="T:System.Windows.Forms.ToolStripDropDownCloseReason" /> values.</param>
		// Token: 0x06003DEB RID: 15851 RVA: 0x0010DDBF File Offset: 0x0010BFBF
		public ToolStripDropDownClosingEventArgs(ToolStripDropDownCloseReason reason)
		{
			this.closeReason = reason;
		}

		/// <summary>Gets the reason that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> is closing.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripDropDownCloseReason" /> values.</returns>
		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06003DEC RID: 15852 RVA: 0x0010DDCE File Offset: 0x0010BFCE
		public ToolStripDropDownCloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
		}

		// Token: 0x040023D6 RID: 9174
		private ToolStripDropDownCloseReason closeReason;
	}
}
