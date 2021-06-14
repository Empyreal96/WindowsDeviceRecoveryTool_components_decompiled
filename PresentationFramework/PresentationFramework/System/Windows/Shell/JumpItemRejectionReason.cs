using System;

namespace System.Windows.Shell
{
	/// <summary>Describes why a <see cref="T:System.Windows.Shell.JumpItem" /> could not be added to the Jump List by the Windows shell.</summary>
	// Token: 0x02000145 RID: 325
	public enum JumpItemRejectionReason
	{
		/// <summary>The reason is not specified.</summary>
		// Token: 0x0400111E RID: 4382
		None,
		/// <summary>The <see cref="T:System.Windows.Shell.JumpItem" /> references an invalid file path, or the operating system does not support Jump Lists.</summary>
		// Token: 0x0400111F RID: 4383
		InvalidItem,
		/// <summary>The application is not registered to handle the file name extension of the <see cref="T:System.Windows.Shell.JumpItem" />.</summary>
		// Token: 0x04001120 RID: 4384
		NoRegisteredHandler,
		/// <summary>The item was previously in the Jump List but was removed by the user.</summary>
		// Token: 0x04001121 RID: 4385
		RemovedByUser
	}
}
