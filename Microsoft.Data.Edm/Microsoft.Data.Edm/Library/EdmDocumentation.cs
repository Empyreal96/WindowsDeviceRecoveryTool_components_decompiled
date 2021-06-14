using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001CC RID: 460
	public class EdmDocumentation : IEdmDocumentation
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x0001FEFF File Offset: 0x0001E0FF
		public EdmDocumentation(string summary, string description)
		{
			this.summary = summary;
			this.description = description;
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0001FF15 File Offset: 0x0001E115
		public string Summary
		{
			get
			{
				return this.summary;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0001FF1D File Offset: 0x0001E11D
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x0400051F RID: 1311
		private readonly string summary;

		// Token: 0x04000520 RID: 1312
		private readonly string description;
	}
}
