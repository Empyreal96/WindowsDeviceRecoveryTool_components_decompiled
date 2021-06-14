using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000102 RID: 258
	internal static class StringUtils
	{
		// Token: 0x06000C02 RID: 3074 RVA: 0x0003121C File Offset: 0x0002F41C
		public static string FormatWith(this string format, IFormatProvider provider, object arg0)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0
			});
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0003123C File Offset: 0x0002F43C
		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1
			});
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00031260 File Offset: 0x0002F460
		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1,
				arg2
			});
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0003128C File Offset: 0x0002F48C
		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2, object arg3)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1,
				arg2,
				arg3
			});
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000312BA File Offset: 0x0002F4BA
		private static string FormatWith(this string format, IFormatProvider provider, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000312D0 File Offset: 0x0002F4D0
		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00031317 File Offset: 0x0002F517
		public static string NullEmptyString(string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				return s;
			}
			return null;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00031324 File Offset: 0x0002F524
		public static StringWriter CreateStringWriter(int capacity)
		{
			StringBuilder sb = new StringBuilder(capacity);
			return new StringWriter(sb, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00031348 File Offset: 0x0002F548
		public static int? GetLength(string value)
		{
			if (value == null)
			{
				return null;
			}
			return new int?(value.Length);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00031370 File Offset: 0x0002F570
		public static void ToCharAsUnicode(char c, char[] buffer)
		{
			buffer[0] = '\\';
			buffer[1] = 'u';
			buffer[2] = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
			buffer[3] = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
			buffer[4] = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
			buffer[5] = MathUtils.IntToHex((int)(c & '\u000f'));
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000313FC File Offset: 0x0002F5FC
		public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			IEnumerable<TSource> source2 = from s in source
			where string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase)
			select s;
			if (source2.Count<TSource>() <= 1)
			{
				return source2.SingleOrDefault<TSource>();
			}
			IEnumerable<TSource> source3 = from s in source
			where string.Equals(valueSelector(s), testValue, StringComparison.Ordinal)
			select s;
			return source3.SingleOrDefault<TSource>();
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00031484 File Offset: 0x0002F684
		public static string ToCamelCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			if (!char.IsUpper(s[0]))
			{
				return s;
			}
			char[] array = s.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = i + 1 < array.Length;
				if (i > 0 && flag && !char.IsUpper(array[i + 1]))
				{
					break;
				}
				array[i] = char.ToLower(array[i], CultureInfo.InvariantCulture);
			}
			return new string(array);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000314F2 File Offset: 0x0002F6F2
		public static bool IsHighSurrogate(char c)
		{
			return char.IsHighSurrogate(c);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000314FA File Offset: 0x0002F6FA
		public static bool IsLowSurrogate(char c)
		{
			return char.IsLowSurrogate(c);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00031502 File Offset: 0x0002F702
		public static bool StartsWith(this string source, char value)
		{
			return source.Length > 0 && source[0] == value;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00031519 File Offset: 0x0002F719
		public static bool EndsWith(this string source, char value)
		{
			return source.Length > 0 && source[source.Length - 1] == value;
		}

		// Token: 0x0400045B RID: 1115
		public const string CarriageReturnLineFeed = "\r\n";

		// Token: 0x0400045C RID: 1116
		public const string Empty = "";

		// Token: 0x0400045D RID: 1117
		public const char CarriageReturn = '\r';

		// Token: 0x0400045E RID: 1118
		public const char LineFeed = '\n';

		// Token: 0x0400045F RID: 1119
		public const char Tab = '\t';
	}
}
