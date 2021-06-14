using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Drawing.Imaging
{
	/// <summary>The <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> class provides the necessary storage members and methods to retrieve all pertinent information about the installed image encoders and decoders (called codecs). Not inheritable. </summary>
	// Token: 0x020000A0 RID: 160
	public sealed class ImageCodecInfo
	{
		// Token: 0x06000986 RID: 2438 RVA: 0x00003800 File Offset: 0x00001A00
		internal ImageCodecInfo()
		{
		}

		/// <summary>Gets or sets a <see cref="T:System.Guid" /> structure that contains a GUID that identifies a specific codec.</summary>
		/// <returns>A <see cref="T:System.Guid" /> structure that contains a GUID that identifies a specific codec.</returns>
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x00024354 File Offset: 0x00022554
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x0002435C File Offset: 0x0002255C
		public Guid Clsid
		{
			get
			{
				return this.clsid;
			}
			set
			{
				this.clsid = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Guid" /> structure that contains a GUID that identifies the codec's format.</summary>
		/// <returns>A <see cref="T:System.Guid" /> structure that contains a GUID that identifies the codec's format.</returns>
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00024365 File Offset: 0x00022565
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0002436D File Offset: 0x0002256D
		public Guid FormatID
		{
			get
			{
				return this.formatID;
			}
			set
			{
				this.formatID = value;
			}
		}

		/// <summary>Gets or sets a string that contains the name of the codec.</summary>
		/// <returns>A string that contains the name of the codec.</returns>
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00024376 File Offset: 0x00022576
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0002437E File Offset: 0x0002257E
		public string CodecName
		{
			get
			{
				return this.codecName;
			}
			set
			{
				this.codecName = value;
			}
		}

		/// <summary>Gets or sets string that contains the path name of the DLL that holds the codec. If the codec is not in a DLL, this pointer is <see langword="null" />.</summary>
		/// <returns>A string that contains the path name of the DLL that holds the codec.</returns>
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x00024387 File Offset: 0x00022587
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x000243A8 File Offset: 0x000225A8
		public string DllName
		{
			get
			{
				if (this.dllName != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.dllName).Demand();
				}
				return this.dllName;
			}
			set
			{
				if (value != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, value).Demand();
				}
				this.dllName = value;
			}
		}

		/// <summary>Gets or sets a string that describes the codec's file format.</summary>
		/// <returns>A string that describes the codec's file format.</returns>
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x000243C0 File Offset: 0x000225C0
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x000243C8 File Offset: 0x000225C8
		public string FormatDescription
		{
			get
			{
				return this.formatDescription;
			}
			set
			{
				this.formatDescription = value;
			}
		}

		/// <summary>Gets or sets string that contains the file name extension(s) used in the codec. The extensions are separated by semicolons.</summary>
		/// <returns>A string that contains the file name extension(s) used in the codec.</returns>
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x000243D1 File Offset: 0x000225D1
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x000243D9 File Offset: 0x000225D9
		public string FilenameExtension
		{
			get
			{
				return this.filenameExtension;
			}
			set
			{
				this.filenameExtension = value;
			}
		}

		/// <summary>Gets or sets a string that contains the codec's Multipurpose Internet Mail Extensions (MIME) type.</summary>
		/// <returns>A string that contains the codec's Multipurpose Internet Mail Extensions (MIME) type.</returns>
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x000243E2 File Offset: 0x000225E2
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x000243EA File Offset: 0x000225EA
		public string MimeType
		{
			get
			{
				return this.mimeType;
			}
			set
			{
				this.mimeType = value;
			}
		}

		/// <summary>Gets or sets 32-bit value used to store additional information about the codec. This property returns a combination of flags from the <see cref="T:System.Drawing.Imaging.ImageCodecFlags" /> enumeration.</summary>
		/// <returns>A 32-bit value used to store additional information about the codec.</returns>
		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x000243F3 File Offset: 0x000225F3
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x000243FB File Offset: 0x000225FB
		public ImageCodecFlags Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		/// <summary>Gets or sets the version number of the codec.</summary>
		/// <returns>The version number of the codec.</returns>
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x00024404 File Offset: 0x00022604
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0002440C File Offset: 0x0002260C
		public int Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		/// <summary>Gets or sets a two dimensional array of bytes that represents the signature of the codec.</summary>
		/// <returns>A two dimensional array of bytes that represents the signature of the codec.</returns>
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x00024415 File Offset: 0x00022615
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0002441D File Offset: 0x0002261D
		[CLSCompliant(false)]
		public byte[][] SignaturePatterns
		{
			get
			{
				return this.signaturePatterns;
			}
			set
			{
				this.signaturePatterns = value;
			}
		}

		/// <summary>Gets or sets a two dimensional array of bytes that can be used as a filter.</summary>
		/// <returns>A two dimensional array of bytes that can be used as a filter.</returns>
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x00024426 File Offset: 0x00022626
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x0002442E File Offset: 0x0002262E
		[CLSCompliant(false)]
		public byte[][] SignatureMasks
		{
			get
			{
				return this.signatureMasks;
			}
			set
			{
				this.signatureMasks = value;
			}
		}

		/// <summary>Returns an array of <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> objects that contain information about the image decoders built into GDI+.</summary>
		/// <returns>An array of <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> objects. Each <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> object in the array contains information about one of the built-in image decoders.</returns>
		// Token: 0x0600099D RID: 2461 RVA: 0x00024438 File Offset: 0x00022638
		public static ImageCodecInfo[] GetImageDecoders()
		{
			int num2;
			int num3;
			int num = SafeNativeMethods.Gdip.GdipGetImageDecodersSize(out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num3);
			ImageCodecInfo[] result;
			try
			{
				num = SafeNativeMethods.Gdip.GdipGetImageDecoders(num2, num3, intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				result = ImageCodecInfo.ConvertFromMemory(intPtr, num2);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		/// <summary>Returns an array of <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> objects that contain information about the image encoders built into GDI+.</summary>
		/// <returns>An array of <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> objects. Each <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> object in the array contains information about one of the built-in image encoders.</returns>
		// Token: 0x0600099E RID: 2462 RVA: 0x0002449C File Offset: 0x0002269C
		public static ImageCodecInfo[] GetImageEncoders()
		{
			int num2;
			int num3;
			int num = SafeNativeMethods.Gdip.GdipGetImageEncodersSize(out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num3);
			ImageCodecInfo[] result;
			try
			{
				num = SafeNativeMethods.Gdip.GdipGetImageEncoders(num2, num3, intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				result = ImageCodecInfo.ConvertFromMemory(intPtr, num2);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00024500 File Offset: 0x00022700
		private static ImageCodecInfo[] ConvertFromMemory(IntPtr memoryStart, int numCodecs)
		{
			ImageCodecInfo[] array = new ImageCodecInfo[numCodecs];
			for (int i = 0; i < numCodecs; i++)
			{
				IntPtr lparam = (IntPtr)((long)memoryStart + (long)(Marshal.SizeOf(typeof(ImageCodecInfoPrivate)) * i));
				ImageCodecInfoPrivate imageCodecInfoPrivate = new ImageCodecInfoPrivate();
				UnsafeNativeMethods.PtrToStructure(lparam, imageCodecInfoPrivate);
				array[i] = new ImageCodecInfo();
				array[i].Clsid = imageCodecInfoPrivate.Clsid;
				array[i].FormatID = imageCodecInfoPrivate.FormatID;
				array[i].CodecName = Marshal.PtrToStringUni(imageCodecInfoPrivate.CodecName);
				array[i].DllName = Marshal.PtrToStringUni(imageCodecInfoPrivate.DllName);
				array[i].FormatDescription = Marshal.PtrToStringUni(imageCodecInfoPrivate.FormatDescription);
				array[i].FilenameExtension = Marshal.PtrToStringUni(imageCodecInfoPrivate.FilenameExtension);
				array[i].MimeType = Marshal.PtrToStringUni(imageCodecInfoPrivate.MimeType);
				array[i].Flags = (ImageCodecFlags)imageCodecInfoPrivate.Flags;
				array[i].Version = imageCodecInfoPrivate.Version;
				array[i].SignaturePatterns = new byte[imageCodecInfoPrivate.SigCount][];
				array[i].SignatureMasks = new byte[imageCodecInfoPrivate.SigCount][];
				for (int j = 0; j < imageCodecInfoPrivate.SigCount; j++)
				{
					array[i].SignaturePatterns[j] = new byte[imageCodecInfoPrivate.SigSize];
					array[i].SignatureMasks[j] = new byte[imageCodecInfoPrivate.SigSize];
					Marshal.Copy((IntPtr)((long)imageCodecInfoPrivate.SigMask + (long)(j * imageCodecInfoPrivate.SigSize)), array[i].SignatureMasks[j], 0, imageCodecInfoPrivate.SigSize);
					Marshal.Copy((IntPtr)((long)imageCodecInfoPrivate.SigPattern + (long)(j * imageCodecInfoPrivate.SigSize)), array[i].SignaturePatterns[j], 0, imageCodecInfoPrivate.SigSize);
				}
			}
			return array;
		}

		// Token: 0x040008B4 RID: 2228
		private Guid clsid;

		// Token: 0x040008B5 RID: 2229
		private Guid formatID;

		// Token: 0x040008B6 RID: 2230
		private string codecName;

		// Token: 0x040008B7 RID: 2231
		private string dllName;

		// Token: 0x040008B8 RID: 2232
		private string formatDescription;

		// Token: 0x040008B9 RID: 2233
		private string filenameExtension;

		// Token: 0x040008BA RID: 2234
		private string mimeType;

		// Token: 0x040008BB RID: 2235
		private ImageCodecFlags flags;

		// Token: 0x040008BC RID: 2236
		private int version;

		// Token: 0x040008BD RID: 2237
		private byte[][] signaturePatterns;

		// Token: 0x040008BE RID: 2238
		private byte[][] signatureMasks;
	}
}
