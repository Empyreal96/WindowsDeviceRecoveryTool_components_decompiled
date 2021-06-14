using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200005C RID: 92
	[StructLayout(LayoutKind.Sequential)]
	internal class StartupInput
	{
		// Token: 0x04000494 RID: 1172
		public int GdiplusVersion = 1;

		// Token: 0x04000495 RID: 1173
		public IntPtr DebugEventCallback;

		// Token: 0x04000496 RID: 1174
		public bool SuppressBackgroundThread;

		// Token: 0x04000497 RID: 1175
		public bool SuppressExternalCodecs;
	}
}
