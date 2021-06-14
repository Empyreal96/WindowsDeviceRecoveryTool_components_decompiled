using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Navigation
{
	/// <summary>This type or member supports the Microsoft .NET infrastructure and is not intended to be used directly from your code.</summary>
	// Token: 0x02000305 RID: 773
	public sealed class JournalEntryListConverter : IValueConverter
	{
		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value.</returns>
		// Token: 0x06002913 RID: 10515 RVA: 0x000BDFD0 File Offset: 0x000BC1D0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			return ((JournalEntryStack)value).GetLimitedJournalEntryStackEnumerable();
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">The value produced by the binding target.</param>
		/// <param name="targetType">The type of the binding source property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value.</returns>
		// Token: 0x06002914 RID: 10516 RVA: 0x000BDFE2 File Offset: 0x000BC1E2
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
