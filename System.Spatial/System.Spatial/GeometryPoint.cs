using System;

namespace System.Spatial
{
	// Token: 0x0200002B RID: 43
	public abstract class GeometryPoint : Geometry
	{
		// Token: 0x06000135 RID: 309 RVA: 0x00003FF8 File Offset: 0x000021F8
		protected GeometryPoint(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000136 RID: 310
		public abstract double X { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000137 RID: 311
		public abstract double Y { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000138 RID: 312
		public abstract double? Z { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000139 RID: 313
		public abstract double? M { get; }

		// Token: 0x0600013A RID: 314 RVA: 0x00004004 File Offset: 0x00002204
		public static GeometryPoint Create(double x, double y)
		{
			return GeometryPoint.Create(CoordinateSystem.DefaultGeometry, x, y, null, null);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004030 File Offset: 0x00002230
		public static GeometryPoint Create(double x, double y, double? z)
		{
			return GeometryPoint.Create(CoordinateSystem.DefaultGeometry, x, y, z, null);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004053 File Offset: 0x00002253
		public static GeometryPoint Create(double x, double y, double? z, double? m)
		{
			return GeometryPoint.Create(CoordinateSystem.DefaultGeometry, x, y, z, m);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004064 File Offset: 0x00002264
		public static GeometryPoint Create(CoordinateSystem coordinateSystem, double x, double y, double? z, double? m)
		{
			SpatialBuilder spatialBuilder = SpatialBuilder.Create();
			GeometryPipeline geometryPipeline = spatialBuilder.GeometryPipeline;
			geometryPipeline.SetCoordinateSystem(coordinateSystem);
			geometryPipeline.BeginGeometry(SpatialType.Point);
			geometryPipeline.BeginFigure(new GeometryPosition(x, y, z, m));
			geometryPipeline.EndFigure();
			geometryPipeline.EndGeometry();
			return (GeometryPoint)spatialBuilder.ConstructedGeometry;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000040B4 File Offset: 0x000022B4
		public bool Equals(GeometryPoint other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				if (this.X == other.X && this.Y == other.Y)
				{
					double? z = this.Z;
					double? z2 = other.Z;
					if (z.GetValueOrDefault() == z2.GetValueOrDefault() && z != null == (z2 != null))
					{
						double? m = this.M;
						double? m2 = other.M;
						return m.GetValueOrDefault() == m2.GetValueOrDefault() && m != null == (m2 != null);
					}
				}
				return false;
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004163 File Offset: 0x00002363
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeometryPoint);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00004174 File Offset: 0x00002374
		public override int GetHashCode()
		{
			CoordinateSystem coordinateSystem = base.CoordinateSystem;
			double[] array = new double[4];
			array[0] = (this.IsEmpty ? 0.0 : this.X);
			array[1] = (this.IsEmpty ? 0.0 : this.Y);
			double[] array2 = array;
			int num = 2;
			double? z = this.Z;
			array2[num] = ((z != null) ? z.GetValueOrDefault() : 0.0);
			double[] array3 = array;
			int num2 = 3;
			double? m = this.M;
			array3[num2] = ((m != null) ? m.GetValueOrDefault() : 0.0);
			return Geography.ComputeHashCodeFor<double>(coordinateSystem, array);
		}
	}
}
