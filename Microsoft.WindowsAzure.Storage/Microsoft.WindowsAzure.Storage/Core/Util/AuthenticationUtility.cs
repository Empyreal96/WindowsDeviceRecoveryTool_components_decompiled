using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000093 RID: 147
	internal static class AuthenticationUtility
	{
		// Token: 0x06000FD8 RID: 4056 RVA: 0x0003C0E4 File Offset: 0x0003A2E4
		public static string GetPreferredDateHeaderValue(HttpWebRequest request)
		{
			string text = request.Headers["x-ms-date"];
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return request.Headers[HttpRequestHeader.Date];
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0003C118 File Offset: 0x0003A318
		public static void AppendCanonicalizedContentLengthHeader(CanonicalizedString canonicalizedString, HttpWebRequest request)
		{
			if (request.ContentLength != -1L && request.ContentLength != 0L)
			{
				canonicalizedString.AppendCanonicalizedElement(request.ContentLength.ToString(CultureInfo.InvariantCulture));
				return;
			}
			canonicalizedString.AppendCanonicalizedElement(null);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0003C15C File Offset: 0x0003A35C
		public static void AppendCanonicalizedDateHeader(CanonicalizedString canonicalizedString, HttpWebRequest request, bool allowMicrosoftDateHeader = false)
		{
			string text = request.Headers["x-ms-date"];
			if (string.IsNullOrEmpty(text))
			{
				canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.Date]);
				return;
			}
			if (allowMicrosoftDateHeader)
			{
				canonicalizedString.AppendCanonicalizedElement(text);
				return;
			}
			canonicalizedString.AppendCanonicalizedElement(null);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0003C1A8 File Offset: 0x0003A3A8
		public static void AppendCanonicalizedCustomHeaders(CanonicalizedString canonicalizedString, HttpWebRequest request)
		{
			List<string> list = new List<string>(request.Headers.AllKeys.Length);
			foreach (string text in request.Headers.AllKeys)
			{
				if (text.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase))
				{
					list.Add(text.ToLowerInvariant());
				}
			}
			CultureInfo culture = new CultureInfo("en-US");
			StringComparer comparer = StringComparer.Create(culture, false);
			list.Sort(comparer);
			StringBuilder stringBuilder = new StringBuilder(50);
			foreach (string text2 in list)
			{
				string text3 = request.Headers[text2];
				if (!string.IsNullOrEmpty(text3))
				{
					stringBuilder.Length = 0;
					stringBuilder.Append(text2);
					stringBuilder.Append(':');
					stringBuilder.Append(text3.TrimStart(new char[0]).Replace("\r\n", string.Empty));
					canonicalizedString.AppendCanonicalizedElement(stringBuilder.ToString());
				}
			}
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0003C2CC File Offset: 0x0003A4CC
		public static string GetCanonicalizedHeaderValue(DateTimeOffset? value)
		{
			if (value != null)
			{
				return HttpWebUtility.ConvertDateTimeToHttpString(value.Value);
			}
			return null;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0003C2E8 File Offset: 0x0003A4E8
		private static string GetAbsolutePathWithoutSecondarySuffix(Uri uri, string accountName)
		{
			string text = uri.AbsolutePath;
			string value = accountName + "-secondary";
			int num = text.IndexOf(value, StringComparison.OrdinalIgnoreCase);
			if (num == 1)
			{
				num += accountName.Length;
				text = text.Remove(num, "-secondary".Length);
			}
			return text;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0003C334 File Offset: 0x0003A534
		public static string GetCanonicalizedResourceString(Uri uri, string accountName, bool isSharedKeyLiteOrTableService = false)
		{
			StringBuilder stringBuilder = new StringBuilder(100);
			stringBuilder.Append('/');
			stringBuilder.Append(accountName);
			stringBuilder.Append(AuthenticationUtility.GetAbsolutePathWithoutSecondarySuffix(uri, accountName));
			IDictionary<string, string> dictionary = HttpWebUtility.ParseQueryString(uri.Query);
			if (!isSharedKeyLiteOrTableService)
			{
				List<string> list = new List<string>(dictionary.Keys);
				list.Sort(StringComparer.OrdinalIgnoreCase);
				using (List<string>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						stringBuilder.Append('\n');
						stringBuilder.Append(text.ToLowerInvariant());
						stringBuilder.Append(':');
						stringBuilder.Append(dictionary[text]);
					}
					goto IL_C9;
				}
			}
			string value;
			if (dictionary.TryGetValue("comp", out value))
			{
				stringBuilder.Append("?comp=");
				stringBuilder.Append(value);
			}
			IL_C9:
			return stringBuilder.ToString();
		}

		// Token: 0x040003AF RID: 943
		private const int ExpectedResourceStringLength = 100;

		// Token: 0x040003B0 RID: 944
		private const int ExpectedHeaderNameAndValueLength = 50;

		// Token: 0x040003B1 RID: 945
		private const char HeaderNameValueSeparator = ':';

		// Token: 0x040003B2 RID: 946
		private const char HeaderValueDelimiter = ',';
	}
}
