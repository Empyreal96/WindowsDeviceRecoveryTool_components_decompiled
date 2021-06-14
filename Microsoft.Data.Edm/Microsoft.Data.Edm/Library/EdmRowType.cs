using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001DB RID: 475
	public class EdmRowType : EdmStructuredType, IEdmRowType, IEdmStructuredType, IEdmType, IEdmElement
	{
		// Token: 0x06000B4C RID: 2892 RVA: 0x00020CF3 File Offset: 0x0001EEF3
		public EdmRowType() : base(false, false, null)
		{
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00020CFE File Offset: 0x0001EEFE
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Row;
			}
		}
	}
}
