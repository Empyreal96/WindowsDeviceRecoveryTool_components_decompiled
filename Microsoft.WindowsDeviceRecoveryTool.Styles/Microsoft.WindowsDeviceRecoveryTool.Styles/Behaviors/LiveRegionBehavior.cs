using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.Common;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Behaviors
{
	// Token: 0x0200000C RID: 12
	public sealed class LiveRegionBehavior
	{
		// Token: 0x0600004E RID: 78 RVA: 0x000032EC File Offset: 0x000014EC
		private LiveRegionBehavior(FrameworkElement element)
		{
			this.element = element;
			LiveRegionBehavior.AddLiveRegionChangedHandler(element, new RoutedEventHandler(this.FrameworkElement_OnLiveRegionChanged));
			element.Loaded += this.FrameworkElement_OnLoaded;
			element.DataContextChanged += this.FrameworkElement_OnDataContextChanged;
			INotifyLiveRegionChanged notifyLiveRegionChanged = element.DataContext as INotifyLiveRegionChanged;
			if (notifyLiveRegionChanged != null)
			{
				notifyLiveRegionChanged.LiveRegionChanged += this.DataContext_OnLiveRegionChanged;
			}
			this.LiveSetting = (LiveSetting)LiveRegionBehavior.LiveSettingProperty.DefaultMetadata.DefaultValue;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003388 File Offset: 0x00001588
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000339F File Offset: 0x0000159F
		public LiveSetting LiveSetting { get; set; }

		// Token: 0x06000051 RID: 81 RVA: 0x000033A8 File Offset: 0x000015A8
		public static LiveSetting GetLiveSetting(FrameworkElement element)
		{
			return (LiveSetting)element.GetValue(LiveRegionBehavior.LiveSettingProperty);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000033CA File Offset: 0x000015CA
		public static void SetLiveSetting(FrameworkElement element, LiveSetting value)
		{
			element.SetValue(LiveRegionBehavior.LiveSettingProperty, value);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000033E0 File Offset: 0x000015E0
		public static void RaiseLiveRegionChanged(FrameworkElement element)
		{
			RoutedEventArgs e = new RoutedEventArgs
			{
				RoutedEvent = LiveRegionBehavior.LiveRegionChangedEvent
			};
			element.RaiseEvent(e);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000340A File Offset: 0x0000160A
		public static void AddLiveRegionChangedHandler(FrameworkElement element, RoutedEventHandler handler)
		{
			element.AddHandler(LiveRegionBehavior.LiveRegionChangedEvent, handler);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000341A File Offset: 0x0000161A
		public static void RemoveLiveRegionChangedHandler(FrameworkElement element, RoutedEventHandler handler)
		{
			element.RemoveHandler(LiveRegionBehavior.LiveRegionChangedEvent, handler);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000342A File Offset: 0x0000162A
		private static void SetBehavior(FrameworkElement element, LiveRegionBehavior value)
		{
			element.SetValue(LiveRegionBehavior.BehaviorProperty, value);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000343C File Offset: 0x0000163C
		private static LiveRegionBehavior GetBehavior(FrameworkElement element)
		{
			return (LiveRegionBehavior)element.GetValue(LiveRegionBehavior.BehaviorProperty);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003460 File Offset: 0x00001660
		private static void LiveSetting_OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			FrameworkElement frameworkElement = obj as FrameworkElement;
			if (frameworkElement != null)
			{
				LiveRegionBehavior liveRegionBehavior = LiveRegionBehavior.GetBehavior(frameworkElement);
				if (liveRegionBehavior == null)
				{
					liveRegionBehavior = new LiveRegionBehavior(frameworkElement);
					LiveRegionBehavior.SetBehavior(frameworkElement, liveRegionBehavior);
				}
				liveRegionBehavior.LiveSetting = (LiveSetting)args.NewValue;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000034BC File Offset: 0x000016BC
		private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
		{
			if (this.isNotificationPending)
			{
				this.NotifyLiveRegionChanged();
				this.isNotificationPending = false;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000034E8 File Offset: 0x000016E8
		private void FrameworkElement_OnLiveRegionChanged(object sender, RoutedEventArgs e)
		{
			if (this.element.IsLoaded)
			{
				this.NotifyLiveRegionChanged();
			}
			else
			{
				this.isNotificationPending = true;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000351C File Offset: 0x0000171C
		private void FrameworkElement_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			INotifyLiveRegionChanged notifyLiveRegionChanged = e.OldValue as INotifyLiveRegionChanged;
			INotifyLiveRegionChanged notifyLiveRegionChanged2 = e.NewValue as INotifyLiveRegionChanged;
			if (notifyLiveRegionChanged != null)
			{
				notifyLiveRegionChanged.LiveRegionChanged -= this.DataContext_OnLiveRegionChanged;
			}
			if (notifyLiveRegionChanged2 != null)
			{
				notifyLiveRegionChanged2.LiveRegionChanged += this.DataContext_OnLiveRegionChanged;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000357E File Offset: 0x0000177E
		private void DataContext_OnLiveRegionChanged(object sender, EventArgs e)
		{
			LiveRegionBehavior.RaiseLiveRegionChanged(this.element);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003590 File Offset: 0x00001790
		private void NotifyLiveRegionChanged()
		{
			switch (this.LiveSetting)
			{
			case LiveSetting.Assertive:
				this.ResetKeyboardFocus();
				break;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000035C8 File Offset: 0x000017C8
		private void ResetKeyboardFocus()
		{
			if (this.element.IsKeyboardFocused)
			{
				Keyboard.ClearFocus();
			}
			Keyboard.Focus(this.element);
		}

		// Token: 0x04000018 RID: 24
		public static readonly DependencyProperty LiveSettingProperty = DependencyProperty.RegisterAttached("LiveSetting", typeof(LiveSetting), typeof(LiveRegionBehavior), new FrameworkPropertyMetadata(LiveSetting.Off, new PropertyChangedCallback(LiveRegionBehavior.LiveSetting_OnPropertyChanged)));

		// Token: 0x04000019 RID: 25
		public static readonly RoutedEvent LiveRegionChangedEvent = EventManager.RegisterRoutedEvent("LiveRegionChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(LiveRegionBehavior));

		// Token: 0x0400001A RID: 26
		private static readonly DependencyProperty BehaviorProperty = DependencyProperty.RegisterAttached("Behavior", typeof(LiveRegionBehavior), typeof(LiveRegionBehavior), new FrameworkPropertyMetadata(null));

		// Token: 0x0400001B RID: 27
		private readonly FrameworkElement element;

		// Token: 0x0400001C RID: 28
		private bool isNotificationPending;
	}
}
