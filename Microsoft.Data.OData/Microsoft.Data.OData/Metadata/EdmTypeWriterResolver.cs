using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000148 RID: 328
	internal sealed class EdmTypeWriterResolver : EdmTypeResolver
	{
		// Token: 0x060008D9 RID: 2265 RVA: 0x0001C783 File Offset: 0x0001A983
		private EdmTypeWriterResolver()
		{
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001C78B File Offset: 0x0001A98B
		internal override IEdmEntityType GetElementType(IEdmEntitySet entitySet)
		{
			if (entitySet != null)
			{
				return entitySet.ElementType;
			}
			return null;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0001C798 File Offset: 0x0001A998
		internal override IEdmTypeReference GetReturnType(IEdmFunctionImport functionImport)
		{
			if (functionImport != null)
			{
				return functionImport.ReturnType;
			}
			return null;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001C7A5 File Offset: 0x0001A9A5
		internal override IEdmTypeReference GetReturnType(IEnumerable<IEdmFunctionImport> functionImportGroup)
		{
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EdmTypeWriterResolver_GetReturnTypeForFunctionImportGroup));
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001C7B8 File Offset: 0x0001A9B8
		internal override IEdmTypeReference GetParameterType(IEdmFunctionParameter functionParameter)
		{
			if (functionParameter != null)
			{
				return functionParameter.Type;
			}
			return null;
		}

		// Token: 0x04000355 RID: 853
		internal static EdmTypeWriterResolver Instance = new EdmTypeWriterResolver();
	}
}
