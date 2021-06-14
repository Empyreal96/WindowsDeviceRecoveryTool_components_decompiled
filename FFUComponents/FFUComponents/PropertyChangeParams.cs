using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x0200003E RID: 62
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct PropertyChangeParams
	{
		// Token: 0x040000D0 RID: 208
		public ClassInstallHeader Header;

		// Token: 0x040000D1 RID: 209
		public uint StateChange;

		// Token: 0x040000D2 RID: 210
		public uint Scope;

		// Token: 0x040000D3 RID: 211
		public uint HwProfile;
	}
}
