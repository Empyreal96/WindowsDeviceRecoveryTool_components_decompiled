using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000053 RID: 83
	internal static class PathParserModelUtils
	{
		// Token: 0x06000227 RID: 551 RVA: 0x00008348 File Offset: 0x00006548
		internal static bool IsOpenType(this IEdmType edmType)
		{
			IEdmStructuredType edmStructuredType = edmType as IEdmStructuredType;
			if (edmStructuredType != null)
			{
				PathParserModelUtils.ThrowIfOpenComplexType(edmType);
				return edmStructuredType.IsOpen;
			}
			IEdmCollectionType edmCollectionType = edmType as IEdmCollectionType;
			return edmCollectionType != null && edmCollectionType.ElementType.Definition.IsOpenType();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008388 File Offset: 0x00006588
		internal static bool IsEntityOrEntityCollectionType(this IEdmType edmType)
		{
			IEdmEntityType edmEntityType;
			return edmType.IsEntityOrEntityCollectionType(out edmEntityType);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000083A0 File Offset: 0x000065A0
		internal static bool IsEntityOrEntityCollectionType(this IEdmType edmType, out IEdmEntityType entityType)
		{
			if (edmType.TypeKind == EdmTypeKind.Entity)
			{
				entityType = (IEdmEntityType)edmType;
				return true;
			}
			if (edmType.TypeKind != EdmTypeKind.Collection)
			{
				entityType = null;
				return false;
			}
			entityType = (((IEdmCollectionType)edmType).ElementType.Definition as IEdmEntityType);
			return entityType != null;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000083F0 File Offset: 0x000065F0
		internal static IEdmEntitySet GetTargetEntitySet(this IEdmFunctionImport functionImport, IEdmEntitySet sourceEntitySet, IEdmModel model)
		{
			IEdmEntitySet edmEntitySet;
			if (functionImport.TryGetStaticEntitySet(out edmEntitySet))
			{
				return edmEntitySet;
			}
			if (sourceEntitySet == null)
			{
				return null;
			}
			IEdmFunctionParameter edmFunctionParameter;
			IEnumerable<IEdmNavigationProperty> enumerable;
			if (functionImport.IsBindable && functionImport.Parameters.Any<IEdmFunctionParameter>() && functionImport.TryGetRelativeEntitySetPath(model, out edmFunctionParameter, out enumerable))
			{
				ExceptionUtil.ThrowSyntaxErrorIfNotValid(edmFunctionParameter == functionImport.Parameters.First<IEdmFunctionParameter>());
				edmEntitySet = sourceEntitySet;
				foreach (IEdmNavigationProperty navigationProperty in enumerable)
				{
					edmEntitySet = edmEntitySet.FindNavigationTarget(navigationProperty);
					if (edmEntitySet == null)
					{
						return null;
					}
				}
				return edmEntitySet;
			}
			return null;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00008494 File Offset: 0x00006694
		private static void ThrowIfOpenComplexType(IEdmType edmType)
		{
			if (edmType.TypeKind == EdmTypeKind.Complex)
			{
				IEdmComplexType edmComplexType = (IEdmComplexType)edmType;
				if (edmComplexType.IsOpen)
				{
					throw new InvalidOperationException(Strings.ResourceType_ComplexTypeCannotBeOpen(edmComplexType.FullName()));
				}
			}
		}
	}
}
