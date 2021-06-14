using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x0200028B RID: 651
	internal sealed class ODataResponseMessage : ODataMessage, IODataResponseMessageAsync, IODataResponseMessage
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x0005051B File Offset: 0x0004E71B
		internal ODataResponseMessage(IODataResponseMessage responseMessage, bool writing, bool disableMessageStreamDisposal, long maxMessageSize) : base(writing, disableMessageStreamDisposal, maxMessageSize)
		{
			this.responseMessage = responseMessage;
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060015ED RID: 5613 RVA: 0x0005052E File Offset: 0x0004E72E
		// (set) Token: 0x060015EE RID: 5614 RVA: 0x0005053B File Offset: 0x0004E73B
		public int StatusCode
		{
			get
			{
				return this.responseMessage.StatusCode;
			}
			set
			{
				throw new ODataException(Strings.ODataMessage_MustNotModifyMessage);
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x00050547 File Offset: 0x0004E747
		public override IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				return this.responseMessage.Headers;
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00050554 File Offset: 0x0004E754
		public override string GetHeader(string headerName)
		{
			return this.responseMessage.GetHeader(headerName);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x00050562 File Offset: 0x0004E762
		public override void SetHeader(string headerName, string headerValue)
		{
			base.VerifyCanSetHeader();
			this.responseMessage.SetHeader(headerName, headerValue);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00050577 File Offset: 0x0004E777
		public override Stream GetStream()
		{
			return base.GetStream(new Func<Stream>(this.responseMessage.GetStream), false);
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00050594 File Offset: 0x0004E794
		public override Task<Stream> GetStreamAsync()
		{
			IODataResponseMessageAsync iodataResponseMessageAsync = this.responseMessage as IODataResponseMessageAsync;
			if (iodataResponseMessageAsync == null)
			{
				throw new ODataException(Strings.ODataResponseMessage_AsyncNotAvailable);
			}
			return base.GetStreamAsync(new Func<Task<Stream>>(iodataResponseMessageAsync.GetStreamAsync), false);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000505CF File Offset: 0x0004E7CF
		internal override TInterface QueryInterface<TInterface>()
		{
			return this.responseMessage as TInterface;
		}

		// Token: 0x0400085E RID: 2142
		private readonly IODataResponseMessage responseMessage;
	}
}
