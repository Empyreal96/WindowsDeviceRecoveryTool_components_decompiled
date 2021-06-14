using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000029 RID: 41
	internal class BatchSaveResult : BaseSaveResult
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00006D89 File Offset: 0x00004F89
		internal BatchSaveResult(DataServiceContext context, string method, DataServiceRequest[] queries, SaveChangesOptions options, AsyncCallback callback, object state) : base(context, method, queries, options, callback, state)
		{
			this.Queries = queries;
			this.streamCopyBuffer = new byte[4000];
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00006DB1 File Offset: 0x00004FB1
		internal override bool IsBatchRequest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006DB4 File Offset: 0x00004FB4
		protected override Stream ResponseStream
		{
			get
			{
				return this.responseStream;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006DBC File Offset: 0x00004FBC
		protected override bool ProcessResponsePayload
		{
			get
			{
				return !this.currentOperationResponse.HasEmptyContent;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006DCC File Offset: 0x00004FCC
		internal void BatchBeginRequest()
		{
			BaseAsyncResult.PerRequest perRequest = null;
			try
			{
				ODataRequestMessageWrapper odataRequestMessageWrapper = this.GenerateBatchRequest();
				base.Abortable = odataRequestMessageWrapper;
				if (odataRequestMessageWrapper != null)
				{
					odataRequestMessageWrapper.SetContentLengthHeader();
					perRequest = (this.perRequest = new BaseAsyncResult.PerRequest());
					perRequest.Request = odataRequestMessageWrapper;
					perRequest.RequestContentStream = odataRequestMessageWrapper.CachedRequestStream;
					BaseAsyncResult.AsyncStateBag state = new BaseAsyncResult.AsyncStateBag(perRequest);
					this.responseStream = new MemoryStream();
					IAsyncResult asyncResult = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(odataRequestMessageWrapper.BeginGetRequestStream), new AsyncCallback(base.AsyncEndGetRequestStream), state);
					perRequest.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
				}
			}
			catch (Exception e)
			{
				base.HandleFailure(perRequest, e);
				throw;
			}
			finally
			{
				this.HandleCompleted(perRequest);
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006E88 File Offset: 0x00005088
		internal void BatchRequest()
		{
			ODataRequestMessageWrapper odataRequestMessageWrapper = this.GenerateBatchRequest();
			if (odataRequestMessageWrapper != null)
			{
				odataRequestMessageWrapper.SetRequestStream(odataRequestMessageWrapper.CachedRequestStream);
				try
				{
					this.batchResponseMessage = this.RequestInfo.GetSyncronousResponse(odataRequestMessageWrapper, false);
				}
				catch (DataServiceTransportException exception)
				{
					InvalidOperationException httpWebResponse = WebUtil.GetHttpWebResponse(exception, ref this.batchResponseMessage);
					throw httpWebResponse;
				}
				finally
				{
					if (this.batchResponseMessage != null)
					{
						this.responseStream = this.batchResponseMessage.GetStream();
					}
				}
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006F08 File Offset: 0x00005108
		protected override void FinishCurrentChange(BaseAsyncResult.PerRequest pereq)
		{
			base.FinishCurrentChange(pereq);
			this.ResponseStream.Position = 0L;
			this.perRequest = null;
			base.SetCompleted();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006F2B File Offset: 0x0000512B
		protected override void HandleOperationResponse(IODataResponseMessage responseMessage)
		{
			Error.ThrowInternalError(InternalError.InvalidHandleOperationResponse);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006F34 File Offset: 0x00005134
		protected override DataServiceResponse HandleResponse()
		{
			if (this.ResponseStream != null)
			{
				return this.HandleBatchResponse();
			}
			return new DataServiceResponse(null, 0, new List<OperationResponse>(0), true);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006F54 File Offset: 0x00005154
		protected override MaterializeAtom GetMaterializer(EntityDescriptor entityDescriptor, ResponseInfo responseInfo)
		{
			QueryComponents queryComponents = new QueryComponents(null, Util.DataServiceVersionEmpty, entityDescriptor.Entity.GetType(), null, null);
			return new MaterializeAtom(responseInfo, queryComponents, null, this.currentOperationResponse.CreateResponseMessage(), ODataPayloadKind.Entry);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006F90 File Offset: 0x00005190
		protected override ODataRequestMessageWrapper CreateRequestMessage(string method, Uri requestUri, HeaderCollection headers, HttpStack httpStack, Descriptor descriptor)
		{
			BuildingRequestEventArgs requestMessageArgs = this.RequestInfo.CreateRequestArgsAndFireBuildingRequest(method, requestUri, headers, this.RequestInfo.HttpStack, descriptor);
			return ODataRequestMessageWrapper.CreateBatchPartRequestMessage(this.batchWriter, requestMessageArgs, this.RequestInfo);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006FCC File Offset: 0x000051CC
		private static string CreateMultiPartMimeContentType()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}; {1}={2}_{3}", new object[]
			{
				"multipart/mixed",
				"boundary",
				"batch",
				Guid.NewGuid()
			});
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007018 File Offset: 0x00005218
		private ODataRequestMessageWrapper CreateBatchRequest()
		{
			Uri requestUri = UriUtil.CreateUri(this.RequestInfo.BaseUriResolver.GetBaseUriWithSlash(), UriUtil.CreateUri("$batch", UriKind.Relative));
			HeaderCollection headerCollection = new HeaderCollection();
			headerCollection.SetRequestVersion(Util.DataServiceVersion1, this.RequestInfo.MaxProtocolVersionAsVersion);
			headerCollection.SetHeader("Content-Type", BatchSaveResult.CreateMultiPartMimeContentType());
			this.RequestInfo.Format.SetRequestAcceptHeaderForBatch(headerCollection);
			return base.CreateTopLevelRequest("POST", requestUri, headerCollection, this.RequestInfo.HttpStack, null);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000709C File Offset: 0x0000529C
		private ODataRequestMessageWrapper GenerateBatchRequest()
		{
			if (this.ChangedEntries.Count == 0 && this.Queries == null)
			{
				base.SetCompleted();
				return null;
			}
			ODataRequestMessageWrapper odataRequestMessageWrapper = this.CreateBatchRequest();
			odataRequestMessageWrapper.FireSendingRequest2(null);
			using (ODataMessageWriter odataMessageWriter = Serializer.CreateMessageWriter(odataRequestMessageWrapper, this.RequestInfo, false))
			{
				this.batchWriter = odataMessageWriter.CreateODataBatchWriter();
				this.batchWriter.WriteStartBatch();
				if (this.Queries != null)
				{
					foreach (DataServiceRequest dataServiceRequest in this.Queries)
					{
						QueryComponents queryComponents = dataServiceRequest.QueryComponents(this.RequestInfo.Model);
						Uri orCreateAbsoluteUri = this.RequestInfo.BaseUriResolver.GetOrCreateAbsoluteUri(queryComponents.Uri);
						HeaderCollection headerCollection = new HeaderCollection();
						headerCollection.SetRequestVersion(queryComponents.Version, this.RequestInfo.MaxProtocolVersionAsVersion);
						this.RequestInfo.Format.SetRequestAcceptHeaderForQuery(headerCollection, queryComponents);
						ODataRequestMessageWrapper odataRequestMessageWrapper2 = this.CreateRequestMessage("GET", orCreateAbsoluteUri, headerCollection, this.RequestInfo.HttpStack, null);
						odataRequestMessageWrapper2.FireSendingEventHandlers(null);
					}
				}
				else if (0 < this.ChangedEntries.Count)
				{
					if (Util.IsBatchWithSingleChangeset(this.Options))
					{
						this.batchWriter.WriteStartChangeset();
					}
					ClientEdmModel model = this.RequestInfo.Model;
					for (int j = 0; j < this.ChangedEntries.Count; j++)
					{
						if (Util.IsBatchWithIndependentOperations(this.Options))
						{
							this.batchWriter.WriteStartChangeset();
						}
						Descriptor descriptor = this.ChangedEntries[j];
						if (!descriptor.ContentGeneratedForSave)
						{
							EntityDescriptor entityDescriptor = descriptor as EntityDescriptor;
							if (descriptor.DescriptorKind == DescriptorKind.Entity)
							{
								if (entityDescriptor.State == EntityStates.Added)
								{
									ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(entityDescriptor.Entity.GetType()));
									if (clientTypeAnnotation.IsMediaLinkEntry || entityDescriptor.IsMediaLinkEntry)
									{
										throw Error.NotSupported(Strings.Context_BatchNotSupportedForMediaLink);
									}
								}
								else if ((entityDescriptor.State == EntityStates.Unchanged || entityDescriptor.State == EntityStates.Modified) && entityDescriptor.SaveStream != null)
								{
									throw Error.NotSupported(Strings.Context_BatchNotSupportedForMediaLink);
								}
							}
							else if (descriptor.DescriptorKind == DescriptorKind.NamedStream)
							{
								throw Error.NotSupported(Strings.Context_BatchNotSupportedForNamedStreams);
							}
							ODataRequestMessageWrapper odataRequestMessageWrapper3;
							if (descriptor.DescriptorKind == DescriptorKind.Entity)
							{
								odataRequestMessageWrapper3 = base.CreateRequest(entityDescriptor);
							}
							else
							{
								odataRequestMessageWrapper3 = base.CreateRequest((LinkDescriptor)descriptor);
							}
							odataRequestMessageWrapper3.FireSendingRequest2(descriptor);
							base.CreateChangeData(j, odataRequestMessageWrapper3);
							if (Util.IsBatchWithIndependentOperations(this.Options))
							{
								this.batchWriter.WriteEndChangeset();
							}
						}
					}
					if (Util.IsBatchWithSingleChangeset(this.Options))
					{
						this.batchWriter.WriteEndChangeset();
					}
				}
				this.batchWriter.WriteEndBatch();
				this.batchWriter.Flush();
			}
			return odataRequestMessageWrapper;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000737C File Offset: 0x0000557C
		private DataServiceResponse HandleBatchResponse()
		{
			bool flag = true;
			DataServiceResponse result;
			try
			{
				if (this.batchResponseMessage == null || this.batchResponseMessage.StatusCode == 204)
				{
					throw Error.InvalidOperation(Strings.Batch_ExpectedResponse(1));
				}
				Func<Stream> getResponseStream = () => this.ResponseStream;
				Version version;
				BaseSaveResult.HandleResponse(this.RequestInfo, (HttpStatusCode)this.batchResponseMessage.StatusCode, this.batchResponseMessage.GetHeader("DataServiceVersion"), getResponseStream, true, out version);
				if (this.ResponseStream == null)
				{
					Error.ThrowBatchExpectedResponse(InternalError.NullResponseStream);
				}
				this.batchResponseMessage = new HttpWebResponseMessage(new HeaderCollection(this.batchResponseMessage), this.batchResponseMessage.StatusCode, getResponseStream);
				ODataMessageReaderSettings settings = this.RequestInfo.GetDeserializationInfo(null).ReadHelper.CreateSettings(null);
				this.batchMessageReader = new ODataMessageReader(this.batchResponseMessage, settings);
				ODataBatchReader batchReader;
				try
				{
					batchReader = this.batchMessageReader.CreateODataBatchReader();
				}
				catch (ODataContentTypeException ex)
				{
					Exception innerException = ex;
					string b;
					Encoding encoding;
					ContentTypeUtil.ReadContentType(this.batchResponseMessage.GetHeader("Content-Type"), out b, out encoding);
					if (string.Equals("text/plain", b))
					{
						innerException = BaseSaveResult.GetResponseText(new Func<Stream>(this.batchResponseMessage.GetStream), (HttpStatusCode)this.batchResponseMessage.StatusCode);
					}
					throw Error.InvalidOperation(Strings.Batch_ExpectedContentType(this.batchResponseMessage.GetHeader("Content-Type")), innerException);
				}
				DataServiceResponse dataServiceResponse = this.HandleBatchResponseInternal(batchReader);
				flag = false;
				result = dataServiceResponse;
			}
			catch (DataServiceRequestException)
			{
				throw;
			}
			catch (InvalidOperationException innerException2)
			{
				HeaderCollection headers = new HeaderCollection(this.batchResponseMessage);
				int statusCode = (this.batchResponseMessage == null) ? 500 : this.batchResponseMessage.StatusCode;
				DataServiceResponse response = new DataServiceResponse(headers, statusCode, new OperationResponse[0], this.IsBatchRequest);
				throw new DataServiceRequestException(Strings.DataServiceException_GeneralError, innerException2, response);
			}
			finally
			{
				if (flag)
				{
					Util.Dispose<ODataMessageReader>(ref this.batchMessageReader);
				}
			}
			return result;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000075B8 File Offset: 0x000057B8
		private DataServiceResponse HandleBatchResponseInternal(ODataBatchReader batchReader)
		{
			HeaderCollection headers = new HeaderCollection(this.batchResponseMessage);
			IEnumerable<OperationResponse> enumerable = this.HandleBatchResponse(batchReader);
			DataServiceResponse dataServiceResponse;
			if (this.Queries != null)
			{
				dataServiceResponse = new DataServiceResponse(headers, this.batchResponseMessage.StatusCode, enumerable, true);
			}
			else
			{
				List<OperationResponse> list = new List<OperationResponse>();
				dataServiceResponse = new DataServiceResponse(headers, this.batchResponseMessage.StatusCode, list, true);
				Exception ex = null;
				foreach (OperationResponse operationResponse in enumerable)
				{
					ChangeOperationResponse changeOperationResponse = (ChangeOperationResponse)operationResponse;
					list.Add(changeOperationResponse);
					if (Util.IsBatchWithSingleChangeset(this.Options) && ex == null && changeOperationResponse.Error != null)
					{
						ex = changeOperationResponse.Error;
					}
				}
				if (ex != null)
				{
					throw new DataServiceRequestException(Strings.DataServiceException_GeneralError, ex, dataServiceResponse);
				}
			}
			return dataServiceResponse;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007DA0 File Offset: 0x00005FA0
		private IEnumerable<OperationResponse> HandleBatchResponse(ODataBatchReader batchReader)
		{
			try
			{
				if (this.batchMessageReader == null)
				{
					yield break;
				}
				bool changesetFound = false;
				bool insideChangeset = false;
				int queryCount = 0;
				int operationCount = 0;
				this.entryIndex = 0;
				while (batchReader.Read())
				{
					switch (batchReader.State)
					{
					case ODataBatchReaderState.Operation:
					{
						Exception exception = this.ProcessCurrentOperationResponse(batchReader, insideChangeset);
						if (!insideChangeset)
						{
							QueryOperationResponse qresponse = null;
							try
							{
								if (exception == null)
								{
									DataServiceRequest dataServiceRequest = this.Queries[queryCount];
									ResponseInfo deserializationInfo = this.RequestInfo.GetDeserializationInfo(null);
									MaterializeAtom results = DataServiceRequest.Materialize(deserializationInfo, dataServiceRequest.QueryComponents(this.RequestInfo.Model), null, this.currentOperationResponse.Headers.GetHeader("Content-Type"), this.currentOperationResponse.CreateResponseMessage(), dataServiceRequest.PayloadKind);
									qresponse = QueryOperationResponse.GetInstance(dataServiceRequest.ElementType, this.currentOperationResponse.Headers, dataServiceRequest, results);
								}
							}
							catch (ArgumentException ex)
							{
								exception = ex;
							}
							catch (FormatException ex2)
							{
								exception = ex2;
							}
							catch (InvalidOperationException ex3)
							{
								exception = ex3;
							}
							if (qresponse == null)
							{
								if (this.Queries == null)
								{
									throw exception;
								}
								DataServiceRequest dataServiceRequest2 = this.Queries[queryCount];
								if (this.RequestInfo.IgnoreResourceNotFoundException && this.currentOperationResponse.StatusCode == HttpStatusCode.NotFound)
								{
									qresponse = QueryOperationResponse.GetInstance(dataServiceRequest2.ElementType, this.currentOperationResponse.Headers, dataServiceRequest2, MaterializeAtom.EmptyResults);
								}
								else
								{
									qresponse = QueryOperationResponse.GetInstance(dataServiceRequest2.ElementType, this.currentOperationResponse.Headers, dataServiceRequest2, MaterializeAtom.EmptyResults);
									qresponse.Error = exception;
								}
							}
							qresponse.StatusCode = (int)this.currentOperationResponse.StatusCode;
							queryCount++;
							yield return qresponse;
						}
						else
						{
							try
							{
								Descriptor descriptor = this.ChangedEntries[this.entryIndex];
								operationCount += base.SaveResultProcessed(descriptor);
								if (exception != null)
								{
									throw exception;
								}
								base.HandleOperationResponseHeaders(this.currentOperationResponse.StatusCode, this.currentOperationResponse.Headers);
								base.HandleOperationResponse(descriptor, this.currentOperationResponse.Headers);
							}
							catch (Exception ex4)
							{
								this.ChangedEntries[this.entryIndex].SaveError = ex4;
								exception = ex4;
								if (!CommonUtil.IsCatchableExceptionType(ex4))
								{
									throw;
								}
							}
							ChangeOperationResponse changeOperationResponse = new ChangeOperationResponse(this.currentOperationResponse.Headers, this.ChangedEntries[this.entryIndex]);
							changeOperationResponse.StatusCode = (int)this.currentOperationResponse.StatusCode;
							if (exception != null)
							{
								changeOperationResponse.Error = exception;
							}
							operationCount++;
							this.entryIndex++;
							yield return changeOperationResponse;
						}
						break;
					}
					case ODataBatchReaderState.ChangesetStart:
						if ((Util.IsBatchWithSingleChangeset(this.Options) && changesetFound) || operationCount != 0)
						{
							Error.ThrowBatchUnexpectedContent(InternalError.UnexpectedBeginChangeSet);
						}
						insideChangeset = true;
						break;
					case ODataBatchReaderState.ChangesetEnd:
						changesetFound = true;
						operationCount = 0;
						insideChangeset = false;
						break;
					default:
						Error.ThrowBatchExpectedResponse(InternalError.UnexpectedBatchState);
						break;
					}
				}
				if (this.Queries == null)
				{
					if (!changesetFound || 0 < queryCount)
					{
						goto IL_546;
					}
					if (this.ChangedEntries.Any((Descriptor o) => o.ContentGeneratedForSave && o.SaveResultWasProcessed == (EntityStates)0))
					{
						if (!this.IsBatchRequest)
						{
							goto IL_546;
						}
						if (this.ChangedEntries.FirstOrDefault((Descriptor o) => o.SaveError != null) == null)
						{
							goto IL_546;
						}
					}
				}
				if (this.Queries == null || queryCount == this.Queries.Length)
				{
					goto JumpOutOfTryFinally1;
				}
				IL_546:
				throw Error.InvalidOperation(Strings.Batch_IncompleteResponseCount);
			}
			finally
			{
				Util.Dispose<ODataMessageReader>(ref this.batchMessageReader);
			}
			JumpOutOfTryFinally1:
			yield break;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007DD4 File Offset: 0x00005FD4
		private Exception ProcessCurrentOperationResponse(ODataBatchReader batchReader, bool isChangesetOperation)
		{
			IODataResponseMessage iodataResponseMessage = batchReader.CreateOperationResponseMessage();
			Descriptor descriptor = null;
			if (isChangesetOperation)
			{
				this.entryIndex = this.ValidateContentID(new HeaderCollection(iodataResponseMessage.Headers));
				descriptor = this.ChangedEntries[this.entryIndex];
			}
			if (!WebUtil.SuccessStatusCode((HttpStatusCode)iodataResponseMessage.StatusCode))
			{
				descriptor = null;
			}
			this.RequestInfo.Context.FireReceivingResponseEvent(new ReceivingResponseEventArgs(iodataResponseMessage, descriptor, true));
			Stream stream = iodataResponseMessage.GetStream();
			if (stream == null)
			{
				Error.ThrowBatchExpectedResponse(InternalError.NullResponseStream);
			}
			MemoryStream memoryStream;
			try
			{
				memoryStream = new MemoryStream();
				WebUtil.CopyStream(stream, memoryStream, ref this.streamCopyBuffer);
				memoryStream.Position = 0L;
			}
			finally
			{
				stream.Dispose();
			}
			this.currentOperationResponse = new BatchSaveResult.CurrentOperationResponse((HttpStatusCode)iodataResponseMessage.StatusCode, iodataResponseMessage.Headers, memoryStream);
			string headerName = "DataServiceVersion";
			Version version;
			return BaseSaveResult.HandleResponse(this.RequestInfo, this.currentOperationResponse.StatusCode, this.currentOperationResponse.Headers.GetHeader(headerName), () => this.currentOperationResponse.ContentStream, false, out version);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007ED8 File Offset: 0x000060D8
		private int ValidateContentID(HeaderCollection contentHeaders)
		{
			int num = 0;
			string s;
			if (!contentHeaders.TryGetHeader("Content-ID", out s) || !int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out num))
			{
				Error.ThrowBatchUnexpectedContent(InternalError.ChangeResponseMissingContentID);
			}
			for (int i = 0; i < this.ChangedEntries.Count; i++)
			{
				if ((ulong)this.ChangedEntries[i].ChangeOrder == (ulong)((long)num))
				{
					return i;
				}
			}
			Error.ThrowBatchUnexpectedContent(InternalError.ChangeResponseUnknownContentID);
			return -1;
		}

		// Token: 0x040001D4 RID: 468
		private const int StreamCopyBufferSize = 4000;

		// Token: 0x040001D5 RID: 469
		private readonly DataServiceRequest[] Queries;

		// Token: 0x040001D6 RID: 470
		private Stream responseStream;

		// Token: 0x040001D7 RID: 471
		private ODataBatchWriter batchWriter;

		// Token: 0x040001D8 RID: 472
		private ODataMessageReader batchMessageReader;

		// Token: 0x040001D9 RID: 473
		private BatchSaveResult.CurrentOperationResponse currentOperationResponse;

		// Token: 0x040001DA RID: 474
		private byte[] streamCopyBuffer;

		// Token: 0x0200002A RID: 42
		private sealed class CurrentOperationResponse
		{
			// Token: 0x06000143 RID: 323 RVA: 0x00007F44 File Offset: 0x00006144
			public CurrentOperationResponse(HttpStatusCode statusCode, IEnumerable<KeyValuePair<string, string>> headers, MemoryStream contentStream)
			{
				this.statusCode = statusCode;
				this.contentStream = contentStream;
				this.headers = new HeaderCollection();
				foreach (KeyValuePair<string, string> keyValuePair in headers)
				{
					this.headers.SetHeader(keyValuePair.Key, keyValuePair.Value);
				}
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x06000144 RID: 324 RVA: 0x00007FC0 File Offset: 0x000061C0
			public HttpStatusCode StatusCode
			{
				get
				{
					return this.statusCode;
				}
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x06000145 RID: 325 RVA: 0x00007FC8 File Offset: 0x000061C8
			public Stream ContentStream
			{
				get
				{
					return this.contentStream;
				}
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x06000146 RID: 326 RVA: 0x00007FD0 File Offset: 0x000061D0
			public bool HasEmptyContent
			{
				get
				{
					return this.contentStream.Length == 0L;
				}
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x06000147 RID: 327 RVA: 0x00007FE1 File Offset: 0x000061E1
			public HeaderCollection Headers
			{
				get
				{
					return this.headers;
				}
			}

			// Token: 0x06000148 RID: 328 RVA: 0x00007FF1 File Offset: 0x000061F1
			public IODataResponseMessage CreateResponseMessage()
			{
				if (!this.HasEmptyContent)
				{
					return new HttpWebResponseMessage(this.headers, (int)this.statusCode, () => this.contentStream);
				}
				return null;
			}

			// Token: 0x040001DD RID: 477
			private readonly HttpStatusCode statusCode;

			// Token: 0x040001DE RID: 478
			private readonly HeaderCollection headers;

			// Token: 0x040001DF RID: 479
			private readonly MemoryStream contentStream;
		}
	}
}
