using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x02000252 RID: 594
	[DebuggerDisplay("Xmlns:{_prefix)={_xmlNamespace}")]
	internal class XamlXmlnsPropertyNode : XamlNode
	{
		// Token: 0x060022F2 RID: 8946 RVA: 0x000AC62D File Offset: 0x000AA82D
		internal XamlXmlnsPropertyNode(int lineNumber, int linePosition, int depth, string prefix, string xmlNamespace) : base(XamlNodeType.XmlnsProperty, lineNumber, linePosition, depth)
		{
			this._prefix = prefix;
			this._xmlNamespace = xmlNamespace;
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x000AC64A File Offset: 0x000AA84A
		internal string Prefix
		{
			get
			{
				return this._prefix;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x000AC652 File Offset: 0x000AA852
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x04001A59 RID: 6745
		private string _prefix;

		// Token: 0x04001A5A RID: 6746
		private string _xmlNamespace;
	}
}
