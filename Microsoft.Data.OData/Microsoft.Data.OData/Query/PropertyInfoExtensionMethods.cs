using System;
using System.Reflection;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000BE RID: 190
	internal static class PropertyInfoExtensionMethods
	{
		// Token: 0x060004A0 RID: 1184 RVA: 0x0000FCCC File Offset: 0x0000DECC
		internal static PropertyInfo GetPropertyInfo(this IEdmStructuredTypeReference typeReference, IEdmProperty property, IEdmModel model)
		{
			IEdmStructuredType structuredType = typeReference.StructuredDefinition();
			return PropertyInfoTypeAnnotation.GetPropertyInfoTypeAnnotation(structuredType, model).GetPropertyInfo(structuredType, property, model);
		}
	}
}
