using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200007E RID: 126
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		// Token: 0x060006C3 RID: 1731 RVA: 0x0001AA70 File Offset: 0x00018C70
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001AA79 File Offset: 0x00018C79
		public int GetHashCode(JToken obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetDeepHashCode();
		}
	}
}
