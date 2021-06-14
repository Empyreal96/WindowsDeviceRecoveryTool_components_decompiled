using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000132 RID: 306
	internal class CsdlAssociationSetEnd : CsdlElementWithDocumentation
	{
		// Token: 0x060005DC RID: 1500 RVA: 0x0000F3CA File Offset: 0x0000D5CA
		public CsdlAssociationSetEnd(string role, string entitySet, CsdlDocumentation documentation, CsdlLocation location) : base(documentation, location)
		{
			this.role = role;
			this.entitySet = entitySet;
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x0000F3E3 File Offset: 0x0000D5E3
		public string Role
		{
			get
			{
				return this.role;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0000F3EB File Offset: 0x0000D5EB
		public string EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x04000318 RID: 792
		private readonly string role;

		// Token: 0x04000319 RID: 793
		private readonly string entitySet;
	}
}
