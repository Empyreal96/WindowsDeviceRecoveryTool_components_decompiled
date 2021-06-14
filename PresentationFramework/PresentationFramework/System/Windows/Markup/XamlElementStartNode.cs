using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x02000249 RID: 585
	[DebuggerDisplay("Elem:{_typeFullName}")]
	internal class XamlElementStartNode : XamlNode
	{
		// Token: 0x060022D7 RID: 8919 RVA: 0x000AC464 File Offset: 0x000AA664
		internal XamlElementStartNode(int lineNumber, int linePosition, int depth, string assemblyName, string typeFullName, Type elementType, Type serializerType) : this(XamlNodeType.ElementStart, lineNumber, linePosition, depth, assemblyName, typeFullName, elementType, serializerType, false, false, false)
		{
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000AC488 File Offset: 0x000AA688
		internal XamlElementStartNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth, string assemblyName, string typeFullName, Type elementType, Type serializerType, bool isEmptyElement, bool needsDictionaryKey, bool isInjected) : base(tokenType, lineNumber, linePosition, depth)
		{
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._elementType = elementType;
			this._serializerType = serializerType;
			this._isEmptyElement = isEmptyElement;
			this._needsDictionaryKey = needsDictionaryKey;
			this._useTypeConverter = false;
			this.IsInjected = isInjected;
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x000AC4DF File Offset: 0x000AA6DF
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x000AC4E7 File Offset: 0x000AA6E7
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x000AC4EF File Offset: 0x000AA6EF
		internal Type ElementType
		{
			get
			{
				return this._elementType;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x000AC4F7 File Offset: 0x000AA6F7
		internal Type SerializerType
		{
			get
			{
				return this._serializerType;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x000AC4FF File Offset: 0x000AA6FF
		internal string SerializerTypeFullName
		{
			get
			{
				if (!(this._serializerType == null))
				{
					return this._serializerType.FullName;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x000AC520 File Offset: 0x000AA720
		// (set) Token: 0x060022DF RID: 8927 RVA: 0x000AC528 File Offset: 0x000AA728
		internal bool CreateUsingTypeConverter
		{
			get
			{
				return this._useTypeConverter;
			}
			set
			{
				this._useTypeConverter = value;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x060022E0 RID: 8928 RVA: 0x000AC531 File Offset: 0x000AA731
		// (set) Token: 0x060022E1 RID: 8929 RVA: 0x000AC539 File Offset: 0x000AA739
		internal bool IsInjected
		{
			get
			{
				return this._isInjected;
			}
			set
			{
				this._isInjected = value;
			}
		}

		// Token: 0x04001A48 RID: 6728
		private string _assemblyName;

		// Token: 0x04001A49 RID: 6729
		private string _typeFullName;

		// Token: 0x04001A4A RID: 6730
		private Type _elementType;

		// Token: 0x04001A4B RID: 6731
		private Type _serializerType;

		// Token: 0x04001A4C RID: 6732
		private bool _isEmptyElement;

		// Token: 0x04001A4D RID: 6733
		private bool _needsDictionaryKey;

		// Token: 0x04001A4E RID: 6734
		private bool _useTypeConverter;

		// Token: 0x04001A4F RID: 6735
		private bool _isInjected;
	}
}
