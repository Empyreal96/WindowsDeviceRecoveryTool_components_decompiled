using System;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x020001D2 RID: 466
	internal sealed class ODataBatchOperationWriteStream : ODataBatchOperationStream
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x00032B5A File Offset: 0x00030D5A
		internal ODataBatchOperationWriteStream(Stream batchStream, IODataBatchOperationListener listener) : base(listener)
		{
			this.batchStream = batchStream;
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00032B6A File Offset: 0x00030D6A
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x00032B6D File Offset: 0x00030D6D
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00032B70 File Offset: 0x00030D70
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00032B73 File Offset: 0x00030D73
		public override long Length
		{
			get
			{
				base.ValidateNotDisposed();
				return this.batchStream.Length;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00032B86 File Offset: 0x00030D86
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x00032B99 File Offset: 0x00030D99
		public override long Position
		{
			get
			{
				base.ValidateNotDisposed();
				return this.batchStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00032BA0 File Offset: 0x00030DA0
		public override void SetLength(long value)
		{
			base.ValidateNotDisposed();
			this.batchStream.SetLength(value);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00032BB4 File Offset: 0x00030DB4
		public override void Write(byte[] buffer, int offset, int count)
		{
			base.ValidateNotDisposed();
			this.batchStream.Write(buffer, offset, count);
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00032BCA File Offset: 0x00030DCA
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			base.ValidateNotDisposed();
			return this.batchStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00032BE4 File Offset: 0x00030DE4
		public override void EndWrite(IAsyncResult asyncResult)
		{
			base.ValidateNotDisposed();
			this.batchStream.EndWrite(asyncResult);
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00032BF8 File Offset: 0x00030DF8
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00032BFF File Offset: 0x00030DFF
		public override void Flush()
		{
			base.ValidateNotDisposed();
			this.batchStream.Flush();
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00032C12 File Offset: 0x00030E12
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.batchStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x040004FB RID: 1275
		private Stream batchStream;
	}
}
