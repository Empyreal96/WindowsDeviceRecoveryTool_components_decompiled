using System;

namespace Microsoft.Data.Edm.Annotations
{
	// Token: 0x0200005F RID: 95
	public interface IEdmDirectValueAnnotation : IEdmNamedElement, IEdmElement
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000181 RID: 385
		string NamespaceUri { get; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000182 RID: 386
		object Value { get; }
	}
}
