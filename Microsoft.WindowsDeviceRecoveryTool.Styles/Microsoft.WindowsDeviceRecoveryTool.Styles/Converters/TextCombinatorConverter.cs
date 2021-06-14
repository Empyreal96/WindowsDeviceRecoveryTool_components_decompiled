using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x0200001B RID: 27
	public class TextCombinatorConverter : IMultiValueConverter
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00003BC8 File Offset: 0x00001DC8
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (values == null || !values.Any<object>())
			{
				result = string.Empty;
			}
			else
			{
				object[] array = new object[values.Length - 1];
				Array.Copy(values, array, values.Length - 1);
				if (array.All((object value) => !string.IsNullOrEmpty(value as string)))
				{
					result = string.Join(string.Empty, array);
				}
				else
				{
					result = values.Last<object>();
				}
			}
			return result;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003C4D File Offset: 0x00001E4D
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
