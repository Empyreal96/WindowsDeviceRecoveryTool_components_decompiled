using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x02000078 RID: 120
	internal enum QueryOperator
	{
		// Token: 0x040001D5 RID: 469
		None,
		// Token: 0x040001D6 RID: 470
		Equals,
		// Token: 0x040001D7 RID: 471
		NotEquals,
		// Token: 0x040001D8 RID: 472
		Exists,
		// Token: 0x040001D9 RID: 473
		LessThan,
		// Token: 0x040001DA RID: 474
		LessThanOrEquals,
		// Token: 0x040001DB RID: 475
		GreaterThan,
		// Token: 0x040001DC RID: 476
		GreaterThanOrEquals,
		// Token: 0x040001DD RID: 477
		And,
		// Token: 0x040001DE RID: 478
		Or
	}
}
