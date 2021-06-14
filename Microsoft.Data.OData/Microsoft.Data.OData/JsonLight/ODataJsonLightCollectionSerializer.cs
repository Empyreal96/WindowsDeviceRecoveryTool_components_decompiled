using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000182 RID: 386
	internal sealed class ODataJsonLightCollectionSerializer : ODataJsonLightValueSerializer
	{
		// Token: 0x06000ADB RID: 2779 RVA: 0x000243C7 File Offset: 0x000225C7
		internal ODataJsonLightCollectionSerializer(ODataJsonLightOutputContext jsonLightOutputContext, bool writingTopLevelCollection) : base(jsonLightOutputContext)
		{
			this.writingTopLevelCollection = writingTopLevelCollection;
			this.metadataUriBuilder = jsonLightOutputContext.CreateMetadataUriBuilder();
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x000243E4 File Offset: 0x000225E4
		internal void WriteCollectionStart(ODataCollectionStart collectionStart, IEdmTypeReference itemTypeReference)
		{
			if (this.writingTopLevelCollection)
			{
				base.JsonWriter.StartObjectScope();
				Uri metadataUri;
				if (this.metadataUriBuilder.TryBuildCollectionMetadataUri(collectionStart.SerializationInfo, itemTypeReference, out metadataUri))
				{
					base.WriteMetadataUriProperty(metadataUri);
				}
				base.JsonWriter.WriteValuePropertyName();
			}
			base.JsonWriter.StartArrayScope();
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00024437 File Offset: 0x00022637
		internal void WriteCollectionEnd()
		{
			base.JsonWriter.EndArrayScope();
			if (this.writingTopLevelCollection)
			{
				base.JsonWriter.EndObjectScope();
			}
		}

		// Token: 0x04000400 RID: 1024
		private readonly bool writingTopLevelCollection;

		// Token: 0x04000401 RID: 1025
		private readonly ODataJsonLightMetadataUriBuilder metadataUriBuilder;
	}
}
