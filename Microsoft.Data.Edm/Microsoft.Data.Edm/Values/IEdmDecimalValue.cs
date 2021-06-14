using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000075 RID: 117
	public interface IEdmDecimalValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060001E0 RID: 480
		decimal Value { get; }
	}
}
