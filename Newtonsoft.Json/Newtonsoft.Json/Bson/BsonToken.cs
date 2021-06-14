using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200000A RID: 10
	internal abstract class BsonToken
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000074 RID: 116
		public abstract BsonType Type { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00004395 File Offset: 0x00002595
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000439D File Offset: 0x0000259D
		public BsonToken Parent { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000043A6 File Offset: 0x000025A6
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000043AE File Offset: 0x000025AE
		public int CalculatedSize { get; set; }
	}
}
