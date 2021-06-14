using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.Metadata;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200001B RID: 27
	internal static class FunctionOverloadResolver
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00003CCC File Offset: 0x00001ECC
		internal static IEdmFunctionImport ResolveOverloadsByParameterNames(ICollection<IEdmFunctionImport> functionImports, ICollection<string> parameters, string functionName)
		{
			IEdmFunctionImport edmFunctionImport = null;
			foreach (IEdmFunctionImport edmFunctionImport2 in functionImports)
			{
				IEnumerable<IEdmFunctionParameter> source = edmFunctionImport2.Parameters;
				if (edmFunctionImport2.IsBindable)
				{
					source = source.Skip(1);
				}
				List<IEdmFunctionParameter> list = source.ToList<IEdmFunctionParameter>();
				if (list.Count == parameters.Count)
				{
					if (!list.Any((IEdmFunctionParameter p) => parameters.All((string k) => k != p.Name)))
					{
						if (edmFunctionImport != null)
						{
							throw new ODataException(Strings.FunctionOverloadResolver_NoSingleMatchFound(functionName, string.Join(", ", parameters.ToArray<string>())));
						}
						edmFunctionImport = edmFunctionImport2;
					}
				}
			}
			return edmFunctionImport;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003DB0 File Offset: 0x00001FB0
		internal static bool ResolveFunctionsFromList(string identifier, IList<string> parameterNames, IEdmType bindingType, IEdmModel model, out IEdmFunctionImport matchingFunctionImport)
		{
			if (bindingType != null && bindingType.IsOpenType() && !identifier.Contains("."))
			{
				matchingFunctionImport = null;
				return false;
			}
			IODataUriParserModelExtensions iodataUriParserModelExtensions = model as IODataUriParserModelExtensions;
			if (iodataUriParserModelExtensions != null)
			{
				matchingFunctionImport = iodataUriParserModelExtensions.FindFunctionImportByBindingParameterType(bindingType, identifier, parameterNames);
				return matchingFunctionImport != null;
			}
			IList<IEdmFunctionImport> list = model.FindFunctionImportsBySpecificBindingParameterType(bindingType, identifier).ToList<IEdmFunctionImport>();
			if (list.Count == 0)
			{
				matchingFunctionImport = null;
				return false;
			}
			if (!list.AllHaveEqualReturnTypeAndAttributes())
			{
				throw new ODataException(Strings.RequestUriProcessor_FoundInvalidFunctionImport(identifier));
			}
			if (!list.Any((IEdmFunctionImport f) => f.IsSideEffecting))
			{
				matchingFunctionImport = FunctionOverloadResolver.ResolveOverloadsByParameterNames(list, parameterNames, identifier);
				return matchingFunctionImport != null;
			}
			if (list.Count > 1)
			{
				throw new ODataException(Strings.FunctionOverloadResolver_MultipleActionOverloads(identifier));
			}
			if (parameterNames.Count<string>() != 0)
			{
				throw ExceptionUtil.CreateBadRequestError(Strings.RequestUriProcessor_SegmentDoesNotSupportKeyPredicates(identifier));
			}
			matchingFunctionImport = list.Single<IEdmFunctionImport>();
			return true;
		}
	}
}
