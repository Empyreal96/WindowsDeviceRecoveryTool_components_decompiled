using System;
using System.ComponentModel;

namespace System.Windows
{
	/// <summary>Contains the event arguments for the <see cref="E:System.Windows.Application.SessionEnding" /> event.</summary>
	// Token: 0x020000F8 RID: 248
	public class SessionEndingCancelEventArgs : CancelEventArgs
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x0001C3B6 File Offset: 0x0001A5B6
		internal SessionEndingCancelEventArgs(ReasonSessionEnding reasonSessionEnding)
		{
			this._reasonSessionEnding = reasonSessionEnding;
		}

		/// <summary>Gets a value that indicates why the session is ending.</summary>
		/// <returns>A <see cref="T:System.Windows.ReasonSessionEnding" /> value that indicates why the session ended.</returns>
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0001C3C5 File Offset: 0x0001A5C5
		public ReasonSessionEnding ReasonSessionEnding
		{
			get
			{
				return this._reasonSessionEnding;
			}
		}

		// Token: 0x040007BA RID: 1978
		private ReasonSessionEnding _reasonSessionEnding;
	}
}
