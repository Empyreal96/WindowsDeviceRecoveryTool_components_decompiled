using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000144 RID: 324
	internal class ODataJsonLightValueSerializer : ODataJsonLightSerializer, IODataJsonLightValueSerializer
	{
		// Token: 0x060008BC RID: 2236 RVA: 0x0001C28E File Offset: 0x0001A48E
		internal ODataJsonLightValueSerializer(ODataJsonLightPropertySerializer propertySerializer) : base(propertySerializer.JsonLightOutputContext)
		{
			this.propertySerializer = propertySerializer;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0001C2A3 File Offset: 0x0001A4A3
		internal ODataJsonLightValueSerializer(ODataJsonLightOutputContext outputContext) : base(outputContext)
		{
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0001C2AC File Offset: 0x0001A4AC
		IJsonWriter IODataJsonLightValueSerializer.JsonWriter
		{
			get
			{
				return base.JsonWriter;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0001C2B4 File Offset: 0x0001A4B4
		ODataVersion IODataJsonLightValueSerializer.Version
		{
			get
			{
				return base.Version;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0001C2BC File Offset: 0x0001A4BC
		IEdmModel IODataJsonLightValueSerializer.Model
		{
			get
			{
				return base.Model;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0001C2C4 File Offset: 0x0001A4C4
		ODataMessageWriterSettings IODataJsonLightValueSerializer.Settings
		{
			get
			{
				return base.JsonLightOutputContext.MessageWriterSettings;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0001C2D1 File Offset: 0x0001A4D1
		private ODataJsonLightPropertySerializer PropertySerializer
		{
			get
			{
				if (this.propertySerializer == null)
				{
					this.propertySerializer = new ODataJsonLightPropertySerializer(base.JsonLightOutputContext);
				}
				return this.propertySerializer;
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001C2F2 File Offset: 0x0001A4F2
		public void WriteNullValue()
		{
			base.JsonWriter.WriteValue(null);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001C300 File Offset: 0x0001A500
		public void WriteComplexValue(ODataComplexValue complexValue, IEdmTypeReference metadataTypeReference, bool isTopLevel, bool isOpenPropertyType, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			this.IncreaseRecursionDepth();
			if (!isTopLevel)
			{
				base.JsonWriter.StartObjectScope();
			}
			string text = complexValue.TypeName;
			if (isTopLevel)
			{
				if (text == null)
				{
					throw new ODataException(Strings.ODataJsonLightValueSerializer_MissingTypeNameOnComplex);
				}
			}
			else if (metadataTypeReference == null && !base.WritingResponse && text == null && base.Model.IsUserModel())
			{
				throw new ODataException(Strings.ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForComplexValueRequest);
			}
			IEdmComplexTypeReference edmComplexTypeReference = (IEdmComplexTypeReference)TypeNameOracle.ResolveAndValidateTypeNameForValue(base.Model, metadataTypeReference, complexValue, isOpenPropertyType);
			text = base.JsonLightOutputContext.TypeNameOracle.GetValueTypeNameForWriting(complexValue, metadataTypeReference, edmComplexTypeReference, isOpenPropertyType);
			if (text != null)
			{
				ODataJsonLightWriterUtils.WriteODataTypeInstanceAnnotation(base.JsonWriter, text);
			}
			this.PropertySerializer.WriteProperties((edmComplexTypeReference == null) ? null : edmComplexTypeReference.ComplexDefinition(), complexValue.Properties, true, duplicatePropertyNamesChecker, null);
			if (!isTopLevel)
			{
				base.JsonWriter.EndObjectScope();
			}
			this.DecreaseRecursionDepth();
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001C3D0 File Offset: 0x0001A5D0
		public void WriteCollectionValue(ODataCollectionValue collectionValue, IEdmTypeReference metadataTypeReference, bool isTopLevelProperty, bool isInUri, bool isOpenPropertyType)
		{
			this.IncreaseRecursionDepth();
			string text = collectionValue.TypeName;
			if (isTopLevelProperty)
			{
				if (text == null)
				{
					throw new ODataException(Strings.ODataJsonLightValueSerializer_MissingTypeNameOnCollection);
				}
			}
			else if (metadataTypeReference == null && !base.WritingResponse && text == null && base.Model.IsUserModel())
			{
				throw new ODataException(Strings.ODataJsonLightPropertyAndValueSerializer_NoExpectedTypeOrTypeNameSpecifiedForCollectionValueInRequest);
			}
			IEdmCollectionTypeReference edmCollectionTypeReference = (IEdmCollectionTypeReference)TypeNameOracle.ResolveAndValidateTypeNameForValue(base.Model, metadataTypeReference, collectionValue, isOpenPropertyType);
			text = base.JsonLightOutputContext.TypeNameOracle.GetValueTypeNameForWriting(collectionValue, metadataTypeReference, edmCollectionTypeReference, isOpenPropertyType);
			bool flag = isInUri && !string.IsNullOrEmpty(text);
			if (flag)
			{
				base.JsonWriter.StartObjectScope();
				ODataJsonLightWriterUtils.WriteODataTypeInstanceAnnotation(base.JsonWriter, text);
				base.JsonWriter.WriteValuePropertyName();
			}
			base.JsonWriter.StartArrayScope();
			IEnumerable items = collectionValue.Items;
			if (items != null)
			{
				IEdmTypeReference edmTypeReference = (edmCollectionTypeReference == null) ? null : edmCollectionTypeReference.ElementType();
				DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = null;
				foreach (object obj in items)
				{
					ValidationUtils.ValidateCollectionItem(obj, false);
					ODataComplexValue odataComplexValue = obj as ODataComplexValue;
					if (odataComplexValue != null)
					{
						if (duplicatePropertyNamesChecker == null)
						{
							duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
						}
						this.WriteComplexValue(odataComplexValue, edmTypeReference, false, false, duplicatePropertyNamesChecker);
						duplicatePropertyNamesChecker.Clear();
					}
					else
					{
						this.WritePrimitiveValue(obj, edmTypeReference);
					}
				}
			}
			base.JsonWriter.EndArrayScope();
			if (flag)
			{
				base.JsonWriter.EndObjectScope();
			}
			this.DecreaseRecursionDepth();
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001C550 File Offset: 0x0001A750
		public void WritePrimitiveValue(object value, IEdmTypeReference expectedTypeReference)
		{
			IEdmPrimitiveTypeReference primitiveTypeReference = EdmLibraryExtensions.GetPrimitiveTypeReference(value.GetType());
			if (expectedTypeReference != null)
			{
				ValidationUtils.ValidateIsExpectedPrimitiveType(value, primitiveTypeReference, expectedTypeReference);
			}
			if (primitiveTypeReference != null && primitiveTypeReference.IsSpatial())
			{
				PrimitiveConverter.Instance.WriteJsonLight(value, base.JsonWriter, base.Version);
				return;
			}
			base.JsonWriter.WritePrimitiveValue(value, base.Version);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001C5A9 File Offset: 0x0001A7A9
		DuplicatePropertyNamesChecker IODataJsonLightValueSerializer.CreateDuplicatePropertyNamesChecker()
		{
			return base.CreateDuplicatePropertyNamesChecker();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001C5B1 File Offset: 0x0001A7B1
		[Conditional("DEBUG")]
		internal void AssertRecursionDepthIsZero()
		{
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001C5B3 File Offset: 0x0001A7B3
		private void IncreaseRecursionDepth()
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref this.recursionDepth, base.MessageWriterSettings.MessageQuotas.MaxNestingDepth);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001C5D0 File Offset: 0x0001A7D0
		private void DecreaseRecursionDepth()
		{
			this.recursionDepth--;
		}

		// Token: 0x04000350 RID: 848
		private int recursionDepth;

		// Token: 0x04000351 RID: 849
		private ODataJsonLightPropertySerializer propertySerializer;
	}
}
