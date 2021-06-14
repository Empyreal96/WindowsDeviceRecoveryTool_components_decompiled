using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000097 RID: 151
	[StructLayout(LayoutKind.Sequential)]
	internal class SubcategoryMembershipEntry
	{
		// Token: 0x0400028A RID: 650
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Subcategory;

		// Token: 0x0400028B RID: 651
		public ISection CategoryMembershipData;
	}
}
