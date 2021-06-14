using System;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x0200024E RID: 590
	internal enum JsonNodeType
	{
		// Token: 0x040006EA RID: 1770
		None,
		// Token: 0x040006EB RID: 1771
		StartObject,
		// Token: 0x040006EC RID: 1772
		EndObject,
		// Token: 0x040006ED RID: 1773
		StartArray,
		// Token: 0x040006EE RID: 1774
		EndArray,
		// Token: 0x040006EF RID: 1775
		Property,
		// Token: 0x040006F0 RID: 1776
		PrimitiveValue,
		// Token: 0x040006F1 RID: 1777
		EndOfInput
	}
}
