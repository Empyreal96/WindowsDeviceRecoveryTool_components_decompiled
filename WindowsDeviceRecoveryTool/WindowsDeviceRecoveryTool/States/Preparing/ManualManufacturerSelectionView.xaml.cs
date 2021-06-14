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
	// Token: 0x020000CA RID: 202
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class ManualManufacturerSelectionView : Grid
	{
		// Token: 0x06000624 RID: 1572 RVA: 0x0001FD97 File Offset: 0x0001DF97
		public ManualManufacturerSelectionView()
		{
			this.InitializeComponent();
			this.selectedItem = null;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001FDB0 File Offset: 0x0001DFB0
		private void ManufacturersListBoxOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox != null)
			{
				FrameworkElement frameworkElement = listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem) as FrameworkElement;
				if (frameworkElement != null && this.selectedItem != null && object.Equals(frameworkElement, this.selectedItem))
				{
					ManualManufacturerSelectionViewModel manualManufacturerSelectionViewModel = (ManualManufacturerSelectionViewModel)base.DataContext;
					if (manualManufacturerSelectionViewModel.SelectTileCommand.CanExecute(null))
					{
						manualManufacturerSelectionViewModel.SelectTileCommand.Execute(this.selectedItem.DataContext);
					}
				}
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001FE44 File Offset: 0x0001E044
		private void ManufacturersListBoxOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
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

		// Token: 0x06000627 RID: 1575 RVA: 0x0001FEA0 File Offset: 0x0001E0A0
		private void ManufacturersListBoxOnFocusedItemKeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space || e.Key == Key.Return)
			{
				ListBoxItem listBoxItem = e.OriginalSource as ListBoxItem;
				if (listBoxItem != null)
				{
					ManualManufacturerSelectionViewModel manualManufacturerSelectionViewModel = (ManualManufacturerSelectionViewModel)base.DataContext;
					if (manualManufacturerSelectionViewModel.SelectTileCommand.CanExecute(null))
					{
						manualManufacturerSelectionViewModel.SelectTileCommand.Execute(listBoxItem.DataContext);
					}
				}
			}
		}

		// Token: 0x04000298 RID: 664
		private FrameworkElement selectedItem;
	}
}
