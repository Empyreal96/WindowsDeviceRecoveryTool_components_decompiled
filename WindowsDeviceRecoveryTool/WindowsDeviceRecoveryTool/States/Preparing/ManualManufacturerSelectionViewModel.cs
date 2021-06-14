using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x020000CC RID: 204
	[Export]
	public class ManualManufacturerSelectionViewModel : BaseViewModel, ICanHandle<SupportedManufacturersMessage>, ICanHandle
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x0001FF7C File Offset: 0x0001E17C
		[ImportingConstructor]
		public ManualManufacturerSelectionViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
			this.SelectTileCommand = new DelegateCommand<object>(new Action<object>(this.TileSelected));
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001FFB4 File Offset: 0x0001E1B4
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x0001FFCB File Offset: 0x0001E1CB
		public ICommand SelectTileCommand { get; private set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0001FFD4 File Offset: 0x0001E1D4
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x0001FFEC File Offset: 0x0001E1EC
		public ManualManuFacturerSelectionViewState ViewState
		{
			get
			{
				return this.viewState;
			}
			set
			{
				if (this.viewState != value)
				{
					this.viewState = value;
					base.RaisePropertyChanged<ManualManuFacturerSelectionViewState>(() => this.ViewState);
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0002004C File Offset: 0x0001E24C
		public override string PreviousStateName
		{
			get
			{
				return (this.ViewState == ManualManuFacturerSelectionViewState.OtherManufacturerSelection) ? "ManualManufacturerSelectionState" : "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00020074 File Offset: 0x0001E274
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x0002008C File Offset: 0x0001E28C
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

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000200DC File Offset: 0x0001E2DC
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x000200F4 File Offset: 0x0001E2F4
		public ObservableCollection<Tile> Tiles
		{
			get
			{
				return this.tiles;
			}
			set
			{
				base.SetValue<ObservableCollection<Tile>>(() => this.Tiles, ref this.tiles, value);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00020144 File Offset: 0x0001E344
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x00020164 File Offset: 0x0001E364
		public Tile SelectedManufacturer
		{
			get
			{
				return this.selectedManufacturer;
			}
			set
			{
				base.SetValue<Tile>(() => this.SelectedManufacturer, ref this.selectedManufacturer, value);
				this.AppContext.SelectedManufacturer = ((value == null) ? PhoneTypes.Generic : value.PhoneType);
				if (value != null)
				{
					string manufacturerStartingState;
					switch (this.AppContext.SelectedManufacturer)
					{
					case PhoneTypes.Lumia:
						manufacturerStartingState = "AwaitRecoveryDeviceState";
						goto IL_145;
					case PhoneTypes.Htc:
						manufacturerStartingState = "AwaitHtcState";
						goto IL_145;
					case PhoneTypes.Analog:
						manufacturerStartingState = "AwaitAnalogDeviceState";
						goto IL_145;
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
						manufacturerStartingState = "ManualGenericModelSelectionState";
						goto IL_145;
					case PhoneTypes.HoloLensAccessory:
						manufacturerStartingState = "AwaitFawkesDeviceState";
						goto IL_145;
					}
					manufacturerStartingState = "AwaitGenericDeviceState";
					IL_145:
					base.EventAggregator.Publish<DetectionTypeMessage>(new DetectionTypeMessage(DetectionType.RecoveryMode));
					base.Commands.Run((AppController c) => c.SwitchToState(manufacturerStartingState));
				}
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00020338 File Offset: 0x0001E538
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("ManufacturerHeader"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			this.SelectedManufacturer = null;
			this.ViewState = ManualManuFacturerSelectionViewState.InitialManufacturerSelection;
			if (this.AppContext.CurrentPhone != null && this.AppContext.CurrentPhone.Type != PhoneTypes.UnknownWp)
			{
				this.AppContext.CurrentPhone = null;
			}
			base.Commands.Run((FlowController c) => c.GetSupportedManufacturers());
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00020428 File Offset: 0x0001E628
		private void TileSelected(object parameter)
		{
			GroupTile groupTile = parameter as GroupTile;
			if (groupTile != null)
			{
				this.Tiles = new ObservableCollection<Tile>(groupTile.TilesInGroup);
				this.ViewState = ManualManuFacturerSelectionViewState.OtherManufacturerSelection;
			}
			else
			{
				Tile tile = parameter as Tile;
				this.SelectedManufacturer = tile;
				base.Commands.Run((FlowController c) => c.StartSessionFlow(string.Empty, new CancellationTokenSource().Token));
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00020530 File Offset: 0x0001E730
		public void Handle(SupportedManufacturersMessage message)
		{
			if (base.IsStarted)
			{
				this.Tiles.Clear();
				Phone currentPhone = this.AppContext.CurrentPhone;
				foreach (ManufacturerInfo manufacturerInfo in message.Manufacturers.Where(new Func<ManufacturerInfo, bool>(this.IsPreferredManufacturer)))
				{
					if (manufacturerInfo.RecoverySupport)
					{
						if (currentPhone != null && currentPhone.Type == PhoneTypes.UnknownWp)
						{
							if (!currentPhone.MatchedAdaptationTypes.Contains(manufacturerInfo.Type))
							{
								continue;
							}
						}
						this.Tiles.Add(this.ConvertToTile(manufacturerInfo));
					}
				}
				GroupTile item = new GroupTile((from m in message.Manufacturers
				where !this.IsPreferredManufacturer(m)
				select m).Select(new Func<ManufacturerInfo, Tile>(this.ConvertToTile)))
				{
					IsEnabled = true,
					IsWaiting = false,
					PhoneType = PhoneTypes.UnknownWp,
					Title = LocalizationManager.GetTranslation("OtherOEMs_GroupTile_Title"),
					Image = new BitmapImage(new Uri("pack://application:,,,/Resources/unknown_wp.png"))
				};
				this.Tiles.Add(item);
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x000206B0 File Offset: 0x0001E8B0
		private Tile ConvertToTile(ManufacturerInfo manufacturerInfo)
		{
			return new Tile
			{
				Title = manufacturerInfo.Name,
				PhoneType = manufacturerInfo.Type,
				Image = this.GetImage(manufacturerInfo.ImageData),
				IsEnabled = true,
				IsWaiting = false
			};
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00020708 File Offset: 0x0001E908
		private bool IsPreferredManufacturer(ManufacturerInfo manufacturerInfo)
		{
			PhoneTypes type = manufacturerInfo.Type;
			switch (type)
			{
			case PhoneTypes.Lumia:
			case PhoneTypes.Analog:
				break;
			case PhoneTypes.Htc:
				goto IL_24;
			default:
				if (type != PhoneTypes.HoloLensAccessory)
				{
					goto IL_24;
				}
				break;
			}
			return true;
			IL_24:
			return false;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00020740 File Offset: 0x0001E940
		private int FavorFirstPartyDevices(ManufacturerInfo manufacturerInfo)
		{
			return this.IsPreferredManufacturer(manufacturerInfo) ? 0 : 1;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00020760 File Offset: 0x0001E960
		private BitmapImage GetImage(byte[] imageData)
		{
			if (imageData != null)
			{
				using (MemoryStream memoryStream = new MemoryStream(imageData))
				{
					BitmapImage bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.StreamSource = memoryStream;
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.EndInit();
					bitmapImage.Freeze();
					return bitmapImage;
				}
			}
			return null;
		}

		// Token: 0x0400029D RID: 669
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x0400029E RID: 670
		private Tile selectedManufacturer;

		// Token: 0x0400029F RID: 671
		private ObservableCollection<Tile> tiles = new ObservableCollection<Tile>();

		// Token: 0x040002A0 RID: 672
		private ManualManuFacturerSelectionViewState viewState;
	}
}
