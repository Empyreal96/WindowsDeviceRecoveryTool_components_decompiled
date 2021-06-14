using System;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Atom;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000298 RID: 664
	internal static class AtomValueUtils
	{
		// Token: 0x0600166C RID: 5740 RVA: 0x0005136C File Offset: 0x0004F56C
		internal static void WritePrimitiveValue(XmlWriter writer, object value)
		{
			if (!PrimitiveConverter.Instance.TryWriteAtom(value, writer))
			{
				string value2 = AtomValueUtils.ConvertPrimitiveToString(value);
				ODataAtomWriterUtils.WriteString(writer, value2);
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00051398 File Offset: 0x0004F598
		internal static string ConvertPrimitiveToString(object value)
		{
			string result;
			if (!AtomValueUtils.TryConvertPrimitiveToString(value, out result))
			{
				throw new ODataException(Strings.AtomValueUtils_CannotConvertValueToAtomPrimitive(value.GetType().FullName));
			}
			return result;
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x000513C8 File Offset: 0x0004F5C8
		internal static object ReadPrimitiveValue(XmlReader reader, IEdmPrimitiveTypeReference primitiveTypeReference)
		{
			object result;
			if (!PrimitiveConverter.Instance.TryTokenizeFromXml(reader, EdmLibraryExtensions.GetPrimitiveClrType(primitiveTypeReference), out result))
			{
				string text = reader.ReadElementContentValue();
				return AtomValueUtils.ConvertStringToPrimitive(text, primitiveTypeReference);
			}
			return result;
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x000513FC File Offset: 0x0004F5FC
		internal static string ToString(AtomTextConstructKind textConstructKind)
		{
			switch (textConstructKind)
			{
			case AtomTextConstructKind.Text:
				return "text";
			case AtomTextConstructKind.Html:
				return "html";
			case AtomTextConstructKind.Xhtml:
				return "xhtml";
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataAtomConvert_ToString));
			}
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00051444 File Offset: 0x0004F644
		internal static bool TryConvertPrimitiveToString(object value, out string result)
		{
			result = null;
			switch (PlatformHelper.GetTypeCode(value.GetType()))
			{
			case TypeCode.Boolean:
				result = ODataAtomConvert.ToString((bool)value);
				return true;
			case TypeCode.SByte:
				result = ((sbyte)value).ToString();
				return true;
			case TypeCode.Byte:
				result = ODataAtomConvert.ToString((byte)value);
				return true;
			case TypeCode.Int16:
				result = ((short)value).ToString();
				return true;
			case TypeCode.Int32:
				result = ((int)value).ToString();
				return true;
			case TypeCode.Int64:
				result = ((long)value).ToString();
				return true;
			case TypeCode.Single:
				result = ((float)value).ToString();
				return true;
			case TypeCode.Double:
				result = ((double)value).ToString();
				return true;
			case TypeCode.Decimal:
				result = ODataAtomConvert.ToString((decimal)value);
				return true;
			case TypeCode.DateTime:
				result = ((DateTime)value).ToString();
				return true;
			case TypeCode.String:
				result = (string)value;
				return true;
			}
			byte[] array = value as byte[];
			if (array != null)
			{
				result = array.ToString();
			}
			else if (value is DateTimeOffset)
			{
				result = ODataAtomConvert.ToString((DateTimeOffset)value);
			}
			else if (value is Guid)
			{
				result = ((Guid)value).ToString();
			}
			else
			{
				if (!(value is TimeSpan))
				{
					return false;
				}
				result = ((TimeSpan)value).ToString();
			}
			return true;
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x000515C4 File Offset: 0x0004F7C4
		internal static object ConvertStringToPrimitive(string text, IEdmPrimitiveTypeReference targetTypeReference)
		{
			try
			{
				switch (targetTypeReference.PrimitiveKind())
				{
				case EdmPrimitiveTypeKind.Binary:
					return Convert.FromBase64String(text);
				case EdmPrimitiveTypeKind.Boolean:
					return AtomValueUtils.ConvertXmlBooleanValue(text);
				case EdmPrimitiveTypeKind.Byte:
					return XmlConvert.ToByte(text);
				case EdmPrimitiveTypeKind.DateTime:
					return PlatformHelper.ConvertStringToDateTime(text);
				case EdmPrimitiveTypeKind.DateTimeOffset:
					return PlatformHelper.ConvertStringToDateTimeOffset(text);
				case EdmPrimitiveTypeKind.Decimal:
					return XmlConvert.ToDecimal(text);
				case EdmPrimitiveTypeKind.Double:
					return XmlConvert.ToDouble(text);
				case EdmPrimitiveTypeKind.Guid:
					return new Guid(text);
				case EdmPrimitiveTypeKind.Int16:
					return XmlConvert.ToInt16(text);
				case EdmPrimitiveTypeKind.Int32:
					return XmlConvert.ToInt32(text);
				case EdmPrimitiveTypeKind.Int64:
					return XmlConvert.ToInt64(text);
				case EdmPrimitiveTypeKind.SByte:
					return XmlConvert.ToSByte(text);
				case EdmPrimitiveTypeKind.Single:
					return XmlConvert.ToSingle(text);
				case EdmPrimitiveTypeKind.String:
					return text;
				case EdmPrimitiveTypeKind.Time:
					return XmlConvert.ToTimeSpan(text);
				}
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.AtomValueUtils_ConvertStringToPrimitive));
			}
			catch (Exception ex)
			{
				if (!ExceptionUtils.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ReaderValidationUtils.GetPrimitiveTypeConversionException(targetTypeReference, ex);
			}
			object result;
			return result;
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00051784 File Offset: 0x0004F984
		private static bool ConvertXmlBooleanValue(string text)
		{
			text = text.Trim(AtomValueUtils.XmlWhitespaceChars);
			string key;
			switch (key = text)
			{
			case "true":
			case "True":
			case "1":
				return true;
			case "false":
			case "False":
			case "0":
				return false;
			}
			return XmlConvert.ToBoolean(text);
		}

		// Token: 0x040008FD RID: 2301
		private static readonly char[] XmlWhitespaceChars = new char[]
		{
			' ',
			'\t',
			'\n',
			'\r'
		};
	}
}
