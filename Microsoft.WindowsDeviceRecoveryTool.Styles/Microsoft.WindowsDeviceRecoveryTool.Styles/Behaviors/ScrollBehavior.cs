using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Behaviors
{
	// Token: 0x0200000B RID: 11
	public static class ScrollBehavior
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002E08 File Offset: 0x00001008
		public static bool GetResetScrollOnItemsChanged(DependencyObject obj)
		{
			return (bool)obj.GetValue(ScrollBehavior.ResetScrollOnItemsChangedProperty);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E2A File Offset: 0x0000102A
		public static void SetResetScrollOnItemsChanged(DependencyObject obj, bool value)
		{
			obj.SetValue(ScrollBehavior.ResetScrollOnItemsChangedProperty, value);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002E3F File Offset: 0x0000103F
		public static void SetMouseWheelScrollValue(DependencyObject element, int value)
		{
			element.SetValue(ScrollBehavior.MouseWheelScrollValueProperty, value);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002E54 File Offset: 0x00001054
		public static double GetMouseWheelScrollValue(DependencyObject element)
		{
			return (double)element.GetValue(ScrollBehavior.MouseWheelScrollValueProperty);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002E76 File Offset: 0x00001076
		public static void SetScrollWithChildren(DependencyObject element, bool value)
		{
			element.SetValue(ScrollBehavior.ScrollWithChildrenProperty, value);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002E8C File Offset: 0x0000108C
		public static bool GetScrollWithChildren(DependencyObject element)
		{
			return (bool)element.GetValue(ScrollBehavior.ScrollWithChildrenProperty);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002EAE File Offset: 0x000010AE
		public static void SetHorizontalScroll(ScrollViewer scrollViewer, bool value)
		{
			scrollViewer.SetValue(ScrollBehavior.HorizontalScrollProperty, value);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002EC4 File Offset: 0x000010C4
		public static bool GetHorizontalScroll(ScrollViewer scrollViewer)
		{
			return (bool)scrollViewer.GetValue(ScrollBehavior.HorizontalScrollProperty);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002EE8 File Offset: 0x000010E8
		private static void HorizontalScrollOnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			ScrollViewer scrollViewer = dependencyObject as ScrollViewer;
			if (scrollViewer != null)
			{
				if ((bool)eventArgs.NewValue)
				{
					scrollViewer.PreviewMouseWheel += ScrollBehavior.HorizontalScrollScrollViewerOnPreviewMouseWheel;
				}
				else
				{
					scrollViewer.PreviewMouseWheel -= ScrollBehavior.HorizontalScrollScrollViewerOnPreviewMouseWheel;
				}
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002F4C File Offset: 0x0000114C
		private static void HorizontalScrollScrollViewerOnPreviewMouseWheel(object sender, MouseWheelEventArgs eventArgs)
		{
			ScrollViewer scrollViewer = (ScrollViewer)sender;
			if (scrollViewer.VerticalScrollBarVisibility == ScrollBarVisibility.Disabled)
			{
				if (scrollViewer.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto || scrollViewer.HorizontalScrollBarVisibility == ScrollBarVisibility.Visible)
				{
					int num = eventArgs.Delta;
					if (scrollViewer.GetValue(ScrollBehavior.MouseWheelScrollValueProperty) != DependencyProperty.UnsetValue)
					{
						int num2 = Math.Sign(num);
						num = num2 * (int)scrollViewer.GetValue(ScrollBehavior.MouseWheelScrollValueProperty);
						if (num == 0)
						{
							num = eventArgs.Delta;
						}
					}
					scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - (double)num);
					eventArgs.Handled = true;
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002FF8 File Offset: 0x000011F8
		private static void ScrollWithChildrenPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			ScrollViewer scrollViewer = dependencyObject as ScrollViewer;
			if (scrollViewer != null)
			{
				if ((bool)eventArgs.NewValue)
				{
					scrollViewer.PreviewMouseWheel += ScrollBehavior.ScrollWithChildrenScrollViewerOnPreviewMouseWheel;
				}
				else
				{
					scrollViewer.PreviewMouseWheel -= ScrollBehavior.ScrollWithChildrenScrollViewerOnPreviewMouseWheel;
				}
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000305C File Offset: 0x0000125C
		private static void ScrollWithChildrenScrollViewerOnPreviewMouseWheel(object sender, MouseWheelEventArgs eventArgs)
		{
			ScrollViewer scrollViewer = (ScrollViewer)sender;
			if (scrollViewer.HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled)
			{
				scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - (double)eventArgs.Delta);
			}
			else
			{
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (double)eventArgs.Delta);
			}
			eventArgs.Handled = true;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000030B8 File Offset: 0x000012B8
		private static void OnResetScrollOnItemsChangedPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = dpo as ItemsControl;
			if (itemsControl != null)
			{
				DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));
				if (dependencyPropertyDescriptor != null)
				{
					if ((bool)e.NewValue)
					{
						dependencyPropertyDescriptor.AddValueChanged(itemsControl, new EventHandler(ScrollBehavior.ItemsSourceChanged));
					}
					else
					{
						dependencyPropertyDescriptor.RemoveValueChanged(itemsControl, new EventHandler(ScrollBehavior.ItemsSourceChanged));
					}
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003138 File Offset: 0x00001338
		private static void ItemsSourceChanged(object sender, EventArgs e)
		{
			ItemsControl itemsControl = sender as ItemsControl;
			if (itemsControl != null)
			{
				if (itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
				{
					ScrollViewer visualChild = ScrollBehavior.GetVisualChild<ScrollViewer>(itemsControl);
					visualChild.ScrollToHome();
				}
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003180 File Offset: 0x00001380
		private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
		{
			T t = default(T);
			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual visual = (Visual)VisualTreeHelper.GetChild(parent, i);
				t = (visual as T);
				if (t == null)
				{
					t = ScrollBehavior.GetVisualChild<T>(visual);
				}
				if (t != null)
				{
					break;
				}
			}
			return t;
		}

		// Token: 0x04000014 RID: 20
		public static readonly DependencyProperty HorizontalScrollProperty = DependencyProperty.RegisterAttached("HorizontalScroll", typeof(bool), typeof(ScrollBehavior), new PropertyMetadata(false, new PropertyChangedCallback(ScrollBehavior.HorizontalScrollOnPropertyChanged)));

		// Token: 0x04000015 RID: 21
		public static readonly DependencyProperty ScrollWithChildrenProperty = DependencyProperty.RegisterAttached("ScrollWithChildren", typeof(bool), typeof(ScrollBehavior), new PropertyMetadata(false, new PropertyChangedCallback(ScrollBehavior.ScrollWithChildrenPropertyChangedCallback)));

		// Token: 0x04000016 RID: 22
		public static readonly DependencyProperty MouseWheelScrollValueProperty = DependencyProperty.RegisterAttached("MouseWheelScrollValue", typeof(int), typeof(ScrollBehavior), new PropertyMetadata(0));

		// Token: 0x04000017 RID: 23
		public static readonly DependencyProperty ResetScrollOnItemsChangedProperty = DependencyProperty.RegisterAttached("ResetScrollOnItemsChanged", typeof(bool), typeof(ScrollBehavior), new UIPropertyMetadata(false, new PropertyChangedCallback(ScrollBehavior.OnResetScrollOnItemsChangedPropertyChanged)));
	}
}
