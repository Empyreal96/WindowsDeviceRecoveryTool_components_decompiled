using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020000F0 RID: 240
	public class EdmComplexTypeReference : EdmTypeReference, IEdmComplexTypeReference, IEdmStructuredTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x0000C41B File Offset: 0x0000A61B
		public EdmComplexTypeReference(IEdmComplexType complexType, bool isNullable) : base(complexType, isNullable)
		{
		}
	}
}
