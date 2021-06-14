using System;
using System.Windows;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool.States.BaseStates
{
	// Token: 0x020000DF RID: 223
	public class UiState<TView, TViewModel> : UiBaseState where TView : FrameworkElement where TViewModel : BaseViewModel
	{
		// Token: 0x06000726 RID: 1830 RVA: 0x0002605C File Offset: 0x0002425C
		public UiState(TView view, TViewModel viewModel, string showInRegion = null)
		{
			this.view = view;
			this.viewModel = viewModel;
			if (!string.IsNullOrWhiteSpace(showInRegion))
			{
				RegionAttribute regionAttribute = this.GetRegionAttribute();
				if (!regionAttribute.Names.Contains(showInRegion))
				{
					string message = string.Format(LocalizationManager.GetTranslation("MissingAttributeExceptionMessage"), showInRegion, view);
					throw new NotImplementedException(message);
				}
				this.showInRegion = showInRegion;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x000260D0 File Offset: 0x000242D0
		protected TView View
		{
			get
			{
				return this.view;
			}
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00026104 File Offset: 0x00024304
		public override void Start()
		{
			TView tview = this.view;
			tview.DataContext = this.viewModel;
			this.VisibleRegions.ForEach(delegate(string region)
			{
				RegionManager.Instance.ShowRegion(region);
			});
			this.InvisibleRegions.ForEach(delegate(string region)
			{
				RegionManager.Instance.HideRegion(region);
			});
			this.ShowView();
			base.Start();
			TViewModel tviewModel = this.viewModel;
			tviewModel.IsStarted = true;
			tviewModel = this.viewModel;
			tviewModel.OnStarted();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000261C4 File Offset: 0x000243C4
		public override void Stop()
		{
			TViewModel tviewModel = this.viewModel;
			tviewModel.IsStarted = false;
			tviewModel = this.viewModel;
			tviewModel.OnStopped();
			base.Stop();
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00026204 File Offset: 0x00024404
		private void ShowView()
		{
			RegionAttribute regionAttribute = this.GetRegionAttribute();
			string regionName = string.IsNullOrWhiteSpace(this.showInRegion) ? regionAttribute.Names[0] : this.showInRegion;
			RegionManager.Instance.ShowView(regionName, this.view);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00026254 File Offset: 0x00024454
		private RegionAttribute GetRegionAttribute()
		{
			RegionAttribute attribute = this.view.GetAttribute<RegionAttribute>();
			if (attribute == null)
			{
				string format = "The class {0} should have RegionAttribute. Please correct it.";
				TView tview = this.view;
				throw new NotImplementedException(string.Format(format, tview.GetType().Name));
			}
			return attribute;
		}

		// Token: 0x04000335 RID: 821
		private readonly TView view;

		// Token: 0x04000336 RID: 822
		private readonly TViewModel viewModel;

		// Token: 0x04000337 RID: 823
		private readonly string showInRegion;
	}
}
