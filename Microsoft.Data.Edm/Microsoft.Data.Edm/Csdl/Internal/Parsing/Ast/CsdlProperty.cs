using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000146 RID: 326
	internal class CsdlProperty : CsdlNamedElement
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x0000F75B File Offset: 0x0000D95B
		public CsdlProperty(string name, CsdlTypeReference type, bool isFixedConcurrency, string defaultValue, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.type = type;
			this.isFixedConcurrency = isFixedConcurrency;
			this.defaultValue = defaultValue;
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0000F77E File Offset: 0x0000D97E
		public CsdlTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0000F786 File Offset: 0x0000D986
		public string DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0000F78E File Offset: 0x0000D98E
		public bool IsFixedConcurrency
		{
			get
			{
				return this.isFixedConcurrency;
			}
		}

		// Token: 0x04000340 RID: 832
		private readonly CsdlTypeReference type;

		// Token: 0x04000341 RID: 833
		private readonly string defaultValue;

		// Token: 0x04000342 RID: 834
		private readonly bool isFixedConcurrency;
	}
}
