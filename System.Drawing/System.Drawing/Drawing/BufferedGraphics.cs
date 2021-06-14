using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	/// <summary>Provides a graphics buffer for double buffering.</summary>
	// Token: 0x02000014 RID: 20
	public sealed class BufferedGraphics : IDisposable
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x0000630B File Offset: 0x0000450B
		internal BufferedGraphics(Graphics bufferedGraphicsSurface, BufferedGraphicsContext context, Graphics targetGraphics, IntPtr targetDC, Point targetLoc, Size virtualSize)
		{
			this.context = context;
			this.bufferedGraphicsSurface = bufferedGraphicsSurface;
			this.targetDC = targetDC;
			this.targetGraphics = targetGraphics;
			this.targetLoc = targetLoc;
			this.virtualSize = virtualSize;
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x060000F3 RID: 243 RVA: 0x00006340 File Offset: 0x00004540
		~BufferedGraphics()
		{
			this.Dispose(false);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Drawing.BufferedGraphics" /> object.</summary>
		// Token: 0x060000F4 RID: 244 RVA: 0x00006370 File Offset: 0x00004570
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000637C File Offset: 0x0000457C
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.context != null)
				{
					this.context.ReleaseBuffer(this);
					if (this.DisposeContext)
					{
						this.context.Dispose();
						this.context = null;
					}
				}
				if (this.bufferedGraphicsSurface != null)
				{
					this.bufferedGraphicsSurface.Dispose();
					this.bufferedGraphicsSurface = null;
				}
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000063D4 File Offset: 0x000045D4
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x000063DC File Offset: 0x000045DC
		internal bool DisposeContext
		{
			get
			{
				return this.disposeContext;
			}
			set
			{
				this.disposeContext = value;
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Graphics" /> object that outputs to the graphics buffer.</summary>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> object that outputs to the graphics buffer.</returns>
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000063E5 File Offset: 0x000045E5
		public Graphics Graphics
		{
			get
			{
				return this.bufferedGraphicsSurface;
			}
		}

		/// <summary>Writes the contents of the graphics buffer to the default device.</summary>
		// Token: 0x060000F9 RID: 249 RVA: 0x000063ED File Offset: 0x000045ED
		public void Render()
		{
			if (this.targetGraphics != null)
			{
				this.Render(this.targetGraphics);
				return;
			}
			this.RenderInternal(new HandleRef(this.Graphics, this.targetDC), this);
		}

		/// <summary>Writes the contents of the graphics buffer to the specified <see cref="T:System.Drawing.Graphics" /> object.</summary>
		/// <param name="target">A <see cref="T:System.Drawing.Graphics" /> object to which to write the contents of the graphics buffer. </param>
		// Token: 0x060000FA RID: 250 RVA: 0x0000641C File Offset: 0x0000461C
		public void Render(Graphics target)
		{
			if (target != null)
			{
				IntPtr hdc = target.GetHdc();
				try
				{
					this.RenderInternal(new HandleRef(target, hdc), this);
				}
				finally
				{
					target.ReleaseHdcInternal(hdc);
				}
			}
		}

		/// <summary>Writes the contents of the graphics buffer to the device context associated with the specified <see cref="T:System.IntPtr" /> handle.</summary>
		/// <param name="targetDC">An <see cref="T:System.IntPtr" /> that points to the device context to which to write the contents of the graphics buffer. </param>
		// Token: 0x060000FB RID: 251 RVA: 0x0000645C File Offset: 0x0000465C
		public void Render(IntPtr targetDC)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.RenderInternal(new HandleRef(null, targetDC), this);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006478 File Offset: 0x00004678
		private void RenderInternal(HandleRef refTargetDC, BufferedGraphics buffer)
		{
			IntPtr hdc = buffer.Graphics.GetHdc();
			try
			{
				SafeNativeMethods.BitBlt(refTargetDC, this.targetLoc.X, this.targetLoc.Y, this.virtualSize.Width, this.virtualSize.Height, new HandleRef(buffer.Graphics, hdc), 0, 0, BufferedGraphics.rop);
			}
			finally
			{
				buffer.Graphics.ReleaseHdcInternal(hdc);
			}
		}

		// Token: 0x0400012C RID: 300
		private Graphics bufferedGraphicsSurface;

		// Token: 0x0400012D RID: 301
		private Graphics targetGraphics;

		// Token: 0x0400012E RID: 302
		private BufferedGraphicsContext context;

		// Token: 0x0400012F RID: 303
		private IntPtr targetDC;

		// Token: 0x04000130 RID: 304
		private Point targetLoc;

		// Token: 0x04000131 RID: 305
		private Size virtualSize;

		// Token: 0x04000132 RID: 306
		private bool disposeContext;

		// Token: 0x04000133 RID: 307
		private static int rop = 13369376;
	}
}
