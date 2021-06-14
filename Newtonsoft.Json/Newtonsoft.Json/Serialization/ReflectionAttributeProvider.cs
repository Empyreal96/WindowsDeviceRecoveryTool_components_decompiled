using System;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000CB RID: 203
	public class ReflectionAttributeProvider : IAttributeProvider
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x00027F90 File Offset: 0x00026190
		public ReflectionAttributeProvider(object attributeProvider)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			this._attributeProvider = attributeProvider;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00027FAA File Offset: 0x000261AA
		public IList<Attribute> GetAttributes(bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, null, inherit);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00027FB9 File Offset: 0x000261B9
		public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
		}

		// Token: 0x0400036E RID: 878
		private readonly object _attributeProvider;
	}
}
