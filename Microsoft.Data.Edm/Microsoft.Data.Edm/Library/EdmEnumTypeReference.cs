using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x02000195 RID: 405
	public class EdmEnumTypeReference : EdmTypeReference, IEdmEnumTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060008E0 RID: 2272 RVA: 0x000183FF File Offset: 0x000165FF
		public EdmEnumTypeReference(IEdmEnumType enumType, bool isNullable) : base(enumType, isNullable)
		{
		}
	}
}
