using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000031 RID: 49
	internal interface ISyntacticTreeVisitor<T>
	{
		// Token: 0x0600013B RID: 315
		T Visit(AllToken tokenIn);

		// Token: 0x0600013C RID: 316
		T Visit(AnyToken tokenIn);

		// Token: 0x0600013D RID: 317
		T Visit(BinaryOperatorToken tokenIn);

		// Token: 0x0600013E RID: 318
		T Visit(DottedIdentifierToken tokenIn);

		// Token: 0x0600013F RID: 319
		T Visit(ExpandToken tokenIn);

		// Token: 0x06000140 RID: 320
		T Visit(ExpandTermToken tokenIn);

		// Token: 0x06000141 RID: 321
		T Visit(FunctionCallToken tokenIn);

		// Token: 0x06000142 RID: 322
		T Visit(LambdaToken tokenIn);

		// Token: 0x06000143 RID: 323
		T Visit(LiteralToken tokenIn);

		// Token: 0x06000144 RID: 324
		T Visit(InnerPathToken tokenIn);

		// Token: 0x06000145 RID: 325
		T Visit(OrderByToken tokenIn);

		// Token: 0x06000146 RID: 326
		T Visit(EndPathToken tokenIn);

		// Token: 0x06000147 RID: 327
		T Visit(CustomQueryOptionToken tokenIn);

		// Token: 0x06000148 RID: 328
		T Visit(RangeVariableToken tokenIn);

		// Token: 0x06000149 RID: 329
		T Visit(SelectToken tokenIn);

		// Token: 0x0600014A RID: 330
		T Visit(StarToken tokenIn);

		// Token: 0x0600014B RID: 331
		T Visit(UnaryOperatorToken tokenIn);

		// Token: 0x0600014C RID: 332
		T Visit(FunctionParameterToken tokenIn);
	}
}
