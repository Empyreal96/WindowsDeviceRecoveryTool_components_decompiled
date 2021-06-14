using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x02000470 RID: 1136
	[Localizability(LocalizationCategory.NeverLocalize)]
	internal sealed class BooleanToSelectiveScrollingOrientationConverter : IValueConverter
	{
		// Token: 0x06004254 RID: 16980 RVA: 0x0012F75C File Offset: 0x0012D95C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && parameter is SelectiveScrollingOrientation)
			{
				bool flag = (bool)value;
				SelectiveScrollingOrientation selectiveScrollingOrientation = (SelectiveScrollingOrientation)parameter;
				if (flag)
				{
					return selectiveScrollingOrientation;
				}
			}
			return SelectiveScrollingOrientation.Both;
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x0003E384 File Offset: 0x0003C584
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
