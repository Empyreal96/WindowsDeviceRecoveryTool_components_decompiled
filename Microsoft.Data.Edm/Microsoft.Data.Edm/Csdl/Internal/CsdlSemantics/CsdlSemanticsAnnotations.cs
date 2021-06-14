using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000068 RID: 104
	internal class CsdlSemanticsAnnotations
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x00005243 File Offset: 0x00003443
		public CsdlSemanticsAnnotations(CsdlSemanticsSchema context, CsdlAnnotations annotations)
		{
			this.context = context;
			this.annotations = annotations;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00005259 File Offset: 0x00003459
		public CsdlSemanticsSchema Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00005261 File Offset: 0x00003461
		public CsdlAnnotations Annotations
		{
			get
			{
				return this.annotations;
			}
		}

		// Token: 0x040000BA RID: 186
		private readonly CsdlAnnotations annotations;

		// Token: 0x040000BB RID: 187
		private readonly CsdlSemanticsSchema context;
	}
}
