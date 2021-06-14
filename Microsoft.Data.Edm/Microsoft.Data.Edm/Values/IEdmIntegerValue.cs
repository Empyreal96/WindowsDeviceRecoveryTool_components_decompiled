using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x0200007B RID: 123
	public interface IEdmIntegerValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060001FA RID: 506
		long Value { get; }
	}
}
