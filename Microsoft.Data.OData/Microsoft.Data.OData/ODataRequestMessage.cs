using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x0200028C RID: 652
	internal sealed class ODataRequestMessage : ODataMessage, IODataRequestMessageAsync, IODataRequestMessage
	{
		// Token: 0x060015F5 RID: 5621 RVA: 0x000505E1 File Offset: 0x0004E7E1
		internal ODataRequestMessage(IODataRequestMessage requestMessage, bool writing, bool disableMessageStreamDisposal, long maxMessageSize) : base(writing, disableMessageStreamDisposal, maxMessageSize)
		{
			this.requestMessage = requestMessage;
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x000505F4 File Offset: 0x0004E7F4
		// (set) Token: 0x060015F7 RID: 5623 RVA: 0x00050601 File Offset: 0x0004E801
		public Uri Url
		{
			get
			{
				return this.requestMessage.Url;
			}
			set
			{
				throw new ODataException(Strings.ODataMessage_MustNotModifyMessage);
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x0005060D File Offset: 0x0004E80D
		// (set) Token: 0x060015F9 RID: 5625 RVA: 0x0005061A File Offset: 0x0004E81A
		public string Method
		{
			get
			{
				return this.requestMessage.Method;
			}
			set
			{
				throw new ODataException(Strings.ODataMessage_MustNotModifyMessage);
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x00050626 File Offset: 0x0004E826
		public override IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				return this.requestMessage.Headers;
			}
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00050633 File Offset: 0x0004E833
		public override string GetHeader(string headerName)
		{
			return this.requestMessage.GetHeader(headerName);
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00050641 File Offset: 0x0004E841
		public override void SetHeader(string headerName, string headerValue)
		{
			base.VerifyCanSetHeader();
			this.requestMessage.SetHeader(headerName, headerValue);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00050656 File Offset: 0x0004E856
		public override Stream GetStream()
		{
			return base.GetStream(new Func<Stream>(this.requestMessage.GetStream), true);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00050674 File Offset: 0x0004E874
		public override Task<Stream> GetStreamAsync()
		{
			IODataRequestMessageAsync iodataRequestMessageAsync = this.requestMessage as IODataRequestMessageAsync;
			if (iodataRequestMessageAsync == null)
			{
				throw new ODataException(Strings.ODataRequestMessage_AsyncNotAvailable);
			}
			return base.GetStreamAsync(new Func<Task<Stream>>(iodataRequestMessageAsync.GetStreamAsync), true);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x000506AF File Offset: 0x0004E8AF
		internal override TInterface QueryInterface<TInterface>()
		{
			return this.requestMessage as TInterface;
		}

		// Token: 0x0400085F RID: 2143
		private readonly IODataRequestMessage requestMessage;
	}
}
