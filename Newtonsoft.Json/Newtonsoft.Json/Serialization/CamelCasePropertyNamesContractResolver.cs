using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A0 RID: 160
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		// Token: 0x0600083A RID: 2106 RVA: 0x00020252 File Offset: 0x0001E452
		public CamelCasePropertyNamesContractResolver() : base(true)
		{
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002025B File Offset: 0x0001E45B
		protected internal override string ResolvePropertyName(string propertyName)
		{
			return StringUtils.ToCamelCase(propertyName);
		}
	}
}
