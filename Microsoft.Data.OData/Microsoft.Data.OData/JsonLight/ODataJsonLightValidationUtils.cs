using System;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200013F RID: 319
	internal static class ODataJsonLightValidationUtils
	{
		// Token: 0x0600088C RID: 2188 RVA: 0x0001BD24 File Offset: 0x00019F24
		internal static void ValidateMetadataReferencePropertyName(Uri metadataDocumentUri, string propertyName)
		{
			string uriString = propertyName;
			if (propertyName[0] == '#')
			{
				uriString = UriUtilsCommon.UriToString(metadataDocumentUri) + UriUtils.EnsureEscapedFragment(propertyName);
			}
			if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute) || !ODataJsonLightUtils.IsMetadataReferenceProperty(propertyName) || propertyName[propertyName.Length - 1] == '#')
			{
				throw new ODataException(Strings.ValidationUtils_InvalidMetadataReferenceProperty(propertyName));
			}
			if (ODataJsonLightValidationUtils.IsOpenMetadataReferencePropertyName(metadataDocumentUri, propertyName))
			{
				throw new ODataException(Strings.ODataJsonLightValidationUtils_OpenMetadataReferencePropertyNotSupported(propertyName, UriUtilsCommon.UriToString(metadataDocumentUri)));
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001BD9C File Offset: 0x00019F9C
		internal static void ValidateOperation(Uri metadataDocumentUri, ODataOperation operation)
		{
			ValidationUtils.ValidateOperationMetadataNotNull(operation);
			string propertyName = UriUtilsCommon.UriToString(operation.Metadata);
			if (metadataDocumentUri != null)
			{
				ODataJsonLightValidationUtils.ValidateMetadataReferencePropertyName(metadataDocumentUri, propertyName);
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001BDCB File Offset: 0x00019FCB
		internal static bool IsOpenMetadataReferencePropertyName(Uri metadataDocumentUri, string propertyName)
		{
			return ODataJsonLightUtils.IsMetadataReferenceProperty(propertyName) && propertyName[0] != '#' && !propertyName.StartsWith(UriUtilsCommon.UriToString(metadataDocumentUri), StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001BDF2 File Offset: 0x00019FF2
		internal static void ValidateOperationPropertyValueIsNotNull(object propertyValue, string propertyName, string metadata)
		{
			if (propertyValue == null)
			{
				throw new ODataException(Strings.ODataJsonLightValidationUtils_OperationPropertyCannotBeNull(propertyName, metadata));
			}
		}
	}
}
