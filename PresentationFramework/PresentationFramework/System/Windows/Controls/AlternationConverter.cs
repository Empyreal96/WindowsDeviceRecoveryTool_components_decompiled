using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows.Controls
{
	/// <summary>Converts an integer to and from an object by applying the integer as an index to a list of objects.</summary>
	// Token: 0x0200046D RID: 1133
	[ContentProperty("Values")]
	public class AlternationConverter : IValueConverter
	{
		/// <summary>Gets a list of objects that the <see cref="T:System.Windows.Controls.AlternationConverter" /> returns when an integer is passed to the <see cref="M:System.Windows.Controls.AlternationConverter.Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)" /> method.</summary>
		/// <returns>A list of objects that the <see cref="T:System.Windows.Controls.AlternationConverter" /> returns when an integer is passed to the <see cref="M:System.Windows.Controls.AlternationConverter.Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)" /> method.</returns>
		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06004230 RID: 16944 RVA: 0x0012ED54 File Offset: 0x0012CF54
		public IList Values
		{
			get
			{
				return this._values;
			}
		}

		/// <summary>Converts an integer to an object in the <see cref="P:System.Windows.Controls.AlternationConverter.Values" /> list.</summary>
		/// <param name="o">The integer to use to find an object in the <see cref="P:System.Windows.Controls.AlternationConverter.Values" /> property. </param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>The object that is in the position of <paramref name="o" /> modulo the number of items in <see cref="P:System.Windows.Controls.AlternationConverter.Values" />.</returns>
		// Token: 0x06004231 RID: 16945 RVA: 0x0012ED5C File Offset: 0x0012CF5C
		public object Convert(object o, Type targetType, object parameter, CultureInfo culture)
		{
			if (this._values.Count > 0 && o is int)
			{
				int num = (int)o % this._values.Count;
				if (num < 0)
				{
					num += this._values.Count;
				}
				return this._values[num];
			}
			return DependencyProperty.UnsetValue;
		}

		/// <summary>Converts an object in the <see cref="P:System.Windows.Controls.AlternationConverter.Values" /> list to an integer.</summary>
		/// <param name="o">The object to find in the <see cref="P:System.Windows.Controls.AlternationConverter.Values" /> property. </param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>The index of <paramref name="o" /> if it is in <see cref="P:System.Windows.Controls.AlternationConverter.Values" />, or –1 if o does not exist in <see cref="P:System.Windows.Controls.AlternationConverter.Values" />.</returns>
		// Token: 0x06004232 RID: 16946 RVA: 0x0012EDB6 File Offset: 0x0012CFB6
		public object ConvertBack(object o, Type targetType, object parameter, CultureInfo culture)
		{
			return this._values.IndexOf(o);
		}

		// Token: 0x040027D8 RID: 10200
		private List<object> _values = new List<object>();
	}
}
