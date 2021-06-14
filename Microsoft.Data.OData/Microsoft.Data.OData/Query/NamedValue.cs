using System;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000CC RID: 204
	internal sealed class NamedValue
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x0001155F File Offset: 0x0000F75F
		public NamedValue(string name, LiteralToken value)
		{
			ExceptionUtils.CheckArgumentNotNull<LiteralToken>(value, "value");
			this.name = name;
			this.value = value;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00011580 File Offset: 0x0000F780
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00011588 File Offset: 0x0000F788
		public LiteralToken Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x040001D8 RID: 472
		private readonly string name;

		// Token: 0x040001D9 RID: 473
		private readonly LiteralToken value;
	}
}
