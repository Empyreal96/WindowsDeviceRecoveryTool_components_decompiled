using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200000D RID: 13
	internal abstract class CsdlElementWithDocumentation : CsdlElement
	{
		// Token: 0x06000043 RID: 67 RVA: 0x000029DC File Offset: 0x00000BDC
		public CsdlElementWithDocumentation(CsdlDocumentation documentation, CsdlLocation location) : base(location)
		{
			this.documentation = documentation;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000029EC File Offset: 0x00000BEC
		public CsdlDocumentation Documentation
		{
			get
			{
				return this.documentation;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000029F4 File Offset: 0x00000BF4
		public override bool HasDirectValueAnnotations
		{
			get
			{
				return this.documentation != null || base.HasDirectValueAnnotations;
			}
		}

		// Token: 0x04000013 RID: 19
		private readonly CsdlDocumentation documentation;
	}
}
