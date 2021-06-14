using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x0200008D RID: 141
	internal class TruncatingNonCloseableStream : Stream
	{
		// Token: 0x06000F82 RID: 3970 RVA: 0x0003ACB2 File Offset: 0x00038EB2
		public TruncatingNonCloseableStream(Stream wrappedStream, long? length = null)
		{
			CommonUtility.AssertNotNull("WrappedStream", wrappedStream);
			if (!wrappedStream.CanSeek || !wrappedStream.CanRead)
			{
				throw new NotSupportedException();
			}
			this.wrappedStream = wrappedStream;
			this.length = length;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0003ACE9 File Offset: 0x00038EE9
		public override bool CanRead
		{
			get
			{
				return this.wrappedStream.CanRead;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0003ACF6 File Offset: 0x00038EF6
		public override bool CanSeek
		{
			get
			{
				return this.wrappedStream.CanSeek;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0003AD03 File Offset: 0x00038F03
		public override bool CanTimeout
		{
			get
			{
				return this.wrappedStream.CanTimeout;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0003AD10 File Offset: 0x00038F10
		public override bool CanWrite
		{
			get
			{
				return this.wrappedStream.CanWrite;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0003AD1D File Offset: 0x00038F1D
		public override long Length
		{
			get
			{
				if (this.length == null)
				{
					return this.wrappedStream.Length;
				}
				return this.length.Value;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0003AD43 File Offset: 0x00038F43
		// (set) Token: 0x06000F89 RID: 3977 RVA: 0x0003AD50 File Offset: 0x00038F50
		public override long Position
		{
			get
			{
				return this.wrappedStream.Position;
			}
			set
			{
				this.wrappedStream.Position = value;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0003AD5E File Offset: 0x00038F5E
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x0003AD6B File Offset: 0x00038F6B
		public override int ReadTimeout
		{
			get
			{
				return this.wrappedStream.ReadTimeout;
			}
			set
			{
				this.wrappedStream.ReadTimeout = value;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0003AD79 File Offset: 0x00038F79
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x0003AD86 File Offset: 0x00038F86
		public override int WriteTimeout
		{
			get
			{
				return this.wrappedStream.WriteTimeout;
			}
			set
			{
				this.wrappedStream.WriteTimeout = value;
			}
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0003AD94 File Offset: 0x00038F94
		public override void Flush()
		{
			this.wrappedStream.Flush();
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0003ADA1 File Offset: 0x00038FA1
		public override void SetLength(long value)
		{
			if (value < 0L || value > this.Length)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			this.length = new long?(value);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0003ADC8 File Offset: 0x00038FC8
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.wrappedStream.Seek(offset, origin);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0003ADD8 File Offset: 0x00038FD8
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			long num = (this.Length > this.Position) ? (this.Length - this.Position) : 0L;
			if ((long)count > num)
			{
				count = (int)num;
			}
			if (count == 0)
			{
				return 0;
			}
			return this.wrappedStream.Read(buffer, offset, count);
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0003AE30 File Offset: 0x00039030
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			long num = (this.Length > this.Position) ? (this.Length - this.Position) : 0L;
			if ((long)count > num)
			{
				count = (int)num;
			}
			StorageAsyncResult<int> storageAsyncResult = new StorageAsyncResult<int>(callback, state);
			if (count == 0)
			{
				storageAsyncResult.Result = 0;
				storageAsyncResult.OnComplete();
				return storageAsyncResult;
			}
			return this.wrappedStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0003AEA3 File Offset: 0x000390A3
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.wrappedStream.EndRead(asyncResult);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0003AEB1 File Offset: 0x000390B1
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.wrappedStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0003AEC5 File Offset: 0x000390C5
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.wrappedStream.EndWrite(asyncResult);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0003AED3 File Offset: 0x000390D3
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.wrappedStream.Write(buffer, offset, count);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0003AEE3 File Offset: 0x000390E3
		protected override void Dispose(bool disposing)
		{
		}

		// Token: 0x0400037C RID: 892
		private readonly Stream wrappedStream;

		// Token: 0x0400037D RID: 893
		private long? length;
	}
}
