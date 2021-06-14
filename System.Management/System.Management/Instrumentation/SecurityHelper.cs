using System;
using System.Security.Permissions;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B6 RID: 182
	internal sealed class SecurityHelper
	{
		// Token: 0x04000502 RID: 1282
		internal static readonly SecurityPermission UnmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
	}
}
