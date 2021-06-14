using System;
using System.IO;
using System.Net;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x020000B4 RID: 180
	internal class GetReadStreamResult : BaseAsyncResult
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x00015BCC File Offset: 0x00013DCC
		internal GetReadStreamResult(DataServiceContext context, string method, ODataRequestMessageWrapper request, AsyncCallback callback, object state, StreamDescriptor streamDescriptor) : base(context, method, callback, state)
		{
			this.requestMessage = request;
			base.Abortable = request;
			this.streamDescriptor = streamDescriptor;
			this.requestInfo = new RequestInfo(context);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00015BFC File Offset: 0x00013DFC
		internal void Begin()
		{
			try
			{
				IAsyncResult asyncResult = BaseAsyncResult.InvokeAsync(new Func<AsyncCallback, object, IAsyncResult>(this.requestMessage.BeginGetResponse), new AsyncCallback(this.AsyncEndGetResponse), null);
				base.SetCompletedSynchronously(asyncResult.CompletedSynchronously);
			}
			catch (Exception e)
			{
				base.HandleFailure(e);
				throw;
			}
			finally
			{
				base.HandleCompleted();
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00015C6C File Offset: 0x00013E6C
		internal DataServiceStreamResponse End()
		{
			if (this.responseMessage != null)
			{
				this.streamDescriptor.ETag = this.responseMessage.GetHeader("ETag");
				this.streamDescriptor.ContentType = this.responseMessage.GetHeader("Content-Type");
				return new DataServiceStreamResponse(this.responseMessage);
			}
			return null;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00015CC8 File Offset: 0x00013EC8
		internal DataServiceStreamResponse Execute()
		{
			try
			{
				this.responseMessage = this.requestInfo.GetSyncronousResponse(this.requestMessage, true);
			}
			catch (Exception e)
			{
				base.HandleFailure(e);
				throw;
			}
			finally
			{
				base.SetCompleted();
				this.CompletedRequest();
			}
			if (base.Failure != null)
			{
				throw base.Failure;
			}
			return this.End();
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00015D3C File Offset: 0x00013F3C
		protected override void CompletedRequest()
		{
			if (this.responseMessage != null)
			{
				InvalidOperationException ex = null;
				if (!WebUtil.SuccessStatusCode((HttpStatusCode)this.responseMessage.StatusCode))
				{
					ex = BaseSaveResult.GetResponseText(new Func<Stream>(this.responseMessage.GetStream), (HttpStatusCode)this.responseMessage.StatusCode);
				}
				if (ex != null)
				{
					WebUtil.DisposeMessage(this.responseMessage);
					base.HandleFailure(ex);
				}
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00015D9E File Offset: 0x00013F9E
		protected override void HandleCompleted(BaseAsyncResult.PerRequest pereq)
		{
			Error.ThrowInternalError(InternalError.InvalidHandleCompleted);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00015DA8 File Offset: 0x00013FA8
		protected override void AsyncEndGetResponse(IAsyncResult asyncResult)
		{
			try
			{
				base.SetCompletedSynchronously(asyncResult.CompletedSynchronously);
				ODataRequestMessageWrapper request = Util.NullCheck<ODataRequestMessageWrapper>(this.requestMessage, InternalError.InvalidEndGetResponseRequest);
				this.responseMessage = this.requestInfo.EndGetResponse(request, asyncResult);
				base.SetCompleted();
			}
			catch (Exception e)
			{
				if (base.HandleFailure(e))
				{
					throw;
				}
			}
			finally
			{
				base.HandleCompleted();
			}
		}

		// Token: 0x0400031A RID: 794
		private readonly ODataRequestMessageWrapper requestMessage;

		// Token: 0x0400031B RID: 795
		private readonly StreamDescriptor streamDescriptor;

		// Token: 0x0400031C RID: 796
		private readonly RequestInfo requestInfo;

		// Token: 0x0400031D RID: 797
		private IODataResponseMessage responseMessage;
	}
}
