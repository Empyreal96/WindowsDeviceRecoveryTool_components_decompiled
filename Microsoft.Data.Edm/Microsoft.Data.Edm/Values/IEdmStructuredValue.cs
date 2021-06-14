using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x0200012A RID: 298
	public interface IEdmStructuredValue : IEdmValue, IEdmElement
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060005C6 RID: 1478
		IEnumerable<IEdmPropertyValue> PropertyValues { get; }

		// Token: 0x060005C7 RID: 1479
		IEdmPropertyValue FindPropertyValue(string propertyName);
	}
}
