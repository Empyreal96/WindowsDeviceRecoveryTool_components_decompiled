using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000025 RID: 37
	internal struct STORE_CATEGORY_INSTANCE
	{
		// Token: 0x04000110 RID: 272
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x04000111 RID: 273
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
