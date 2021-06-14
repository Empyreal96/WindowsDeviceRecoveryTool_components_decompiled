using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query.Metadata
{
	// Token: 0x0200003F RID: 63
	internal static class UriEdmHelpers
	{
		// Token: 0x0600018D RID: 397 RVA: 0x00006F18 File Offset: 0x00005118
		public static IEdmSchemaType FindTypeFromModel(IEdmModel model, string qualifiedName)
		{
			return model.FindType(qualifiedName);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006F24 File Offset: 0x00005124
		public static IEdmTypeReference FindCollectionTypeFromModel(IEdmModel model, string qualifiedName)
		{
			if (qualifiedName.StartsWith("Collection", StringComparison.Ordinal))
			{
				string[] array = qualifiedName.Split(new char[]
				{
					'('
				});
				string qualifiedName2 = array[1].Split(new char[]
				{
					')'
				})[0];
				return EdmCoreModel.GetCollection(UriEdmHelpers.FindTypeFromModel(model, qualifiedName2).ToTypeReference());
			}
			return null;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006F7E File Offset: 0x0000517E
		public static IEdmTypeReference GetFunctionReturnType(IEdmFunctionImport serviceOperation)
		{
			return serviceOperation.ReturnType;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006F86 File Offset: 0x00005186
		public static IEdmEntityType GetEntitySetElementType(IEdmEntitySet entitySet)
		{
			return entitySet.ElementType;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006F8E File Offset: 0x0000518E
		public static IEdmTypeReference GetOperationParameterType(IEdmFunctionParameter serviceOperationParameter)
		{
			return serviceOperationParameter.Type;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006F98 File Offset: 0x00005198
		public static void CheckRelatedTo(IEdmType parentType, IEdmType childType)
		{
			IEdmEntityType edmEntityType = childType as IEdmEntityType;
			if (!edmEntityType.IsOrInheritsFrom(parentType) && !parentType.IsOrInheritsFrom(edmEntityType))
			{
				string p = (parentType != null) ? parentType.ODataFullName() : "<null>";
				throw new ODataException(Strings.MetadataBinder_HierarchyNotFollowed(edmEntityType.FullName(), p));
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006FE4 File Offset: 0x000051E4
		public static IEdmNavigationProperty GetNavigationPropertyFromExpandPath(ODataPath path)
		{
			NavigationPropertySegment navigationPropertySegment = null;
			foreach (ODataPathSegment odataPathSegment in path)
			{
				TypeSegment typeSegment = odataPathSegment as TypeSegment;
				navigationPropertySegment = (odataPathSegment as NavigationPropertySegment);
				if (typeSegment == null && navigationPropertySegment == null)
				{
					throw new ODataException(Strings.ExpandItemBinder_TypeSegmentNotFollowedByPath);
				}
			}
			if (navigationPropertySegment == null)
			{
				throw new ODataException(Strings.ExpandItemBinder_TypeSegmentNotFollowedByPath);
			}
			return navigationPropertySegment.NavigationProperty;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000705C File Offset: 0x0000525C
		public static IEdmType GetMostDerivedTypeFromPath(ODataPath path, IEdmType startingType)
		{
			IEdmType edmType = startingType;
			foreach (ODataPathSegment odataPathSegment in path)
			{
				TypeSegment typeSegment = odataPathSegment as TypeSegment;
				if (typeSegment != null && typeSegment.EdmType.IsOrInheritsFrom(edmType))
				{
					edmType = typeSegment.EdmType;
				}
			}
			return edmType;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000070C0 File Offset: 0x000052C0
		public static bool TryGetEntityContainer(string containerIdentifier, IEdmModel model, out IEdmEntityContainer entityContainer)
		{
			entityContainer = model.FindEntityContainer(containerIdentifier);
			return entityContainer != null;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000070D4 File Offset: 0x000052D4
		public static bool IsEntityCollection(this IEdmTypeReference type)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmTypeReference>(type, "type");
			IEdmCollectionTypeReference edmCollectionTypeReference = type as IEdmCollectionTypeReference;
			return edmCollectionTypeReference != null && edmCollectionTypeReference.ElementType().IsEntity();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007104 File Offset: 0x00005304
		public static bool AllHaveEqualReturnTypeAndAttributes(this IList<IEdmFunctionImport> functionImports)
		{
			if (!functionImports.Any<IEdmFunctionImport>())
			{
				return true;
			}
			IEdmType edmType = (functionImports[0].ReturnType == null) ? null : functionImports[0].ReturnType.Definition;
			bool isComposable = functionImports[0].IsComposable;
			bool isSideEffecting = functionImports[0].IsSideEffecting;
			foreach (IEdmFunctionImport edmFunctionImport in functionImports)
			{
				if (edmFunctionImport.IsComposable != isComposable)
				{
					return false;
				}
				if (edmFunctionImport.IsSideEffecting != isSideEffecting)
				{
					return false;
				}
				if (edmType != null)
				{
					if (edmFunctionImport.ReturnType.Definition.ODataFullName() != edmType.ODataFullName())
					{
						return false;
					}
				}
				else if (edmFunctionImport.ReturnType != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000071E4 File Offset: 0x000053E4
		public static bool IsBindingTypeValid(IEdmType bindingType)
		{
			return bindingType == null || bindingType.IsEntityOrEntityCollectionType() || bindingType.IsODataComplexTypeKind();
		}
	}
}
