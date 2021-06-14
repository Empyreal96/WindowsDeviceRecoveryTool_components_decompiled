using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200014D RID: 333
	internal class CsdlTemporalTypeReference : CsdlPrimitiveTypeReference
	{
		// Token: 0x06000639 RID: 1593 RVA: 0x0000F969 File Offset: 0x0000DB69
		public CsdlTemporalTypeReference(EdmPrimitiveTypeKind kind, int? precision, string typeName, bool isNullable, CsdlLocation location) : base(kind, typeName, isNullable, location)
		{
			this.precision = precision;
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0000F97E File Offset: 0x0000DB7E
		public int? Precision
		{
			get
			{
				return this.precision;
			}
		}

		// Token: 0x04000358 RID: 856
		private readonly int? precision;
	}
}
