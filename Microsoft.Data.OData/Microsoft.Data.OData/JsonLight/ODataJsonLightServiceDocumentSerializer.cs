using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x020001A1 RID: 417
	internal sealed class ODataJsonLightServiceDocumentSerializer : ODataJsonLightSerializer
	{
		// Token: 0x06000CAF RID: 3247 RVA: 0x0002BCFF File Offset: 0x00029EFF
		internal ODataJsonLightServiceDocumentSerializer(ODataJsonLightOutputContext jsonLightOutputContext) : base(jsonLightOutputContext)
		{
			this.metadataUriBuilder = jsonLightOutputContext.CreateMetadataUriBuilder();
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0002BE84 File Offset: 0x0002A084
		internal void WriteServiceDocument(ODataWorkspace defaultWorkspace)
		{
			IEnumerable<ODataResourceCollectionInfo> collections = defaultWorkspace.Collections;
			base.WriteTopLevelPayload(delegate
			{
				this.JsonWriter.StartObjectScope();
				Uri metadataUri;
				if (this.metadataUriBuilder.TryBuildServiceDocumentMetadataUri(out metadataUri))
				{
					this.WriteMetadataUriProperty(metadataUri);
				}
				this.JsonWriter.WriteValuePropertyName();
				this.JsonWriter.StartArrayScope();
				if (collections != null)
				{
					foreach (ODataResourceCollectionInfo odataResourceCollectionInfo in collections)
					{
						ValidationUtils.ValidateResourceCollectionInfo(odataResourceCollectionInfo);
						if (string.IsNullOrEmpty(odataResourceCollectionInfo.Name))
						{
							throw new ODataException(Strings.ODataJsonLightServiceDocumentSerializer_ResourceCollectionMustSpecifyName);
						}
						this.JsonWriter.StartObjectScope();
						this.JsonWriter.WriteName("name");
						this.JsonWriter.WriteValue(odataResourceCollectionInfo.Name);
						this.JsonWriter.WriteName("url");
						this.JsonWriter.WriteValue(this.UriToString(odataResourceCollectionInfo.Url));
						this.JsonWriter.EndObjectScope();
					}
				}
				this.JsonWriter.EndArrayScope();
				this.JsonWriter.EndObjectScope();
			});
		}

		// Token: 0x0400044C RID: 1100
		private readonly ODataJsonLightMetadataUriBuilder metadataUriBuilder;
	}
}
