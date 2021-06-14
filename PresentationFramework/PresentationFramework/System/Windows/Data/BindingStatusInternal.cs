using System;

namespace System.Windows.Data
{
	// Token: 0x02000199 RID: 409
	internal enum BindingStatusInternal : byte
	{
		// Token: 0x040012E0 RID: 4832
		Unattached,
		// Token: 0x040012E1 RID: 4833
		Inactive,
		// Token: 0x040012E2 RID: 4834
		Active,
		// Token: 0x040012E3 RID: 4835
		Detached,
		// Token: 0x040012E4 RID: 4836
		AsyncRequestPending,
		// Token: 0x040012E5 RID: 4837
		PathError,
		// Token: 0x040012E6 RID: 4838
		UpdateTargetError,
		// Token: 0x040012E7 RID: 4839
		UpdateSourceError
	}
}
