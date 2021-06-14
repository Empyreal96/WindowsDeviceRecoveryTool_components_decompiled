using System;
using System.Windows.Threading;

namespace System.Windows.Interop
{
	/// <summary>Defines the interaction between Windows Presentation Foundation (WPF) applications that are hosting interoperation content, and a host supplied progress page. </summary>
	// Token: 0x020005C0 RID: 1472
	public interface IProgressPage
	{
		/// <summary>Gets or sets the <see cref="T:System.Uri" /> path to the application deployment manifest.</summary>
		/// <returns>The application deployment manifest path.</returns>
		// Token: 0x17001798 RID: 6040
		// (get) Token: 0x0600623C RID: 25148
		// (set) Token: 0x0600623D RID: 25149
		Uri DeploymentPath { get; set; }

		/// <summary>Gets or sets a reference to a <see cref="T:System.Windows.Threading.DispatcherOperationCallback" /> handler, that can handle the case of a user-initiated Stop command.</summary>
		/// <returns>The callback reference.</returns>
		// Token: 0x17001799 RID: 6041
		// (get) Token: 0x0600623E RID: 25150
		// (set) Token: 0x0600623F RID: 25151
		DispatcherOperationCallback StopCallback { get; set; }

		/// <summary>Gets or sets a reference to a <see cref="T:System.Windows.Threading.DispatcherOperationCallback" /> handler, that can handle the case of a user-initiated Refresh command.</summary>
		/// <returns>The callback reference.</returns>
		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x06006240 RID: 25152
		// (set) Token: 0x06006241 RID: 25153
		DispatcherOperationCallback RefreshCallback { get; set; }

		/// <summary>Gets or sets  the application's name.</summary>
		/// <returns>Name of the application that originates the progress page.</returns>
		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x06006242 RID: 25154
		// (set) Token: 0x06006243 RID: 25155
		string ApplicationName { get; set; }

		/// <summary>Gets or sets the application's publisher.</summary>
		/// <returns>The publisher identifying string.</returns>
		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x06006244 RID: 25156
		// (set) Token: 0x06006245 RID: 25157
		string PublisherName { get; set; }

		/// <summary>Provides upload progress numeric information that can be used to update the progress indicators.</summary>
		/// <param name="bytesDownloaded">Total bytes downloaded thus far.</param>
		/// <param name="bytesTotal">Total bytes that need to be downloaded for the application.</param>
		// Token: 0x06006246 RID: 25158
		void UpdateProgress(long bytesDownloaded, long bytesTotal);
	}
}
