using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000135 RID: 309
	internal abstract class CsdlNamedStructuredType : CsdlStructuredType
	{
		// Token: 0x060005E5 RID: 1509 RVA: 0x0000F44D File Offset: 0x0000D64D
		protected CsdlNamedStructuredType(string name, string baseTypeName, bool isAbstract, IEnumerable<CsdlProperty> properties, CsdlDocumentation documentation, CsdlLocation location) : base(properties, documentation, location)
		{
			this.isAbstract = isAbstract;
			this.name = name;
			this.baseTypeName = baseTypeName;
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0000F470 File Offset: 0x0000D670
		public string BaseTypeName
		{
			get
			{
				return this.baseTypeName;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0000F478 File Offset: 0x0000D678
		public bool IsAbstract
		{
			get
			{
				return this.isAbstract;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0000F480 File Offset: 0x0000D680
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x0400031E RID: 798
		protected string baseTypeName;

		// Token: 0x0400031F RID: 799
		protected bool isAbstract;

		// Token: 0x04000320 RID: 800
		protected string name;
	}
}
