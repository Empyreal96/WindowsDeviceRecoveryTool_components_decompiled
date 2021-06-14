using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000DE RID: 222
	internal sealed class UriQueryExpressionParser
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x00012818 File Offset: 0x00010A18
		internal UriQueryExpressionParser(int maxDepth)
		{
			this.maxDepth = maxDepth;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00012850 File Offset: 0x00010A50
		internal static LiteralToken TryParseLiteral(ExpressionLexer lexer)
		{
			switch (lexer.CurrentToken.Kind)
			{
			case ExpressionTokenKind.NullLiteral:
				return UriQueryExpressionParser.ParseNullLiteral(lexer);
			case ExpressionTokenKind.BooleanLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetBoolean(false), "Edm.Boolean");
			case ExpressionTokenKind.StringLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetString(true), "Edm.String");
			case ExpressionTokenKind.IntegerLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetInt32(false), "Edm.Int32");
			case ExpressionTokenKind.Int64Literal:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetInt64(false), "Edm.Int64");
			case ExpressionTokenKind.SingleLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetSingle(false), "Edm.Single");
			case ExpressionTokenKind.DateTimeLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTime, false), "Edm.DateTime");
			case ExpressionTokenKind.DateTimeOffsetLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTimeOffset, false), "Edm.DateTimeOffset");
			case ExpressionTokenKind.TimeLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.Time, false), "Edm.Time");
			case ExpressionTokenKind.DecimalLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetDecimal(false), "Edm.Decimal");
			case ExpressionTokenKind.DoubleLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetDouble(false), "Edm.Double");
			case ExpressionTokenKind.GuidLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetGuid(false), "Edm.Guid");
			case ExpressionTokenKind.BinaryLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetBinary(true), "Edm.Binary");
			case ExpressionTokenKind.GeographyLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.Geography, false), "Edm.Geography");
			case ExpressionTokenKind.GeometryLiteral:
				return UriQueryExpressionParser.ParseTypedLiteral(lexer, EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.Geometry, false), "Edm.Geometry");
			default:
				return null;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00012A04 File Offset: 0x00010C04
		internal QueryToken ParseFilter(string filter)
		{
			this.recursionDepth = 0;
			this.lexer = UriQueryExpressionParser.CreateLexerForFilterOrOrderByExpression(filter);
			QueryToken result = this.ParseExpression();
			this.lexer.ValidateToken(ExpressionTokenKind.End);
			return result;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00012A38 File Offset: 0x00010C38
		internal IEnumerable<OrderByToken> ParseOrderBy(string orderBy)
		{
			this.recursionDepth = 0;
			this.lexer = UriQueryExpressionParser.CreateLexerForFilterOrOrderByExpression(orderBy);
			List<OrderByToken> list = new List<OrderByToken>();
			for (;;)
			{
				QueryToken expression = this.ParseExpression();
				bool flag = true;
				if (this.TokenIdentifierIs("asc"))
				{
					this.lexer.NextToken();
				}
				else if (this.TokenIdentifierIs("desc"))
				{
					this.lexer.NextToken();
					flag = false;
				}
				OrderByToken item = new OrderByToken(expression, flag ? OrderByDirection.Ascending : OrderByDirection.Descending);
				list.Add(item);
				if (this.lexer.CurrentToken.Kind != ExpressionTokenKind.Comma)
				{
					break;
				}
				this.lexer.NextToken();
			}
			this.lexer.ValidateToken(ExpressionTokenKind.End);
			return new ReadOnlyCollection<OrderByToken>(list);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00012AE6 File Offset: 0x00010CE6
		private static ExpressionLexer CreateLexerForFilterOrOrderByExpression(string expression)
		{
			return new ExpressionLexer(expression, true, false, true);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00012AF1 File Offset: 0x00010CF1
		private static Exception ParseError(string message)
		{
			return new ODataException(message);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00012AFC File Offset: 0x00010CFC
		private static LiteralToken ParseTypedLiteral(ExpressionLexer lexer, IEdmPrimitiveTypeReference targetTypeReference, string targetTypeName)
		{
			object value;
			if (!UriPrimitiveTypeParser.TryUriStringToPrimitive(lexer.CurrentToken.Text, targetTypeReference, out value))
			{
				string message = Strings.UriQueryExpressionParser_UnrecognizedLiteral(targetTypeName, lexer.CurrentToken.Text, lexer.CurrentToken.Position, lexer.ExpressionText);
				throw UriQueryExpressionParser.ParseError(message);
			}
			LiteralToken result = new LiteralToken(value, lexer.CurrentToken.Text);
			lexer.NextToken();
			return result;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00012B68 File Offset: 0x00010D68
		private static LiteralToken ParseNullLiteral(ExpressionLexer lexer)
		{
			LiteralToken result = new LiteralToken(null, lexer.CurrentToken.Text);
			lexer.NextToken();
			return result;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00012B90 File Offset: 0x00010D90
		private QueryToken ParseExpression()
		{
			this.RecurseEnter();
			QueryToken result = this.ParseLogicalOr();
			this.RecurseLeave();
			return result;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00012BB4 File Offset: 0x00010DB4
		private QueryToken ParseLogicalOr()
		{
			this.RecurseEnter();
			QueryToken queryToken = this.ParseLogicalAnd();
			while (this.TokenIdentifierIs("or"))
			{
				this.lexer.NextToken();
				QueryToken right = this.ParseLogicalAnd();
				queryToken = new BinaryOperatorToken(BinaryOperatorKind.Or, queryToken, right);
			}
			this.RecurseLeave();
			return queryToken;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00012C00 File Offset: 0x00010E00
		private QueryToken ParseLogicalAnd()
		{
			this.RecurseEnter();
			QueryToken queryToken = this.ParseComparison();
			while (this.TokenIdentifierIs("and"))
			{
				this.lexer.NextToken();
				QueryToken right = this.ParseComparison();
				queryToken = new BinaryOperatorToken(BinaryOperatorKind.And, queryToken, right);
			}
			this.RecurseLeave();
			return queryToken;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00012C4C File Offset: 0x00010E4C
		private QueryToken ParseComparison()
		{
			this.RecurseEnter();
			QueryToken queryToken = this.ParseAdditive();
			while (this.lexer.CurrentToken.IsComparisonOperator)
			{
				string text;
				if ((text = this.lexer.CurrentToken.Text) != null)
				{
					if (<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6000536-1 == null)
					{
						<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6000536-1 = new Dictionary<string, int>(6)
						{
							{
								"eq",
								0
							},
							{
								"ne",
								1
							},
							{
								"gt",
								2
							},
							{
								"ge",
								3
							},
							{
								"lt",
								4
							},
							{
								"le",
								5
							}
						};
					}
					int num;
					if (<PrivateImplementationDetails>{D7F3BFF1-6690-4BAC-BAB5-77DD89A1B7E1}.$$method0x6000536-1.TryGetValue(text, out num))
					{
						BinaryOperatorKind operatorKind;
						switch (num)
						{
						case 0:
							operatorKind = BinaryOperatorKind.Equal;
							break;
						case 1:
							operatorKind = BinaryOperatorKind.NotEqual;
							break;
						case 2:
							operatorKind = BinaryOperatorKind.GreaterThan;
							break;
						case 3:
							operatorKind = BinaryOperatorKind.GreaterThanOrEqual;
							break;
						case 4:
							operatorKind = BinaryOperatorKind.LessThan;
							break;
						case 5:
							operatorKind = BinaryOperatorKind.LessThanOrEqual;
							break;
						default:
							goto IL_D1;
						}
						this.lexer.NextToken();
						QueryToken right = this.ParseAdditive();
						queryToken = new BinaryOperatorToken(operatorKind, queryToken, right);
						continue;
					}
				}
				IL_D1:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.UriQueryExpressionParser_ParseComparison));
			}
			this.RecurseLeave();
			return queryToken;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00012D78 File Offset: 0x00010F78
		private QueryToken ParseAdditive()
		{
			this.RecurseEnter();
			QueryToken queryToken = this.ParseMultiplicative();
			while (this.lexer.CurrentToken.IdentifierIs("add") || this.lexer.CurrentToken.IdentifierIs("sub"))
			{
				BinaryOperatorKind operatorKind;
				if (this.lexer.CurrentToken.IdentifierIs("add"))
				{
					operatorKind = BinaryOperatorKind.Add;
				}
				else
				{
					operatorKind = BinaryOperatorKind.Subtract;
				}
				this.lexer.NextToken();
				QueryToken right = this.ParseMultiplicative();
				queryToken = new BinaryOperatorToken(operatorKind, queryToken, right);
			}
			this.RecurseLeave();
			return queryToken;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00012E10 File Offset: 0x00011010
		private QueryToken ParseMultiplicative()
		{
			this.RecurseEnter();
			QueryToken queryToken = this.ParseUnary();
			while (this.lexer.CurrentToken.IdentifierIs("mul") || this.lexer.CurrentToken.IdentifierIs("div") || this.lexer.CurrentToken.IdentifierIs("mod"))
			{
				BinaryOperatorKind operatorKind;
				if (this.lexer.CurrentToken.IdentifierIs("mul"))
				{
					operatorKind = BinaryOperatorKind.Multiply;
				}
				else if (this.lexer.CurrentToken.IdentifierIs("div"))
				{
					operatorKind = BinaryOperatorKind.Divide;
				}
				else
				{
					operatorKind = BinaryOperatorKind.Modulo;
				}
				this.lexer.NextToken();
				QueryToken right = this.ParseUnary();
				queryToken = new BinaryOperatorToken(operatorKind, queryToken, right);
			}
			this.RecurseLeave();
			return queryToken;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00012EE8 File Offset: 0x000110E8
		private QueryToken ParseUnary()
		{
			this.RecurseEnter();
			if (this.lexer.CurrentToken.Kind != ExpressionTokenKind.Minus && !this.lexer.CurrentToken.IdentifierIs("not"))
			{
				this.RecurseLeave();
				return this.ParsePrimary();
			}
			ExpressionToken currentToken = this.lexer.CurrentToken;
			this.lexer.NextToken();
			if (currentToken.Kind == ExpressionTokenKind.Minus && ExpressionLexerUtils.IsNumeric(this.lexer.CurrentToken.Kind))
			{
				ExpressionToken currentToken2 = this.lexer.CurrentToken;
				currentToken2.Text = "-" + currentToken2.Text;
				currentToken2.Position = currentToken.Position;
				this.lexer.CurrentToken = currentToken2;
				this.RecurseLeave();
				return this.ParsePrimary();
			}
			QueryToken operand = this.ParseUnary();
			UnaryOperatorKind operatorKind;
			if (currentToken.Kind == ExpressionTokenKind.Minus)
			{
				operatorKind = UnaryOperatorKind.Negate;
			}
			else
			{
				operatorKind = UnaryOperatorKind.Not;
			}
			this.RecurseLeave();
			return new UnaryOperatorToken(operatorKind, operand);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00012FE4 File Offset: 0x000111E4
		private QueryToken ParsePrimary()
		{
			this.RecurseEnter();
			QueryToken queryToken;
			if (this.lexer.PeekNextToken().Kind == ExpressionTokenKind.Slash)
			{
				queryToken = this.ParseSegment(null);
			}
			else
			{
				queryToken = this.ParsePrimaryStart();
			}
			while (this.lexer.CurrentToken.Kind == ExpressionTokenKind.Slash)
			{
				this.lexer.NextToken();
				if (this.lexer.CurrentToken.Text == "any")
				{
					queryToken = this.ParseAny(queryToken);
				}
				else if (this.lexer.CurrentToken.Text == "all")
				{
					queryToken = this.ParseAll(queryToken);
				}
				else if (this.lexer.PeekNextToken().Kind == ExpressionTokenKind.Slash)
				{
					queryToken = this.ParseSegment(queryToken);
				}
				else
				{
					IdentifierTokenizer identifierTokenizer = new IdentifierTokenizer(this.parameters, new FunctionCallParser(this.lexer, new UriQueryExpressionParser.Parser(this.ParseExpression)));
					queryToken = identifierTokenizer.ParseIdentifier(queryToken);
				}
			}
			this.RecurseLeave();
			return queryToken;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000130E4 File Offset: 0x000112E4
		private QueryToken ParsePrimaryStart()
		{
			ExpressionTokenKind kind = this.lexer.CurrentToken.Kind;
			if (kind == ExpressionTokenKind.Identifier)
			{
				IdentifierTokenizer identifierTokenizer = new IdentifierTokenizer(this.parameters, new FunctionCallParser(this.lexer, new UriQueryExpressionParser.Parser(this.ParseExpression)));
				return identifierTokenizer.ParseIdentifier(null);
			}
			if (kind == ExpressionTokenKind.OpenParen)
			{
				return this.ParseParenExpression();
			}
			if (kind == ExpressionTokenKind.Star)
			{
				IdentifierTokenizer identifierTokenizer2 = new IdentifierTokenizer(this.parameters, new FunctionCallParser(this.lexer, new UriQueryExpressionParser.Parser(this.ParseExpression)));
				return identifierTokenizer2.ParseStarMemberAccess(null);
			}
			QueryToken queryToken = UriQueryExpressionParser.TryParseLiteral(this.lexer);
			if (queryToken == null)
			{
				throw UriQueryExpressionParser.ParseError(Strings.UriQueryExpressionParser_ExpressionExpected(this.lexer.CurrentToken.Position, this.lexer.ExpressionText));
			}
			return queryToken;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x000131AC File Offset: 0x000113AC
		private QueryToken ParseParenExpression()
		{
			if (this.lexer.CurrentToken.Kind != ExpressionTokenKind.OpenParen)
			{
				throw UriQueryExpressionParser.ParseError(Strings.UriQueryExpressionParser_OpenParenExpected(this.lexer.CurrentToken.Position, this.lexer.ExpressionText));
			}
			this.lexer.NextToken();
			QueryToken result = this.ParseExpression();
			if (this.lexer.CurrentToken.Kind != ExpressionTokenKind.CloseParen)
			{
				throw UriQueryExpressionParser.ParseError(Strings.UriQueryExpressionParser_CloseParenOrOperatorExpected(this.lexer.CurrentToken.Position, this.lexer.ExpressionText));
			}
			this.lexer.NextToken();
			return result;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00013257 File Offset: 0x00011457
		private QueryToken ParseAny(QueryToken parent)
		{
			return this.ParseAnyAll(parent, true);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00013261 File Offset: 0x00011461
		private QueryToken ParseAll(QueryToken parent)
		{
			return this.ParseAnyAll(parent, false);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001326C File Offset: 0x0001146C
		private QueryToken ParseAnyAll(QueryToken parent, bool isAny)
		{
			this.lexer.NextToken();
			if (this.lexer.CurrentToken.Kind != ExpressionTokenKind.OpenParen)
			{
				throw UriQueryExpressionParser.ParseError(Strings.UriQueryExpressionParser_OpenParenExpected(this.lexer.CurrentToken.Position, this.lexer.ExpressionText));
			}
			this.lexer.NextToken();
			if (this.lexer.CurrentToken.Kind == ExpressionTokenKind.CloseParen)
			{
				this.lexer.NextToken();
				if (isAny)
				{
					return new AnyToken(new LiteralToken(true, "True"), null, parent);
				}
				return new AllToken(new LiteralToken(true, "True"), null, parent);
			}
			else
			{
				string identifier = this.lexer.CurrentToken.GetIdentifier();
				if (!this.parameters.Add(identifier))
				{
					throw UriQueryExpressionParser.ParseError(Strings.UriQueryExpressionParser_RangeVariableAlreadyDeclared(identifier));
				}
				this.lexer.NextToken();
				this.lexer.ValidateToken(ExpressionTokenKind.Colon);
				this.lexer.NextToken();
				QueryToken expression = this.ParseExpression();
				if (this.lexer.CurrentToken.Kind != ExpressionTokenKind.CloseParen)
				{
					throw UriQueryExpressionParser.ParseError(Strings.UriQueryExpressionParser_CloseParenOrCommaExpected(this.lexer.CurrentToken.Position, this.lexer.ExpressionText));
				}
				this.parameters.Remove(identifier);
				this.lexer.NextToken();
				if (isAny)
				{
					return new AnyToken(expression, identifier, parent);
				}
				return new AllToken(expression, identifier, parent);
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000133EC File Offset: 0x000115EC
		private QueryToken ParseSegment(QueryToken parent)
		{
			string identifier = this.lexer.CurrentToken.GetIdentifier();
			this.lexer.NextToken();
			if (this.parameters.Contains(identifier) && parent == null)
			{
				return new RangeVariableToken(identifier);
			}
			return new InnerPathToken(identifier, parent, null);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001343C File Offset: 0x0001163C
		private bool TokenIdentifierIs(string id)
		{
			return this.lexer.CurrentToken.IdentifierIs(id);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001345D File Offset: 0x0001165D
		private void RecurseEnter()
		{
			this.recursionDepth++;
			if (this.recursionDepth > this.maxDepth)
			{
				throw new ODataException(Strings.UriQueryExpressionParser_TooDeep);
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00013486 File Offset: 0x00011686
		private void RecurseLeave()
		{
			this.recursionDepth--;
		}

		// Token: 0x04000251 RID: 593
		private readonly int maxDepth;

		// Token: 0x04000252 RID: 594
		private readonly HashSet<string> parameters = new HashSet<string>(StringComparer.Ordinal)
		{
			"$it"
		};

		// Token: 0x04000253 RID: 595
		private int recursionDepth;

		// Token: 0x04000254 RID: 596
		private ExpressionLexer lexer;

		// Token: 0x020000DF RID: 223
		// (Invoke) Token: 0x06000574 RID: 1396
		internal delegate QueryToken Parser();
	}
}
