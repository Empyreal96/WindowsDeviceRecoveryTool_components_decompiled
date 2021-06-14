using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x0200012F RID: 303
	public interface IODataUriParserModelExtensions
	{
		// Token: 0x060007F8 RID: 2040
		IEnumerable<IEdmFunctionImport> FindFunctionImportsByBindingParameterTypeHierarchy(IEdmType bindingType, string functionImportName);

		// Token: 0x060007F9 RID: 2041
		IEdmEntitySet FindEntitySetFromContainerQualifiedName(string containerQualifiedEntitySetName);

		// Token: 0x060007FA RID: 2042
		IEdmFunctionImport FindServiceOperation(string serviceOperationName);

		// Token: 0x060007FB RID: 2043
		IEdmFunctionImport FindFunctionImportByBindingParameterType(IEdmType bindingType, string functionImportName, IEnumerable<string> nonBindingParameterNamesFromUri);
	}
}
