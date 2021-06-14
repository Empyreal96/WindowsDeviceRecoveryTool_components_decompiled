using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic.Services;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x020000A1 RID: 161
	[Export]
	public class ManualGenericVariantSelectionViewModel : BaseViewModel
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x000159C0 File Offset: 0x00013BC0
		[ImportingConstructor]
		public ManualGenericVariantSelectionViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext, AdaptationManager adaptationManager)
		{
			this.appContext = appContext;
			this.adaptationManager = adaptationManager;
			this.SelectTileCommand = new DelegateCommand<object>(new Action<object>(this.TileSelected));
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x000159FC File Offset: 0x00013BFC
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x00015A14 File Offset: 0x00013C14
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

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00015A64 File Offset: 0x00013C64
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x00015A7C File Offset: 0x00013C7C
		public string ModelIdentificationInstruction
		{
			get
			{
				return this.modelIdentificationInstruction;
			}
			set
			{
				base.SetValue<string>(() => this.ModelIdentificationInstruction, ref this.modelIdentificationInstruction, value);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x00015ACC File Offset: 0x00013CCC
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x00015AE3 File Offset: 0x00013CE3
		public ICommand SelectTileCommand { get; private set; }

		// Token: 0x06000480 RID: 1152 RVA: 0x00015AEC File Offset: 0x00013CEC
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			this.Tiles.Clear();
			Phone currentPhone = this.appContext.CurrentPhone;
			this.ModelIdentificationInstruction = currentPhone.ModelIdentificationInstruction;
			BaseAdaptation adaptation = this.adaptationManager.GetAdaptation(currentPhone.Type);
			List<Phone> list = adaptation.ManuallySupportedVariants(currentPhone);
			list.ForEach(new Action<Phone>(this.AddTile));
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00015B68 File Offset: 0x00013D68
		private void AddTile(Phone phone)
		{
			Tile item = new Tile
			{
				Phone = phone,
				PhoneType = phone.Type,
				Title = phone.SalesName,
				IsEnabled = true,
				Image = this.GetImage(phone.ImageData)
			};
			this.Tiles.Add(item);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00015BC8 File Offset: 0x00013DC8
		private void TileSelected(object obj)
		{
			Tile tile = obj as Tile;
			if (tile != null)
			{
				this.appContext.CurrentPhone.SalesName = tile.Phone.SalesName;
				this.appContext.CurrentPhone.HardwareModel = tile.Phone.HardwareModel;
				this.appContext.CurrentPhone.HardwareVariant = tile.Phone.HardwareVariant;
				this.appContext.CurrentPhone.QueryParameters = tile.Phone.QueryParameters;
				base.Commands.Run((AppController c) => c.SwitchToState("CheckLatestPackageState"));
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00015CC4 File Offset: 0x00013EC4
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

		// Token: 0x040001F4 RID: 500
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040001F5 RID: 501
		private readonly AdaptationManager adaptationManager;

		// Token: 0x040001F6 RID: 502
		private CollectionObservable<Tile> tiles = new CollectionObservable<Tile>();

		// Token: 0x040001F7 RID: 503
		private string modelIdentificationInstruction;
	}
}
