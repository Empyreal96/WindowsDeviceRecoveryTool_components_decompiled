using System;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x02000092 RID: 146
	internal class RESTCommand<T> : StorageCommandBase<T>
	{
		// Token: 0x06000FD0 RID: 4048 RVA: 0x0003C069 File Offset: 0x0003A269
		public RESTCommand(StorageCredentials credentials, StorageUri storageUri) : this(credentials, storageUri, null)
		{
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0003C074 File Offset: 0x0003A274
		public RESTCommand(StorageCredentials credentials, StorageUri storageUri, UriQueryBuilder builder)
		{
			this.Credentials = credentials;
			this.StorageUri = storageUri;
			this.Builder = builder;
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x0003C09D File Offset: 0x0003A29D
		// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x0003C0A5 File Offset: 0x0003A2A5
		public Stream ResponseStream
		{
			get
			{
				return this.responseStream;
			}
			set
			{
				this.responseStream = ((value == null) ? null : value.WrapWithByteCountingStream(base.CurrentResult));
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0003C0BF File Offset: 0x0003A2BF
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0003C0C7 File Offset: 0x0003A2C7
		public Stream StreamToDispose { get; set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0003C0D0 File Offset: 0x0003A2D0
		// (set) Token: 0x06000FD7 RID: 4055 RVA: 0x0003C0D8 File Offset: 0x0003A2D8
		public Stream SendStream
		{
			get
			{
				return this.sendStream;
			}
			set
			{
				this.sendStream = value;
			}
		}

		// Token: 0x0400039C RID: 924
		public LocationMode LocationMode;

		// Token: 0x0400039D RID: 925
		public CommandLocationMode CommandLocationMode;

		// Token: 0x0400039E RID: 926
		public StorageCredentials Credentials;

		// Token: 0x0400039F RID: 927
		public StorageUri StorageUri;

		// Token: 0x040003A0 RID: 928
		public UriQueryBuilder Builder;

		// Token: 0x040003A1 RID: 929
		private Stream responseStream;

		// Token: 0x040003A2 RID: 930
		public Stream DestinationStream;

		// Token: 0x040003A3 RID: 931
		public Stream ErrorStream;

		// Token: 0x040003A4 RID: 932
		public bool RetrieveResponseStream;

		// Token: 0x040003A5 RID: 933
		public bool CalculateMd5ForResponseStream;

		// Token: 0x040003A6 RID: 934
		private Stream sendStream;

		// Token: 0x040003A7 RID: 935
		public long? SendStreamLength = null;

		// Token: 0x040003A8 RID: 936
		public Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest> BuildRequestDelegate;

		// Token: 0x040003A9 RID: 937
		public Action<HttpWebRequest, OperationContext> SetHeaders;

		// Token: 0x040003AA RID: 938
		public Action<HttpWebRequest, OperationContext> SignRequest;

		// Token: 0x040003AB RID: 939
		public Func<RESTCommand<T>, HttpWebResponse, Exception, OperationContext, T> PreProcessResponse;

		// Token: 0x040003AC RID: 940
		public Func<RESTCommand<T>, HttpWebResponse, OperationContext, T> PostProcessResponse;

		// Token: 0x040003AD RID: 941
		public Action<RESTCommand<T>> DisposeAction;
	}
}
