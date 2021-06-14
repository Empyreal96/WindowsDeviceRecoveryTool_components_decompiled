using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x02000014 RID: 20
	public sealed class GenericBoolConverter : IValueConverter
	{
		// Token: 0x06000076 RID: 118 RVA: 0x0000385C File Offset: 0x00001A5C
		public GenericBoolConverter()
		{
			this.TrueValue = Visibility.Visible;
			this.FalseValue = Visibility.Collapsed;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003884 File Offset: 0x00001A84
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000389B File Offset: 0x00001A9B
		public object TrueValue { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000038A4 File Offset: 0x00001AA4
		// (set) Token: 0x0600007A RID: 122 RVA: 0x000038BB File Offset: 0x00001ABB
		public object FalseValue { get; set; }

		// Token: 0x0600007B RID: 123 RVA: 0x000038C4 File Offset: 0x00001AC4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (!(value is bool))
			{
				result = null;
			}
			else
			{
				result = (((bool)value) ? this.TrueValue : this.FalseValue);
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003900 File Offset: 0x00001B00
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (object.Equals(value, this.TrueValue))
			{
				result = true;
			}
			else if (object.Equals(value, this.FalseValue))
			{
				result = false;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
