using System;

namespace System.Windows.Markup
{
	// Token: 0x02000250 RID: 592
	internal class XamlUnknownTagEndNode : XamlNode
	{
		// Token: 0x060022EC RID: 8940 RVA: 0x000AC5C1 File Offset: 0x000AA7C1
		internal XamlUnknownTagEndNode(int lineNumber, int linePosition, int depth, string localName, string xmlNamespace) : base(XamlNodeType.UnknownTagEnd, lineNumber, linePosition, depth)
		{
			this._localName = localName;
			this._xmlNamespace = xmlNamespace;
		}

		// Token: 0x04001A53 RID: 6739
		private string _localName;

		// Token: 0x04001A54 RID: 6740
		private string _xmlNamespace;
	}
}
