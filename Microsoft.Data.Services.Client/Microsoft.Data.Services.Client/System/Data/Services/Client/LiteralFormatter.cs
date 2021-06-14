using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace System.Data.Services.Client
{
	// Token: 0x0200000C RID: 12
	internal abstract class LiteralFormatter
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000322E File Offset: 0x0000142E
		internal static LiteralFormatter ForConstants
		{
			get
			{
				return LiteralFormatter.DefaultInstance;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003235 File Offset: 0x00001435
		internal static LiteralFormatter ForKeys(bool keysAsSegment)
		{
			if (!keysAsSegment)
			{
				return LiteralFormatter.DefaultInstance;
			}
			return LiteralFormatter.KeyAsSegmentInstance;
		}

		// Token: 0x0600004C RID: 76
		internal abstract string Format(object value);

		// Token: 0x0600004D RID: 77 RVA: 0x00003245 File Offset: 0x00001445
		protected virtual string EscapeResultForUri(string result)
		{
			return Uri.EscapeDataString(result);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003250 File Offset: 0x00001450
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

		// Token: 0x0600004F RID: 79 RVA: 0x000032B0 File Offset: 0x000014B0
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

		// Token: 0x06000050 RID: 80 RVA: 0x0000341C File Offset: 0x0000161C
		private string FormatAndEscapeLiteral(object value)
		{
			string text = LiteralFormatter.FormatRawLiteral(value);
			if (value is string)
			{
				text = text.Replace("'", "''");
			}
			return this.EscapeResultForUri(text);
		}

		// Token: 0x04000010 RID: 16
		private const string HexValues = "0123456789ABCDEF";

		// Token: 0x04000011 RID: 17
		private static readonly LiteralFormatter DefaultInstance = new LiteralFormatter.DefaultLiteralFormatter();

		// Token: 0x04000012 RID: 18
		private static readonly LiteralFormatter KeyAsSegmentInstance = new LiteralFormatter.KeysAsSegmentsLiteralFormatter();

		// Token: 0x0200000D RID: 13
		private static class SharedUtils
		{
			// Token: 0x06000053 RID: 83 RVA: 0x0000346E File Offset: 0x0000166E
			internal static InvalidOperationException CreateExceptionForUnconvertableType(object value)
			{
				return Error.InvalidOperation(Strings.Context_CannotConvertKey(value));
			}

			// Token: 0x06000054 RID: 84 RVA: 0x0000347C File Offset: 0x0000167C
			internal static bool TryConvertToStandardType(object value, out object converted)
			{
				byte[] array;
				if (LiteralFormatter.SharedUtils.TryGetByteArrayFromBinary(value, out array))
				{
					converted = array;
					return true;
				}
				XElement xelement = value as XElement;
				if (xelement != null)
				{
					converted = xelement.ToString();
					return true;
				}
				converted = null;
				return false;
			}

			// Token: 0x06000055 RID: 85 RVA: 0x000034B0 File Offset: 0x000016B0
			internal static string AppendDecimalMarkerToDouble(string input)
			{
				IEnumerable<char> source = input.ToCharArray();
				if (source.All(new Func<char, bool>(char.IsDigit)))
				{
					return input + ".0";
				}
				return input;
			}

			// Token: 0x06000056 RID: 86 RVA: 0x000034E5 File Offset: 0x000016E5
			internal static bool ShouldAppendLiteralSuffixToDouble(double value)
			{
				return false;
			}

			// Token: 0x06000057 RID: 87 RVA: 0x000034E8 File Offset: 0x000016E8
			private static bool TryGetByteArrayFromBinary(object value, out byte[] array)
			{
				return ClientConvert.TryConvertBinaryToByteArray(value, out array);
			}
		}

		// Token: 0x0200000E RID: 14
		private sealed class DefaultLiteralFormatter : LiteralFormatter
		{
			// Token: 0x06000058 RID: 88 RVA: 0x000034F1 File Offset: 0x000016F1
			internal DefaultLiteralFormatter() : this(false)
			{
			}

			// Token: 0x06000059 RID: 89 RVA: 0x000034FA File Offset: 0x000016FA
			private DefaultLiteralFormatter(bool disableUrlEncoding)
			{
				this.disableUrlEncoding = disableUrlEncoding;
			}

			// Token: 0x0600005A RID: 90 RVA: 0x0000350C File Offset: 0x0000170C
			internal override string Format(object value)
			{
				object obj;
				if (LiteralFormatter.SharedUtils.TryConvertToStandardType(value, out obj))
				{
					value = obj;
				}
				return this.FormatLiteralWithTypePrefix(value);
			}

			// Token: 0x0600005B RID: 91 RVA: 0x0000352D File Offset: 0x0000172D
			protected override string EscapeResultForUri(string result)
			{
				if (!this.disableUrlEncoding)
				{
					result = base.EscapeResultForUri(result);
				}
				return result;
			}

			// Token: 0x0600005C RID: 92 RVA: 0x00003544 File Offset: 0x00001744
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

			// Token: 0x04000013 RID: 19
			private readonly bool disableUrlEncoding;
		}

		// Token: 0x0200000F RID: 15
		private sealed class KeysAsSegmentsLiteralFormatter : LiteralFormatter
		{
			// Token: 0x0600005D RID: 93 RVA: 0x00003681 File Offset: 0x00001881
			internal KeysAsSegmentsLiteralFormatter()
			{
			}

			// Token: 0x0600005E RID: 94 RVA: 0x0000368C File Offset: 0x0000188C
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

			// Token: 0x0600005F RID: 95 RVA: 0x000036BF File Offset: 0x000018BF
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
