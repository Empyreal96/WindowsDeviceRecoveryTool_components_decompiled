using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000AE RID: 174
	public interface IAttributeProvider
	{
		// Token: 0x06000875 RID: 2165
		IList<Attribute> GetAttributes(bool inherit);

		// Token: 0x06000876 RID: 2166
		IList<Attribute> GetAttributes(Type attributeType, bool inherit);
	}
}
