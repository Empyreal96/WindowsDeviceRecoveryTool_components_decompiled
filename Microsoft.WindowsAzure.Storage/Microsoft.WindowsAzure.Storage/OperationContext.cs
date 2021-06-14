using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x0200007A RID: 122
	public sealed class OperationContext
	{
		// Token: 0x06000E9A RID: 3738 RVA: 0x000386B4 File Offset: 0x000368B4
		public OperationContext()
		{
			this.ClientRequestID = Guid.NewGuid().ToString();
			this.LogLevel = OperationContext.DefaultLogLevel;
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x000386F6 File Offset: 0x000368F6
		// (set) Token: 0x06000E9C RID: 3740 RVA: 0x000386FE File Offset: 0x000368FE
		public IDictionary<string, string> UserHeaders { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x00038707 File Offset: 0x00036907
		// (set) Token: 0x06000E9E RID: 3742 RVA: 0x0003870F File Offset: 0x0003690F
		public string ClientRequestID { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x00038718 File Offset: 0x00036918
		// (set) Token: 0x06000EA0 RID: 3744 RVA: 0x0003871F File Offset: 0x0003691F
		public static LogLevel DefaultLogLevel { get; set; } = LogLevel.Verbose;

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x00038727 File Offset: 0x00036927
		// (set) Token: 0x06000EA2 RID: 3746 RVA: 0x0003872F File Offset: 0x0003692F
		public LogLevel LogLevel { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000EA3 RID: 3747 RVA: 0x00038738 File Offset: 0x00036938
		// (remove) Token: 0x06000EA4 RID: 3748 RVA: 0x0003876C File Offset: 0x0003696C
		public static event EventHandler<RequestEventArgs> GlobalSendingRequest;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000EA5 RID: 3749 RVA: 0x000387A0 File Offset: 0x000369A0
		// (remove) Token: 0x06000EA6 RID: 3750 RVA: 0x000387D4 File Offset: 0x000369D4
		public static event EventHandler<RequestEventArgs> GlobalResponseReceived;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000EA7 RID: 3751 RVA: 0x00038808 File Offset: 0x00036A08
		// (remove) Token: 0x06000EA8 RID: 3752 RVA: 0x0003883C File Offset: 0x00036A3C
		public static event EventHandler<RequestEventArgs> GlobalRequestCompleted;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000EA9 RID: 3753 RVA: 0x00038870 File Offset: 0x00036A70
		// (remove) Token: 0x06000EAA RID: 3754 RVA: 0x000388A4 File Offset: 0x00036AA4
		public static event EventHandler<RequestEventArgs> GlobalRetrying;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000EAB RID: 3755 RVA: 0x000388D8 File Offset: 0x00036AD8
		// (remove) Token: 0x06000EAC RID: 3756 RVA: 0x00038910 File Offset: 0x00036B10
		public event EventHandler<RequestEventArgs> SendingRequest;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000EAD RID: 3757 RVA: 0x00038948 File Offset: 0x00036B48
		// (remove) Token: 0x06000EAE RID: 3758 RVA: 0x00038980 File Offset: 0x00036B80
		public event EventHandler<RequestEventArgs> ResponseReceived;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000EAF RID: 3759 RVA: 0x000389B8 File Offset: 0x00036BB8
		// (remove) Token: 0x06000EB0 RID: 3760 RVA: 0x000389F0 File Offset: 0x00036BF0
		public event EventHandler<RequestEventArgs> RequestCompleted;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000EB1 RID: 3761 RVA: 0x00038A28 File Offset: 0x00036C28
		// (remove) Token: 0x06000EB2 RID: 3762 RVA: 0x00038A60 File Offset: 0x00036C60
		public event EventHandler<RequestEventArgs> Retrying;

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00038A98 File Offset: 0x00036C98
		internal void FireSendingRequest(RequestEventArgs args)
		{
			EventHandler<RequestEventArgs> sendingRequest = this.SendingRequest;
			if (sendingRequest != null)
			{
				sendingRequest(this, args);
			}
			EventHandler<RequestEventArgs> globalSendingRequest = OperationContext.GlobalSendingRequest;
			if (globalSendingRequest != null)
			{
				globalSendingRequest(this, args);
			}
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00038AC8 File Offset: 0x00036CC8
		internal void FireResponseReceived(RequestEventArgs args)
		{
			EventHandler<RequestEventArgs> responseReceived = this.ResponseReceived;
			if (responseReceived != null)
			{
				responseReceived(this, args);
			}
			EventHandler<RequestEventArgs> globalResponseReceived = OperationContext.GlobalResponseReceived;
			if (globalResponseReceived != null)
			{
				globalResponseReceived(this, args);
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00038AF8 File Offset: 0x00036CF8
		internal void FireRequestCompleted(RequestEventArgs args)
		{
			EventHandler<RequestEventArgs> requestCompleted = this.RequestCompleted;
			if (requestCompleted != null)
			{
				requestCompleted(this, args);
			}
			EventHandler<RequestEventArgs> globalRequestCompleted = OperationContext.GlobalRequestCompleted;
			if (globalRequestCompleted != null)
			{
				globalRequestCompleted(this, args);
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00038B28 File Offset: 0x00036D28
		internal void FireRetrying(RequestEventArgs args)
		{
			EventHandler<RequestEventArgs> retrying = this.Retrying;
			if (retrying != null)
			{
				retrying(this, args);
			}
			EventHandler<RequestEventArgs> globalRetrying = OperationContext.GlobalRetrying;
			if (globalRetrying != null)
			{
				globalRetrying(this, args);
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x00038B58 File Offset: 0x00036D58
		// (set) Token: 0x06000EB8 RID: 3768 RVA: 0x00038B60 File Offset: 0x00036D60
		public DateTime StartTime { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x00038B69 File Offset: 0x00036D69
		// (set) Token: 0x06000EBA RID: 3770 RVA: 0x00038B71 File Offset: 0x00036D71
		public DateTime EndTime { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x00038B7A File Offset: 0x00036D7A
		public IList<RequestResult> RequestResults
		{
			get
			{
				return this.requestResults;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x00038B82 File Offset: 0x00036D82
		public RequestResult LastResult
		{
			get
			{
				if (this.requestResults == null || this.requestResults.Count == 0)
				{
					return null;
				}
				return this.requestResults[this.requestResults.Count - 1];
			}
		}

		// Token: 0x04000254 RID: 596
		private IList<RequestResult> requestResults = new List<RequestResult>();
	}
}
