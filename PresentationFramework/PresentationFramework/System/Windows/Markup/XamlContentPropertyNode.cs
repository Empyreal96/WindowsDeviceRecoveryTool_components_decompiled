using System;

namespace System.Windows.Markup
{
	// Token: 0x02000262 RID: 610
	internal class XamlContentPropertyNode : XamlNode
	{
		// Token: 0x0600230E RID: 8974 RVA: 0x000AC828 File Offset: 0x000AAA28
		internal XamlContentPropertyNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.ContentProperty, lineNumber, linePosition, depth)
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

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x000AC87E File Offset: 0x000AAA7E
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002310 RID: 8976 RVA: 0x000AC886 File Offset: 0x000AAA86
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x000AC88E File Offset: 0x000AAA8E
		internal string PropName
		{
			get
			{
				return this._propName;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000AC896 File Offset: 0x000AAA96
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

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x000AC8C5 File Offset: 0x000AAAC5
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

		// Token: 0x04001A64 RID: 6756
		private Type _declaringType;

		// Token: 0x04001A65 RID: 6757
		private Type _validType;

		// Token: 0x04001A66 RID: 6758
		private object _propertyMember;

		// Token: 0x04001A67 RID: 6759
		private string _assemblyName;

		// Token: 0x04001A68 RID: 6760
		private string _typeFullName;

		// Token: 0x04001A69 RID: 6761
		private string _propName;
	}
}
