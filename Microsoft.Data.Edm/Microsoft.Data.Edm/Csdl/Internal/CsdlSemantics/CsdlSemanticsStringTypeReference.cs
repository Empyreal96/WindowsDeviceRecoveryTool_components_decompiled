using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001AE RID: 430
	internal class CsdlSemanticsStringTypeReference : CsdlSemanticsPrimitiveTypeReference, IEdmStringTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x06000980 RID: 2432 RVA: 0x0001A02D File Offset: 0x0001822D
		public CsdlSemanticsStringTypeReference(CsdlSemanticsSchema schema, CsdlStringTypeReference reference) : base(schema, reference)
		{
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0001A037 File Offset: 0x00018237
		public bool? IsFixedLength
		{
			get
			{
				return ((CsdlStringTypeReference)this.Reference).IsFixedLength;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0001A049 File Offset: 0x00018249
		public bool IsUnbounded
		{
			get
			{
				return ((CsdlStringTypeReference)this.Reference).IsUnbounded;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0001A05B File Offset: 0x0001825B
		public int? MaxLength
		{
			get
			{
				return ((CsdlStringTypeReference)this.Reference).MaxLength;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0001A06D File Offset: 0x0001826D
		public bool? IsUnicode
		{
			get
			{
				return ((CsdlStringTypeReference)this.Reference).IsUnicode;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0001A07F File Offset: 0x0001827F
		public string Collation
		{
			get
			{
				return ((CsdlStringTypeReference)this.Reference).Collation;
			}
		}
	}
}
