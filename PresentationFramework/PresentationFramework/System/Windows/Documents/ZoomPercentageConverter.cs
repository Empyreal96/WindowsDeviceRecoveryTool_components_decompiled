using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Documents
{
	/// <summary>Implements a type converter for converting <see cref="T:System.Double" /> (used as the value of <see cref="P:System.Windows.Controls.DocumentViewer.Zoom" />) to and from other types.</summary>
	// Token: 0x0200043A RID: 1082
	public sealed class ZoomPercentageConverter : IValueConverter
	{
		/// <summary>Converts the <see cref="T:System.Double" /> (used as the value of <see cref="P:System.Windows.Controls.DocumentViewer.Zoom" />) to an object of the specified type. </summary>
		/// <param name="value">The current value of <see cref="P:System.Windows.Controls.DocumentViewer.Zoom" />.</param>
		/// <param name="targetType">The type to which <paramref name="value" /> is to be converted. This must be <see cref="T:System.Double" /> or <see cref="T:System.String" />. See Remarks.</param>
		/// <param name="parameter">
		///       <see langword="null" />. See Remarks.</param>
		/// <param name="culture">The language and culture assumed during the conversion.</param>
		/// <returns>
		///     <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> when the converter cannot produce a value; for example, when <paramref name="value" /> is <see langword="null" /> or when <paramref name="targetType" /> is not <see cref="T:System.Double" /> or <see cref="T:System.String" />.- or -The new <see cref="T:System.Object" /> of the designated type. As implemented in this class, this must be either a <see cref="T:System.Double" /> or a <see cref="T:System.String" />. If it is a string, it will be formatted appropriately for the <paramref name="culture" />.</returns>
		// Token: 0x06003F7E RID: 16254 RVA: 0x00124B78 File Offset: 0x00122D78
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType == null)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value != null && value is double)
			{
				double num = (double)value;
				if (targetType == typeof(string) || targetType == typeof(object))
				{
					if (double.IsNaN(num) || double.IsInfinity(num))
					{
						return DependencyProperty.UnsetValue;
					}
					return string.Format(CultureInfo.CurrentCulture, SR.Get("ZoomPercentageConverterStringFormat"), new object[]
					{
						num
					});
				}
				else if (targetType == typeof(double))
				{
					return num;
				}
			}
			return DependencyProperty.UnsetValue;
		}

		/// <summary>Returns a previously converted value of <see cref="P:System.Windows.Controls.DocumentViewer.Zoom" /> back to a <see cref="T:System.Double" /> that can be assigned to <see cref="P:System.Windows.Controls.DocumentViewer.Zoom" />. </summary>
		/// <param name="value">The object that is to be converted back to a <see cref="T:System.Double" />. </param>
		/// <param name="targetType">The type of <paramref name="value" />. This must be <see cref="T:System.Double" /> or <see cref="T:System.String" />. See Remarks.</param>
		/// <param name="parameter">
		///       <see langword="null" />. See Remarks.</param>
		/// <param name="culture">The language and culture assumed during the conversion.</param>
		/// <returns>
		///     <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> when the converter cannot produce a value; for example, when <paramref name="value" /> is not a valid percentage when <paramref name="targetType" /> is not <see cref="T:System.Double" /> or <see cref="T:System.String" />.- or -A <see cref="T:System.Double" /> representing the zoom percentage of a <see cref="T:System.Windows.Controls.DocumentViewer" />.</returns>
		// Token: 0x06003F7F RID: 16255 RVA: 0x00124C28 File Offset: 0x00122E28
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(targetType == typeof(double)) || value == null)
			{
				return DependencyProperty.UnsetValue;
			}
			double num = 0.0;
			bool flag = false;
			if (value is int)
			{
				num = (double)((int)value);
				flag = true;
			}
			else if (value is double)
			{
				num = (double)value;
				flag = true;
			}
			else if (value is string)
			{
				try
				{
					string text = (string)value;
					if (culture != null && !string.IsNullOrEmpty(text))
					{
						text = ((string)value).Trim();
						if (!culture.IsNeutralCulture && text.Length > 0 && culture.NumberFormat != null)
						{
							int percentPositivePattern = culture.NumberFormat.PercentPositivePattern;
							if (percentPositivePattern > 1)
							{
								if (percentPositivePattern == 2)
								{
									if (text.IndexOf(culture.NumberFormat.PercentSymbol, StringComparison.CurrentCultureIgnoreCase) == 0)
									{
										text = text.Substring(1);
									}
								}
							}
							else if (text.Length - 1 == text.LastIndexOf(culture.NumberFormat.PercentSymbol, StringComparison.CurrentCultureIgnoreCase))
							{
								text = text.Substring(0, text.Length - 1);
							}
						}
						num = System.Convert.ToDouble(text, culture);
						flag = true;
					}
				}
				catch (ArgumentOutOfRangeException)
				{
				}
				catch (ArgumentNullException)
				{
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			if (!flag)
			{
				return DependencyProperty.UnsetValue;
			}
			return num;
		}
	}
}
