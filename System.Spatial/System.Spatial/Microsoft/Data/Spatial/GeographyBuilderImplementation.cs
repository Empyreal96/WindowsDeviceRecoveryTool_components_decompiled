using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200004C RID: 76
	internal class GeographyBuilderImplementation : GeographyPipeline, IGeographyProvider
	{
		// Token: 0x060001EE RID: 494 RVA: 0x00005B4F File Offset: 0x00003D4F
		public GeographyBuilderImplementation(SpatialImplementation creator)
		{
			this.builder = new GeographyBuilderImplementation.GeographyTreeBuilder(creator);
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060001EF RID: 495 RVA: 0x00005B63 File Offset: 0x00003D63
		// (remove) Token: 0x060001F0 RID: 496 RVA: 0x00005B71 File Offset: 0x00003D71
		public event Action<Geography> ProduceGeography
		{
			add
			{
				this.builder.ProduceInstance += value;
			}
			remove
			{
				this.builder.ProduceInstance -= value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00005B7F File Offset: 0x00003D7F
		public Geography ConstructedGeography
		{
			get
			{
				return this.builder.ConstructedInstance;
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00005B8C File Offset: 0x00003D8C
		public override void LineTo(GeographyPosition position)
		{
			this.builder.LineTo(position.Latitude, position.Longitude, position.Z, position.M);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00005BB1 File Offset: 0x00003DB1
		public override void BeginFigure(GeographyPosition position)
		{
			this.builder.BeginFigure(position.Latitude, position.Longitude, position.Z, position.M);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00005BD6 File Offset: 0x00003DD6
		public override void BeginGeography(SpatialType type)
		{
			this.builder.BeginGeo(type);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00005BE4 File Offset: 0x00003DE4
		public override void EndFigure()
		{
			this.builder.EndFigure();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00005BF1 File Offset: 0x00003DF1
		public override void EndGeography()
		{
			this.builder.EndGeo();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00005BFE File Offset: 0x00003DFE
		public override void Reset()
		{
			this.builder.Reset();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00005C0B File Offset: 0x00003E0B
		public override void SetCoordinateSystem(CoordinateSystem coordinateSystem)
		{
			Util.CheckArgumentNull(coordinateSystem, "coordinateSystem");
			this.builder.SetCoordinateSystem(coordinateSystem.EpsgId);
		}

		// Token: 0x04000056 RID: 86
		private readonly SpatialTreeBuilder<Geography> builder;

		// Token: 0x0200004F RID: 79
		private class GeographyTreeBuilder : SpatialTreeBuilder<Geography>
		{
			// Token: 0x06000213 RID: 531 RVA: 0x0000603C File Offset: 0x0000423C
			public GeographyTreeBuilder(SpatialImplementation creator)
			{
				Util.CheckArgumentNull(creator, "creator");
				this.creator = creator;
			}

			// Token: 0x06000214 RID: 532 RVA: 0x00006056 File Offset: 0x00004256
			internal override void SetCoordinateSystem(int? epsgId)
			{
				this.currentCoordinateSystem = CoordinateSystem.Geography(epsgId);
			}

			// Token: 0x06000215 RID: 533 RVA: 0x00006064 File Offset: 0x00004264
			protected override Geography CreatePoint(bool isEmpty, double x, double y, double? z, double? m)
			{
				if (!isEmpty)
				{
					return new GeographyPointImplementation(this.currentCoordinateSystem, this.creator, x, y, z, m);
				}
				return new GeographyPointImplementation(this.currentCoordinateSystem, this.creator);
			}

			// Token: 0x06000216 RID: 534 RVA: 0x00006094 File Offset: 0x00004294
			protected override Geography CreateShapeInstance(SpatialType type, IEnumerable<Geography> spatialData)
			{
				switch (type)
				{
				case SpatialType.LineString:
					return new GeographyLineStringImplementation(this.currentCoordinateSystem, this.creator, spatialData.Cast<GeographyPoint>().ToArray<GeographyPoint>());
				case SpatialType.Polygon:
					return new GeographyPolygonImplementation(this.currentCoordinateSystem, this.creator, spatialData.Cast<GeographyLineString>().ToArray<GeographyLineString>());
				case SpatialType.MultiPoint:
					return new GeographyMultiPointImplementation(this.currentCoordinateSystem, this.creator, spatialData.Cast<GeographyPoint>().ToArray<GeographyPoint>());
				case SpatialType.MultiLineString:
					return new GeographyMultiLineStringImplementation(this.currentCoordinateSystem, this.creator, spatialData.Cast<GeographyLineString>().ToArray<GeographyLineString>());
				case SpatialType.MultiPolygon:
					return new GeographyMultiPolygonImplementation(this.currentCoordinateSystem, this.creator, spatialData.Cast<GeographyPolygon>().ToArray<GeographyPolygon>());
				case SpatialType.Collection:
					return new GeographyCollectionImplementation(this.currentCoordinateSystem, this.creator, spatialData.ToArray<Geography>());
				case SpatialType.FullGlobe:
					return new GeographyFullGlobeImplementation(this.currentCoordinateSystem, this.creator);
				}
				return null;
			}

			// Token: 0x04000060 RID: 96
			private readonly SpatialImplementation creator;

			// Token: 0x04000061 RID: 97
			private CoordinateSystem currentCoordinateSystem;
		}
	}
}
