using System;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x020001B4 RID: 436
	internal static class MessageStreamWrapper
	{
		// Token: 0x06000D81 RID: 3457 RVA: 0x0002EC94 File Offset: 0x0002CE94
		internal static Stream CreateNonDisposingStream(Stream innerStream)
		{
			return new MessageStreamWrapper.MessageStreamWrappingStream(innerStream, true, -1L);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0002EC9F File Offset: 0x0002CE9F
		internal static Stream CreateStreamWithMaxSize(Stream innerStream, long maxBytesToBeRead)
		{
			return new MessageStreamWrapper.MessageStreamWrappingStream(innerStream, false, maxBytesToBeRead);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0002ECA9 File Offset: 0x0002CEA9
		internal static Stream CreateNonDisposingStreamWithMaxSize(Stream innerStream, long maxBytesToBeRead)
		{
			return new MessageStreamWrapper.MessageStreamWrappingStream(innerStream, true, maxBytesToBeRead);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0002ECB4 File Offset: 0x0002CEB4
		internal static bool IsNonDisposingStream(Stream stream)
		{
			MessageStreamWrapper.MessageStreamWrappingStream messageStreamWrappingStream = stream as MessageStreamWrapper.MessageStreamWrappingStream;
			return messageStreamWrappingStream != null && messageStreamWrappingStream.IgnoreDispose;
		}

		// Token: 0x020001B5 RID: 437
		private sealed class MessageStreamWrappingStream : Stream
		{
			// Token: 0x06000D85 RID: 3461 RVA: 0x0002ECD3 File Offset: 0x0002CED3
			internal MessageStreamWrappingStream(Stream innerStream, bool ignoreDispose, long maxBytesToBeRead)
			{
				this.innerStream = innerStream;
				this.ignoreDispose = ignoreDispose;
				this.maxBytesToBeRead = maxBytesToBeRead;
			}

			// Token: 0x170002E9 RID: 745
			// (get) Token: 0x06000D86 RID: 3462 RVA: 0x0002ECF0 File Offset: 0x0002CEF0
			public override bool CanRead
			{
				get
				{
					return this.innerStream.CanRead;
				}
			}

			// Token: 0x170002EA RID: 746
			// (get) Token: 0x06000D87 RID: 3463 RVA: 0x0002ECFD File Offset: 0x0002CEFD
			public override bool CanSeek
			{
				get
				{
					return this.innerStream.CanSeek;
				}
			}

			// Token: 0x170002EB RID: 747
			// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0002ED0A File Offset: 0x0002CF0A
			public override bool CanWrite
			{
				get
				{
					return this.innerStream.CanWrite;
				}
			}

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0002ED17 File Offset: 0x0002CF17
			public override long Length
			{
				get
				{
					return this.innerStream.Length;
				}
			}

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0002ED24 File Offset: 0x0002CF24
			// (set) Token: 0x06000D8B RID: 3467 RVA: 0x0002ED31 File Offset: 0x0002CF31
			public override long Position
			{
				get
				{
					return this.innerStream.Position;
				}
				set
				{
					this.innerStream.Position = value;
				}
			}

			// Token: 0x170002EE RID: 750
			// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0002ED3F File Offset: 0x0002CF3F
			internal bool IgnoreDispose
			{
				get
				{
					return this.ignoreDispose;
				}
			}

			// Token: 0x06000D8D RID: 3469 RVA: 0x0002ED47 File Offset: 0x0002CF47
			public override void Flush()
			{
				this.innerStream.Flush();
			}

			// Token: 0x06000D8E RID: 3470 RVA: 0x0002ED54 File Offset: 0x0002CF54
			public override int Read(byte[] buffer, int offset, int count)
			{
				int num = this.innerStream.Read(buffer, offset, count);
				this.IncreaseTotalBytesRead(num);
				return num;
			}

			// Token: 0x06000D8F RID: 3471 RVA: 0x0002ED78 File Offset: 0x0002CF78
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				return this.innerStream.BeginRead(buffer, offset, count, callback, state);
			}

			// Token: 0x06000D90 RID: 3472 RVA: 0x0002ED8C File Offset: 0x0002CF8C
			public override int EndRead(IAsyncResult asyncResult)
			{
				int num = this.innerStream.EndRead(asyncResult);
				this.IncreaseTotalBytesRead(num);
				return num;
			}

			// Token: 0x06000D91 RID: 3473 RVA: 0x0002EDAE File Offset: 0x0002CFAE
			public override long Seek(long offset, SeekOrigin origin)
			{
				return this.innerStream.Seek(offset, origin);
			}

			// Token: 0x06000D92 RID: 3474 RVA: 0x0002EDBD File Offset: 0x0002CFBD
			public override void SetLength(long value)
			{
				this.innerStream.SetLength(value);
			}

			// Token: 0x06000D93 RID: 3475 RVA: 0x0002EDCB File Offset: 0x0002CFCB
			public override void Write(byte[] buffer, int offset, int count)
			{
				this.innerStream.Write(buffer, offset, count);
			}

			// Token: 0x06000D94 RID: 3476 RVA: 0x0002EDDB File Offset: 0x0002CFDB
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				return this.innerStream.BeginWrite(buffer, offset, count, callback, state);
			}

			// Token: 0x06000D95 RID: 3477 RVA: 0x0002EDEF File Offset: 0x0002CFEF
			public override void EndWrite(IAsyncResult asyncResult)
			{
				this.innerStream.EndWrite(asyncResult);
			}

			// Token: 0x06000D96 RID: 3478 RVA: 0x0002EDFD File Offset: 0x0002CFFD
			protected override void Dispose(bool disposing)
			{
				if (disposing && !this.ignoreDispose && this.innerStream != null)
				{
					this.innerStream.Dispose();
					this.innerStream = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x06000D97 RID: 3479 RVA: 0x0002EE2C File Offset: 0x0002D02C
			private void IncreaseTotalBytesRead(int bytesRead)
			{
				if (this.maxBytesToBeRead <= 0L)
				{
					return;
				}
				this.totalBytesRead += (long)((bytesRead < 0) ? 0 : bytesRead);
				if (this.totalBytesRead > this.maxBytesToBeRead)
				{
					throw new ODataException(Strings.MessageStreamWrappingStream_ByteLimitExceeded(this.totalBytesRead, this.maxBytesToBeRead));
				}
			}

			// Token: 0x0400048B RID: 1163
			private readonly long maxBytesToBeRead;

			// Token: 0x0400048C RID: 1164
			private readonly bool ignoreDispose;

			// Token: 0x0400048D RID: 1165
			private Stream innerStream;

			// Token: 0x0400048E RID: 1166
			private long totalBytesRead;
		}
	}
}
