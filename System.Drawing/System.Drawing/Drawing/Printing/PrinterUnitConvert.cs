using System;

namespace System.Drawing.Printing
{
	/// <summary>Specifies a series of conversion methods that are useful when interoperating with the Win32 printing API. This class cannot be inherited.</summary>
	// Token: 0x02000066 RID: 102
	public sealed class PrinterUnitConvert
	{
		// Token: 0x060007E8 RID: 2024 RVA: 0x00003800 File Offset: 0x00001A00
		private PrinterUnitConvert()
		{
		}

		/// <summary>Converts a double-precision floating-point number from one <see cref="T:System.Drawing.Printing.PrinterUnit" /> type to another <see cref="T:System.Drawing.Printing.PrinterUnit" /> type.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.Point" /> being converted. </param>
		/// <param name="fromUnit">The unit to convert from. </param>
		/// <param name="toUnit">The unit to convert to. </param>
		/// <returns>A double-precision floating-point number that represents the converted <see cref="T:System.Drawing.Printing.PrinterUnit" />.</returns>
		// Token: 0x060007E9 RID: 2025 RVA: 0x0002065C File Offset: 0x0001E85C
		public static double Convert(double value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			double num = PrinterUnitConvert.UnitsPerDisplay(fromUnit);
			double num2 = PrinterUnitConvert.UnitsPerDisplay(toUnit);
			return value * num2 / num;
		}

		/// <summary>Converts a 32-bit signed integer from one <see cref="T:System.Drawing.Printing.PrinterUnit" /> type to another <see cref="T:System.Drawing.Printing.PrinterUnit" /> type.</summary>
		/// <param name="value">The value being converted. </param>
		/// <param name="fromUnit">The unit to convert from. </param>
		/// <param name="toUnit">The unit to convert to. </param>
		/// <returns>A 32-bit signed integer that represents the converted <see cref="T:System.Drawing.Printing.PrinterUnit" />.</returns>
		// Token: 0x060007EA RID: 2026 RVA: 0x0002067C File Offset: 0x0001E87C
		public static int Convert(int value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return (int)Math.Round(PrinterUnitConvert.Convert((double)value, fromUnit, toUnit));
		}

		/// <summary>Converts a <see cref="T:System.Drawing.Point" /> from one <see cref="T:System.Drawing.Printing.PrinterUnit" /> type to another <see cref="T:System.Drawing.Printing.PrinterUnit" /> type.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.Point" /> being converted. </param>
		/// <param name="fromUnit">The unit to convert from. </param>
		/// <param name="toUnit">The unit to convert to. </param>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that represents the converted <see cref="T:System.Drawing.Printing.PrinterUnit" />.</returns>
		// Token: 0x060007EB RID: 2027 RVA: 0x0002068D File Offset: 0x0001E88D
		public static Point Convert(Point value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return new Point(PrinterUnitConvert.Convert(value.X, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Y, fromUnit, toUnit));
		}

		/// <summary>Converts a <see cref="T:System.Drawing.Size" /> from one <see cref="T:System.Drawing.Printing.PrinterUnit" /> type to another <see cref="T:System.Drawing.Printing.PrinterUnit" /> type.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.Size" /> being converted. </param>
		/// <param name="fromUnit">The unit to convert from. </param>
		/// <param name="toUnit">The unit to convert to. </param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the converted <see cref="T:System.Drawing.Printing.PrinterUnit" />.</returns>
		// Token: 0x060007EC RID: 2028 RVA: 0x000206B0 File Offset: 0x0001E8B0
		public static Size Convert(Size value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return new Size(PrinterUnitConvert.Convert(value.Width, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Height, fromUnit, toUnit));
		}

		/// <summary>Converts a <see cref="T:System.Drawing.Rectangle" /> from one <see cref="T:System.Drawing.Printing.PrinterUnit" /> type to another <see cref="T:System.Drawing.Printing.PrinterUnit" /> type.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.Rectangle" /> being converted. </param>
		/// <param name="fromUnit">The unit to convert from. </param>
		/// <param name="toUnit">The unit to convert to. </param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the converted <see cref="T:System.Drawing.Printing.PrinterUnit" />.</returns>
		// Token: 0x060007ED RID: 2029 RVA: 0x000206D3 File Offset: 0x0001E8D3
		public static Rectangle Convert(Rectangle value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return new Rectangle(PrinterUnitConvert.Convert(value.X, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Y, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Width, fromUnit, toUnit), PrinterUnitConvert.Convert(value.Height, fromUnit, toUnit));
		}

		/// <summary>Converts a <see cref="T:System.Drawing.Printing.Margins" /> from one <see cref="T:System.Drawing.Printing.PrinterUnit" /> type to another <see cref="T:System.Drawing.Printing.PrinterUnit" /> type.</summary>
		/// <param name="value">The <see cref="T:System.Drawing.Printing.Margins" /> being converted. </param>
		/// <param name="fromUnit">The unit to convert from. </param>
		/// <param name="toUnit">The unit to convert to. </param>
		/// <returns>A <see cref="T:System.Drawing.Printing.Margins" /> that represents the converted <see cref="T:System.Drawing.Printing.PrinterUnit" />.</returns>
		// Token: 0x060007EE RID: 2030 RVA: 0x00020714 File Offset: 0x0001E914
		public static Margins Convert(Margins value, PrinterUnit fromUnit, PrinterUnit toUnit)
		{
			return new Margins
			{
				DoubleLeft = PrinterUnitConvert.Convert(value.DoubleLeft, fromUnit, toUnit),
				DoubleRight = PrinterUnitConvert.Convert(value.DoubleRight, fromUnit, toUnit),
				DoubleTop = PrinterUnitConvert.Convert(value.DoubleTop, fromUnit, toUnit),
				DoubleBottom = PrinterUnitConvert.Convert(value.DoubleBottom, fromUnit, toUnit)
			};
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00020774 File Offset: 0x0001E974
		private static double UnitsPerDisplay(PrinterUnit unit)
		{
			double result;
			switch (unit)
			{
			case PrinterUnit.Display:
				result = 1.0;
				break;
			case PrinterUnit.ThousandthsOfAnInch:
				result = 10.0;
				break;
			case PrinterUnit.HundredthsOfAMillimeter:
				result = 25.4;
				break;
			case PrinterUnit.TenthsOfAMillimeter:
				result = 2.54;
				break;
			default:
				result = 1.0;
				break;
			}
			return result;
		}
	}
}
