using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Text;
using System.Xml;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000002 RID: 2
	internal abstract class LiteralFormatter
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		internal static LiteralFormatter ForConstants
		{
			get
			{
				return LiteralFormatter.DefaultInstance;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000020D7 File Offset: 0x000002D7
		internal static LiteralFormatter ForConstantsWithoutEncoding
		{
			get
			{
				return LiteralFormatter.DefaultInstanceWithoutEncoding;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020DE File Offset: 0x000002DE
		internal static LiteralFormatter ForKeys(bool keysAsSegment)
		{
			if (!keysAsSegment)
			{
				return LiteralFormatter.DefaultInstance;
			}
			return LiteralFormatter.KeyAsSegmentInstance;
		}

		// Token: 0x06000004 RID: 4
		internal abstract string Format(object value);

		// Token: 0x06000005 RID: 5 RVA: 0x000020EE File Offset: 0x000002EE
		protected virtual string EscapeResultForUri(string result)
		{
			return Uri.EscapeDataString(result);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F8 File Offset: 0x000002F8
		private static string ConvertByteArrayToKeyString(byte[] byteArray)
		{
			StringBuilder stringBuilder = new StringBuilder(3 + byteArray.Length * 2);
			foreach (byte b in byteArray)
			{
				stringBuilder.Append("0123456789ABCDEF"[b >> 4]);
				stringBuilder.Append("0123456789ABCDEF"[(int)(b & 15)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002158 File Offset: 0x00000358
		private static string FormatRawLiteral(object value)
		{
			string text = value as string;
			if (text != null)
			{
				return text;
			}
			if (value is bool)
			{
				return XmlConvert.ToString((bool)value);
			}
			if (value is byte)
			{
				return XmlConvert.ToString((byte)value);
			}
			if (value is DateTime)
			{
				return XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind);
			}
			if (value is decimal)
			{
				return XmlConvert.ToString((decimal)value);
			}
			if (value is double)
			{
				string input = XmlConvert.ToString((double)value);
				return LiteralFormatter.SharedUtils.AppendDecimalMarkerToDouble(input);
			}
			if (value is Guid)
			{
				return value.ToString();
			}
			if (value is short)
			{
				return XmlConvert.ToString((short)value);
			}
			if (value is int)
			{
				return XmlConvert.ToString((int)value);
			}
			if (value is long)
			{
				return XmlConvert.ToString((long)value);
			}
			if (value is sbyte)
			{
				return XmlConvert.ToString((sbyte)value);
			}
			if (value is float)
			{
				return XmlConvert.ToString((float)value);
			}
			byte[] array = value as byte[];
			if (array != null)
			{
				return LiteralFormatter.ConvertByteArrayToKeyString(array);
			}
			if (value is DateTimeOffset)
			{
				return XmlConvert.ToString((DateTimeOffset)value);
			}
			if (value is TimeSpan)
			{
				return XmlConvert.ToString((TimeSpan)value);
			}
			Geography geography = value as Geography;
			if (geography != null)
			{
				return WellKnownTextSqlFormatter.Create(true).Write(geography);
			}
			Geometry geometry = value as Geometry;
			if (geometry != null)
			{
				return WellKnownTextSqlFormatter.Create(true).Write(geometry);
			}
			throw LiteralFormatter.SharedUtils.CreateExceptionForUnconvertableType(value);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022C4 File Offset: 0x000004C4
		private string FormatAndEscapeLiteral(object value)
		{
			string text = LiteralFormatter.FormatRawLiteral(value);
			if (value is string)
			{
				text = text.Replace("'", "''");
			}
			return this.EscapeResultForUri(text);
		}

		// Token: 0x04000001 RID: 1
		private const string HexValues = "0123456789ABCDEF";

		// Token: 0x04000002 RID: 2
		private static readonly LiteralFormatter DefaultInstance = new LiteralFormatter.DefaultLiteralFormatter();

		// Token: 0x04000003 RID: 3
		private static readonly LiteralFormatter DefaultInstanceWithoutEncoding = new LiteralFormatter.DefaultLiteralFormatter(true);

		// Token: 0x04000004 RID: 4
		private static readonly LiteralFormatter KeyAsSegmentInstance = new LiteralFormatter.KeysAsSegmentsLiteralFormatter();

		// Token: 0x02000003 RID: 3
		private static class SharedUtils
		{
			// Token: 0x0600000B RID: 11 RVA: 0x00002321 File Offset: 0x00000521
			internal static InvalidOperationException CreateExceptionForUnconvertableType(object value)
			{
				return new ODataException(Strings.ODataUriUtils_ConvertToUriLiteralUnsupportedType(value.GetType().ToString()));
			}

			// Token: 0x0600000C RID: 12 RVA: 0x00002338 File Offset: 0x00000538
			internal static bool TryConvertToStandardType(object value, out object converted)
			{
				byte[] array;
				if (LiteralFormatter.SharedUtils.TryGetByteArrayFromBinary(value, out array))
				{
					converted = array;
					return true;
				}
				converted = null;
				return false;
			}

			// Token: 0x0600000D RID: 13 RVA: 0x00002358 File Offset: 0x00000558
			internal static string AppendDecimalMarkerToDouble(string input)
			{
				IEnumerable<char> source = input.ToCharArray();
				if (input[0] == '-')
				{
					source = source.Skip(1);
				}
				if (source.All(new Func<char, bool>(char.IsDigit)))
				{
					return input + ".0";
				}
				return input;
			}

			// Token: 0x0600000E RID: 14 RVA: 0x000023A0 File Offset: 0x000005A0
			internal static bool ShouldAppendLiteralSuffixToDouble(double value)
			{
				return !double.IsInfinity(value) && !double.IsNaN(value);
			}

			// Token: 0x0600000F RID: 15 RVA: 0x000023B5 File Offset: 0x000005B5
			private static bool TryGetByteArrayFromBinary(object value, out byte[] array)
			{
				array = null;
				return false;
			}
		}

		// Token: 0x02000004 RID: 4
		private sealed class DefaultLiteralFormatter : LiteralFormatter
		{
			// Token: 0x06000010 RID: 16 RVA: 0x000023BB File Offset: 0x000005BB
			internal DefaultLiteralFormatter() : this(false)
			{
			}

			// Token: 0x06000011 RID: 17 RVA: 0x000023C4 File Offset: 0x000005C4
			internal DefaultLiteralFormatter(bool disableUrlEncoding)
			{
				this.disableUrlEncoding = disableUrlEncoding;
			}

			// Token: 0x06000012 RID: 18 RVA: 0x000023D4 File Offset: 0x000005D4
			internal override string Format(object value)
			{
				object obj;
				if (LiteralFormatter.SharedUtils.TryConvertToStandardType(value, out obj))
				{
					value = obj;
				}
				return this.FormatLiteralWithTypePrefix(value);
			}

			// Token: 0x06000013 RID: 19 RVA: 0x000023F5 File Offset: 0x000005F5
			protected override string EscapeResultForUri(string result)
			{
				if (!this.disableUrlEncoding)
				{
					result = base.EscapeResultForUri(result);
				}
				return result;
			}

			// Token: 0x06000014 RID: 20 RVA: 0x0000240C File Offset: 0x0000060C
			private string FormatLiteralWithTypePrefix(object value)
			{
				string text = base.FormatAndEscapeLiteral(value);
				if (value is byte[])
				{
					return "X'" + text + "'";
				}
				if (value is DateTime)
				{
					return "datetime'" + text + "'";
				}
				if (value is DateTimeOffset)
				{
					return "datetimeoffset'" + text + "'";
				}
				if (value is decimal)
				{
					return text + "M";
				}
				if (value is Guid)
				{
					return "guid'" + text + "'";
				}
				if (value is long)
				{
					return text + "L";
				}
				if (value is float)
				{
					return text + "f";
				}
				if (value is double)
				{
					if (LiteralFormatter.SharedUtils.ShouldAppendLiteralSuffixToDouble((double)value))
					{
						return text + "D";
					}
					return text;
				}
				else
				{
					if (value is Geography)
					{
						return "geography'" + text + "'";
					}
					if (value is Geometry)
					{
						return "geometry'" + text + "'";
					}
					if (value is TimeSpan)
					{
						return "time'" + text + "'";
					}
					if (value is string)
					{
						return "'" + text + "'";
					}
					return text;
				}
			}

			// Token: 0x04000005 RID: 5
			private readonly bool disableUrlEncoding;
		}

		// Token: 0x02000005 RID: 5
		private sealed class KeysAsSegmentsLiteralFormatter : LiteralFormatter
		{
			// Token: 0x06000015 RID: 21 RVA: 0x00002549 File Offset: 0x00000749
			internal KeysAsSegmentsLiteralFormatter()
			{
			}

			// Token: 0x06000016 RID: 22 RVA: 0x00002554 File Offset: 0x00000754
			internal override string Format(object value)
			{
				object obj;
				if (LiteralFormatter.SharedUtils.TryConvertToStandardType(value, out obj))
				{
					value = obj;
				}
				string text = value as string;
				if (text != null)
				{
					value = LiteralFormatter.KeysAsSegmentsLiteralFormatter.EscapeLeadingDollarSign(text);
				}
				return base.FormatAndEscapeLiteral(value);
			}

			// Token: 0x06000017 RID: 23 RVA: 0x00002587 File Offset: 0x00000787
			private static string EscapeLeadingDollarSign(string stringValue)
			{
				if (stringValue.Length > 0 && stringValue[0] == '$')
				{
					stringValue = '$' + stringValue;
				}
				return stringValue;
			}
		}
	}
}
