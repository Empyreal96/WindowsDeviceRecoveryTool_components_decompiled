using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x0200024E RID: 590
	[DebuggerDisplay("Attr:{_value}")]
	internal class XamlAttributeNode : XamlNode
	{
		// Token: 0x060022E8 RID: 8936 RVA: 0x000AC585 File Offset: 0x000AA785
		internal XamlAttributeNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth, string value) : base(tokenType, lineNumber, linePosition, depth)
		{
			this._value = value;
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x000AC59A File Offset: 0x000AA79A
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x04001A51 RID: 6737
		private string _value;
	}
}
