using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000261 RID: 609
	public sealed class ODataBatchOperationResponseMessage : IODataResponseMessageAsync, IODataResponseMessage, IODataUrlResolver
	{
		// Token: 0x06001419 RID: 5145 RVA: 0x0004AFA4 File Offset: 0x000491A4
		private ODataBatchOperationResponseMessage(Func<Stream> contentStreamCreatorFunc, ODataBatchOperationHeaders headers, IODataBatchOperationListener operationListener, IODataUrlResolver urlResolver, bool writing)
		{
			this.message = new ODataBatchOperationMessage(contentStreamCreatorFunc, headers, operationListener, urlResolver, writing);
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0004AFBE File Offset: 0x000491BE
		// (set) Token: 0x0600141B RID: 5147 RVA: 0x0004AFC6 File Offset: 0x000491C6
		public int StatusCode
		{
			get
			{
				return this.statusCode;
			}
			set
			{
				this.message.VerifyNotCompleted();
				this.statusCode = value;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0004AFDA File Offset: 0x000491DA
		public IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				return this.message.Headers;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0004AFE7 File Offset: 0x000491E7
		internal ODataBatchOperationMessage OperationMessage
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0004AFEF File Offset: 0x000491EF
		public string GetHeader(string headerName)
		{
			return this.message.GetHeader(headerName);
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0004AFFD File Offset: 0x000491FD
		public void SetHeader(string headerName, string headerValue)
		{
			this.message.SetHeader(headerName, headerValue);
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0004B00C File Offset: 0x0004920C
		public Stream GetStream()
		{
			return this.message.GetStream();
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0004B019 File Offset: 0x00049219
		public Task<Stream> GetStreamAsync()
		{
			return this.message.GetStreamAsync();
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0004B026 File Offset: 0x00049226
		Uri IODataUrlResolver.ResolveUrl(Uri baseUri, Uri payloadUri)
		{
			return this.message.ResolveUrl(baseUri, payloadUri);
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0004B050 File Offset: 0x00049250
		internal static ODataBatchOperationResponseMessage CreateWriteMessage(Stream outputStream, IODataBatchOperationListener operationListener, IODataUrlResolver urlResolver)
		{
			Func<Stream> contentStreamCreatorFunc = () => ODataBatchUtils.CreateBatchOperationWriteStream(outputStream, operationListener);
			return new ODataBatchOperationResponseMessage(contentStreamCreatorFunc, null, operationListener, urlResolver, true);
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0004B0B0 File Offset: 0x000492B0
		internal static ODataBatchOperationResponseMessage CreateReadMessage(ODataBatchReaderStream batchReaderStream, int statusCode, ODataBatchOperationHeaders headers, IODataBatchOperationListener operationListener, IODataUrlResolver urlResolver)
		{
			Func<Stream> contentStreamCreatorFunc = () => ODataBatchUtils.CreateBatchOperationReadStream(batchReaderStream, headers, operationListener);
			return new ODataBatchOperationResponseMessage(contentStreamCreatorFunc, headers, operationListener, urlResolver, false)
			{
				statusCode = statusCode
			};
		}

		// Token: 0x04000723 RID: 1827
		private readonly ODataBatchOperationMessage message;

		// Token: 0x04000724 RID: 1828
		private int statusCode;
	}
}
