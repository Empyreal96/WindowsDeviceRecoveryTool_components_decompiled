using System;
using System.Drawing;
using System.Internal;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004FE RID: 1278
	internal sealed class WindowsRegion : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x0600540B RID: 21515 RVA: 0x0001640F File Offset: 0x0001460F
		private WindowsRegion()
		{
		}

		// Token: 0x0600540C RID: 21516 RVA: 0x0015F02F File Offset: 0x0015D22F
		public WindowsRegion(Rectangle rect)
		{
			this.CreateRegion(rect);
		}

		// Token: 0x0600540D RID: 21517 RVA: 0x0015F03E File Offset: 0x0015D23E
		public WindowsRegion(int x, int y, int width, int height)
		{
			this.CreateRegion(new Rectangle(x, y, width, height));
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x0015F058 File Offset: 0x0015D258
		public static WindowsRegion FromHregion(IntPtr hRegion, bool takeOwnership)
		{
			WindowsRegion windowsRegion = new WindowsRegion();
			if (hRegion != IntPtr.Zero)
			{
				windowsRegion.nativeHandle = hRegion;
				if (takeOwnership)
				{
					windowsRegion.ownHandle = true;
					System.Internal.HandleCollector.Add(hRegion, IntSafeNativeMethods.CommonHandles.GDI);
				}
			}
			return windowsRegion;
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x0015F096 File Offset: 0x0015D296
		public static WindowsRegion FromRegion(Region region, Graphics g)
		{
			if (region.IsInfinite(g))
			{
				return new WindowsRegion();
			}
			return WindowsRegion.FromHregion(region.GetHrgn(g), true);
		}

		// Token: 0x06005410 RID: 21520 RVA: 0x0015F0B4 File Offset: 0x0015D2B4
		public object Clone()
		{
			if (!this.IsInfinite)
			{
				return new WindowsRegion(this.ToRectangle());
			}
			return new WindowsRegion();
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x0015F0CF File Offset: 0x0015D2CF
		public IntNativeMethods.RegionFlags CombineRegion(WindowsRegion region1, WindowsRegion region2, RegionCombineMode mode)
		{
			return IntUnsafeNativeMethods.CombineRgn(new HandleRef(this, this.HRegion), new HandleRef(region1, region1.HRegion), new HandleRef(region2, region2.HRegion), mode);
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x0015F0FB File Offset: 0x0015D2FB
		private void CreateRegion(Rectangle rect)
		{
			this.nativeHandle = IntSafeNativeMethods.CreateRectRgn(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
			this.ownHandle = true;
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x0015F13B File Offset: 0x0015D33B
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x0015F144 File Offset: 0x0015D344
		public void Dispose(bool disposing)
		{
			if (this.nativeHandle != IntPtr.Zero)
			{
				if (this.ownHandle)
				{
					IntUnsafeNativeMethods.DeleteObject(new HandleRef(this, this.nativeHandle));
				}
				this.nativeHandle = IntPtr.Zero;
				if (disposing)
				{
					GC.SuppressFinalize(this);
				}
			}
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x0015F194 File Offset: 0x0015D394
		~WindowsRegion()
		{
			this.Dispose(false);
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06005416 RID: 21526 RVA: 0x0015F1C4 File Offset: 0x0015D3C4
		public IntPtr HRegion
		{
			get
			{
				return this.nativeHandle;
			}
		}

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x06005417 RID: 21527 RVA: 0x0015F1CC File Offset: 0x0015D3CC
		public bool IsInfinite
		{
			get
			{
				return this.nativeHandle == IntPtr.Zero;
			}
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x0015F1E0 File Offset: 0x0015D3E0
		public Rectangle ToRectangle()
		{
			if (this.IsInfinite)
			{
				return new Rectangle(-2147483647, -2147483647, int.MaxValue, int.MaxValue);
			}
			IntNativeMethods.RECT rect = default(IntNativeMethods.RECT);
			IntUnsafeNativeMethods.GetRgnBox(new HandleRef(this, this.nativeHandle), ref rect);
			return new Rectangle(new Point(rect.left, rect.top), rect.Size);
		}

		// Token: 0x0400362A RID: 13866
		private IntPtr nativeHandle;

		// Token: 0x0400362B RID: 13867
		private bool ownHandle;
	}
}
