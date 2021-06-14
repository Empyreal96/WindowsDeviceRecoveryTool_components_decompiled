using System;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000076 RID: 118
	public interface IContinuationToken
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000E83 RID: 3715
		// (set) Token: 0x06000E84 RID: 3716
		StorageLocation? TargetLocation { get; set; }
	}
}
