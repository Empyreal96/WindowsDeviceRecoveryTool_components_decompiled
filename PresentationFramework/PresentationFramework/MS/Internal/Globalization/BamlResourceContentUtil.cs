using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MS.Internal.Globalization
{
	// Token: 0x02000698 RID: 1688
	internal static class BamlResourceContentUtil
	{
		// Token: 0x06006E0D RID: 28173 RVA: 0x001FA7E8 File Offset: 0x001F89E8
		internal static string EscapeString(string content)
		{
			if (content == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			while (i < content.Length)
			{
				char c = content[i];
				switch (c)
				{
				case '"':
					stringBuilder.Append("&quot;");
					break;
				case '#':
					goto IL_59;
				case '$':
				case '%':
					goto IL_B8;
				case '&':
					stringBuilder.Append("&amp;");
					break;
				case '\'':
					stringBuilder.Append("&apos;");
					break;
				default:
					switch (c)
					{
					case ';':
						goto IL_59;
					case '<':
						stringBuilder.Append("&lt;");
						break;
					case '=':
						goto IL_B8;
					case '>':
						stringBuilder.Append("&gt;");
						break;
					default:
						if (c == '\\')
						{
							goto IL_59;
						}
						goto IL_B8;
					}
					break;
				}
				IL_C6:
				i++;
				continue;
				IL_59:
				stringBuilder.Append('\\');
				stringBuilder.Append(content[i]);
				goto IL_C6;
				IL_B8:
				stringBuilder.Append(content[i]);
				goto IL_C6;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006E0E RID: 28174 RVA: 0x001FA8D1 File Offset: 0x001F8AD1
		internal static string UnescapeString(string content)
		{
			return BamlResourceContentUtil.UnescapePattern.Replace(content, BamlResourceContentUtil.UnescapeMatchEvaluator);
		}

		// Token: 0x06006E0F RID: 28175 RVA: 0x001FA8E4 File Offset: 0x001F8AE4
		private static string UnescapeMatch(Match match)
		{
			string value = match.Value;
			if (value == "&lt;")
			{
				return "<";
			}
			if (value == "&gt;")
			{
				return ">";
			}
			if (value == "&amp;")
			{
				return "&";
			}
			if (value == "&apos;")
			{
				return "'";
			}
			if (value == "&quot;")
			{
				return "\"";
			}
			if (match.Value.Length == 2)
			{
				return match.Value[1].ToString();
			}
			return string.Empty;
		}

		// Token: 0x06006E10 RID: 28176 RVA: 0x001FA984 File Offset: 0x001F8B84
		internal static BamlStringToken[] ParseChildPlaceholder(string input)
		{
			if (input == null)
			{
				return null;
			}
			List<BamlStringToken> list = new List<BamlStringToken>(8);
			int num = 0;
			bool flag = false;
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '#')
				{
					if (i == 0 || input[i - 1] != '\\')
					{
						if (flag)
						{
							return null;
						}
						flag = true;
						if (num < i)
						{
							list.Add(new BamlStringToken(BamlStringToken.TokenType.Text, BamlResourceContentUtil.UnescapeString(input.Substring(num, i - num))));
							num = i;
						}
					}
				}
				else if (input[i] == ';' && (i > 0 && input[i - 1] != '\\' && flag))
				{
					list.Add(new BamlStringToken(BamlStringToken.TokenType.ChildPlaceHolder, BamlResourceContentUtil.UnescapeString(input.Substring(num + 1, i - num - 1))));
					num = i + 1;
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			if (num < input.Length)
			{
				list.Add(new BamlStringToken(BamlStringToken.TokenType.Text, BamlResourceContentUtil.UnescapeString(input.Substring(num))));
			}
			return list.ToArray();
		}

		// Token: 0x04003627 RID: 13863
		private static Regex UnescapePattern = new Regex("(\\\\.?|&lt;|&gt;|&quot;|&apos;|&amp;)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		// Token: 0x04003628 RID: 13864
		private static MatchEvaluator UnescapeMatchEvaluator = new MatchEvaluator(BamlResourceContentUtil.UnescapeMatch);
	}
}
