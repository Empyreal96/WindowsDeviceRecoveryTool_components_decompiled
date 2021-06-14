using System;
using System.Collections.Generic;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000114 RID: 276
	internal sealed class ODataJsonLightGeneralDeserializer : ODataJsonLightDeserializer
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x00019769 File Offset: 0x00017969
		internal ODataJsonLightGeneralDeserializer(ODataJsonLightInputContext jsonLightInputContext) : base(jsonLightInputContext)
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00019774 File Offset: 0x00017974
		public object ReadValue()
		{
			if (base.JsonReader.NodeType == JsonNodeType.PrimitiveValue)
			{
				return base.JsonReader.ReadPrimitiveValue();
			}
			if (base.JsonReader.NodeType == JsonNodeType.StartObject)
			{
				return this.ReadAsComplexValue();
			}
			if (base.JsonReader.NodeType == JsonNodeType.StartArray)
			{
				return this.ReadAsCollectionValue();
			}
			return null;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000197C8 File Offset: 0x000179C8
		private ODataComplexValue ReadAsComplexValue()
		{
			base.JsonReader.ReadStartObject();
			List<ODataProperty> list = new List<ODataProperty>();
			while (base.JsonReader.NodeType != JsonNodeType.EndObject)
			{
				string name = base.JsonReader.ReadPropertyName();
				object value = this.ReadValue();
				list.Add(new ODataProperty
				{
					Name = name,
					Value = value
				});
			}
			base.JsonReader.ReadEndObject();
			return new ODataComplexValue
			{
				Properties = list,
				TypeName = null
			};
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00019848 File Offset: 0x00017A48
		private ODataCollectionValue ReadAsCollectionValue()
		{
			base.JsonReader.ReadStartArray();
			List<object> list = new List<object>();
			while (base.JsonReader.NodeType != JsonNodeType.EndArray)
			{
				object item = this.ReadValue();
				list.Add(item);
			}
			base.JsonReader.ReadEndArray();
			return new ODataCollectionValue
			{
				Items = list,
				TypeName = null
			};
		}
	}
}
