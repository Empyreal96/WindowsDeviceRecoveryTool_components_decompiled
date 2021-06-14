using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200026B RID: 619
	internal sealed class EntryPropertiesValueCache : EpmValueCache
	{
		// Token: 0x06001462 RID: 5218 RVA: 0x0004BD56 File Offset: 0x00049F56
		internal EntryPropertiesValueCache(ODataEntry entry)
		{
			if (entry.Properties != null)
			{
				this.entryPropertiesCache = new List<ODataProperty>(entry.Properties);
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x0004BD8F File Offset: 0x00049F8F
		internal IEnumerable<ODataProperty> EntryProperties
		{
			get
			{
				if (this.entryPropertiesCache == null)
				{
					return null;
				}
				return from p in this.entryPropertiesCache
				where p == null || !(p.Value is ODataStreamReferenceValue)
				select p;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0004BDD8 File Offset: 0x00049FD8
		internal IEnumerable<ODataProperty> EntryStreamProperties
		{
			get
			{
				if (this.entryPropertiesCache == null)
				{
					return null;
				}
				return from p in this.entryPropertiesCache
				where p != null && p.Value is ODataStreamReferenceValue
				select p;
			}
		}

		// Token: 0x04000734 RID: 1844
		private readonly List<ODataProperty> entryPropertiesCache;
	}
}
