using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x0200002B RID: 43
	public interface IEdmType : IEdmElement
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000091 RID: 145
		EdmTypeKind TypeKind { get; }
	}
}
