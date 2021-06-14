using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000293 RID: 659
	internal static class UriUtils
	{
		// Token: 0x06001637 RID: 5687 RVA: 0x00050E96 File Offset: 0x0004F096
		internal static Uri UriToAbsoluteUri(Uri baseUri, Uri relativeUri)
		{
			return new Uri(baseUri, relativeUri);
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00050EA0 File Offset: 0x0004F0A0
		internal static Uri EnsureEscapedRelativeUri(Uri uri)
		{
			string components = uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
			if (string.CompareOrdinal(uri.OriginalString, components) == 0)
			{
				return uri;
			}
			return new Uri(components, UriKind.Relative);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00050ED1 File Offset: 0x0004F0D1
		internal static string EnsureEscapedFragment(string fragmentString)
		{
			return new Uri(UriUtils.ExampleMetadataAbsoluteUri, fragmentString).Fragment;
		}

		// Token: 0x040008C5 RID: 2245
		private static Uri ExampleMetadataAbsoluteUri = new Uri("http://www.example.com/$metadata", UriKind.Absolute);
	}
}
