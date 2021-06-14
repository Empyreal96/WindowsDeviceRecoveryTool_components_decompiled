using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x0200000F RID: 15
	public class BooleanToVisibilityHiddenConverter : IValueConverter
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003714 File Offset: 0x00001914
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			if (value is bool)
			{
				flag = (bool)value;
			}
			return flag ? Visibility.Visible : Visibility.Hidden;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000374E File Offset: 0x0000194E
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
