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
	// Token: 0x020000A0 RID: 160
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class ManualGenericVariantSelectionView : Grid
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x000157A0 File Offset: 0x000139A0
		public ManualGenericVariantSelectionView()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000157B4 File Offset: 0x000139B4
		private void DevicesListBoxOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox != null)
			{
				FrameworkElement frameworkElement = listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem) as FrameworkElement;
				if (frameworkElement != null && this.selectedItem != null && object.Equals(frameworkElement, this.selectedItem))
				{
					ManualGenericVariantSelectionViewModel manualGenericVariantSelectionViewModel = (ManualGenericVariantSelectionViewModel)base.DataContext;
					if (manualGenericVariantSelectionViewModel.SelectTileCommand.CanExecute(null))
					{
						manualGenericVariantSelectionViewModel.SelectTileCommand.Execute(this.selectedItem.DataContext);
					}
				}
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00015848 File Offset: 0x00013A48
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

		// Token: 0x06000475 RID: 1141 RVA: 0x000158A4 File Offset: 0x00013AA4
		private void DevicesListBoxOnFocusedItemKeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space || e.Key == Key.Return)
			{
				ListBox listBox = sender as ListBox;
				if (listBox != null)
				{
					FrameworkElement frameworkElement = listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem) as FrameworkElement;
					DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
					if (frameworkElement != null && dependencyObject != null && object.Equals(frameworkElement, dependencyObject))
					{
						ManualGenericVariantSelectionViewModel manualGenericVariantSelectionViewModel = (ManualGenericVariantSelectionViewModel)base.DataContext;
						if (manualGenericVariantSelectionViewModel.SelectTileCommand.CanExecute(null))
						{
							manualGenericVariantSelectionViewModel.SelectTileCommand.Execute(frameworkElement.DataContext);
						}
					}
				}
			}
		}

		// Token: 0x040001F2 RID: 498
		private FrameworkElement selectedItem;
	}
}
