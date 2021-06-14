using System;
using System.Globalization;
using System.Xml;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl.Internal
{
	// Token: 0x02000007 RID: 7
	internal static class EdmValueWriter
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002644 File Offset: 0x00000844
		internal static string PrimitiveValueAsXml(IEdmPrimitiveValue v)
		{
			switch (v.ValueKind)
			{
			case EdmValueKind.Binary:
				return EdmValueWriter.BinaryAsXml(((IEdmBinaryValue)v).Value);
			case EdmValueKind.Boolean:
				return EdmValueWriter.BooleanAsXml(((IEdmBooleanValue)v).Value);
			case EdmValueKind.DateTimeOffset:
				return EdmValueWriter.DateTimeOffsetAsXml(((IEdmDateTimeOffsetValue)v).Value);
			case EdmValueKind.DateTime:
				return EdmValueWriter.DateTimeAsXml(((IEdmDateTimeValue)v).Value);
			case EdmValueKind.Decimal:
				return EdmValueWriter.DecimalAsXml(((IEdmDecimalValue)v).Value);
			case EdmValueKind.Floating:
				return EdmValueWriter.FloatAsXml(((IEdmFloatingValue)v).Value);
			case EdmValueKind.Guid:
				return EdmValueWriter.GuidAsXml(((IEdmGuidValue)v).Value);
			case EdmValueKind.Integer:
				return EdmValueWriter.LongAsXml(((IEdmIntegerValue)v).Value);
			case EdmValueKind.String:
				return EdmValueWriter.StringAsXml(((IEdmStringValue)v).Value);
			case EdmValueKind.Time:
				return EdmValueWriter.TimeAsXml(((IEdmTimeValue)v).Value);
			}
			throw new NotSupportedException(Strings.ValueWriter_NonSerializableValue(v.ValueKind));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000275A File Offset: 0x0000095A
		internal static string StringAsXml(string s)
		{
			return s;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002760 File Offset: 0x00000960
		internal static string BinaryAsXml(byte[] binary)
		{
			char[] array = new char[binary.Length * 2];
			for (int i = 0; i < binary.Length; i++)
			{
				array[i << 1] = EdmValueWriter.Hex[binary[i] >> 4];
				array[i << 1 | 1] = EdmValueWriter.Hex[(int)(binary[i] & 15)];
			}
			return new string(array);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027AF File Offset: 0x000009AF
		internal static string BooleanAsXml(bool b)
		{
			return XmlConvert.ToString(b);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027B7 File Offset: 0x000009B7
		internal static string BooleanAsXml(bool? b)
		{
			return EdmValueWriter.BooleanAsXml(b.Value);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027C5 File Offset: 0x000009C5
		internal static string IntAsXml(int i)
		{
			return XmlConvert.ToString(i);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027CD File Offset: 0x000009CD
		internal static string IntAsXml(int? i)
		{
			return EdmValueWriter.IntAsXml(i.Value);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027DB File Offset: 0x000009DB
		internal static string LongAsXml(long l)
		{
			return XmlConvert.ToString(l);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000027E3 File Offset: 0x000009E3
		internal static string FloatAsXml(double f)
		{
			return XmlConvert.ToString(f);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027EB File Offset: 0x000009EB
		internal static string DecimalAsXml(decimal d)
		{
			return XmlConvert.ToString(d);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000027F3 File Offset: 0x000009F3
		internal static string DateTimeAsXml(DateTime d)
		{
			return PlatformHelper.ConvertDateTimeToString(d);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027FB File Offset: 0x000009FB
		internal static string TimeAsXml(TimeSpan d)
		{
			return d.ToString("hh\\:mm\\:ss\\.fff", CultureInfo.InvariantCulture);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000280E File Offset: 0x00000A0E
		internal static string DateTimeOffsetAsXml(DateTimeOffset d)
		{
			return XmlConvert.ToString(d);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002816 File Offset: 0x00000A16
		internal static string GuidAsXml(Guid g)
		{
			return XmlConvert.ToString(g);
		}

		// Token: 0x0400000A RID: 10
		private static char[] Hex = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};
	}
}
