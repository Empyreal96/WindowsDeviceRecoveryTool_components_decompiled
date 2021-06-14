using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.PresentationFramework.Interop
{
	// Token: 0x02000801 RID: 2049
	internal static class OSVersionHelper
	{
		// Token: 0x17001D2E RID: 7470
		// (get) Token: 0x06007DB0 RID: 32176 RVA: 0x00234A8E File Offset: 0x00232C8E
		// (set) Token: 0x06007DB1 RID: 32177 RVA: 0x00234A95 File Offset: 0x00232C95
		internal static bool IsOsWindows10RS5OrGreater { get; set; }

		// Token: 0x17001D2F RID: 7471
		// (get) Token: 0x06007DB2 RID: 32178 RVA: 0x00234A9D File Offset: 0x00232C9D
		// (set) Token: 0x06007DB3 RID: 32179 RVA: 0x00234AA4 File Offset: 0x00232CA4
		internal static bool IsOsWindows10RS3OrGreater { get; set; }

		// Token: 0x17001D30 RID: 7472
		// (get) Token: 0x06007DB4 RID: 32180 RVA: 0x00234AAC File Offset: 0x00232CAC
		// (set) Token: 0x06007DB5 RID: 32181 RVA: 0x00234AB3 File Offset: 0x00232CB3
		internal static bool IsOsWindows10RS2OrGreater { get; set; }

		// Token: 0x17001D31 RID: 7473
		// (get) Token: 0x06007DB6 RID: 32182 RVA: 0x00234ABB File Offset: 0x00232CBB
		// (set) Token: 0x06007DB7 RID: 32183 RVA: 0x00234AC2 File Offset: 0x00232CC2
		internal static bool IsOsWindows10RS1OrGreater { get; set; }

		// Token: 0x17001D32 RID: 7474
		// (get) Token: 0x06007DB8 RID: 32184 RVA: 0x00234ACA File Offset: 0x00232CCA
		// (set) Token: 0x06007DB9 RID: 32185 RVA: 0x00234AD1 File Offset: 0x00232CD1
		internal static bool IsOsWindows10TH2OrGreater { get; set; }

		// Token: 0x17001D33 RID: 7475
		// (get) Token: 0x06007DBA RID: 32186 RVA: 0x00234AD9 File Offset: 0x00232CD9
		// (set) Token: 0x06007DBB RID: 32187 RVA: 0x00234AE0 File Offset: 0x00232CE0
		internal static bool IsOsWindows10TH1OrGreater { get; set; }

		// Token: 0x17001D34 RID: 7476
		// (get) Token: 0x06007DBC RID: 32188 RVA: 0x00234AE8 File Offset: 0x00232CE8
		// (set) Token: 0x06007DBD RID: 32189 RVA: 0x00234AEF File Offset: 0x00232CEF
		internal static bool IsOsWindows10OrGreater { get; set; }

		// Token: 0x17001D35 RID: 7477
		// (get) Token: 0x06007DBE RID: 32190 RVA: 0x00234AF7 File Offset: 0x00232CF7
		// (set) Token: 0x06007DBF RID: 32191 RVA: 0x00234AFE File Offset: 0x00232CFE
		internal static bool IsOsWindows8Point1OrGreater { get; set; }

		// Token: 0x17001D36 RID: 7478
		// (get) Token: 0x06007DC0 RID: 32192 RVA: 0x00234B06 File Offset: 0x00232D06
		// (set) Token: 0x06007DC1 RID: 32193 RVA: 0x00234B0D File Offset: 0x00232D0D
		internal static bool IsOsWindows8OrGreater { get; set; }

		// Token: 0x17001D37 RID: 7479
		// (get) Token: 0x06007DC2 RID: 32194 RVA: 0x00234B15 File Offset: 0x00232D15
		// (set) Token: 0x06007DC3 RID: 32195 RVA: 0x00234B1C File Offset: 0x00232D1C
		internal static bool IsOsWindows7SP1OrGreater { get; set; }

		// Token: 0x17001D38 RID: 7480
		// (get) Token: 0x06007DC4 RID: 32196 RVA: 0x00234B24 File Offset: 0x00232D24
		// (set) Token: 0x06007DC5 RID: 32197 RVA: 0x00234B2B File Offset: 0x00232D2B
		internal static bool IsOsWindows7OrGreater { get; set; }

		// Token: 0x17001D39 RID: 7481
		// (get) Token: 0x06007DC6 RID: 32198 RVA: 0x00234B33 File Offset: 0x00232D33
		// (set) Token: 0x06007DC7 RID: 32199 RVA: 0x00234B3A File Offset: 0x00232D3A
		internal static bool IsOsWindowsVistaSP2OrGreater { get; set; }

		// Token: 0x17001D3A RID: 7482
		// (get) Token: 0x06007DC8 RID: 32200 RVA: 0x00234B42 File Offset: 0x00232D42
		// (set) Token: 0x06007DC9 RID: 32201 RVA: 0x00234B49 File Offset: 0x00232D49
		internal static bool IsOsWindowsVistaSP1OrGreater { get; set; }

		// Token: 0x17001D3B RID: 7483
		// (get) Token: 0x06007DCA RID: 32202 RVA: 0x00234B51 File Offset: 0x00232D51
		// (set) Token: 0x06007DCB RID: 32203 RVA: 0x00234B58 File Offset: 0x00232D58
		internal static bool IsOsWindowsVistaOrGreater { get; set; }

		// Token: 0x17001D3C RID: 7484
		// (get) Token: 0x06007DCC RID: 32204 RVA: 0x00234B60 File Offset: 0x00232D60
		// (set) Token: 0x06007DCD RID: 32205 RVA: 0x00234B67 File Offset: 0x00232D67
		internal static bool IsOsWindowsXPSP3OrGreater { get; set; }

		// Token: 0x17001D3D RID: 7485
		// (get) Token: 0x06007DCE RID: 32206 RVA: 0x00234B6F File Offset: 0x00232D6F
		// (set) Token: 0x06007DCF RID: 32207 RVA: 0x00234B76 File Offset: 0x00232D76
		internal static bool IsOsWindowsXPSP2OrGreater { get; set; }

		// Token: 0x17001D3E RID: 7486
		// (get) Token: 0x06007DD0 RID: 32208 RVA: 0x00234B7E File Offset: 0x00232D7E
		// (set) Token: 0x06007DD1 RID: 32209 RVA: 0x00234B85 File Offset: 0x00232D85
		internal static bool IsOsWindowsXPSP1OrGreater { get; set; }

		// Token: 0x17001D3F RID: 7487
		// (get) Token: 0x06007DD2 RID: 32210 RVA: 0x00234B8D File Offset: 0x00232D8D
		// (set) Token: 0x06007DD3 RID: 32211 RVA: 0x00234B94 File Offset: 0x00232D94
		internal static bool IsOsWindowsXPOrGreater { get; set; }

		// Token: 0x17001D40 RID: 7488
		// (get) Token: 0x06007DD4 RID: 32212 RVA: 0x00234B9C File Offset: 0x00232D9C
		// (set) Token: 0x06007DD5 RID: 32213 RVA: 0x00234BA3 File Offset: 0x00232DA3
		internal static bool IsOsWindowsServer { get; set; }

		// Token: 0x06007DD6 RID: 32214 RVA: 0x00234BAC File Offset: 0x00232DAC
		[SecurityCritical]
		static OSVersionHelper()
		{
			WpfLibraryLoader.EnsureLoaded("PresentationNative_v0400.dll");
			OSVersionHelper.IsOsWindows10RS5OrGreater = OSVersionHelper.IsWindows10RS5OrGreater();
			OSVersionHelper.IsOsWindows10RS3OrGreater = OSVersionHelper.IsWindows10RS3OrGreater();
			OSVersionHelper.IsOsWindows10RS2OrGreater = OSVersionHelper.IsWindows10RS2OrGreater();
			OSVersionHelper.IsOsWindows10RS1OrGreater = OSVersionHelper.IsWindows10RS1OrGreater();
			OSVersionHelper.IsOsWindows10TH2OrGreater = OSVersionHelper.IsWindows10TH2OrGreater();
			OSVersionHelper.IsOsWindows10TH1OrGreater = OSVersionHelper.IsWindows10TH1OrGreater();
			OSVersionHelper.IsOsWindows10OrGreater = OSVersionHelper.IsWindows10OrGreater();
			OSVersionHelper.IsOsWindows8Point1OrGreater = OSVersionHelper.IsWindows8Point1OrGreater();
			OSVersionHelper.IsOsWindows8OrGreater = OSVersionHelper.IsWindows8OrGreater();
			OSVersionHelper.IsOsWindows7SP1OrGreater = OSVersionHelper.IsWindows7SP1OrGreater();
			OSVersionHelper.IsOsWindows7OrGreater = OSVersionHelper.IsWindows7OrGreater();
			OSVersionHelper.IsOsWindowsVistaSP2OrGreater = OSVersionHelper.IsWindowsVistaSP2OrGreater();
			OSVersionHelper.IsOsWindowsVistaSP1OrGreater = OSVersionHelper.IsWindowsVistaSP1OrGreater();
			OSVersionHelper.IsOsWindowsVistaOrGreater = OSVersionHelper.IsWindowsVistaOrGreater();
			OSVersionHelper.IsOsWindowsXPSP3OrGreater = OSVersionHelper.IsWindowsXPSP3OrGreater();
			OSVersionHelper.IsOsWindowsXPSP2OrGreater = OSVersionHelper.IsWindowsXPSP2OrGreater();
			OSVersionHelper.IsOsWindowsXPSP1OrGreater = OSVersionHelper.IsWindowsXPSP1OrGreater();
			OSVersionHelper.IsOsWindowsXPOrGreater = OSVersionHelper.IsWindowsXPOrGreater();
			OSVersionHelper.IsOsWindowsServer = OSVersionHelper.IsWindowsServer();
		}

		// Token: 0x06007DD7 RID: 32215
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10RS5OrGreater();

		// Token: 0x06007DD8 RID: 32216
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10RS3OrGreater();

		// Token: 0x06007DD9 RID: 32217
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10RS2OrGreater();

		// Token: 0x06007DDA RID: 32218
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10RS1OrGreater();

		// Token: 0x06007DDB RID: 32219
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10TH2OrGreater();

		// Token: 0x06007DDC RID: 32220
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10TH1OrGreater();

		// Token: 0x06007DDD RID: 32221
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10OrGreater();

		// Token: 0x06007DDE RID: 32222
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows8Point1OrGreater();

		// Token: 0x06007DDF RID: 32223
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows8OrGreater();

		// Token: 0x06007DE0 RID: 32224
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows7SP1OrGreater();

		// Token: 0x06007DE1 RID: 32225
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows7OrGreater();

		// Token: 0x06007DE2 RID: 32226
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsVistaSP2OrGreater();

		// Token: 0x06007DE3 RID: 32227
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsVistaSP1OrGreater();

		// Token: 0x06007DE4 RID: 32228
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsVistaOrGreater();

		// Token: 0x06007DE5 RID: 32229
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsXPSP3OrGreater();

		// Token: 0x06007DE6 RID: 32230
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsXPSP2OrGreater();

		// Token: 0x06007DE7 RID: 32231
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsXPSP1OrGreater();

		// Token: 0x06007DE8 RID: 32232
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsXPOrGreater();

		// Token: 0x06007DE9 RID: 32233
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsServer();

		// Token: 0x06007DEA RID: 32234 RVA: 0x00234C84 File Offset: 0x00232E84
		internal static bool IsOsVersionOrGreater(OperatingSystemVersion osVer)
		{
			switch (osVer)
			{
			case OperatingSystemVersion.WindowsXPSP2:
				return OSVersionHelper.IsOsWindowsXPSP2OrGreater;
			case OperatingSystemVersion.WindowsXPSP3:
				return OSVersionHelper.IsOsWindowsXPSP3OrGreater;
			case OperatingSystemVersion.WindowsVista:
				return OSVersionHelper.IsOsWindowsVistaOrGreater;
			case OperatingSystemVersion.WindowsVistaSP1:
				return OSVersionHelper.IsOsWindowsVistaSP1OrGreater;
			case OperatingSystemVersion.WindowsVistaSP2:
				return OSVersionHelper.IsOsWindowsVistaSP2OrGreater;
			case OperatingSystemVersion.Windows7:
				return OSVersionHelper.IsOsWindows7OrGreater;
			case OperatingSystemVersion.Windows7SP1:
				return OSVersionHelper.IsOsWindows7SP1OrGreater;
			case OperatingSystemVersion.Windows8:
				return OSVersionHelper.IsOsWindows8OrGreater;
			case OperatingSystemVersion.Windows8Point1:
				return OSVersionHelper.IsOsWindows8Point1OrGreater;
			case OperatingSystemVersion.Windows10:
				return OSVersionHelper.IsOsWindows10OrGreater;
			case OperatingSystemVersion.Windows10TH2:
				return OSVersionHelper.IsOsWindows10TH2OrGreater;
			case OperatingSystemVersion.Windows10RS1:
				return OSVersionHelper.IsOsWindows10RS1OrGreater;
			case OperatingSystemVersion.Windows10RS2:
				return OSVersionHelper.IsOsWindows10RS2OrGreater;
			case OperatingSystemVersion.Windows10RS3:
				return OSVersionHelper.IsOsWindows10RS3OrGreater;
			case OperatingSystemVersion.Windows10RS5:
				return OSVersionHelper.IsOsWindows10RS5OrGreater;
			default:
				throw new ArgumentException(string.Format("{0} is not a valid OS!", osVer.ToString()), "osVer");
			}
		}

		// Token: 0x06007DEB RID: 32235 RVA: 0x00234D50 File Offset: 0x00232F50
		internal static OperatingSystemVersion GetOsVersion()
		{
			if (OSVersionHelper.IsOsWindows10RS5OrGreater)
			{
				return OperatingSystemVersion.Windows10RS3;
			}
			if (OSVersionHelper.IsOsWindows10RS3OrGreater)
			{
				return OperatingSystemVersion.Windows10RS3;
			}
			if (OSVersionHelper.IsOsWindows10RS2OrGreater)
			{
				return OperatingSystemVersion.Windows10RS2;
			}
			if (OSVersionHelper.IsOsWindows10RS1OrGreater)
			{
				return OperatingSystemVersion.Windows10RS1;
			}
			if (OSVersionHelper.IsOsWindows10TH2OrGreater)
			{
				return OperatingSystemVersion.Windows10TH2;
			}
			if (OSVersionHelper.IsOsWindows10OrGreater)
			{
				return OperatingSystemVersion.Windows10;
			}
			if (OSVersionHelper.IsOsWindows8Point1OrGreater)
			{
				return OperatingSystemVersion.Windows8Point1;
			}
			if (OSVersionHelper.IsOsWindows8OrGreater)
			{
				return OperatingSystemVersion.Windows8;
			}
			if (OSVersionHelper.IsOsWindows7SP1OrGreater)
			{
				return OperatingSystemVersion.Windows7SP1;
			}
			if (OSVersionHelper.IsOsWindows7OrGreater)
			{
				return OperatingSystemVersion.Windows7;
			}
			if (OSVersionHelper.IsOsWindowsVistaSP2OrGreater)
			{
				return OperatingSystemVersion.WindowsVistaSP2;
			}
			if (OSVersionHelper.IsOsWindowsVistaSP1OrGreater)
			{
				return OperatingSystemVersion.WindowsVistaSP1;
			}
			if (OSVersionHelper.IsOsWindowsVistaOrGreater)
			{
				return OperatingSystemVersion.WindowsVista;
			}
			if (OSVersionHelper.IsOsWindowsXPSP3OrGreater)
			{
				return OperatingSystemVersion.WindowsXPSP3;
			}
			if (OSVersionHelper.IsOsWindowsXPSP2OrGreater)
			{
				return OperatingSystemVersion.WindowsXPSP2;
			}
			throw new Exception("OSVersionHelper.GetOsVersion Could not detect OS!");
		}
	}
}
