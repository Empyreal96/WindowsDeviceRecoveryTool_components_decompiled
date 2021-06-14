using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200014E RID: 334
	internal class CsdlUsing : CsdlElementWithDocumentation
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x0000F986 File Offset: 0x0000DB86
		public CsdlUsing(string namespaceName, string alias, CsdlDocumentation documentation, CsdlLocation location) : base(documentation, location)
		{
			this.alias = alias;
			this.namespaceName = namespaceName;
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x0000F99F File Offset: 0x0000DB9F
		public string Alias
		{
			get
			{
				return this.alias;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0000F9A7 File Offset: 0x0000DBA7
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x04000359 RID: 857
		private readonly string alias;

		// Token: 0x0400035A RID: 858
		private readonly string namespaceName;
	}
}
