using System;
using System.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.MTP
{
	// Token: 0x0200001F RID: 31
	internal static class SynchronizationHelper
	{
		// Token: 0x06000122 RID: 290 RVA: 0x000075C0 File Offset: 0x000057C0
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
