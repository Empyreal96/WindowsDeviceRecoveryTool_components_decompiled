using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000059 RID: 89
	public enum JsonToken
	{
		// Token: 0x04000173 RID: 371
		None,
		// Token: 0x04000174 RID: 372
		StartObject,
		// Token: 0x04000175 RID: 373
		StartArray,
		// Token: 0x04000176 RID: 374
		StartConstructor,
		// Token: 0x04000177 RID: 375
		PropertyName,
		// Token: 0x04000178 RID: 376
		Comment,
		// Token: 0x04000179 RID: 377
		Raw,
		// Token: 0x0400017A RID: 378
		Integer,
		// Token: 0x0400017B RID: 379
		Float,
		// Token: 0x0400017C RID: 380
		String,
		// Token: 0x0400017D RID: 381
		Boolean,
		// Token: 0x0400017E RID: 382
		Null,
		// Token: 0x0400017F RID: 383
		Undefined,
		// Token: 0x04000180 RID: 384
		EndObject,
		// Token: 0x04000181 RID: 385
		EndArray,
		// Token: 0x04000182 RID: 386
		EndConstructor,
		// Token: 0x04000183 RID: 387
		Date,
		// Token: 0x04000184 RID: 388
		Bytes
	}
}
