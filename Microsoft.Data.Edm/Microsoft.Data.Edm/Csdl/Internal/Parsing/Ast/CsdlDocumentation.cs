using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000138 RID: 312
	internal class CsdlDocumentation : CsdlElement
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x0000F4C5 File Offset: 0x0000D6C5
		public CsdlDocumentation(string summary, string longDescription, CsdlLocation location) : base(location)
		{
			this.summary = summary;
			this.longDescription = longDescription;
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0000F4DC File Offset: 0x0000D6DC
		public string Summary
		{
			get
			{
				return this.summary;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0000F4E4 File Offset: 0x0000D6E4
		public string LongDescription
		{
			get
			{
				return this.longDescription;
			}
		}

		// Token: 0x04000323 RID: 803
		private readonly string summary;

		// Token: 0x04000324 RID: 804
		private readonly string longDescription;
	}
}
