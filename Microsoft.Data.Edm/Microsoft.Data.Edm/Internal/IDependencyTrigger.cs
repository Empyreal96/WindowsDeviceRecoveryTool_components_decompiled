using System;

namespace Microsoft.Data.Edm.Internal
{
	// Token: 0x020001C5 RID: 453
	internal interface IDependencyTrigger
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000ABF RID: 2751
		HashSetInternal<IDependent> Dependents { get; }
	}
}
