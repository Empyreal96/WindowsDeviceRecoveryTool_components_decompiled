using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Nokia.Mira.Chunks.Serialization;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000007 RID: 7
	internal class ChunkInformationXmlWriter : IChunkInformationWriter
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002346 File Offset: 0x00000546
		public ChunkInformationXmlWriter(IStreamContainer streamContainer)
		{
			if (streamContainer == null)
			{
				throw new ArgumentNullException("streamContainer");
			}
			this.streamContainer = streamContainer;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002363 File Offset: 0x00000563
		public void Write(IEnumerable<ChunkRaw> chunkInformations)
		{
			this.WriteInternal(chunkInformations);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023A4 File Offset: 0x000005A4
		private void WriteInternal(IEnumerable<ChunkRaw> chunkInformations)
		{
			WebFileSerializable webFileSerializable = new WebFileSerializable();
			webFileSerializable.Chunks = (from ch in chunkInformations
			select new ChunkSerializable
			{
				Begin = new long?(ch.Begin),
				Current = new long?(ch.Current)
			}).ToList<ChunkSerializable>();
			Stream stream;
			using (this.streamContainer.ReserveStream(out stream))
			{
				stream.SetLength(0L);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(WebFileSerializable));
				xmlSerializer.Serialize(stream, webFileSerializable);
			}
		}

		// Token: 0x04000005 RID: 5
		private readonly IStreamContainer streamContainer;
	}
}
