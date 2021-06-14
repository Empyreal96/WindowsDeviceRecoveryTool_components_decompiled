using System;
using System.Diagnostics;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x02000009 RID: 9
	internal static class ContentTypeUtil
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002C11 File Offset: 0x00000E11
		internal static Encoding FallbackEncoding
		{
			get
			{
				return ContentTypeUtil.EncodingUtf8NoPreamble;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002C18 File Offset: 0x00000E18
		private static Encoding MissingEncoding
		{
			get
			{
				return Encoding.GetEncoding("ISO-8859-1", new EncoderExceptionFallback(), new DecoderExceptionFallback());
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002C30 File Offset: 0x00000E30
		internal static ContentTypeUtil.MediaParameter[] ReadContentType(string contentType, out string mime)
		{
			if (string.IsNullOrEmpty(contentType))
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_ContentTypeMissing);
			}
			ContentTypeUtil.MediaType mediaType = ContentTypeUtil.ReadMediaType(contentType);
			mime = mediaType.MimeType;
			return mediaType.Parameters;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002C6C File Offset: 0x00000E6C
		internal static string WriteContentType(string mimeType, ContentTypeUtil.MediaParameter[] parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(mimeType);
			foreach (ContentTypeUtil.MediaParameter mediaParameter in parameters)
			{
				stringBuilder.Append(';');
				stringBuilder.Append(mediaParameter.Name);
				stringBuilder.Append("=");
				stringBuilder.Append(mediaParameter.GetOriginalValue());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002CD0 File Offset: 0x00000ED0
		internal static ContentTypeUtil.MediaParameter[] ReadContentType(string contentType, out string mime, out Encoding encoding)
		{
			if (string.IsNullOrEmpty(contentType))
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_ContentTypeMissing);
			}
			ContentTypeUtil.MediaType mediaType = ContentTypeUtil.ReadMediaType(contentType);
			mime = mediaType.MimeType;
			encoding = mediaType.SelectEncoding();
			return mediaType.Parameters;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002D14 File Offset: 0x00000F14
		private static Encoding EncodingFromName(string name)
		{
			if (name == null)
			{
				return ContentTypeUtil.MissingEncoding;
			}
			name = name.Trim();
			if (name.Length == 0)
			{
				return ContentTypeUtil.MissingEncoding;
			}
			Encoding encoding;
			try
			{
				encoding = Encoding.GetEncoding(name);
			}
			catch (ArgumentException)
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_EncodingNotSupported(name));
			}
			return encoding;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002D6C File Offset: 0x00000F6C
		private static void ReadMediaTypeAndSubtype(string text, ref int textIndex, out string type, out string subType)
		{
			int num = textIndex;
			if (ContentTypeUtil.ReadToken(text, ref textIndex))
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_MediaTypeUnspecified);
			}
			if (text[textIndex] != '/')
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_MediaTypeRequiresSlash);
			}
			type = text.Substring(num, textIndex - num);
			textIndex++;
			int num2 = textIndex;
			ContentTypeUtil.ReadToken(text, ref textIndex);
			if (textIndex == num2)
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_MediaTypeRequiresSubType);
			}
			subType = text.Substring(num2, textIndex - num2);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002DF4 File Offset: 0x00000FF4
		private static ContentTypeUtil.MediaType ReadMediaType(string text)
		{
			int num = 0;
			string type;
			string subType;
			ContentTypeUtil.ReadMediaTypeAndSubtype(text, ref num, out type, out subType);
			ContentTypeUtil.MediaParameter[] parameters = null;
			while (!ContentTypeUtil.SkipWhitespace(text, ref num))
			{
				if (text[num] != ';')
				{
					throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_MediaTypeRequiresSemicolonBeforeParameter);
				}
				num++;
				if (ContentTypeUtil.SkipWhitespace(text, ref num))
				{
					break;
				}
				ContentTypeUtil.ReadMediaTypeParameter(text, ref num, ref parameters);
			}
			return new ContentTypeUtil.MediaType(type, subType, parameters);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002E58 File Offset: 0x00001058
		private static bool ReadToken(string text, ref int textIndex)
		{
			while (textIndex < text.Length && ContentTypeUtil.IsHttpToken(text[textIndex]))
			{
				textIndex++;
			}
			return textIndex == text.Length;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002E85 File Offset: 0x00001085
		private static bool SkipWhitespace(string text, ref int textIndex)
		{
			while (textIndex < text.Length && char.IsWhiteSpace(text, textIndex))
			{
				textIndex++;
			}
			return textIndex == text.Length;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002EB0 File Offset: 0x000010B0
		private static void ReadMediaTypeParameter(string text, ref int textIndex, ref ContentTypeUtil.MediaParameter[] parameters)
		{
			int num = textIndex;
			if (ContentTypeUtil.ReadToken(text, ref textIndex))
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_MediaTypeMissingValue);
			}
			string parameterName = text.Substring(num, textIndex - num);
			if (text[textIndex] != '=')
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_MediaTypeMissingValue);
			}
			textIndex++;
			ContentTypeUtil.MediaParameter mediaParameter = ContentTypeUtil.ReadQuotedParameterValue(parameterName, text, ref textIndex);
			if (parameters == null)
			{
				parameters = new ContentTypeUtil.MediaParameter[1];
			}
			else
			{
				ContentTypeUtil.MediaParameter[] array = new ContentTypeUtil.MediaParameter[parameters.Length + 1];
				Array.Copy(parameters, array, parameters.Length);
				parameters = array;
			}
			parameters[parameters.Length - 1] = mediaParameter;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002F44 File Offset: 0x00001144
		private static ContentTypeUtil.MediaParameter ReadQuotedParameterValue(string parameterName, string headerText, ref int textIndex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool isQuoted = false;
			bool flag = false;
			if (textIndex < headerText.Length && headerText[textIndex] == '"')
			{
				textIndex++;
				flag = true;
				isQuoted = true;
			}
			while (textIndex < headerText.Length)
			{
				char c = headerText[textIndex];
				if (c == '\\' || c == '"')
				{
					if (!flag)
					{
						throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_EscapeCharWithoutQuotes(parameterName));
					}
					textIndex++;
					if (c == '"')
					{
						flag = false;
						break;
					}
					if (textIndex >= headerText.Length)
					{
						throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_EscapeCharAtEnd(parameterName));
					}
					c = headerText[textIndex];
				}
				else if (!ContentTypeUtil.IsHttpToken(c))
				{
					break;
				}
				stringBuilder.Append(c);
				textIndex++;
			}
			if (flag)
			{
				throw Error.HttpHeaderFailure(400, Strings.HttpProcessUtility_ClosingQuoteNotFound(parameterName));
			}
			return new ContentTypeUtil.MediaParameter(parameterName, stringBuilder.ToString(), isQuoted);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003020 File Offset: 0x00001220
		private static bool IsHttpSeparator(char c)
		{
			return c == '(' || c == ')' || c == '<' || c == '>' || c == '@' || c == ',' || c == ';' || c == ':' || c == '\\' || c == '"' || c == '/' || c == '[' || c == ']' || c == '?' || c == '=' || c == '{' || c == '}' || c == ' ' || c == '\t';
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000308E File Offset: 0x0000128E
		private static bool IsHttpToken(char c)
		{
			return c < '\u007f' && c > '\u001f' && !ContentTypeUtil.IsHttpSeparator(c);
		}

		// Token: 0x04000009 RID: 9
		internal static readonly UTF8Encoding EncodingUtf8NoPreamble = new UTF8Encoding(false, true);

		// Token: 0x0200000A RID: 10
		internal class MediaParameter
		{
			// Token: 0x0600003E RID: 62 RVA: 0x000030B3 File Offset: 0x000012B3
			public MediaParameter(string name, string value, bool isQuoted)
			{
				this.Name = name;
				this.Value = value;
				this.IsQuoted = isQuoted;
			}

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600003F RID: 63 RVA: 0x000030D0 File Offset: 0x000012D0
			// (set) Token: 0x06000040 RID: 64 RVA: 0x000030D8 File Offset: 0x000012D8
			public string Name { get; private set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000041 RID: 65 RVA: 0x000030E1 File Offset: 0x000012E1
			// (set) Token: 0x06000042 RID: 66 RVA: 0x000030E9 File Offset: 0x000012E9
			public string Value { get; private set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000043 RID: 67 RVA: 0x000030F2 File Offset: 0x000012F2
			// (set) Token: 0x06000044 RID: 68 RVA: 0x000030FA File Offset: 0x000012FA
			private bool IsQuoted { get; set; }

			// Token: 0x06000045 RID: 69 RVA: 0x00003103 File Offset: 0x00001303
			public string GetOriginalValue()
			{
				if (!this.IsQuoted)
				{
					return this.Value;
				}
				return "\"" + this.Value + "\"";
			}
		}

		// Token: 0x0200000B RID: 11
		[DebuggerDisplay("MediaType [{type}/{subType}]")]
		private sealed class MediaType
		{
			// Token: 0x06000046 RID: 70 RVA: 0x00003129 File Offset: 0x00001329
			internal MediaType(string type, string subType, ContentTypeUtil.MediaParameter[] parameters)
			{
				this.type = type;
				this.subType = subType;
				this.parameters = parameters;
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000047 RID: 71 RVA: 0x00003146 File Offset: 0x00001346
			internal string MimeType
			{
				get
				{
					return this.type + "/" + this.subType;
				}
			}

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000048 RID: 72 RVA: 0x0000315E File Offset: 0x0000135E
			internal ContentTypeUtil.MediaParameter[] Parameters
			{
				get
				{
					return this.parameters;
				}
			}

			// Token: 0x06000049 RID: 73 RVA: 0x00003168 File Offset: 0x00001368
			internal Encoding SelectEncoding()
			{
				if (this.parameters != null)
				{
					foreach (ContentTypeUtil.MediaParameter mediaParameter in this.parameters)
					{
						if (string.Equals(mediaParameter.Name, "charset", StringComparison.OrdinalIgnoreCase))
						{
							string text = mediaParameter.Value.Trim();
							if (text.Length > 0)
							{
								return ContentTypeUtil.EncodingFromName(mediaParameter.Value);
							}
						}
					}
				}
				if (string.Equals(this.type, "text", StringComparison.OrdinalIgnoreCase))
				{
					if (string.Equals(this.subType, "xml", StringComparison.OrdinalIgnoreCase))
					{
						return null;
					}
					return ContentTypeUtil.MissingEncoding;
				}
				else
				{
					if (string.Equals(this.type, "application", StringComparison.OrdinalIgnoreCase) && string.Equals(this.subType, "json", StringComparison.OrdinalIgnoreCase))
					{
						return ContentTypeUtil.FallbackEncoding;
					}
					return null;
				}
			}

			// Token: 0x0400000D RID: 13
			private readonly ContentTypeUtil.MediaParameter[] parameters;

			// Token: 0x0400000E RID: 14
			private readonly string subType;

			// Token: 0x0400000F RID: 15
			private readonly string type;
		}
	}
}
