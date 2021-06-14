using System;
using System.Collections.Generic;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000069 RID: 105
	internal class SpatialValidatorImplementation : SpatialPipeline
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00007A1F File Offset: 0x00005C1F
		public override GeographyPipeline GeographyPipeline
		{
			get
			{
				return this.geographyValidatorInstance.GeographyPipeline;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00007A2C File Offset: 0x00005C2C
		public override GeometryPipeline GeometryPipeline
		{
			get
			{
				return this.geometryValidatorInstance.GeometryPipeline;
			}
		}

		// Token: 0x040000B7 RID: 183
		internal const double MaxLongitude = 15069.0;

		// Token: 0x040000B8 RID: 184
		internal const double MaxLatitude = 90.0;

		// Token: 0x040000B9 RID: 185
		private readonly SpatialValidatorImplementation.NestedValidator geographyValidatorInstance = new SpatialValidatorImplementation.NestedValidator();

		// Token: 0x040000BA RID: 186
		private readonly SpatialValidatorImplementation.NestedValidator geometryValidatorInstance = new SpatialValidatorImplementation.NestedValidator();

		// Token: 0x0200006A RID: 106
		private class NestedValidator : DrawBoth
		{
			// Token: 0x060002B0 RID: 688 RVA: 0x00007A57 File Offset: 0x00005C57
			public NestedValidator()
			{
				this.InitializeObject();
			}

			// Token: 0x060002B1 RID: 689 RVA: 0x00007A74 File Offset: 0x00005C74
			protected override CoordinateSystem OnSetCoordinateSystem(CoordinateSystem coordinateSystem)
			{
				SpatialValidatorImplementation.NestedValidator.ValidatorState validatorState = this.stack.Peek();
				this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem);
				if (validatorState == SpatialValidatorImplementation.NestedValidator.CoordinateSystem)
				{
					this.validationCoordinateSystem = coordinateSystem;
				}
				else if (this.validationCoordinateSystem != coordinateSystem)
				{
					throw new FormatException(Strings.Validator_SridMismatch);
				}
				return coordinateSystem;
			}

			// Token: 0x060002B2 RID: 690 RVA: 0x00007ABA File Offset: 0x00005CBA
			protected override SpatialType OnBeginGeography(SpatialType shape)
			{
				if (this.depth > 0 && !this.processingGeography)
				{
					throw new FormatException(Strings.Validator_UnexpectedGeometry);
				}
				this.processingGeography = true;
				this.BeginShape(shape);
				return shape;
			}

			// Token: 0x060002B3 RID: 691 RVA: 0x00007AE7 File Offset: 0x00005CE7
			protected override void OnEndGeography()
			{
				this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.End);
				if (!this.processingGeography)
				{
					throw new FormatException(Strings.Validator_UnexpectedGeometry);
				}
				this.depth--;
			}

			// Token: 0x060002B4 RID: 692 RVA: 0x00007B12 File Offset: 0x00005D12
			protected override SpatialType OnBeginGeometry(SpatialType shape)
			{
				if (this.depth > 0 && this.processingGeography)
				{
					throw new FormatException(Strings.Validator_UnexpectedGeography);
				}
				this.processingGeography = false;
				this.BeginShape(shape);
				return shape;
			}

			// Token: 0x060002B5 RID: 693 RVA: 0x00007B3F File Offset: 0x00005D3F
			protected override void OnEndGeometry()
			{
				this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.End);
				if (this.processingGeography)
				{
					throw new FormatException(Strings.Validator_UnexpectedGeography);
				}
				this.depth--;
			}

			// Token: 0x060002B6 RID: 694 RVA: 0x00007B6A File Offset: 0x00005D6A
			protected override GeographyPosition OnBeginFigure(GeographyPosition position)
			{
				this.BeginFigure(new Action<double, double, double?, double?>(SpatialValidatorImplementation.NestedValidator.ValidateGeographyPosition), position.Latitude, position.Longitude, position.Z, position.M);
				return position;
			}

			// Token: 0x060002B7 RID: 695 RVA: 0x00007B97 File Offset: 0x00005D97
			protected override GeometryPosition OnBeginFigure(GeometryPosition position)
			{
				this.BeginFigure(new Action<double, double, double?, double?>(SpatialValidatorImplementation.NestedValidator.ValidateGeometryPosition), position.X, position.Y, position.Z, position.M);
				return position;
			}

			// Token: 0x060002B8 RID: 696 RVA: 0x00007BC4 File Offset: 0x00005DC4
			protected override void OnEndFigure()
			{
				this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.EndFigure);
			}

			// Token: 0x060002B9 RID: 697 RVA: 0x00007BCE File Offset: 0x00005DCE
			protected override void OnReset()
			{
				this.InitializeObject();
			}

			// Token: 0x060002BA RID: 698 RVA: 0x00007BD8 File Offset: 0x00005DD8
			protected override GeographyPosition OnLineTo(GeographyPosition position)
			{
				if (this.processingGeography)
				{
					SpatialValidatorImplementation.NestedValidator.ValidateGeographyPosition(position.Latitude, position.Longitude, position.Z, position.M);
				}
				this.AddControlPoint(position.Latitude, position.Longitude);
				if (!this.processingGeography)
				{
					throw new FormatException(Strings.Validator_UnexpectedGeometry);
				}
				return position;
			}

			// Token: 0x060002BB RID: 699 RVA: 0x00007C30 File Offset: 0x00005E30
			protected override GeometryPosition OnLineTo(GeometryPosition position)
			{
				if (!this.processingGeography)
				{
					SpatialValidatorImplementation.NestedValidator.ValidateGeometryPosition(position.X, position.Y, position.Z, position.M);
				}
				this.AddControlPoint(position.X, position.Y);
				if (this.processingGeography)
				{
					throw new FormatException(Strings.Validator_UnexpectedGeography);
				}
				return position;
			}

			// Token: 0x060002BC RID: 700 RVA: 0x00007C88 File Offset: 0x00005E88
			private static bool IsFinite(double value)
			{
				return !double.IsNaN(value) && !double.IsInfinity(value);
			}

			// Token: 0x060002BD RID: 701 RVA: 0x00007CA0 File Offset: 0x00005EA0
			private static bool IsPointValid(double first, double second, double? z, double? m)
			{
				return SpatialValidatorImplementation.NestedValidator.IsFinite(first) && SpatialValidatorImplementation.NestedValidator.IsFinite(second) && (z == null || SpatialValidatorImplementation.NestedValidator.IsFinite(z.Value)) && (m == null || SpatialValidatorImplementation.NestedValidator.IsFinite(m.Value));
			}

			// Token: 0x060002BE RID: 702 RVA: 0x00007CED File Offset: 0x00005EED
			private static void ValidateOnePosition(double first, double second, double? z, double? m)
			{
				if (!SpatialValidatorImplementation.NestedValidator.IsPointValid(first, second, z, m))
				{
					throw new FormatException(Strings.Validator_InvalidPointCoordinate(first, second, z, m));
				}
			}

			// Token: 0x060002BF RID: 703 RVA: 0x00007D1D File Offset: 0x00005F1D
			private static void ValidateGeographyPosition(double latitude, double longitude, double? z, double? m)
			{
				SpatialValidatorImplementation.NestedValidator.ValidateOnePosition(latitude, longitude, z, m);
				if (!SpatialValidatorImplementation.NestedValidator.IsLatitudeValid(latitude))
				{
					throw new FormatException(Strings.Validator_InvalidLatitudeCoordinate(latitude));
				}
				if (!SpatialValidatorImplementation.NestedValidator.IsLongitudeValid(longitude))
				{
					throw new FormatException(Strings.Validator_InvalidLongitudeCoordinate(longitude));
				}
			}

			// Token: 0x060002C0 RID: 704 RVA: 0x00007D5A File Offset: 0x00005F5A
			private static void ValidateGeometryPosition(double x, double y, double? z, double? m)
			{
				SpatialValidatorImplementation.NestedValidator.ValidateOnePosition(x, y, z, m);
			}

			// Token: 0x060002C1 RID: 705 RVA: 0x00007D65 File Offset: 0x00005F65
			private static bool IsLatitudeValid(double latitude)
			{
				return latitude >= -90.0 && latitude <= 90.0;
			}

			// Token: 0x060002C2 RID: 706 RVA: 0x00007D84 File Offset: 0x00005F84
			private static bool IsLongitudeValid(double longitude)
			{
				return longitude >= -15069.0 && longitude <= 15069.0;
			}

			// Token: 0x060002C3 RID: 707 RVA: 0x00007DA3 File Offset: 0x00005FA3
			private static void ValidateGeographyPolygon(int numOfPoints, double initialFirstCoordinate, double initialSecondCoordinate, double mostRecentFirstCoordinate, double mostRecentSecondCoordinate)
			{
				if (numOfPoints < 4 || initialFirstCoordinate != mostRecentFirstCoordinate || !SpatialValidatorImplementation.NestedValidator.AreLongitudesEqual(initialSecondCoordinate, mostRecentSecondCoordinate))
				{
					throw new FormatException(Strings.Validator_InvalidPolygonPoints);
				}
			}

			// Token: 0x060002C4 RID: 708 RVA: 0x00007DC2 File Offset: 0x00005FC2
			private static void ValidateGeometryPolygon(int numOfPoints, double initialFirstCoordinate, double initialSecondCoordinate, double mostRecentFirstCoordinate, double mostRecentSecondCoordinate)
			{
				if (numOfPoints < 4 || initialFirstCoordinate != mostRecentFirstCoordinate || initialSecondCoordinate != mostRecentSecondCoordinate)
				{
					throw new FormatException(Strings.Validator_InvalidPolygonPoints);
				}
			}

			// Token: 0x060002C5 RID: 709 RVA: 0x00007DDC File Offset: 0x00005FDC
			private static bool AreLongitudesEqual(double left, double right)
			{
				return left == right || (left - right) % 360.0 == 0.0;
			}

			// Token: 0x060002C6 RID: 710 RVA: 0x00007DFC File Offset: 0x00005FFC
			private void BeginFigure(Action<double, double, double?, double?> validate, double x, double y, double? z, double? m)
			{
				validate(x, y, z, m);
				this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFigure);
				this.pointCount = 0;
				this.TrackPosition(x, y);
			}

			// Token: 0x060002C7 RID: 711 RVA: 0x00007E24 File Offset: 0x00006024
			private void BeginShape(SpatialType type)
			{
				this.depth++;
				switch (type)
				{
				case SpatialType.Point:
					this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPoint);
					return;
				case SpatialType.LineString:
					this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginLineString);
					return;
				case SpatialType.Polygon:
					this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPolygon);
					return;
				case SpatialType.MultiPoint:
					this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiPoint);
					return;
				case SpatialType.MultiLineString:
					this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiLineString);
					return;
				case SpatialType.MultiPolygon:
					this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiPolygon);
					return;
				case SpatialType.Collection:
					this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginCollection);
					return;
				case SpatialType.FullGlobe:
					if (!this.processingGeography)
					{
						throw new FormatException(Strings.Validator_InvalidType(type));
					}
					this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFullGlobe);
					return;
				}
				throw new FormatException(Strings.Validator_InvalidType(type));
			}

			// Token: 0x060002C8 RID: 712 RVA: 0x00007EE1 File Offset: 0x000060E1
			private void AddControlPoint(double first, double second)
			{
				this.Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall.LineTo);
				this.TrackPosition(first, second);
			}

			// Token: 0x060002C9 RID: 713 RVA: 0x00007EF3 File Offset: 0x000060F3
			private void TrackPosition(double first, double second)
			{
				if (this.pointCount == 0)
				{
					this.initialFirstCoordinate = first;
					this.initialSecondCoordinate = second;
				}
				this.mostRecentFirstCoordinate = first;
				this.mostRecentSecondCoordinate = second;
				this.pointCount++;
			}

			// Token: 0x060002CA RID: 714 RVA: 0x00007F28 File Offset: 0x00006128
			private void Execute(SpatialValidatorImplementation.NestedValidator.PipelineCall transition)
			{
				SpatialValidatorImplementation.NestedValidator.ValidatorState validatorState = this.stack.Peek();
				validatorState.ValidateTransition(transition, this);
			}

			// Token: 0x060002CB RID: 715 RVA: 0x00007F4C File Offset: 0x0000614C
			private void InitializeObject()
			{
				this.depth = 0;
				this.initialFirstCoordinate = 0.0;
				this.initialSecondCoordinate = 0.0;
				this.mostRecentFirstCoordinate = 0.0;
				this.mostRecentSecondCoordinate = 0.0;
				this.pointCount = 0;
				this.validationCoordinateSystem = null;
				this.ringCount = 0;
				this.stack.Clear();
				this.stack.Push(SpatialValidatorImplementation.NestedValidator.CoordinateSystem);
			}

			// Token: 0x060002CC RID: 716 RVA: 0x00007FCC File Offset: 0x000061CC
			private void Call(SpatialValidatorImplementation.NestedValidator.ValidatorState state)
			{
				if (this.stack.Count > 28)
				{
					throw new FormatException(Strings.Validator_NestingOverflow(28));
				}
				this.stack.Push(state);
			}

			// Token: 0x060002CD RID: 717 RVA: 0x00007FFB File Offset: 0x000061FB
			private void Return()
			{
				this.stack.Pop();
			}

			// Token: 0x060002CE RID: 718 RVA: 0x00008009 File Offset: 0x00006209
			private void Jump(SpatialValidatorImplementation.NestedValidator.ValidatorState state)
			{
				this.stack.Pop();
				this.stack.Push(state);
			}

			// Token: 0x040000BB RID: 187
			private const int MaxGeometryCollectionDepth = 28;

			// Token: 0x040000BC RID: 188
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState CoordinateSystem = new SpatialValidatorImplementation.NestedValidator.SetCoordinateSystemState();

			// Token: 0x040000BD RID: 189
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState BeginSpatial = new SpatialValidatorImplementation.NestedValidator.BeginGeoState();

			// Token: 0x040000BE RID: 190
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState PointStart = new SpatialValidatorImplementation.NestedValidator.PointStartState();

			// Token: 0x040000BF RID: 191
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState PointBuilding = new SpatialValidatorImplementation.NestedValidator.PointBuildingState();

			// Token: 0x040000C0 RID: 192
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState PointEnd = new SpatialValidatorImplementation.NestedValidator.PointEndState();

			// Token: 0x040000C1 RID: 193
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState LineStringStart = new SpatialValidatorImplementation.NestedValidator.LineStringStartState();

			// Token: 0x040000C2 RID: 194
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState LineStringBuilding = new SpatialValidatorImplementation.NestedValidator.LineStringBuildingState();

			// Token: 0x040000C3 RID: 195
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState LineStringEnd = new SpatialValidatorImplementation.NestedValidator.LineStringEndState();

			// Token: 0x040000C4 RID: 196
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState PolygonStart = new SpatialValidatorImplementation.NestedValidator.PolygonStartState();

			// Token: 0x040000C5 RID: 197
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState PolygonBuilding = new SpatialValidatorImplementation.NestedValidator.PolygonBuildingState();

			// Token: 0x040000C6 RID: 198
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState MultiPoint = new SpatialValidatorImplementation.NestedValidator.MultiPointState();

			// Token: 0x040000C7 RID: 199
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState MultiLineString = new SpatialValidatorImplementation.NestedValidator.MultiLineStringState();

			// Token: 0x040000C8 RID: 200
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState MultiPolygon = new SpatialValidatorImplementation.NestedValidator.MultiPolygonState();

			// Token: 0x040000C9 RID: 201
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState Collection = new SpatialValidatorImplementation.NestedValidator.CollectionState();

			// Token: 0x040000CA RID: 202
			private static readonly SpatialValidatorImplementation.NestedValidator.ValidatorState FullGlobe = new SpatialValidatorImplementation.NestedValidator.FullGlobeState();

			// Token: 0x040000CB RID: 203
			private readonly Stack<SpatialValidatorImplementation.NestedValidator.ValidatorState> stack = new Stack<SpatialValidatorImplementation.NestedValidator.ValidatorState>(16);

			// Token: 0x040000CC RID: 204
			private CoordinateSystem validationCoordinateSystem;

			// Token: 0x040000CD RID: 205
			private int ringCount;

			// Token: 0x040000CE RID: 206
			private double initialFirstCoordinate;

			// Token: 0x040000CF RID: 207
			private double initialSecondCoordinate;

			// Token: 0x040000D0 RID: 208
			private double mostRecentFirstCoordinate;

			// Token: 0x040000D1 RID: 209
			private double mostRecentSecondCoordinate;

			// Token: 0x040000D2 RID: 210
			private bool processingGeography;

			// Token: 0x040000D3 RID: 211
			private int pointCount;

			// Token: 0x040000D4 RID: 212
			private int depth;

			// Token: 0x0200006B RID: 107
			private enum PipelineCall
			{
				// Token: 0x040000D6 RID: 214
				SetCoordinateSystem,
				// Token: 0x040000D7 RID: 215
				Begin,
				// Token: 0x040000D8 RID: 216
				BeginPoint,
				// Token: 0x040000D9 RID: 217
				BeginLineString,
				// Token: 0x040000DA RID: 218
				BeginPolygon,
				// Token: 0x040000DB RID: 219
				BeginMultiPoint,
				// Token: 0x040000DC RID: 220
				BeginMultiLineString,
				// Token: 0x040000DD RID: 221
				BeginMultiPolygon,
				// Token: 0x040000DE RID: 222
				BeginCollection,
				// Token: 0x040000DF RID: 223
				BeginFullGlobe,
				// Token: 0x040000E0 RID: 224
				BeginFigure,
				// Token: 0x040000E1 RID: 225
				LineTo,
				// Token: 0x040000E2 RID: 226
				EndFigure,
				// Token: 0x040000E3 RID: 227
				End
			}

			// Token: 0x0200006C RID: 108
			private abstract class ValidatorState
			{
				// Token: 0x060002D0 RID: 720
				internal abstract void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator);

				// Token: 0x060002D1 RID: 721 RVA: 0x000080C7 File Offset: 0x000062C7
				protected static void ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator.PipelineCall actual)
				{
					throw new FormatException(Strings.Validator_UnexpectedCall(transition, actual));
				}

				// Token: 0x060002D2 RID: 722 RVA: 0x000080DF File Offset: 0x000062DF
				protected static void ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall transition1, SpatialValidatorImplementation.NestedValidator.PipelineCall transition2, SpatialValidatorImplementation.NestedValidator.PipelineCall actual)
				{
					throw new FormatException(Strings.Validator_UnexpectedCall2(transition1, transition2, actual));
				}

				// Token: 0x060002D3 RID: 723 RVA: 0x000080FD File Offset: 0x000062FD
				protected static void ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall transition1, SpatialValidatorImplementation.NestedValidator.PipelineCall transition2, SpatialValidatorImplementation.NestedValidator.PipelineCall transition3, SpatialValidatorImplementation.NestedValidator.PipelineCall actual)
				{
					throw new FormatException(Strings.Validator_UnexpectedCall2(transition1 + ", " + transition2, transition3, actual));
				}
			}

			// Token: 0x0200006D RID: 109
			private class SetCoordinateSystemState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002D5 RID: 725 RVA: 0x00008134 File Offset: 0x00006334
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem)
					{
						validator.Call(SpatialValidatorImplementation.NestedValidator.BeginSpatial);
						return;
					}
					SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem, transition);
				}
			}

			// Token: 0x0200006E RID: 110
			private class BeginGeoState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002D7 RID: 727 RVA: 0x00008164 File Offset: 0x00006364
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					switch (transition)
					{
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPoint:
						validator.Jump(SpatialValidatorImplementation.NestedValidator.PointStart);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginLineString:
						validator.Jump(SpatialValidatorImplementation.NestedValidator.LineStringStart);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPolygon:
						validator.Jump(SpatialValidatorImplementation.NestedValidator.PolygonStart);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiPoint:
						validator.Jump(SpatialValidatorImplementation.NestedValidator.MultiPoint);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiLineString:
						validator.Jump(SpatialValidatorImplementation.NestedValidator.MultiLineString);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiPolygon:
						validator.Jump(SpatialValidatorImplementation.NestedValidator.MultiPolygon);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginCollection:
						validator.Jump(SpatialValidatorImplementation.NestedValidator.Collection);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFullGlobe:
						if (validator.depth != 1)
						{
							throw new FormatException(Strings.Validator_FullGlobeInCollection);
						}
						validator.Jump(SpatialValidatorImplementation.NestedValidator.FullGlobe);
						return;
					default:
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.Begin, transition);
						return;
					}
				}
			}

			// Token: 0x0200006F RID: 111
			private class PointStartState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002D9 RID: 729 RVA: 0x00008220 File Offset: 0x00006420
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFigure)
					{
						validator.Jump(SpatialValidatorImplementation.NestedValidator.PointBuilding);
						return;
					}
					if (transition != SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
					{
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFigure, SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
						return;
					}
					validator.Return();
				}
			}

			// Token: 0x02000070 RID: 112
			private class PointBuildingState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002DB RID: 731 RVA: 0x00008260 File Offset: 0x00006460
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					switch (transition)
					{
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.LineTo:
						if (validator.pointCount != 0)
						{
							SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.EndFigure, transition);
						}
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.EndFigure:
						if (validator.pointCount == 0)
						{
							SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFigure, transition);
						}
						validator.Jump(SpatialValidatorImplementation.NestedValidator.PointEnd);
						return;
					default:
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.EndFigure, transition);
						return;
					}
				}
			}

			// Token: 0x02000071 RID: 113
			private class PointEndState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002DD RID: 733 RVA: 0x000082C0 File Offset: 0x000064C0
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
					{
						validator.Return();
						return;
					}
					SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
				}
			}

			// Token: 0x02000072 RID: 114
			private class LineStringStartState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002DF RID: 735 RVA: 0x000082EC File Offset: 0x000064EC
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFigure)
					{
						validator.Jump(SpatialValidatorImplementation.NestedValidator.LineStringBuilding);
						return;
					}
					if (transition != SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
					{
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFigure, SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
						return;
					}
					validator.Return();
				}
			}

			// Token: 0x02000073 RID: 115
			private class LineStringBuildingState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002E1 RID: 737 RVA: 0x0000832C File Offset: 0x0000652C
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					switch (transition)
					{
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.LineTo:
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.EndFigure:
						if (validator.pointCount < 2)
						{
							throw new FormatException(Strings.Validator_LineStringNeedsTwoPoints);
						}
						validator.Jump(SpatialValidatorImplementation.NestedValidator.LineStringEnd);
						return;
					default:
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.LineTo, SpatialValidatorImplementation.NestedValidator.PipelineCall.EndFigure, transition);
						return;
					}
				}
			}

			// Token: 0x02000074 RID: 116
			private class LineStringEndState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002E3 RID: 739 RVA: 0x00008384 File Offset: 0x00006584
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
					{
						validator.Return();
						return;
					}
					SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
				}
			}

			// Token: 0x02000075 RID: 117
			private class PolygonStartState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002E5 RID: 741 RVA: 0x000083B0 File Offset: 0x000065B0
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFigure)
					{
						validator.Jump(SpatialValidatorImplementation.NestedValidator.PolygonBuilding);
						return;
					}
					if (transition != SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
					{
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFigure, SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
						return;
					}
					validator.ringCount = 0;
					validator.Return();
				}
			}

			// Token: 0x02000076 RID: 118
			private class PolygonBuildingState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002E7 RID: 743 RVA: 0x000083F8 File Offset: 0x000065F8
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					switch (transition)
					{
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.LineTo:
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.EndFigure:
						validator.ringCount++;
						if (validator.processingGeography)
						{
							SpatialValidatorImplementation.NestedValidator.ValidateGeographyPolygon(validator.pointCount, validator.initialFirstCoordinate, validator.initialSecondCoordinate, validator.mostRecentFirstCoordinate, validator.mostRecentSecondCoordinate);
						}
						else
						{
							SpatialValidatorImplementation.NestedValidator.ValidateGeometryPolygon(validator.pointCount, validator.initialFirstCoordinate, validator.initialSecondCoordinate, validator.mostRecentFirstCoordinate, validator.mostRecentSecondCoordinate);
						}
						validator.Jump(SpatialValidatorImplementation.NestedValidator.PolygonStart);
						return;
					default:
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.LineTo, SpatialValidatorImplementation.NestedValidator.PipelineCall.EndFigure, transition);
						return;
					}
				}
			}

			// Token: 0x02000077 RID: 119
			private class MultiPointState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002E9 RID: 745 RVA: 0x00008498 File Offset: 0x00006698
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					switch (transition)
					{
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem:
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.Begin:
						break;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPoint:
						validator.Call(SpatialValidatorImplementation.NestedValidator.PointStart);
						return;
					default:
						if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
						{
							validator.Return();
							return;
						}
						break;
					}
					SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem, SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPoint, SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
				}
			}

			// Token: 0x02000078 RID: 120
			private class MultiLineStringState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002EB RID: 747 RVA: 0x000084E8 File Offset: 0x000066E8
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem)
					{
						return;
					}
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginLineString)
					{
						validator.Call(SpatialValidatorImplementation.NestedValidator.LineStringStart);
						return;
					}
					if (transition != SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
					{
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem, SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginLineString, SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
						return;
					}
					validator.Return();
				}
			}

			// Token: 0x02000079 RID: 121
			private class MultiPolygonState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002ED RID: 749 RVA: 0x0000852C File Offset: 0x0000672C
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem)
					{
						return;
					}
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPolygon)
					{
						validator.Call(SpatialValidatorImplementation.NestedValidator.PolygonStart);
						return;
					}
					if (transition != SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
					{
						SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem, SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPolygon, SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
						return;
					}
					validator.Return();
				}
			}

			// Token: 0x0200007A RID: 122
			private class CollectionState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002EF RID: 751 RVA: 0x00008570 File Offset: 0x00006770
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					switch (transition)
					{
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem:
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPoint:
						validator.Call(SpatialValidatorImplementation.NestedValidator.PointStart);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginLineString:
						validator.Call(SpatialValidatorImplementation.NestedValidator.LineStringStart);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginPolygon:
						validator.Call(SpatialValidatorImplementation.NestedValidator.PolygonStart);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiPoint:
						validator.Call(SpatialValidatorImplementation.NestedValidator.MultiPoint);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiLineString:
						validator.Call(SpatialValidatorImplementation.NestedValidator.MultiLineString);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginMultiPolygon:
						validator.Call(SpatialValidatorImplementation.NestedValidator.MultiPolygon);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginCollection:
						validator.Call(SpatialValidatorImplementation.NestedValidator.Collection);
						return;
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.BeginFullGlobe:
						throw new FormatException(Strings.Validator_FullGlobeInCollection);
					case SpatialValidatorImplementation.NestedValidator.PipelineCall.End:
						validator.Return();
						return;
					}
					SpatialValidatorImplementation.NestedValidator.ValidatorState.ThrowExpected(SpatialValidatorImplementation.NestedValidator.PipelineCall.SetCoordinateSystem, SpatialValidatorImplementation.NestedValidator.PipelineCall.Begin, SpatialValidatorImplementation.NestedValidator.PipelineCall.End, transition);
				}
			}

			// Token: 0x0200007B RID: 123
			private class FullGlobeState : SpatialValidatorImplementation.NestedValidator.ValidatorState
			{
				// Token: 0x060002F1 RID: 753 RVA: 0x00008638 File Offset: 0x00006838
				internal override void ValidateTransition(SpatialValidatorImplementation.NestedValidator.PipelineCall transition, SpatialValidatorImplementation.NestedValidator validator)
				{
					if (transition == SpatialValidatorImplementation.NestedValidator.PipelineCall.End)
					{
						validator.Return();
						return;
					}
					throw new FormatException(Strings.Validator_FullGlobeCannotHaveElements);
				}
			}
		}
	}
}
