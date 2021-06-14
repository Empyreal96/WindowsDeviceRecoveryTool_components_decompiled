using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200016D RID: 365
	internal class CsdlSemanticsEntityReferenceTypeDefinition : CsdlSemanticsTypeDefinition, IEdmEntityReferenceType, IEdmType, IEdmElement
	{
		// Token: 0x060007CF RID: 1999 RVA: 0x000154AD File Offset: 0x000136AD
		public CsdlSemanticsEntityReferenceTypeDefinition(CsdlSemanticsSchema schema, CsdlEntityReferenceType entityTypeReference) : base(entityTypeReference)
		{
			this.schema = schema;
			this.entityTypeReference = entityTypeReference;
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x000154CF File Offset: 0x000136CF
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.EntityReference;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x000154D2 File Offset: 0x000136D2
		public IEdmEntityType EntityType
		{
			get
			{
				return this.entityTypeCache.GetValue(this, CsdlSemanticsEntityReferenceTypeDefinition.ComputeEntityTypeFunc, null);
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x000154E6 File Offset: 0x000136E6
		public override CsdlElement Element
		{
			get
			{
				return this.entityTypeReference;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x000154EE File Offset: 0x000136EE
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.schema.Model;
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000154FC File Offset: 0x000136FC
		private IEdmEntityType ComputeEntityType()
		{
			IEdmTypeReference type = CsdlSemanticsModel.WrapTypeReference(this.schema, this.entityTypeReference.EntityType);
			if (type.TypeKind() != EdmTypeKind.Entity)
			{
				return new UnresolvedEntityType(this.schema.UnresolvedName(type.FullName()), base.Location);
			}
			return type.AsEntity().EntityDefinition();
		}

		// Token: 0x040003EF RID: 1007
		private readonly CsdlSemanticsSchema schema;

		// Token: 0x040003F0 RID: 1008
		private readonly Cache<CsdlSemanticsEntityReferenceTypeDefinition, IEdmEntityType> entityTypeCache = new Cache<CsdlSemanticsEntityReferenceTypeDefinition, IEdmEntityType>();

		// Token: 0x040003F1 RID: 1009
		private static readonly Func<CsdlSemanticsEntityReferenceTypeDefinition, IEdmEntityType> ComputeEntityTypeFunc = (CsdlSemanticsEntityReferenceTypeDefinition me) => me.ComputeEntityType();

		// Token: 0x040003F2 RID: 1010
		private readonly CsdlEntityReferenceType entityTypeReference;
	}
}
