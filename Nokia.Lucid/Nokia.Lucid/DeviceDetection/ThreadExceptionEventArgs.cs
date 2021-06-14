using System;
using System.Threading;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceDetection
{
	// Token: 0x02000010 RID: 16
	public sealed class ThreadExceptionEventArgs : EventArgs
	{
		// Token: 0x0600003B RID: 59 RVA: 0x0000375B File Offset: 0x0000195B
		public ThreadExceptionEventArgs(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			this.exception = exception;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003778 File Offset: 0x00001978
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003780 File Offset: 0x00001980
		public bool IsHandled
		{
			get
			{
				return this.handled == 1;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000378C File Offset: 0x0000198C
		public void SetHandled()
		{
			int num = Interlocked.CompareExchange(ref this.handled, 1, 0);
			if (num != 0)
			{
				throw new InvalidOperationException(Resources.InvalidOperationException_MessageText_ExceptionAlreadyMarkedAsHandled);
			}
		}

		// Token: 0x0400002A RID: 42
		private const int NotHandled = 0;

		// Token: 0x0400002B RID: 43
		private const int Handled = 1;

		// Token: 0x0400002C RID: 44
		private readonly Exception exception;

		// Token: 0x0400002D RID: 45
		private int handled;
	}
}
