using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.JsonLight;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000142 RID: 322
	internal sealed class JsonLightInstanceAnnotationWriter
	{
		// Token: 0x060008AD RID: 2221 RVA: 0x0001BE1B File Offset: 0x0001A01B
		internal JsonLightInstanceAnnotationWriter(IODataJsonLightValueSerializer valueSerializer, JsonLightTypeNameOracle typeNameOracle)
		{
			this.valueSerializer = valueSerializer;
			this.typeNameOracle = typeNameOracle;
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0001BE31 File Offset: 0x0001A031
		private IJsonWriter JsonWriter
		{
			get
			{
				return this.valueSerializer.JsonWriter;
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001BE40 File Offset: 0x0001A040
		internal void WriteInstanceAnnotations(IEnumerable<ODataInstanceAnnotation> instanceAnnotations, InstanceAnnotationWriteTracker tracker)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			foreach (ODataInstanceAnnotation odataInstanceAnnotation in instanceAnnotations)
			{
				if (!hashSet.Add(odataInstanceAnnotation.Name))
				{
					throw new ODataException(Strings.JsonLightInstanceAnnotationWriter_DuplicateAnnotationNameInCollection(odataInstanceAnnotation.Name));
				}
				if (!tracker.IsAnnotationWritten(odataInstanceAnnotation.Name))
				{
					this.WriteInstanceAnnotation(odataInstanceAnnotation);
					tracker.MarkAnnotationWritten(odataInstanceAnnotation.Name);
				}
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001BED0 File Offset: 0x0001A0D0
		internal void WriteInstanceAnnotations(IEnumerable<ODataInstanceAnnotation> instanceAnnotations)
		{
			this.WriteInstanceAnnotations(instanceAnnotations, new InstanceAnnotationWriteTracker());
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001BEE0 File Offset: 0x0001A0E0
		internal void WriteInstanceAnnotation(ODataInstanceAnnotation instanceAnnotation)
		{
			string name = instanceAnnotation.Name;
			ODataValue value = instanceAnnotation.Value;
			if (this.valueSerializer.Settings.ShouldSkipAnnotation(name))
			{
				return;
			}
			IEdmTypeReference edmTypeReference = MetadataUtils.LookupTypeOfValueTerm(name, this.valueSerializer.Model);
			if (value is ODataNullValue)
			{
				if (edmTypeReference != null && !edmTypeReference.IsNullable)
				{
					throw new ODataException(Strings.ODataAtomPropertyAndValueSerializer_NullValueNotAllowedForInstanceAnnotation(instanceAnnotation.Name, edmTypeReference.ODataFullName()));
				}
				this.JsonWriter.WriteName(name);
				this.valueSerializer.WriteNullValue();
				return;
			}
			else
			{
				bool flag = edmTypeReference == null;
				ODataComplexValue odataComplexValue = value as ODataComplexValue;
				if (odataComplexValue != null)
				{
					this.JsonWriter.WriteName(name);
					this.valueSerializer.WriteComplexValue(odataComplexValue, edmTypeReference, false, flag, this.valueSerializer.CreateDuplicatePropertyNamesChecker());
					return;
				}
				IEdmTypeReference typeReferenceFromValue = TypeNameOracle.ResolveAndValidateTypeNameForValue(this.valueSerializer.Model, edmTypeReference, value, flag);
				ODataCollectionValue odataCollectionValue = value as ODataCollectionValue;
				if (odataCollectionValue != null)
				{
					string valueTypeNameForWriting = this.typeNameOracle.GetValueTypeNameForWriting(odataCollectionValue, edmTypeReference, typeReferenceFromValue, flag);
					if (valueTypeNameForWriting != null)
					{
						ODataJsonLightWriterUtils.WriteODataTypePropertyAnnotation(this.JsonWriter, name, valueTypeNameForWriting);
					}
					this.JsonWriter.WriteName(name);
					this.valueSerializer.WriteCollectionValue(odataCollectionValue, edmTypeReference, false, false, flag);
					return;
				}
				ODataPrimitiveValue odataPrimitiveValue = value as ODataPrimitiveValue;
				string valueTypeNameForWriting2 = this.typeNameOracle.GetValueTypeNameForWriting(odataPrimitiveValue, edmTypeReference, typeReferenceFromValue, flag);
				if (valueTypeNameForWriting2 != null)
				{
					ODataJsonLightWriterUtils.WriteODataTypePropertyAnnotation(this.JsonWriter, name, valueTypeNameForWriting2);
				}
				this.JsonWriter.WriteName(name);
				this.valueSerializer.WritePrimitiveValue(odataPrimitiveValue.Value, edmTypeReference);
				return;
			}
		}

		// Token: 0x0400034B RID: 843
		private readonly IODataJsonLightValueSerializer valueSerializer;

		// Token: 0x0400034C RID: 844
		private readonly JsonLightTypeNameOracle typeNameOracle;
	}
}
