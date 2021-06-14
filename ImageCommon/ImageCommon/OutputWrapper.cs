using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200001A RID: 26
	public class OutputWrapper : IPayloadWrapper
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00006E30 File Offset: 0x00005030
		public OutputWrapper(string path)
		{
			this.path = path;
			this.writes = new Queue<IAsyncResult>();
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006E4A File Offset: 0x0000504A
		public void InitializeWrapper(long payloadSize)
		{
			this.fileStream = new FileStream(this.path, FileMode.Create, FileAccess.Write, FileShare.None, 1048576, true);
			this.fileStream.SetLength(payloadSize);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006E72 File Offset: 0x00005072
		public void ResetPosition()
		{
			this.fileStream.Seek(0L, SeekOrigin.Begin);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006E84 File Offset: 0x00005084
		public void Write(byte[] data)
		{
			while (this.writes.Count > 0 && this.writes.Peek().IsCompleted)
			{
				this.fileStream.EndWrite(this.writes.Dequeue());
			}
			IAsyncResult item = this.fileStream.BeginWrite(data, 0, data.Length, null, null);
			this.writes.Enqueue(item);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006EE8 File Offset: 0x000050E8
		public void FinalizeWrapper()
		{
			while (this.writes.Count > 0)
			{
				this.fileStream.EndWrite(this.writes.Dequeue());
			}
			if (this.fileStream != null)
			{
				this.fileStream.Close();
				this.fileStream = null;
			}
		}

		// Token: 0x0400009C RID: 156
		private string path;

		// Token: 0x0400009D RID: 157
		private FileStream fileStream;

		// Token: 0x0400009E RID: 158
		private Queue<IAsyncResult> writes;
	}
}
