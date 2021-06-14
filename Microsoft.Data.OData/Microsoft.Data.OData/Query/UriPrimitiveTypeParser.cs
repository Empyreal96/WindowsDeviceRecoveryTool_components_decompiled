using System;
using System.Globalization;
using System.Spatial;
using System.Text;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000D9 RID: 217
	internal static class UriPrimitiveTypeParser
	{
		// Token: 0x06000542 RID: 1346 RVA: 0x00011EB1 File Offset: 0x000100B1
		internal static bool IsCharHexDigit(char c)
		{
			return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00011ED8 File Offset: 0x000100D8
		internal static bool TryUriStringToPrimitive(string text, IEdmTypeReference targetType, out object targetValue)
		{
			if (targetType.IsNullable && text == "null")
			{
				targetValue = null;
				return true;
			}
			IEdmPrimitiveTypeReference edmPrimitiveTypeReference = targetType.AsPrimitiveOrNull();
			if (edmPrimitiveTypeReference == null)
			{
				targetValue = null;
				return false;
			}
			EdmPrimitiveTypeKind edmPrimitiveTypeKind = edmPrimitiveTypeReference.PrimitiveKind();
			byte[] array;
			bool flag = UriPrimitiveTypeParser.TryUriStringToByteArray(text, out array);
			if (edmPrimitiveTypeKind == EdmPrimitiveTypeKind.Binary)
			{
				targetValue = array;
				return flag;
			}
			if (flag)
			{
				string @string = Encoding.UTF8.GetString(array, 0, array.Length);
				return UriPrimitiveTypeParser.TryUriStringToPrimitive(@string, targetType, out targetValue);
			}
			if (edmPrimitiveTypeKind == EdmPrimitiveTypeKind.Guid)
			{
				Guid guid;
				bool result = UriPrimitiveTypeParser.TryUriStringToGuid(text, out guid);
				targetValue = guid;
				return result;
			}
			if (edmPrimitiveTypeKind == EdmPrimitiveTypeKind.DateTime)
			{
				DateTime dateTime;
				bool result2 = UriPrimitiveTypeParser.TryUriStringToDateTime(text, out dateTime);
				targetValue = dateTime;
				return result2;
			}
			if (edmPrimitiveTypeKind == EdmPrimitiveTypeKind.DateTimeOffset)
			{
				DateTimeOffset dateTimeOffset;
				bool result3 = UriPrimitiveTypeParser.TryUriStringToDateTimeOffset(text, out dateTimeOffset);
				targetValue = dateTimeOffset;
				return result3;
			}
			if (edmPrimitiveTypeKind == EdmPrimitiveTypeKind.Time)
			{
				TimeSpan timeSpan;
				bool result4 = UriPrimitiveTypeParser.TryUriStringToTime(text, out timeSpan);
				targetValue = timeSpan;
				return result4;
			}
			if (edmPrimitiveTypeKind == EdmPrimitiveTypeKind.Geography)
			{
				Geography geography;
				bool result5 = UriPrimitiveTypeParser.TryUriStringToGeography(text, out geography);
				targetValue = geography;
				return result5;
			}
			if (edmPrimitiveTypeKind == EdmPrimitiveTypeKind.Geometry)
			{
				Geometry geometry;
				bool result6 = UriPrimitiveTypeParser.TryUriStringToGeometry(text, out geometry);
				targetValue = geometry;
				return result6;
			}
			bool flag2 = edmPrimitiveTypeKind == EdmPrimitiveTypeKind.String;
			if (flag2 != UriPrimitiveTypeParser.IsUriValueQuoted(text))
			{
				targetValue = null;
				return false;
			}
			if (flag2)
			{
				text = UriPrimitiveTypeParser.RemoveQuotes(text);
			}
			bool result7;
			try
			{
				switch (edmPrimitiveTypeKind)
				{
				case EdmPrimitiveTypeKind.Boolean:
					targetValue = XmlConvert.ToBoolean(text);
					goto IL_2B3;
				case EdmPrimitiveTypeKind.Byte:
					targetValue = XmlConvert.ToByte(text);
					goto IL_2B3;
				case EdmPrimitiveTypeKind.Decimal:
					if (UriPrimitiveTypeParser.TryRemoveLiteralSuffix("M", ref text))
					{
						try
						{
							targetValue = XmlConvert.ToDecimal(text);
							goto IL_2B3;
						}
						catch (FormatException)
						{
							decimal num;
							if (decimal.TryParse(text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num))
							{
								targetValue = num;
								goto IL_2B3;
							}
							targetValue = 0m;
							return false;
						}
					}
					targetValue = 0m;
					return false;
				case EdmPrimitiveTypeKind.Double:
					UriPrimitiveTypeParser.TryRemoveLiteralSuffix("D", ref text);
					targetValue = XmlConvert.ToDouble(text);
					goto IL_2B3;
				case EdmPrimitiveTypeKind.Int16:
					targetValue = XmlConvert.ToInt16(text);
					goto IL_2B3;
				case EdmPrimitiveTypeKind.Int32:
					targetValue = XmlConvert.ToInt32(text);
					goto IL_2B3;
				case EdmPrimitiveTypeKind.Int64:
					if (UriPrimitiveTypeParser.TryRemoveLiteralSuffix("L", ref text))
					{
						targetValue = XmlConvert.ToInt64(text);
						goto IL_2B3;
					}
					targetValue = 0L;
					return false;
				case EdmPrimitiveTypeKind.SByte:
					targetValue = XmlConvert.ToSByte(text);
					goto IL_2B3;
				case EdmPrimitiveTypeKind.Single:
					if (UriPrimitiveTypeParser.TryRemoveLiteralSuffix("f", ref text))
					{
						targetValue = XmlConvert.ToSingle(text);
						goto IL_2B3;
					}
					targetValue = 0f;
					return false;
				case EdmPrimitiveTypeKind.String:
					targetValue = text;
					goto IL_2B3;
				}
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.UriPrimitiveTypeParser_TryUriStringToPrimitive));
				IL_2B3:
				result7 = true;
			}
			catch (FormatException)
			{
				targetValue = null;
				result7 = false;
			}
			catch (OverflowException)
			{
				targetValue = null;
				result7 = false;
			}
			return result7;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00012200 File Offset: 0x00010400
		internal static bool TryUriStringToNonNegativeInteger(string text, out int nonNegativeInteger)
		{
			object obj;
			if (!UriPrimitiveTypeParser.TryUriStringToPrimitive(text, EdmCoreModel.Instance.GetInt32(false), out obj))
			{
				nonNegativeInteger = -1;
				return false;
			}
			nonNegativeInteger = (int)obj;
			return nonNegativeInteger >= 0;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00012237 File Offset: 0x00010437
		internal static bool TryRemoveSuffix(string suffix, ref string text)
		{
			return UriPrimitiveTypeParser.TryRemoveLiteralSuffix(suffix, ref text);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00012240 File Offset: 0x00010440
		internal static bool TryRemovePrefix(string prefix, ref string text)
		{
			return UriPrimitiveTypeParser.TryRemoveLiteralPrefix(prefix, ref text);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001224C File Offset: 0x0001044C
		internal static bool TryRemoveQuotes(ref string text)
		{
			if (text.Length < 2)
			{
				return false;
			}
			char c = text[0];
			if (c != '\'' || text[text.Length - 1] != c)
			{
				return false;
			}
			string text2 = text.Substring(1, text.Length - 2);
			int startIndex = 0;
			for (;;)
			{
				int num = text2.IndexOf(c, startIndex);
				if (num < 0)
				{
					goto IL_76;
				}
				text2 = text2.Remove(num, 1);
				if (text2.Length < num + 1 || text2[num] != c)
				{
					break;
				}
				startIndex = num + 1;
			}
			return false;
			IL_76:
			text = text2;
			return true;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000122D4 File Offset: 0x000104D4
		private static bool TryUriStringToByteArray(string text, out byte[] targetValue)
		{
			if (!UriPrimitiveTypeParser.TryRemoveLiteralPrefix("binary", ref text) && !UriPrimitiveTypeParser.TryRemoveLiteralPrefix("X", ref text))
			{
				targetValue = null;
				return false;
			}
			if (!UriPrimitiveTypeParser.TryRemoveQuotes(ref text))
			{
				targetValue = null;
				return false;
			}
			if (text.Length % 2 != 0)
			{
				targetValue = null;
				return false;
			}
			byte[] array = new byte[text.Length / 2];
			int i = 0;
			int num = 0;
			while (i < array.Length)
			{
				char c = text[num];
				char c2 = text[num + 1];
				if (!UriPrimitiveTypeParser.IsCharHexDigit(c) || !UriPrimitiveTypeParser.IsCharHexDigit(c2))
				{
					targetValue = null;
					return false;
				}
				array[i] = (byte)(UriPrimitiveTypeParser.HexCharToNibble(c) << 4) + UriPrimitiveTypeParser.HexCharToNibble(c2);
				num += 2;
				i++;
			}
			targetValue = array;
			return true;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00012384 File Offset: 0x00010584
		private static bool TryUriStringToGuid(string text, out Guid targetValue)
		{
			if (!UriPrimitiveTypeParser.TryRemoveLiteralPrefix("guid", ref text))
			{
				targetValue = default(Guid);
				return false;
			}
			if (!UriPrimitiveTypeParser.TryRemoveQuotes(ref text))
			{
				targetValue = default(Guid);
				return false;
			}
			bool result;
			try
			{
				targetValue = XmlConvert.ToGuid(text);
				result = true;
			}
			catch (FormatException)
			{
				targetValue = default(Guid);
				result = false;
			}
			return result;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000123E8 File Offset: 0x000105E8
		private static bool TryUriStringToDateTime(string text, out DateTime targetValue)
		{
			targetValue = default(DateTime);
			if (!UriPrimitiveTypeParser.TryRemoveLiteralPrefix("datetime", ref text))
			{
				return false;
			}
			if (!UriPrimitiveTypeParser.TryRemoveQuotes(ref text))
			{
				return false;
			}
			bool result;
			try
			{
				targetValue = PlatformHelper.ConvertStringToDateTime(text);
				result = true;
			}
			catch (FormatException)
			{
				result = false;
			}
			catch (ArgumentOutOfRangeException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00012450 File Offset: 0x00010650
		private static bool TryUriStringToDateTimeOffset(string text, out DateTimeOffset targetValue)
		{
			targetValue = default(DateTimeOffset);
			if (!UriPrimitiveTypeParser.TryRemoveLiteralPrefix("datetimeoffset", ref text))
			{
				return false;
			}
			if (!UriPrimitiveTypeParser.TryRemoveQuotes(ref text))
			{
				return false;
			}
			bool result;
			try
			{
				targetValue = PlatformHelper.ConvertStringToDateTimeOffset(text);
				result = true;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000124A8 File Offset: 0x000106A8
		private static bool TryUriStringToTime(string text, out TimeSpan targetValue)
		{
			if (!UriPrimitiveTypeParser.TryRemoveLiteralPrefix("time", ref text))
			{
				targetValue = default(TimeSpan);
				return false;
			}
			if (!UriPrimitiveTypeParser.TryRemoveQuotes(ref text))
			{
				targetValue = default(TimeSpan);
				return false;
			}
			bool result;
			try
			{
				targetValue = XmlConvert.ToTimeSpan(text);
				result = true;
			}
			catch (FormatException)
			{
				targetValue = default(TimeSpan);
				result = false;
			}
			return result;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001250C File Offset: 0x0001070C
		private static bool TryUriStringToGeography(string text, out Geography targetValue)
		{
			if (!UriPrimitiveTypeParser.TryRemoveLiteralPrefix("geography", ref text))
			{
				targetValue = null;
				return false;
			}
			if (!UriPrimitiveTypeParser.TryRemoveQuotes(ref text))
			{
				targetValue = null;
				return false;
			}
			bool result;
			try
			{
				targetValue = LiteralUtils.ParseGeography(text);
				result = true;
			}
			catch (ParseErrorException)
			{
				targetValue = null;
				result = false;
			}
			return result;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00012560 File Offset: 0x00010760
		private static bool TryUriStringToGeometry(string text, out Geometry targetValue)
		{
			if (!UriPrimitiveTypeParser.TryRemoveLiteralPrefix("geometry", ref text))
			{
				targetValue = null;
				return false;
			}
			if (!UriPrimitiveTypeParser.TryRemoveQuotes(ref text))
			{
				targetValue = null;
				return false;
			}
			bool result;
			try
			{
				targetValue = LiteralUtils.ParseGeometry(text);
				result = true;
			}
			catch (ParseErrorException)
			{
				targetValue = null;
				result = false;
			}
			return result;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x000125B4 File Offset: 0x000107B4
		private static bool TryRemoveLiteralSuffix(string suffix, ref string text)
		{
			text = text.Trim(UriPrimitiveTypeParser.WhitespaceChars);
			if (text.Length <= suffix.Length || !text.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			text = text.Substring(0, text.Length - suffix.Length);
			return true;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00012604 File Offset: 0x00010804
		private static bool TryRemoveLiteralPrefix(string prefix, ref string text)
		{
			if (text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
			{
				text = text.Remove(0, prefix.Length);
				return true;
			}
			return false;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00012624 File Offset: 0x00010824
		private static bool IsUriValueQuoted(string text)
		{
			if (text.Length < 2 || text[0] != '\'' || text[text.Length - 1] != '\'')
			{
				return false;
			}
			int num;
			for (int i = 1; i < text.Length - 1; i = num + 2)
			{
				num = text.IndexOf('\'', i, text.Length - i - 1);
				if (num == -1)
				{
					break;
				}
				if (num == text.Length - 2 || text[num + 1] != '\'')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000126A0 File Offset: 0x000108A0
		private static string RemoveQuotes(string text)
		{
			char value = text[0];
			string text2 = text.Substring(1, text.Length - 2);
			int startIndex = 0;
			for (;;)
			{
				int num = text2.IndexOf(value, startIndex);
				if (num < 0)
				{
					break;
				}
				text2 = text2.Remove(num, 1);
				startIndex = num + 1;
			}
			return text2;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000126E4 File Offset: 0x000108E4
		private static byte HexCharToNibble(char c)
		{
			switch (c)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			case ':':
			case ';':
			case '<':
			case '=':
			case '>':
			case '?':
			case '@':
				goto IL_B1;
			case 'A':
				break;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			default:
				switch (c)
				{
				case 'a':
					break;
				case 'b':
					return 11;
				case 'c':
					return 12;
				case 'd':
					return 13;
				case 'e':
					return 14;
				case 'f':
					return 15;
				default:
					goto IL_B1;
				}
				break;
			}
			return 10;
			IL_B1:
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.UriPrimitiveTypeParser_HexCharToNibble));
		}

		// Token: 0x04000228 RID: 552
		private static char[] WhitespaceChars = new char[]
		{
			' ',
			'\t',
			'\n',
			'\r'
		};
	}
}
