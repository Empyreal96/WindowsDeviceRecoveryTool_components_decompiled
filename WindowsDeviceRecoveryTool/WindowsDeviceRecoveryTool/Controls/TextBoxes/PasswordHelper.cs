using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes
{
	// Token: 0x0200003D RID: 61
	public static class PasswordHelper
	{
		// Token: 0x0600022A RID: 554 RVA: 0x0000E46F File Offset: 0x0000C66F
		public static void SetAttach(DependencyObject dp, bool value)
		{
			dp.SetValue(PasswordHelper.AttachProperty, value);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000E484 File Offset: 0x0000C684
		public static bool GetAttach(DependencyObject dp)
		{
			return (bool)dp.GetValue(PasswordHelper.AttachProperty);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000E4A8 File Offset: 0x0000C6A8
		public static string GetPassword(DependencyObject dp)
		{
			return (string)dp.GetValue(PasswordHelper.PasswordProperty);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E4CA File Offset: 0x0000C6CA
		public static void SetPassword(DependencyObject dp, string value)
		{
			dp.SetValue(PasswordHelper.PasswordProperty, value);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000E4DC File Offset: 0x0000C6DC
		private static bool GetIsUpdating(DependencyObject dp)
		{
			return (bool)dp.GetValue(PasswordHelper.IsUpdatingProperty);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E4FE File Offset: 0x0000C6FE
		private static void SetIsUpdating(DependencyObject dp, bool value)
		{
			dp.SetValue(PasswordHelper.IsUpdatingProperty, value);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000E514 File Offset: 0x0000C714
		private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)sender;
			if (sender != null && PasswordHelper.GetAttach(sender))
			{
				passwordBox.PasswordChanged -= PasswordHelper.PasswordChanged;
				if (!PasswordHelper.GetIsUpdating(passwordBox))
				{
					passwordBox.Password = (string)e.NewValue;
				}
				passwordBox.PasswordChanged += PasswordHelper.PasswordChanged;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E584 File Offset: 0x0000C784
		private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)sender;
			if (passwordBox != null)
			{
				if ((bool)e.OldValue)
				{
					passwordBox.PasswordChanged -= PasswordHelper.PasswordChanged;
				}
				if ((bool)e.NewValue)
				{
					passwordBox.PasswordChanged += PasswordHelper.PasswordChanged;
				}
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
		private static void PasswordChanged(object sender, RoutedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)sender;
			PasswordHelper.SetIsUpdating(passwordBox, true);
			PasswordHelper.SetPassword(passwordBox, passwordBox.Password);
			PasswordHelper.SetIsUpdating(passwordBox, false);
		}

		// Token: 0x040000E7 RID: 231
		public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(PasswordHelper.OnPasswordPropertyChanged)));

		// Token: 0x040000E8 RID: 232
		public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, new PropertyChangedCallback(PasswordHelper.Attach)));

		// Token: 0x040000E9 RID: 233
		private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false));
	}
}
