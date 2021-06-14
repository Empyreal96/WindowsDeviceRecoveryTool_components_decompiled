using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200023B RID: 571
	internal class XamlGridLengthSerializer : XamlSerializer
	{
		// Token: 0x06002294 RID: 8852 RVA: 0x000AB6CD File Offset: 0x000A98CD
		private XamlGridLengthSerializer()
		{
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000ABD40 File Offset: 0x000A9F40
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			double num;
			GridUnitType gridUnitType;
			XamlGridLengthSerializer.FromString(stringValue, TypeConverterHelper.InvariantEnglishUS, out num, out gridUnitType);
			byte b = (byte)gridUnitType;
			int num2 = (int)num;
			if ((double)num2 == num)
			{
				if (num2 <= 127 && num2 >= 0 && gridUnitType == GridUnitType.Pixel)
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

		// Token: 0x06002296 RID: 8854 RVA: 0x000ABE08 File Offset: 0x000AA008
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			byte b = reader.ReadByte();
			GridUnitType type;
			double value;
			if ((b & 128) == 0)
			{
				type = GridUnitType.Pixel;
				value = (double)b;
			}
			else
			{
				type = (GridUnitType)(b & 31);
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
			return new GridLength(value, type);
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x000ABE94 File Offset: 0x000AA094
		internal static void FromString(string s, CultureInfo cultureInfo, out double value, out GridUnitType unit)
		{
			string text = s.Trim().ToLowerInvariant();
			value = 0.0;
			unit = GridUnitType.Pixel;
			int length = text.Length;
			int num = 0;
			double num2 = 1.0;
			int i = 0;
			if (text == XamlGridLengthSerializer.UnitStrings[i])
			{
				num = XamlGridLengthSerializer.UnitStrings[i].Length;
				unit = (GridUnitType)i;
			}
			else
			{
				for (i = 1; i < XamlGridLengthSerializer.UnitStrings.Length; i++)
				{
					if (text.EndsWith(XamlGridLengthSerializer.UnitStrings[i], StringComparison.Ordinal))
					{
						num = XamlGridLengthSerializer.UnitStrings[i].Length;
						unit = (GridUnitType)i;
						break;
					}
				}
			}
			if (i >= XamlGridLengthSerializer.UnitStrings.Length)
			{
				for (i = 0; i < XamlGridLengthSerializer.PixelUnitStrings.Length; i++)
				{
					if (text.EndsWith(XamlGridLengthSerializer.PixelUnitStrings[i], StringComparison.Ordinal))
					{
						num = XamlGridLengthSerializer.PixelUnitStrings[i].Length;
						num2 = XamlGridLengthSerializer.PixelUnitFactors[i];
						break;
					}
				}
			}
			if (length == num && (unit == GridUnitType.Auto || unit == GridUnitType.Star))
			{
				value = 1.0;
				return;
			}
			string value2 = text.Substring(0, length - num);
			value = Convert.ToDouble(value2, cultureInfo) * num2;
		}

		// Token: 0x040019F6 RID: 6646
		private static string[] UnitStrings = new string[]
		{
			"auto",
			"px",
			"*"
		};

		// Token: 0x040019F7 RID: 6647
		private static string[] PixelUnitStrings = new string[]
		{
			"in",
			"cm",
			"pt"
		};

		// Token: 0x040019F8 RID: 6648
		private static double[] PixelUnitFactors = new double[]
		{
			96.0,
			37.79527559055118,
			1.3333333333333333
		};
	}
}
