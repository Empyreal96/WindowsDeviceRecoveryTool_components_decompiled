using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000263 RID: 611
	public sealed class ODataBatchOperationRequestMessage : IODataRequestMessageAsync, IODataRequestMessage, IODataUrlResolver
	{
		// Token: 0x06001426 RID: 5158 RVA: 0x0004B103 File Offset: 0x00049303
		private ODataBatchOperationRequestMessage(Func<Stream> contentStreamCreatorFunc, string method, Uri requestUrl, ODataBatchOperationHeaders headers, IODataBatchOperationListener operationListener, IODataUrlResolver urlResolver, bool writing)
		{
			this.Method = method;
			this.Url = requestUrl;
			this.message = new ODataBatchOperationMessage(contentStreamCreatorFunc, headers, operationListener, urlResolver, writing);
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0004B12D File Offset: 0x0004932D
		public IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				return this.message.Headers;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0004B13A File Offset: 0x0004933A
		// (set) Token: 0x06001429 RID: 5161 RVA: 0x0004B142 File Offset: 0x00049342
		public Uri Url { get; set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0004B14B File Offset: 0x0004934B
		// (set) Token: 0x0600142B RID: 5163 RVA: 0x0004B153 File Offset: 0x00049353
		public string Method { get; set; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0004B15C File Offset: 0x0004935C
		internal ODataBatchOperationMessage OperationMessage
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0004B164 File Offset: 0x00049364
		public string GetHeader(string headerName)
		{
			return this.message.GetHeader(headerName);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0004B172 File Offset: 0x00049372
		public void SetHeader(string headerName, string headerValue)
		{
			this.message.SetHeader(headerName, headerValue);
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0004B181 File Offset: 0x00049381
		public Stream GetStream()
		{
			return this.message.GetStream();
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0004B18E File Offset: 0x0004938E
		public Task<Stream> GetStreamAsync()
		{
			return this.message.GetStreamAsync();
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0004B19B File Offset: 0x0004939B
		Uri IODataUrlResolver.ResolveUrl(Uri baseUri, Uri payloadUri)
		{
			return this.message.ResolveUrl(baseUri, payloadUri);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0004B1C8 File Offset: 0x000493C8
		internal static ODataBatchOperationRequestMessage CreateWriteMessage(Stream outputStream, string method, Uri requestUrl, IODataBatchOperationListener operationListener, IODataUrlResolver urlResolver)
		{
			Func<Stream> contentStreamCreatorFunc = () => ODataBatchUtils.CreateBatchOperationWriteStream(outputStream, operationListener);
			return new ODataBatchOperationRequestMessage(contentStreamCreatorFunc, method, requestUrl, null, operationListener, urlResolver, true);
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0004B22C File Offset: 0x0004942C
		internal static ODataBatchOperationRequestMessage CreateReadMessage(ODataBatchReaderStream batchReaderStream, string method, Uri requestUrl, ODataBatchOperationHeaders headers, IODataBatchOperationListener operationListener, IODataUrlResolver urlResolver)
		{
			Func<Stream> contentStreamCreatorFunc = () => ODataBatchUtils.CreateBatchOperationReadStream(batchReaderStream, headers, operationListener);
			return new ODataBatchOperationRequestMessage(contentStreamCreatorFunc, method, requestUrl, headers, operationListener, urlResolver, false);
		}

		// Token: 0x04000725 RID: 1829
		private readonly ODataBatchOperationMessage message;
	}
}
