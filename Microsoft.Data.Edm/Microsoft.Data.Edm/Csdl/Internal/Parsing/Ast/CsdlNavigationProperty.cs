using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000144 RID: 324
	internal class CsdlNavigationProperty : CsdlNamedElement
	{
		// Token: 0x06000612 RID: 1554 RVA: 0x0000F6F7 File Offset: 0x0000D8F7
		public CsdlNavigationProperty(string name, string relationship, string toRole, string fromRole, bool containsTarget, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.relationship = relationship;
			this.toRole = toRole;
			this.fromRole = fromRole;
			this.containsTarget = containsTarget;
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0000F722 File Offset: 0x0000D922
		public string Relationship
		{
			get
			{
				return this.relationship;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0000F72A File Offset: 0x0000D92A
		public string ToRole
		{
			get
			{
				return this.toRole;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0000F732 File Offset: 0x0000D932
		public string FromRole
		{
			get
			{
				return this.fromRole;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0000F73A File Offset: 0x0000D93A
		public bool ContainsTarget
		{
			get
			{
				return this.containsTarget;
			}
		}

		// Token: 0x0400033B RID: 827
		private readonly string relationship;

		// Token: 0x0400033C RID: 828
		private readonly string toRole;

		// Token: 0x0400033D RID: 829
		private readonly string fromRole;

		// Token: 0x0400033E RID: 830
		private readonly bool containsTarget;
	}
}
