using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000037 RID: 55
	public class MainAreaControl : ContentControl
	{
		// Token: 0x06000200 RID: 512 RVA: 0x0000DD24 File Offset: 0x0000BF24
		static MainAreaControl()
		{
			MainAreaControl.ContentChangedEvent = EventManager.RegisterRoutedEvent("ContentChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainAreaControl));
			ContentControl.ContentProperty.OverrideMetadata(typeof(MainAreaControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(MainAreaControl.OnContentPropertyChanged)));
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000201 RID: 513 RVA: 0x0000DD7D File Offset: 0x0000BF7D
		// (remove) Token: 0x06000202 RID: 514 RVA: 0x0000DD8D File Offset: 0x0000BF8D
		public event RoutedEventHandler ContentChanged
		{
			add
			{
				base.AddHandler(MainAreaControl.ContentChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MainAreaControl.ContentChangedEvent, value);
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MainAreaControl mainAreaControl = d as MainAreaControl;
			if (mainAreaControl != null && e.NewValue != e.OldValue)
			{
				mainAreaControl.RaiseContentChangedEvent();
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		private void RaiseContentChangedEvent()
		{
			RoutedEventArgs e = new RoutedEventArgs(MainAreaControl.ContentChangedEvent);
			base.RaiseEvent(e);
		}
	}
}
