using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.States.BaseStates
{
	// Token: 0x02000076 RID: 118
	public abstract class BaseStateMachineState : StateMachineState
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00010AC8 File Offset: 0x0000ECC8
		// (set) Token: 0x0600036F RID: 879 RVA: 0x00010ADF File Offset: 0x0000ECDF
		internal CompositionContainer Container { get; set; }

		// Token: 0x06000370 RID: 880 RVA: 0x00010AE8 File Offset: 0x0000ECE8
		public void ShowRegions(params string[] regions)
		{
			this.VisibleRegions.AddRange(regions);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00010AF8 File Offset: 0x0000ECF8
		public void HideRegions(params string[] regions)
		{
			this.InvisibleRegions.AddRange(regions);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00010B24 File Offset: 0x0000ED24
		public override void Start()
		{
			this.VisibleRegions.ForEach(delegate(string region)
			{
				RegionManager.Instance.ShowRegion(region);
			});
			this.InvisibleRegions.ForEach(delegate(string region)
			{
				RegionManager.Instance.HideRegion(region);
			});
			base.Start();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00010B8F File Offset: 0x0000ED8F
		public virtual void InitializeStateMachine()
		{
			this.InitializeStates();
			this.InitializeTransitions();
			this.ConfigureStates();
		}

		// Token: 0x06000374 RID: 884
		protected abstract void InitializeTransitions();

		// Token: 0x06000375 RID: 885
		protected abstract void InitializeStates();

		// Token: 0x06000376 RID: 886
		protected abstract void ConfigureStates();

		// Token: 0x06000377 RID: 887 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		protected UiState<TView, TViewModel> GetUiState<TView, TViewModel>(string showInRegion = null) where TView : FrameworkElement where TViewModel : BaseViewModel
		{
			return new UiState<TView, TViewModel>(this.Container.Get<TView>(), this.Container.Get<TViewModel>(), showInRegion);
		}

		// Token: 0x04000181 RID: 385
		protected readonly List<string> VisibleRegions = new List<string>();

		// Token: 0x04000182 RID: 386
		protected readonly List<string> InvisibleRegions = new List<string>();
	}
}
