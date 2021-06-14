using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000CE RID: 206
	internal class MetadataBinder
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x000115CB File Offset: 0x0000F7CB
		internal MetadataBinder(BindingState initialState)
		{
			ExceptionUtils.CheckArgumentNotNull<BindingState>(initialState, "initialState");
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(initialState.Model, "initialState.Model");
			this.BindingState = initialState;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x000115F5 File Offset: 0x0000F7F5
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x000115FD File Offset: 0x0000F7FD
		internal BindingState BindingState
		{
			get
			{
				return this.bindingState;
			}
			private set
			{
				this.bindingState = value;
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00011608 File Offset: 0x0000F808
		public static long? ProcessSkip(long? skip)
		{
			if (skip == null)
			{
				return null;
			}
			if (skip < 0L)
			{
				throw new ODataException(Strings.MetadataBinder_SkipRequiresNonNegativeInteger(skip.ToString()));
			}
			return skip;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001165C File Offset: 0x0000F85C
		public static long? ProcessTop(long? top)
		{
			if (top == null)
			{
				return null;
			}
			if (top < 0L)
			{
				throw new ODataException(Strings.MetadataBinder_TopRequiresNonNegativeInteger(top.ToString()));
			}
			return top;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000116B0 File Offset: 0x0000F8B0
		public static List<QueryNode> ProcessQueryOptions(BindingState bindingState, MetadataBinder.QueryTokenVisitor bindMethod)
		{
			List<QueryNode> list = new List<QueryNode>();
			foreach (CustomQueryOptionToken token in bindingState.QueryOptions)
			{
				QueryNode queryNode = bindMethod(token);
				if (queryNode != null)
				{
					list.Add(queryNode);
				}
			}
			bindingState.QueryOptions = null;
			return list;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001171C File Offset: 0x0000F91C
		protected internal QueryNode Bind(QueryToken token)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryToken>(token, "token");
			QueryNode queryNode;
			switch (token.Kind)
			{
			case QueryTokenKind.BinaryOperator:
				queryNode = this.BindBinaryOperator((BinaryOperatorToken)token);
				goto IL_12F;
			case QueryTokenKind.UnaryOperator:
				queryNode = this.BindUnaryOperator((UnaryOperatorToken)token);
				goto IL_12F;
			case QueryTokenKind.Literal:
				queryNode = this.BindLiteral((LiteralToken)token);
				goto IL_12F;
			case QueryTokenKind.FunctionCall:
				queryNode = this.BindFunctionCall((FunctionCallToken)token);
				goto IL_12F;
			case QueryTokenKind.EndPath:
				queryNode = this.BindEndPath((EndPathToken)token);
				goto IL_12F;
			case QueryTokenKind.Any:
				queryNode = this.BindAnyAll((AnyToken)token);
				goto IL_12F;
			case QueryTokenKind.InnerPath:
				queryNode = this.BindInnerPathSegment((InnerPathToken)token);
				goto IL_12F;
			case QueryTokenKind.DottedIdentifier:
				queryNode = this.BindCast((DottedIdentifierToken)token);
				goto IL_12F;
			case QueryTokenKind.RangeVariable:
				queryNode = this.BindRangeVariable((RangeVariableToken)token);
				goto IL_12F;
			case QueryTokenKind.All:
				queryNode = this.BindAnyAll((AllToken)token);
				goto IL_12F;
			case QueryTokenKind.FunctionParameter:
				queryNode = this.BindFunctionParameter((FunctionParameterToken)token);
				goto IL_12F;
			}
			throw new ODataException(Strings.MetadataBinder_UnsupportedQueryTokenKind(token.Kind));
			IL_12F:
			if (queryNode == null)
			{
				throw new ODataException(Strings.MetadataBinder_BoundNodeCannotBeNull(token.Kind));
			}
			return queryNode;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00011872 File Offset: 0x0000FA72
		protected virtual QueryNode BindFunctionParameter(FunctionParameterToken token)
		{
			if (token.ParameterName != null)
			{
				return new NamedFunctionParameterNode(token.ParameterName, this.Bind(token.ValueToken));
			}
			return this.Bind(token.ValueToken);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000118A0 File Offset: 0x0000FAA0
		protected virtual QueryNode BindInnerPathSegment(InnerPathToken token)
		{
			InnerPathTokenBinder innerPathTokenBinder = new InnerPathTokenBinder(new MetadataBinder.QueryTokenVisitor(this.Bind));
			return innerPathTokenBinder.BindInnerPathSegment(token, this.BindingState);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000118CC File Offset: 0x0000FACC
		protected virtual SingleValueNode BindRangeVariable(RangeVariableToken rangeVariableToken)
		{
			return RangeVariableBinder.BindRangeVariableToken(rangeVariableToken, this.BindingState);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000118DA File Offset: 0x0000FADA
		protected virtual QueryNode BindLiteral(LiteralToken literalToken)
		{
			return LiteralBinder.BindLiteral(literalToken);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000118E4 File Offset: 0x0000FAE4
		protected virtual QueryNode BindBinaryOperator(BinaryOperatorToken binaryOperatorToken)
		{
			BinaryOperatorBinder binaryOperatorBinder = new BinaryOperatorBinder(new Func<QueryToken, QueryNode>(this.Bind));
			return binaryOperatorBinder.BindBinaryOperator(binaryOperatorToken);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001190C File Offset: 0x0000FB0C
		protected virtual QueryNode BindUnaryOperator(UnaryOperatorToken unaryOperatorToken)
		{
			UnaryOperatorBinder unaryOperatorBinder = new UnaryOperatorBinder(new Func<QueryToken, QueryNode>(this.Bind));
			return unaryOperatorBinder.BindUnaryOperator(unaryOperatorToken);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00011934 File Offset: 0x0000FB34
		protected virtual QueryNode BindCast(DottedIdentifierToken dottedIdentifierToken)
		{
			DottedIdentifierBinder dottedIdentifierBinder = new DottedIdentifierBinder(new MetadataBinder.QueryTokenVisitor(this.Bind));
			return dottedIdentifierBinder.BindDottedIdentifier(dottedIdentifierToken, this.BindingState);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00011960 File Offset: 0x0000FB60
		protected virtual QueryNode BindAnyAll(LambdaToken lambdaToken)
		{
			ExceptionUtils.CheckArgumentNotNull<LambdaToken>(lambdaToken, "LambdaToken");
			LambdaBinder lambdaBinder = new LambdaBinder(new MetadataBinder.QueryTokenVisitor(this.Bind));
			return lambdaBinder.BindLambdaToken(lambdaToken, this.BindingState);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00011998 File Offset: 0x0000FB98
		protected virtual QueryNode BindEndPath(EndPathToken endPathToken)
		{
			EndPathBinder endPathBinder = new EndPathBinder(new MetadataBinder.QueryTokenVisitor(this.Bind));
			return endPathBinder.BindEndPath(endPathToken, this.BindingState);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000119C4 File Offset: 0x0000FBC4
		protected virtual QueryNode BindFunctionCall(FunctionCallToken functionCallToken)
		{
			FunctionCallBinder functionCallBinder = new FunctionCallBinder(new MetadataBinder.QueryTokenVisitor(this.Bind));
			return functionCallBinder.BindFunctionCall(functionCallToken, this.BindingState);
		}

		// Token: 0x040001DC RID: 476
		private BindingState bindingState;

		// Token: 0x020000CF RID: 207
		// (Invoke) Token: 0x06000517 RID: 1303
		internal delegate QueryNode QueryTokenVisitor(QueryToken token);
	}
}
