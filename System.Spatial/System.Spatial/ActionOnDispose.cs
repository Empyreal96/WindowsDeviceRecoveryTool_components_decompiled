using System;

namespace System.Spatial
{
	// Token: 0x02000039 RID: 57
	internal class ActionOnDispose : IDisposable
	{
		// Token: 0x0600017D RID: 381 RVA: 0x000047A2 File Offset: 0x000029A2
		public ActionOnDispose(Action action)
		{
			Util.CheckArgumentNull(action, "action");
			this.action = action;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000047BC File Offset: 0x000029BC
		public void Dispose()
		{
			if (this.action != null)
			{
				this.action();
				this.action = null;
			}
		}

		// Token: 0x04000033 RID: 51
		private Action action;
	}
}
