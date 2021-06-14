using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Printing
{
	// Token: 0x02000656 RID: 1622
	internal static class NativeMethods
	{
		// Token: 0x040034E6 RID: 13542
		internal const uint PD_ALLPAGES = 0U;

		// Token: 0x040034E7 RID: 13543
		internal const uint PD_SELECTION = 1U;

		// Token: 0x040034E8 RID: 13544
		internal const uint PD_PAGENUMS = 2U;

		// Token: 0x040034E9 RID: 13545
		internal const uint PD_NOSELECTION = 4U;

		// Token: 0x040034EA RID: 13546
		internal const uint PD_NOPAGENUMS = 8U;

		// Token: 0x040034EB RID: 13547
		internal const uint PD_USEDEVMODECOPIESANDCOLLATE = 262144U;

		// Token: 0x040034EC RID: 13548
		internal const uint PD_DISABLEPRINTTOFILE = 524288U;

		// Token: 0x040034ED RID: 13549
		internal const uint PD_HIDEPRINTTOFILE = 1048576U;

		// Token: 0x040034EE RID: 13550
		internal const uint PD_CURRENTPAGE = 4194304U;

		// Token: 0x040034EF RID: 13551
		internal const uint PD_NOCURRENTPAGE = 8388608U;

		// Token: 0x040034F0 RID: 13552
		internal const uint PD_RESULT_CANCEL = 0U;

		// Token: 0x040034F1 RID: 13553
		internal const uint PD_RESULT_PRINT = 1U;

		// Token: 0x040034F2 RID: 13554
		internal const uint PD_RESULT_APPLY = 2U;

		// Token: 0x040034F3 RID: 13555
		internal const uint START_PAGE_GENERAL = 4294967295U;

		// Token: 0x02000B15 RID: 2837
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal class PRINTDLGEX32
		{
			// Token: 0x040049F1 RID: 18929
			public int lStructSize = Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX32));

			// Token: 0x040049F2 RID: 18930
			public IntPtr hwndOwner = IntPtr.Zero;

			// Token: 0x040049F3 RID: 18931
			public IntPtr hDevMode = IntPtr.Zero;

			// Token: 0x040049F4 RID: 18932
			public IntPtr hDevNames = IntPtr.Zero;

			// Token: 0x040049F5 RID: 18933
			public IntPtr hDC = IntPtr.Zero;

			// Token: 0x040049F6 RID: 18934
			public uint Flags;

			// Token: 0x040049F7 RID: 18935
			public uint Flags2;

			// Token: 0x040049F8 RID: 18936
			public uint ExclusionFlags;

			// Token: 0x040049F9 RID: 18937
			public uint nPageRanges;

			// Token: 0x040049FA RID: 18938
			public uint nMaxPageRanges;

			// Token: 0x040049FB RID: 18939
			public IntPtr lpPageRanges = IntPtr.Zero;

			// Token: 0x040049FC RID: 18940
			public uint nMinPage;

			// Token: 0x040049FD RID: 18941
			public uint nMaxPage;

			// Token: 0x040049FE RID: 18942
			public uint nCopies;

			// Token: 0x040049FF RID: 18943
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x04004A00 RID: 18944
			public IntPtr lpPrintTemplateName = IntPtr.Zero;

			// Token: 0x04004A01 RID: 18945
			public IntPtr lpCallback = IntPtr.Zero;

			// Token: 0x04004A02 RID: 18946
			public uint nPropertyPages;

			// Token: 0x04004A03 RID: 18947
			public IntPtr lphPropertyPages = IntPtr.Zero;

			// Token: 0x04004A04 RID: 18948
			public uint nStartPage = uint.MaxValue;

			// Token: 0x04004A05 RID: 18949
			public uint dwResultAction;
		}

		// Token: 0x02000B16 RID: 2838
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 8)]
		internal class PRINTDLGEX64
		{
			// Token: 0x04004A06 RID: 18950
			public int lStructSize = Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX64));

			// Token: 0x04004A07 RID: 18951
			public IntPtr hwndOwner = IntPtr.Zero;

			// Token: 0x04004A08 RID: 18952
			public IntPtr hDevMode = IntPtr.Zero;

			// Token: 0x04004A09 RID: 18953
			public IntPtr hDevNames = IntPtr.Zero;

			// Token: 0x04004A0A RID: 18954
			public IntPtr hDC = IntPtr.Zero;

			// Token: 0x04004A0B RID: 18955
			public uint Flags;

			// Token: 0x04004A0C RID: 18956
			public uint Flags2;

			// Token: 0x04004A0D RID: 18957
			public uint ExclusionFlags;

			// Token: 0x04004A0E RID: 18958
			public uint nPageRanges;

			// Token: 0x04004A0F RID: 18959
			public uint nMaxPageRanges;

			// Token: 0x04004A10 RID: 18960
			public IntPtr lpPageRanges = IntPtr.Zero;

			// Token: 0x04004A11 RID: 18961
			public uint nMinPage;

			// Token: 0x04004A12 RID: 18962
			public uint nMaxPage;

			// Token: 0x04004A13 RID: 18963
			public uint nCopies;

			// Token: 0x04004A14 RID: 18964
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x04004A15 RID: 18965
			public IntPtr lpPrintTemplateName = IntPtr.Zero;

			// Token: 0x04004A16 RID: 18966
			public IntPtr lpCallback = IntPtr.Zero;

			// Token: 0x04004A17 RID: 18967
			public uint nPropertyPages;

			// Token: 0x04004A18 RID: 18968
			public IntPtr lphPropertyPages = IntPtr.Zero;

			// Token: 0x04004A19 RID: 18969
			public uint nStartPage = uint.MaxValue;

			// Token: 0x04004A1A RID: 18970
			public uint dwResultAction;
		}

		// Token: 0x02000B17 RID: 2839
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal struct DEVMODE
		{
			// Token: 0x04004A1B RID: 18971
			private const int CCHDEVICENAME = 32;

			// Token: 0x04004A1C RID: 18972
			private const int CCHFORMNAME = 32;

			// Token: 0x04004A1D RID: 18973
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmDeviceName;

			// Token: 0x04004A1E RID: 18974
			public ushort dmSpecVersion;

			// Token: 0x04004A1F RID: 18975
			public ushort dmDriverVersion;

			// Token: 0x04004A20 RID: 18976
			public ushort dmSize;

			// Token: 0x04004A21 RID: 18977
			public ushort dmDriverExtra;

			// Token: 0x04004A22 RID: 18978
			public uint dmFields;

			// Token: 0x04004A23 RID: 18979
			public int dmPositionX;

			// Token: 0x04004A24 RID: 18980
			public int dmPositionY;

			// Token: 0x04004A25 RID: 18981
			public uint dmDisplayOrientation;

			// Token: 0x04004A26 RID: 18982
			public uint dmDisplayFixedOutput;

			// Token: 0x04004A27 RID: 18983
			public short dmColor;

			// Token: 0x04004A28 RID: 18984
			public short dmDuplex;

			// Token: 0x04004A29 RID: 18985
			public short dmYResolution;

			// Token: 0x04004A2A RID: 18986
			public short dmTTOption;

			// Token: 0x04004A2B RID: 18987
			public short dmCollate;

			// Token: 0x04004A2C RID: 18988
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmFormName;

			// Token: 0x04004A2D RID: 18989
			public ushort dmLogPixels;

			// Token: 0x04004A2E RID: 18990
			public uint dmBitsPerPel;

			// Token: 0x04004A2F RID: 18991
			public uint dmPelsWidth;

			// Token: 0x04004A30 RID: 18992
			public uint dmPelsHeight;

			// Token: 0x04004A31 RID: 18993
			public uint dmDisplayFlags;

			// Token: 0x04004A32 RID: 18994
			public uint dmDisplayFrequency;

			// Token: 0x04004A33 RID: 18995
			public uint dmICMMethod;

			// Token: 0x04004A34 RID: 18996
			public uint dmICMIntent;

			// Token: 0x04004A35 RID: 18997
			public uint dmMediaType;

			// Token: 0x04004A36 RID: 18998
			public uint dmDitherType;

			// Token: 0x04004A37 RID: 18999
			public uint dmReserved1;

			// Token: 0x04004A38 RID: 19000
			public uint dmReserved2;

			// Token: 0x04004A39 RID: 19001
			public uint dmPanningWidth;

			// Token: 0x04004A3A RID: 19002
			public uint dmPanningHeight;
		}

		// Token: 0x02000B18 RID: 2840
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal struct DEVNAMES
		{
			// Token: 0x04004A3B RID: 19003
			public ushort wDriverOffset;

			// Token: 0x04004A3C RID: 19004
			public ushort wDeviceOffset;

			// Token: 0x04004A3D RID: 19005
			public ushort wOutputOffset;

			// Token: 0x04004A3E RID: 19006
			public ushort wDefault;
		}

		// Token: 0x02000B19 RID: 2841
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal struct PRINTPAGERANGE
		{
			// Token: 0x04004A3F RID: 19007
			public uint nFromPage;

			// Token: 0x04004A40 RID: 19008
			public uint nToPage;
		}
	}
}
