using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000044 RID: 68
	[CLSCompliant(false)]
	public struct CREATE_VIRTUAL_DISK_PARAMETERS_V1
	{
		// Token: 0x04000112 RID: 274
		public Guid UniqueId;

		// Token: 0x04000113 RID: 275
		public ulong MaximumSize;

		// Token: 0x04000114 RID: 276
		public uint BlockSizeInBytes;

		// Token: 0x04000115 RID: 277
		public uint SectorSizeInBytes;

		// Token: 0x04000116 RID: 278
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ParentPath;

		// Token: 0x04000117 RID: 279
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourcePath;
	}
}
