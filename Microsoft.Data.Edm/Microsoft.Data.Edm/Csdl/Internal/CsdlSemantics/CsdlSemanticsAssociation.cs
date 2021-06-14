using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000162 RID: 354
	internal class CsdlSemanticsAssociation : CsdlSemanticsElement, IEdmAssociation, IEdmNamedElement, IEdmElement, IEdmCheckable
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x00013CFF File Offset: 0x00011EFF
		public CsdlSemanticsAssociation(CsdlSemanticsSchema context, CsdlAssociation association) : base(association)
		{
			this.association = association;
			this.context = context;
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00013D37 File Offset: 0x00011F37
		public string Namespace
		{
			get
			{
				return this.context.Namespace;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00013D44 File Offset: 0x00011F44
		public string Name
		{
			get
			{
				return this.association.Name;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x00013D51 File Offset: 0x00011F51
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.context.Model;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00013D5E File Offset: 0x00011F5E
		public override CsdlElement Element
		{
			get
			{
				return this.association;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x00013D66 File Offset: 0x00011F66
		public IEdmAssociationEnd End1
		{
			get
			{
				return this.endsCache.GetValue(this, CsdlSemanticsAssociation.ComputeEndsFunc, null).Item1;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00013D7F File Offset: 0x00011F7F
		public IEdmAssociationEnd End2
		{
			get
			{
				return this.endsCache.GetValue(this, CsdlSemanticsAssociation.ComputeEndsFunc, null).Item2;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x00013D98 File Offset: 0x00011F98
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsAssociation.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00013DAC File Offset: 0x00011FAC
		public CsdlSemanticsReferentialConstraint ReferentialConstraint
		{
			get
			{
				return this.referentialConstraintCache.GetValue(this, CsdlSemanticsAssociation.ComputeReferentialConstraintFunc, null);
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00013DC0 File Offset: 0x00011FC0
		private TupleInternal<IEdmAssociationEnd, IEdmAssociationEnd> ComputeEnds()
		{
			IEdmAssociationEnd item;
			if (this.association.End1 == null)
			{
				IEdmAssociationEnd edmAssociationEnd = new BadAssociationEnd(this, "End1", new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidAssociation, Strings.CsdlParser_InvalidAssociationIncorrectNumberOfEnds(this.Namespace + "." + this.Name))
				});
				item = edmAssociationEnd;
			}
			else
			{
				item = new CsdlSemanticsAssociationEnd(this.context, this, this.association.End1);
			}
			IEdmAssociationEnd item2;
			if (this.association.End2 == null)
			{
				IEdmAssociationEnd edmAssociationEnd2 = new BadAssociationEnd(this, "End2", new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.InvalidAssociation, Strings.CsdlParser_InvalidAssociationIncorrectNumberOfEnds(this.Namespace + "." + this.Name))
				});
				item2 = edmAssociationEnd2;
			}
			else
			{
				item2 = new CsdlSemanticsAssociationEnd(this.context, this, this.association.End2);
			}
			return TupleInternal.Create<IEdmAssociationEnd, IEdmAssociationEnd>(item, item2);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00013EA0 File Offset: 0x000120A0
		private IEnumerable<EdmError> ComputeErrors()
		{
			List<EdmError> list = null;
			if (this.association.End1.Name == this.association.End2.Name)
			{
				list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, new EdmError(this.association.End2.Location ?? base.Location, EdmErrorCode.AlreadyDefined, Strings.EdmModel_Validator_Semantic_EndNameAlreadyDefinedDuplicate(this.association.End1.Name)));
			}
			return list ?? Enumerable.Empty<EdmError>();
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00013F1D File Offset: 0x0001211D
		private CsdlSemanticsReferentialConstraint ComputeReferentialConstraint()
		{
			if (this.association.Constraint == null)
			{
				return null;
			}
			return new CsdlSemanticsReferentialConstraint(this, this.association.Constraint);
		}

		// Token: 0x0400039F RID: 927
		private readonly CsdlAssociation association;

		// Token: 0x040003A0 RID: 928
		private readonly CsdlSemanticsSchema context;

		// Token: 0x040003A1 RID: 929
		private readonly Cache<CsdlSemanticsAssociation, CsdlSemanticsReferentialConstraint> referentialConstraintCache = new Cache<CsdlSemanticsAssociation, CsdlSemanticsReferentialConstraint>();

		// Token: 0x040003A2 RID: 930
		private static readonly Func<CsdlSemanticsAssociation, CsdlSemanticsReferentialConstraint> ComputeReferentialConstraintFunc = (CsdlSemanticsAssociation me) => me.ComputeReferentialConstraint();

		// Token: 0x040003A3 RID: 931
		private readonly Cache<CsdlSemanticsAssociation, TupleInternal<IEdmAssociationEnd, IEdmAssociationEnd>> endsCache = new Cache<CsdlSemanticsAssociation, TupleInternal<IEdmAssociationEnd, IEdmAssociationEnd>>();

		// Token: 0x040003A4 RID: 932
		private static readonly Func<CsdlSemanticsAssociation, TupleInternal<IEdmAssociationEnd, IEdmAssociationEnd>> ComputeEndsFunc = (CsdlSemanticsAssociation me) => me.ComputeEnds();

		// Token: 0x040003A5 RID: 933
		private readonly Cache<CsdlSemanticsAssociation, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsAssociation, IEnumerable<EdmError>>();

		// Token: 0x040003A6 RID: 934
		private static readonly Func<CsdlSemanticsAssociation, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsAssociation me) => me.ComputeErrors();
	}
}
