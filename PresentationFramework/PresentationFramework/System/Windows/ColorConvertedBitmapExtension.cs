using System;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace System.Windows
{
	/// <summary>Implements a markup extension that enables <see cref="T:System.Windows.Media.Imaging.ColorConvertedBitmap" /> creation. A <see cref="T:System.Windows.Media.Imaging.ColorConvertedBitmap" /> does not have an embedded profile, the profile instead being based on source and destination values.</summary>
	// Token: 0x020000EA RID: 234
	[MarkupExtensionReturnType(typeof(ColorConvertedBitmap))]
	public class ColorConvertedBitmapExtension : MarkupExtension
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ColorConvertedBitmapExtension" /> class.</summary>
		// Token: 0x06000857 RID: 2135 RVA: 0x0000B03E File Offset: 0x0000923E
		public ColorConvertedBitmapExtension()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ColorConvertedBitmapExtension" /> class.</summary>
		/// <param name="image">A string that is parsed to determine three URIs: image source, source color context, and destination color context.</param>
		// Token: 0x06000858 RID: 2136 RVA: 0x0001B12C File Offset: 0x0001932C
		public ColorConvertedBitmapExtension(object image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			string[] array = ((string)image).Split(new char[]
			{
				' '
			});
			foreach (string text in array)
			{
				if (text.Length > 0)
				{
					if (this._image == null)
					{
						this._image = text;
					}
					else if (this._sourceProfile == null)
					{
						this._sourceProfile = text;
					}
					else
					{
						if (this._destinationProfile != null)
						{
							throw new InvalidOperationException(SR.Get("ColorConvertedBitmapExtensionSyntax"));
						}
						this._destinationProfile = text;
					}
				}
			}
		}

		/// <summary>Returns an object that should be set on the property where this extension is applied. For <see cref="T:System.Windows.ColorConvertedBitmapExtension" />, this is the completed <see cref="T:System.Windows.Media.Imaging.ColorConvertedBitmap" />.</summary>
		/// <param name="serviceProvider">An object that can provide services for the markup extension. This service is expected to provide results for <see cref="T:System.Windows.Markup.IUriContext" />.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Imaging.ColorConvertedBitmap" /> based on the values passed to the constructor.</returns>
		// Token: 0x06000859 RID: 2137 RVA: 0x0001B1C4 File Offset: 0x000193C4
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this._image == null)
			{
				throw new InvalidOperationException(SR.Get("ColorConvertedBitmapExtensionNoSourceImage"));
			}
			if (this._sourceProfile == null)
			{
				throw new InvalidOperationException(SR.Get("ColorConvertedBitmapExtensionNoSourceProfile"));
			}
			IUriContext uriContext = serviceProvider.GetService(typeof(IUriContext)) as IUriContext;
			if (uriContext == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionNoContext", new object[]
				{
					base.GetType().Name,
					"IUriContext"
				}));
			}
			this._baseUri = uriContext.BaseUri;
			Uri resolvedUri = this.GetResolvedUri(this._image);
			Uri resolvedUri2 = this.GetResolvedUri(this._sourceProfile);
			Uri resolvedUri3 = this.GetResolvedUri(this._destinationProfile);
			ColorContext sourceColorContext = new ColorContext(resolvedUri2);
			ColorContext destinationColorContext = (resolvedUri3 != null) ? new ColorContext(resolvedUri3) : new ColorContext(PixelFormats.Default);
			BitmapDecoder bitmapDecoder = BitmapDecoder.Create(resolvedUri, BitmapCreateOptions.IgnoreColorProfile | BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None);
			BitmapSource source = bitmapDecoder.Frames[0];
			FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0.0);
			object result = formatConvertedBitmap;
			try
			{
				ColorConvertedBitmap colorConvertedBitmap = new ColorConvertedBitmap(formatConvertedBitmap, sourceColorContext, destinationColorContext, PixelFormats.Bgra32);
				result = colorConvertedBitmap;
			}
			catch (FileFormatException)
			{
			}
			return result;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001B304 File Offset: 0x00019504
		private Uri GetResolvedUri(string uri)
		{
			if (uri == null)
			{
				return null;
			}
			return new Uri(this._baseUri, uri);
		}

		// Token: 0x04000799 RID: 1945
		private string _image;

		// Token: 0x0400079A RID: 1946
		private string _sourceProfile;

		// Token: 0x0400079B RID: 1947
		private Uri _baseUri;

		// Token: 0x0400079C RID: 1948
		private string _destinationProfile;
	}
}
