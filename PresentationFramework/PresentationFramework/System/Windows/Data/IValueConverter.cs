using System;
using System.Globalization;

namespace System.Windows.Data
{
	/// <summary>Provides a way to apply custom logic to a binding.</summary>
	// Token: 0x020001B1 RID: 433
	public interface IValueConverter
	{
		/// <summary>Converts a value. </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		// Token: 0x06001B67 RID: 7015
		object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		/// <summary>Converts a value. </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
		// Token: 0x06001B68 RID: 7016
		object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
	}
}
