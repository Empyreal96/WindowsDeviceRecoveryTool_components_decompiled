using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Library.Annotations;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001E9 RID: 489
	public class EdmCoreModel : EdmElement, IEdmModel, IEdmElement, IEdmValidCoreModelElement
	{
		// Token: 0x06000B74 RID: 2932 RVA: 0x00021330 File Offset: 0x0001F530
		private EdmCoreModel()
		{
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Double", EdmPrimitiveTypeKind.Double);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType2 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Single", EdmPrimitiveTypeKind.Single);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType3 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Int64", EdmPrimitiveTypeKind.Int64);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType4 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Int32", EdmPrimitiveTypeKind.Int32);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType5 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Int16", EdmPrimitiveTypeKind.Int16);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType6 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "SByte", EdmPrimitiveTypeKind.SByte);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType7 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Byte", EdmPrimitiveTypeKind.Byte);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType8 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Boolean", EdmPrimitiveTypeKind.Boolean);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType9 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Guid", EdmPrimitiveTypeKind.Guid);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType10 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Time", EdmPrimitiveTypeKind.Time);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType11 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "DateTime", EdmPrimitiveTypeKind.DateTime);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType12 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "DateTimeOffset", EdmPrimitiveTypeKind.DateTimeOffset);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType13 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Decimal", EdmPrimitiveTypeKind.Decimal);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType14 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Binary", EdmPrimitiveTypeKind.Binary);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType15 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "String", EdmPrimitiveTypeKind.String);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType16 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Stream", EdmPrimitiveTypeKind.Stream);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType17 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Geography", EdmPrimitiveTypeKind.Geography);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType18 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeographyPoint", EdmPrimitiveTypeKind.GeographyPoint);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType19 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeographyLineString", EdmPrimitiveTypeKind.GeographyLineString);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType20 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeographyPolygon", EdmPrimitiveTypeKind.GeographyPolygon);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType21 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeographyCollection", EdmPrimitiveTypeKind.GeographyCollection);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType22 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeographyMultiPolygon", EdmPrimitiveTypeKind.GeographyMultiPolygon);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType23 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeographyMultiLineString", EdmPrimitiveTypeKind.GeographyMultiLineString);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType24 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeographyMultiPoint", EdmPrimitiveTypeKind.GeographyMultiPoint);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType25 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "Geometry", EdmPrimitiveTypeKind.Geometry);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType26 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeometryPoint", EdmPrimitiveTypeKind.GeometryPoint);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType27 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeometryLineString", EdmPrimitiveTypeKind.GeometryLineString);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType28 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeometryPolygon", EdmPrimitiveTypeKind.GeometryPolygon);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType29 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeometryCollection", EdmPrimitiveTypeKind.GeometryCollection);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType30 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeometryMultiPolygon", EdmPrimitiveTypeKind.GeometryMultiPolygon);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType31 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeometryMultiLineString", EdmPrimitiveTypeKind.GeometryMultiLineString);
			EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType32 = new EdmCoreModel.EdmValidCoreModelPrimitiveType("Edm", "GeometryMultiPoint", EdmPrimitiveTypeKind.GeometryMultiPoint);
			this.primitiveTypes = new EdmCoreModel.EdmValidCoreModelPrimitiveType[]
			{
				edmValidCoreModelPrimitiveType14,
				edmValidCoreModelPrimitiveType8,
				edmValidCoreModelPrimitiveType7,
				edmValidCoreModelPrimitiveType11,
				edmValidCoreModelPrimitiveType12,
				edmValidCoreModelPrimitiveType13,
				edmValidCoreModelPrimitiveType,
				edmValidCoreModelPrimitiveType9,
				edmValidCoreModelPrimitiveType5,
				edmValidCoreModelPrimitiveType4,
				edmValidCoreModelPrimitiveType3,
				edmValidCoreModelPrimitiveType6,
				edmValidCoreModelPrimitiveType2,
				edmValidCoreModelPrimitiveType16,
				edmValidCoreModelPrimitiveType15,
				edmValidCoreModelPrimitiveType10,
				edmValidCoreModelPrimitiveType17,
				edmValidCoreModelPrimitiveType18,
				edmValidCoreModelPrimitiveType19,
				edmValidCoreModelPrimitiveType20,
				edmValidCoreModelPrimitiveType21,
				edmValidCoreModelPrimitiveType22,
				edmValidCoreModelPrimitiveType23,
				edmValidCoreModelPrimitiveType24,
				edmValidCoreModelPrimitiveType25,
				edmValidCoreModelPrimitiveType26,
				edmValidCoreModelPrimitiveType27,
				edmValidCoreModelPrimitiveType28,
				edmValidCoreModelPrimitiveType29,
				edmValidCoreModelPrimitiveType30,
				edmValidCoreModelPrimitiveType31,
				edmValidCoreModelPrimitiveType32
			};
			foreach (EdmCoreModel.EdmValidCoreModelPrimitiveType edmValidCoreModelPrimitiveType33 in this.primitiveTypes)
			{
				this.primitiveTypeKinds[edmValidCoreModelPrimitiveType33.Name] = edmValidCoreModelPrimitiveType33.PrimitiveKind;
				this.primitiveTypeKinds[edmValidCoreModelPrimitiveType33.Namespace + '.' + edmValidCoreModelPrimitiveType33.Name] = edmValidCoreModelPrimitiveType33.PrimitiveKind;
				this.primitiveTypesByKind[edmValidCoreModelPrimitiveType33.PrimitiveKind] = edmValidCoreModelPrimitiveType33;
				this.primitiveTypeByName[edmValidCoreModelPrimitiveType33.Namespace + '.' + edmValidCoreModelPrimitiveType33.Name] = edmValidCoreModelPrimitiveType33;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0002174F File Offset: 0x0001F94F
		public static string Namespace
		{
			get
			{
				return "Edm";
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x00021756 File Offset: 0x0001F956
		public IEnumerable<IEdmSchemaElement> SchemaElements
		{
			get
			{
				return this.primitiveTypes;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x0002175E File Offset: 0x0001F95E
		public IEnumerable<IEdmVocabularyAnnotation> VocabularyAnnotations
		{
			get
			{
				return Enumerable.Empty<IEdmVocabularyAnnotation>();
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00021765 File Offset: 0x0001F965
		public IEnumerable<IEdmModel> ReferencedModels
		{
			get
			{
				return Enumerable.Empty<IEdmModel>();
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0002176C File Offset: 0x0001F96C
		public IEdmDirectValueAnnotationsManager DirectValueAnnotationsManager
		{
			get
			{
				return this.annotationsManager;
			}
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00021774 File Offset: 0x0001F974
		public static IEdmCollectionTypeReference GetCollection(IEdmTypeReference elementType)
		{
			return new EdmCollectionTypeReference(new EdmCollectionType(elementType), false);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00021784 File Offset: 0x0001F984
		public IEdmSchemaType FindDeclaredType(string qualifiedName)
		{
			EdmCoreModel.EdmValidCoreModelPrimitiveType result;
			if (!this.primitiveTypeByName.TryGetValue(qualifiedName, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000217A4 File Offset: 0x0001F9A4
		public IEdmValueTerm FindDeclaredValueTerm(string qualifiedName)
		{
			return null;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x000217A7 File Offset: 0x0001F9A7
		public IEnumerable<IEdmFunction> FindDeclaredFunctions(string qualifiedName)
		{
			return Enumerable.Empty<IEdmFunction>();
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x000217AE File Offset: 0x0001F9AE
		public IEdmEntityContainer FindDeclaredEntityContainer(string name)
		{
			return null;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x000217B1 File Offset: 0x0001F9B1
		public IEdmPrimitiveType GetPrimitiveType(EdmPrimitiveTypeKind kind)
		{
			return this.GetCoreModelPrimitiveType(kind);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x000217BC File Offset: 0x0001F9BC
		public EdmPrimitiveTypeKind GetPrimitiveTypeKind(string typeName)
		{
			EdmPrimitiveTypeKind result;
			if (!this.primitiveTypeKinds.TryGetValue(typeName, out result))
			{
				return EdmPrimitiveTypeKind.None;
			}
			return result;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x000217DC File Offset: 0x0001F9DC
		public IEdmPrimitiveTypeReference GetPrimitive(EdmPrimitiveTypeKind kind, bool isNullable)
		{
			IEdmPrimitiveType coreModelPrimitiveType = this.GetCoreModelPrimitiveType(kind);
			if (coreModelPrimitiveType != null)
			{
				return coreModelPrimitiveType.GetPrimitiveTypeReference(isNullable);
			}
			throw new InvalidOperationException(Strings.EdmPrimitive_UnexpectedKind);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00021806 File Offset: 0x0001FA06
		public IEdmPrimitiveTypeReference GetInt16(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Int16), isNullable);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00021816 File Offset: 0x0001FA16
		public IEdmPrimitiveTypeReference GetInt32(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Int32), isNullable);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00021826 File Offset: 0x0001FA26
		public IEdmPrimitiveTypeReference GetInt64(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Int64), isNullable);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00021836 File Offset: 0x0001FA36
		public IEdmPrimitiveTypeReference GetBoolean(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Boolean), isNullable);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00021845 File Offset: 0x0001FA45
		public IEdmPrimitiveTypeReference GetByte(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Byte), isNullable);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00021854 File Offset: 0x0001FA54
		public IEdmPrimitiveTypeReference GetSByte(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.SByte), isNullable);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00021864 File Offset: 0x0001FA64
		public IEdmPrimitiveTypeReference GetGuid(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Guid), isNullable);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00021873 File Offset: 0x0001FA73
		public IEdmTemporalTypeReference GetDateTime(bool isNullable)
		{
			return new EdmTemporalTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.DateTime), isNullable);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00021882 File Offset: 0x0001FA82
		public IEdmTemporalTypeReference GetDateTimeOffset(bool isNullable)
		{
			return new EdmTemporalTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.DateTimeOffset), isNullable);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00021891 File Offset: 0x0001FA91
		public IEdmTemporalTypeReference GetTime(bool isNullable)
		{
			return new EdmTemporalTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Time), isNullable);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000218A1 File Offset: 0x0001FAA1
		public IEdmDecimalTypeReference GetDecimal(int? precision, int? scale, bool isNullable)
		{
			if (precision != null || scale != null)
			{
				return new EdmDecimalTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Decimal), isNullable, precision, scale);
			}
			return new EdmDecimalTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Decimal), isNullable);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000218D2 File Offset: 0x0001FAD2
		public IEdmDecimalTypeReference GetDecimal(bool isNullable)
		{
			return new EdmDecimalTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Decimal), isNullable);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x000218E1 File Offset: 0x0001FAE1
		public IEdmPrimitiveTypeReference GetSingle(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Single), isNullable);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x000218F1 File Offset: 0x0001FAF1
		public IEdmPrimitiveTypeReference GetDouble(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Double), isNullable);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00021900 File Offset: 0x0001FB00
		public IEdmPrimitiveTypeReference GetStream(bool isNullable)
		{
			return new EdmPrimitiveTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Stream), isNullable);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00021910 File Offset: 0x0001FB10
		public IEdmTemporalTypeReference GetTemporal(EdmPrimitiveTypeKind kind, int? precision, bool isNullable)
		{
			switch (kind)
			{
			case EdmPrimitiveTypeKind.DateTime:
			case EdmPrimitiveTypeKind.DateTimeOffset:
				break;
			default:
				if (kind != EdmPrimitiveTypeKind.Time)
				{
					throw new InvalidOperationException(Strings.EdmPrimitive_UnexpectedKind);
				}
				break;
			}
			return new EdmTemporalTypeReference(this.GetCoreModelPrimitiveType(kind), isNullable, precision);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00021950 File Offset: 0x0001FB50
		public IEdmTemporalTypeReference GetTemporal(EdmPrimitiveTypeKind kind, bool isNullable)
		{
			switch (kind)
			{
			case EdmPrimitiveTypeKind.DateTime:
			case EdmPrimitiveTypeKind.DateTimeOffset:
				break;
			default:
				if (kind != EdmPrimitiveTypeKind.Time)
				{
					throw new InvalidOperationException(Strings.EdmPrimitive_UnexpectedKind);
				}
				break;
			}
			return new EdmTemporalTypeReference(this.GetCoreModelPrimitiveType(kind), isNullable);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002198C File Offset: 0x0001FB8C
		public IEdmBinaryTypeReference GetBinary(bool isUnbounded, int? maxLength, bool? isFixedLength, bool isNullable)
		{
			return new EdmBinaryTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Binary), isNullable, isUnbounded, maxLength, isFixedLength);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002199F File Offset: 0x0001FB9F
		public IEdmBinaryTypeReference GetBinary(bool isNullable)
		{
			return new EdmBinaryTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.Binary), isNullable);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000219B0 File Offset: 0x0001FBB0
		public IEdmSpatialTypeReference GetSpatial(EdmPrimitiveTypeKind kind, int? spatialReferenceIdentifier, bool isNullable)
		{
			switch (kind)
			{
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
				return new EdmSpatialTypeReference(this.GetCoreModelPrimitiveType(kind), isNullable, spatialReferenceIdentifier);
			default:
				throw new InvalidOperationException(Strings.EdmPrimitive_UnexpectedKind);
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00021A24 File Offset: 0x0001FC24
		public IEdmSpatialTypeReference GetSpatial(EdmPrimitiveTypeKind kind, bool isNullable)
		{
			switch (kind)
			{
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
				return new EdmSpatialTypeReference(this.GetCoreModelPrimitiveType(kind), isNullable);
			default:
				throw new InvalidOperationException(Strings.EdmPrimitive_UnexpectedKind);
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00021A96 File Offset: 0x0001FC96
		public IEdmStringTypeReference GetString(bool isUnbounded, int? maxLength, bool? isFixedLength, bool? isUnicode, string collation, bool isNullable)
		{
			return new EdmStringTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.String), isNullable, isUnbounded, maxLength, isFixedLength, isUnicode, collation);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00021AAE File Offset: 0x0001FCAE
		public IEdmStringTypeReference GetString(bool isNullable)
		{
			return new EdmStringTypeReference(this.GetCoreModelPrimitiveType(EdmPrimitiveTypeKind.String), isNullable);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00021ABE File Offset: 0x0001FCBE
		public IEnumerable<IEdmVocabularyAnnotation> FindDeclaredVocabularyAnnotations(IEdmVocabularyAnnotatable element)
		{
			return Enumerable.Empty<IEdmVocabularyAnnotation>();
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00021AC5 File Offset: 0x0001FCC5
		public IEnumerable<IEdmStructuredType> FindDirectlyDerivedTypes(IEdmStructuredType baseType)
		{
			return Enumerable.Empty<IEdmStructuredType>();
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00021ACC File Offset: 0x0001FCCC
		private EdmCoreModel.EdmValidCoreModelPrimitiveType GetCoreModelPrimitiveType(EdmPrimitiveTypeKind kind)
		{
			EdmCoreModel.EdmValidCoreModelPrimitiveType result;
			if (!this.primitiveTypesByKind.TryGetValue(kind, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x04000582 RID: 1410
		private const string EdmNamespace = "Edm";

		// Token: 0x04000583 RID: 1411
		public static readonly EdmCoreModel Instance = new EdmCoreModel();

		// Token: 0x04000584 RID: 1412
		private readonly EdmCoreModel.EdmValidCoreModelPrimitiveType[] primitiveTypes;

		// Token: 0x04000585 RID: 1413
		private readonly Dictionary<string, EdmPrimitiveTypeKind> primitiveTypeKinds = new Dictionary<string, EdmPrimitiveTypeKind>();

		// Token: 0x04000586 RID: 1414
		private readonly Dictionary<EdmPrimitiveTypeKind, EdmCoreModel.EdmValidCoreModelPrimitiveType> primitiveTypesByKind = new Dictionary<EdmPrimitiveTypeKind, EdmCoreModel.EdmValidCoreModelPrimitiveType>();

		// Token: 0x04000587 RID: 1415
		private readonly Dictionary<string, EdmCoreModel.EdmValidCoreModelPrimitiveType> primitiveTypeByName = new Dictionary<string, EdmCoreModel.EdmValidCoreModelPrimitiveType>();

		// Token: 0x04000588 RID: 1416
		private readonly IEdmDirectValueAnnotationsManager annotationsManager = new EdmDirectValueAnnotationsManager();

		// Token: 0x020001EA RID: 490
		private sealed class EdmValidCoreModelPrimitiveType : EdmType, IEdmPrimitiveType, IEdmSchemaType, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmType, IEdmElement, IEdmValidCoreModelElement
		{
			// Token: 0x06000B9D RID: 2973 RVA: 0x00021AF8 File Offset: 0x0001FCF8
			public EdmValidCoreModelPrimitiveType(string namespaceName, string name, EdmPrimitiveTypeKind primitiveKind)
			{
				this.namespaceName = (namespaceName ?? string.Empty);
				this.name = (name ?? string.Empty);
				this.primitiveKind = primitiveKind;
			}

			// Token: 0x17000467 RID: 1127
			// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00021B27 File Offset: 0x0001FD27
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x17000468 RID: 1128
			// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00021B2F File Offset: 0x0001FD2F
			public string Namespace
			{
				get
				{
					return this.namespaceName;
				}
			}

			// Token: 0x17000469 RID: 1129
			// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00021B37 File Offset: 0x0001FD37
			public override EdmTypeKind TypeKind
			{
				get
				{
					return EdmTypeKind.Primitive;
				}
			}

			// Token: 0x1700046A RID: 1130
			// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00021B3A File Offset: 0x0001FD3A
			public EdmPrimitiveTypeKind PrimitiveKind
			{
				get
				{
					return this.primitiveKind;
				}
			}

			// Token: 0x1700046B RID: 1131
			// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00021B42 File Offset: 0x0001FD42
			public EdmSchemaElementKind SchemaElementKind
			{
				get
				{
					return EdmSchemaElementKind.TypeDefinition;
				}
			}

			// Token: 0x04000589 RID: 1417
			private readonly string namespaceName;

			// Token: 0x0400058A RID: 1418
			private readonly string name;

			// Token: 0x0400058B RID: 1419
			private readonly EdmPrimitiveTypeKind primitiveKind;
		}
	}
}
