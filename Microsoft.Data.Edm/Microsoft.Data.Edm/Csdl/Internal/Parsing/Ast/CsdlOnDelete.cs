using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000145 RID: 325
	internal class CsdlOnDelete : CsdlElementWithDocumentation
	{
		// Token: 0x06000617 RID: 1559 RVA: 0x0000F742 File Offset: 0x0000D942
		public CsdlOnDelete(EdmOnDeleteAction action, CsdlDocumentation documentation, CsdlLocation location) : base(documentation, location)
		{
			this.action = action;
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0000F753 File Offset: 0x0000D953
		public EdmOnDeleteAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x0400033F RID: 831
		private readonly EdmOnDeleteAction action;
	}
}
