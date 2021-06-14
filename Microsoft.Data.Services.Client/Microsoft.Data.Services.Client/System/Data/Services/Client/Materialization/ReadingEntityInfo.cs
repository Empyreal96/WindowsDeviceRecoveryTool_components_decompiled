using System;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000074 RID: 116
	internal sealed class ReadingEntityInfo
	{
		// Token: 0x060003CC RID: 972 RVA: 0x000103AB File Offset: 0x0000E5AB
		internal ReadingEntityInfo(XElement payload, Uri uri)
		{
			this.EntryPayload = payload;
			this.BaseUri = uri;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000103C4 File Offset: 0x0000E5C4
		internal static XmlReader BufferAndCacheEntryPayload(ODataEntry entry, XmlReader entryReader, Uri baseUri)
		{
			XElement xelement = XElement.Load(entryReader.ReadSubtree(), LoadOptions.None);
			entryReader.Read();
			entry.SetAnnotation<ReadingEntityInfo>(new ReadingEntityInfo(xelement, baseUri));
			XmlReader xmlReader = xelement.CreateReader();
			xmlReader.Read();
			return xmlReader;
		}

		// Token: 0x040002B7 RID: 695
		internal readonly XElement EntryPayload;

		// Token: 0x040002B8 RID: 696
		internal readonly Uri BaseUri;
	}
}
