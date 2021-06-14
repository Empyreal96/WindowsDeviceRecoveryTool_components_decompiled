using System;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x020001C8 RID: 456
	internal sealed class NonDisposingStream : Stream
	{
		// Token: 0x06000E24 RID: 3620 RVA: 0x00031B54 File Offset: 0x0002FD54
		internal NonDisposingStream(Stream innerStream)
		{
			this.innerStream = innerStream;
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00031B63 File Offset: 0x0002FD63
		public override bool CanRead
		{
			get
			{
				return this.innerStream.CanRead;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00031B70 File Offset: 0x0002FD70
		public override bool CanSeek
		{
			get
			{
				return this.innerStream.CanSeek;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x00031B7D File Offset: 0x0002FD7D
		public override bool CanWrite
		{
			get
			{
				return this.innerStream.CanWrite;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x00031B8A File Offset: 0x0002FD8A
		public override long Length
		{
			get
			{
				return this.innerStream.Length;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x00031B97 File Offset: 0x0002FD97
		// (set) Token: 0x06000E2A RID: 3626 RVA: 0x00031BA4 File Offset: 0x0002FDA4
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

		// Token: 0x06000E2B RID: 3627 RVA: 0x00031BB2 File Offset: 0x0002FDB2
		public override void Flush()
		{
			this.innerStream.Flush();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00031BBF File Offset: 0x0002FDBF
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.innerStream.Read(buffer, offset, count);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00031BCF File Offset: 0x0002FDCF
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.innerStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00031BE3 File Offset: 0x0002FDE3
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.innerStream.EndRead(asyncResult);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00031BF1 File Offset: 0x0002FDF1
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.innerStream.Seek(offset, origin);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00031C00 File Offset: 0x0002FE00
		public override void SetLength(long value)
		{
			this.innerStream.SetLength(value);
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00031C0E File Offset: 0x0002FE0E
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.innerStream.Write(buffer, offset, count);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00031C1E File Offset: 0x0002FE1E
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.innerStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00031C32 File Offset: 0x0002FE32
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.innerStream.EndWrite(asyncResult);
		}

		// Token: 0x040004AD RID: 1197
		private readonly Stream innerStream;
	}
}
