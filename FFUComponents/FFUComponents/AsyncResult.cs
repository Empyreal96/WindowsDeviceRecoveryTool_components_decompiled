using System;

namespace FFUComponents
{
	// Token: 0x02000003 RID: 3
	internal class AsyncResult<TResult> : AsyncResultNoResult
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002242 File Offset: 0x00000442
		public AsyncResult(AsyncCallback asyncCallback, object state) : base(asyncCallback, state)
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002258 File Offset: 0x00000458
		public void SetAsCompleted(TResult result, bool completedSynchronously)
		{
			this.result = result;
			base.SetAsCompleted(null, completedSynchronously);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002269 File Offset: 0x00000469
		public new TResult EndInvoke()
		{
			base.EndInvoke();
			return this.result;
		}

		// Token: 0x04000009 RID: 9
		private TResult result = default(TResult);
	}
}
