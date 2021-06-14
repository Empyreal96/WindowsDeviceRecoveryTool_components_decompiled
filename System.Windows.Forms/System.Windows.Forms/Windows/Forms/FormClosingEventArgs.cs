using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Form.FormClosing" /> event.</summary>
	// Token: 0x02000251 RID: 593
	public class FormClosingEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.FormClosingEventArgs" /> class.</summary>
		/// <param name="closeReason">A <see cref="T:System.Windows.Forms.CloseReason" /> value that represents the reason why the form is being closed.</param>
		/// <param name="cancel">
		///       <see langword="true" /> to cancel the event; otherwise, <see langword="false" />.</param>
		// Token: 0x0600242F RID: 9263 RVA: 0x000B0875 File Offset: 0x000AEA75
		public FormClosingEventArgs(CloseReason closeReason, bool cancel) : base(cancel)
		{
			this.closeReason = closeReason;
		}

		/// <summary>Gets a value that indicates why the form is being closed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CloseReason" /> enumerated values. </returns>
		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x000B0885 File Offset: 0x000AEA85
		public CloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
		}

		// Token: 0x04000F8B RID: 3979
		private CloseReason closeReason;
	}
}
