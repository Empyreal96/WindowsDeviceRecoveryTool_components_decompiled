using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace Microsoft.Data.Edm.Csdl.Internal
{
	// Token: 0x02000006 RID: 6
	internal static class EdmValueParser
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000021E4 File Offset: 0x000003E4
		internal static bool TryParseBinary(string value, out byte[] result)
		{
			if (value.Length % 2 != 0)
			{
				result = null;
				return false;
			}
			result = new byte[value.Length >> 1];
			for (int i = 0; i < value.Length; i++)
			{
				byte b;
				if (!EdmValueParser.TryParseCharAsBinary(value[i], out b))
				{
					result = null;
					return false;
				}
				byte b2;
				if (!EdmValueParser.TryParseCharAsBinary(value[++i], out b2))
				{
					result = null;
					return false;
				}
				result[i >> 1] = (byte)((int)b << 4 | (int)b2);
			}
			return true;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000225C File Offset: 0x0000045C
		internal static bool TryParseBool(string value, out bool? result)
		{
			switch (value.Length)
			{
			case 1:
				switch (value[0])
				{
				case '0':
					result = new bool?(false);
					return true;
				case '1':
					result = new bool?(true);
					return true;
				}
				break;
			case 4:
				if ((value[0] == 't' || value[0] == 'T') && (value[1] == 'r' || value[1] == 'R') && (value[2] == 'u' || value[2] == 'U') && (value[3] == 'e' || value[3] == 'E'))
				{
					result = new bool?(true);
					return true;
				}
				break;
			case 5:
				if ((value[0] == 'f' || value[0] == 'F') && (value[1] == 'a' || value[1] == 'A') && (value[2] == 'l' || value[2] == 'L') && (value[3] == 's' || value[3] == 'S') && (value[4] == 'e' || value[4] == 'E'))
				{
					result = new bool?(false);
					return true;
				}
				break;
			}
			result = null;
			return false;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023C4 File Offset: 0x000005C4
		internal static bool TryParseDateTime(string value, out DateTime? result)
		{
			bool result2;
			try
			{
				result = new DateTime?(PlatformHelper.ConvertStringToDateTime(value));
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002404 File Offset: 0x00000604
		internal static bool TryParseTime(string value, out TimeSpan? result)
		{
			if (!EdmValueParser.TimeValidator.IsMatch(value))
			{
				result = null;
				return false;
			}
			bool result2;
			try
			{
				result = new TimeSpan?(TimeSpan.Parse("00:" + value, CultureInfo.InvariantCulture));
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002468 File Offset: 0x00000668
		internal static bool TryParseDateTimeOffset(string value, out DateTimeOffset? result)
		{
			bool result2;
			try
			{
				result = new DateTimeOffset?(PlatformHelper.ConvertStringToDateTimeOffset(value));
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024A8 File Offset: 0x000006A8
		internal static bool TryParseInt(string value, out int? result)
		{
			bool result2;
			try
			{
				result = new int?(XmlConvert.ToInt32(value));
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024E8 File Offset: 0x000006E8
		internal static bool TryParseLong(string value, out long? result)
		{
			bool result2;
			try
			{
				result = new long?(XmlConvert.ToInt64(value));
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002528 File Offset: 0x00000728
		internal static bool TryParseDecimal(string value, out decimal? result)
		{
			bool result2;
			try
			{
				result = new decimal?(XmlConvert.ToDecimal(value));
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002568 File Offset: 0x00000768
		internal static bool TryParseFloat(string value, out double? result)
		{
			bool result2;
			try
			{
				result = new double?(XmlConvert.ToDouble(value));
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000025A8 File Offset: 0x000007A8
		internal static bool TryParseGuid(string value, out Guid? result)
		{
			bool result2;
			try
			{
				result = new Guid?(XmlConvert.ToGuid(value));
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025E8 File Offset: 0x000007E8
		private static bool TryParseCharAsBinary(char c, out byte b)
		{
			uint num = (uint)(c - '0');
			if (num >= 0U && num <= 9U)
			{
				b = (byte)num;
				return true;
			}
			num = (uint)(c - 'A');
			if (num < 0U || num > 5U)
			{
				num = (uint)(c - 'a');
			}
			if (num >= 0U && num <= 5U)
			{
				b = (byte)(num + 10U);
				return true;
			}
			b = 0;
			return false;
		}

		// Token: 0x04000008 RID: 8
		private const string TimeExp = "[0-9]{2}:[0-9]{2}:[0-9]{2}(\\.[0-9]{0,3})?";

		// Token: 0x04000009 RID: 9
		private static Regex TimeValidator = PlatformHelper.CreateCompiled("^[0-9]{2}:[0-9]{2}:[0-9]{2}(\\.[0-9]{0,3})?$", RegexOptions.Singleline);
	}
}
