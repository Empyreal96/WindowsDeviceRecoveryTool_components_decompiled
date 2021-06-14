using System;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000005 RID: 5
	internal abstract class DrawBoth
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000024DB File Offset: 0x000006DB
		public virtual GeographyPipeline GeographyPipeline
		{
			get
			{
				return new DrawBoth.DrawGeographyInput(this);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000024E3 File Offset: 0x000006E3
		public virtual GeometryPipeline GeometryPipeline
		{
			get
			{
				return new DrawBoth.DrawGeometryInput(this);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000024EB File Offset: 0x000006EB
		public static implicit operator SpatialPipeline(DrawBoth both)
		{
			if (both != null)
			{
				return new SpatialPipeline(both.GeographyPipeline, both.GeometryPipeline);
			}
			return null;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002503 File Offset: 0x00000703
		protected virtual GeographyPosition OnLineTo(GeographyPosition position)
		{
			return position;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002506 File Offset: 0x00000706
		protected virtual GeometryPosition OnLineTo(GeometryPosition position)
		{
			return position;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002509 File Offset: 0x00000709
		protected virtual GeographyPosition OnBeginFigure(GeographyPosition position)
		{
			return position;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000250C File Offset: 0x0000070C
		protected virtual GeometryPosition OnBeginFigure(GeometryPosition position)
		{
			return position;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000250F File Offset: 0x0000070F
		protected virtual SpatialType OnBeginGeography(SpatialType type)
		{
			return type;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002512 File Offset: 0x00000712
		protected virtual SpatialType OnBeginGeometry(SpatialType type)
		{
			return type;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002515 File Offset: 0x00000715
		protected virtual void OnEndFigure()
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002517 File Offset: 0x00000717
		protected virtual void OnEndGeography()
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002519 File Offset: 0x00000719
		protected virtual void OnEndGeometry()
		{
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000251B File Offset: 0x0000071B
		protected virtual void OnReset()
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000251D File Offset: 0x0000071D
		protected virtual CoordinateSystem OnSetCoordinateSystem(CoordinateSystem coordinateSystem)
		{
			return coordinateSystem;
		}

		// Token: 0x02000007 RID: 7
		private class DrawGeographyInput : GeographyPipeline
		{
			// Token: 0x06000046 RID: 70 RVA: 0x00002530 File Offset: 0x00000730
			public DrawGeographyInput(DrawBoth both)
			{
				this.both = both;
			}

			// Token: 0x06000047 RID: 71 RVA: 0x0000253F File Offset: 0x0000073F
			public override void LineTo(GeographyPosition position)
			{
				this.both.OnLineTo(position);
			}

			// Token: 0x06000048 RID: 72 RVA: 0x0000254E File Offset: 0x0000074E
			public override void BeginFigure(GeographyPosition position)
			{
				this.both.OnBeginFigure(position);
			}

			// Token: 0x06000049 RID: 73 RVA: 0x0000255D File Offset: 0x0000075D
			public override void BeginGeography(SpatialType type)
			{
				this.both.OnBeginGeography(type);
			}

			// Token: 0x0600004A RID: 74 RVA: 0x0000256C File Offset: 0x0000076C
			public override void EndFigure()
			{
				this.both.OnEndFigure();
			}

			// Token: 0x0600004B RID: 75 RVA: 0x00002579 File Offset: 0x00000779
			public override void EndGeography()
			{
				this.both.OnEndGeography();
			}

			// Token: 0x0600004C RID: 76 RVA: 0x00002586 File Offset: 0x00000786
			public override void Reset()
			{
				this.both.OnReset();
			}

			// Token: 0x0600004D RID: 77 RVA: 0x00002593 File Offset: 0x00000793
			public override void SetCoordinateSystem(CoordinateSystem coordinateSystem)
			{
				this.both.OnSetCoordinateSystem(coordinateSystem);
			}

			// Token: 0x04000007 RID: 7
			private readonly DrawBoth both;
		}

		// Token: 0x02000009 RID: 9
		private class DrawGeometryInput : GeometryPipeline
		{
			// Token: 0x06000056 RID: 86 RVA: 0x000025AA File Offset: 0x000007AA
			public DrawGeometryInput(DrawBoth both)
			{
				this.both = both;
			}

			// Token: 0x06000057 RID: 87 RVA: 0x000025B9 File Offset: 0x000007B9
			public override void LineTo(GeometryPosition position)
			{
				this.both.OnLineTo(position);
			}

			// Token: 0x06000058 RID: 88 RVA: 0x000025C8 File Offset: 0x000007C8
			public override void BeginFigure(GeometryPosition position)
			{
				this.both.OnBeginFigure(position);
			}

			// Token: 0x06000059 RID: 89 RVA: 0x000025D7 File Offset: 0x000007D7
			public override void BeginGeometry(SpatialType type)
			{
				this.both.OnBeginGeometry(type);
			}

			// Token: 0x0600005A RID: 90 RVA: 0x000025E6 File Offset: 0x000007E6
			public override void EndFigure()
			{
				this.both.OnEndFigure();
			}

			// Token: 0x0600005B RID: 91 RVA: 0x000025F3 File Offset: 0x000007F3
			public override void EndGeometry()
			{
				this.both.OnEndGeometry();
			}

			// Token: 0x0600005C RID: 92 RVA: 0x00002600 File Offset: 0x00000800
			public override void Reset()
			{
				this.both.OnReset();
			}

			// Token: 0x0600005D RID: 93 RVA: 0x0000260D File Offset: 0x0000080D
			public override void SetCoordinateSystem(CoordinateSystem coordinateSystem)
			{
				this.both.OnSetCoordinateSystem(coordinateSystem);
			}

			// Token: 0x04000008 RID: 8
			private readonly DrawBoth both;
		}
	}
}
