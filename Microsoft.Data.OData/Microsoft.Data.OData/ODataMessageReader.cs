using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x0200024D RID: 589
	public sealed class ODataMessageReader : IDisposable
	{
		// Token: 0x060012FB RID: 4859 RVA: 0x00046DDA File Offset: 0x00044FDA
		public ODataMessageReader(IODataRequestMessage requestMessage) : this(requestMessage, new ODataMessageReaderSettings())
		{
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00046DE8 File Offset: 0x00044FE8
		public ODataMessageReader(IODataRequestMessage requestMessage, ODataMessageReaderSettings settings) : this(requestMessage, settings, null)
		{
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00046DF4 File Offset: 0x00044FF4
		public ODataMessageReader(IODataRequestMessage requestMessage, ODataMessageReaderSettings settings, IEdmModel model)
		{
			this.readerPayloadKind = ODataPayloadKind.Unsupported;
			base..ctor();
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessage>(requestMessage, "requestMessage");
			this.settings = ((settings == null) ? new ODataMessageReaderSettings() : new ODataMessageReaderSettings(settings));
			ReaderValidationUtils.ValidateMessageReaderSettings(this.settings, false);
			this.readingResponse = false;
			this.message = new ODataRequestMessage(requestMessage, false, this.settings.DisableMessageStreamDisposal, this.settings.MessageQuotas.MaxReceivedMessageSize);
			this.urlResolver = (requestMessage as IODataUrlResolver);
			this.version = ODataUtilsInternal.GetDataServiceVersion(this.message, this.settings.MaxProtocolVersion);
			this.model = (model ?? EdmCoreModel.Instance);
			this.edmTypeResolver = new EdmTypeReaderResolver(this.model, this.settings.ReaderBehavior, this.version);
			ODataVersionChecker.CheckVersionSupported(this.version, this.settings);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00046ED9 File Offset: 0x000450D9
		public ODataMessageReader(IODataResponseMessage responseMessage) : this(responseMessage, new ODataMessageReaderSettings())
		{
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00046EE7 File Offset: 0x000450E7
		public ODataMessageReader(IODataResponseMessage responseMessage, ODataMessageReaderSettings settings) : this(responseMessage, settings, null)
		{
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00046EF4 File Offset: 0x000450F4
		public ODataMessageReader(IODataResponseMessage responseMessage, ODataMessageReaderSettings settings, IEdmModel model)
		{
			this.readerPayloadKind = ODataPayloadKind.Unsupported;
			base..ctor();
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessage>(responseMessage, "responseMessage");
			this.settings = ((settings == null) ? new ODataMessageReaderSettings() : new ODataMessageReaderSettings(settings));
			ReaderValidationUtils.ValidateMessageReaderSettings(this.settings, true);
			this.readingResponse = true;
			this.message = new ODataResponseMessage(responseMessage, false, this.settings.DisableMessageStreamDisposal, this.settings.MessageQuotas.MaxReceivedMessageSize);
			this.urlResolver = (responseMessage as IODataUrlResolver);
			this.version = ODataUtilsInternal.GetDataServiceVersion(this.message, this.settings.MaxProtocolVersion);
			this.model = (model ?? EdmCoreModel.Instance);
			this.edmTypeResolver = new EdmTypeReaderResolver(this.model, this.settings.ReaderBehavior, this.version);
			string annotationFilter = responseMessage.PreferenceAppliedHeader().AnnotationFilter;
			if (this.settings.ShouldIncludeAnnotation == null && !string.IsNullOrEmpty(annotationFilter))
			{
				this.settings.ShouldIncludeAnnotation = ODataUtils.CreateAnnotationFilter(annotationFilter);
			}
			ODataVersionChecker.CheckVersionSupported(this.version, this.settings);
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0004700B File Offset: 0x0004520B
		internal ODataMessageReaderSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x00047013 File Offset: 0x00045213
		private MediaTypeResolver MediaTypeResolver
		{
			get
			{
				if (this.mediaTypeResolver == null)
				{
					this.mediaTypeResolver = MediaTypeResolver.CreateReaderMediaTypeResolver(this.version, this.settings.ReaderBehavior.FormatBehaviorKind);
				}
				return this.mediaTypeResolver;
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0004706C File Offset: 0x0004526C
		public IEnumerable<ODataPayloadKindDetectionResult> DetectPayloadKind()
		{
			if (this.settings.ReaderBehavior.ApiBehaviorKind == ODataBehaviorKind.WcfDataServicesServer)
			{
				throw new ODataException(Strings.ODataMessageReader_PayloadKindDetectionInServerMode);
			}
			IEnumerable<ODataPayloadKindDetectionResult> enumerable;
			if (this.TryGetSinglePayloadKindResultFromContentType(out enumerable))
			{
				return enumerable;
			}
			this.payloadKindDetectionFormatStates = new Dictionary<ODataFormat, object>(ReferenceEqualityComparer<ODataFormat>.Instance);
			List<ODataPayloadKindDetectionResult> list = new List<ODataPayloadKindDetectionResult>();
			try
			{
				IEnumerable<IGrouping<ODataFormat, ODataPayloadKindDetectionResult>> enumerable2 = from kvp in enumerable
				group kvp by kvp.Format;
				foreach (IGrouping<ODataFormat, ODataPayloadKindDetectionResult> grouping in enumerable2)
				{
					ODataPayloadKindDetectionInfo odataPayloadKindDetectionInfo = new ODataPayloadKindDetectionInfo(this.contentType, this.encoding, this.settings, this.model, from pkg in grouping
					select pkg.PayloadKind);
					IEnumerable<ODataPayloadKind> enumerable3 = this.readingResponse ? grouping.Key.DetectPayloadKind((IODataResponseMessage)this.message, odataPayloadKindDetectionInfo) : grouping.Key.DetectPayloadKind((IODataRequestMessage)this.message, odataPayloadKindDetectionInfo);
					if (enumerable3 != null)
					{
						using (IEnumerator<ODataPayloadKind> enumerator2 = enumerable3.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ODataPayloadKind kind = enumerator2.Current;
								if (enumerable.Any((ODataPayloadKindDetectionResult pk) => pk.PayloadKind == kind))
								{
									list.Add(new ODataPayloadKindDetectionResult(kind, grouping.Key));
								}
							}
						}
					}
					this.payloadKindDetectionFormatStates.Add(grouping.Key, odataPayloadKindDetectionInfo.PayloadKindDetectionFormatState);
				}
			}
			finally
			{
				this.message.UseBufferingReadStream = new bool?(false);
				this.message.BufferingReadStream.StopBuffering();
			}
			list.Sort(new Comparison<ODataPayloadKindDetectionResult>(this.ComparePayloadKindDetectionResult));
			return list;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000472E8 File Offset: 0x000454E8
		public Task<IEnumerable<ODataPayloadKindDetectionResult>> DetectPayloadKindAsync()
		{
			if (this.settings.ReaderBehavior.ApiBehaviorKind == ODataBehaviorKind.WcfDataServicesServer)
			{
				throw new ODataException(Strings.ODataMessageReader_PayloadKindDetectionInServerMode);
			}
			IEnumerable<ODataPayloadKindDetectionResult> enumerable;
			if (this.TryGetSinglePayloadKindResultFromContentType(out enumerable))
			{
				return TaskUtils.GetCompletedTask<IEnumerable<ODataPayloadKindDetectionResult>>(enumerable);
			}
			this.payloadKindDetectionFormatStates = new Dictionary<ODataFormat, object>(ReferenceEqualityComparer<ODataFormat>.Instance);
			List<ODataPayloadKindDetectionResult> detectedPayloadKinds = new List<ODataPayloadKindDetectionResult>();
			return Task.Factory.Iterate(this.GetPayloadKindDetectionTasks(enumerable, detectedPayloadKinds)).FollowAlwaysWith(delegate(Task t)
			{
				this.message.UseBufferingReadStream = new bool?(false);
				this.message.BufferingReadStream.StopBuffering();
			}).FollowOnSuccessWith(delegate(Task t)
			{
				detectedPayloadKinds.Sort(new Comparison<ODataPayloadKindDetectionResult>(this.ComparePayloadKindDetectionResult));
				return detectedPayloadKinds;
			});
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00047385 File Offset: 0x00045585
		public ODataReader CreateODataFeedReader()
		{
			return this.CreateODataFeedReader(null, null);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0004738F File Offset: 0x0004558F
		public ODataReader CreateODataFeedReader(IEdmEntityType expectedBaseEntityType)
		{
			return this.CreateODataFeedReader(null, expectedBaseEntityType);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x000473B8 File Offset: 0x000455B8
		public ODataReader CreateODataFeedReader(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			this.VerifyCanCreateODataFeedReader(entitySet, expectedBaseEntityType);
			expectedBaseEntityType = (expectedBaseEntityType ?? this.edmTypeResolver.GetElementType(entitySet));
			Func<ODataInputContext, ODataReader> readFunc = (ODataInputContext context) => context.CreateFeedReader(entitySet, expectedBaseEntityType);
			ODataPayloadKind[] payloadKinds = new ODataPayloadKind[1];
			return this.ReadFromInput<ODataReader>(readFunc, payloadKinds);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00047426 File Offset: 0x00045626
		public Task<ODataReader> CreateODataFeedReaderAsync()
		{
			return this.CreateODataFeedReaderAsync(null, null);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00047430 File Offset: 0x00045630
		public Task<ODataReader> CreateODataFeedReaderAsync(IEdmEntityType expectedBaseEntityType)
		{
			return this.CreateODataFeedReaderAsync(null, expectedBaseEntityType);
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00047458 File Offset: 0x00045658
		public Task<ODataReader> CreateODataFeedReaderAsync(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			this.VerifyCanCreateODataFeedReader(entitySet, expectedBaseEntityType);
			expectedBaseEntityType = (expectedBaseEntityType ?? this.edmTypeResolver.GetElementType(entitySet));
			Func<ODataInputContext, Task<ODataReader>> readFunc = (ODataInputContext context) => context.CreateFeedReaderAsync(entitySet, expectedBaseEntityType);
			ODataPayloadKind[] payloadKinds = new ODataPayloadKind[1];
			return this.ReadFromInputAsync<ODataReader>(readFunc, payloadKinds);
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000474C6 File Offset: 0x000456C6
		public ODataReader CreateODataEntryReader()
		{
			return this.CreateODataEntryReader(null, null);
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000474D0 File Offset: 0x000456D0
		public ODataReader CreateODataEntryReader(IEdmEntityType entityType)
		{
			return this.CreateODataEntryReader(null, entityType);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000474F8 File Offset: 0x000456F8
		public ODataReader CreateODataEntryReader(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			this.VerifyCanCreateODataEntryReader(entitySet, entityType);
			entityType = (entityType ?? this.edmTypeResolver.GetElementType(entitySet));
			return this.ReadFromInput<ODataReader>((ODataInputContext context) => context.CreateEntryReader(entitySet, entityType), new ODataPayloadKind[]
			{
				ODataPayloadKind.Entry
			});
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0004756A File Offset: 0x0004576A
		public Task<ODataReader> CreateODataEntryReaderAsync()
		{
			return this.CreateODataEntryReaderAsync(null, null);
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00047574 File Offset: 0x00045774
		public Task<ODataReader> CreateODataEntryReaderAsync(IEdmEntityType entityType)
		{
			return this.CreateODataEntryReaderAsync(null, entityType);
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0004759C File Offset: 0x0004579C
		public Task<ODataReader> CreateODataEntryReaderAsync(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			this.VerifyCanCreateODataEntryReader(entitySet, entityType);
			entityType = (entityType ?? this.edmTypeResolver.GetElementType(entitySet));
			return this.ReadFromInputAsync<ODataReader>((ODataInputContext context) => context.CreateEntryReaderAsync(entitySet, entityType), new ODataPayloadKind[]
			{
				ODataPayloadKind.Entry
			});
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0004760E File Offset: 0x0004580E
		public ODataCollectionReader CreateODataCollectionReader()
		{
			return this.CreateODataCollectionReader(null);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00047630 File Offset: 0x00045830
		public ODataCollectionReader CreateODataCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			this.VerifyCanCreateODataCollectionReader(expectedItemTypeReference);
			return this.ReadFromInput<ODataCollectionReader>((ODataInputContext context) => context.CreateCollectionReader(expectedItemTypeReference), new ODataPayloadKind[]
			{
				ODataPayloadKind.Collection
			});
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00047674 File Offset: 0x00045874
		public Task<ODataCollectionReader> CreateODataCollectionReaderAsync()
		{
			return this.CreateODataCollectionReaderAsync(null);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00047694 File Offset: 0x00045894
		public Task<ODataCollectionReader> CreateODataCollectionReaderAsync(IEdmTypeReference expectedItemTypeReference)
		{
			this.VerifyCanCreateODataCollectionReader(expectedItemTypeReference);
			return this.ReadFromInputAsync<ODataCollectionReader>((ODataInputContext context) => context.CreateCollectionReaderAsync(expectedItemTypeReference), new ODataPayloadKind[]
			{
				ODataPayloadKind.Collection
			});
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x000476E8 File Offset: 0x000458E8
		public ODataBatchReader CreateODataBatchReader()
		{
			this.VerifyCanCreateODataBatchReader();
			return this.ReadFromInput<ODataBatchReader>((ODataInputContext context) => context.CreateBatchReader(this.batchBoundary), new ODataPayloadKind[]
			{
				ODataPayloadKind.Batch
			});
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00047728 File Offset: 0x00045928
		public Task<ODataBatchReader> CreateODataBatchReaderAsync()
		{
			this.VerifyCanCreateODataBatchReader();
			return this.ReadFromInputAsync<ODataBatchReader>((ODataInputContext context) => context.CreateBatchReaderAsync(this.batchBoundary), new ODataPayloadKind[]
			{
				ODataPayloadKind.Batch
			});
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00047770 File Offset: 0x00045970
		public ODataParameterReader CreateODataParameterReader(IEdmFunctionImport functionImport)
		{
			this.VerifyCanCreateODataParameterReader(functionImport);
			return this.ReadFromInput<ODataParameterReader>((ODataInputContext context) => context.CreateParameterReader(functionImport), new ODataPayloadKind[]
			{
				ODataPayloadKind.Parameter
			});
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x000477CC File Offset: 0x000459CC
		public Task<ODataParameterReader> CreateODataParameterReaderAsync(IEdmFunctionImport functionImport)
		{
			this.VerifyCanCreateODataParameterReader(functionImport);
			return this.ReadFromInputAsync<ODataParameterReader>((ODataInputContext context) => context.CreateParameterReaderAsync(functionImport), new ODataPayloadKind[]
			{
				ODataPayloadKind.Parameter
			});
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0004781C File Offset: 0x00045A1C
		public ODataWorkspace ReadServiceDocument()
		{
			this.VerifyCanReadServiceDocument();
			return this.ReadFromInput<ODataWorkspace>((ODataInputContext context) => context.ReadServiceDocument(), new ODataPayloadKind[]
			{
				ODataPayloadKind.ServiceDocument
			});
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00047868 File Offset: 0x00045A68
		public Task<ODataWorkspace> ReadServiceDocumentAsync()
		{
			this.VerifyCanReadServiceDocument();
			return this.ReadFromInputAsync<ODataWorkspace>((ODataInputContext context) => context.ReadServiceDocumentAsync(), new ODataPayloadKind[]
			{
				ODataPayloadKind.ServiceDocument
			});
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x000478AA File Offset: 0x00045AAA
		public ODataProperty ReadProperty()
		{
			return this.ReadProperty(null);
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x000478CC File Offset: 0x00045ACC
		public ODataProperty ReadProperty(IEdmTypeReference expectedPropertyTypeReference)
		{
			this.VerifyCanReadProperty(expectedPropertyTypeReference);
			return this.ReadFromInput<ODataProperty>((ODataInputContext context) => context.ReadProperty(null, expectedPropertyTypeReference), new ODataPayloadKind[]
			{
				ODataPayloadKind.Property
			});
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00047934 File Offset: 0x00045B34
		public ODataProperty ReadProperty(IEdmStructuralProperty property)
		{
			this.VerifyCanReadProperty(property);
			return this.ReadFromInput<ODataProperty>((ODataInputContext context) => context.ReadProperty(property, property.Type), new ODataPayloadKind[]
			{
				ODataPayloadKind.Property
			});
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00047978 File Offset: 0x00045B78
		public Task<ODataProperty> ReadPropertyAsync()
		{
			return this.ReadPropertyAsync(null);
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00047998 File Offset: 0x00045B98
		public Task<ODataProperty> ReadPropertyAsync(IEdmTypeReference expectedPropertyTypeReference)
		{
			this.VerifyCanReadProperty(expectedPropertyTypeReference);
			return this.ReadFromInputAsync<ODataProperty>((ODataInputContext context) => context.ReadPropertyAsync(null, expectedPropertyTypeReference), new ODataPayloadKind[]
			{
				ODataPayloadKind.Property
			});
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00047A00 File Offset: 0x00045C00
		public Task<ODataProperty> ReadPropertyAsync(IEdmStructuralProperty property)
		{
			this.VerifyCanReadProperty(property);
			return this.ReadFromInputAsync<ODataProperty>((ODataInputContext context) => context.ReadPropertyAsync(property, property.Type), new ODataPayloadKind[]
			{
				ODataPayloadKind.Property
			});
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00047A4C File Offset: 0x00045C4C
		public ODataError ReadError()
		{
			this.VerifyCanReadError();
			return this.ReadFromInput<ODataError>((ODataInputContext context) => context.ReadError(), new ODataPayloadKind[]
			{
				ODataPayloadKind.Error
			});
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00047A98 File Offset: 0x00045C98
		public Task<ODataError> ReadErrorAsync()
		{
			this.VerifyCanReadError();
			return this.ReadFromInputAsync<ODataError>((ODataInputContext context) => context.ReadErrorAsync(), new ODataPayloadKind[]
			{
				ODataPayloadKind.Error
			});
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00047ADB File Offset: 0x00045CDB
		public ODataEntityReferenceLinks ReadEntityReferenceLinks()
		{
			return this.ReadEntityReferenceLinks(null);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00047AFC File Offset: 0x00045CFC
		public ODataEntityReferenceLinks ReadEntityReferenceLinks(IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanReadEntityReferenceLinks(navigationProperty);
			return this.ReadFromInput<ODataEntityReferenceLinks>((ODataInputContext context) => context.ReadEntityReferenceLinks(navigationProperty), new ODataPayloadKind[]
			{
				ODataPayloadKind.EntityReferenceLinks
			});
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00047B40 File Offset: 0x00045D40
		public Task<ODataEntityReferenceLinks> ReadEntityReferenceLinksAsync()
		{
			return this.ReadEntityReferenceLinksAsync(null);
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00047B60 File Offset: 0x00045D60
		public Task<ODataEntityReferenceLinks> ReadEntityReferenceLinksAsync(IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanReadEntityReferenceLinks(navigationProperty);
			return this.ReadFromInputAsync<ODataEntityReferenceLinks>((ODataInputContext context) => context.ReadEntityReferenceLinksAsync(navigationProperty), new ODataPayloadKind[]
			{
				ODataPayloadKind.EntityReferenceLinks
			});
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00047BA4 File Offset: 0x00045DA4
		public ODataEntityReferenceLink ReadEntityReferenceLink()
		{
			return this.ReadEntityReferenceLink(null);
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x00047BC4 File Offset: 0x00045DC4
		public ODataEntityReferenceLink ReadEntityReferenceLink(IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanReadEntityReferenceLink();
			return this.ReadFromInput<ODataEntityReferenceLink>((ODataInputContext context) => context.ReadEntityReferenceLink(navigationProperty), new ODataPayloadKind[]
			{
				ODataPayloadKind.EntityReferenceLink
			});
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00047C02 File Offset: 0x00045E02
		public Task<ODataEntityReferenceLink> ReadEntityReferenceLinkAsync()
		{
			return this.ReadEntityReferenceLinkAsync(null);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00047C24 File Offset: 0x00045E24
		public Task<ODataEntityReferenceLink> ReadEntityReferenceLinkAsync(IEdmNavigationProperty navigationProperty)
		{
			this.VerifyCanReadEntityReferenceLink();
			return this.ReadFromInputAsync<ODataEntityReferenceLink>((ODataInputContext context) => context.ReadEntityReferenceLinkAsync(navigationProperty), new ODataPayloadKind[]
			{
				ODataPayloadKind.EntityReferenceLink
			});
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00047C80 File Offset: 0x00045E80
		public object ReadValue(IEdmTypeReference expectedTypeReference)
		{
			ODataPayloadKind[] payloadKinds = this.VerifyCanReadValue(expectedTypeReference);
			return this.ReadFromInput<object>((ODataInputContext context) => context.ReadValue((IEdmPrimitiveTypeReference)expectedTypeReference), payloadKinds);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00047CD8 File Offset: 0x00045ED8
		public Task<object> ReadValueAsync(IEdmTypeReference expectedTypeReference)
		{
			ODataPayloadKind[] payloadKinds = this.VerifyCanReadValue(expectedTypeReference);
			return this.ReadFromInputAsync<object>((ODataInputContext context) => context.ReadValueAsync((IEdmPrimitiveTypeReference)expectedTypeReference), payloadKinds);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00047D1C File Offset: 0x00045F1C
		public IEdmModel ReadMetadataDocument()
		{
			this.VerifyCanReadMetadataDocument();
			return this.ReadFromInput<IEdmModel>((ODataInputContext context) => context.ReadMetadataDocument(), new ODataPayloadKind[]
			{
				ODataPayloadKind.MetadataDocument
			});
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00047D5F File Offset: 0x00045F5F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00047D6E File Offset: 0x00045F6E
		internal ODataFormat GetFormat()
		{
			if (this.format == null)
			{
				throw new ODataException(Strings.ODataMessageReader_GetFormatCalledBeforeReadingStarted);
			}
			return this.format;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00047D8C File Offset: 0x00045F8C
		private void ProcessContentType(params ODataPayloadKind[] payloadKinds)
		{
			string contentTypeHeader = this.GetContentTypeHeader();
			this.format = MediaTypeUtils.GetFormatFromContentType(contentTypeHeader, payloadKinds, this.MediaTypeResolver, out this.contentType, out this.encoding, out this.readerPayloadKind, out this.batchBoundary);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00047DCC File Offset: 0x00045FCC
		private string GetContentTypeHeader()
		{
			string text = this.message.GetHeader("Content-Type");
			text = ((text == null) ? null : text.Trim());
			if (string.IsNullOrEmpty(text))
			{
				throw new ODataContentTypeException(Strings.ODataMessageReader_NoneOrEmptyContentTypeHeader);
			}
			return text;
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00047E0C File Offset: 0x0004600C
		private void VerifyCanCreateODataFeedReader(IEdmEntitySet entitySet, IEdmEntityType expectedBaseEntityType)
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (!this.model.IsUserModel())
			{
				if (entitySet != null)
				{
					throw new ArgumentException(Strings.ODataMessageReader_EntitySetSpecifiedWithoutMetadata("entitySet"), "entitySet");
				}
				if (expectedBaseEntityType != null)
				{
					throw new ArgumentException(Strings.ODataMessageReader_ExpectedTypeSpecifiedWithoutMetadata("expectedBaseEntityType"), "expectedBaseEntityType");
				}
			}
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00047E5C File Offset: 0x0004605C
		private void VerifyCanCreateODataEntryReader(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (!this.model.IsUserModel())
			{
				if (entitySet != null)
				{
					throw new ArgumentException(Strings.ODataMessageReader_EntitySetSpecifiedWithoutMetadata("entitySet"), "entitySet");
				}
				if (entityType != null)
				{
					throw new ArgumentException(Strings.ODataMessageReader_ExpectedTypeSpecifiedWithoutMetadata("entityType"), "entityType");
				}
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00047EAC File Offset: 0x000460AC
		private void VerifyCanCreateODataCollectionReader(IEdmTypeReference expectedItemTypeReference)
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (expectedItemTypeReference != null)
			{
				if (!this.model.IsUserModel())
				{
					throw new ArgumentException(Strings.ODataMessageReader_ExpectedTypeSpecifiedWithoutMetadata("expectedItemTypeReference"), "expectedItemTypeReference");
				}
				if (!expectedItemTypeReference.IsODataPrimitiveTypeKind() && expectedItemTypeReference.TypeKind() != EdmTypeKind.Complex)
				{
					throw new ArgumentException(Strings.ODataMessageReader_ExpectedCollectionTypeWrongKind(expectedItemTypeReference.TypeKind().ToString()), "expectedItemTypeReference");
				}
			}
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00047F15 File Offset: 0x00046115
		private void VerifyCanCreateODataBatchReader()
		{
			this.VerifyReaderNotDisposedAndNotUsed();
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00047F20 File Offset: 0x00046120
		private void VerifyCanCreateODataParameterReader(IEdmFunctionImport functionImport)
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (this.readingResponse)
			{
				throw new ODataException(Strings.ODataMessageReader_ParameterPayloadInResponse);
			}
			ODataVersionChecker.CheckParameterPayload(this.version);
			if (functionImport != null && !this.model.IsUserModel())
			{
				throw new ArgumentException(Strings.ODataMessageReader_FunctionImportSpecifiedWithoutMetadata("functionImport"), "functionImport");
			}
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00047F76 File Offset: 0x00046176
		private void VerifyCanReadServiceDocument()
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (!this.readingResponse)
			{
				throw new ODataException(Strings.ODataMessageReader_ServiceDocumentInRequest);
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00047F91 File Offset: 0x00046191
		private void VerifyCanReadMetadataDocument()
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (!this.readingResponse)
			{
				throw new ODataException(Strings.ODataMessageReader_MetadataDocumentInRequest);
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00047FAC File Offset: 0x000461AC
		private void VerifyCanReadProperty(IEdmStructuralProperty property)
		{
			if (property == null)
			{
				return;
			}
			this.VerifyCanReadProperty(property.Type);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00047FC0 File Offset: 0x000461C0
		private void VerifyCanReadProperty(IEdmTypeReference expectedPropertyTypeReference)
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (expectedPropertyTypeReference != null)
			{
				if (!this.model.IsUserModel())
				{
					throw new ArgumentException(Strings.ODataMessageReader_ExpectedTypeSpecifiedWithoutMetadata("expectedPropertyTypeReference"), "expectedPropertyTypeReference");
				}
				IEdmCollectionType edmCollectionType = expectedPropertyTypeReference.Definition as IEdmCollectionType;
				if (edmCollectionType != null && edmCollectionType.ElementType.IsODataEntityTypeKind())
				{
					throw new ArgumentException(Strings.ODataMessageReader_ExpectedPropertyTypeEntityCollectionKind, "expectedPropertyTypeReference");
				}
				if (expectedPropertyTypeReference.IsODataEntityTypeKind())
				{
					throw new ArgumentException(Strings.ODataMessageReader_ExpectedPropertyTypeEntityKind, "expectedPropertyTypeReference");
				}
				if (expectedPropertyTypeReference.IsStream())
				{
					throw new ArgumentException(Strings.ODataMessageReader_ExpectedPropertyTypeStream, "expectedPropertyTypeReference");
				}
			}
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00048054 File Offset: 0x00046254
		private void VerifyCanReadError()
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (!this.readingResponse)
			{
				throw new ODataException(Strings.ODataMessageReader_ErrorPayloadInRequest);
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00048070 File Offset: 0x00046270
		private void VerifyCanReadEntityReferenceLinks(IEdmNavigationProperty navigationProperty)
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (!this.readingResponse)
			{
				throw new ODataException(Strings.ODataMessageReader_EntityReferenceLinksInRequestNotAllowed);
			}
			if (navigationProperty != null && !navigationProperty.Type.IsCollection())
			{
				throw new ODataException(Strings.ODataMessageReader_SingletonNavigationPropertyForEntityReferenceLinks(navigationProperty.Name, navigationProperty.DeclaringEntityType().FullName()));
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000480C2 File Offset: 0x000462C2
		private void VerifyCanReadEntityReferenceLink()
		{
			this.VerifyReaderNotDisposedAndNotUsed();
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x000480CC File Offset: 0x000462CC
		private ODataPayloadKind[] VerifyCanReadValue(IEdmTypeReference expectedTypeReference)
		{
			this.VerifyReaderNotDisposedAndNotUsed();
			if (expectedTypeReference == null)
			{
				return new ODataPayloadKind[]
				{
					ODataPayloadKind.Value,
					ODataPayloadKind.BinaryValue
				};
			}
			if (!expectedTypeReference.IsODataPrimitiveTypeKind())
			{
				throw new ArgumentException(Strings.ODataMessageReader_ExpectedValueTypeWrongKind(expectedTypeReference.TypeKind().ToString()), "expectedTypeReference");
			}
			if (expectedTypeReference.IsBinary())
			{
				return new ODataPayloadKind[]
				{
					ODataPayloadKind.BinaryValue
				};
			}
			return new ODataPayloadKind[]
			{
				ODataPayloadKind.Value
			};
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0004813C File Offset: 0x0004633C
		private void VerifyReaderNotDisposedAndNotUsed()
		{
			this.VerifyNotDisposed();
			if (this.readMethodCalled)
			{
				throw new ODataException(Strings.ODataMessageReader_ReaderAlreadyUsed);
			}
			if (this.message.BufferingReadStream != null && this.message.BufferingReadStream.IsBuffering)
			{
				throw new ODataException(Strings.ODataMessageReader_PayloadKindDetectionRunning);
			}
			this.readMethodCalled = true;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00048193 File Offset: 0x00046393
		private void VerifyNotDisposed()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x000481B0 File Offset: 0x000463B0
		private void Dispose(bool disposing)
		{
			this.isDisposed = true;
			if (disposing)
			{
				try
				{
					if (this.inputContext != null)
					{
						this.inputContext.Dispose();
					}
				}
				finally
				{
					this.inputContext = null;
				}
				if (!this.settings.DisableMessageStreamDisposal && this.message.BufferingReadStream != null)
				{
					this.message.BufferingReadStream.Dispose();
				}
			}
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00048220 File Offset: 0x00046420
		private T ReadFromInput<T>(Func<ODataInputContext, T> readFunc, params ODataPayloadKind[] payloadKinds) where T : class
		{
			this.ProcessContentType(payloadKinds);
			object payloadKindDetectionFormatState = null;
			if (this.payloadKindDetectionFormatStates != null)
			{
				this.payloadKindDetectionFormatStates.TryGetValue(this.format, out payloadKindDetectionFormatState);
			}
			this.inputContext = this.format.CreateInputContext(this.readerPayloadKind, this.message, this.contentType, this.encoding, this.settings, this.version, this.readingResponse, this.model, this.urlResolver, payloadKindDetectionFormatState);
			return readFunc(this.inputContext);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x000482BC File Offset: 0x000464BC
		private bool TryGetSinglePayloadKindResultFromContentType(out IEnumerable<ODataPayloadKindDetectionResult> payloadKindResults)
		{
			if (this.message.UseBufferingReadStream == true)
			{
				throw new ODataException(Strings.ODataMessageReader_DetectPayloadKindMultipleTimes);
			}
			string contentTypeHeader = this.GetContentTypeHeader();
			IList<ODataPayloadKindDetectionResult> payloadKindsForContentType = MediaTypeUtils.GetPayloadKindsForContentType(contentTypeHeader, this.MediaTypeResolver, out this.contentType, out this.encoding);
			payloadKindResults = from r in payloadKindsForContentType
			where ODataUtilsInternal.IsPayloadKindSupported(r.PayloadKind, !this.readingResponse)
			select r;
			if (payloadKindResults.Count<ODataPayloadKindDetectionResult>() > 1)
			{
				this.message.UseBufferingReadStream = new bool?(true);
				return false;
			}
			return true;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00048348 File Offset: 0x00046548
		private int ComparePayloadKindDetectionResult(ODataPayloadKindDetectionResult first, ODataPayloadKindDetectionResult second)
		{
			ODataPayloadKind payloadKind = first.PayloadKind;
			ODataPayloadKind payloadKind2 = second.PayloadKind;
			if (payloadKind == payloadKind2)
			{
				return 0;
			}
			if (first.PayloadKind >= second.PayloadKind)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00048818 File Offset: 0x00046A18
		private IEnumerable<Task> GetPayloadKindDetectionTasks(IEnumerable<ODataPayloadKindDetectionResult> payloadKindsFromContentType, List<ODataPayloadKindDetectionResult> detectionResults)
		{
			IEnumerable<IGrouping<ODataFormat, ODataPayloadKindDetectionResult>> payloadKindFromContentTypeGroups = from kvp in payloadKindsFromContentType
			group kvp by kvp.Format;
			using (IEnumerator<IGrouping<ODataFormat, ODataPayloadKindDetectionResult>> enumerator = payloadKindFromContentTypeGroups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ODataMessageReader.<>c__DisplayClass57 CS$<>8__locals2 = new ODataMessageReader.<>c__DisplayClass57();
					CS$<>8__locals2.payloadKindGroup = enumerator.Current;
					ODataPayloadKindDetectionInfo detectionInfo = new ODataPayloadKindDetectionInfo(this.contentType, this.encoding, this.settings, this.model, from pkg in CS$<>8__locals2.payloadKindGroup
					select pkg.PayloadKind);
					Task<IEnumerable<ODataPayloadKind>> detectionResult = this.readingResponse ? CS$<>8__locals2.payloadKindGroup.Key.DetectPayloadKindAsync((IODataResponseMessageAsync)this.message, detectionInfo) : CS$<>8__locals2.payloadKindGroup.Key.DetectPayloadKindAsync((IODataRequestMessageAsync)this.message, detectionInfo);
					yield return detectionResult.FollowOnSuccessWith(delegate(Task<IEnumerable<ODataPayloadKind>> t)
					{
						IEnumerable<ODataPayloadKind> result = t.Result;
						if (result != null)
						{
							using (IEnumerator<ODataPayloadKind> enumerator2 = result.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									ODataPayloadKind kind = enumerator2.Current;
									if (payloadKindsFromContentType.Any((ODataPayloadKindDetectionResult pk) => pk.PayloadKind == kind))
									{
										detectionResults.Add(new ODataPayloadKindDetectionResult(kind, CS$<>8__locals2.payloadKindGroup.Key));
									}
								}
							}
						}
						this.payloadKindDetectionFormatStates.Add(CS$<>8__locals2.payloadKindGroup.Key, detectionInfo.PayloadKindDetectionFormatState);
					});
				}
			}
			yield break;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00048874 File Offset: 0x00046A74
		private Task<T> ReadFromInputAsync<T>(Func<ODataInputContext, Task<T>> readFunc, params ODataPayloadKind[] payloadKinds) where T : class
		{
			this.ProcessContentType(payloadKinds);
			object payloadKindDetectionFormatState = null;
			if (this.payloadKindDetectionFormatStates != null)
			{
				this.payloadKindDetectionFormatStates.TryGetValue(this.format, out payloadKindDetectionFormatState);
			}
			return this.format.CreateInputContextAsync(this.readerPayloadKind, this.message, this.contentType, this.encoding, this.settings, this.version, this.readingResponse, this.model, this.urlResolver, payloadKindDetectionFormatState).FollowOnSuccessWithTask(delegate(Task<ODataInputContext> createInputContextTask)
			{
				this.inputContext = createInputContextTask.Result;
				return readFunc(this.inputContext);
			});
		}

		// Token: 0x040006CF RID: 1743
		private readonly ODataMessage message;

		// Token: 0x040006D0 RID: 1744
		private readonly bool readingResponse;

		// Token: 0x040006D1 RID: 1745
		private readonly ODataMessageReaderSettings settings;

		// Token: 0x040006D2 RID: 1746
		private readonly IEdmModel model;

		// Token: 0x040006D3 RID: 1747
		private readonly ODataVersion version;

		// Token: 0x040006D4 RID: 1748
		private readonly IODataUrlResolver urlResolver;

		// Token: 0x040006D5 RID: 1749
		private readonly EdmTypeResolver edmTypeResolver;

		// Token: 0x040006D6 RID: 1750
		private bool readMethodCalled;

		// Token: 0x040006D7 RID: 1751
		private bool isDisposed;

		// Token: 0x040006D8 RID: 1752
		private ODataInputContext inputContext;

		// Token: 0x040006D9 RID: 1753
		private ODataPayloadKind readerPayloadKind;

		// Token: 0x040006DA RID: 1754
		private ODataFormat format;

		// Token: 0x040006DB RID: 1755
		private MediaType contentType;

		// Token: 0x040006DC RID: 1756
		private Encoding encoding;

		// Token: 0x040006DD RID: 1757
		private string batchBoundary;

		// Token: 0x040006DE RID: 1758
		private MediaTypeResolver mediaTypeResolver;

		// Token: 0x040006DF RID: 1759
		private Dictionary<ODataFormat, object> payloadKindDetectionFormatStates;
	}
}
