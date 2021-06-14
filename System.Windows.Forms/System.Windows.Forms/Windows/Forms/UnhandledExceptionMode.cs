using System;

namespace System.Windows.Forms
{
	/// <summary>Defines where a Windows Forms application should send unhandled exceptions.</summary>
	// Token: 0x020002F6 RID: 758
	public enum UnhandledExceptionMode
	{
		/// <summary>Route all exceptions to the <see cref="E:System.Windows.Forms.Application.ThreadException" /> handler, unless the application's configuration file specifies otherwise.</summary>
		// Token: 0x04001D03 RID: 7427
		Automatic,
		/// <summary>Never route exceptions to the <see cref="E:System.Windows.Forms.Application.ThreadException" /> handler. Ignore the application configuration file.</summary>
		// Token: 0x04001D04 RID: 7428
		ThrowException,
		/// <summary>Always route exceptions to the <see cref="E:System.Windows.Forms.Application.ThreadException" /> handler. Ignore the application configuration file.</summary>
		// Token: 0x04001D05 RID: 7429
		CatchException
	}
}
