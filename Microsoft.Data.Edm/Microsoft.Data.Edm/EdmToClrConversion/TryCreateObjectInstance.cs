using System;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.EdmToClrConversion
{
	// Token: 0x020000C1 RID: 193
	// (Invoke) Token: 0x060003D3 RID: 979
	public delegate bool TryCreateObjectInstance(IEdmStructuredValue edmValue, Type clrType, EdmToClrConverter converter, out object objectInstance, out bool objectInstanceInitialized);
}
