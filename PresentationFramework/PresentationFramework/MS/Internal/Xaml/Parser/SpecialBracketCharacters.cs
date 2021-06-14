using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Internal.Xaml.Parser
{
	// Token: 0x02000806 RID: 2054
	internal class SpecialBracketCharacters : ISupportInitialize
	{
		// Token: 0x06007E06 RID: 32262 RVA: 0x002352DB File Offset: 0x002334DB
		internal SpecialBracketCharacters()
		{
			this.BeginInit();
		}

		// Token: 0x06007E07 RID: 32263 RVA: 0x002352E9 File Offset: 0x002334E9
		internal SpecialBracketCharacters(IReadOnlyDictionary<char, char> attributeList)
		{
			this.BeginInit();
			if (attributeList != null && attributeList.Count > 0)
			{
				this.Tokenize(attributeList);
			}
		}

		// Token: 0x06007E08 RID: 32264 RVA: 0x0023530A File Offset: 0x0023350A
		internal void AddBracketCharacters(char openingBracket, char closingBracket)
		{
			if (this._initializing)
			{
				this._startCharactersStringBuilder.Append(openingBracket);
				this._endCharactersStringBuilder.Append(closingBracket);
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x00235334 File Offset: 0x00233534
		private void Tokenize(IReadOnlyDictionary<char, char> attributeList)
		{
			if (this._initializing)
			{
				foreach (char c in attributeList.Keys)
				{
					char c2 = attributeList[c];
					string empty = string.Empty;
					if (this.IsValidBracketCharacter(c, c2))
					{
						this._startCharactersStringBuilder.Append(c);
						this._endCharactersStringBuilder.Append(c2);
					}
				}
			}
		}

		// Token: 0x06007E0A RID: 32266 RVA: 0x002353B4 File Offset: 0x002335B4
		private bool IsValidBracketCharacter(char openingBracket, char closingBracket)
		{
			if (openingBracket == closingBracket)
			{
				throw new InvalidOperationException("Opening bracket character cannot be the same as closing bracket character.");
			}
			if (char.IsLetterOrDigit(openingBracket) || char.IsLetterOrDigit(closingBracket) || char.IsWhiteSpace(openingBracket) || char.IsWhiteSpace(closingBracket))
			{
				throw new InvalidOperationException("Bracket characters cannot be alpha-numeric or whitespace.");
			}
			if (SpecialBracketCharacters._restrictedCharSet.Contains(openingBracket) || SpecialBracketCharacters._restrictedCharSet.Contains(closingBracket))
			{
				throw new InvalidOperationException("Bracket characters cannot be one of the following: '=' , ',', ''', '\"', '{ ', ' }', '\\'");
			}
			return true;
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x00235421 File Offset: 0x00233621
		internal bool IsSpecialCharacter(char ch)
		{
			return this._startChars.Contains(ch.ToString()) || this._endChars.Contains(ch.ToString());
		}

		// Token: 0x06007E0C RID: 32268 RVA: 0x0023544B File Offset: 0x0023364B
		internal bool StartsEscapeSequence(char ch)
		{
			return this._startChars.Contains(ch.ToString());
		}

		// Token: 0x06007E0D RID: 32269 RVA: 0x0023545F File Offset: 0x0023365F
		internal bool EndsEscapeSequence(char ch)
		{
			return this._endChars.Contains(ch.ToString());
		}

		// Token: 0x06007E0E RID: 32270 RVA: 0x00235473 File Offset: 0x00233673
		internal bool Match(char start, char end)
		{
			return this._endChars.IndexOf(end.ToString()) == this._startChars.IndexOf(start.ToString());
		}

		// Token: 0x17001D49 RID: 7497
		// (get) Token: 0x06007E0F RID: 32271 RVA: 0x0023549B File Offset: 0x0023369B
		internal string StartBracketCharacters
		{
			get
			{
				return this._startChars;
			}
		}

		// Token: 0x17001D4A RID: 7498
		// (get) Token: 0x06007E10 RID: 32272 RVA: 0x002354A3 File Offset: 0x002336A3
		internal string EndBracketCharacters
		{
			get
			{
				return this._endChars;
			}
		}

		// Token: 0x06007E11 RID: 32273 RVA: 0x002354AB File Offset: 0x002336AB
		public void BeginInit()
		{
			this._initializing = true;
			this._startCharactersStringBuilder = new StringBuilder();
			this._endCharactersStringBuilder = new StringBuilder();
		}

		// Token: 0x06007E12 RID: 32274 RVA: 0x002354CA File Offset: 0x002336CA
		public void EndInit()
		{
			this._startChars = this._startCharactersStringBuilder.ToString();
			this._endChars = this._endCharactersStringBuilder.ToString();
			this._initializing = false;
		}

		// Token: 0x04003B7C RID: 15228
		private string _startChars;

		// Token: 0x04003B7D RID: 15229
		private string _endChars;

		// Token: 0x04003B7E RID: 15230
		private static readonly ISet<char> _restrictedCharSet = new SortedSet<char>(new char[]
		{
			'=',
			',',
			'\'',
			'"',
			'{',
			'}',
			'\\'
		});

		// Token: 0x04003B7F RID: 15231
		private bool _initializing;

		// Token: 0x04003B80 RID: 15232
		private StringBuilder _startCharactersStringBuilder;

		// Token: 0x04003B81 RID: 15233
		private StringBuilder _endCharactersStringBuilder;
	}
}
