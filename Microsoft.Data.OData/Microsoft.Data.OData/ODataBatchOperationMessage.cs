using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x0200025F RID: 607
	internal sealed class ODataBatchOperationMessage : ODataMessage
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x0004AE00 File Offset: 0x00049000
		internal ODataBatchOperationMessage(Func<Stream> contentStreamCreatorFunc, ODataBatchOperationHeaders headers, IODataBatchOperationListener operationListener, IODataUrlResolver urlResolver, bool writing) : base(writing, false, -1L)
		{
			this.contentStreamCreatorFunc = contentStreamCreatorFunc;
			this.operationListener = operationListener;
			this.headers = headers;
			this.urlResolver = urlResolver;
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x0004AE2A File Offset: 0x0004902A
		public override IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				return this.headers ?? Enumerable.Empty<KeyValuePair<string, string>>();
			}
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0004AE3C File Offset: 0x0004903C
		public override string GetHeader(string headerName)
		{
			string result;
			if (this.headers != null && this.headers.TryGetValue(headerName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0004AE64 File Offset: 0x00049064
		public override void SetHeader(string headerName, string headerValue)
		{
			this.VerifyNotCompleted();
			base.VerifyCanSetHeader();
			if (headerValue == null)
			{
				if (this.headers != null)
				{
					this.headers.Remove(headerName);
					return;
				}
			}
			else
			{
				if (this.headers == null)
				{
					this.headers = new ODataBatchOperationHeaders();
				}
				this.headers[headerName] = headerValue;
			}
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0004AEB8 File Offset: 0x000490B8
		public override Stream GetStream()
		{
			this.VerifyNotCompleted();
			this.operationListener.BatchOperationContentStreamRequested();
			Stream result = this.contentStreamCreatorFunc();
			this.PartHeaderProcessingCompleted();
			return result;
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0004AEFC File Offset: 0x000490FC
		public override Task<Stream> GetStreamAsync()
		{
			this.VerifyNotCompleted();
			Task antecedentTask = this.operationListener.BatchOperationContentStreamRequestedAsync();
			Stream contentStream = this.contentStreamCreatorFunc();
			this.PartHeaderProcessingCompleted();
			return antecedentTask.FollowOnSuccessWith((Task task) => contentStream);
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0004AF4C File Offset: 0x0004914C
		internal override TInterface QueryInterface<TInterface>()
		{
			return default(TInterface);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0004AF62 File Offset: 0x00049162
		internal Uri ResolveUrl(Uri baseUri, Uri payloadUri)
		{
			ExceptionUtils.CheckArgumentNotNull<Uri>(payloadUri, "payloadUri");
			if (this.urlResolver != null)
			{
				return this.urlResolver.ResolveUrl(baseUri, payloadUri);
			}
			return null;
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0004AF86 File Offset: 0x00049186
		internal void PartHeaderProcessingCompleted()
		{
			this.contentStreamCreatorFunc = null;
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0004AF8F File Offset: 0x0004918F
		internal void VerifyNotCompleted()
		{
			if (this.contentStreamCreatorFunc == null)
			{
				throw new ODataException(Strings.ODataBatchOperationMessage_VerifyNotCompleted);
			}
		}

		// Token: 0x0400071F RID: 1823
		private readonly IODataBatchOperationListener operationListener;

		// Token: 0x04000720 RID: 1824
		private readonly IODataUrlResolver urlResolver;

		// Token: 0x04000721 RID: 1825
		private Func<Stream> contentStreamCreatorFunc;

		// Token: 0x04000722 RID: 1826
		private ODataBatchOperationHeaders headers;
	}
}
