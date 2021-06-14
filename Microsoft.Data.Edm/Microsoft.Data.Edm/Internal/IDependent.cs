using System;

namespace Microsoft.Data.Edm.Internal
{
	// Token: 0x020001C7 RID: 455
	internal interface IDependent : IFlushCaches
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000AC1 RID: 2753
		HashSetInternal<IDependencyTrigger> DependsOn { get; }
	}
}
