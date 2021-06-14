using System;

namespace System.Windows.Markup
{
	// Token: 0x02000246 RID: 582
	internal class XamlPropertyNode : XamlPropertyBaseNode
	{
		// Token: 0x060022B9 RID: 8889 RVA: 0x000AC268 File Offset: 0x000AA468
		internal XamlPropertyNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string value, BamlAttributeUsage attributeUsage, bool complexAsSimple) : base(XamlNodeType.Property, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
			this._value = value;
			this._attributeUsage = attributeUsage;
			this._complexAsSimple = complexAsSimple;
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000AC2A0 File Offset: 0x000AA4A0
		internal XamlPropertyNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string value, BamlAttributeUsage attributeUsage, bool complexAsSimple, bool isDefinitionName) : this(lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName, value, attributeUsage, complexAsSimple)
		{
			this._isDefinitionName = isDefinitionName;
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x000AC2CC File Offset: 0x000AA4CC
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000AC2D4 File Offset: 0x000AA4D4
		internal void SetValue(string value)
		{
			this._value = value;
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x000AC2DD File Offset: 0x000AA4DD
		// (set) Token: 0x060022BE RID: 8894 RVA: 0x000AC2FA File Offset: 0x000AA4FA
		internal Type ValueDeclaringType
		{
			get
			{
				if (this._valueDeclaringType == null)
				{
					return base.PropDeclaringType;
				}
				return this._valueDeclaringType;
			}
			set
			{
				this._valueDeclaringType = value;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x000AC303 File Offset: 0x000AA503
		// (set) Token: 0x060022C0 RID: 8896 RVA: 0x000AC31A File Offset: 0x000AA51A
		internal string ValuePropertyName
		{
			get
			{
				if (this._valuePropertyName == null)
				{
					return base.PropName;
				}
				return this._valuePropertyName;
			}
			set
			{
				this._valuePropertyName = value;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x000AC323 File Offset: 0x000AA523
		// (set) Token: 0x060022C2 RID: 8898 RVA: 0x000AC340 File Offset: 0x000AA540
		internal Type ValuePropertyType
		{
			get
			{
				if (this._valuePropertyType == null)
				{
					return base.PropValidType;
				}
				return this._valuePropertyType;
			}
			set
			{
				this._valuePropertyType = value;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060022C3 RID: 8899 RVA: 0x000AC349 File Offset: 0x000AA549
		// (set) Token: 0x060022C4 RID: 8900 RVA: 0x000AC360 File Offset: 0x000AA560
		internal object ValuePropertyMember
		{
			get
			{
				if (this._valuePropertyMember == null)
				{
					return base.PropertyMember;
				}
				return this._valuePropertyMember;
			}
			set
			{
				this._valuePropertyMember = value;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x000AC369 File Offset: 0x000AA569
		internal bool HasValueId
		{
			get
			{
				return this._hasValueId;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x000AC371 File Offset: 0x000AA571
		// (set) Token: 0x060022C7 RID: 8903 RVA: 0x000AC379 File Offset: 0x000AA579
		internal short ValueId
		{
			get
			{
				return this._valueId;
			}
			set
			{
				this._valueId = value;
				this._hasValueId = true;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x000AC389 File Offset: 0x000AA589
		// (set) Token: 0x060022C9 RID: 8905 RVA: 0x000AC391 File Offset: 0x000AA591
		internal string MemberName
		{
			get
			{
				return this._memberName;
			}
			set
			{
				this._memberName = value;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x000AC39A File Offset: 0x000AA59A
		// (set) Token: 0x060022CB RID: 8907 RVA: 0x000AC3A2 File Offset: 0x000AA5A2
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

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060022CC RID: 8908 RVA: 0x000AC3AB File Offset: 0x000AA5AB
		internal BamlAttributeUsage AttributeUsage
		{
			get
			{
				return this._attributeUsage;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x000AC3B3 File Offset: 0x000AA5B3
		internal bool ComplexAsSimple
		{
			get
			{
				return this._complexAsSimple;
			}
		}

		// Token: 0x04001A34 RID: 6708
		private string _value;

		// Token: 0x04001A35 RID: 6709
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x04001A36 RID: 6710
		private bool _complexAsSimple;

		// Token: 0x04001A37 RID: 6711
		private bool _isDefinitionName;

		// Token: 0x04001A38 RID: 6712
		private Type _valueDeclaringType;

		// Token: 0x04001A39 RID: 6713
		private string _valuePropertyName;

		// Token: 0x04001A3A RID: 6714
		private Type _valuePropertyType;

		// Token: 0x04001A3B RID: 6715
		private object _valuePropertyMember;

		// Token: 0x04001A3C RID: 6716
		private bool _hasValueId;

		// Token: 0x04001A3D RID: 6717
		private short _valueId;

		// Token: 0x04001A3E RID: 6718
		private string _memberName;

		// Token: 0x04001A3F RID: 6719
		private Type _defaultTargetType;
	}
}
