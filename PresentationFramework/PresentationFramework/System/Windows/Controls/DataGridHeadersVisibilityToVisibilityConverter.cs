using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x020004AB RID: 1195
	[Localizability(LocalizationCategory.NeverLocalize)]
	internal sealed class DataGridHeadersVisibilityToVisibilityConverter : IValueConverter
	{
		// Token: 0x060048C3 RID: 18627 RVA: 0x0014A848 File Offset: 0x00148A48
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			if (value is DataGridHeadersVisibility && parameter is DataGridHeadersVisibility)
			{
				DataGridHeadersVisibility dataGridHeadersVisibility = (DataGridHeadersVisibility)value;
				DataGridHeadersVisibility dataGridHeadersVisibility2 = (DataGridHeadersVisibility)parameter;
				switch (dataGridHeadersVisibility)
				{
				case DataGridHeadersVisibility.Column:
					flag = (dataGridHeadersVisibility2 == DataGridHeadersVisibility.Column || dataGridHeadersVisibility2 == DataGridHeadersVisibility.None);
					break;
				case DataGridHeadersVisibility.Row:
					flag = (dataGridHeadersVisibility2 == DataGridHeadersVisibility.Row || dataGridHeadersVisibility2 == DataGridHeadersVisibility.None);
					break;
				case DataGridHeadersVisibility.All:
					flag = true;
					break;
				}
			}
			if (targetType == typeof(Visibility))
			{
				return flag ? Visibility.Visible : Visibility.Collapsed;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x0003E384 File Offset: 0x0003C584
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
