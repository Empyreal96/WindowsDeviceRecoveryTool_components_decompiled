using System;
using System.Collections.ObjectModel;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200009B RID: 155
	public sealed class AllNode : LambdaNode
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x0000BD02 File Offset: 0x00009F02
		public AllNode(Collection<RangeVariable> rangeVariables) : this(rangeVariables, null)
		{
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000BD0C File Offset: 0x00009F0C
		public AllNode(Collection<RangeVariable> rangeVariables, RangeVariable currentRangeVariable) : base(rangeVariables, currentRangeVariable)
		{
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0000BD16 File Offset: 0x00009F16
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return EdmCoreModel.Instance.GetBoolean(true);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000BD23 File Offset: 0x00009F23
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.All;
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000BD27 File Offset: 0x00009F27
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}
	}
}
