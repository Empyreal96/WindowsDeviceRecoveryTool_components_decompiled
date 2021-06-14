using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000019 RID: 25
	public class ObjectNullToVisibilityConverter : IValueConverter
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00003AEC File Offset: 0x00001CEC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value == null) ? Visibility.Collapsed : Visibility.Visible;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003B0B File Offset: 0x00001D0B
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
