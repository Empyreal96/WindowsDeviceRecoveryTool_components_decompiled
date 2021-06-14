using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200013C RID: 316
	internal class CsdlEntityType : CsdlNamedStructuredType
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x0000F579 File Offset: 0x0000D779
		public CsdlEntityType(string name, string baseTypeName, bool isAbstract, bool isOpen, CsdlKey key, IEnumerable<CsdlProperty> properties, IEnumerable<CsdlNavigationProperty> navigationProperties, CsdlDocumentation documentation, CsdlLocation location) : base(name, baseTypeName, isAbstract, properties, documentation, location)
		{
			this.isOpen = isOpen;
			this.key = key;
			this.navigationProperties = new List<CsdlNavigationProperty>(navigationProperties);
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0000F5A7 File Offset: 0x0000D7A7
		public IEnumerable<CsdlNavigationProperty> NavigationProperties
		{
			get
			{
				return this.navigationProperties;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0000F5AF File Offset: 0x0000D7AF
		public CsdlKey Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x0000F5B7 File Offset: 0x0000D7B7
		public bool IsOpen
		{
			get
			{
				return this.isOpen;
			}
		}

		// Token: 0x0400032B RID: 811
		private readonly CsdlKey key;

		// Token: 0x0400032C RID: 812
		private readonly bool isOpen;

		// Token: 0x0400032D RID: 813
		private readonly List<CsdlNavigationProperty> navigationProperties;
	}
}
