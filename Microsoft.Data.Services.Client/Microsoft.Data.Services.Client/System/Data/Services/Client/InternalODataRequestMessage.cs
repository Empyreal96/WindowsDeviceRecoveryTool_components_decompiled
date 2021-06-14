using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000064 RID: 100
	internal class InternalODataRequestMessage : DataServiceClientRequestMessage
	{
		// Token: 0x06000348 RID: 840 RVA: 0x0000EA9E File Offset: 0x0000CC9E
		internal InternalODataRequestMessage(IODataRequestMessage requestMessage, bool allowGetStream) : base(requestMessage.Method)
		{
			this.requestMessage = requestMessage;
			this.allowGetStream = allowGetStream;
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000EABA File Offset: 0x0000CCBA
		public override IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				return this.HeaderCollection.AsEnumerable();
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000EAC7 File Offset: 0x0000CCC7
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
		public override Uri Url
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

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000EADB File Offset: 0x0000CCDB
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000EAE8 File Offset: 0x0000CCE8
		public override string Method
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000EAEF File Offset: 0x0000CCEF
		// (set) Token: 0x0600034F RID: 847 RVA: 0x0000EAF6 File Offset: 0x0000CCF6
		public override ICredentials Credentials
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000EAFD File Offset: 0x0000CCFD
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0000EB04 File Offset: 0x0000CD04
		public override int Timeout
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000EB0B File Offset: 0x0000CD0B
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0000EB12 File Offset: 0x0000CD12
		public override bool SendChunked
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000EB19 File Offset: 0x0000CD19
		private HeaderCollection HeaderCollection
		{
			get
			{
				if (this.headers == null)
				{
					this.headers = new HeaderCollection(this.requestMessage.Headers);
				}
				return this.headers;
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000EB3F File Offset: 0x0000CD3F
		public override string GetHeader(string headerName)
		{
			return this.HeaderCollection.GetHeader(headerName);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000EB4D File Offset: 0x0000CD4D
		public override void SetHeader(string headerName, string headerValue)
		{
			this.requestMessage.SetHeader(headerName, headerValue);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000EB5C File Offset: 0x0000CD5C
		public override Stream GetStream()
		{
			if (!this.allowGetStream)
			{
				throw new NotImplementedException();
			}
			if (this.cachedRequestStream == null)
			{
				this.cachedRequestStream = this.requestMessage.GetStream();
			}
			return this.cachedRequestStream;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000EB8B File Offset: 0x0000CD8B
		public override void Abort()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000EB92 File Offset: 0x0000CD92
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000EB99 File Offset: 0x0000CD99
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000EBA7 File Offset: 0x0000CDA7
		public override IODataResponseMessage EndGetResponse(IAsyncResult asyncResult)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000EBAE File Offset: 0x0000CDAE
		public override IODataResponseMessage GetResponse()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000290 RID: 656
		private readonly IODataRequestMessage requestMessage;

		// Token: 0x04000291 RID: 657
		private readonly bool allowGetStream;

		// Token: 0x04000292 RID: 658
		private Stream cachedRequestStream;

		// Token: 0x04000293 RID: 659
		private HeaderCollection headers;
	}
}
