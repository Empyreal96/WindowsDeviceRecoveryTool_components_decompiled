using System;
using System.Windows.Input;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x02000042 RID: 66
	public interface IDelegateCommand : ICommand
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000256 RID: 598
		KeyGesture KeyGesture { get; }

		// Token: 0x06000257 RID: 599
		void RaiseCanExecuteChanged();
	}
}
