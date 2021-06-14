using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.Transitions;
using Microsoft.WindowsDeviceRecoveryTool.States.BaseStates;
using Microsoft.WindowsDeviceRecoveryTool.States.Error;
using Microsoft.WindowsDeviceRecoveryTool.States.Help;
using Microsoft.WindowsDeviceRecoveryTool.States.Preparing;
using Microsoft.WindowsDeviceRecoveryTool.States.Settings;
using Microsoft.WindowsDeviceRecoveryTool.States.Workflow;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Shell
{
	// Token: 0x020000DD RID: 221
	[Export]
	public class ShellState : BaseStateMachineState
	{
		// Token: 0x060006E0 RID: 1760 RVA: 0x00023E90 File Offset: 0x00022090
		public void StartStateMachine()
		{
			base.Machine.Start();
			this.appContext.IsMachineStateRunning = true;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00023EAC File Offset: 0x000220AC
		public override void InitializeStateMachine()
		{
			base.InitializeStateMachine();
			base.Machine.AddState(this.checkAppAutoUpdateState);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00023EC8 File Offset: 0x000220C8
		protected override void OnCurrentStateChanged(BaseState oldValue, BaseState newValue)
		{
			if (oldValue != newValue)
			{
				this.previousTransition.SetNextState(oldValue);
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00023EF0 File Offset: 0x000220F0
		protected override void ConfigureStates()
		{
			this.ConfigureHelpState();
			this.ConfigureCheckAppAutoUpdateState();
			this.ConfigureAppAutoUpdateState();
			this.ConfigureManualPackageSelectionState();
			this.ConfigureAutomaticPackageSelectionState();
			this.ConfigureAwaitGenericDeviceState();
			this.ConfigureAwaitHtcState();
			this.ConfigureAwaitAnalogDeviceState();
			this.ConfigureFlashingState();
			this.ConfigureSummaryState();
			this.ConfigureManualManufacturerSelectionState();
			this.ConfigureAutomaticManufacturerSelectionState();
			this.ConfigurePackageIntegrityCheckState();
			this.ConfigureErrorState();
			this.ConfigureSettingsState();
			this.ConfigureCheckLatestPackageState();
			this.ConfigureDownloadPackageState();
			this.ConfigureAwaitRecoveryDeviceState();
			this.ConfigureDeviceDetectionState();
			this.ConfigureReadingDeviceInfoState();
			this.ConfigureReadingDeviceInfoWithThorState();
			this.ConfigureBatteryCheckingState();
			this.ConfigureDisclaimerState();
			this.ConfigureManualHtcRestartState();
			this.ConfigureRebootHtcState();
			this.ConfigureManualDeviceTypeSelectionState();
			this.ConfigureDownloadEmergencyPackageState();
			this.ConfigureAwaitRecoveryModeAfterEmergencyFlashingState();
			this.ConfigureAbsoluteConfirmationState();
			this.ConfigureManualGenericModelSelectionState();
			this.ConfigureManualGenericVariantSelectionState();
			this.ConfigureAwaitHoloLensAccessoryDeviceState();
			this.ConfigureUnsupportedDeviceState();
			this.ConfigureSurveyState();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00023FEC File Offset: 0x000221EC
		protected override void InitializeStates()
		{
			this.checkAppAutoUpdateState = base.GetUiState<AppUpdateCheckingView, AppUpdateCheckingViewModel>(null);
			this.appAutoUpdateState = base.GetUiState<DownloadAppUpdateView, DownloadAppUpdateViewModel>(null);
			this.manualPackageSelectionState = base.GetUiState<ManualPackageSelectionView, ManualPackageSelectionViewModel>(null);
			this.automaticPackageSelectionState = base.GetUiState<AutomaticPackageSelectionView, AutomaticPackageSelectionViewModel>(null);
			this.checkLatestPackageState = base.GetUiState<CheckLatestPackageView, CheckLatestPackageViewModel>(null);
			this.downloadPackageState = base.GetUiState<DownloadPackageView, DownloadPackageViewModel>(null);
			this.awaitGenericDeviceState = base.GetUiState<AwaitGenericDeviceView, AwaitGenericDeviceViewModel>(null);
			this.awaitHtcState = base.GetUiState<AwaitHtcView, AwaitHtcViewModel>(null);
			this.awaitAnalogDeviceState = base.GetUiState<AwaitAnalogDeviceView, AwaitAnalogDeviceViewModel>(null);
			this.awaitRecoveryDeviceState = base.GetUiState<AwaitRecoveryDeviceView, AwaitRecoveryDeviceViewModel>(null);
			this.flashingState = base.GetUiState<FlashingView, FlashingViewModel>(null);
			this.summaryState = base.GetUiState<SummaryView, SummaryViewModel>(null);
			this.manualManufacturerSelectionState = base.GetUiState<ManualManufacturerSelectionView, ManualManufacturerSelectionViewModel>(null);
			this.automaticManufacturerSelectionState = base.GetUiState<AutomaticManufacturerSelectionView, AutomaticManufacturerSelectionViewModel>(null);
			this.packageIntegrityCheckState = base.GetUiState<PackageIntegrityCheckView, PackageIntegrityCheckViewModel>(null);
			this.errorState = base.GetUiState<ErrorView, ErrorViewModel>(null);
			this.deviceSelectionState = base.GetUiState<DeviceSelectionView, DeviceSelectionViewModel>(null);
			this.readingDeviceInfoState = base.GetUiState<ReadingDeviceInfoView, ReadingDeviceInfoViewModel>(null);
			this.readingDeviceInfoWithThorState = base.GetUiState<ReadingDeviceInfoView, ReadingDeviceInfoWithThorViewModel>(null);
			this.batteryCheckingState = base.GetUiState<BatteryCheckingView, BatteryCheckingViewModel>(null);
			this.settingsState = new SettingsState
			{
				Container = base.Container
			};
			this.disclaimerState = base.GetUiState<DisclaimerView, DisclaimerViewModel>(null);
			this.manualHtcRestartState = base.GetUiState<ManualHtcRestartView, ManualHtcRestartViewModel>(null);
			this.rebootHtcState = base.GetUiState<RebootHtcView, RebootHtcViewModel>(null);
			this.manualDeviceTypeSelectionState = base.GetUiState<ManualDeviceTypeSelectionView, ManualDeviceTypeSelectionViewModel>(null);
			this.downloadEmergencyPackageState = base.GetUiState<DownloadEmergencyPackageView, DownloadEmergencyPackageViewModel>(null);
			this.awaitRecoveryModeAfterEmergencyFlashingState = base.GetUiState<AwaitRecoveryDeviceView, AwaitRecoveryAfterEmergencyDeviceViewModel>(null);
			this.absoluteConfirmationState = base.GetUiState<AbsoluteConfirmationView, AbsoluteConfirmationViewModel>(null);
			this.helpState = new HelpState
			{
				Container = base.Container
			};
			this.manualGenericModelSelectionState = base.GetUiState<ManualGenericModelSelectionView, ManualGenericModelSelectionViewModel>(null);
			this.manualGenericVariantSelectionState = base.GetUiState<ManualGenericVariantSelectionView, ManualGenericVariantSelectionViewModel>(null);
			this.unsupportedDeviceState = base.GetUiState<UnsupportedDeviceView, UnsupportedDeviceViewModel>(null);
			this.surveyState = base.GetUiState<SurveyView, SurveyViewModel>(null);
			this.awaitFawkesDeviceState = base.GetUiState<AwaitFawkesDeviceView, AwaitFawkesDeviceViewModel>(null);
			this.settingsState.InitializeStateMachine();
			this.helpState.InitializeStateMachine();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002421C File Offset: 0x0002241C
		protected override void InitializeTransitions()
		{
			this.appAutoUpdateTransition = new StateStatusTransition(this.appAutoUpdateState, "AppAutoUpdateState");
			this.checkAppAutoUpdateTransition = new StateStatusTransition(this.checkAppAutoUpdateState, "CheckAppAutoUpdateState");
			this.manualPackageSelectionTransition = new StateStatusTransition(this.manualPackageSelectionState, "ManualPackageSelectionState");
			this.automaticPackageSelectionTransition = new StateStatusTransition(this.automaticPackageSelectionState, "AutomaticPackageSelectionState");
			this.checkLatestPackageTransition = new StateStatusTransition(this.checkLatestPackageState, "CheckLatestPackageState");
			this.downloadPackageTransition = new StateStatusTransition(this.downloadPackageState, "DownloadPackageState");
			this.flashingStateTransition = new StateStatusTransition(this.flashingState, "FlashingState");
			this.awaitGenericDeviceTransition = new StateStatusTransition(this.awaitGenericDeviceState, "AwaitGenericDeviceState");
			this.awaitHtcTransition = new StateStatusTransition(this.awaitHtcState, "AwaitHtcState");
			this.awaitAnalogDeviceTransition = new StateStatusTransition(this.awaitAnalogDeviceState, "AwaitAnalogDeviceState");
			this.awaitRecoveryDeviceTransition = new StateStatusTransition(this.awaitRecoveryDeviceState, "AwaitRecoveryDeviceState");
			this.summaryTransition = new StateStatusTransition(this.summaryState, "SummaryState");
			this.manualManufacturerSelectionTransition = new StateStatusTransition(this.manualManufacturerSelectionState, "ManualManufacturerSelectionState");
			this.automaticManufacturerSelectionTransition = new StateStatusTransition(this.automaticManufacturerSelectionState, "AutomaticManufacturerSelectionState");
			this.packageIntegrityCheckTransition = new StateStatusTransition(this.packageIntegrityCheckState, "PackageIntegrityCheckState");
			this.previousTransition = new PreviousStateTransition(null, "PreviousState");
			this.errorTransition = new StateStatusTransition(this.errorState, "ErrorState");
			this.settingsTransition = new StateStatusTransition(this.settingsState, "SettingsState");
			this.deviceSelectionTransition = new StateStatusTransition(this.deviceSelectionState, "DeviceSelectionState");
			this.readingDeviceInfoTransition = new StateStatusTransition(this.readingDeviceInfoState, "ReadingDeviceInfoState");
			this.readingDeviceInfoWithThorTransition = new StateStatusTransition(this.readingDeviceInfoWithThorState, "ReadingDeviceInfoWithThorState");
			this.batteryCheckingTransition = new StateStatusTransition(this.batteryCheckingState, "BatteryCheckingState");
			this.disclaimerTransition = new StateStatusTransition(this.disclaimerState, "DisclaimerState");
			this.manualHtcRestartTransition = new StateStatusTransition(this.manualHtcRestartState, "ManualHtcRestartState");
			this.rebootHtcTransition = new StateStatusTransition(this.rebootHtcState, "RebootHtcState");
			this.manualDeviceTypeSelectionTransition = new StateStatusTransition(this.manualDeviceTypeSelectionState, "ManualDeviceTypeSelectionState");
			this.downloadEmergencyPackageTransition = new StateStatusTransition(this.downloadEmergencyPackageState, "DownloadEmergencyPackageState");
			this.awaitRecoveryModeAfterEmergencyFlashingTransition = new StateStatusTransition(this.awaitRecoveryModeAfterEmergencyFlashingState, "AwaitRecoveryModeAfterEmergencyFlashingState");
			this.rebootHtcAfterErrorTransition = new LambdaTransition(new Func<bool>(this.conditions.IsHtcConnected), this.rebootHtcState);
			this.helpTransition = new StateStatusTransition(this.helpState, "HelpState");
			this.manufacturerSelectionTransitionAfterError = new LambdaTransition(() => this.appContext.CurrentPhone == null || this.appContext.SelectedManufacturer != PhoneTypes.Htc, this.automaticManufacturerSelectionState);
			this.absoluteConfirmationTransition = new StateStatusTransition(this.absoluteConfirmationState, "AbsoluteConfirmationState");
			this.manualGenericModelSelectionTransition = new StateStatusTransition(this.manualGenericModelSelectionState, "ManualGenericModelSelectionState");
			this.manualGenericVariantSelectionTransition = new StateStatusTransition(this.manualGenericVariantSelectionState, "ManualGenericVariantSelectionState");
			this.awaitFawkesDeviceTransition = new StateStatusTransition(this.awaitFawkesDeviceState, "AwaitFawkesDeviceState");
			this.unsupportedDeviceTransition = new StateStatusTransition(this.unsupportedDeviceState, "UnsupportedDeviceState");
			this.surveyTransition = new StateStatusTransition(this.surveyState, "SurveyState");
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00024568 File Offset: 0x00022768
		private void ConfigureHelpState()
		{
			this.helpState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.helpState.AddConditionalTransition(this.previousTransition);
			this.helpState.AddConditionalTransition(this.deviceSelectionTransition);
			this.helpState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000245C8 File Offset: 0x000227C8
		private void ConfigureManualDeviceTypeSelectionState()
		{
			this.manualDeviceTypeSelectionState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.manualDeviceTypeSelectionState.AddConditionalTransition(this.previousTransition);
			this.manualDeviceTypeSelectionState.AddConditionalTransition(this.downloadEmergencyPackageTransition);
			this.manualDeviceTypeSelectionState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00024628 File Offset: 0x00022828
		private void ConfigureDownloadEmergencyPackageState()
		{
			this.downloadEmergencyPackageState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.downloadEmergencyPackageState.AddConditionalTransition(this.previousTransition);
			this.downloadEmergencyPackageState.AddConditionalTransition(this.flashingStateTransition);
			this.downloadEmergencyPackageState.AddConditionalTransition(this.errorTransition);
			this.downloadEmergencyPackageState.AddConditionalTransition(this.summaryTransition);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002469C File Offset: 0x0002289C
		private void ConfigureAwaitRecoveryModeAfterEmergencyFlashingState()
		{
			this.awaitRecoveryModeAfterEmergencyFlashingState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.awaitRecoveryModeAfterEmergencyFlashingState.AddConditionalTransition(this.errorTransition);
			this.awaitRecoveryModeAfterEmergencyFlashingState.AddConditionalTransition(this.awaitRecoveryDeviceTransition);
			this.awaitRecoveryModeAfterEmergencyFlashingState.AddConditionalTransition(this.summaryTransition);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000246FC File Offset: 0x000228FC
		private void ConfigureFlashingState()
		{
			this.flashingState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.flashingState.AddConditionalTransition(this.summaryTransition);
			this.flashingState.AddConditionalTransition(this.awaitRecoveryModeAfterEmergencyFlashingTransition);
			this.flashingState.AddConditionalTransition(this.errorTransition);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002475C File Offset: 0x0002295C
		private void ConfigureSettingsState()
		{
			this.settingsState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.settingsState.AddConditionalTransition(this.previousTransition);
			this.settingsState.AddConditionalTransition(this.errorTransition);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000247AC File Offset: 0x000229AC
		private void ConfigureErrorState()
		{
			this.errorState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.errorState.AddConditionalTransition(this.previousTransition);
			this.errorState.AddConditionalTransition(this.flashingStateTransition);
			this.errorState.AddConditionalTransition(this.summaryTransition);
			this.errorState.AddConditionalTransition(this.settingsTransition);
			this.errorState.AddConditionalTransition(this.awaitGenericDeviceTransition);
			this.errorState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.errorState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.errorState.AddConditionalTransition(this.checkAppAutoUpdateTransition);
			this.errorState.AddConditionalTransition(this.rebootHtcAfterErrorTransition);
			this.errorState.AddConditionalTransition(this.manufacturerSelectionTransitionAfterError);
			this.errorState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002489C File Offset: 0x00022A9C
		private void ConfigureRebootHtcState()
		{
			this.rebootHtcState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.rebootHtcState.AddConditionalTransition(this.settingsTransition);
			this.rebootHtcState.AddConditionalTransition(this.errorTransition);
			this.rebootHtcState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.rebootHtcState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00024918 File Offset: 0x00022B18
		private void ConfigureCheckAppAutoUpdateState()
		{
			this.checkAppAutoUpdateState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.checkAppAutoUpdateState.AddConditionalTransition(this.appAutoUpdateTransition);
			this.checkAppAutoUpdateState.AddConditionalTransition(this.settingsTransition);
			this.checkAppAutoUpdateState.AddConditionalTransition(this.errorTransition);
			this.checkAppAutoUpdateState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.checkAppAutoUpdateState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000249A4 File Offset: 0x00022BA4
		private void ConfigureAppAutoUpdateState()
		{
			this.appAutoUpdateState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.appAutoUpdateState.AddConditionalTransition(this.checkAppAutoUpdateTransition);
			this.appAutoUpdateState.AddConditionalTransition(this.errorTransition);
			this.appAutoUpdateState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00024A04 File Offset: 0x00022C04
		private void ConfigureManualPackageSelectionState()
		{
			this.manualPackageSelectionState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.manualPackageSelectionState.AddConditionalTransition(this.settingsTransition);
			this.manualPackageSelectionState.AddConditionalTransition(this.previousTransition);
			this.manualPackageSelectionState.AddConditionalTransition(this.errorTransition);
			this.manualPackageSelectionState.AddConditionalTransition(this.packageIntegrityCheckTransition);
			this.manualPackageSelectionState.AddConditionalTransition(this.helpTransition);
			this.manualPackageSelectionState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00024AA4 File Offset: 0x00022CA4
		private void ConfigureAutomaticPackageSelectionState()
		{
			this.automaticPackageSelectionState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.automaticPackageSelectionState.AddConditionalTransition(this.settingsTransition);
			this.automaticPackageSelectionState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.automaticPackageSelectionState.AddConditionalTransition(this.previousTransition);
			this.automaticPackageSelectionState.AddConditionalTransition(this.manualPackageSelectionTransition);
			this.automaticPackageSelectionState.AddConditionalTransition(this.errorTransition);
			this.automaticPackageSelectionState.AddConditionalTransition(this.packageIntegrityCheckTransition);
			this.automaticPackageSelectionState.AddConditionalTransition(this.helpTransition);
			this.automaticPackageSelectionState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00024B68 File Offset: 0x00022D68
		private void ConfigureCheckLatestPackageState()
		{
			this.checkLatestPackageState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.checkLatestPackageState.AddConditionalTransition(this.settingsTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.downloadPackageTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.previousTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.disclaimerTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.errorTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.rebootHtcTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.manualPackageSelectionTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.helpTransition);
			this.checkLatestPackageState.AddConditionalTransition(this.packageIntegrityCheckTransition);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00024C60 File Offset: 0x00022E60
		private void ConfigureDownloadPackageState()
		{
			this.downloadPackageState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.downloadPackageState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.downloadPackageState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.downloadPackageState.AddConditionalTransition(this.previousTransition);
			this.downloadPackageState.AddConditionalTransition(this.batteryCheckingTransition);
			this.downloadPackageState.AddConditionalTransition(this.flashingStateTransition);
			this.downloadPackageState.AddConditionalTransition(this.errorTransition);
			this.downloadPackageState.AddConditionalTransition(this.summaryTransition);
			this.downloadPackageState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00024D24 File Offset: 0x00022F24
		private void ConfigurePackageIntegrityCheckState()
		{
			this.packageIntegrityCheckState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.packageIntegrityCheckState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.packageIntegrityCheckState.AddConditionalTransition(this.previousTransition);
			this.packageIntegrityCheckState.AddConditionalTransition(this.batteryCheckingTransition);
			this.packageIntegrityCheckState.AddConditionalTransition(this.errorTransition);
			this.packageIntegrityCheckState.AddConditionalTransition(this.flashingStateTransition);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00024DB4 File Offset: 0x00022FB4
		private void ConfigureManualManufacturerSelectionState()
		{
			this.manualManufacturerSelectionState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.manualManufacturerSelectionState.AddConditionalTransition(this.settingsTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.awaitGenericDeviceTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.awaitRecoveryDeviceTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.awaitHtcTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.awaitAnalogDeviceTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.errorTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.previousTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.helpTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.manualGenericModelSelectionTransition);
			this.manualManufacturerSelectionState.AddConditionalTransition(this.awaitFawkesDeviceTransition);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00024EAC File Offset: 0x000230AC
		private void ConfigureAutomaticManufacturerSelectionState()
		{
			this.automaticManufacturerSelectionState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.automaticManufacturerSelectionState.AddConditionalTransition(this.settingsTransition);
			this.automaticManufacturerSelectionState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.automaticManufacturerSelectionState.AddConditionalTransition(this.errorTransition);
			this.automaticManufacturerSelectionState.AddConditionalTransition(this.deviceSelectionTransition);
			this.automaticManufacturerSelectionState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00024F38 File Offset: 0x00023138
		private void ConfigureAwaitGenericDeviceState()
		{
			this.awaitGenericDeviceState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.awaitGenericDeviceState.AddConditionalTransition(this.previousTransition);
			this.awaitGenericDeviceState.AddConditionalTransition(this.readingDeviceInfoTransition);
			this.awaitGenericDeviceState.AddConditionalTransition(this.automaticPackageSelectionTransition);
			this.awaitGenericDeviceState.AddConditionalTransition(this.settingsTransition);
			this.awaitGenericDeviceState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.awaitGenericDeviceState.AddConditionalTransition(this.errorTransition);
			this.awaitGenericDeviceState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.awaitGenericDeviceState.AddConditionalTransition(this.helpTransition);
			this.awaitGenericDeviceState.AddConditionalTransition(this.flashingStateTransition);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002500C File Offset: 0x0002320C
		private void ConfigureAwaitHtcState()
		{
			this.awaitHtcState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.awaitHtcState.AddConditionalTransition(this.previousTransition);
			this.awaitHtcState.AddConditionalTransition(this.readingDeviceInfoTransition);
			this.awaitHtcState.AddConditionalTransition(this.checkLatestPackageTransition);
			this.awaitHtcState.AddConditionalTransition(this.settingsTransition);
			this.awaitHtcState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.awaitHtcState.AddConditionalTransition(this.errorTransition);
			this.awaitHtcState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.awaitHtcState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000250D0 File Offset: 0x000232D0
		private void ConfigureAwaitAnalogDeviceState()
		{
			this.awaitAnalogDeviceState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.awaitAnalogDeviceState.AddConditionalTransition(this.previousTransition);
			this.awaitAnalogDeviceState.AddConditionalTransition(this.readingDeviceInfoTransition);
			this.awaitAnalogDeviceState.AddConditionalTransition(this.checkLatestPackageTransition);
			this.awaitAnalogDeviceState.AddConditionalTransition(this.settingsTransition);
			this.awaitAnalogDeviceState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.awaitAnalogDeviceState.AddConditionalTransition(this.errorTransition);
			this.awaitAnalogDeviceState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.awaitAnalogDeviceState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00025194 File Offset: 0x00023394
		private void ConfigureAwaitRecoveryDeviceState()
		{
			this.awaitRecoveryDeviceState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.awaitRecoveryDeviceState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.awaitRecoveryDeviceState.AddConditionalTransition(this.previousTransition);
			this.awaitRecoveryDeviceState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.awaitRecoveryDeviceState.AddConditionalTransition(this.checkLatestPackageTransition);
			this.awaitRecoveryDeviceState.AddConditionalTransition(this.errorTransition);
			this.awaitRecoveryDeviceState.AddConditionalTransition(this.settingsTransition);
			this.awaitRecoveryDeviceState.AddConditionalTransition(this.manualDeviceTypeSelectionTransition);
			this.awaitRecoveryDeviceState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00025258 File Offset: 0x00023458
		private void ConfigureDeviceDetectionState()
		{
			this.deviceSelectionState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.deviceSelectionState.AddConditionalTransition(this.checkLatestPackageTransition);
			this.deviceSelectionState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.deviceSelectionState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.deviceSelectionState.AddConditionalTransition(this.errorTransition);
			this.deviceSelectionState.AddConditionalTransition(this.settingsTransition);
			this.deviceSelectionState.AddConditionalTransition(this.readingDeviceInfoTransition);
			this.deviceSelectionState.AddConditionalTransition(this.awaitHtcTransition);
			this.deviceSelectionState.AddConditionalTransition(this.awaitGenericDeviceTransition);
			this.deviceSelectionState.AddConditionalTransition(this.manualPackageSelectionTransition);
			this.deviceSelectionState.AddConditionalTransition(this.helpTransition);
			this.deviceSelectionState.AddConditionalTransition(this.manualGenericVariantSelectionTransition);
			this.deviceSelectionState.AddConditionalTransition(this.awaitFawkesDeviceTransition);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00025364 File Offset: 0x00023564
		private void ConfigureBatteryCheckingState()
		{
			this.batteryCheckingState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.batteryCheckingState.AddConditionalTransition(this.flashingStateTransition);
			this.batteryCheckingState.AddConditionalTransition(this.errorTransition);
			this.batteryCheckingState.AddConditionalTransition(this.settingsTransition);
			this.batteryCheckingState.AddConditionalTransition(this.summaryTransition);
			this.batteryCheckingState.AddConditionalTransition(this.helpTransition);
			this.batteryCheckingState.AddConditionalTransition(this.absoluteConfirmationTransition);
			this.batteryCheckingState.AddConditionalTransition(this.awaitGenericDeviceTransition);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00025414 File Offset: 0x00023614
		private void ConfigureSummaryState()
		{
			this.summaryState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.summaryState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.summaryState.AddConditionalTransition(this.settingsTransition);
			this.summaryState.AddConditionalTransition(this.rebootHtcTransition);
			this.summaryState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00025490 File Offset: 0x00023690
		private void ConfigureReadingDeviceInfoState()
		{
			this.readingDeviceInfoState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.readingDeviceInfoState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.readingDeviceInfoState.AddConditionalTransition(this.previousTransition);
			this.readingDeviceInfoState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.readingDeviceInfoState.AddConditionalTransition(this.errorTransition);
			this.readingDeviceInfoState.AddConditionalTransition(this.deviceSelectionTransition);
			this.readingDeviceInfoState.AddConditionalTransition(this.readingDeviceInfoWithThorTransition);
			this.readingDeviceInfoState.AddConditionalTransition(this.checkLatestPackageTransition);
			this.readingDeviceInfoState.AddConditionalTransition(this.manualHtcRestartTransition);
			this.readingDeviceInfoState.AddConditionalTransition(this.rebootHtcTransition);
			this.readingDeviceInfoState.AddConditionalTransition(this.unsupportedDeviceTransition);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00025578 File Offset: 0x00023778
		private void ConfigureReadingDeviceInfoWithThorState()
		{
			this.readingDeviceInfoWithThorState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.readingDeviceInfoWithThorState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.readingDeviceInfoWithThorState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.readingDeviceInfoWithThorState.AddConditionalTransition(this.errorTransition);
			this.readingDeviceInfoWithThorState.AddConditionalTransition(this.deviceSelectionTransition);
			this.readingDeviceInfoWithThorState.AddConditionalTransition(this.checkLatestPackageTransition);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00025608 File Offset: 0x00023808
		private void ConfigureDisclaimerState()
		{
			this.disclaimerState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.disclaimerState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.disclaimerState.AddConditionalTransition(this.previousTransition);
			this.disclaimerState.AddConditionalTransition(this.errorTransition);
			this.disclaimerState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.disclaimerState.AddConditionalTransition(this.downloadPackageTransition);
			this.disclaimerState.AddConditionalTransition(this.packageIntegrityCheckTransition);
			this.disclaimerState.AddConditionalTransition(this.surveyTransition);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x000256BC File Offset: 0x000238BC
		private void ConfigureManualHtcRestartState()
		{
			this.manualHtcRestartState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.manualHtcRestartState.AddConditionalTransition(this.previousTransition);
			this.manualHtcRestartState.AddConditionalTransition(this.awaitHtcTransition);
			this.manualHtcRestartState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002571C File Offset: 0x0002391C
		private void ConfigureAbsoluteConfirmationState()
		{
			this.absoluteConfirmationState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.absoluteConfirmationState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.absoluteConfirmationState.AddConditionalTransition(this.errorTransition);
			this.absoluteConfirmationState.AddConditionalTransition(this.flashingStateTransition);
			this.absoluteConfirmationState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.absoluteConfirmationState.AddConditionalTransition(this.batteryCheckingTransition);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000257AC File Offset: 0x000239AC
		private void ConfigureManualGenericModelSelectionState()
		{
			this.manualGenericModelSelectionState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.manualGenericModelSelectionState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.manualGenericModelSelectionState.AddConditionalTransition(this.previousTransition);
			this.manualGenericModelSelectionState.AddConditionalTransition(this.errorTransition);
			this.manualGenericModelSelectionState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.manualGenericModelSelectionState.AddConditionalTransition(this.manualGenericVariantSelectionTransition);
			this.manualGenericModelSelectionState.AddConditionalTransition(this.checkLatestPackageTransition);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002584C File Offset: 0x00023A4C
		private void ConfigureManualGenericVariantSelectionState()
		{
			this.manualGenericVariantSelectionState.AddConditionalTransition(this.checkLatestPackageTransition);
			this.manualGenericVariantSelectionState.AddConditionalTransition(this.previousTransition);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00025874 File Offset: 0x00023A74
		private void ConfigureAwaitHoloLensAccessoryDeviceState()
		{
			this.awaitFawkesDeviceState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.awaitFawkesDeviceState.AddConditionalTransition(this.previousTransition);
			this.awaitFawkesDeviceState.AddConditionalTransition(this.readingDeviceInfoTransition);
			this.awaitFawkesDeviceState.AddConditionalTransition(this.checkLatestPackageTransition);
			this.awaitFawkesDeviceState.AddConditionalTransition(this.settingsTransition);
			this.awaitFawkesDeviceState.AddConditionalTransition(this.manualManufacturerSelectionTransition);
			this.awaitFawkesDeviceState.AddConditionalTransition(this.errorTransition);
			this.awaitFawkesDeviceState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
			this.awaitFawkesDeviceState.AddConditionalTransition(this.helpTransition);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00025938 File Offset: 0x00023B38
		private void ConfigureUnsupportedDeviceState()
		{
			this.unsupportedDeviceState.ShowRegions(new string[]
			{
				"MainArea",
				"SettingsArea"
			});
			this.unsupportedDeviceState.AddConditionalTransition(this.automaticManufacturerSelectionTransition);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002597C File Offset: 0x00023B7C
		private void ConfigureSurveyState()
		{
			this.surveyState.ShowRegions(new string[]
			{
				"MainArea"
			});
			this.surveyState.HideRegions(new string[]
			{
				"SettingsArea"
			});
			this.surveyState.AddConditionalTransition(this.previousTransition);
			this.surveyState.AddConditionalTransition(this.errorTransition);
			this.surveyState.AddConditionalTransition(this.downloadPackageTransition);
			this.surveyState.AddConditionalTransition(this.packageIntegrityCheckTransition);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00025A0C File Offset: 0x00023C0C
		protected T GetMachineState<T>() where T : BaseStateMachineState
		{
			T result = base.Container.Get<T>();
			result.Container = base.Container;
			result.InitializeStateMachine();
			return result;
		}

		// Token: 0x040002E2 RID: 738
		[Import]
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040002E3 RID: 739
		[Import]
		private Conditions conditions;

		// Token: 0x040002E4 RID: 740
		private UiBaseState appAutoUpdateState;

		// Token: 0x040002E5 RID: 741
		private UiBaseState checkAppAutoUpdateState;

		// Token: 0x040002E6 RID: 742
		private UiBaseState manualPackageSelectionState;

		// Token: 0x040002E7 RID: 743
		private UiBaseState manualManufacturerSelectionState;

		// Token: 0x040002E8 RID: 744
		private UiBaseState automaticManufacturerSelectionState;

		// Token: 0x040002E9 RID: 745
		private UiBaseState automaticPackageSelectionState;

		// Token: 0x040002EA RID: 746
		private UiBaseState checkLatestPackageState;

		// Token: 0x040002EB RID: 747
		private UiBaseState downloadPackageState;

		// Token: 0x040002EC RID: 748
		private UiBaseState flashingState;

		// Token: 0x040002ED RID: 749
		private UiBaseState awaitGenericDeviceState;

		// Token: 0x040002EE RID: 750
		private UiBaseState awaitHtcState;

		// Token: 0x040002EF RID: 751
		private UiBaseState awaitAnalogDeviceState;

		// Token: 0x040002F0 RID: 752
		private UiBaseState awaitRecoveryDeviceState;

		// Token: 0x040002F1 RID: 753
		private UiBaseState summaryState;

		// Token: 0x040002F2 RID: 754
		private UiBaseState packageIntegrityCheckState;

		// Token: 0x040002F3 RID: 755
		private UiBaseState errorState;

		// Token: 0x040002F4 RID: 756
		private UiBaseState deviceSelectionState;

		// Token: 0x040002F5 RID: 757
		private UiBaseState readingDeviceInfoState;

		// Token: 0x040002F6 RID: 758
		private UiBaseState readingDeviceInfoWithThorState;

		// Token: 0x040002F7 RID: 759
		private UiBaseState batteryCheckingState;

		// Token: 0x040002F8 RID: 760
		private UiBaseState disclaimerState;

		// Token: 0x040002F9 RID: 761
		private UiBaseState manualHtcRestartState;

		// Token: 0x040002FA RID: 762
		private UiBaseState rebootHtcState;

		// Token: 0x040002FB RID: 763
		private UiBaseState manualDeviceTypeSelectionState;

		// Token: 0x040002FC RID: 764
		private UiBaseState downloadEmergencyPackageState;

		// Token: 0x040002FD RID: 765
		private UiBaseState awaitRecoveryModeAfterEmergencyFlashingState;

		// Token: 0x040002FE RID: 766
		private UiBaseState absoluteConfirmationState;

		// Token: 0x040002FF RID: 767
		private UiBaseState manualGenericModelSelectionState;

		// Token: 0x04000300 RID: 768
		private UiBaseState manualGenericVariantSelectionState;

		// Token: 0x04000301 RID: 769
		private UiBaseState unsupportedDeviceState;

		// Token: 0x04000302 RID: 770
		private UiBaseState surveyState;

		// Token: 0x04000303 RID: 771
		private UiBaseState awaitFawkesDeviceState;

		// Token: 0x04000304 RID: 772
		private HelpState helpState;

		// Token: 0x04000305 RID: 773
		private SettingsState settingsState;

		// Token: 0x04000306 RID: 774
		private BaseTransition appAutoUpdateTransition;

		// Token: 0x04000307 RID: 775
		private BaseTransition checkAppAutoUpdateTransition;

		// Token: 0x04000308 RID: 776
		private BaseTransition manualPackageSelectionTransition;

		// Token: 0x04000309 RID: 777
		private BaseTransition automaticPackageSelectionTransition;

		// Token: 0x0400030A RID: 778
		private BaseTransition checkLatestPackageTransition;

		// Token: 0x0400030B RID: 779
		private BaseTransition downloadPackageTransition;

		// Token: 0x0400030C RID: 780
		private BaseTransition manualManufacturerSelectionTransition;

		// Token: 0x0400030D RID: 781
		private BaseTransition automaticManufacturerSelectionTransition;

		// Token: 0x0400030E RID: 782
		private BaseTransition flashingStateTransition;

		// Token: 0x0400030F RID: 783
		private BaseTransition awaitGenericDeviceTransition;

		// Token: 0x04000310 RID: 784
		private BaseTransition awaitHtcTransition;

		// Token: 0x04000311 RID: 785
		private BaseTransition awaitAnalogDeviceTransition;

		// Token: 0x04000312 RID: 786
		private BaseTransition awaitRecoveryDeviceTransition;

		// Token: 0x04000313 RID: 787
		private BaseTransition summaryTransition;

		// Token: 0x04000314 RID: 788
		private BaseTransition packageIntegrityCheckTransition;

		// Token: 0x04000315 RID: 789
		private BaseTransition errorTransition;

		// Token: 0x04000316 RID: 790
		private BaseTransition settingsTransition;

		// Token: 0x04000317 RID: 791
		private BaseTransition deviceSelectionTransition;

		// Token: 0x04000318 RID: 792
		private BaseTransition readingDeviceInfoTransition;

		// Token: 0x04000319 RID: 793
		private BaseTransition readingDeviceInfoWithThorTransition;

		// Token: 0x0400031A RID: 794
		private BaseTransition batteryCheckingTransition;

		// Token: 0x0400031B RID: 795
		private BaseTransition disclaimerTransition;

		// Token: 0x0400031C RID: 796
		private BaseTransition manualHtcRestartTransition;

		// Token: 0x0400031D RID: 797
		private BaseTransition rebootHtcTransition;

		// Token: 0x0400031E RID: 798
		private BaseTransition rebootHtcAfterErrorTransition;

		// Token: 0x0400031F RID: 799
		private BaseTransition manufacturerSelectionTransitionAfterError;

		// Token: 0x04000320 RID: 800
		private BaseTransition manualDeviceTypeSelectionTransition;

		// Token: 0x04000321 RID: 801
		private BaseTransition downloadEmergencyPackageTransition;

		// Token: 0x04000322 RID: 802
		private BaseTransition awaitRecoveryModeAfterEmergencyFlashingTransition;

		// Token: 0x04000323 RID: 803
		private BaseTransition absoluteConfirmationTransition;

		// Token: 0x04000324 RID: 804
		private BaseTransition helpTransition;

		// Token: 0x04000325 RID: 805
		private BaseTransition manualGenericModelSelectionTransition;

		// Token: 0x04000326 RID: 806
		private BaseTransition manualGenericVariantSelectionTransition;

		// Token: 0x04000327 RID: 807
		private BaseTransition unsupportedDeviceTransition;

		// Token: 0x04000328 RID: 808
		private BaseTransition surveyTransition;

		// Token: 0x04000329 RID: 809
		private PreviousStateTransition previousTransition;

		// Token: 0x0400032A RID: 810
		private BaseTransition awaitFawkesDeviceTransition;
	}
}
