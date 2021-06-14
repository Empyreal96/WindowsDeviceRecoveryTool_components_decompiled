using System;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x0200017D RID: 381
	internal static class ODataEdmValueUtils
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x00023AA0 File Offset: 0x00021CA0
		internal static IEdmPropertyValue GetEdmPropertyValue(this ODataProperty property, IEdmStructuredTypeReference declaringType)
		{
			IEdmTypeReference type = null;
			if (declaringType != null)
			{
				IEdmProperty edmProperty = declaringType.FindProperty(property.Name);
				if (edmProperty == null && !declaringType.IsOpen())
				{
					throw new ODataException(Strings.ODataEdmStructuredValue_UndeclaredProperty(property.Name, declaringType.FullName()));
				}
				type = ((edmProperty == null) ? null : edmProperty.Type);
			}
			return new EdmPropertyValue(property.Name, ODataEdmValueUtils.ConvertValue(property.Value, type).Value);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00023B0C File Offset: 0x00021D0C
		internal static IEdmDelayedValue ConvertValue(object value, IEdmTypeReference type)
		{
			if (value == null)
			{
				if (type != null)
				{
					return new ODataEdmNullValue(type);
				}
				return ODataEdmNullValue.UntypedInstance;
			}
			else
			{
				ODataComplexValue odataComplexValue = value as ODataComplexValue;
				if (odataComplexValue != null)
				{
					return new ODataEdmStructuredValue(odataComplexValue);
				}
				ODataCollectionValue odataCollectionValue = value as ODataCollectionValue;
				if (odataCollectionValue != null)
				{
					return new ODataEdmCollectionValue(odataCollectionValue);
				}
				return EdmValueUtils.ConvertPrimitiveValue(value, (type == null) ? null : type.AsPrimitive());
			}
		}
	}
}
