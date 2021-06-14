using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Drawing.Imaging
{
	/// <summary>Defines a graphic metafile. A metafile contains records that describe a sequence of graphics operations that can be recorded (constructed) and played back (displayed). This class is not inheritable.</summary>
	// Token: 0x020000A5 RID: 165
	[Editor("System.Drawing.Design.MetafileEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Serializable]
	public sealed class Metafile : Image
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified handle and a <see cref="T:System.Drawing.Imaging.WmfPlaceableFileHeader" />.</summary>
		/// <param name="hmetafile">A windows handle to a <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="wmfHeader">A <see cref="T:System.Drawing.Imaging.WmfPlaceableFileHeader" />. </param>
		// Token: 0x060009B2 RID: 2482 RVA: 0x000249B9 File Offset: 0x00022BB9
		public Metafile(IntPtr hmetafile, WmfPlaceableFileHeader wmfHeader) : this(hmetafile, wmfHeader, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified handle and a <see cref="T:System.Drawing.Imaging.WmfPlaceableFileHeader" />. Also, the <paramref name="deleteWmf" /> parameter can be used to delete the handle when the metafile is deleted.</summary>
		/// <param name="hmetafile">A windows handle to a <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="wmfHeader">A <see cref="T:System.Drawing.Imaging.WmfPlaceableFileHeader" />. </param>
		/// <param name="deleteWmf">
		///       <see langword="true" /> to delete the handle to the new <see cref="T:System.Drawing.Imaging.Metafile" /> when the <see cref="T:System.Drawing.Imaging.Metafile" /> is deleted; otherwise, <see langword="false" />. </param>
		// Token: 0x060009B3 RID: 2483 RVA: 0x000249C4 File Offset: 0x00022BC4
		public Metafile(IntPtr hmetafile, WmfPlaceableFileHeader wmfHeader, bool deleteWmf)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMetafileFromWmf(new HandleRef(null, hmetafile), deleteWmf, wmfHeader, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified handle.</summary>
		/// <param name="henhmetafile">A handle to an enhanced metafile. </param>
		/// <param name="deleteEmf">
		///       <see langword="true" /> to delete the enhanced metafile handle when the <see cref="T:System.Drawing.Imaging.Metafile" /> is deleted; otherwise, <see langword="false" />. </param>
		// Token: 0x060009B4 RID: 2484 RVA: 0x00024A0C File Offset: 0x00022C0C
		public Metafile(IntPtr henhmetafile, bool deleteEmf)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMetafileFromEmf(new HandleRef(null, henhmetafile), deleteEmf, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified file name.</summary>
		/// <param name="filename">A <see cref="T:System.String" /> that represents the file name from which to create the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009B5 RID: 2485 RVA: 0x00024A50 File Offset: 0x00022C50
		public Metafile(string filename)
		{
			IntSecurity.DemandReadFileIO(filename);
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMetafileFromFile(filename, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> from which to create the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x060009B6 RID: 2486 RVA: 0x00024A8C File Offset: 0x00022C8C
		public Metafile(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"stream",
					"null"
				}));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMetafileFromStream(new GPStream(stream), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified handle to a device context and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="emfType">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009B7 RID: 2487 RVA: 0x00024AED File Offset: 0x00022CED
		public Metafile(IntPtr referenceHdc, EmfType emfType) : this(referenceHdc, emfType, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified handle to a device context and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. A string can be supplied to name the file.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="emfType">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="description">A descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009B8 RID: 2488 RVA: 0x00024AF8 File Offset: 0x00022CF8
		public Metafile(IntPtr referenceHdc, EmfType emfType, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipRecordMetafile(new HandleRef(null, referenceHdc), (int)emfType, NativeMethods.NullHandleRef, 7, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified device context, bounded by the specified rectangle.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009B9 RID: 2489 RVA: 0x00024B43 File Offset: 0x00022D43
		public Metafile(IntPtr referenceHdc, RectangleF frameRect) : this(referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified device context, bounded by the specified rectangle that uses the supplied unit of measure.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		// Token: 0x060009BA RID: 2490 RVA: 0x00024B4E File Offset: 0x00022D4E
		public Metafile(IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit) : this(referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified device context, bounded by the specified rectangle that uses the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009BB RID: 2491 RVA: 0x00024B5A File Offset: 0x00022D5A
		public Metafile(IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type) : this(referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified device context, bounded by the specified rectangle that uses the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. A string can be provided to name the file.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="description">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009BC RID: 2492 RVA: 0x00024B68 File Offset: 0x00022D68
		public Metafile(IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = new GPRECTF(frameRect);
			int num = SafeNativeMethods.Gdip.GdipRecordMetafile(new HandleRef(null, referenceHdc), (int)type, ref gprectf, (int)frameUnit, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified device context, bounded by the specified rectangle.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009BD RID: 2493 RVA: 0x00024BBA File Offset: 0x00022DBA
		public Metafile(IntPtr referenceHdc, Rectangle frameRect) : this(referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified device context, bounded by the specified rectangle that uses the supplied unit of measure.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		// Token: 0x060009BE RID: 2494 RVA: 0x00024BC5 File Offset: 0x00022DC5
		public Metafile(IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit) : this(referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified device context, bounded by the specified rectangle that uses the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009BF RID: 2495 RVA: 0x00024BD1 File Offset: 0x00022DD1
		public Metafile(IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type) : this(referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified device context, bounded by the specified rectangle that uses the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. A string can be provided to name the file.</summary>
		/// <param name="referenceHdc">The handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="desc">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009C0 RID: 2496 RVA: 0x00024BE0 File Offset: 0x00022DE0
		public Metafile(IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string desc)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num;
			if (frameRect.IsEmpty)
			{
				num = SafeNativeMethods.Gdip.GdipRecordMetafile(new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, 7, desc, out zero);
			}
			else
			{
				GPRECT gprect = new GPRECT(frameRect);
				num = SafeNativeMethods.Gdip.GdipRecordMetafileI(new HandleRef(null, referenceHdc), (int)type, ref gprect, (int)frameUnit, desc, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		// Token: 0x060009C1 RID: 2497 RVA: 0x00024C56 File Offset: 0x00022E56
		public Metafile(string fileName, IntPtr referenceHdc) : this(fileName, referenceHdc, EmfType.EmfPlusDual, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009C2 RID: 2498 RVA: 0x00024C62 File Offset: 0x00022E62
		public Metafile(string fileName, IntPtr referenceHdc, EmfType type) : this(fileName, referenceHdc, type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. A descriptive string can be added, as well.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="description">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009C3 RID: 2499 RVA: 0x00024C70 File Offset: 0x00022E70
		public Metafile(string fileName, IntPtr referenceHdc, EmfType type, string description)
		{
			IntSecurity.DemandReadFileIO(fileName);
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipRecordMetafileFileName(fileName, new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, 7, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, and a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009C4 RID: 2500 RVA: 0x00024CC3 File Offset: 0x00022EC3
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect) : this(fileName, referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, and the supplied unit of measure.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		// Token: 0x060009C5 RID: 2501 RVA: 0x00024CCF File Offset: 0x00022ECF
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit) : this(fileName, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009C6 RID: 2502 RVA: 0x00024CDD File Offset: 0x00022EDD
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type) : this(fileName, referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, and the supplied unit of measure. A descriptive string can also be added.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="desc">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009C7 RID: 2503 RVA: 0x00024CED File Offset: 0x00022EED
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, string desc) : this(fileName, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual, desc)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. A descriptive string can also be added.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="description">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009C8 RID: 2504 RVA: 0x00024D00 File Offset: 0x00022F00
		public Metafile(string fileName, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.DemandReadFileIO(fileName);
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = new GPRECTF(frameRect);
			int num = SafeNativeMethods.Gdip.GdipRecordMetafileFileName(fileName, new HandleRef(null, referenceHdc), (int)type, ref gprectf, (int)frameUnit, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, and a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009C9 RID: 2505 RVA: 0x00024D5A File Offset: 0x00022F5A
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect) : this(fileName, referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, and the supplied unit of measure.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		// Token: 0x060009CA RID: 2506 RVA: 0x00024D66 File Offset: 0x00022F66
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit) : this(fileName, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009CB RID: 2507 RVA: 0x00024D74 File Offset: 0x00022F74
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type) : this(fileName, referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, and the supplied unit of measure. A descriptive string can also be added.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="description">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009CC RID: 2508 RVA: 0x00024D84 File Offset: 0x00022F84
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, string description) : this(fileName, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual, description)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class with the specified file name, a Windows handle to a device context, a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. A descriptive string can also be added.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that represents the file name of the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="description">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009CD RID: 2509 RVA: 0x00024D94 File Offset: 0x00022F94
		public Metafile(string fileName, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.DemandReadFileIO(fileName);
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num;
			if (frameRect.IsEmpty)
			{
				num = SafeNativeMethods.Gdip.GdipRecordMetafileFileName(fileName, new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, (int)frameUnit, description, out zero);
			}
			else
			{
				GPRECT gprect = new GPRECT(frameRect);
				num = SafeNativeMethods.Gdip.GdipRecordMetafileFileNameI(fileName, new HandleRef(null, referenceHdc), (int)type, ref gprect, (int)frameUnit, description, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		// Token: 0x060009CE RID: 2510 RVA: 0x00024E14 File Offset: 0x00023014
		public Metafile(Stream stream, IntPtr referenceHdc) : this(stream, referenceHdc, EmfType.EmfPlusDual, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009CF RID: 2511 RVA: 0x00024E20 File Offset: 0x00023020
		public Metafile(Stream stream, IntPtr referenceHdc, EmfType type) : this(stream, referenceHdc, type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. Also, a string that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" /> can be added.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="description">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009D0 RID: 2512 RVA: 0x00024E2C File Offset: 0x0002302C
		public Metafile(Stream stream, IntPtr referenceHdc, EmfType type, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipRecordMetafileStream(new GPStream(stream), new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, 7, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, and a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009D1 RID: 2513 RVA: 0x00024E7E File Offset: 0x0002307E
		public Metafile(Stream stream, IntPtr referenceHdc, RectangleF frameRect) : this(stream, referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, and the supplied unit of measure.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		// Token: 0x060009D2 RID: 2514 RVA: 0x00024E8A File Offset: 0x0002308A
		public Metafile(Stream stream, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit) : this(stream, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009D3 RID: 2515 RVA: 0x00024E98 File Offset: 0x00023098
		public Metafile(Stream stream, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type) : this(stream, referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, a <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. A string that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" /> can be added.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.RectangleF" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="description">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009D4 RID: 2516 RVA: 0x00024EA8 File Offset: 0x000230A8
		public Metafile(Stream stream, IntPtr referenceHdc, RectangleF frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = new GPRECTF(frameRect);
			int num = SafeNativeMethods.Gdip.GdipRecordMetafileStream(new GPStream(stream), new HandleRef(null, referenceHdc), (int)type, ref gprectf, (int)frameUnit, description, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, and a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009D5 RID: 2517 RVA: 0x00024F01 File Offset: 0x00023101
		public Metafile(Stream stream, IntPtr referenceHdc, Rectangle frameRect) : this(stream, referenceHdc, frameRect, MetafileFrameUnit.GdiCompatible)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, and the supplied unit of measure.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		// Token: 0x060009D6 RID: 2518 RVA: 0x00024F0D File Offset: 0x0002310D
		public Metafile(Stream stream, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit) : this(stream, referenceHdc, frameRect, frameUnit, EmfType.EmfPlusDual)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009D7 RID: 2519 RVA: 0x00024F1B File Offset: 0x0002311B
		public Metafile(Stream stream, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type) : this(stream, referenceHdc, frameRect, frameUnit, type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Metafile" /> class from the specified data stream, a Windows handle to a device context, a <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />, the supplied unit of measure, and an <see cref="T:System.Drawing.Imaging.EmfType" /> enumeration that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. A string that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" /> can be added.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="referenceHdc">A Windows handle to a device context. </param>
		/// <param name="frameRect">A <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle that bounds the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="frameUnit">A <see cref="T:System.Drawing.Imaging.MetafileFrameUnit" /> that specifies the unit of measure for <paramref name="frameRect" />. </param>
		/// <param name="type">An <see cref="T:System.Drawing.Imaging.EmfType" /> that specifies the format of the <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		/// <param name="description">A <see cref="T:System.String" /> that contains a descriptive name for the new <see cref="T:System.Drawing.Imaging.Metafile" />. </param>
		// Token: 0x060009D8 RID: 2520 RVA: 0x00024F2C File Offset: 0x0002312C
		public Metafile(Stream stream, IntPtr referenceHdc, Rectangle frameRect, MetafileFrameUnit frameUnit, EmfType type, string description)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num;
			if (frameRect.IsEmpty)
			{
				num = SafeNativeMethods.Gdip.GdipRecordMetafileStream(new GPStream(stream), new HandleRef(null, referenceHdc), (int)type, NativeMethods.NullHandleRef, (int)frameUnit, description, out zero);
			}
			else
			{
				GPRECT gprect = new GPRECT(frameRect);
				num = SafeNativeMethods.Gdip.GdipRecordMetafileStreamI(new GPStream(stream), new HandleRef(null, referenceHdc), (int)type, ref gprect, (int)frameUnit, description, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00002EB4 File Offset: 0x000010B4
		private Metafile(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Returns the <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with the specified <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="hmetafile">The handle to the <see cref="T:System.Drawing.Imaging.Metafile" /> for which to return a header. </param>
		/// <param name="wmfHeader">A <see cref="T:System.Drawing.Imaging.WmfPlaceableFileHeader" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with the specified <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x060009DA RID: 2522 RVA: 0x00024FB0 File Offset: 0x000231B0
		public static MetafileHeader GetMetafileHeader(IntPtr hmetafile, WmfPlaceableFileHeader wmfHeader)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			MetafileHeader metafileHeader = new MetafileHeader();
			metafileHeader.wmf = new MetafileHeaderWmf();
			int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromWmf(new HandleRef(null, hmetafile), wmfHeader, metafileHeader.wmf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return metafileHeader;
		}

		/// <summary>Returns the <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with the specified <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="henhmetafile">The handle to the enhanced <see cref="T:System.Drawing.Imaging.Metafile" /> for which a header is returned. </param>
		/// <returns>The <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with the specified <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x060009DB RID: 2523 RVA: 0x00024FF8 File Offset: 0x000231F8
		public static MetafileHeader GetMetafileHeader(IntPtr henhmetafile)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			MetafileHeader metafileHeader = new MetafileHeader();
			metafileHeader.emf = new MetafileHeaderEmf();
			int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromEmf(new HandleRef(null, henhmetafile), metafileHeader.emf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return metafileHeader;
		}

		/// <summary>Returns the <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with the specified <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> containing the name of the <see cref="T:System.Drawing.Imaging.Metafile" /> for which a header is retrieved. </param>
		/// <returns>The <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with the specified <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x060009DC RID: 2524 RVA: 0x00025040 File Offset: 0x00023240
		public static MetafileHeader GetMetafileHeader(string fileName)
		{
			IntSecurity.DemandReadFileIO(fileName);
			MetafileHeader metafileHeader = new MetafileHeader();
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MetafileHeaderEmf)));
			try
			{
				int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromFile(fileName, intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int[] array = new int[1];
				Marshal.Copy(intPtr, array, 0, 1);
				MetafileType metafileType = (MetafileType)array[0];
				if (metafileType == MetafileType.Wmf || metafileType == MetafileType.WmfPlaceable)
				{
					metafileHeader.wmf = (MetafileHeaderWmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderWmf));
					metafileHeader.emf = null;
				}
				else
				{
					metafileHeader.wmf = null;
					metafileHeader.emf = (MetafileHeaderEmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderEmf));
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return metafileHeader;
		}

		/// <summary>Returns the <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with the specified <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> containing the <see cref="T:System.Drawing.Imaging.Metafile" /> for which a header is retrieved. </param>
		/// <returns>The <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with the specified <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x060009DD RID: 2525 RVA: 0x00025100 File Offset: 0x00023300
		public static MetafileHeader GetMetafileHeader(Stream stream)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MetafileHeaderEmf)));
			MetafileHeader metafileHeader;
			try
			{
				int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromStream(new GPStream(stream), intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int[] array = new int[1];
				Marshal.Copy(intPtr, array, 0, 1);
				MetafileType metafileType = (MetafileType)array[0];
				metafileHeader = new MetafileHeader();
				if (metafileType == MetafileType.Wmf || metafileType == MetafileType.WmfPlaceable)
				{
					metafileHeader.wmf = (MetafileHeaderWmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderWmf));
					metafileHeader.emf = null;
				}
				else
				{
					metafileHeader.wmf = null;
					metafileHeader.emf = (MetafileHeaderEmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderEmf));
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return metafileHeader;
		}

		/// <summary>Returns the <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with this <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Imaging.MetafileHeader" /> associated with this <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x060009DE RID: 2526 RVA: 0x000251C0 File Offset: 0x000233C0
		public MetafileHeader GetMetafileHeader()
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MetafileHeaderEmf)));
			MetafileHeader metafileHeader;
			try
			{
				int num = SafeNativeMethods.Gdip.GdipGetMetafileHeaderFromMetafile(new HandleRef(this, this.nativeImage), intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int[] array = new int[1];
				Marshal.Copy(intPtr, array, 0, 1);
				MetafileType metafileType = (MetafileType)array[0];
				metafileHeader = new MetafileHeader();
				if (metafileType == MetafileType.Wmf || metafileType == MetafileType.WmfPlaceable)
				{
					metafileHeader.wmf = (MetafileHeaderWmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderWmf));
					metafileHeader.emf = null;
				}
				else
				{
					metafileHeader.wmf = null;
					metafileHeader.emf = (MetafileHeaderEmf)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(MetafileHeaderEmf));
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return metafileHeader;
		}

		/// <summary>Returns a Windows handle to an enhanced <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <returns>A Windows handle to this enhanced <see cref="T:System.Drawing.Imaging.Metafile" />.</returns>
		// Token: 0x060009DF RID: 2527 RVA: 0x00025284 File Offset: 0x00023484
		public IntPtr GetHenhmetafile()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetHemfFromMetafile(new HandleRef(this, this.nativeImage), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		/// <summary>Plays an individual metafile record.</summary>
		/// <param name="recordType">Element of the <see cref="T:System.Drawing.Imaging.EmfPlusRecordType" /> that specifies the type of metafile record being played. </param>
		/// <param name="flags">A set of flags that specify attributes of the record. </param>
		/// <param name="dataSize">The number of bytes in the record data. </param>
		/// <param name="data">An array of bytes that contains the record data. </param>
		// Token: 0x060009E0 RID: 2528 RVA: 0x000252B8 File Offset: 0x000234B8
		public void PlayRecord(EmfPlusRecordType recordType, int flags, int dataSize, byte[] data)
		{
			int num = SafeNativeMethods.Gdip.GdipPlayMetafileRecord(new HandleRef(this, this.nativeImage), recordType, flags, dataSize, data);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x000252E8 File Offset: 0x000234E8
		internal static Metafile FromGDIplus(IntPtr nativeImage)
		{
			Metafile metafile = new Metafile();
			metafile.SetNativeImage(nativeImage);
			return metafile;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0000301B File Offset: 0x0000121B
		private Metafile()
		{
		}
	}
}
