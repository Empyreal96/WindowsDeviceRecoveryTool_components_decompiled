using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000018 RID: 24
	public class InvertedBooleanToVisibilityConverter : IValueConverter
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00003A68 File Offset: 0x00001C68
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			if (value is bool)
			{
				flag = (bool)value;
			}
			return flag ? Visibility.Collapsed : Visibility.Visible;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003AA4 File Offset: 0x00001CA4
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (value is Visibility)
			{
				result = ((Visibility)value != Visibility.Visible);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
