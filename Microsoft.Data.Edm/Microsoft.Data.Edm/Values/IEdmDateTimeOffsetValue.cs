using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000045 RID: 69
	public interface IEdmDateTimeOffsetValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060000F7 RID: 247
		DateTimeOffset Value { get; }
	}
}
