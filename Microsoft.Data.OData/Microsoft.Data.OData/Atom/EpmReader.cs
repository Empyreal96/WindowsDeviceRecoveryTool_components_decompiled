using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020001F4 RID: 500
	internal abstract class EpmReader
	{
		// Token: 0x06000F49 RID: 3913 RVA: 0x00036750 File Offset: 0x00034950
		protected EpmReader(IODataAtomReaderEntryState entryState, ODataAtomInputContext inputContext)
		{
			this.entryState = entryState;
			this.atomInputContext = inputContext;
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x00036766 File Offset: 0x00034966
		protected IODataAtomReaderEntryState EntryState
		{
			get
			{
				return this.entryState;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x0003676E File Offset: 0x0003496E
		protected ODataVersion Version
		{
			get
			{
				return this.atomInputContext.Version;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000F4C RID: 3916 RVA: 0x0003677B File Offset: 0x0003497B
		protected ODataMessageReaderSettings MessageReaderSettings
		{
			get
			{
				return this.atomInputContext.MessageReaderSettings;
			}
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00036788 File Offset: 0x00034988
		protected void SetEntryEpmValue(EntityPropertyMappingInfo epmInfo, object propertyValue)
		{
			this.SetEpmValue(this.entryState.Entry.Properties.ToReadOnlyEnumerable("Properties"), this.entryState.EntityType.ToTypeReference(), epmInfo, propertyValue);
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x000367BC File Offset: 0x000349BC
		protected void SetEpmValue(ReadOnlyEnumerable<ODataProperty> targetList, IEdmTypeReference targetTypeReference, EntityPropertyMappingInfo epmInfo, object propertyValue)
		{
			this.SetEpmValueForSegment(epmInfo, 0, targetTypeReference.AsStructuredOrNull(), targetList, propertyValue);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x000367F0 File Offset: 0x000349F0
		private void SetEpmValueForSegment(EntityPropertyMappingInfo epmInfo, int propertyValuePathIndex, IEdmStructuredTypeReference segmentStructuralTypeReference, ReadOnlyEnumerable<ODataProperty> existingProperties, object propertyValue)
		{
			string propertyName = epmInfo.PropertyValuePath[propertyValuePathIndex].PropertyName;
			if (epmInfo.Attribute.KeepInContent)
			{
				return;
			}
			ODataProperty odataProperty = existingProperties.FirstOrDefault((ODataProperty p) => string.CompareOrdinal(p.Name, propertyName) == 0);
			ODataComplexValue odataComplexValue = null;
			if (odataProperty != null)
			{
				odataComplexValue = (odataProperty.Value as ODataComplexValue);
				if (odataComplexValue == null)
				{
					return;
				}
			}
			IEdmProperty edmProperty = segmentStructuralTypeReference.FindProperty(propertyName);
			if (edmProperty == null && propertyValuePathIndex != epmInfo.PropertyValuePath.Length - 1)
			{
				throw new ODataException(Strings.EpmReader_OpenComplexOrCollectionEpmProperty(epmInfo.Attribute.SourcePath));
			}
			IEdmTypeReference edmTypeReference;
			if (edmProperty == null || (this.MessageReaderSettings.DisablePrimitiveTypeConversion && edmProperty.Type.IsODataPrimitiveTypeKind()))
			{
				edmTypeReference = EdmCoreModel.Instance.GetString(true);
			}
			else
			{
				edmTypeReference = edmProperty.Type;
			}
			switch (edmTypeReference.TypeKind())
			{
			case EdmTypeKind.Primitive:
			{
				if (edmTypeReference.IsStream())
				{
					throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmReader_SetEpmValueForSegment_StreamProperty));
				}
				object propertyValue2;
				if (propertyValue == null)
				{
					ReaderValidationUtils.ValidateNullValue(this.atomInputContext.Model, edmTypeReference, this.atomInputContext.MessageReaderSettings, true, this.atomInputContext.Version, propertyName);
					propertyValue2 = null;
				}
				else
				{
					propertyValue2 = AtomValueUtils.ConvertStringToPrimitive((string)propertyValue, edmTypeReference.AsPrimitive());
				}
				this.AddEpmPropertyValue(existingProperties, propertyName, propertyValue2, segmentStructuralTypeReference.IsODataEntityTypeKind());
				return;
			}
			case EdmTypeKind.Complex:
			{
				if (odataComplexValue == null)
				{
					odataComplexValue = new ODataComplexValue
					{
						TypeName = edmTypeReference.ODataFullName(),
						Properties = new ReadOnlyEnumerable<ODataProperty>()
					};
					this.AddEpmPropertyValue(existingProperties, propertyName, odataComplexValue, segmentStructuralTypeReference.IsODataEntityTypeKind());
				}
				IEdmComplexTypeReference segmentStructuralTypeReference2 = edmTypeReference.AsComplex();
				this.SetEpmValueForSegment(epmInfo, propertyValuePathIndex + 1, segmentStructuralTypeReference2, odataComplexValue.Properties.ToReadOnlyEnumerable("Properties"), propertyValue);
				return;
			}
			case EdmTypeKind.Collection:
			{
				ODataCollectionValue propertyValue3 = new ODataCollectionValue
				{
					TypeName = edmTypeReference.ODataFullName(),
					Items = new ReadOnlyEnumerable((List<object>)propertyValue)
				};
				this.AddEpmPropertyValue(existingProperties, propertyName, propertyValue3, segmentStructuralTypeReference.IsODataEntityTypeKind());
				return;
			}
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.EpmReader_SetEpmValueForSegment_TypeKind));
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00036A18 File Offset: 0x00034C18
		private void AddEpmPropertyValue(ReadOnlyEnumerable<ODataProperty> properties, string propertyName, object propertyValue, bool checkDuplicateEntryPropertyNames)
		{
			ODataProperty odataProperty = new ODataProperty
			{
				Name = propertyName,
				Value = propertyValue
			};
			if (checkDuplicateEntryPropertyNames)
			{
				this.entryState.DuplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(odataProperty);
			}
			properties.AddToSourceList(odataProperty);
		}

		// Token: 0x0400056C RID: 1388
		private readonly ODataAtomInputContext atomInputContext;

		// Token: 0x0400056D RID: 1389
		private readonly IODataAtomReaderEntryState entryState;
	}
}
