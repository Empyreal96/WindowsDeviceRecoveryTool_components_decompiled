using System;
using System.Collections;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000042 RID: 66
	internal class CollectionValueMaterializationPolicy : MaterializationPolicy
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000ACED File Offset: 0x00008EED
		internal CollectionValueMaterializationPolicy(IODataMaterializerContext context, PrimitiveValueMaterializationPolicy primitivePolicy)
		{
			this.materializerContext = context;
			this.primitiveValueMaterializationPolicy = primitivePolicy;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000AD03 File Offset: 0x00008F03
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000AD0B File Offset: 0x00008F0B
		internal ComplexValueMaterializationPolicy ComplexValueMaterializationPolicy
		{
			get
			{
				return this.complexValueMaterializationPolicy;
			}
			set
			{
				this.complexValueMaterializationPolicy = value;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000AD3C File Offset: 0x00008F3C
		internal object CreateCollectionPropertyInstance(ODataProperty collectionProperty, Type userCollectionType)
		{
			ODataCollectionValue odataCollectionValue = collectionProperty.Value as ODataCollectionValue;
			ClientTypeAnnotation collectionClientType = this.materializerContext.ResolveTypeForMaterialization(userCollectionType, odataCollectionValue.TypeName);
			return this.CreateCollectionInstance(collectionClientType.EdmTypeReference as IEdmCollectionTypeReference, collectionClientType.ElementType, () => Strings.AtomMaterializer_NoParameterlessCtorForCollectionProperty(collectionProperty.Name, collectionClientType.ElementTypeName));
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000ADC8 File Offset: 0x00008FC8
		internal object CreateCollectionInstance(IEdmCollectionTypeReference edmCollectionTypeReference, Type clientCollectionType)
		{
			return this.CreateCollectionInstance(edmCollectionTypeReference, clientCollectionType, () => Strings.AtomMaterializer_MaterializationTypeError(clientCollectionType.FullName));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000ADFC File Offset: 0x00008FFC
		internal void ApplyCollectionDataValues(ODataProperty collectionProperty, object collectionInstance, Type collectionItemType, Action<object, object> addValueToBackingICollectionInstance)
		{
			ODataCollectionValue odataCollectionValue = collectionProperty.Value as ODataCollectionValue;
			this.ApplyCollectionDataValues(odataCollectionValue.Items, odataCollectionValue.TypeName, collectionInstance, collectionItemType, addValueToBackingICollectionInstance);
			collectionProperty.SetMaterializedValue(collectionInstance);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000AE34 File Offset: 0x00009034
		internal void ApplyCollectionDataValues(IEnumerable items, string wireTypeName, object collectionInstance, Type collectionItemType, Action<object, object> addValueToBackingICollectionInstance)
		{
			if (items != null)
			{
				bool flag = PrimitiveType.IsKnownNullableType(collectionItemType);
				ClientEdmModel model = this.materializerContext.Model;
				foreach (object obj in items)
				{
					if (obj == null)
					{
						throw Error.InvalidOperation(Strings.Collection_NullCollectionItemsNotSupported);
					}
					ODataComplexValue odataComplexValue = obj as ODataComplexValue;
					if (flag)
					{
						if (odataComplexValue != null || obj is ODataCollectionValue)
						{
							throw Error.InvalidOperation(Strings.Collection_ComplexTypesInCollectionOfPrimitiveTypesNotAllowed);
						}
						object arg = this.primitiveValueMaterializationPolicy.MaterializePrimitiveDataValueCollectionElement(collectionItemType, wireTypeName, obj);
						addValueToBackingICollectionInstance(collectionInstance, arg);
					}
					else
					{
						if (odataComplexValue == null)
						{
							throw Error.InvalidOperation(Strings.Collection_PrimitiveTypesInCollectionOfComplexTypesNotAllowed);
						}
						ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(collectionItemType));
						object obj2 = this.CreateNewInstance(clientTypeAnnotation.EdmTypeReference, clientTypeAnnotation.ElementType);
						this.ComplexValueMaterializationPolicy.ApplyDataValues(clientTypeAnnotation, odataComplexValue.Properties, obj2);
						addValueToBackingICollectionInstance(collectionInstance, obj2);
					}
				}
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000AF44 File Offset: 0x00009144
		private object CreateCollectionInstance(IEdmCollectionTypeReference edmCollectionTypeReference, Type clientCollectionType, Func<string> error)
		{
			if (ClientTypeUtil.IsDataServiceCollection(clientCollectionType))
			{
				throw Error.InvalidOperation(Strings.AtomMaterializer_DataServiceCollectionNotSupportedForNonEntities);
			}
			object result;
			try
			{
				result = this.CreateNewInstance(edmCollectionTypeReference, clientCollectionType);
			}
			catch (MissingMethodException innerException)
			{
				throw Error.InvalidOperation(error(), innerException);
			}
			return result;
		}

		// Token: 0x04000222 RID: 546
		private readonly IODataMaterializerContext materializerContext;

		// Token: 0x04000223 RID: 547
		private ComplexValueMaterializationPolicy complexValueMaterializationPolicy;

		// Token: 0x04000224 RID: 548
		private PrimitiveValueMaterializationPolicy primitiveValueMaterializationPolicy;
	}
}
