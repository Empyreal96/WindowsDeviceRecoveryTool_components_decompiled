using System;
using System.Runtime.InteropServices;

namespace Interop.SirepClient
{
	// Token: 0x0200000B RID: 11
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FileInfox
	{
		// Token: 0x04000020 RID: 32
		public int m_FileAttributes;

		// Token: 0x04000021 RID: 33
		public long m_FileSize;

		// Token: 0x04000022 RID: 34
		public long m_CreationTime;

		// Token: 0x04000023 RID: 35
		public long m_LastAccessTime;

		// Token: 0x04000024 RID: 36
		public long m_LastWriteTime;
	}
}
