using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x02000048 RID: 72
	public class RegionManager : DependencyObject
	{
		// Token: 0x06000281 RID: 641 RVA: 0x0000F4AC File Offset: 0x0000D6AC
		private RegionManager()
		{
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
		public static RegionManager Instance
		{
			get
			{
				return RegionManager.Nested.NestedInstance;
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000F4DB File Offset: 0x0000D6DB
		public static void SetRegionName(ContentControl element, string value)
		{
			element.SetValue(RegionManager.RegionNameProperty, value);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000F4EC File Offset: 0x0000D6EC
		public static string GetRegionName(ContentControl element)
		{
			return (string)element.GetValue(RegionManager.RegionNameProperty);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000F50E File Offset: 0x0000D70E
		public void ShowView(string regionName, FrameworkElement content)
		{
			this.regions[regionName].Content = content;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000F524 File Offset: 0x0000D724
		public void AddRegion(string name, ContentControl control)
		{
			this.regions.Add(name, control);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000F538 File Offset: 0x0000D738
		public ContentControl GetRegion(string name)
		{
			return this.regions[name];
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000F556 File Offset: 0x0000D756
		public void HideRegion(string name)
		{
			this.regions[name].Visibility = Visibility.Collapsed;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000F56C File Offset: 0x0000D76C
		public void RemoveRegion(string name)
		{
			this.regions.Remove(name);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000F57C File Offset: 0x0000D77C
		public void ShowRegion(string name)
		{
			this.regions[name].Visibility = Visibility.Visible;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000F594 File Offset: 0x0000D794
		private static void OnSetRegionNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentControl contentControl = d as ContentControl;
			if (contentControl != null)
			{
				RegionManager.Instance.AddRegion(e.NewValue as string, contentControl);
			}
		}

		// Token: 0x040000FB RID: 251
		private readonly IDictionary<string, ContentControl> regions = new Dictionary<string, ContentControl>();

		// Token: 0x040000FC RID: 252
		public static readonly DependencyProperty RegionNameProperty = DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(RegionManager), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(RegionManager.OnSetRegionNameChanged)));

		// Token: 0x02000049 RID: 73
		private class Nested
		{
			// Token: 0x040000FD RID: 253
			internal static readonly RegionManager NestedInstance = new RegionManager();
		}
	}
}
