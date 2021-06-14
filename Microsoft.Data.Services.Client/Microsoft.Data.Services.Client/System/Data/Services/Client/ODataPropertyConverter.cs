using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Xml.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace System.Data.Services.Client
{
	// Token: 0x0200008D RID: 141
	internal class ODataPropertyConverter
	{
		// Token: 0x0600051B RID: 1307 RVA: 0x00014634 File Offset: 0x00012834
		internal ODataPropertyConverter(RequestInfo requestInfo)
		{
			this.requestInfo = requestInfo;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00014644 File Offset: 0x00012844
		internal IEnumerable<ODataProperty> PopulateProperties(object resource, string serverTypeName, IEnumerable<ClientPropertyAnnotation> properties)
		{
			List<ODataProperty> list = new List<ODataProperty>();
			foreach (ClientPropertyAnnotation clientPropertyAnnotation in properties)
			{
				object value = clientPropertyAnnotation.GetValue(resource);
				ODataValue odataValue;
				if (this.TryConvertPropertyValue(clientPropertyAnnotation, value, null, out odataValue))
				{
					list.Add(new ODataProperty
					{
						Name = clientPropertyAnnotation.EdmProperty.Name,
						Value = odataValue
					});
					this.AddTypeAnnotationNotDeclaredOnServer(serverTypeName, clientPropertyAnnotation, odataValue);
				}
			}
			return list;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000146D8 File Offset: 0x000128D8
		internal ODataComplexValue CreateODataComplexValue(Type complexType, object value, string propertyName, bool isCollectionItem, HashSet<object> visitedComplexTypeObjects)
		{
			ClientEdmModel model = this.requestInfo.Model;
			ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(complexType);
			if (value == null)
			{
				return null;
			}
			if (visitedComplexTypeObjects == null)
			{
				visitedComplexTypeObjects = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);
			}
			else if (visitedComplexTypeObjects.Contains(value))
			{
				if (propertyName != null)
				{
					throw Error.InvalidOperation(Strings.Serializer_LoopsNotAllowedInComplexTypes(propertyName));
				}
				throw Error.InvalidOperation(Strings.Serializer_LoopsNotAllowedInNonPropertyComplexTypes(clientTypeAnnotation.ElementTypeName));
			}
			visitedComplexTypeObjects.Add(value);
			ODataComplexValue odataComplexValue = new ODataComplexValue();
			if (!this.requestInfo.Format.UsingAtom)
			{
				odataComplexValue.TypeName = clientTypeAnnotation.ElementTypeName;
			}
			if (!isCollectionItem)
			{
				odataComplexValue.SetAnnotation<SerializationTypeNameAnnotation>(new SerializationTypeNameAnnotation
				{
					TypeName = this.requestInfo.GetServerTypeName(clientTypeAnnotation)
				});
			}
			odataComplexValue.Properties = this.PopulateProperties(value, clientTypeAnnotation.PropertiesToSerialize(), visitedComplexTypeObjects);
			visitedComplexTypeObjects.Remove(value);
			return odataComplexValue;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00014810 File Offset: 0x00012A10
		internal ODataCollectionValue CreateODataCollection(Type collectionItemType, string propertyName, object value, HashSet<object> visitedComplexTypeObjects)
		{
			WebUtil.ValidateCollection(collectionItemType, value, propertyName);
			PrimitiveType primitiveType;
			bool flag = PrimitiveType.TryGetPrimitiveType(collectionItemType, out primitiveType);
			ODataCollectionValue odataCollectionValue = new ODataCollectionValue();
			IEnumerable enumerable = (IEnumerable)value;
			string text;
			string itemTypeName;
			if (flag)
			{
				text = ClientConvert.GetEdmType(Nullable.GetUnderlyingType(collectionItemType) ?? collectionItemType);
				odataCollectionValue.Items = Util.GetEnumerable<object>(enumerable, delegate(object val)
				{
					WebUtil.ValidateCollectionItem(val);
					WebUtil.ValidatePrimitiveCollectionItem(val, propertyName, collectionItemType);
					return ODataPropertyConverter.ConvertPrimitiveValueToRecognizedODataType(val, collectionItemType);
				});
				itemTypeName = text;
			}
			else
			{
				text = this.requestInfo.ResolveNameFromType(collectionItemType);
				odataCollectionValue.Items = Util.GetEnumerable<ODataComplexValue>(enumerable, delegate(object val)
				{
					WebUtil.ValidateCollectionItem(val);
					WebUtil.ValidateComplexCollectionItem(val, propertyName, collectionItemType);
					return this.CreateODataComplexValue(collectionItemType, val, propertyName, true, visitedComplexTypeObjects);
				});
				itemTypeName = collectionItemType.FullName;
			}
			if (!this.requestInfo.Format.UsingAtom)
			{
				odataCollectionValue.TypeName = ODataPropertyConverter.GetCollectionName(itemTypeName);
			}
			string collectionName = ODataPropertyConverter.GetCollectionName(text);
			odataCollectionValue.SetAnnotation<SerializationTypeNameAnnotation>(new SerializationTypeNameAnnotation
			{
				TypeName = collectionName
			});
			return odataCollectionValue;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00014948 File Offset: 0x00012B48
		private static object ConvertPrimitiveValueToRecognizedODataType(object propertyValue, Type propertyType)
		{
			if (propertyValue == null)
			{
				return null;
			}
			PrimitiveType primitiveType;
			PrimitiveType.TryGetPrimitiveType(propertyType, out primitiveType);
			if (propertyType == typeof(char) || propertyType == typeof(char[]) || propertyType == typeof(Type) || propertyType == typeof(Uri) || propertyType == typeof(XDocument) || propertyType == typeof(XElement))
			{
				return primitiveType.TypeConverter.ToString(propertyValue);
			}
			if (propertyType.FullName == "System.Data.Linq.Binary")
			{
				return ((BinaryTypeConverter)primitiveType.TypeConverter).ToArray(propertyValue);
			}
			if (primitiveType.EdmTypeName == null)
			{
				throw new NotSupportedException(Strings.ALinq_CantCastToUnsupportedPrimitive(propertyType.Name));
			}
			return propertyValue;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00014A1A File Offset: 0x00012C1A
		private static string GetCollectionName(string itemTypeName)
		{
			if (itemTypeName != null)
			{
				return EdmLibraryExtensions.GetCollectionTypeName(itemTypeName);
			}
			return null;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00014A27 File Offset: 0x00012C27
		private static ODataValue CreateODataPrimitivePropertyValue(ClientPropertyAnnotation property, object propertyValue)
		{
			if (propertyValue == null)
			{
				return new ODataNullValue();
			}
			propertyValue = ODataPropertyConverter.ConvertPrimitiveValueToRecognizedODataType(propertyValue, property.PropertyType);
			return new ODataPrimitiveValue(propertyValue);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00014A48 File Offset: 0x00012C48
		private IEnumerable<ODataProperty> PopulateProperties(object resource, IEnumerable<ClientPropertyAnnotation> properties, HashSet<object> visitedComplexTypeObjects)
		{
			List<ODataProperty> list = new List<ODataProperty>();
			foreach (ClientPropertyAnnotation clientPropertyAnnotation in properties)
			{
				object value = clientPropertyAnnotation.GetValue(resource);
				ODataValue value2;
				if (this.TryConvertPropertyValue(clientPropertyAnnotation, value, visitedComplexTypeObjects, out value2))
				{
					list.Add(new ODataProperty
					{
						Name = clientPropertyAnnotation.EdmProperty.Name,
						Value = value2
					});
				}
			}
			return list;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00014AD4 File Offset: 0x00012CD4
		private bool TryConvertPropertyValue(ClientPropertyAnnotation property, object propertyValue, HashSet<object> visitedComplexTypeObjects, out ODataValue odataValue)
		{
			if (property.IsKnownType)
			{
				odataValue = ODataPropertyConverter.CreateODataPrimitivePropertyValue(property, propertyValue);
				return true;
			}
			if (property.IsPrimitiveOrComplexCollection)
			{
				odataValue = this.CreateODataCollectionPropertyValue(property, propertyValue, visitedComplexTypeObjects);
				return true;
			}
			if (!property.IsEntityCollection && !ClientTypeUtil.TypeIsEntity(property.PropertyType, this.requestInfo.Model))
			{
				odataValue = this.CreateODataComplexPropertyValue(property, propertyValue, visitedComplexTypeObjects);
				return true;
			}
			odataValue = null;
			return false;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00014B40 File Offset: 0x00012D40
		private ODataComplexValue CreateODataComplexPropertyValue(ClientPropertyAnnotation property, object propertyValue, HashSet<object> visitedComplexTypeObjects)
		{
			Type complexType = property.IsPrimitiveOrComplexCollection ? property.PrimitiveOrComplexCollectionItemType : property.PropertyType;
			return this.CreateODataComplexValue(complexType, propertyValue, property.PropertyName, property.IsPrimitiveOrComplexCollection, visitedComplexTypeObjects);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00014B79 File Offset: 0x00012D79
		private ODataCollectionValue CreateODataCollectionPropertyValue(ClientPropertyAnnotation property, object propertyValue, HashSet<object> visitedComplexTypeObjects)
		{
			return this.CreateODataCollection(property.PrimitiveOrComplexCollectionItemType, property.PropertyName, propertyValue, visitedComplexTypeObjects);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00014B90 File Offset: 0x00012D90
		private void AddTypeAnnotationNotDeclaredOnServer(string serverTypeName, ClientPropertyAnnotation property, ODataValue odataValue)
		{
			ODataPrimitiveValue odataPrimitiveValue = odataValue as ODataPrimitiveValue;
			if (odataPrimitiveValue == null)
			{
				return;
			}
			if (!this.requestInfo.Format.UsingAtom && this.requestInfo.TypeResolver.ShouldWriteClientTypeForOpenServerProperty(property.EdmProperty, serverTypeName) && !JsonSharedUtils.ValueTypeMatchesJsonType(odataPrimitiveValue, property.EdmProperty.Type.AsPrimitive()))
			{
				odataPrimitiveValue.SetAnnotation<SerializationTypeNameAnnotation>(new SerializationTypeNameAnnotation
				{
					TypeName = property.EdmProperty.Type.FullName()
				});
			}
		}

		// Token: 0x04000303 RID: 771
		private readonly RequestInfo requestInfo;
	}
}
