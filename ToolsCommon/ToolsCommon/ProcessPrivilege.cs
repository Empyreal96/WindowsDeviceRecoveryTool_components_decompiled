using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000026 RID: 38
	public static class ProcessPrivilege
	{
		// Token: 0x0600012E RID: 302 RVA: 0x00007C5C File Offset: 0x00005E5C
		public static void Adjust(TokenPrivilege privilege, bool enablePrivilege)
		{
			int num = NativeSecurityMethods.IU_AdjustProcessPrivilege(privilege.Value, enablePrivilege);
			if (num != 0)
			{
				throw new Exception(string.Format("Failed to adjust privilege with name {0} and value {1}", privilege.Value, enablePrivilege));
			}
		}
	}
}
