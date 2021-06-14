using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000EA RID: 234
	internal class BadCollectionType : BadType, IEdmCollectionType, IEdmType, IEdmElement
	{
		// Token: 0x060004AD RID: 1197 RVA: 0x0000C38E File Offset: 0x0000A58E
		public BadCollectionType(IEnumerable<EdmError> errors) : base(errors)
		{
			this.elementType = new BadTypeReference(new BadType(errors), true);
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0000C3A9 File Offset: 0x0000A5A9
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Collection;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		public IEdmTypeReference ElementType
		{
			get
			{
				return this.elementType;
			}
		}

		// Token: 0x040001C0 RID: 448
		private readonly IEdmTypeReference elementType;
	}
}
