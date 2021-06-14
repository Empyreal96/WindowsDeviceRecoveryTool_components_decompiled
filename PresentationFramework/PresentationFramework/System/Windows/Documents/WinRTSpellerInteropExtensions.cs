using System;
using System.Collections.Generic;
using System.Windows.Documents.MsSpellCheckLib;
using MS.Internal.WindowsRuntime.Windows.Data.Text;

namespace System.Windows.Documents
{
	// Token: 0x02000431 RID: 1073
	internal static class WinRTSpellerInteropExtensions
	{
		// Token: 0x06003F1B RID: 16155 RVA: 0x00120350 File Offset: 0x0011E550
		public static IReadOnlyList<WinRTSpellerInterop.SpellerSegment> ComprehensiveGetTokens(this WordsSegmenter segmenter, string text, SpellChecker spellChecker, WinRTSpellerInterop owner)
		{
			IReadOnlyList<WordSegment> readOnlyList = ((segmenter != null) ? segmenter.GetTokens(text) : null) ?? new List<WordSegment>().AsReadOnly();
			List<WinRTSpellerInterop.SpellerSegment> list = new List<WinRTSpellerInterop.SpellerSegment>();
			if (readOnlyList.Count == 0)
			{
				return list.AsReadOnly();
			}
			int num = 0;
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				int startPosition = (int)readOnlyList[i].SourceTextSegment.StartPosition;
				int length = (int)readOnlyList[i].SourceTextSegment.Length;
				if (spellChecker != null && startPosition > num)
				{
					WinRTSpellerInterop.SpellerSegment missingFragment = new WinRTSpellerInterop.SpellerSegment(text, new WinRTSpellerInterop.TextRange(num, startPosition - num), spellChecker, owner);
					if (list.Count > 0)
					{
						WinRTSpellerInterop.TextRange? spellCheckCleanSubstitutionToken = WinRTSpellerInteropExtensions.GetSpellCheckCleanSubstitutionToken(spellChecker, text, list[list.Count - 1], missingFragment);
						if (spellCheckCleanSubstitutionToken != null)
						{
							list[list.Count - 1] = new WinRTSpellerInterop.SpellerSegment(text, spellCheckCleanSubstitutionToken.Value, spellChecker, owner);
						}
					}
				}
				list.Add(new WinRTSpellerInterop.SpellerSegment(text, new WinRTSpellerInterop.TextRange(startPosition, length), spellChecker, owner));
				num = startPosition + length;
			}
			if (readOnlyList.Count > 0 && spellChecker != null && spellChecker.HasErrors(readOnlyList[readOnlyList.Count - 1].Text, true) && num < text.Length)
			{
				WinRTSpellerInterop.SpellerSegment missingFragment2 = new WinRTSpellerInterop.SpellerSegment(text, new WinRTSpellerInterop.TextRange(num, text.Length - num), spellChecker, owner);
				if (list.Count > 0)
				{
					WinRTSpellerInterop.TextRange? spellCheckCleanSubstitutionToken2 = WinRTSpellerInteropExtensions.GetSpellCheckCleanSubstitutionToken(spellChecker, text, list[list.Count - 1], missingFragment2);
					if (spellCheckCleanSubstitutionToken2 != null)
					{
						list[list.Count - 1] = new WinRTSpellerInterop.SpellerSegment(text, spellCheckCleanSubstitutionToken2.Value, spellChecker, owner);
					}
				}
			}
			return list.AsReadOnly();
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x00120504 File Offset: 0x0011E704
		private static WinRTSpellerInterop.TextRange? GetSpellCheckCleanSubstitutionToken(SpellChecker spellChecker, string documentText, WinRTSpellerInterop.SpellerSegment lastToken, WinRTSpellerInterop.SpellerSegment missingFragment)
		{
			string text = (lastToken != null) ? lastToken.Text : null;
			string text2 = (missingFragment != null) ? missingFragment.Text.TrimEnd(new char[1]) : null;
			if (string.IsNullOrWhiteSpace(text2) || string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(documentText) || spellChecker == null || !spellChecker.HasErrors(text, true))
			{
				return null;
			}
			string text3 = text;
			int num = Math.Min(text2.Length, 4);
			for (int i = 1; i <= num; i++)
			{
				string text4 = documentText.Substring(lastToken.TextRange.Start, text.Length + i).TrimEnd(new char[1]).TrimEnd(new char[0]);
				if (text4.Length > text3.Length)
				{
					text3 = text4;
					if (!spellChecker.HasErrors(text4, true))
					{
						return new WinRTSpellerInterop.TextRange?(new WinRTSpellerInterop.TextRange(lastToken.TextRange.Start, text4.Length));
					}
				}
			}
			return null;
		}
	}
}
