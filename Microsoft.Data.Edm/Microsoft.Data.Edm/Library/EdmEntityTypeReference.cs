using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020000FB RID: 251
	public class EdmEntityTypeReference : EdmTypeReference, IEdmEntityTypeReference, IEdmStructuredTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060004E0 RID: 1248 RVA: 0x0000C63E File Offset: 0x0000A83E
		public EdmEntityTypeReference(IEdmEntityType entityType, bool isNullable) : base(entityType, isNullable)
		{
		}
	}
}
