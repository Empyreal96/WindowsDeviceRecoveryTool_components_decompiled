using System;
using System.Linq;
using System.Text;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000125 RID: 293
	public static class ToTraceStringExtensionMethods
	{
		// Token: 0x060005B3 RID: 1459 RVA: 0x0000EB09 File Offset: 0x0000CD09
		public static string ToTraceString(this IEdmSchemaType schemaType)
		{
			return schemaType.ToTraceString();
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0000EB11 File Offset: 0x0000CD11
		public static string ToTraceString(this IEdmSchemaElement schemaElement)
		{
			return schemaElement.FullName();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0000EB1C File Offset: 0x0000CD1C
		public static string ToTraceString(this IEdmType type)
		{
			EdmUtil.CheckArgumentNull<IEdmType>(type, "type");
			switch (type.TypeKind)
			{
			case EdmTypeKind.Row:
				return ((IEdmRowType)type).ToTraceString();
			case EdmTypeKind.Collection:
				return ((IEdmCollectionType)type).ToTraceString();
			case EdmTypeKind.EntityReference:
				return ((IEdmEntityReferenceType)type).ToTraceString();
			default:
			{
				IEdmSchemaType edmSchemaType = type as IEdmSchemaType;
				if (edmSchemaType == null)
				{
					return "UnknownType";
				}
				return edmSchemaType.ToTraceString();
			}
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000EB8C File Offset: 0x0000CD8C
		public static string ToTraceString(this IEdmTypeReference type)
		{
			EdmUtil.CheckArgumentNull<IEdmTypeReference>(type, "type");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			if (type.Definition != null)
			{
				stringBuilder.Append(type.Definition.ToTraceString());
				stringBuilder.AppendKeyValue("Nullable", type.IsNullable.ToString());
				if (type.IsPrimitive())
				{
					stringBuilder.AppendFacets(type.AsPrimitive());
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0000EC0C File Offset: 0x0000CE0C
		public static string ToTraceString(this IEdmProperty property)
		{
			EdmUtil.CheckArgumentNull<IEdmProperty>(property, "property");
			return ((property.Name != null) ? property.Name : "") + ":" + ((property.Type != null) ? property.Type.ToTraceString() : "");
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000EC60 File Offset: 0x0000CE60
		private static string ToTraceString(this IEdmEntityReferenceType type)
		{
			return string.Concat(new object[]
			{
				EdmTypeKind.EntityReference.ToString(),
				'(',
				(type.EntityType != null) ? type.EntityType.ToTraceString() : "",
				')'
			});
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0000ECBC File Offset: 0x0000CEBC
		private static string ToTraceString(this IEdmCollectionType type)
		{
			return string.Concat(new object[]
			{
				EdmTypeKind.Collection.ToString(),
				'(',
				(type.ElementType != null) ? type.ElementType.ToTraceString() : "",
				')'
			});
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000ED18 File Offset: 0x0000CF18
		private static string ToTraceString(this IEdmRowType type)
		{
			StringBuilder stringBuilder = new StringBuilder(EdmTypeKind.Row.ToString());
			stringBuilder.Append('(');
			if (type.Properties().Any<IEdmProperty>())
			{
				IEdmProperty obj = type.Properties().Last<IEdmProperty>();
				foreach (IEdmProperty edmProperty in type.Properties())
				{
					if (edmProperty != null)
					{
						stringBuilder.Append(edmProperty.ToTraceString());
						if (!edmProperty.Equals(obj))
						{
							stringBuilder.Append(", ");
						}
					}
				}
			}
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0000EDC8 File Offset: 0x0000CFC8
		private static void AppendFacets(this StringBuilder sb, IEdmPrimitiveTypeReference type)
		{
			switch (type.PrimitiveKind())
			{
			case EdmPrimitiveTypeKind.Binary:
				sb.AppendBinaryFacets(type.AsBinary());
				return;
			case EdmPrimitiveTypeKind.Boolean:
			case EdmPrimitiveTypeKind.Byte:
			case EdmPrimitiveTypeKind.Double:
			case EdmPrimitiveTypeKind.Guid:
			case EdmPrimitiveTypeKind.Int16:
			case EdmPrimitiveTypeKind.Int32:
			case EdmPrimitiveTypeKind.Int64:
			case EdmPrimitiveTypeKind.SByte:
			case EdmPrimitiveTypeKind.Single:
			case EdmPrimitiveTypeKind.Stream:
				break;
			case EdmPrimitiveTypeKind.DateTime:
			case EdmPrimitiveTypeKind.DateTimeOffset:
			case EdmPrimitiveTypeKind.Time:
				sb.AppendTemporalFacets(type.AsTemporal());
				return;
			case EdmPrimitiveTypeKind.Decimal:
				sb.AppendDecimalFacets(type.AsDecimal());
				return;
			case EdmPrimitiveTypeKind.String:
				sb.AppendStringFacets(type.AsString());
				return;
			case EdmPrimitiveTypeKind.Geography:
			case EdmPrimitiveTypeKind.GeographyPoint:
			case EdmPrimitiveTypeKind.GeographyLineString:
			case EdmPrimitiveTypeKind.GeographyPolygon:
			case EdmPrimitiveTypeKind.GeographyCollection:
			case EdmPrimitiveTypeKind.GeographyMultiPolygon:
			case EdmPrimitiveTypeKind.GeographyMultiLineString:
			case EdmPrimitiveTypeKind.GeographyMultiPoint:
			case EdmPrimitiveTypeKind.Geometry:
			case EdmPrimitiveTypeKind.GeometryPoint:
			case EdmPrimitiveTypeKind.GeometryLineString:
			case EdmPrimitiveTypeKind.GeometryPolygon:
			case EdmPrimitiveTypeKind.GeometryCollection:
			case EdmPrimitiveTypeKind.GeometryMultiPolygon:
			case EdmPrimitiveTypeKind.GeometryMultiLineString:
			case EdmPrimitiveTypeKind.GeometryMultiPoint:
				sb.AppendSpatialFacets(type.AsSpatial());
				break;
			default:
				return;
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0000EEA8 File Offset: 0x0000D0A8
		private static void AppendBinaryFacets(this StringBuilder sb, IEdmBinaryTypeReference type)
		{
			sb.AppendKeyValue("FixedLength", type.IsFixedLength.ToString());
			if (type.IsUnbounded || type.MaxLength != null)
			{
				sb.AppendKeyValue("MaxLength", type.IsUnbounded ? "Max" : type.MaxLength.ToString());
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0000EF1C File Offset: 0x0000D11C
		private static void AppendStringFacets(this StringBuilder sb, IEdmStringTypeReference type)
		{
			if (type.IsFixedLength != null)
			{
				sb.AppendKeyValue("FixedLength", type.IsFixedLength.ToString());
			}
			if (type.IsUnbounded || type.MaxLength != null)
			{
				sb.AppendKeyValue("MaxLength", type.IsUnbounded ? "Max" : type.MaxLength.ToString());
			}
			if (type.IsUnicode != null)
			{
				sb.AppendKeyValue("Unicode", type.IsUnicode.ToString());
			}
			if (type.Collation != null)
			{
				sb.AppendKeyValue("Collation", type.Collation.ToString());
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
		private static void AppendTemporalFacets(this StringBuilder sb, IEdmTemporalTypeReference type)
		{
			if (type.Precision != null)
			{
				sb.AppendKeyValue("Precision", type.Precision.ToString());
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0000F02C File Offset: 0x0000D22C
		private static void AppendDecimalFacets(this StringBuilder sb, IEdmDecimalTypeReference type)
		{
			if (type.Precision != null)
			{
				sb.AppendKeyValue("Precision", type.Precision.ToString());
			}
			if (type.Scale != null)
			{
				sb.AppendKeyValue("Scale", type.Scale.ToString());
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0000F098 File Offset: 0x0000D298
		private static void AppendSpatialFacets(this StringBuilder sb, IEdmSpatialTypeReference type)
		{
			sb.AppendKeyValue("SRID", (type.SpatialReferenceIdentifier != null) ? type.SpatialReferenceIdentifier.ToString() : "Variable");
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000F0DB File Offset: 0x0000D2DB
		private static void AppendKeyValue(this StringBuilder sb, string key, string value)
		{
			sb.Append(' ');
			sb.Append(key);
			sb.Append('=');
			sb.Append(value);
		}
	}
}
