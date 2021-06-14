using System;
using System.Windows.Threading;

namespace System.Windows.Interop
{
	/// <summary>Defines the interaction between Windows Presentation Foundation (WPF) applications that are hosting interoperation content and interpreted by the Windows Presentation Foundation (WPF) executable, and a host supplied error page. </summary>
	// Token: 0x020005BF RID: 1471
	public interface IErrorPage
	{
		/// <summary>Gets or sets the path to an application's deployment manifest.</summary>
		/// <returns>The path to an application's deployment manifest.</returns>
		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x0600622C RID: 25132
		// (set) Token: 0x0600622D RID: 25133
		Uri DeploymentPath { get; set; }

		/// <summary>Gets or sets the string title of the error page.</summary>
		/// <returns>The string title of the error page.</returns>
		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x0600622E RID: 25134
		// (set) Token: 0x0600622F RID: 25135
		string ErrorTitle { get; set; }

		/// <summary>Gets or sets a verbose description of the error.</summary>
		/// <returns>Description of the error.</returns>
		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x06006230 RID: 25136
		// (set) Token: 0x06006231 RID: 25137
		string ErrorText { get; set; }

		/// <summary>Gets or sets a value that indicates whether this represents an error or some other condition such as a warning. <see langword="true" /> denotes an error.</summary>
		/// <returns>
		///     <see langword="true" /> denotes an error; <see langword="false" /> denotes another condition such as a warning.</returns>
		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x06006232 RID: 25138
		// (set) Token: 0x06006233 RID: 25139
		bool ErrorFlag { get; set; }

		/// <summary>Gets or sets the string path to the error's log file, if any.</summary>
		/// <returns>Path to an associated error file. May be an empty string.</returns>
		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x06006234 RID: 25140
		// (set) Token: 0x06006235 RID: 25141
		string LogFilePath { get; set; }

		/// <summary>Gets or sets a uniform resource identifier (URI) for support information associated with the error.</summary>
		/// <returns>A link for support information.</returns>
		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x06006236 RID: 25142
		// (set) Token: 0x06006237 RID: 25143
		Uri SupportUri { get; set; }

		/// <summary>Gets or sets a reference to a <see cref="T:System.Windows.Threading.DispatcherOperationCallback" /> handler, that can handle refresh of the error page.</summary>
		/// <returns>A <see cref="T:System.Windows.Threading.DispatcherOperationCallback" /> handler to handle refresh of error page.</returns>
		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x06006238 RID: 25144
		// (set) Token: 0x06006239 RID: 25145
		DispatcherOperationCallback RefreshCallback { get; set; }

		/// <summary>Gets or sets a reference to a <see cref="T:System.Windows.Threading.DispatcherOperationCallback" />  handler, which can handle requests for Microsoft .NET Framework runtime downloads.</summary>
		/// <returns>A <see cref="T:System.Windows.Threading.DispatcherOperationCallback" />  handler,</returns>
		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x0600623A RID: 25146
		// (set) Token: 0x0600623B RID: 25147
		DispatcherOperationCallback GetWinFxCallback { get; set; }
	}
}
