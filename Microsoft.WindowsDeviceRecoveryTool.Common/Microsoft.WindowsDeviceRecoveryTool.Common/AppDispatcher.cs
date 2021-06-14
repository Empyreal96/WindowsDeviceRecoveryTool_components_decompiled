using System;
using System.Windows.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.Common
{
	// Token: 0x02000002 RID: 2
	public static class AppDispatcher
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static Action<Action, bool> UiExecutor
		{
			get
			{
				return AppDispatcher.executor;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002067 File Offset: 0x00000267
		public static void Execute(Action action, bool isSync = false)
		{
			AppDispatcher.executor(action, isSync);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020DC File Offset: 0x000002DC
		public static void Initialize(Dispatcher dispatcher)
		{
			AppDispatcher.executor = delegate(Action action, bool isSync)
			{
				if (dispatcher.CheckAccess())
				{
					action();
				}
				else if (isSync)
				{
					dispatcher.Invoke(action);
				}
				else
				{
					dispatcher.BeginInvoke(action, new object[0]);
				}
			};
		}

		// Token: 0x04000001 RID: 1
		private static Action<Action, bool> executor = delegate(Action action, bool isSync)
		{
			action();
		};
	}
}
