using System;

namespace System.Windows.Documents
{
	// Token: 0x0200038F RID: 911
	internal interface IXamlContentHandler
	{
		// Token: 0x06003196 RID: 12694
		XamlToRtfError StartDocument();

		// Token: 0x06003197 RID: 12695
		XamlToRtfError EndDocument();

		// Token: 0x06003198 RID: 12696
		XamlToRtfError StartPrefixMapping(string prefix, string uri);

		// Token: 0x06003199 RID: 12697
		XamlToRtfError StartElement(string nameSpaceUri, string localName, string qName, IXamlAttributes attributes);

		// Token: 0x0600319A RID: 12698
		XamlToRtfError EndElement(string nameSpaceUri, string localName, string qName);

		// Token: 0x0600319B RID: 12699
		XamlToRtfError Characters(string characters);

		// Token: 0x0600319C RID: 12700
		XamlToRtfError IgnorableWhitespace(string characters);

		// Token: 0x0600319D RID: 12701
		XamlToRtfError ProcessingInstruction(string target, string data);

		// Token: 0x0600319E RID: 12702
		XamlToRtfError SkippedEntity(string name);
	}
}
