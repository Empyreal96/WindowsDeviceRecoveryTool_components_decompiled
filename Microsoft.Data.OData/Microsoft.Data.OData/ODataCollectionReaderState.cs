using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200023A RID: 570
	public enum ODataCollectionReaderState
	{
		// Token: 0x04000695 RID: 1685
		Start,
		// Token: 0x04000696 RID: 1686
		CollectionStart,
		// Token: 0x04000697 RID: 1687
		Value,
		// Token: 0x04000698 RID: 1688
		CollectionEnd,
		// Token: 0x04000699 RID: 1689
		Exception,
		// Token: 0x0400069A RID: 1690
		Completed
	}
}
