using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001E6 RID: 486
	public class EdmEntityReferenceTypeReference : EdmTypeReference, IEdmEntityReferenceTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x06000B71 RID: 2929 RVA: 0x000212C5 File Offset: 0x0001F4C5
		public EdmEntityReferenceTypeReference(IEdmEntityReferenceType entityReferenceType, bool isNullable) : base(entityReferenceType, isNullable)
		{
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x000212CF File Offset: 0x0001F4CF
		public IEdmEntityReferenceType EntityReferenceDefinition
		{
			get
			{
				return (IEdmEntityReferenceType)base.Definition;
			}
		}
	}
}
