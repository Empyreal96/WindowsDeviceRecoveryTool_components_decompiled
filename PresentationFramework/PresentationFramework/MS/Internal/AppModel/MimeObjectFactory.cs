using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A0 RID: 1952
	internal static class MimeObjectFactory
	{
		// Token: 0x06007A65 RID: 31333 RVA: 0x0022AF0C File Offset: 0x0022910C
		internal static object GetObjectAndCloseStream(Stream s, ContentType contentType, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			object result = null;
			asyncObjectConverter = null;
			StreamToObjectFactoryDelegate streamToObjectFactoryDelegate;
			if (contentType != null && MimeObjectFactory._objectConverters.TryGetValue(contentType, out streamToObjectFactoryDelegate))
			{
				result = streamToObjectFactoryDelegate(s, baseUri, canUseTopLevelBrowser, sandboxExternalContent, allowAsync, isJournalNavigation, out asyncObjectConverter);
			}
			return result;
		}

		// Token: 0x06007A66 RID: 31334 RVA: 0x0022AF44 File Offset: 0x00229144
		internal static void Register(ContentType contentType, StreamToObjectFactoryDelegate method)
		{
			MimeObjectFactory._objectConverters[contentType] = method;
		}

		// Token: 0x040039E0 RID: 14816
		private static readonly Dictionary<ContentType, StreamToObjectFactoryDelegate> _objectConverters = new Dictionary<ContentType, StreamToObjectFactoryDelegate>(5, new ContentType.WeakComparer());
	}
}
