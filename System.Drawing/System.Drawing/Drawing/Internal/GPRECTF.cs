using System;

namespace System.Drawing.Internal
{
	// Token: 0x020000E8 RID: 232
	internal struct GPRECTF
	{
		// Token: 0x06000C3A RID: 3130 RVA: 0x0002B61E File Offset: 0x0002981E
		internal GPRECTF(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0002B63D File Offset: 0x0002983D
		internal GPRECTF(RectangleF rect)
		{
			this.X = rect.X;
			this.Y = rect.Y;
			this.Width = rect.Width;
			this.Height = rect.Height;
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0002B673 File Offset: 0x00029873
		internal SizeF SizeF
		{
			get
			{
				return new SizeF(this.Width, this.Height);
			}
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0002B686 File Offset: 0x00029886
		internal RectangleF ToRectangleF()
		{
			return new RectangleF(this.X, this.Y, this.Width, this.Height);
		}

		// Token: 0x04000AC3 RID: 2755
		internal float X;

		// Token: 0x04000AC4 RID: 2756
		internal float Y;

		// Token: 0x04000AC5 RID: 2757
		internal float Width;

		// Token: 0x04000AC6 RID: 2758
		internal float Height;
	}
}
