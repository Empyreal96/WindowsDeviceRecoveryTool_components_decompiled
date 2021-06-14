using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000050 RID: 80
	internal struct StoreOperationStageComponentFile
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000705A File Offset: 0x0000525A
		public StoreOperationStageComponentFile(IDefinitionAppId App, string CompRelPath, string SrcFile)
		{
			this = new StoreOperationStageComponentFile(App, null, CompRelPath, SrcFile);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007066 File Offset: 0x00005266
		[SecuritySafeCritical]
		public StoreOperationStageComponentFile(IDefinitionAppId App, IDefinitionIdentity Component, string CompRelPath, string SrcFile)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponentFile));
			this.Flags = StoreOperationStageComponentFile.OpFlags.Nothing;
			this.Application = App;
			this.Component = Component;
			this.ComponentRelativePath = CompRelPath;
			this.SourceFilePath = SrcFile;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000701A File Offset: 0x0000521A
		public void Destroy()
		{
		}

		// Token: 0x0400014D RID: 333
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400014E RID: 334
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponentFile.OpFlags Flags;

		// Token: 0x0400014F RID: 335
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000150 RID: 336
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x04000151 RID: 337
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ComponentRelativePath;

		// Token: 0x04000152 RID: 338
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceFilePath;

		// Token: 0x02000519 RID: 1305
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040036EF RID: 14063
			Nothing = 0
		}

		// Token: 0x0200051A RID: 1306
		public enum Disposition
		{
			// Token: 0x040036F1 RID: 14065
			Failed,
			// Token: 0x040036F2 RID: 14066
			Installed,
			// Token: 0x040036F3 RID: 14067
			Refreshed,
			// Token: 0x040036F4 RID: 14068
			AlreadyInstalled
		}
	}
}
