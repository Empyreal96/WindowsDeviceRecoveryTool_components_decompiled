using System;

namespace Microsoft.Data.OData.Query.Metadata
{
	// Token: 0x020000A0 RID: 160
	internal sealed class ODataQueryEdmTypeAnnotation
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0000BDE2 File Offset: 0x00009FE2
		// (set) Token: 0x060003BF RID: 959 RVA: 0x0000BDEA File Offset: 0x00009FEA
		public bool CanReflectOnInstanceType { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000BDF3 File Offset: 0x00009FF3
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0000BDFB File Offset: 0x00009FFB
		public Type InstanceType { get; set; }
	}
}
