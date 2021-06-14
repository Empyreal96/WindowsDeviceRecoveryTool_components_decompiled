using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200013B RID: 315
	internal class CsdlEntitySet : CsdlNamedElement
	{
		// Token: 0x060005F7 RID: 1527 RVA: 0x0000F55E File Offset: 0x0000D75E
		public CsdlEntitySet(string name, string entityType, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.entityType = entityType;
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0000F571 File Offset: 0x0000D771
		public string EntityType
		{
			get
			{
				return this.entityType;
			}
		}

		// Token: 0x0400032A RID: 810
		private readonly string entityType;
	}
}
