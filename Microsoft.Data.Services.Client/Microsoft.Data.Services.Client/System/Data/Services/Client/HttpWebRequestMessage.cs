using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200008B RID: 139
	public class HttpWebRequestMessage : DataServiceClientRequestMessage
	{
		// Token: 0x060004EC RID: 1260 RVA: 0x00013E18 File Offset: 0x00012018
		public HttpWebRequestMessage(DataServiceClientRequestMessageArgs args) : base(args.ActualMethod)
		{
			Util.CheckArgumentNull<DataServiceClientRequestMessageArgs>(args, "args");
			this.effectiveHttpMethod = args.Method;
			this.requestUrl = args.RequestUri;
			this.httpRequest = HttpWebRequestMessage.CreateRequest(this.ActualMethod, this.Url, args);
			foreach (KeyValuePair<string, string> keyValuePair in args.Headers)
			{
				this.SetHeader(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00013EBC File Offset: 0x000120BC
		internal HttpWebRequestMessage(DataServiceClientRequestMessageArgs args, RequestInfo requestInfo) : this(args)
		{
			this.requestInfo = requestInfo;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00013ECC File Offset: 0x000120CC
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x00013ED4 File Offset: 0x000120D4
		public override Uri Url
		{
			get
			{
				return this.requestUrl;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00013EDB File Offset: 0x000120DB
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x00013EE3 File Offset: 0x000120E3
		public override string Method
		{
			get
			{
				return this.effectiveHttpMethod;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00013EEC File Offset: 0x000120EC
		public override IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(this.httpRequest.Headers.Count);
				foreach (string text in this.httpRequest.Headers.AllKeys)
				{
					string value = this.httpRequest.Headers[text];
					list.Add(new KeyValuePair<string, string>(text, value));
				}
				return list;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00013F57 File Offset: 0x00012157
		public HttpWebRequest HttpWebRequest
		{
			get
			{
				return this.httpRequest;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00013F5F File Offset: 0x0001215F
		// (set) Token: 0x060004F5 RID: 1269 RVA: 0x00013F6C File Offset: 0x0001216C
		public override ICredentials Credentials
		{
			get
			{
				return this.httpRequest.Credentials;
			}
			set
			{
				this.httpRequest.Credentials = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00013F7A File Offset: 0x0001217A
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00013F88 File Offset: 0x00012188
		public override int Timeout
		{
			get
			{
				return this.httpRequest.Timeout;
			}
			set
			{
				this.httpRequest.Timeout = (int)Math.Min(2147483647.0, new TimeSpan(0, 0, value).TotalMilliseconds);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00013FBF File Offset: 0x000121BF
		// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00013FCC File Offset: 0x000121CC
		public override bool SendChunked
		{
			get
			{
				return this.httpRequest.SendChunked;
			}
			set
			{
				this.httpRequest.SendChunked = value;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00013FDA File Offset: 0x000121DA
		public override string GetHeader(string headerName)
		{
			Util.CheckArgumentNullAndEmpty(headerName, "headerName");
			return HttpWebRequestMessage.GetHeaderValue(this.httpRequest, headerName);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00013FF3 File Offset: 0x000121F3
		public override void SetHeader(string headerName, string headerValue)
		{
			Util.CheckArgumentNullAndEmpty(headerName, "headerName");
			HttpWebRequestMessage.SetHeaderValue(this.httpRequest, headerName, headerValue);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001400D File Offset: 0x0001220D
		public override Stream GetStream()
		{
			if (this.inSendingRequest2Event)
			{
				throw new NotSupportedException(Strings.ODataRequestMessage_GetStreamMethodNotSupported);
			}
			this.FireSendingRequest();
			return this.httpRequest.GetRequestStream();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00014033 File Offset: 0x00012233
		public override void Abort()
		{
			this.httpRequest.Abort();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00014040 File Offset: 0x00012240
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			if (this.inSendingRequest2Event)
			{
				throw new NotSupportedException(Strings.ODataRequestMessage_GetStreamMethodNotSupported);
			}
			this.FireSendingRequest();
			return this.httpRequest.BeginGetRequestStream(callback, state);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00014068 File Offset: 0x00012268
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			return this.httpRequest.EndGetRequestStream(asyncResult);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00014076 File Offset: 0x00012276
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			this.FireSendingRequest();
			return this.httpRequest.BeginGetResponse(callback, state);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001408C File Offset: 0x0001228C
		public override IODataResponseMessage EndGetResponse(IAsyncResult asyncResult)
		{
			IODataResponseMessage result;
			try
			{
				HttpWebResponse httpResponse = (HttpWebResponse)this.httpRequest.EndGetResponse(asyncResult);
				result = new HttpWebResponseMessage(httpResponse);
			}
			catch (WebException webException)
			{
				throw HttpWebRequestMessage.ConvertToDataServiceWebException(webException);
			}
			return result;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000140D0 File Offset: 0x000122D0
		public override IODataResponseMessage GetResponse()
		{
			this.FireSendingRequest();
			IODataResponseMessage result;
			try
			{
				HttpWebResponse httpResponse = (HttpWebResponse)this.httpRequest.GetResponse();
				result = new HttpWebResponseMessage(httpResponse);
			}
			catch (WebException webException)
			{
				throw HttpWebRequestMessage.ConvertToDataServiceWebException(webException);
			}
			return result;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00014118 File Offset: 0x00012318
		internal static void SetHttpWebRequestContentLength(HttpWebRequest httpWebRequest, long contentLength)
		{
			httpWebRequest.ContentLength = contentLength;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00014121 File Offset: 0x00012321
		internal static void SetAcceptCharset(HttpWebRequest httpWebRequest, string headerValue)
		{
			httpWebRequest.Headers["Accept-Charset"] = headerValue;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00014134 File Offset: 0x00012334
		internal static void SetUserAgentHeader(HttpWebRequest httpWebRequest, string headerValue)
		{
			httpWebRequest.UserAgent = headerValue;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001413D File Offset: 0x0001233D
		internal void AddHeadersToReset(IEnumerable<string> headerNames)
		{
			if (this.headersToReset == null)
			{
				this.headersToReset = new List<string>();
			}
			this.headersToReset.AddRange(headerNames);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001415E File Offset: 0x0001235E
		internal void BeforeSendingRequest2Event()
		{
			this.inSendingRequest2Event = true;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00014167 File Offset: 0x00012367
		internal void AfterSendingRequest2Event()
		{
			this.inSendingRequest2Event = false;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00014170 File Offset: 0x00012370
		private static void SetHeaderValues(HttpWebRequestMessage requestMessage, HeaderCollection cachedHeaders, string effectiveHttpMethod)
		{
			bool flag = true;
			HttpWebRequest httpWebRequest = requestMessage.httpRequest;
			string method = requestMessage.Method;
			string contentType = null;
			cachedHeaders.TryGetHeader("Content-Type", out contentType);
			if (string.CompareOrdinal(effectiveHttpMethod, "GET") != 0)
			{
				if (string.CompareOrdinal(effectiveHttpMethod, "DELETE") == 0)
				{
					httpWebRequest.ContentType = null;
					HttpWebRequestMessage.SetHttpWebRequestContentLength(httpWebRequest, 0L);
				}
				else
				{
					httpWebRequest.ContentType = contentType;
				}
				if (requestMessage.requestInfo.UsePostTunneling && string.CompareOrdinal(effectiveHttpMethod, "POST") != 0)
				{
					httpWebRequest.Headers["X-HTTP-Method"] = effectiveHttpMethod;
					method = "POST";
					flag = false;
				}
			}
			httpWebRequest.Headers.Remove(HttpRequestHeader.IfMatch);
			if (flag)
			{
				httpWebRequest.Headers.Remove("X-HTTP-Method");
			}
			httpWebRequest.Method = method;
			if (requestMessage.headersToReset != null)
			{
				foreach (string headerName in requestMessage.headersToReset)
				{
					HttpWebRequestMessage.SetHeaderValue(httpWebRequest, headerName, cachedHeaders.GetHeader(headerName));
				}
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00014284 File Offset: 0x00012484
		private static HttpWebRequest CreateRequest(string method, Uri requestUrl, DataServiceClientRequestMessageArgs args)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Method = method;
			return httpWebRequest;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000142AC File Offset: 0x000124AC
		private static void SetHeaderValue(HttpWebRequest request, string headerName, string headerValue)
		{
			if (string.Equals(headerName, "Accept", StringComparison.OrdinalIgnoreCase))
			{
				request.Accept = headerValue;
				return;
			}
			if (string.Equals(headerName, "Content-Type", StringComparison.OrdinalIgnoreCase))
			{
				request.ContentType = headerValue;
				return;
			}
			if (string.Equals(headerName, "Content-Length", StringComparison.OrdinalIgnoreCase))
			{
				HttpWebRequestMessage.SetHttpWebRequestContentLength(request, long.Parse(headerValue, CultureInfo.InvariantCulture));
				return;
			}
			if (string.Equals(headerName, "User-Agent", StringComparison.OrdinalIgnoreCase))
			{
				HttpWebRequestMessage.SetUserAgentHeader(request, headerValue);
				return;
			}
			if (string.Equals(headerName, "Accept-Charset", StringComparison.OrdinalIgnoreCase))
			{
				HttpWebRequestMessage.SetAcceptCharset(request, headerValue);
				return;
			}
			request.Headers[headerName] = headerValue;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00014340 File Offset: 0x00012540
		private static string GetHeaderValue(HttpWebRequest request, string headerName)
		{
			if (string.Equals(headerName, "Accept", StringComparison.OrdinalIgnoreCase))
			{
				return request.Accept;
			}
			if (string.Equals(headerName, "Content-Type", StringComparison.OrdinalIgnoreCase))
			{
				return request.ContentType;
			}
			if (string.Equals(headerName, "Content-Length", StringComparison.OrdinalIgnoreCase))
			{
				return request.ContentLength.ToString(CultureInfo.InvariantCulture);
			}
			return request.Headers[headerName];
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000143A8 File Offset: 0x000125A8
		private static DataServiceTransportException ConvertToDataServiceWebException(WebException webException)
		{
			HttpWebResponseMessage response = null;
			if (webException.Response != null)
			{
				HttpWebResponse httpResponse = (HttpWebResponse)webException.Response;
				response = new HttpWebResponseMessage(httpResponse);
			}
			return new DataServiceTransportException(response, webException);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000143DC File Offset: 0x000125DC
		private void FireSendingRequest()
		{
			if (this.fireSendingRequestMethodCalled || this.requestInfo == null)
			{
				return;
			}
			this.fireSendingRequestMethodCalled = true;
			HeaderCollection headerCollection = null;
			if (this.requestInfo.HasSendingRequestEventHandlers)
			{
				headerCollection = new HeaderCollection();
				foreach (KeyValuePair<string, string> keyValuePair in this.Headers)
				{
					headerCollection.SetHeader(keyValuePair.Key, keyValuePair.Value);
				}
				headerCollection.SetHeader("Content-Length", this.httpRequest.ContentLength.ToString(CultureInfo.InvariantCulture));
			}
			if (this.requestInfo.HasSendingRequestEventHandlers)
			{
				WebHeaderCollection headers = this.httpRequest.Headers;
				SendingRequestEventArgs sendingRequestEventArgs = new SendingRequestEventArgs(this.httpRequest, headers);
				this.requestInfo.FireSendingRequest(sendingRequestEventArgs);
				if (!object.ReferenceEquals(sendingRequestEventArgs.Request, this.httpRequest))
				{
					this.httpRequest = (HttpWebRequest)sendingRequestEventArgs.Request;
				}
				HttpWebRequestMessage.SetHeaderValues(this, headerCollection, this.Method);
			}
		}

		// Token: 0x040002F8 RID: 760
		private readonly Uri requestUrl;

		// Token: 0x040002F9 RID: 761
		private readonly string effectiveHttpMethod;

		// Token: 0x040002FA RID: 762
		private readonly RequestInfo requestInfo;

		// Token: 0x040002FB RID: 763
		private HttpWebRequest httpRequest;

		// Token: 0x040002FC RID: 764
		private List<string> headersToReset;

		// Token: 0x040002FD RID: 765
		private bool fireSendingRequestMethodCalled;

		// Token: 0x040002FE RID: 766
		private bool inSendingRequest2Event;
	}
}
