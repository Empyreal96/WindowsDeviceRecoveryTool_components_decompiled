using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000250 RID: 592
	internal sealed class BufferedReadStream : Stream
	{
		// Token: 0x0600136F RID: 4975 RVA: 0x00048F34 File Offset: 0x00047134
		private BufferedReadStream(Stream inputStream)
		{
			this.buffers = new List<BufferedReadStream.DataBuffer>();
			this.inputStream = inputStream;
			this.currentBufferIndex = -1;
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x00048F55 File Offset: 0x00047155
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x00048F58 File Offset: 0x00047158
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x00048F5B File Offset: 0x0004715B
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x00048F5E File Offset: 0x0004715E
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x00048F65 File Offset: 0x00047165
		// (set) Token: 0x06001375 RID: 4981 RVA: 0x00048F6C File Offset: 0x0004716C
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

		// Token: 0x06001376 RID: 4982 RVA: 0x00048F73 File Offset: 0x00047173
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00048F7C File Offset: 0x0004717C
		public override int Read(byte[] buffer, int offset, int count)
		{
			ExceptionUtils.CheckArgumentNotNull<byte[]>(buffer, "buffer");
			if (this.currentBufferIndex == -1)
			{
				return 0;
			}
			BufferedReadStream.DataBuffer dataBuffer = this.buffers[this.currentBufferIndex];
			while (this.currentBufferReadCount >= dataBuffer.StoredCount)
			{
				this.currentBufferIndex++;
				if (this.currentBufferIndex >= this.buffers.Count)
				{
					this.currentBufferIndex = -1;
					return 0;
				}
				dataBuffer = this.buffers[this.currentBufferIndex];
				this.currentBufferReadCount = 0;
			}
			int num = count;
			if (count > dataBuffer.StoredCount - this.currentBufferReadCount)
			{
				num = dataBuffer.StoredCount - this.currentBufferReadCount;
			}
			Array.Copy(dataBuffer.Buffer, this.currentBufferReadCount, buffer, offset, num);
			this.currentBufferReadCount += num;
			return num;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00049047 File Offset: 0x00047247
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0004904E File Offset: 0x0004724E
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00049055 File Offset: 0x00047255
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00049084 File Offset: 0x00047284
		internal static Task<BufferedReadStream> BufferStreamAsync(Stream inputStream)
		{
			BufferedReadStream bufferedReadStream = new BufferedReadStream(inputStream);
			return Task.Factory.Iterate(bufferedReadStream.BufferInputStream()).FollowAlwaysWith(delegate(Task task)
			{
				inputStream.Dispose();
			}).FollowOnSuccessWith(delegate(Task task)
			{
				bufferedReadStream.ResetForReading();
				return bufferedReadStream;
			});
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x000490E6 File Offset: 0x000472E6
		internal void ResetForReading()
		{
			this.currentBufferIndex = ((this.buffers.Count == 0) ? -1 : 0);
			this.currentBufferReadCount = 0;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00049106 File Offset: 0x00047306
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0004936C File Offset: 0x0004756C
		private IEnumerable<Task> BufferInputStream()
		{
			while (this.inputStream != null)
			{
				BufferedReadStream.DataBuffer currentBuffer = (this.currentBufferIndex == -1) ? null : this.buffers[this.currentBufferIndex];
				if (currentBuffer != null && currentBuffer.FreeBytes < 1024)
				{
					currentBuffer = null;
				}
				if (currentBuffer == null)
				{
					currentBuffer = this.AddNewBuffer();
				}
				yield return Task.Factory.FromAsync((AsyncCallback asyncCallback, object asyncState) => this.inputStream.BeginRead(currentBuffer.Buffer, currentBuffer.OffsetToWriteTo, currentBuffer.FreeBytes, asyncCallback, asyncState), delegate(IAsyncResult asyncResult)
				{
					try
					{
						int num = this.inputStream.EndRead(asyncResult);
						if (num == 0)
						{
							this.inputStream = null;
						}
						else
						{
							currentBuffer.MarkBytesAsWritten(num);
						}
					}
					catch (Exception e)
					{
						if (!ExceptionUtils.IsCatchableExceptionType(e))
						{
							throw;
						}
						this.inputStream = null;
						throw;
					}
				}, null);
			}
			yield break;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0004938C File Offset: 0x0004758C
		private BufferedReadStream.DataBuffer AddNewBuffer()
		{
			BufferedReadStream.DataBuffer dataBuffer = new BufferedReadStream.DataBuffer();
			this.buffers.Add(dataBuffer);
			this.currentBufferIndex = this.buffers.Count - 1;
			return dataBuffer;
		}

		// Token: 0x040006F5 RID: 1781
		private readonly List<BufferedReadStream.DataBuffer> buffers;

		// Token: 0x040006F6 RID: 1782
		private Stream inputStream;

		// Token: 0x040006F7 RID: 1783
		private int currentBufferIndex;

		// Token: 0x040006F8 RID: 1784
		private int currentBufferReadCount;

		// Token: 0x02000251 RID: 593
		private sealed class DataBuffer
		{
			// Token: 0x06001380 RID: 4992 RVA: 0x000493BF File Offset: 0x000475BF
			public DataBuffer()
			{
				this.buffer = new byte[65536];
				this.StoredCount = 0;
			}

			// Token: 0x170003F9 RID: 1017
			// (get) Token: 0x06001381 RID: 4993 RVA: 0x000493DE File Offset: 0x000475DE
			public byte[] Buffer
			{
				get
				{
					return this.buffer;
				}
			}

			// Token: 0x170003FA RID: 1018
			// (get) Token: 0x06001382 RID: 4994 RVA: 0x000493E6 File Offset: 0x000475E6
			public int OffsetToWriteTo
			{
				get
				{
					return this.StoredCount;
				}
			}

			// Token: 0x170003FB RID: 1019
			// (get) Token: 0x06001383 RID: 4995 RVA: 0x000493EE File Offset: 0x000475EE
			// (set) Token: 0x06001384 RID: 4996 RVA: 0x000493F6 File Offset: 0x000475F6
			public int StoredCount { get; private set; }

			// Token: 0x170003FC RID: 1020
			// (get) Token: 0x06001385 RID: 4997 RVA: 0x000493FF File Offset: 0x000475FF
			public int FreeBytes
			{
				get
				{
					return this.buffer.Length - this.StoredCount;
				}
			}

			// Token: 0x06001386 RID: 4998 RVA: 0x00049410 File Offset: 0x00047610
			public void MarkBytesAsWritten(int count)
			{
				this.StoredCount += count;
			}

			// Token: 0x040006F9 RID: 1785
			internal const int MinReadBufferSize = 1024;

			// Token: 0x040006FA RID: 1786
			private const int BufferSize = 65536;

			// Token: 0x040006FB RID: 1787
			private readonly byte[] buffer;
		}
	}
}
