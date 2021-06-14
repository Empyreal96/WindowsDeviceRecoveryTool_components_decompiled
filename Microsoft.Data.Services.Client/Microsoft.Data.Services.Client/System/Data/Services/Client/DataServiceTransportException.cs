using System;
using System.Runtime.Serialization;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200005F RID: 95
	[Serializable]
	public class DataServiceTransportException : InvalidOperationException
	{
		// Token: 0x06000321 RID: 801 RVA: 0x0000E074 File Offset: 0x0000C274
		public DataServiceTransportException(IODataResponseMessage response, Exception innerException) : base(innerException.Message, innerException)
		{
			Util.CheckArgumentNull<Exception>(innerException, "innerException");
			this.state.ResponseMessage = response;
			base.SerializeObjectState += delegate(object sender, SafeSerializationEventArgs e)
			{
				e.AddSerializedState(this.state);
			};
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000E0BF File Offset: 0x0000C2BF
		public IODataResponseMessage Response
		{
			get
			{
				return this.state.ResponseMessage;
			}
		}

		// Token: 0x04000287 RID: 647
		[NonSerialized]
		private DataServiceTransportException.DataServiceWebExceptionSerializationState state;

		// Token: 0x02000060 RID: 96
		[Serializable]
		private struct DataServiceWebExceptionSerializationState : ISafeSerializationData
		{
			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x06000324 RID: 804 RVA: 0x0000E0CC File Offset: 0x0000C2CC
			// (set) Token: 0x06000325 RID: 805 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
			public IODataResponseMessage ResponseMessage { get; set; }

			// Token: 0x06000326 RID: 806 RVA: 0x0000E0E0 File Offset: 0x0000C2E0
			void ISafeSerializationData.CompleteDeserialization(object deserialized)
			{
				DataServiceTransportException ex = (DataServiceTransportException)deserialized;
				ex.state = this;
			}
		}
	}
}
