using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x020000C0 RID: 192
	public sealed class SingleValuePropertyAccessNode : SingleValueNode
	{
		// Token: 0x060004A4 RID: 1188 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		public SingleValuePropertyAccessNode(SingleValueNode source, IEdmProperty property)
		{
			ExceptionUtils.CheckArgumentNotNull<SingleValueNode>(source, "source");
			ExceptionUtils.CheckArgumentNotNull<IEdmProperty>(property, "property");
			if (property.PropertyKind != EdmPropertyKind.Structural)
			{
				throw new ArgumentException(Strings.Nodes_PropertyAccessShouldBeNonEntityProperty(property.Name));
			}
			if (property.Type.IsCollection())
			{
				throw new ArgumentException(Strings.Nodes_PropertyAccessTypeShouldNotBeCollection(property.Name));
			}
			this.source = source;
			this.property = property;
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0000FE12 File Offset: 0x0000E012
		public SingleValueNode Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0000FE1A File Offset: 0x0000E01A
		public IEdmProperty Property
		{
			get
			{
				return this.property;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0000FE22 File Offset: 0x0000E022
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return this.Property.Type;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0000FE2F File Offset: 0x0000E02F
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.SingleValuePropertyAccess;
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000FE32 File Offset: 0x0000E032
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x04000194 RID: 404
		private readonly SingleValueNode source;

		// Token: 0x04000195 RID: 405
		private readonly IEdmProperty property;
	}
}
