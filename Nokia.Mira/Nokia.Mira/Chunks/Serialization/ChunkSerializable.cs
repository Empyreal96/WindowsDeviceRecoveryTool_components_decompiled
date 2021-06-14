using System;
using System.Xml.Serialization;

namespace Nokia.Mira.Chunks.Serialization
{
	// Token: 0x0200000B RID: 11
	[XmlType(TypeName = "ChunkInformation")]
	[Serializable]
	public sealed class ChunkSerializable
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000025A9 File Offset: 0x000007A9
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000025B1 File Offset: 0x000007B1
		[XmlElement(IsNullable = true)]
		public long? Begin { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000025BA File Offset: 0x000007BA
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000025C2 File Offset: 0x000007C2
		[XmlElement(IsNullable = true)]
		public long? Current { get; set; }
	}
}
