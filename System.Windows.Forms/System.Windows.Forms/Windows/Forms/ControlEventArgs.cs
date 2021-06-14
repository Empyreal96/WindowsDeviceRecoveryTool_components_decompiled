using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.ControlAdded" /> and <see cref="E:System.Windows.Forms.Control.ControlRemoved" /> events.</summary>
	// Token: 0x0200015D RID: 349
	public class ControlEventArgs : EventArgs
	{
		/// <summary>Gets the control object used by this event.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> used by this event.</returns>
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0003503E File Offset: 0x0003323E
		public Control Control
		{
			get
			{
				return this.control;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ControlEventArgs" /> class for the specified control.</summary>
		/// <param name="control">The <see cref="T:System.Windows.Forms.Control" /> to store in this event. </param>
		// Token: 0x06000FC9 RID: 4041 RVA: 0x00035046 File Offset: 0x00033246
		public ControlEventArgs(Control control)
		{
			this.control = control;
		}

		// Token: 0x04000853 RID: 2131
		private Control control;
	}
}
