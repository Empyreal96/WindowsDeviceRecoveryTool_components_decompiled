using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x020000A9 RID: 169
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class DeviceSelectionView : Grid
	{
		// Token: 0x060004D2 RID: 1234 RVA: 0x00018808 File Offset: 0x00016A08
		public DeviceSelectionView()
		{
			this.InitializeComponent();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001881C File Offset: 0x00016A1C
		private void DevicesListBoxOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox != null)
			{
				FrameworkElement frameworkElement = listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem) as FrameworkElement;
				if (frameworkElement != null && this.selectedItem != null && object.Equals(frameworkElement, this.selectedItem))
				{
					DeviceSelectionViewModel deviceSelectionViewModel = (DeviceSelectionViewModel)base.DataContext;
					if (deviceSelectionViewModel.SelectTileCommand.CanExecute(this.selectedItem.DataContext))
					{
						deviceSelectionViewModel.SelectTileCommand.Execute(this.selectedItem.DataContext);
					}
				}
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000188B8 File Offset: 0x00016AB8
		private void DevicesListBoxOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
		{
			ListBox listBox = sender as ListBox;
			if (args != null && listBox != null)
			{
				DependencyObject dependencyObject = args.OriginalSource as DependencyObject;
				if (dependencyObject != null)
				{
					ListBoxItem listBoxItem = ItemsControl.ContainerFromElement(listBox, dependencyObject) as ListBoxItem;
					if (listBoxItem != null)
					{
						this.selectedItem = listBoxItem;
					}
				}
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00018914 File Offset: 0x00016B14
		private void DevicesListBoxOnFocusedItemKeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space || e.Key == Key.Return)
			{
				ListBoxItem listBoxItem = e.OriginalSource as ListBoxItem;
				if (listBoxItem != null)
				{
					DeviceSelectionViewModel deviceSelectionViewModel = (DeviceSelectionViewModel)base.DataContext;
					ICommand selectTileCommand = deviceSelectionViewModel.SelectTileCommand;
					if (selectTileCommand.CanExecute(listBoxItem.DataContext))
					{
						selectTileCommand.Execute(listBoxItem.DataContext);
					}
				}
			}
		}

		// Token: 0x04000218 RID: 536
		private FrameworkElement selectedItem;
	}
}
