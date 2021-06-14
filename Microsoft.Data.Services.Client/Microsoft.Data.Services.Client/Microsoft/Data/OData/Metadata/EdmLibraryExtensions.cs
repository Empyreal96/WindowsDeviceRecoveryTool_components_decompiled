using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x0200001D RID: 29
	internal static class EdmLibraryExtensions
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00004458 File Offset: 0x00002658
		static EdmLibraryExtensions()
		{
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(bool), EdmLibraryExtensions.BooleanTypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(byte), EdmLibraryExtensions.ByteTypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(DateTime), EdmLibraryExtensions.DateTimeTypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(decimal), EdmLibraryExtensions.DecimalTypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(double), EdmLibraryExtensions.DoubleTypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(short), EdmLibraryExtensions.Int16TypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(int), EdmLibraryExtensions.Int32TypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(long), EdmLibraryExtensions.Int64TypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(sbyte), EdmLibraryExtensions.SByteTypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(string), EdmLibraryExtensions.StringTypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(float), EdmLibraryExtensions.SingleTypeReference);
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(DateTimeOffset), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.DateTimeOffset), false));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(Guid), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Guid), false));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(TimeSpan), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Time), false));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(byte[]), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Binary), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(Stream), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Stream), false));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(bool?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Boolean), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(byte?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Byte), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(DateTime?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.DateTime), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(DateTimeOffset?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.DateTimeOffset), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(decimal?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Decimal), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(double?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Double), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(short?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Int16), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(int?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Int32), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(long?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Int64), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(sbyte?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.SByte), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(float?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Single), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(Guid?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Guid), true));
			EdmLibraryExtensions.PrimitiveTypeReferenceMap.Add(typeof(TimeSpan?), EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Time), true));
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004921 File Offset: 0x00002B21
		internal static string FullName(this IEdmEntityContainerElement containerElement)
		{
			return containerElement.Container.Name + "." + containerElement.Name;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004940 File Offset: 0x00002B40
		internal static IEdmTypeReference ToTypeReference(this IEdmType type, bool nullable)
		{
			if (type == null)
			{
				return null;
			}
			switch (type.TypeKind)
			{
			case EdmTypeKind.Primitive:
				return EdmLibraryExtensions.ToTypeReference((IEdmPrimitiveType)type, nullable);
			case EdmTypeKind.Entity:
				return new EdmEntityTypeReference((IEdmEntityType)type, nullable);
			case EdmTypeKind.Complex:
				return new EdmComplexTypeReference((IEdmComplexType)type, nullable);
			case EdmTypeKind.Row:
				return new EdmRowTypeReference((IEdmRowType)type, nullable);
			case EdmTypeKind.Collection:
				return new EdmCollectionTypeReference((IEdmCollectionType)type, nullable);
			case EdmTypeKind.EntityReference:
				return new EdmEntityReferenceTypeReference((IEdmEntityReferenceType)type, nullable);
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodesCommon.EdmLibraryExtensions_ToTypeReference));
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000049DC File Offset: 0x00002BDC
		internal static string GetCollectionTypeName(string itemTypeName)
		{
			return string.Format(CultureInfo.InvariantCulture, "Collection({0})", new object[]
			{
				itemTypeName
			});
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004A2E File Offset: 0x00002C2E
		internal static IEdmEntitySet ResolveEntitySet(this IEdmModel model, string containerQualifiedEntitySetName)
		{
			return EdmLibraryExtensions.ResolveContainerQualifiedElementName(model, containerQualifiedEntitySetName, delegate(IEdmEntityContainer container, string entitySetName)
			{
				IEdmEntitySet edmEntitySet = container.FindEntitySet(entitySetName);
				if (edmEntitySet != null)
				{
					return new IEdmEntitySet[]
					{
						edmEntitySet
					};
				}
				return Enumerable.Empty<IEdmEntityContainerElement>();
			}).Cast<IEdmEntitySet>().SingleOrDefault<IEdmEntitySet>();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004A60 File Offset: 0x00002C60
		private static EdmPrimitiveTypeReference ToTypeReference(IEdmPrimitiveType primitiveType, bool nullable)
		{
			switch (primitiveType.PrimitiveKind)
			{
			case EdmPrimitiveTypeKind.Binary:
				return new EdmBinaryTypeReference(primitiveType, nullable);
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
				return new EdmPrimitiveTypeReference(primitiveType, nullable);
			case EdmPrimitiveTypeKind.DateTime:
			case EdmPrimitiveTypeKind.DateTimeOffset:
			case EdmPrimitiveTypeKind.Time:
				return new EdmTemporalTypeReference(primitiveType, nullable);
			case EdmPrimitiveTypeKind.Decimal:
				return new EdmDecimalTypeReference(primitiveType, nullable);
			case EdmPrimitiveTypeKind.String:
				return new EdmStringTypeReference(primitiveType, nullable);
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
				return new EdmSpatialTypeReference(primitiveType, nullable);
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodesCommon.EdmLibraryExtensions_PrimitiveTypeReference));
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004B40 File Offset: 0x00002D40
		private static bool TryGetSingleOrDefaultEntityContainer(this IEdmModel model, out IEdmEntityContainer foundContainer)
		{
			IEdmEntityContainer[] array = model.EntityContainers().ToArray<IEdmEntityContainer>();
			foundContainer = null;
			if (array.Length > 1)
			{
				IEdmEntityContainer edmEntityContainer = array.SingleOrDefault(new Func<IEdmEntityContainer, bool>(model.IsDefaultEntityContainer));
				if (edmEntityContainer != null)
				{
					foundContainer = edmEntityContainer;
				}
			}
			else if (array.Length == 1)
			{
				foundContainer = array[0];
			}
			return foundContainer != null;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004B90 File Offset: 0x00002D90
		private static IEnumerable<IEdmEntityContainerElement> ResolveContainerQualifiedElementName(IEdmModel model, string containerQualifiedElementName, Func<IEdmEntityContainer, string, IEnumerable<IEdmEntityContainerElement>> resolver)
		{
			if (string.IsNullOrEmpty(containerQualifiedElementName))
			{
				return Enumerable.Empty<IEdmEntityContainerElement>();
			}
			IEdmEntityContainer edmEntityContainer = null;
			string text;
			if (containerQualifiedElementName.IndexOf('.') < 0)
			{
				if (!model.TryGetSingleOrDefaultEntityContainer(out edmEntityContainer))
				{
					return Enumerable.Empty<IEdmEntityContainerElement>();
				}
				text = containerQualifiedElementName;
			}
			else
			{
				int num = containerQualifiedElementName.IndexOf('(');
				string text2 = (num < 0) ? containerQualifiedElementName : containerQualifiedElementName.Substring(0, num);
				int num2 = text2.LastIndexOf('.');
				if (num2 < 0)
				{
					return Enumerable.Empty<IEdmEntityContainerElement>();
				}
				string text3 = containerQualifiedElementName.Substring(0, num2);
				text = containerQualifiedElementName.Substring(num2 + 1);
				if (string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text))
				{
					return Enumerable.Empty<IEdmEntityContainerElement>();
				}
				edmEntityContainer = model.FindEntityContainer(text3);
				if (edmEntityContainer == null)
				{
					return Enumerable.Empty<IEdmEntityContainerElement>();
				}
			}
			return resolver(edmEntityContainer, text);
		}

		// Token: 0x04000183 RID: 387
		private const string CollectionTypeQualifier = "Collection";

		// Token: 0x04000184 RID: 388
		private const string CollectionTypeFormat = "Collection({0})";

		// Token: 0x04000185 RID: 389
		private static readonly Dictionary<Type, IEdmPrimitiveTypeReference> PrimitiveTypeReferenceMap = new Dictionary<Type, IEdmPrimitiveTypeReference>(EqualityComparer<Type>.Default);

		// Token: 0x04000186 RID: 390
		private static readonly EdmPrimitiveTypeReference BooleanTypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Boolean), false);

		// Token: 0x04000187 RID: 391
		private static readonly EdmPrimitiveTypeReference ByteTypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Byte), false);

		// Token: 0x04000188 RID: 392
		private static readonly EdmPrimitiveTypeReference DateTimeTypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.DateTime), false);

		// Token: 0x04000189 RID: 393
		private static readonly EdmPrimitiveTypeReference DecimalTypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Decimal), false);

		// Token: 0x0400018A RID: 394
		private static readonly EdmPrimitiveTypeReference DoubleTypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Double), false);

		// Token: 0x0400018B RID: 395
		private static readonly EdmPrimitiveTypeReference Int16TypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Int16), false);

		// Token: 0x0400018C RID: 396
		private static readonly EdmPrimitiveTypeReference Int32TypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Int32), false);

		// Token: 0x0400018D RID: 397
		private static readonly EdmPrimitiveTypeReference Int64TypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Int64), false);

		// Token: 0x0400018E RID: 398
		private static readonly EdmPrimitiveTypeReference SByteTypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.SByte), false);

		// Token: 0x0400018F RID: 399
		private static readonly EdmPrimitiveTypeReference StringTypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.String), true);

		// Token: 0x04000190 RID: 400
		private static readonly EdmPrimitiveTypeReference SingleTypeReference = EdmLibraryExtensions.ToTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Single), false);
	}
}
