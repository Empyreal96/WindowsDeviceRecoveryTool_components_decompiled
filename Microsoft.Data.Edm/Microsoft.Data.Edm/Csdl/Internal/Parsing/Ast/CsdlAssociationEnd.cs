using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000130 RID: 304
	internal class CsdlAssociationEnd : CsdlNamedElement
	{
		// Token: 0x060005D4 RID: 1492 RVA: 0x0000F354 File Offset: 0x0000D554
		public CsdlAssociationEnd(string role, CsdlTypeReference type, EdmMultiplicity multiplicity, CsdlOnDelete onDelete, CsdlDocumentation documentation, CsdlLocation location) : base(role, documentation, location)
		{
			this.type = type;
			this.multiplicity = multiplicity;
			this.onDelete = onDelete;
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0000F377 File Offset: 0x0000D577
		public CsdlTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0000F37F File Offset: 0x0000D57F
		public EdmMultiplicity Multiplicity
		{
			get
			{
				return this.multiplicity;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0000F387 File Offset: 0x0000D587
		public CsdlOnDelete OnDelete
		{
			get
			{
				return this.onDelete;
			}
		}

		// Token: 0x04000312 RID: 786
		private readonly CsdlTypeReference type;

		// Token: 0x04000313 RID: 787
		private readonly EdmMultiplicity multiplicity;

		// Token: 0x04000314 RID: 788
		private readonly CsdlOnDelete onDelete;
	}
}
