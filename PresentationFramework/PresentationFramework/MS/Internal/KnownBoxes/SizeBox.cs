using System;
using System.Windows;

namespace MS.Internal.KnownBoxes
{
	// Token: 0x0200065B RID: 1627
	internal class SizeBox
	{
		// Token: 0x06006C07 RID: 27655 RVA: 0x001F1979 File Offset: 0x001EFB79
		internal SizeBox(double width, double height)
		{
			if (width < 0.0 || height < 0.0)
			{
				throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
			}
			this._width = width;
			this._height = height;
		}

		// Token: 0x06006C08 RID: 27656 RVA: 0x001F19B7 File Offset: 0x001EFBB7
		internal SizeBox(Size size) : this(size.Width, size.Height)
		{
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x06006C09 RID: 27657 RVA: 0x001F19CD File Offset: 0x001EFBCD
		// (set) Token: 0x06006C0A RID: 27658 RVA: 0x001F19D5 File Offset: 0x001EFBD5
		internal double Width
		{
			get
			{
				return this._width;
			}
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
				}
				this._width = value;
			}
		}

		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x06006C0B RID: 27659 RVA: 0x001F19FA File Offset: 0x001EFBFA
		// (set) Token: 0x06006C0C RID: 27660 RVA: 0x001F1A02 File Offset: 0x001EFC02
		internal double Height
		{
			get
			{
				return this._height;
			}
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
				}
				this._height = value;
			}
		}

		// Token: 0x04003506 RID: 13574
		private double _width;

		// Token: 0x04003507 RID: 13575
		private double _height;
	}
}
