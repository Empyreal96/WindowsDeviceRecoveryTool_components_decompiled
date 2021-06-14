using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000147 RID: 327
	internal class CsdlPropertyReference : CsdlElement
	{
		// Token: 0x0600061D RID: 1565 RVA: 0x0000F796 File Offset: 0x0000D996
		public CsdlPropertyReference(string propertyName, CsdlLocation location) : base(location)
		{
			this.propertyName = propertyName;
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0000F7A6 File Offset: 0x0000D9A6
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x04000343 RID: 835
		private readonly string propertyName;
	}
}
