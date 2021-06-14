using System;
using System.Globalization;
using MS.Internal.Text;

namespace System.Windows.Controls
{
	// Token: 0x020004C4 RID: 1220
	internal static class DateTimeHelper
	{
		// Token: 0x06004A2A RID: 18986 RVA: 0x0014F500 File Offset: 0x0014D700
		public static DateTime? AddDays(DateTime time, int days)
		{
			DateTime? result;
			try
			{
				result = new DateTime?(DateTimeHelper.cal.AddDays(time, days));
			}
			catch (ArgumentException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x0014F540 File Offset: 0x0014D740
		public static DateTime? AddMonths(DateTime time, int months)
		{
			DateTime? result;
			try
			{
				result = new DateTime?(DateTimeHelper.cal.AddMonths(time, months));
			}
			catch (ArgumentException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x0014F580 File Offset: 0x0014D780
		public static DateTime? AddYears(DateTime time, int years)
		{
			DateTime? result;
			try
			{
				result = new DateTime?(DateTimeHelper.cal.AddYears(time, years));
			}
			catch (ArgumentException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x0014F5C0 File Offset: 0x0014D7C0
		public static DateTime? SetYear(DateTime date, int year)
		{
			return DateTimeHelper.AddYears(date, year - date.Year);
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x0014F5D4 File Offset: 0x0014D7D4
		public static DateTime? SetYearMonth(DateTime date, DateTime yearMonth)
		{
			DateTime? result = DateTimeHelper.SetYear(date, yearMonth.Year);
			if (result != null)
			{
				result = DateTimeHelper.AddMonths(result.Value, yearMonth.Month - date.Month);
			}
			return result;
		}

		// Token: 0x06004A2F RID: 18991 RVA: 0x0014F618 File Offset: 0x0014D818
		public static int CompareDays(DateTime dt1, DateTime dt2)
		{
			return DateTime.Compare(DateTimeHelper.DiscardTime(new DateTime?(dt1)).Value, DateTimeHelper.DiscardTime(new DateTime?(dt2)).Value);
		}

		// Token: 0x06004A30 RID: 18992 RVA: 0x0014F650 File Offset: 0x0014D850
		public static int CompareYearMonth(DateTime dt1, DateTime dt2)
		{
			return (dt1.Year - dt2.Year) * 12 + (dt1.Month - dt2.Month);
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x0014F674 File Offset: 0x0014D874
		public static int DecadeOfDate(DateTime date)
		{
			return date.Year - date.Year % 10;
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x0014F688 File Offset: 0x0014D888
		public static DateTime DiscardDayTime(DateTime d)
		{
			return new DateTime(d.Year, d.Month, 1, 0, 0, 0);
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x0014F6A4 File Offset: 0x0014D8A4
		public static DateTime? DiscardTime(DateTime? d)
		{
			if (d == null)
			{
				return null;
			}
			return new DateTime?(d.Value.Date);
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x0014F6D8 File Offset: 0x0014D8D8
		public static int EndOfDecade(DateTime date)
		{
			return DateTimeHelper.DecadeOfDate(date) + 9;
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x0014F6E3 File Offset: 0x0014D8E3
		public static DateTimeFormatInfo GetCurrentDateFormat()
		{
			return DateTimeHelper.GetDateFormat(CultureInfo.CurrentCulture);
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x0014F6F0 File Offset: 0x0014D8F0
		internal static CultureInfo GetCulture(FrameworkElement element)
		{
			bool flag;
			CultureInfo result;
			if (element.GetValueSource(FrameworkElement.LanguageProperty, null, out flag) != BaseValueSourceInternal.Default)
			{
				result = DynamicPropertyReader.GetCultureInfo(element);
			}
			else
			{
				result = CultureInfo.CurrentCulture;
			}
			return result;
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x0014F720 File Offset: 0x0014D920
		internal static DateTimeFormatInfo GetDateFormat(CultureInfo culture)
		{
			if (culture.Calendar is GregorianCalendar)
			{
				return culture.DateTimeFormat;
			}
			GregorianCalendar gregorianCalendar = null;
			foreach (Calendar calendar in culture.OptionalCalendars)
			{
				if (calendar is GregorianCalendar)
				{
					if (gregorianCalendar == null)
					{
						gregorianCalendar = (calendar as GregorianCalendar);
					}
					if (((GregorianCalendar)calendar).CalendarType == GregorianCalendarTypes.Localized)
					{
						gregorianCalendar = (calendar as GregorianCalendar);
						break;
					}
				}
			}
			DateTimeFormatInfo dateTimeFormat;
			if (gregorianCalendar == null)
			{
				dateTimeFormat = ((CultureInfo)CultureInfo.InvariantCulture.Clone()).DateTimeFormat;
				dateTimeFormat.Calendar = new GregorianCalendar();
			}
			else
			{
				dateTimeFormat = ((CultureInfo)culture.Clone()).DateTimeFormat;
				dateTimeFormat.Calendar = gregorianCalendar;
			}
			return dateTimeFormat;
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x0014F7CA File Offset: 0x0014D9CA
		public static bool InRange(DateTime date, CalendarDateRange range)
		{
			return DateTimeHelper.InRange(date, range.Start, range.End);
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x0014F7DE File Offset: 0x0014D9DE
		public static bool InRange(DateTime date, DateTime start, DateTime end)
		{
			return DateTimeHelper.CompareDays(date, start) > -1 && DateTimeHelper.CompareDays(date, end) < 1;
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x0014F7F8 File Offset: 0x0014D9F8
		public static string ToDayString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				result = date.Value.Day.ToString(dateFormat);
			}
			return result;
		}

		// Token: 0x06004A3B RID: 19003 RVA: 0x0014F838 File Offset: 0x0014DA38
		public static string ToDecadeRangeString(int decade, FrameworkElement fe)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(fe));
			if (dateFormat != null)
			{
				bool flag = fe.FlowDirection == FlowDirection.RightToLeft;
				int num = flag ? decade : (decade + 9);
				result = (flag ? (decade + 9) : decade).ToString(dateFormat) + "-" + num.ToString(dateFormat);
			}
			return result;
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x0014F898 File Offset: 0x0014DA98
		public static string ToYearMonthPatternString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				result = date.Value.ToString(dateFormat.YearMonthPattern, dateFormat);
			}
			return result;
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x0014F8D8 File Offset: 0x0014DAD8
		public static string ToYearString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				result = date.Value.Year.ToString(dateFormat);
			}
			return result;
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x0014F918 File Offset: 0x0014DB18
		public static string ToAbbreviatedMonthString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				string[] abbreviatedMonthNames = dateFormat.AbbreviatedMonthNames;
				if (abbreviatedMonthNames != null && abbreviatedMonthNames.Length != 0)
				{
					result = abbreviatedMonthNames[(date.Value.Month - 1) % abbreviatedMonthNames.Length];
				}
			}
			return result;
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x0014F968 File Offset: 0x0014DB68
		public static string ToLongDateString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				result = date.Value.Date.ToString(dateFormat.LongDatePattern, dateFormat);
			}
			return result;
		}

		// Token: 0x04002A50 RID: 10832
		private static Calendar cal = new GregorianCalendar();
	}
}
