using System;

namespace System.Spatial
{
	// Token: 0x02000020 RID: 32
	public abstract class GeographyPoint : Geography
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00003A90 File Offset: 0x00001C90
		protected GeographyPoint(CoordinateSystem coordinateSystem, SpatialImplementation creator) : base(coordinateSystem, creator)
		{
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000101 RID: 257
		public abstract double Latitude { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000102 RID: 258
		public abstract double Longitude { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000103 RID: 259
		public abstract double? Z { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000104 RID: 260
		public abstract double? M { get; }

		// Token: 0x06000105 RID: 261 RVA: 0x00003A9C File Offset: 0x00001C9C
		public static GeographyPoint Create(double latitude, double longitude)
		{
			return GeographyPoint.Create(CoordinateSystem.DefaultGeography, latitude, longitude, null, null);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00003AC8 File Offset: 0x00001CC8
		public static GeographyPoint Create(double latitude, double longitude, double? z)
		{
			return GeographyPoint.Create(CoordinateSystem.DefaultGeography, latitude, longitude, z, null);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003AEB File Offset: 0x00001CEB
		public static GeographyPoint Create(double latitude, double longitude, double? z, double? m)
		{
			return GeographyPoint.Create(CoordinateSystem.DefaultGeography, latitude, longitude, z, m);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003AFC File Offset: 0x00001CFC
		public static GeographyPoint Create(CoordinateSystem coordinateSystem, double latitude, double longitude, double? z, double? m)
		{
			SpatialBuilder spatialBuilder = SpatialBuilder.Create();
			GeographyPipeline geographyPipeline = spatialBuilder.GeographyPipeline;
			geographyPipeline.SetCoordinateSystem(coordinateSystem);
			geographyPipeline.BeginGeography(SpatialType.Point);
			geographyPipeline.BeginFigure(new GeographyPosition(latitude, longitude, z, m));
			geographyPipeline.EndFigure();
			geographyPipeline.EndGeography();
			return (GeographyPoint)spatialBuilder.ConstructedGeography;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00003B4C File Offset: 0x00001D4C
		public bool Equals(GeographyPoint other)
		{
			bool? flag = base.BaseEquals(other);
			if (flag == null)
			{
				if (this.Latitude == other.Latitude && this.Longitude == other.Longitude)
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

		// Token: 0x0600010A RID: 266 RVA: 0x00003BFB File Offset: 0x00001DFB
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GeographyPoint);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003C0C File Offset: 0x00001E0C
		public override int GetHashCode()
		{
			CoordinateSystem coordinateSystem = base.CoordinateSystem;
			double[] array = new double[4];
			array[0] = (this.IsEmpty ? 0.0 : this.Latitude);
			array[1] = (this.IsEmpty ? 0.0 : this.Longitude);
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
