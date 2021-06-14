using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000235 RID: 565
	internal sealed class ODataVerboseJsonInputContext : ODataInputContext
	{
		// Token: 0x06001202 RID: 4610 RVA: 0x00043DD0 File Offset: 0x00041FD0
		internal ODataVerboseJsonInputContext(ODataFormat format, TextReader reader, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageReaderSettings, version, readingResponse, synchronous, model, urlResolver)
		{
			try
			{
				ExceptionUtils.CheckArgumentNotNull<ODataFormat>(format, "format");
				ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
				this.textReader = reader;
				if (base.UseServerFormatBehavior)
				{
					this.jsonReader = new PropertyDeduplicatingJsonReader(this.textReader, messageReaderSettings.MessageQuotas.MaxNestingDepth);
				}
				else
				{
					this.jsonReader = new BufferingJsonReader(this.textReader, "error", messageReaderSettings.MessageQuotas.MaxNestingDepth, ODataFormat.VerboseJson);
				}
			}
			catch (Exception e)
			{
				if (ExceptionUtils.IsCatchableExceptionType(e) && reader != null)
				{
					reader.Dispose();
				}
				throw;
			}
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00043E80 File Offset: 0x00042080
		internal ODataVerboseJsonInputContext(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : this(format, ODataVerboseJsonInputContext.CreateTextReaderForMessageStreamConstructor(messageStream, encoding), messageReaderSettings, version, readingResponse, synchronous, model, urlResolver)
		{
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x00043EA7 File Offset: 0x000420A7
		internal BufferingJsonReader JsonReader
		{
			get
			{
				return this.jsonReader;
			}
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00043EAF File Offset: 0x000420AF
		internal override ODataReader CreateFeedReader(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			return this.CreateFeedReaderImplementation(entitySet, expectedBaseEntityType);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00043EDC File Offset: 0x000420DC
		internal override Task<ODataReader> CreateFeedReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataReader>(() => this.CreateFeedReaderImplementation(entitySet, expectedBaseEntityType));
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00043F15 File Offset: 0x00042115
		internal override ODataReader CreateEntryReader(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			return this.CreateEntryReaderImplementation(entitySet, expectedEntityType);
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00043F40 File Offset: 0x00042140
		internal override Task<ODataReader> CreateEntryReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataReader>(() => this.CreateEntryReaderImplementation(entitySet, expectedEntityType));
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00043F79 File Offset: 0x00042179
		internal override ODataCollectionReader CreateCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			return this.CreateCollectionReaderImplementation(expectedItemTypeReference);
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00043FA0 File Offset: 0x000421A0
		internal override Task<ODataCollectionReader> CreateCollectionReaderAsync(IEdmTypeReference expectedItemTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataCollectionReader>(() => this.CreateCollectionReaderImplementation(expectedItemTypeReference));
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00043FD2 File Offset: 0x000421D2
		internal override ODataParameterReader CreateParameterReader(IEdmFunctionImport functionImport)
		{
			ODataVerboseJsonInputContext.VerifyCanCreateParameterReader(functionImport);
			return this.CreateParameterReaderImplementation(functionImport);
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00043FFC File Offset: 0x000421FC
		internal override Task<ODataParameterReader> CreateParameterReaderAsync(IEdmFunctionImport functionImport)
		{
			ODataVerboseJsonInputContext.VerifyCanCreateParameterReader(functionImport);
			return TaskUtils.GetTaskForSynchronousOperation<ODataParameterReader>(() => this.CreateParameterReaderImplementation(functionImport));
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00044039 File Offset: 0x00042239
		internal override ODataWorkspace ReadServiceDocument()
		{
			return this.ReadServiceDocumentImplementation();
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00044041 File Offset: 0x00042241
		internal override Task<ODataWorkspace> ReadServiceDocumentAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataWorkspace>(new Func<ODataWorkspace>(this.ReadServiceDocumentImplementation));
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00044054 File Offset: 0x00042254
		internal override ODataProperty ReadProperty(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			return this.ReadPropertyImplementation(property, expectedPropertyTypeReference);
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00044080 File Offset: 0x00042280
		internal override Task<ODataProperty> ReadPropertyAsync(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataProperty>(() => this.ReadPropertyImplementation(property, expectedPropertyTypeReference));
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x000440B9 File Offset: 0x000422B9
		internal override ODataError ReadError()
		{
			return this.ReadErrorImplementation();
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x000440C9 File Offset: 0x000422C9
		internal override Task<ODataError> ReadErrorAsync()
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataError>(() => this.ReadErrorImplementation());
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x000440DC File Offset: 0x000422DC
		internal override ODataEntityReferenceLinks ReadEntityReferenceLinks(IEdmNavigationProperty navigationProperty)
		{
			return this.ReadEntityReferenceLinksImplementation();
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x000440EC File Offset: 0x000422EC
		internal override Task<ODataEntityReferenceLinks> ReadEntityReferenceLinksAsync(IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetTaskForSynchronousOperation<ODataEntityReferenceLinks>(() => this.ReadEntityReferenceLinksImplementation());
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x000440FF File Offset: 0x000422FF
		internal override ODataEntityReferenceLink ReadEntityReferenceLink(IEdmNavigationProperty navigationProperty)
		{
			return this.ReadEntityReferenceLinkImplementation();
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00044107 File Offset: 0x00042307
		internal override Task<ODataEntityReferenceLink> ReadEntityReferenceLinkAsync(IEdmNavigationProperty navigationProperty)
		{
			return TaskUtils.GetCompletedTask<ODataEntityReferenceLink>(this.ReadEntityReferenceLinkImplementation());
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00044114 File Offset: 0x00042314
		internal IEnumerable<ODataPayloadKind> DetectPayloadKind()
		{
			ODataVerboseJsonPayloadKindDetectionDeserializer odataVerboseJsonPayloadKindDetectionDeserializer = new ODataVerboseJsonPayloadKindDetectionDeserializer(this);
			return odataVerboseJsonPayloadKindDetectionDeserializer.DetectPayloadKind();
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00044130 File Offset: 0x00042330
		protected override void DisposeImplementation()
		{
			try
			{
				if (this.textReader != null)
				{
					this.textReader.Dispose();
				}
			}
			finally
			{
				this.textReader = null;
				this.jsonReader = null;
			}
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00044174 File Offset: 0x00042374
		private static TextReader CreateTextReaderForMessageStreamConstructor(Stream messageStream, Encoding encoding)
		{
			TextReader result;
			try
			{
				result = new StreamReader(messageStream, encoding);
			}
			catch (Exception e)
			{
				if (ExceptionUtils.IsCatchableExceptionType(e) && messageStream != null)
				{
					messageStream.Dispose();
				}
				throw;
			}
			return result;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x000441B0 File Offset: 0x000423B0
		private static void VerifyCanCreateParameterReader(IEdmFunctionImport functionImport)
		{
			if (functionImport == null)
			{
				throw new ArgumentNullException("functionImport", Strings.ODataJsonInputContext_FunctionImportCannotBeNullForCreateParameterReader("functionImport"));
			}
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x000441CA File Offset: 0x000423CA
		private ODataReader CreateFeedReaderImplementation(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			return new ODataVerboseJsonReader(this, entitySet, expectedBaseEntityType, true, null);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x000441D6 File Offset: 0x000423D6
		private ODataReader CreateEntryReaderImplementation(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			return new ODataVerboseJsonReader(this, entitySet, expectedEntityType, false, null);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x000441E2 File Offset: 0x000423E2
		private ODataCollectionReader CreateCollectionReaderImplementation(IEdmTypeReference expectedItemTypeReference)
		{
			return new ODataVerboseJsonCollectionReader(this, expectedItemTypeReference, null);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x000441EC File Offset: 0x000423EC
		private ODataParameterReader CreateParameterReaderImplementation(IEdmFunctionImport functionImport)
		{
			return new ODataVerboseJsonParameterReader(this, functionImport);
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x000441F8 File Offset: 0x000423F8
		private ODataProperty ReadPropertyImplementation(IEdmStructuralProperty Property, IEdmTypeReference expectedPropertyTypeReference)
		{
			ODataVerboseJsonPropertyAndValueDeserializer odataVerboseJsonPropertyAndValueDeserializer = new ODataVerboseJsonPropertyAndValueDeserializer(this);
			return odataVerboseJsonPropertyAndValueDeserializer.ReadTopLevelProperty(Property, expectedPropertyTypeReference);
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00044214 File Offset: 0x00042414
		private ODataWorkspace ReadServiceDocumentImplementation()
		{
			ODataVerboseJsonServiceDocumentDeserializer odataVerboseJsonServiceDocumentDeserializer = new ODataVerboseJsonServiceDocumentDeserializer(this);
			return odataVerboseJsonServiceDocumentDeserializer.ReadServiceDocument();
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00044230 File Offset: 0x00042430
		private ODataError ReadErrorImplementation()
		{
			ODataVerboseJsonErrorDeserializer odataVerboseJsonErrorDeserializer = new ODataVerboseJsonErrorDeserializer(this);
			return odataVerboseJsonErrorDeserializer.ReadTopLevelError();
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0004424C File Offset: 0x0004244C
		private ODataEntityReferenceLinks ReadEntityReferenceLinksImplementation()
		{
			ODataVerboseJsonEntityReferenceLinkDeserializer odataVerboseJsonEntityReferenceLinkDeserializer = new ODataVerboseJsonEntityReferenceLinkDeserializer(this);
			return odataVerboseJsonEntityReferenceLinkDeserializer.ReadEntityReferenceLinks();
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00044268 File Offset: 0x00042468
		private ODataEntityReferenceLink ReadEntityReferenceLinkImplementation()
		{
			ODataVerboseJsonEntityReferenceLinkDeserializer odataVerboseJsonEntityReferenceLinkDeserializer = new ODataVerboseJsonEntityReferenceLinkDeserializer(this);
			return odataVerboseJsonEntityReferenceLinkDeserializer.ReadEntityReferenceLink();
		}

		// Token: 0x0400068A RID: 1674
		private TextReader textReader;

		// Token: 0x0400068B RID: 1675
		private BufferingJsonReader jsonReader;
	}
}
