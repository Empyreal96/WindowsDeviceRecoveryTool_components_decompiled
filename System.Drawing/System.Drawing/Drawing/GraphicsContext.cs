using System;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
	// Token: 0x0200001C RID: 28
	internal class GraphicsContext : IDisposable
	{
		// Token: 0x060001DD RID: 477 RVA: 0x00003800 File Offset: 0x00001A00
		private GraphicsContext()
		{
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00008D58 File Offset: 0x00006F58
		public GraphicsContext(Graphics g)
		{
			Matrix transform = g.Transform;
			if (!transform.IsIdentity)
			{
				float[] elements = transform.Elements;
				this.transformOffset.X = elements[4];
				this.transformOffset.Y = elements[5];
			}
			transform.Dispose();
			Region clip = g.Clip;
			if (clip.IsInfinite(g))
			{
				clip.Dispose();
				return;
			}
			this.clipRegion = clip;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008DC1 File Offset: 0x00006FC1
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008DD0 File Offset: 0x00006FD0
		public void Dispose(bool disposing)
		{
			if (this.nextContext != null)
			{
				this.nextContext.Dispose();
				this.nextContext = null;
			}
			if (this.clipRegion != null)
			{
				this.clipRegion.Dispose();
				this.clipRegion = null;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00008E06 File Offset: 0x00007006
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00008E0E File Offset: 0x0000700E
		public int State
		{
			get
			{
				return this.contextState;
			}
			set
			{
				this.contextState = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00008E17 File Offset: 0x00007017
		public PointF TransformOffset
		{
			get
			{
				return this.transformOffset;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00008E1F File Offset: 0x0000701F
		public Region Clip
		{
			get
			{
				return this.clipRegion;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00008E27 File Offset: 0x00007027
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00008E2F File Offset: 0x0000702F
		public GraphicsContext Next
		{
			get
			{
				return this.nextContext;
			}
			set
			{
				this.nextContext = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00008E38 File Offset: 0x00007038
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00008E40 File Offset: 0x00007040
		public GraphicsContext Previous
		{
			get
			{
				return this.prevContext;
			}
			set
			{
				this.prevContext = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00008E49 File Offset: 0x00007049
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00008E51 File Offset: 0x00007051
		public bool IsCumulative
		{
			get
			{
				return this.isCumulative;
			}
			set
			{
				this.isCumulative = value;
			}
		}

		// Token: 0x04000176 RID: 374
		private int contextState;

		// Token: 0x04000177 RID: 375
		private PointF transformOffset;

		// Token: 0x04000178 RID: 376
		private Region clipRegion;

		// Token: 0x04000179 RID: 377
		private GraphicsContext nextContext;

		// Token: 0x0400017A RID: 378
		private GraphicsContext prevContext;

		// Token: 0x0400017B RID: 379
		private bool isCumulative;
	}
}
