using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Form.FormClosed" /> event.</summary>
	// Token: 0x0200024F RID: 591
	public class FormClosedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.FormClosedEventArgs" /> class.</summary>
		/// <param name="closeReason">A <see cref="T:System.Windows.Forms.CloseReason" /> value that represents the reason why the form was closed.</param>
		// Token: 0x06002429 RID: 9257 RVA: 0x000B085E File Offset: 0x000AEA5E
		public FormClosedEventArgs(CloseReason closeReason)
		{
			this.closeReason = closeReason;
		}

		/// <summary>Gets a value that indicates why the form was closed. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CloseReason" /> enumerated values. </returns>
		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x000B086D File Offset: 0x000AEA6D
		public CloseReason CloseReason
		{
			get
			{
				return this.closeReason;
			}
		}

		// Token: 0x04000F8A RID: 3978
		private CloseReason closeReason;
	}
}
