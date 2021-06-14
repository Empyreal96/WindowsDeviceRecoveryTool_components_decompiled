using System;
using System.IO;

namespace System.Windows.Forms
{
	// Token: 0x02000215 RID: 533
	internal class DataStreamFromComStream : Stream
	{
		// Token: 0x06002064 RID: 8292 RVA: 0x000A252A File Offset: 0x000A072A
		public DataStreamFromComStream(UnsafeNativeMethods.IStream comStream)
		{
			this.comStream = comStream;
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002065 RID: 8293 RVA: 0x000A2539 File Offset: 0x000A0739
		// (set) Token: 0x06002066 RID: 8294 RVA: 0x000A2544 File Offset: 0x000A0744
		public override long Position
		{
			get
			{
				return this.Seek(0L, SeekOrigin.Current);
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000A2550 File Offset: 0x000A0750
		public override long Length
		{
			get
			{
				long position = this.Position;
				long num = this.Seek(0L, SeekOrigin.End);
				this.Position = position;
				return num - position;
			}
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x000A2578 File Offset: 0x000A0778
		private unsafe int _Read(void* handle, int bytes)
		{
			return this.comStream.Read((IntPtr)handle, bytes);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x000A258C File Offset: 0x000A078C
		private unsafe int _Write(void* handle, int bytes)
		{
			return this.comStream.Write((IntPtr)handle, bytes);
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0000701A File Offset: 0x0000521A
		public override void Flush()
		{
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000A25A0 File Offset: 0x000A07A0
		public unsafe override int Read(byte[] buffer, int index, int count)
		{
			int result = 0;
			if (count > 0 && index >= 0 && count + index <= buffer.Length)
			{
				fixed (byte* ptr = buffer)
				{
					result = this._Read((void*)((byte*)ptr + index), count);
				}
			}
			return result;
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x000A25E6 File Offset: 0x000A07E6
		public override void SetLength(long value)
		{
			this.comStream.SetSize(value);
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x000A25F4 File Offset: 0x000A07F4
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.comStream.Seek(offset, (int)origin);
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000A2604 File Offset: 0x000A0804
		public unsafe override void Write(byte[] buffer, int index, int count)
		{
			int num = 0;
			if (count > 0 && index >= 0 && count + index <= buffer.Length)
			{
				try
				{
					try
					{
						fixed (byte* ptr = buffer)
						{
							num = this._Write((void*)((byte*)ptr + index), count);
						}
					}
					finally
					{
						byte* ptr = null;
					}
				}
				catch
				{
				}
			}
			if (num < count)
			{
				throw new IOException(SR.GetString("DataStreamWrite"));
			}
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000A2684 File Offset: 0x000A0884
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.comStream != null)
				{
					try
					{
						this.comStream.Commit(0);
					}
					catch (Exception)
					{
					}
				}
				this.comStream = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000A26DC File Offset: 0x000A08DC
		~DataStreamFromComStream()
		{
			this.Dispose(false);
		}

		// Token: 0x04000DFB RID: 3579
		private UnsafeNativeMethods.IStream comStream;
	}
}
