using System;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000097 RID: 151
	internal abstract class SyntacticTreeVisitor<T> : ISyntacticTreeVisitor<T>
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0000BB72 File Offset: 0x00009D72
		public virtual T Visit(AllToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000BB79 File Offset: 0x00009D79
		public virtual T Visit(AnyToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000BB80 File Offset: 0x00009D80
		public virtual T Visit(BinaryOperatorToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000BB87 File Offset: 0x00009D87
		public virtual T Visit(DottedIdentifierToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000BB8E File Offset: 0x00009D8E
		public virtual T Visit(ExpandToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000BB95 File Offset: 0x00009D95
		public virtual T Visit(ExpandTermToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000BB9C File Offset: 0x00009D9C
		public virtual T Visit(FunctionCallToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000BBA3 File Offset: 0x00009DA3
		public virtual T Visit(LiteralToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000BBAA File Offset: 0x00009DAA
		public virtual T Visit(LambdaToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000BBB1 File Offset: 0x00009DB1
		public virtual T Visit(InnerPathToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		public virtual T Visit(OrderByToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000BBBF File Offset: 0x00009DBF
		public virtual T Visit(EndPathToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000BBC6 File Offset: 0x00009DC6
		public virtual T Visit(CustomQueryOptionToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000BBCD File Offset: 0x00009DCD
		public virtual T Visit(RangeVariableToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000BBD4 File Offset: 0x00009DD4
		public virtual T Visit(SelectToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000BBDB File Offset: 0x00009DDB
		public virtual T Visit(StarToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000BBE2 File Offset: 0x00009DE2
		public virtual T Visit(UnaryOperatorToken tokenIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000BBE9 File Offset: 0x00009DE9
		public virtual T Visit(FunctionParameterToken tokenIn)
		{
			throw new NotImplementedException();
		}
	}
}
