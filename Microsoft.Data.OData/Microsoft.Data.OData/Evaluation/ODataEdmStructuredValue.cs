using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x0200017E RID: 382
	internal sealed class ODataEdmStructuredValue : EdmValue, IEdmStructuredValue, IEdmValue, IEdmElement
	{
		// Token: 0x06000AB7 RID: 2743 RVA: 0x00023B60 File Offset: 0x00021D60
		internal ODataEdmStructuredValue(ODataEntry entry) : base(entry.GetEdmType())
		{
			this.properties = entry.NonComputedProperties;
			this.structuredType = ((base.Type == null) ? null : base.Type.AsStructured());
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00023B96 File Offset: 0x00021D96
		internal ODataEdmStructuredValue(ODataComplexValue complexValue) : base(complexValue.GetEdmType())
		{
			this.properties = complexValue.Properties;
			this.structuredType = ((base.Type == null) ? null : base.Type.AsStructured());
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00023D7C File Offset: 0x00021F7C
		public IEnumerable<IEdmPropertyValue> PropertyValues
		{
			get
			{
				if (this.properties != null)
				{
					foreach (ODataProperty property in this.properties)
					{
						yield return property.GetEdmPropertyValue(this.structuredType);
					}
				}
				yield break;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00023D99 File Offset: 0x00021F99
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Structured;
			}
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00023DB8 File Offset: 0x00021FB8
		public IEdmPropertyValue FindPropertyValue(string propertyName)
		{
			ODataProperty odataProperty = (this.properties == null) ? null : (from p in this.properties
			where p.Name == propertyName
			select p).FirstOrDefault<ODataProperty>();
			if (odataProperty == null)
			{
				return null;
			}
			return odataProperty.GetEdmPropertyValue(this.structuredType);
		}

		// Token: 0x040003FB RID: 1019
		private readonly IEnumerable<ODataProperty> properties;

		// Token: 0x040003FC RID: 1020
		private readonly IEdmStructuredTypeReference structuredType;
	}
}
