using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000028 RID: 40
	internal struct CATEGORY_INSTANCE
	{
		// Token: 0x04000114 RID: 276
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x04000115 RID: 277
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
