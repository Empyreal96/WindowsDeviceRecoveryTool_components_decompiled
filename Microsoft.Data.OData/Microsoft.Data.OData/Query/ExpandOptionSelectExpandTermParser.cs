using System;
using System.Linq;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000026 RID: 38
	internal sealed class ExpandOptionSelectExpandTermParser : SelectExpandTermParser
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x0000488B File Offset: 0x00002A8B
		public ExpandOptionSelectExpandTermParser(string clauseToParse, int maxDepth) : base(clauseToParse, maxDepth)
		{
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004898 File Offset: 0x00002A98
		internal override ExpandTermToken BuildExpandTermToken(bool isInnerTerm, PathSegmentToken pathToken)
		{
			QueryToken filterOption = null;
			OrderByToken orderByOption = null;
			long? topOption = null;
			long? skipOption = null;
			InlineCountKind? inlineCountOption = null;
			SelectToken selectOption = null;
			ExpandToken expandOption = null;
			if (this.Lexer.CurrentToken.Kind == ExpressionTokenKind.OpenParen)
			{
				while (this.Lexer.PeekNextToken().Kind != ExpressionTokenKind.CloseParen)
				{
					string text;
					switch (text = this.Lexer.NextToken().Text)
					{
					case "$filter":
					{
						this.Lexer.NextToken();
						string filter = this.ReadQueryOption();
						UriQueryExpressionParser uriQueryExpressionParser = new UriQueryExpressionParser(base.MaxDepth);
						filterOption = uriQueryExpressionParser.ParseFilter(filter);
						continue;
					}
					case "$orderby":
					{
						this.Lexer.NextToken();
						string orderBy = this.ReadQueryOption();
						UriQueryExpressionParser uriQueryExpressionParser2 = new UriQueryExpressionParser(base.MaxDepth);
						orderByOption = uriQueryExpressionParser2.ParseOrderBy(orderBy).Single<OrderByToken>();
						continue;
					}
					case "$top":
					{
						this.Lexer.NextToken();
						string text2 = this.ReadQueryOption();
						long value;
						if (!long.TryParse(text2, out value))
						{
							throw new ODataException(Strings.UriSelectParser_InvalidTopOption(text2));
						}
						topOption = new long?(value);
						continue;
					}
					case "$skip":
					{
						this.Lexer.NextToken();
						string text3 = this.ReadQueryOption();
						long value2;
						if (!long.TryParse(text3, out value2))
						{
							throw new ODataException(Strings.UriSelectParser_InvalidSkipOption(text3));
						}
						skipOption = new long?(value2);
						continue;
					}
					case "$inlinecount":
					{
						this.Lexer.NextToken();
						string text4 = this.ReadQueryOption();
						string a;
						if ((a = text4) != null)
						{
							if (a == "none")
							{
								inlineCountOption = new InlineCountKind?(InlineCountKind.None);
								continue;
							}
							if (a == "allpages")
							{
								inlineCountOption = new InlineCountKind?(InlineCountKind.AllPages);
								continue;
							}
						}
						throw new ODataException(Strings.UriSelectParser_TermIsNotValid(this.Lexer.ExpressionText));
					}
					case "$select":
						this.Lexer.NextToken();
						selectOption = base.ParseSelect();
						continue;
					case "$expand":
						this.Lexer.NextToken();
						expandOption = base.ParseExpand();
						continue;
					}
					throw new ODataException(Strings.UriSelectParser_TermIsNotValid(this.Lexer.ExpressionText));
				}
			}
			else if (this.IsNotEndOfTerm(isInnerTerm))
			{
				throw new ODataException(Strings.UriSelectParser_TermIsNotValid(this.Lexer.ExpressionText));
			}
			return new ExpandTermToken(pathToken, filterOption, orderByOption, topOption, skipOption, inlineCountOption, selectOption, expandOption);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004B68 File Offset: 0x00002D68
		internal override bool IsNotEndOfTerm(bool isInnerTerm)
		{
			if (!isInnerTerm)
			{
				return this.Lexer.CurrentToken.Kind != ExpressionTokenKind.End && this.Lexer.CurrentToken.Kind != ExpressionTokenKind.Comma;
			}
			return this.Lexer.CurrentToken.Kind != ExpressionTokenKind.End && this.Lexer.CurrentToken.Kind != ExpressionTokenKind.Comma && this.Lexer.CurrentToken.Kind != ExpressionTokenKind.SemiColon;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004BE8 File Offset: 0x00002DE8
		private string ReadQueryOption()
		{
			if (this.Lexer.CurrentToken.Kind != ExpressionTokenKind.Equal)
			{
				throw new ODataException(Strings.UriSelectParser_TermIsNotValid(this.Lexer.ExpressionText));
			}
			this.Lexer.NextToken();
			string text = this.Lexer.ExpressionText.Substring(this.Lexer.Position);
			text = text.Split(new char[]
			{
				';'
			}).First<string>();
			while (this.Lexer.PeekNextToken().Kind != ExpressionTokenKind.SemiColon)
			{
				this.Lexer.NextToken();
			}
			this.Lexer.NextToken();
			return text;
		}
	}
}
