using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000133 RID: 307
	internal class CsdlBinaryTypeReference : CsdlPrimitiveTypeReference
	{
		// Token: 0x060005DF RID: 1503 RVA: 0x0000F3F3 File Offset: 0x0000D5F3
		public CsdlBinaryTypeReference(bool? isFixedLength, bool isUnbounded, int? maxLength, string typeName, bool isNullable, CsdlLocation location) : base(EdmPrimitiveTypeKind.Binary, typeName, isNullable, location)
		{
			this.isFixedLength = isFixedLength;
			this.isUnbounded = isUnbounded;
			this.maxLength = maxLength;
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0000F417 File Offset: 0x0000D617
		public bool? IsFixedLength
		{
			get
			{
				return this.isFixedLength;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0000F41F File Offset: 0x0000D61F
		public bool IsUnbounded
		{
			get
			{
				return this.isUnbounded;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0000F427 File Offset: 0x0000D627
		public int? MaxLength
		{
			get
			{
				return this.maxLength;
			}
		}

		// Token: 0x0400031A RID: 794
		private readonly bool? isFixedLength;

		// Token: 0x0400031B RID: 795
		private readonly bool isUnbounded;

		// Token: 0x0400031C RID: 796
		private readonly int? maxLength;
	}
}
