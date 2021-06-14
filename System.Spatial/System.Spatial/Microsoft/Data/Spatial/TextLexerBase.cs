using System;
using System.IO;
using System.Text;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000048 RID: 72
	internal abstract class TextLexerBase
	{
		// Token: 0x060001DE RID: 478 RVA: 0x000057FC File Offset: 0x000039FC
		protected TextLexerBase(TextReader text)
		{
			this.reader = text;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000580B File Offset: 0x00003A0B
		public LexerToken CurrentToken
		{
			get
			{
				return this.currentToken;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00005814 File Offset: 0x00003A14
		public bool Peek(out LexerToken token)
		{
			if (this.peekToken != null)
			{
				token = this.peekToken;
				return true;
			}
			LexerToken lexerToken = this.currentToken;
			if (this.Next())
			{
				this.peekToken = this.currentToken;
				token = this.currentToken;
				this.currentToken = lexerToken;
				return true;
			}
			this.peekToken = null;
			token = null;
			this.currentToken = lexerToken;
			return false;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00005874 File Offset: 0x00003A74
		public bool Next()
		{
			if (this.peekToken != null)
			{
				this.currentToken = this.peekToken;
				this.peekToken = null;
				return true;
			}
			LexerToken lexerToken = this.CurrentToken;
			int? num = null;
			StringBuilder stringBuilder = null;
			bool flag = false;
			int num2;
			while (!flag && (num2 = this.reader.Peek()) >= 0)
			{
				char c = (char)num2;
				int num3;
				flag = this.MatchTokenType(c, num, out num3);
				if (num == null)
				{
					num = new int?(num3);
					stringBuilder = new StringBuilder();
					stringBuilder.Append(c);
					this.reader.Read();
				}
				else if (num == num3)
				{
					stringBuilder.Append(c);
					this.reader.Read();
				}
				else
				{
					flag = true;
				}
			}
			if (num != null)
			{
				this.currentToken = new LexerToken
				{
					Text = stringBuilder.ToString(),
					Type = num.Value
				};
			}
			return lexerToken != this.currentToken;
		}

		// Token: 0x060001E2 RID: 482
		protected abstract bool MatchTokenType(char nextChar, int? currentType, out int type);

		// Token: 0x04000048 RID: 72
		private TextReader reader;

		// Token: 0x04000049 RID: 73
		private LexerToken currentToken;

		// Token: 0x0400004A RID: 74
		private LexerToken peekToken;
	}
}
