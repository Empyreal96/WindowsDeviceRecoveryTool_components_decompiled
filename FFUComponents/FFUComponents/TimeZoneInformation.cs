using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000035 RID: 53
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct TimeZoneInformation
	{
		// Token: 0x040000A3 RID: 163
		public int Bias;

		// Token: 0x040000A4 RID: 164
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string StandardName;

		// Token: 0x040000A5 RID: 165
		public SystemTime StandardDate;

		// Token: 0x040000A6 RID: 166
		public int StandardBias;

		// Token: 0x040000A7 RID: 167
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DaylightName;

		// Token: 0x040000A8 RID: 168
		public SystemTime DaylightDate;

		// Token: 0x040000A9 RID: 169
		public int DaylightBias;
	}
}
