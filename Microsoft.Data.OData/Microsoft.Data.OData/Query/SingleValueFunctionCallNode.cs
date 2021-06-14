using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000B8 RID: 184
	public sealed class SingleValueFunctionCallNode : SingleValueNode
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x0000EADE File Offset: 0x0000CCDE
		public SingleValueFunctionCallNode(string name, IEnumerable<QueryNode> arguments, IEdmTypeReference typeReference) : this(name, null, arguments, typeReference, null)
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000EAEC File Offset: 0x0000CCEC
		public SingleValueFunctionCallNode(string name, IEnumerable<IEdmFunctionImport> functionImports, IEnumerable<QueryNode> arguments, IEdmTypeReference typeReference, QueryNode source)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(name, "name");
			this.name = name;
			this.functionImports = new ReadOnlyCollection<IEdmFunctionImport>((functionImports != null) ? functionImports.ToList<IEdmFunctionImport>() : new List<IEdmFunctionImport>());
			this.arguments = arguments;
			this.typeReference = typeReference;
			this.source = source;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0000EB43 File Offset: 0x0000CD43
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0000EB4B File Offset: 0x0000CD4B
		public IEnumerable<IEdmFunctionImport> FunctionImports
		{
			get
			{
				return this.functionImports;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0000EB53 File Offset: 0x0000CD53
		public IEnumerable<QueryNode> Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0000EB5B File Offset: 0x0000CD5B
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return this.typeReference;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000EB63 File Offset: 0x0000CD63
		public QueryNode Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000EB6B File Offset: 0x0000CD6B
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.SingleValueFunctionCall;
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000EB6E File Offset: 0x0000CD6E
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x04000181 RID: 385
		private readonly string name;

		// Token: 0x04000182 RID: 386
		private readonly ReadOnlyCollection<IEdmFunctionImport> functionImports;

		// Token: 0x04000183 RID: 387
		private readonly IEnumerable<QueryNode> arguments;

		// Token: 0x04000184 RID: 388
		private readonly IEdmTypeReference typeReference;

		// Token: 0x04000185 RID: 389
		private readonly QueryNode source;
	}
}
