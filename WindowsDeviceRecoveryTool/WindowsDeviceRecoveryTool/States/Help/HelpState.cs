using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.Transitions;
using Microsoft.WindowsDeviceRecoveryTool.States.BaseStates;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x02000078 RID: 120
	[Export]
	public class HelpState : UiStateMachineState<MainHelpView, MainHelpViewModel>
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00010D00 File Offset: 0x0000EF00
		public string CurrentStateName
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00010D18 File Offset: 0x0000EF18
		public override void InitializeStateMachine()
		{
			base.SetViewViewModel(base.Container.Get<MainHelpView>(), base.Container.Get<MainHelpViewModel>());
			base.InitializeStateMachine();
			base.Machine.AddState(this.lumiaChooseHelpState);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00010D51 File Offset: 0x0000EF51
		protected override void ConfigureStates()
		{
			this.ConfigureLumiaChooseHelpState();
			this.ConfigureLumiaEmergencyHelpState();
			this.ConfigureLumiaFlashingHelpState();
			this.ConfigureLumiaNormalHelpState();
			this.ConfigureHtcChooseHelpState();
			this.ConfigureHtcBootloaderHelpState();
			this.ConfigureHtcNormalHelpState();
			this.ConfigureLumiaOldFlashingHelpState();
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00010D8C File Offset: 0x0000EF8C
		private void ConfigureHtcNormalHelpState()
		{
			this.htcNormalHelpState.AddConditionalTransition(this.lumiaChooseHelpTransition);
			this.htcNormalHelpState.AddConditionalTransition(this.lumiaEmergencyHelpTransition);
			this.htcNormalHelpState.AddConditionalTransition(this.lumiaFlashingHelpTransition);
			this.htcNormalHelpState.AddConditionalTransition(this.lumiaNormalHelpTransition);
			this.htcNormalHelpState.AddConditionalTransition(this.htcChooseHelpTransition);
			this.htcNormalHelpState.AddConditionalTransition(this.htcBootloaderHelpTransition);
			this.htcNormalHelpState.AddConditionalTransition(this.lumiaOldFlashingHelpTransition);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00010E18 File Offset: 0x0000F018
		private void ConfigureHtcBootloaderHelpState()
		{
			this.htcBootloaderHelpState.AddConditionalTransition(this.lumiaChooseHelpTransition);
			this.htcBootloaderHelpState.AddConditionalTransition(this.lumiaEmergencyHelpTransition);
			this.htcBootloaderHelpState.AddConditionalTransition(this.lumiaFlashingHelpTransition);
			this.htcBootloaderHelpState.AddConditionalTransition(this.lumiaNormalHelpTransition);
			this.htcBootloaderHelpState.AddConditionalTransition(this.htcChooseHelpTransition);
			this.htcBootloaderHelpState.AddConditionalTransition(this.htcNormalHelpTransition);
			this.htcBootloaderHelpState.AddConditionalTransition(this.lumiaOldFlashingHelpTransition);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00010EA4 File Offset: 0x0000F0A4
		private void ConfigureHtcChooseHelpState()
		{
			this.htcChooseHelpState.AddConditionalTransition(this.lumiaChooseHelpTransition);
			this.htcChooseHelpState.AddConditionalTransition(this.lumiaEmergencyHelpTransition);
			this.htcChooseHelpState.AddConditionalTransition(this.lumiaFlashingHelpTransition);
			this.htcChooseHelpState.AddConditionalTransition(this.lumiaNormalHelpTransition);
			this.htcChooseHelpState.AddConditionalTransition(this.htcBootloaderHelpTransition);
			this.htcChooseHelpState.AddConditionalTransition(this.htcNormalHelpTransition);
			this.htcChooseHelpState.AddConditionalTransition(this.lumiaOldFlashingHelpTransition);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00010F30 File Offset: 0x0000F130
		private void ConfigureLumiaNormalHelpState()
		{
			this.lumiaNormalHelpState.AddConditionalTransition(this.lumiaChooseHelpTransition);
			this.lumiaNormalHelpState.AddConditionalTransition(this.lumiaEmergencyHelpTransition);
			this.lumiaNormalHelpState.AddConditionalTransition(this.lumiaFlashingHelpTransition);
			this.lumiaNormalHelpState.AddConditionalTransition(this.htcChooseHelpTransition);
			this.lumiaNormalHelpState.AddConditionalTransition(this.htcBootloaderHelpTransition);
			this.lumiaNormalHelpState.AddConditionalTransition(this.htcNormalHelpTransition);
			this.lumiaNormalHelpState.AddConditionalTransition(this.lumiaOldFlashingHelpTransition);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00010FBC File Offset: 0x0000F1BC
		private void ConfigureLumiaFlashingHelpState()
		{
			this.lumiaFlashingHelpState.AddConditionalTransition(this.lumiaChooseHelpTransition);
			this.lumiaFlashingHelpState.AddConditionalTransition(this.lumiaEmergencyHelpTransition);
			this.lumiaFlashingHelpState.AddConditionalTransition(this.lumiaNormalHelpTransition);
			this.lumiaFlashingHelpState.AddConditionalTransition(this.htcChooseHelpTransition);
			this.lumiaFlashingHelpState.AddConditionalTransition(this.htcBootloaderHelpTransition);
			this.lumiaFlashingHelpState.AddConditionalTransition(this.htcNormalHelpTransition);
			this.lumiaFlashingHelpState.AddConditionalTransition(this.lumiaOldFlashingHelpTransition);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00011048 File Offset: 0x0000F248
		private void ConfigureLumiaEmergencyHelpState()
		{
			this.lumiaEmergencyHelpState.AddConditionalTransition(this.lumiaChooseHelpTransition);
			this.lumiaEmergencyHelpState.AddConditionalTransition(this.lumiaFlashingHelpTransition);
			this.lumiaEmergencyHelpState.AddConditionalTransition(this.lumiaNormalHelpTransition);
			this.lumiaEmergencyHelpState.AddConditionalTransition(this.htcChooseHelpTransition);
			this.lumiaEmergencyHelpState.AddConditionalTransition(this.htcBootloaderHelpTransition);
			this.lumiaEmergencyHelpState.AddConditionalTransition(this.htcNormalHelpTransition);
			this.lumiaEmergencyHelpState.AddConditionalTransition(this.lumiaOldFlashingHelpTransition);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000110D4 File Offset: 0x0000F2D4
		private void ConfigureLumiaChooseHelpState()
		{
			this.lumiaChooseHelpState.AddConditionalTransition(this.lumiaEmergencyHelpTransition);
			this.lumiaChooseHelpState.AddConditionalTransition(this.lumiaFlashingHelpTransition);
			this.lumiaChooseHelpState.AddConditionalTransition(this.lumiaNormalHelpTransition);
			this.lumiaChooseHelpState.AddConditionalTransition(this.htcChooseHelpTransition);
			this.lumiaChooseHelpState.AddConditionalTransition(this.htcBootloaderHelpTransition);
			this.lumiaChooseHelpState.AddConditionalTransition(this.htcNormalHelpTransition);
			this.lumiaChooseHelpState.AddConditionalTransition(this.lumiaOldFlashingHelpTransition);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00011160 File Offset: 0x0000F360
		private void ConfigureLumiaOldFlashingHelpState()
		{
			this.lumiaOldFlashingHelpState.AddConditionalTransition(this.lumiaChooseHelpTransition);
			this.lumiaOldFlashingHelpState.AddConditionalTransition(this.lumiaEmergencyHelpTransition);
			this.lumiaOldFlashingHelpState.AddConditionalTransition(this.lumiaFlashingHelpTransition);
			this.lumiaOldFlashingHelpState.AddConditionalTransition(this.lumiaNormalHelpTransition);
			this.lumiaOldFlashingHelpState.AddConditionalTransition(this.htcChooseHelpTransition);
			this.lumiaOldFlashingHelpState.AddConditionalTransition(this.htcBootloaderHelpTransition);
			this.lumiaOldFlashingHelpState.AddConditionalTransition(this.htcNormalHelpTransition);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000111EC File Offset: 0x0000F3EC
		protected override void InitializeStates()
		{
			this.lumiaChooseHelpState = base.GetUiState<LumiaChooseHelpView, LumiaChooseHelpViewModel>(null);
			this.lumiaEmergencyHelpState = base.GetUiState<LumiaEmergencyHelpView, LumiaEmergencyHelpViewModel>(null);
			this.lumiaFlashingHelpState = base.GetUiState<LumiaFlashingHelpView, LumiaFlashingHelpViewModel>(null);
			this.lumiaNormalHelpState = base.GetUiState<LumiaNormalHelpView, LumiaNormalHelpViewModel>(null);
			this.htcChooseHelpState = base.GetUiState<HtcChooseHelpView, HtcChooseHelpViewModel>(null);
			this.htcBootloaderHelpState = base.GetUiState<HtcBootloaderHelpView, HtcBootloaderHelpViewModel>(null);
			this.htcNormalHelpState = base.GetUiState<HtcNormalHelpView, HtcNormalHelpViewModel>(null);
			this.lumiaOldFlashingHelpState = base.GetUiState<LumiaOldFlashingHelpView, LumiaOldFlashingHelpViewModel>(null);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00011264 File Offset: 0x0000F464
		protected override void InitializeTransitions()
		{
			this.lumiaChooseHelpTransition = new StateStatusTransition(this.lumiaChooseHelpState, "LumiaChooseHelpState");
			this.lumiaEmergencyHelpTransition = new StateStatusTransition(this.lumiaEmergencyHelpState, "LumiaEmergencyHelpState");
			this.lumiaFlashingHelpTransition = new StateStatusTransition(this.lumiaFlashingHelpState, "LumiaFlashingHelpState");
			this.lumiaNormalHelpTransition = new StateStatusTransition(this.lumiaNormalHelpState, "LumiaNormalHelpState");
			this.htcChooseHelpTransition = new StateStatusTransition(this.htcChooseHelpState, "HtcChooseHelpState");
			this.htcBootloaderHelpTransition = new StateStatusTransition(this.htcBootloaderHelpState, "HtcBootloaderHelpState");
			this.htcNormalHelpTransition = new StateStatusTransition(this.htcNormalHelpState, "HtcNormalHelpState");
			this.lumiaOldFlashingHelpTransition = new StateStatusTransition(this.lumiaOldFlashingHelpState, "LumiaOldFlashingHelpState");
		}

		// Token: 0x04000188 RID: 392
		private UiBaseState lumiaChooseHelpState;

		// Token: 0x04000189 RID: 393
		private UiBaseState lumiaEmergencyHelpState;

		// Token: 0x0400018A RID: 394
		private UiBaseState lumiaFlashingHelpState;

		// Token: 0x0400018B RID: 395
		private UiBaseState lumiaNormalHelpState;

		// Token: 0x0400018C RID: 396
		private UiBaseState htcChooseHelpState;

		// Token: 0x0400018D RID: 397
		private UiBaseState htcBootloaderHelpState;

		// Token: 0x0400018E RID: 398
		private UiBaseState htcNormalHelpState;

		// Token: 0x0400018F RID: 399
		private UiBaseState lumiaOldFlashingHelpState;

		// Token: 0x04000190 RID: 400
		private StateStatusTransition lumiaChooseHelpTransition;

		// Token: 0x04000191 RID: 401
		private StateStatusTransition lumiaEmergencyHelpTransition;

		// Token: 0x04000192 RID: 402
		private StateStatusTransition lumiaFlashingHelpTransition;

		// Token: 0x04000193 RID: 403
		private StateStatusTransition lumiaNormalHelpTransition;

		// Token: 0x04000194 RID: 404
		private StateStatusTransition htcChooseHelpTransition;

		// Token: 0x04000195 RID: 405
		private StateStatusTransition htcBootloaderHelpTransition;

		// Token: 0x04000196 RID: 406
		private StateStatusTransition htcNormalHelpTransition;

		// Token: 0x04000197 RID: 407
		private StateStatusTransition lumiaOldFlashingHelpTransition;
	}
}
