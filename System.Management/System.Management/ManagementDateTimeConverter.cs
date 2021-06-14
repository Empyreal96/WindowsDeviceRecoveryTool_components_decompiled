using System;
using System.Globalization;

namespace System.Management
{
	/// <summary>Provides methods to convert DMTF datetime and time intervals to CLR-compliant <see cref="T:System.DateTime" /> and <see cref="T:System.TimeSpan" /> format and vice versa.                           </summary>
	// Token: 0x0200000B RID: 11
	public sealed class ManagementDateTimeConverter
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000035AF File Offset: 0x000017AF
		private ManagementDateTimeConverter()
		{
		}

		/// <summary>Converts a given DMTF datetime to <see cref="T:System.DateTime" />. The returned <see cref="T:System.DateTime" /> will be in the current time zone of the system.          </summary>
		/// <param name="dmtfDate">A string representing the datetime in DMTF format.</param>
		/// <returns>A <see cref="T:System.DateTime" /> that represents the given DMTF datetime.</returns>
		// Token: 0x06000049 RID: 73 RVA: 0x000035B8 File Offset: 0x000017B8
		public static DateTime ToDateTime(string dmtfDate)
		{
			int num = DateTime.MinValue.Year;
			int num2 = DateTime.MinValue.Month;
			int num3 = DateTime.MinValue.Day;
			int num4 = DateTime.MinValue.Hour;
			int num5 = DateTime.MinValue.Minute;
			int num6 = DateTime.MinValue.Second;
			int millisecond = 0;
			DateTime dateTime = DateTime.MinValue;
			if (dmtfDate == null)
			{
				throw new ArgumentOutOfRangeException("dmtfDate");
			}
			if (dmtfDate.Length == 0)
			{
				throw new ArgumentOutOfRangeException("dmtfDate");
			}
			if (dmtfDate.Length != 25)
			{
				throw new ArgumentOutOfRangeException("dmtfDate");
			}
			IFormatProvider provider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			long num7 = 0L;
			try
			{
				string text = string.Empty;
				text = dmtfDate.Substring(0, 4);
				if ("****" != text)
				{
					num = int.Parse(text, provider);
				}
				text = dmtfDate.Substring(4, 2);
				if ("**" != text)
				{
					num2 = int.Parse(text, provider);
				}
				text = dmtfDate.Substring(6, 2);
				if ("**" != text)
				{
					num3 = int.Parse(text, provider);
				}
				text = dmtfDate.Substring(8, 2);
				if ("**" != text)
				{
					num4 = int.Parse(text, provider);
				}
				text = dmtfDate.Substring(10, 2);
				if ("**" != text)
				{
					num5 = int.Parse(text, provider);
				}
				text = dmtfDate.Substring(12, 2);
				if ("**" != text)
				{
					num6 = int.Parse(text, provider);
				}
				text = dmtfDate.Substring(15, 6);
				if ("******" != text)
				{
					num7 = long.Parse(text, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long))) * 10L;
				}
				if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0 || num6 < 0 || num7 < 0L)
				{
					throw new ArgumentOutOfRangeException("dmtfDate");
				}
			}
			catch
			{
				throw new ArgumentOutOfRangeException("dmtfDate");
			}
			dateTime = new DateTime(num, num2, num3, num4, num5, num6, millisecond);
			dateTime = dateTime.AddTicks(num7);
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			long num8 = currentTimeZone.GetUtcOffset(dateTime).Ticks / 600000000L;
			int num9 = 0;
			string text2 = dmtfDate.Substring(22, 3);
			if ("***" != text2)
			{
				text2 = dmtfDate.Substring(21, 4);
				try
				{
					num9 = int.Parse(text2, provider);
				}
				catch
				{
					throw new ArgumentOutOfRangeException();
				}
				long num10 = (long)num9 - num8;
				dateTime = dateTime.AddMinutes((double)(num10 * -1L));
			}
			return dateTime;
		}

		/// <summary>Converts a given <see cref="T:System.DateTime" /> to DMTF datetime format.          </summary>
		/// <param name="date">A <see cref="T:System.DateTime" /> representing the datetime to be converted to DMTF datetime.</param>
		/// <returns>A string that represents the DMTF datetime for the given <see cref="T:System.DateTime" />.</returns>
		// Token: 0x0600004A RID: 74 RVA: 0x000038A8 File Offset: 0x00001AA8
		public static string ToDmtfDateTime(DateTime date)
		{
			string str = string.Empty;
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			TimeSpan utcOffset = currentTimeZone.GetUtcOffset(date);
			long value = utcOffset.Ticks / 600000000L;
			IFormatProvider provider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			if (Math.Abs(value) > 999L)
			{
				date = date.ToUniversalTime();
				str = "+000";
			}
			else if (utcOffset.Ticks >= 0L)
			{
				str = "+" + (utcOffset.Ticks / 600000000L).ToString(provider).PadLeft(3, '0');
			}
			else
			{
				string text = value.ToString(provider);
				str = "-" + text.Substring(1, text.Length - 1).PadLeft(3, '0');
			}
			string str2 = date.Year.ToString(provider).PadLeft(4, '0');
			str2 += date.Month.ToString(provider).PadLeft(2, '0');
			str2 += date.Day.ToString(provider).PadLeft(2, '0');
			str2 += date.Hour.ToString(provider).PadLeft(2, '0');
			str2 += date.Minute.ToString(provider).PadLeft(2, '0');
			str2 += date.Second.ToString(provider).PadLeft(2, '0');
			str2 += ".";
			DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
			string text2 = ((date.Ticks - dateTime.Ticks) * 1000L / 10000L).ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long)));
			if (text2.Length > 6)
			{
				text2 = text2.Substring(0, 6);
			}
			str2 += text2.PadLeft(6, '0');
			return str2 + str;
		}

		/// <summary>Converts a given DMTF time interval to a <see cref="T:System.TimeSpan" />.          </summary>
		/// <param name="dmtfTimespan">A string representation of the DMTF time interval.</param>
		/// <returns>A <see cref="T:System.TimeSpan" /> that represents the given DMTF time interval.</returns>
		// Token: 0x0600004B RID: 75 RVA: 0x00003AFC File Offset: 0x00001CFC
		public static TimeSpan ToTimeSpan(string dmtfTimespan)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			IFormatProvider provider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			TimeSpan timeSpan = TimeSpan.MinValue;
			if (dmtfTimespan == null)
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			if (dmtfTimespan.Length == 0)
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			if (dmtfTimespan.Length != 25)
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			if (dmtfTimespan.Substring(21, 4) != ":000")
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			long num5 = 0L;
			try
			{
				string s = string.Empty;
				s = dmtfTimespan.Substring(0, 8);
				num = int.Parse(s, provider);
				s = dmtfTimespan.Substring(8, 2);
				num2 = int.Parse(s, provider);
				s = dmtfTimespan.Substring(10, 2);
				num3 = int.Parse(s, provider);
				s = dmtfTimespan.Substring(12, 2);
				num4 = int.Parse(s, provider);
				s = dmtfTimespan.Substring(15, 6);
				num5 = long.Parse(s, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long))) * 10L;
			}
			catch
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0L)
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			timeSpan = new TimeSpan(num, num2, num3, num4, 0);
			TimeSpan t = TimeSpan.FromTicks(num5);
			timeSpan += t;
			return timeSpan;
		}

		/// <summary>Converts a given <see cref="T:System.TimeSpan" /> to DMTF time interval.          </summary>
		/// <param name="timespan">A <see cref="T:System.TimeSpan" /> representing the datetime to be converted to DMTF time interval.             </param>
		/// <returns>A string that represents the DMTF time interval for the given <see cref="T:System.TimeSpan" />.</returns>
		// Token: 0x0600004C RID: 76 RVA: 0x00003C84 File Offset: 0x00001E84
		public static string ToDmtfTimeInterval(TimeSpan timespan)
		{
			string str = timespan.Days.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int))).PadLeft(8, '0');
			IFormatProvider provider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			if ((long)timespan.Days > 99999999L || timespan < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException();
			}
			str += timespan.Hours.ToString(provider).PadLeft(2, '0');
			str += timespan.Minutes.ToString(provider).PadLeft(2, '0');
			str += timespan.Seconds.ToString(provider).PadLeft(2, '0');
			str += ".";
			TimeSpan timeSpan = new TimeSpan(timespan.Days, timespan.Hours, timespan.Minutes, timespan.Seconds, 0);
			string text = ((timespan.Ticks - timeSpan.Ticks) * 1000L / 10000L).ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long)));
			if (text.Length > 6)
			{
				text = text.Substring(0, 6);
			}
			str += text.PadLeft(6, '0');
			return str + ":000";
		}

		// Token: 0x04000078 RID: 120
		private const int SIZEOFDMTFDATETIME = 25;

		// Token: 0x04000079 RID: 121
		private const int MAXSIZE_UTC_DMTF = 999;

		// Token: 0x0400007A RID: 122
		private const long MAXDATE_INTIMESPAN = 99999999L;
	}
}
