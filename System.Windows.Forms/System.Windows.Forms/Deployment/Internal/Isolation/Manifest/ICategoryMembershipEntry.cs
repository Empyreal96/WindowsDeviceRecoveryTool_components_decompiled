using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200009C RID: 156
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("97FDCA77-B6F2-4718-A1EB-29D0AECE9C03")]
	[ComImport]
	internal interface ICategoryMembershipEntry
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600026B RID: 619
		CategoryMembershipEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600026C RID: 620
		IDefinitionIdentity Identity { [SecurityCritical] get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600026D RID: 621
		ISection SubcategoryMembership { [SecurityCritical] get; }
	}
}
