using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000139 RID: 313
	internal class CsdlEntityContainer : CsdlNamedElement
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x0000F4EC File Offset: 0x0000D6EC
		public CsdlEntityContainer(string name, string extends, IEnumerable<CsdlEntitySet> entitySets, IEnumerable<CsdlAssociationSet> associationSets, IEnumerable<CsdlFunctionImport> functionImports, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.extends = extends;
			this.entitySets = new List<CsdlEntitySet>(entitySets);
			this.associationSets = new List<CsdlAssociationSet>(associationSets);
			this.functionImports = new List<CsdlFunctionImport>(functionImports);
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0000F526 File Offset: 0x0000D726
		public string Extends
		{
			get
			{
				return this.extends;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0000F52E File Offset: 0x0000D72E
		public IEnumerable<CsdlEntitySet> EntitySets
		{
			get
			{
				return this.entitySets;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0000F536 File Offset: 0x0000D736
		public IEnumerable<CsdlAssociationSet> AssociationSets
		{
			get
			{
				return this.associationSets;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0000F53E File Offset: 0x0000D73E
		public IEnumerable<CsdlFunctionImport> FunctionImports
		{
			get
			{
				return this.functionImports;
			}
		}

		// Token: 0x04000325 RID: 805
		private readonly string extends;

		// Token: 0x04000326 RID: 806
		private readonly List<CsdlEntitySet> entitySets;

		// Token: 0x04000327 RID: 807
		private readonly List<CsdlAssociationSet> associationSets;

		// Token: 0x04000328 RID: 808
		private readonly List<CsdlFunctionImport> functionImports;
	}
}
