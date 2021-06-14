using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200004C RID: 76
	internal static class SddlNormalizer
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00009074 File Offset: 0x00007274
		private static string ToFullSddl(string sid)
		{
			if (string.IsNullOrEmpty(sid) || sid.StartsWith("S-") || SddlNormalizer.s_knownSids.Contains(sid))
			{
				return sid;
			}
			string text = null;
			if (!SddlNormalizer.s_map.TryGetValue(sid, out text))
			{
				text = new SecurityIdentifier(sid).ToString();
				SddlNormalizer.s_map.Add(sid, text);
			}
			return text;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000090D0 File Offset: 0x000072D0
		private static string FormatFullAccountSid(string matchGroupIndex, Match match)
		{
			string text = match.Value;
			string value = match.Groups[matchGroupIndex].Value;
			char c = text[text.Length - 1];
			text = text.Remove(text.Length - (value.Length + 1));
			return text + SddlNormalizer.ToFullSddl(value) + c;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000913A File Offset: 0x0000733A
		public static string FixAceSddl(string sddl)
		{
			if (string.IsNullOrEmpty(sddl))
			{
				return sddl;
			}
			return Regex.Replace(sddl, "((;[^;]*){4};)(?<sid>[^;\\)]+)([;\\)])", (Match x) => SddlNormalizer.FormatFullAccountSid("sid", x));
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000091AE File Offset: 0x000073AE
		public static string FixOwnerSddl(string sddl)
		{
			if (string.IsNullOrEmpty(sddl))
			{
				return sddl;
			}
			return Regex.Replace(sddl, "O:(?<oid>.*?)G:(?<gid>.*)", (Match x) => string.Format("O:{0}G:{1}", SddlNormalizer.ToFullSddl(x.Groups["oid"].Value), SddlNormalizer.ToFullSddl(x.Groups["gid"].Value)));
		}

		// Token: 0x0400012B RID: 299
		private static readonly HashSet<string> s_knownSids = new HashSet<string>
		{
			"AN",
			"AO",
			"AU",
			"BA",
			"BG",
			"BO",
			"BU",
			"CA",
			"CD",
			"CG",
			"CO",
			"CY",
			"EA",
			"ED",
			"ER",
			"IS",
			"IU",
			"LA",
			"LG",
			"LS",
			"LU",
			"MU",
			"NO",
			"NS",
			"NU",
			"OW",
			"PA",
			"PO",
			"PS",
			"PU",
			"RC",
			"RD",
			"RE",
			"RO",
			"RS",
			"RU",
			"SA",
			"SO",
			"SU",
			"SY",
			"WD",
			"WR"
		};

		// Token: 0x0400012C RID: 300
		private static Dictionary<string, string> s_map = new Dictionary<string, string>();
	}
}
