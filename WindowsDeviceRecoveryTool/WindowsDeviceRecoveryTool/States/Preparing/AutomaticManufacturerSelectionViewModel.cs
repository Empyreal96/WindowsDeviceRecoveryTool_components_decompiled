using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Detection;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x0200008C RID: 140
	[Export]
	public sealed class AutomaticManufacturerSelectionViewModel : BaseViewModel, ICanHandle<SupportedManufacturersMessage>, ICanHandle, INotifyLiveRegionChanged
	{
		// Token: 0x060003CB RID: 971 RVA: 0x00012114 File Offset: 0x00010314
		[ImportingConstructor]
		internal AutomaticManufacturerSelectionViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext, DetectionHandlerFactory detectionHandlerFactory)
		{
			this.appContext = appContext;
			this.detectionHandlerFactory = detectionHandlerFactory;
			this.attachedDeviceIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			this.switchToDeviceSelectionTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(0.75)
			};
			this.switchToDeviceSelectionTimer.Tick += delegate(object s, EventArgs e)
			{
				this.OnSwitchToDeviceSelectionTimerTick();
			};
			this.DeviceNotDetectedCommand = new DelegateCommand<object>(new Action<object>(this.OnDeviceNotDetectedCommandExecuted));
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060003CC RID: 972 RVA: 0x000121A4 File Offset: 0x000103A4
		// (remove) Token: 0x060003CD RID: 973 RVA: 0x000121E0 File Offset: 0x000103E0
		public event EventHandler LiveRegionChanged;

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0001221C File Offset: 0x0001041C
		// (set) Token: 0x060003CF RID: 975 RVA: 0x00012233 File Offset: 0x00010433
		public ICommand DeviceNotDetectedCommand { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0001223C File Offset: 0x0001043C
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x00012254 File Offset: 0x00010454
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x000122A4 File Offset: 0x000104A4
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x000122BC File Offset: 0x000104BC
		public bool AnalogSupported
		{
			get
			{
				return this.analogSupported;
			}
			set
			{
				base.SetValue<bool>(() => this.AnalogSupported, ref this.analogSupported, value);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0001230C File Offset: 0x0001050C
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x00012324 File Offset: 0x00010524
		public string LiveText
		{
			get
			{
				return this.liveText;
			}
			set
			{
				base.SetValue<string>(() => this.LiveText, ref this.liveText, value);
				if (!string.IsNullOrWhiteSpace(this.liveText))
				{
					this.OnLiveRegionChanged();
				}
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001280C File Offset: 0x00010A0C
		public override async void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("WelcomeHeader"), ""));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			this.AppContext.CurrentPhone = null;
			this.LiveText = null;
			base.Commands.Run((FlowController c) => c.GetSupportedManufacturers());
			this.attachedDeviceIds.Clear();
			using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
			{
				using (IDetectionHandler deviceMonitor = this.detectionHandlerFactory.CreateDetectionHandler())
				{
					this.internalTokenSource = cancellationTokenSource;
					try
					{
						CancellationToken cancellationToken = cancellationTokenSource.Token;
						while (!cancellationToken.IsCancellationRequested)
						{
							DeviceInfoEventArgs deviceInfoEvent = await deviceMonitor.TakeDeviceInfoEventAsync(cancellationToken);
							if (deviceInfoEvent.DeviceInfoAction == DeviceInfoAction.Attached)
							{
								DeviceInfo deviceInfo = deviceInfoEvent.DeviceInfo;
								Tracer<AutomaticManufacturerSelectionViewModel>.WriteInformation("Attached device detected: {0}", new object[]
								{
									deviceInfo.DeviceIdentifier
								});
								if (deviceInfoEvent.IsEnumerated)
								{
									this.OnDeviceAttachedOnStartup(deviceInfo);
								}
								else
								{
									this.OnDeviceAttached(deviceInfo);
								}
							}
							else if (deviceInfoEvent.DeviceInfoAction == DeviceInfoAction.Detached)
							{
								DeviceInfo deviceInfo = deviceInfoEvent.DeviceInfo;
								Tracer<AutomaticManufacturerSelectionViewModel>.WriteInformation("Detached device detected: {0}", new object[]
								{
									deviceInfo.DeviceIdentifier
								});
								this.OnDeviceDetached(deviceInfo);
							}
						}
					}
					catch (OperationCanceledException)
					{
						Tracer<AutomaticManufacturerSelectionViewModel>.WriteInformation("Detection cancelled");
					}
					catch (Exception ex)
					{
						Tracer<AutomaticManufacturerSelectionViewModel>.WriteError(ex.ToString(), new object[0]);
						throw;
					}
					finally
					{
						this.internalTokenSource = null;
					}
				}
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00012848 File Offset: 0x00010A48
		public override void OnStopped()
		{
			base.OnStopped();
			this.switchToDeviceSelectionTimer.IsEnabled = false;
			if (this.internalTokenSource != null)
			{
				this.internalTokenSource.Cancel();
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00012888 File Offset: 0x00010A88
		public void Handle(SupportedManufacturersMessage message)
		{
			if (base.IsStarted)
			{
				foreach (ManufacturerInfo manufacturerInfo in message.Manufacturers)
				{
					if (manufacturerInfo.Type == PhoneTypes.Analog)
					{
						this.AnalogSupported = true;
						break;
					}
				}
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00012908 File Offset: 0x00010B08
		private void OnDeviceNotDetectedCommandExecuted(object obj)
		{
			this.SwitchToManufacturerSelection();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00012914 File Offset: 0x00010B14
		private void OnDeviceDetached(DeviceInfo device)
		{
			if (this.attachedDeviceIds.Remove(device.DeviceIdentifier))
			{
				this.LiveText = LocalizationManager.GetTranslation("DeviceDisconnected");
			}
			if (this.attachedDeviceIds.Count == 0)
			{
				this.switchToDeviceSelectionTimer.IsEnabled = false;
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00012974 File Offset: 0x00010B74
		private void OnDeviceAttached(DeviceInfo device)
		{
			if (this.attachedDeviceIds.Add(device.DeviceIdentifier))
			{
				this.LiveText = LocalizationManager.GetTranslation("DeviceConnected");
			}
			this.switchToDeviceSelectionTimer.IsEnabled = true;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000129BA File Offset: 0x00010BBA
		private void OnDeviceAttachedOnStartup(DeviceInfo device)
		{
			this.SwitchToDeviceSelection();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000129C4 File Offset: 0x00010BC4
		private void OnSwitchToDeviceSelectionTimerTick()
		{
			this.SwitchToDeviceSelection();
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000129CE File Offset: 0x00010BCE
		private void SwitchToDeviceSelection()
		{
			this.SwitchToState("DeviceSelectionState");
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000129DD File Offset: 0x00010BDD
		private void SwitchToManufacturerSelection()
		{
			this.SwitchToState("ManualManufacturerSelectionState");
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000129F4 File Offset: 0x00010BF4
		private void SwitchToState(string nextState)
		{
			this.switchToDeviceSelectionTimer.IsEnabled = false;
			base.Commands.Run((AppController c) => c.SwitchToState(nextState));
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00012A84 File Offset: 0x00010C84
		private void OnLiveRegionChanged()
		{
			EventHandler liveRegionChanged = this.LiveRegionChanged;
			if (liveRegionChanged != null)
			{
				liveRegionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x040001BA RID: 442
		private readonly DetectionHandlerFactory detectionHandlerFactory;

		// Token: 0x040001BB RID: 443
		private readonly DispatcherTimer switchToDeviceSelectionTimer;

		// Token: 0x040001BC RID: 444
		private readonly HashSet<string> attachedDeviceIds;

		// Token: 0x040001BD RID: 445
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040001BE RID: 446
		private string liveText;

		// Token: 0x040001BF RID: 447
		private bool analogSupported;

		// Token: 0x040001C0 RID: 448
		private CancellationTokenSource internalTokenSource;
	}
}
