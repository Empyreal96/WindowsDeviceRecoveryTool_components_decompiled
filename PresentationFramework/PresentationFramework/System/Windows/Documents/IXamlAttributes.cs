using System;

namespace System.Windows.Documents
{
	// Token: 0x0200038E RID: 910
	internal interface IXamlAttributes
	{
		// Token: 0x06003189 RID: 12681
		XamlToRtfError GetLength(ref int length);

		// Token: 0x0600318A RID: 12682
		XamlToRtfError GetUri(int index, ref string uri);

		// Token: 0x0600318B RID: 12683
		XamlToRtfError GetLocalName(int index, ref string localName);

		// Token: 0x0600318C RID: 12684
		XamlToRtfError GetQName(int index, ref string qName);

		// Token: 0x0600318D RID: 12685
		XamlToRtfError GetName(int index, ref string uri, ref string localName, ref string qName);

		// Token: 0x0600318E RID: 12686
		XamlToRtfError GetIndexFromName(string uri, string localName, ref int index);

		// Token: 0x0600318F RID: 12687
		XamlToRtfError GetIndexFromQName(string qName, ref int index);

		// Token: 0x06003190 RID: 12688
		XamlToRtfError GetType(int index, ref string type);

		// Token: 0x06003191 RID: 12689
		XamlToRtfError GetTypeFromName(string uri, string localName, ref string type);

		// Token: 0x06003192 RID: 12690
		XamlToRtfError GetTypeFromQName(string qName, ref string type);

		// Token: 0x06003193 RID: 12691
		XamlToRtfError GetValue(int index, ref string value);

		// Token: 0x06003194 RID: 12692
		XamlToRtfError GetValueFromName(string uri, string localName, ref string value);

		// Token: 0x06003195 RID: 12693
		XamlToRtfError GetValueFromQName(string qName, ref string value);
	}
}
