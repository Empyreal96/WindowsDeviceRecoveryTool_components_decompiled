using System;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x020001D1 RID: 465
	internal abstract class ODataBatchOperationStream : Stream
	{
		// Token: 0x06000E70 RID: 3696 RVA: 0x00032B08 File Offset: 0x00030D08
		internal ODataBatchOperationStream(IODataBatchOperationListener listener)
		{
			this.listener = listener;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00032B17 File Offset: 0x00030D17
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00032B1E File Offset: 0x00030D1E
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.listener != null)
			{
				this.listener.BatchOperationContentStreamDisposed();
				this.listener = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00032B44 File Offset: 0x00030D44
		protected void ValidateNotDisposed()
		{
			if (this.listener == null)
			{
				throw new ObjectDisposedException(null, Strings.ODataBatchOperationStream_Disposed);
			}
		}

		// Token: 0x040004FA RID: 1274
		private IODataBatchOperationListener listener;
	}
}
