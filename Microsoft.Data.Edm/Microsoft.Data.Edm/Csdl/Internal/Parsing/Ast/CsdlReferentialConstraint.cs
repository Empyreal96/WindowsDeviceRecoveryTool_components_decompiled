using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000148 RID: 328
	internal class CsdlReferentialConstraint : CsdlElementWithDocumentation
	{
		// Token: 0x0600061F RID: 1567 RVA: 0x0000F7AE File Offset: 0x0000D9AE
		public CsdlReferentialConstraint(CsdlReferentialConstraintRole principal, CsdlReferentialConstraintRole dependent, CsdlDocumentation documentation, CsdlLocation location) : base(documentation, location)
		{
			this.principal = principal;
			this.dependent = dependent;
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0000F7C7 File Offset: 0x0000D9C7
		public CsdlReferentialConstraintRole Principal
		{
			get
			{
				return this.principal;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0000F7CF File Offset: 0x0000D9CF
		public CsdlReferentialConstraintRole Dependent
		{
			get
			{
				return this.dependent;
			}
		}

		// Token: 0x04000344 RID: 836
		private readonly CsdlReferentialConstraintRole principal;

		// Token: 0x04000345 RID: 837
		private readonly CsdlReferentialConstraintRole dependent;
	}
}
