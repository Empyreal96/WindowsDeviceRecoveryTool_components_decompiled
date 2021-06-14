using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace System.Data.Services.Client
{
	// Token: 0x020000FE RID: 254
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public sealed class DataServiceRequestException : InvalidOperationException
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x00023087 File Offset: 0x00021287
		public DataServiceRequestException() : base(Strings.DataServiceException_GeneralError)
		{
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00023094 File Offset: 0x00021294
		public DataServiceRequestException(string message) : base(message)
		{
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0002309D File Offset: 0x0002129D
		public DataServiceRequestException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x000230A7 File Offset: 0x000212A7
		public DataServiceRequestException(string message, Exception innerException, DataServiceResponse response) : base(message, innerException)
		{
			this.response = response;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000230B8 File Offset: 0x000212B8
		protected DataServiceRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x000230C2 File Offset: 0x000212C2
		public DataServiceResponse Response
		{
			get
			{
				return this.response;
			}
		}

		// Token: 0x040004EA RID: 1258
		[NonSerialized]
		private readonly DataServiceResponse response;
	}
}
