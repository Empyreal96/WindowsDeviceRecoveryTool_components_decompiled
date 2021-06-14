using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200012F RID: 303
	internal class CsdlAssociation : CsdlNamedElement
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x0000F319 File Offset: 0x0000D519
		public CsdlAssociation(string name, CsdlAssociationEnd end1, CsdlAssociationEnd end2, CsdlReferentialConstraint constraint, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.end1 = end1;
			this.end2 = end2;
			this.constraint = constraint;
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0000F33C File Offset: 0x0000D53C
		public CsdlReferentialConstraint Constraint
		{
			get
			{
				return this.constraint;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0000F344 File Offset: 0x0000D544
		public CsdlAssociationEnd End1
		{
			get
			{
				return this.end1;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0000F34C File Offset: 0x0000D54C
		public CsdlAssociationEnd End2
		{
			get
			{
				return this.end2;
			}
		}

		// Token: 0x0400030F RID: 783
		private readonly CsdlReferentialConstraint constraint;

		// Token: 0x04000310 RID: 784
		private readonly CsdlAssociationEnd end1;

		// Token: 0x04000311 RID: 785
		private readonly CsdlAssociationEnd end2;
	}
}
