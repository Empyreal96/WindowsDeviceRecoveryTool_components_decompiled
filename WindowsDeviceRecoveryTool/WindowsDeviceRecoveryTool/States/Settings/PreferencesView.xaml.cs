using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Styles.Assets;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000D8 RID: 216
	[Export]
	[Region(new string[]
	{
		"SettingsMainArea"
	})]
	public partial class PreferencesView : StackPanel
	{
		// Token: 0x060006A8 RID: 1704 RVA: 0x00022678 File Offset: 0x00020878
		public PreferencesView()
		{
			this.InitializeComponent();
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00022984 File Offset: 0x00020B84
		private static IEnumerable<T> FindVisualChildrenWithName<T>(DependencyObject depObj, string name) where T : DependencyObject
		{
			if (depObj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					FrameworkElement frameworkElement = child as FrameworkElement;
					if (frameworkElement != null && child is T && frameworkElement.Name == name)
					{
						yield return (T)((object)child);
					}
					foreach (T childOfChild in PreferencesView.FindVisualChildrenWithName<T>(child, name))
					{
						yield return childOfChild;
					}
				}
			}
			yield break;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00022C58 File Offset: 0x00020E58
		private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if (child is T)
					{
						yield return (T)((object)child);
					}
					foreach (T childOfChild in PreferencesView.FindVisualChildren<T>(child))
					{
						yield return childOfChild;
					}
				}
			}
			yield break;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00022C7C File Offset: 0x00020E7C
		private void OnLanguagesComboBoxPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.IsRepeat)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00022CA4 File Offset: 0x00020EA4
		private void OnLanguagesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ThemeStyle themeStyle = this.ThemesComboBox.SelectedItem as ThemeStyle;
			if (themeStyle != null)
			{
				this.UpdateComboBoxText(this.ThemesComboBox, themeStyle.LocalizedName);
			}
			DictionaryStyle dictionaryStyle = this.ColorsComboBox.SelectedItem as DictionaryStyle;
			if (dictionaryStyle != null)
			{
				this.UpdateComboBoxText(this.ColorsComboBox, dictionaryStyle.LocalizedName);
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00022D10 File Offset: 0x00020F10
		private void UpdateComboBoxText(ComboBox comboBox, string text)
		{
			IEnumerable<ContentPresenter> enumerable = PreferencesView.FindVisualChildrenWithName<ContentPresenter>(comboBox, "ContentPresenter");
			IList<ContentPresenter> source = (enumerable as IList<ContentPresenter>) ?? enumerable.ToList<ContentPresenter>();
			if (source.FirstOrDefault<ContentPresenter>() != null)
			{
				IEnumerable<TextBlock> enumerable2 = PreferencesView.FindVisualChildren<TextBlock>(source.FirstOrDefault<ContentPresenter>());
				TextBlock[] source2 = (enumerable2 as TextBlock[]) ?? enumerable2.ToArray<TextBlock>();
				if (source2.FirstOrDefault<TextBlock>() != null)
				{
					comboBox.Text = text;
					source2.First<TextBlock>().Text = text;
				}
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00022D8F File Offset: 0x00020F8F
		private void OnStyleComboBoxSelectionChanged(object sender, SelectionChangedEventArgs args)
		{
			this.lastFocusedElement = Keyboard.FocusedElement;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00022DA0 File Offset: 0x00020FA0
		private void OnPreferencesViewLoaded(object sender, RoutedEventArgs args)
		{
			if (this.lastFocusedElement != null)
			{
				Keyboard.Focus(this.lastFocusedElement);
				this.lastFocusedElement = null;
			}
		}

		// Token: 0x040002C8 RID: 712
		private IInputElement lastFocusedElement;
	}
}
