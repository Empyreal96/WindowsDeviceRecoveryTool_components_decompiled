using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x02000096 RID: 150
	[Export]
	public class AppUpdateCheckingViewModel : BaseViewModel, ICanHandle<ApplicationUpdateMessage>, ICanHandle
	{
		// Token: 0x0600041D RID: 1053 RVA: 0x00013D8F File Offset: 0x00011F8F
		[ImportingConstructor]
		public AppUpdateCheckingViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00013DA4 File Offset: 0x00011FA4
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00013DBC File Offset: 0x00011FBC
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00013E0C File Offset: 0x0001200C
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x00013E24 File Offset: 0x00012024
		public bool IsChecking
		{
			get
			{
				return this.isChecking;
			}
			set
			{
				base.SetValue<bool>(() => this.IsChecking, ref this.isChecking, value);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00013E74 File Offset: 0x00012074
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x00013E8C File Offset: 0x0001208C
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				base.SetValue<string>(() => this.Version, ref this.version, value);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00013EDC File Offset: 0x000120DC
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x00013EF4 File Offset: 0x000120F4
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				base.SetValue<string>(() => this.Description, ref this.description, value);
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00013F44 File Offset: 0x00012144
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("ApplicationUpdate"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			this.IsChecking = true;
			base.Commands.Run((AppController c) => c.CheckForAppUpdate(null, CancellationToken.None));
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001401F File Offset: 0x0001221F
		public void Handle(ApplicationUpdateMessage message)
		{
			this.Description = this.ReplaceTagByNewLine(message.Update.Description);
			this.Version = message.Update.Version;
			this.IsChecking = false;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00014054 File Offset: 0x00012254
		private string ReplaceTagByNewLine(string text)
		{
			return (text != null) ? Regex.Replace(text, "<br>", "\n", RegexOptions.IgnoreCase) : string.Empty;
		}

		// Token: 0x040001D5 RID: 469
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040001D6 RID: 470
		private string version;

		// Token: 0x040001D7 RID: 471
		private string description;

		// Token: 0x040001D8 RID: 472
		private bool isChecking;
	}
}
