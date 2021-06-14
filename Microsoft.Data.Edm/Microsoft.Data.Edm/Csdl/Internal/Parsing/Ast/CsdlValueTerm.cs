using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000067 RID: 103
	internal class CsdlValueTerm : CsdlNamedElement
	{
		// Token: 0x060001AF RID: 431 RVA: 0x00005228 File Offset: 0x00003428
		public CsdlValueTerm(string name, CsdlTypeReference type, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.type = type;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000523B File Offset: 0x0000343B
		public CsdlTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x040000B9 RID: 185
		private readonly CsdlTypeReference type;
	}
}
