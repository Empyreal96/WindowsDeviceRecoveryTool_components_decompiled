using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Security;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000432 RID: 1074
	internal class WpfPayload
	{
		// Token: 0x06003F1D RID: 16157 RVA: 0x001205FB File Offset: 0x0011E7FB
		private WpfPayload(Package package)
		{
			this._package = package;
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0012060A File Offset: 0x0011E80A
		internal static string SaveRange(ITextRange range, ref Stream stream, bool useFlowDocumentAsRoot)
		{
			return WpfPayload.SaveRange(range, ref stream, useFlowDocumentAsRoot, false);
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x00120618 File Offset: 0x0011E818
		internal static string SaveRange(ITextRange range, ref Stream stream, bool useFlowDocumentAsRoot, bool preserveTextElements)
		{
			if (range == null)
			{
				throw new ArgumentNullException("range");
			}
			WpfPayload wpfPayload = new WpfPayload(null);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
			TextRangeSerialization.WriteXaml(xmlWriter, range, useFlowDocumentAsRoot, wpfPayload, preserveTextElements);
			string text = stringWriter.ToString();
			if (stream != null || wpfPayload._images != null)
			{
				if (stream == null)
				{
					stream = new MemoryStream();
				}
				using (wpfPayload.CreatePackage(stream))
				{
					PackagePart packagePart = wpfPayload.CreateWpfEntryPart();
					Stream stream2 = packagePart.GetStream();
					using (stream2)
					{
						StreamWriter streamWriter = new StreamWriter(stream2);
						using (streamWriter)
						{
							streamWriter.Write(text);
						}
					}
					wpfPayload.CreateComponentParts(packagePart);
				}
				Invariant.Assert(wpfPayload._images == null);
			}
			return text;
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x00120710 File Offset: 0x0011E910
		[SecurityCritical]
		internal static MemoryStream SaveImage(BitmapSource bitmapSource, string imageContentType)
		{
			MemoryStream memoryStream = new MemoryStream();
			WpfPayload wpfPayload = new WpfPayload(null);
			using (wpfPayload.CreatePackage(memoryStream))
			{
				int imageIndex = 0;
				string imageReference = WpfPayload.GetImageReference(WpfPayload.GetImageName(imageIndex, imageContentType));
				PackagePart packagePart = wpfPayload.CreateWpfEntryPart();
				Stream stream = packagePart.GetStream();
				using (stream)
				{
					StreamWriter streamWriter = new StreamWriter(stream);
					using (streamWriter)
					{
						string value = string.Concat(new object[]
						{
							"<Span xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><InlineUIContainer><Image Width=\"",
							bitmapSource.Width,
							"\" Height=\"",
							bitmapSource.Height,
							"\" ><Image.Source><BitmapImage CacheOption=\"OnLoad\" UriSource=\"",
							imageReference,
							"\"/></Image.Source></Image></InlineUIContainer></Span>"
						});
						streamWriter.Write(value);
					}
				}
				wpfPayload.CreateImagePart(packagePart, bitmapSource, imageContentType, imageIndex);
			}
			return memoryStream;
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x00120818 File Offset: 0x0011EA18
		internal static object LoadElement(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			object result;
			try
			{
				WpfPayload wpfPayload = WpfPayload.OpenWpfPayload(stream);
				using (wpfPayload.Package)
				{
					PackagePart packagePart = wpfPayload.ValidatePayload();
					int num = Interlocked.Increment(ref WpfPayload._wpfPayloadCount);
					Uri packageUri = new Uri("payload://wpf" + num, UriKind.Absolute);
					Uri uri = PackUriHelper.Create(packageUri, packagePart.Uri);
					Uri packageUri2 = PackUriHelper.GetPackageUri(uri);
					PackageStore.AddPackage(packageUri2, wpfPayload.Package);
					ParserContext parserContext = new ParserContext();
					parserContext.BaseUri = uri;
					bool useRestrictiveXamlReader = !Clipboard.UseLegacyDangerousClipboardDeserializationMode();
					result = XamlReader.Load(packagePart.GetStream(), parserContext, useRestrictiveXamlReader);
					PackageStore.RemovePackage(packageUri2);
				}
			}
			catch (XamlParseException ex)
			{
				Invariant.Assert(ex != null);
				result = null;
			}
			catch (FileFormatException)
			{
				result = null;
			}
			catch (FileLoadException)
			{
				result = null;
			}
			catch (OutOfMemoryException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x00120934 File Offset: 0x0011EB34
		private PackagePart ValidatePayload()
		{
			PackagePart wpfEntryPart = this.GetWpfEntryPart();
			if (wpfEntryPart == null)
			{
				throw new XamlParseException(SR.Get("TextEditorCopyPaste_EntryPartIsMissingInXamlPackage"));
			}
			return wpfEntryPart;
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x0012095C File Offset: 0x0011EB5C
		public Package Package
		{
			get
			{
				return this._package;
			}
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x00120964 File Offset: 0x0011EB64
		private BitmapSource GetBitmapSourceFromImage(Image image)
		{
			if (image.Source is BitmapSource)
			{
				return (BitmapSource)image.Source;
			}
			Invariant.Assert(image.Source is DrawingImage);
			DpiScale dpi = image.GetDpi();
			DrawingImage drawingImage = (DrawingImage)image.Source;
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)(drawingImage.Width * dpi.DpiScaleX), (int)(drawingImage.Height * dpi.DpiScaleY), 96.0, 96.0, PixelFormats.Default);
			renderTargetBitmap.Render(image);
			return renderTargetBitmap;
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x001209F4 File Offset: 0x0011EBF4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void CreateComponentParts(PackagePart sourcePart)
		{
			if (this._images != null)
			{
				for (int i = 0; i < this._images.Count; i++)
				{
					Image image = this._images[i];
					string imageContentType = WpfPayload.GetImageContentType(image.Source.ToString());
					this.CreateImagePart(sourcePart, this.GetBitmapSourceFromImage(image), imageContentType, i);
				}
				this._images = null;
			}
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x00120A54 File Offset: 0x0011EC54
		[SecurityCritical]
		private void CreateImagePart(PackagePart sourcePart, BitmapSource imageSource, string imageContentType, int imageIndex)
		{
			string imageName = WpfPayload.GetImageName(imageIndex, imageContentType);
			Uri uri = new Uri("/Xaml" + imageName, UriKind.Relative);
			PackagePart packagePart = this._package.CreatePart(uri, imageContentType, CompressionOption.NotCompressed);
			PackageRelationship packageRelationship = sourcePart.CreateRelationship(uri, TargetMode.Internal, "http://schemas.microsoft.com/wpf/2005/10/xaml/component");
			BitmapEncoder bitmapEncoder = WpfPayload.GetBitmapEncoder(imageContentType);
			bitmapEncoder.Frames.Add(BitmapFrame.Create(imageSource));
			Stream stream = packagePart.GetStream();
			using (stream)
			{
				bitmapEncoder.Save(stream);
			}
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00120AE8 File Offset: 0x0011ECE8
		internal string AddImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (image.Source == null)
			{
				throw new ArgumentNullException("image.Source");
			}
			if (string.IsNullOrEmpty(image.Source.ToString()))
			{
				throw new ArgumentException(SR.Get("WpfPayload_InvalidImageSource"));
			}
			if (this._images == null)
			{
				this._images = new List<Image>();
			}
			string text = null;
			string imageContentType = WpfPayload.GetImageContentType(image.Source.ToString());
			for (int i = 0; i < this._images.Count; i++)
			{
				if (WpfPayload.ImagesAreIdentical(this.GetBitmapSourceFromImage(this._images[i]), this.GetBitmapSourceFromImage(image)))
				{
					Invariant.Assert(imageContentType == WpfPayload.GetImageContentType(this._images[i].Source.ToString()), "Image content types expected to be consistent: " + imageContentType + " vs. " + WpfPayload.GetImageContentType(this._images[i].Source.ToString()));
					text = WpfPayload.GetImageName(i, imageContentType);
				}
			}
			if (text == null)
			{
				text = WpfPayload.GetImageName(this._images.Count, imageContentType);
				this._images.Add(image);
			}
			return WpfPayload.GetImageReference(text);
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x00120C18 File Offset: 0x0011EE18
		private static string GetImageContentType(string imageUriString)
		{
			string result;
			if (imageUriString.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
			{
				result = "image/bmp";
			}
			else if (imageUriString.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
			{
				result = "image/gif";
			}
			else if (imageUriString.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || imageUriString.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
			{
				result = "image/jpeg";
			}
			else if (imageUriString.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase))
			{
				result = "image/tiff";
			}
			else
			{
				result = "image/png";
			}
			return result;
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x00120C94 File Offset: 0x0011EE94
		private static BitmapEncoder GetBitmapEncoder(string imageContentType)
		{
			BitmapEncoder result;
			if (!(imageContentType == "image/bmp"))
			{
				if (!(imageContentType == "image/gif"))
				{
					if (!(imageContentType == "image/jpeg"))
					{
						if (!(imageContentType == "image/tiff"))
						{
							if (!(imageContentType == "image/png"))
							{
								Invariant.Assert(false, "Unexpected image content type: " + imageContentType);
								result = null;
							}
							else
							{
								result = new PngBitmapEncoder();
							}
						}
						else
						{
							result = new TiffBitmapEncoder();
						}
					}
					else
					{
						result = new JpegBitmapEncoder();
					}
				}
				else
				{
					result = new GifBitmapEncoder();
				}
			}
			else
			{
				result = new BmpBitmapEncoder();
			}
			return result;
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x00120D20 File Offset: 0x0011EF20
		private static string GetImageFileExtension(string imageContentType)
		{
			string result;
			if (!(imageContentType == "image/bmp"))
			{
				if (!(imageContentType == "image/gif"))
				{
					if (!(imageContentType == "image/jpeg"))
					{
						if (!(imageContentType == "image/tiff"))
						{
							if (!(imageContentType == "image/png"))
							{
								Invariant.Assert(false, "Unexpected image content type: " + imageContentType);
								result = null;
							}
							else
							{
								result = ".png";
							}
						}
						else
						{
							result = ".tiff";
						}
					}
					else
					{
						result = ".jpeg";
					}
				}
				else
				{
					result = ".gif";
				}
			}
			else
			{
				result = ".bmp";
			}
			return result;
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x00120DAC File Offset: 0x0011EFAC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static bool ImagesAreIdentical(BitmapSource imageSource1, BitmapSource imageSource2)
		{
			BitmapFrameDecode bitmapFrameDecode = imageSource1 as BitmapFrameDecode;
			BitmapFrameDecode bitmapFrameDecode2 = imageSource2 as BitmapFrameDecode;
			if (bitmapFrameDecode != null && bitmapFrameDecode2 != null && bitmapFrameDecode.Decoder.Frames.Count == 1 && bitmapFrameDecode2.Decoder.Frames.Count == 1 && bitmapFrameDecode.Decoder.Frames[0] == bitmapFrameDecode2.Decoder.Frames[0])
			{
				return true;
			}
			if (imageSource1.Format.BitsPerPixel != imageSource2.Format.BitsPerPixel || imageSource1.PixelWidth != imageSource2.PixelWidth || imageSource1.PixelHeight != imageSource2.PixelHeight || imageSource1.DpiX != imageSource2.DpiX || imageSource1.DpiY != imageSource2.DpiY || imageSource1.Palette != imageSource2.Palette)
			{
				return false;
			}
			int num = (imageSource1.PixelWidth * imageSource1.Format.BitsPerPixel + 7) / 8;
			int num2 = num * (imageSource1.PixelHeight - 1) + num;
			byte[] array = new byte[num2];
			byte[] array2 = new byte[num2];
			imageSource1.CopyPixels(array, num, 0);
			imageSource2.CopyPixels(array2, num, 0);
			for (int i = 0; i < num2; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x00120EEC File Offset: 0x0011F0EC
		internal Stream CreateXamlStream()
		{
			PackagePart packagePart = this.CreateWpfEntryPart();
			return packagePart.GetStream();
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x00120F08 File Offset: 0x0011F108
		internal Stream CreateImageStream(int imageCount, string contentType, out string imagePartUriString)
		{
			imagePartUriString = WpfPayload.GetImageName(imageCount, contentType);
			Uri partUri = new Uri("/Xaml" + imagePartUriString, UriKind.Relative);
			PackagePart packagePart = this._package.CreatePart(partUri, contentType, CompressionOption.NotCompressed);
			imagePartUriString = WpfPayload.GetImageReference(imagePartUriString);
			return packagePart.GetStream();
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00120F50 File Offset: 0x0011F150
		internal Stream GetImageStream(string imageSourceString)
		{
			Invariant.Assert(imageSourceString.StartsWith("./", StringComparison.OrdinalIgnoreCase));
			imageSourceString = imageSourceString.Substring(1);
			Uri partUri = new Uri("/Xaml" + imageSourceString, UriKind.Relative);
			PackagePart part = this._package.GetPart(partUri);
			return part.GetStream();
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00120F9C File Offset: 0x0011F19C
		private Package CreatePackage(Stream stream)
		{
			Invariant.Assert(this._package == null, "Package has been already created or open for this WpfPayload");
			this._package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite);
			return this._package;
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x00120FC8 File Offset: 0x0011F1C8
		internal static WpfPayload CreateWpfPayload(Stream stream)
		{
			Package package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite);
			return new WpfPayload(package);
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x00120FE4 File Offset: 0x0011F1E4
		internal static WpfPayload OpenWpfPayload(Stream stream)
		{
			Package package = Package.Open(stream, FileMode.Open, FileAccess.Read);
			return new WpfPayload(package);
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x00121000 File Offset: 0x0011F200
		private PackagePart CreateWpfEntryPart()
		{
			Uri uri = new Uri("/Xaml/Document.xaml", UriKind.Relative);
			PackagePart result = this._package.CreatePart(uri, "application/vnd.ms-wpf.xaml+xml", CompressionOption.Normal);
			PackageRelationship packageRelationship = this._package.CreateRelationship(uri, TargetMode.Internal, "http://schemas.microsoft.com/wpf/2005/10/xaml/entry");
			return result;
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x00121040 File Offset: 0x0011F240
		private PackagePart GetWpfEntryPart()
		{
			PackagePart result = null;
			PackageRelationshipCollection relationshipsByType = this._package.GetRelationshipsByType("http://schemas.microsoft.com/wpf/2005/10/xaml/entry");
			PackageRelationship packageRelationship = null;
			using (IEnumerator<PackageRelationship> enumerator = relationshipsByType.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					PackageRelationship packageRelationship2 = enumerator.Current;
					packageRelationship = packageRelationship2;
				}
			}
			if (packageRelationship != null)
			{
				Uri targetUri = packageRelationship.TargetUri;
				result = this._package.GetPart(targetUri);
			}
			return result;
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x001210B8 File Offset: 0x0011F2B8
		private static string GetImageName(int imageIndex, string imageContentType)
		{
			string imageFileExtension = WpfPayload.GetImageFileExtension(imageContentType);
			return "/Image" + (imageIndex + 1) + imageFileExtension;
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x001210DF File Offset: 0x0011F2DF
		private static string GetImageReference(string imageName)
		{
			return "." + imageName;
		}

		// Token: 0x040026D5 RID: 9941
		private const string XamlContentType = "application/vnd.ms-wpf.xaml+xml";

		// Token: 0x040026D6 RID: 9942
		internal const string ImageBmpContentType = "image/bmp";

		// Token: 0x040026D7 RID: 9943
		private const string ImageGifContentType = "image/gif";

		// Token: 0x040026D8 RID: 9944
		private const string ImageJpegContentType = "image/jpeg";

		// Token: 0x040026D9 RID: 9945
		private const string ImageTiffContentType = "image/tiff";

		// Token: 0x040026DA RID: 9946
		private const string ImagePngContentType = "image/png";

		// Token: 0x040026DB RID: 9947
		private const string ImageBmpFileExtension = ".bmp";

		// Token: 0x040026DC RID: 9948
		private const string ImageGifFileExtension = ".gif";

		// Token: 0x040026DD RID: 9949
		private const string ImageJpegFileExtension = ".jpeg";

		// Token: 0x040026DE RID: 9950
		private const string ImageJpgFileExtension = ".jpg";

		// Token: 0x040026DF RID: 9951
		private const string ImageTiffFileExtension = ".tiff";

		// Token: 0x040026E0 RID: 9952
		private const string ImagePngFileExtension = ".png";

		// Token: 0x040026E1 RID: 9953
		private const string XamlRelationshipFromPackageToEntryPart = "http://schemas.microsoft.com/wpf/2005/10/xaml/entry";

		// Token: 0x040026E2 RID: 9954
		private const string XamlRelationshipFromXamlPartToComponentPart = "http://schemas.microsoft.com/wpf/2005/10/xaml/component";

		// Token: 0x040026E3 RID: 9955
		private const string XamlPayloadDirectory = "/Xaml";

		// Token: 0x040026E4 RID: 9956
		private const string XamlEntryName = "/Document.xaml";

		// Token: 0x040026E5 RID: 9957
		private const string XamlImageName = "/Image";

		// Token: 0x040026E6 RID: 9958
		private static int _wpfPayloadCount;

		// Token: 0x040026E7 RID: 9959
		private Package _package;

		// Token: 0x040026E8 RID: 9960
		private List<Image> _images;
	}
}
