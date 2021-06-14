using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200002B RID: 43
	internal interface IXmlElement : IXmlNode
	{
		// Token: 0x060001C8 RID: 456
		void SetAttributeNode(IXmlNode attribute);

		// Token: 0x060001C9 RID: 457
		string GetPrefixOfNamespace(string namespaceUri);

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001CA RID: 458
		bool IsEmpty { get; }
	}
}
