using System;
using System.Threading;

namespace Nokia.Mira.Primitives
{
	// Token: 0x02000015 RID: 21
	internal class Disposable : IDisposable
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002A70 File Offset: 0x00000C70
		public Disposable(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			this.action = action;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002A8D File Offset: 0x00000C8D
		private Disposable()
		{
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002A95 File Offset: 0x00000C95
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002AA4 File Offset: 0x00000CA4
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			Action action = Interlocked.Exchange<Action>(ref this.action, null);
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002ACB File Offset: 0x00000CCB
		protected void VerifyNotDisposed()
		{
			if (this.action == null)
			{
				throw new ObjectDisposedException(this.ToString());
			}
		}

		// Token: 0x04000025 RID: 37
		public static readonly Disposable None = new Disposable.DisposableNone();

		// Token: 0x04000026 RID: 38
		private Action action;

		// Token: 0x02000016 RID: 22
		private sealed class DisposableNone : Disposable
		{
			// Token: 0x06000046 RID: 70 RVA: 0x00002AED File Offset: 0x00000CED
			protected override void Dispose(bool disposing)
			{
			}
		}
	}
}
