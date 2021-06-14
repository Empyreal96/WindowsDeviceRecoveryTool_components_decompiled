using System;
using System.IO;
using System.Windows.Markup;

namespace MS.Internal.AppModel
{
	// Token: 0x0200079F RID: 1951
	// (Invoke) Token: 0x06007A62 RID: 31330
	internal delegate object StreamToObjectFactoryDelegate(Stream s, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter);
}
