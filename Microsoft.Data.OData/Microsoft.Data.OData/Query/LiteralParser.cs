using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000036 RID: 54
	internal abstract class LiteralParser
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000066B7 File Offset: 0x000048B7
		internal static LiteralParser ForETags
		{
			get
			{
				return LiteralParser.DefaultInstance;
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000066BE File Offset: 0x000048BE
		internal static LiteralParser ForKeys(bool keyAsSegment)
		{
			if (!keyAsSegment)
			{
				return LiteralParser.DefaultInstance;
			}
			return LiteralParser.KeysAsSegmentsInstance;
		}

		// Token: 0x06000167 RID: 359
		internal abstract bool TryParseLiteral(Type targetType, string text, out object result);

		// Token: 0x06000169 RID: 361 RVA: 0x000066D8 File Offset: 0x000048D8
		// Note: this type is marked as 'beforefieldinit'.
		static LiteralParser()
		{
			Dictionary<Type, LiteralParser.PrimitiveParser> dictionary = new Dictionary<Type, LiteralParser.PrimitiveParser>(ReferenceEqualityComparer<Type>.Instance);
			dictionary.Add(typeof(byte[]), new LiteralParser.BinaryPrimitiveParser());
			dictionary.Add(typeof(string), new LiteralParser.StringPrimitiveParser());
			dictionary.Add(typeof(decimal), new LiteralParser.DecimalPrimitiveParser());
			dictionary.Add(typeof(bool), LiteralParser.DelegatingPrimitiveParser<bool>.WithoutMarkup(new Func<string, bool>(XmlConvert.ToBoolean)));
			dictionary.Add(typeof(byte), LiteralParser.DelegatingPrimitiveParser<byte>.WithoutMarkup(new Func<string, byte>(XmlConvert.ToByte)));
			dictionary.Add(typeof(sbyte), LiteralParser.DelegatingPrimitiveParser<sbyte>.WithoutMarkup(new Func<string, sbyte>(XmlConvert.ToSByte)));
			dictionary.Add(typeof(short), LiteralParser.DelegatingPrimitiveParser<short>.WithoutMarkup(new Func<string, short>(XmlConvert.ToInt16)));
			dictionary.Add(typeof(int), LiteralParser.DelegatingPrimitiveParser<int>.WithoutMarkup(new Func<string, int>(XmlConvert.ToInt32)));
			dictionary.Add(typeof(Guid), LiteralParser.DelegatingPrimitiveParser<Guid>.WithPrefix(new Func<string, Guid>(XmlConvert.ToGuid), "guid"));
			dictionary.Add(typeof(DateTime), LiteralParser.DelegatingPrimitiveParser<DateTime>.WithPrefix((string s) => PlatformHelper.ConvertStringToDateTime(s), "datetime"));
			dictionary.Add(typeof(DateTimeOffset), LiteralParser.DelegatingPrimitiveParser<DateTimeOffset>.WithPrefix(new Func<string, DateTimeOffset>(XmlConvert.ToDateTimeOffset), "datetimeoffset"));
			dictionary.Add(typeof(TimeSpan), LiteralParser.DelegatingPrimitiveParser<TimeSpan>.WithPrefix(new Func<string, TimeSpan>(XmlConvert.ToTimeSpan), "time"));
			dictionary.Add(typeof(long), LiteralParser.DelegatingPrimitiveParser<long>.WithSuffix(new Func<string, long>(XmlConvert.ToInt64), "L"));
			dictionary.Add(typeof(float), LiteralParser.DelegatingPrimitiveParser<float>.WithSuffix(new Func<string, float>(XmlConvert.ToSingle), "f"));
			dictionary.Add(typeof(double), LiteralParser.DelegatingPrimitiveParser<double>.WithSuffix(new Func<string, double>(XmlConvert.ToDouble), "D", false));
			LiteralParser.Parsers = dictionary;
		}

		// Token: 0x04000069 RID: 105
		private static readonly LiteralParser DefaultInstance = new LiteralParser.DefaultLiteralParser();

		// Token: 0x0400006A RID: 106
		private static readonly LiteralParser KeysAsSegmentsInstance = new LiteralParser.KeysAsSegmentsLiteralParser();

		// Token: 0x0400006B RID: 107
		private static readonly IDictionary<Type, LiteralParser.PrimitiveParser> Parsers;

		// Token: 0x02000037 RID: 55
		private sealed class DefaultLiteralParser : LiteralParser
		{
			// Token: 0x0600016B RID: 363 RVA: 0x00006914 File Offset: 0x00004B14
			internal override bool TryParseLiteral(Type targetType, string text, out object result)
			{
				targetType = (Nullable.GetUnderlyingType(targetType) ?? targetType);
				bool flag = LiteralParser.DefaultLiteralParser.TryRemoveFormattingAndConvert(text, typeof(byte[]), out result);
				if (flag)
				{
					byte[] array = (byte[])result;
					if (targetType == typeof(byte[]))
					{
						result = array;
						return true;
					}
					string @string = Encoding.UTF8.GetString(array, 0, array.Length);
					return LiteralParser.DefaultLiteralParser.TryRemoveFormattingAndConvert(@string, targetType, out result);
				}
				else
				{
					if (targetType == typeof(byte[]))
					{
						result = null;
						return false;
					}
					return LiteralParser.DefaultLiteralParser.TryRemoveFormattingAndConvert(text, targetType, out result);
				}
			}

			// Token: 0x0600016C RID: 364 RVA: 0x0000699C File Offset: 0x00004B9C
			private static bool TryRemoveFormattingAndConvert(string text, Type targetType, out object targetValue)
			{
				LiteralParser.PrimitiveParser primitiveParser = LiteralParser.Parsers[targetType];
				if (!primitiveParser.TryRemoveFormatting(ref text))
				{
					targetValue = null;
					return false;
				}
				return primitiveParser.TryConvert(text, out targetValue);
			}
		}

		// Token: 0x02000038 RID: 56
		private sealed class KeysAsSegmentsLiteralParser : LiteralParser
		{
			// Token: 0x0600016E RID: 366 RVA: 0x000069D4 File Offset: 0x00004BD4
			internal override bool TryParseLiteral(Type targetType, string text, out object result)
			{
				text = LiteralParser.KeysAsSegmentsLiteralParser.UnescapeLeadingDollarSign(text);
				targetType = (Nullable.GetUnderlyingType(targetType) ?? targetType);
				return LiteralParser.Parsers[targetType].TryConvert(text, out result);
			}

			// Token: 0x0600016F RID: 367 RVA: 0x000069FD File Offset: 0x00004BFD
			private static string UnescapeLeadingDollarSign(string text)
			{
				if (text.Length > 1 && text[0] == '$')
				{
					text = text.Substring(1);
				}
				return text;
			}
		}

		// Token: 0x02000039 RID: 57
		private abstract class PrimitiveParser
		{
			// Token: 0x06000171 RID: 369 RVA: 0x00006A25 File Offset: 0x00004C25
			protected PrimitiveParser(Type expectedType, string suffix, bool suffixRequired) : this(expectedType)
			{
				this.prefix = null;
				this.suffix = suffix;
				this.suffixRequired = suffixRequired;
			}

			// Token: 0x06000172 RID: 370 RVA: 0x00006A43 File Offset: 0x00004C43
			protected PrimitiveParser(Type expectedType, string prefix) : this(expectedType)
			{
				this.prefix = prefix;
				this.suffix = null;
				this.suffixRequired = false;
			}

			// Token: 0x06000173 RID: 371 RVA: 0x00006A61 File Offset: 0x00004C61
			protected PrimitiveParser(Type expectedType)
			{
				this.expectedType = expectedType;
			}

			// Token: 0x06000174 RID: 372
			internal abstract bool TryConvert(string text, out object targetValue);

			// Token: 0x06000175 RID: 373 RVA: 0x00006A70 File Offset: 0x00004C70
			internal virtual bool TryRemoveFormatting(ref string text)
			{
				if (this.prefix != null && !UriPrimitiveTypeParser.TryRemovePrefix(this.prefix, ref text))
				{
					return false;
				}
				bool flag = this.prefix != null || LiteralParser.PrimitiveParser.ValueOfTypeCanContainQuotes(this.expectedType);
				return (!flag || UriPrimitiveTypeParser.TryRemoveQuotes(ref text)) && (this.suffix == null || LiteralParser.PrimitiveParser.TryRemoveLiteralSuffix(this.suffix, ref text) || !this.suffixRequired);
			}

			// Token: 0x06000176 RID: 374 RVA: 0x00006ADA File Offset: 0x00004CDA
			private static bool ValueOfTypeCanContainQuotes(Type type)
			{
				return type == typeof(string);
			}

			// Token: 0x06000177 RID: 375 RVA: 0x00006AEC File Offset: 0x00004CEC
			private static bool TryRemoveLiteralSuffix(string suffix, ref string text)
			{
				text = text.Trim(LiteralParser.PrimitiveParser.XmlWhitespaceChars);
				if (text.Length <= suffix.Length || !text.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				text = text.Substring(0, text.Length - suffix.Length);
				return true;
			}

			// Token: 0x0400006D RID: 109
			private static readonly char[] XmlWhitespaceChars = new char[]
			{
				' ',
				'\t',
				'\n',
				'\r'
			};

			// Token: 0x0400006E RID: 110
			private readonly string prefix;

			// Token: 0x0400006F RID: 111
			private readonly string suffix;

			// Token: 0x04000070 RID: 112
			private readonly bool suffixRequired;

			// Token: 0x04000071 RID: 113
			private readonly Type expectedType;
		}

		// Token: 0x0200003A RID: 58
		private class DelegatingPrimitiveParser<T> : LiteralParser.PrimitiveParser
		{
			// Token: 0x06000179 RID: 377 RVA: 0x00006B60 File Offset: 0x00004D60
			protected DelegatingPrimitiveParser(Func<string, T> convertMethod, string suffix, bool suffixRequired) : base(typeof(T), suffix, suffixRequired)
			{
				this.convertMethod = convertMethod;
			}

			// Token: 0x0600017A RID: 378 RVA: 0x00006B7B File Offset: 0x00004D7B
			private DelegatingPrimitiveParser(Func<string, T> convertMethod) : base(typeof(T))
			{
				this.convertMethod = convertMethod;
			}

			// Token: 0x0600017B RID: 379 RVA: 0x00006B94 File Offset: 0x00004D94
			private DelegatingPrimitiveParser(Func<string, T> convertMethod, string prefix) : base(typeof(T), prefix)
			{
				this.convertMethod = convertMethod;
			}

			// Token: 0x0600017C RID: 380 RVA: 0x00006BAE File Offset: 0x00004DAE
			internal static LiteralParser.DelegatingPrimitiveParser<T> WithoutMarkup(Func<string, T> convertMethod)
			{
				return new LiteralParser.DelegatingPrimitiveParser<T>(convertMethod);
			}

			// Token: 0x0600017D RID: 381 RVA: 0x00006BB6 File Offset: 0x00004DB6
			internal static LiteralParser.DelegatingPrimitiveParser<T> WithPrefix(Func<string, T> convertMethod, string prefix)
			{
				return new LiteralParser.DelegatingPrimitiveParser<T>(convertMethod, prefix);
			}

			// Token: 0x0600017E RID: 382 RVA: 0x00006BBF File Offset: 0x00004DBF
			internal static LiteralParser.DelegatingPrimitiveParser<T> WithSuffix(Func<string, T> convertMethod, string suffix)
			{
				return LiteralParser.DelegatingPrimitiveParser<T>.WithSuffix(convertMethod, suffix, true);
			}

			// Token: 0x0600017F RID: 383 RVA: 0x00006BC9 File Offset: 0x00004DC9
			internal static LiteralParser.DelegatingPrimitiveParser<T> WithSuffix(Func<string, T> convertMethod, string suffix, bool required)
			{
				return new LiteralParser.DelegatingPrimitiveParser<T>(convertMethod, suffix, required);
			}

			// Token: 0x06000180 RID: 384 RVA: 0x00006BD4 File Offset: 0x00004DD4
			internal override bool TryConvert(string text, out object targetValue)
			{
				bool result;
				try
				{
					targetValue = this.convertMethod(text);
					result = true;
				}
				catch (FormatException)
				{
					targetValue = default(T);
					result = false;
				}
				catch (OverflowException)
				{
					targetValue = default(T);
					result = false;
				}
				return result;
			}

			// Token: 0x04000072 RID: 114
			private readonly Func<string, T> convertMethod;
		}

		// Token: 0x0200003B RID: 59
		private sealed class DecimalPrimitiveParser : LiteralParser.DelegatingPrimitiveParser<decimal>
		{
			// Token: 0x06000181 RID: 385 RVA: 0x00006C40 File Offset: 0x00004E40
			internal DecimalPrimitiveParser() : base(new Func<string, decimal>(LiteralParser.DecimalPrimitiveParser.ConvertDecimal), "M", true)
			{
			}

			// Token: 0x06000182 RID: 386 RVA: 0x00006C5C File Offset: 0x00004E5C
			private static decimal ConvertDecimal(string text)
			{
				decimal result;
				try
				{
					result = XmlConvert.ToDecimal(text);
				}
				catch (FormatException)
				{
					decimal num;
					if (!decimal.TryParse(text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num))
					{
						throw;
					}
					result = num;
				}
				return result;
			}
		}

		// Token: 0x0200003C RID: 60
		private sealed class BinaryPrimitiveParser : LiteralParser.PrimitiveParser
		{
			// Token: 0x06000183 RID: 387 RVA: 0x00006CA0 File Offset: 0x00004EA0
			internal BinaryPrimitiveParser() : base(typeof(byte[]))
			{
			}

			// Token: 0x06000184 RID: 388 RVA: 0x00006CB4 File Offset: 0x00004EB4
			internal override bool TryConvert(string text, out object targetValue)
			{
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
					array[i] = (byte)(LiteralParser.BinaryPrimitiveParser.HexCharToNibble(c) << 4) + LiteralParser.BinaryPrimitiveParser.HexCharToNibble(c2);
					num += 2;
					i++;
				}
				targetValue = array;
				return true;
			}

			// Token: 0x06000185 RID: 389 RVA: 0x00006D34 File Offset: 0x00004F34
			internal override bool TryRemoveFormatting(ref string text)
			{
				return (UriPrimitiveTypeParser.TryRemovePrefix("binary", ref text) || UriPrimitiveTypeParser.TryRemovePrefix("X", ref text)) && UriPrimitiveTypeParser.TryRemoveQuotes(ref text);
			}

			// Token: 0x06000186 RID: 390 RVA: 0x00006D60 File Offset: 0x00004F60
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
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0200003D RID: 61
		private sealed class StringPrimitiveParser : LiteralParser.PrimitiveParser
		{
			// Token: 0x06000187 RID: 391 RVA: 0x00006E23 File Offset: 0x00005023
			public StringPrimitiveParser() : base(typeof(string))
			{
			}

			// Token: 0x06000188 RID: 392 RVA: 0x00006E35 File Offset: 0x00005035
			internal override bool TryConvert(string text, out object targetValue)
			{
				targetValue = text;
				return true;
			}

			// Token: 0x06000189 RID: 393 RVA: 0x00006E3B File Offset: 0x0000503B
			internal override bool TryRemoveFormatting(ref string text)
			{
				return UriPrimitiveTypeParser.TryRemoveQuotes(ref text);
			}
		}
	}
}
