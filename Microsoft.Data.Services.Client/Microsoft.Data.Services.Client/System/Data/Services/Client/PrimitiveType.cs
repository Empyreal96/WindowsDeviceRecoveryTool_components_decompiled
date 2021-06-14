using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Spatial;
using System.Xml.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace System.Data.Services.Client
{
	// Token: 0x020000AB RID: 171
	internal sealed class PrimitiveType
	{
		// Token: 0x06000581 RID: 1409 RVA: 0x00015048 File Offset: 0x00013248
		static PrimitiveType()
		{
			PrimitiveType.derivedPrimitiveTypeMapping = new Dictionary<Type, PrimitiveType>(EqualityComparer<Type>.Default);
			PrimitiveType.knownNonPrimitiveTypes = new HashSet<Type>(EqualityComparer<Type>.Default);
			PrimitiveType.InitializeTypes();
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00015096 File Offset: 0x00013296
		private PrimitiveType(Type clrType, string edmTypeName, EdmPrimitiveTypeKind primitiveKind, PrimitiveTypeConverter typeConverter, bool hasReverseMapping)
		{
			this.ClrType = clrType;
			this.EdmTypeName = edmTypeName;
			this.PrimitiveKind = primitiveKind;
			this.TypeConverter = typeConverter;
			this.HasReverseMapping = hasReverseMapping;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x000150C3 File Offset: 0x000132C3
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x000150CB File Offset: 0x000132CB
		internal Type ClrType { get; private set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x000150D4 File Offset: 0x000132D4
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x000150DC File Offset: 0x000132DC
		internal string EdmTypeName { get; private set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x000150E5 File Offset: 0x000132E5
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x000150ED File Offset: 0x000132ED
		internal PrimitiveTypeConverter TypeConverter { get; private set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x000150F6 File Offset: 0x000132F6
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x000150FE File Offset: 0x000132FE
		internal bool HasReverseMapping { get; private set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00015107 File Offset: 0x00013307
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x0001510F File Offset: 0x0001330F
		internal EdmPrimitiveTypeKind PrimitiveKind { get; private set; }

		// Token: 0x0600058D RID: 1421 RVA: 0x0001513C File Offset: 0x0001333C
		internal static bool TryGetPrimitiveType(Type clrType, out PrimitiveType ptype)
		{
			Type type = Nullable.GetUnderlyingType(clrType) ?? clrType;
			if (!PrimitiveType.TryGetWellKnownPrimitiveType(type, out ptype))
			{
				lock (PrimitiveType.knownNonPrimitiveTypes)
				{
					if (PrimitiveType.knownNonPrimitiveTypes.Contains(clrType))
					{
						ptype = null;
						return false;
					}
				}
				KeyValuePair<Type, PrimitiveType>[] array;
				lock (PrimitiveType.derivedPrimitiveTypeMapping)
				{
					if (PrimitiveType.derivedPrimitiveTypeMapping.TryGetValue(clrType, out ptype))
					{
						return true;
					}
					array = (from m in PrimitiveType.clrMapping
					where !m.Key.IsPrimitive() && !m.Key.IsSealed()
					select m).Concat(PrimitiveType.derivedPrimitiveTypeMapping).ToArray<KeyValuePair<Type, PrimitiveType>>();
				}
				KeyValuePair<Type, PrimitiveType> keyValuePair = new KeyValuePair<Type, PrimitiveType>(typeof(object), null);
				foreach (KeyValuePair<Type, PrimitiveType> keyValuePair2 in array)
				{
					if (type.IsSubclassOf(keyValuePair2.Key) && keyValuePair2.Key.IsSubclassOf(keyValuePair.Key))
					{
						keyValuePair = keyValuePair2;
					}
				}
				if (keyValuePair.Value == null)
				{
					lock (PrimitiveType.knownNonPrimitiveTypes)
					{
						PrimitiveType.knownNonPrimitiveTypes.Add(clrType);
					}
					return false;
				}
				ptype = keyValuePair.Value;
				lock (PrimitiveType.derivedPrimitiveTypeMapping)
				{
					PrimitiveType.derivedPrimitiveTypeMapping[type] = ptype;
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00015308 File Offset: 0x00013508
		internal static bool TryGetPrimitiveType(string edmTypeName, out PrimitiveType ptype)
		{
			return PrimitiveType.edmMapping.TryGetValue(edmTypeName, out ptype);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00015318 File Offset: 0x00013518
		internal static bool IsKnownType(Type type)
		{
			PrimitiveType primitiveType;
			return PrimitiveType.TryGetPrimitiveType(type, out primitiveType);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001532D File Offset: 0x0001352D
		internal static bool IsKnownNullableType(Type type)
		{
			return PrimitiveType.IsKnownType(Nullable.GetUnderlyingType(type) ?? type);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001533F File Offset: 0x0001353F
		internal static void DeleteKnownType(Type clrType, string edmTypeName)
		{
			PrimitiveType.clrMapping.Remove(clrType);
			if (edmTypeName != null)
			{
				PrimitiveType.edmMapping.Remove(edmTypeName);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001535C File Offset: 0x0001355C
		internal static void RegisterKnownType(Type clrType, string edmTypeName, EdmPrimitiveTypeKind primitiveKind, PrimitiveTypeConverter converter, bool twoWay)
		{
			PrimitiveType value = new PrimitiveType(clrType, edmTypeName, primitiveKind, converter, twoWay);
			PrimitiveType.clrMapping.Add(clrType, value);
			if (twoWay)
			{
				PrimitiveType.edmMapping.Add(edmTypeName, value);
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00015391 File Offset: 0x00013591
		internal IEdmPrimitiveType CreateEdmPrimitiveType()
		{
			return PrimitiveType.ClientEdmPrimitiveType.CreateType(this.PrimitiveKind);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x000153A0 File Offset: 0x000135A0
		private static void InitializeTypes()
		{
			PrimitiveType.RegisterKnownType(typeof(bool), "Edm.Boolean", EdmPrimitiveTypeKind.Boolean, new BooleanTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(byte), "Edm.Byte", EdmPrimitiveTypeKind.Byte, new ByteTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(byte[]), "Edm.Binary", EdmPrimitiveTypeKind.Binary, new ByteArrayTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(DateTime), "Edm.DateTime", EdmPrimitiveTypeKind.DateTime, new DateTimeTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(DateTimeOffset), "Edm.DateTimeOffset", EdmPrimitiveTypeKind.DateTimeOffset, new DateTimeOffsetTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(decimal), "Edm.Decimal", EdmPrimitiveTypeKind.Decimal, new DecimalTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(double), "Edm.Double", EdmPrimitiveTypeKind.Double, new DoubleTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(Guid), "Edm.Guid", EdmPrimitiveTypeKind.Guid, new GuidTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(short), "Edm.Int16", EdmPrimitiveTypeKind.Int16, new Int16TypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(int), "Edm.Int32", EdmPrimitiveTypeKind.Int32, new Int32TypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(long), "Edm.Int64", EdmPrimitiveTypeKind.Int64, new Int64TypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(float), "Edm.Single", EdmPrimitiveTypeKind.Single, new SingleTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(string), "Edm.String", EdmPrimitiveTypeKind.String, new StringTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(sbyte), "Edm.SByte", EdmPrimitiveTypeKind.SByte, new SByteTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(TimeSpan), "Edm.Time", EdmPrimitiveTypeKind.Time, new TimeSpanTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(Geography), "Edm.Geography", EdmPrimitiveTypeKind.Geography, new GeographyTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeographyPoint), "Edm.GeographyPoint", EdmPrimitiveTypeKind.GeographyPoint, new GeographyTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeographyLineString), "Edm.GeographyLineString", EdmPrimitiveTypeKind.GeographyLineString, new GeographyTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeographyPolygon), "Edm.GeographyPolygon", EdmPrimitiveTypeKind.GeographyPolygon, new GeographyTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeographyCollection), "Edm.GeographyCollection", EdmPrimitiveTypeKind.GeographyCollection, new GeographyTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeographyMultiPoint), "Edm.GeographyMultiPoint", EdmPrimitiveTypeKind.GeographyMultiPoint, new GeographyTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeographyMultiLineString), "Edm.GeographyMultiLineString", EdmPrimitiveTypeKind.GeographyMultiLineString, new GeographyTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeographyMultiPolygon), "Edm.GeographyMultiPolygon", EdmPrimitiveTypeKind.GeographyMultiPolygon, new GeographyTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(Geometry), "Edm.Geometry", EdmPrimitiveTypeKind.Geometry, new GeometryTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeometryPoint), "Edm.GeometryPoint", EdmPrimitiveTypeKind.GeometryPoint, new GeometryTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeometryLineString), "Edm.GeometryLineString", EdmPrimitiveTypeKind.GeometryLineString, new GeometryTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeometryPolygon), "Edm.GeometryPolygon", EdmPrimitiveTypeKind.GeometryPolygon, new GeometryTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeometryCollection), "Edm.GeometryCollection", EdmPrimitiveTypeKind.GeometryCollection, new GeometryTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeometryMultiPoint), "Edm.GeometryMultiPoint", EdmPrimitiveTypeKind.GeometryMultiPoint, new GeometryTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeometryMultiLineString), "Edm.GeometryMultiLineString", EdmPrimitiveTypeKind.GeometryMultiLineString, new GeometryTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(GeometryMultiPolygon), "Edm.GeometryMultiPolygon", EdmPrimitiveTypeKind.GeometryMultiPolygon, new GeometryTypeConverter(), true);
			PrimitiveType.RegisterKnownType(typeof(DataServiceStreamLink), "Edm.Stream", EdmPrimitiveTypeKind.Stream, new NamedStreamTypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(char), "Edm.String", EdmPrimitiveTypeKind.String, new CharTypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(char[]), "Edm.String", EdmPrimitiveTypeKind.String, new CharArrayTypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(Type), "Edm.String", EdmPrimitiveTypeKind.String, new ClrTypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(Uri), "Edm.String", EdmPrimitiveTypeKind.String, new UriTypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(XDocument), "Edm.String", EdmPrimitiveTypeKind.String, new XDocumentTypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(XElement), "Edm.String", EdmPrimitiveTypeKind.String, new XElementTypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(ushort), null, EdmPrimitiveTypeKind.String, new UInt16TypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(uint), null, EdmPrimitiveTypeKind.String, new UInt32TypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(ulong), null, EdmPrimitiveTypeKind.String, new UInt64TypeConverter(), false);
			PrimitiveType.RegisterKnownType(typeof(PrimitiveType.BinaryTypeSub), "Edm.Binary", EdmPrimitiveTypeKind.Binary, new BinaryTypeConverter(), false);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00015830 File Offset: 0x00013A30
		private static bool TryGetWellKnownPrimitiveType(Type clrType, out PrimitiveType ptype)
		{
			ptype = null;
			if (!PrimitiveType.clrMapping.TryGetValue(clrType, out ptype) && PrimitiveType.IsBinaryType(clrType))
			{
				ptype = PrimitiveType.clrMapping[typeof(PrimitiveType.BinaryTypeSub)];
			}
			return ptype != null;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001586C File Offset: 0x00013A6C
		private static bool IsBinaryType(Type type)
		{
			if (BinaryTypeConverter.BinaryType == null && type.Name == "Binary" && type.Namespace == "System.Data.Linq" && AssemblyName.ReferenceMatchesDefinition(type.Assembly.GetName(), new AssemblyName("System.Data.Linq")))
			{
				BinaryTypeConverter.BinaryType = type;
			}
			return type == BinaryTypeConverter.BinaryType;
		}

		// Token: 0x04000306 RID: 774
		private static readonly Dictionary<Type, PrimitiveType> clrMapping = new Dictionary<Type, PrimitiveType>(EqualityComparer<Type>.Default);

		// Token: 0x04000307 RID: 775
		private static readonly Dictionary<Type, PrimitiveType> derivedPrimitiveTypeMapping;

		// Token: 0x04000308 RID: 776
		private static readonly Dictionary<string, PrimitiveType> edmMapping = new Dictionary<string, PrimitiveType>(StringComparer.Ordinal);

		// Token: 0x04000309 RID: 777
		private static readonly HashSet<Type> knownNonPrimitiveTypes;

		// Token: 0x020000AC RID: 172
		private sealed class BinaryTypeSub
		{
		}

		// Token: 0x020000AD RID: 173
		private class ClientEdmPrimitiveType : EdmType, IEdmPrimitiveType, IEdmSchemaType, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmType, IEdmElement
		{
			// Token: 0x06000599 RID: 1433 RVA: 0x000158DF File Offset: 0x00013ADF
			private ClientEdmPrimitiveType(string namespaceName, string name, EdmPrimitiveTypeKind primitiveKind)
			{
				this.namespaceName = namespaceName;
				this.name = name;
				this.primitiveKind = primitiveKind;
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x0600059A RID: 1434 RVA: 0x000158FC File Offset: 0x00013AFC
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x0600059B RID: 1435 RVA: 0x00015904 File Offset: 0x00013B04
			public string Namespace
			{
				get
				{
					return this.namespaceName;
				}
			}

			// Token: 0x17000160 RID: 352
			// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001590C File Offset: 0x00013B0C
			public EdmPrimitiveTypeKind PrimitiveKind
			{
				get
				{
					return this.primitiveKind;
				}
			}

			// Token: 0x17000161 RID: 353
			// (get) Token: 0x0600059D RID: 1437 RVA: 0x00015914 File Offset: 0x00013B14
			public EdmSchemaElementKind SchemaElementKind
			{
				get
				{
					return EdmSchemaElementKind.TypeDefinition;
				}
			}

			// Token: 0x17000162 RID: 354
			// (get) Token: 0x0600059E RID: 1438 RVA: 0x00015917 File Offset: 0x00013B17
			public override EdmTypeKind TypeKind
			{
				get
				{
					return EdmTypeKind.Primitive;
				}
			}

			// Token: 0x0600059F RID: 1439 RVA: 0x0001591A File Offset: 0x00013B1A
			public static IEdmPrimitiveType CreateType(EdmPrimitiveTypeKind primitiveKind)
			{
				return new PrimitiveType.ClientEdmPrimitiveType("Edm", primitiveKind.ToString(), primitiveKind);
			}

			// Token: 0x04000310 RID: 784
			private readonly string namespaceName;

			// Token: 0x04000311 RID: 785
			private readonly string name;

			// Token: 0x04000312 RID: 786
			private readonly EdmPrimitiveTypeKind primitiveKind;
		}
	}
}
