using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x02000105 RID: 261
	internal class BadRowType : BadStructuredType, IEdmRowType, IEdmStructuredType, IEdmType, IEdmElement
	{
		// Token: 0x06000507 RID: 1287 RVA: 0x0000C92D File Offset: 0x0000AB2D
		public BadRowType(IEnumerable<EdmError> errors) : base(errors)
		{
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0000C936 File Offset: 0x0000AB36
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Row;
			}
		}
	}
}
