using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000025 RID: 37
	public class PrivilegeNames
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00007C2F File Offset: 0x00005E2F
		public static TokenPrivilege BackupPrivilege
		{
			get
			{
				return new TokenPrivilege("SeBackupPrivilege");
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00007C3B File Offset: 0x00005E3B
		public static TokenPrivilege SecurityPrivilege
		{
			get
			{
				return new TokenPrivilege("SeSecurityPrivilege");
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00007C47 File Offset: 0x00005E47
		public static TokenPrivilege RestorePrivilege
		{
			get
			{
				return new TokenPrivilege("SeRestorePrivilege");
			}
		}
	}
}
