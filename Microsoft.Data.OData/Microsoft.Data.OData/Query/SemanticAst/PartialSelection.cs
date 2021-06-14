using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000075 RID: 117
	internal sealed class PartialSelection : Selection
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x0000A7FE File Offset: 0x000089FE
		public PartialSelection(IEnumerable<SelectItem> selectedItems)
		{
			this.selectedItems = (selectedItems ?? ((IEnumerable<SelectItem>)new SelectItem[0]));
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000A81C File Offset: 0x00008A1C
		public IEnumerable<SelectItem> SelectedItems
		{
			get
			{
				return this.selectedItems;
			}
		}

		// Token: 0x040000BF RID: 191
		private readonly IEnumerable<SelectItem> selectedItems;
	}
}
