using System;

namespace System.Windows.Markup
{
	// Token: 0x0200025E RID: 606
	internal class XamlDefAttributeKeyTypeNode : XamlAttributeNode
	{
		// Token: 0x06002307 RID: 8967 RVA: 0x000AC7A8 File Offset: 0x000AA9A8
		internal XamlDefAttributeKeyTypeNode(int lineNumber, int linePosition, int depth, string value, string assemblyName, Type valueType) : base(XamlNodeType.DefKeyTypeAttribute, lineNumber, linePosition, depth, value)
		{
			this._assemblyName = assemblyName;
			this._valueType = valueType;
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x000AC7C7 File Offset: 0x000AA9C7
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002309 RID: 8969 RVA: 0x000AC7CF File Offset: 0x000AA9CF
		internal Type ValueType
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x04001A61 RID: 6753
		private string _assemblyName;

		// Token: 0x04001A62 RID: 6754
		private Type _valueType;
	}
}
