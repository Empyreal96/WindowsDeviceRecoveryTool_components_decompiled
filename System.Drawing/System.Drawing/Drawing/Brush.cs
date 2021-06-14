using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	/// <summary>Defines objects used to fill the interiors of graphical shapes such as rectangles, ellipses, pies, polygons, and paths.</summary>
	// Token: 0x02000012 RID: 18
	public abstract class Brush : MarshalByRefObject, ICloneable, IDisposable
	{
		/// <summary>When overridden in a derived class, creates an exact copy of this <see cref="T:System.Drawing.Brush" />.</summary>
		/// <returns>The new <see cref="T:System.Drawing.Brush" /> that this method creates.</returns>
		// Token: 0x0600005B RID: 91
		public abstract object Clone();

		/// <summary>In a derived class, sets a reference to a GDI+ brush object. </summary>
		/// <param name="brush">A pointer to the GDI+ brush object.</param>
		// Token: 0x0600005C RID: 92 RVA: 0x0000372C File Offset: 0x0000192C
		protected internal void SetNativeBrush(IntPtr brush)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.SetNativeBrushInternal(brush);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000373F File Offset: 0x0000193F
		internal void SetNativeBrushInternal(IntPtr brush)
		{
			this.nativeBrush = brush;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003748 File Offset: 0x00001948
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal IntPtr NativeBrush
		{
			get
			{
				return this.nativeBrush;
			}
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Brush" /> object.</summary>
		// Token: 0x0600005F RID: 95 RVA: 0x00003750 File Offset: 0x00001950
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Drawing.Brush" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06000060 RID: 96 RVA: 0x00003760 File Offset: 0x00001960
		protected virtual void Dispose(bool disposing)
		{
			if (this.nativeBrush != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeleteBrush(new HandleRef(this, this.nativeBrush));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativeBrush = IntPtr.Zero;
				}
			}
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x06000061 RID: 97 RVA: 0x000037C8 File Offset: 0x000019C8
		~Brush()
		{
			this.Dispose(false);
		}

		// Token: 0x0400009E RID: 158
		private IntPtr nativeBrush;
	}
}
