using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	/// <summary>Contains attributes of an associated <see cref="T:System.Drawing.Imaging.Metafile" />. Not inheritable.</summary>
	// Token: 0x020000A7 RID: 167
	[StructLayout(LayoutKind.Sequential)]
	public sealed class MetafileHeader
	{
		// Token: 0x060009E3 RID: 2531 RVA: 0x00003800 File Offset: 0x00001A00
		internal MetafileHeader()
		{
		}

		/// <summary>Gets the type of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Imaging.MetafileType" /> enumeration that represents the type of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00025303 File Offset: 0x00023503
		public MetafileType Type
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.type;
				}
				return this.wmf.type;
			}
		}

		/// <summary>Gets the size, in bytes, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>The size, in bytes, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00025324 File Offset: 0x00023524
		public int MetafileSize
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.size;
				}
				return this.wmf.size;
			}
		}

		/// <summary>Gets the version number of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>The version number of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x00025345 File Offset: 0x00023545
		public int Version
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.version;
				}
				return this.wmf.version;
			}
		}

		/// <summary>Gets the horizontal resolution, in dots per inch, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>The horizontal resolution, in dots per inch, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00025366 File Offset: 0x00023566
		public float DpiX
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.dpiX;
				}
				return this.wmf.dpiX;
			}
		}

		/// <summary>Gets the vertical resolution, in dots per inch, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>The vertical resolution, in dots per inch, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x00025387 File Offset: 0x00023587
		public float DpiY
		{
			get
			{
				if (!this.IsWmf())
				{
					return this.emf.dpiY;
				}
				return this.wmf.dpiY;
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Rectangle" /> that bounds the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that bounds the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x000253A8 File Offset: 0x000235A8
		public Rectangle Bounds
		{
			get
			{
				if (!this.IsWmf())
				{
					return new Rectangle(this.emf.X, this.emf.Y, this.emf.Width, this.emf.Height);
				}
				return new Rectangle(this.wmf.X, this.wmf.Y, this.wmf.Width, this.wmf.Height);
			}
		}

		/// <summary>Returns a value that indicates whether the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows metafile format.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows metafile format; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009EA RID: 2538 RVA: 0x00025420 File Offset: 0x00023620
		public bool IsWmf()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.wmf != null && (this.wmf.type == MetafileType.Wmf || this.wmf.type == MetafileType.WmfPlaceable);
		}

		/// <summary>Returns a value that indicates whether the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows placeable metafile format.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows placeable metafile format; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009EB RID: 2539 RVA: 0x00025460 File Offset: 0x00023660
		public bool IsWmfPlaceable()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.wmf != null && this.wmf.type == MetafileType.WmfPlaceable;
		}

		/// <summary>Returns a value that indicates whether the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows enhanced metafile format.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows enhanced metafile format; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009EC RID: 2540 RVA: 0x00025491 File Offset: 0x00023691
		public bool IsEmf()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type == MetafileType.Emf;
		}

		/// <summary>Returns a value that indicates whether the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows enhanced metafile format or the Windows enhanced metafile plus format.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows enhanced metafile format or the Windows enhanced metafile plus format; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009ED RID: 2541 RVA: 0x000254C2 File Offset: 0x000236C2
		public bool IsEmfOrEmfPlus()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type >= MetafileType.Emf;
		}

		/// <summary>Returns a value that indicates whether the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows enhanced metafile plus format.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Windows enhanced metafile plus format; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009EE RID: 2542 RVA: 0x000254F6 File Offset: 0x000236F6
		public bool IsEmfPlus()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type >= MetafileType.EmfPlusOnly;
		}

		/// <summary>Returns a value that indicates whether the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Dual enhanced metafile format. This format supports both the enhanced and the enhanced plus format.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is in the Dual enhanced metafile format; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009EF RID: 2543 RVA: 0x0002552A File Offset: 0x0002372A
		public bool IsEmfPlusDual()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type == MetafileType.EmfPlusDual;
		}

		/// <summary>Returns a value that indicates whether the associated <see cref="T:System.Drawing.Imaging.Metafile" /> supports only the Windows enhanced metafile plus format.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated <see cref="T:System.Drawing.Imaging.Metafile" /> supports only the Windows enhanced metafile plus format; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009F0 RID: 2544 RVA: 0x0002555B File Offset: 0x0002375B
		public bool IsEmfPlusOnly()
		{
			if (this.wmf == null && this.emf == null)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			return this.emf != null && this.emf.type == MetafileType.EmfPlusOnly;
		}

		/// <summary>Returns a value that indicates whether the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is device dependent.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated <see cref="T:System.Drawing.Imaging.Metafile" /> is device dependent; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009F1 RID: 2545 RVA: 0x0002558C File Offset: 0x0002378C
		public bool IsDisplay()
		{
			return this.IsEmfPlus() && (this.emf.emfPlusFlags & EmfPlusFlags.Display) > (EmfPlusFlags)0;
		}

		/// <summary>Gets the Windows metafile (WMF) header file for the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Imaging.MetaHeader" /> that contains the WMF header file for the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x000255A8 File Offset: 0x000237A8
		public MetaHeader WmfHeader
		{
			get
			{
				if (this.wmf == null)
				{
					throw SafeNativeMethods.Gdip.StatusException(2);
				}
				return this.wmf.WmfHeader;
			}
		}

		/// <summary>Gets the size, in bytes, of the enhanced metafile plus header file.</summary>
		/// <returns>The size, in bytes, of the enhanced metafile plus header file.</returns>
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x000255C4 File Offset: 0x000237C4
		public int EmfPlusHeaderSize
		{
			get
			{
				if (this.wmf == null && this.emf == null)
				{
					throw SafeNativeMethods.Gdip.StatusException(2);
				}
				if (!this.IsWmf())
				{
					return this.emf.EmfPlusHeaderSize;
				}
				return this.wmf.EmfPlusHeaderSize;
			}
		}

		/// <summary>Gets the logical horizontal resolution, in dots per inch, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>The logical horizontal resolution, in dots per inch, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x000255FC File Offset: 0x000237FC
		public int LogicalDpiX
		{
			get
			{
				if (this.wmf == null && this.emf == null)
				{
					throw SafeNativeMethods.Gdip.StatusException(2);
				}
				if (!this.IsWmf())
				{
					return this.emf.LogicalDpiX;
				}
				return this.wmf.LogicalDpiX;
			}
		}

		/// <summary>Gets the logical vertical resolution, in dots per inch, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>The logical vertical resolution, in dots per inch, of the associated <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x00025634 File Offset: 0x00023834
		public int LogicalDpiY
		{
			get
			{
				if (this.wmf == null && this.emf == null)
				{
					throw SafeNativeMethods.Gdip.StatusException(2);
				}
				if (!this.IsWmf())
				{
					return this.emf.LogicalDpiX;
				}
				return this.wmf.LogicalDpiY;
			}
		}

		// Token: 0x040008F4 RID: 2292
		internal MetafileHeaderWmf wmf;

		// Token: 0x040008F5 RID: 2293
		internal MetafileHeaderEmf emf;
	}
}
