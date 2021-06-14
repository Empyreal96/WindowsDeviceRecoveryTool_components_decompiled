using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200023A RID: 570
	internal class XamlFigureLengthSerializer : XamlSerializer
	{
		// Token: 0x0600228F RID: 8847 RVA: 0x000AB6CD File Offset: 0x000A98CD
		private XamlFigureLengthSerializer()
		{
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x000AB9E4 File Offset: 0x000A9BE4
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			double num;
			FigureUnitType figureUnitType;
			XamlFigureLengthSerializer.FromString(stringValue, TypeConverterHelper.InvariantEnglishUS, out num, out figureUnitType);
			byte b = (byte)figureUnitType;
			int num2 = (int)num;
			if ((double)num2 == num)
			{
				if (num2 <= 127 && num2 >= 0 && figureUnitType == FigureUnitType.Pixel)
				{
					writer.Write((byte)num2);
				}
				else if (num2 <= 255 && num2 >= 0)
				{
					writer.Write(128 | b);
					writer.Write((byte)num2);
				}
				else if (num2 <= 32767 && num2 >= -32768)
				{
					writer.Write(192 | b);
					writer.Write((short)num2);
				}
				else
				{
					writer.Write(160 | b);
					writer.Write(num2);
				}
			}
			else
			{
				writer.Write(224 | b);
				writer.Write(num);
			}
			return true;
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x000ABAAC File Offset: 0x000A9CAC
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			byte b = reader.ReadByte();
			FigureUnitType type;
			double value;
			if ((b & 128) == 0)
			{
				type = FigureUnitType.Pixel;
				value = (double)b;
			}
			else
			{
				type = (FigureUnitType)(b & 31);
				byte b2 = b & 224;
				if (b2 == 128)
				{
					value = (double)reader.ReadByte();
				}
				else if (b2 == 192)
				{
					value = (double)reader.ReadInt16();
				}
				else if (b2 == 160)
				{
					value = (double)reader.ReadInt32();
				}
				else
				{
					value = reader.ReadDouble();
				}
			}
			return new FigureLength(value, type);
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x000ABB38 File Offset: 0x000A9D38
		internal static void FromString(string s, CultureInfo cultureInfo, out double value, out FigureUnitType unit)
		{
			string text = s.Trim().ToLowerInvariant();
			value = 0.0;
			unit = FigureUnitType.Pixel;
			int length = text.Length;
			int num = 0;
			double num2 = 1.0;
			int i = 0;
			if (text == XamlFigureLengthSerializer.UnitStrings[i].Name)
			{
				num = XamlFigureLengthSerializer.UnitStrings[i].Name.Length;
				unit = XamlFigureLengthSerializer.UnitStrings[i].UnitType;
			}
			else
			{
				for (i = 1; i < XamlFigureLengthSerializer.UnitStrings.Length; i++)
				{
					if (text.EndsWith(XamlFigureLengthSerializer.UnitStrings[i].Name, StringComparison.Ordinal))
					{
						num = XamlFigureLengthSerializer.UnitStrings[i].Name.Length;
						unit = XamlFigureLengthSerializer.UnitStrings[i].UnitType;
						break;
					}
				}
			}
			if (i >= XamlFigureLengthSerializer.UnitStrings.Length)
			{
				for (i = 0; i < XamlFigureLengthSerializer.PixelUnitStrings.Length; i++)
				{
					if (text.EndsWith(XamlFigureLengthSerializer.PixelUnitStrings[i], StringComparison.Ordinal))
					{
						num = XamlFigureLengthSerializer.PixelUnitStrings[i].Length;
						num2 = XamlFigureLengthSerializer.PixelUnitFactors[i];
						break;
					}
				}
			}
			if (length == num && unit != FigureUnitType.Pixel)
			{
				value = 1.0;
				return;
			}
			string value2 = text.Substring(0, length - num);
			value = Convert.ToDouble(value2, cultureInfo) * num2;
		}

		// Token: 0x040019F3 RID: 6643
		private static XamlFigureLengthSerializer.FigureUnitTypeStringConvert[] UnitStrings = new XamlFigureLengthSerializer.FigureUnitTypeStringConvert[]
		{
			new XamlFigureLengthSerializer.FigureUnitTypeStringConvert("auto", FigureUnitType.Auto),
			new XamlFigureLengthSerializer.FigureUnitTypeStringConvert("px", FigureUnitType.Pixel),
			new XamlFigureLengthSerializer.FigureUnitTypeStringConvert("column", FigureUnitType.Column),
			new XamlFigureLengthSerializer.FigureUnitTypeStringConvert("columns", FigureUnitType.Column),
			new XamlFigureLengthSerializer.FigureUnitTypeStringConvert("content", FigureUnitType.Content),
			new XamlFigureLengthSerializer.FigureUnitTypeStringConvert("page", FigureUnitType.Page)
		};

		// Token: 0x040019F4 RID: 6644
		private static string[] PixelUnitStrings = new string[]
		{
			"in",
			"cm",
			"pt"
		};

		// Token: 0x040019F5 RID: 6645
		private static double[] PixelUnitFactors = new double[]
		{
			96.0,
			37.79527559055118,
			1.3333333333333333
		};

		// Token: 0x0200089B RID: 2203
		private struct FigureUnitTypeStringConvert
		{
			// Token: 0x060083A7 RID: 33703 RVA: 0x00245EC4 File Offset: 0x002440C4
			internal FigureUnitTypeStringConvert(string name, FigureUnitType unitType)
			{
				this.Name = name;
				this.UnitType = unitType;
			}

			// Token: 0x040041A4 RID: 16804
			internal string Name;

			// Token: 0x040041A5 RID: 16805
			internal FigureUnitType UnitType;
		}
	}
}
