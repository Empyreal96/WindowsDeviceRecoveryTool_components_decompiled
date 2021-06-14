using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000011 RID: 17
	public class EqualToVisibilityConverter : IValueConverter
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00003798 File Offset: 0x00001998
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (parameter != null && parameter.Equals(value)) ? Visibility.Visible : Visibility.Collapsed;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000037C0 File Offset: 0x000019C0
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
