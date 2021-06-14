using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000166 RID: 358
	internal class CsdlSemanticsBinaryTypeReference : CsdlSemanticsPrimitiveTypeReference, IEdmBinaryTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x0600077E RID: 1918 RVA: 0x00014851 File Offset: 0x00012A51
		public CsdlSemanticsBinaryTypeReference(CsdlSemanticsSchema schema, CsdlBinaryTypeReference reference) : base(schema, reference)
		{
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0001485B File Offset: 0x00012A5B
		public bool? IsFixedLength
		{
			get
			{
				return ((CsdlBinaryTypeReference)this.Reference).IsFixedLength;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x0001486D File Offset: 0x00012A6D
		public bool IsUnbounded
		{
			get
			{
				return ((CsdlBinaryTypeReference)this.Reference).IsUnbounded;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0001487F File Offset: 0x00012A7F
		public int? MaxLength
		{
			get
			{
				return ((CsdlBinaryTypeReference)this.Reference).MaxLength;
			}
		}
	}
}
