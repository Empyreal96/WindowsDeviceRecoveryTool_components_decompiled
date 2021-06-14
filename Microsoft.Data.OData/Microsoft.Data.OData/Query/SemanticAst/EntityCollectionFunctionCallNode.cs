using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000046 RID: 70
	public sealed class EntityCollectionFunctionCallNode : EntityCollectionNode
	{
		// Token: 0x060001BB RID: 443 RVA: 0x000076C0 File Offset: 0x000058C0
		public EntityCollectionFunctionCallNode(string name, IEnumerable<IEdmFunctionImport> functionImports, IEnumerable<QueryNode> parameters, IEdmCollectionTypeReference returnedCollectionTypeReference, IEdmEntitySet entitySet, QueryNode source)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(name, "name");
			ExceptionUtils.CheckArgumentNotNull<IEdmCollectionTypeReference>(returnedCollectionTypeReference, "returnedCollectionTypeReference");
			this.name = name;
			this.functionImports = new ReadOnlyCollection<IEdmFunctionImport>((functionImports == null) ? new List<IEdmFunctionImport>() : functionImports.ToList<IEdmFunctionImport>());
			this.parameters = new ReadOnlyCollection<QueryNode>((parameters == null) ? new List<QueryNode>() : parameters.ToList<QueryNode>());
			this.returnedCollectionTypeReference = returnedCollectionTypeReference;
			this.entitySet = entitySet;
			this.entityTypeReference = returnedCollectionTypeReference.ElementType().AsEntityOrNull();
			if (this.entityTypeReference == null)
			{
				throw new ArgumentException(Strings.Nodes_EntityCollectionFunctionCallNode_ItemTypeMustBeAnEntity);
			}
			this.source = source;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00007764 File Offset: 0x00005964
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000776C File Offset: 0x0000596C
		public IEnumerable<IEdmFunctionImport> FunctionImports
		{
			get
			{
				return this.functionImports;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00007774 File Offset: 0x00005974
		public IEnumerable<QueryNode> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000777C File Offset: 0x0000597C
		public override IEdmTypeReference ItemType
		{
			get
			{
				return this.entityTypeReference;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00007784 File Offset: 0x00005984
		public override IEdmCollectionTypeReference CollectionType
		{
			get
			{
				return this.returnedCollectionTypeReference;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000778C File Offset: 0x0000598C
		public override IEdmEntityTypeReference EntityItemType
		{
			get
			{
				return this.entityTypeReference;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00007794 File Offset: 0x00005994
		public override IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000779C File Offset: 0x0000599C
		public QueryNode Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000077A4 File Offset: 0x000059A4
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.EntityCollectionFunctionCall;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000077A8 File Offset: 0x000059A8
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x04000074 RID: 116
		private readonly string name;

		// Token: 0x04000075 RID: 117
		private readonly ReadOnlyCollection<IEdmFunctionImport> functionImports;

		// Token: 0x04000076 RID: 118
		private readonly ReadOnlyCollection<QueryNode> parameters;

		// Token: 0x04000077 RID: 119
		private readonly IEdmEntityTypeReference entityTypeReference;

		// Token: 0x04000078 RID: 120
		private readonly IEdmCollectionTypeReference returnedCollectionTypeReference;

		// Token: 0x04000079 RID: 121
		private readonly IEdmEntitySet entitySet;

		// Token: 0x0400007A RID: 122
		private readonly QueryNode source;
	}
}
