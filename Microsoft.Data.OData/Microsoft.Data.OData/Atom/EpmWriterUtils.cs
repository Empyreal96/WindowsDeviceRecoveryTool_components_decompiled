using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200026C RID: 620
	internal static class EpmWriterUtils
	{
		// Token: 0x06001467 RID: 5223 RVA: 0x0004BE0C File Offset: 0x0004A00C
		internal static string GetPropertyValueAsText(object propertyValue)
		{
			if (propertyValue == null)
			{
				return null;
			}
			return AtomValueUtils.ConvertPrimitiveToString(propertyValue);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0004BE1C File Offset: 0x0004A01C
		internal static EntityPropertyMappingAttribute GetEntityPropertyMapping(EpmSourcePathSegment epmParentSourcePathSegment, string propertyName)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(propertyName, "propertyName");
			EpmSourcePathSegment propertySourcePathSegment = EpmWriterUtils.GetPropertySourcePathSegment(epmParentSourcePathSegment, propertyName);
			return EpmWriterUtils.GetEntityPropertyMapping(propertySourcePathSegment);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0004BE44 File Offset: 0x0004A044
		internal static EntityPropertyMappingAttribute GetEntityPropertyMapping(EpmSourcePathSegment epmSourcePathSegment)
		{
			if (epmSourcePathSegment == null)
			{
				return null;
			}
			EntityPropertyMappingInfo epmInfo = epmSourcePathSegment.EpmInfo;
			if (epmInfo == null)
			{
				return null;
			}
			return epmInfo.Attribute;
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0004BE88 File Offset: 0x0004A088
		internal static EpmSourcePathSegment GetPropertySourcePathSegment(EpmSourcePathSegment epmParentSourcePathSegment, string propertyName)
		{
			EpmSourcePathSegment result = null;
			if (epmParentSourcePathSegment != null)
			{
				result = epmParentSourcePathSegment.SubProperties.FirstOrDefault((EpmSourcePathSegment subProperty) => subProperty.PropertyName == propertyName);
			}
			return result;
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0004BEC8 File Offset: 0x0004A0C8
		internal static void CacheEpmProperties(EntryPropertiesValueCache propertyValueCache, EpmSourceTree sourceTree)
		{
			EpmSourcePathSegment root = sourceTree.Root;
			EpmWriterUtils.CacheEpmSourcePathSegments(propertyValueCache, root.SubProperties, propertyValueCache.EntryProperties);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0004BEF0 File Offset: 0x0004A0F0
		private static void CacheEpmSourcePathSegments(EpmValueCache valueCache, List<EpmSourcePathSegment> segments, IEnumerable<ODataProperty> properties)
		{
			if (properties == null)
			{
				return;
			}
			foreach (EpmSourcePathSegment epmSourcePathSegment in segments)
			{
				ODataComplexValue complexValue;
				if (epmSourcePathSegment.EpmInfo == null && EpmWriterUtils.TryGetPropertyValue<ODataComplexValue>(properties, epmSourcePathSegment.PropertyName, out complexValue))
				{
					IEnumerable<ODataProperty> properties2 = valueCache.CacheComplexValueProperties(complexValue);
					EpmWriterUtils.CacheEpmSourcePathSegments(valueCache, epmSourcePathSegment.SubProperties, properties2);
				}
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004BF88 File Offset: 0x0004A188
		private static bool TryGetPropertyValue<T>(IEnumerable<ODataProperty> properties, string propertyName, out T propertyValue) where T : class
		{
			propertyValue = default(T);
			ODataProperty odataProperty = (from p in properties
			where string.CompareOrdinal(p.Name, propertyName) == 0
			select p).FirstOrDefault<ODataProperty>();
			if (odataProperty != null)
			{
				propertyValue = (odataProperty.Value as T);
				return propertyValue != null || odataProperty.Value == null;
			}
			return false;
		}
	}
}
