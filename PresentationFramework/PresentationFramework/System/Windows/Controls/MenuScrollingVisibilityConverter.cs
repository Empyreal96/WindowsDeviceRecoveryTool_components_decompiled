using System;
using System.Globalization;
using System.Windows.Data;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a data-binding converter to handle the visibility of repeat buttons in scrolling menus.</summary>
	// Token: 0x02000504 RID: 1284
	public sealed class MenuScrollingVisibilityConverter : IMultiValueConverter
	{
		/// <summary>Called when moving a value from a source to a target.</summary>
		/// <param name="values">Values produced by the source binding.</param>
		/// <param name="targetType">Type of the target. Type that the source will be converted into.</param>
		/// <param name="parameter">Converter parameter.</param>
		/// <param name="culture">Culture information.</param>
		/// <returns>Converted value.</returns>
		// Token: 0x06005264 RID: 21092 RVA: 0x00170654 File Offset: 0x0016E854
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Type typeFromHandle = typeof(double);
			if (parameter == null || values == null || values.Length != 4 || values[0] == null || values[1] == null || values[2] == null || values[3] == null || !typeof(Visibility).IsAssignableFrom(values[0].GetType()) || !typeFromHandle.IsAssignableFrom(values[1].GetType()) || !typeFromHandle.IsAssignableFrom(values[2].GetType()) || !typeFromHandle.IsAssignableFrom(values[3].GetType()))
			{
				return DependencyProperty.UnsetValue;
			}
			Type type = parameter.GetType();
			if (!typeFromHandle.IsAssignableFrom(type) && !typeof(string).IsAssignableFrom(type))
			{
				return DependencyProperty.UnsetValue;
			}
			if ((Visibility)values[0] == Visibility.Visible)
			{
				double value;
				if (parameter is string)
				{
					value = double.Parse((string)parameter, NumberFormatInfo.InvariantInfo);
				}
				else
				{
					value = (double)parameter;
				}
				double num = (double)values[1];
				double num2 = (double)values[2];
				double num3 = (double)values[3];
				if (num2 != num3)
				{
					double value2 = Math.Min(100.0, Math.Max(0.0, num * 100.0 / (num2 - num3)));
					if (DoubleUtil.AreClose(value2, value))
					{
						return Visibility.Collapsed;
					}
				}
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		/// <summary>Not supported.</summary>
		/// <param name="value">This parameter is not used.</param>
		/// <param name="targetTypes">This parameter is not used.</param>
		/// <param name="parameter">This parameter is not used.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>
		///     <see cref="F:System.Windows.Data.Binding.DoNothing" />
		///   </returns>
		// Token: 0x06005265 RID: 21093 RVA: 0x000BE07B File Offset: 0x000BC27B
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[]
			{
				Binding.DoNothing
			};
		}
	}
}
