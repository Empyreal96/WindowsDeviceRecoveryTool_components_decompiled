using System;

namespace System.Windows.Markup
{
	// Token: 0x0200024F RID: 591
	internal class XamlUnknownTagStartNode : XamlAttributeNode
	{
		// Token: 0x060022EA RID: 8938 RVA: 0x000AC5A2 File Offset: 0x000AA7A2
		internal XamlUnknownTagStartNode(int lineNumber, int linePosition, int depth, string xmlNamespace, string value) : base(XamlNodeType.UnknownTagStart, lineNumber, linePosition, depth, value)
		{
			this._xmlNamespace = xmlNamespace;
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x000AC5B9 File Offset: 0x000AA7B9
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x04001A52 RID: 6738
		private string _xmlNamespace;
	}
}
