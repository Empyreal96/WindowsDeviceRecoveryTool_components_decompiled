using System;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C7 RID: 1991
	[Flags]
	internal enum AttachmentLevel
	{
		// Token: 0x04003A21 RID: 14881
		Full = 7,
		// Token: 0x04003A22 RID: 14882
		StartPortion = 4,
		// Token: 0x04003A23 RID: 14883
		MiddlePortion = 2,
		// Token: 0x04003A24 RID: 14884
		EndPortion = 1,
		// Token: 0x04003A25 RID: 14885
		Incomplete = 256,
		// Token: 0x04003A26 RID: 14886
		Unresolved = 0
	}
}
