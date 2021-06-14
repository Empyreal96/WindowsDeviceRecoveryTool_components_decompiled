using System;
using System.Security;
using System.Security.Permissions;

namespace MS.Internal.Permissions
{
	// Token: 0x020005FC RID: 1532
	[Serializable]
	internal class UserInitiatedNavigationPermission : InternalParameterlessPermissionBase
	{
		// Token: 0x06006601 RID: 26113 RVA: 0x001CACB4 File Offset: 0x001C8EB4
		public UserInitiatedNavigationPermission() : this(PermissionState.Unrestricted)
		{
		}

		// Token: 0x06006602 RID: 26114 RVA: 0x001CACBD File Offset: 0x001C8EBD
		public UserInitiatedNavigationPermission(PermissionState state) : base(state)
		{
		}

		// Token: 0x06006603 RID: 26115 RVA: 0x001CACC6 File Offset: 0x001C8EC6
		public override IPermission Copy()
		{
			return new UserInitiatedNavigationPermission();
		}
	}
}
