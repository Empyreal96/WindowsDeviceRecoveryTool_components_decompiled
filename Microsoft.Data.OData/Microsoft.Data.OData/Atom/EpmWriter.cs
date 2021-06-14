using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000215 RID: 533
	internal abstract class EpmWriter
	{
		// Token: 0x06001070 RID: 4208 RVA: 0x0003C2CF File Offset: 0x0003A4CF
		protected EpmWriter(ODataAtomOutputContext atomOutputContext)
		{
			this.atomOutputContext = atomOutputContext;
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0003C2DE File Offset: 0x0003A4DE
		protected ODataVersion Version
		{
			get
			{
				return this.atomOutputContext.Version;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x0003C2EB File Offset: 0x0003A4EB
		protected ODataWriterBehavior WriterBehavior
		{
			get
			{
				return this.atomOutputContext.MessageWriterSettings.WriterBehavior;
			}
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0003C2FD File Offset: 0x0003A4FD
		protected object ReadEntryPropertyValue(EntityPropertyMappingInfo epmInfo, EntryPropertiesValueCache epmValueCache, IEdmEntityTypeReference entityType)
		{
			return this.ReadPropertyValue(epmInfo, epmValueCache.EntryProperties, 0, entityType, epmValueCache);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0003C30F File Offset: 0x0003A50F
		private object ReadComplexPropertyValue(EntityPropertyMappingInfo epmInfo, ODataComplexValue complexValue, EpmValueCache epmValueCache, int sourceSegmentIndex, IEdmComplexTypeReference complexType)
		{
			return this.ReadPropertyValue(epmInfo, EpmValueCache.GetComplexValueProperties(epmValueCache, complexValue, false), sourceSegmentIndex, complexType, epmValueCache);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0003C340 File Offset: 0x0003A540
		private object ReadPropertyValue(EntityPropertyMappingInfo epmInfo, IEnumerable<ODataProperty> cachedProperties, int sourceSegmentIndex, IEdmStructuredTypeReference structuredTypeReference, EpmValueCache epmValueCache)
		{
			EpmSourcePathSegment epmSourcePathSegment = epmInfo.PropertyValuePath[sourceSegmentIndex];
			string propertyName = epmSourcePathSegment.PropertyName;
			bool flag = epmInfo.PropertyValuePath.Length == sourceSegmentIndex + 1;
			IEdmStructuredType owningStructuredType = structuredTypeReference.StructuredDefinition();
			IEdmProperty edmProperty = WriterValidationUtils.ValidatePropertyDefined(propertyName, owningStructuredType);
			IEdmTypeReference edmTypeReference = null;
			if (edmProperty != null)
			{
				edmTypeReference = edmProperty.Type;
				if (flag)
				{
					if (!edmTypeReference.IsODataPrimitiveTypeKind() && !edmTypeReference.IsNonEntityCollectionType())
					{
						throw new ODataException(Strings.EpmSourceTree_EndsWithNonPrimitiveType(propertyName));
					}
				}
				else if (edmTypeReference.TypeKind() != EdmTypeKind.Complex)
				{
					throw new ODataException(Strings.EpmSourceTree_TraversalOfNonComplexType(propertyName));
				}
			}
			ODataProperty odataProperty = (cachedProperties == null) ? null : cachedProperties.FirstOrDefault((ODataProperty p) => p.Name == propertyName);
			if (odataProperty == null)
			{
				throw new ODataException(Strings.EpmSourceTree_MissingPropertyOnInstance(propertyName, structuredTypeReference.ODataFullName()));
			}
			object value = odataProperty.Value;
			ODataComplexValue odataComplexValue = value as ODataComplexValue;
			if (flag)
			{
				if (value == null)
				{
					WriterValidationUtils.ValidateNullPropertyValue(edmTypeReference, propertyName, this.WriterBehavior, this.atomOutputContext.Model);
				}
				else
				{
					if (odataComplexValue != null)
					{
						throw new ODataException(Strings.EpmSourceTree_EndsWithNonPrimitiveType(propertyName));
					}
					ODataCollectionValue odataCollectionValue = value as ODataCollectionValue;
					if (odataCollectionValue != null)
					{
						TypeNameOracle.ResolveAndValidateTypeNameForValue(this.atomOutputContext.Model, edmTypeReference, odataCollectionValue, edmProperty == null);
					}
					else
					{
						if (value is ODataStreamReferenceValue)
						{
							throw new ODataException(Strings.ODataWriter_StreamPropertiesMustBePropertiesOfODataEntry(propertyName));
						}
						if (value is ISpatial)
						{
							throw new ODataException(Strings.EpmSourceTree_OpenPropertySpatialTypeCannotBeMapped(propertyName, epmInfo.DefiningType.FullName()));
						}
						if (edmTypeReference != null)
						{
							ValidationUtils.ValidateIsExpectedPrimitiveType(value, edmTypeReference);
						}
					}
				}
				return value;
			}
			if (odataComplexValue != null)
			{
				IEdmComplexTypeReference complexType = TypeNameOracle.ResolveAndValidateTypeNameForValue(this.atomOutputContext.Model, (edmProperty == null) ? null : edmProperty.Type, odataComplexValue, edmProperty == null).AsComplexOrNull();
				return this.ReadComplexPropertyValue(epmInfo, odataComplexValue, epmValueCache, sourceSegmentIndex + 1, complexType);
			}
			if (value != null)
			{
				throw new ODataException(Strings.EpmSourceTree_TraversalOfNonComplexType(propertyName));
			}
			return null;
		}

		// Token: 0x04000602 RID: 1538
		private readonly ODataAtomOutputContext atomOutputContext;
	}
}
