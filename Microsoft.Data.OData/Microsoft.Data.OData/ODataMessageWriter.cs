using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x0200028A RID: 650
	public sealed class ODataMessageWriter : IDisposable
	{
		// Token: 0x0600159F RID: 5535 RVA: 0x0004F2A4 File Offset: 0x0004D4A4
		public ODataMessageWriter(IODataRequestMessage requestMessage) : this(requestMessage, null)
		{
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0004F2AE File Offset: 0x0004D4AE
		public ODataMessageWriter(IODataRequestMessage requestMessage, ODataMessageWriterSettings settings) : this(requestMessage, settings, null)
		{
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0004F2BC File Offset: 0x0004D4BC
		public ODataMessageWriter(IODataRequestMessage requestMessage, ODataMessageWriterSettings settings, IEdmModel model)
		{
			this.writerPayloadKind = ODataPayloadKind.Unsupported;
			base..ctor();
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessage>(requestMessage, "requestMessage");
			this.settings = ((settings == null) ? new ODataMessageWriterSettings() : new ODataMessageWriterSettings(settings));
			this.writingResponse = false;
			this.urlResolver = (requestMessage as IODataUrlResolver);
			this.model = (model ?? EdmCoreModel.Instance);
			WriterValidationUtils.ValidateMessageWriterSettings(this.settings, this.writingResponse);
			this.message = new ODataRequestMessage(requestMessage, true, this.settings.DisableMessageStreamDisposal, -1L);
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0004F349 File Offset: 0x0004D549
		public ODataMessageWriter(IODataResponseMessage responseMessage) : this(responseMessage, null)
		{
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0004F353 File Offset: 0x0004D553
		public ODataMessageWriter(IODataResponseMessage responseMessage, ODataMessageWriterSettings settings) : this(responseMessage, settings, null)
		{
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0004F360 File Offset: 0x0004D560
		public ODataMessageWriter(IODataResponseMessage responseMessage, ODataMessageWriterSettings settings, IEdmModel model)
		{
			this.writerPayloadKind = ODataPayloadKind.Unsupported;
			base..ctor();
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessage>(responseMessage, "responseMessage");
			this.settings = ((settings == null) ? new ODataMessageWriterSettings() : new ODataMessageWriterSettings(settings));
			this.writingResponse = true;
			this.urlResolver = (responseMessage as IODataUrlResolver);
			this.model = (model ?? EdmCoreModel.Instance);
			WriterValidationUtils.ValidateMessageWriterSettings(this.settings, this.writingResponse);
			this.message = new ODataResponseMessage(responseMessage, true, this.settings.DisableMessageStreamDisposal, -1L);
			string annotationFilter = responseMessage.PreferenceAppliedHeader().AnnotationFilter;
			if (!string.IsNullOrEmpty(annotationFilter))
			{
				this.settings.ShouldIncludeAnnotation = ODataUtils.CreateAnnotationFilter(annotationFilter);
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x0004F412 File Offset: 0x0004D612
		internal ODataMessageWriterSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x0004F41C File Offset: 0x0004D61C
		private MediaTypeResolver MediaTypeResolver
		{
			get
			{
				if (this.mediaTypeResolver == null)
				{
					this.mediaTypeResolver = MediaTypeResolver.GetWriterMediaTypeResolver(this.settings.Version.Value);
				}
				return this.mediaTypeResolver;
			}
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x0004F455 File Offset: 0x0004D655
		public ODataWriter CreateODataFeedWriter()
		{
			return this.CreateODataFeedWriter(null, null);
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0004F45F File Offset: 0x0004D65F
		public ODataWriter CreateODataFeedWriter(IEdmEntitySet entitySet)
		{
			return this.CreateODataFeedWriter(entitySet, null);
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0004F488 File Offset: 0x0004D688
		public ODataWriter CreateODataFeedWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			this.VerifyCanCreateODataFeedWriter();
			return this.WriteToOutput<ODataWriter>(ODataPayloadKind.Feed, null, (ODataOutputContext context) => context.CreateODataFeedWriter(entitySet, entityType));
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0004F4C3 File Offset: 0x0004D6C3
		public Task<ODataWriter> CreateODataFeedWriterAsync()
		{
			return this.CreateODataFeedWriterAsync(null, null);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0004F4CD File Offset: 0x0004D6CD
		public Task<ODataWriter> CreateODataFeedWriterAsync(IEdmEntitySet entitySet)
		{
			return this.CreateODataFeedWriterAsync(entitySet, null);
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0004F4F4 File Offset: 0x0004D6F4
		public Task<ODataWriter> CreateODataFeedWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			this.VerifyCanCreateODataFeedWriter();
			return this.WriteToOutputAsync<ODataWriter>(ODataPayloadKind.Feed, null, (ODataOutputContext context) => context.CreateODataFeedWriterAsync(entitySet, entityType));
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0004F52F File Offset: 0x0004D72F
		public ODataWriter CreateODataEntryWriter()
		{
			return this.CreateODataEntryWriter(null, null);
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0004F539 File Offset: 0x0004D739
		public ODataWriter CreateODataEntryWriter(IEdmEntitySet entitySet)
		{
			return this.CreateODataEntryWriter(entitySet, null);
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0004F560 File Offset: 0x0004D760
		public ODataWriter CreateODataEntryWriter(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			this.VerifyCanCreateODataEntryWriter();
			return this.WriteToOutput<ODataWriter>(ODataPayloadKind.Entry, null, (ODataOutputContext context) => context.CreateODataEntryWriter(entitySet, entityType));
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0004F59B File Offset: 0x0004D79B
		public Task<ODataWriter> CreateODataEntryWriterAsync()
		{
			return this.CreateODataEntryWriterAsync(null, null);
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0004F5A5 File Offset: 0x0004D7A5
		public Task<ODataWriter> CreateODataEntryWriterAsync(IEdmEntitySet entitySet)
		{
			return this.CreateODataEntryWriterAsync(entitySet, null);
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0004F5CC File Offset: 0x0004D7CC
		public Task<ODataWriter> CreateODataEntryWriterAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			this.VerifyCanCreateODataEntryWriter();
			return this.WriteToOutputAsync<ODataWriter>(ODataPayloadKind.Entry, null, (ODataOutputContext context) => context.CreateODataEntryWriterAsync(entitySet, entityType));
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0004F607 File Offset: 0x0004D807
		public ODataCollectionWriter CreateODataCollectionWriter()
		{
			return this.CreateODataCollectionWriter(null);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0004F628 File Offset: 0x0004D828
		public ODataCollectionWriter CreateODataCollectionWriter(IEdmTypeReference itemTypeReference)
		{
			this.VerifyCanCreateODataCollectionWriter(itemTypeReference);
			return this.WriteToOutput<ODataCollectionWriter>(ODataPayloadKind.Collection, null, (ODataOutputContext context) => context.CreateODataCollectionWriter(itemTypeReference));
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0004F662 File Offset: 0x0004D862
		public Task<ODataCollectionWriter> CreateODataCollectionWriterAsync()
		{
			return this.CreateODataCollectionWriterAsync(null);
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0004F684 File Offset: 0x0004D884
		public Task<ODataCollectionWriter> CreateODataCollectionWriterAsync(IEdmTypeReference itemTypeReference)
		{
			this.VerifyCanCreateODataCollectionWriter(itemTypeReference);
			return this.WriteToOutputAsync<ODataCollectionWriter>(ODataPayloadKind.Collection, null, (ODataOutputContext context) => context.CreateODataCollectionWriterAsync(itemTypeReference));
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0004F6CC File Offset: 0x0004D8CC
		public ODataBatchWriter CreateODataBatchWriter()
		{
			this.VerifyCanCreateODataBatchWriter();
			return this.WriteToOutput<ODataBatchWriter>(ODataPayloadKind.Batch, null, (ODataOutputContext context) => context.CreateODataBatchWriter(this.batchBoundary));
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0004F6F7 File Offset: 0x0004D8F7
		public Task<ODataBatchWriter> CreateODataBatchWriterAsync()
		{
			this.VerifyCanCreateODataBatchWriter();
			return this.WriteToOutputAsync<ODataBatchWriter>(ODataPayloadKind.Batch, null, (ODataOutputContext context) => context.CreateODataBatchWriterAsync(this.batchBoundary));
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0004F72C File Offset: 0x0004D92C
		public ODataParameterWriter CreateODataParameterWriter(IEdmFunctionImport functionImport)
		{
			this.VerifyCanCreateODataParameterWriter(functionImport);
			return this.WriteToOutput<ODataParameterWriter>(ODataPayloadKind.Parameter, new Action(this.VerifyODataParameterWriterHeaders), (ODataOutputContext context) => context.CreateODataParameterWriter(functionImport));
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0004F788 File Offset: 0x0004D988
		public Task<ODataParameterWriter> CreateODataParameterWriterAsync(IEdmFunctionImport functionImport)
		{
			this.VerifyCanCreateODataParameterWriter(functionImport);
			return this.WriteToOutputAsync<ODataParameterWriter>(ODataPayloadKind.Parameter, new Action(this.VerifyODataParameterWriterHeaders), (ODataOutputContext context) => context.CreateODataParameterWriterAsync(functionImport));
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0004F7E4 File Offset: 0x0004D9E4
		public void WriteServiceDocument(ODataWorkspace defaultWorkspace)
		{
			this.VerifyCanWriteServiceDocument(defaultWorkspace);
			this.WriteToOutput(ODataPayloadKind.ServiceDocument, null, delegate(ODataOutputContext context)
			{
				context.WriteServiceDocument(defaultWorkspace);
			});
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0004F834 File Offset: 0x0004DA34
		public Task WriteServiceDocumentAsync(ODataWorkspace defaultWorkspace)
		{
			this.VerifyCanWriteServiceDocument(defaultWorkspace);
			return this.WriteToOutputAsync(ODataPayloadKind.ServiceDocument, null, (ODataOutputContext context) => context.WriteServiceDocumentAsync(defaultWorkspace));
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0004F884 File Offset: 0x0004DA84
		public void WriteProperty(ODataProperty property)
		{
			this.VerifyCanWriteProperty(property);
			this.WriteToOutput(ODataPayloadKind.Property, null, delegate(ODataOutputContext context)
			{
				context.WriteProperty(property);
			});
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0004F8D4 File Offset: 0x0004DAD4
		public Task WritePropertyAsync(ODataProperty property)
		{
			this.VerifyCanWriteProperty(property);
			return this.WriteToOutputAsync(ODataPayloadKind.Property, null, (ODataOutputContext context) => context.WritePropertyAsync(property));
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0004F92C File Offset: 0x0004DB2C
		public void WriteError(ODataError error, bool includeDebugInformation)
		{
			if (this.outputContext == null)
			{
				this.VerifyCanWriteTopLevelError(error);
				this.WriteToOutput(ODataPayloadKind.Error, null, delegate(ODataOutputContext context)
				{
					context.WriteError(error, includeDebugInformation);
				});
				return;
			}
			this.VerifyCanWriteInStreamError(error);
			this.outputContext.WriteInStreamError(error, includeDebugInformation);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0004F9C0 File Offset: 0x0004DBC0
		public Task WriteErrorAsync(ODataError error, bool includeDebugInformation)
		{
			if (this.outputContext == null)
			{
				this.VerifyCanWriteTopLevelError(error);
				return this.WriteToOutputAsync(ODataPayloadKind.Error, null, (ODataOutputContext context) => context.WriteErrorAsync(error, includeDebugInformation));
			}
			this.VerifyCanWriteInStreamError(error);
			return this.outputContext.WriteInStreamErrorAsync(error, includeDebugInformation);
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0004FA35 File Offset: 0x0004DC35
		public void WriteEntityReferenceLinks(ODataEntityReferenceLinks links)
		{
			this.WriteEntityReferenceLinks(links, null, null);
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0004FA78 File Offset: 0x0004DC78
		public void WriteEntityReferenceLinks(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanWriteEntityReferenceLinks(links, navigationProperty);
			this.WriteToOutput(ODataPayloadKind.EntityReferenceLinks, delegate()
			{
				this.VerifyEntityReferenceLinksHeaders(links);
			}, delegate(ODataOutputContext context)
			{
				context.WriteEntityReferenceLinks(links, entitySet, navigationProperty);
			});
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0004FAD8 File Offset: 0x0004DCD8
		public Task WriteEntityReferenceLinksAsync(ODataEntityReferenceLinks links)
		{
			return this.WriteEntityReferenceLinksAsync(links, null, null);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0004FB18 File Offset: 0x0004DD18
		public Task WriteEntityReferenceLinksAsync(ODataEntityReferenceLinks links, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanWriteEntityReferenceLinks(links, navigationProperty);
			return this.WriteToOutputAsync(ODataPayloadKind.EntityReferenceLinks, delegate()
			{
				this.VerifyEntityReferenceLinksHeaders(links);
			}, (ODataOutputContext context) => context.WriteEntityReferenceLinksAsync(links, entitySet, navigationProperty));
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0004FB78 File Offset: 0x0004DD78
		public void WriteEntityReferenceLink(ODataEntityReferenceLink link)
		{
			this.WriteEntityReferenceLink(link, null, null);
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0004FBA8 File Offset: 0x0004DDA8
		public void WriteEntityReferenceLink(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanWriteEntityReferenceLink(link);
			this.WriteToOutput(ODataPayloadKind.EntityReferenceLink, null, delegate(ODataOutputContext context)
			{
				context.WriteEntityReferenceLink(link, entitySet, navigationProperty);
			});
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0004FBF0 File Offset: 0x0004DDF0
		public Task WriteEntityReferenceLinkAsync(ODataEntityReferenceLink link)
		{
			return this.WriteEntityReferenceLinkAsync(link, null, null);
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0004FC20 File Offset: 0x0004DE20
		public Task WriteEntityReferenceLinkAsync(ODataEntityReferenceLink link, IEdmEntitySet entitySet, IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanWriteEntityReferenceLink(link);
			return this.WriteToOutputAsync(ODataPayloadKind.EntityReferenceLink, null, (ODataOutputContext context) => context.WriteEntityReferenceLinkAsync(link, entitySet, navigationProperty));
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0004FC80 File Offset: 0x0004DE80
		public void WriteValue(object value)
		{
			ODataPayloadKind payloadKind = this.VerifyCanWriteValue(value);
			this.WriteToOutput(payloadKind, null, delegate(ODataOutputContext context)
			{
				context.WriteValue(value);
			});
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0004FCD4 File Offset: 0x0004DED4
		public Task WriteValueAsync(object value)
		{
			ODataPayloadKind payloadKind = this.VerifyCanWriteValue(value);
			return this.WriteToOutputAsync(payloadKind, null, (ODataOutputContext context) => context.WriteValueAsync(value));
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0004FD17 File Offset: 0x0004DF17
		public void WriteMetadataDocument()
		{
			this.VerifyCanWriteMetadataDocument();
			this.WriteToOutput(ODataPayloadKind.MetadataDocument, new Action(this.VerifyMetadataDocumentHeaders), delegate(ODataOutputContext context)
			{
				context.WriteMetadataDocument();
			});
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0004FD50 File Offset: 0x0004DF50
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0004FD5F File Offset: 0x0004DF5F
		internal ODataFormat SetHeaders(ODataPayloadKind payloadKind)
		{
			this.writerPayloadKind = payloadKind;
			this.EnsureODataVersion();
			this.EnsureODataFormatAndContentType();
			return this.format;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0004FD7A File Offset: 0x0004DF7A
		private void SetOrVerifyHeaders(ODataPayloadKind payloadKind)
		{
			this.VerifyPayloadKind(payloadKind);
			if (this.writerPayloadKind == ODataPayloadKind.Unsupported)
			{
				this.SetHeaders(payloadKind);
			}
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0004FD98 File Offset: 0x0004DF98
		private void EnsureODataVersion()
		{
			if (this.settings.Version == null)
			{
				this.settings.Version = new ODataVersion?(ODataUtilsInternal.GetDataServiceVersion(this.message, ODataVersion.V3));
			}
			else
			{
				ODataUtilsInternal.SetDataServiceVersion(this.message, this.settings);
			}
			if (this.settings.Version >= ODataVersion.V3 && this.settings.WriterBehavior.FormatBehaviorKind != ODataBehaviorKind.Default)
			{
				this.settings.WriterBehavior.UseDefaultFormatBehavior();
			}
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0004FE30 File Offset: 0x0004E030
		private void EnsureODataFormatAndContentType()
		{
			string text = null;
			if (this.settings.UseFormat == null)
			{
				text = this.message.GetHeader("Content-Type");
				text = ((text == null) ? null : text.Trim());
			}
			if (!string.IsNullOrEmpty(text))
			{
				ODataPayloadKind odataPayloadKind;
				this.format = MediaTypeUtils.GetFormatFromContentType(text, new ODataPayloadKind[]
				{
					this.writerPayloadKind
				}, this.MediaTypeResolver, out this.mediaType, out this.encoding, out odataPayloadKind, out this.batchBoundary);
				if (this.settings.HasJsonPaddingFunction())
				{
					text = MediaTypeUtils.AlterContentTypeForJsonPadding(text);
					this.message.SetHeader("Content-Type", text);
					return;
				}
			}
			else
			{
				this.format = MediaTypeUtils.GetContentTypeFromSettings(this.settings, this.writerPayloadKind, this.MediaTypeResolver, out this.mediaType, out this.encoding);
				if (this.writerPayloadKind == ODataPayloadKind.Batch)
				{
					this.batchBoundary = ODataBatchWriterUtils.CreateBatchBoundary(this.writingResponse);
					text = ODataBatchWriterUtils.CreateMultipartMixedContentType(this.batchBoundary);
				}
				else
				{
					this.batchBoundary = null;
					text = HttpUtils.BuildContentType(this.mediaType, this.encoding);
				}
				if (this.settings.HasJsonPaddingFunction())
				{
					text = MediaTypeUtils.AlterContentTypeForJsonPadding(text);
				}
				this.message.SetHeader("Content-Type", text);
			}
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0004FF6A File Offset: 0x0004E16A
		private void VerifyCanCreateODataFeedWriter()
		{
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0004FF72 File Offset: 0x0004E172
		private void VerifyCanCreateODataEntryWriter()
		{
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0004FF7A File Offset: 0x0004E17A
		private void VerifyCanCreateODataCollectionWriter(IEdmTypeReference itemTypeReference)
		{
			if (itemTypeReference != null && !itemTypeReference.IsPrimitive() && !itemTypeReference.IsComplex())
			{
				throw new ODataException(Strings.ODataMessageWriter_NonCollectionType(itemTypeReference.ODataFullName()));
			}
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0004FFA6 File Offset: 0x0004E1A6
		private void VerifyCanCreateODataBatchWriter()
		{
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0004FFAE File Offset: 0x0004E1AE
		private void VerifyCanCreateODataParameterWriter(IEdmFunctionImport functionImport)
		{
			if (this.writingResponse)
			{
				throw new ODataException(Strings.ODataParameterWriter_CannotCreateParameterWriterOnResponseMessage);
			}
			if (functionImport != null && !this.model.IsUserModel())
			{
				throw new ODataException(Strings.ODataMessageWriter_CannotSpecifyFunctionImportWithoutModel);
			}
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0004FFE4 File Offset: 0x0004E1E4
		private void VerifyODataParameterWriterHeaders()
		{
			ODataVersionChecker.CheckParameterPayload(this.settings.Version.Value);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00050009 File Offset: 0x0004E209
		private void VerifyCanWriteServiceDocument(ODataWorkspace defaultWorkspace)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataWorkspace>(defaultWorkspace, "defaultWorkspace");
			if (!this.writingResponse)
			{
				throw new ODataException(Strings.ODataMessageWriter_ServiceDocumentInRequest);
			}
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0005002F File Offset: 0x0004E22F
		private void VerifyCanWriteProperty(ODataProperty property)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataProperty>(property, "property");
			if (property.Value is ODataStreamReferenceValue)
			{
				throw new ODataException(Strings.ODataMessageWriter_CannotWriteStreamPropertyAsTopLevelProperty(property.Name));
			}
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00050060 File Offset: 0x0004E260
		private void VerifyCanWriteTopLevelError(ODataError error)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataError>(error, "error");
			if (!this.writingResponse)
			{
				throw new ODataException(Strings.ODataMessageWriter_ErrorPayloadInRequest);
			}
			this.VerifyWriterNotDisposedAndNotUsed();
			this.writeErrorCalled = true;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00050090 File Offset: 0x0004E290
		private void VerifyCanWriteInStreamError(ODataError error)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataError>(error, "error");
			this.VerifyNotDisposed();
			if (!this.writingResponse)
			{
				throw new ODataException(Strings.ODataMessageWriter_ErrorPayloadInRequest);
			}
			if (this.writeErrorCalled)
			{
				throw new ODataException(Strings.ODataMessageWriter_WriteErrorAlreadyCalled);
			}
			this.writeErrorCalled = true;
			this.writeMethodCalled = true;
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x000500E4 File Offset: 0x0004E2E4
		private void VerifyCanWriteEntityReferenceLinks(ODataEntityReferenceLinks links, IEdmNavigationProperty navigationProperty)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataEntityReferenceLinks>(links, "links");
			if (!this.writingResponse)
			{
				throw new ODataException(Strings.ODataMessageWriter_EntityReferenceLinksInRequestNotAllowed);
			}
			if (navigationProperty != null && navigationProperty.Type != null && !navigationProperty.Type.IsCollection())
			{
				throw new ODataException(Strings.ODataMessageWriter_EntityReferenceLinksWithSingletonNavPropNotAllowed(navigationProperty.Name));
			}
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x00050140 File Offset: 0x0004E340
		private void VerifyEntityReferenceLinksHeaders(ODataEntityReferenceLinks links)
		{
			if (links.Count != null)
			{
				ODataVersionChecker.CheckCount(this.settings.Version.Value);
			}
			if (links.NextPageLink != null)
			{
				ODataVersionChecker.CheckNextLink(this.settings.Version.Value);
			}
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0005019B File Offset: 0x0004E39B
		private void VerifyCanWriteEntityReferenceLink(ODataEntityReferenceLink link)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataEntityReferenceLink>(link, "link");
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x000501AE File Offset: 0x0004E3AE
		private ODataPayloadKind VerifyCanWriteValue(object value)
		{
			if (value == null)
			{
				throw new ODataException(Strings.ODataMessageWriter_CannotWriteNullInRawFormat);
			}
			this.VerifyWriterNotDisposedAndNotUsed();
			if (!(value is byte[]))
			{
				return ODataPayloadKind.Value;
			}
			return ODataPayloadKind.BinaryValue;
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x000501CF File Offset: 0x0004E3CF
		private void VerifyCanWriteMetadataDocument()
		{
			if (!this.writingResponse)
			{
				throw new ODataException(Strings.ODataMessageWriter_MetadataDocumentInRequest);
			}
			if (!this.model.IsUserModel())
			{
				throw new ODataException(Strings.ODataMessageWriter_CannotWriteMetadataWithoutModel);
			}
			this.VerifyWriterNotDisposedAndNotUsed();
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x00050204 File Offset: 0x0004E404
		private void VerifyMetadataDocumentHeaders()
		{
			Version version = this.model.GetDataServiceVersion();
			if (version == null)
			{
				version = this.settings.Version.Value.ToDataServiceVersion();
				this.model.SetDataServiceVersion(version);
			}
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0005024B File Offset: 0x0004E44B
		private void VerifyWriterNotDisposedAndNotUsed()
		{
			this.VerifyNotDisposed();
			if (this.writeMethodCalled)
			{
				throw new ODataException(Strings.ODataMessageWriter_WriterAlreadyUsed);
			}
			this.writeMethodCalled = true;
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x0005026D File Offset: 0x0004E46D
		private void VerifyNotDisposed()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00050288 File Offset: 0x0004E488
		private void Dispose(bool disposing)
		{
			this.isDisposed = true;
			if (disposing)
			{
				try
				{
					if (this.outputContext != null)
					{
						this.outputContext.Dispose();
					}
				}
				finally
				{
					this.outputContext = null;
				}
			}
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x000502CC File Offset: 0x0004E4CC
		private void VerifyPayloadKind(ODataPayloadKind payloadKindToWrite)
		{
			if (this.writerPayloadKind != ODataPayloadKind.Unsupported && this.writerPayloadKind != payloadKindToWrite)
			{
				throw new ODataException(Strings.ODataMessageWriter_IncompatiblePayloadKinds(this.writerPayloadKind, payloadKindToWrite));
			}
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00050300 File Offset: 0x0004E500
		private void WriteToOutput(ODataPayloadKind payloadKind, Action verifyHeaders, Action<ODataOutputContext> writeAction)
		{
			this.SetOrVerifyHeaders(payloadKind);
			if (verifyHeaders != null)
			{
				verifyHeaders();
			}
			this.outputContext = this.format.CreateOutputContext(this.message, this.mediaType, this.encoding, this.settings, this.writingResponse, this.model, this.urlResolver);
			writeAction(this.outputContext);
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00050364 File Offset: 0x0004E564
		private TResult WriteToOutput<TResult>(ODataPayloadKind payloadKind, Action verifyHeaders, Func<ODataOutputContext, TResult> writeFunc)
		{
			this.SetOrVerifyHeaders(payloadKind);
			if (verifyHeaders != null)
			{
				verifyHeaders();
			}
			this.outputContext = this.format.CreateOutputContext(this.message, this.mediaType, this.encoding, this.settings, this.writingResponse, this.model, this.urlResolver);
			return writeFunc(this.outputContext);
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x000503FC File Offset: 0x0004E5FC
		private Task WriteToOutputAsync(ODataPayloadKind payloadKind, Action verifyHeaders, Func<ODataOutputContext, Task> writeAsyncAction)
		{
			this.SetOrVerifyHeaders(payloadKind);
			if (verifyHeaders != null)
			{
				verifyHeaders();
			}
			return this.format.CreateOutputContextAsync(this.message, this.mediaType, this.encoding, this.settings, this.writingResponse, this.model, this.urlResolver).FollowOnSuccessWithTask(delegate(Task<ODataOutputContext> createOutputContextTask)
			{
				this.outputContext = createOutputContextTask.Result;
				return writeAsyncAction(this.outputContext);
			});
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x000504A4 File Offset: 0x0004E6A4
		private Task<TResult> WriteToOutputAsync<TResult>(ODataPayloadKind payloadKind, Action verifyHeaders, Func<ODataOutputContext, Task<TResult>> writeFunc)
		{
			this.SetOrVerifyHeaders(payloadKind);
			if (verifyHeaders != null)
			{
				verifyHeaders();
			}
			return this.format.CreateOutputContextAsync(this.message, this.mediaType, this.encoding, this.settings, this.writingResponse, this.model, this.urlResolver).FollowOnSuccessWithTask(delegate(Task<ODataOutputContext> createOutputContextTask)
			{
				this.outputContext = createOutputContextTask.Result;
				return writeFunc(this.outputContext);
			});
		}

		// Token: 0x0400084E RID: 2126
		private readonly ODataMessage message;

		// Token: 0x0400084F RID: 2127
		private readonly bool writingResponse;

		// Token: 0x04000850 RID: 2128
		private readonly ODataMessageWriterSettings settings;

		// Token: 0x04000851 RID: 2129
		private readonly IEdmModel model;

		// Token: 0x04000852 RID: 2130
		private readonly IODataUrlResolver urlResolver;

		// Token: 0x04000853 RID: 2131
		private bool writeMethodCalled;

		// Token: 0x04000854 RID: 2132
		private bool isDisposed;

		// Token: 0x04000855 RID: 2133
		private ODataOutputContext outputContext;

		// Token: 0x04000856 RID: 2134
		private ODataPayloadKind writerPayloadKind;

		// Token: 0x04000857 RID: 2135
		private ODataFormat format;

		// Token: 0x04000858 RID: 2136
		private Encoding encoding;

		// Token: 0x04000859 RID: 2137
		private string batchBoundary;

		// Token: 0x0400085A RID: 2138
		private bool writeErrorCalled;

		// Token: 0x0400085B RID: 2139
		private MediaTypeResolver mediaTypeResolver;

		// Token: 0x0400085C RID: 2140
		private MediaType mediaType;
	}
}
