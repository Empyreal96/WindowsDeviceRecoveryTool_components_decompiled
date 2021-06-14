using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
	// Token: 0x02000307 RID: 775
	public class PaintEventArgs : EventArgs, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PaintEventArgs" /> class with the specified graphics and clipping rectangle.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the item. </param>
		/// <param name="clipRect">The <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle in which to paint. </param>
		// Token: 0x06002EF6 RID: 12022 RVA: 0x000D9C62 File Offset: 0x000D7E62
		public PaintEventArgs(Graphics graphics, Rectangle clipRect)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			this.graphics = graphics;
			this.clipRect = clipRect;
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000D9C9C File Offset: 0x000D7E9C
		internal PaintEventArgs(IntPtr dc, Rectangle clipRect)
		{
			this.dc = dc;
			this.clipRect = clipRect;
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x06002EF8 RID: 12024 RVA: 0x000D9CC8 File Offset: 0x000D7EC8
		~PaintEventArgs()
		{
			this.Dispose(false);
		}

		/// <summary>Gets the rectangle in which to paint.</summary>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> in which to paint.</returns>
		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06002EF9 RID: 12025 RVA: 0x000D9CF8 File Offset: 0x000D7EF8
		public Rectangle ClipRectangle
		{
			get
			{
				return this.clipRect;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x000D9D00 File Offset: 0x000D7F00
		internal IntPtr HDC
		{
			get
			{
				if (this.graphics == null)
				{
					return this.dc;
				}
				return IntPtr.Zero;
			}
		}

		/// <summary>Gets the graphics used to paint.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> object used to paint. The <see cref="T:System.Drawing.Graphics" /> object provides methods for drawing objects on the display device.</returns>
		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06002EFB RID: 12027 RVA: 0x000D9D18 File Offset: 0x000D7F18
		public Graphics Graphics
		{
			get
			{
				if (this.graphics == null && this.dc != IntPtr.Zero)
				{
					this.oldPal = Control.SetUpPalette(this.dc, false, false);
					this.graphics = Graphics.FromHdcInternal(this.dc);
					this.graphics.PageUnit = GraphicsUnit.Pixel;
					this.savedGraphicsState = this.graphics.Save();
				}
				return this.graphics;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.PaintEventArgs" />.</summary>
		// Token: 0x06002EFC RID: 12028 RVA: 0x000D9D86 File Offset: 0x000D7F86
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.PaintEventArgs" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06002EFD RID: 12029 RVA: 0x000D9D98 File Offset: 0x000D7F98
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.graphics != null && this.dc != IntPtr.Zero)
			{
				this.graphics.Dispose();
			}
			if (this.oldPal != IntPtr.Zero && this.dc != IntPtr.Zero)
			{
				SafeNativeMethods.SelectPalette(new HandleRef(this, this.dc), new HandleRef(this, this.oldPal), 0);
				this.oldPal = IntPtr.Zero;
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000D9E1B File Offset: 0x000D801B
		internal void ResetGraphics()
		{
			if (this.graphics != null && this.savedGraphicsState != null)
			{
				this.graphics.Restore(this.savedGraphicsState);
				this.savedGraphicsState = null;
			}
		}

		// Token: 0x04001D50 RID: 7504
		private Graphics graphics;

		// Token: 0x04001D51 RID: 7505
		private GraphicsState savedGraphicsState;

		// Token: 0x04001D52 RID: 7506
		private readonly IntPtr dc = IntPtr.Zero;

		// Token: 0x04001D53 RID: 7507
		private IntPtr oldPal = IntPtr.Zero;

		// Token: 0x04001D54 RID: 7508
		private readonly Rectangle clipRect;
	}
}
