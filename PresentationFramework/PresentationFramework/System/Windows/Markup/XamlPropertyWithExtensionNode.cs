using System;

namespace System.Windows.Markup
{
	// Token: 0x02000245 RID: 581
	internal class XamlPropertyWithExtensionNode : XamlPropertyBaseNode
	{
		// Token: 0x060022B2 RID: 8882 RVA: 0x000AC1F0 File Offset: 0x000AA3F0
		internal XamlPropertyWithExtensionNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string value, short extensionTypeId, bool isValueNestedExtension, bool isValueTypeExtension) : base(XamlNodeType.PropertyWithExtension, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
			this._value = value;
			this._extensionTypeId = extensionTypeId;
			this._isValueNestedExtension = isValueNestedExtension;
			this._isValueTypeExtension = isValueTypeExtension;
			this._defaultTargetType = null;
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x060022B3 RID: 8883 RVA: 0x000AC237 File Offset: 0x000AA437
		internal short ExtensionTypeId
		{
			get
			{
				return this._extensionTypeId;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000AC23F File Offset: 0x000AA43F
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000AC247 File Offset: 0x000AA447
		internal bool IsValueNestedExtension
		{
			get
			{
				return this._isValueNestedExtension;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x000AC24F File Offset: 0x000AA44F
		internal bool IsValueTypeExtension
		{
			get
			{
				return this._isValueTypeExtension;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000AC257 File Offset: 0x000AA457
		// (set) Token: 0x060022B8 RID: 8888 RVA: 0x000AC25F File Offset: 0x000AA45F
		internal Type DefaultTargetType
		{
			get
			{
				return this._defaultTargetType;
			}
			set
			{
				this._defaultTargetType = value;
			}
		}

		// Token: 0x04001A2F RID: 6703
		private short _extensionTypeId;

		// Token: 0x04001A30 RID: 6704
		private string _value;

		// Token: 0x04001A31 RID: 6705
		private bool _isValueNestedExtension;

		// Token: 0x04001A32 RID: 6706
		private bool _isValueTypeExtension;

		// Token: 0x04001A33 RID: 6707
		private Type _defaultTargetType;
	}
}
