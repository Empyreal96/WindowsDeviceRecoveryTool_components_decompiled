using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic.Services;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x0200009F RID: 159
	[Export]
	public class ManualGenericModelSelectionViewModel : BaseViewModel, ICanHandle<SupportedAdaptationModelsMessage>, ICanHandle
	{
		// Token: 0x06000465 RID: 1125 RVA: 0x000151B0 File Offset: 0x000133B0
		[ImportingConstructor]
		public ManualGenericModelSelectionViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext, AdaptationManager adaptationManager)
		{
			this.appContext = appContext;
			this.adaptationManager = adaptationManager;
			this.SelectTileCommand = new DelegateCommand<object>(new Action<object>(this.TileSelected));
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000151EC File Offset: 0x000133EC
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00015203 File Offset: 0x00013403
		public ICommand SelectTileCommand { get; private set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0001520C File Offset: 0x0001340C
		public override string PreviousStateName
		{
			get
			{
				return "ManualManufacturerSelectionState";
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00015224 File Offset: 0x00013424
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x0001523C File Offset: 0x0001343C
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0001528C File Offset: 0x0001348C
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x000152AC File Offset: 0x000134AC
		public Tile SelectedModel
		{
			get
			{
				return this.selectedModel;
			}
			set
			{
				base.SetValue<Tile>(() => this.SelectedModel, ref this.selectedModel, value);
				if (value != null)
				{
					this.appContext.CurrentPhone = new Phone
					{
						SalesName = value.Phone.SalesName,
						HardwareModel = value.Phone.HardwareModel,
						HardwareVariant = value.Phone.HardwareVariant,
						ModelIdentificationInstruction = value.Phone.ModelIdentificationInstruction,
						Type = value.Phone.Type,
						ImageData = value.Phone.ImageData,
						QueryParameters = value.Phone.QueryParameters
					};
					this.goingForward = true;
					BaseAdaptation adaptation = this.adaptationManager.GetAdaptation(value.Phone.Type);
					string nextState;
					if (adaptation.ManuallySupportedVariants(value.Phone).Any<Phone>())
					{
						nextState = "ManualGenericVariantSelectionState";
					}
					else
					{
						nextState = "CheckLatestPackageState";
					}
					base.Commands.Run((AppController c) => c.SwitchToState(nextState));
				}
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00015464 File Offset: 0x00013664
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("ManualModelSelection"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			this.goingForward = false;
			if (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.Type == PhoneTypes.UnknownWp)
			{
				this.appContext.CurrentPhone.Type = this.appContext.SelectedManufacturer;
			}
			base.Commands.Run((FlowController c) => c.GetSupportedAdaptationModels(this.appContext.SelectedManufacturer));
			base.Commands.Run((FlowController c) => c.StartSessionFlow(string.Empty, CancellationToken.None));
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00015614 File Offset: 0x00013814
		public override void OnStopped()
		{
			base.OnStopped();
			if (!this.goingForward && this.appContext.CurrentPhone != null)
			{
				this.appContext.CurrentPhone = null;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00015656 File Offset: 0x00013856
		private void TileSelected(object obj)
		{
			this.SelectedModel = (obj as Tile);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00015668 File Offset: 0x00013868
		public void Handle(SupportedAdaptationModelsMessage message)
		{
			if (base.IsStarted)
			{
				this.Tiles.Clear();
				foreach (Phone phone in message.Models)
				{
					this.Tiles.Add(new Tile
					{
						Title = phone.SalesName,
						PhoneType = phone.Type,
						Image = this.GetImage(phone.ImageData),
						Phone = phone,
						IsEnabled = true
					});
				}
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001572C File Offset: 0x0001392C
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

		// Token: 0x040001EC RID: 492
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040001ED RID: 493
		private readonly AdaptationManager adaptationManager;

		// Token: 0x040001EE RID: 494
		private bool goingForward;

		// Token: 0x040001EF RID: 495
		private Tile selectedModel;

		// Token: 0x040001F0 RID: 496
		private ObservableCollection<Tile> tiles = new ObservableCollection<Tile>();
	}
}
