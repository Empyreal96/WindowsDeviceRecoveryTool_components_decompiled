using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace System.Data.Services.Client
{
	// Token: 0x02000107 RID: 263
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public sealed class DataServiceClientException : InvalidOperationException
	{
		// Token: 0x0600088D RID: 2189 RVA: 0x00023E98 File Offset: 0x00022098
		public DataServiceClientException() : this(Strings.DataServiceException_GeneralError)
		{
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00023EA5 File Offset: 0x000220A5
		public DataServiceClientException(string message) : this(message, null)
		{
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00023EAF File Offset: 0x000220AF
		public DataServiceClientException(string message, Exception innerException) : this(message, innerException, 500)
		{
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00023EBE File Offset: 0x000220BE
		public DataServiceClientException(string message, int statusCode) : this(message, null, statusCode)
		{
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00023EDC File Offset: 0x000220DC
		public DataServiceClientException(string message, Exception innerException, int statusCode) : base(message, innerException)
		{
			this.state.StatusCode = statusCode;
			base.SerializeObjectState += delegate(object sender, SafeSerializationEventArgs e)
			{
				e.AddSerializedState(this.state);
			};
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00023F16 File Offset: 0x00022116
		public int StatusCode
		{
			get
			{
				return this.state.StatusCode;
			}
		}

		// Token: 0x04000509 RID: 1289
		[NonSerialized]
		private DataServiceClientException.DataServiceClientExceptionSerializationState state;

		// Token: 0x02000108 RID: 264
		[Serializable]
		private struct DataServiceClientExceptionSerializationState : ISafeSerializationData
		{
			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x06000894 RID: 2196 RVA: 0x00023F23 File Offset: 0x00022123
			// (set) Token: 0x06000895 RID: 2197 RVA: 0x00023F2B File Offset: 0x0002212B
			public int StatusCode { get; set; }

			// Token: 0x06000896 RID: 2198 RVA: 0x00023F34 File Offset: 0x00022134
			void ISafeSerializationData.CompleteDeserialization(object deserialized)
			{
				DataServiceClientException ex = (DataServiceClientException)deserialized;
				ex.state = this;
			}
		}
	}
}
