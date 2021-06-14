using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Behaviors
{
	// Token: 0x02000008 RID: 8
	public static class ComboBoxBehavior
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002A00 File Offset: 0x00000C00
		public static bool GetOpenDropDownOnEnter(ComboBox comboBox)
		{
			return (bool)comboBox.GetValue(ComboBoxBehavior.OpenDropDownOnEnterProperty);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002A22 File Offset: 0x00000C22
		public static void SetOpenDropDownOnEnter(ComboBox comboBox, bool value)
		{
			comboBox.SetValue(ComboBoxBehavior.OpenDropDownOnEnterProperty, value);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A38 File Offset: 0x00000C38
		public static bool GetMoveFocusOnTab(ComboBox comboBox)
		{
			return (bool)comboBox.GetValue(ComboBoxBehavior.MoveFocusOnTabProperty);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002A5A File Offset: 0x00000C5A
		public static void SetMoveFocusOnTab(ComboBox comboBox, bool value)
		{
			comboBox.SetValue(ComboBoxBehavior.MoveFocusOnTabProperty, value);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002A70 File Offset: 0x00000C70
		private static void OpenDropDownOnEnter_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			ComboBox comboBox = obj as ComboBox;
			if (comboBox != null)
			{
				bool flag = (bool)args.NewValue;
				if (flag)
				{
					comboBox.PreviewKeyDown += ComboBoxBehavior.OpenDropDownOnEnter_ComboBox_PreviewKeyDown;
				}
				else
				{
					comboBox.PreviewKeyDown -= ComboBoxBehavior.OpenDropDownOnEnter_ComboBox_PreviewKeyDown;
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002AD4 File Offset: 0x00000CD4
		private static void MoveFocusOnTab_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			ComboBox comboBox = obj as ComboBox;
			if (comboBox != null)
			{
				bool flag = (bool)args.NewValue;
				if (flag)
				{
					comboBox.KeyDown += ComboBoxBehavior.MoveFocusOnTab_ComboBox_KeyDown;
				}
				else
				{
					comboBox.KeyDown -= ComboBoxBehavior.MoveFocusOnTab_ComboBox_KeyDown;
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B38 File Offset: 0x00000D38
		private static void OpenDropDownOnEnter_ComboBox_PreviewKeyDown(object sender, KeyEventArgs args)
		{
			if (args.Key == Key.Return)
			{
				ComboBox comboBox = (ComboBox)sender;
				if (comboBox.IsKeyboardFocused)
				{
					comboBox.IsDropDownOpen = true;
					args.Handled = true;
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002B7C File Offset: 0x00000D7C
		private static void MoveFocusOnTab_ComboBox_KeyDown(object sender, KeyEventArgs args)
		{
			if (args.Key == Key.Tab)
			{
				ComboBox comboBox = (ComboBox)sender;
				if (comboBox.IsDropDownOpen && !comboBox.IsEditable)
				{
					ComboBoxItem comboBoxItem = args.OriginalSource as ComboBoxItem;
					if (comboBoxItem != null)
					{
						ItemContainerGenerator itemContainerGenerator = comboBox.ItemContainerGenerator;
						int selectedIndex = itemContainerGenerator.IndexFromContainer(comboBoxItem);
						KeyboardDevice keyboardDevice = args.KeyboardDevice;
						ModifierKeys modifiers = keyboardDevice.Modifiers;
						FocusNavigationDirection focusNavigationDirection = (!modifiers.HasFlag(ModifierKeys.Shift)) ? FocusNavigationDirection.Next : FocusNavigationDirection.Previous;
						TraversalRequest request = new TraversalRequest(focusNavigationDirection);
						comboBox.MoveFocus(request);
						comboBox.SelectedIndex = selectedIndex;
						comboBox.IsDropDownOpen = false;
						args.Handled = true;
					}
				}
			}
		}

		// Token: 0x0400000E RID: 14
		public static readonly DependencyProperty OpenDropDownOnEnterProperty = DependencyProperty.RegisterAttached("OpenDropDownOnEnter", typeof(bool), typeof(ComboBoxBehavior), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ComboBoxBehavior.OpenDropDownOnEnter_PropertyChanged)));

		// Token: 0x0400000F RID: 15
		public static readonly DependencyProperty MoveFocusOnTabProperty = DependencyProperty.RegisterAttached("MoveFocusOnTab", typeof(bool), typeof(ComboBoxBehavior), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ComboBoxBehavior.MoveFocusOnTab_PropertyChanged)));
	}
}
