using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200013B RID: 315
	internal static class ODataAnnotationNames
	{
		// Token: 0x0600086E RID: 2158 RVA: 0x0001B754 File Offset: 0x00019954
		internal static bool IsODataAnnotationName(string annotationName)
		{
			return annotationName.StartsWith("odata.", StringComparison.Ordinal);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0001B767 File Offset: 0x00019967
		internal static bool IsUnknownODataAnnotationName(string annotationName)
		{
			return ODataAnnotationNames.IsODataAnnotationName(annotationName) && !ODataAnnotationNames.KnownODataAnnotationNames.Contains(annotationName);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0001B781 File Offset: 0x00019981
		internal static void ValidateIsCustomAnnotationName(string annotationName)
		{
			if (ODataAnnotationNames.KnownODataAnnotationNames.Contains(annotationName))
			{
				throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties(annotationName));
			}
		}

		// Token: 0x04000332 RID: 818
		internal const string ODataMetadata = "odata.metadata";

		// Token: 0x04000333 RID: 819
		internal const string ODataNull = "odata.null";

		// Token: 0x04000334 RID: 820
		internal const string ODataType = "odata.type";

		// Token: 0x04000335 RID: 821
		internal const string ODataId = "odata.id";

		// Token: 0x04000336 RID: 822
		internal const string ODataETag = "odata.etag";

		// Token: 0x04000337 RID: 823
		internal const string ODataEditLink = "odata.editLink";

		// Token: 0x04000338 RID: 824
		internal const string ODataReadLink = "odata.readLink";

		// Token: 0x04000339 RID: 825
		internal const string ODataMediaEditLink = "odata.mediaEditLink";

		// Token: 0x0400033A RID: 826
		internal const string ODataMediaReadLink = "odata.mediaReadLink";

		// Token: 0x0400033B RID: 827
		internal const string ODataMediaContentType = "odata.mediaContentType";

		// Token: 0x0400033C RID: 828
		internal const string ODataMediaETag = "odata.mediaETag";

		// Token: 0x0400033D RID: 829
		internal const string ODataCount = "odata.count";

		// Token: 0x0400033E RID: 830
		internal const string ODataNextLink = "odata.nextLink";

		// Token: 0x0400033F RID: 831
		internal const string ODataNavigationLinkUrl = "odata.navigationLinkUrl";

		// Token: 0x04000340 RID: 832
		internal const string ODataBind = "odata.bind";

		// Token: 0x04000341 RID: 833
		internal const string ODataAssociationLinkUrl = "odata.associationLinkUrl";

		// Token: 0x04000342 RID: 834
		internal const string ODataAnnotationGroup = "odata.annotationGroup";

		// Token: 0x04000343 RID: 835
		internal const string ODataAnnotationGroupReference = "odata.annotationGroupReference";

		// Token: 0x04000344 RID: 836
		internal const string ODataError = "odata.error";

		// Token: 0x04000345 RID: 837
		internal const string ODataDeltaLink = "odata.deltaLink";

		// Token: 0x04000346 RID: 838
		internal static readonly HashSet<string> KnownODataAnnotationNames = new HashSet<string>(new string[]
		{
			"odata.metadata",
			"odata.null",
			"odata.type",
			"odata.id",
			"odata.etag",
			"odata.editLink",
			"odata.readLink",
			"odata.mediaEditLink",
			"odata.mediaReadLink",
			"odata.mediaContentType",
			"odata.mediaETag",
			"odata.count",
			"odata.nextLink",
			"odata.bind",
			"odata.associationLinkUrl",
			"odata.navigationLinkUrl",
			"odata.annotationGroup",
			"odata.annotationGroupReference",
			"odata.error",
			"odata.deltaLink"
		}, StringComparer.Ordinal);
	}
}
