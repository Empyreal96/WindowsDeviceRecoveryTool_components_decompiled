using System;

namespace System.Windows.Documents
{
	// Token: 0x0200034D RID: 845
	internal enum FlowNodeType : byte
	{
		// Token: 0x04001D83 RID: 7555
		Boundary,
		// Token: 0x04001D84 RID: 7556
		Start,
		// Token: 0x04001D85 RID: 7557
		Run,
		// Token: 0x04001D86 RID: 7558
		End = 4,
		// Token: 0x04001D87 RID: 7559
		Object = 8,
		// Token: 0x04001D88 RID: 7560
		Virtual = 16,
		// Token: 0x04001D89 RID: 7561
		Noop = 32
	}
}
