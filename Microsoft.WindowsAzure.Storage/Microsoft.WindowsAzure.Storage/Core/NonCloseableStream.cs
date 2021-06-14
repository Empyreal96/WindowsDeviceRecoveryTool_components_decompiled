using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x02000089 RID: 137
	internal class NonCloseableStream : Stream
	{
		// Token: 0x06000F61 RID: 3937 RVA: 0x0003AA0E File Offset: 0x00038C0E
		public NonCloseableStream(Stream wrappedStream)
		{
			CommonUtility.AssertNotNull("WrappedStream", wrappedStream);
			this.wrappedStream = wrappedStream;
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0003AA28 File Offset: 0x00038C28
		public override bool CanRead
		{
			get
			{
				return this.wrappedStream.CanRead;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0003AA35 File Offset: 0x00038C35
		public override bool CanSeek
		{
			get
			{
				return this.wrappedStream.CanSeek;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x0003AA42 File Offset: 0x00038C42
		public override bool CanTimeout
		{
			get
			{
				return this.wrappedStream.CanTimeout;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0003AA4F File Offset: 0x00038C4F
		public override bool CanWrite
		{
			get
			{
				return this.wrappedStream.CanWrite;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0003AA5C File Offset: 0x00038C5C
		public override long Length
		{
			get
			{
				return this.wrappedStream.Length;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0003AA69 File Offset: 0x00038C69
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x0003AA76 File Offset: 0x00038C76
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

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0003AA84 File Offset: 0x00038C84
		// (set) Token: 0x06000F6A RID: 3946 RVA: 0x0003AA91 File Offset: 0x00038C91
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

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0003AA9F File Offset: 0x00038C9F
		// (set) Token: 0x06000F6C RID: 3948 RVA: 0x0003AAAC File Offset: 0x00038CAC
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

		// Token: 0x06000F6D RID: 3949 RVA: 0x0003AABA File Offset: 0x00038CBA
		public override void Flush()
		{
			this.wrappedStream.Flush();
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0003AAC7 File Offset: 0x00038CC7
		public override void SetLength(long value)
		{
			this.wrappedStream.SetLength(value);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0003AAD5 File Offset: 0x00038CD5
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.wrappedStream.Seek(offset, origin);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0003AAE4 File Offset: 0x00038CE4
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.wrappedStream.Read(buffer, offset, count);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0003AAF4 File Offset: 0x00038CF4
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.wrappedStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0003AB08 File Offset: 0x00038D08
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.wrappedStream.EndRead(asyncResult);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0003AB16 File Offset: 0x00038D16
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.wrappedStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0003AB2A File Offset: 0x00038D2A
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.wrappedStream.EndWrite(asyncResult);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0003AB38 File Offset: 0x00038D38
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.wrappedStream.Write(buffer, offset, count);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0003AB48 File Offset: 0x00038D48
		protected override void Dispose(bool disposing)
		{
		}

		// Token: 0x04000291 RID: 657
		private readonly Stream wrappedStream;
	}
}
