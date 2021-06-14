using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200004F RID: 79
	internal struct StoreOperationStageComponent
	{
		// Token: 0x0600017D RID: 381 RVA: 0x0000701A File Offset: 0x0000521A
		public void Destroy()
		{
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000701C File Offset: 0x0000521C
		public StoreOperationStageComponent(IDefinitionAppId app, string Manifest)
		{
			this = new StoreOperationStageComponent(app, null, Manifest);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007027 File Offset: 0x00005227
		[SecuritySafeCritical]
		public StoreOperationStageComponent(IDefinitionAppId app, IDefinitionIdentity comp, string Manifest)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponent));
			this.Flags = StoreOperationStageComponent.OpFlags.Nothing;
			this.Application = app;
			this.Component = comp;
			this.ManifestPath = Manifest;
		}

		// Token: 0x04000148 RID: 328
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000149 RID: 329
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponent.OpFlags Flags;

		// Token: 0x0400014A RID: 330
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x0400014B RID: 331
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x0400014C RID: 332
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x02000517 RID: 1303
		[Flags]
		public enum OpFlags
		{
			// Token: 0x040036E8 RID: 14056
			Nothing = 0
		}

		// Token: 0x02000518 RID: 1304
		public enum Disposition
		{
			// Token: 0x040036EA RID: 14058
			Failed,
			// Token: 0x040036EB RID: 14059
			Installed,
			// Token: 0x040036EC RID: 14060
			Refreshed,
			// Token: 0x040036ED RID: 14061
			AlreadyInstalled
		}
	}
}
