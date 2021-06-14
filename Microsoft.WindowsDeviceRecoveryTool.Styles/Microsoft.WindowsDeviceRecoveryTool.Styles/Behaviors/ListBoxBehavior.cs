using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Behaviors
{
	// Token: 0x02000009 RID: 9
	public sealed class ListBoxBehavior
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00002CCC File Offset: 0x00000ECC
		public static bool GetAllowUnselect(ListBox listBox)
		{
			return (bool)listBox.GetValue(ListBoxBehavior.AllowUnselectProperty);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002CEE File Offset: 0x00000EEE
		public static void SetAllowUnselect(ListBox listBox, bool value)
		{
			listBox.SetValue(ListBoxBehavior.AllowUnselectProperty, value);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002D04 File Offset: 0x00000F04
		private static void AllowUnselect_OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			ListBox listBox = obj as ListBox;
			if (listBox != null)
			{
				if (!(bool)args.NewValue)
				{
					listBox.SelectionChanged += ListBoxBehavior.ListBox_OnSelectionChanged;
				}
				else
				{
					listBox.SelectionChanged -= ListBoxBehavior.ListBox_OnSelectionChanged;
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D68 File Offset: 0x00000F68
		private static void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox listBox = (ListBox)sender;
			if (listBox.SelectionMode == SelectionMode.Single && e.RemovedItems.Count >= 1 && e.AddedItems.Count <= 0)
			{
				listBox.SelectedItem = e.RemovedItems[0];
			}
		}

		// Token: 0x04000010 RID: 16
		public static readonly DependencyProperty AllowUnselectProperty = DependencyProperty.RegisterAttached("AllowUnselect", typeof(bool), typeof(ListBoxBehavior), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(ListBoxBehavior.AllowUnselect_OnPropertyChanged)));
	}
}
