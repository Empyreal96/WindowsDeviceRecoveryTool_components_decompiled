using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020001F6 RID: 502
	internal sealed class EpmCustomReaderValueCache
	{
		// Token: 0x06000F54 RID: 3924 RVA: 0x00036AE8 File Offset: 0x00034CE8
		internal EpmCustomReaderValueCache()
		{
			this.customEpmValues = new List<KeyValuePair<EntityPropertyMappingInfo, string>>();
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x00036AFB File Offset: 0x00034CFB
		internal IEnumerable<KeyValuePair<EntityPropertyMappingInfo, string>> CustomEpmValues
		{
			get
			{
				return this.customEpmValues;
			}
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00036B20 File Offset: 0x00034D20
		internal bool Contains(EntityPropertyMappingInfo epmInfo)
		{
			return this.customEpmValues.Any((KeyValuePair<EntityPropertyMappingInfo, string> epmValue) => object.ReferenceEquals(epmValue.Key, epmInfo));
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00036B51 File Offset: 0x00034D51
		internal void Add(EntityPropertyMappingInfo epmInfo, string value)
		{
			this.customEpmValues.Add(new KeyValuePair<EntityPropertyMappingInfo, string>(epmInfo, value));
		}

		// Token: 0x0400056E RID: 1390
		private readonly List<KeyValuePair<EntityPropertyMappingInfo, string>> customEpmValues;
	}
}
