using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nokia.Mira.Chunks.Serialization
{
	// Token: 0x02000027 RID: 39
	[XmlType(TypeName = "WebFile")]
	[Serializable]
	public sealed class WebFileSerializable
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000346D File Offset: 0x0000166D
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00003475 File Offset: 0x00001675
		[XmlArray("ChunkInformations")]
		public List<ChunkSerializable> Chunks
		{
			get
			{
				return this.chunks;
			}
			set
			{
				this.chunks = value;
			}
		}

		// Token: 0x0400004C RID: 76
		private List<ChunkSerializable> chunks = new List<ChunkSerializable>();
	}
}
