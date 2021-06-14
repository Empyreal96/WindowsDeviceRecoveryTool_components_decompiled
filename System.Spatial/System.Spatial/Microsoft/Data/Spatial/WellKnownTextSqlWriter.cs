using System;
using System.Collections.Generic;
using System.IO;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000080 RID: 128
	internal sealed class WellKnownTextSqlWriter : DrawBoth
	{
		// Token: 0x06000313 RID: 787 RVA: 0x00008E28 File Offset: 0x00007028
		public WellKnownTextSqlWriter(TextWriter writer) : this(writer, false)
		{
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00008E32 File Offset: 0x00007032
		public WellKnownTextSqlWriter(TextWriter writer, bool allowOnlyTwoDimensions)
		{
			this.allowOnlyTwoDimensions = allowOnlyTwoDimensions;
			this.writer = writer;
			this.parentStack = new Stack<SpatialType>();
			this.Reset();
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00008E59 File Offset: 0x00007059
		protected override GeographyPosition OnLineTo(GeographyPosition position)
		{
			this.AddLineTo(position.Longitude, position.Latitude, position.Z, position.M);
			return position;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00008E7A File Offset: 0x0000707A
		protected override GeometryPosition OnLineTo(GeometryPosition position)
		{
			this.AddLineTo(position.X, position.Y, position.Z, position.M);
			return position;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00008E9B File Offset: 0x0000709B
		protected override SpatialType OnBeginGeography(SpatialType type)
		{
			this.BeginGeo(type);
			return type;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00008EA5 File Offset: 0x000070A5
		protected override SpatialType OnBeginGeometry(SpatialType type)
		{
			this.BeginGeo(type);
			return type;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00008EAF File Offset: 0x000070AF
		protected override GeographyPosition OnBeginFigure(GeographyPosition position)
		{
			this.WriteFigureScope(position.Longitude, position.Latitude, position.Z, position.M);
			return position;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00008ED0 File Offset: 0x000070D0
		protected override GeometryPosition OnBeginFigure(GeometryPosition position)
		{
			this.WriteFigureScope(position.X, position.Y, position.Z, position.M);
			return position;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00008EF1 File Offset: 0x000070F1
		protected override void OnEndFigure()
		{
			this.EndFigure();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00008EF9 File Offset: 0x000070F9
		protected override void OnEndGeography()
		{
			this.EndGeo();
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00008F01 File Offset: 0x00007101
		protected override void OnEndGeometry()
		{
			this.EndGeo();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00008F09 File Offset: 0x00007109
		protected override CoordinateSystem OnSetCoordinateSystem(CoordinateSystem coordinateSystem)
		{
			this.WriteCoordinateSystem(coordinateSystem);
			return coordinateSystem;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00008F13 File Offset: 0x00007113
		protected override void OnReset()
		{
			this.Reset();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00008F1C File Offset: 0x0000711C
		private void WriteCoordinateSystem(CoordinateSystem coordinateSystem)
		{
			if (!this.coordinateSystemWritten)
			{
				this.writer.Write("SRID");
				this.writer.Write("=");
				this.writer.Write(coordinateSystem.Id);
				this.writer.Write(";");
				this.coordinateSystemWritten = true;
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00008F79 File Offset: 0x00007179
		private void Reset()
		{
			this.figureWritten = false;
			this.parentStack.Clear();
			this.shapeWritten = false;
			this.coordinateSystemWritten = false;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00008F9C File Offset: 0x0000719C
		private void BeginGeo(SpatialType type)
		{
			SpatialType spatialType = (this.parentStack.Count == 0) ? SpatialType.Unknown : this.parentStack.Peek();
			if (spatialType == SpatialType.MultiPoint || spatialType == SpatialType.MultiLineString || spatialType == SpatialType.MultiPolygon || spatialType == SpatialType.Collection)
			{
				this.writer.Write(this.shapeWritten ? ", " : "(");
			}
			if (spatialType == SpatialType.Unknown || spatialType == SpatialType.Collection)
			{
				this.WriteTaggedText(type);
			}
			this.figureWritten = false;
			this.parentStack.Push(type);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00009015 File Offset: 0x00007215
		private void AddLineTo(double x, double y, double? z, double? m)
		{
			this.writer.Write(", ");
			this.WritePoint(x, y, z, m);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00009032 File Offset: 0x00007232
		private void EndFigure()
		{
			this.writer.Write(")");
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00009044 File Offset: 0x00007244
		private void WriteTaggedText(SpatialType type)
		{
			switch (type)
			{
			case SpatialType.Point:
				this.writer.Write("POINT");
				break;
			case SpatialType.LineString:
				this.writer.Write("LINESTRING");
				break;
			case SpatialType.Polygon:
				this.writer.Write("POLYGON");
				break;
			case SpatialType.MultiPoint:
				this.shapeWritten = false;
				this.writer.Write("MULTIPOINT");
				break;
			case SpatialType.MultiLineString:
				this.shapeWritten = false;
				this.writer.Write("MULTILINESTRING");
				break;
			case SpatialType.MultiPolygon:
				this.shapeWritten = false;
				this.writer.Write("MULTIPOLYGON");
				break;
			case SpatialType.Collection:
				this.shapeWritten = false;
				this.writer.Write("GEOMETRYCOLLECTION");
				break;
			case SpatialType.FullGlobe:
				this.writer.Write("FULLGLOBE");
				break;
			}
			if (type != SpatialType.FullGlobe)
			{
				this.writer.Write(" ");
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00009154 File Offset: 0x00007354
		private void WriteFigureScope(double coordinate1, double coordinate2, double? coordinate3, double? coordinate4)
		{
			if (this.figureWritten)
			{
				this.writer.Write(", ");
			}
			else if (this.parentStack.Peek() == SpatialType.Polygon)
			{
				this.writer.Write("(");
			}
			this.writer.Write("(");
			this.figureWritten = true;
			this.WritePoint(coordinate1, coordinate2, coordinate3, coordinate4);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000091BC File Offset: 0x000073BC
		private void EndGeo()
		{
			switch (this.parentStack.Pop())
			{
			case SpatialType.Point:
			case SpatialType.LineString:
				if (!this.figureWritten)
				{
					this.writer.Write("EMPTY");
				}
				break;
			case SpatialType.Polygon:
				this.writer.Write(this.figureWritten ? ")" : "EMPTY");
				break;
			case SpatialType.MultiPoint:
			case SpatialType.MultiLineString:
			case SpatialType.MultiPolygon:
			case SpatialType.Collection:
				this.writer.Write(this.shapeWritten ? ")" : "EMPTY");
				break;
			case SpatialType.FullGlobe:
				this.writer.Write(")");
				break;
			}
			this.shapeWritten = true;
			this.writer.Flush();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000928C File Offset: 0x0000748C
		private void WritePoint(double x, double y, double? z, double? m)
		{
			this.writer.WriteRoundtrippable(x);
			this.writer.Write(" ");
			this.writer.WriteRoundtrippable(y);
			if (!this.allowOnlyTwoDimensions && z != null)
			{
				this.writer.Write(" ");
				this.writer.WriteRoundtrippable(z.Value);
				if (!this.allowOnlyTwoDimensions && m != null)
				{
					this.writer.Write(" ");
					this.writer.WriteRoundtrippable(m.Value);
					return;
				}
			}
			else if (!this.allowOnlyTwoDimensions && m != null)
			{
				this.writer.Write(" ");
				this.writer.Write("NULL");
				this.writer.Write(" ");
				this.writer.Write(m.Value);
			}
		}

		// Token: 0x040000EA RID: 234
		private bool allowOnlyTwoDimensions;

		// Token: 0x040000EB RID: 235
		private TextWriter writer;

		// Token: 0x040000EC RID: 236
		private Stack<SpatialType> parentStack;

		// Token: 0x040000ED RID: 237
		private bool coordinateSystemWritten;

		// Token: 0x040000EE RID: 238
		private bool figureWritten;

		// Token: 0x040000EF RID: 239
		private bool shapeWritten;
	}
}
