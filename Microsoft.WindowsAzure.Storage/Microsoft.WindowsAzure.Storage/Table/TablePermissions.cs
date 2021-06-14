using System;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000149 RID: 329
	public sealed class TablePermissions
	{
		// Token: 0x060014BC RID: 5308 RVA: 0x0004F4BD File Offset: 0x0004D6BD
		public TablePermissions()
		{
			this.SharedAccessPolicies = new SharedAccessTablePolicies();
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x0004F4D0 File Offset: 0x0004D6D0
		// (set) Token: 0x060014BE RID: 5310 RVA: 0x0004F4D8 File Offset: 0x0004D6D8
		public SharedAccessTablePolicies SharedAccessPolicies { get; private set; }
	}
}
