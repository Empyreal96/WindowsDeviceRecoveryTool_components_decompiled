using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000079 RID: 121
	internal abstract class ODataRequestMessageWrapper
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x00011212 File Offset: 0x0000F412
		protected ODataRequestMessageWrapper(DataServiceClientRequestMessage requestMessage, RequestInfo requestInfo, Descriptor descriptor)
		{
			this.requestMessage = requestMessage;
			this.requestInfo = requestInfo;
			this.Descriptor = descriptor;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0001122F File Offset: 0x0000F42F
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x00011237 File Offset: 0x0000F437
		internal Descriptor Descriptor { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000403 RID: 1027
		internal abstract ContentStream CachedRequestStream { get; }

		// Token: 0x170000FF RID: 255
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x00011240 File Offset: 0x0000F440
		internal bool SendChunked
		{
			set
			{
				this.requestMessage.SendChunked = value;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000405 RID: 1029
		internal abstract bool IsBatchPartRequest { get; }

		// Token: 0x06000406 RID: 1030 RVA: 0x00011250 File Offset: 0x0000F450
		internal static ODataRequestMessageWrapper CreateBatchPartRequestMessage(ODataBatchWriter batchWriter, BuildingRequestEventArgs requestMessageArgs, RequestInfo requestInfo)
		{
			IODataRequestMessage iodataRequestMessage = batchWriter.CreateOperationRequestMessage(requestMessageArgs.Method, requestMessageArgs.RequestUri);
			foreach (KeyValuePair<string, string> keyValuePair in requestMessageArgs.Headers)
			{
				iodataRequestMessage.SetHeader(keyValuePair.Key, keyValuePair.Value);
			}
			InternalODataRequestMessage clientRequestMessage = new InternalODataRequestMessage(iodataRequestMessage, false);
			return new ODataRequestMessageWrapper.InnerBatchRequestMessageWrapper(clientRequestMessage, iodataRequestMessage, requestInfo, requestMessageArgs.Descriptor);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000112DC File Offset: 0x0000F4DC
		internal static ODataRequestMessageWrapper CreateRequestMessageWrapper(BuildingRequestEventArgs requestMessageArgs, RequestInfo requestInfo)
		{
			DataServiceClientRequestMessage dataServiceClientRequestMessage = requestInfo.CreateRequestMessage(requestMessageArgs);
			if (requestInfo.Credentials != null)
			{
				dataServiceClientRequestMessage.Credentials = requestInfo.Credentials;
			}
			if (requestInfo.Timeout != 0)
			{
				dataServiceClientRequestMessage.Timeout = requestInfo.Timeout;
			}
			return new ODataRequestMessageWrapper.TopLevelRequestMessageWrapper(dataServiceClientRequestMessage, requestInfo, requestMessageArgs.Descriptor);
		}

		// Token: 0x06000408 RID: 1032
		internal abstract ODataMessageWriter CreateWriter(ODataMessageWriterSettings writerSettings, bool isParameterPayload);

		// Token: 0x06000409 RID: 1033 RVA: 0x00011326 File Offset: 0x0000F526
		internal void Abort()
		{
			this.requestMessage.Abort();
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00011333 File Offset: 0x0000F533
		internal void SetHeader(string headerName, string headerValue)
		{
			this.requestMessage.SetHeader(headerName, headerValue);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00011342 File Offset: 0x0000F542
		internal IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			return this.requestMessage.BeginGetRequestStream(callback, state);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00011351 File Offset: 0x0000F551
		internal Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			return this.requestMessage.EndGetRequestStream(asyncResult);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00011360 File Offset: 0x0000F560
		internal void SetRequestStream(ContentStream requestStreamContent)
		{
			if (requestStreamContent.IsKnownMemoryStream)
			{
				this.SetContentLengthHeader();
			}
			using (Stream stream = this.requestMessage.GetStream())
			{
				if (requestStreamContent.IsKnownMemoryStream)
				{
					MemoryStream memoryStream = (MemoryStream)requestStreamContent.Stream;
					byte[] buffer = memoryStream.GetBuffer();
					int num = checked((int)memoryStream.Position);
					int count = checked((int)memoryStream.Length) - num;
					stream.Write(buffer, num, count);
				}
				else
				{
					byte[] array = new byte[65536];
					WebUtil.CopyStream(requestStreamContent.Stream, stream, ref array);
				}
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000113F8 File Offset: 0x0000F5F8
		internal IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			return this.requestMessage.BeginGetResponse(callback, state);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00011407 File Offset: 0x0000F607
		internal IODataResponseMessage EndGetResponse(IAsyncResult asyncResult)
		{
			return this.requestMessage.EndGetResponse(asyncResult);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00011415 File Offset: 0x0000F615
		internal IODataResponseMessage GetResponse()
		{
			return this.requestMessage.GetResponse();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00011424 File Offset: 0x0000F624
		internal void SetContentLengthHeader()
		{
			if (this.requestInfo.HasSendingRequestEventHandlers || this.requestInfo.HasSendingRequest2EventHandlers)
			{
				this.SetHeader("Content-Length", this.CachedRequestStream.Stream.Length.ToString(CultureInfo.InvariantCulture));
				if (this.requestInfo.HasSendingRequestEventHandlers)
				{
					this.AddHeadersToReset(new string[]
					{
						"Content-Length"
					});
				}
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00011498 File Offset: 0x0000F698
		internal void AddHeadersToReset(IEnumerable<string> headerNames)
		{
			if (this.requestInfo.HasSendingRequestEventHandlers)
			{
				HttpWebRequestMessage httpWebRequestMessage = this.requestMessage as HttpWebRequestMessage;
				if (httpWebRequestMessage != null)
				{
					httpWebRequestMessage.AddHeadersToReset(headerNames);
				}
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x000114C8 File Offset: 0x0000F6C8
		internal void AddHeadersToReset(params string[] headerNames)
		{
			this.AddHeadersToReset((IEnumerable<string>)headerNames);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000114D6 File Offset: 0x0000F6D6
		internal void FireSendingEventHandlers(Descriptor descriptor)
		{
			this.FireSendingRequest2(descriptor);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000114E0 File Offset: 0x0000F6E0
		internal void FireSendingRequest2(Descriptor descriptor)
		{
			if (this.requestInfo.HasSendingRequest2EventHandlers)
			{
				HttpWebRequestMessage httpWebRequestMessage = this.requestMessage as HttpWebRequestMessage;
				if (httpWebRequestMessage != null)
				{
					httpWebRequestMessage.BeforeSendingRequest2Event();
				}
				try
				{
					this.requestInfo.FireSendingRequest2(new SendingRequest2EventArgs(this.requestMessage, descriptor, this.IsBatchPartRequest));
				}
				finally
				{
					if (httpWebRequestMessage != null)
					{
						httpWebRequestMessage.AfterSendingRequest2Event();
					}
				}
			}
		}

		// Token: 0x040002C0 RID: 704
		private readonly DataServiceClientRequestMessage requestMessage;

		// Token: 0x040002C1 RID: 705
		private readonly RequestInfo requestInfo;

		// Token: 0x0200007A RID: 122
		private class RequestMessageWithCachedStream : IODataRequestMessage
		{
			// Token: 0x06000416 RID: 1046 RVA: 0x00011548 File Offset: 0x0000F748
			internal RequestMessageWithCachedStream(DataServiceClientRequestMessage requestMessage)
			{
				this.requestMessage = requestMessage;
			}

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x06000417 RID: 1047 RVA: 0x00011557 File Offset: 0x0000F757
			public IEnumerable<KeyValuePair<string, string>> Headers
			{
				get
				{
					return this.requestMessage.Headers;
				}
			}

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x06000418 RID: 1048 RVA: 0x00011564 File Offset: 0x0000F764
			// (set) Token: 0x06000419 RID: 1049 RVA: 0x00011571 File Offset: 0x0000F771
			public Uri Url
			{
				get
				{
					return this.requestMessage.Url;
				}
				set
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x0600041A RID: 1050 RVA: 0x00011578 File Offset: 0x0000F778
			// (set) Token: 0x0600041B RID: 1051 RVA: 0x00011585 File Offset: 0x0000F785
			public string Method
			{
				get
				{
					return this.requestMessage.Method;
				}
				set
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001158C File Offset: 0x0000F78C
			internal ContentStream CachedRequestStream
			{
				get
				{
					this.cachedRequestStream.Stream.Position = 0L;
					return this.cachedRequestStream;
				}
			}

			// Token: 0x0600041D RID: 1053 RVA: 0x000115A6 File Offset: 0x0000F7A6
			public string GetHeader(string headerName)
			{
				return this.requestMessage.GetHeader(headerName);
			}

			// Token: 0x0600041E RID: 1054 RVA: 0x000115B4 File Offset: 0x0000F7B4
			public void SetHeader(string headerName, string headerValue)
			{
				this.requestMessage.SetHeader(headerName, headerValue);
			}

			// Token: 0x0600041F RID: 1055 RVA: 0x000115C3 File Offset: 0x0000F7C3
			public Stream GetStream()
			{
				if (this.cachedRequestStream == null)
				{
					this.cachedRequestStream = new ContentStream(new MemoryStream(), true);
				}
				return this.cachedRequestStream.Stream;
			}

			// Token: 0x040002C3 RID: 707
			private readonly DataServiceClientRequestMessage requestMessage;

			// Token: 0x040002C4 RID: 708
			private ContentStream cachedRequestStream;
		}

		// Token: 0x0200007B RID: 123
		private class TopLevelRequestMessageWrapper : ODataRequestMessageWrapper
		{
			// Token: 0x06000420 RID: 1056 RVA: 0x000115E9 File Offset: 0x0000F7E9
			internal TopLevelRequestMessageWrapper(DataServiceClientRequestMessage requestMessage, RequestInfo requestInfo, Descriptor descriptor) : base(requestMessage, requestInfo, descriptor)
			{
				this.messageWithCachedStream = new ODataRequestMessageWrapper.RequestMessageWithCachedStream(this.requestMessage);
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000421 RID: 1057 RVA: 0x00011605 File Offset: 0x0000F805
			internal override bool IsBatchPartRequest
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x06000422 RID: 1058 RVA: 0x00011608 File Offset: 0x0000F808
			internal override ContentStream CachedRequestStream
			{
				get
				{
					return this.messageWithCachedStream.CachedRequestStream;
				}
			}

			// Token: 0x06000423 RID: 1059 RVA: 0x00011615 File Offset: 0x0000F815
			internal override ODataMessageWriter CreateWriter(ODataMessageWriterSettings writerSettings, bool isParameterPayload)
			{
				return this.requestInfo.WriteHelper.CreateWriter(this.messageWithCachedStream, writerSettings, isParameterPayload);
			}

			// Token: 0x040002C5 RID: 709
			private readonly ODataRequestMessageWrapper.RequestMessageWithCachedStream messageWithCachedStream;
		}

		// Token: 0x0200007C RID: 124
		private class InnerBatchRequestMessageWrapper : ODataRequestMessageWrapper
		{
			// Token: 0x06000424 RID: 1060 RVA: 0x0001162F File Offset: 0x0000F82F
			internal InnerBatchRequestMessageWrapper(DataServiceClientRequestMessage clientRequestMessage, IODataRequestMessage odataRequestMessage, RequestInfo requestInfo, Descriptor descriptor) : base(clientRequestMessage, requestInfo, descriptor)
			{
				this.innerBatchRequestMessage = odataRequestMessage;
			}

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x06000425 RID: 1061 RVA: 0x00011642 File Offset: 0x0000F842
			internal override bool IsBatchPartRequest
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000108 RID: 264
			// (get) Token: 0x06000426 RID: 1062 RVA: 0x00011645 File Offset: 0x0000F845
			internal override ContentStream CachedRequestStream
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x06000427 RID: 1063 RVA: 0x0001164C File Offset: 0x0000F84C
			internal override ODataMessageWriter CreateWriter(ODataMessageWriterSettings writerSettings, bool isParameterPayload)
			{
				return this.requestInfo.WriteHelper.CreateWriter(this.innerBatchRequestMessage, writerSettings, isParameterPayload);
			}

			// Token: 0x040002C6 RID: 710
			private readonly IODataRequestMessage innerBatchRequestMessage;
		}
	}
}
