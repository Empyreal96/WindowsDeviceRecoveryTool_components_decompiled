using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002F RID: 47
	internal interface IXmlDocumentType : IXmlNode
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001DA RID: 474
		string Name { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001DB RID: 475
		string System { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001DC RID: 476
		string Public { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001DD RID: 477
		string InternalSubset { get; }
	}
}
