using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071A RID: 1818
	internal class SourceDefaultValueConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x060074E0 RID: 29920 RVA: 0x00217192 File Offset: 0x00215392
		public SourceDefaultValueConverter(TypeConverter typeConverter, Type sourceType, Type targetType, bool shouldConvertFrom, bool shouldConvertTo, DataBindEngine engine) : base(typeConverter, sourceType, targetType, shouldConvertFrom, shouldConvertTo, engine)
		{
		}

		// Token: 0x060074E1 RID: 29921 RVA: 0x002171A3 File Offset: 0x002153A3
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertTo(o, this._targetType, parameter as DependencyObject, culture);
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x002171BA File Offset: 0x002153BA
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertFrom(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
