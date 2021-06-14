using System;

namespace System.Windows
{
	/// <summary>Event arguments for the <see cref="E:System.Windows.Application.Exit" /> event.</summary>
	// Token: 0x020000BA RID: 186
	public class ExitEventArgs : EventArgs
	{
		// Token: 0x060003FA RID: 1018 RVA: 0x0000B632 File Offset: 0x00009832
		internal ExitEventArgs(int exitCode)
		{
			this._exitCode = exitCode;
		}

		/// <summary>Gets or sets the exit code that an application returns to the operating system when the application exits.</summary>
		/// <returns>The exit code that an application returns to the operating system when the application exits.</returns>
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000B641 File Offset: 0x00009841
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x0000B649 File Offset: 0x00009849
		public int ApplicationExitCode
		{
			get
			{
				return this._exitCode;
			}
			set
			{
				this._exitCode = value;
			}
		}

		// Token: 0x04000620 RID: 1568
		internal int _exitCode;
	}
}
