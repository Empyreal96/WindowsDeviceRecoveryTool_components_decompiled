using System;
using System.Globalization;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Assets
{
	// Token: 0x02000004 RID: 4
	public class DictionaryStyleToStringConverter : IValueConverter
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000021A0 File Offset: 0x000003A0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return StyleLogic.GetStyle((string)value);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021C0 File Offset: 0x000003C0
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DictionaryStyle dictionaryStyle = (DictionaryStyle)value;
			return dictionaryStyle.Name;
		}
	}
}
