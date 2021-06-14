using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001C6 RID: 454
	internal sealed class ODataVerboseJsonOutputContext : ODataJsonOutputContextBase
	{
		// Token: 0x06000E01 RID: 3585 RVA: 0x000314B1 File Offset: 0x0002F6B1
		internal ODataVerboseJsonOutputContext(ODataFormat format, TextWriter textWriter, ODataMessageWriterSettings messageWriterSettings, IEdmModel model) : base(format, textWriter, messageWriterSettings, model)
		{
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000314CC File Offset: 0x0002F6CC
		internal ODataVerboseJsonOutputContext(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageStream, encoding, messageWriterSettings, writingResponse, synchronous, model, urlResolver)
		{
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x000314F7 File Offset: 0x0002F6F7
		internal AtomAndVerboseJsonTypeNameOracle TypeNameOracle
		{
			get
			{
				return this.typeNameOracle;
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x000314FF File Offset: 0x0002F6FF
		internal override void WriteInStreamError(ODataError error, bool includeDebugInformation)
		{
			this.WriteInStreamErrorImplementation(error, includeDebugInformation);
			base.Flush();
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0003153C File Offset: 0x0002F73C
		internal override Task WriteInStreamErrorAsync(ODataError error, bool includeDebugInformation)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteInStreamErrorImplementation(error, includeDebugInformation);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00031575 File Offset: 0x0002F775
		internal override ODataWriter CreateODataFeedWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return this.CreateODataFeedWriterImplementation(entitySet, entityType);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x000315A0 File Offset: 0x0002F7A0
		internal override Task<ODataWriter> CreateODataFeedWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataWriter>(() => this.CreateODataFeedWriterImplementation(entitySet, entityType));
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000315D9 File Offset: 0x0002F7D9
		internal override ODataWriter CreateODataEntryWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return this.CreateODataEntryWriterImplementation(entitySet, entityType);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00031604 File Offset: 0x0002F804
		internal override Task<ODataWriter> CreateODataEntryWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataWriter>(() => this.CreateODataEntryWriterImplementation(entitySet, entityType));
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0003163D File Offset: 0x0002F83D
		internal override ODataCollectionWriter CreateODataCollectionWriter(IEdmTypeReference itemTypeReference)
		{
			return this.CreateODataCollectionWriterImplementation(itemTypeReference);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00031664 File Offset: 0x0002F864
		internal override Task<ODataCollectionWriter> CreateODataCollectionWriterAsync(IEdmTypeReference itemTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataCollectionWriter>(() => this.CreateODataCollectionWriterImplementation(itemTypeReference));
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00031696 File Offset: 0x0002F896
		internal override ODataParameterWriter CreateODataParameterWriter(IEdmFunctionImport functionImport)
		{
			return this.CreateODataParameterWriterImplementation(functionImport);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x000316BC File Offset: 0x0002F8BC
		internal override Task<ODataParameterWriter> CreateODataParameterWriterAsync(IEdmFunctionImport functionImport)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataParameterWriter>(() => this.CreateODataParameterWriterImplementation(functionImport));
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000316EE File Offset: 0x0002F8EE
		internal override void WriteServiceDocument(ODataWorkspace defaultWorkspace)
		{
			this.WriteServiceDocumentImplementation(defaultWorkspace);
			base.Flush();
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00031724 File Offset: 0x0002F924
		internal override Task WriteServiceDocumentAsync(ODataWorkspace defaultWorkspace)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteServiceDocumentImplementation(defaultWorkspace);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00031756 File Offset: 0x0002F956
		internal override void WriteProperty(ODataProperty property)
		{
			this.WritePropertyImplementation(property);
			base.Flush();
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0003178C File Offset: 0x0002F98C
		internal override Task WritePropertyAsync(ODataProperty property)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WritePropertyImplementation(property);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x000317BE File Offset: 0x0002F9BE
		internal override void WriteError(ODataError error, bool includeDebugInformation)
		{
			this.WriteErrorImplementation(error, includeDebugInformation);
			base.Flush();
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000317FC File Offset: 0x0002F9FC
		internal override Task WriteErrorAsync(ODataError error, bool includeDebugInformation)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteErrorImplementation(error, includeDebugInformation);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00031835 File Offset: 0x0002FA35
		internal override void WriteEntityReferenceLinks(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.WriteEntityReferenceLinksImplementation(links);
			base.Flush();
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0003186C File Offset: 0x0002FA6C
		internal override Task WriteEntityReferenceLinksAsync(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteEntityReferenceLinksImplementation(links);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0003189E File Offset: 0x0002FA9E
		internal override void WriteEntityReferenceLink(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.WriteEntityReferenceLinkImplementation(link);
			base.Flush();
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x000318D4 File Offset: 0x0002FAD4
		internal override Task WriteEntityReferenceLinkAsync(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetTaskForSynchronousOperationReturningTask(delegate()
			{
				this.WriteEntityReferenceLinkImplementation(link);
				return this.FlushAsync();
			});
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00031908 File Offset: 0x0002FB08
		private ODataWriter CreateODataFeedWriterImplementation(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			ODataVerboseJsonWriter odataVerboseJsonWriter = new ODataVerboseJsonWriter(this, entitySet, entityType, true);
			this.outputInStreamErrorListener = odataVerboseJsonWriter;
			return odataVerboseJsonWriter;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00031928 File Offset: 0x0002FB28
		private ODataWriter CreateODataEntryWriterImplementation(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			ODataVerboseJsonWriter odataVerboseJsonWriter = new ODataVerboseJsonWriter(this, entitySet, entityType, false);
			this.outputInStreamErrorListener = odataVerboseJsonWriter;
			return odataVerboseJsonWriter;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00031948 File Offset: 0x0002FB48
		private ODataCollectionWriter CreateODataCollectionWriterImplementation(IEdmTypeReference itemTypeReference)
		{
			ODataVerboseJsonCollectionWriter odataVerboseJsonCollectionWriter = new ODataVerboseJsonCollectionWriter(this, itemTypeReference);
			this.outputInStreamErrorListener = odataVerboseJsonCollectionWriter;
			return odataVerboseJsonCollectionWriter;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00031968 File Offset: 0x0002FB68
		private ODataParameterWriter CreateODataParameterWriterImplementation(IEdmFunctionImport functionImport)
		{
			ODataVerboseJsonParameterWriter odataVerboseJsonParameterWriter = new ODataVerboseJsonParameterWriter(this, functionImport);
			this.outputInStreamErrorListener = odataVerboseJsonParameterWriter;
			return odataVerboseJsonParameterWriter;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00031985 File Offset: 0x0002FB85
		private void WriteInStreamErrorImplementation(ODataError error, bool includeDebugInformation)
		{
			if (this.outputInStreamErrorListener != null)
			{
				this.outputInStreamErrorListener.OnInStreamError();
			}
			ODataJsonWriterUtils.WriteError(base.JsonWriter, null, error, includeDebugInformation, base.MessageWriterSettings.MessageQuotas.MaxNestingDepth, false);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000319BC File Offset: 0x0002FBBC
		private void WritePropertyImplementation(ODataProperty property)
		{
			ODataVerboseJsonPropertyAndValueSerializer odataVerboseJsonPropertyAndValueSerializer = new ODataVerboseJsonPropertyAndValueSerializer(this);
			odataVerboseJsonPropertyAndValueSerializer.WriteTopLevelProperty(property);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x000319D8 File Offset: 0x0002FBD8
		private void WriteServiceDocumentImplementation(ODataWorkspace defaultWorkspace)
		{
			ODataVerboseJsonServiceDocumentSerializer odataVerboseJsonServiceDocumentSerializer = new ODataVerboseJsonServiceDocumentSerializer(this);
			odataVerboseJsonServiceDocumentSerializer.WriteServiceDocument(defaultWorkspace);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x000319F4 File Offset: 0x0002FBF4
		private void WriteErrorImplementation(ODataError error, bool includeDebugInformation)
		{
			ODataVerboseJsonSerializer odataVerboseJsonSerializer = new ODataVerboseJsonSerializer(this);
			odataVerboseJsonSerializer.WriteTopLevelError(error, includeDebugInformation);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00031A10 File Offset: 0x0002FC10
		private void WriteEntityReferenceLinksImplementation(ODataEntityReferenceLinks links)
		{
			ODataVerboseJsonEntityReferenceLinkSerializer odataVerboseJsonEntityReferenceLinkSerializer = new ODataVerboseJsonEntityReferenceLinkSerializer(this);
			odataVerboseJsonEntityReferenceLinkSerializer.WriteEntityReferenceLinks(links);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00031A2C File Offset: 0x0002FC2C
		private void WriteEntityReferenceLinkImplementation(ODataEntityReferenceLink link)
		{
			ODataVerboseJsonEntityReferenceLinkSerializer odataVerboseJsonEntityReferenceLinkSerializer = new ODataVerboseJsonEntityReferenceLinkSerializer(this);
			odataVerboseJsonEntityReferenceLinkSerializer.WriteEntityReferenceLink(link);
		}

		// Token: 0x040004AC RID: 1196
		private readonly AtomAndVerboseJsonTypeNameOracle typeNameOracle = new AtomAndVerboseJsonTypeNameOracle();
	}
}
