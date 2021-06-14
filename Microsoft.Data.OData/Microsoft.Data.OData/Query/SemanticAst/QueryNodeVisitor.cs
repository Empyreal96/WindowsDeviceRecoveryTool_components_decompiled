using System;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000090 RID: 144
	public abstract class QueryNodeVisitor<T>
	{
		// Token: 0x06000351 RID: 849 RVA: 0x0000B858 File Offset: 0x00009A58
		public virtual T Visit(AllNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000B85F File Offset: 0x00009A5F
		public virtual T Visit(AnyNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000B866 File Offset: 0x00009A66
		public virtual T Visit(BinaryOperatorNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000B86D File Offset: 0x00009A6D
		public virtual T Visit(CollectionNavigationNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000B874 File Offset: 0x00009A74
		public virtual T Visit(CollectionPropertyAccessNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000B87B File Offset: 0x00009A7B
		public virtual T Visit(ConstantNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000B882 File Offset: 0x00009A82
		public virtual T Visit(ConvertNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000B889 File Offset: 0x00009A89
		public virtual T Visit(EntityCollectionCastNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000B890 File Offset: 0x00009A90
		public virtual T Visit(EntityRangeVariableReferenceNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000B897 File Offset: 0x00009A97
		public virtual T Visit(NonentityRangeVariableReferenceNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000B89E File Offset: 0x00009A9E
		public virtual T Visit(SingleEntityCastNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000B8A5 File Offset: 0x00009AA5
		public virtual T Visit(SingleNavigationNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000B8AC File Offset: 0x00009AAC
		public virtual T Visit(SingleEntityFunctionCallNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000B8B3 File Offset: 0x00009AB3
		public virtual T Visit(SingleValueFunctionCallNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000B8BA File Offset: 0x00009ABA
		public virtual T Visit(EntityCollectionFunctionCallNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000B8C1 File Offset: 0x00009AC1
		public virtual T Visit(CollectionFunctionCallNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000B8C8 File Offset: 0x00009AC8
		public virtual T Visit(SingleValueOpenPropertyAccessNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000B8CF File Offset: 0x00009ACF
		public virtual T Visit(SingleValuePropertyAccessNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000B8D6 File Offset: 0x00009AD6
		public virtual T Visit(UnaryOperatorNode nodeIn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000B8DD File Offset: 0x00009ADD
		public virtual T Visit(NamedFunctionParameterNode nodeIn)
		{
			throw new NotImplementedException();
		}
	}
}
