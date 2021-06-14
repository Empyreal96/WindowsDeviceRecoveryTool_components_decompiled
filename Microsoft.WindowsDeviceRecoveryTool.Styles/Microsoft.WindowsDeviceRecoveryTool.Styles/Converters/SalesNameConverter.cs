using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000012 RID: 18
	public class SalesNameConverter : IValueConverter
	{
		// Token: 0x06000070 RID: 112 RVA: 0x000037D0 File Offset: 0x000019D0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (text != null)
			{
				if (text == "DeviceInUefiMode")
				{
					return LocalizationManager.GetTranslation("DeviceInUefiMode");
				}
			}
			return value;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003814 File Offset: 0x00001A14
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
