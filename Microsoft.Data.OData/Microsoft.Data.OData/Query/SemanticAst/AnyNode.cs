using System;
using System.Collections.ObjectModel;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200009E RID: 158
	public sealed class AnyNode : LambdaNode
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x0000BD91 File Offset: 0x00009F91
		public AnyNode(Collection<RangeVariable> parameters) : this(parameters, null)
		{
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000BD9B File Offset: 0x00009F9B
		public AnyNode(Collection<RangeVariable> parameters, RangeVariable currentRangeVariable) : base(parameters, currentRangeVariable)
		{
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000BDA5 File Offset: 0x00009FA5
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return EdmCoreModel.Instance.GetBoolean(true);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000BDB2 File Offset: 0x00009FB2
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.Any;
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000BDB6 File Offset: 0x00009FB6
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}
	}
}
