using System;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace System.Windows.Documents
{
	// Token: 0x0200035B RID: 859
	internal sealed class FixedSOMImage : FixedSOMElement
	{
		// Token: 0x06002DC6 RID: 11718 RVA: 0x000CE004 File Offset: 0x000CC204
		private FixedSOMImage(Rect imageRect, GeneralTransform trans, Uri sourceUri, FixedNode node, DependencyObject o) : base(node, trans)
		{
			this._boundingRect = trans.TransformBounds(imageRect);
			this._source = sourceUri;
			this._startIndex = 0;
			this._endIndex = 1;
			this._name = AutomationProperties.GetName(o);
			this._helpText = AutomationProperties.GetHelpText(o);
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000CE058 File Offset: 0x000CC258
		public static FixedSOMImage Create(FixedPage page, Image image, FixedNode fixedNode)
		{
			Uri sourceUri = null;
			if (image.Source is BitmapImage)
			{
				BitmapImage bitmapImage = image.Source as BitmapImage;
				sourceUri = bitmapImage.UriSource;
			}
			else if (image.Source is BitmapFrame)
			{
				BitmapFrame bitmapFrame = image.Source as BitmapFrame;
				sourceUri = new Uri(bitmapFrame.ToString(), UriKind.RelativeOrAbsolute);
			}
			Rect imageRect = new Rect(image.RenderSize);
			GeneralTransform trans = image.TransformToAncestor(page);
			return new FixedSOMImage(imageRect, trans, sourceUri, fixedNode, image);
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000CE0D0 File Offset: 0x000CC2D0
		public static FixedSOMImage Create(FixedPage page, Path path, FixedNode fixedNode)
		{
			ImageSource imageSource = ((ImageBrush)path.Fill).ImageSource;
			Uri sourceUri = null;
			if (imageSource is BitmapImage)
			{
				BitmapImage bitmapImage = imageSource as BitmapImage;
				sourceUri = bitmapImage.UriSource;
			}
			else if (imageSource is BitmapFrame)
			{
				BitmapFrame bitmapFrame = imageSource as BitmapFrame;
				sourceUri = new Uri(bitmapFrame.ToString(), UriKind.RelativeOrAbsolute);
			}
			Rect bounds = path.Data.Bounds;
			GeneralTransform trans = path.TransformToAncestor(page);
			return new FixedSOMImage(bounds, trans, sourceUri, fixedNode, path);
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x000CE146 File Offset: 0x000CC346
		internal Uri Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06002DCA RID: 11722 RVA: 0x000CE14E File Offset: 0x000CC34E
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06002DCB RID: 11723 RVA: 0x000CE156 File Offset: 0x000CC356
		internal string HelpText
		{
			get
			{
				return this._helpText;
			}
		}

		// Token: 0x04001DC7 RID: 7623
		private Uri _source;

		// Token: 0x04001DC8 RID: 7624
		private string _name;

		// Token: 0x04001DC9 RID: 7625
		private string _helpText;
	}
}
