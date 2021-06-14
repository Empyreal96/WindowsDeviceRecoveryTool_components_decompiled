using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200042B RID: 1067
	internal static class WebBrowserHelper
	{
		// Token: 0x06004AB5 RID: 19125 RVA: 0x00135333 File Offset: 0x00133533
		internal static int Pix2HM(int pix, int logP)
		{
			return (2540 * pix + (logP >> 1)) / logP;
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x00135342 File Offset: 0x00133542
		internal static int HM2Pix(int hm, int logP)
		{
			return (logP * hm + 1270) / 2540;
		}

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06004AB7 RID: 19127 RVA: 0x00135354 File Offset: 0x00133554
		internal static int LogPixelsX
		{
			get
			{
				if (WebBrowserHelper.logPixelsX == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						WebBrowserHelper.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return WebBrowserHelper.logPixelsX;
			}
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x001353AB File Offset: 0x001335AB
		internal static void ResetLogPixelsX()
		{
			WebBrowserHelper.logPixelsX = -1;
		}

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06004AB9 RID: 19129 RVA: 0x001353B4 File Offset: 0x001335B4
		internal static int LogPixelsY
		{
			get
			{
				if (WebBrowserHelper.logPixelsY == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						WebBrowserHelper.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return WebBrowserHelper.logPixelsY;
			}
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0013540B File Offset: 0x0013360B
		internal static void ResetLogPixelsY()
		{
			WebBrowserHelper.logPixelsY = -1;
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x00135414 File Offset: 0x00133614
		internal static ISelectionService GetSelectionService(Control ctl)
		{
			ISite site = ctl.Site;
			if (site != null)
			{
				object service = site.GetService(typeof(ISelectionService));
				if (service is ISelectionService)
				{
					return (ISelectionService)service;
				}
			}
			return null;
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x0013544C File Offset: 0x0013364C
		internal static NativeMethods.COMRECT GetClipRect()
		{
			return new NativeMethods.COMRECT(new Rectangle(0, 0, 32000, 32000));
		}

		// Token: 0x0400273A RID: 10042
		internal static readonly int sinkAttached = BitVector32.CreateMask();

		// Token: 0x0400273B RID: 10043
		internal static readonly int manualUpdate = BitVector32.CreateMask(WebBrowserHelper.sinkAttached);

		// Token: 0x0400273C RID: 10044
		internal static readonly int setClientSiteFirst = BitVector32.CreateMask(WebBrowserHelper.manualUpdate);

		// Token: 0x0400273D RID: 10045
		internal static readonly int addedSelectionHandler = BitVector32.CreateMask(WebBrowserHelper.setClientSiteFirst);

		// Token: 0x0400273E RID: 10046
		internal static readonly int siteProcessedInputKey = BitVector32.CreateMask(WebBrowserHelper.addedSelectionHandler);

		// Token: 0x0400273F RID: 10047
		internal static readonly int inTransition = BitVector32.CreateMask(WebBrowserHelper.siteProcessedInputKey);

		// Token: 0x04002740 RID: 10048
		internal static readonly int processingKeyUp = BitVector32.CreateMask(WebBrowserHelper.inTransition);

		// Token: 0x04002741 RID: 10049
		internal static readonly int isMaskEdit = BitVector32.CreateMask(WebBrowserHelper.processingKeyUp);

		// Token: 0x04002742 RID: 10050
		internal static readonly int recomputeContainingControl = BitVector32.CreateMask(WebBrowserHelper.isMaskEdit);

		// Token: 0x04002743 RID: 10051
		private static int logPixelsX = -1;

		// Token: 0x04002744 RID: 10052
		private static int logPixelsY = -1;

		// Token: 0x04002745 RID: 10053
		private const int HMperInch = 2540;

		// Token: 0x04002746 RID: 10054
		private static Guid ifont_Guid = typeof(UnsafeNativeMethods.IFont).GUID;

		// Token: 0x04002747 RID: 10055
		internal static Guid windowsMediaPlayer_Clsid = new Guid("{22d6f312-b0f6-11d0-94ab-0080c74c7e95}");

		// Token: 0x04002748 RID: 10056
		internal static Guid comctlImageCombo_Clsid = new Guid("{a98a24c0-b06f-3684-8c12-c52ae341e0bc}");

		// Token: 0x04002749 RID: 10057
		internal static Guid maskEdit_Clsid = new Guid("{c932ba85-4374-101b-a56c-00aa003668dc}");

		// Token: 0x0400274A RID: 10058
		internal static readonly int REGMSG_MSG = SafeNativeMethods.RegisterWindowMessage(Application.WindowMessagesVersion + "_subclassCheck");

		// Token: 0x0400274B RID: 10059
		internal const int REGMSG_RETVAL = 123;

		// Token: 0x02000800 RID: 2048
		internal enum AXState
		{
			// Token: 0x0400422D RID: 16941
			Passive,
			// Token: 0x0400422E RID: 16942
			Loaded,
			// Token: 0x0400422F RID: 16943
			Running,
			// Token: 0x04004230 RID: 16944
			InPlaceActive = 4,
			// Token: 0x04004231 RID: 16945
			UIActive = 8
		}

		// Token: 0x02000801 RID: 2049
		internal enum AXEditMode
		{
			// Token: 0x04004233 RID: 16947
			None,
			// Token: 0x04004234 RID: 16948
			Object,
			// Token: 0x04004235 RID: 16949
			Host
		}

		// Token: 0x02000802 RID: 2050
		internal enum SelectionStyle
		{
			// Token: 0x04004237 RID: 16951
			NotSelected,
			// Token: 0x04004238 RID: 16952
			Selected,
			// Token: 0x04004239 RID: 16953
			Active
		}
	}
}
