using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives
{
	// Token: 0x02000008 RID: 8
	public sealed class VariantInfo
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000023B0 File Offset: 0x000005B0
		public VariantInfo(string name, IdentificationInfo identificationInfo, QueryParameters msrQueryParameters)
		{
			this.Name = name;
			this.IdentificationInfo = identificationInfo;
			this.MsrQueryParameters = msrQueryParameters;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000023CD File Offset: 0x000005CD
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000023D5 File Offset: 0x000005D5
		public string Name { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023DE File Offset: 0x000005DE
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000023E6 File Offset: 0x000005E6
		public IdentificationInfo IdentificationInfo { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000023EF File Offset: 0x000005EF
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000023F7 File Offset: 0x000005F7
		public QueryParameters MsrQueryParameters { get; private set; }
	}
}
