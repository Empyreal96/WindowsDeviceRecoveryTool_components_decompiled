using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000072 RID: 114
	public interface IEdmDateTimeValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060001D3 RID: 467
		DateTime Value { get; }
	}
}
