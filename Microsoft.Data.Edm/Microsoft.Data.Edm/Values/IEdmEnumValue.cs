using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000126 RID: 294
	public interface IEdmEnumValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060005C2 RID: 1474
		IEdmPrimitiveValue Value { get; }
	}
}
