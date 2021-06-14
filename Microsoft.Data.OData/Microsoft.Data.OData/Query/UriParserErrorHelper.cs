using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200008C RID: 140
	internal static class UriParserErrorHelper
	{
		// Token: 0x06000348 RID: 840 RVA: 0x0000B68C File Offset: 0x0000988C
		public static void ThrowIfTypesUnrelated(IEdmType type, IEdmType secondType, string segmentName)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmType>(type, "type");
			ExceptionUtils.CheckArgumentNotNull<IEdmType>(secondType, "secondType");
			IEdmType edmType = type;
			IEdmCollectionType edmCollectionType = type as IEdmCollectionType;
			if (edmCollectionType != null)
			{
				edmType = edmCollectionType.ElementType.Definition;
			}
			if (!edmType.IsOrInheritsFrom(secondType) && !secondType.IsOrInheritsFrom(edmType))
			{
				throw new ODataException(Strings.PathParser_TypeMustBeRelatedToSet(type, secondType, segmentName));
			}
		}
	}
}
