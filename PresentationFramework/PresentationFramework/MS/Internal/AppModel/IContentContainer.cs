using System;

namespace MS.Internal.AppModel
{
	// Token: 0x0200077C RID: 1916
	internal interface IContentContainer
	{
		// Token: 0x0600791A RID: 31002
		void OnContentReady(ContentType contentType, object content, Uri uri, object navState);

		// Token: 0x0600791B RID: 31003
		void OnNavigationProgress(Uri uri, long bytesRead, long maxBytes);

		// Token: 0x0600791C RID: 31004
		void OnStreamClosed(Uri uri);
	}
}
