using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000E3 RID: 227
	internal static class UriUtils
	{
		// Token: 0x06000588 RID: 1416 RVA: 0x000137FC File Offset: 0x000119FC
		internal static bool UriInvariantInsensitiveIsBaseOf(Uri baseUri, Uri uri)
		{
			Uri baseUri2 = UriUtils.CreateBaseComparableUri(baseUri);
			Uri uri2 = UriUtils.CreateBaseComparableUri(uri);
			return UriUtils.IsBaseOf(baseUri2, uri2);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00013820 File Offset: 0x00011A20
		internal static List<CustomQueryOptionToken> ParseQueryOptions(Uri uri)
		{
			List<CustomQueryOptionToken> list = new List<CustomQueryOptionToken>();
			string text = uri.Query.Replace('+', ' ');
			int num;
			if (text != null)
			{
				if (text.Length > 0 && text[0] == '?')
				{
					text = text.Substring(1);
				}
				num = text.Length;
			}
			else
			{
				num = 0;
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = i;
				int num3 = -1;
				while (i < num)
				{
					char c = text[i];
					if (c == '=')
					{
						if (num3 < 0)
						{
							num3 = i;
						}
					}
					else if (c == '&')
					{
						break;
					}
					i++;
				}
				string text2 = null;
				string text3;
				if (num3 >= 0)
				{
					text2 = text.Substring(num2, num3 - num2);
					text3 = text.Substring(num3 + 1, i - num3 - 1);
				}
				else
				{
					text3 = text.Substring(num2, i - num2);
				}
				text2 = ((text2 == null) ? null : Uri.UnescapeDataString(text2).Trim());
				text3 = ((text3 == null) ? null : Uri.UnescapeDataString(text3).Trim());
				list.Add(new CustomQueryOptionToken(text2, text3));
				if (i == num - 1 && text[i] == '&')
				{
					list.Add(new CustomQueryOptionToken(null, string.Empty));
				}
			}
			return list;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00013948 File Offset: 0x00011B48
		internal static bool IsStructuralOrNavigationPropertySelectionItem(SelectItem selectItem)
		{
			PathSelectItem pathSelectItem = selectItem as PathSelectItem;
			return pathSelectItem != null && (pathSelectItem.SelectedPath.LastSegment is NavigationPropertySegment || pathSelectItem.SelectedPath.LastSegment is PropertySegment);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00013988 File Offset: 0x00011B88
		private static Uri CreateBaseComparableUri(Uri uri)
		{
			uri = new Uri(UriUtilsCommon.UriToString(uri).ToUpper(CultureInfo.InvariantCulture), UriKind.RelativeOrAbsolute);
			return new UriBuilder(uri)
			{
				Host = "h",
				Port = 80,
				Scheme = "http"
			}.Uri;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000139D8 File Offset: 0x00011BD8
		private static bool IsBaseOf(Uri baseUri, Uri uri)
		{
			return baseUri.IsBaseOf(uri);
		}
	}
}
