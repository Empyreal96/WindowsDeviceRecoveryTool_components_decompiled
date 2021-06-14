using System;
using System.IO;
using System.Threading;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Metadata
{
	// Token: 0x02000025 RID: 37
	internal class MetadataStreamContainer : IStreamContainer, IDisposable
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00003380 File Offset: 0x00001580
		public MetadataStreamContainer(string metadataFile)
		{
			this.metadataFile = metadataFile;
			this.internalStream = new FileStream(this.metadataFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
			this.removeMetadataOnDispose = false;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000033AA File Offset: 0x000015AA
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000033B2 File Offset: 0x000015B2
		public bool RemoveMetadataOnDispose
		{
			get
			{
				return this.removeMetadataOnDispose;
			}
			set
			{
				this.removeMetadataOnDispose = value;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000033F0 File Offset: 0x000015F0
		public IStreamReservationContext ReserveStream(out Stream stream)
		{
			Stream localStream = Interlocked.CompareExchange<Stream>(ref this.internalStream, null, null);
			if (localStream == null)
			{
				throw new InvalidOperationException();
			}
			stream = localStream;
			return new MetadataStreamContainer.StreamReservationContext(delegate()
			{
				localStream.Flush();
				localStream.Seek(0L, SeekOrigin.Begin);
				this.internalStream = localStream;
			});
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003444 File Offset: 0x00001644
		public void Dispose()
		{
			this.internalStream.Dispose();
			if (this.removeMetadataOnDispose)
			{
				File.Delete(this.metadataFile);
			}
		}

		// Token: 0x04000049 RID: 73
		private readonly string metadataFile;

		// Token: 0x0400004A RID: 74
		private Stream internalStream;

		// Token: 0x0400004B RID: 75
		private bool removeMetadataOnDispose;

		// Token: 0x02000026 RID: 38
		private class StreamReservationContext : Disposable, IStreamReservationContext, IDisposable
		{
			// Token: 0x060000A5 RID: 165 RVA: 0x00003464 File Offset: 0x00001664
			public StreamReservationContext(Action action) : base(action)
			{
			}
		}
	}
}
