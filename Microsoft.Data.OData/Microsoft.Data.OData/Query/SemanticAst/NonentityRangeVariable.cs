using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000081 RID: 129
	public sealed class NonentityRangeVariable : RangeVariable
	{
		// Token: 0x0600030B RID: 779 RVA: 0x0000AFCC File Offset: 0x000091CC
		public NonentityRangeVariable(string name, IEdmTypeReference typeReference, CollectionNode collectionNode)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(name, "name");
			this.name = name;
			if (typeReference != null && typeReference.Definition.TypeKind == EdmTypeKind.Entity)
			{
				throw new ArgumentException(Strings.Nodes_NonentityParameterQueryNodeWithEntityType(typeReference.FullName()));
			}
			this.typeReference = typeReference;
			this.collectionNode = collectionNode;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000B021 File Offset: 0x00009221
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000B029 File Offset: 0x00009229
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return this.typeReference;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000B031 File Offset: 0x00009231
		public CollectionNode CollectionNode
		{
			get
			{
				return this.collectionNode;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000B039 File Offset: 0x00009239
		public override int Kind
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x040000DE RID: 222
		private readonly string name;

		// Token: 0x040000DF RID: 223
		private readonly CollectionNode collectionNode;

		// Token: 0x040000E0 RID: 224
		private readonly IEdmTypeReference typeReference;
	}
}
