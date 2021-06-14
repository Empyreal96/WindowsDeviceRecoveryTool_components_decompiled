using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200077F RID: 1919
	internal interface IDownloader
	{
		// Token: 0x17001C9E RID: 7326
		// (get) Token: 0x0600791F RID: 31007
		NavigationService Downloader { get; }

		// Token: 0x1400015D RID: 349
		// (add) Token: 0x06007920 RID: 31008
		// (remove) Token: 0x06007921 RID: 31009
		event EventHandler ContentRendered;
	}
}
