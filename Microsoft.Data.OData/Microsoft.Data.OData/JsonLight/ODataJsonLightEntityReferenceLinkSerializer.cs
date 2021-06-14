using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200018A RID: 394
	internal sealed class ODataJsonLightEntityReferenceLinkSerializer : ODataJsonLightSerializer
	{
		// Token: 0x06000B2C RID: 2860 RVA: 0x000253B5 File Offset: 0x000235B5
		internal ODataJsonLightEntityReferenceLinkSerializer(ODataJsonLightOutputContext jsonLightOutputContext) : base(jsonLightOutputContext)
		{
			this.metadataUriBuilder = jsonLightOutputContext.CreateMetadataUriBuilder();
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x000253F4 File Offset: 0x000235F4
		internal void WriteEntityReferenceLink(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			base.WriteTopLevelPayload(delegate
			{
				this.WriteEntityReferenceLinkImplementation(link, entitySet, navigationProperty, true);
			});
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0002545C File Offset: 0x0002365C
		internal void WriteEntityReferenceLinks(ODataEntityReferenceLinks entityReferenceLinks, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			base.WriteTopLevelPayload(delegate
			{
				this.WriteEntityReferenceLinksImplementation(entityReferenceLinks, entitySet, navigationProperty);
			});
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000254A0 File Offset: 0x000236A0
		private void WriteEntityReferenceLinkImplementation(ODataEntityReferenceLink entityReferenceLink, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty, bool isTopLevel)
		{
			WriterValidationUtils.ValidateEntityReferenceLink(entityReferenceLink);
			base.JsonWriter.StartObjectScope();
			Uri metadataUri;
			if (isTopLevel && this.metadataUriBuilder.TryBuildEntityReferenceLinkMetadataUri(entityReferenceLink.SerializationInfo, entitySet, navigationProperty, out metadataUri))
			{
				base.WriteMetadataUriProperty(metadataUri);
			}
			base.JsonWriter.WriteName("url");
			base.JsonWriter.WriteValue(base.UriToString(entityReferenceLink.Url));
			base.JsonWriter.EndObjectScope();
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00025514 File Offset: 0x00023714
		private void WriteEntityReferenceLinksImplementation(ODataEntityReferenceLinks entityReferenceLinks, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			bool flag = false;
			bool flag2 = false;
			base.JsonWriter.StartObjectScope();
			Uri metadataUri;
			if (this.metadataUriBuilder.TryBuildEntityReferenceLinksMetadataUri(entityReferenceLinks.SerializationInfo, entitySet, navigationProperty, out metadataUri))
			{
				base.WriteMetadataUriProperty(metadataUri);
			}
			if (entityReferenceLinks.Count != null)
			{
				flag2 = true;
				this.WriteCountAnnotation(entityReferenceLinks.Count.Value);
			}
			if (entityReferenceLinks.NextPageLink != null)
			{
				flag = true;
				this.WriteNextLinkAnnotation(entityReferenceLinks.NextPageLink);
			}
			base.JsonWriter.WriteValuePropertyName();
			base.JsonWriter.StartArrayScope();
			IEnumerable<ODataEntityReferenceLink> links = entityReferenceLinks.Links;
			if (links != null)
			{
				foreach (ODataEntityReferenceLink entityReferenceLink in links)
				{
					WriterValidationUtils.ValidateEntityReferenceLinkNotNull(entityReferenceLink);
					this.WriteEntityReferenceLinkImplementation(entityReferenceLink, entitySet, null, false);
				}
			}
			base.JsonWriter.EndArrayScope();
			if (!flag2 && entityReferenceLinks.Count != null)
			{
				this.WriteCountAnnotation(entityReferenceLinks.Count.Value);
			}
			if (!flag && entityReferenceLinks.NextPageLink != null)
			{
				this.WriteNextLinkAnnotation(entityReferenceLinks.NextPageLink);
			}
			base.JsonWriter.EndObjectScope();
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002565C File Offset: 0x0002385C
		private void WriteNextLinkAnnotation(Uri nextPageLink)
		{
			base.JsonWriter.WriteName("odata.nextLink");
			base.JsonWriter.WriteValue(base.UriToString(nextPageLink));
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00025680 File Offset: 0x00023880
		private void WriteCountAnnotation(long countValue)
		{
			base.JsonWriter.WriteName("odata.count");
			base.JsonWriter.WriteValue(countValue);
		}

		// Token: 0x04000414 RID: 1044
		private readonly ODataJsonLightMetadataUriBuilder metadataUriBuilder;
	}
}
