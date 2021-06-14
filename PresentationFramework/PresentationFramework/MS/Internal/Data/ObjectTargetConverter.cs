using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071D RID: 1821
	internal class ObjectTargetConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x060074EC RID: 29932 RVA: 0x002174DB File Offset: 0x002156DB
		public ObjectTargetConverter(Type sourceType, DataBindEngine engine) : base(null, sourceType, typeof(object), true, false, engine)
		{
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x00012630 File Offset: 0x00010830
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return o;
		}

		// Token: 0x060074EE RID: 29934 RVA: 0x002174F4 File Offset: 0x002156F4
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			if (o == null && !this._sourceType.IsValueType)
			{
				return o;
			}
			if (o != null && this._sourceType.IsAssignableFrom(o.GetType()))
			{
				return o;
			}
			if (this._sourceType == typeof(string))
			{
				return string.Format(culture, "{0}", new object[]
				{
					o
				});
			}
			base.EnsureConverter(this._sourceType);
			return base.ConvertFrom(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
