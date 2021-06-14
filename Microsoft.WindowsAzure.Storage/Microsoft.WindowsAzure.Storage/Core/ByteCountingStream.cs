using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x02000055 RID: 85
	internal class ByteCountingStream : Stream
	{
		// Token: 0x06000D23 RID: 3363 RVA: 0x00030C07 File Offset: 0x0002EE07
		public ByteCountingStream(Stream wrappedStream, RequestResult requestObject)
		{
			CommonUtility.AssertNotNull("WrappedStream", wrappedStream);
			CommonUtility.AssertNotNull("RequestObject", requestObject);
			this.wrappedStream = wrappedStream;
			this.requestObject = requestObject;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00030C33 File Offset: 0x0002EE33
		public override bool CanRead
		{
			get
			{
				return this.wrappedStream.CanRead;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00030C40 File Offset: 0x0002EE40
		public override bool CanSeek
		{
			get
			{
				return this.wrappedStream.CanSeek;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00030C4D File Offset: 0x0002EE4D
		public override bool CanTimeout
		{
			get
			{
				return this.wrappedStream.CanTimeout;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00030C5A File Offset: 0x0002EE5A
		public override bool CanWrite
		{
			get
			{
				return this.wrappedStream.CanWrite;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00030C67 File Offset: 0x0002EE67
		public override long Length
		{
			get
			{
				return this.wrappedStream.Length;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00030C74 File Offset: 0x0002EE74
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00030C81 File Offset: 0x0002EE81
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

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00030C8F File Offset: 0x0002EE8F
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00030C9C File Offset: 0x0002EE9C
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

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00030CAA File Offset: 0x0002EEAA
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x00030CB7 File Offset: 0x0002EEB7
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

		// Token: 0x06000D2F RID: 3375 RVA: 0x00030CC5 File Offset: 0x0002EEC5
		public override void Flush()
		{
			this.wrappedStream.Flush();
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00030CD2 File Offset: 0x0002EED2
		public override void SetLength(long value)
		{
			this.wrappedStream.SetLength(value);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00030CE0 File Offset: 0x0002EEE0
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.wrappedStream.Seek(offset, origin);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00030CF0 File Offset: 0x0002EEF0
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this.wrappedStream.Read(buffer, offset, count);
			this.requestObject.IngressBytes += (long)num;
			return num;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00030D24 File Offset: 0x0002EF24
		public override int ReadByte()
		{
			int num = this.wrappedStream.ReadByte();
			if (num != -1)
			{
				this.requestObject.IngressBytes += 1L;
			}
			return num;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00030D56 File Offset: 0x0002EF56
		public override void Close()
		{
			this.wrappedStream.Close();
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00030D63 File Offset: 0x0002EF63
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.wrappedStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00030D78 File Offset: 0x0002EF78
		public override int EndRead(IAsyncResult asyncResult)
		{
			int num = this.wrappedStream.EndRead(asyncResult);
			this.requestObject.IngressBytes += (long)num;
			return num;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00030DA8 File Offset: 0x0002EFA8
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			IAsyncResult result = this.wrappedStream.BeginWrite(buffer, offset, count, callback, state);
			this.requestObject.EgressBytes += (long)count;
			return result;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00030DDD File Offset: 0x0002EFDD
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.wrappedStream.EndWrite(asyncResult);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00030DEB File Offset: 0x0002EFEB
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.wrappedStream.Write(buffer, offset, count);
			this.requestObject.EgressBytes += (long)count;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00030E0F File Offset: 0x0002F00F
		public override void WriteByte(byte value)
		{
			this.wrappedStream.WriteByte(value);
			this.requestObject.EgressBytes += 1L;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00030E31 File Offset: 0x0002F031
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.wrappedStream.Dispose();
			}
		}

		// Token: 0x040001A3 RID: 419
		private readonly Stream wrappedStream;

		// Token: 0x040001A4 RID: 420
		private readonly RequestResult requestObject;
	}
}
