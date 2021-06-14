using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001D3 RID: 467
	internal abstract class ODataBatchOperationReadStream : ODataBatchOperationStream
	{
		// Token: 0x06000E82 RID: 3714 RVA: 0x00032C25 File Offset: 0x00030E25
		internal ODataBatchOperationReadStream(ODataBatchReaderStream batchReaderStream, IODataBatchOperationListener listener) : base(listener)
		{
			this.batchReaderStream = batchReaderStream;
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00032C35 File Offset: 0x00030E35
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00032C38 File Offset: 0x00030E38
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x00032C3B File Offset: 0x00030E3B
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00032C3E File Offset: 0x00030E3E
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x00032C45 File Offset: 0x00030E45
		// (set) Token: 0x06000E88 RID: 3720 RVA: 0x00032C4C File Offset: 0x00030E4C
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00032C53 File Offset: 0x00030E53
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00032C5A File Offset: 0x00030E5A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00032C61 File Offset: 0x00030E61
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00032C68 File Offset: 0x00030E68
		internal static ODataBatchOperationReadStream Create(ODataBatchReaderStream batchReaderStream, IODataBatchOperationListener listener, int length)
		{
			return new ODataBatchOperationReadStream.ODataBatchOperationReadStreamWithLength(batchReaderStream, listener, length);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00032C72 File Offset: 0x00030E72
		internal static ODataBatchOperationReadStream Create(ODataBatchReaderStream batchReaderStream, IODataBatchOperationListener listener)
		{
			return new ODataBatchOperationReadStream.ODataBatchOperationReadStreamWithDelimiter(batchReaderStream, listener);
		}

		// Token: 0x040004FC RID: 1276
		protected ODataBatchReaderStream batchReaderStream;

		// Token: 0x020001D4 RID: 468
		private sealed class ODataBatchOperationReadStreamWithLength : ODataBatchOperationReadStream
		{
			// Token: 0x06000E8E RID: 3726 RVA: 0x00032C7B File Offset: 0x00030E7B
			internal ODataBatchOperationReadStreamWithLength(ODataBatchReaderStream batchReaderStream, IODataBatchOperationListener listener, int length) : base(batchReaderStream, listener)
			{
				ExceptionUtils.CheckIntegerNotNegative(length, "length");
				this.length = length;
			}

			// Token: 0x06000E8F RID: 3727 RVA: 0x00032C98 File Offset: 0x00030E98
			public override int Read(byte[] buffer, int offset, int count)
			{
				ExceptionUtils.CheckArgumentNotNull<byte[]>(buffer, "buffer");
				ExceptionUtils.CheckIntegerNotNegative(offset, "offset");
				ExceptionUtils.CheckIntegerNotNegative(count, "count");
				base.ValidateNotDisposed();
				if (this.length == 0)
				{
					return 0;
				}
				int num = this.batchReaderStream.ReadWithLength(buffer, offset, Math.Min(count, this.length));
				this.length -= num;
				return num;
			}

			// Token: 0x040004FD RID: 1277
			private int length;
		}

		// Token: 0x020001D5 RID: 469
		private sealed class ODataBatchOperationReadStreamWithDelimiter : ODataBatchOperationReadStream
		{
			// Token: 0x06000E90 RID: 3728 RVA: 0x00032CFF File Offset: 0x00030EFF
			internal ODataBatchOperationReadStreamWithDelimiter(ODataBatchReaderStream batchReaderStream, IODataBatchOperationListener listener) : base(batchReaderStream, listener)
			{
			}

			// Token: 0x06000E91 RID: 3729 RVA: 0x00032D0C File Offset: 0x00030F0C
			public override int Read(byte[] buffer, int offset, int count)
			{
				ExceptionUtils.CheckArgumentNotNull<byte[]>(buffer, "buffer");
				ExceptionUtils.CheckIntegerNotNegative(offset, "offset");
				ExceptionUtils.CheckIntegerNotNegative(count, "count");
				base.ValidateNotDisposed();
				if (this.exhausted)
				{
					return 0;
				}
				int num = this.batchReaderStream.ReadWithDelimiter(buffer, offset, count);
				if (num < count)
				{
					this.exhausted = true;
				}
				return num;
			}

			// Token: 0x040004FE RID: 1278
			private bool exhausted;
		}
	}
}
