using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071B RID: 1819
	internal class TargetDefaultValueConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x060074E3 RID: 29923 RVA: 0x00217192 File Offset: 0x00215392
		public TargetDefaultValueConverter(TypeConverter typeConverter, Type sourceType, Type targetType, bool shouldConvertFrom, bool shouldConvertTo, DataBindEngine engine) : base(typeConverter, sourceType, targetType, shouldConvertFrom, shouldConvertTo, engine)
		{
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x002171D1 File Offset: 0x002153D1
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertFrom(o, this._targetType, parameter as DependencyObject, culture);
		}

		// Token: 0x060074E5 RID: 29925 RVA: 0x002171E8 File Offset: 0x002153E8
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertTo(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
