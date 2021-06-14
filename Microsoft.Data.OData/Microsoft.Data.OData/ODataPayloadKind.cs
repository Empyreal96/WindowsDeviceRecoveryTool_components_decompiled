using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000274 RID: 628
	public enum ODataPayloadKind
	{
		// Token: 0x04000758 RID: 1880
		Feed,
		// Token: 0x04000759 RID: 1881
		Entry,
		// Token: 0x0400075A RID: 1882
		Property,
		// Token: 0x0400075B RID: 1883
		EntityReferenceLink,
		// Token: 0x0400075C RID: 1884
		EntityReferenceLinks,
		// Token: 0x0400075D RID: 1885
		Value,
		// Token: 0x0400075E RID: 1886
		BinaryValue,
		// Token: 0x0400075F RID: 1887
		Collection,
		// Token: 0x04000760 RID: 1888
		ServiceDocument,
		// Token: 0x04000761 RID: 1889
		MetadataDocument,
		// Token: 0x04000762 RID: 1890
		Error,
		// Token: 0x04000763 RID: 1891
		Batch,
		// Token: 0x04000764 RID: 1892
		Parameter,
		// Token: 0x04000765 RID: 1893
		Unsupported = 2147483647
	}
}
