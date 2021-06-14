using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000226 RID: 550
	internal sealed class ODataAtomInputContext : ODataInputContext
	{
		// Token: 0x06001135 RID: 4405 RVA: 0x000403D8 File Offset: 0x0003E5D8
		internal ODataAtomInputContext(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageReaderSettings, version, readingResponse, synchronous, model, urlResolver)
		{
			try
			{
				ExceptionUtils.CheckArgumentNotNull<ODataFormat>(format, "format");
				ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
				this.baseXmlReader = ODataAtomReaderUtils.CreateXmlReader(messageStream, encoding, messageReaderSettings);
				this.xmlReader = new BufferingXmlReader(this.baseXmlReader, null, messageReaderSettings.BaseUri, base.UseServerFormatBehavior && base.Version < ODataVersion.V3, messageReaderSettings.MessageQuotas.MaxNestingDepth, messageReaderSettings.ReaderBehavior.ODataNamespace);
			}
			catch (Exception e)
			{
				if (ExceptionUtils.IsCatchableExceptionType(e) && messageStream != null)
				{
					messageStream.Dispose();
				}
				throw;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001136 RID: 4406 RVA: 0x0004048C File Offset: 0x0003E68C
		internal BufferingXmlReader XmlReader
		{
			get
			{
				return this.xmlReader;
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00040494 File Offset: 0x0003E694
		internal override ODataReader CreateFeedReader(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			return this.CreateFeedReaderImplementation(entitySet, expectedBaseEntityType);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x000404C0 File Offset: 0x0003E6C0
		internal override Task<ODataReader> CreateFeedReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataReader>(() => this.CreateFeedReaderImplementation(entitySet, expectedBaseEntityType));
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000404F9 File Offset: 0x0003E6F9
		internal override ODataReader CreateEntryReader(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			return this.CreateEntryReaderImplementation(entitySet, expectedEntityType);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00040524 File Offset: 0x0003E724
		internal override Task<ODataReader> CreateEntryReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataReader>(() => this.CreateEntryReaderImplementation(entitySet, expectedEntityType));
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0004055D File Offset: 0x0003E75D
		internal override ODataCollectionReader CreateCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			return this.CreateCollectionReaderImplementation(expectedItemTypeReference);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00040584 File Offset: 0x0003E784
		internal override Task<ODataCollectionReader> CreateCollectionReaderAsync(IEdmTypeReference expectedItemTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataCollectionReader>(() => this.CreateCollectionReaderImplementation(expectedItemTypeReference));
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x000405B6 File Offset: 0x0003E7B6
		internal override ODataWorkspace ReadServiceDocument()
		{
			return this.ReadServiceDocumentImplementation();
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x000405C6 File Offset: 0x0003E7C6
		internal override Task<ODataWorkspace> ReadServiceDocumentAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataWorkspace>(() => this.ReadServiceDocumentImplementation());
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x000405D9 File Offset: 0x0003E7D9
		internal override ODataProperty ReadProperty(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			return this.ReadPropertyImplementation(property, expectedPropertyTypeReference);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00040604 File Offset: 0x0003E804
		internal override Task<ODataProperty> ReadPropertyAsync(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataProperty>(() => this.ReadPropertyImplementation(property, expectedPropertyTypeReference));
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0004063D File Offset: 0x0003E83D
		internal override ODataError ReadError()
		{
			return this.ReadErrorImplementation();
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0004064D File Offset: 0x0003E84D
		internal override Task<ODataError> ReadErrorAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataError>(() => this.ReadErrorImplementation());
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00040660 File Offset: 0x0003E860
		internal override ODataEntityReferenceLinks ReadEntityReferenceLinks(IEdmNavigationProperty navigationProperty)
		{
			return this.ReadEntityReferenceLinksImplementation();
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00040668 File Offset: 0x0003E868
		internal override Task<ODataEntityReferenceLinks> ReadEntityReferenceLinksAsync(IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetCompletedTask<ODataEntityReferenceLinks>(this.ReadEntityReferenceLinksImplementation());
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00040675 File Offset: 0x0003E875
		internal override ODataEntityReferenceLink ReadEntityReferenceLink(IEdmNavigationProperty navigationProperty)
		{
			return this.ReadEntityReferenceLinkImplementation();
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0004067D File Offset: 0x0003E87D
		internal override Task<ODataEntityReferenceLink> ReadEntityReferenceLinkAsync(IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetCompletedTask<ODataEntityReferenceLink>(this.ReadEntityReferenceLinkImplementation());
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0004068C File Offset: 0x0003E88C
		internal IEnumerable<ODataPayloadKind> DetectPayloadKind(ODataPayloadKindDetectionInfo detectionInfo)
		{
			ODataAtomPayloadKindDetectionDeserializer odataAtomPayloadKindDetectionDeserializer = new ODataAtomPayloadKindDetectionDeserializer(this);
			return odataAtomPayloadKindDetectionDeserializer.DetectPayloadKind(detectionInfo);
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x000406A7 File Offset: 0x0003E8A7
		internal void InitializeReaderCustomization()
		{
			this.xmlCustomizationReaders = new Stack<BufferingXmlReader>();
			this.xmlCustomizationReaders.Push(this.xmlReader);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x000406C8 File Offset: 0x0003E8C8
		internal void PushCustomReader(XmlReader customXmlReader, Uri xmlBaseUri)
		{
			if (!object.ReferenceEquals(this.xmlReader, customXmlReader))
			{
				BufferingXmlReader item = new BufferingXmlReader(customXmlReader, xmlBaseUri, base.MessageReaderSettings.BaseUri, false, base.MessageReaderSettings.MessageQuotas.MaxNestingDepth, base.MessageReaderSettings.ReaderBehavior.ODataNamespace);
				this.xmlCustomizationReaders.Push(item);
				this.xmlReader = item;
				return;
			}
			this.xmlCustomizationReaders.Push(this.xmlReader);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0004073C File Offset: 0x0003E93C
		internal BufferingXmlReader PopCustomReader()
		{
			BufferingXmlReader result = this.xmlCustomizationReaders.Pop();
			this.xmlReader = this.xmlCustomizationReaders.Peek();
			return result;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00040768 File Offset: 0x0003E968
		protected override void DisposeImplementation()
		{
			try
			{
				if (this.baseXmlReader != null)
				{
					((IDisposable)this.baseXmlReader).Dispose();
				}
			}
			finally
			{
				this.baseXmlReader = null;
				this.xmlReader = null;
			}
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x000407AC File Offset: 0x0003E9AC
		private ODataReader CreateFeedReaderImplementation(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			return new ODataAtomReader(this, entitySet, expectedBaseEntityType, true);
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x000407B7 File Offset: 0x0003E9B7
		private ODataReader CreateEntryReaderImplementation(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			return new ODataAtomReader(this, entitySet, expectedEntityType, false);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x000407C2 File Offset: 0x0003E9C2
		private ODataCollectionReader CreateCollectionReaderImplementation(IEdmTypeReference expectedItemTypeReference)
		{
			return new ODataAtomCollectionReader(this, expectedItemTypeReference);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x000407CC File Offset: 0x0003E9CC
		private ODataProperty ReadPropertyImplementation(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			ODataAtomPropertyAndValueDeserializer odataAtomPropertyAndValueDeserializer = new ODataAtomPropertyAndValueDeserializer(this);
			return odataAtomPropertyAndValueDeserializer.ReadTopLevelProperty(property, expectedPropertyTypeReference);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x000407E8 File Offset: 0x0003E9E8
		private ODataWorkspace ReadServiceDocumentImplementation()
		{
			ODataAtomServiceDocumentDeserializer odataAtomServiceDocumentDeserializer = new ODataAtomServiceDocumentDeserializer(this);
			return odataAtomServiceDocumentDeserializer.ReadServiceDocument();
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00040804 File Offset: 0x0003EA04
		private ODataError ReadErrorImplementation()
		{
			ODataAtomErrorDeserializer odataAtomErrorDeserializer = new ODataAtomErrorDeserializer(this);
			return odataAtomErrorDeserializer.ReadTopLevelError();
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00040820 File Offset: 0x0003EA20
		private ODataEntityReferenceLinks ReadEntityReferenceLinksImplementation()
		{
			ODataAtomEntityReferenceLinkDeserializer odataAtomEntityReferenceLinkDeserializer = new ODataAtomEntityReferenceLinkDeserializer(this);
			return odataAtomEntityReferenceLinkDeserializer.ReadEntityReferenceLinks();
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x0004083C File Offset: 0x0003EA3C
		private ODataEntityReferenceLink ReadEntityReferenceLinkImplementation()
		{
			ODataAtomEntityReferenceLinkDeserializer odataAtomEntityReferenceLinkDeserializer = new ODataAtomEntityReferenceLinkDeserializer(this);
			return odataAtomEntityReferenceLinkDeserializer.ReadEntityReferenceLink();
		}

		// Token: 0x04000659 RID: 1625
		private XmlReader baseXmlReader;

		// Token: 0x0400065A RID: 1626
		private BufferingXmlReader xmlReader;

		// Token: 0x0400065B RID: 1627
		private Stack<BufferingXmlReader> xmlCustomizationReaders;
	}
}
