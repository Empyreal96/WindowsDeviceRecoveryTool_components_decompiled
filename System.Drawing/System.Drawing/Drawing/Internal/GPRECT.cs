using System;

namespace System.Drawing.Internal
{
	// Token: 0x020000E7 RID: 231
	internal struct GPRECT
	{
		// Token: 0x06000C37 RID: 3127 RVA: 0x0002B5AA File Offset: 0x000297AA
		internal GPRECT(int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0002B5C9 File Offset: 0x000297C9
		internal GPRECT(Rectangle rect)
		{
			this.X = rect.X;
			this.Y = rect.Y;
			this.Width = rect.Width;
			this.Height = rect.Height;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0002B5FF File Offset: 0x000297FF
		internal Rectangle ToRectangle()
		{
			return new Rectangle(this.X, this.Y, this.Width, this.Height);
		}

		// Token: 0x04000ABF RID: 2751
		internal int X;

		// Token: 0x04000AC0 RID: 2752
		internal int Y;

		// Token: 0x04000AC1 RID: 2753
		internal int Width;

		// Token: 0x04000AC2 RID: 2754
		internal int Height;
	}
}
