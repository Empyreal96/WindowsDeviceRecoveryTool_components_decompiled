using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Nokia.Mira.Chunks.Serialization;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000005 RID: 5
	internal class ChunkInformationXmlReader : IChunkInformationReader
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002189 File Offset: 0x00000389
		public ChunkInformationXmlReader(IStreamContainer streamContainer)
		{
			if (streamContainer == null)
			{
				throw new ArgumentNullException("streamContainer");
			}
			this.streamContainer = streamContainer;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021D8 File Offset: 0x000003D8
		public ReadOnlyCollection<ChunkRaw> Read()
		{
			ReadOnlyCollection<ChunkRaw> result;
			try
			{
				result = Array.AsReadOnly<ChunkRaw>(this.ReadWebFileInformation().Chunks.Select(delegate(ChunkSerializable ch)
				{
					long value = ch.Begin.Value;
					long? num = ch.Current;
					return new ChunkRaw(value, num.Value);
				}).ToArray<ChunkRaw>());
			}
			catch (Exception)
			{
				result = Array.AsReadOnly<ChunkRaw>(Enumerable.Empty<ChunkRaw>().ToArray<ChunkRaw>());
			}
			return result;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002244 File Offset: 0x00000444
		private WebFileSerializable ReadWebFileInformation()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(WebFileSerializable));
			Stream stream;
			WebFileSerializable result;
			using (this.streamContainer.ReserveStream(out stream))
			{
				WebFileSerializable webFileSerializable = (WebFileSerializable)xmlSerializer.Deserialize(stream);
				this.VerifyWebFileInformation(webFileSerializable);
				result = webFileSerializable;
			}
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002306 File Offset: 0x00000506
		private void VerifyWebFileInformation(WebFileSerializable information)
		{
			if (information == null)
			{
				throw new ArgumentNullException("information");
			}
			if (information.Chunks.Any(delegate(ChunkSerializable ch)
			{
				if (ch.Begin != null)
				{
					long? num = ch.Current;
					if (num != null)
					{
						return ch.Begin > ch.Current;
					}
				}
				return true;
			}))
			{
				throw new ArgumentException();
			}
		}

		// Token: 0x04000002 RID: 2
		private readonly IStreamContainer streamContainer;
	}
}
