using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000017 RID: 23
	public class InverseBooleanConverter : IValueConverter
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00003A20 File Offset: 0x00001C20
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			if (value is bool)
			{
				flag = (bool)value;
			}
			return !flag;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003A56 File Offset: 0x00001C56
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
