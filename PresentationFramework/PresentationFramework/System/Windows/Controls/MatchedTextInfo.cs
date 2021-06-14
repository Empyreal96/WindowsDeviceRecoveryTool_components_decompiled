using System;

namespace System.Windows.Controls
{
	// Token: 0x02000468 RID: 1128
	internal class MatchedTextInfo
	{
		// Token: 0x060041D4 RID: 16852 RVA: 0x0012DE68 File Offset: 0x0012C068
		internal MatchedTextInfo(int matchedItemIndex, string matchedText, int matchedPrefixLength, int textExcludingPrefixLength)
		{
			this._matchedItemIndex = matchedItemIndex;
			this._matchedText = matchedText;
			this._matchedPrefixLength = matchedPrefixLength;
			this._textExcludingPrefixLength = textExcludingPrefixLength;
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x060041D5 RID: 16853 RVA: 0x0012DE8D File Offset: 0x0012C08D
		internal static MatchedTextInfo NoMatch
		{
			get
			{
				return MatchedTextInfo.s_NoMatch;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x060041D6 RID: 16854 RVA: 0x0012DE94 File Offset: 0x0012C094
		internal string MatchedText
		{
			get
			{
				return this._matchedText;
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x060041D7 RID: 16855 RVA: 0x0012DE9C File Offset: 0x0012C09C
		internal int MatchedItemIndex
		{
			get
			{
				return this._matchedItemIndex;
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x0012DEA4 File Offset: 0x0012C0A4
		internal int MatchedPrefixLength
		{
			get
			{
				return this._matchedPrefixLength;
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x060041D9 RID: 16857 RVA: 0x0012DEAC File Offset: 0x0012C0AC
		internal int TextExcludingPrefixLength
		{
			get
			{
				return this._textExcludingPrefixLength;
			}
		}

		// Token: 0x040027B5 RID: 10165
		private readonly string _matchedText;

		// Token: 0x040027B6 RID: 10166
		private readonly int _matchedItemIndex;

		// Token: 0x040027B7 RID: 10167
		private readonly int _matchedPrefixLength;

		// Token: 0x040027B8 RID: 10168
		private readonly int _textExcludingPrefixLength;

		// Token: 0x040027B9 RID: 10169
		private static MatchedTextInfo s_NoMatch = new MatchedTextInfo(-1, null, 0, 0);
	}
}
