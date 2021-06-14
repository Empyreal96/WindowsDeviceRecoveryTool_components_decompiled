using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000078 RID: 120
	public interface IEdmFloatingValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060001ED RID: 493
		double Value { get; }
	}
}
