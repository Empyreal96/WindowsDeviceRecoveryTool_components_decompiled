using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	/// <summary>Represents the converter that converts Boolean values to and from <see cref="T:System.Windows.Visibility" /> enumeration values. </summary>
	// Token: 0x02000471 RID: 1137
	[Localizability(LocalizationCategory.NeverLocalize)]
	public sealed class BooleanToVisibilityConverter : IValueConverter
	{
		/// <summary>Converts a Boolean value to a <see cref="T:System.Windows.Visibility" /> enumeration value.</summary>
		/// <param name="value">The Boolean value to convert. This value can be a standard Boolean value or a nullable Boolean value.</param>
		/// <param name="targetType">This parameter is not used.</param>
		/// <param name="parameter">This parameter is not used.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>
		///     <see cref="F:System.Windows.Visibility.Visible" /> if <paramref name="value" /> is <see langword="true" />; otherwise, <see cref="F:System.Windows.Visibility.Collapsed" />.</returns>
		// Token: 0x06004257 RID: 16983 RVA: 0x0012F798 File Offset: 0x0012D998
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			if (value is bool)
			{
				flag = (bool)value;
			}
			else if (value is bool?)
			{
				bool? flag2 = (bool?)value;
				flag = (flag2 != null && flag2.Value);
			}
			return flag ? Visibility.Visible : Visibility.Collapsed;
		}

		/// <summary>Converts a <see cref="T:System.Windows.Visibility" /> enumeration value to a Boolean value.</summary>
		/// <param name="value">A <see cref="T:System.Windows.Visibility" /> enumeration value. </param>
		/// <param name="targetType">This parameter is not used.</param>
		/// <param name="parameter">This parameter is not used.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="value" /> is <see cref="F:System.Windows.Visibility.Visible" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004258 RID: 16984 RVA: 0x0012F7E7 File Offset: 0x0012D9E7
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility)
			{
				return (Visibility)value == Visibility.Visible;
			}
			return false;
		}
	}
}
