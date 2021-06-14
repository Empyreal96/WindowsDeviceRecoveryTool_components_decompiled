using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200002D RID: 45
	internal sealed class FunctionCallParser : IFunctionCallParser
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00005B96 File Offset: 0x00003D96
		public FunctionCallParser(ExpressionLexer lexer, UriQueryExpressionParser.Parser parseMethod)
		{
			ExceptionUtils.CheckArgumentNotNull<ExpressionLexer>(lexer, "lexer");
			ExceptionUtils.CheckArgumentNotNull<UriQueryExpressionParser.Parser>(parseMethod, "parseMethod");
			this.lexer = lexer;
			this.parseMethod = parseMethod;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00005BC2 File Offset: 0x00003DC2
		public ExpressionLexer Lexer
		{
			get
			{
				return this.lexer;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005BCC File Offset: 0x00003DCC
		public QueryToken ParseIdentifierAsFunction(QueryToken parent)
		{
			string name;
			if (this.Lexer.PeekNextToken().Kind == ExpressionTokenKind.Dot)
			{
				name = this.Lexer.ReadDottedIdentifier(false);
			}
			else
			{
				name = this.Lexer.CurrentToken.Text;
				this.Lexer.NextToken();
			}
			FunctionParameterToken[] arguments = this.ParseArgumentList();
			return new FunctionCallToken(name, arguments, parent);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005C28 File Offset: 0x00003E28
		public FunctionParameterToken[] ParseArgumentList()
		{
			if (this.Lexer.CurrentToken.Kind != ExpressionTokenKind.OpenParen)
			{
				throw new ODataException(Strings.UriQueryExpressionParser_OpenParenExpected(this.Lexer.CurrentToken.Position, this.Lexer.ExpressionText));
			}
			this.Lexer.NextToken();
			FunctionParameterToken[] result;
			if (this.Lexer.CurrentToken.Kind == ExpressionTokenKind.CloseParen)
			{
				result = FunctionParameterToken.EmptyParameterList;
			}
			else
			{
				result = this.ParseArguments();
			}
			if (this.Lexer.CurrentToken.Kind != ExpressionTokenKind.CloseParen)
			{
				throw new ODataException(Strings.UriQueryExpressionParser_CloseParenOrCommaExpected(this.Lexer.CurrentToken.Position, this.Lexer.ExpressionText));
			}
			this.Lexer.NextToken();
			return result;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005CF0 File Offset: 0x00003EF0
		public FunctionParameterToken[] ParseArguments()
		{
			ICollection<FunctionParameterToken> source;
			if (this.TryReadArgumentsAsNamedValues(out source))
			{
				return source.ToArray<FunctionParameterToken>();
			}
			return this.ReadArgumentsAsPositionalValues().ToArray();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005D1C File Offset: 0x00003F1C
		private List<FunctionParameterToken> ReadArgumentsAsPositionalValues()
		{
			List<FunctionParameterToken> list = new List<FunctionParameterToken>();
			for (;;)
			{
				list.Add(new FunctionParameterToken(null, this.parseMethod()));
				if (this.Lexer.CurrentToken.Kind != ExpressionTokenKind.Comma)
				{
					break;
				}
				this.Lexer.NextToken();
			}
			return list;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005D74 File Offset: 0x00003F74
		private bool TryReadArgumentsAsNamedValues(out ICollection<FunctionParameterToken> argList)
		{
			if (!this.lexer.TrySplitFunctionParameters(out argList))
			{
				return false;
			}
			if ((from t in argList
			select t.ParameterName).Distinct<string>().Count<string>() != argList.Count)
			{
				throw new ODataException(Strings.FunctionCallParser_DuplicateParameterName);
			}
			return true;
		}

		// Token: 0x0400005D RID: 93
		private readonly ExpressionLexer lexer;

		// Token: 0x0400005E RID: 94
		private readonly UriQueryExpressionParser.Parser parseMethod;
	}
}
