using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000011 RID: 17
	internal class BsonProperty
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004532 File Offset: 0x00002732
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000453A File Offset: 0x0000273A
		public BsonString Name { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004543 File Offset: 0x00002743
		// (set) Token: 0x06000098 RID: 152 RVA: 0x0000454B File Offset: 0x0000274B
		public BsonToken Value { get; set; }
	}
}
