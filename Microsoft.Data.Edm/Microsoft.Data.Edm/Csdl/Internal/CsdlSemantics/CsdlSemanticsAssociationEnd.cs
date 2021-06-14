using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000163 RID: 355
	internal class CsdlSemanticsAssociationEnd : CsdlSemanticsElement, IEdmAssociationEnd, IEdmNamedElement, IEdmElement, IEdmCheckable
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x00013FCB File Offset: 0x000121CB
		public CsdlSemanticsAssociationEnd(CsdlSemanticsSchema context, CsdlSemanticsAssociation association, CsdlAssociationEnd end) : base(end)
		{
			this.end = end;
			this.definingAssociation = association;
			this.context = context;
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00013FFF File Offset: 0x000121FF
		public EdmMultiplicity Multiplicity
		{
			get
			{
				return this.end.Multiplicity;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0001400C File Offset: 0x0001220C
		public EdmOnDeleteAction OnDelete
		{
			get
			{
				if (this.end.OnDelete == null)
				{
					return EdmOnDeleteAction.None;
				}
				return this.end.OnDelete.Action;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0001402D File Offset: 0x0001222D
		public IEdmAssociation DeclaringAssociation
		{
			get
			{
				return this.definingAssociation;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00014035 File Offset: 0x00012235
		public IEdmEntityType EntityType
		{
			get
			{
				return this.typeCache.GetValue(this, CsdlSemanticsAssociationEnd.ComputeTypeFunc, null);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00014049 File Offset: 0x00012249
		public string Name
		{
			get
			{
				return this.end.Name ?? string.Empty;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0001405F File Offset: 0x0001225F
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.context.Model;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0001406C File Offset: 0x0001226C
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsAssociationEnd.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x00014080 File Offset: 0x00012280
		public override CsdlElement Element
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00014088 File Offset: 0x00012288
		private IEdmEntityType ComputeType()
		{
			IEdmTypeReference type = CsdlSemanticsModel.WrapTypeReference(this.context, this.end.Type);
			if (type.TypeKind() != EdmTypeKind.Entity)
			{
				return new UnresolvedEntityType(type.FullName(), base.Location);
			}
			return type.AsEntity().EntityDefinition();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000140D4 File Offset: 0x000122D4
		private IEnumerable<EdmError> ComputeErrors()
		{
			List<EdmError> list = null;
			if (this.EntityType is UnresolvedEntityType)
			{
				list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, this.EntityType.Errors());
			}
			return list ?? Enumerable.Empty<EdmError>();
		}

		// Token: 0x040003AA RID: 938
		private readonly CsdlAssociationEnd end;

		// Token: 0x040003AB RID: 939
		private readonly CsdlSemanticsAssociation definingAssociation;

		// Token: 0x040003AC RID: 940
		private readonly CsdlSemanticsSchema context;

		// Token: 0x040003AD RID: 941
		private readonly Cache<CsdlSemanticsAssociationEnd, IEdmEntityType> typeCache = new Cache<CsdlSemanticsAssociationEnd, IEdmEntityType>();

		// Token: 0x040003AE RID: 942
		private static readonly Func<CsdlSemanticsAssociationEnd, IEdmEntityType> ComputeTypeFunc = (CsdlSemanticsAssociationEnd me) => me.ComputeType();

		// Token: 0x040003AF RID: 943
		private readonly Cache<CsdlSemanticsAssociationEnd, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsAssociationEnd, IEnumerable<EdmError>>();

		// Token: 0x040003B0 RID: 944
		private static readonly Func<CsdlSemanticsAssociationEnd, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsAssociationEnd me) => me.ComputeErrors();
	}
}
