using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000051 RID: 81
	public interface IEdmGuidValue : IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000136 RID: 310
		Guid Value { get; }
	}
}
