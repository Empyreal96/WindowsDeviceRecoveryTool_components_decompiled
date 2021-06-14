using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200026A RID: 618
	internal class EpmValueCache
	{
		// Token: 0x0600145E RID: 5214 RVA: 0x0004BCBC File Offset: 0x00049EBC
		internal EpmValueCache()
		{
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0004BCC4 File Offset: 0x00049EC4
		internal static IEnumerable<ODataProperty> GetComplexValueProperties(EpmValueCache epmValueCache, ODataComplexValue complexValue, bool writingContent)
		{
			if (epmValueCache == null)
			{
				return complexValue.Properties;
			}
			return epmValueCache.GetComplexValueProperties(complexValue, writingContent);
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0004BCD8 File Offset: 0x00049ED8
		internal IEnumerable<ODataProperty> CacheComplexValueProperties(ODataComplexValue complexValue)
		{
			if (complexValue == null)
			{
				return null;
			}
			IEnumerable<ODataProperty> properties = complexValue.Properties;
			List<ODataProperty> list = null;
			if (properties != null)
			{
				list = new List<ODataProperty>(properties);
			}
			if (this.epmValuesCache == null)
			{
				this.epmValuesCache = new Dictionary<object, object>(ReferenceEqualityComparer<object>.Instance);
			}
			this.epmValuesCache.Add(complexValue, list);
			return list;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0004BD24 File Offset: 0x00049F24
		private IEnumerable<ODataProperty> GetComplexValueProperties(ODataComplexValue complexValue, bool writingContent)
		{
			object obj;
			if (this.epmValuesCache != null && this.epmValuesCache.TryGetValue(complexValue, out obj))
			{
				return (IEnumerable<ODataProperty>)obj;
			}
			return complexValue.Properties;
		}

		// Token: 0x04000733 RID: 1843
		private Dictionary<object, object> epmValuesCache;
	}
}
