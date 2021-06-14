using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200000F RID: 15
	internal class BsonBinary : BsonValue
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000044C8 File Offset: 0x000026C8
		// (set) Token: 0x0600008D RID: 141 RVA: 0x000044D0 File Offset: 0x000026D0
		public BsonBinaryType BinaryType { get; set; }

		// Token: 0x0600008E RID: 142 RVA: 0x000044D9 File Offset: 0x000026D9
		public BsonBinary(byte[] value, BsonBinaryType binaryType) : base(value, BsonType.Binary)
		{
			this.BinaryType = binaryType;
		}
	}
}
