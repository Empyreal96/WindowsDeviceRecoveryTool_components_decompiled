using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.Transitions;
using Microsoft.WindowsDeviceRecoveryTool.States.BaseStates;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000DA RID: 218
	[Export]
	public class SettingsState : UiStateMachineState<MainSettingsView, MainSettingsViewModel>
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x0002358A File Offset: 0x0002178A
		public override void InitializeStateMachine()
		{
			base.SetViewViewModel(base.Container.Get<MainSettingsView>(), base.Container.Get<MainSettingsViewModel>());
			base.InitializeStateMachine();
			base.Machine.AddState(this.preferencesState);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000235C3 File Offset: 0x000217C3
		protected override void ConfigureStates()
		{
			this.ConfigurePreferencesState();
			this.ConfigureNetworkState();
			this.ConfigureTraceState();
			this.ConfigurePackagesState();
			this.ConfigureApplicationDataState();
			this.ConfigureFolderBrowseState();
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000235F0 File Offset: 0x000217F0
		protected override void InitializeStates()
		{
			this.preferencesState = base.GetUiState<PreferencesView, PreferencesViewModel>(null);
			this.networkState = base.GetUiState<NetworkView, NetworkViewModel>(null);
			this.traceState = base.GetUiState<TraceView, TraceViewModel>(null);
			this.packagesState = base.GetUiState<PackagesView, PackagesViewModel>(null);
			this.applicationDataState = base.GetUiState<ApplicationDataView, ApplicationDataViewModel>(null);
			this.folderBrowseState = base.GetUiState<FolderBrowsingView, FolderBrowsingViewModel>(null);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002364C File Offset: 0x0002184C
		protected override void InitializeTransitions()
		{
			this.preferencesTransition = new StateStatusTransition(this.preferencesState, "PreferencesState");
			this.networkTransition = new StateStatusTransition(this.networkState, "NetworkState");
			this.traceTransition = new StateStatusTransition(this.traceState, "TraceState");
			this.packagesTransition = new StateStatusTransition(this.packagesState, "PackagesState");
			this.applicationDataTransition = new StateStatusTransition(this.applicationDataState, "ApplicationDataState");
			this.folderBrowseTransition = new StateStatusTransition(this.folderBrowseState, "FolderBrowseAreaState");
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000236E0 File Offset: 0x000218E0
		private void ConfigureTraceState()
		{
			this.traceState.AddConditionalTransition(this.preferencesTransition);
			this.traceState.AddConditionalTransition(this.networkTransition);
			this.traceState.AddConditionalTransition(this.traceTransition);
			this.traceState.AddConditionalTransition(this.packagesTransition);
			this.traceState.AddConditionalTransition(this.applicationDataTransition);
			this.traceState.AddConditionalTransition(this.folderBrowseTransition);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002375C File Offset: 0x0002195C
		private void ConfigureNetworkState()
		{
			this.networkState.AddConditionalTransition(this.preferencesTransition);
			this.networkState.AddConditionalTransition(this.networkTransition);
			this.networkState.AddConditionalTransition(this.traceTransition);
			this.networkState.AddConditionalTransition(this.packagesTransition);
			this.networkState.AddConditionalTransition(this.applicationDataTransition);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000237C4 File Offset: 0x000219C4
		private void ConfigurePreferencesState()
		{
			this.preferencesState.AddConditionalTransition(this.preferencesTransition);
			this.preferencesState.AddConditionalTransition(this.networkTransition);
			this.preferencesState.AddConditionalTransition(this.traceTransition);
			this.preferencesState.AddConditionalTransition(this.packagesTransition);
			this.preferencesState.AddConditionalTransition(this.applicationDataTransition);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002382C File Offset: 0x00021A2C
		private void ConfigurePackagesState()
		{
			this.packagesState.AddConditionalTransition(this.preferencesTransition);
			this.packagesState.AddConditionalTransition(this.networkTransition);
			this.packagesState.AddConditionalTransition(this.traceTransition);
			this.packagesState.AddConditionalTransition(this.packagesTransition);
			this.packagesState.AddConditionalTransition(this.applicationDataTransition);
			this.packagesState.AddConditionalTransition(this.folderBrowseTransition);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000238A8 File Offset: 0x00021AA8
		private void ConfigureApplicationDataState()
		{
			this.applicationDataState.AddConditionalTransition(this.preferencesTransition);
			this.applicationDataState.AddConditionalTransition(this.networkTransition);
			this.applicationDataState.AddConditionalTransition(this.traceTransition);
			this.applicationDataState.AddConditionalTransition(this.packagesTransition);
			this.applicationDataState.AddConditionalTransition(this.applicationDataTransition);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00023910 File Offset: 0x00021B10
		private void ConfigureFolderBrowseState()
		{
			this.folderBrowseState.AddConditionalTransition(this.traceTransition);
			this.folderBrowseState.AddConditionalTransition(this.packagesTransition);
		}

		// Token: 0x040002D3 RID: 723
		private UiBaseState preferencesState;

		// Token: 0x040002D4 RID: 724
		private UiBaseState traceState;

		// Token: 0x040002D5 RID: 725
		private UiBaseState networkState;

		// Token: 0x040002D6 RID: 726
		private UiBaseState packagesState;

		// Token: 0x040002D7 RID: 727
		private UiBaseState applicationDataState;

		// Token: 0x040002D8 RID: 728
		private UiBaseState folderBrowseState;

		// Token: 0x040002D9 RID: 729
		private StateStatusTransition preferencesTransition;

		// Token: 0x040002DA RID: 730
		private StateStatusTransition networkTransition;

		// Token: 0x040002DB RID: 731
		private StateStatusTransition traceTransition;

		// Token: 0x040002DC RID: 732
		private StateStatusTransition packagesTransition;

		// Token: 0x040002DD RID: 733
		private StateStatusTransition applicationDataTransition;

		// Token: 0x040002DE RID: 734
		private StateStatusTransition folderBrowseTransition;
	}
}
