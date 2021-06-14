using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000086 RID: 134
	public sealed class CollectionPropertyAccessNode : CollectionNode
	{
		// Token: 0x0600032F RID: 815 RVA: 0x0000B480 File Offset: 0x00009680
		public CollectionPropertyAccessNode(SingleValueNode source, IEdmProperty property)
		{
			ExceptionUtils.CheckArgumentNotNull<SingleValueNode>(source, "source");
			ExceptionUtils.CheckArgumentNotNull<IEdmProperty>(property, "property");
			if (property.PropertyKind != EdmPropertyKind.Structural)
			{
				throw new ArgumentException(Strings.Nodes_PropertyAccessShouldBeNonEntityProperty(property.Name));
			}
			if (!property.Type.IsCollection())
			{
				throw new ArgumentException(Strings.Nodes_PropertyAccessTypeMustBeCollection(property.Name));
			}
			this.source = source;
			this.property = property;
			this.collectionTypeReference = property.Type.AsCollection();
			this.itemType = this.collectionTypeReference.ElementType();
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000B510 File Offset: 0x00009710
		public SingleValueNode Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000B518 File Offset: 0x00009718
		public IEdmProperty Property
		{
			get
			{
				return this.property;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000B520 File Offset: 0x00009720
		public override IEdmTypeReference ItemType
		{
			get
			{
				return this.itemType;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000B528 File Offset: 0x00009728
		public override IEdmCollectionTypeReference CollectionType
		{
			get
			{
				return this.collectionTypeReference;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000B530 File Offset: 0x00009730
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.CollectionPropertyAccess;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000B533 File Offset: 0x00009733
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x040000F2 RID: 242
		private readonly SingleValueNode source;

		// Token: 0x040000F3 RID: 243
		private readonly IEdmProperty property;

		// Token: 0x040000F4 RID: 244
		private readonly IEdmTypeReference itemType;

		// Token: 0x040000F5 RID: 245
		private readonly IEdmCollectionTypeReference collectionTypeReference;
	}
}
