using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x020001E5 RID: 485
	internal sealed class ODataBatchUrlResolver : IODataUrlResolver
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x000351E8 File Offset: 0x000333E8
		internal ODataBatchUrlResolver(IODataUrlResolver batchMessageUrlResolver)
		{
			this.batchMessageUrlResolver = batchMessageUrlResolver;
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x000351F7 File Offset: 0x000333F7
		internal IODataUrlResolver BatchMessageUrlResolver
		{
			get
			{
				return this.batchMessageUrlResolver;
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00035200 File Offset: 0x00033400
		Uri IODataUrlResolver.ResolveUrl(Uri baseUri, Uri payloadUri)
		{
			ExceptionUtils.CheckArgumentNotNull<Uri>(payloadUri, "payloadUri");
			if (this.contentIdCache != null && !payloadUri.IsAbsoluteUri)
			{
				string text = UriUtilsCommon.UriToString(payloadUri);
				if (text.Length > 0 && text[0] == '$')
				{
					int num = text.IndexOf('/', 1);
					string item;
					if (num > 0)
					{
						item = text.Substring(1, num - 1);
					}
					else
					{
						item = text.Substring(1);
					}
					if (this.contentIdCache.Contains(item))
					{
						return payloadUri;
					}
				}
			}
			if (this.batchMessageUrlResolver != null)
			{
				return this.batchMessageUrlResolver.ResolveUrl(baseUri, payloadUri);
			}
			return null;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0003528D File Offset: 0x0003348D
		internal void AddContentId(string contentId)
		{
			if (this.contentIdCache == null)
			{
				this.contentIdCache = new HashSet<string>(StringComparer.Ordinal);
			}
			this.contentIdCache.Add(contentId);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x000352B4 File Offset: 0x000334B4
		internal bool ContainsContentId(string contentId)
		{
			return this.contentIdCache != null && this.contentIdCache.Contains(contentId);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x000352CC File Offset: 0x000334CC
		internal void Reset()
		{
			if (this.contentIdCache != null)
			{
				this.contentIdCache.Clear();
			}
		}

		// Token: 0x04000530 RID: 1328
		private readonly IODataUrlResolver batchMessageUrlResolver;

		// Token: 0x04000531 RID: 1329
		private HashSet<string> contentIdCache;
	}
}
