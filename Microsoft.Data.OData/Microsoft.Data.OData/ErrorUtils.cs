using System;
using System.Xml;

namespace Microsoft.Data.OData
{
	// Token: 0x0200022F RID: 559
	internal static class ErrorUtils
	{
		// Token: 0x060011CD RID: 4557 RVA: 0x00041F23 File Offset: 0x00040123
		internal static void GetErrorDetails(ODataError error, out string code, out string message, out string messageLanguage)
		{
			code = (error.ErrorCode ?? string.Empty);
			message = (error.Message ?? string.Empty);
			messageLanguage = (error.MessageLanguage ?? "en-US");
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00041F58 File Offset: 0x00040158
		internal static void WriteXmlError(XmlWriter writer, ODataError error, bool includeDebugInformation, int maxInnerErrorDepth)
		{
			string code;
			string message;
			string messageLanguage;
			ErrorUtils.GetErrorDetails(error, out code, out message, out messageLanguage);
			ODataInnerError innerError = includeDebugInformation ? error.InnerError : null;
			ErrorUtils.WriteXmlError(writer, code, message, messageLanguage, innerError, maxInnerErrorDepth);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00041F8C File Offset: 0x0004018C
		private static void WriteXmlError(XmlWriter writer, string code, string message, string messageLanguage, ODataInnerError innerError, int maxInnerErrorDepth)
		{
			writer.WriteStartElement("m", "error", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			writer.WriteElementString("m", "code", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", code);
			writer.WriteStartElement("m", "message", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", messageLanguage);
			writer.WriteString(message);
			writer.WriteEndElement();
			if (innerError != null)
			{
				ErrorUtils.WriteXmlInnerError(writer, innerError, "innererror", 0, maxInnerErrorDepth);
			}
			writer.WriteEndElement();
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00042018 File Offset: 0x00040218
		private static void WriteXmlInnerError(XmlWriter writer, ODataInnerError innerError, string innerErrorElementName, int recursionDepth, int maxInnerErrorDepth)
		{
			recursionDepth++;
			if (recursionDepth > maxInnerErrorDepth)
			{
				throw new ODataException(Strings.ValidationUtils_RecursionDepthLimitReached(maxInnerErrorDepth));
			}
			writer.WriteStartElement("m", innerErrorElementName, "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			string text = innerError.Message ?? string.Empty;
			writer.WriteStartElement("message", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			writer.WriteString(text);
			writer.WriteEndElement();
			string text2 = innerError.TypeName ?? string.Empty;
			writer.WriteStartElement("type", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			writer.WriteString(text2);
			writer.WriteEndElement();
			string text3 = innerError.StackTrace ?? string.Empty;
			writer.WriteStartElement("stacktrace", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			writer.WriteString(text3);
			writer.WriteEndElement();
			if (innerError.InnerError != null)
			{
				ErrorUtils.WriteXmlInnerError(writer, innerError.InnerError, "internalexception", recursionDepth, maxInnerErrorDepth);
			}
			writer.WriteEndElement();
		}

		// Token: 0x04000686 RID: 1670
		internal const string ODataErrorMessageDefaultLanguage = "en-US";
	}
}
