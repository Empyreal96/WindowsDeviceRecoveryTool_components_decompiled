using System;
using System.Diagnostics;

namespace Microsoft.Data.OData
{
	// Token: 0x02000237 RID: 567
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public sealed class ODataInnerError
	{
		// Token: 0x06001228 RID: 4648 RVA: 0x0004438B File Offset: 0x0004258B
		public ODataInnerError()
		{
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00044394 File Offset: 0x00042594
		public ODataInnerError(Exception exception)
		{
			ExceptionUtils.CheckArgumentNotNull<Exception>(exception, "exception");
			this.Message = (exception.Message ?? string.Empty);
			this.TypeName = exception.GetType().FullName;
			this.StackTrace = exception.StackTrace;
			if (exception.InnerException != null)
			{
				this.InnerError = new ODataInnerError(exception.InnerException);
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x000443FD File Offset: 0x000425FD
		// (set) Token: 0x0600122B RID: 4651 RVA: 0x00044405 File Offset: 0x00042605
		public string Message { get; set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x0004440E File Offset: 0x0004260E
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x00044416 File Offset: 0x00042616
		public string TypeName { get; set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x0004441F File Offset: 0x0004261F
		// (set) Token: 0x0600122F RID: 4655 RVA: 0x00044427 File Offset: 0x00042627
		public string StackTrace { get; set; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x00044430 File Offset: 0x00042630
		// (set) Token: 0x06001231 RID: 4657 RVA: 0x00044438 File Offset: 0x00042638
		public ODataInnerError InnerError { get; set; }
	}
}
