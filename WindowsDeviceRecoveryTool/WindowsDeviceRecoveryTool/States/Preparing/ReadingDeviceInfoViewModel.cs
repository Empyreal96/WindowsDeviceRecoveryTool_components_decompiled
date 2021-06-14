using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Timers;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Microsoft.WindowsDeviceRecoveryTool.States.BaseStates;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x020000A6 RID: 166
	[Export]
	public class ReadingDeviceInfoViewModel : BaseViewModel, ICanHandle<DeviceInfoReadMessage>, ICanHandle<DetectionTypeMessage>, ICanHandle<SelectedDeviceMessage>, ICanHandle<DeviceConnectionStatusReadMessage>, ICanHandle<DeviceDisconnectedMessage>, ICanHandle
	{
		// Token: 0x0600049C RID: 1180 RVA: 0x000164B3 File Offset: 0x000146B3
		[ImportingConstructor]
		public ReadingDeviceInfoViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x000164C8 File Offset: 0x000146C8
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x000164E0 File Offset: 0x000146E0
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00016530 File Offset: 0x00014730
		public override string PreviousStateName
		{
			get
			{
				string result;
				if (this.conditions.IsHtcConnected())
				{
					base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false));
					result = "RebootHtcState";
				}
				else
				{
					result = "AutomaticManufacturerSelectionState";
				}
				return result;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00016574 File Offset: 0x00014774
		// (set) Token: 0x060004A1 RID: 1185 RVA: 0x0001658C File Offset: 0x0001478C
		public Phone SelectedPhone
		{
			get
			{
				return this.selectedPhone;
			}
			set
			{
				base.SetValue<Phone>(() => this.SelectedPhone, ref this.selectedPhone, value);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x000165DC File Offset: 0x000147DC
		// (set) Token: 0x060004A3 RID: 1187 RVA: 0x000165F3 File Offset: 0x000147F3
		private DetectionType DetectionType { get; set; }

		// Token: 0x060004A4 RID: 1188 RVA: 0x00016604 File Offset: 0x00014804
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("ReadingDeviceInfo"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			if ((this.DetectionType != DetectionType.RecoveryMode || (this.AppContext.SelectedManufacturer != PhoneTypes.Htc && this.AppContext.SelectedManufacturer != PhoneTypes.Lg && this.AppContext.SelectedManufacturer != PhoneTypes.Mcj && this.AppContext.SelectedManufacturer != PhoneTypes.Alcatel && this.AppContext.SelectedManufacturer != PhoneTypes.Generic && this.AppContext.SelectedManufacturer != PhoneTypes.Analog && this.AppContext.SelectedManufacturer != PhoneTypes.HoloLensAccessory)) && this.SelectedPhone == null)
			{
				throw new Exception("No phone from DeviceSelection state.");
			}
			PhoneTypes selectedManufacturer = this.AppContext.SelectedManufacturer;
			switch (selectedManufacturer)
			{
			case PhoneTypes.Lumia:
				base.Commands.Run((LumiaController c) => c.SetSelectedPhone(this.SelectedPhone, CancellationToken.None));
				base.Commands.Run((LumiaController c) => c.StartCurrentLumiaDetection(this.DetectionType, CancellationToken.None));
				goto IL_346;
			case PhoneTypes.Htc:
			case PhoneTypes.Analog:
			case PhoneTypes.Lg:
			case PhoneTypes.Mcj:
			case PhoneTypes.Blu:
			case PhoneTypes.Alcatel:
			case PhoneTypes.HoloLensAccessory:
				break;
			default:
				if (selectedManufacturer != PhoneTypes.Generic)
				{
					throw new NotImplementedException();
				}
				break;
			}
			this.readingDeviceInfoTimer = new System.Timers.Timer(60000.0);
			this.readingDeviceInfoTimer.Elapsed += this.ReadingDeviceInfoTimerElapsed;
			this.readingDeviceInfoTimer.Start();
			base.Commands.Run((FlowController c) => c.ReadDeviceInfo(this.AppContext.CurrentPhone, CancellationToken.None));
			IL_346:
			DetectionParameters detectionParams = new DetectionParameters(PhoneTypes.All, PhoneModes.Normal);
			base.Commands.Run((FlowController c) => c.StartDeviceDetection(detectionParams));
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000169D4 File Offset: 0x00014BD4
		public override void OnStopped()
		{
			PhoneTypes selectedManufacturer = this.AppContext.SelectedManufacturer;
			switch (selectedManufacturer)
			{
			case PhoneTypes.Lumia:
				base.Commands.Run((LumiaController c) => c.StopCurrentLumiaDetection());
				goto IL_E7;
			case PhoneTypes.Htc:
			case PhoneTypes.Analog:
			case PhoneTypes.Lg:
			case PhoneTypes.Mcj:
			case PhoneTypes.Blu:
			case PhoneTypes.Alcatel:
			case PhoneTypes.HoloLensAccessory:
				break;
			default:
				if (selectedManufacturer != PhoneTypes.Generic)
				{
					goto IL_E7;
				}
				break;
			}
			this.readingDeviceInfoTimer.Stop();
			base.Commands.Run((FlowController c) => c.CancelReadDeviceInfo());
			IL_E7:
			base.Commands.Run((FlowController c) => c.StopDeviceDetection());
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00016B18 File Offset: 0x00014D18
		public void Handle(DeviceInfoReadMessage message)
		{
			if (base.IsStarted)
			{
				if (this.AppContext.SelectedManufacturer == PhoneTypes.Generic && !this.AppContext.CurrentPhone.PlatformId.ToString().StartsWith("MCJ.QC8916.M54TJP", StringComparison.InvariantCultureIgnoreCase) && !this.AppContext.CurrentPhone.PlatformId.ToString().StartsWith("BLU.QC8612.MTP", StringComparison.InvariantCultureIgnoreCase) && !this.AppContext.CurrentPhone.PlatformId.ToString().StartsWith("BLU.QC8916.QL850", StringComparison.InvariantCultureIgnoreCase))
				{
					Tracer<ReadingDeviceInfoViewModel>.WriteError("Uknown PlatformID: {0}", new object[]
					{
						this.AppContext.CurrentPhone.PlatformId
					});
					base.Commands.Run((AppController c) => c.SwitchToState("UnsupportedDeviceState"));
				}
				else if ((this.AppContext.SelectedManufacturer == PhoneTypes.Analog || this.AppContext.SelectedManufacturer == PhoneTypes.Htc || this.AppContext.SelectedManufacturer == PhoneTypes.Lg || this.AppContext.SelectedManufacturer == PhoneTypes.Mcj || this.AppContext.SelectedManufacturer == PhoneTypes.Blu || this.AppContext.SelectedManufacturer == PhoneTypes.Alcatel || this.AppContext.SelectedManufacturer == PhoneTypes.HoloLensAccessory) && message.Result)
				{
					base.Commands.Run((AppController c) => c.SwitchToState("CheckLatestPackageState"));
				}
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00016D2C File Offset: 0x00014F2C
		private void ReadingDeviceInfoTimerElapsed(object sender, ElapsedEventArgs e)
		{
			if (this.AppContext.CurrentPhone.Type == PhoneTypes.Htc)
			{
				base.Commands.Run((AppController c) => c.SwitchToState("ManualHtcRestartState"));
			}
			else if (this.AppContext.CurrentPhone.Type == PhoneTypes.Analog)
			{
				base.EventAggregator.Publish<ErrorMessage>(new ErrorMessage(new ReadPhoneInformationException("Please check if device was connected in NORMAL mode")));
				base.Commands.Run((AppController c) => c.SwitchToState("ErrorState"));
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00016E61 File Offset: 0x00015061
		public void Handle(DetectionTypeMessage message)
		{
			this.DetectionType = message.DetectionType;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00016E71 File Offset: 0x00015071
		public void Handle(SelectedDeviceMessage message)
		{
			this.SelectedPhone = message.SelectedPhone;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00016E84 File Offset: 0x00015084
		private bool NeedToCheckIfDeviceWasDisconnected()
		{
			return this.appContext != null && this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.Type == PhoneTypes.HoloLensAccessory;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00016EC4 File Offset: 0x000150C4
		public void Handle(DeviceDisconnectedMessage message)
		{
			if (base.IsStarted)
			{
				if (this.NeedToCheckIfDeviceWasDisconnected())
				{
					base.Commands.Run((FlowController fc) => fc.CheckIfDeviceStillConnected(this.appContext.CurrentPhone, CancellationToken.None));
				}
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00016F98 File Offset: 0x00015198
		public void Handle(DeviceConnectionStatusReadMessage message)
		{
			if (base.IsStarted)
			{
				if (!message.Status)
				{
					throw new DeviceDisconnectedException();
				}
			}
		}

		// Token: 0x04000200 RID: 512
		private const string McjPlatformId = "MCJ.QC8916.M54TJP";

		// Token: 0x04000201 RID: 513
		private const string BluW510UPlatformId = "BLU.QC8612.MTP";

		// Token: 0x04000202 RID: 514
		private const string BluLtePlatformId = "BLU.QC8916.QL850";

		// Token: 0x04000203 RID: 515
		private System.Timers.Timer readingDeviceInfoTimer;

		// Token: 0x04000204 RID: 516
		[Import]
		private Conditions conditions;

		// Token: 0x04000205 RID: 517
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x04000206 RID: 518
		private Phone selectedPhone;
	}
}
