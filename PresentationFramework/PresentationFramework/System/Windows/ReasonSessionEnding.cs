using System;

namespace System.Windows
{
	/// <summary>Specifies the reason for which the user's session is ending. Used by the <see cref="P:System.Windows.SessionEndingCancelEventArgs.ReasonSessionEnding" /> property.</summary>
	// Token: 0x0200009E RID: 158
	public enum ReasonSessionEnding : byte
	{
		/// <summary>The session is ending because the user is logging off.</summary>
		// Token: 0x040005C5 RID: 1477
		Logoff,
		/// <summary>The session is ending because the user is shutting down Windows.</summary>
		// Token: 0x040005C6 RID: 1478
		Shutdown
	}
}
