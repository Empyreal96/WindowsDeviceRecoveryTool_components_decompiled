using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x0200017B RID: 379
	internal sealed class ODataEdmCollectionValue : EdmValue, IEdmCollectionValue, IEdmValue, IEdmElement
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x0002385B File Offset: 0x00021A5B
		internal ODataEdmCollectionValue(ODataCollectionValue collectionValue) : base(collectionValue.GetEdmType())
		{
			this.collectionValue = collectionValue;
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00023A64 File Offset: 0x00021C64
		public IEnumerable<IEdmDelayedValue> Elements
		{
			get
			{
				if (this.collectionValue != null)
				{
					IEdmTypeReference itemType = (base.Type == null) ? null : (base.Type.Definition as IEdmCollectionType).ElementType;
					foreach (object collectionItem in this.collectionValue.Items)
					{
						yield return ODataEdmValueUtils.ConvertValue(collectionItem, itemType);
					}
				}
				yield break;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x00023A81 File Offset: 0x00021C81
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Collection;
			}
		}

		// Token: 0x040003F9 RID: 1017
		private readonly ODataCollectionValue collectionValue;
	}
}
