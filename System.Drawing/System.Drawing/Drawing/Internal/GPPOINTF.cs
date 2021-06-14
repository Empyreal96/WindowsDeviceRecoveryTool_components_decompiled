using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Internal
{
	// Token: 0x020000E6 RID: 230
	[StructLayout(LayoutKind.Sequential)]
	internal class GPPOINTF
	{
		// Token: 0x06000C33 RID: 3123 RVA: 0x00003800 File Offset: 0x00001A00
		internal GPPOINTF()
		{
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0002B551 File Offset: 0x00029751
		internal GPPOINTF(PointF pt)
		{
			this.X = pt.X;
			this.Y = pt.Y;
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0002B573 File Offset: 0x00029773
		internal GPPOINTF(Point pt)
		{
			this.X = (float)pt.X;
			this.Y = (float)pt.Y;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0002B597 File Offset: 0x00029797
		internal PointF ToPoint()
		{
			return new PointF(this.X, this.Y);
		}

		// Token: 0x04000ABD RID: 2749
		internal float X;

		// Token: 0x04000ABE RID: 2750
		internal float Y;
	}
}
