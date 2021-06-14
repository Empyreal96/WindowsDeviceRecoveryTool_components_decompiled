using System;
using System.Diagnostics.CodeAnalysis;

namespace SoftwareRepository.Streaming
{
	// Token: 0x0200000C RID: 12
	// (Invoke) Token: 0x06000032 RID: 50
	[SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
	public delegate void DownloadReadyEventHandler(object sender, DownloadReadyEventArgs e);
}
