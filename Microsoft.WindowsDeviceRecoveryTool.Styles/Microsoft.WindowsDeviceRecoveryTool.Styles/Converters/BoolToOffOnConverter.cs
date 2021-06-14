using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000015 RID: 21
	public class BoolToOffOnConverter : IValueConverter
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00003950 File Offset: 0x00001B50
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = string.Empty;
			if (value is bool)
			{
				result = (((bool)value) ? LocalizationManager.GetTranslation("ButtonOn") : LocalizationManager.GetTranslation("ButtonOff"));
			}
			return result;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000399D File Offset: 0x00001B9D
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
