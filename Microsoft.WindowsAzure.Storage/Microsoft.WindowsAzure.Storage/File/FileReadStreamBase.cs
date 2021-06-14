using System;
using System.Globalization;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x02000029 RID: 41
	internal abstract class FileReadStreamBase : Stream
	{
		// Token: 0x06000901 RID: 2305 RVA: 0x000209CC File Offset: 0x0001EBCC
		protected FileReadStreamBase(CloudFile file, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			if (options.UseTransactionalMD5.Value)
			{
				CommonUtility.AssertInBounds<int>("StreamMinimumReadSizeInBytes", file.StreamMinimumReadSizeInBytes, 1, 4194304);
			}
			this.file = file;
			this.fileProperties = new FileProperties(file.Properties);
			this.currentOffset = 0L;
			this.streamMinimumReadSizeInBytes = this.file.StreamMinimumReadSizeInBytes;
			this.internalBuffer = new MultiBufferMemoryStream(file.ServiceClient.BufferManager, 65536);
			this.accessCondition = accessCondition;
			this.options = options;
			this.operationContext = operationContext;
			this.fileMD5 = ((this.options.DisableContentMD5Validation.Value || string.IsNullOrEmpty(this.fileProperties.ContentMD5)) ? null : new MD5Wrapper());
			this.lastException = null;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00020AA3 File Offset: 0x0001ECA3
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00020AA6 File Offset: 0x0001ECA6
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00020AA9 File Offset: 0x0001ECA9
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00020AAC File Offset: 0x0001ECAC
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x00020AB4 File Offset: 0x0001ECB4
		public override long Position
		{
			get
			{
				return this.currentOffset;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00020ABF File Offset: 0x0001ECBF
		public override long Length
		{
			get
			{
				return this.fileProperties.Length;
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00020ACC File Offset: 0x0001ECCC
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this.lastException != null)
			{
				throw this.lastException;
			}
			long num;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = this.currentOffset + offset;
				break;
			case SeekOrigin.End:
				num = this.Length + offset;
				break;
			default:
				CommonUtility.ArgumentOutOfRange("origin", origin);
				throw new ArgumentOutOfRangeException("origin");
			}
			CommonUtility.AssertInBounds<long>("offset", num, 0L, this.Length);
			if (num != this.currentOffset)
			{
				long num2 = this.internalBuffer.Position + (num - this.currentOffset);
				if (num2 >= 0L && num2 < this.internalBuffer.Length)
				{
					this.internalBuffer.Position = num2;
				}
				else
				{
					this.internalBuffer.SetLength(0L);
				}
				this.fileMD5 = null;
				this.currentOffset = num;
			}
			return this.currentOffset;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00020BA6 File Offset: 0x0001EDA6
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00020BAD File Offset: 0x0001EDAD
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00020BB4 File Offset: 0x0001EDB4
		public override void Flush()
		{
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00020BB8 File Offset: 0x0001EDB8
		protected int ConsumeBuffer(byte[] buffer, int offset, int count)
		{
			int num = this.internalBuffer.Read(buffer, offset, count);
			this.currentOffset += (long)num;
			this.VerifyFileMD5(buffer, offset, num);
			return num;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00020BED File Offset: 0x0001EDED
		protected int GetReadSize()
		{
			if (this.currentOffset < this.Length)
			{
				return (int)Math.Min((long)this.streamMinimumReadSizeInBytes, this.Length - this.currentOffset);
			}
			return 0;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00020C1C File Offset: 0x0001EE1C
		protected void VerifyFileMD5(byte[] buffer, int offset, int count)
		{
			if (this.fileMD5 != null && this.lastException == null && count > 0)
			{
				this.fileMD5.UpdateHash(buffer, offset, count);
				if (this.currentOffset == this.Length && !string.IsNullOrEmpty(this.fileProperties.ContentMD5))
				{
					string text = this.fileMD5.ComputeHash();
					this.fileMD5.Dispose();
					this.fileMD5 = null;
					if (!text.Equals(this.fileProperties.ContentMD5))
					{
						this.lastException = new IOException(string.Format(CultureInfo.InvariantCulture, "File data corrupted (integrity check failed), Expected value is '{0}', retrieved '{1}'", new object[]
						{
							this.fileProperties.ContentMD5,
							text
						}));
					}
				}
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00020CD9 File Offset: 0x0001EED9
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.internalBuffer != null)
				{
					this.internalBuffer.Dispose();
					this.internalBuffer = null;
				}
				if (this.fileMD5 != null)
				{
					this.fileMD5.Dispose();
					this.fileMD5 = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x040000F7 RID: 247
		protected CloudFile file;

		// Token: 0x040000F8 RID: 248
		protected FileProperties fileProperties;

		// Token: 0x040000F9 RID: 249
		protected long currentOffset;

		// Token: 0x040000FA RID: 250
		protected MultiBufferMemoryStream internalBuffer;

		// Token: 0x040000FB RID: 251
		protected int streamMinimumReadSizeInBytes;

		// Token: 0x040000FC RID: 252
		protected AccessCondition accessCondition;

		// Token: 0x040000FD RID: 253
		protected FileRequestOptions options;

		// Token: 0x040000FE RID: 254
		protected OperationContext operationContext;

		// Token: 0x040000FF RID: 255
		protected MD5Wrapper fileMD5;

		// Token: 0x04000100 RID: 256
		protected Exception lastException;
	}
}
