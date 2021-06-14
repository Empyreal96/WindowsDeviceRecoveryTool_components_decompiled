using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x020001EA RID: 490
	public sealed class SingleValueOpenPropertyAccessNode : SingleValueNode
	{
		// Token: 0x06000F18 RID: 3864 RVA: 0x00036076 File Offset: 0x00034276
		public SingleValueOpenPropertyAccessNode(SingleValueNode source, string openPropertyName)
		{
			ExceptionUtils.CheckArgumentNotNull<SingleValueNode>(source, "source");
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(openPropertyName, "openPropertyName");
			this.name = openPropertyName;
			this.source = source;
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x000360A2 File Offset: 0x000342A2
		public SingleValueNode Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x000360AA File Offset: 0x000342AA
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x000360B2 File Offset: 0x000342B2
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x000360B5 File Offset: 0x000342B5
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.SingleValueOpenPropertyAccess;
			}
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x000360B9 File Offset: 0x000342B9
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x04000532 RID: 1330
		private readonly SingleValueNode source;

		// Token: 0x04000533 RID: 1331
		private readonly string name;
	}
}
