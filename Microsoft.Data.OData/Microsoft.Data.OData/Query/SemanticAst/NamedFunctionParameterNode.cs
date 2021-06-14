using System;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000018 RID: 24
	public class NamedFunctionParameterNode : QueryNode
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00003701 File Offset: 0x00001901
		public NamedFunctionParameterNode(string name, QueryNode value)
		{
			this.name = name;
			this.value = value;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003717 File Offset: 0x00001917
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000371F File Offset: 0x0000191F
		public QueryNode Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003727 File Offset: 0x00001927
		public override QueryNodeKind Kind
		{
			get
			{
				return (QueryNodeKind)this.InternalKind;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000372F File Offset: 0x0000192F
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.NamedFunctionParameter;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003733 File Offset: 0x00001933
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x04000036 RID: 54
		private readonly string name;

		// Token: 0x04000037 RID: 55
		private readonly QueryNode value;
	}
}
