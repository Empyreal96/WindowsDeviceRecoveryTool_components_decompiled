using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020000E6 RID: 230
	public class EdmPrimitiveTypeReference : EdmTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x0000C284 File Offset: 0x0000A484
		public EdmPrimitiveTypeReference(IEdmPrimitiveType definition, bool isNullable) : base(definition, isNullable)
		{
		}
	}
}
