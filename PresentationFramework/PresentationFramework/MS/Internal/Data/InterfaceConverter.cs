using System;
using System.Globalization;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000720 RID: 1824
	internal class InterfaceConverter : IValueConverter
	{
		// Token: 0x060074F5 RID: 29941 RVA: 0x00217639 File Offset: 0x00215839
		internal InterfaceConverter(Type sourceType, Type targetType)
		{
			this._sourceType = sourceType;
			this._targetType = targetType;
		}

		// Token: 0x060074F6 RID: 29942 RVA: 0x0021764F File Offset: 0x0021584F
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return this.ConvertTo(o, this._targetType);
		}

		// Token: 0x060074F7 RID: 29943 RVA: 0x0021765E File Offset: 0x0021585E
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return this.ConvertTo(o, this._sourceType);
		}

		// Token: 0x060074F8 RID: 29944 RVA: 0x0021766D File Offset: 0x0021586D
		private object ConvertTo(object o, Type type)
		{
			if (!type.IsInstanceOfType(o))
			{
				return null;
			}
			return o;
		}

		// Token: 0x0400380A RID: 14346
		private Type _sourceType;

		// Token: 0x0400380B RID: 14347
		private Type _targetType;
	}
}
