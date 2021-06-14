using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Converters
{
	// Token: 0x0200001A RID: 26
	[ValueConversion(typeof(string), typeof(Visibility))]
	public sealed class StringNullOrEmptyToVisibilityConverter : IValueConverter
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00003B1B File Offset: 0x00001D1B
		public StringNullOrEmptyToVisibilityConverter()
		{
			this.Collapse = true;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003B30 File Offset: 0x00001D30
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00003B47 File Offset: 0x00001D47
		public bool Collapse { get; set; }

		// Token: 0x0600008F RID: 143 RVA: 0x00003B50 File Offset: 0x00001D50
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string value2 = value as string;
			Visibility visibility;
			if (!string.IsNullOrEmpty(value2))
			{
				visibility = Visibility.Visible;
			}
			else
			{
				visibility = (this.Collapse ? Visibility.Collapsed : Visibility.Hidden);
			}
			return visibility;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003B90 File Offset: 0x00001D90
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
