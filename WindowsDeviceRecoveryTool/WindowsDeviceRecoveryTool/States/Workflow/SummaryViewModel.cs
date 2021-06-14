using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000D3 RID: 211
	[Export]
	public class SummaryViewModel : BaseViewModel, ICanHandle<FlashResultMessage>, ICanHandle
	{
		// Token: 0x06000678 RID: 1656 RVA: 0x0002179F File Offset: 0x0001F99F
		[ImportingConstructor]
		public SummaryViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x000217B4 File Offset: 0x0001F9B4
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x000217CC File Offset: 0x0001F9CC
		public Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext AppContext
		{
			get
			{
				return this.appContext;
			}
			set
			{
				base.SetValue<Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext>(() => this.AppContext, ref this.appContext, value);
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0002181C File Offset: 0x0001FA1C
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x00021834 File Offset: 0x0001FA34
		public bool IsPassed
		{
			get
			{
				return this.isPassed;
			}
			set
			{
				base.SetValue<bool>(() => this.IsPassed, ref this.isPassed, value);
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00021884 File Offset: 0x0001FA84
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x0002189C File Offset: 0x0001FA9C
		public string FinishText
		{
			get
			{
				return this.finishText;
			}
			set
			{
				base.SetValue<string>(() => this.FinishText, ref this.finishText, value);
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x000218EC File Offset: 0x0001FAEC
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00021904 File Offset: 0x0001FB04
		public string ResultMessage
		{
			get
			{
				return this.resultMessage;
			}
			set
			{
				base.SetValue<string>(() => this.ResultMessage, ref this.resultMessage, value);
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00021954 File Offset: 0x0001FB54
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x0002196C File Offset: 0x0001FB6C
		public string InstructionMessage
		{
			get
			{
				return this.instructionMessage;
			}
			set
			{
				base.SetValue<string>(() => this.InstructionMessage, ref this.instructionMessage, value);
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x000219BC File Offset: 0x0001FBBC
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x000219D4 File Offset: 0x0001FBD4
		private FlashResultMessage Message
		{
			get
			{
				return this.message;
			}
			set
			{
				base.SetValue<FlashResultMessage>(() => this.Message, ref this.message, value);
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00021A24 File Offset: 0x0001FC24
		public void Handle(FlashResultMessage flashResultMessage)
		{
			this.Message = flashResultMessage;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00021A30 File Offset: 0x0001FC30
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Summary"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			this.RefreshResults();
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00021A94 File Offset: 0x0001FC94
		private void RefreshResults()
		{
			this.IsPassed = this.Message.Result;
			this.ResultMessage = LocalizationManager.GetTranslation(this.Message.Result ? "OperationPassed" : "OperationFailed");
			this.InstructionMessage = this.ConstructExtendedMessage(this.Message.ExtendedMessage);
			this.FinishText = ((!this.IsPassed && this.appContext.SelectedManufacturer == PhoneTypes.Htc) ? LocalizationManager.GetTranslation("ButtonContinue") : LocalizationManager.GetTranslation("ButtonFinish"));
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00021B28 File Offset: 0x0001FD28
		private string ConstructExtendedMessage(List<string> extendedMessage)
		{
			string result = string.Empty;
			if (extendedMessage == null || extendedMessage.Count == 0)
			{
				result = string.Empty;
			}
			else if (extendedMessage.Count == 1)
			{
				if (string.IsNullOrEmpty(this.Message.Argument))
				{
					result = string.Format(LocalizationManager.GetTranslation(extendedMessage[0]), new object[0]);
				}
				else
				{
					result = string.Format(LocalizationManager.GetTranslation(extendedMessage[0]), this.Message.Argument);
				}
			}
			else if (extendedMessage.Count == 2)
			{
				result = string.Format(LocalizationManager.GetTranslation(extendedMessage[0]), LocalizationManager.GetTranslation(extendedMessage[1]));
			}
			return result;
		}

		// Token: 0x040002B5 RID: 693
		private string resultMessage;

		// Token: 0x040002B6 RID: 694
		private string instructionMessage;

		// Token: 0x040002B7 RID: 695
		private bool isPassed;

		// Token: 0x040002B8 RID: 696
		private string finishText;

		// Token: 0x040002B9 RID: 697
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040002BA RID: 698
		private FlashResultMessage message;
	}
}
