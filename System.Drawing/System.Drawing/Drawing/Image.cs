using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Drawing
{
	/// <summary>An abstract base class that provides functionality for the <see cref="T:System.Drawing.Bitmap" /> and <see cref="T:System.Drawing.Imaging.Metafile" /> descended classes.</summary>
	// Token: 0x02000020 RID: 32
	[TypeConverter(typeof(ImageConverter))]
	[Editor("System.Drawing.Design.ImageEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[ImmutableObject(true)]
	[ComVisible(true)]
	[Serializable]
	public abstract class Image : MarshalByRefObject, ISerializable, ICloneable, IDisposable
	{
		// Token: 0x0600031E RID: 798 RVA: 0x000037F8 File Offset: 0x000019F8
		internal Image()
		{
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000EB54 File Offset: 0x0000CD54
		internal Image(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			if (enumerator == null)
			{
				return;
			}
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Name, "Data", StringComparison.OrdinalIgnoreCase))
				{
					try
					{
						byte[] array = (byte[])enumerator.Value;
						if (array != null)
						{
							this.InitializeFromStream(new MemoryStream(array));
						}
					}
					catch (ExternalException ex)
					{
					}
					catch (ArgumentException ex2)
					{
					}
					catch (OutOfMemoryException ex3)
					{
					}
					catch (InvalidOperationException ex4)
					{
					}
					catch (NotImplementedException ex5)
					{
					}
					catch (FileNotFoundException ex6)
					{
					}
				}
			}
		}

		/// <summary>Gets or sets an object that provides additional data about the image.</summary>
		/// <returns>The <see cref="T:System.Object" /> that provides additional data about the image.</returns>
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000EC0C File Offset: 0x0000CE0C
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000EC14 File Offset: 0x0000CE14
		[Localizable(false)]
		[Bindable(true)]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified file.</summary>
		/// <param name="filename">A string that contains the name of the file from which to create the <see cref="T:System.Drawing.Image" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.OutOfMemoryException">The file does not have a valid image format.-or-
		///         GDI+ does not support the pixel format of the file.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="filename" /> is a <see cref="T:System.Uri" />.</exception>
		// Token: 0x06000322 RID: 802 RVA: 0x0000EC1D File Offset: 0x0000CE1D
		public static Image FromFile(string filename)
		{
			return Image.FromFile(filename, false);
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified file using embedded color management information in that file.</summary>
		/// <param name="filename">A string that contains the name of the file from which to create the <see cref="T:System.Drawing.Image" />. </param>
		/// <param name="useEmbeddedColorManagement">Set to <see langword="true" /> to use color management information embedded in the image file; otherwise, <see langword="false" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.OutOfMemoryException">The file does not have a valid image format.-or-
		///         GDI+ does not support the pixel format of the file.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="filename" /> is a <see cref="T:System.Uri" />.</exception>
		// Token: 0x06000323 RID: 803 RVA: 0x0000EC28 File Offset: 0x0000CE28
		public static Image FromFile(string filename, bool useEmbeddedColorManagement)
		{
			if (!File.Exists(filename))
			{
				IntSecurity.DemandReadFileIO(filename);
				throw new FileNotFoundException(filename);
			}
			filename = Path.GetFullPath(filename);
			IntPtr zero = IntPtr.Zero;
			int num;
			if (useEmbeddedColorManagement)
			{
				num = SafeNativeMethods.Gdip.GdipLoadImageFromFileICM(filename, out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipLoadImageFromFile(filename, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			Image image = Image.CreateImageObject(zero);
			Image.EnsureSave(image, filename, null);
			return image;
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified data stream.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Image" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">The stream does not have a valid image format-or-
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x06000324 RID: 804 RVA: 0x0000ECAD File Offset: 0x0000CEAD
		public static Image FromStream(Stream stream)
		{
			return Image.FromStream(stream, false);
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified data stream, optionally using embedded color management information in that stream.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Image" />. </param>
		/// <param name="useEmbeddedColorManagement">
		///       <see langword="true" /> to use color management information embedded in the data stream; otherwise, <see langword="false" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">The stream does not have a valid image format -or-
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x06000325 RID: 805 RVA: 0x0000ECB6 File Offset: 0x0000CEB6
		public static Image FromStream(Stream stream, bool useEmbeddedColorManagement)
		{
			return Image.FromStream(stream, useEmbeddedColorManagement, true);
		}

		/// <summary>Creates an <see cref="T:System.Drawing.Image" /> from the specified data stream, optionally using embedded color management information and validating the image data.</summary>
		/// <param name="stream">A <see cref="T:System.IO.Stream" /> that contains the data for this <see cref="T:System.Drawing.Image" />. </param>
		/// <param name="useEmbeddedColorManagement">
		///       <see langword="true" /> to use color management information embedded in the data stream; otherwise, <see langword="false" />. </param>
		/// <param name="validateImageData">
		///       <see langword="true" /> to validate the image data; otherwise, <see langword="false" />.</param>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">The stream does not have a valid image format.</exception>
		// Token: 0x06000326 RID: 806 RVA: 0x0000ECC0 File Offset: 0x0000CEC0
		public static Image FromStream(Stream stream, bool useEmbeddedColorManagement, bool validateImageData)
		{
			if (!validateImageData)
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"stream",
					"null"
				}));
			}
			IntPtr zero = IntPtr.Zero;
			int num;
			if (useEmbeddedColorManagement)
			{
				num = SafeNativeMethods.Gdip.GdipLoadImageFromStreamICM(new GPStream(stream), out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipLoadImageFromStream(new GPStream(stream), out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (validateImageData)
			{
				num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
				if (num != 0)
				{
					SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
			Image image = Image.CreateImageObject(zero);
			Image.EnsureSave(image, null, stream);
			return image;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000ED6C File Offset: 0x0000CF6C
		private void InitializeFromStream(Stream stream)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipLoadImageFromStream(new GPStream(stream), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeImage = zero;
			int num2 = -1;
			num = SafeNativeMethods.Gdip.GdipGetImageType(new HandleRef(this, this.nativeImage), out num2);
			Image.EnsureSave(this, null, stream);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000EDEA File Offset: 0x0000CFEA
		internal Image(IntPtr nativeImage)
		{
			this.SetNativeImage(nativeImage);
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> this method creates, cast as an object.</returns>
		// Token: 0x06000329 RID: 809 RVA: 0x0000EDFC File Offset: 0x0000CFFC
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneImage(new HandleRef(this, this.nativeImage), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Image.CreateImageObject(zero);
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Image" />.</summary>
		// Token: 0x0600032A RID: 810 RVA: 0x0000EE57 File Offset: 0x0000D057
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Drawing.Image" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x0600032B RID: 811 RVA: 0x0000EE68 File Offset: 0x0000D068
		protected virtual void Dispose(bool disposing)
		{
			if (this.nativeImage != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(this, this.nativeImage));
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
					this.nativeImage = IntPtr.Zero;
				}
			}
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x0600032C RID: 812 RVA: 0x0000EED0 File Offset: 0x0000D0D0
		~Image()
		{
			this.Dispose(false);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000EF00 File Offset: 0x0000D100
		internal static void EnsureSave(Image image, string filename, Stream dataStream)
		{
			if (image.RawFormat.Equals(ImageFormat.Gif))
			{
				bool flag = false;
				Guid[] frameDimensionsList = image.FrameDimensionsList;
				foreach (Guid guid in frameDimensionsList)
				{
					FrameDimension frameDimension = new FrameDimension(guid);
					if (frameDimension.Equals(FrameDimension.Time))
					{
						flag = (image.GetFrameCount(FrameDimension.Time) > 1);
						break;
					}
				}
				if (flag)
				{
					try
					{
						Stream stream = null;
						long position = 0L;
						if (dataStream != null)
						{
							position = dataStream.Position;
							dataStream.Position = 0L;
						}
						try
						{
							if (dataStream == null)
							{
								dataStream = (stream = File.OpenRead(filename));
							}
							image.rawData = new byte[(int)dataStream.Length];
							dataStream.Read(image.rawData, 0, (int)dataStream.Length);
						}
						finally
						{
							if (stream != null)
							{
								stream.Close();
							}
							else
							{
								dataStream.Position = position;
							}
						}
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (DirectoryNotFoundException)
					{
					}
					catch (IOException)
					{
					}
					catch (NotSupportedException)
					{
					}
					catch (ObjectDisposedException)
					{
					}
					catch (ArgumentException)
					{
					}
				}
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000F044 File Offset: 0x0000D244
		internal static Image CreateImageObject(IntPtr nativeImage)
		{
			int num = -1;
			int num2 = SafeNativeMethods.Gdip.GdipGetImageType(new HandleRef(null, nativeImage), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			Image.ImageTypeEnum imageTypeEnum = (Image.ImageTypeEnum)num;
			Image result;
			if (imageTypeEnum != Image.ImageTypeEnum.Bitmap)
			{
				if (imageTypeEnum != Image.ImageTypeEnum.Metafile)
				{
					throw new ArgumentException(SR.GetString("InvalidImage"));
				}
				result = Metafile.FromGDIplus(nativeImage);
			}
			else
			{
				result = Bitmap.FromGDIplus(nativeImage);
			}
			return result;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="si">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
		// Token: 0x0600032F RID: 815 RVA: 0x0000F09C File Offset: 0x0000D29C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.Save(memoryStream);
				si.AddValue("Data", memoryStream.ToArray(), typeof(byte[]));
			}
		}

		/// <summary>Returns information about the parameters supported by the specified image encoder.</summary>
		/// <param name="encoder">A GUID that specifies the image encoder. </param>
		/// <returns>An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> that contains an array of <see cref="T:System.Drawing.Imaging.EncoderParameter" /> objects. Each <see cref="T:System.Drawing.Imaging.EncoderParameter" /> contains information about one of the parameters supported by the specified image encoder.</returns>
		// Token: 0x06000330 RID: 816 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		public EncoderParameters GetEncoderParameterList(Guid encoder)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipGetEncoderParameterListSize(new HandleRef(this, this.nativeImage), ref encoder, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (num2 <= 0)
			{
				return null;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num2);
			num = SafeNativeMethods.Gdip.GdipGetEncoderParameterList(new HandleRef(this, this.nativeImage), ref encoder, num2, intPtr);
			EncoderParameters result;
			try
			{
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				result = EncoderParameters.ConvertFromMemory(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		/// <summary>Saves this <see cref="T:System.Drawing.Image" /> to the specified file or stream.</summary>
		/// <param name="filename">A string that contains the name of the file to which to save this <see cref="T:System.Drawing.Image" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="filename" /> is <see langword="null." /></exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
		// Token: 0x06000331 RID: 817 RVA: 0x0000F16C File Offset: 0x0000D36C
		public void Save(string filename)
		{
			this.Save(filename, this.RawFormat);
		}

		/// <summary>Saves this <see cref="T:System.Drawing.Image" /> to the specified file in the specified format.</summary>
		/// <param name="filename">A string that contains the name of the file to which to save this <see cref="T:System.Drawing.Image" />. </param>
		/// <param name="format">The <see cref="T:System.Drawing.Imaging.ImageFormat" /> for this <see cref="T:System.Drawing.Image" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="filename" /> or <paramref name="format" /> is <see langword="null." /></exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
		// Token: 0x06000332 RID: 818 RVA: 0x0000F17C File Offset: 0x0000D37C
		public void Save(string filename, ImageFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			ImageCodecInfo imageCodecInfo = format.FindEncoder();
			if (imageCodecInfo == null)
			{
				imageCodecInfo = ImageFormat.Png.FindEncoder();
			}
			this.Save(filename, imageCodecInfo, null);
		}

		/// <summary>Saves this <see cref="T:System.Drawing.Image" /> to the specified file, with the specified encoder and image-encoder parameters.</summary>
		/// <param name="filename">A string that contains the name of the file to which to save this <see cref="T:System.Drawing.Image" />. </param>
		/// <param name="encoder">The <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> for this <see cref="T:System.Drawing.Image" />. </param>
		/// <param name="encoderParams">An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> to use for this <see cref="T:System.Drawing.Image" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="filename" /> or <paramref name="encoder" /> is <see langword="null." /></exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.-or- The image was saved to the same file it was created from.</exception>
		// Token: 0x06000333 RID: 819 RVA: 0x0000F1B8 File Offset: 0x0000D3B8
		public void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (encoder == null)
			{
				throw new ArgumentNullException("encoder");
			}
			IntSecurity.DemandWriteFileIO(filename);
			IntPtr intPtr = IntPtr.Zero;
			if (encoderParams != null)
			{
				this.rawData = null;
				intPtr = encoderParams.ConvertToMemory();
			}
			int num = 0;
			try
			{
				Guid clsid = encoder.Clsid;
				bool flag = false;
				if (this.rawData != null)
				{
					ImageCodecInfo imageCodecInfo = this.RawFormat.FindEncoder();
					if (imageCodecInfo != null && imageCodecInfo.Clsid == clsid)
					{
						using (FileStream fileStream = File.OpenWrite(filename))
						{
							fileStream.Write(this.rawData, 0, this.rawData.Length);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					num = SafeNativeMethods.Gdip.GdipSaveImageToFile(new HandleRef(this, this.nativeImage), filename, ref clsid, new HandleRef(encoderParams, intPtr));
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000F2BC File Offset: 0x0000D4BC
		internal void Save(MemoryStream stream)
		{
			ImageFormat imageFormat = this.RawFormat;
			if (imageFormat == ImageFormat.Jpeg)
			{
				imageFormat = ImageFormat.Png;
			}
			ImageCodecInfo imageCodecInfo = imageFormat.FindEncoder();
			if (imageCodecInfo == null)
			{
				imageCodecInfo = ImageFormat.Png.FindEncoder();
			}
			this.Save(stream, imageCodecInfo, null);
		}

		/// <summary>Saves this image to the specified stream in the specified format.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> where the image will be saved. </param>
		/// <param name="format">An <see cref="T:System.Drawing.Imaging.ImageFormat" /> that specifies the format of the saved image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> or <paramref name="format" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format</exception>
		// Token: 0x06000335 RID: 821 RVA: 0x0000F2FC File Offset: 0x0000D4FC
		public void Save(Stream stream, ImageFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			ImageCodecInfo encoder = format.FindEncoder();
			this.Save(stream, encoder, null);
		}

		/// <summary>Saves this image to the specified stream, with the specified encoder and image encoder parameters.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> where the image will be saved. </param>
		/// <param name="encoder">The <see cref="T:System.Drawing.Imaging.ImageCodecInfo" /> for this <see cref="T:System.Drawing.Image" />.</param>
		/// <param name="encoderParams">An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> that specifies parameters used by the image encoder. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The image was saved with the wrong image format.</exception>
		// Token: 0x06000336 RID: 822 RVA: 0x0000F328 File Offset: 0x0000D528
		public void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (encoder == null)
			{
				throw new ArgumentNullException("encoder");
			}
			IntPtr intPtr = IntPtr.Zero;
			if (encoderParams != null)
			{
				this.rawData = null;
				intPtr = encoderParams.ConvertToMemory();
			}
			int num = 0;
			try
			{
				Guid clsid = encoder.Clsid;
				bool flag = false;
				if (this.rawData != null)
				{
					ImageCodecInfo imageCodecInfo = this.RawFormat.FindEncoder();
					if (imageCodecInfo != null && imageCodecInfo.Clsid == clsid)
					{
						stream.Write(this.rawData, 0, this.rawData.Length);
						flag = true;
					}
				}
				if (!flag)
				{
					num = SafeNativeMethods.Gdip.GdipSaveImageToStream(new HandleRef(this, this.nativeImage), new UnsafeNativeMethods.ComStreamFromDataStream(stream), ref clsid, new HandleRef(encoderParams, intPtr));
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a frame to the file or stream specified in a previous call to the <see cref="Overload:System.Drawing.Image.Save" /> method. Use this method to save selected frames from a multiple-frame image to another multiple-frame image.</summary>
		/// <param name="encoderParams">An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> that holds parameters required by the image encoder that is used by the save-add operation. </param>
		// Token: 0x06000337 RID: 823 RVA: 0x0000F408 File Offset: 0x0000D608
		public void SaveAdd(EncoderParameters encoderParams)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (encoderParams != null)
			{
				intPtr = encoderParams.ConvertToMemory();
			}
			this.rawData = null;
			int num = SafeNativeMethods.Gdip.GdipSaveAdd(new HandleRef(this, this.nativeImage), new HandleRef(encoderParams, intPtr));
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Adds a frame to the file or stream specified in a previous call to the <see cref="Overload:System.Drawing.Image.Save" /> method.</summary>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> that contains the frame to add. </param>
		/// <param name="encoderParams">An <see cref="T:System.Drawing.Imaging.EncoderParameters" /> that holds parameters required by the image encoder that is used by the save-add operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000338 RID: 824 RVA: 0x0000F464 File Offset: 0x0000D664
		public void SaveAdd(Image image, EncoderParameters encoderParams)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (encoderParams != null)
			{
				intPtr = encoderParams.ConvertToMemory();
			}
			this.rawData = null;
			int num = SafeNativeMethods.Gdip.GdipSaveAddImage(new HandleRef(this, this.nativeImage), new HandleRef(image, image.nativeImage), new HandleRef(encoderParams, intPtr));
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000F4D8 File Offset: 0x0000D6D8
		private SizeF _GetPhysicalDimension()
		{
			float width;
			float height;
			int num = SafeNativeMethods.Gdip.GdipGetImageDimension(new HandleRef(this, this.nativeImage), out width, out height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new SizeF(width, height);
		}

		/// <summary>Gets the width and height of this image.</summary>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> structure that represents the width and height of this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000F50C File Offset: 0x0000D70C
		public SizeF PhysicalDimension
		{
			get
			{
				return this._GetPhysicalDimension();
			}
		}

		/// <summary>Gets the width and height, in pixels, of this image.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> structure that represents the width and height, in pixels, of this image.</returns>
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000F514 File Offset: 0x0000D714
		public Size Size
		{
			get
			{
				return new Size(this.Width, this.Height);
			}
		}

		/// <summary>Gets the width, in pixels, of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The width, in pixels, of this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000F528 File Offset: 0x0000D728
		[DefaultValue(false)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Width
		{
			get
			{
				int result;
				int num = SafeNativeMethods.Gdip.GdipGetImageWidth(new HandleRef(this, this.nativeImage), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
		}

		/// <summary>Gets the height, in pixels, of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The height, in pixels, of this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000F554 File Offset: 0x0000D754
		[DefaultValue(false)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Height
		{
			get
			{
				int result;
				int num = SafeNativeMethods.Gdip.GdipGetImageHeight(new HandleRef(this, this.nativeImage), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
		}

		/// <summary>Gets the horizontal resolution, in pixels per inch, of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The horizontal resolution, in pixels per inch, of this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000F580 File Offset: 0x0000D780
		public float HorizontalResolution
		{
			get
			{
				float result;
				int num = SafeNativeMethods.Gdip.GdipGetImageHorizontalResolution(new HandleRef(this, this.nativeImage), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
		}

		/// <summary>Gets the vertical resolution, in pixels per inch, of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The vertical resolution, in pixels per inch, of this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000F5AC File Offset: 0x0000D7AC
		public float VerticalResolution
		{
			get
			{
				float result;
				int num = SafeNativeMethods.Gdip.GdipGetImageVerticalResolution(new HandleRef(this, this.nativeImage), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
		}

		/// <summary>Gets attribute flags for the pixel data of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The integer representing a bitwise combination of <see cref="T:System.Drawing.Imaging.ImageFlags" /> for this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000F5D8 File Offset: 0x0000D7D8
		[Browsable(false)]
		public int Flags
		{
			get
			{
				int result;
				int num = SafeNativeMethods.Gdip.GdipGetImageFlags(new HandleRef(this, this.nativeImage), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
		}

		/// <summary>Gets the file format of this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Imaging.ImageFormat" /> that represents the file format of this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000F604 File Offset: 0x0000D804
		public ImageFormat RawFormat
		{
			get
			{
				Guid guid = default(Guid);
				int num = SafeNativeMethods.Gdip.GdipGetImageRawFormat(new HandleRef(this, this.nativeImage), ref guid);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new ImageFormat(guid);
			}
		}

		/// <summary>Gets the pixel format for this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Imaging.PixelFormat" /> that represents the pixel format for this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000F640 File Offset: 0x0000D840
		public PixelFormat PixelFormat
		{
			get
			{
				int result;
				int num = SafeNativeMethods.Gdip.GdipGetImagePixelFormat(new HandleRef(this, this.nativeImage), out result);
				if (num != 0)
				{
					return PixelFormat.Undefined;
				}
				return (PixelFormat)result;
			}
		}

		/// <summary>Gets the bounds of the image in the specified unit.</summary>
		/// <param name="pageUnit">One of the <see cref="T:System.Drawing.GraphicsUnit" /> values indicating the unit of measure for the bounding rectangle.</param>
		/// <returns>The <see cref="T:System.Drawing.RectangleF" /> that represents the bounds of the image, in the specified unit.</returns>
		// Token: 0x06000343 RID: 835 RVA: 0x0000F668 File Offset: 0x0000D868
		public RectangleF GetBounds(ref GraphicsUnit pageUnit)
		{
			GPRECTF gprectf = default(GPRECTF);
			int num = SafeNativeMethods.Gdip.GdipGetImageBounds(new HandleRef(this, this.nativeImage), ref gprectf, out pageUnit);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf.ToRectangleF();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
		private ColorPalette _GetColorPalette()
		{
			int num = -1;
			int num2 = SafeNativeMethods.Gdip.GdipGetImagePaletteSize(new HandleRef(this, this.nativeImage), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			ColorPalette colorPalette = new ColorPalette(num);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			num2 = SafeNativeMethods.Gdip.GdipGetImagePalette(new HandleRef(this, this.nativeImage), intPtr, num);
			try
			{
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				colorPalette.ConvertFromMemory(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return colorPalette;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000F720 File Offset: 0x0000D920
		private void _SetColorPalette(ColorPalette palette)
		{
			IntPtr intPtr = palette.ConvertToMemory();
			int num = SafeNativeMethods.Gdip.GdipSetImagePalette(new HandleRef(this, this.nativeImage), intPtr);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets or sets the color palette used for this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Imaging.ColorPalette" /> that represents the color palette used for this <see cref="T:System.Drawing.Image" />.</returns>
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000F764 File Offset: 0x0000D964
		// (set) Token: 0x06000347 RID: 839 RVA: 0x0000F76C File Offset: 0x0000D96C
		[Browsable(false)]
		public ColorPalette Palette
		{
			get
			{
				return this._GetColorPalette();
			}
			set
			{
				this._SetColorPalette(value);
			}
		}

		/// <summary>Returns a thumbnail for this <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="thumbWidth">The width, in pixels, of the requested thumbnail image. </param>
		/// <param name="thumbHeight">The height, in pixels, of the requested thumbnail image. </param>
		/// <param name="callback">A <see cref="T:System.Drawing.Image.GetThumbnailImageAbort" /> delegate. 
		///       Note   You must create a delegate and pass a reference to the delegate as the <paramref name="callback" /> parameter, but the delegate is not used.</param>
		/// <param name="callbackData">Must be <see cref="F:System.IntPtr.Zero" />. </param>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that represents the thumbnail.</returns>
		// Token: 0x06000348 RID: 840 RVA: 0x0000F778 File Offset: 0x0000D978
		public Image GetThumbnailImage(int thumbWidth, int thumbHeight, Image.GetThumbnailImageAbort callback, IntPtr callbackData)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetImageThumbnail(new HandleRef(this, this.nativeImage), thumbWidth, thumbHeight, out zero, callback, callbackData);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Image.CreateImageObject(zero);
		}

		/// <summary>Gets an array of GUIDs that represent the dimensions of frames within this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>An array of GUIDs that specify the dimensions of frames within this <see cref="T:System.Drawing.Image" /> from most significant to least significant.</returns>
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
		[Browsable(false)]
		public Guid[] FrameDimensionsList
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipImageGetFrameDimensionsCount(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				if (num2 <= 0)
				{
					return new Guid[0];
				}
				int num3 = Marshal.SizeOf(typeof(Guid));
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num3 * num2));
				if (intPtr == IntPtr.Zero)
				{
					throw SafeNativeMethods.Gdip.StatusException(3);
				}
				num = SafeNativeMethods.Gdip.GdipImageGetFrameDimensionsList(new HandleRef(this, this.nativeImage), intPtr, num2);
				if (num != 0)
				{
					Marshal.FreeHGlobal(intPtr);
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				Guid[] array = new Guid[num2];
				try
				{
					for (int i = 0; i < num2; i++)
					{
						array[i] = (Guid)UnsafeNativeMethods.PtrToStructure((IntPtr)((long)intPtr + (long)(num3 * i)), typeof(Guid));
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return array;
			}
		}

		/// <summary>Returns the number of frames of the specified dimension.</summary>
		/// <param name="dimension">A <see cref="T:System.Drawing.Imaging.FrameDimension" /> that specifies the identity of the dimension type. </param>
		/// <returns>The number of frames in the specified dimension.</returns>
		// Token: 0x0600034A RID: 842 RVA: 0x0000F89C File Offset: 0x0000DA9C
		public int GetFrameCount(FrameDimension dimension)
		{
			int[] array = new int[1];
			Guid guid = dimension.Guid;
			int num = SafeNativeMethods.Gdip.GdipImageGetFrameCount(new HandleRef(this, this.nativeImage), ref guid, array);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return array[0];
		}

		/// <summary>Selects the frame specified by the dimension and index.</summary>
		/// <param name="dimension">A <see cref="T:System.Drawing.Imaging.FrameDimension" /> that specifies the identity of the dimension type. </param>
		/// <param name="frameIndex">The index of the active frame. </param>
		/// <returns>Always returns 0.</returns>
		// Token: 0x0600034B RID: 843 RVA: 0x0000F8DC File Offset: 0x0000DADC
		public int SelectActiveFrame(FrameDimension dimension, int frameIndex)
		{
			int[] array = new int[1];
			Guid guid = dimension.Guid;
			int num = SafeNativeMethods.Gdip.GdipImageSelectActiveFrame(new HandleRef(this, this.nativeImage), ref guid, frameIndex);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return array[0];
		}

		/// <summary>Rotates, flips, or rotates and flips the <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="rotateFlipType">A <see cref="T:System.Drawing.RotateFlipType" /> member that specifies the type of rotation and flip to apply to the image. </param>
		// Token: 0x0600034C RID: 844 RVA: 0x0000F91C File Offset: 0x0000DB1C
		public void RotateFlip(RotateFlipType rotateFlipType)
		{
			int num = SafeNativeMethods.Gdip.GdipImageRotateFlip(new HandleRef(this, this.nativeImage), (int)rotateFlipType);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets IDs of the property items stored in this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>An array of the property IDs, one for each property item stored in this image.</returns>
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000F948 File Offset: 0x0000DB48
		[Browsable(false)]
		public int[] PropertyIdList
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetPropertyCount(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int[] array = new int[num2];
				if (num2 == 0)
				{
					return array;
				}
				num = SafeNativeMethods.Gdip.GdipGetPropertyIdList(new HandleRef(this, this.nativeImage), num2, array);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array;
			}
		}

		/// <summary>Gets the specified property item from this <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="propid">The ID of the property item to get. </param>
		/// <returns>The <see cref="T:System.Drawing.Imaging.PropertyItem" /> this method gets.</returns>
		/// <exception cref="T:System.ArgumentException">The image format of this image does not support property items.</exception>
		// Token: 0x0600034E RID: 846 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
		public PropertyItem GetPropertyItem(int propid)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipGetPropertyItemSize(new HandleRef(this, this.nativeImage), propid, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (num2 == 0)
			{
				return null;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num2);
			if (intPtr == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			num = SafeNativeMethods.Gdip.GdipGetPropertyItem(new HandleRef(this, this.nativeImage), propid, num2, intPtr);
			PropertyItem result;
			try
			{
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				result = PropertyItemInternal.ConvertFromMemory(intPtr, 1)[0];
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		/// <summary>Removes the specified property item from this <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="propid">The ID of the property item to remove. </param>
		/// <exception cref="T:System.ArgumentException">The image does not contain the requested property item.-or-The image format for this image does not support property items.</exception>
		// Token: 0x0600034F RID: 847 RVA: 0x0000FA30 File Offset: 0x0000DC30
		public void RemovePropertyItem(int propid)
		{
			int num = SafeNativeMethods.Gdip.GdipRemovePropertyItem(new HandleRef(this, this.nativeImage), propid);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Stores a property item (piece of metadata) in this <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="propitem">The <see cref="T:System.Drawing.Imaging.PropertyItem" /> to be stored. </param>
		/// <exception cref="T:System.ArgumentException">The image format of this image does not support property items.</exception>
		// Token: 0x06000350 RID: 848 RVA: 0x0000FA5C File Offset: 0x0000DC5C
		public void SetPropertyItem(PropertyItem propitem)
		{
			PropertyItemInternal propertyItemInternal = PropertyItemInternal.ConvertFromPropertyItem(propitem);
			using (propertyItemInternal)
			{
				int num = SafeNativeMethods.Gdip.GdipSetPropertyItem(new HandleRef(this, this.nativeImage), propertyItemInternal);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets all the property items (pieces of metadata) stored in this <see cref="T:System.Drawing.Image" />.</summary>
		/// <returns>An array of <see cref="T:System.Drawing.Imaging.PropertyItem" /> objects, one for each property item stored in the image.</returns>
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		[Browsable(false)]
		public PropertyItem[] PropertyItems
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetPropertyCount(new HandleRef(this, this.nativeImage), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				int num3;
				num = SafeNativeMethods.Gdip.GdipGetPropertySize(new HandleRef(this, this.nativeImage), out num3, ref num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				if (num3 == 0 || num2 == 0)
				{
					return new PropertyItem[0];
				}
				IntPtr intPtr = Marshal.AllocHGlobal(num3);
				num = SafeNativeMethods.Gdip.GdipGetAllPropertyItems(new HandleRef(this, this.nativeImage), num3, num2, intPtr);
				PropertyItem[] result = null;
				try
				{
					if (num != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num);
					}
					result = PropertyItemInternal.ConvertFromMemory(intPtr, num2);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return result;
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000FB54 File Offset: 0x0000DD54
		internal void SetNativeImage(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("NativeHandle0"), "handle");
			}
			this.nativeImage = handle;
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Bitmap" /> from a handle to a GDI bitmap.</summary>
		/// <param name="hbitmap">The GDI bitmap handle from which to create the <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Bitmap" /> this method creates.</returns>
		// Token: 0x06000353 RID: 851 RVA: 0x0000FB7F File Offset: 0x0000DD7F
		public static Bitmap FromHbitmap(IntPtr hbitmap)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			return Image.FromHbitmap(hbitmap, IntPtr.Zero);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Bitmap" /> from a handle to a GDI bitmap and a handle to a GDI palette.</summary>
		/// <param name="hbitmap">The GDI bitmap handle from which to create the <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="hpalette">A handle to a GDI palette used to define the bitmap colors if the bitmap specified in the <paramref name="hBitmap" /> parameter is not a device-independent bitmap (DIB). </param>
		/// <returns>The <see cref="T:System.Drawing.Bitmap" /> this method creates.</returns>
		// Token: 0x06000354 RID: 852 RVA: 0x0000FB98 File Offset: 0x0000DD98
		public static Bitmap FromHbitmap(IntPtr hbitmap, IntPtr hpalette)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromHBITMAP(new HandleRef(null, hbitmap), new HandleRef(null, hpalette), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(zero);
		}

		/// <summary>Returns the color depth, in number of bits per pixel, of the specified pixel format.</summary>
		/// <param name="pixfmt">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> member that specifies the format for which to find the size. </param>
		/// <returns>The color depth of the specified pixel format.</returns>
		// Token: 0x06000355 RID: 853 RVA: 0x0000FBDB File Offset: 0x0000DDDB
		public static int GetPixelFormatSize(PixelFormat pixfmt)
		{
			return (int)(pixfmt >> 8 & (PixelFormat)255);
		}

		/// <summary>Returns a value that indicates whether the pixel format for this <see cref="T:System.Drawing.Image" /> contains alpha information.</summary>
		/// <param name="pixfmt">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> to test. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="pixfmt" /> contains alpha information; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000356 RID: 854 RVA: 0x0000FBE6 File Offset: 0x0000DDE6
		public static bool IsAlphaPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Alpha) > PixelFormat.Undefined;
		}

		/// <summary>Returns a value that indicates whether the pixel format is 64 bits per pixel.</summary>
		/// <param name="pixfmt">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> enumeration to test. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="pixfmt" /> is extended; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000357 RID: 855 RVA: 0x0000FBF2 File Offset: 0x0000DDF2
		public static bool IsExtendedPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Extended) > PixelFormat.Undefined;
		}

		/// <summary>Returns a value that indicates whether the pixel format is 32 bits per pixel.</summary>
		/// <param name="pixfmt">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> to test. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="pixfmt" /> is canonical; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000358 RID: 856 RVA: 0x0000FBFE File Offset: 0x0000DDFE
		public static bool IsCanonicalPixelFormat(PixelFormat pixfmt)
		{
			return (pixfmt & PixelFormat.Canonical) > PixelFormat.Undefined;
		}

		// Token: 0x0400018E RID: 398
		internal IntPtr nativeImage;

		// Token: 0x0400018F RID: 399
		private byte[] rawData;

		// Token: 0x04000190 RID: 400
		private object userData;

		/// <summary>Provides a callback method for determining when the <see cref="M:System.Drawing.Image.GetThumbnailImage(System.Int32,System.Int32,System.Drawing.Image.GetThumbnailImageAbort,System.IntPtr)" /> method should prematurely cancel execution.</summary>
		/// <returns>This method returns <see langword="true" /> if it decides that the <see cref="M:System.Drawing.Image.GetThumbnailImage(System.Int32,System.Int32,System.Drawing.Image.GetThumbnailImageAbort,System.IntPtr)" /> method should prematurely stop execution; otherwise, it returns <see langword="false" />.</returns>
		// Token: 0x020000FA RID: 250
		// (Invoke) Token: 0x06000CA8 RID: 3240
		public delegate bool GetThumbnailImageAbort();

		// Token: 0x020000FB RID: 251
		private enum ImageTypeEnum
		{
			// Token: 0x04000AED RID: 2797
			Bitmap = 1,
			// Token: 0x04000AEE RID: 2798
			Metafile
		}
	}
}
