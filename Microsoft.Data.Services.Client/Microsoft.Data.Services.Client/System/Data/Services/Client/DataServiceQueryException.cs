using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace System.Data.Services.Client
{
	// Token: 0x020000FC RID: 252
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public sealed class DataServiceQueryException : InvalidOperationException
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x00022F91 File Offset: 0x00021191
		public DataServiceQueryException() : base(Strings.DataServiceException_GeneralError)
		{
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00022F9E File Offset: 0x0002119E
		public DataServiceQueryException(string message) : base(message)
		{
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00022FA7 File Offset: 0x000211A7
		public DataServiceQueryException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00022FB1 File Offset: 0x000211B1
		public DataServiceQueryException(string message, Exception innerException, QueryOperationResponse response) : base(message, innerException)
		{
			this.response = response;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00022FC2 File Offset: 0x000211C2
		protected DataServiceQueryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x00022FCC File Offset: 0x000211CC
		public QueryOperationResponse Response
		{
			get
			{
				return this.response;
			}
		}

		// Token: 0x040004E8 RID: 1256
		[NonSerialized]
		private readonly QueryOperationResponse response;
	}
}
