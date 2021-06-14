using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x020002A7 RID: 679
	internal static class JsonValueUtils
	{
		// Token: 0x060016D6 RID: 5846 RVA: 0x00052ABF File Offset: 0x00050CBF
		internal static void WriteValue(TextWriter writer, bool value)
		{
			writer.Write(value ? "true" : "false");
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00052AD6 File Offset: 0x00050CD6
		internal static void WriteValue(TextWriter writer, int value)
		{
			writer.Write(value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00052AEA File Offset: 0x00050CEA
		internal static void WriteValue(TextWriter writer, float value)
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				JsonValueUtils.WriteQuoted(writer, value.ToString(null, CultureInfo.InvariantCulture));
				return;
			}
			writer.Write(XmlConvert.ToString(value));
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00052B1C File Offset: 0x00050D1C
		internal static void WriteValue(TextWriter writer, short value)
		{
			writer.Write(value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00052B30 File Offset: 0x00050D30
		internal static void WriteValue(TextWriter writer, long value)
		{
			JsonValueUtils.WriteQuoted(writer, value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00052B44 File Offset: 0x00050D44
		internal static void WriteValue(TextWriter writer, double value, bool mustIncludeDecimalPoint)
		{
			if (JsonSharedUtils.IsDoubleValueSerializedAsString(value))
			{
				JsonValueUtils.WriteQuoted(writer, value.ToString(null, CultureInfo.InvariantCulture));
				return;
			}
			string text = XmlConvert.ToString(value);
			writer.Write(text);
			if (mustIncludeDecimalPoint && text.IndexOfAny(JsonValueUtils.DoubleIndicatingCharacters) < 0)
			{
				writer.Write(".0");
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00052B97 File Offset: 0x00050D97
		internal static void WriteValue(TextWriter writer, Guid value)
		{
			JsonValueUtils.WriteQuoted(writer, value.ToString());
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00052BAC File Offset: 0x00050DAC
		internal static void WriteValue(TextWriter writer, decimal value)
		{
			JsonValueUtils.WriteQuoted(writer, value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00052BC0 File Offset: 0x00050DC0
		internal static void WriteValue(TextWriter writer, DateTime value, ODataJsonDateTimeFormat dateTimeFormat)
		{
			switch (dateTimeFormat)
			{
			case ODataJsonDateTimeFormat.ODataDateTime:
			{
				value = JsonValueUtils.GetUniversalDate(value);
				string text = string.Format(CultureInfo.InvariantCulture, "\\/Date({0})\\/", new object[]
				{
					JsonValueUtils.DateTimeTicksToJsonTicks(value.Ticks)
				});
				JsonValueUtils.WriteQuoted(writer, text);
				return;
			}
			case ODataJsonDateTimeFormat.ISO8601DateTime:
			{
				string text2 = PlatformHelper.ConvertDateTimeToString(value);
				JsonValueUtils.WriteQuoted(writer, text2);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00052C28 File Offset: 0x00050E28
		internal static void WriteValue(TextWriter writer, DateTimeOffset value, ODataJsonDateTimeFormat dateTimeFormat)
		{
			int num = (int)value.Offset.TotalMinutes;
			switch (dateTimeFormat)
			{
			case ODataJsonDateTimeFormat.ODataDateTime:
			{
				string text = string.Format(CultureInfo.InvariantCulture, "\\/Date({0}{1}{2:D4})\\/", new object[]
				{
					JsonValueUtils.DateTimeTicksToJsonTicks(value.Ticks),
					(num >= 0) ? "+" : string.Empty,
					num
				});
				JsonValueUtils.WriteQuoted(writer, text);
				return;
			}
			case ODataJsonDateTimeFormat.ISO8601DateTime:
			{
				string text2 = XmlConvert.ToString(value);
				JsonValueUtils.WriteQuoted(writer, text2);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00052CBC File Offset: 0x00050EBC
		internal static void WriteValue(TextWriter writer, TimeSpan value)
		{
			JsonValueUtils.WriteQuoted(writer, XmlConvert.ToString(value));
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00052CCA File Offset: 0x00050ECA
		internal static void WriteValue(TextWriter writer, byte value)
		{
			writer.Write(value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00052CDE File Offset: 0x00050EDE
		internal static void WriteValue(TextWriter writer, sbyte value)
		{
			writer.Write(value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00052CF2 File Offset: 0x00050EF2
		internal static void WriteValue(TextWriter writer, string value)
		{
			if (value == null)
			{
				writer.Write("null");
				return;
			}
			JsonValueUtils.WriteEscapedJsonString(writer, value);
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00052D0C File Offset: 0x00050F0C
		internal static void WriteEscapedJsonString(TextWriter writer, string inputString)
		{
			writer.Write('"');
			int num = 0;
			int length = inputString.Length;
			int i = 0;
			int num2;
			while (i < length)
			{
				char c = inputString[i];
				char c2 = c;
				string value;
				switch (c2)
				{
				case '\b':
					value = "\\b";
					goto IL_A9;
				case '\t':
					value = "\\t";
					goto IL_A9;
				case '\n':
					value = "\\n";
					goto IL_A9;
				case '\v':
					goto IL_93;
				case '\f':
					value = "\\f";
					goto IL_A9;
				case '\r':
					value = "\\r";
					goto IL_A9;
				default:
					if (c2 == '"')
					{
						value = "\\\"";
						goto IL_A9;
					}
					if (c2 != '\\')
					{
						goto IL_93;
					}
					value = "\\\\";
					goto IL_A9;
				}
				IL_CB:
				i++;
				continue;
				IL_93:
				if (c >= ' ' && c <= '\u007f')
				{
					goto IL_CB;
				}
				value = JsonValueUtils.SpecialCharToEscapedStringMap[(int)c];
				IL_A9:
				num2 = i - num;
				if (num2 > 0)
				{
					writer.Write(inputString.Substring(num, num2));
				}
				writer.Write(value);
				num = i + 1;
				goto IL_CB;
			}
			num2 = length - num;
			if (num2 > 0)
			{
				writer.Write(inputString.Substring(num, num2));
			}
			writer.Write('"');
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00052E0D File Offset: 0x0005100D
		internal static long JsonTicksToDateTimeTicks(long ticks)
		{
			return ticks * 10000L + JsonValueUtils.JsonDateTimeMinTimeTicks;
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x00052E1D File Offset: 0x0005101D
		private static void WriteQuoted(TextWriter writer, string text)
		{
			writer.Write('"');
			writer.Write(text);
			writer.Write('"');
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00052E36 File Offset: 0x00051036
		private static long DateTimeTicksToJsonTicks(long ticks)
		{
			return (ticks - JsonValueUtils.JsonDateTimeMinTimeTicks) / 10000L;
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00052E48 File Offset: 0x00051048
		private static DateTime GetUniversalDate(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				value = new DateTime(value.Ticks, DateTimeKind.Utc);
				break;
			case DateTimeKind.Local:
				value = value.ToUniversalTime();
				break;
			}
			return value;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00052E8C File Offset: 0x0005108C
		private static string[] CreateSpecialCharToEscapedStringMap()
		{
			string[] array = new string[65536];
			for (int i = 0; i <= 65535; i++)
			{
				array[i] = string.Format(CultureInfo.InvariantCulture, "\\u{0:x4}", new object[]
				{
					i
				});
			}
			return array;
		}

		// Token: 0x04000974 RID: 2420
		private static readonly long JsonDateTimeMinTimeTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

		// Token: 0x04000975 RID: 2421
		private static readonly char[] DoubleIndicatingCharacters = new char[]
		{
			'.',
			'e',
			'E'
		};

		// Token: 0x04000976 RID: 2422
		private static readonly string[] SpecialCharToEscapedStringMap = JsonValueUtils.CreateSpecialCharToEscapedStringMap();
	}
}
