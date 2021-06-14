using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000164 RID: 356
	internal class CsdlSemanticsAssociationSet : CsdlSemanticsElement, IEdmAssociationSet, IEdmNamedElement, IEdmElement, IEdmCheckable
	{
		// Token: 0x0600075F RID: 1887 RVA: 0x0001416D File Offset: 0x0001236D
		public CsdlSemanticsAssociationSet(CsdlSemanticsEntityContainer context, CsdlAssociationSet associationSet) : base(associationSet)
		{
			this.context = context;
			this.associationSet = associationSet;
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x000141A5 File Offset: 0x000123A5
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.context.Model;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x000141B2 File Offset: 0x000123B2
		public IEdmEntityContainer Container
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x000141BA File Offset: 0x000123BA
		public override CsdlElement Element
		{
			get
			{
				return this.associationSet;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x000141C2 File Offset: 0x000123C2
		public string Name
		{
			get
			{
				return this.associationSet.Name;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x000141CF File Offset: 0x000123CF
		public IEdmAssociation Association
		{
			get
			{
				return this.elementTypeCache.GetValue(this, CsdlSemanticsAssociationSet.ComputeElementTypeFunc, null);
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x000141E3 File Offset: 0x000123E3
		public IEdmAssociationSetEnd End1
		{
			get
			{
				return this.Ends.Item1;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x000141F0 File Offset: 0x000123F0
		public IEdmAssociationSetEnd End2
		{
			get
			{
				return this.Ends.Item2;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x000141FD File Offset: 0x000123FD
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsAssociationSet.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00014211 File Offset: 0x00012411
		private TupleInternal<CsdlSemanticsAssociationSetEnd, CsdlSemanticsAssociationSetEnd> Ends
		{
			get
			{
				return this.endsCache.GetValue(this, CsdlSemanticsAssociationSet.ComputeEndsFunc, null);
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00014225 File Offset: 0x00012425
		private IEdmAssociation ComputeElementType()
		{
			return this.context.Context.FindAssociation(this.associationSet.Association) ?? new UnresolvedAssociation(this.associationSet.Association, base.Location);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00014284 File Offset: 0x00012484
		private IEdmAssociationEnd GetRole(CsdlAssociationSetEnd end)
		{
			Func<IEdmAssociationEnd, bool> func = (IEdmAssociationEnd endCandidate) => endCandidate != null && endCandidate.Name == end.Role;
			if (func(this.Association.End1))
			{
				return this.Association.End1;
			}
			if (func(this.Association.End2))
			{
				return this.Association.End2;
			}
			return new UnresolvedAssociationEnd(this.Association, end.Role, end.Location);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001430C File Offset: 0x0001250C
		private TupleInternal<CsdlSemanticsAssociationSetEnd, CsdlSemanticsAssociationSetEnd> ComputeEnds()
		{
			CsdlAssociationSetEnd end = this.associationSet.End1;
			CsdlAssociationSetEnd end2 = this.associationSet.End2;
			IEdmAssociationEnd edmAssociationEnd = null;
			IEdmAssociationEnd edmAssociationEnd2 = null;
			bool flag = false;
			bool flag2 = false;
			if (end != null)
			{
				edmAssociationEnd = this.GetRole(end);
				flag = (edmAssociationEnd is IUnresolvedElement);
			}
			if (end2 != null)
			{
				edmAssociationEnd2 = this.GetRole(end2);
				flag2 = (edmAssociationEnd2 is IUnresolvedElement);
			}
			if (end == null)
			{
				if (flag2)
				{
					edmAssociationEnd = new UnresolvedAssociationEnd(this.Association, "End1", base.Location);
					flag = true;
				}
				else if (edmAssociationEnd2 != null)
				{
					edmAssociationEnd = ((edmAssociationEnd2 != this.Association.End1) ? this.Association.End1 : this.Association.End2);
				}
				else
				{
					edmAssociationEnd = this.Association.End1;
				}
			}
			if (end2 == null)
			{
				if (flag)
				{
					edmAssociationEnd2 = new UnresolvedAssociationEnd(this.Association, "End2", base.Location);
				}
				else
				{
					edmAssociationEnd2 = ((edmAssociationEnd != this.Association.End1) ? this.Association.End1 : this.Association.End2);
				}
			}
			return TupleInternal.Create<CsdlSemanticsAssociationSetEnd, CsdlSemanticsAssociationSetEnd>(new CsdlSemanticsAssociationSetEnd(this, this.associationSet.End1, edmAssociationEnd), new CsdlSemanticsAssociationSetEnd(this, this.associationSet.End2, edmAssociationEnd2));
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00014438 File Offset: 0x00012638
		private IEnumerable<EdmError> ComputeErrors()
		{
			List<EdmError> list = null;
			if (this.Association is UnresolvedAssociation)
			{
				list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, this.Association.Errors());
			}
			if (this.End1.Role != null && this.End2.Role != null && this.End1.Role.Name == this.End2.Role.Name)
			{
				list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, new EdmError(this.End2.Location(), EdmErrorCode.InvalidName, Strings.EdmModel_Validator_Semantic_DuplicateEndName(this.End1.Role.Name)));
			}
			return list ?? Enumerable.Empty<EdmError>();
		}

		// Token: 0x040003B3 RID: 947
		private readonly CsdlSemanticsEntityContainer context;

		// Token: 0x040003B4 RID: 948
		private readonly CsdlAssociationSet associationSet;

		// Token: 0x040003B5 RID: 949
		private readonly Cache<CsdlSemanticsAssociationSet, TupleInternal<CsdlSemanticsAssociationSetEnd, CsdlSemanticsAssociationSetEnd>> endsCache = new Cache<CsdlSemanticsAssociationSet, TupleInternal<CsdlSemanticsAssociationSetEnd, CsdlSemanticsAssociationSetEnd>>();

		// Token: 0x040003B6 RID: 950
		private static readonly Func<CsdlSemanticsAssociationSet, TupleInternal<CsdlSemanticsAssociationSetEnd, CsdlSemanticsAssociationSetEnd>> ComputeEndsFunc = (CsdlSemanticsAssociationSet me) => me.ComputeEnds();

		// Token: 0x040003B7 RID: 951
		private readonly Cache<CsdlSemanticsAssociationSet, IEdmAssociation> elementTypeCache = new Cache<CsdlSemanticsAssociationSet, IEdmAssociation>();

		// Token: 0x040003B8 RID: 952
		private static readonly Func<CsdlSemanticsAssociationSet, IEdmAssociation> ComputeElementTypeFunc = (CsdlSemanticsAssociationSet me) => me.ComputeElementType();

		// Token: 0x040003B9 RID: 953
		private readonly Cache<CsdlSemanticsAssociationSet, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsAssociationSet, IEnumerable<EdmError>>();

		// Token: 0x040003BA RID: 954
		private static readonly Func<CsdlSemanticsAssociationSet, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsAssociationSet me) => me.ComputeErrors();
	}
}
