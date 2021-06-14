using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.Controllers
{
	// Token: 0x02000034 RID: 52
	[Export("Microsoft.WindowsDeviceRecoveryTool.Controllers.LumiaController", typeof(IController))]
	public class LumiaController : BaseController
	{
		// Token: 0x060001DE RID: 478 RVA: 0x0000CE14 File Offset: 0x0000B014
		[ImportingConstructor]
		public LumiaController(ICommandRepository commandRepository, Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext, LogicContext logics, EventAggregator eventAggregator) : base(commandRepository, eventAggregator)
		{
			this.appContext = appContext;
			this.logics = logics;
			this.logics.LumiaAdaptation.DeviceDisconnected += this.LumiaServiceOnDeviceDisconnectedEvent;
			this.logics.LumiaAdaptation.DeviceConnected += this.LumiaServiceOnNewDeviceConnectedEvent;
			this.logics.LumiaAdaptation.DeviceReadyChanged += this.LumiaAdaptationDeviceReadyChanged;
			this.logics.AdaptationManager.ProgressChanged += this.OnCurrentOperationProgressChanged;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000CEC1 File Offset: 0x0000B0C1
		private void OnCurrentOperationProgressChanged(Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs.ProgressChangedEventArgs progressMessage)
		{
			base.EventAggregator.Publish<ProgressMessage>(new ProgressMessage(progressMessage.Percentage, progressMessage.Message, progressMessage.DownloadedSize, progressMessage.TotalSize, progressMessage.BytesPerSecond, progressMessage.SecondsLeft));
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000CEFC File Offset: 0x0000B0FC
		private void LumiaServiceOnNewDeviceConnectedEvent(Phone phone)
		{
			this.appContext.CurrentPhone = phone;
			if (this.currentDetectionType == DetectionType.RecoveryModeAfterEmergencyFlashing)
			{
				base.Commands.Run((FlowController c) => c.FinishAwaitRecoveryAfterEmergency(false, CancellationToken.None));
			}
			else if (this.currentDetectionType == DetectionType.RecoveryMode && this.appContext.CurrentPhone.IsDeviceInEmergencyMode())
			{
				base.EventAggregator.Publish<SwitchStateMessage>(new SwitchStateMessage("ManualDeviceTypeSelectionState"));
			}
			else
			{
				base.EventAggregator.Publish<DeviceConnectedMessage>(new DeviceConnectedMessage(phone));
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				lock (this.readDeviceInfoCtsList)
				{
					this.readDeviceInfoCtsList.Add(cancellationTokenSource);
				}
				if (phone.DeviceReady)
				{
					this.ReadDeviceInfo(cancellationTokenSource.Token);
				}
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000D070 File Offset: 0x0000B270
		private void TryFinishWaitingForDevice()
		{
			if (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.DeviceReady)
			{
				base.EventAggregator.Publish<SwitchStateMessage>(new SwitchStateMessage("CheckLatestPackageState"));
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
		private void LumiaServiceOnDeviceDisconnectedEvent(Phone phone)
		{
			if (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.PortId == phone.PortId)
			{
				base.EventAggregator.Publish<DeviceConnectedMessage>(new DeviceConnectedMessage(phone));
				this.appContext.CurrentPhone = null;
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000D124 File Offset: 0x0000B324
		private void LumiaAdaptationDeviceReadyChanged(Phone phone)
		{
			if (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.PortId == phone.PortId)
			{
				this.appContext.CurrentPhone = phone;
				if (phone.DeviceReady)
				{
					CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
					lock (this.readDeviceInfoCtsList)
					{
						this.readDeviceInfoCtsList.Add(cancellationTokenSource);
					}
					this.ReadDeviceInfo(cancellationTokenSource.Token);
					base.EventAggregator.Publish<DeviceConnectedMessage>(new DeviceConnectedMessage(phone));
				}
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000D1EC File Offset: 0x0000B3EC
		[CustomCommand(IsAsynchronous = true)]
		public void TryReadMissingInfoWithThor(object parameter, CancellationToken token)
		{
			Thor2Service thor2Service = this.logics.Thor2Service;
			int num = 3;
			Phone currentPhone = this.appContext.CurrentPhone;
			Exception ex = null;
			while (currentPhone.IsProductCodeTypeEmpty() && num-- > 0)
			{
				try
				{
					Tracer<LumiaController>.WriteInformation("Trying to read missing info with Thor2");
					thor2Service.TryReadMissingInfoWithThor(currentPhone, token, true);
					ex = null;
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
			}
			if (num < 3)
			{
				thor2Service.RestartToNormalMode(currentPhone, token);
			}
			if (ex != null)
			{
				ex = new ReadPhoneInformationException(ex.Message);
				this.logics.ReportingService.OperationFailed(currentPhone, ReportOperationType.ReadDeviceInfo, UriData.ProductCodeReadFailed, ex);
				throw ex;
			}
			if (currentPhone.IsProductCodeTypeEmpty())
			{
				ex = new ReadPhoneInformationException();
				this.logics.ReportingService.OperationFailed(currentPhone, ReportOperationType.ReadDeviceInfo, UriData.ProductCodeReadFailed, ex);
				throw ex;
			}
			base.EventAggregator.Publish<SwitchStateMessage>(new SwitchStateMessage("CheckLatestPackageState"));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
		[CustomCommand(IsAsynchronous = true)]
		public void StartLumiaDetection(DetectionType detectionType, CancellationToken token)
		{
			this.currentDetectionType = detectionType;
			this.logics.LumiaAdaptation.StartDetection(detectionType);
			this.FindCurrentlyConnectedPhone();
			if (this.appContext.CurrentPhone != null)
			{
				base.EventAggregator.Publish<DeviceConnectedMessage>(new DeviceConnectedMessage(this.appContext.CurrentPhone));
				if (detectionType != DetectionType.RecoveryModeAfterEmergencyFlashing && this.appContext.CurrentPhone.DeviceReady)
				{
					this.ReadDeviceInfo(token);
				}
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000D37C File Offset: 0x0000B57C
		[CustomCommand(IsAsynchronous = true)]
		public void StartCurrentLumiaDetection(DetectionType detectionType, CancellationToken token)
		{
			this.currentDetectionType = detectionType;
			this.logics.LumiaAdaptation.StartDetection(detectionType);
			if (this.selectedPhone == null)
			{
				throw new Exception("No phone from DeviceSelection state.");
			}
			this.FindCurrentPhone(this.selectedPhone);
			if (this.appContext.CurrentPhone != null)
			{
				base.EventAggregator.Publish<DeviceConnectedMessage>(new DeviceConnectedMessage(this.appContext.CurrentPhone));
				if (this.appContext.CurrentPhone.DeviceReady)
				{
					this.ReadDeviceInfo(token);
				}
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000D41D File Offset: 0x0000B61D
		[CustomCommand(IsAsynchronous = true)]
		public void SetSelectedPhone(Phone phone, CancellationToken token)
		{
			this.selectedPhone = phone;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000D428 File Offset: 0x0000B628
		private void ReadNormalModeDeviceInfo(CancellationToken token)
		{
			if (this.appContext.CurrentPhone == null)
			{
				Tracer<LumiaController>.WriteInformation("Current phone is null. Unable to read device information.");
				throw new DeviceNotFoundException();
			}
			try
			{
				this.logics.LumiaAdaptation.FillLumiaDeviceInfo(this.appContext.CurrentPhone, token);
			}
			catch (OperationCanceledException)
			{
				Tracer<LumiaController>.WriteInformation("Reading device info cancelled.");
			}
			catch (Win32Exception innerException)
			{
				throw new RestartApplicationException("Reading device info failed!", innerException);
			}
			catch (Exception ex)
			{
				Tracer<LumiaController>.WriteInformation(ex.Message);
				throw;
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000D4D8 File Offset: 0x0000B6D8
		private void ReadDeviceInfo(CancellationToken cancellationToken)
		{
			if (this.appContext.CurrentPhone == null || (!this.appContext.CurrentPhone.DeviceReady && this.currentDetectionType != DetectionType.RecoveryMode))
			{
				Tracer<LumiaController>.WriteInformation("Current phone is empty. Unable to read device information.");
				throw new DeviceNotFoundException();
			}
			switch (this.currentDetectionType)
			{
			case DetectionType.NormalMode:
				this.ReadNormalModeDeviceInfo(cancellationToken);
				break;
			case DetectionType.RecoveryMode:
				this.logics.LumiaAdaptation.StopDetection();
				this.logics.LumiaAdaptation.TryReadMissingInfoWithThor(this.appContext.CurrentPhone, cancellationToken);
				break;
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				if (this.appContext.CurrentPhone == null)
				{
					Tracer<LumiaController>.WriteInformation("Current phone is null. Unable to read device information.");
					throw new DeviceNotFoundException();
				}
				if (this.appContext.CurrentPhone.IsProductCodeTypeEmpty())
				{
					base.Commands.Run((AppController c) => c.SwitchToState("ReadingDeviceInfoWithThorState"));
				}
				else
				{
					this.TryFinishWaitingForDevice();
				}
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000D670 File Offset: 0x0000B870
		private void FindCurrentlyConnectedPhone()
		{
			ReadOnlyCollection<Phone> allPhones = this.logics.LumiaAdaptation.GetAllPhones();
			if (this.appContext.CurrentPhone == null)
			{
				this.appContext.CurrentPhone = allPhones.FirstOrDefault<Phone>();
			}
			else
			{
				Phone currentPhone = allPhones.FirstOrDefault((Phone phone) => phone.PortId == this.appContext.CurrentPhone.PortId);
				this.appContext.CurrentPhone = currentPhone;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000D718 File Offset: 0x0000B918
		private void FindCurrentPhone(Phone phone)
		{
			ReadOnlyCollection<Phone> allPhones = this.logics.LumiaAdaptation.GetAllPhones();
			try
			{
				Phone currentPhone = allPhones.First((Phone p) => p.PortId == phone.PortId);
				this.appContext.CurrentPhone = currentPhone;
			}
			catch (Exception)
			{
				this.appContext.CurrentPhone = null;
				throw new DeviceNotFoundException(string.Format("Phone name: {0}", phone.SalesName));
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		[CustomCommand]
		public void StopLumiaDetection()
		{
			try
			{
				((IAsyncDelegateCommand)base.Commands["StartLumiaDetection"]).Cancel();
				lock (this.readDeviceInfoCtsList)
				{
					foreach (CancellationTokenSource cancellationTokenSource in this.readDeviceInfoCtsList)
					{
						cancellationTokenSource.Cancel();
					}
					this.readDeviceInfoCtsList.Clear();
				}
				this.logics.LumiaAdaptation.StopDetection();
			}
			catch (Exception error)
			{
				Tracer<LumiaController>.WriteError(error);
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000D898 File Offset: 0x0000BA98
		[CustomCommand]
		public void StopCurrentLumiaDetection()
		{
			try
			{
				((IAsyncDelegateCommand)base.Commands["StartCurrentLumiaDetection"]).Cancel();
				lock (this.readDeviceInfoCtsList)
				{
					foreach (CancellationTokenSource cancellationTokenSource in this.readDeviceInfoCtsList)
					{
						cancellationTokenSource.Cancel();
					}
					this.readDeviceInfoCtsList.Clear();
				}
				this.logics.LumiaAdaptation.StopDetection();
			}
			catch (Exception error)
			{
				Tracer<LumiaController>.WriteError(error);
			}
		}

		// Token: 0x040000D0 RID: 208
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040000D1 RID: 209
		private readonly LogicContext logics;

		// Token: 0x040000D2 RID: 210
		private readonly List<CancellationTokenSource> readDeviceInfoCtsList = new List<CancellationTokenSource>();

		// Token: 0x040000D3 RID: 211
		private DetectionType currentDetectionType = DetectionType.None;

		// Token: 0x040000D4 RID: 212
		private Phone selectedPhone;
	}
}
