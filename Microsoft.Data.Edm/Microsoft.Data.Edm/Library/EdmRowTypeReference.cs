using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001EB RID: 491
	public class EdmRowTypeReference : EdmTypeReference, IEdmRowTypeReference, IEdmStructuredTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x06000BA3 RID: 2979 RVA: 0x00021B45 File Offset: 0x0001FD45
		public EdmRowTypeReference(IEdmRowType rowType, bool isNullable) : base(rowType, isNullable)
		{
		}
	}
}
