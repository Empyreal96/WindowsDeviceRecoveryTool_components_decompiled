using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020003A0 RID: 928
	internal class CachedItemHdcInfo : IDisposable
	{
		// Token: 0x06003BEA RID: 15338 RVA: 0x001099F2 File Offset: 0x00107BF2
		internal CachedItemHdcInfo()
		{
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x00109A1C File Offset: 0x00107C1C
		~CachedItemHdcInfo()
		{
			this.Dispose();
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x00109A48 File Offset: 0x00107C48
		public HandleRef GetCachedItemDC(HandleRef toolStripHDC, Size bitmapSize)
		{
			if (this.cachedHDCSize.Width < bitmapSize.Width || this.cachedHDCSize.Height < bitmapSize.Height)
			{
				if (this.cachedItemHDC.Handle == IntPtr.Zero)
				{
					IntPtr handle = UnsafeNativeMethods.CreateCompatibleDC(toolStripHDC);
					this.cachedItemHDC = new HandleRef(this, handle);
				}
				this.cachedItemBitmap = new HandleRef(this, SafeNativeMethods.CreateCompatibleBitmap(toolStripHDC, bitmapSize.Width, bitmapSize.Height));
				IntPtr intPtr = SafeNativeMethods.SelectObject(this.cachedItemHDC, this.cachedItemBitmap);
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.ExternalDeleteObject(new HandleRef(null, intPtr));
					intPtr = IntPtr.Zero;
				}
				this.cachedHDCSize = bitmapSize;
			}
			return this.cachedItemHDC;
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x00109B0C File Offset: 0x00107D0C
		private void DeleteCachedItemHDC()
		{
			if (this.cachedItemHDC.Handle != IntPtr.Zero)
			{
				if (this.cachedItemBitmap.Handle != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(this.cachedItemBitmap);
					this.cachedItemBitmap = NativeMethods.NullHandleRef;
				}
				UnsafeNativeMethods.DeleteCompatibleDC(this.cachedItemHDC);
			}
			this.cachedItemHDC = NativeMethods.NullHandleRef;
			this.cachedItemBitmap = NativeMethods.NullHandleRef;
			this.cachedHDCSize = Size.Empty;
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x00109B8B File Offset: 0x00107D8B
		public void Dispose()
		{
			this.DeleteCachedItemHDC();
			GC.SuppressFinalize(this);
		}

		// Token: 0x04002385 RID: 9093
		private HandleRef cachedItemHDC = NativeMethods.NullHandleRef;

		// Token: 0x04002386 RID: 9094
		private Size cachedHDCSize = Size.Empty;

		// Token: 0x04002387 RID: 9095
		private HandleRef cachedItemBitmap = NativeMethods.NullHandleRef;
	}
}
