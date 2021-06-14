using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000013 RID: 19
	public class NotEqualToBoolConverter : IValueConverter
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00003824 File Offset: 0x00001A24
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return parameter != null && !parameter.Equals(value);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000384C File Offset: 0x00001A4C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
