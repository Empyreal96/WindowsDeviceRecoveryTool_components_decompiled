using System;
using System.Drawing;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004F7 RID: 1271
	internal abstract class WindowsBrush : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x060053A4 RID: 21412
		public abstract object Clone();

		// Token: 0x060053A5 RID: 21413
		protected abstract void CreateBrush();

		// Token: 0x060053A6 RID: 21414 RVA: 0x0015D83A File Offset: 0x0015BA3A
		public WindowsBrush(DeviceContext dc)
		{
			this.dc = dc;
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x0015D854 File Offset: 0x0015BA54
		public WindowsBrush(DeviceContext dc, Color color)
		{
			this.dc = dc;
			this.color = color;
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x0015D878 File Offset: 0x0015BA78
		~WindowsBrush()
		{
			this.Dispose(false);
		}

		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x060053A9 RID: 21417 RVA: 0x0015D8A8 File Offset: 0x0015BAA8
		protected DeviceContext DC
		{
			get
			{
				return this.dc;
			}
		}

		// Token: 0x060053AA RID: 21418 RVA: 0x0015D8B0 File Offset: 0x0015BAB0
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060053AB RID: 21419 RVA: 0x0015D8BC File Offset: 0x0015BABC
		protected virtual void Dispose(bool disposing)
		{
			if (this.dc != null && this.nativeHandle != IntPtr.Zero)
			{
				this.dc.DeleteObject(this.nativeHandle, GdiObjectType.Brush);
				this.nativeHandle = IntPtr.Zero;
			}
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x0015D909 File Offset: 0x0015BB09
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x0015D911 File Offset: 0x0015BB11
		// (set) Token: 0x060053AE RID: 21422 RVA: 0x0015D931 File Offset: 0x0015BB31
		protected IntPtr NativeHandle
		{
			get
			{
				if (this.nativeHandle == IntPtr.Zero)
				{
					this.CreateBrush();
				}
				return this.nativeHandle;
			}
			set
			{
				this.nativeHandle = value;
			}
		}

		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x060053AF RID: 21423 RVA: 0x0015D93A File Offset: 0x0015BB3A
		public IntPtr HBrush
		{
			get
			{
				return this.NativeHandle;
			}
		}

		// Token: 0x040035EB RID: 13803
		private DeviceContext dc;

		// Token: 0x040035EC RID: 13804
		private IntPtr nativeHandle;

		// Token: 0x040035ED RID: 13805
		private Color color = Color.White;
	}
}
