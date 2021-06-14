using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SoftwareRepository.Streaming
{
	// Token: 0x02000018 RID: 24
	public abstract class Streamer : IDisposable
	{
		// Token: 0x0600009E RID: 158 RVA: 0x000037EC File Offset: 0x000019EC
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GetStreamInternal", Justification = "Spelled correctly.")]
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Can throw.")]
		public Stream GetStream()
		{
			if (this.Stream != null)
			{
				return this.Stream;
			}
			Stream streamInternal = this.GetStreamInternal();
			if (streamInternal == null)
			{
				throw new InvalidOperationException("GetStreamInternal() returned null");
			}
			if (!streamInternal.CanWrite)
			{
				throw new InvalidOperationException("Stream returned by GetStreamInternal() must support writing");
			}
			if (!streamInternal.CanSeek)
			{
				throw new InvalidOperationException("Stream returned by GetStreamInternal() must support seeking");
			}
			return this.Stream = streamInternal;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000384D File Offset: 0x00001A4D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000385C File Offset: 0x00001A5C
		public virtual void SetMetadata(byte[] metadata)
		{
			this.Metadata = metadata;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003865 File Offset: 0x00001A65
		public virtual byte[] GetMetadata()
		{
			return this.Metadata;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000386D File Offset: 0x00001A6D
		public virtual void ClearMetadata()
		{
			this.Metadata = null;
		}

		// Token: 0x060000A3 RID: 163
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Implementors can employ arbitrarily complex logic.")]
		protected abstract Stream GetStreamInternal();

		// Token: 0x060000A4 RID: 164 RVA: 0x00003876 File Offset: 0x00001A76
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.Stream != null)
			{
				this.Stream.Dispose();
				this.Stream = null;
			}
		}

		// Token: 0x04000066 RID: 102
		private Stream Stream;

		// Token: 0x04000067 RID: 103
		private byte[] Metadata;
	}
}
