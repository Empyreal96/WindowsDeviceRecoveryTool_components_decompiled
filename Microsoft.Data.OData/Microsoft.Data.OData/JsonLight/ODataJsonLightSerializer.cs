using System;
using System.Collections.Generic;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000143 RID: 323
	internal class ODataJsonLightSerializer : ODataSerializer
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x0001C074 File Offset: 0x0001A274
		internal ODataJsonLightSerializer(ODataJsonLightOutputContext jsonLightOutputContext) : base(jsonLightOutputContext)
		{
			this.jsonLightOutputContext = jsonLightOutputContext;
			this.instanceAnnotationWriter = new SimpleLazy<JsonLightInstanceAnnotationWriter>(() => new JsonLightInstanceAnnotationWriter(new ODataJsonLightValueSerializer(jsonLightOutputContext), jsonLightOutputContext.TypeNameOracle));
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0001C0C4 File Offset: 0x0001A2C4
		internal ODataJsonLightOutputContext JsonLightOutputContext
		{
			get
			{
				return this.jsonLightOutputContext;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0001C0CC File Offset: 0x0001A2CC
		internal IJsonWriter JsonWriter
		{
			get
			{
				return this.jsonLightOutputContext.JsonWriter;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0001C0D9 File Offset: 0x0001A2D9
		internal JsonLightInstanceAnnotationWriter InstanceAnnotationWriter
		{
			get
			{
				return this.instanceAnnotationWriter.Value;
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0001C0E6 File Offset: 0x0001A2E6
		internal void WritePayloadStart()
		{
			ODataJsonWriterUtils.StartJsonPaddingIfRequired(this.JsonWriter, base.MessageWriterSettings);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001C0F9 File Offset: 0x0001A2F9
		internal void WritePayloadEnd()
		{
			ODataJsonWriterUtils.EndJsonPaddingIfRequired(this.JsonWriter, base.MessageWriterSettings);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001C10C File Offset: 0x0001A30C
		internal void WriteMetadataUriProperty(Uri metadataUri)
		{
			this.JsonWriter.WriteName("odata.metadata");
			this.JsonWriter.WritePrimitiveValue(metadataUri.AbsoluteUri, base.Version);
			this.allowRelativeUri = true;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001C13C File Offset: 0x0001A33C
		internal void WriteTopLevelPayload(Action payloadWriterAction)
		{
			this.WritePayloadStart();
			payloadWriterAction();
			this.WritePayloadEnd();
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0001C1B4 File Offset: 0x0001A3B4
		internal void WriteTopLevelError(ODataError error, bool includeDebugInformation)
		{
			this.WriteTopLevelPayload(delegate
			{
				ODataJsonWriterUtils.WriteError(this.JsonLightOutputContext.JsonWriter, new Action<IEnumerable<ODataInstanceAnnotation>>(this.InstanceAnnotationWriter.WriteInstanceAnnotations), error, includeDebugInformation, this.MessageWriterSettings.MessageQuotas.MaxNestingDepth, true);
			});
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001C1F0 File Offset: 0x0001A3F0
		internal string UriToString(Uri uri)
		{
			ODataMetadataDocumentUri metadataDocumentUri = this.jsonLightOutputContext.MessageWriterSettings.MetadataDocumentUri;
			Uri uri2 = (metadataDocumentUri == null) ? null : metadataDocumentUri.BaseUri;
			Uri uri3;
			if (this.jsonLightOutputContext.UrlResolver != null)
			{
				uri3 = this.jsonLightOutputContext.UrlResolver.ResolveUrl(uri2, uri);
				if (uri3 != null)
				{
					return UriUtilsCommon.UriToString(uri3);
				}
			}
			uri3 = uri;
			if (!uri3.IsAbsoluteUri)
			{
				if (!this.allowRelativeUri)
				{
					if (uri2 == null)
					{
						throw new ODataException(Strings.ODataJsonLightSerializer_RelativeUriUsedWithoutMetadataDocumentUriOrMetadata(UriUtilsCommon.UriToString(uri3)));
					}
					uri3 = UriUtils.UriToAbsoluteUri(uri2, uri);
				}
				else
				{
					uri3 = UriUtils.EnsureEscapedRelativeUri(uri3);
				}
			}
			return UriUtilsCommon.UriToString(uri3);
		}

		// Token: 0x0400034D RID: 845
		private readonly ODataJsonLightOutputContext jsonLightOutputContext;

		// Token: 0x0400034E RID: 846
		private readonly SimpleLazy<JsonLightInstanceAnnotationWriter> instanceAnnotationWriter;

		// Token: 0x0400034F RID: 847
		private bool allowRelativeUri;
	}
}
