using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000284 RID: 644
	internal sealed class AsyncBufferedStream : Stream
	{
		// Token: 0x06001565 RID: 5477 RVA: 0x0004DFE8 File Offset: 0x0004C1E8
		internal AsyncBufferedStream(Stream stream)
		{
			this.innerStream = stream;
			this.bufferQueue = new Queue<AsyncBufferedStream.DataBuffer>();
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001566 RID: 5478 RVA: 0x0004E002 File Offset: 0x0004C202
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001567 RID: 5479 RVA: 0x0004E005 File Offset: 0x0004C205
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x0004E008 File Offset: 0x0004C208
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001569 RID: 5481 RVA: 0x0004E00B File Offset: 0x0004C20B
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x0004E012 File Offset: 0x0004C212
		// (set) Token: 0x0600156B RID: 5483 RVA: 0x0004E019 File Offset: 0x0004C219
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

		// Token: 0x0600156C RID: 5484 RVA: 0x0004E020 File Offset: 0x0004C220
		public override void Flush()
		{
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0004E022 File Offset: 0x0004C222
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0004E029 File Offset: 0x0004C229
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0004E030 File Offset: 0x0004C230
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0004E038 File Offset: 0x0004C238
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				if (this.bufferToAppendTo == null)
				{
					this.QueueNewBuffer();
				}
				while (count > 0)
				{
					int num = this.bufferToAppendTo.Write(buffer, offset, count);
					if (num < count)
					{
						this.QueueNewBuffer();
					}
					count -= num;
					offset += num;
				}
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0004E080 File Offset: 0x0004C280
		internal void Clear()
		{
			this.bufferQueue.Clear();
			this.bufferToAppendTo = null;
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0004E094 File Offset: 0x0004C294
		internal void FlushSync()
		{
			Queue<AsyncBufferedStream.DataBuffer> queue = this.PrepareFlushBuffers();
			if (queue == null)
			{
				return;
			}
			while (queue.Count > 0)
			{
				AsyncBufferedStream.DataBuffer dataBuffer = queue.Dequeue();
				dataBuffer.WriteToStream(this.innerStream);
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0004E0C8 File Offset: 0x0004C2C8
		internal new Task FlushAsync()
		{
			return this.FlushAsyncInternal();
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0004E0D0 File Offset: 0x0004C2D0
		internal Task FlushAsyncInternal()
		{
			Queue<AsyncBufferedStream.DataBuffer> queue = this.PrepareFlushBuffers();
			if (queue == null)
			{
				return TaskUtils.CompletedTask;
			}
			return Task.Factory.Iterate(this.FlushBuffersAsync(queue));
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0004E0FE File Offset: 0x0004C2FE
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.bufferQueue.Count > 0)
			{
				throw new ODataException(Strings.AsyncBufferedStream_WriterDisposedWithoutFlush);
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0004E123 File Offset: 0x0004C323
		private void QueueNewBuffer()
		{
			this.bufferToAppendTo = new AsyncBufferedStream.DataBuffer();
			this.bufferQueue.Enqueue(this.bufferToAppendTo);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0004E144 File Offset: 0x0004C344
		private Queue<AsyncBufferedStream.DataBuffer> PrepareFlushBuffers()
		{
			if (this.bufferQueue.Count == 0)
			{
				return null;
			}
			this.bufferToAppendTo = null;
			Queue<AsyncBufferedStream.DataBuffer> result = this.bufferQueue;
			this.bufferQueue = new Queue<AsyncBufferedStream.DataBuffer>();
			return result;
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0004E28C File Offset: 0x0004C48C
		private IEnumerable<Task> FlushBuffersAsync(Queue<AsyncBufferedStream.DataBuffer> buffers)
		{
			while (buffers.Count > 0)
			{
				AsyncBufferedStream.DataBuffer buffer = buffers.Dequeue();
				yield return buffer.WriteToStreamAsync(this.innerStream);
			}
			yield break;
		}

		// Token: 0x040007CB RID: 1995
		private readonly Stream innerStream;

		// Token: 0x040007CC RID: 1996
		private Queue<AsyncBufferedStream.DataBuffer> bufferQueue;

		// Token: 0x040007CD RID: 1997
		private AsyncBufferedStream.DataBuffer bufferToAppendTo;

		// Token: 0x02000285 RID: 645
		private sealed class DataBuffer
		{
			// Token: 0x06001579 RID: 5497 RVA: 0x0004E2B0 File Offset: 0x0004C4B0
			public DataBuffer()
			{
				this.buffer = new byte[80896];
				this.storedCount = 0;
			}

			// Token: 0x0600157A RID: 5498 RVA: 0x0004E2D0 File Offset: 0x0004C4D0
			public int Write(byte[] data, int index, int count)
			{
				int num = count;
				if (num > this.buffer.Length - this.storedCount)
				{
					num = this.buffer.Length - this.storedCount;
				}
				if (num > 0)
				{
					Array.Copy(data, index, this.buffer, this.storedCount, num);
					this.storedCount += num;
				}
				return num;
			}

			// Token: 0x0600157B RID: 5499 RVA: 0x0004E328 File Offset: 0x0004C528
			public void WriteToStream(Stream stream)
			{
				stream.Write(this.buffer, 0, this.storedCount);
			}

			// Token: 0x0600157C RID: 5500 RVA: 0x0004E36C File Offset: 0x0004C56C
			public Task WriteToStreamAsync(Stream stream)
			{
				return Task.Factory.FromAsync((AsyncCallback callback, object state) => stream.BeginWrite(this.buffer, 0, this.storedCount, callback, state), new Action<IAsyncResult>(stream.EndWrite), null);
			}

			// Token: 0x040007CE RID: 1998
			private const int BufferSize = 80896;

			// Token: 0x040007CF RID: 1999
			private readonly byte[] buffer;

			// Token: 0x040007D0 RID: 2000
			private int storedCount;
		}
	}
}
