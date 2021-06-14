using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200009A RID: 154
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipEntry
	{
		// Token: 0x0400028E RID: 654
		public IDefinitionIdentity Identity;

		// Token: 0x0400028F RID: 655
		public ISection SubcategoryMembership;
	}
}
