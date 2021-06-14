using System;
using System.Threading;
using System.Windows.Input;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x02000043 RID: 67
	public interface IAsyncDelegateCommand : IDelegateCommand, ICommand
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000258 RID: 600
		CancellationTokenSource CancellationTokenSource { get; }

		// Token: 0x06000259 RID: 601
		void Cancel();

		// Token: 0x0600025A RID: 602
		void Wait();
	}
}
