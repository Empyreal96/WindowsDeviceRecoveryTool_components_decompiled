using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x02000242 RID: 578
	[DebuggerDisplay("Prop:{_typeFullName}.{_propName}")]
	internal class XamlPropertyBaseNode : XamlNode
	{
		// Token: 0x060022A7 RID: 8871 RVA: 0x000AC0C8 File Offset: 0x000AA2C8
		internal XamlPropertyBaseNode(XamlNodeType token, int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(token, lineNumber, linePosition, depth)
		{
			if (typeFullName == null)
			{
				throw new ArgumentNullException("typeFullName");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			this._propertyMember = propertyMember;
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._propName = propertyName;
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x000AC11E File Offset: 0x000AA31E
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x000AC126 File Offset: 0x000AA326
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060022AA RID: 8874 RVA: 0x000AC12E File Offset: 0x000AA32E
		internal string PropName
		{
			get
			{
				return this._propName;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x000AC136 File Offset: 0x000AA336
		internal Type PropDeclaringType
		{
			get
			{
				if (this._declaringType == null && this._propertyMember != null)
				{
					this._declaringType = XamlTypeMapper.GetDeclaringType(this._propertyMember);
				}
				return this._declaringType;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x000AC165 File Offset: 0x000AA365
		internal Type PropValidType
		{
			get
			{
				if (this._validType == null)
				{
					this._validType = XamlTypeMapper.GetPropertyType(this._propertyMember);
				}
				return this._validType;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x000AC18C File Offset: 0x000AA38C
		internal object PropertyMember
		{
			get
			{
				return this._propertyMember;
			}
		}

		// Token: 0x04001A29 RID: 6697
		private object _propertyMember;

		// Token: 0x04001A2A RID: 6698
		private string _assemblyName;

		// Token: 0x04001A2B RID: 6699
		private string _typeFullName;

		// Token: 0x04001A2C RID: 6700
		private string _propName;

		// Token: 0x04001A2D RID: 6701
		private Type _validType;

		// Token: 0x04001A2E RID: 6702
		private Type _declaringType;
	}
}
