using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200000E RID: 14
	internal abstract class CsdlNamedElement : CsdlElementWithDocumentation
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00002A06 File Offset: 0x00000C06
		protected CsdlNamedElement(string name, CsdlDocumentation documentation, CsdlLocation location) : base(documentation, location)
		{
			this.name = name;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002A17 File Offset: 0x00000C17
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x04000014 RID: 20
		private readonly string name;
	}
}
