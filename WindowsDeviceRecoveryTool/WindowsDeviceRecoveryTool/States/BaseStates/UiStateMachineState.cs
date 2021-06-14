using System;
using System.Windows;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.BaseStates
{
	// Token: 0x02000077 RID: 119
	public abstract class UiStateMachineState<TView, TViewModel> : BaseStateMachineState where TView : FrameworkElement where TViewModel : BaseViewModel
	{
		// Token: 0x0600037B RID: 891 RVA: 0x00010BF5 File Offset: 0x0000EDF5
		protected void SetViewViewModel(TView view, TViewModel viewModel)
		{
			this.view = view;
			this.viewModel = viewModel;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00010C08 File Offset: 0x0000EE08
		public override void Start()
		{
			this.view.DataContext = this.viewModel;
			RegionAttribute attribute = this.view.GetAttribute<RegionAttribute>();
			if (attribute == null)
			{
				throw new NotImplementedException(string.Format("The class {0} should have RegionAttribute. Please correct it.", this.view.GetType().Name));
			}
			string regionName = attribute.Names[0];
			RegionManager.Instance.ShowView(regionName, this.view);
			base.Start();
			this.viewModel.IsStarted = true;
			this.viewModel.OnStarted();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00010CC6 File Offset: 0x0000EEC6
		public override void Stop()
		{
			this.viewModel.IsStarted = false;
			this.viewModel.OnStopped();
			base.Stop();
		}

		// Token: 0x04000186 RID: 390
		private TView view;

		// Token: 0x04000187 RID: 391
		private TViewModel viewModel;
	}
}
