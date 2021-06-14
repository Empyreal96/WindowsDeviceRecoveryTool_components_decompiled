using System;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200013D RID: 317
	internal static class ODataJsonLightUtils
	{
		// Token: 0x0600087B RID: 2171 RVA: 0x0001B86C File Offset: 0x00019A6C
		internal static bool IsMetadataReferenceProperty(string propertyName)
		{
			return propertyName.IndexOf('#') >= 0;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001B87C File Offset: 0x00019A7C
		internal static string GetFullyQualifiedFunctionImportName(Uri metadataDocumentUri, string metadataReferencePropertyName, out string firstParameterTypeName)
		{
			string text = ODataJsonLightUtils.GetUriFragmentFromMetadataReferencePropertyName(metadataDocumentUri, metadataReferencePropertyName);
			firstParameterTypeName = null;
			int num = text.IndexOf('(');
			if (num > -1)
			{
				string text2 = text.Substring(num + 1);
				text = text.Substring(0, num);
				firstParameterTypeName = text2.Split(ODataJsonLightUtils.ParameterSeparatorSplitCharacters).First<string>().Trim(ODataJsonLightUtils.CharactersToTrimFromParameters);
			}
			return text;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001B8D4 File Offset: 0x00019AD4
		internal static string GetUriFragmentFromMetadataReferencePropertyName(Uri metadataDocumentUri, string propertyName)
		{
			return ODataJsonLightUtils.GetAbsoluteUriFromMetadataReferencePropertyName(metadataDocumentUri, propertyName).GetComponents(UriComponents.Fragment, UriFormat.Unescaped);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001B8F2 File Offset: 0x00019AF2
		internal static Uri GetAbsoluteUriFromMetadataReferencePropertyName(Uri metadataDocumentUri, string propertyName)
		{
			if (propertyName[0] == '#')
			{
				propertyName = UriUtils.EnsureEscapedFragment(propertyName);
				return new Uri(metadataDocumentUri, propertyName);
			}
			return new Uri(propertyName, UriKind.Absolute);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001B918 File Offset: 0x00019B18
		internal static string GetMetadataReferenceName(IEdmFunctionImport functionImport)
		{
			string result = functionImport.FullName();
			bool flag = functionImport.Container.FindFunctionImports(functionImport.Name).Take(2).Count<IEdmFunctionImport>() > 1;
			if (flag)
			{
				result = functionImport.FullNameWithParameters();
			}
			return result;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001B958 File Offset: 0x00019B58
		internal static ODataOperation CreateODataOperation(Uri metadataDocumentUri, string metadataReferencePropertyName, IEdmFunctionImport functionImport, out bool isAction)
		{
			isAction = functionImport.IsSideEffecting;
			ODataOperation odataOperation = isAction ? new ODataAction() : new ODataFunction();
			odataOperation.Metadata = ODataJsonLightUtils.GetAbsoluteUriFromMetadataReferencePropertyName(metadataDocumentUri, metadataReferencePropertyName);
			return odataOperation;
		}

		// Token: 0x04000347 RID: 839
		private static readonly char[] ParameterSeparatorSplitCharacters = new char[]
		{
			","[0]
		};

		// Token: 0x04000348 RID: 840
		private static readonly char[] CharactersToTrimFromParameters = new char[]
		{
			'(',
			')'
		};
	}
}
