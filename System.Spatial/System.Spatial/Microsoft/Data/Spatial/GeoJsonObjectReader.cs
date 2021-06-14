using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Spatial;

namespace Microsoft.Data.Spatial
{
	// Token: 0x02000010 RID: 16
	internal class GeoJsonObjectReader : SpatialReader<IDictionary<string, object>>
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00002DC2 File Offset: 0x00000FC2
		internal GeoJsonObjectReader(SpatialPipeline destination) : base(destination)
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00002DCC File Offset: 0x00000FCC
		protected override void ReadGeographyImplementation(IDictionary<string, object> input)
		{
			TypeWashedPipeline pipeline = new TypeWashedToGeographyLongLatPipeline(base.Destination);
			new GeoJsonObjectReader.SendToTypeWashedPipeline(pipeline).SendToPipeline(input, true);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00002DF4 File Offset: 0x00000FF4
		protected override void ReadGeometryImplementation(IDictionary<string, object> input)
		{
			TypeWashedPipeline pipeline = new TypeWashedToGeometryPipeline(base.Destination);
			new GeoJsonObjectReader.SendToTypeWashedPipeline(pipeline).SendToPipeline(input, true);
		}

		// Token: 0x02000011 RID: 17
		private class SendToTypeWashedPipeline
		{
			// Token: 0x060000B4 RID: 180 RVA: 0x00002E1A File Offset: 0x0000101A
			internal SendToTypeWashedPipeline(TypeWashedPipeline pipeline)
			{
				this.pipeline = pipeline;
			}

			// Token: 0x060000B5 RID: 181 RVA: 0x00002E2C File Offset: 0x0000102C
			internal void SendToPipeline(IDictionary<string, object> members, bool requireSetCoordinates)
			{
				SpatialType spatialType = GeoJsonObjectReader.SendToTypeWashedPipeline.GetSpatialType(members);
				int? coordinateSystem;
				if (!GeoJsonObjectReader.SendToTypeWashedPipeline.TryGetCoordinateSystemId(members, out coordinateSystem))
				{
					coordinateSystem = null;
				}
				if (requireSetCoordinates || coordinateSystem != null)
				{
					this.pipeline.SetCoordinateSystem(coordinateSystem);
				}
				string memberName;
				if (spatialType == SpatialType.Collection)
				{
					memberName = "geometries";
				}
				else
				{
					memberName = "coordinates";
				}
				IEnumerable memberValueAsJsonArray = GeoJsonObjectReader.SendToTypeWashedPipeline.GetMemberValueAsJsonArray(members, memberName);
				this.SendShape(spatialType, memberValueAsJsonArray);
			}

			// Token: 0x060000B6 RID: 182 RVA: 0x00002E8C File Offset: 0x0000108C
			private static void SendArrayOfArray(IEnumerable array, Action<IEnumerable> send)
			{
				foreach (object value in array)
				{
					IEnumerable obj = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsJsonArray(value);
					send(obj);
				}
			}

			// Token: 0x060000B7 RID: 183 RVA: 0x00002EE4 File Offset: 0x000010E4
			private static double? ValueAsNullableDouble(object value)
			{
				if (value != null)
				{
					return new double?(GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsDouble(value));
				}
				return null;
			}

			// Token: 0x060000B8 RID: 184 RVA: 0x00002F0C File Offset: 0x0000110C
			private static double ValueAsDouble(object value)
			{
				if (value == null)
				{
					throw new FormatException(Strings.GeoJsonReader_InvalidNullElement);
				}
				if (value is string || value is IDictionary<string, object> || value is IEnumerable || value is bool)
				{
					throw new FormatException(Strings.GeoJsonReader_ExpectedNumeric);
				}
				return Convert.ToDouble(value, CultureInfo.InvariantCulture);
			}

			// Token: 0x060000B9 RID: 185 RVA: 0x00002F60 File Offset: 0x00001160
			private static IEnumerable ValueAsJsonArray(object value)
			{
				if (value == null)
				{
					return null;
				}
				if (value is string)
				{
					throw new FormatException(Strings.GeoJsonReader_ExpectedArray);
				}
				if (value is IDictionary || value is IDictionary<string, object>)
				{
					throw new FormatException(Strings.GeoJsonReader_ExpectedArray);
				}
				IEnumerable enumerable = value as IEnumerable;
				if (enumerable != null)
				{
					return enumerable;
				}
				throw new FormatException(Strings.GeoJsonReader_ExpectedArray);
			}

			// Token: 0x060000BA RID: 186 RVA: 0x00002FB8 File Offset: 0x000011B8
			private static IDictionary<string, object> ValueAsJsonObject(object value)
			{
				if (value == null)
				{
					return null;
				}
				IDictionary<string, object> dictionary = value as IDictionary<string, object>;
				if (dictionary != null)
				{
					return dictionary;
				}
				throw new FormatException(Strings.JsonReaderExtensions_CannotReadValueAsJsonObject(value));
			}

			// Token: 0x060000BB RID: 187 RVA: 0x00002FE4 File Offset: 0x000011E4
			private static string ValueAsString(string propertyName, object value)
			{
				if (value == null)
				{
					return null;
				}
				string text = value as string;
				if (text != null)
				{
					return text;
				}
				throw new FormatException(Strings.JsonReaderExtensions_CannotReadPropertyValueAsString(value, propertyName));
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00003010 File Offset: 0x00001210
			private static SpatialType GetSpatialType(IDictionary<string, object> geoJsonObject)
			{
				object value;
				if (geoJsonObject.TryGetValue("type", out value))
				{
					return GeoJsonObjectReader.SendToTypeWashedPipeline.ReadTypeName(GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsString("type", value));
				}
				throw new FormatException(Strings.GeoJsonReader_MissingRequiredMember("type"));
			}

			// Token: 0x060000BD RID: 189 RVA: 0x0000304C File Offset: 0x0000124C
			private static bool TryGetCoordinateSystemId(IDictionary<string, object> geoJsonObject, out int? epsgId)
			{
				object value;
				if (!geoJsonObject.TryGetValue("crs", out value))
				{
					epsgId = null;
					return false;
				}
				IDictionary<string, object> crsJsonObject = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsJsonObject(value);
				epsgId = new int?(GeoJsonObjectReader.SendToTypeWashedPipeline.GetCoordinateSystemIdFromCrs(crsJsonObject));
				return true;
			}

			// Token: 0x060000BE RID: 190 RVA: 0x0000308C File Offset: 0x0000128C
			private static int GetCoordinateSystemIdFromCrs(IDictionary<string, object> crsJsonObject)
			{
				object value;
				if (!crsJsonObject.TryGetValue("type", out value))
				{
					throw new FormatException(Strings.GeoJsonReader_MissingRequiredMember("type"));
				}
				string text = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsString("type", value);
				if (!string.Equals(text, "name", StringComparison.Ordinal))
				{
					throw new FormatException(Strings.GeoJsonReader_InvalidCrsType(text));
				}
				object value2;
				if (!crsJsonObject.TryGetValue("properties", out value2))
				{
					throw new FormatException(Strings.GeoJsonReader_MissingRequiredMember("properties"));
				}
				IDictionary<string, object> dictionary = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsJsonObject(value2);
				object value3;
				if (!dictionary.TryGetValue("name", out value3))
				{
					throw new FormatException(Strings.GeoJsonReader_MissingRequiredMember("name"));
				}
				string text2 = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsString("name", value3);
				int length = "EPSG".Length;
				int result;
				if (text2 == null || !text2.StartsWith("EPSG", StringComparison.Ordinal) || text2.Length == length || text2[length] != ':' || !int.TryParse(text2.Substring(length + 1), out result))
				{
					throw new FormatException(Strings.GeoJsonReader_InvalidCrsName(text2));
				}
				return result;
			}

			// Token: 0x060000BF RID: 191 RVA: 0x0000318C File Offset: 0x0000138C
			private static IEnumerable GetMemberValueAsJsonArray(IDictionary<string, object> geoJsonObject, string memberName)
			{
				object value;
				if (geoJsonObject.TryGetValue(memberName, out value))
				{
					return GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsJsonArray(value);
				}
				throw new FormatException(Strings.GeoJsonReader_MissingRequiredMember(memberName));
			}

			// Token: 0x060000C0 RID: 192 RVA: 0x000031B8 File Offset: 0x000013B8
			private static bool EnumerableAny(IEnumerable enumerable)
			{
				IEnumerator enumerator = enumerable.GetEnumerator();
				return enumerator.MoveNext();
			}

			// Token: 0x060000C1 RID: 193 RVA: 0x000031D4 File Offset: 0x000013D4
			private static SpatialType ReadTypeName(string typeName)
			{
				switch (typeName)
				{
				case "Point":
					return SpatialType.Point;
				case "LineString":
					return SpatialType.LineString;
				case "Polygon":
					return SpatialType.Polygon;
				case "MultiPoint":
					return SpatialType.MultiPoint;
				case "MultiLineString":
					return SpatialType.MultiLineString;
				case "MultiPolygon":
					return SpatialType.MultiPolygon;
				case "GeometryCollection":
					return SpatialType.Collection;
				}
				throw new FormatException(Strings.GeoJsonReader_InvalidTypeName(typeName));
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x000032A1 File Offset: 0x000014A1
			private void SendShape(SpatialType spatialType, IEnumerable contentMembers)
			{
				this.pipeline.BeginGeo(spatialType);
				this.SendCoordinates(spatialType, contentMembers);
				this.pipeline.EndGeo();
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x000032C4 File Offset: 0x000014C4
			private void SendCoordinates(SpatialType spatialType, IEnumerable contentMembers)
			{
				if (GeoJsonObjectReader.SendToTypeWashedPipeline.EnumerableAny(contentMembers))
				{
					switch (spatialType)
					{
					case SpatialType.Point:
						this.SendPoint(contentMembers);
						return;
					case SpatialType.LineString:
						this.SendLineString(contentMembers);
						return;
					case SpatialType.Polygon:
						this.SendPolygon(contentMembers);
						return;
					case SpatialType.MultiPoint:
						this.SendMultiShape(SpatialType.Point, contentMembers);
						return;
					case SpatialType.MultiLineString:
						this.SendMultiShape(SpatialType.LineString, contentMembers);
						return;
					case SpatialType.MultiPolygon:
						this.SendMultiShape(SpatialType.Polygon, contentMembers);
						return;
					case SpatialType.Collection:
						foreach (object obj in contentMembers)
						{
							IDictionary<string, object> members = (IDictionary<string, object>)obj;
							this.SendToPipeline(members, false);
						}
						break;
					default:
						return;
					}
				}
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00003380 File Offset: 0x00001580
			private void SendPoint(IEnumerable coordinates)
			{
				this.SendPosition(coordinates, true);
				this.pipeline.EndFigure();
			}

			// Token: 0x060000C5 RID: 197 RVA: 0x00003395 File Offset: 0x00001595
			private void SendLineString(IEnumerable coordinates)
			{
				this.SendPositionArray(coordinates);
			}

			// Token: 0x060000C6 RID: 198 RVA: 0x000033A7 File Offset: 0x000015A7
			private void SendPolygon(IEnumerable coordinates)
			{
				GeoJsonObjectReader.SendToTypeWashedPipeline.SendArrayOfArray(coordinates, delegate(IEnumerable positionArray)
				{
					this.SendPositionArray(positionArray);
				});
			}

			// Token: 0x060000C7 RID: 199 RVA: 0x000033D8 File Offset: 0x000015D8
			private void SendMultiShape(SpatialType containedSpatialType, IEnumerable coordinates)
			{
				GeoJsonObjectReader.SendToTypeWashedPipeline.SendArrayOfArray(coordinates, delegate(IEnumerable containedShapeCoordinates)
				{
					this.SendShape(containedSpatialType, containedShapeCoordinates);
				});
			}

			// Token: 0x060000C8 RID: 200 RVA: 0x00003438 File Offset: 0x00001638
			private void SendPositionArray(IEnumerable positionArray)
			{
				bool first = true;
				GeoJsonObjectReader.SendToTypeWashedPipeline.SendArrayOfArray(positionArray, delegate(IEnumerable array)
				{
					this.SendPosition(array, first);
					if (first)
					{
						first = false;
					}
				});
				this.pipeline.EndFigure();
			}

			// Token: 0x060000C9 RID: 201 RVA: 0x00003478 File Offset: 0x00001678
			private void SendPosition(IEnumerable positionElements, bool first)
			{
				int num = 0;
				double coordinate = 0.0;
				double coordinate2 = 0.0;
				double? coordinate3 = null;
				double? coordinate4 = null;
				foreach (object value in positionElements)
				{
					num++;
					switch (num)
					{
					case 1:
						coordinate = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsDouble(value);
						break;
					case 2:
						coordinate2 = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsDouble(value);
						break;
					case 3:
						coordinate3 = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsNullableDouble(value);
						break;
					case 4:
						coordinate4 = GeoJsonObjectReader.SendToTypeWashedPipeline.ValueAsNullableDouble(value);
						break;
					}
				}
				if (num < 2 || num > 4)
				{
					throw new FormatException(Strings.GeoJsonReader_InvalidPosition);
				}
				if (first)
				{
					this.pipeline.BeginFigure(coordinate, coordinate2, coordinate3, coordinate4);
					return;
				}
				this.pipeline.LineTo(coordinate, coordinate2, coordinate3, coordinate4);
			}

			// Token: 0x04000012 RID: 18
			private TypeWashedPipeline pipeline;
		}
	}
}
