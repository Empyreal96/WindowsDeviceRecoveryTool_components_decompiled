using System;
using System.Collections.Generic;
using System.Data.Services.Client.Materialization;
using System.Data.Services.Client.Metadata;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200011E RID: 286
	internal class SaveResult : BaseSaveResult
	{
		// Token: 0x06000973 RID: 2419 RVA: 0x00025DFE File Offset: 0x00023FFE
		internal SaveResult(DataServiceContext context, string method, SaveChangesOptions options, AsyncCallback callback, object state) : base(context, method, null, options, callback, state)
		{
			this.cachedResponses = new List<SaveResult.CachedResponse>();
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x00025E19 File Offset: 0x00024019
		internal override bool IsBatchRequest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00025E1C File Offset: 0x0002401C
		protected override bool ProcessResponsePayload
		{
			get
			{
				return this.cachedResponse.MaterializerEntry != null;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00025E2F File Offset: 0x0002402F
		protected override Stream ResponseStream
		{
			get
			{
				return this.inMemoryResponseStream;
			}
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00025E38 File Offset: 0x00024038
		internal void BeginCreateNextChange()
		{
			this.inMemoryResponseStream = new MemoryStream();
			BaseAsyncResult.PerRequest perRequest = null;
			for (;;)
			{
				IODataResponseMessage response = null;
				try
				{
					if (this.perRequest != null)
					{
						base.SetCompleted();
						Error.ThrowInternalError(InternalError.InvalidBeginNextChange);
					}
					ODataRequestMessageWrapper odataRequestMessageWrapper = this.CreateNextRequest();
					if (odataRequestMessageWrapper == null)
					{
						base.Abortable = null;
					}
					if (odataRequestMessageWrapper != null || this.entryIndex < this.ChangedEntries.Count)
					{
						if (this.ChangedEntries[this.entryIndex].ContentGeneratedForSave)
						{
							goto IL_183;
						}
						base.Abortable = odataRequestMessageWrapper;
						ContentStream contentStream = this.CreateNonBatchChangeData(this.entryIndex, odataRequestMessageWrapper);
						perRequest = (this.perRequest = new BaseAsyncResult.PerRequest());
						perRequest.Request = odataRequestMessageWrapper;
						BaseAsyncResult.AsyncStateBag state = new BaseAsyncResult.AsyncStateBag(perRequest);
						IAsyncResult asyncResult;
						if (contentStream == null || contentStream.Stream == null)
						{
							asyncResult = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(odataRequestMessageWrapper.BeginGetResponse), new AsyncCallback(this.AsyncEndGetResponse), state);
						}
						else
						{
							if (contentStream.IsKnownMemoryStream)
							{
								odataRequestMessageWrapper.SetContentLengthHeader();
							}
							perRequest.RequestContentStream = contentStream;
							asyncResult = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(odataRequestMessageWrapper.BeginGetRequestStream), new AsyncCallback(base.AsyncEndGetRequestStream), state);
						}
						perRequest.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
						base.SetCompletedSynchronously(perRequest.RequestCompletedSynchronously);
					}
					else
					{
						base.SetCompleted();
						if (base.CompletedSynchronously)
						{
							this.HandleCompleted(perRequest);
						}
					}
				}
				catch (InvalidOperationException httpWebResponse)
				{
					httpWebResponse = WebUtil.GetHttpWebResponse(httpWebResponse, ref response);
					this.HandleOperationException(httpWebResponse, response);
					this.HandleCompleted(perRequest);
				}
				finally
				{
					WebUtil.DisposeMessage(response);
				}
				goto IL_161;
				IL_183:
				if ((perRequest != null && (!perRequest.RequestCompleted || !perRequest.RequestCompletedSynchronously)) || base.IsCompletedInternally)
				{
					break;
				}
				continue;
				IL_161:
				if (perRequest != null && perRequest.RequestCompleted && perRequest.RequestCompletedSynchronously && !base.IsCompletedInternally)
				{
					this.FinishCurrentChange(perRequest);
					goto IL_183;
				}
				goto IL_183;
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0002601C File Offset: 0x0002421C
		internal void CreateNextChange()
		{
			do
			{
				IODataResponseMessage iodataResponseMessage = null;
				try
				{
					ODataRequestMessageWrapper odataRequestMessageWrapper = this.CreateNextRequest();
					if (odataRequestMessageWrapper != null || this.entryIndex < this.ChangedEntries.Count)
					{
						if (!this.ChangedEntries[this.entryIndex].ContentGeneratedForSave)
						{
							ContentStream contentStream = this.CreateNonBatchChangeData(this.entryIndex, odataRequestMessageWrapper);
							if (contentStream != null && contentStream.Stream != null)
							{
								odataRequestMessageWrapper.SetRequestStream(contentStream);
							}
							iodataResponseMessage = this.RequestInfo.GetSyncronousResponse(odataRequestMessageWrapper, false);
							this.HandleOperationResponse(iodataResponseMessage);
							base.HandleOperationResponseHeaders((HttpStatusCode)iodataResponseMessage.StatusCode, new HeaderCollection(iodataResponseMessage));
							this.HandleOperationResponseData(iodataResponseMessage);
							this.perRequest = null;
						}
					}
				}
				catch (InvalidOperationException httpWebResponse)
				{
					httpWebResponse = WebUtil.GetHttpWebResponse(httpWebResponse, ref iodataResponseMessage);
					this.HandleOperationException(httpWebResponse, iodataResponseMessage);
				}
				finally
				{
					WebUtil.DisposeMessage(iodataResponseMessage);
				}
			}
			while (this.entryIndex < this.ChangedEntries.Count && !base.IsCompletedInternally);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00026114 File Offset: 0x00024314
		protected override void FinishCurrentChange(BaseAsyncResult.PerRequest pereq)
		{
			base.FinishCurrentChange(pereq);
			if (this.ResponseStream.Position != 0L)
			{
				this.ResponseStream.Position = 0L;
				this.HandleOperationResponseData(this.responseMessage, this.ResponseStream);
			}
			else
			{
				this.HandleOperationResponseData(this.responseMessage, null);
			}
			pereq.Dispose();
			this.perRequest = null;
			if (!pereq.RequestCompletedSynchronously && !base.IsCompletedInternally)
			{
				this.BeginCreateNextChange();
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00026188 File Offset: 0x00024388
		protected override void HandleOperationResponse(IODataResponseMessage responseMsg)
		{
			this.responseMessage = responseMsg;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00026194 File Offset: 0x00024394
		protected override DataServiceResponse HandleResponse()
		{
			List<OperationResponse> list = new List<OperationResponse>((this.cachedResponses != null) ? this.cachedResponses.Count : 0);
			DataServiceResponse dataServiceResponse = new DataServiceResponse(null, -1, list, false);
			Exception ex = null;
			try
			{
				foreach (SaveResult.CachedResponse cachedResponse in this.cachedResponses)
				{
					Descriptor descriptor = cachedResponse.Descriptor;
					base.SaveResultProcessed(descriptor);
					OperationResponse operationResponse = new ChangeOperationResponse(cachedResponse.Headers, descriptor);
					operationResponse.StatusCode = (int)cachedResponse.StatusCode;
					if (cachedResponse.Exception != null)
					{
						operationResponse.Error = cachedResponse.Exception;
						if (ex == null)
						{
							ex = cachedResponse.Exception;
						}
					}
					else
					{
						this.cachedResponse = cachedResponse;
						base.HandleOperationResponse(descriptor, cachedResponse.Headers);
					}
					list.Add(operationResponse);
				}
			}
			catch (InvalidOperationException ex2)
			{
				ex = ex2;
			}
			if (ex != null)
			{
				throw new DataServiceRequestException(Strings.DataServiceException_GeneralError, ex, dataServiceResponse);
			}
			return dataServiceResponse;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x000262A4 File Offset: 0x000244A4
		protected override MaterializeAtom GetMaterializer(EntityDescriptor entityDescriptor, ResponseInfo responseInfo)
		{
			ODataEntry odataEntry = (this.cachedResponse.MaterializerEntry == null) ? null : this.cachedResponse.MaterializerEntry.Entry;
			return new MaterializeAtom(responseInfo, new ODataEntry[]
			{
				odataEntry
			}, entityDescriptor.Entity.GetType(), this.cachedResponse.MaterializerEntry.Format);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000262FF File Offset: 0x000244FF
		protected override ODataRequestMessageWrapper CreateRequestMessage(string method, Uri requestUri, HeaderCollection headers, HttpStack httpStack, Descriptor descriptor)
		{
			return base.CreateTopLevelRequest(method, requestUri, headers, httpStack, descriptor);
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00026310 File Offset: 0x00024510
		protected ContentStream CreateNonBatchChangeData(int index, ODataRequestMessageWrapper requestMessage)
		{
			Descriptor descriptor = this.ChangedEntries[index];
			if (descriptor.DescriptorKind == DescriptorKind.Entity && this.streamRequestKind != BaseSaveResult.StreamRequestKind.None)
			{
				if (this.streamRequestKind != BaseSaveResult.StreamRequestKind.None)
				{
					return new ContentStream(this.mediaResourceRequestStream, false);
				}
			}
			else
			{
				if (descriptor.DescriptorKind == DescriptorKind.NamedStream)
				{
					descriptor.ContentGeneratedForSave = true;
					return new ContentStream(this.mediaResourceRequestStream, false);
				}
				if (base.CreateChangeData(index, requestMessage))
				{
					return requestMessage.CachedRequestStream;
				}
			}
			return null;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00026380 File Offset: 0x00024580
		private ODataRequestMessageWrapper CreateNextRequest()
		{
			bool flag = this.streamRequestKind == BaseSaveResult.StreamRequestKind.None;
			if (this.entryIndex < this.ChangedEntries.Count)
			{
				Descriptor descriptor = this.ChangedEntries[this.entryIndex];
				if (descriptor.DescriptorKind == DescriptorKind.Entity)
				{
					EntityDescriptor entityDescriptor = (EntityDescriptor)descriptor;
					entityDescriptor.CloseSaveStream();
					if (this.streamRequestKind == BaseSaveResult.StreamRequestKind.PutMediaResource && EntityStates.Unchanged == entityDescriptor.State)
					{
						entityDescriptor.ContentGeneratedForSave = true;
						flag = true;
					}
				}
				else if (descriptor.DescriptorKind == DescriptorKind.NamedStream)
				{
					((StreamDescriptor)descriptor).CloseSaveStream();
				}
			}
			if (flag)
			{
				this.entryIndex++;
			}
			ODataRequestMessageWrapper odataRequestMessageWrapper = null;
			if (this.entryIndex < this.ChangedEntries.Count)
			{
				Descriptor descriptor2 = this.ChangedEntries[this.entryIndex];
				Descriptor descriptor3 = descriptor2;
				if (descriptor2.DescriptorKind == DescriptorKind.Entity)
				{
					EntityDescriptor entityDescriptor2 = (EntityDescriptor)descriptor2;
					if ((EntityStates.Unchanged == descriptor2.State || EntityStates.Modified == descriptor2.State) && (odataRequestMessageWrapper = this.CheckAndProcessMediaEntryPut(entityDescriptor2)) != null)
					{
						this.streamRequestKind = BaseSaveResult.StreamRequestKind.PutMediaResource;
						descriptor3 = entityDescriptor2.DefaultStreamDescriptor;
					}
					else if (EntityStates.Added == descriptor2.State && (odataRequestMessageWrapper = this.CheckAndProcessMediaEntryPost(entityDescriptor2)) != null)
					{
						this.streamRequestKind = BaseSaveResult.StreamRequestKind.PostMediaResource;
						entityDescriptor2.StreamState = EntityStates.Added;
					}
					else
					{
						this.streamRequestKind = BaseSaveResult.StreamRequestKind.None;
						odataRequestMessageWrapper = base.CreateRequest(entityDescriptor2);
					}
				}
				else if (descriptor2.DescriptorKind == DescriptorKind.NamedStream)
				{
					odataRequestMessageWrapper = this.CreateNamedStreamRequest((StreamDescriptor)descriptor2);
				}
				else
				{
					odataRequestMessageWrapper = base.CreateRequest((LinkDescriptor)descriptor2);
				}
				if (odataRequestMessageWrapper != null)
				{
					odataRequestMessageWrapper.FireSendingEventHandlers(descriptor3);
				}
			}
			return odataRequestMessageWrapper;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x000264F4 File Offset: 0x000246F4
		private ODataRequestMessageWrapper CheckAndProcessMediaEntryPost(EntityDescriptor entityDescriptor)
		{
			ClientEdmModel model = this.RequestInfo.Model;
			ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(entityDescriptor.Entity.GetType()));
			if (!clientTypeAnnotation.IsMediaLinkEntry && !entityDescriptor.IsMediaLinkEntry)
			{
				return null;
			}
			if (clientTypeAnnotation.MediaDataMember == null && entityDescriptor.SaveStream == null)
			{
				throw Error.InvalidOperation(Strings.Context_MLEWithoutSaveStream(clientTypeAnnotation.ElementTypeName));
			}
			ODataRequestMessageWrapper odataRequestMessageWrapper;
			if (clientTypeAnnotation.MediaDataMember != null)
			{
				int num = 0;
				string text;
				if (clientTypeAnnotation.MediaDataMember.MimeTypeProperty == null)
				{
					text = "application/octet-stream";
				}
				else
				{
					object value = clientTypeAnnotation.MediaDataMember.MimeTypeProperty.GetValue(entityDescriptor.Entity);
					string text2 = (value != null) ? value.ToString() : null;
					if (string.IsNullOrEmpty(text2))
					{
						throw Error.InvalidOperation(Strings.Context_NoContentTypeForMediaLink(clientTypeAnnotation.ElementTypeName, clientTypeAnnotation.MediaDataMember.MimeTypeProperty.PropertyName));
					}
					text = text2;
				}
				object value2 = clientTypeAnnotation.MediaDataMember.GetValue(entityDescriptor.Entity);
				if (value2 == null)
				{
					this.mediaResourceRequestStream = null;
				}
				else
				{
					byte[] array = value2 as byte[];
					if (array == null)
					{
						string text3;
						Encoding utf;
						ContentTypeUtil.ReadContentType(text, out text3, out utf);
						if (utf == null)
						{
							utf = Encoding.UTF8;
							text += ";charset=UTF-8";
						}
						array = utf.GetBytes(ClientConvert.ToString(value2));
					}
					num = array.Length;
					this.mediaResourceRequestStream = new MemoryStream(array, 0, array.Length, false, true);
				}
				HeaderCollection headerCollection = new HeaderCollection();
				headerCollection.SetHeader("Content-Length", num.ToString(CultureInfo.InvariantCulture));
				headerCollection.SetHeader("Content-Type", text);
				odataRequestMessageWrapper = this.CreateMediaResourceRequest(entityDescriptor.GetResourceUri(this.RequestInfo.BaseUriResolver, false), "POST", Util.DataServiceVersion1, clientTypeAnnotation.MediaDataMember == null, true, headerCollection, entityDescriptor);
				odataRequestMessageWrapper.AddHeadersToReset(new string[]
				{
					"Content-Length"
				});
				odataRequestMessageWrapper.AddHeadersToReset(new string[]
				{
					"Content-Type"
				});
			}
			else
			{
				HeaderCollection headers = new HeaderCollection();
				IEnumerable<string> headerNames = this.SetupMediaResourceRequest(headers, entityDescriptor.SaveStream, null);
				odataRequestMessageWrapper = this.CreateMediaResourceRequest(entityDescriptor.GetResourceUri(this.RequestInfo.BaseUriResolver, false), "POST", Util.DataServiceVersion1, clientTypeAnnotation.MediaDataMember == null, true, headers, entityDescriptor);
				odataRequestMessageWrapper.AddHeadersToReset(headerNames);
			}
			entityDescriptor.State = EntityStates.Modified;
			return odataRequestMessageWrapper;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00026738 File Offset: 0x00024938
		private ODataRequestMessageWrapper CheckAndProcessMediaEntryPut(EntityDescriptor entityDescriptor)
		{
			if (entityDescriptor.SaveStream == null)
			{
				return null;
			}
			Uri latestEditStreamUri = entityDescriptor.GetLatestEditStreamUri();
			if (latestEditStreamUri == null)
			{
				throw Error.InvalidOperation(Strings.Context_SetSaveStreamWithoutEditMediaLink);
			}
			HeaderCollection headers = new HeaderCollection();
			IEnumerable<string> headerNames = this.SetupMediaResourceRequest(headers, entityDescriptor.SaveStream, entityDescriptor.GetLatestStreamETag());
			ODataRequestMessageWrapper odataRequestMessageWrapper = this.CreateMediaResourceRequest(latestEditStreamUri, "PUT", Util.DataServiceVersion1, true, false, headers, entityDescriptor.DefaultStreamDescriptor);
			odataRequestMessageWrapper.AddHeadersToReset(headerNames);
			return odataRequestMessageWrapper;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x000267A8 File Offset: 0x000249A8
		private ODataRequestMessageWrapper CreateMediaResourceRequest(Uri requestUri, string method, Version version, bool sendChunked, bool applyResponsePreference, HeaderCollection headers, Descriptor descriptor)
		{
			headers.SetHeaderIfUnset("Content-Type", "*/*");
			if (applyResponsePreference)
			{
				BaseSaveResult.ApplyPreferences(headers, method, this.RequestInfo.AddAndUpdateResponsePreference, ref version);
			}
			headers.SetRequestVersion(version, this.RequestInfo.MaxProtocolVersionAsVersion);
			this.RequestInfo.Format.SetRequestAcceptHeader(headers);
			ODataRequestMessageWrapper odataRequestMessageWrapper = this.CreateRequestMessage(method, requestUri, headers, this.RequestInfo.HttpStack, descriptor);
			odataRequestMessageWrapper.SendChunked = sendChunked;
			return odataRequestMessageWrapper;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00026850 File Offset: 0x00024A50
		private IEnumerable<string> SetupMediaResourceRequest(HeaderCollection headers, DataServiceSaveStream saveStream, string etag)
		{
			this.mediaResourceRequestStream = saveStream.Stream;
			headers.SetHeaders(from h in saveStream.Args.Headers
			where !string.Equals(h.Key, "Accept", StringComparison.OrdinalIgnoreCase)
			select h);
			Dictionary<string, string> headers2 = saveStream.Args.Headers;
			List<string> list;
			if (headers2.ContainsKey("Accept"))
			{
				list = new List<string>(headers2.Count - 1);
				list.AddRange(from h in headers2.Keys
				where !string.Equals(h, "Accept", StringComparison.OrdinalIgnoreCase)
				select h);
			}
			else
			{
				list = headers2.Keys.ToList<string>();
			}
			if (etag != null)
			{
				headers.SetHeader("If-Match", etag);
				list.Add("If-Match");
			}
			return list;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0002691C File Offset: 0x00024B1C
		private void HandleOperationException(InvalidOperationException e, IODataResponseMessage response)
		{
			Descriptor descriptor = this.ChangedEntries[this.entryIndex];
			HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
			Version responseVersion = null;
			HeaderCollection headerCollection;
			if (response != null)
			{
				headerCollection = new HeaderCollection(response);
				statusCode = (HttpStatusCode)response.StatusCode;
				base.HandleOperationResponseHeaders(statusCode, headerCollection);
				e = BaseSaveResult.HandleResponse(this.RequestInfo, statusCode, response.GetHeader("DataServiceVersion"), new Func<Stream>(response.GetStream), false, out responseVersion);
			}
			else
			{
				headerCollection = new HeaderCollection();
				headerCollection.SetHeader("Content-Type", "text/plain");
				if (e.GetType() != typeof(DataServiceClientException))
				{
					e = new DataServiceClientException(e.Message, e);
				}
			}
			this.cachedResponses.Add(new SaveResult.CachedResponse(descriptor, headerCollection, statusCode, responseVersion, null, e));
			this.perRequest = null;
			this.CheckContinueOnError();
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x000269E7 File Offset: 0x00024BE7
		private void CheckContinueOnError()
		{
			if (!Util.IsFlagSet(this.Options, SaveChangesOptions.ContinueOnError))
			{
				base.SetCompleted();
				return;
			}
			this.streamRequestKind = BaseSaveResult.StreamRequestKind.None;
			this.ChangedEntries[this.entryIndex].ContentGeneratedForSave = true;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00026A1C File Offset: 0x00024C1C
		private void HandleOperationResponseData(IODataResponseMessage response)
		{
			using (Stream stream = response.GetStream())
			{
				if (stream != null)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						if (WebUtil.CopyStream(stream, memoryStream, ref this.buildBatchBuffer) != 0L)
						{
							memoryStream.Position = 0L;
							this.HandleOperationResponseData(response, memoryStream);
						}
						else
						{
							this.HandleOperationResponseData(response, null);
						}
					}
				}
			}
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00026AB0 File Offset: 0x00024CB0
		private void HandleOperationResponseData(IODataResponseMessage responseMsg, Stream responseStream)
		{
			Descriptor descriptor = this.ChangedEntries[this.entryIndex];
			MaterializerEntry materializerEntry = null;
			Version responseVersion;
			Exception ex = BaseSaveResult.HandleResponse(this.RequestInfo, (HttpStatusCode)responseMsg.StatusCode, responseMsg.GetHeader("DataServiceVersion"), () => responseStream, false, out responseVersion);
			HeaderCollection headers = new HeaderCollection(responseMsg);
			if (responseStream != null && descriptor.DescriptorKind == DescriptorKind.Entity && ex == null)
			{
				EntityDescriptor entityDescriptor = (EntityDescriptor)descriptor;
				if (entityDescriptor.State != EntityStates.Added && entityDescriptor.StreamState != EntityStates.Added && entityDescriptor.State != EntityStates.Modified)
				{
					if (entityDescriptor.StreamState != EntityStates.Modified)
					{
						goto IL_10F;
					}
				}
				try
				{
					ResponseInfo responseInfo = base.CreateResponseInfo(entityDescriptor);
					HttpWebResponseMessage message = new HttpWebResponseMessage(headers, responseMsg.StatusCode, () => responseStream);
					materializerEntry = ODataReaderEntityMaterializer.ParseSingleEntityPayload(message, responseInfo, entityDescriptor.Entity.GetType());
					entityDescriptor.TransientEntityDescriptor = materializerEntry.EntityDescriptor;
				}
				catch (Exception ex2)
				{
					ex = ex2;
					if (!CommonUtil.IsCatchableExceptionType(ex2))
					{
						throw;
					}
				}
			}
			IL_10F:
			this.cachedResponses.Add(new SaveResult.CachedResponse(descriptor, headers, (HttpStatusCode)responseMsg.StatusCode, responseVersion, materializerEntry, ex));
			if (ex != null)
			{
				descriptor.SaveError = ex;
			}
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00026C04 File Offset: 0x00024E04
		private ODataRequestMessageWrapper CreateNamedStreamRequest(StreamDescriptor namedStreamInfo)
		{
			Uri latestEditLink = namedStreamInfo.GetLatestEditLink();
			if (latestEditLink == null)
			{
				throw Error.InvalidOperation(Strings.Context_SetSaveStreamWithoutNamedStreamEditLink(namedStreamInfo.Name));
			}
			HeaderCollection headers = new HeaderCollection();
			IEnumerable<string> headerNames = this.SetupMediaResourceRequest(headers, namedStreamInfo.SaveStream, namedStreamInfo.GetLatestETag());
			ODataRequestMessageWrapper odataRequestMessageWrapper = this.CreateMediaResourceRequest(latestEditLink, "PUT", Util.DataServiceVersion3, true, false, headers, namedStreamInfo);
			odataRequestMessageWrapper.AddHeadersToReset(headerNames);
			return odataRequestMessageWrapper;
		}

		// Token: 0x04000584 RID: 1412
		private readonly List<SaveResult.CachedResponse> cachedResponses;

		// Token: 0x04000585 RID: 1413
		private MemoryStream inMemoryResponseStream;

		// Token: 0x04000586 RID: 1414
		private IODataResponseMessage responseMessage;

		// Token: 0x04000587 RID: 1415
		private SaveResult.CachedResponse cachedResponse;

		// Token: 0x0200011F RID: 287
		private struct CachedResponse
		{
			// Token: 0x0600098B RID: 2443 RVA: 0x00026C6A File Offset: 0x00024E6A
			internal CachedResponse(Descriptor descriptor, HeaderCollection headers, HttpStatusCode statusCode, Version responseVersion, MaterializerEntry entry, Exception exception)
			{
				this.Descriptor = descriptor;
				this.MaterializerEntry = entry;
				this.Exception = exception;
				this.Headers = headers;
				this.StatusCode = statusCode;
				this.Version = responseVersion;
			}

			// Token: 0x0400058A RID: 1418
			public readonly HeaderCollection Headers;

			// Token: 0x0400058B RID: 1419
			public readonly HttpStatusCode StatusCode;

			// Token: 0x0400058C RID: 1420
			public readonly Version Version;

			// Token: 0x0400058D RID: 1421
			public readonly MaterializerEntry MaterializerEntry;

			// Token: 0x0400058E RID: 1422
			public readonly Exception Exception;

			// Token: 0x0400058F RID: 1423
			public readonly Descriptor Descriptor;
		}
	}
}
