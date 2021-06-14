using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000138 RID: 312
	internal static class WebConvert
	{
		// Token: 0x0600143A RID: 5178 RVA: 0x0004DB48 File Offset: 0x0004BD48
		internal static string ConvertByteArrayToKeyString(byte[] byteArray)
		{
			StringBuilder stringBuilder = new StringBuilder(3 + byteArray.Length * 2);
			stringBuilder.Append("X");
			stringBuilder.Append("'");
			for (int i = 0; i < byteArray.Length; i++)
			{
				stringBuilder.Append("0123456789ABCDEF"[byteArray[i] >> 4]);
				stringBuilder.Append("0123456789ABCDEF"[(int)(byteArray[i] & 15)]);
			}
			stringBuilder.Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0004DBC7 File Offset: 0x0004BDC7
		internal static bool IsKeyTypeQuoted(Type type)
		{
			return type == typeof(XElement) || type == typeof(string);
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0004DBF0 File Offset: 0x0004BDF0
		internal static bool TryKeyPrimitiveToString(object value, out string result)
		{
			if (value.GetType() == typeof(byte[]))
			{
				result = WebConvert.ConvertByteArrayToKeyString((byte[])value);
			}
			else
			{
				if (!WebConvert.TryXmlPrimitiveToString(value, out result))
				{
					return false;
				}
				if (value.GetType() == typeof(DateTime))
				{
					result = "datetime'" + result + "'";
				}
				else if (value.GetType() == typeof(decimal))
				{
					result += "M";
				}
				else if (value.GetType() == typeof(Guid))
				{
					result = "guid'" + result + "'";
				}
				else if (value.GetType() == typeof(long))
				{
					result += "L";
				}
				else if (value.GetType() == typeof(float))
				{
					result += "f";
				}
				else if (value.GetType() == typeof(double))
				{
					result = WebConvert.AppendDecimalMarkerToDouble(result);
				}
				else if (WebConvert.IsKeyTypeQuoted(value.GetType()))
				{
					result = "'" + result.Replace("'", "''") + "'";
				}
			}
			return true;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0004DD5C File Offset: 0x0004BF5C
		internal static bool TryXmlPrimitiveToString(object value, out string result)
		{
			result = null;
			Type type = value.GetType();
			type = (Nullable.GetUnderlyingType(type) ?? type);
			if (typeof(string) == type)
			{
				result = (string)value;
			}
			else if (typeof(bool) == type)
			{
				result = XmlConvert.ToString((bool)value);
			}
			else if (typeof(byte) == type)
			{
				result = XmlConvert.ToString((byte)value);
			}
			else if (typeof(DateTime) == type)
			{
				result = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind);
			}
			else if (typeof(decimal) == type)
			{
				result = XmlConvert.ToString((decimal)value);
			}
			else if (typeof(double) == type)
			{
				result = XmlConvert.ToString((double)value);
			}
			else if (typeof(Guid) == type)
			{
				result = value.ToString();
			}
			else if (typeof(short) == type)
			{
				result = XmlConvert.ToString((short)value);
			}
			else if (typeof(int) == type)
			{
				result = XmlConvert.ToString((int)value);
			}
			else if (typeof(long) == type)
			{
				result = XmlConvert.ToString((long)value);
			}
			else if (typeof(sbyte) == type)
			{
				result = XmlConvert.ToString((sbyte)value);
			}
			else if (typeof(float) == type)
			{
				result = XmlConvert.ToString((float)value);
			}
			else if (typeof(byte[]) == type)
			{
				byte[] inArray = (byte[])value;
				result = Convert.ToBase64String(inArray);
			}
			else
			{
				if (ClientConvert.IsBinaryValue(value))
				{
					return ClientConvert.TryKeyBinaryToString(value, out result);
				}
				if (!(typeof(XElement) == type))
				{
					result = null;
					return false;
				}
				result = ((XElement)value).ToString(SaveOptions.None);
			}
			return true;
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0004DF7C File Offset: 0x0004C17C
		private static string AppendDecimalMarkerToDouble(string input)
		{
			foreach (char c in input)
			{
				if (!char.IsDigit(c))
				{
					return input;
				}
			}
			return input + ".0";
		}

		// Token: 0x040006DB RID: 1755
		private const string HexValues = "0123456789ABCDEF";

		// Token: 0x040006DC RID: 1756
		private const string XmlHexEncodePrefix = "0x";
	}
}
