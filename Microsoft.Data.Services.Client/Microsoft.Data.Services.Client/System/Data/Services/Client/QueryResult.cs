using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000068 RID: 104
	internal class QueryResult : BaseAsyncResult
	{
		// Token: 0x0600036C RID: 876 RVA: 0x0000EC42 File Offset: 0x0000CE42
		internal QueryResult(object source, string method, DataServiceRequest serviceRequest, ODataRequestMessageWrapper request, RequestInfo requestInfo, AsyncCallback callback, object state) : base(source, method, callback, state)
		{
			this.ServiceRequest = serviceRequest;
			this.Request = request;
			this.RequestInfo = requestInfo;
			base.Abortable = request;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000EC6F File Offset: 0x0000CE6F
		internal QueryResult(object source, string method, DataServiceRequest serviceRequest, ODataRequestMessageWrapper request, RequestInfo requestInfo, AsyncCallback callback, object state, ContentStream requestContentStream) : this(source, method, serviceRequest, request, requestInfo, callback, state)
		{
			this.requestContentStream = requestContentStream;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000EC8A File Offset: 0x0000CE8A
		internal long ContentLength
		{
			get
			{
				return this.contentLength;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000EC92 File Offset: 0x0000CE92
		internal string ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000EC9A File Offset: 0x0000CE9A
		internal HttpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
		internal static QueryResult EndExecuteQuery<TElement>(object source, string method, IAsyncResult asyncResult)
		{
			QueryResult queryResult = null;
			try
			{
				queryResult = BaseAsyncResult.EndExecute<QueryResult>(source, method, asyncResult);
			}
			catch (InvalidOperationException ex)
			{
				queryResult = (asyncResult as QueryResult);
				QueryOperationResponse response = queryResult.GetResponse<TElement>(MaterializeAtom.EmptyResults);
				if (response != null)
				{
					response.Error = ex;
					throw new DataServiceQueryException(Strings.DataServiceException_GeneralError, ex, response);
				}
				throw;
			}
			return queryResult;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000ECFC File Offset: 0x0000CEFC
		internal Stream GetResponseStream()
		{
			return this.outputResponseStream;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000ED04 File Offset: 0x0000CF04
		internal void BeginExecuteQuery()
		{
			BaseAsyncResult.PerRequest perRequest = new BaseAsyncResult.PerRequest();
			BaseAsyncResult.AsyncStateBag state = new BaseAsyncResult.AsyncStateBag(perRequest);
			perRequest.Request = this.Request;
			this.perRequest = perRequest;
			try
			{
				IAsyncResult asyncResult;
				if (this.requestContentStream != null && this.requestContentStream.Stream != null)
				{
					if (this.requestContentStream.IsKnownMemoryStream)
					{
						this.Request.SetContentLengthHeader();
					}
					this.perRequest.RequestContentStream = this.requestContentStream;
					asyncResult = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(this.Request.BeginGetRequestStream), new AsyncCallback(base.AsyncEndGetRequestStream), state);
				}
				else
				{
					asyncResult = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(this.Request.BeginGetResponse), new AsyncCallback(this.AsyncEndGetResponse), state);
				}
				perRequest.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
				base.SetCompletedSynchronously(asyncResult.CompletedSynchronously);
			}
			catch (Exception e)
			{
				base.HandleFailure(e);
				throw;
			}
			finally
			{
				this.HandleCompleted(perRequest);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000EE08 File Offset: 0x0000D008
		internal void ExecuteQuery()
		{
			try
			{
				if (this.requestContentStream != null && this.requestContentStream.Stream != null)
				{
					this.Request.SetRequestStream(this.requestContentStream);
				}
				IODataResponseMessage syncronousResponse = this.RequestInfo.GetSyncronousResponse(this.Request, true);
				this.SetHttpWebResponse(Util.NullCheck<IODataResponseMessage>(syncronousResponse, InternalError.InvalidGetResponse));
				if (HttpStatusCode.NoContent != this.StatusCode)
				{
					using (Stream stream = this.responseMessage.GetStream())
					{
						if (stream != null)
						{
							Stream asyncResponseStreamCopy = this.GetAsyncResponseStreamCopy();
							this.outputResponseStream = asyncResponseStreamCopy;
							byte[] asyncResponseStreamCopyBuffer = this.GetAsyncResponseStreamCopyBuffer();
							long num = WebUtil.CopyStream(stream, asyncResponseStreamCopy, ref asyncResponseStreamCopyBuffer);
							if (this.responseStreamOwner)
							{
								if (0L == num)
								{
									this.outputResponseStream = null;
								}
								else if (asyncResponseStreamCopy.Position < asyncResponseStreamCopy.Length)
								{
									((MemoryStream)asyncResponseStreamCopy).SetLength(asyncResponseStreamCopy.Position);
								}
							}
							this.PutAsyncResponseStreamCopyBuffer(asyncResponseStreamCopyBuffer);
						}
					}
				}
			}
			catch (Exception e)
			{
				base.HandleFailure(e);
				throw;
			}
			finally
			{
				base.SetCompleted();
				this.CompletedRequest();
			}
			if (base.Failure != null)
			{
				throw base.Failure;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000EF38 File Offset: 0x0000D138
		internal QueryOperationResponse<TElement> GetResponse<TElement>(MaterializeAtom results)
		{
			if (this.responseMessage != null)
			{
				HeaderCollection headers = new HeaderCollection(this.responseMessage);
				return new QueryOperationResponse<TElement>(headers, this.ServiceRequest, results)
				{
					StatusCode = this.responseMessage.StatusCode
				};
			}
			return null;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000EF7C File Offset: 0x0000D17C
		internal QueryOperationResponse GetResponseWithType(MaterializeAtom results, Type elementType)
		{
			if (this.responseMessage != null)
			{
				HeaderCollection headers = new HeaderCollection(this.responseMessage);
				QueryOperationResponse instance = QueryOperationResponse.GetInstance(elementType, headers, this.ServiceRequest, results);
				instance.StatusCode = this.responseMessage.StatusCode;
				return instance;
			}
			return null;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000EFC0 File Offset: 0x0000D1C0
		internal MaterializeAtom GetMaterializer(ProjectionPlan plan)
		{
			MaterializeAtom result;
			if (HttpStatusCode.NoContent != this.StatusCode)
			{
				result = this.CreateMaterializer(plan, ODataPayloadKind.Unsupported);
			}
			else
			{
				result = MaterializeAtom.EmptyResults;
			}
			return result;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
		internal QueryOperationResponse<TElement> ProcessResult<TElement>(ProjectionPlan plan)
		{
			MaterializeAtom results = this.CreateMaterializer(plan, this.ServiceRequest.PayloadKind);
			return this.GetResponse<TElement>(results);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000F018 File Offset: 0x0000D218
		protected override void CompletedRequest()
		{
			byte[] array = this.asyncStreamCopyBuffer;
			this.asyncStreamCopyBuffer = null;
			if (array != null && !this.usingBuffer)
			{
				this.PutAsyncResponseStreamCopyBuffer(array);
			}
			if (this.responseStreamOwner && this.outputResponseStream != null)
			{
				this.outputResponseStream.Position = 0L;
			}
			if (this.responseMessage != null)
			{
				WebUtil.DisposeMessage(this.responseMessage);
				Version version;
				Exception ex = BaseSaveResult.HandleResponse(this.RequestInfo, this.StatusCode, this.responseMessage.GetHeader("DataServiceVersion"), new Func<Stream>(this.GetResponseStream), false, out version);
				if (ex != null)
				{
					base.HandleFailure(ex);
					return;
				}
				this.responseInfo = this.CreateResponseInfo();
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
		protected virtual ResponseInfo CreateResponseInfo()
		{
			return this.RequestInfo.GetDeserializationInfo(null);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000F0E4 File Offset: 0x0000D2E4
		protected virtual Stream GetAsyncResponseStreamCopy()
		{
			this.responseStreamOwner = true;
			long num = this.contentLength;
			if (0L < num && num <= 2147483647L)
			{
				return new MemoryStream((int)num);
			}
			return new MemoryStream();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000F11A File Offset: 0x0000D31A
		protected virtual byte[] GetAsyncResponseStreamCopyBuffer()
		{
			return Interlocked.Exchange<byte[]>(ref QueryResult.reusableAsyncCopyBuffer, null) ?? new byte[8000];
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000F135 File Offset: 0x0000D335
		protected virtual void PutAsyncResponseStreamCopyBuffer(byte[] buffer)
		{
			QueryResult.reusableAsyncCopyBuffer = buffer;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000F140 File Offset: 0x0000D340
		protected virtual void SetHttpWebResponse(IODataResponseMessage response)
		{
			this.responseMessage = response;
			this.statusCode = (HttpStatusCode)response.StatusCode;
			string header = response.GetHeader("Content-Length");
			if (header != null)
			{
				this.contentLength = (long)int.Parse(header, CultureInfo.InvariantCulture);
			}
			else
			{
				this.contentLength = -1L;
			}
			this.contentType = response.GetHeader("Content-Type");
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000F19C File Offset: 0x0000D39C
		protected override void HandleCompleted(BaseAsyncResult.PerRequest pereq)
		{
			if (pereq != null)
			{
				base.SetCompletedSynchronously(pereq.RequestCompletedSynchronously);
				if (pereq.RequestCompleted)
				{
					Interlocked.CompareExchange<BaseAsyncResult.PerRequest>(ref this.perRequest, null, pereq);
					pereq.Dispose();
				}
			}
			base.HandleCompleted();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000F1D0 File Offset: 0x0000D3D0
		protected override void AsyncEndGetResponse(IAsyncResult asyncResult)
		{
			BaseAsyncResult.AsyncStateBag asyncStateBag = asyncResult.AsyncState as BaseAsyncResult.AsyncStateBag;
			BaseAsyncResult.PerRequest perRequest = (asyncStateBag == null) ? null : asyncStateBag.PerRequest;
			try
			{
				if (base.IsAborted)
				{
					if (perRequest != null)
					{
						perRequest.SetComplete();
					}
					base.SetCompleted();
				}
				else
				{
					this.CompleteCheck(perRequest, InternalError.InvalidEndGetResponseCompleted);
					perRequest.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
					base.SetCompletedSynchronously(asyncResult.CompletedSynchronously);
					ODataRequestMessageWrapper request = Util.NullCheck<ODataRequestMessageWrapper>(perRequest.Request, InternalError.InvalidEndGetResponseRequest);
					IODataResponseMessage iodataResponseMessage = this.RequestInfo.EndGetResponse(request, asyncResult);
					perRequest.ResponseMessage = Util.NullCheck<IODataResponseMessage>(iodataResponseMessage, InternalError.InvalidEndGetResponseResponse);
					this.SetHttpWebResponse(perRequest.ResponseMessage);
					Stream stream = null;
					if (204 != iodataResponseMessage.StatusCode)
					{
						stream = iodataResponseMessage.GetStream();
						perRequest.ResponseStream = stream;
					}
					if (stream != null && stream.CanRead)
					{
						if (this.outputResponseStream == null)
						{
							this.outputResponseStream = Util.NullCheck<Stream>(this.GetAsyncResponseStreamCopy(), InternalError.InvalidAsyncResponseStreamCopy);
						}
						if (this.asyncStreamCopyBuffer == null)
						{
							this.asyncStreamCopyBuffer = Util.NullCheck<byte[]>(this.GetAsyncResponseStreamCopyBuffer(), InternalError.InvalidAsyncResponseStreamCopyBuffer);
						}
						this.ReadResponseStream(asyncStateBag);
					}
					else
					{
						perRequest.SetComplete();
						base.SetCompleted();
					}
				}
			}
			catch (Exception e)
			{
				if (base.HandleFailure(e))
				{
					throw;
				}
			}
			finally
			{
				this.HandleCompleted(perRequest);
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000F31C File Offset: 0x0000D51C
		protected override void CompleteCheck(BaseAsyncResult.PerRequest pereq, InternalError errorcode)
		{
			if (pereq == null || ((pereq.RequestCompleted || base.IsCompletedInternally) && !base.IsAborted && !pereq.RequestAborted))
			{
				Error.ThrowInternalError(errorcode);
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000F348 File Offset: 0x0000D548
		private void ReadResponseStream(BaseAsyncResult.AsyncStateBag asyncStateBag)
		{
			BaseAsyncResult.PerRequest perRequest = asyncStateBag.PerRequest;
			byte[] array = this.asyncStreamCopyBuffer;
			Stream responseStream = perRequest.ResponseStream;
			IAsyncResult asyncResult;
			do
			{
				int offset = 0;
				int length = array.Length;
				this.usingBuffer = true;
				asyncResult = BaseAsyncResult.InvokeAsync(new BaseAsyncResult.AsyncAction(responseStream.BeginRead), array, offset, length, new AsyncCallback(this.AsyncEndRead), asyncStateBag);
				perRequest.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
				base.SetCompletedSynchronously(asyncResult.CompletedSynchronously);
			}
			while (asyncResult.CompletedSynchronously && !perRequest.RequestCompleted && !base.IsCompletedInternally && responseStream.CanRead);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000F3D8 File Offset: 0x0000D5D8
		private void AsyncEndRead(IAsyncResult asyncResult)
		{
			BaseAsyncResult.AsyncStateBag asyncStateBag = asyncResult.AsyncState as BaseAsyncResult.AsyncStateBag;
			BaseAsyncResult.PerRequest perRequest = (asyncStateBag == null) ? null : asyncStateBag.PerRequest;
			try
			{
				this.CompleteCheck(perRequest, InternalError.InvalidEndReadCompleted);
				perRequest.SetRequestCompletedSynchronously(asyncResult.CompletedSynchronously);
				base.SetCompletedSynchronously(asyncResult.CompletedSynchronously);
				Stream stream = Util.NullCheck<Stream>(perRequest.ResponseStream, InternalError.InvalidEndReadStream);
				Stream stream2 = Util.NullCheck<Stream>(this.outputResponseStream, InternalError.InvalidEndReadCopy);
				byte[] array = Util.NullCheck<byte[]>(this.asyncStreamCopyBuffer, InternalError.InvalidEndReadBuffer);
				int num = stream.EndRead(asyncResult);
				this.usingBuffer = false;
				if (0 < num)
				{
					stream2.Write(array, 0, num);
				}
				if (0 < num && 0 < array.Length && stream.CanRead)
				{
					if (!asyncResult.CompletedSynchronously)
					{
						this.ReadResponseStream(asyncStateBag);
					}
				}
				else
				{
					if (stream2.Position < stream2.Length)
					{
						((MemoryStream)stream2).SetLength(stream2.Position);
					}
					perRequest.SetComplete();
					base.SetCompleted();
				}
			}
			catch (Exception e)
			{
				if (base.HandleFailure(e))
				{
					throw;
				}
			}
			finally
			{
				this.HandleCompleted(perRequest);
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000F4F8 File Offset: 0x0000D6F8
		private MaterializeAtom CreateMaterializer(ProjectionPlan plan, ODataPayloadKind payloadKind)
		{
			QueryComponents queryComponents = this.ServiceRequest.QueryComponents(this.responseInfo.Model);
			if (plan != null || queryComponents.Projection != null)
			{
				this.RequestInfo.TypeResolver.IsProjectionRequest();
			}
			HttpWebResponseMessage message = new HttpWebResponseMessage(new HeaderCollection(this.responseMessage), this.responseMessage.StatusCode, new Func<Stream>(this.GetResponseStream));
			return DataServiceRequest.Materialize(this.responseInfo, queryComponents, plan, this.ContentType, message, payloadKind);
		}

		// Token: 0x0400029A RID: 666
		internal readonly DataServiceRequest ServiceRequest;

		// Token: 0x0400029B RID: 667
		internal readonly RequestInfo RequestInfo;

		// Token: 0x0400029C RID: 668
		internal readonly ODataRequestMessageWrapper Request;

		// Token: 0x0400029D RID: 669
		private static byte[] reusableAsyncCopyBuffer;

		// Token: 0x0400029E RID: 670
		private ContentStream requestContentStream;

		// Token: 0x0400029F RID: 671
		private IODataResponseMessage responseMessage;

		// Token: 0x040002A0 RID: 672
		private ResponseInfo responseInfo;

		// Token: 0x040002A1 RID: 673
		private byte[] asyncStreamCopyBuffer;

		// Token: 0x040002A2 RID: 674
		private Stream outputResponseStream;

		// Token: 0x040002A3 RID: 675
		private string contentType;

		// Token: 0x040002A4 RID: 676
		private long contentLength;

		// Token: 0x040002A5 RID: 677
		private HttpStatusCode statusCode;

		// Token: 0x040002A6 RID: 678
		private bool responseStreamOwner;

		// Token: 0x040002A7 RID: 679
		private bool usingBuffer;
	}
}
