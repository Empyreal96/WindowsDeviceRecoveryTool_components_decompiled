using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000E0 RID: 224
	internal sealed class UriPathParser
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x00013496 File Offset: 0x00011696
		internal UriPathParser(int maxSegments)
		{
			this.maxSegments = maxSegments;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x000134A8 File Offset: 0x000116A8
		internal string[] ParsePath(string escapedRelativePathUri)
		{
			if (escapedRelativePathUri == null || string.IsNullOrEmpty(escapedRelativePathUri.Trim()))
			{
				return new string[0];
			}
			string[] array = escapedRelativePathUri.Split(new char[]
			{
				'/'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length >= this.maxSegments)
			{
				throw new ODataException(Strings.UriQueryPathParser_TooManySegments);
			}
			return array;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x000134F8 File Offset: 0x000116F8
		internal ICollection<string> ParsePathIntoSegments(Uri absoluteUri, Uri serviceBaseUri)
		{
			if (!UriUtils.UriInvariantInsensitiveIsBaseOf(serviceBaseUri, absoluteUri))
			{
				throw new ODataException(Strings.UriQueryPathParser_RequestUriDoesNotHaveTheCorrectBaseUri(absoluteUri, serviceBaseUri));
			}
			ICollection<string> result;
			try
			{
				int num = serviceBaseUri.Segments.Length;
				string[] segments = absoluteUri.Segments;
				List<string> list = new List<string>();
				for (int i = num; i < segments.Length; i++)
				{
					string text = segments[i];
					if (text.Length != 0 && text != "/")
					{
						if (text[text.Length - 1] == '/')
						{
							text = text.Substring(0, text.Length - 1);
						}
						if (list.Count == this.maxSegments)
						{
							throw new ODataException(Strings.UriQueryPathParser_TooManySegments);
						}
						list.Add(Uri.UnescapeDataString(text));
					}
				}
				result = list.ToArray();
			}
			catch (UriFormatException innerException)
			{
				throw new ODataException(Strings.UriQueryPathParser_SyntaxError, innerException);
			}
			return result;
		}

		// Token: 0x04000255 RID: 597
		private readonly int maxSegments;
	}
}
