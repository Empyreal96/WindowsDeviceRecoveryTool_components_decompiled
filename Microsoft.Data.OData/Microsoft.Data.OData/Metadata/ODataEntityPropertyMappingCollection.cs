using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services.Common;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000204 RID: 516
	public sealed class ODataEntityPropertyMappingCollection : IEnumerable<EntityPropertyMappingAttribute>, IEnumerable
	{
		// Token: 0x06000FCD RID: 4045 RVA: 0x0003963B File Offset: 0x0003783B
		public ODataEntityPropertyMappingCollection()
		{
			this.mappings = new List<EntityPropertyMappingAttribute>();
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0003964E File Offset: 0x0003784E
		public ODataEntityPropertyMappingCollection(IEnumerable<EntityPropertyMappingAttribute> other)
		{
			ExceptionUtils.CheckArgumentNotNull<IEnumerable<EntityPropertyMappingAttribute>>(other, "other");
			this.mappings = new List<EntityPropertyMappingAttribute>(other);
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x0003966D File Offset: 0x0003786D
		internal int Count
		{
			get
			{
				return this.mappings.Count;
			}
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0003967A File Offset: 0x0003787A
		public void Add(EntityPropertyMappingAttribute mapping)
		{
			ExceptionUtils.CheckArgumentNotNull<EntityPropertyMappingAttribute>(mapping, "mapping");
			this.mappings.Add(mapping);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00039693 File Offset: 0x00037893
		public IEnumerator<EntityPropertyMappingAttribute> GetEnumerator()
		{
			return this.mappings.GetEnumerator();
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x000396A5 File Offset: 0x000378A5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.mappings.GetEnumerator();
		}

		// Token: 0x040005BE RID: 1470
		private readonly List<EntityPropertyMappingAttribute> mappings;
	}
}
