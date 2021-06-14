using System;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x02000265 RID: 613
	internal class XamlParser
	{
		// Token: 0x0600232D RID: 9005 RVA: 0x000ACD20 File Offset: 0x000AAF20
		internal static void ThrowException(string id, int lineNumber, int linePosition)
		{
			string message = SR.Get(id);
			XamlParser.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000ACD3C File Offset: 0x000AAF3C
		internal static void ThrowException(string id, string value, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				value
			});
			XamlParser.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x000ACD64 File Offset: 0x000AAF64
		internal static void ThrowException(string id, string value1, string value2, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				value1,
				value2
			});
			XamlParser.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x000ACD90 File Offset: 0x000AAF90
		internal static void ThrowException(string id, string value1, string value2, string value3, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				value1,
				value2,
				value3
			});
			XamlParser.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x000ACDC0 File Offset: 0x000AAFC0
		internal static void ThrowException(string id, string value1, string value2, string value3, string value4, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				value1,
				value2,
				value3,
				value4
			});
			XamlParser.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x000ACDF8 File Offset: 0x000AAFF8
		private static void ThrowExceptionWithLine(string message, int lineNumber, int linePosition)
		{
			message += " ";
			message += SR.Get("ParserLineAndOffset", new object[]
			{
				lineNumber.ToString(CultureInfo.CurrentCulture),
				linePosition.ToString(CultureInfo.CurrentCulture)
			});
			XamlParseException ex = new XamlParseException(message, lineNumber, linePosition);
			throw ex;
		}
	}
}
