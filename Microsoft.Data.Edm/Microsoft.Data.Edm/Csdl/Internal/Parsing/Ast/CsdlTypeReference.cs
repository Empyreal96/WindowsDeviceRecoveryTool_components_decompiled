using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000020 RID: 32
	internal abstract class CsdlTypeReference : CsdlElement
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00002D15 File Offset: 0x00000F15
		protected CsdlTypeReference(bool isNullable, CsdlLocation location) : base(location)
		{
			this.isNullable = isNullable;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002D25 File Offset: 0x00000F25
		public bool IsNullable
		{
			get
			{
				return this.isNullable;
			}
		}

		// Token: 0x04000032 RID: 50
		private readonly bool isNullable;
	}
}
