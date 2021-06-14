using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000050 RID: 80
	internal class GeometryBuilderImplementation : GeometryPipeline, IGeometryProvider
	{
		// Token: 0x06000217 RID: 535 RVA: 0x00006194 File Offset: 0x00004394
		public GeometryBuilderImplementation(SpatialImplementation creator)
		{
			this.builder = new GeometryBuilderImplementation.GeometryTreeBuilder(creator);
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000218 RID: 536 RVA: 0x000061A8 File Offset: 0x000043A8
		// (remove) Token: 0x06000219 RID: 537 RVA: 0x000061B6 File Offset: 0x000043B6
		public event Action<Geometry> ProduceGeometry
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000061C4 File Offset: 0x000043C4
		public Geometry ConstructedGeometry
		{
			get
			{
				return this.builder.ConstructedInstance;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000061D1 File Offset: 0x000043D1
		public override void LineTo(GeometryPosition position)
		{
			this.builder.LineTo(position.X, position.Y, position.Z, position.M);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000061F6 File Offset: 0x000043F6
		public override void BeginFigure(GeometryPosition position)
		{
			this.builder.BeginFigure(position.X, position.Y, position.Z, position.M);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000621B File Offset: 0x0000441B
		public override void BeginGeometry(SpatialType type)
		{
			this.builder.BeginGeo(type);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00006229 File Offset: 0x00004429
		public override void EndFigure()
		{
			this.builder.EndFigure();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006236 File Offset: 0x00004436
		public override void EndGeometry()
		{
			this.builder.EndGeo();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00006243 File Offset: 0x00004443
		public override void Reset()
		{
			this.builder.Reset();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00006250 File Offset: 0x00004450
		public override void SetCoordinateSystem(CoordinateSystem coordinateSystem)
		{
			Util.CheckArgumentNull(coordinateSystem, "coordinateSystem");
			this.builder.SetCoordinateSystem(coordinateSystem.EpsgId);
		}

		// Token: 0x04000062 RID: 98
		private readonly SpatialTreeBuilder<Geometry> builder;

		// Token: 0x02000051 RID: 81
		private class GeometryTreeBuilder : SpatialTreeBuilder<Geometry>
		{
			// Token: 0x06000222 RID: 546 RVA: 0x0000626E File Offset: 0x0000446E
			public GeometryTreeBuilder(SpatialImplementation creator)
			{
				Util.CheckArgumentNull(creator, "creator");
				this.creator = creator;
			}

			// Token: 0x06000223 RID: 547 RVA: 0x00006288 File Offset: 0x00004488
			internal override void SetCoordinateSystem(int? epsgId)
			{
				this.buildCoordinateSystem = CoordinateSystem.Geometry(epsgId);
			}

			// Token: 0x06000224 RID: 548 RVA: 0x00006296 File Offset: 0x00004496
			protected override Geometry CreatePoint(bool isEmpty, double x, double y, double? z, double? m)
			{
				if (!isEmpty)
				{
					return new GeometryPointImplementation(this.buildCoordinateSystem, this.creator, x, y, z, m);
				}
				return new GeometryPointImplementation(this.buildCoordinateSystem, this.creator);
			}

			// Token: 0x06000225 RID: 549 RVA: 0x000062C4 File Offset: 0x000044C4
			protected override Geometry CreateShapeInstance(SpatialType type, IEnumerable<Geometry> spatialData)
			{
				switch (type)
				{
				case SpatialType.LineString:
					return new GeometryLineStringImplementation(this.buildCoordinateSystem, this.creator, spatialData.Cast<GeometryPoint>().ToArray<GeometryPoint>());
				case SpatialType.Polygon:
					return new GeometryPolygonImplementation(this.buildCoordinateSystem, this.creator, spatialData.Cast<GeometryLineString>().ToArray<GeometryLineString>());
				case SpatialType.MultiPoint:
					return new GeometryMultiPointImplementation(this.buildCoordinateSystem, this.creator, spatialData.Cast<GeometryPoint>().ToArray<GeometryPoint>());
				case SpatialType.MultiLineString:
					return new GeometryMultiLineStringImplementation(this.buildCoordinateSystem, this.creator, spatialData.Cast<GeometryLineString>().ToArray<GeometryLineString>());
				case SpatialType.MultiPolygon:
					return new GeometryMultiPolygonImplementation(this.buildCoordinateSystem, this.creator, spatialData.Cast<GeometryPolygon>().ToArray<GeometryPolygon>());
				case SpatialType.Collection:
					return new GeometryCollectionImplementation(this.buildCoordinateSystem, this.creator, spatialData.ToArray<Geometry>());
				default:
					return null;
				}
			}

			// Token: 0x04000063 RID: 99
			private readonly SpatialImplementation creator;

			// Token: 0x04000064 RID: 100
			private CoordinateSystem buildCoordinateSystem;
		}
	}
}
