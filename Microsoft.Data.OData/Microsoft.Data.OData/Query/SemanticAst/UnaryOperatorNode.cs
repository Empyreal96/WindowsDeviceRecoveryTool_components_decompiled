using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x020000C3 RID: 195
	public sealed class UnaryOperatorNode : SingleValueNode
	{
		// Token: 0x060004B7 RID: 1207 RVA: 0x0000FEFE File Offset: 0x0000E0FE
		public UnaryOperatorNode(UnaryOperatorKind operatorKind, SingleValueNode operand)
		{
			ExceptionUtils.CheckArgumentNotNull<SingleValueNode>(operand, "operand");
			this.operand = operand;
			this.operatorKind = operatorKind;
			if (operand == null || operand.TypeReference == null)
			{
				this.typeReference = null;
				return;
			}
			this.typeReference = operand.TypeReference;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0000FF3E File Offset: 0x0000E13E
		public UnaryOperatorKind OperatorKind
		{
			get
			{
				return this.operatorKind;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0000FF46 File Offset: 0x0000E146
		public SingleValueNode Operand
		{
			get
			{
				return this.operand;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0000FF4E File Offset: 0x0000E14E
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return this.typeReference;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0000FF56 File Offset: 0x0000E156
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.UnaryOperator;
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000FF59 File Offset: 0x0000E159
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x0400019B RID: 411
		private readonly SingleValueNode operand;

		// Token: 0x0400019C RID: 412
		private readonly UnaryOperatorKind operatorKind;

		// Token: 0x0400019D RID: 413
		private IEdmTypeReference typeReference;
	}
}
