using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000054 RID: 84
	public interface IEdmBinaryValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000143 RID: 323
		byte[] Value { get; }
	}
}
