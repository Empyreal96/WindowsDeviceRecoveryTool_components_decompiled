using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200010B RID: 267
	internal sealed class JsonFullMetadataLevel : JsonLightMetadataLevel
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x00018E10 File Offset: 0x00017010
		internal JsonFullMetadataLevel(Uri metadataDocumentUri, IEdmModel model)
		{
			this.metadataDocumentUri = metadataDocumentUri;
			this.model = model;
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00018E26 File Offset: 0x00017026
		private Uri NonNullMetadataDocumentUri
		{
			get
			{
				if (this.metadataDocumentUri == null)
				{
					throw new ODataException(Strings.ODataJsonLightOutputContext_MetadataDocumentUriMissing);
				}
				return this.metadataDocumentUri;
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00018E47 File Offset: 0x00017047
		internal override JsonLightTypeNameOracle GetTypeNameOracle(bool autoComputePayloadMetadataInJson)
		{
			if (autoComputePayloadMetadataInJson)
			{
				return new JsonFullMetadataTypeNameOracle();
			}
			return new JsonMinimalMetadataTypeNameOracle();
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00018E57 File Offset: 0x00017057
		internal override bool ShouldWriteODataMetadataUri()
		{
			return true;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00018E5C File Offset: 0x0001705C
		internal override ODataEntityMetadataBuilder CreateEntityMetadataBuilder(ODataEntry entry, IODataFeedAndEntryTypeContext typeContext, ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntityType actualEntityType, SelectedPropertiesNode selectedProperties, bool isResponse, bool? keyAsSegment)
		{
			IODataMetadataContext iodataMetadataContext = new ODataMetadataContext(isResponse, this.model, this.NonNullMetadataDocumentUri);
			UrlConvention urlConvention = UrlConvention.ForUserSettingAndTypeContext(keyAsSegment, typeContext);
			ODataConventionalUriBuilder uriBuilder = new ODataConventionalUriBuilder(iodataMetadataContext.ServiceBaseUri, urlConvention);
			IODataEntryMetadataContext entryMetadataContext = ODataEntryMetadataContext.Create(entry, typeContext, serializationInfo, actualEntityType, iodataMetadataContext, selectedProperties);
			return new ODataConventionalEntityMetadataBuilder(entryMetadataContext, iodataMetadataContext, uriBuilder);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00018EAC File Offset: 0x000170AC
		internal override void InjectMetadataBuilder(ODataEntry entry, ODataEntityMetadataBuilder builder)
		{
			entry.MetadataBuilder = builder;
			ODataStreamReferenceValue nonComputedMediaResource = entry.NonComputedMediaResource;
			if (nonComputedMediaResource != null)
			{
				nonComputedMediaResource.SetMetadataBuilder(builder, null);
			}
			if (entry.NonComputedProperties != null)
			{
				foreach (ODataProperty odataProperty in entry.NonComputedProperties)
				{
					ODataStreamReferenceValue odataStreamReferenceValue = odataProperty.ODataValue as ODataStreamReferenceValue;
					if (odataStreamReferenceValue != null)
					{
						odataStreamReferenceValue.SetMetadataBuilder(builder, odataProperty.Name);
					}
				}
			}
			IEnumerable<ODataOperation> enumerable = ODataUtilsInternal.ConcatEnumerables<ODataOperation>(entry.NonComputedActions, entry.NonComputedFunctions);
			if (enumerable != null)
			{
				foreach (ODataOperation odataOperation in enumerable)
				{
					odataOperation.SetMetadataBuilder(builder, this.NonNullMetadataDocumentUri);
				}
			}
		}

		// Token: 0x040002C0 RID: 704
		private readonly IEdmModel model;

		// Token: 0x040002C1 RID: 705
		private readonly Uri metadataDocumentUri;
	}
}
