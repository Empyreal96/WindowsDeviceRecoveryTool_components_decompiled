using System;

namespace System.Windows.Markup
{
	// Token: 0x02000221 RID: 545
	internal interface IParserHelper
	{
		// Token: 0x060021AA RID: 8618
		string LookupNamespace(string prefix);

		// Token: 0x060021AB RID: 8619
		bool GetElementType(bool extensionFirst, string localName, string namespaceURI, ref string assemblyName, ref string typeFullName, ref Type baseType, ref Type serializerType);

		// Token: 0x060021AC RID: 8620
		bool CanResolveLocalAssemblies();
	}
}
