using System;
using System.Collections.Generic;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200002E RID: 46
	internal sealed class IdentifierTokenizer
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00005DD4 File Offset: 0x00003FD4
		public IdentifierTokenizer(HashSet<string> parameters, IFunctionCallParser functionCallParser)
		{
			ExceptionUtils.CheckArgumentNotNull<HashSet<string>>(parameters, "parameters");
			ExceptionUtils.CheckArgumentNotNull<IFunctionCallParser>(functionCallParser, "functionCallParser");
			this.lexer = functionCallParser.Lexer;
			this.parameters = parameters;
			this.functionCallParser = functionCallParser;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005E0C File Offset: 0x0000400C
		public QueryToken ParseIdentifier(QueryToken parent)
		{
			this.lexer.ValidateToken(ExpressionTokenKind.Identifier);
			bool flag = this.lexer.ExpandIdentifierAsFunction();
			if (flag)
			{
				return this.functionCallParser.ParseIdentifierAsFunction(parent);
			}
			if (this.lexer.PeekNextToken().Kind == ExpressionTokenKind.Dot)
			{
				string identifier = this.lexer.ReadDottedIdentifier(false);
				return new DottedIdentifierToken(identifier, parent);
			}
			return this.ParseMemberAccess(parent);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005E74 File Offset: 0x00004074
		public QueryToken ParseMemberAccess(QueryToken instance)
		{
			if (this.lexer.CurrentToken.Text == "*")
			{
				return this.ParseStarMemberAccess(instance);
			}
			string identifier = this.lexer.CurrentToken.GetIdentifier();
			if (instance == null && this.parameters.Contains(identifier))
			{
				this.lexer.NextToken();
				return new RangeVariableToken(identifier);
			}
			this.lexer.NextToken();
			return new EndPathToken(identifier, instance);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005EF0 File Offset: 0x000040F0
		public QueryToken ParseStarMemberAccess(QueryToken instance)
		{
			if (this.lexer.CurrentToken.Text != "*")
			{
				throw IdentifierTokenizer.ParseError(Strings.UriQueryExpressionParser_CannotCreateStarTokenFromNonStar(this.lexer.CurrentToken.Text));
			}
			this.lexer.NextToken();
			return new StarToken(instance);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005F46 File Offset: 0x00004146
		private static Exception ParseError(string message)
		{
			return new ODataException(message);
		}

		// Token: 0x04000060 RID: 96
		private readonly ExpressionLexer lexer;

		// Token: 0x04000061 RID: 97
		private readonly HashSet<string> parameters;

		// Token: 0x04000062 RID: 98
		private readonly IFunctionCallParser functionCallParser;
	}
}
