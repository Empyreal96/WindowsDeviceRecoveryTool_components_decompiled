using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000147 RID: 327
	internal sealed class EdmTypeReaderResolver : EdmTypeResolver
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x0001C641 File Offset: 0x0001A841
		public EdmTypeReaderResolver(IEdmModel model, ODataReaderBehavior readerBehavior, ODataVersion version)
		{
			this.model = model;
			this.readerBehavior = readerBehavior;
			this.version = version;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0001C65E File Offset: 0x0001A85E
		internal override IEdmEntityType GetElementType(IEdmEntitySet entitySet)
		{
			if (entitySet != null)
			{
				return (IEdmEntityType)this.ResolveType(entitySet.ElementType);
			}
			return null;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0001C676 File Offset: 0x0001A876
		internal override IEdmTypeReference GetReturnType(IEdmFunctionImport functionImport)
		{
			if (functionImport != null && functionImport.ReturnType != null)
			{
				return this.ResolveTypeReference(functionImport.ReturnType);
			}
			return null;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0001C694 File Offset: 0x0001A894
		internal override IEdmTypeReference GetReturnType(IEnumerable<IEdmFunctionImport> functionImportGroup)
		{
			IEdmFunctionImport functionImport = functionImportGroup.FirstOrDefault<IEdmFunctionImport>();
			return this.GetReturnType(functionImport);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0001C6AF File Offset: 0x0001A8AF
		internal override IEdmTypeReference GetParameterType(IEdmFunctionParameter functionParameter)
		{
			if (functionParameter != null)
			{
				return this.ResolveTypeReference(functionParameter.Type);
			}
			return null;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0001C6C4 File Offset: 0x0001A8C4
		private IEdmTypeReference ResolveTypeReference(IEdmTypeReference typeReferenceToResolve)
		{
			if (this.readerBehavior.TypeResolver == null)
			{
				return typeReferenceToResolve;
			}
			return this.ResolveType(typeReferenceToResolve.Definition).ToTypeReference(typeReferenceToResolve.IsNullable);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		private IEdmType ResolveType(IEdmType typeToResolve)
		{
			Func<IEdmType, string, IEdmType> typeResolver = this.readerBehavior.TypeResolver;
			if (typeResolver == null)
			{
				return typeToResolve;
			}
			IEdmCollectionType edmCollectionType = typeToResolve as IEdmCollectionType;
			EdmTypeKind edmTypeKind;
			if (edmCollectionType != null && edmCollectionType.ElementType.IsEntity())
			{
				IEdmTypeReference elementType = edmCollectionType.ElementType;
				IEdmType type = MetadataUtils.ResolveTypeName(this.model, null, elementType.FullName(), typeResolver, this.version, out edmTypeKind);
				return new EdmCollectionType(type.ToTypeReference(elementType.IsNullable));
			}
			return MetadataUtils.ResolveTypeName(this.model, null, typeToResolve.ODataFullName(), typeResolver, this.version, out edmTypeKind);
		}

		// Token: 0x04000352 RID: 850
		private readonly IEdmModel model;

		// Token: 0x04000353 RID: 851
		private readonly ODataReaderBehavior readerBehavior;

		// Token: 0x04000354 RID: 852
		private readonly ODataVersion version;
	}
}
