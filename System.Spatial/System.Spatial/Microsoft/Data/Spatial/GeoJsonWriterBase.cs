using System;
using System.Collections.Generic;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200000A RID: 10
	internal abstract class GeoJsonWriterBase : DrawBoth
	{
		// Token: 0x0600005E RID: 94 RVA: 0x0000261C File Offset: 0x0000081C
		public GeoJsonWriterBase()
		{
			this.stack = new Stack<SpatialType>();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000262F File Offset: 0x0000082F
		private bool ShapeHasObjectScope
		{
			get
			{
				return this.IsTopLevel || this.stack.Peek() == SpatialType.Collection;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002649 File Offset: 0x00000849
		private bool IsTopLevel
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002659 File Offset: 0x00000859
		private bool FigureHasArrayScope
		{
			get
			{
				return this.stack.Peek() != SpatialType.Point;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000266C File Offset: 0x0000086C
		protected override GeographyPosition OnLineTo(GeographyPosition position)
		{
			this.WriteControlPoint(position.Longitude, position.Latitude, position.Z, position.M);
			return position;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000268D File Offset: 0x0000088D
		protected override GeometryPosition OnLineTo(GeometryPosition position)
		{
			this.WriteControlPoint(position.X, position.Y, position.Z, position.M);
			return position;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000026AE File Offset: 0x000008AE
		protected override SpatialType OnBeginGeography(SpatialType type)
		{
			this.BeginShape(type, CoordinateSystem.DefaultGeography);
			return type;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000026BD File Offset: 0x000008BD
		protected override SpatialType OnBeginGeometry(SpatialType type)
		{
			this.BeginShape(type, CoordinateSystem.DefaultGeometry);
			return type;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000026CC File Offset: 0x000008CC
		protected override GeographyPosition OnBeginFigure(GeographyPosition position)
		{
			this.BeginFigure();
			this.WriteControlPoint(position.Longitude, position.Latitude, position.Z, position.M);
			return position;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000026F3 File Offset: 0x000008F3
		protected override GeometryPosition OnBeginFigure(GeometryPosition position)
		{
			this.BeginFigure();
			this.WriteControlPoint(position.X, position.Y, position.Z, position.M);
			return position;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000271A File Offset: 0x0000091A
		protected override void OnEndFigure()
		{
			this.EndFigure();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002722 File Offset: 0x00000922
		protected override void OnEndGeography()
		{
			this.EndShape();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000272A File Offset: 0x0000092A
		protected override void OnEndGeometry()
		{
			this.EndShape();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002732 File Offset: 0x00000932
		protected override CoordinateSystem OnSetCoordinateSystem(CoordinateSystem coordinateSystem)
		{
			this.SetCoordinateSystem(coordinateSystem);
			return coordinateSystem;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000273C File Offset: 0x0000093C
		protected override void OnReset()
		{
			this.Reset();
		}

		// Token: 0x0600006D RID: 109
		protected abstract void AddPropertyName(string name);

		// Token: 0x0600006E RID: 110
		protected abstract void AddValue(string value);

		// Token: 0x0600006F RID: 111
		protected abstract void AddValue(double value);

		// Token: 0x06000070 RID: 112
		protected abstract void StartObjectScope();

		// Token: 0x06000071 RID: 113
		protected abstract void StartArrayScope();

		// Token: 0x06000072 RID: 114
		protected abstract void EndObjectScope();

		// Token: 0x06000073 RID: 115
		protected abstract void EndArrayScope();

		// Token: 0x06000074 RID: 116 RVA: 0x00002744 File Offset: 0x00000944
		protected virtual void Reset()
		{
			this.stack.Clear();
			this.currentCoordinateSystem = null;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002758 File Offset: 0x00000958
		private static string GetSpatialTypeName(SpatialType type)
		{
			switch (type)
			{
			case SpatialType.Point:
				return "Point";
			case SpatialType.LineString:
				return "LineString";
			case SpatialType.Polygon:
				return "Polygon";
			case SpatialType.MultiPoint:
				return "MultiPoint";
			case SpatialType.MultiLineString:
				return "MultiLineString";
			case SpatialType.MultiPolygon:
				return "MultiPolygon";
			case SpatialType.Collection:
				return "GeometryCollection";
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000027BC File Offset: 0x000009BC
		private static string GetDataName(SpatialType type)
		{
			switch (type)
			{
			case SpatialType.Point:
			case SpatialType.LineString:
			case SpatialType.Polygon:
			case SpatialType.MultiPoint:
			case SpatialType.MultiLineString:
			case SpatialType.MultiPolygon:
				return "coordinates";
			case SpatialType.Collection:
				return "geometries";
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002802 File Offset: 0x00000A02
		private static bool TypeHasArrayScope(SpatialType type)
		{
			return type != SpatialType.Point && type != SpatialType.LineString;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002811 File Offset: 0x00000A11
		private void SetCoordinateSystem(CoordinateSystem coordinateSystem)
		{
			this.currentCoordinateSystem = coordinateSystem;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000281C File Offset: 0x00000A1C
		private void BeginShape(SpatialType type, CoordinateSystem defaultCoordinateSystem)
		{
			if (this.currentCoordinateSystem == null)
			{
				this.currentCoordinateSystem = defaultCoordinateSystem;
			}
			if (this.ShapeHasObjectScope)
			{
				this.WriteShapeHeader(type);
			}
			if (GeoJsonWriterBase.TypeHasArrayScope(type))
			{
				this.StartArrayScope();
			}
			this.stack.Push(type);
			this.figureDrawn = false;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002868 File Offset: 0x00000A68
		private void WriteShapeHeader(SpatialType type)
		{
			this.StartObjectScope();
			this.AddPropertyName("type");
			this.AddValue(GeoJsonWriterBase.GetSpatialTypeName(type));
			this.AddPropertyName(GeoJsonWriterBase.GetDataName(type));
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002893 File Offset: 0x00000A93
		private void BeginFigure()
		{
			if (this.FigureHasArrayScope)
			{
				this.StartArrayScope();
			}
			this.figureDrawn = true;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000028AC File Offset: 0x00000AAC
		private void WriteControlPoint(double first, double second, double? z, double? m)
		{
			this.StartArrayScope();
			this.AddValue(first);
			this.AddValue(second);
			if (z != null)
			{
				this.AddValue(z.Value);
				if (m != null)
				{
					this.AddValue(m.Value);
				}
			}
			else if (m != null)
			{
				this.AddValue(null);
				this.AddValue(m.Value);
			}
			this.EndArrayScope();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000291E File Offset: 0x00000B1E
		private void EndFigure()
		{
			if (this.FigureHasArrayScope)
			{
				this.EndArrayScope();
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002930 File Offset: 0x00000B30
		private void EndShape()
		{
			SpatialType type = this.stack.Pop();
			if (GeoJsonWriterBase.TypeHasArrayScope(type))
			{
				this.EndArrayScope();
			}
			else if (!this.figureDrawn)
			{
				this.StartArrayScope();
				this.EndArrayScope();
			}
			if (this.IsTopLevel)
			{
				this.WriteCrs();
			}
			if (this.ShapeHasObjectScope)
			{
				this.EndObjectScope();
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000298C File Offset: 0x00000B8C
		private void WriteCrs()
		{
			this.AddPropertyName("crs");
			this.StartObjectScope();
			this.AddPropertyName("type");
			this.AddValue("name");
			this.AddPropertyName("properties");
			this.StartObjectScope();
			this.AddPropertyName("name");
			this.AddValue("EPSG" + ':' + this.currentCoordinateSystem.Id);
			this.EndObjectScope();
			this.EndObjectScope();
		}

		// Token: 0x04000009 RID: 9
		private readonly Stack<SpatialType> stack;

		// Token: 0x0400000A RID: 10
		private CoordinateSystem currentCoordinateSystem;

		// Token: 0x0400000B RID: 11
		private bool figureDrawn;
	}
}
