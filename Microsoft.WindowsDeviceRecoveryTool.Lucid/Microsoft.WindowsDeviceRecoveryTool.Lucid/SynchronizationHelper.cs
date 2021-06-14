using System;
using System.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.Lucid
{
	// Token: 0x02000009 RID: 9
	public static class SynchronizationHelper
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00003048 File Offset: 0x00001248
		public static EventHandler<T> ExecuteInCurrentContext<T>(EventHandler<T> handler)
		{
			SynchronizationContext context = SynchronizationContext.Current;
			return delegate(object sender, T e)
			{
				context.Post(delegate(object _)
				{
					handler(sender, e);
				}, null);
			};
		}
	}
}
