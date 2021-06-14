using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000149 RID: 329
	internal class CsdlReferentialConstraintRole : CsdlElementWithDocumentation
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x0000F7D7 File Offset: 0x0000D9D7
		public CsdlReferentialConstraintRole(string role, IEnumerable<CsdlPropertyReference> properties, CsdlDocumentation documentation, CsdlLocation location) : base(documentation, location)
		{
			this.role = role;
			this.properties = new List<CsdlPropertyReference>(properties);
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0000F7F5 File Offset: 0x0000D9F5
		public string Role
		{
			get
			{
				return this.role;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0000F7FD File Offset: 0x0000D9FD
		public IEnumerable<CsdlPropertyReference> Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0000F805 File Offset: 0x0000DA05
		public int IndexOf(CsdlPropertyReference reference)
		{
			return this.properties.IndexOf(reference);
		}

		// Token: 0x04000346 RID: 838
		private readonly string role;

		// Token: 0x04000347 RID: 839
		private readonly List<CsdlPropertyReference> properties;
	}
}
