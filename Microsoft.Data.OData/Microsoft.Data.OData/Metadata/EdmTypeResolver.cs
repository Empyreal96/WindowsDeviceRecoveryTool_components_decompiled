using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000146 RID: 326
	internal abstract class EdmTypeResolver
	{
		// Token: 0x060008CD RID: 2253
		internal abstract IEdmEntityType GetElementType(IEdmEntitySet entitySet);

		// Token: 0x060008CE RID: 2254
		internal abstract IEdmTypeReference GetReturnType(IEdmFunctionImport functionImport);

		// Token: 0x060008CF RID: 2255
		internal abstract IEdmTypeReference GetReturnType(IEnumerable<IEdmFunctionImport> functionImportGroup);

		// Token: 0x060008D0 RID: 2256
		internal abstract IEdmTypeReference GetParameterType(IEdmFunctionParameter functionParameter);
	}
}
