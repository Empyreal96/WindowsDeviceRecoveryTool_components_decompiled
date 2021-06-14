using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000142 RID: 322
	internal class CsdlKey : CsdlElement
	{
		// Token: 0x0600060D RID: 1549 RVA: 0x0000F6B1 File Offset: 0x0000D8B1
		public CsdlKey(IEnumerable<CsdlPropertyReference> properties, CsdlLocation location) : base(location)
		{
			this.properties = new List<CsdlPropertyReference>(properties);
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0000F6C6 File Offset: 0x0000D8C6
		public IEnumerable<CsdlPropertyReference> Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04000339 RID: 825
		private readonly List<CsdlPropertyReference> properties;
	}
}
