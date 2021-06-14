using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions.HTC;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Error
{
	// Token: 0x020000BF RID: 191
	[Export]
	public class ErrorViewModel : BaseViewModel, ICanHandle<ErrorMessage>, ICanHandle
	{
		// Token: 0x060005BA RID: 1466 RVA: 0x0001DAE4 File Offset: 0x0001BCE4
		[ImportingConstructor]
		public ErrorViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x0001DAF8 File Offset: 0x0001BCF8
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x0001DB10 File Offset: 0x0001BD10
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
			set
			{
				base.SetValue<Exception>(() => this.Exception, ref this.exception, value);
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x0001DB60 File Offset: 0x0001BD60
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x0001DB78 File Offset: 0x0001BD78
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

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0001DBC8 File Offset: 0x0001BDC8
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0001DBE0 File Offset: 0x0001BDE0
		public bool ExpanderExpanded
		{
			get
			{
				return this.expanderExpanded;
			}
			set
			{
				base.SetValue<bool>(() => this.ExpanderExpanded, ref this.expanderExpanded, value);
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0001DC30 File Offset: 0x0001BE30
		public string ErrorHeader
		{
			get
			{
				AutoUpdateNotEnoughSpaceException ex = this.Exception as AutoUpdateNotEnoughSpaceException;
				string translation;
				if (ex != null)
				{
					translation = LocalizationManager.GetTranslation("Error_NotEnoughSpaceException");
				}
				else
				{
					translation = LocalizationManager.GetTranslation("Error_" + this.Exception.GetType().Name);
				}
				return translation;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0001DC84 File Offset: 0x0001BE84
		public string ErrorDescription
		{
			get
			{
				NotEnoughSpaceException ex = this.Exception as NotEnoughSpaceException;
				string result;
				if (ex != null && ex.Needed == 0L)
				{
					result = string.Empty;
				}
				else
				{
					string customErrorMessage = this.GetCustomErrorMessage();
					if (!string.IsNullOrWhiteSpace(customErrorMessage))
					{
						result = customErrorMessage;
					}
					else
					{
						DownloadPackageException ex2 = this.Exception as DownloadPackageException;
						if (ex2 != null)
						{
							string translation = LocalizationManager.GetTranslation("ErrorDescription_" + ex2.GetType().Name);
							result = string.Format(translation, LocalizationManager.GetTranslation("ButtonTryAgain"), LocalizationManager.GetTranslation("ButtonExit"));
						}
						else
						{
							result = LocalizationManager.GetTranslation("ErrorDescription_" + this.Exception.GetType().Name);
						}
					}
				}
				return result;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001DD54 File Offset: 0x0001BF54
		public string ErrorDetails
		{
			get
			{
				string result;
				if (this.Exception != null)
				{
					result = this.Exception.Message;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001DD84 File Offset: 0x0001BF84
		public bool ErrorDetailsVisibile
		{
			get
			{
				return this.Exception != null && !string.IsNullOrWhiteSpace(this.Exception.Message) && !(this.Exception is UnauthorizedAccessException) && !(this.Exception is PlannedServiceBreakException) && !(this.Exception is RestartApplicationException);
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001DDE0 File Offset: 0x0001BFE0
		public void Handle(ErrorMessage message)
		{
			this.Exception = message.Exception;
			base.RaisePropertyChanged<string>(() => this.ErrorHeader);
			base.RaisePropertyChanged<string>(() => this.ErrorDescription);
			base.RaisePropertyChanged<string>(() => this.ErrorDetails);
			base.RaisePropertyChanged<bool>(() => this.ErrorDetailsVisibile);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001DEE8 File Offset: 0x0001C0E8
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Error"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			if (this.Exception != null)
			{
				Tracer<ErrorViewModel>.WriteError(this.Exception);
			}
			this.ExpanderExpanded = false;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001DF68 File Offset: 0x0001C168
		private string GetCustomErrorMessage()
		{
			NotEnoughSpaceException ex = this.Exception as NotEnoughSpaceException;
			string result;
			if (ex != null)
			{
				string translation = LocalizationManager.GetTranslation("ErrorDescription_" + this.Exception.GetType().Name);
				long num = (ex.Needed - ex.Available) / 1024L / 1024L;
				result = string.Format(translation, num + "MB", ex.Disk);
			}
			else
			{
				AutoUpdateNotEnoughSpaceException ex2 = this.Exception as AutoUpdateNotEnoughSpaceException;
				if (ex2 != null)
				{
					string translation = LocalizationManager.GetTranslation("ErrorDescription_NotEnoughSpaceException");
					long num = (ex2.Needed - ex2.Available) / 1024L / 1024L;
					result = string.Format(translation, num + "MB", ex2.Disk);
				}
				else
				{
					UnauthorizedAccessException ex3 = this.Exception as UnauthorizedAccessException;
					if (ex3 != null)
					{
						result = ex3.Message;
					}
					else
					{
						PlannedServiceBreakException ex4 = this.Exception as PlannedServiceBreakException;
						if (ex4 != null)
						{
							string translation = LocalizationManager.GetTranslation("ErrorDescription_" + ex4.GetType().Name);
							result = string.Format(translation, ex4.Start, ex4.End);
						}
						else if (this.Exception is HtcDeviceHandshakingException || this.Exception is HtcDeviceCommunicationException)
						{
							string translation = LocalizationManager.GetTranslation("ErrorDescription_HtcDeviceCommunicationException");
							result = string.Format(translation, "boot-loader");
						}
						else
						{
							HtcUsbCommunicationException ex5 = this.Exception as HtcUsbCommunicationException;
							if (ex5 != null)
							{
								string translation = LocalizationManager.GetTranslation("ErrorDescription_" + ex5.GetType().Name);
								result = string.Format(translation, "boot-loader");
							}
							else
							{
								HtcServiceControlException ex6 = this.Exception as HtcServiceControlException;
								if (ex6 != null)
								{
									string translation = LocalizationManager.GetTranslation("ErrorDescription_" + ex6.GetType().Name);
									result = string.Format(translation, "WcesComm");
								}
								else
								{
									RestartApplicationException ex7 = this.Exception as RestartApplicationException;
									if (ex7 != null)
									{
										string translation = LocalizationManager.GetTranslation("ErrorDescription_" + ex7.GetType().Name);
										result = string.Format(translation, AppInfo.AppTitle());
									}
									else
									{
										result = null;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04000271 RID: 625
		private Exception exception;

		// Token: 0x04000272 RID: 626
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x04000273 RID: 627
		private bool expanderExpanded;
	}
}
