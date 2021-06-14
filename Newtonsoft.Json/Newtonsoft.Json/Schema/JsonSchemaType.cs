using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000097 RID: 151
	[Flags]
	public enum JsonSchemaType
	{
		// Token: 0x0400029E RID: 670
		None = 0,
		// Token: 0x0400029F RID: 671
		String = 1,
		// Token: 0x040002A0 RID: 672
		Float = 2,
		// Token: 0x040002A1 RID: 673
		Integer = 4,
		// Token: 0x040002A2 RID: 674
		Boolean = 8,
		// Token: 0x040002A3 RID: 675
		Object = 16,
		// Token: 0x040002A4 RID: 676
		Array = 32,
		// Token: 0x040002A5 RID: 677
		Null = 64,
		// Token: 0x040002A6 RID: 678
		Any = 127
	}
}
