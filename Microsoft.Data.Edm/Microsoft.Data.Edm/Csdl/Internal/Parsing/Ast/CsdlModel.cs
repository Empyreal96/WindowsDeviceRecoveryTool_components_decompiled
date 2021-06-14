using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000143 RID: 323
	internal class CsdlModel
	{
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0000F6CE File Offset: 0x0000D8CE
		public IEnumerable<CsdlSchema> Schemata
		{
			get
			{
				return this.schemata;
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0000F6D6 File Offset: 0x0000D8D6
		public void AddSchema(CsdlSchema schema)
		{
			this.schemata.Add(schema);
		}

		// Token: 0x0400033A RID: 826
		private readonly List<CsdlSchema> schemata = new List<CsdlSchema>();
	}
}
