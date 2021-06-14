using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000010 RID: 16
	internal class BsonRegex : BsonToken
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000044EA File Offset: 0x000026EA
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000044F2 File Offset: 0x000026F2
		public BsonString Pattern { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000044FB File Offset: 0x000026FB
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00004503 File Offset: 0x00002703
		public BsonString Options { get; set; }

		// Token: 0x06000093 RID: 147 RVA: 0x0000450C File Offset: 0x0000270C
		public BsonRegex(string pattern, string options)
		{
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000452E File Offset: 0x0000272E
		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}
	}
}
