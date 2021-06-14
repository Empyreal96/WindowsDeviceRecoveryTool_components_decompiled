using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic.Services;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Detection;
using Microsoft.WindowsDeviceRecoveryTool.Detection.LegacySupport;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x020000A8 RID: 168
	[Export]
	public sealed class DeviceSelectionViewModel : BaseViewModel, INotifyLiveRegionChanged
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x0001701C File Offset: 0x0001521C
		[ImportingConstructor]
		internal DeviceSelectionViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext, AdaptationManager adaptationManager, DetectionHandlerFactory detectionHandlerFactory, PhoneFactory phoneFactory)
		{
			this.detectionHandlerFactory = detectionHandlerFactory;
			this.phoneFactory = phoneFactory;
			this.appContext = appContext;
			this.adaptationManager = adaptationManager;
			this.cancelTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(0.6)
			};
			this.cancelTimer.Tick += this.OnCancelTimerTick;
			this.SelectTileCommand = new DelegateCommand<object>(new Action<object>(this.OnTileSelectedCommandExecuted), new Func<object, bool>(this.OnCanExecuteSelectTileCommand));
			this.DeviceNotDetectedCommand = new DelegateCommand<object>(new Action<object>(this.OnDeviceNotDetectedCommandExecuted));
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000170D0 File Offset: 0x000152D0
		private bool OnCanExecuteSelectTileCommand(object arg)
		{
			Tile tile = arg as Tile;
			return tile != null && tile.IsEnabled;
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060004B2 RID: 1202 RVA: 0x00017100 File Offset: 0x00015300
		// (remove) Token: 0x060004B3 RID: 1203 RVA: 0x0001713C File Offset: 0x0001533C
		public event EventHandler LiveRegionChanged;

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00017178 File Offset: 0x00015378
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x0001718F File Offset: 0x0001538F
		public ICommand DeviceNotDetectedCommand { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00017198 File Offset: 0x00015398
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x000171AF File Offset: 0x000153AF
		public ICommand SelectTileCommand { get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x000171B8 File Offset: 0x000153B8
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x000171D0 File Offset: 0x000153D0
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

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00017220 File Offset: 0x00015420
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x00017238 File Offset: 0x00015438
		public CollectionObservable<Tile> Tiles
		{
			get
			{
				return this.tiles;
			}
			set
			{
				base.SetValue<CollectionObservable<Tile>>(() => this.Tiles, ref this.tiles, value);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00017288 File Offset: 0x00015488
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x000172A0 File Offset: 0x000154A0
		public Tile SelectedPhoneTile
		{
			get
			{
				return this.selectedPhoneTile;
			}
			set
			{
				base.SetValue<Tile>(() => this.SelectedPhoneTile, ref this.selectedPhoneTile, value);
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x000172F0 File Offset: 0x000154F0
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x00017308 File Offset: 0x00015508
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

		// Token: 0x060004C0 RID: 1216 RVA: 0x00017CE8 File Offset: 0x00015EE8
		public override async void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("DeviceSelectionHeader"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			this.SelectedPhoneTile = null;
			this.LiveText = null;
			this.Tiles.Clear();
			List<Task> tasks = new List<Task>();
			Exception error = null;
			bool operationCancelledInternally = false;
			bool operationCancelledExternally = false;
			this.itemSelectedTaskCompletionSource = new TaskCompletionSource<Tile>();
			Task<Tile> itemSelectedTask = this.itemSelectedTaskCompletionSource.Task;
			using (this.externalTokenSource = new CancellationTokenSource())
			{
				using (this.internalTokenSource = new CancellationTokenSource())
				{
					using (CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.externalTokenSource.Token, this.internalTokenSource.Token))
					{
						using (IDetectionHandler detectionHandler = this.detectionHandlerFactory.CreateDetectionHandler())
						{
							for (;;)
							{
								DeviceInfoEventArgs deviceInfoEventArgs;
								try
								{
									Task<DeviceInfoEventArgs> deviceChangeEventTask = detectionHandler.TakeDeviceInfoEventAsync(linkedTokenSource.Token);
									Task completedTask = await Task.WhenAny(new Task[]
									{
										itemSelectedTask,
										deviceChangeEventTask
									});
									if (completedTask == itemSelectedTask)
									{
										tasks.Add(deviceChangeEventTask);
										this.internalTokenSource.Cancel();
										break;
									}
									deviceInfoEventArgs = await deviceChangeEventTask;
									this.PurgeCompletedTasks(tasks);
								}
								catch (OperationCanceledException)
								{
									if (this.externalTokenSource.IsCancellationRequested)
									{
										operationCancelledExternally = true;
									}
									else if (this.internalTokenSource.IsCancellationRequested)
									{
										operationCancelledInternally = true;
									}
									break;
								}
								catch (Exception ex)
								{
									Tracer<DeviceSelectionViewModel>.WriteError(ex.ToString(), new object[0]);
									error = ex;
									break;
								}
								if (deviceInfoEventArgs.DeviceInfoAction == DeviceInfoAction.Attached)
								{
									Tracer<DeviceSelectionViewModel>.WriteInformation("Attached device detected: {0}", new object[]
									{
										deviceInfoEventArgs.DeviceInfo.DeviceIdentifier
									});
									Task item = this.ProcessDeviceAttachedAsync(detectionHandler, deviceInfoEventArgs.DeviceInfo, deviceInfoEventArgs.IsEnumerated, linkedTokenSource.Token);
									tasks.Add(item);
								}
								else
								{
									Tracer<DeviceSelectionViewModel>.WriteInformation("DetachedTask device detected: {0}", new object[]
									{
										deviceInfoEventArgs.DeviceInfo.DeviceIdentifier
									});
									Task item2 = this.ProcessDeviceDetachedAsync(deviceInfoEventArgs.DeviceInfo);
									tasks.Add(item2);
								}
							}
							try
							{
								await Task.WhenAll(tasks);
							}
							catch (Exception ex)
							{
								Tracer<DeviceSelectionViewModel>.WriteWarning(ex.ToString(), new object[0]);
							}
						}
					}
				}
			}
			this.externalTokenSource = null;
			this.internalTokenSource = null;
			if (error != null)
			{
				ExceptionDispatchInfo.Capture(error).Throw();
			}
			if (operationCancelledInternally)
			{
				IDictionary<string, IDelegateCommand> commands = base.Commands;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(AppController), "c");
				commands.Run(Expression.Lambda<Action<AppController>>(Expression.Call(parameterExpression, methodof(AppController.SwitchToState(string)), new Expression[]
				{
					Expression.Constant("ManualManufacturerSelectionState", typeof(string))
				}), new ParameterExpression[]
				{
					parameterExpression
				}));
			}
			else if (!operationCancelledExternally)
			{
				Tile selectedTile;
				try
				{
					selectedTile = await itemSelectedTask;
				}
				catch (OperationCanceledException)
				{
					IDictionary<string, IDelegateCommand> commands2 = base.Commands;
					ParameterExpression parameterExpression = Expression.Parameter(typeof(AppController), "c");
					commands2.Run(Expression.Lambda<Action<AppController>>(Expression.Call(parameterExpression, methodof(AppController.SwitchToState(string)), new Expression[]
					{
						Expression.Constant("AutomaticManufacturerSelectionState", typeof(string))
					}), new ParameterExpression[]
					{
						parameterExpression
					}));
					return;
				}
				this.OnCompleted(selectedTile.Phone);
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00017D24 File Offset: 0x00015F24
		public override void OnStopped()
		{
			if (this.externalTokenSource != null)
			{
				this.externalTokenSource.Cancel();
			}
			this.cancelTimer.IsEnabled = false;
			base.OnStopped();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00017D6C File Offset: 0x00015F6C
		private void OnCompleted(Phone phone)
		{
			this.AppContext.CurrentPhone = phone;
			this.AppContext.SelectedManufacturer = phone.Type;
			PhoneTypes selectedManufacturer = this.appContext.SelectedManufacturer;
			string nextState;
			switch (selectedManufacturer)
			{
			case PhoneTypes.Lumia:
			case PhoneTypes.Analog:
				nextState = "ReadingDeviceInfoState";
				goto IL_137;
			case PhoneTypes.Htc:
				nextState = "AwaitHtcState";
				goto IL_137;
			case PhoneTypes.Lg:
				break;
			case PhoneTypes.Mcj:
			case PhoneTypes.Blu:
			case PhoneTypes.Alcatel:
			case PhoneTypes.Acer:
			case PhoneTypes.Trinity:
			case PhoneTypes.Unistrong:
			case PhoneTypes.YEZZ:
			case PhoneTypes.Micromax:
			case PhoneTypes.Funker:
			case PhoneTypes.Diginnos:
			case PhoneTypes.VAIO:
			case PhoneTypes.HP:
			case PhoneTypes.Inversenet:
			case PhoneTypes.Freetel:
			case PhoneTypes.XOLO:
			case PhoneTypes.KM:
			case PhoneTypes.Jenesis:
			case PhoneTypes.Gomobile:
			case PhoneTypes.Lenovo:
			case PhoneTypes.Zebra:
			case PhoneTypes.Honeywell:
			case PhoneTypes.Panasonic:
			case PhoneTypes.TrekStor:
			case PhoneTypes.Wileyfox:
			{
				BaseAdaptation adaptation = this.adaptationManager.GetAdaptation(phone.Type);
				if (adaptation.ManuallySupportedVariants(phone).Any<Phone>())
				{
					nextState = "ManualGenericVariantSelectionState";
				}
				else
				{
					nextState = "CheckLatestPackageState";
				}
				goto IL_137;
			}
			case PhoneTypes.HoloLensAccessory:
				nextState = "AwaitFawkesDeviceState";
				goto IL_137;
			default:
				if (selectedManufacturer == PhoneTypes.UnknownWp)
				{
					nextState = "ManualManufacturerSelectionState";
					goto IL_137;
				}
				break;
			}
			nextState = "AwaitGenericDeviceState";
			IL_137:
			base.EventAggregator.Publish<DetectionTypeMessage>(new DetectionTypeMessage(DetectionType.NormalMode));
			base.EventAggregator.Publish<SelectedDeviceMessage>(new SelectedDeviceMessage(phone));
			base.Commands.Run((AppController c) => c.SwitchToState(nextState));
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00017F45 File Offset: 0x00016145
		private void OnDeviceNotDetectedCommandExecuted(object obj)
		{
			this.internalTokenSource.Cancel();
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00017F54 File Offset: 0x00016154
		private void OnTileSelectedCommandExecuted(object obj)
		{
			this.SelectedPhoneTile = (obj as Tile);
			this.itemSelectedTaskCompletionSource.SetResult(obj as Tile);
			base.Commands.Run((FlowController c) => c.StartSessionFlow(string.Empty, new CancellationTokenSource().Token));
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00018040 File Offset: 0x00016240
		private void PurgeCompletedTasks(List<Task> tasks)
		{
			Task[] array = (from t in tasks
			where t.IsCanceled || t.IsCompleted || t.IsFaulted
			select t).ToArray<Task>();
			foreach (Task item in array)
			{
				tasks.Remove(item);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001838C File Offset: 0x0001658C
		private async Task ProcessDeviceAttachedAsync(IDetectionHandler detectionHandler, DeviceInfo deviceInfo, bool isEnumerated, CancellationToken cancellationToken)
		{
			Tile tile = this.CreateNewTile(deviceInfo.DeviceIdentifier);
			this.AddTile(tile);
			if (!isEnumerated)
			{
				this.LiveText = LocalizationManager.GetTranslation("DeviceConnected");
			}
			Phone phone;
			try
			{
				await detectionHandler.UpdateDeviceInfoAsync(deviceInfo, cancellationToken);
				phone = await this.phoneFactory.CreateAsync(deviceInfo, cancellationToken);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception error)
			{
				Tracer<DeviceSelectionViewModel>.WriteError(error);
				return;
			}
			if (!deviceInfo.IsDeviceSupported)
			{
				Tracer<DeviceSelectionViewModel>.WriteInformation("Device not supppoted: {0}", new object[]
				{
					deviceInfo.DeviceIdentifier
				});
				this.UpdateTileInformationForNotSupported(tile);
			}
			else
			{
				Tracer<DeviceSelectionViewModel>.WriteInformation("Device supppoted: {0}", new object[]
				{
					deviceInfo.DeviceIdentifier
				});
				this.UpdateTileInformation(tile, phone, deviceInfo);
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00018484 File Offset: 0x00016684
		private Task ProcessDeviceDetachedAsync(DeviceInfo deviceInfo)
		{
			TaskCompletionSource<object> source = new TaskCompletionSource<object>();
			foreach (Tile tile2 in this.Tiles)
			{
				tile2.ShowStartAnimation = false;
			}
			Tile tileToRemove = this.Tiles.FirstOrDefault((Tile tile) => string.Equals(tile.DevicePath, deviceInfo.DeviceIdentifier, StringComparison.CurrentCultureIgnoreCase));
			if (tileToRemove != null)
			{
				this.LiveText = LocalizationManager.GetTranslation("DeviceDisconnected");
				EventHandler onTimerElapsed = null;
				onTimerElapsed = delegate(object sender, EventArgs args)
				{
					tileToRemove.OnRemoveTimerElapsed -= onTimerElapsed;
					this.RemoveTile(tileToRemove);
					source.SetResult(null);
				};
				tileToRemove.OnRemoveTimerElapsed += onTimerElapsed;
				tileToRemove.IsDeleted = true;
				Tracer<DeviceSelectionViewModel>.WriteInformation("Removed device: {0}", new object[]
				{
					deviceInfo.DeviceIdentifier
				});
			}
			else
			{
				Tracer<DeviceSelectionViewModel>.WriteError("Tile not found !!!", new object[0]);
				source.SetResult(null);
			}
			return source.Task;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000185E0 File Offset: 0x000167E0
		private void RemoveTile(Tile tile)
		{
			this.Tiles.Remove(tile);
			if (this.Tiles.Count == 0)
			{
				this.cancelTimer.Start();
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001861E File Offset: 0x0001681E
		private void AddTile(Tile tile)
		{
			this.Tiles.Add(tile);
			this.cancelTimer.IsEnabled = false;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001863C File Offset: 0x0001683C
		private void OnCancelTimerTick(object sender, EventArgs eventArgs)
		{
			this.cancelTimer.IsEnabled = false;
			if (this.Tiles.Count == 0)
			{
				this.itemSelectedTaskCompletionSource.SetCanceled();
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001867C File Offset: 0x0001687C
		private Tile CreateNewTile(string path)
		{
			return new Tile
			{
				DevicePath = path,
				IsEnabled = false,
				IsWaiting = true,
				ShowStartAnimation = true,
				Title = LocalizationManager.GetTranslation("ReadingDeviceInfo"),
				Image = new BitmapImage(new Uri("pack://application:,,,/Resources/unknown_wp.png"))
			};
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000186DC File Offset: 0x000168DC
		private void OnLiveRegionChanged()
		{
			EventHandler liveRegionChanged = this.LiveRegionChanged;
			if (liveRegionChanged != null)
			{
				EventArgs empty = EventArgs.Empty;
				liveRegionChanged(this, empty);
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001870A File Offset: 0x0001690A
		private void UpdateTileInformationForNotSupported(Tile tile)
		{
			tile.Title = LocalizationManager.GetTranslation("ManufacturerDetectionFailed");
			tile.IsWaiting = false;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00018728 File Offset: 0x00016928
		private void UpdateTileInformation(Tile tile, Phone phone, DeviceInfo deviceInfo)
		{
			tile.Phone = phone;
			tile.PhoneType = phone.Type;
			tile.Image = DeviceSelectionViewModel.LoadBitmapImageFromBytes(deviceInfo.DeviceBitmapBytes);
			tile.Title = deviceInfo.DeviceSalesName;
			tile.SupportId = deviceInfo.SupportId;
			tile.IsWaiting = false;
			tile.IsEnabled = true;
			tile.BasicDeviceInformation = deviceInfo;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00018790 File Offset: 0x00016990
		private static BitmapImage LoadBitmapImageFromBytes(byte[] bytes)
		{
			BitmapImage result;
			if (bytes == null)
			{
				result = null;
			}
			else
			{
				using (MemoryStream memoryStream = new MemoryStream(bytes))
				{
					BitmapImage bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.StreamSource = memoryStream;
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.EndInit();
					result = bitmapImage;
				}
			}
			return result;
		}

		// Token: 0x04000209 RID: 521
		private readonly DetectionHandlerFactory detectionHandlerFactory;

		// Token: 0x0400020A RID: 522
		private readonly PhoneFactory phoneFactory;

		// Token: 0x0400020B RID: 523
		private readonly AdaptationManager adaptationManager;

		// Token: 0x0400020C RID: 524
		private readonly DispatcherTimer cancelTimer;

		// Token: 0x0400020D RID: 525
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x0400020E RID: 526
		private Tile selectedPhoneTile;

		// Token: 0x0400020F RID: 527
		private string liveText;

		// Token: 0x04000210 RID: 528
		private CollectionObservable<Tile> tiles = new CollectionObservable<Tile>();

		// Token: 0x04000211 RID: 529
		private TaskCompletionSource<Tile> itemSelectedTaskCompletionSource;

		// Token: 0x04000212 RID: 530
		private CancellationTokenSource externalTokenSource;

		// Token: 0x04000213 RID: 531
		private CancellationTokenSource internalTokenSource;
	}
}
