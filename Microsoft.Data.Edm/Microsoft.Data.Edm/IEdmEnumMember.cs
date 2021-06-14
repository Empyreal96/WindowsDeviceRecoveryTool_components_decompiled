using System;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000061 RID: 97
	public interface IEdmEnumMember : IEdmNamedElement, IEdmElement
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600018C RID: 396
		IEdmPrimitiveValue Value { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600018D RID: 397
		IEdmEnumType DeclaringType { get; }
	}
}
