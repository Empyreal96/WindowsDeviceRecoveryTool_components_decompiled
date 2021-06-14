using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000DE RID: 222
	internal static class DateTimeUtils
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x0002AA02 File Offset: 0x00028C02
		public static TimeSpan GetUtcOffset(this DateTime d)
		{
			return TimeZoneInfo.Local.GetUtcOffset(d);
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002AA10 File Offset: 0x00028C10
		public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
		{
			switch (kind)
			{
			case DateTimeKind.Unspecified:
				return XmlDateTimeSerializationMode.Unspecified;
			case DateTimeKind.Utc:
				return XmlDateTimeSerializationMode.Utc;
			case DateTimeKind.Local:
				return XmlDateTimeSerializationMode.Local;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
			}
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002AA50 File Offset: 0x00028C50
		internal static DateTime EnsureDateTime(DateTime value, DateTimeZoneHandling timeZone)
		{
			switch (timeZone)
			{
			case DateTimeZoneHandling.Local:
				value = DateTimeUtils.SwitchToLocalTime(value);
				break;
			case DateTimeZoneHandling.Utc:
				value = DateTimeUtils.SwitchToUtcTime(value);
				break;
			case DateTimeZoneHandling.Unspecified:
				value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
				break;
			case DateTimeZoneHandling.RoundtripKind:
				break;
			default:
				throw new ArgumentException("Invalid date time handling value.");
			}
			return value;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002AAA8 File Offset: 0x00028CA8
		private static DateTime SwitchToLocalTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Local);
			case DateTimeKind.Utc:
				return value.ToLocalTime();
			case DateTimeKind.Local:
				return value;
			default:
				return value;
			}
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0002AAEC File Offset: 0x00028CEC
		private static DateTime SwitchToUtcTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Utc);
			case DateTimeKind.Utc:
				return value;
			case DateTimeKind.Local:
				return value.ToUniversalTime();
			default:
				return value;
			}
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0002AB2E File Offset: 0x00028D2E
		private static long ToUniversalTicks(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				return dateTime.Ticks;
			}
			return DateTimeUtils.ToUniversalTicks(dateTime, dateTime.GetUtcOffset());
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0002AB50 File Offset: 0x00028D50
		private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == DateTimeKind.Utc || dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
			{
				return dateTime.Ticks;
			}
			long num = dateTime.Ticks - offset.Ticks;
			if (num > 3155378975999999999L)
			{
				return 3155378975999999999L;
			}
			if (num < 0L)
			{
				return 0L;
			}
			return num;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0002ABB8 File Offset: 0x00028DB8
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
		{
			long universialTicks = DateTimeUtils.ToUniversalTicks(dateTime, offset);
			return DateTimeUtils.UniversialTicksToJavaScriptTicks(universialTicks);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0002ABD3 File Offset: 0x00028DD3
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
		{
			return DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime, true);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0002ABDC File Offset: 0x00028DDC
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
		{
			long universialTicks = convertToUtc ? DateTimeUtils.ToUniversalTicks(dateTime) : dateTime.Ticks;
			return DateTimeUtils.UniversialTicksToJavaScriptTicks(universialTicks);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0002AC04 File Offset: 0x00028E04
		private static long UniversialTicksToJavaScriptTicks(long universialTicks)
		{
			return (universialTicks - DateTimeUtils.InitialJavaScriptDateTicks) / 10000L;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002AC24 File Offset: 0x00028E24
		internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
		{
			DateTime result = new DateTime(javaScriptTicks * 10000L + DateTimeUtils.InitialJavaScriptDateTicks, DateTimeKind.Utc);
			return result;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0002AC48 File Offset: 0x00028E48
		internal static bool TryParseDateIso(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, out object dt)
		{
			DateTimeParser dateTimeParser = default(DateTimeParser);
			if (!dateTimeParser.Parse(text))
			{
				dt = null;
				return false;
			}
			DateTime dateTime = new DateTime(dateTimeParser.Year, dateTimeParser.Month, dateTimeParser.Day, dateTimeParser.Hour, dateTimeParser.Minute, dateTimeParser.Second);
			dateTime = dateTime.AddTicks((long)dateTimeParser.Fraction);
			if (dateParseHandling != DateParseHandling.DateTimeOffset)
			{
				switch (dateTimeParser.Zone)
				{
				case ParserTimeZone.Utc:
					dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
					break;
				case ParserTimeZone.LocalWestOfUtc:
				{
					TimeSpan timeSpan = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
					long num = dateTime.Ticks + timeSpan.Ticks;
					if (num <= DateTime.MaxValue.Ticks)
					{
						dateTime = new DateTime(num, DateTimeKind.Utc).ToLocalTime();
					}
					else
					{
						num += dateTime.GetUtcOffset().Ticks;
						if (num > DateTime.MaxValue.Ticks)
						{
							num = DateTime.MaxValue.Ticks;
						}
						dateTime = new DateTime(num, DateTimeKind.Local);
					}
					break;
				}
				case ParserTimeZone.LocalEastOfUtc:
				{
					TimeSpan timeSpan2 = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
					long num = dateTime.Ticks - timeSpan2.Ticks;
					if (num >= DateTime.MinValue.Ticks)
					{
						dateTime = new DateTime(num, DateTimeKind.Utc).ToLocalTime();
					}
					else
					{
						num += dateTime.GetUtcOffset().Ticks;
						if (num < DateTime.MinValue.Ticks)
						{
							num = DateTime.MinValue.Ticks;
						}
						dateTime = new DateTime(num, DateTimeKind.Local);
					}
					break;
				}
				}
				dt = DateTimeUtils.EnsureDateTime(dateTime, dateTimeZoneHandling);
				return true;
			}
			TimeSpan utcOffset;
			switch (dateTimeParser.Zone)
			{
			case ParserTimeZone.Utc:
				utcOffset = new TimeSpan(0L);
				break;
			case ParserTimeZone.LocalWestOfUtc:
				utcOffset = new TimeSpan(-dateTimeParser.ZoneHour, -dateTimeParser.ZoneMinute, 0);
				break;
			case ParserTimeZone.LocalEastOfUtc:
				utcOffset = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
				break;
			default:
				utcOffset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
				break;
			}
			long num2 = dateTime.Ticks - utcOffset.Ticks;
			if (num2 < 0L || num2 > 3155378975999999999L)
			{
				dt = null;
				return false;
			}
			dt = new DateTimeOffset(dateTime, utcOffset);
			return true;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0002AEBC File Offset: 0x000290BC
		internal static bool TryParseDateTime(string s, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out object dt)
		{
			if (s.Length > 0)
			{
				if (s[0] == '/')
				{
					if (s.StartsWith("/Date(", StringComparison.Ordinal) && s.EndsWith(")/", StringComparison.Ordinal) && DateTimeUtils.TryParseDateMicrosoft(s, dateParseHandling, dateTimeZoneHandling, out dt))
					{
						return true;
					}
				}
				else if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[0]) && s[10] == 'T' && DateTimeUtils.TryParseDateIso(s, dateParseHandling, dateTimeZoneHandling, out dt))
				{
					return true;
				}
				if (!string.IsNullOrEmpty(dateFormatString) && DateTimeUtils.TryParseDateExact(s, dateParseHandling, dateTimeZoneHandling, dateFormatString, culture, out dt))
				{
					return true;
				}
			}
			dt = null;
			return false;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0002AF64 File Offset: 0x00029164
		private static bool TryParseDateMicrosoft(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, out object dt)
		{
			string text2 = text.Substring(6, text.Length - 8);
			DateTimeKind dateTimeKind = DateTimeKind.Utc;
			int num = text2.IndexOf('+', 1);
			if (num == -1)
			{
				num = text2.IndexOf('-', 1);
			}
			TimeSpan timeSpan = TimeSpan.Zero;
			if (num != -1)
			{
				dateTimeKind = DateTimeKind.Local;
				timeSpan = DateTimeUtils.ReadOffset(text2.Substring(num));
				text2 = text2.Substring(0, num);
			}
			long javaScriptTicks;
			if (!long.TryParse(text2, NumberStyles.Integer, CultureInfo.InvariantCulture, out javaScriptTicks))
			{
				dt = null;
				return false;
			}
			DateTime dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			if (dateParseHandling == DateParseHandling.DateTimeOffset)
			{
				dt = new DateTimeOffset(dateTime.Add(timeSpan).Ticks, timeSpan);
				return true;
			}
			DateTime value;
			switch (dateTimeKind)
			{
			case DateTimeKind.Unspecified:
				value = DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Unspecified);
				goto IL_C6;
			case DateTimeKind.Local:
				value = dateTime.ToLocalTime();
				goto IL_C6;
			}
			value = dateTime;
			IL_C6:
			dt = DateTimeUtils.EnsureDateTime(value, dateTimeZoneHandling);
			return true;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0002B048 File Offset: 0x00029248
		private static bool TryParseDateExact(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out object dt)
		{
			DateTime dateTime;
			if (dateParseHandling == DateParseHandling.DateTimeOffset)
			{
				DateTimeOffset dateTimeOffset;
				if (DateTimeOffset.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
				{
					dt = dateTimeOffset;
					return true;
				}
			}
			else if (DateTime.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out dateTime))
			{
				dateTime = DateTimeUtils.EnsureDateTime(dateTime, dateTimeZoneHandling);
				dt = dateTime;
				return true;
			}
			dt = null;
			return false;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002B0A0 File Offset: 0x000292A0
		private static TimeSpan ReadOffset(string offsetText)
		{
			bool flag = offsetText[0] == '-';
			int num = int.Parse(offsetText.Substring(1, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
			int num2 = 0;
			if (offsetText.Length >= 5)
			{
				num2 = int.Parse(offsetText.Substring(3, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
			}
			TimeSpan result = TimeSpan.FromHours((double)num) + TimeSpan.FromMinutes((double)num2);
			if (flag)
			{
				result = result.Negate();
			}
			return result;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0002B10C File Offset: 0x0002930C
		internal static void WriteDateTimeString(TextWriter writer, DateTime value, DateFormatHandling format, string formatString, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				char[] array = new char[64];
				int count = DateTimeUtils.WriteDateTimeString(array, 0, value, null, value.Kind, format);
				writer.Write(array, 0, count);
				return;
			}
			writer.Write(value.ToString(formatString, culture));
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0002B160 File Offset: 0x00029360
		internal static int WriteDateTimeString(char[] chars, int start, DateTime value, TimeSpan? offset, DateTimeKind kind, DateFormatHandling format)
		{
			int num2;
			if (format == DateFormatHandling.MicrosoftDateFormat)
			{
				TimeSpan offset2 = offset ?? value.GetUtcOffset();
				long num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(value, offset2);
				"\\/Date(".CopyTo(0, chars, start, 7);
				num2 = start + 7;
				string text = num.ToString(CultureInfo.InvariantCulture);
				text.CopyTo(0, chars, num2, text.Length);
				num2 += text.Length;
				switch (kind)
				{
				case DateTimeKind.Unspecified:
					if (value != DateTime.MaxValue && value != DateTime.MinValue)
					{
						num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, offset2, format);
					}
					break;
				case DateTimeKind.Local:
					num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, offset2, format);
					break;
				}
				")\\/".CopyTo(0, chars, num2, 3);
				num2 += 3;
			}
			else
			{
				num2 = DateTimeUtils.WriteDefaultIsoDate(chars, start, value);
				switch (kind)
				{
				case DateTimeKind.Utc:
					chars[num2++] = 'Z';
					break;
				case DateTimeKind.Local:
					num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, offset ?? value.GetUtcOffset(), format);
					break;
				}
			}
			return num2;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0002B284 File Offset: 0x00029484
		internal static int WriteDefaultIsoDate(char[] chars, int start, DateTime dt)
		{
			int num = 19;
			int value;
			int value2;
			int value3;
			DateTimeUtils.GetDateValues(dt, out value, out value2, out value3);
			DateTimeUtils.CopyIntToCharArray(chars, start, value, 4);
			chars[start + 4] = '-';
			DateTimeUtils.CopyIntToCharArray(chars, start + 5, value2, 2);
			chars[start + 7] = '-';
			DateTimeUtils.CopyIntToCharArray(chars, start + 8, value3, 2);
			chars[start + 10] = 'T';
			DateTimeUtils.CopyIntToCharArray(chars, start + 11, dt.Hour, 2);
			chars[start + 13] = ':';
			DateTimeUtils.CopyIntToCharArray(chars, start + 14, dt.Minute, 2);
			chars[start + 16] = ':';
			DateTimeUtils.CopyIntToCharArray(chars, start + 17, dt.Second, 2);
			int num2 = (int)(dt.Ticks % 10000000L);
			if (num2 != 0)
			{
				int num3 = 7;
				while (num2 % 10 == 0)
				{
					num3--;
					num2 /= 10;
				}
				chars[start + 19] = '.';
				DateTimeUtils.CopyIntToCharArray(chars, start + 20, num2, num3);
				num += num3 + 1;
			}
			return start + num;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002B369 File Offset: 0x00029569
		private static void CopyIntToCharArray(char[] chars, int start, int value, int digits)
		{
			while (digits-- != 0)
			{
				chars[start + digits] = (char)(value % 10 + 48);
				value /= 10;
			}
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002B388 File Offset: 0x00029588
		internal static int WriteDateTimeOffset(char[] chars, int start, TimeSpan offset, DateFormatHandling format)
		{
			chars[start++] = ((offset.Ticks >= 0L) ? '+' : '-');
			int value = Math.Abs(offset.Hours);
			DateTimeUtils.CopyIntToCharArray(chars, start, value, 2);
			start += 2;
			if (format == DateFormatHandling.IsoDateFormat)
			{
				chars[start++] = ':';
			}
			int value2 = Math.Abs(offset.Minutes);
			DateTimeUtils.CopyIntToCharArray(chars, start, value2, 2);
			start += 2;
			return start;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002B3F4 File Offset: 0x000295F4
		internal static void WriteDateTimeOffsetString(TextWriter writer, DateTimeOffset value, DateFormatHandling format, string formatString, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				char[] array = new char[64];
				int count = DateTimeUtils.WriteDateTimeString(array, 0, (format == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, new TimeSpan?(value.Offset), DateTimeKind.Local, format);
				writer.Write(array, 0, count);
				return;
			}
			writer.Write(value.ToString(formatString, culture));
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002B454 File Offset: 0x00029654
		private static void GetDateValues(DateTime td, out int year, out int month, out int day)
		{
			long ticks = td.Ticks;
			int i = (int)(ticks / 864000000000L);
			int num = i / 146097;
			i -= num * 146097;
			int num2 = i / 36524;
			if (num2 == 4)
			{
				num2 = 3;
			}
			i -= num2 * 36524;
			int num3 = i / 1461;
			i -= num3 * 1461;
			int num4 = i / 365;
			if (num4 == 4)
			{
				num4 = 3;
			}
			year = num * 400 + num2 * 100 + num3 * 4 + num4 + 1;
			i -= num4 * 365;
			int[] array = (num4 == 3 && (num3 != 24 || num2 == 3)) ? DateTimeUtils.DaysToMonth366 : DateTimeUtils.DaysToMonth365;
			int num5 = i >> 6;
			while (i >= array[num5])
			{
				num5++;
			}
			month = num5;
			day = i - array[num5 - 1] + 1;
		}

		// Token: 0x040003E9 RID: 1001
		private const int DaysPer100Years = 36524;

		// Token: 0x040003EA RID: 1002
		private const int DaysPer400Years = 146097;

		// Token: 0x040003EB RID: 1003
		private const int DaysPer4Years = 1461;

		// Token: 0x040003EC RID: 1004
		private const int DaysPerYear = 365;

		// Token: 0x040003ED RID: 1005
		private const long TicksPerDay = 864000000000L;

		// Token: 0x040003EE RID: 1006
		internal static readonly long InitialJavaScriptDateTicks = 621355968000000000L;

		// Token: 0x040003EF RID: 1007
		private static readonly int[] DaysToMonth365 = new int[]
		{
			0,
			31,
			59,
			90,
			120,
			151,
			181,
			212,
			243,
			273,
			304,
			334,
			365
		};

		// Token: 0x040003F0 RID: 1008
		private static readonly int[] DaysToMonth366 = new int[]
		{
			0,
			31,
			60,
			91,
			121,
			152,
			182,
			213,
			244,
			274,
			305,
			335,
			366
		};
	}
}
