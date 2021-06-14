using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000004 RID: 4
	public class BsonObjectId
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002841 File Offset: 0x00000A41
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002849 File Offset: 0x00000A49
		public byte[] Value { get; private set; }

		// Token: 0x06000010 RID: 16 RVA: 0x00002852 File Offset: 0x00000A52
		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			this.Value = value;
		}
	}
}
