using System;
using System.Collections.Generic;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000025 RID: 37
	internal abstract class SelectExpandTermParser : ISelectExpandTermParser
	{
		// Token: 0x060000EC RID: 236 RVA: 0x0000452E File Offset: 0x0000272E
		protected SelectExpandTermParser(string clauseToParse, int maxDepth)
		{
			this.maxDepth = maxDepth;
			this.recursionDepth = 0;
			this.Lexer = ((clauseToParse != null) ? new ExpressionLexer(clauseToParse, false, true) : null);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004558 File Offset: 0x00002758
		public int MaxDepth
		{
			get
			{
				return this.maxDepth;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004560 File Offset: 0x00002760
		public SelectToken ParseSelect()
		{
			this.isSelect = true;
			if (this.Lexer == null)
			{
				return new SelectToken(new List<PathSegmentToken>());
			}
			List<PathSegmentToken> list = new List<PathSegmentToken>();
			bool isInnerTerm = this.Lexer.CurrentToken.Kind == ExpressionTokenKind.Equal;
			while (this.Lexer.PeekNextToken().Kind != ExpressionTokenKind.End && this.Lexer.PeekNextToken().Kind != ExpressionTokenKind.CloseParen)
			{
				list.Add(this.ParseSingleSelectTerm(isInnerTerm));
			}
			return new SelectToken(list);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000045E0 File Offset: 0x000027E0
		public ExpandToken ParseExpand()
		{
			this.isSelect = false;
			if (this.Lexer == null)
			{
				return new ExpandToken(new List<ExpandTermToken>());
			}
			List<ExpandTermToken> list = new List<ExpandTermToken>();
			bool isInnerTerm = this.Lexer.CurrentToken.Kind == ExpressionTokenKind.Equal;
			while (this.Lexer.PeekNextToken().Kind != ExpressionTokenKind.End && this.Lexer.PeekNextToken().Kind != ExpressionTokenKind.CloseParen)
			{
				list.Add(this.ParseSingleExpandTerm(isInnerTerm));
			}
			return new ExpandToken(list);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004660 File Offset: 0x00002860
		public PathSegmentToken ParseSingleSelectTerm(bool isInnerTerm)
		{
			this.isSelect = true;
			PathSegmentToken result = this.ParseSelectExpandProperty();
			if (this.IsNotEndOfTerm(isInnerTerm))
			{
				throw new ODataException(Strings.UriSelectParser_TermIsNotValid(this.Lexer.ExpressionText));
			}
			return result;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000469C File Offset: 0x0000289C
		public ExpandTermToken ParseSingleExpandTerm(bool isInnerTerm)
		{
			this.isSelect = false;
			this.RecurseEnter();
			PathSegmentToken pathToken = this.ParseSelectExpandProperty();
			this.RecurseLeave();
			return this.BuildExpandTermToken(isInnerTerm, pathToken);
		}

		// Token: 0x060000F2 RID: 242
		internal abstract ExpandTermToken BuildExpandTermToken(bool isInnerTerm, PathSegmentToken pathToken);

		// Token: 0x060000F3 RID: 243
		internal abstract bool IsNotEndOfTerm(bool isInnerTerm);

		// Token: 0x060000F4 RID: 244 RVA: 0x000046CC File Offset: 0x000028CC
		private PathSegmentToken ParseSelectExpandProperty()
		{
			PathSegmentToken pathSegmentToken = null;
			int num = 0;
			for (;;)
			{
				num++;
				if (num > this.maxDepth)
				{
					break;
				}
				this.Lexer.NextToken();
				if (num > 1 && this.Lexer.CurrentToken.Kind == ExpressionTokenKind.End)
				{
					return pathSegmentToken;
				}
				pathSegmentToken = this.ParseNext(pathSegmentToken);
				if (this.Lexer.CurrentToken.Kind != ExpressionTokenKind.Slash)
				{
					return pathSegmentToken;
				}
			}
			throw new ODataException(Strings.UriQueryExpressionParser_TooDeep);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004738 File Offset: 0x00002938
		private PathSegmentToken ParseNext(PathSegmentToken previousToken)
		{
			if (this.Lexer.CurrentToken.Text.StartsWith("$", StringComparison.CurrentCulture))
			{
				throw new ODataException(Strings.UriSelectParser_SystemTokenInSelectExpand(this.Lexer.CurrentToken.Text, this.Lexer.ExpressionText));
			}
			return this.ParseSegment(previousToken);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004790 File Offset: 0x00002990
		private PathSegmentToken ParseSegment(PathSegmentToken parent)
		{
			string identifier;
			if (this.Lexer.PeekNextToken().Kind == ExpressionTokenKind.Dot)
			{
				identifier = this.Lexer.ReadDottedIdentifier(this.isSelect);
			}
			else if (this.Lexer.CurrentToken.Kind == ExpressionTokenKind.Star)
			{
				if (this.Lexer.PeekNextToken().Kind == ExpressionTokenKind.Slash)
				{
					throw new ODataException(Strings.ExpressionToken_IdentifierExpected(this.Lexer.Position));
				}
				identifier = this.Lexer.CurrentToken.Text;
				this.Lexer.NextToken();
			}
			else
			{
				identifier = this.Lexer.CurrentToken.GetIdentifier();
				this.Lexer.NextToken();
			}
			return new NonSystemToken(identifier, null, parent);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004852 File Offset: 0x00002A52
		private void RecurseEnter()
		{
			this.recursionDepth++;
			if (this.recursionDepth > this.maxDepth)
			{
				throw new ODataException(Strings.UriQueryExpressionParser_TooDeep);
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000487B File Offset: 0x00002A7B
		private void RecurseLeave()
		{
			this.recursionDepth--;
		}

		// Token: 0x04000051 RID: 81
		public ExpressionLexer Lexer;

		// Token: 0x04000052 RID: 82
		private bool isSelect;

		// Token: 0x04000053 RID: 83
		private int maxDepth;

		// Token: 0x04000054 RID: 84
		private int recursionDepth;
	}
}
