using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000193 RID: 403
	internal sealed class ODataJsonLightOutputContext : ODataJsonOutputContextBase
	{
		// Token: 0x06000BE2 RID: 3042 RVA: 0x000292C8 File Offset: 0x000274C8
		internal ODataJsonLightOutputContext(ODataFormat format, TextWriter textWriter, ODataMessageWriterSettings messageWriterSettings, IEdmModel model) : base(format, textWriter, messageWriterSettings, model)
		{
			this.metadataLevel = new JsonMinimalMetadataLevel();
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x000292E0 File Offset: 0x000274E0
		internal ODataJsonLightOutputContext(ODataFormat format, Stream messageStream, MediaType mediaType, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageStream, encoding, messageWriterSettings, writingResponse, synchronous, model, urlResolver)
		{
			Uri metadataDocumentUri = (messageWriterSettings.MetadataDocumentUri == null) ? null : messageWriterSettings.MetadataDocumentUri.BaseUri;
			this.metadataLevel = JsonLightMetadataLevel.Create(mediaType, metadataDocumentUri, model, writingResponse);
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0002932B File Offset: 0x0002752B
		internal JsonLightTypeNameOracle TypeNameOracle
		{
			get
			{
				if (this.typeNameOracle == null)
				{
					this.typeNameOracle = this.MetadataLevel.GetTypeNameOracle(base.MessageWriterSettings.AutoComputePayloadMetadataInJson);
				}
				return this.typeNameOracle;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00029357 File Offset: 0x00027557
		internal JsonLightMetadataLevel MetadataLevel
		{
			get
			{
				return this.metadataLevel;
			}
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0002935F File Offset: 0x0002755F
		internal ODataJsonLightMetadataUriBuilder CreateMetadataUriBuilder()
		{
			return ODataJsonLightMetadataUriBuilder.CreateFromSettings(this.MetadataLevel, base.WritingResponse, base.MessageWriterSettings, base.Model);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0002937E File Offset: 0x0002757E
		internal override void WriteInStreamError(ODataError error, bool includeDebugInformation)
		{
			this.WriteInStreamErrorImplementation(error, includeDebugInformation);
			base.Flush();
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000293BC File Offset: 0x000275BC
		internal override Task WriteInStreamErrorAsync(ODataError error, bool includeDebugInformation)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteInStreamErrorImplementation(error, includeDebugInformation);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x000293F5 File Offset: 0x000275F5
		internal override ODataWriter CreateODataFeedWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return this.CreateODataFeedWriterImplementation(entitySet, entityType);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00029420 File Offset: 0x00027620
		internal override Task<ODataWriter> CreateODataFeedWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataWriter>(() => this.CreateODataFeedWriterImplementation(entitySet, entityType));
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00029459 File Offset: 0x00027659
		internal override ODataWriter CreateODataEntryWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return this.CreateODataEntryWriterImplementation(entitySet, entityType);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00029484 File Offset: 0x00027684
		internal override Task<ODataWriter> CreateODataEntryWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataWriter>(() => this.CreateODataEntryWriterImplementation(entitySet, entityType));
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x000294BD File Offset: 0x000276BD
		internal override ODataCollectionWriter CreateODataCollectionWriter(IEdmTypeReference itemTypeReference)
		{
			return this.CreateODataCollectionWriterImplementation(itemTypeReference);
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x000294E4 File Offset: 0x000276E4
		internal override Task<ODataCollectionWriter> CreateODataCollectionWriterAsync(IEdmTypeReference itemTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataCollectionWriter>(() => this.CreateODataCollectionWriterImplementation(itemTypeReference));
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00029516 File Offset: 0x00027716
		internal override ODataParameterWriter CreateODataParameterWriter(IEdmFunctionImport functionImport)
		{
			return this.CreateODataParameterWriterImplementation(functionImport);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0002953C File Offset: 0x0002773C
		internal override Task<ODataParameterWriter> CreateODataParameterWriterAsync(IEdmFunctionImport functionImport)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataParameterWriter>(() => this.CreateODataParameterWriterImplementation(functionImport));
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0002956E File Offset: 0x0002776E
		internal override void WriteServiceDocument(ODataWorkspace defaultWorkspace)
		{
			this.WriteServiceDocumentImplementation(defaultWorkspace);
			base.Flush();
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x000295A4 File Offset: 0x000277A4
		internal override Task WriteServiceDocumentAsync(ODataWorkspace defaultWorkspace)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteServiceDocumentImplementation(defaultWorkspace);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000295D6 File Offset: 0x000277D6
		internal override void WriteProperty(ODataProperty property)
		{
			this.WritePropertyImplementation(property);
			base.Flush();
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0002960C File Offset: 0x0002780C
		internal override Task WritePropertyAsync(ODataProperty property)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WritePropertyImplementation(property);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002963E File Offset: 0x0002783E
		internal override void WriteError(ODataError error, bool includeDebugInformation)
		{
			this.WriteErrorImplementation(error, includeDebugInformation);
			base.Flush();
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002967C File Offset: 0x0002787C
		internal override Task WriteErrorAsync(ODataError error, bool includeDebugInformation)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteErrorImplementation(error, includeDebugInformation);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000296B5 File Offset: 0x000278B5
		internal override void WriteEntityReferenceLinks(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.WriteEntityReferenceLinksImplementation(links, entitySet, navigationProperty);
			base.Flush();
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x000296F8 File Offset: 0x000278F8
		internal override Task WriteEntityReferenceLinksAsync(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteEntityReferenceLinksImplementation(links, entitySet, navigationProperty);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00029738 File Offset: 0x00027938
		internal override void WriteEntityReferenceLink(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.WriteEntityReferenceLinkImplementation(link, entitySet, navigationProperty);
			base.Flush();
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0002977C File Offset: 0x0002797C
		internal override Task WriteEntityReferenceLinkAsync(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteEntityReferenceLinkImplementation(link, entitySet, navigationProperty);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000297BC File Offset: 0x000279BC
		private ODataWriter CreateODataFeedWriterImplementation(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			ODataJsonLightWriter odataJsonLightWriter = new ODataJsonLightWriter(this, entitySet, entityType, true);
			this.outputInStreamErrorListener = odataJsonLightWriter;
			return odataJsonLightWriter;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000297DC File Offset: 0x000279DC
		private ODataWriter CreateODataEntryWriterImplementation(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			ODataJsonLightWriter odataJsonLightWriter = new ODataJsonLightWriter(this, entitySet, entityType, false);
			this.outputInStreamErrorListener = odataJsonLightWriter;
			return odataJsonLightWriter;
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000297FC File Offset: 0x000279FC
		private ODataCollectionWriter CreateODataCollectionWriterImplementation(IEdmTypeReference itemTypeReference)
		{
			ODataJsonLightCollectionWriter odataJsonLightCollectionWriter = new ODataJsonLightCollectionWriter(this, itemTypeReference);
			this.outputInStreamErrorListener = odataJsonLightCollectionWriter;
			return odataJsonLightCollectionWriter;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002981C File Offset: 0x00027A1C
		private ODataParameterWriter CreateODataParameterWriterImplementation(IEdmFunctionImport functionImport)
		{
			ODataJsonLightParameterWriter odataJsonLightParameterWriter = new ODataJsonLightParameterWriter(this, functionImport);
			this.outputInStreamErrorListener = odataJsonLightParameterWriter;
			return odataJsonLightParameterWriter;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0002983C File Offset: 0x00027A3C
		private void WriteInStreamErrorImplementation(ODataError error, bool includeDebugInformation)
		{
			if (this.outputInStreamErrorListener != null)
			{
				this.outputInStreamErrorListener.OnInStreamError();
			}
			JsonLightInstanceAnnotationWriter @object = new JsonLightInstanceAnnotationWriter(new ODataJsonLightValueSerializer(this), this.TypeNameOracle);
			ODataJsonWriterUtils.WriteError(base.JsonWriter, new Action<IEnumerable<ODataInstanceAnnotation>>(@object.WriteInstanceAnnotations), error, includeDebugInformation, base.MessageWriterSettings.MessageQuotas.MaxNestingDepth, true);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00029898 File Offset: 0x00027A98
		private void WritePropertyImplementation(ODataProperty property)
		{
			ODataJsonLightPropertySerializer odataJsonLightPropertySerializer = new ODataJsonLightPropertySerializer(this);
			odataJsonLightPropertySerializer.WriteTopLevelProperty(property);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000298B4 File Offset: 0x00027AB4
		private void WriteServiceDocumentImplementation(ODataWorkspace defaultWorkspace)
		{
			ODataJsonLightServiceDocumentSerializer odataJsonLightServiceDocumentSerializer = new ODataJsonLightServiceDocumentSerializer(this);
			odataJsonLightServiceDocumentSerializer.WriteServiceDocument(defaultWorkspace);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x000298D0 File Offset: 0x00027AD0
		private void WriteErrorImplementation(ODataError error, bool includeDebugInformation)
		{
			ODataJsonLightSerializer odataJsonLightSerializer = new ODataJsonLightSerializer(this);
			odataJsonLightSerializer.WriteTopLevelError(error, includeDebugInformation);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000298EC File Offset: 0x00027AEC
		private void WriteEntityReferenceLinksImplementation(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			ODataJsonLightEntityReferenceLinkSerializer odataJsonLightEntityReferenceLinkSerializer = new ODataJsonLightEntityReferenceLinkSerializer(this);
			odataJsonLightEntityReferenceLinkSerializer.WriteEntityReferenceLinks(links, entitySet, navigationProperty);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0002990C File Offset: 0x00027B0C
		private void WriteEntityReferenceLinkImplementation(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			ODataJsonLightEntityReferenceLinkSerializer odataJsonLightEntityReferenceLinkSerializer = new ODataJsonLightEntityReferenceLinkSerializer(this);
			odataJsonLightEntityReferenceLinkSerializer.WriteEntityReferenceLink(link, entitySet, navigationProperty);
		}

		// Token: 0x04000428 RID: 1064
		private readonly JsonLightMetadataLevel metadataLevel;

		// Token: 0x04000429 RID: 1065
		private JsonLightTypeNameOracle typeNameOracle;
	}
}
