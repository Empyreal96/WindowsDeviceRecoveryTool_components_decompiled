using System;

namespace System.Windows.Markup
{
	// Token: 0x02000247 RID: 583
	internal class XamlPropertyWithTypeNode : XamlPropertyBaseNode
	{
		// Token: 0x060022CE RID: 8910 RVA: 0x000AC3BC File Offset: 0x000AA5BC
		internal XamlPropertyWithTypeNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string valueTypeFullName, string valueAssemblyName, Type valueElementType, string valueSerializerTypeFullName, string valueSerializerTypeAssemblyName) : base(XamlNodeType.PropertyWithType, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
			this._valueTypeFullname = valueTypeFullName;
			this._valueTypeAssemblyName = valueAssemblyName;
			this._valueElementType = valueElementType;
			this._valueSerializerTypeFullName = valueSerializerTypeFullName;
			this._valueSerializerTypeAssemblyName = valueSerializerTypeAssemblyName;
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x000AC404 File Offset: 0x000AA604
		internal string ValueTypeFullName
		{
			get
			{
				return this._valueTypeFullname;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x000AC40C File Offset: 0x000AA60C
		internal string ValueTypeAssemblyName
		{
			get
			{
				return this._valueTypeAssemblyName;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x000AC414 File Offset: 0x000AA614
		internal Type ValueElementType
		{
			get
			{
				return this._valueElementType;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x000AC41C File Offset: 0x000AA61C
		internal string ValueSerializerTypeFullName
		{
			get
			{
				return this._valueSerializerTypeFullName;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x000AC424 File Offset: 0x000AA624
		internal string ValueSerializerTypeAssemblyName
		{
			get
			{
				return this._valueSerializerTypeAssemblyName;
			}
		}

		// Token: 0x04001A40 RID: 6720
		private string _valueTypeFullname;

		// Token: 0x04001A41 RID: 6721
		private string _valueTypeAssemblyName;

		// Token: 0x04001A42 RID: 6722
		private Type _valueElementType;

		// Token: 0x04001A43 RID: 6723
		private string _valueSerializerTypeFullName;

		// Token: 0x04001A44 RID: 6724
		private string _valueSerializerTypeAssemblyName;
	}
}
