using System;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000041 RID: 65
	internal sealed class NonOptionSelectExpandTermParser : SelectExpandTermParser
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00007249 File Offset: 0x00005449
		public NonOptionSelectExpandTermParser(string clauseToParse, int maxDepth) : base(clauseToParse, maxDepth)
		{
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007253 File Offset: 0x00005453
		internal override ExpandTermToken BuildExpandTermToken(bool isInnerTerm, PathSegmentToken pathToken)
		{
			if (this.IsNotEndOfTerm(false))
			{
				throw new ODataException(Strings.UriSelectParser_TermIsNotValid(this.Lexer.ExpressionText));
			}
			return new ExpandTermToken(pathToken);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000727A File Offset: 0x0000547A
		internal override bool IsNotEndOfTerm(bool isInnerTerm)
		{
			return this.Lexer.CurrentToken.Kind != ExpressionTokenKind.End && this.Lexer.CurrentToken.Kind != ExpressionTokenKind.Comma;
		}
	}
}
