using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000010 RID: 16
	public class NotEqualToVisibilityConverter : IValueConverter
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003760 File Offset: 0x00001960
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (parameter != null && !parameter.Equals(value)) ? Visibility.Visible : Visibility.Collapsed;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003788 File Offset: 0x00001988
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
