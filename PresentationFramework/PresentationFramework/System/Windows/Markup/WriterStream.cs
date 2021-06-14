using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000269 RID: 617
	internal class WriterStream : Stream
	{
		// Token: 0x0600235A RID: 9050 RVA: 0x000AD399 File Offset: 0x000AB599
		internal WriterStream(ReadWriteStreamManager streamManager)
		{
			this._streamManager = streamManager;
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000AD3A8 File Offset: 0x000AB5A8
		public override void Close()
		{
			this.StreamManager.WriterClose();
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x00002137 File Offset: 0x00000337
		public override void Flush()
		{
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x000AD3B5 File Offset: 0x000AB5B5
		public override long Length
		{
			get
			{
				return this.StreamManager.WriteLength;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x000AD3C2 File Offset: 0x000AB5C2
		// (set) Token: 0x06002362 RID: 9058 RVA: 0x00041D30 File Offset: 0x0003FF30
		public override long Position
		{
			get
			{
				return -1L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x00041D30 File Offset: 0x0003FF30
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x00041D30 File Offset: 0x0003FF30
		public override int ReadByte()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x000AD3C6 File Offset: 0x000AB5C6
		public override long Seek(long offset, SeekOrigin loc)
		{
			return this.StreamManager.WriterSeek(offset, loc);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x00041D30 File Offset: 0x0003FF30
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x000AD3D5 File Offset: 0x000AB5D5
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.StreamManager.Write(buffer, offset, count);
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x000AD3E5 File Offset: 0x000AB5E5
		internal void UpdateReaderLength(long position)
		{
			this.StreamManager.UpdateReaderLength(position);
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x000AD3F3 File Offset: 0x000AB5F3
		private ReadWriteStreamManager StreamManager
		{
			get
			{
				return this._streamManager;
			}
		}

		// Token: 0x04001A86 RID: 6790
		private ReadWriteStreamManager _streamManager;
	}
}
