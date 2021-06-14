using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000131 RID: 305
	public static class ODataMessageExtensions
	{
		// Token: 0x06000803 RID: 2051 RVA: 0x0001A810 File Offset: 0x00018A10
		public static ODataVersion GetDataServiceVersion(this IODataResponseMessage message, ODataVersion defaultVersion)
		{
			ODataMessage message2 = new ODataResponseMessage(message, false, false, long.MaxValue);
			return ODataUtilsInternal.GetDataServiceVersion(message2, defaultVersion);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001A838 File Offset: 0x00018A38
		public static ODataVersion GetDataServiceVersion(this IODataRequestMessage message, ODataVersion defaultVersion)
		{
			ODataMessage message2 = new ODataRequestMessage(message, false, false, long.MaxValue);
			return ODataUtilsInternal.GetDataServiceVersion(message2, defaultVersion);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001A85E File Offset: 0x00018A5E
		public static ODataPreferenceHeader PreferHeader(this IODataRequestMessage requestMessage)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataRequestMessage>(requestMessage, "requestMessage");
			return new ODataPreferenceHeader(requestMessage);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001A871 File Offset: 0x00018A71
		public static ODataPreferenceHeader PreferenceAppliedHeader(this IODataResponseMessage responseMessage)
		{
			ExceptionUtils.CheckArgumentNotNull<IODataResponseMessage>(responseMessage, "responseMessage");
			return new ODataPreferenceHeader(responseMessage);
		}
	}
}
