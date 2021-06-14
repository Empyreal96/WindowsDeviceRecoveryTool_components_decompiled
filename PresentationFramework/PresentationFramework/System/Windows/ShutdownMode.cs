using System;

namespace System.Windows
{
	/// <summary>Specifies how an application will shutdown. Used by the <see cref="P:System.Windows.Application.ShutdownMode" /> property.</summary>
	// Token: 0x0200009D RID: 157
	public enum ShutdownMode : byte
	{
		/// <summary>An application shuts down when either the last window closes, or <see cref="M:System.Windows.Application.Shutdown" /> is called.</summary>
		// Token: 0x040005C1 RID: 1473
		OnLastWindowClose,
		/// <summary>An application shuts down when either the main window closes, or <see cref="M:System.Windows.Application.Shutdown" /> is called.</summary>
		// Token: 0x040005C2 RID: 1474
		OnMainWindowClose,
		/// <summary>An application shuts down only when <see cref="M:System.Windows.Application.Shutdown" /> is called.</summary>
		// Token: 0x040005C3 RID: 1475
		OnExplicitShutdown
	}
}
