using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x0200006A RID: 106
	internal abstract class StorageCommandBase<T>
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00035019 File Offset: 0x00033219
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x00035023 File Offset: 0x00033223
		internal StreamDescriptor StreamCopyState
		{
			get
			{
				return this.streamCopyState;
			}
			set
			{
				this.streamCopyState = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0003502E File Offset: 0x0003322E
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x00035038 File Offset: 0x00033238
		internal RequestResult CurrentResult
		{
			get
			{
				return this.currentResult;
			}
			set
			{
				this.currentResult = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00035043 File Offset: 0x00033243
		internal IList<RequestResult> RequestResults
		{
			get
			{
				return this.requestResults;
			}
		}

		// Token: 0x040001E7 RID: 487
		public int? ServerTimeoutInSeconds = null;

		// Token: 0x040001E8 RID: 488
		internal DateTime? OperationExpiryTime = null;

		// Token: 0x040001E9 RID: 489
		internal object OperationState;

		// Token: 0x040001EA RID: 490
		private volatile StreamDescriptor streamCopyState;

		// Token: 0x040001EB RID: 491
		private volatile RequestResult currentResult;

		// Token: 0x040001EC RID: 492
		private IList<RequestResult> requestResults = new List<RequestResult>();

		// Token: 0x040001ED RID: 493
		public Action<StorageCommandBase<T>, Exception, OperationContext> RecoveryAction;

		// Token: 0x040001EE RID: 494
		public Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation> ParseError;

		// Token: 0x040001EF RID: 495
		public Func<Stream, IDictionary<string, string>, string, StorageExtendedErrorInformation> ParseDataServiceError;
	}
}
