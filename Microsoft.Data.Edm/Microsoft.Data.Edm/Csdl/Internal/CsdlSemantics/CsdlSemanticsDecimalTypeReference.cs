using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000169 RID: 361
	internal class CsdlSemanticsDecimalTypeReference : CsdlSemanticsPrimitiveTypeReference, IEdmDecimalTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060007A0 RID: 1952 RVA: 0x00014BC9 File Offset: 0x00012DC9
		public CsdlSemanticsDecimalTypeReference(CsdlSemanticsSchema schema, CsdlDecimalTypeReference reference) : base(schema, reference)
		{
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00014BD3 File Offset: 0x00012DD3
		public int? Precision
		{
			get
			{
				return ((CsdlDecimalTypeReference)this.Reference).Precision;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00014BE5 File Offset: 0x00012DE5
		public int? Scale
		{
			get
			{
				return ((CsdlDecimalTypeReference)this.Reference).Scale;
			}
		}
	}
}
