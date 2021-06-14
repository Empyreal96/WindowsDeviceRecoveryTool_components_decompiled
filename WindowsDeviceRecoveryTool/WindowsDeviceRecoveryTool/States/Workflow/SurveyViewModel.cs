using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000E1 RID: 225
	[Export]
	public class SurveyViewModel : BaseViewModel
	{
		// Token: 0x06000731 RID: 1841 RVA: 0x00026328 File Offset: 0x00024528
		[ImportingConstructor]
		public SurveyViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
			this.SubmitAndContinueCommand = new DelegateCommand<object>(new Action<object>(this.SubmitSurvey), new Func<object, bool>(this.CanSubmit));
			this.ContinueNoSubmitCommand = new DelegateCommand<object>(new Action<object>(this.Continue));
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00026384 File Offset: 0x00024584
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0002639B File Offset: 0x0002459B
		public DelegateCommand<object> SubmitAndContinueCommand { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x000263A4 File Offset: 0x000245A4
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x000263BB File Offset: 0x000245BB
		public DelegateCommand<object> ContinueNoSubmitCommand { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x000263C4 File Offset: 0x000245C4
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x000263DC File Offset: 0x000245DC
		public bool Question1
		{
			get
			{
				return this.question1;
			}
			set
			{
				if (this.question1 != value)
				{
					this.question1 = value;
					base.RaisePropertyChanged<bool>(() => this.Question1);
					this.SubmitAndContinueCommand.RaiseCanExecuteChanged();
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x00026448 File Offset: 0x00024648
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x00026460 File Offset: 0x00024660
		public bool Question2
		{
			get
			{
				return this.question2;
			}
			set
			{
				if (this.question2 != value)
				{
					this.question2 = value;
					base.RaisePropertyChanged<bool>(() => this.Question2);
					this.SubmitAndContinueCommand.RaiseCanExecuteChanged();
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x000264CC File Offset: 0x000246CC
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x000264E4 File Offset: 0x000246E4
		public bool Question3
		{
			get
			{
				return this.question3;
			}
			set
			{
				if (this.question3 != value)
				{
					this.question3 = value;
					base.RaisePropertyChanged<bool>(() => this.Question3);
					this.SubmitAndContinueCommand.RaiseCanExecuteChanged();
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x00026550 File Offset: 0x00024750
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x00026568 File Offset: 0x00024768
		public bool Question4
		{
			get
			{
				return this.question4;
			}
			set
			{
				if (this.question4 != value)
				{
					this.question4 = value;
					base.RaisePropertyChanged<bool>(() => this.Question4);
					this.SubmitAndContinueCommand.RaiseCanExecuteChanged();
				}
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x000265D4 File Offset: 0x000247D4
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x000265EC File Offset: 0x000247EC
		public bool Question5
		{
			get
			{
				return this.question5;
			}
			set
			{
				if (this.question5 != value)
				{
					this.question5 = value;
					base.RaisePropertyChanged<bool>(() => this.Question5);
					this.SubmitAndContinueCommand.RaiseCanExecuteChanged();
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00026658 File Offset: 0x00024858
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x00026670 File Offset: 0x00024870
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				if (this.description != value)
				{
					this.description = value;
					base.RaisePropertyChanged<string>(() => this.Description);
					this.SubmitAndContinueCommand.RaiseCanExecuteChanged();
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x000266E4 File Offset: 0x000248E4
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x000266FC File Offset: 0x000248FC
		public bool InsiderProgramQuestion
		{
			get
			{
				return this.insiderProgramQuestion;
			}
			set
			{
				if (this.insiderProgramQuestion != value)
				{
					this.insiderProgramQuestion = value;
					base.RaisePropertyChanged<bool>(() => this.InsiderProgramQuestion);
					this.SubmitAndContinueCommand.RaiseCanExecuteChanged();
				}
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00026768 File Offset: 0x00024968
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Survey1"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			this.CleanSurvey();
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000267B8 File Offset: 0x000249B8
		private bool CanSubmit(object obj)
		{
			return this.Question1 || this.Question2 || this.Question3 || this.Question4 || this.Question5 || !string.IsNullOrEmpty(this.Description) || this.InsiderProgramQuestion;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00026814 File Offset: 0x00024A14
		private void SubmitSurvey(object obj)
		{
			SurveyReport survey = new SurveyReport
			{
				Question1 = this.Question1,
				Question2 = this.Question2,
				Question3 = this.Question3,
				Question4 = this.Question4,
				Question5 = this.Question5,
				Description = this.Description,
				InsiderProgramQuestion = this.InsiderProgramQuestion,
				ManufacturerHardwareVariant = this.appContext.CurrentPhone.HardwareVariant,
				ManufacturerHardwareModel = this.appContext.CurrentPhone.HardwareModel,
				Imei = this.appContext.CurrentPhone.Imei,
				ManufacturerName = this.appContext.CurrentPhone.ReportManufacturerName,
				ManufacturerProductLine = this.appContext.CurrentPhone.ReportManufacturerProductLine,
				PhoneType = this.appContext.CurrentPhone.Type
			};
			base.Commands.Run((FlowController c) => c.SurveyCompleted(survey, CancellationToken.None));
			this.Continue(obj);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000269A8 File Offset: 0x00024BA8
		private void Continue(object parameter)
		{
			if (this.appContext.CurrentPhone.PackageFileInfo.OfflinePackage)
			{
				base.Commands.Run((AppController c) => c.SwitchToState("PackageIntegrityCheckState"));
			}
			else
			{
				base.Commands.Run((AppController c) => c.SwitchToState("DownloadPackageState"));
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00026AA4 File Offset: 0x00024CA4
		private void CleanSurvey()
		{
			this.Question1 = (this.Question2 = (this.Question3 = (this.Question4 = (this.Question5 = false))));
			this.Description = string.Empty;
			this.InsiderProgramQuestion = false;
		}

		// Token: 0x0400033C RID: 828
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x0400033D RID: 829
		private bool question1;

		// Token: 0x0400033E RID: 830
		private bool question2;

		// Token: 0x0400033F RID: 831
		private bool question3;

		// Token: 0x04000340 RID: 832
		private bool question4;

		// Token: 0x04000341 RID: 833
		private bool question5;

		// Token: 0x04000342 RID: 834
		private string description;

		// Token: 0x04000343 RID: 835
		private bool insiderProgramQuestion;
	}
}
