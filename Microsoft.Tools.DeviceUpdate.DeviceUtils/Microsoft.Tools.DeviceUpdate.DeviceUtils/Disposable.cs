using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000004 RID: 4
	public class Disposable : IDisposable
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002083 File Offset: 0x00000283
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002092 File Offset: 0x00000292
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing)
			{
				this.DisposeManaged();
			}
			this.DisposeUnmanaged();
			this.disposed = true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020B3 File Offset: 0x000002B3
		protected virtual void DisposeManaged()
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020B5 File Offset: 0x000002B5
		protected virtual void DisposeUnmanaged()
		{
		}

		// Token: 0x04000002 RID: 2
		private bool disposed;
	}
}
