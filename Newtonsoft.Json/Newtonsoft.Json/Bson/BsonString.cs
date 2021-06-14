using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200000E RID: 14
	internal class BsonString : BsonValue
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00004495 File Offset: 0x00002695
		// (set) Token: 0x06000088 RID: 136 RVA: 0x0000449D File Offset: 0x0000269D
		public int ByteCount { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000044A6 File Offset: 0x000026A6
		// (set) Token: 0x0600008A RID: 138 RVA: 0x000044AE File Offset: 0x000026AE
		public bool IncludeLength { get; set; }

		// Token: 0x0600008B RID: 139 RVA: 0x000044B7 File Offset: 0x000026B7
		public BsonString(object value, bool includeLength) : base(value, BsonType.String)
		{
			this.IncludeLength = includeLength;
		}
	}
}
