using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000752 RID: 1874
	internal class ActiveXHelper
	{
		// Token: 0x0600776A RID: 30570 RVA: 0x0000326D File Offset: 0x0000146D
		private ActiveXHelper()
		{
		}

		// Token: 0x0600776B RID: 30571 RVA: 0x00221BA1 File Offset: 0x0021FDA1
		public static int Pix2HM(int pix, int logP)
		{
			return (2540 * pix + (logP >> 1)) / logP;
		}

		// Token: 0x0600776C RID: 30572 RVA: 0x00221BB0 File Offset: 0x0021FDB0
		public static int HM2Pix(int hm, int logP)
		{
			return (logP * hm + 1270) / 2540;
		}

		// Token: 0x17001C5A RID: 7258
		// (get) Token: 0x0600776D RID: 30573 RVA: 0x00221BC4 File Offset: 0x0021FDC4
		public static int LogPixelsX
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (ActiveXHelper.logPixelsX == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						ActiveXHelper.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return ActiveXHelper.logPixelsX;
			}
		}

		// Token: 0x0600776E RID: 30574 RVA: 0x00221C1B File Offset: 0x0021FE1B
		public static void ResetLogPixelsX()
		{
			ActiveXHelper.logPixelsX = -1;
		}

		// Token: 0x17001C5B RID: 7259
		// (get) Token: 0x0600776F RID: 30575 RVA: 0x00221C24 File Offset: 0x0021FE24
		public static int LogPixelsY
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (ActiveXHelper.logPixelsY == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						ActiveXHelper.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return ActiveXHelper.logPixelsY;
			}
		}

		// Token: 0x06007770 RID: 30576 RVA: 0x00221C7B File Offset: 0x0021FE7B
		public static void ResetLogPixelsY()
		{
			ActiveXHelper.logPixelsY = -1;
		}

		// Token: 0x06007771 RID: 30577
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		[DllImport("PresentationHost_v0400.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		internal static extern object CreateIDispatchSTAForwarder([MarshalAs(UnmanagedType.IDispatch)] object pDispatchDelegate);

		// Token: 0x040038BE RID: 14526
		public static readonly int sinkAttached = BitVector32.CreateMask();

		// Token: 0x040038BF RID: 14527
		public static readonly int inTransition = BitVector32.CreateMask(ActiveXHelper.sinkAttached);

		// Token: 0x040038C0 RID: 14528
		public static readonly int processingKeyUp = BitVector32.CreateMask(ActiveXHelper.inTransition);

		// Token: 0x040038C1 RID: 14529
		private static int logPixelsX = -1;

		// Token: 0x040038C2 RID: 14530
		private static int logPixelsY = -1;

		// Token: 0x040038C3 RID: 14531
		private const int HMperInch = 2540;

		// Token: 0x02000B6A RID: 2922
		public enum ActiveXState
		{
			// Token: 0x04004B41 RID: 19265
			Passive,
			// Token: 0x04004B42 RID: 19266
			Loaded,
			// Token: 0x04004B43 RID: 19267
			Running,
			// Token: 0x04004B44 RID: 19268
			InPlaceActive = 4,
			// Token: 0x04004B45 RID: 19269
			UIActive = 8,
			// Token: 0x04004B46 RID: 19270
			Open = 16
		}
	}
}
