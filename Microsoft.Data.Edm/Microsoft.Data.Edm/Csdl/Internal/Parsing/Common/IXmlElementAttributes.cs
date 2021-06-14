using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x02000153 RID: 339
	internal interface IXmlElementAttributes
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000697 RID: 1687
		IEnumerable<XmlAttributeInfo> Unused { get; }

		// Token: 0x170002BB RID: 699
		XmlAttributeInfo this[string attributeName]
		{
			get;
		}
	}
}
