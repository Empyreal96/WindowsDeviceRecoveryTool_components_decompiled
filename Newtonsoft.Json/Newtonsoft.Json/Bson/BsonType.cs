using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000012 RID: 18
	internal enum BsonType : sbyte
	{
		// Token: 0x04000056 RID: 86
		Number = 1,
		// Token: 0x04000057 RID: 87
		String,
		// Token: 0x04000058 RID: 88
		Object,
		// Token: 0x04000059 RID: 89
		Array,
		// Token: 0x0400005A RID: 90
		Binary,
		// Token: 0x0400005B RID: 91
		Undefined,
		// Token: 0x0400005C RID: 92
		Oid,
		// Token: 0x0400005D RID: 93
		Boolean,
		// Token: 0x0400005E RID: 94
		Date,
		// Token: 0x0400005F RID: 95
		Null,
		// Token: 0x04000060 RID: 96
		Regex,
		// Token: 0x04000061 RID: 97
		Reference,
		// Token: 0x04000062 RID: 98
		Code,
		// Token: 0x04000063 RID: 99
		Symbol,
		// Token: 0x04000064 RID: 100
		CodeWScope,
		// Token: 0x04000065 RID: 101
		Integer,
		// Token: 0x04000066 RID: 102
		TimeStamp,
		// Token: 0x04000067 RID: 103
		Long,
		// Token: 0x04000068 RID: 104
		MinKey = -1,
		// Token: 0x04000069 RID: 105
		MaxKey = 127
	}
}
