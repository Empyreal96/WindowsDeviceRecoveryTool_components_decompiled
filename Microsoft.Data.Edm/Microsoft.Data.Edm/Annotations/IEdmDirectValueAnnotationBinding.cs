using System;

namespace Microsoft.Data.Edm.Annotations
{
	// Token: 0x020000D2 RID: 210
	public interface IEdmDirectValueAnnotationBinding
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600043A RID: 1082
		IEdmElement Element { get; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600043B RID: 1083
		string NamespaceUri { get; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600043C RID: 1084
		string Name { get; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600043D RID: 1085
		object Value { get; }
	}
}
