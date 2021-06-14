using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x020000FE RID: 254
	internal sealed class BufferingReadStream : Stream
	{
		// Token: 0x060006C4 RID: 1732 RVA: 0x000180EB File Offset: 0x000162EB
		internal BufferingReadStream(Stream stream)
		{
			this.innerStream = stream;
			this.buffers = new LinkedList<byte[]>();
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00018105 File Offset: 0x00016305
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00018108 File Offset: 0x00016308
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0001810B File Offset: 0x0001630B
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0001810E File Offset: 0x0001630E
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00018115 File Offset: 0x00016315
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x0001811C File Offset: 0x0001631C
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

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00018123 File Offset: 0x00016323
		internal bool IsBuffering
		{
			get
			{
				return !this.bufferingModeDisabled;
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001812E File Offset: 0x0001632E
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00018138 File Offset: 0x00016338
		public override int Read(byte[] userBuffer, int offset, int count)
		{
			ExceptionUtils.CheckArgumentNotNull<byte[]>(userBuffer, "userBuffer");
			ExceptionUtils.CheckIntegerNotNegative(offset, "offset");
			ExceptionUtils.CheckIntegerPositive(count, "count");
			int num = 0;
			while (this.currentReadNode != null && count > 0)
			{
				byte[] value = this.currentReadNode.Value;
				int num2 = value.Length - this.positionInCurrentBuffer;
				if (num2 == count)
				{
					Buffer.BlockCopy(value, this.positionInCurrentBuffer, userBuffer, offset, count);
					num += count;
					this.MoveToNextBuffer();
					return num;
				}
				if (num2 > count)
				{
					Buffer.BlockCopy(value, this.positionInCurrentBuffer, userBuffer, offset, count);
					num += count;
					this.positionInCurrentBuffer += count;
					return num;
				}
				Buffer.BlockCopy(value, this.positionInCurrentBuffer, userBuffer, offset, num2);
				num += num2;
				offset += num2;
				count -= num2;
				this.MoveToNextBuffer();
			}
			int num3 = this.innerStream.Read(userBuffer, offset, count);
			if (!this.bufferingModeDisabled && num3 > 0)
			{
				byte[] array = new byte[num3];
				Buffer.BlockCopy(userBuffer, offset, array, 0, num3);
				this.buffers.AddLast(array);
			}
			return num + num3;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001823B File Offset: 0x0001643B
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00018242 File Offset: 0x00016442
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00018249 File Offset: 0x00016449
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00018250 File Offset: 0x00016450
		internal void ResetStream()
		{
			this.currentReadNode = this.buffers.First;
			this.positionInCurrentBuffer = 0;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001826A File Offset: 0x0001646A
		internal void StopBuffering()
		{
			this.ResetStream();
			this.bufferingModeDisabled = true;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00018279 File Offset: 0x00016479
		protected override void Dispose(bool disposing)
		{
			if (this.bufferingModeDisabled)
			{
				if (disposing && this.innerStream != null)
				{
					this.innerStream.Dispose();
					this.innerStream = null;
				}
				base.Dispose(disposing);
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000182A7 File Offset: 0x000164A7
		private void MoveToNextBuffer()
		{
			if (this.bufferingModeDisabled)
			{
				this.buffers.RemoveFirst();
				this.currentReadNode = this.buffers.First;
			}
			else
			{
				this.currentReadNode = this.currentReadNode.Next;
			}
			this.positionInCurrentBuffer = 0;
		}

		// Token: 0x0400029D RID: 669
		private readonly LinkedList<byte[]> buffers;

		// Token: 0x0400029E RID: 670
		private Stream innerStream;

		// Token: 0x0400029F RID: 671
		private int positionInCurrentBuffer;

		// Token: 0x040002A0 RID: 672
		private bool bufferingModeDisabled;

		// Token: 0x040002A1 RID: 673
		private LinkedListNode<byte[]> currentReadNode;
	}
}
