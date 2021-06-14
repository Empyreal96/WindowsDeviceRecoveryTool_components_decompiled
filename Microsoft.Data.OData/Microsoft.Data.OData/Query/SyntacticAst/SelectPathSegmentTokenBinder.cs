using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x0200005D RID: 93
	internal static class SelectPathSegmentTokenBinder
	{
		// Token: 0x06000261 RID: 609 RVA: 0x00009A08 File Offset: 0x00007C08
		public static ODataPathSegment ConvertNonTypeTokenToSegment(PathSegmentToken tokenIn, IEdmModel model, IEdmEntityType entityType)
		{
			ODataPathSegment result;
			if (SelectPathSegmentTokenBinder.TryBindAsDeclaredProperty(tokenIn, entityType, out result))
			{
				return result;
			}
			if ((!entityType.IsOpen || tokenIn.IsNamespaceOrContainerQualified()) && SelectPathSegmentTokenBinder.TryBindAsOperation(tokenIn, model, entityType, out result))
			{
				return result;
			}
			if (entityType.IsOpen)
			{
				return new OpenPropertySegment(tokenIn.Identifier);
			}
			throw new ODataException(Strings.MetadataBinder_PropertyNotDeclared(entityType.FullName(), tokenIn.Identifier));
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009A6C File Offset: 0x00007C6C
		public static bool TryBindAsWildcard(PathSegmentToken tokenIn, IEdmModel model, out SelectItem item)
		{
			bool flag = tokenIn.IsNamespaceOrContainerQualified();
			bool flag2 = tokenIn.Identifier.EndsWith("*", StringComparison.Ordinal);
			IEdmEntityContainer container;
			if (flag && flag2 && UriEdmHelpers.TryGetEntityContainer(tokenIn.Identifier.Substring(0, tokenIn.Identifier.LastIndexOf('.')), model, out container))
			{
				item = new ContainerQualifiedWildcardSelectItem(container);
				return true;
			}
			if (tokenIn.Identifier == "*")
			{
				item = new WildcardSelectItem();
				return true;
			}
			item = null;
			return false;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009AE4 File Offset: 0x00007CE4
		private static bool TryBindAsOperation(PathSegmentToken pathToken, IEdmModel model, IEdmEntityType entityType, out ODataPathSegment segment)
		{
			IODataUriParserModelExtensions iodataUriParserModelExtensions = model as IODataUriParserModelExtensions;
			IEnumerable<IEdmFunctionImport> source;
			if (iodataUriParserModelExtensions != null)
			{
				source = iodataUriParserModelExtensions.FindFunctionImportsByBindingParameterTypeHierarchy(entityType, pathToken.Identifier);
			}
			else
			{
				source = model.FindFunctionImportsByBindingParameterTypeHierarchy(entityType, pathToken.Identifier);
			}
			List<IEdmFunctionImport> list = source.ToList<IEdmFunctionImport>();
			if (list.Count <= 0)
			{
				segment = null;
				return false;
			}
			segment = new OperationSegment(list, null);
			return true;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009B38 File Offset: 0x00007D38
		private static bool TryBindAsDeclaredProperty(PathSegmentToken tokenIn, IEdmEntityType entityType, out ODataPathSegment segment)
		{
			IEdmProperty edmProperty = entityType.FindProperty(tokenIn.Identifier);
			if (edmProperty == null)
			{
				segment = null;
				return false;
			}
			if (edmProperty.PropertyKind == EdmPropertyKind.Structural)
			{
				segment = new PropertySegment((IEdmStructuralProperty)edmProperty);
				return true;
			}
			if (edmProperty.PropertyKind == EdmPropertyKind.Navigation)
			{
				segment = new NavigationPropertySegment((IEdmNavigationProperty)edmProperty, null);
				return true;
			}
			throw new ODataException(Strings.SelectExpandBinder_UnknownPropertyType(edmProperty.Name));
		}
	}
}
