using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000054 RID: 84
	internal struct StoreOperationInstallDeployment
	{
		// Token: 0x0600018B RID: 395 RVA: 0x000071CD File Offset: 0x000053CD
		public StoreOperationInstallDeployment(IDefinitionAppId App, StoreApplicationReference reference)
		{
			this = new StoreOperationInstallDeployment(App, true, reference);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000071D8 File Offset: 0x000053D8
		[SecuritySafeCritical]
		public StoreOperationInstallDeployment(IDefinitionAppId App, bool UninstallOthers, StoreApplicationReference reference)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationInstallDeployment));
			this.Flags = StoreOperationInstallDeployment.OpFlags.Nothing;
			this.Application = App;
			if (UninstallOthers)
			{
				this.Flags |= StoreOperationInstallDeployment.OpFlags.UninstallOthers;
			}
			this.Reference = reference.ToIntPtr();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007226 File Offset: 0x00005426
		[SecurityCritical]
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000161 RID: 353
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000162 RID: 354
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationInstallDeployment.OpFlags Flags;

		// Token: 0x04000163 RID: 355
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000164 RID: 356
		public IntPtr Reference;

		// Token: 0x02000520 RID: 1312
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04003703 RID: 14083
			Nothing = 0,
			// Token: 0x04003704 RID: 14084
			UninstallOthers = 1
		}

		// Token: 0x02000521 RID: 1313
		public enum Disposition
		{
			// Token: 0x04003706 RID: 14086
			Failed,
			// Token: 0x04003707 RID: 14087
			AlreadyInstalled,
			// Token: 0x04003708 RID: 14088
			Installed
		}
	}
}
