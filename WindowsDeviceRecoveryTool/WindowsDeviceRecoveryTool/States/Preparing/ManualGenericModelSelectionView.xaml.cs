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
	// Token: 0x0200009C RID: 156
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class ManualGenericModelSelectionView : Grid
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x0001493F File Offset: 0x00012B3F
		public ManualGenericModelSelectionView()
		{
			this.InitializeComponent();
			this.selectedItem = null;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00014958 File Offset: 0x00012B58
		private void ManufacturersListBoxOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox != null)
			{
				FrameworkElement frameworkElement = listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem) as FrameworkElement;
				if (frameworkElement != null && this.selectedItem != null && object.Equals(frameworkElement, this.selectedItem))
				{
					ManualGenericModelSelectionViewModel manualGenericModelSelectionViewModel = (ManualGenericModelSelectionViewModel)base.DataContext;
					if (manualGenericModelSelectionViewModel.SelectTileCommand.CanExecute(null))
					{
						manualGenericModelSelectionViewModel.SelectTileCommand.Execute(this.selectedItem.DataContext);
					}
				}
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000149EC File Offset: 0x00012BEC
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

		// Token: 0x0600044B RID: 1099 RVA: 0x00014A48 File Offset: 0x00012C48
		private void ManufacturersListBoxOnFocusedItemKeyPressed(object sender, KeyEventArgs e)
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
						ManualGenericModelSelectionViewModel manualGenericModelSelectionViewModel = (ManualGenericModelSelectionViewModel)base.DataContext;
						if (manualGenericModelSelectionViewModel.SelectTileCommand.CanExecute(null))
						{
							manualGenericModelSelectionViewModel.SelectTileCommand.Execute(frameworkElement.DataContext);
						}
					}
				}
			}
		}

		// Token: 0x040001E1 RID: 481
		private FrameworkElement selectedItem;
	}
}
