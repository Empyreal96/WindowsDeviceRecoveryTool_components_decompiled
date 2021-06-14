using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000192 RID: 402
	internal sealed class ODataJsonLightInputContext : ODataInputContext
	{
		// Token: 0x06000BBD RID: 3005 RVA: 0x00028CB8 File Offset: 0x00026EB8
		internal ODataJsonLightInputContext(ODataFormat format, Stream messageStream, MediaType contentType, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver, ODataJsonLightPayloadKindDetectionState payloadKindDetectionState) : this(format, ODataJsonLightInputContext.CreateTextReaderForMessageStreamConstructor(messageStream, encoding), contentType, messageReaderSettings, version, readingResponse, synchronous, model, urlResolver, payloadKindDetectionState)
		{
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00028CE4 File Offset: 0x00026EE4
		internal ODataJsonLightInputContext(ODataFormat format, TextReader reader, MediaType contentType, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver, ODataJsonLightPayloadKindDetectionState payloadKindDetectionState) : base(format, messageReaderSettings, version, readingResponse, synchronous, model, urlResolver)
		{
			try
			{
				ExceptionUtils.CheckArgumentNotNull<ODataFormat>(format, "format");
				ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			}
			catch (ArgumentNullException)
			{
				reader.Dispose();
				throw;
			}
			try
			{
				this.textReader = reader;
				if (contentType.HasStreamingSetToTrue())
				{
					this.jsonReader = new BufferingJsonReader(this.textReader, "odata.error", messageReaderSettings.MessageQuotas.MaxNestingDepth, ODataFormat.Json);
				}
				else
				{
					this.jsonReader = new ReorderingJsonReader(this.textReader, messageReaderSettings.MessageQuotas.MaxNestingDepth);
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
			this.payloadKindDetectionState = payloadKindDetectionState;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00028DB8 File Offset: 0x00026FB8
		internal BufferingJsonReader JsonReader
		{
			get
			{
				return this.jsonReader;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00028DC0 File Offset: 0x00026FC0
		internal ODataJsonLightPayloadKindDetectionState PayloadKindDetectionState
		{
			get
			{
				return this.payloadKindDetectionState;
			}
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00028DC8 File Offset: 0x00026FC8
		internal override ODataReader CreateFeedReader(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			this.VerifyCanCreateODataReader(entitySet, expectedBaseEntityType);
			return this.CreateFeedReaderImplementation(entitySet, expectedBaseEntityType);
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00028DFC File Offset: 0x00026FFC
		internal override Task<ODataReader> CreateFeedReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			this.VerifyCanCreateODataReader(entitySet, expectedBaseEntityType);
			return TaskUtils.GetTaskForSynchronousOperation<ODataReader>(() => this.CreateFeedReaderImplementation(entitySet, expectedBaseEntityType));
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00028E47 File Offset: 0x00027047
		internal override ODataReader CreateEntryReader(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			this.VerifyCanCreateODataReader(entitySet, expectedEntityType);
			return this.CreateEntryReaderImplementation(entitySet, expectedEntityType);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00028E7C File Offset: 0x0002707C
		internal override Task<ODataReader> CreateEntryReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			this.VerifyCanCreateODataReader(entitySet, expectedEntityType);
			return TaskUtils.GetTaskForSynchronousOperation<ODataReader>(() => this.CreateEntryReaderImplementation(entitySet, expectedEntityType));
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00028EC7 File Offset: 0x000270C7
		internal override ODataCollectionReader CreateCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			this.VerifyCanCreateCollectionReader(expectedItemTypeReference);
			return this.CreateCollectionReaderImplementation(expectedItemTypeReference);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00028EF4 File Offset: 0x000270F4
		internal override Task<ODataCollectionReader> CreateCollectionReaderAsync(IEdmTypeReference expectedItemTypeReference)
		{
			this.VerifyCanCreateCollectionReader(expectedItemTypeReference);
			return TaskUtils.GetTaskForSynchronousOperation<ODataCollectionReader>(() => this.CreateCollectionReaderImplementation(expectedItemTypeReference));
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00028F32 File Offset: 0x00027132
		internal override ODataParameterReader CreateParameterReader(IEdmFunctionImport functionImport)
		{
			this.VerifyCanCreateParameterReader(functionImport);
			return this.CreateParameterReaderImplementation(functionImport);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00028F60 File Offset: 0x00027160
		internal override Task<ODataParameterReader> CreateParameterReaderAsync(IEdmFunctionImport functionImport)
		{
			this.VerifyCanCreateParameterReader(functionImport);
			return TaskUtils.GetTaskForSynchronousOperation<ODataParameterReader>(() => this.CreateParameterReaderImplementation(functionImport));
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00028FA0 File Offset: 0x000271A0
		internal override ODataWorkspace ReadServiceDocument()
		{
			ODataJsonLightServiceDocumentDeserializer odataJsonLightServiceDocumentDeserializer = new ODataJsonLightServiceDocumentDeserializer(this);
			return odataJsonLightServiceDocumentDeserializer.ReadServiceDocument();
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00028FBC File Offset: 0x000271BC
		internal override Task<ODataWorkspace> ReadServiceDocumentAsync()
		{
			ODataJsonLightServiceDocumentDeserializer odataJsonLightServiceDocumentDeserializer = new ODataJsonLightServiceDocumentDeserializer(this);
			return odataJsonLightServiceDocumentDeserializer.ReadServiceDocumentAsync();
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00028FD8 File Offset: 0x000271D8
		internal override ODataProperty ReadProperty(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			this.VerifyCanReadProperty();
			ODataJsonLightPropertyAndValueDeserializer odataJsonLightPropertyAndValueDeserializer = new ODataJsonLightPropertyAndValueDeserializer(this);
			return odataJsonLightPropertyAndValueDeserializer.ReadTopLevelProperty(expectedPropertyTypeReference);
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00028FFC File Offset: 0x000271FC
		internal override Task<ODataProperty> ReadPropertyAsync(IEdmStructuralProperty property, IEdmTypeReference expectedPropertyTypeReference)
		{
			this.VerifyCanReadProperty();
			ODataJsonLightPropertyAndValueDeserializer odataJsonLightPropertyAndValueDeserializer = new ODataJsonLightPropertyAndValueDeserializer(this);
			return odataJsonLightPropertyAndValueDeserializer.ReadTopLevelPropertyAsync(expectedPropertyTypeReference);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00029020 File Offset: 0x00027220
		internal override ODataError ReadError()
		{
			ODataJsonLightErrorDeserializer odataJsonLightErrorDeserializer = new ODataJsonLightErrorDeserializer(this);
			return odataJsonLightErrorDeserializer.ReadTopLevelError();
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0002903C File Offset: 0x0002723C
		internal override Task<ODataError> ReadErrorAsync()
		{
			ODataJsonLightErrorDeserializer odataJsonLightErrorDeserializer = new ODataJsonLightErrorDeserializer(this);
			return odataJsonLightErrorDeserializer.ReadTopLevelErrorAsync();
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00029058 File Offset: 0x00027258
		internal override ODataEntityReferenceLinks ReadEntityReferenceLinks(IEdmNavigationProperty navigationProperty)
		{
			ODataJsonLightEntityReferenceLinkDeserializer odataJsonLightEntityReferenceLinkDeserializer = new ODataJsonLightEntityReferenceLinkDeserializer(this);
			return odataJsonLightEntityReferenceLinkDeserializer.ReadEntityReferenceLinks(navigationProperty);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00029074 File Offset: 0x00027274
		internal override Task<ODataEntityReferenceLinks> ReadEntityReferenceLinksAsync(IEdmNavigationProperty navigationProperty)
		{
			ODataJsonLightEntityReferenceLinkDeserializer odataJsonLightEntityReferenceLinkDeserializer = new ODataJsonLightEntityReferenceLinkDeserializer(this);
			return odataJsonLightEntityReferenceLinkDeserializer.ReadEntityReferenceLinksAsync(navigationProperty);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00029090 File Offset: 0x00027290
		internal override ODataEntityReferenceLink ReadEntityReferenceLink(IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanReadEntityReferenceLink(navigationProperty);
			ODataJsonLightEntityReferenceLinkDeserializer odataJsonLightEntityReferenceLinkDeserializer = new ODataJsonLightEntityReferenceLinkDeserializer(this);
			return odataJsonLightEntityReferenceLinkDeserializer.ReadEntityReferenceLink(navigationProperty);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000290B4 File Offset: 0x000272B4
		internal override Task<ODataEntityReferenceLink> ReadEntityReferenceLinkAsync(IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanReadEntityReferenceLink(navigationProperty);
			ODataJsonLightEntityReferenceLinkDeserializer odataJsonLightEntityReferenceLinkDeserializer = new ODataJsonLightEntityReferenceLinkDeserializer(this);
			return odataJsonLightEntityReferenceLinkDeserializer.ReadEntityReferenceLinkAsync(navigationProperty);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x000290D8 File Offset: 0x000272D8
		internal IEnumerable<ODataPayloadKind> DetectPayloadKind(ODataPayloadKindDetectionInfo detectionInfo)
		{
			this.VerifyCanDetectPayloadKind();
			ODataJsonLightPayloadKindDetectionDeserializer odataJsonLightPayloadKindDetectionDeserializer = new ODataJsonLightPayloadKindDetectionDeserializer(this);
			return odataJsonLightPayloadKindDetectionDeserializer.DetectPayloadKind(detectionInfo);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000290FC File Offset: 0x000272FC
		internal Task<IEnumerable<ODataPayloadKind>> DetectPayloadKindAsync(ODataPayloadKindDetectionInfo detectionInfo)
		{
			this.VerifyCanDetectPayloadKind();
			ODataJsonLightPayloadKindDetectionDeserializer odataJsonLightPayloadKindDetectionDeserializer = new ODataJsonLightPayloadKindDetectionDeserializer(this);
			return odataJsonLightPayloadKindDetectionDeserializer.DetectPayloadKindAsync(detectionInfo);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00029120 File Offset: 0x00027320
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

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00029164 File Offset: 0x00027364
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

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000291A0 File Offset: 0x000273A0
		private void VerifyCanCreateParameterReader(IEdmFunctionImport functionImport)
		{
			this.VerifyUserModel();
			if (functionImport == null)
			{
				throw new ArgumentNullException("functionImport", Strings.ODataJsonLightInputContext_FunctionImportCannotBeNullForCreateParameterReader("functionImport"));
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000291C0 File Offset: 0x000273C0
		private void VerifyCanCreateODataReader(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			if (!base.ReadingResponse)
			{
				this.VerifyUserModel();
				if (entitySet == null)
				{
					throw new ODataException(Strings.ODataJsonLightInputContext_NoEntitySetForRequest);
				}
			}
			IEdmEntityType elementType = base.EdmTypeResolver.GetElementType(entitySet);
			if (entitySet != null && entityType != null && !entityType.IsOrInheritsFrom(elementType))
			{
				throw new ODataException(Strings.ODataJsonLightInputContext_EntityTypeMustBeCompatibleWithEntitySetBaseType(entityType.FullName(), elementType.FullName(), entitySet.FullName()));
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00029222 File Offset: 0x00027422
		private void VerifyCanCreateCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			if (!base.ReadingResponse)
			{
				this.VerifyUserModel();
				if (expectedItemTypeReference == null)
				{
					throw new ODataException(Strings.ODataJsonLightInputContext_ItemTypeRequiredForCollectionReaderInRequests);
				}
			}
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00029240 File Offset: 0x00027440
		private void VerifyCanReadEntityReferenceLink(IEdmNavigationProperty navigationProperty)
		{
			if (!base.ReadingResponse)
			{
				this.VerifyUserModel();
				if (navigationProperty == null)
				{
					throw new ODataException(Strings.ODataJsonLightInputContext_NavigationPropertyRequiredForReadEntityReferenceLinkInRequests);
				}
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0002925E File Offset: 0x0002745E
		private void VerifyCanReadProperty()
		{
			if (!base.ReadingResponse)
			{
				this.VerifyUserModel();
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0002926E File Offset: 0x0002746E
		private void VerifyCanDetectPayloadKind()
		{
			if (!base.ReadingResponse)
			{
				throw new ODataException(Strings.ODataJsonLightInputContext_PayloadKindDetectionForRequest);
			}
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00029283 File Offset: 0x00027483
		private void VerifyUserModel()
		{
			if (!base.Model.IsUserModel())
			{
				throw new ODataException(Strings.ODataJsonLightInputContext_ModelRequiredForReading);
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0002929D File Offset: 0x0002749D
		private ODataReader CreateFeedReaderImplementation(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			return new ODataJsonLightReader(this, entitySet, expectedBaseEntityType, true, null);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x000292A9 File Offset: 0x000274A9
		private ODataReader CreateEntryReaderImplementation(IEdmEntitySet entitySet, IEdmEntityType expectedEntityType)
		{
			return new ODataJsonLightReader(this, entitySet, expectedEntityType, false, null);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x000292B5 File Offset: 0x000274B5
		private ODataCollectionReader CreateCollectionReaderImplementation(IEdmTypeReference expectedItemTypeReference)
		{
			return new ODataJsonLightCollectionReader(this, expectedItemTypeReference, null);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x000292BF File Offset: 0x000274BF
		private ODataParameterReader CreateParameterReaderImplementation(IEdmFunctionImport functionImport)
		{
			return new ODataJsonLightParameterReader(this, functionImport);
		}

		// Token: 0x04000425 RID: 1061
		private readonly ODataJsonLightPayloadKindDetectionState payloadKindDetectionState;

		// Token: 0x04000426 RID: 1062
		private TextReader textReader;

		// Token: 0x04000427 RID: 1063
		private BufferingJsonReader jsonReader;
	}
}
