using System;
using System.Runtime.InteropServices;

namespace Nokia.Lucid.Interop.Win32Types
{
	// Token: 0x0200002C RID: 44
	[BestFitMapping(false, ThrowOnUnmappableChar = true)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct WNDCLASSEX
	{
		// Token: 0x040000AF RID: 175
		public int cbSize;

		// Token: 0x040000B0 RID: 176
		public int style;

		// Token: 0x040000B1 RID: 177
		public WNDPROC lpfnWndProc;

		// Token: 0x040000B2 RID: 178
		public int cbClsExtra;

		// Token: 0x040000B3 RID: 179
		public int cbWndExtra;

		// Token: 0x040000B4 RID: 180
		public IntPtr hInstance;

		// Token: 0x040000B5 RID: 181
		public IntPtr hIcon;

		// Token: 0x040000B6 RID: 182
		public IntPtr hCursor;

		// Token: 0x040000B7 RID: 183
		public IntPtr hbrBackground;

		// Token: 0x040000B8 RID: 184
		public IntPtr lpszMenuName;

		// Token: 0x040000B9 RID: 185
		public string lpszClassName;

		// Token: 0x040000BA RID: 186
		public IntPtr hIconSm;
	}
}
