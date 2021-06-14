using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x02000089 RID: 137
	public interface ICanHandle<in T> : ICanHandle
	{
		// Token: 0x060003B9 RID: 953
		void Handle(T message);
	}
}
