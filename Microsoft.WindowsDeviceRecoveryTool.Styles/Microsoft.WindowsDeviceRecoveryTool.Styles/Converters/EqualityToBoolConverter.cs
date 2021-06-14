using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000016 RID: 22
	public class EqualityToBoolConverter : IValueConverter
	{
		// Token: 0x06000080 RID: 128 RVA: 0x000039B0 File Offset: 0x00001BB0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return parameter != null && parameter.Equals(value);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000039D8 File Offset: 0x00001BD8
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as bool? == true) ? parameter : DependencyProperty.UnsetValue;
		}
	}
}
