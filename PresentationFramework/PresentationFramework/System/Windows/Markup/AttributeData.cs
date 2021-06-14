using System;

namespace System.Windows.Markup
{
	// Token: 0x020001C5 RID: 453
	internal class AttributeData : DefAttributeData
	{
		// Token: 0x06001D0C RID: 7436 RVA: 0x00087A20 File Offset: 0x00085C20
		internal AttributeData(string targetAssemblyName, string targetFullName, Type targetType, string args, Type declaringType, string propertyName, object info, Type serializerType, int lineNumber, int linePosition, int depth, string targetNamespaceUri, short extensionTypeId, bool isValueNestedExtension, bool isValueTypeExtension, bool isSimple) : base(targetAssemblyName, targetFullName, targetType, args, declaringType, targetNamespaceUri, lineNumber, linePosition, depth, isSimple)
		{
			this.PropertyName = propertyName;
			this.SerializerType = serializerType;
			this.ExtensionTypeId = extensionTypeId;
			this.IsValueNestedExtension = isValueNestedExtension;
			this.IsValueTypeExtension = isValueTypeExtension;
			this.Info = info;
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x00087A74 File Offset: 0x00085C74
		internal bool IsTypeExtension
		{
			get
			{
				return this.ExtensionTypeId == 691;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x00087A83 File Offset: 0x00085C83
		internal bool IsStaticExtension
		{
			get
			{
				return this.ExtensionTypeId == 602;
			}
		}

		// Token: 0x04001406 RID: 5126
		internal string PropertyName;

		// Token: 0x04001407 RID: 5127
		internal Type SerializerType;

		// Token: 0x04001408 RID: 5128
		internal short ExtensionTypeId;

		// Token: 0x04001409 RID: 5129
		internal bool IsValueNestedExtension;

		// Token: 0x0400140A RID: 5130
		internal bool IsValueTypeExtension;

		// Token: 0x0400140B RID: 5131
		internal object Info;
	}
}
