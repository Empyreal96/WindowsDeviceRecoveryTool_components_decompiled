using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200014E RID: 334
	internal sealed class ODataMetadataDocumentUri
	{
		// Token: 0x0600090E RID: 2318 RVA: 0x0001CC9A File Offset: 0x0001AE9A
		internal ODataMetadataDocumentUri(Uri baseUri)
		{
			ExceptionUtils.CheckArgumentNotNull<Uri>(baseUri, "baseUri");
			if (!baseUri.IsAbsoluteUri)
			{
				throw new ODataException(Strings.WriterValidationUtils_MessageWriterSettingsMetadataDocumentUriMustBeNullOrAbsolute(UriUtilsCommon.UriToString(baseUri)));
			}
			this.baseUri = baseUri;
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0001CCCD File Offset: 0x0001AECD
		internal Uri BaseUri
		{
			get
			{
				return this.baseUri;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0001CCD5 File Offset: 0x0001AED5
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x0001CCDD File Offset: 0x0001AEDD
		internal string SelectClause
		{
			get
			{
				return this.selectClause;
			}
			set
			{
				this.selectClause = value;
			}
		}

		// Token: 0x04000362 RID: 866
		private readonly Uri baseUri;

		// Token: 0x04000363 RID: 867
		private string selectClause;
	}
}
